Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text.RegularExpressions
Imports System.Threading
Imports MonkeyCore

Namespace PhoenixSpeak



    ''' <summary>
    ''' Phoenix Speak processing system
    ''' <para>Purpose, to Parse Phoenix Speak responses from the game server</para>
    ''' <para>and store the Phoenix Speak Variables in some sort of a list</para>
    ''' </summary>
    <CLSCompliant(True)>
    Public Class SubSystem

        Public Const SmRegExOptions As RegexOptions = RegexOptions.CultureInvariant _
            Or RegexOptions.IgnoreCase




        Private Shared MultiPage As Boolean = False

        'TODO: FreePSId as PsId

        ''' <summary>
        ''' Phoenix Speak ID manager
        ''' 
        ''' </summary>
        Public Class PsId

            'Recycle ids as they're needed
            'on Receiving Server Data remove Id 
            'If theres more then one set to an ID Count Down first

            'On sending a Command to the Server Set Available IDs for the out going Enqueue
            'Use same ID with counter for Multiple Commands sent
            'IE Restoring multi-page character data


            Private Shared List As New Dictionary(Of Short, PsId)
            Private _id As Short

            Private _Count As Integer
            Public Property Id As Short
                Get
                    Return _id
                End Get
                Set(ByVal value As Short)
                    _id = value
                    _Count += 1
                End Set
            End Property
            Public Property Count As Integer
                Get
                    Return _Count
                End Get
                Private Set(ByVal value As Integer)
                    _Count = value
                End Set
            End Property

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
                If NewId = 0 Then getID(NewId)
                If List.ContainsKey(NewId) Then
                    _id = List.Item(NewId).Id
                    _Count += 1
                Else
                    _id = NewId
                    _Count = 1
                    List.Add(NewId, Me)
                End If
            End Sub

            Public Shared Function HasId(ByVal id As Short) As Boolean
                Return List.ContainsKey(id)
            End Function

            ''' <summary>
            ''' Removes the specified Phoenix Speak ID
            ''' </summary>
            ''' <param name="id"></param>
            Public Shared Sub remove(id As Short)
                Dim PS As PsId
                If List.ContainsKey(id) Then
                    PS = List.Item(id)
                    PS.Count -= 1
                    If PS.Count = 0 Then
                        List.Remove(id)
                    End If
                End If
            End Sub


            ''' <summary>
            ''' Gets a free PS ID 16 bit integer
            ''' <para>this will recycle ids as they're needed</para>
            ''' </summary>
            ''' <param name="v"></param>
            ''' <returns></returns>
            Private Shared idLock As New Object
            Private Shared Function getID(ByRef v As Short) As Short
                If v = 0 Then v = 1
                While List.ContainsKey(v)
                    v += CShort(1)
                    If v = Short.MaxValue - 1 Then
                        v = 1
                    End If
                End While
                Return v
            End Function



        End Class

        ''' <summary>
        ''' PS server responses
        ''' </summary>
        <CLSCompliant(True)>
        Public Enum PsResponseType As Short
            PsUnKnown = -1
            PsOk
            PsError
        End Enum

        <CLSCompliant(True)>
        Public Enum PsFlag As Short
            PsUnknown = -1
            PsGet
            PsSet
        End Enum

        ''' <summary>
        ''' temporary place to store Multi-page PS responses
        ''' <para>Furcadia has a parsing bug where the information is not split correctly</para>
        ''' </summary>
        Private Shared PS_Page As String

        Private Shared PageOverFlow As Boolean = False

        ''' <summary>
        ''' List of Phoenix Speak Variables last received from the Phoenix Speak Server Interface
        ''' <para>This should take into account Multi-Page responses from the servers Command Line interface</para>
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property PSInfoCache As New List(Of Variable)

        ''' <summary>
        ''' SubSystem Constructor
        ''' <para>Clean the system with defaults</para>
        ''' </summary>
        Sub New()

        End Sub

#Region "Events"
        Public Shared Event PsReveived(ByRef id As Short, ByVal PsType As PsResponseType, ByVal Flag As PsFlag, ByVal PageOverFlow As Boolean)
#End Region

        Private Shared PSProcessingResource As Integer = 0
        '^PS (\d+)? ?Error:|Ok: (set:|get:)? multi_result? (\d+)?/?(\d+)?: Key/Value group
        ''' <summary>
        ''' process Phoenix Speak data coming from the game server
        ''' Reg-ex capture of server responses
        ''' Execute Monkey Speak Engine processing (Triggers setting Monkey Speak Variables)
        ''' Always clear PSInfoCache unless its a Multi-Page response
        ''' </summary>
        ''' <param name="data">Server Data</param>
        Public Shared Function ProcessServerPS(ByVal ServerData As String) As Boolean
            Dim ReturnVal As Boolean = False
            Debug.WriteLine(ServerData)

            If (0 = Interlocked.Exchange(PSProcessingResource, 1)) Then
                Dim data As String = ServerData
                Dim _PsId As Short = PsCaptureId(data)

                'safety Check
                If _PsId = 0 Or Not PsId.HasId(_PsId) Then
                    Interlocked.Exchange(PSProcessingResource, 0)
                    Return False
                End If

                'Send to receive Buffer
                Dim ServerItem As New KeyValuePair(Of String, Short)(ServerData, _PsId)
                ReceivedFromServer(ServerItem)

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
                        RaiseEvent PsReveived(_PsId, ResponceType, ResponceMode, PageOverFlow)
                        PS_Page = String.Empty
                        MultiPage = False


                        PageOverFlow = False
                        MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", ServerData)

                        'Phoenix Speak Response set
                        MS_Engine.MainMSEngine.PageExecute(80, 81, 82)
                    Else 'Assume Single page responses
                        PSInfoCache = ProcessVariables(data)
                        'Phoenix Speak Response set
                        MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", ServerData)
                        RaiseEvent PsReveived(_PsId, ResponceType, ResponceMode, PageOverFlow)
                        MS_Engine.MainMSEngine.PageExecute(80, 81, 82)

                        'trigger Monkey Speak responses
                    End If



                ElseIf ResponceType = PsResponseType.PsError Then
                    'capture error
                    'trigger error response

                    'abort processes on error
                    MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", ServerData)
                    RaiseEvent PsReveived(_PsId, ResponceType, ResponceMode, PageOverFlow)

                    Dim AbortError As New Regex("Sorry, PhoenixSpeak commands are currently not available in this dream.$")
                    If AbortError.Match(data).Success Then
                        Abort()

                        '(0:503) When the bot finishes restoring the dreams character Phoenix Speak,
                        MainMSEngine.MSpage.Execute(503)
                    End If
                Else

                End If
                Dim serveItem As New KeyValuePair(Of String, Short)(ServerData, _PsId)
                ReceivedFromServerIsProcessed(serveItem)
                'remove Current PS Id from the list manager
                'Remove Processed item from the enqueue
                Interlocked.Exchange(PSProcessingResource, 0)

            End If

            Return True
        End Function

        <CLSCompliant(False)>
        Private Shared Function PsResponceMode(ByRef data As String) As PsFlag
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

        <CLSCompliant(False)>
        Private Shared Function PsCaptureMode(ByRef data As String) As PsResponseType
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


        ''' <summary>
        ''' gets the PsId from the server
        ''' Defaults to 0 if not found
        ''' </summary>
        ''' <param name="Data"></param>
        ''' <returns></returns>
        Private Shared Function PsCaptureId(ByRef Data As String) As Short
            Dim PsStat As Short = 0
            Dim IdCapture As New Regex("^PS (\d+)?", SmRegExOptions)

            Short.TryParse(IdCapture.Match(Data, 0).Groups(1).ToString(), PsStat)
            Data = IdCapture.Replace(Data, "", 1)

            Return PsStat
        End Function

        ''' <summary>
        ''' Get a Ps Variable List  from a REGEX match collection
        ''' <para>     Match(1) : Value(Name)</para>
        ''' <para>     Match 2: Empty if number, ' if string</para>
        ''' <para>     Match(3) : Value()</para>
        ''' </summary>
        ''' <param name="VariableList"></param>
        ''' <returns></returns>
        Private Shared Function ProcessVariables(ByRef data As String) As List(Of PhoenixSpeak.Variable)
            Dim mc As New Regex("(.*?)=('?)(\d+|.*?)(\2),?", SmRegExOptions)

            Dim PsVarList As New List(Of PhoenixSpeak.Variable)
            For Each M As Match In mc.Matches(data)

                PsVarList.Add(New PhoenixSpeak.Variable(M.Groups(1).Value, M.Groups(3).Value))

            Next
            Return PsVarList
        End Function

        ''' <summary>
        ''' returns number of Phoenix Speak pages remaining
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        Private Shared Function ProcessPage(ByRef data As String) As Short
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


#Region "Server Functions"

        ' clear the last item sent when its received
        Private Shared psSendToServer As New Queue(Of KeyValuePair(Of String, Short))(20)

        'clear the last item received when it's been processed
        Private Shared psReveiveFromServer As New Queue(Of KeyValuePair(Of String, Short))(20)


        ''' <summary>
        ''' Send the Phoenix Commands to the Server enqueue
        ''' </summary>
        ''' <param name="var"></param>
        Public Shared Sub sendServer(ByRef var As String, Optional ByVal id As Short = 0)
            Dim NewPsID As New PsId(id)
            var = var.Replace("ps ", "ps " + NewPsID.Id.ToString() + " ")
            Dim ServerQueueItem As New KeyValuePair(Of String, Short)(var, NewPsID.Id)
            Dim CommandTrigger As Boolean = psSendToServer.Count = 0
            ' there are possibilities that PS errors can come from the game server
            psSendToServer.Enqueue(ServerQueueItem)
            If CommandTrigger Then
                callbk.ServerStack.Enqueue(var)
                psSendToServer.Dequeue()
            End If
        End Sub

        ''' <summary>
        ''' 'Fills the received PS Buffer
        ''' </summary>
        ''' <param name="item">CommandKey PsId</param>
        Private Shared Sub ReceivedFromServer(ByVal item As KeyValuePair(Of String, Short))
            psReveiveFromServer.Enqueue(item)

        End Sub


        Private Shared Function ReceivedFromServerIsProcessed(ByVal item As KeyValuePair(Of String, Short)) As Boolean
            'psSendToServer.Dequeue()
            Dim CommandTrigger As Boolean = psReveiveFromServer.Count > 0
            If CommandTrigger Then


                'If psSendToServer.Count > psReveiveFromServer.Count Then psSendToServer.Dequeue()
                Dim id As Short = psReveiveFromServer.Peek().Value

                If psSendToServer.Count > 0 Then
                    Dim str As String = psSendToServer.Dequeue.Key
                    callbk.ServerStack.Enqueue(str)
                End If

                psReveiveFromServer.Dequeue()

                If PsId.HasId(id) Then PsId.remove(id)
            End If

            Return CommandTrigger
        End Function

        Public Shared Sub ClientMessage(ByVal msg As String)
            Main.SendClientMessage("PhoenixSpeak:", msg)
        End Sub


#End Region

        ''' <summary>
        ''' 
        ''' </summary>
        Public Shared Sub Abort()
            'Clear Enqueue's
            psReveiveFromServer.Clear()
            psSendToServer.Clear()
            PSInfoCache.Clear()
        End Sub

    End Class
End Namespace