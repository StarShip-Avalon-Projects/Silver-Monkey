Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text.RegularExpressions
Imports System.Threading
Imports Furcadia.Net
Imports Monkeyspeak
Imports SilverMonkey.Engine.Libraries.PhoenixSpeak.SubSystem

Namespace Engine.Libraries.PhoenixSpeak

    ''' <summary>
    ''' Phoenix mSpeak Even Arguments
    ''' </summary>
    Public Class PhoenixSpeakEventArgs
        Inherits EventArgs

#Region "Public Fields"
        ''' <summary>
        ''' Phoenix Speak Flag
        ''' </summary>
        Public Flag As PsFlag
        ''' <summary>
        ''' Do we have too much Phoienix-Speak Data then the Server can send to us?
        ''' </summary>
        Public PageOverFlow As Boolean

#End Region

#Region "Public Properties"

        Public Property id As Short
        Public Property PsType As PsResponseType

#End Region

    End Class
    ''' <summary>
    ''' Phoenix Speak processing system
    ''' <para>Purpose, to Parse Phoenix Speak responses from the game server</para>
    ''' <para>and store the Phoenix Speak Variables in some sort of a list</para>
    ''' </summary>
    <CLSCompliant(True)>
    Public Class SubSystem
        Inherits BaseSubSystem

#Region "Public Fields"

        Public Const SmRegExOptions As RegexOptions = RegexOptions.CultureInvariant _
            Or RegexOptions.IgnoreCase

        ''' <summary>
        ''' 16 bit integer Game server ID for the Current Executing Phoenix Speak Instruction set.
        ''' </summary>
        Public SubSystemPsID As PsId = New PsId

#End Region

#Region "Private Fields"

        ''' <summary>
        ''' Did we get a Multi-Page response from the game server?
        ''' </summary>
        Private Shared MultiPage As Boolean = False

        ''' <summary>
        ''' Phoenix MultiPage limit is 6
        ''' <para> If we hit this limit Inform the user there maybe more data unseen</para>
        ''' </summary>
        Private PageOverFlow As Boolean = False

        ''' <summary>
        ''' temporary place to store Multi-page PS responses
        ''' <para>Furcadia has a parsing bug where the information is not split correctly</para>
        ''' </summary>
        Private PS_Page As String
        ''' <summary>
        '''
        ''' </summary>
        Private PSProcessingResource As New Object

#End Region

#Region "Public Enums"

        Public Enum PsFlag As Short
            ''' <summary>
            ''' Unknown Phoenix Speak response
            ''' </summary>
            PsUnknown = -1
            ''' <summary>
            ''' Phoenix Speak Error
            ''' </summary>
            PsError
            ''' <summary>
            ''' Get Phoenix Speak data from server
            ''' </summary>
            PsGet
            ''' <summary>
            ''' Send Phoenix Speak data to server.
            ''' </summary>
            PsSet
        End Enum

        Public Enum PsResponseType As Short
            PsUnKnown = -1
            PsOk
            PSGet
            PSSet
            PsError
        End Enum

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' List of Phoenix Speak Variables last received from the Phoenix Speak Server Interface
        ''' <para>This should take into account Multi-Page responses from the servers Command Line interface</para>
        ''' </summary>
        ''' <returns></returns>
        Public Property PSInfoCache As New List(Of Variable)

#End Region

#Region "Public Methods"

        ''' <summary>
        '''
        ''' </summary>
        Public Sub Abort()
            psSendToServer.Clear()
        End Sub

        '^PS (\d+)? ?Error:|Ok: (set:|get:)? multi_result? (\d+)?/?(\d+)?: Key/Value group
        ''' <summary>
        ''' process Phoenix Speak data coming from the game server
        ''' Reg-ex capture of server responses
        ''' Execute Monkey Speak Engine processing (Triggers setting Monkey Speak Variables)
        ''' Always clear PSInfoCache unless its a Multi-Page response
        ''' </summary>
        ''' <param name="ServerData">Server Data as string</param>
        Public Overrides Sub ParseServerChannel(ServerData As String, ByRef Handled As Boolean)
            Try
                Monitor.Enter(PSProcessingResource)
                Dim ReturnVal As Boolean = False
                Debug.WriteLine(ServerData)
                Dim data As String = ServerData
                Dim _PsId As Short = PsCaptureId(data)

                'safety Check
                If _PsId = 0 Or Not SubSystemPsID.HasId(_PsId) Then
                    ReceivedFromServer(_PsId)
                End If

                'Send to receive Buffer

                Dim ResponceType As PsResponseType = PsCaptureMode(data)
                Dim ResponceMode As PsFlag = PsResponceMode(data)
                Dim pageCount As Short = ProcessPage(data)
                If pageCount = 6 Then PageOverFlow = True
                PSInfoCache.Clear()
                If ResponceMode = PsFlag.PsGet And ResponceType = PsResponseType.PsOk Then

                    'load up the PSInfoCache
                    If pageCount > 0 And Not MultiPage Then
                        MultiPage = True
                        PS_Page += data.Trim
                        'Hold MonkeySpeak Triggers till all pages read

                    ElseIf MultiPage And pageCount = 0 Then
                        'when all pages read
                        'process variables
                        'trigger Monkey Response
                        PS_Page += data.Trim

                        PSInfoCache = ProcessVariables(PS_Page)
                        Dim args As New PhoenixSpeakEventArgs
                        args.id = _PsId
                        args.PsType = ResponceType
                        args.Flag = ResponceMode
                        args.PageOverFlow = PageOverFlow
                        ProcessedFromeServer(PSInfoCache, args)

                        PS_Page = String.Empty
                        MultiPage = False

                        PageOverFlow = False

                        PageSetVariable("MESSAGE", ServerData)
                        'Phoenix Speak Response set
                        PageExecute(80, 81, 82)

                    Else 'Assume Single page responses
                        PSInfoCache = ProcessVariables(data)
                        'Phoenix Speak Response set

                        Dim args As New PhoenixSpeakEventArgs
                        args.id = _PsId
                        args.PsType = ResponceType
                        args.Flag = ResponceMode
                        args.PageOverFlow = PageOverFlow

                        ProcessedFromeServer(ServerData, args)
                        PageSetVariable("MESSAGE", ServerData)
                        PageExecute(80, 81, 82)

                        'trigger Monkey Speak responses
                    End If

                ElseIf ResponceType = PsResponseType.PsError Then
                    'capture error
                    'trigger error response

                    'abort processes on error

                    Dim args As New PhoenixSpeakEventArgs
                    args.id = _PsId
                    args.PsType = ResponceType
                    args.Flag = ResponceMode
                    args.PageOverFlow = PageOverFlow
                    ProcessedFromeServer(ServerData, args)
                    PageSetVariable("MESSAGE", ServerData)
                    Dim AbortError As New Regex("Sorry, PhoenixSpeak commands are currently not available in this dream.$", SmRegExOptions)
                    If AbortError.Match(data).Success Then
                        Abort()
                        '(0:503) When the bot finishes restoring the dreams character Phoenix Speak,
                        PageExecute(503)
                    End If

                End If
                'remove Current PS Id from the list manager
                'Remove Processed item from the enqueue

                If Not MultiPage Then ReceivedFromServer(_PsId)
            Finally
                Monitor.Exit(PSProcessingResource)
            End Try

        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' returns number of Phoenix Speak pages remaining
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        Private Function ProcessPage(ByRef data As String) As Short
            Dim PsPage As New Regex(String.Format("{0}", "multi_result?(\ d +)?/(\d+)?"), SmRegExOptions)
            Dim CurrentPage As Short = 0
            Dim TotalPages As Short = 0
            Short.TryParse(PsPage.Match(data, 0).Groups(1).Value(), CurrentPage)
            Short.TryParse(PsPage.Match(data, 0).Groups(2).Value(), TotalPages)
            data = PsPage.Replace(data, "", 1)
            Return TotalPages - CurrentPage
            'Add "," to the end of match #1.
            'Input: "bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340,"
        End Function

        ''' <summary>
        ''' Get a Phoenix-Speak Variable List  from a REGEX match collection
        ''' <para>     Match(1) : Value(Name)</para>
        ''' <para>     Match 2: Empty if number, ' if string</para>
        ''' <para>     Match(3) : Value()</para>
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        Private Function ProcessVariables(ByRef data As String) As List(Of PhoenixSpeak.Variable)
            Dim mc As New Regex(" (.*?)=('?)(\d+|.*?)(\2),?", SmRegExOptions)

            Dim PsVarList As New List(Of PhoenixSpeak.Variable)
            For Each M As Match In mc.Matches(data)
                PsVarList.Add(New PhoenixSpeak.Variable(M.Groups(1).Value.Trim, M.Groups(3).Value))
            Next

            Return PsVarList
        End Function

        ''' <summary>
        ''' gets the PsId from the server
        ''' Defaults to 0 if not found
        ''' </summary>
        ''' <param name="Data"></param>
        ''' <returns></returns>
        Private Function PsCaptureId(ByRef Data As String) As Short
            Dim ThisPsId As Short = 0
            Dim IdCapture As New Regex("^PS (\d+)? ?", SmRegExOptions)

            Short.TryParse(IdCapture.Match(Data, 0).Groups(1).Value, ThisPsId)
            Data = IdCapture.Replace(Data, "", 1)

            Return ThisPsId
        End Function

        <CLSCompliant(False)>
        Private Function PsCaptureMode(ByRef data As String) As PsResponseType
            Dim ResponceType As PsResponseType = PsResponseType.PsUnKnown
            Dim CaptureMode As New Regex(" (Error:|Ok:)?", SmRegExOptions)
            Select Case CaptureMode.Match(data).Groups(1).Value
                Case "Error:"
                    ResponceType = PsResponseType.PsError
                Case "Ok:"
                    ResponceType = PsResponseType.PsOk
            End Select
            data = CaptureMode.Replace(data, "", 1)
            Return ResponceType
        End Function

        <CLSCompliant(False)>
        Private Function PsResponceMode(ByRef data As String) As PsFlag
            Dim Responceflag As PsFlag = PsFlag.PsUnknown
            Dim CaptureMode As New Regex("^ ?(get:|set:)? ?(result: )?", SmRegExOptions)
            Select Case CaptureMode.Match(data).Groups(1).Value
                Case "get:"
                    Responceflag = PsFlag.PsGet
                Case "set:"
                    Responceflag = PsFlag.PsSet
                Case Else
                    Responceflag = PsFlag.PsUnknown

            End Select
            data = CaptureMode.Replace(data, "", 1)
            Return Responceflag
        End Function

#End Region

        'TODO: FreePSId as PsId

#Region "Public Classes"

        ''' <summary>
        ''' Phoenix Speak ID manager
        '''
        ''' </summary>
        Public Class PsId

#Region "Private Fields"

            ''' <summary>
            ''' Gets a free PS ID 16 bit integer
            ''' <para>this will recycle ids as they're needed</para>
            ''' </summary>
            ''' <param name="v"></param>
            ''' <returns></returns>
            Private Shared idLock As New Object

            'Recycle ids as they're needed
            'on Receiving Server Data remove Id
            'If theres more then one set to an ID Count Down first

            'On sending a Command to the Server Set Available IDs for the out going Enqueue
            'Use same ID with counter for Multiple Commands sent
            'IE Restoring multi-page character data

            Private Shared List As New Dictionary(Of Short, PsId)

            Private _Count As Integer
            Private _id As Short

#End Region

#Region "Public Constructors"

            Sub New()
                _id = getID(0)
                _Count = 1
                If Not List.ContainsKey(_id) Then
                    List.Add(_id, Me)
                End If
            End Sub

            ''' <summary>
            ''' Creates a New PHoenix Speak ID
            ''' <para>If the ID exists, Its Number of uses increases by one</para>
            ''' </summary>
            ''' <param name="NewId"></param>
            Sub New(ByRef NewId As Short)

                If List.ContainsKey(NewId) Then
                    List.Item(NewId).Count += 1
                Else

                    _id = getID(NewId)
                    _Count = 1
                    List.Add(NewId, Me)
                End If

            End Sub

#End Region

#Region "Public Properties"

            Public Property Count As Integer
                Get
                    Return _Count
                End Get
                Private Set(ByVal value As Integer)
                    _Count = value
                End Set
            End Property

            Public Property Id As Short
                Get
                    Return _id
                End Get
                Set(ByVal value As Short)
                    _id = value
                    '_Count += 1
                End Set
            End Property

#End Region

#Region "Public Methods"

            Public Function HasId(ByVal id As Short) As Boolean
                Return List.ContainsKey(id)
            End Function

            ''' <summary>
            ''' Removes the specified Phoenix Speak ID
            ''' </summary>
            ''' <param name="id"></param>
            Public Sub remove(id As Short)
                Dim PS As PsId
                If List.ContainsKey(id) Then
                    PS = List.Item(id)
                    PS.Count -= 1
                    If PS.Count = 0 Then
                        List.Remove(id)
                    End If
                End If
            End Sub

#End Region

#Region "Private Methods"

            Private Shared Function getID(ByRef v As Short) As Short
                If v = 0 Then v = 1
                SyncLock idLock
                    While List.ContainsKey(v)
                        v += CShort(1)
                        If v = Short.MaxValue - 1 Then
                            v = 1
                        End If
                    End While
                End SyncLock
                Return v
            End Function

#End Region

        End Class

#End Region

#Region "Events"
        Public Delegate Sub ParsePhoenixSpeak(o As Object, e As PhoenixSpeakEventArgs)
        Public Event PsReceived As ParsePhoenixSpeak
#End Region

#Region "Server Functions"

        Private Shared bufferlock As New Object

        ' clear the last item sent when its received
        Private Shared psSendToServer As New Queue(Of KeyValuePair(Of String, Short))(20)

        Private SendLock As New Object
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByRef Dream As DREAM, ByRef Player As FURRE)
            MyBase.New(Dream, Player)
        End Sub

        Public Shared Sub ProcessedFromeServer(o As Object, e As PhoenixSpeakEventArgs)

        End Sub

        Public Sub ClientMessage(ByVal msg As String)
            'FurcSession.SendClientMessage("PhoenixSpeak:", msg)
        End Sub

        ''' <summary>
        ''' Send the Phoenix Commands to the Server enqueue
        ''' </summary>
        ''' <param name="var"></param>
        Public Sub sendServer(ByRef var As String)
            Try
                Monitor.Enter(SendLock)
                Dim Id As Short = PsCaptureId(var)
                Dim NewPsID As New PsId(Id)
                var = "ps " + Id.ToString + " " + var

                Dim ServerQueueItem As New KeyValuePair(Of String, Short)(var, Id)
                ' there are possibilities that PS errors can come from the game server
                psSendToServer.Enqueue(ServerQueueItem)
                sendServer(var)
            Finally
                Monitor.Exit(SendLock)
            End Try
            Debug.WriteLine("SendServer: " + var)
        End Sub
        ''' <summary>
        ''' 'Fills the received PS Buffer
        ''' </summary>
        ''' <param name="item">CommandKey PsId</param>
        Private Shared Sub ReceivedFromServer(IsID As Short)
            Try
                Monitor.Enter(bufferlock)
                If psSendToServer.Count = 0 Then Exit Sub
                Dim id As Short = psSendToServer.Dequeue().Value
                '     PsId.remove(id)
                If psSendToServer.Count = 0 Then Exit Sub
                Dim args As New PhoenixSpeakEventArgs
                args.id = id

                ProcessedFromeServer(psSendToServer.Peek().Key, args)
            Finally
                Monitor.Exit(bufferlock)
            End Try
        End Sub
#End Region

    End Class
End Namespace