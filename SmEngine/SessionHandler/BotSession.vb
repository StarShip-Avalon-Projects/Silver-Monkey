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
Imports Furcadia.Net.Proxy
Imports Furcadia.Net.Utils.ServerParser
Imports Furcadia.Text.FurcadiaMarkup
Imports Microsoft.Win32.SafeHandles
Imports MonkeyCore
Imports Monkeyspeak
Imports SilverMonkeyEngine.Engine
Imports SilverMonkeyEngine.Engine.Libraries
Imports SilverMonkeyEngine.Interfaces
Imports SilverMonkeyEngine.SmConstants

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
Public Class BotSession : Inherits ProxySession
    Implements IDisposable

    Public Event CloseSession As Action

    ''' <summary>
    ''' Main MonkeySpeak Engine
    ''' </summary>
    Public WithEvents MainEngine As Engine.MainEngine

    ''' <summary>
    ''' Monkey Speak Page object
    ''' </summary>
    Public WithEvents MSpage As Monkeyspeak.Page

    Public objHost As New smHost(Me)
    Private disposed As Boolean
    Private handle As SafeHandle = New SafeFileHandle(IntPtr.Zero, True)

    ''' <summary>
    ''' Library Objects to load into the Engine
    ''' </summary>
    Private LibList As List(Of Monkeyspeak.Libraries.BaseLibrary)

    Private MainEngineOptions As BotOptions

    Private MainSettings As Settings.cMain

    ''' <summary>
    ''' </summary>
    Sub New()
        MyBase.New()
        MainEngineOptions = New BotOptions()

    End Sub

    ''' <summary>
    ''' New BotSession with Proxy Settings
    ''' </summary>
    ''' <param name="BotSessionOptions">
    ''' </param>
    Sub New(ByRef BotSessionOptions As BotOptions)
        MyBase.New(BotSessionOptions)
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

    'Monkey Speak Bot specific Variables
    ''' <summary>
    ''' Starts the Furcadia Connection Process
    ''' </summary>
    Public Overrides Sub Connect()

        Try

            If MainEngineOptions.MonkeySpeakEngineOptions.MS_Engine_Enable Then
                StartEngine()
            End If
            MyBase.Connect()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Disconnect from the Game Server and Client
    ''' </summary>
    Public Overrides Sub Disconnect()
        ' (0:2) When the bot logs off
        If MainEngineOptions.MonkeySpeakEngineOptions.MS_Engine_Enable Then
            MSpage.ExecuteAsync(2)

        End If

        MyBase.Disconnect()
        StopEngine()
    End Sub

    ''' <summary>
    ''' Implement IDisposable Support
    ''' </summary>
    Public Overrides Sub Dispose() Implements IDisposable.Dispose

        Dispose(True)
    End Sub

    ''' <summary>
    ''' Load Libraries into the engine
    ''' </summary>
    ''' <param name="LoadPlugins">
    ''' </param>
    Public Sub LoadLibrary(ByRef LoadPlugins As Boolean, ByVal silent As Boolean)
        'Library Loaded?.. Get the Hell out of here
        MSpage.SetTriggerHandler(TriggerCategory.Cause, 0, Function(reader) True,
        "(0:0) When the bot starts,")

        MSpage.LoadSysLibrary()

#If CONFIG = "Release" Then
            '(5:110) load library from file {...}.
            MSpage.RemoveTriggerHandler(TriggerCategory.Effect, 110)
#ElseIf CONFIG = "Debug" Then
        '
        MSpage.RemoveTriggerHandler(TriggerCategory.Effect, 105)
        MSpage.SetTriggerHandler(TriggerCategory.Effect, 105,
     Function() False, "(5:105) raise an error.")
        ',
#End If

        MSpage.LoadTimerLibrary(100)
        MSpage.LoadIOLibrary(MainEngineOptions.BotPath)
        MSpage.LoadStringLibrary()
        MSpage.LoadMathLibrary()
        InitializeEngineLibraries()

        For Each Library As Monkeyspeak.Libraries.BaseLibrary In LibList
            Try
                MSpage.LoadLibrary(Library)
                If Not silent Then Console.WriteLine(String.Format("Loaded Monkey Speak Library: {0}", Library.GetType().Name))
            Catch ex As Exception
                Throw New MonkeyspeakException(Library.GetType().Name + " " + ex.Message, ex)
                Exit Sub
            End Try
        Next

        'Define our Triggers before we use them
        'TODO Check for Duplicate and use that one instead
        'we don't want this to cause a memory leak.. its prefered to run this one time and thats  it except for checking for new plugins
        'Loop through available plugins, creating instances and adding them to listbox
        'If Not Plugins Is Nothing And LoadPlugins Then

        'End If
        'Dim objPlugin As Interfaces.msPlugin
        'Dim newPlugin As Boolean = False
        'For intIndex As Integer = 0 To Plugins.Count - 1
        '    Try
        '        objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(intIndex)), Interfaces.msPlugin)
        '        If Not PluginList.ContainsKey(objPlugin.Name.Replace(" ", "")) Then
        '            PluginList.Add(objPlugin.Name.Replace(" ", ""), True)
        '            newPlugin = True
        '        End If

        ' If PluginList.Item(objPlugin.Name.Replace(" ", "")) = True
        ' Then Console.WriteLine("Loading Plugin: " + objPlugin.Name)
        ' objPlugin.Initialize(objHost) objPlugin.MonkeySpeakPage =
        ' MonkeySpeakPage objPlugin.Start() End If Catch ex As Exception
        ' Dim e As New ErrorLogging(ex, Me) End Try Next 'TODO: Add to
        '' Delegate? 'If newPlugin Then Main.MainSettings.SaveMainSettings()

        'End If

    End Sub

    ''' <summary>
    ''' Configure Bot Variables and Execute Monkey Speak Truggers for Client to Server Instructions
    ''' </summary>
    ''' <param name="sender"><see cref="Object"/></param>
    ''' <param name="e"><see cref="ParseServerArgs"/></param>
    Public Sub OnParseSererInstruction(sender As Object, e As ParseServerArgs) Handles MyBase.ProcessServerInstruction

        If MSpage Is Nothing Then Exit Sub
        If IsConnectedCharacter Then Exit Sub
        Try
            MSpage.RemoveVariable("NAME")
            MSpage.RemoveVariable("MESSAGE")
            MSpage.SetVariable("NAME", Player.Name, True)
            MSpage.SetVariable("MESSAGE", Player.Message, True)

        Catch ex As Exception
            RaiseEvent [Error](ex, Me, "Failure to set Triggering Furre and Triggering Furres message variables")
        End Try

        Select Case e.ServerInstruction

            Case ServerInstructionType.LoadDreamEvent
                Try
                    MSpage.RemoveVariable("DREAMOWNER")
                    MSpage.SetVariable("DREAMOWNER", Dream.Owner, True)
                    MSpage.RemoveVariable("DREAMNAME")
                    MSpage.SetVariable("DREAMNAME", Dream.Name, True)
                Catch ex As Exception
                    RaiseEvent [Error](ex, Me, "Failure to set Dream Variables")
                End Try
                '(0:90) When the bot enters a Dream,
                '(0:91) When the bot enters a Dream named {..},
                MSpage.ExecuteAsync(92, 93)

            Case ServerInstructionType.BookmarkDream
                Try
                    MSpage.RemoveVariable("DREAMOWNER")
                    MSpage.SetVariable("DREAMOWNER", Dream.Owner, True)
                    MSpage.RemoveVariable("DREAMNAME")
                    MSpage.SetVariable("DREAMNAME", Dream.Name, True)
                Catch ex As Exception
                    RaiseEvent [Error](ex, Me, "Failure to set Dream Variables")
                End Try
                '(0:90) When the bot enters a Dream,
                '(0:91) When the bot enters a Dream named {..},
                MSpage.ExecuteAsync(90, 91)

        End Select

    End Sub

    Public Sub OnDisconnected() Handles MyBase.ServerDisConnected
        RaiseEvent CloseSession()
    End Sub

    ''' <summary>
    ''' Configure Bot Variables and Execute Monkey Speak Truggers for Text and dynamic channels
    ''' </summary>
    ''' <param name="InstructionObject"><see cref="ChannelObject"/></param>
    ''' <param name="Args"><see cref="ParseServerArgs"/></param>
    Public Sub OnServerChannel(InstructionObject As ChannelObject, Args As ParseServerArgs) Handles MyBase.ProcessServerChannelData
        If MSpage Is Nothing Then Exit Sub
        If IsConnectedCharacter Then Exit Sub
        Try
            MSpage.RemoveVariable("NAME")
            MSpage.RemoveVariable("MESSAGE")
            MSpage.SetVariable("NAME", Player.Name, True)
            MSpage.SetVariable("MESSAGE", Player.Message, True)

        Catch ex As Exception
            RaiseEvent [Error](ex, Me, "Failed to set Triggering Furre Monkeyspeak Variables")
        End Try
        Dim Text As String = InstructionObject.ChannelText

        Select Case InstructionObject.Channel

            Case "@roll"
                Dim DiceObject As DiceRolls = Nothing
                If InstructionObject.GetType().Equals(GetType(DiceRolls)) Then
                    DiceObject = DirectCast(InstructionObject, DiceRolls)
                    MsDice.dice = DiceObject.Dice
                End If

                If IsConnectedCharacter Then
                    '(0:130) When the bot rolls #d#,
                    '(0:132) When the bot rolls #d#+#,
                    '(0:134) When the bot rolls #d#-#,
                    '(0:136) When any one rolls anything,
                    MSpage.ExecuteAsync(130, 131, 132, 136)
                Else
                    '(0:136) When a furre rolls #d#,
                    '(0:138) When a fuure rolls #d#+#,
                    '(0:140) When a furre rolls #d#-#,
                    '(0:136) When any one rolls anything,
                    MSpage.ExecuteAsync(133, 134, 135, 136)
                End If
            Case "trade"
                MSpage.ExecuteAsync(46, 47, 48)
            Case "shout"
                '(0:8) When someone shouts something,
                '(0:9) When someone shouts {..},
                '(0:10) When someone shouts something with {..} in it,
                MSpage.ExecuteAsync(8, 9, 10)
            Case "say"
                ' (0:5) When some one says something
                ' (0:6) When some one says {...}
                '(0:7) When some one says something with {...} in it
                ' (0:18) When someone says or emotes something
                ' (0:19) When someone says or emotes {...}
                ' (0:20) When someone says or emotes something with
                ' {...} in it"

                MSpage.ExecuteAsync(5, 6, 7, 18, 19, 20)

            Case "whisper"

                ' (0:15) When some one whispers something
                ' (0:16) When some one whispers {...}
                ' (0:17) When some one whispers something
                ' with {...} in it
                MSpage.ExecuteAsync(15, 16, 17)

            Case "emote"
                ' (0:12) When someone emotes {...} Execute
                ' (0:13) When someone emotes something with {...} in it
                ' (0:18) When someone says or emotes something
                ' (0:19) When someone says or emotes {...}
                ' (0:20) When someone says or emotes something
                ' with {...} in it

                MSpage.ExecuteAsync(11, 12, 13, 18, 19, 20)
            Case "emit"
                ' (0:21) When someone emits something
                ' (0:22) When someone emits {...}
                ' (0:23) When someone emits something with {...} in it

                MSpage.ExecuteAsync(21, 22, 23)
            Case "@emit"
                ' (0:21) When someone emits something
                ' (0:22) When someone emits {...}
                ' (0:23) When someone emits something with {...} in it

                MSpage.ExecuteAsync(21, 22, 23)

            Case "query"

                Dim QueryComand As String = New Regex("<a.*?href='command://(.*?)'>").Match(Text).Groups(1).Value

                Select Case QueryComand
                    Case "summon"
                        ''JOIN
                        MSpage.ExecuteAsync(34, 35)

                    Case "join"
                        ''SUMMON
                        MSpage.ExecuteAsync(32, 33)

                    Case "follow"
                        ''LEAD
                        MSpage.ExecuteAsync(36, 37)

                    Case "lead"
                        ''FOLLOW
                        MSpage.ExecuteAsync(38, 39)
                    Case "cuddle"
                        MSpage.ExecuteAsync(40, 41)

                End Select
            Case "banish"
                Dim NameStr As String

                MSpage.SetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray), True)

                If Text.Contains(" has been banished from your dreams.") Then
                    'banish <name> (online)
                    'Success: (.*?) has been banished from your dreams.

                    '(0:52) When the bot sucessfilly banishes a furre,
                    '(0:53) When the bot sucessfilly banishes the furre named {...},
                    'Success: You have canceled all banishments from your dreams.
                    MSpage.RemoveVariable("BANISHNAME")
                    MSpage.SetVariable("BANISHNAME", BanishName, True)

                    MSpage.ExecuteAsync(52, 53)

                    ' MSpage.ExecuteAsync(53)
                ElseIf Text = "You have canceled all banishments from your dreams." Then
                    'banish-off-all (active list)
                    'Success: You have canceled all banishments from your dreams.
                    MSpage.RemoveVariable("BANISHNAME")
                    MSpage.RemoveVariable("BANISHLIST")
                    MSpage.SetVariable("BANISHLIST", Nothing, True)
                    MSpage.SetVariable("BANISHNAME", Nothing, True)
                    MSpage.ExecuteAsync(60)

                ElseIf Text.EndsWith(" has been temporarily banished from your dreams.") Then
                    'tempbanish <name> (online)
                    'Success: (.*?) has been temporarily banished from your dreams.

                    '(0:61) When the bot sucessfully temp banishes a Furre
                    '(0:62) When the bot sucessfully temp banishes the furre named {...}
                    MSpage.RemoveVariable("BANISHNAME")
                    MSpage.SetVariable("BANISHNAME", BanishName, True)
                    MSpage.ExecuteAsync(61, 62)

                ElseIf Text.StartsWith("Players banished from your dreams: ") Then
                    'Banish-List
                    '[notify> Players banished from your dreams:
                    '`(0:54) When the bot sees the banish list
                    MSpage.ExecuteAsync(54)

                ElseIf Text.StartsWith("The banishment of player ") Then
                    'banish-off <name> (on list)
                    '[notify> The banishment of player (.*?) has ended.

                    '(0:56) When the bot successfully removes a furre from the banish list,
                    '(0:58) When the bot successfully removes the furre named {...} from the banish list,
                    Dim t As New Regex("The banishment of player (.*?) has ended.", RegexOptions.Compiled)
                    NameStr = t.Match(Text).Groups(1).Value
                    MSpage.RemoveVariable("BANISHNAME")
                    MSpage.SetVariable("BANISHNAME", NameStr, True)
                    MSpage.ExecuteAsync(56, 56)

                    '      MSpage.ExecuteAsync(800)

                ElseIf Text.Contains("There are no furres around right now with a name starting with ") Then
                    'Banish <name> (Not online)
                    'Error:>>  There are no furres around right now with a name starting with (.*?) .

                    '(0:50) When the Bot fails to banish a furre,
                    '(0:51) When the bot fails to banish the furre named {...},
                    Dim t As New Regex("There are no furres around right now with a name starting with (.*?) .", RegexOptions.Compiled)
                    NameStr = t.Match(Text).Groups(1).Value
                    MSpage.RemoveVariable("BANISHNAME")
                    MSpage.SetVariable("BANISHNAME", NameStr, True)
                    MSpage.ExecuteAsync(50, 51)
                ElseIf Text = "Sorry, this player has not been banished from your dreams." Then
                    'banish-off <name> (not on list)
                    'Error:>> Sorry, this player has not been banished from your dreams.

                    '(0:55) When the Bot fails to remove a furre from the banish list,
                    '(0:56) When the bot fails to remove the furre named {...} from the banish list,
                    MSpage.RemoveVariable("BANISHNAME")
                    MSpage.SetVariable("BANISHNAME", BanishName, True)
                    MSpage.ExecuteAsync(50, 51)
                ElseIf Text = "You have not banished anyone." Then
                    'banish-off-all (empty List)
                    'Error:>> You have not banished anyone.

                    '(0:59) When the bot fails to see the banish list,
                    MSpage.RemoveVariable("BANISHLIST")
                    MSpage.SetVariable("BANISHLIST", "", True)
                    MSpage.ExecuteAsync(59)

                ElseIf Text = "You do not have any cookies to give away right now!" Then
                    MSpage.ExecuteAsync(95)
                End If

            Case "@cookie"
                ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>
                ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> All-time Cookie total: 0</font>
                ' <font color='success'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Your cookies are ready.  http://furcadia.com/cookies/ for more info!</font>
                '<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.

                Dim CookieToMe As Regex = New Regex(String.Format("{0}", CookieToMeREGEX))
                If CookieToMe.Match(Text).Success Then

                    MSpage.ExecuteAsync(42, 43)
                End If
                Dim CookieToAnyone As Regex = New Regex(String.Format("<name shortname='(.*?)'>(.*?)</name> just gave <name shortname='(.*?)'>(.*?)</name> a (.*?)"))
                If CookieToAnyone.Match(Text).Success Then

                    If IsConnectedCharacter Then
                        MSpage.ExecuteAsync(42, 43)
                    Else
                        MSpage.ExecuteAsync(44)
                    End If

                End If
                Dim CookieFail As Regex = New Regex(String.Format("You do not have any (.*?) left!"))
                If CookieFail.Match(Text).Success Then
                    MSpage.ExecuteAsync(45)
                End If
                Dim EatCookie As Regex = New Regex(Regex.Escape("<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.") + "(.*?)")
                If EatCookie.Match(Text).Success Then
                    'TODO Cookie eat message can change by Dragon Speak

                    MSpage.ExecuteAsync(49)

                End If
                '(0:96) When the Bot sees "Your cookies are ready."
                Dim CookiesReady As Regex = New Regex(<a>"Your cookies are ready.  http://furcadia.com/cookies/ for more info!"</a>)
                If CookiesReady.Match(Text).Success Then
                    MSpage.ExecuteAsync(96)
                End If
            Case Else
                'TODO: plugin Dynamic(Group)  Channels here

        End Select

    End Sub

    ''' <summary>
    ''' Set a list of Variables for the <see cref="Monkeyspeak.Page"/>
    ''' </summary>
    ''' <param name="VariableList"></param>
    Public Sub PageSetVariable(ByVal VariableList As Dictionary(Of String, Object))

        For Each kv As KeyValuePair(Of String, Object) In VariableList
            MSpage.SetVariable(kv.Key.ToUpper, kv.Value, True)
        Next '

    End Sub

    ''' <summary>
    ''' Send a formatted string to the client and log window
    ''' </summary>
    ''' <param name="msg">
    ''' Channel Subsystem?
    ''' </param>
    ''' <param name="data">
    ''' Message to send
    ''' </param>
    Public Sub SendToClientFormattedText(msg As String, data As String)
        SendToClient("(" + "<b><i>[SM]</i> - " + msg + ":</b> """ + data + """")
        'Writer.WriteLine("<b><i>[SM]</i> - " + msg + ":</b> """ + data + """")

    End Sub

    ''' <summary>
    ''' Start the Monkey Speak Engine
    ''' </summary>
    Public Sub StartEngine()
        MainEngine = New MainEngine(MainEngineOptions.MonkeySpeakEngineOptions, Me)
        MSpage = MainEngine.LoadFromScriptFile(MainEngineOptions.MonkeySpeakEngineOptions.MonkeySpeakScriptFile)

        Dim TimeStart = DateTime.Now
        Dim VariableList As New Dictionary(Of String, Object)

        LoadLibrary(False, False)

        VariableList.Add("DREAMOWNER", Nothing)
        VariableList.Add("DREAMNAME", Nothing)
        VariableList.Add("BOTNAME", Nothing)
        VariableList.Add("BOTCONTROLLER", MainEngineOptions.BotController)
        VariableList.Add(MS_Name, Nothing)
        VariableList.Add("MESSAGE", Nothing)
        VariableList.Add("BANISHNAME", Nothing)
        VariableList.Add("BANISHLIST", Nothing)
        PageSetVariable(VariableList)
        '(0:0) When the bot starts,
        MSpage.ExecuteAsync(0)
        Console.WriteLine(String.Format("Done!!! Executed {0} triggers in {1} seconds.",
                                            MSpage.Size, Date.Now.Subtract(TimeStart).Seconds))

    End Sub

    Public Sub StopEngine()
        '  RemoveHandler ProcessServerChannelData, Me

        If Not MSpage Is Nothing Then
            MSpage.Reset(True)
            MSpage.Dispose()
            MSpage = Nothing
        End If
        If Not MainEngine Is Nothing Then
            MainEngine.Dispose()
            MainEngine = Nothing
        End If

    End Sub

    ''' <summary>
    ''' Dispose components
    ''' </summary>
    ''' <param name="disposing"></param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If Me.disposed Then Exit Sub
        If disposing Then

            If Not MainEngine Is Nothing Then MainEngine.Dispose()
            ' Free your own state (unmanaged objects).
            ' Set large fields to null.
            ' MyBase.Dispose()
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

    Private Sub InitializeEngineLibraries()
        ' Comment out Libs to Disable

        LibList = New List(Of Monkeyspeak.Libraries.BaseLibrary) From {
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
                New MsFurreList(Me),
                New MsWarning(Me),
                New MsMovement(Me),
                New WmCpyDta(Me),
                New MsMemberList(Me),
                New MsPounce(Me),
                New MsVerbot(Me),
                New MsSound(Me),
                New MsTrades(Me),
                New MsDreamInfo(Me)
            }
        ' New MathLibrary(Me),
        'New MsIO(Me),
        'LibList.Add(New MS_MemberList())
    End Sub

    ''' <summary>
    ''' Pump MonkeySpeak Exceptions to the error handler
    ''' </summary>
    ''' <param name="handler"></param>
    ''' <param name="Trigger"></param>
    ''' <param name="ex"></param>
    Private Sub OnMonkeySpeakError(handler As TriggerHandler, Trigger As Trigger, ex As Exception) Handles MSpage.Error
        If ex.GetType IsNot GetType(MonkeySpeakException) Then
            Dim PageError As New MonkeySpeakException(String.Format("Trigger Error: {0}", Trigger.ToString), ex)
            SendError(PageError, handler, Trigger.ToString)
        Else
            SendError(ex, handler, Trigger.ToString)
        End If

    End Sub


End Class