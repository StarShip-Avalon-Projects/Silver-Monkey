Imports System.Text.RegularExpressions
Imports System.Threading
Imports Furcadia.Net
Imports SilverMonkeyEngine.SmConstants

Namespace Engine.Libraries.PhoenixSpeak

    ''' <summary>
    ''' Phoenix Speak processing system
    ''' <para>
    ''' Purpose, to Parse Phoenix Speak responses from the game server
    ''' </para>
    ''' <para>
    ''' and store the Phoenix Speak Variables in some sort of a list
    ''' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix Speak</see>
    ''' </para>
    ''' </summary>
    <CLSCompliant(True)>
    Public Class SubSystem
        Inherits BaseSubSystem

#Region "Public Delegates"

        Public Delegate Sub ParseChannel(obj As ParseChannelArgs, e As NetServerEventArgs)

#End Region

#Region "Public Events"

        Public Event ChannelParsed As ParseChannel

#End Region

#Region "Private Fields"

        ''' <summary>
        ''' PheonixSpeak object to send to <see cref="MsPhoenixSpeak"/>
        ''' </summary>
        Private PsObject As PhoenicSpeakDataObject

#End Region

#Region "Public Fields"

        ''' <summary>
        ''' 16 bit integer Game server ID for the Current Executing Phoenix
        ''' Speak Instruction set.
        ''' </summary>
        Public SubSystemPsID As PsId = New PsId

#End Region

#Region "Private Fields"

        ''' <summary>
        ''' Phoenix MultiPage limit is 6
        ''' <para>
        ''' If we hit this limit Inform the user there maybe more data unseen
        ''' </para>
        ''' </summary>
        Private PageOverFlow As Boolean = False

        ''' <summary>
        ''' </summary>
        Private PSProcessingResource As New Object

#End Region

#Region "Public Enums"

        <Flags>
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

            ''' <summary>
            ''' </summary>
            PsOk

        End Enum

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' </summary>
        Public Sub Abort()
            psSendToServer.Clear()
        End Sub

        '^PS (\d+)? ?Error:|Ok: (set:|get:)? multi_result? (\d+)?/?(\d+)?: Key/Value group
        ''' <summary>
        ''' process Phoenix Speak data coming from the game server Reg-ex
        ''' capture of server responses Execute Monkey Speak Engine
        ''' processing (Triggers setting Monkey Speak Variables) Always
        ''' clear PSInfoCache unless its a Multi-Page response
        ''' </summary>
        ''' <param name="ServerData">
        ''' Server Data as string
        ''' </param>
        Public Overrides Sub ParseServerChannel(ServerData As String, ByRef Handled As Boolean)
            Debug.WriteLine(ServerData)
            Dim data As String = ServerData
            Dim _PsId As Short = PsCaptureId(data)
            Dim ChannelObject As New PsResponseObject()
            ChannelObject.ServerData = ServerData
            'safety Check
            If _PsId = 0 Or Not SubSystemPsID.HasId(_PsId) Then
                'ReceivedFromServer(_PsId)
            End If

            'Send to receive Buffer

            Dim ResponceType As PsFlag = PsCaptureMode(data)
            Dim ResponceMode As PsFlag = PsResponceMode(data)

            If ResponceMode = PsFlag.PsGet Then
                PsObject = New PhoenicSpeakDataObject(data)
            Else 'Objects only good if we have something to put into it
                PsObject = Nothing
            End If

            ChannelObject.PsObject = PsObject

            If ResponceMode = PsFlag.PsGet And ResponceType = PsFlag.PsOk Then

                'load up the PSInfoCache
                If PsObject.MultiPage And PsObject.CurrentPage = 0 Then
                    Dim args As New PhoenixSpeakEventArgs
                    args.id = _PsId
                    args.PsType = ResponceType
                    args.Flag = ResponceMode
                    args.PageOverFlow = PageOverFlow
                    ' ProcessedFromeServer(PSInfoCache, args)

                    RaiseEvent ChannelParsed(ChannelObject, args)
                    FurcadiaSession.MSpage.SetVariable("MESSAGE", ServerData, True)
                    'Phoenix Speak Response set
                    FurcadiaSession.MSpage.Execute(80, 81, 82)
                Else 'Assume Single page responses
                    ' PSInfoCache = ProcessVariables(data)
                    'Phoenix Speak Response set

                    Dim args As New PhoenixSpeakEventArgs
                    args.id = _PsId
                    args.PsType = ResponceType
                    args.Flag = ResponceMode
                    args.PageOverFlow = PageOverFlow
                    RaiseEvent ChannelParsed(ChannelObject, args)

                    FurcadiaSession.MSpage.SetVariable("MESSAGE", ServerData, True)
                    FurcadiaSession.MSpage.Execute(80, 81, 82)

                    'trigger Monkey Speak responses
                End If

            ElseIf ResponceType = PsFlag.PsError Then
                'capture error
                'trigger error response

                'abort processes on error

                Dim args As New PhoenixSpeakEventArgs
                args.id = _PsId
                args.PsType = ResponceType
                args.Flag = ResponceMode
                args.PageOverFlow = PageOverFlow
                RaiseEvent ChannelParsed(ChannelObject, args)
                FurcadiaSession.MSpage.SetVariable("MESSAGE", ServerData, True)
                Dim AbortError As New Regex("Sorry, PhoenixSpeak commands are currently not available in this dream.$", SmRegExOptions)
                If AbortError.Match(data).Success Then
                    Abort()
                    '(0:503) When the bot finishes restoring the dreams character Phoenix Speak,
                    FurcadiaSession.MSpage.Execute(503)
                End If

            End If
            'remove Current PS Id from the list manager
            'Remove Processed item from the enqueue

        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' gets the PsId from the server Defaults to 0 if not found
        ''' </summary>
        ''' <param name="Data">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function PsCaptureId(ByRef Data As String) As Short
            Dim ThisPsId As Short = 0
            Dim IdCapture As New Regex("^PS (\d+)? ?", SmRegExOptions)

            Short.TryParse(IdCapture.Match(Data, 0).Groups(1).Value, ThisPsId)
            Data = IdCapture.Replace(Data, "", 1)

            Return ThisPsId
        End Function

        ''' <summary>
        ''' </summary>
        ''' <param name="data">
        ''' Game-Server data
        ''' </param>
        ''' <returns>
        ''' <see cref="PsFlag.PsError"/> or <see cref="PsFlag.PsOk"/>
        ''' </returns>
        <CLSCompliant(False)>
        Private Function PsCaptureMode(ByRef data As String) As PsFlag
            Dim ResponceType As PsFlag = PsFlag.PsUnknown
            Dim CaptureMode As New Regex(" (Error:|Ok:)?", SmRegExOptions)
            Select Case CaptureMode.Match(data).Groups(1).Value
                Case "Error:"
                    ResponceType = PsFlag.PsError
                Case "Ok:"
                    ResponceType = PsFlag.PsOk
            End Select
            data = CaptureMode.Replace(data, "", 1)
            Return ResponceType
        End Function

        ''' <summary>
        ''' Are we in a Get,Set or Error?
        ''' </summary>
        ''' <param name="data">
        ''' Game-Server data
        ''' </param>
        ''' <returns>
        ''' <see cref="PsFlag.PsGet"/> or <see cref="PsFlag.PsGet"/> or <see cref="PsFlag.PsUnknown"/>
        ''' </returns>
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

#Region "Events"

        ' Public Delegate Sub ParsePhoenixSpeak(o As Object, e As PhoenixSpeakEventArgs)

        ' Public Event PsReceived As ParsePhoenixSpeak

#End Region

#Region "Server Functions"

        Private Shared bufferlock As New Object

        ' clear the last item sent when its received
        Private Shared psSendToServer As New Queue(Of KeyValuePair(Of String, Short))(20)

        Private SendLock As New Object

        Public Sub New(FurcSession As BotSession)
            MyBase.New(FurcSession)
            PsObject = Nothing
        End Sub

        Public Sub ClientMessage(ByVal msg As String)
            'FurcSession.SendClientMessage("PhoenixSpeak:", msg)
        End Sub

        ''' <summary>
        ''' Send the Phoenix Commands to the Server queue
        ''' </summary>
        ''' <param name="var">
        ''' </param>
        Public Sub sendToServer(ByRef var As String)
            Try
                Monitor.Enter(SendLock)
                Dim Id As Short = PsCaptureId(var)
                Dim NewPsID As New PsId(Id)
                var = "ps " + Id.ToString + " " + var

                Dim ServerQueueItem As New KeyValuePair(Of String, Short)(var, Id)
                ' there are possibilities that PS errors can come from the
                ' game server
                psSendToServer.Enqueue(ServerQueueItem)
                FurcadiaSession.SendToServer(var)
            Finally
                Monitor.Exit(SendLock)
            End Try
            Debug.WriteLine("SendServer: " + var)
        End Sub

        '''' <summary>
        '''' 'Fills the received PS Buffer
        '''' </summary>
        'Private Shared Sub ReceivedFromServer(IsID As Short)
        '    Try
        '        Monitor.Enter(bufferlock)
        '        If psSendToServer.Count = 0 Then Exit Sub
        '        Dim id As Short = psSendToServer.Dequeue().Value
        '        ' PsId.remove(id)
        '        If psSendToServer.Count = 0 Then Exit Sub
        '        Dim args As New PhoenixSpeakEventArgs
        '        args.id = id

        '        ProcessedFromeServer(psSendToServer.Peek().Key, args)
        '    Finally
        '        Monitor.Exit(bufferlock)
        '    End Try
        'End Sub

#End Region

    End Class

End Namespace