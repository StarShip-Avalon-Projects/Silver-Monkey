'Furcadia Servver Parser
'Event/Delegates for server instructions
'call subsystem processor

'dream info
'Furre info
'Bot info

'Furre Update events?

Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Furcadia.Net.Proxy
Imports Furcadia.Net.Utils.ServerParser
Imports Furcadia.Text.FurcadiaMarkup
Imports Microsoft.Win32.SafeHandles
Imports MonkeyCore
Imports MonkeyCore.Utils.Logging
Imports Monkeyspeak
Imports SilverMonkeyEngine.Engine
Imports SilverMonkeyEngine.Engine.Libraries
Imports SilverMonkeyEngine.Engine.MsEngineExtentionFunctions
Imports SilverMonkeyEngine.MsLibHelper

''' <summary>
''' This Instance handles the current Furcadia Session.
''' <para>
''' Part1: Manage MonkeySpeak Engine Start,Stop,Restart. System Variables,
'''        MonkeySpeak Execution Triggers
''' </para>
''' <para>
''' Part2: Furcadia Proxy Controls, In/Out Ports, Host, Character Ini file.
'''        Connect, Disconnect, Reconnect
''' </para>
''' <para>
''' Part2a: Proxy Functions do link to Monkey Speak trigger execution
''' </para>
''' <para>
''' Part3: This Class Links loosley to the GUI
''' </para>
''' </summary>
<CLSCompliant(True)>
Public Class BotSession
    Inherits ProxySession
    Implements IDisposable

    Public Event CloseSession As Action

    ''' <summary>
    ''' Main MonkeySpeak Engine
    ''' </summary>
    Private MsEngine As MonkeyspeakEngine

    ''' <summary>
    ''' Monkey Speak Page object
    ''' </summary>
    Public WithEvents MSpage As Monkeyspeak.Page

    '  Public objHost As New smHost(Me)
    Private disposed As Boolean

    Private handle As SafeHandle = New SafeFileHandle(IntPtr.Zero, True)
    Private lastDream As DREAM
    ''' <summary>
    ''' Library Objects to load into the Engine
    ''' </summary>

    Public Property MainEngineOptions As BotOptions

    Private MainSettings As Settings.cMain

    ''' <summary>
    ''' </summary>
    Sub New()
        MyBase.New()
        MainEngineOptions = New BotOptions()
        lastDream = New DREAM
    End Sub

    ''' <summary>
    ''' New BotSession with Proxy Settings
    ''' </summary>
    ''' <param name="BotSessionOptions">
    ''' </param>
    Sub New(ByRef BotSessionOptions As BotOptions)
        MyBase.New(BotSessionOptions)
        lastDream = New DREAM
        MainEngineOptions = BotSessionOptions
    End Sub

    ''' <summary>
    '''
    ''' </summary>
    Public Shadows Event [Error] As ErrorEventHandler

    ''' <summary>
    ''' Name of the controller furre
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property BotController As String
        Get
            Return MainEngineOptions.BotController
        End Get
    End Property

    ''' <summary>
    ''' Is the Current executing Furre the Bot Controller?
    ''' </summary>
    ''' <returns>
    ''' True on success
    ''' </returns>
    Public ReadOnly Property IsBotController As Boolean
        Get
            Return Player.ShortName = MainEngineOptions.BotControllerShortName
        End Get
    End Property

    ''' <summary>
    ''' Starts the Furcadia Connection Process
    ''' </summary>

    Public Async Sub ConnetAsync()
        If MainEngineOptions.MonkeySpeakEngineOptions.MS_Engine_Enable Then
            Await StartEngine()
        End If
        Await Task.Run(Sub() Connect())
    End Sub

    ''' <summary>
    ''' Disconnect from the Game Server and Client
    ''' </summary>
    Public Overrides Sub Disconnect()

        MyBase.Disconnect()

    End Sub

    ''' <summary>
    ''' Implement IDisposable Support
    ''' </summary>
    Public Overrides Sub Dispose() Implements IDisposable.Dispose

        Dispose(True)
    End Sub

    ''' <summary>
    ''' Configure Bot Variables and Execute Monkey Speak Truggers for Client to Server Instructions
    ''' </summary>
    ''' <param name="sender"><see cref="Object"/></param>
    ''' <param name="e"><see cref="ParseServerArgs"/></param>
    Public Async Sub OnParseSererInstructionAsync(sender As Object, e As ParseServerArgs) _
          Handles MyBase.ProcessServerInstruction
        If MSpage Is Nothing Then Exit Sub

        Try
            Select Case e.ServerInstruction

                Case ServerInstructionType.LoadDreamEvent
                    '(0:97) When the bot leaves a Dream,
                    '(0:98) When the bot leaves the Dream named {..},
                    If InDream Then
                        Dim ids() = {97, 98}
                        Await MSpage.ExecuteAsync(ids, lastDream)
                    End If

                Case ServerInstructionType.BookmarkDream
                    '(0:90) When the bot enters a Dream,
                    '(0:91) When the bot enters a Dream named {..},
                    Dim ids() = {90, 91}
                    Await MSpage.ExecuteAsync(ids, Dream)
                    lastDream = Dream

                Case ServerInstructionType.AnimatedMoveAvatar
                    Await MSpage.ExecuteAsync(600, DirectCast(sender, Furre))

                Case ServerInstructionType.MoveAvatar
                    Await MSpage.ExecuteAsync(600, DirectCast(sender, Furre))

                Case ServerInstructionType.RemoveAvatar
                    Dim furId = DirectCast(sender, RemoveAvatar).FurreId
                    Dim fur = Dream.Furres.GetFurreByID(furId)

                Case ServerInstructionType.SpawnAvatar
                    Dim fur = DirectCast(sender, SpawnAvatar).player

                Case ServerInstructionType.UpdateColorString
                    Dim fur = ConnectedFurre

                Case ServerInstructionType.LookResponse
                    Dim fur = DirectCast(sender, SpawnAvatar).player

                Case ServerInstructionType.Unknown
                    Exit Select
                Case Else
                    Exit Select
            End Select
        Catch ex As Exception
            Dim Err As New ErrorLogging(ex, Me)
            Process.Start("notepad.exe", Err.LogFile)
        End Try

    End Sub

    ''' <summary>
    ''' Configure Bot Variables and Execute Monkey Speak Truggers for Text and dynamic channels
    ''' </summary>
    ''' <param name="InstructionObject"><see cref="ChannelObject"/></param>
    ''' <param name="Args"><see cref="ParseServerArgs"/></param>
    Public Async Sub OnServerChannel(InstructionObject As ChannelObject, Args As ParseServerArgs) _
        Handles MyBase.ProcessServerChannelData
        If MSpage Is Nothing Then Exit Sub
        Dim Furr = InstructionObject.Player

        UpdateTriggerigFurreVariables(Furr, MSpage)

        Dim Text As String = InstructionObject.ChannelText
        Try
            Select Case InstructionObject.Channel

                Case "@roll"

                    If IsConnectedCharacter() Then
                        '(0:130) When the bot rolls #d#,
                        '(0:132) When the bot rolls #d#+#,
                        '(0:134) When the bot rolls #d#-#,
                        '(0:136) When any one rolls anything,
                        Dim ids() = {130, 131, 132, 136}
                        Await MSpage.ExecuteAsync(ids, InstructionObject)
                    Else
                        '(0:136) When a furre rolls #d#,
                        '(0:138) When a fuure rolls #d#+#,
                        '(0:140) When a furre rolls #d#-#,
                        '(0:136) When any one rolls anything,
                        Dim ids() = {133, 134, 135, 136}
                        Await MSpage.ExecuteAsync(ids, InstructionObject)
                    End If
                Case "trade"
                    Dim ids() = {46, 47, 48}
                    Await MSpage.ExecuteAsync(ids)
                Case "shout"
                    '(0:8) When someone shouts something,
                    '(0:9) When someone shouts {..},
                    '(0:10) When someone shouts something with {..} in it,
                    If IsConnectedCharacter() Then Exit Sub
                    If InstructionObject.RawInstruction.StartsWith("<font color='shout'>You shout,") Then Exit Sub
                    Dim ids() = {8, 9, 10}
                    Await MSpage.ExecuteAsync(ids, Furr)
                Case "say"
                    ' (0:5) When some one says something
                    ' (0:6) When some one says {...}
                    '(0:7) When some one says something with {...} in it
                    ' (0:18) When someone says or emotes something
                    ' (0:19) When someone says or emotes {...}
                    ' (0:20) When someone says or emotes something with
                    ' {...} in it"
                    Dim ids() = {5, 6, 7, 18, 19, 20}
                    Await MSpage.ExecuteAsync(ids, Furr)

                Case "@whisper"

                    ' (0:15) When some one whispers something
                    ' (0:16) When some one whispers {...}
                    ' (0:17) When some one whispers something
                    ' with {...} in it
                    Dim Ids() = {15, 16, 17}
                    Await MSpage.ExecuteAsync(Ids, Furr)

                Case "emote"
                    If IsConnectedCharacter() Then Exit Sub
                    ' (0:12) When someone emotes {...} Execute
                    ' (0:13) When someone emotes something with {...} in it
                    ' (0:18) When someone says or emotes something
                    ' (0:19) When someone says or emotes {...}
                    ' (0:20) When someone says or emotes something
                    ' with {...} in it
                    Dim ids() = {11, 12, 13, 18, 19, 20}
                    Await MSpage.ExecuteAsync(ids, Furr)
                Case "@emit"
                    ' (0:21) When someone emits something
                    ' (0:22) When someone emits {...}
                    ' (0:23) When someone emits something with {...} in it
                    Dim ids() = {21, 22, 23}
                    Await MSpage.ExecuteAsync(ids, Furr, Dream)

                Case "query"

                    Dim QueryComand As String = New Regex("<a.*?href='command://(.*?)'>").Match(Text).Groups(1).Value

                    Select Case QueryComand
                        Case "summon"
                            ''JOIN
                            Dim ids() = {34, 35}
                            Await MSpage.ExecuteAsync(ids, Furr)

                        Case "join"
                            ''SUMMON
                            Dim ids() = {32, 33}
                            Await MSpage.ExecuteAsync(ids, Furr)

                        Case "follow"
                            ''LEAD
                            Dim ids() = {36, 37}
                            Await MSpage.ExecuteAsync(ids, Furr)

                        Case "lead"
                            ''FOLLOW
                            Dim ids() = {38, 39}
                            Await MSpage.ExecuteAsync(ids, Furr)
                        Case "cuddle"
                            Dim ids() = {40, 41}
                            Await MSpage.ExecuteAsync(ids, Furr)

                    End Select
                Case "banish"
                    Dim NameStr As String

                    DirectCast(MSpage.GetVariable("%BANISHLIST"), ConstantVariable).SetValue(String.Join(" ", BanishString.ToArray))

                    If Text.Contains(" has been banished from your dreams.") Then
                        'banish <name> (online)
                        'Success: (.*?) has been banished from your dreams.

                        '(0:52) When the bot sucessfilly banishes a furre,
                        '(0:53) When the bot sucessfilly banishes the furre named {...},
                        'Success: You have canceled all banishments from your dreams.

                        DirectCast(MSpage.GetVariable("%BANISHNAME"), ConstantVariable).SetValue(BanishName)
                        Dim ids() = {52, 53}
                        Await MSpage.ExecuteAsync(ids)

                        ' MSpage.ExecuteAsync(53)
                    ElseIf Text = "You have canceled all banishments from your dreams." Then
                        'banish-off-all (active list)
                        'Success: You have canceled all banishments from your dreams.
                        DirectCast(MSpage.GetVariable("%BANISHLIST"), ConstantVariable).SetValue(Nothing)
                        DirectCast(MSpage.GetVariable("%BANISHNAME"), ConstantVariable).SetValue(Nothing)

                        Await MSpage.ExecuteAsync(60)
                    ElseIf Text.EndsWith(" has been temporarily banished from your dreams.") Then
                        'tempbanish <name> (online)
                        'Success: (.*?) has been temporarily banished from your dreams.

                        '(0:61) When the bot sucessfully temp banishes a Furre
                        '(0:62) When the bot sucessfully temp banishes the furre named {...}
                        DirectCast(MSpage.GetVariable("%BANISHNAME"), ConstantVariable).SetValue(BanishName)
                        Dim ids() = {61, 62}
                        Await MSpage.ExecuteAsync(ids)

                    ElseIf Text.StartsWith("Players banished from your dreams: ") Then
                        'Banish-List
                        '[notify> Players banished from your dreams:
                        '`(0:54) When the bot sees the banish list

                        Await MSpage.ExecuteAsync(54)
                    ElseIf Text.StartsWith("The banishment of player ") Then
                        'banish-off <name> (on list)
                        '[notify> The banishment of player (.*?) has ended.

                        '(0:56) When the bot successfully removes a furre from the banish list,
                        '(0:58) When the bot successfully removes the furre named {...} from the banish list,
                        Dim t As New Regex("The banishment of player (.*?) has ended.", RegexOptions.Compiled)
                        NameStr = t.Match(Text).Groups(1).Value
                        DirectCast(MSpage.GetVariable("%BANISHNAME"), ConstantVariable).SetValue(BanishName)
                        Dim ids() = {56, 56}
                        Await MSpage.ExecuteAsync(ids)

                        '      MSpage.ExecuteAsync(800)

                    ElseIf Text.Contains("There are no furres around right now with a name starting with ") Then
                        'Banish <name> (Not online)
                        'Error:>>  There are no furres around right now with a name starting with (.*?) .

                        '(0:50) When the Bot fails to banish a furre,
                        '(0:51) When the bot fails to banish the furre named {...},
                        Dim t As New Regex("There are no furres around right now with a name starting with (.*?) .", RegexOptions.Compiled)
                        NameStr = t.Match(Text).Groups(1).Value
                        DirectCast(MSpage.GetVariable("%BANISHNAME"), ConstantVariable).SetValue(NameStr)
                        Dim ids() = {50, 51}
                        Await MSpage.ExecuteAsync(ids)
                    ElseIf Text = "Sorry, this player has not been banished from your dreams." Then
                        'banish-off <name> (not on list)
                        'Error:>> Sorry, this player has not been banished from your dreams.

                        '(0:55) When the Bot fails to remove a furre from the banish list,
                        '(0:56) When the bot fails to remove the furre named {...} from the banish list,
                        DirectCast(MSpage.GetVariable("%BANISHNAME"), ConstantVariable).SetValue(BanishName)
                        Dim ids() = {50, 51}
                        Await MSpage.ExecuteAsync(ids)
                    ElseIf Text = "You have not banished anyone." Then
                        'banish-off-all (empty List)
                        'Error:>> You have not banished anyone.

                        '(0:59) When the bot fails to see the banish list,
                        DirectCast(MSpage.GetVariable("%BANISHLIST"), ConstantVariable).SetValue(Nothing)

                        Await MSpage.ExecuteAsync(59)
                    ElseIf Text = "You do not have any cookies to give away right now!" Then
                        Await MSpage.ExecuteAsync(95)
                    End If

                Case "@cookie"
                    ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>
                    ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> All-time Cookie total: 0</font>
                    ' <font color='success'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Your cookies are ready.  http://furcadia.com/cookies/ for more info!</font>
                    '<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.

                    Dim CookieToMe = New Regex(String.Format("{0}", CookieToMeREGEX))
                    If CookieToMe.Match(Text).Success Then
                        Dim ids() = {42, 43}
                        Await MSpage.ExecuteAsync(ids, Furr)
                    End If
                    Dim CookieToAnyone As Regex = New Regex(String.Format("<name shortname='(.*?)'>(.*?)</name> just gave <name shortname='(.*?)'>(.*?)</name> a (.*?)"))
                    If CookieToAnyone.Match(Text).Success Then

                        If IsConnectedCharacter() Then
                            Dim ids() = {42, 43}
                            Await MSpage.ExecuteAsync(ids, Furr)
                        Else
                            Await MSpage.ExecuteAsync(44, Furr)
                        End If

                    End If
                    Dim CookieFail = New Regex(String.Format("You do not have any (.*?) left!"))
                    If CookieFail.Match(Text).Success Then
                        Await MSpage.ExecuteAsync(45, Furr)
                    End If
                    Dim EatCookie = New Regex(Regex.Escape("<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.") + "(.*?)")
                    If EatCookie.Match(Text).Success Then
                        'TODO Cookie eat %MESSAGE can change by Dragon Speak

                        Await MSpage.ExecuteAsync(49, Furr)

                    End If
                    '(0:96) When the Bot sees "Your cookies are ready."
                    Dim CookiesReady As Regex = New Regex(<a>"Your cookies are ready.  http://furcadia.com/cookies/ for more info!"</a>)
                    If CookiesReady.Match(Text).Success Then
                        Await MSpage.ExecuteAsync(96, Furr)
                    End If
                Case Else
                    'TODO: plugin Dynamic(Group)  Channels here

            End Select
        Catch ex As Exception
            Dim Err As New ErrorLogging(ex, Me)
            Process.Start("notepad.exe", Err.LogFile)
        End Try
    End Sub

    ''' <summary>
    ''' Set the Bot System Variables for the <see cref="Monkeyspeak.Page"/>
    ''' </summary>
    ''' <param name="VariableList"></param>
    Public Sub PageSetVariable(ByVal VariableList As List(Of IVariable))

        For Each var In VariableList
            MSpage.SetVariable(var)
        Next '

    End Sub

    ''' <summary>
    ''' Send a formatted string to the client and log window
    ''' </summary>
    ''' <param name="msg">
    ''' Channel Subsystem?
    ''' </param>
    ''' <param name="data">
    ''' %MESSAGE to send
    ''' </param>
    Public Sub SendToClientFormattedText(msg As String, data As String)
        SendToClient("(" + "<b><i>[SM]</i> - " + msg + ":</b> """ + data + """")
        'Writer.WriteLine("<b><i>[SM]</i> - " + msg + ":</b> """ + data + """")

    End Sub

    ''' <summary>
    ''' Start the Monkey Speak Engine
    ''' </summary>
    Public Async Function StartEngine() As Task
        Try

            MsEngine = New MonkeyspeakEngine(MainEngineOptions.MonkeySpeakEngineOptions)
            Dim SriptFile As String = Await LoadFromScriptFileAsync(MainEngineOptions.MonkeySpeakEngineOptions.MonkeySpeakScriptFile)
            If String.IsNullOrWhiteSpace(SriptFile) Then Throw New NullReferenceException("SriptFile")
            MSpage = Await MsEngine.LoadFromStringAsync(SriptFile)

            Dim TimeStart = DateTime.Now
            Dim VariableList As New List(Of IVariable)

            MSpage = Await LoadLibraryAsync(False)
            Dim fur As IFurre = New Furre
            VariableList.Add(New ConstantVariable("%DREAMOWNER", lastDream.Owner))
            VariableList.Add(New ConstantVariable("%DREAMNAME", lastDream.Name))
            VariableList.Add(New ConstantVariable("%BOTNAME", ConnectedFurre.Name))
            VariableList.Add(New ConstantVariable("%BOTCONTROLLER", MainEngineOptions.BotController))
            VariableList.Add(New ConstantVariable("%NAME", fur.Name))
            VariableList.Add(New ConstantVariable("%SHORTNAME", fur.ShortName))
            VariableList.Add(New ConstantVariable("%MESSAGE", fur.Message))
            VariableList.Add(New ConstantVariable("%BANISHNAME", Nothing))
            VariableList.Add(New ConstantVariable("%BANISHLIST", Nothing))

            PageSetVariable(VariableList)
            '(0:0) When the bot starts,
            Await MSpage.ExecuteAsync(0)

            Logging.Logger.Info(String.Format("Done!!! Executed {0} triggers in {1} seconds.",
                                            MSpage.Size, Date.Now.Subtract(TimeStart).Seconds))
        Catch ex As Exception
            Dim Err As New ErrorLogging(ex, Me)
            Process.Start("notepad.exe", Err.LogFile)
        End Try
    End Function

    ''' <summary>
    ''' Stop the Monkey Speak Engine
    ''' </summary>
    Public Sub StopEngine()
        '  RemoveHandler ProcessServerChannelData, Me

        If Not MSpage Is Nothing Then
            MSpage.Reset(True)
            MSpage.Dispose()
            MSpage = Nothing
        End If

    End Sub

    ''' <summary>
    ''' Dispose components
    ''' </summary>
    ''' <param name="disposing"></param>
    Protected Overloads Sub Dispose(ByVal disposing As Boolean)

        If Me.disposed Then Exit Sub
        If disposing Then

            ' Free your own state (unmanaged objects).
            ' Set large fields to null.
            MyBase.Dispose()
        End If
        Me.disposed = True
        Me.Finalize()
    End Sub

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="e"></param>
    ''' <param name="o"></param>
    ''' <param name="n"></param>
    Protected Overrides Sub SendError(e As Exception, o As Object, n As String) Handles MyBase.[Error]
        RaiseEvent [Error](e, o, n)
    End Sub

    ''' <summary>
    ''' Pump MonkeySpeak Exceptions to the error handler
    ''' </summary>
    ''' <param name="handler"></param>
    ''' <param name="Trigger"></param>
    ''' <param name="ex"></param>
    Private Sub OnMonkeySpeakError(handler As TriggerHandler, Trigger As Trigger, ex As Exception) Handles MSpage.Error
        If ex.GetType IsNot GetType(MonkeyspeakException) Then
            Dim PageError As New MonkeyspeakException(String.Format("Trigger Error: {0}", Trigger.ToString), ex)
            SendError(PageError, handler, Trigger.ToString)
        Else
            SendError(ex, handler, Trigger.ToString)
        End If

    End Sub

    Private Sub BotSession_ClientStatusChanged(Sender As Object, e As NetClientEventArgs) Handles Me.ClientStatusChanged
        If MSpage Is Nothing Then Exit Sub
        Try
            Select Case e.ConnectPhase
                Case ConnectionPhase.Connected
                    DirectCast(MSpage.GetVariable("%BOTNAME"), ConstantVariable).SetValue(ConnectedFurre.Name)

            End Select
        Catch ex As Exception
            Dim Err As New ErrorLogging(ex, Me)
            Process.Start("notepad.exe", Err.LogFile)
        End Try
    End Sub

    Private Async Sub OnServerStatusChanged(Sender As Object, e As NetServerEventArgs) Handles Me.ServerStatusChanged
        Try
            Select Case e.ConnectPhase

                Case ConnectionPhase.Disconnected
                    '(0:2) When the bot logs out of furcadia,
                    If MSpage IsNot Nothing Then Await MSpage.ExecuteAsync(2)
                    If MSpage IsNot Nothing Then Await Task.Run(Sub() StopEngine())
                Case ConnectionPhase.Connected
                    Dim Id() As Integer = {1}
                    '(0:1) When the bot logs into furcadia,
                    If MSpage IsNot Nothing Then Await MSpage.ExecuteAsync(1)

            End Select
        Catch ex As Exception
            Dim Err As New ErrorLogging(ex, Me)
            Process.Start("notepad.exe", Err.LogFile)
        End Try
    End Sub

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="silent"></param>
    ''' <returns></returns>
    Public Async Function LoadLibraryAsync(silent As Boolean) As Task(Of Page)
        Return Await Task.Run(Function() LoadLibrary(silent))
    End Function

    ''' <summary>
    ''' Load Libraries into the engine
    ''' </summary>
    ''' <param name="silent"> Announce Loaded Libraries</param>
    Public Function LoadLibrary(silent As Boolean) As Page

        'Library Loaded?.. Get the Hell out of here
        MSpage.SetTriggerHandler(TriggerCategory.Cause, 0, Function() True,
        " When the Monkey Speak Engine starts,")

        Dim LibList = InitializeEngineLibraries()

        For Each Library As Monkeyspeak.Libraries.BaseLibrary In LibList
            Try
                MSpage.LoadLibrary(Library)
                If Not silent Then Logging.Logger.Info($"{Library.GetType().Name}")
            Catch ex As Exception
                Throw New MonkeyspeakException(Library.GetType().Name + " " + ex.Message, ex)

            End Try
        Next

        Return MSpage

    End Function

    Private Function InitializeEngineLibraries() As List(Of Monkeyspeak.Libraries.BaseLibrary)
        ' Comment out Libs to Disable

        Dim LibList = New List(Of Monkeyspeak.Libraries.BaseLibrary) From {
                New Libraries.Debug(),
                New Libraries.IO(MainEngineOptions.BotPath),
                New Libraries.Math(),
                New Libraries.StringOperations(),
                New Libraries.Sys(),
                New Libraries.Timers(100),
                New Libraries.Loops(),
                New Libraries.Tables(),
                New StringLibrary(Me),
                New MsSayLibrary(Me),
                New MsBanish(Me),
                New MsTime(Me),
                New MsDatabase(Me),
                New MsWebRequests(Me),
                New MS_Cookie(Me),
                New MsPhoenixSpeak(Me),
                New MsPhoenixSpeakBackupAndRestore(Me),
                New MsDice(Me),
                New MsFurres(Me),
                New MsWarning(Me),
                New MsMovement(Me),
                New WmCpyDta(Me),
                New MsMemberList(Me),
                New MsPounce(Me),
                New MsSound(Me),
                New MsTrades(Me),
                New MsDreamInfo(Me)
                                }
        'New MsVerbot(Session),
        ' New MathLibrary(Me),
        'New MsIO(Me),
        'LibList.Add(New MS_MemberList())

        Return LibList
    End Function

End Class