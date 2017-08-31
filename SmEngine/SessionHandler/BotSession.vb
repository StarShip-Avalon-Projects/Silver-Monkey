'Furcadia Servver Parser
'Event/Delegates for server instructions
'call subsystem processor

'dream info
'Furre info
'Bot info

'Furre Update events?

Imports System.Text
Imports System.Text.RegularExpressions
Imports Furcadia.Drawing
Imports Furcadia.Drawing.VisibleArea
Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Furcadia.Net.Proxy
Imports Furcadia.Text.FurcadiaMarkup
Imports Furcadia.Util
Imports MonkeyCore
Imports Monkeyspeak
Imports SilverMonkeyEngine.Engine
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

#Region "Public Methods"
    ''' <summary>
    ''' Handle MonkeySpeak Page errors
    ''' </summary>
    ''' <param name="trigger"></param>
    ''' <param name="ex"></param>
    Public Sub MOnMsPageError(trigger As Trigger, ex As MonkeyspeakException) Handles MSpage.Error
        Dim ErrorString As String = "Error: " + trigger.ToString + ex.Message
        RaiseEvent DisplayError("Error, See Debug Window", EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' Handle Furcadia NetProxy Errors
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub onProxyError(sender As Object, e As EventArgs) Handles MyBase.OnError
        RaiseEvent DisplayError(sender, e)
    End Sub

#End Region

#Region "Constructors"

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

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Is the Current executing Furre the Bot Controller?
    ''' </summary>
    ''' <returns>
    ''' True on sucess
    ''' </returns>
    Public ReadOnly Property IsBotController As Boolean
        Get
            Return Player.ShortName = MainEngineOptions.BotControllerShortName
        End Get
    End Property

    Public ReadOnly Property BotController As String
        Get
            Return MainEngineOptions.BotController
        End Get
    End Property

#End Region

#Region "Public Events"
    ''' <summary>
    ''' Error Message handler
    ''' </summary>
    ''' <param name="DisplayText"></param>
    ''' <param name="e"></param>
    Public Event DisplayError(ByVal DisplayText As Object, ByVal e As EventArgs)

#End Region

#Region "Private Fields"

    Private MainSettings As Settings.cMain

#End Region

#Region "Public Fields"

    ''' <summary>
    ''' Main MonkeySpeak Engine
    ''' </summary>
    Public WithEvents MainEngine As Engine.MainEngine

    Public WithEvents MSpage As Monkeyspeak.Page = Nothing

    'Monkey Speak Bot specific Variables

    Public objHost As New smHost(Me)
    Private MainEngineOptions As BotOptions

#End Region



#Region "Public Methods"

    ''' <summary>
    ''' Starts the Furcadia Connection Process
    ''' </summary>
    Public Overrides Sub Connect()

        Try


            MainEngine = New MainEngine(MainEngineOptions.MonkeySpeakEngineOptions, Me)
            MSpage = MainEngine.LoadFromScriptFile(MainEngineOptions.MonkeySpeakEngineOptions.MonkeySpeakScriptFile)

            Dim page = New MonkeySpeakPage(MainEngine, MSpage)
            MSpage = page.Start()

            MyBase.Connect()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Disconnect from the Game Server and Client
    ''' </summary>
    Public Overrides Sub Disconnect()
        MyBase.Disconnect()
        ' (0:2) When the bot logs off
        MSpage.Execute(2)

        MainEngine.MS_Engine_Running = False
    End Sub



    Public Overrides Sub ParseServerChannel(data As String, Handled As Boolean)
        'Pass Stuff to Base Clqss before we can handle things here
        MyBase.ParseServerChannel(data, Handled)

        Dim psCheck As Boolean = False
        Dim SpecTag As String = ""
        ' Channel = Regex.Match(data, ChannelNameFilter).Groups(1).Value
        Dim Color As String = Regex.Match(data, EntryFilter).Groups(1).Value
        Dim User As String = ""
        Dim Desc As String = ""
        Dim Text As String = ""

        MSpage.SetVariable(MS_Name, Player.Name, True)

        If Color = "success" Then
            Try
                If Text.Contains(" has been banished from your dreams.") Then
                    'banish <name> (online)
                    'Success: (.*?) has been banished from your dreams.

                    '(0:52) When the bot sucessfilly banishes a furre,
                    '(0:53) When the bot sucessfilly banishes the furre named {...},

                    MSpage.SetVariable("BANISHNAME", BanishName, True)
                    MSpage.SetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray), True)
                    MSpage.Execute(52, 53)

                    ' MSPage.Execute(53)
                ElseIf Text = "You have canceled all banishments from your dreams." Then
                    'banish-off-all (active list)
                    'Success: You have canceled all banishments from your dreams.

                    MSpage.SetVariable("BANISHLIST", Nothing, True)
                    MSpage.SetVariable("BANISHNAME", Nothing, True)
                    MSpage.Execute(60)

                ElseIf Text.EndsWith(" has been temporarily banished from your dreams.") Then
                    'tempbanish <name> (online)
                    'Success: (.*?) has been temporarily banished from your dreams.

                    '(0:61) When the bot sucessfully temp banishes a Furre
                    '(0:62) When the bot sucessfully temp banishes the furre named {...}

                    MSpage.SetVariable("BANISHNAME", BanishName, True)
                    ' MSPage.Execute(61)
                    MSpage.SetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray()), True)
                    MSpage.Execute(61, 62)

                ElseIf Text.StartsWith("The endurance limits of player ") Then
                    Dim t As New Regex("The endurance limits of player (.*?) are now toggled off.")
                    Dim m As String = t.Match(Text).Groups(1).Value.ToString
                    If FurcadiaShortName(m) = FurcadiaShortName(ConnectedCharacterName) Then
                        ' NoEndurance = True
                    End If

                ElseIf Channel = "@cookie" Then
                    '(0:96) When the Bot sees "Your cookies are ready."
                    Dim CookiesReady As Regex = New Regex(<a>"Your cookies are ready.  http://furcadia.com/cookies/ for more info!"</a>)
                    If CookiesReady.Match(data).Success Then
                        MSpage.Execute(96)
                    End If
                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
        ElseIf Channel = "@roll" Then
            'Dim DiceREGEX As New Regex(DiceFilter, RegexOptions.IgnoreCase)
            'Dim DiceMatch As System.Text.RegularExpressions.Match = DiceREGEX.Match(data)

            ''Matches, in order:
            ''1:      shortname()
            ''2:      full(name)
            ''3:      dice(count)
            ''4:      sides()
            ''5: +/-#
            ''6: +/-  (component match)
            ''7:      additional(Message)
            ''8:      Final(result)

            'Player = Dream.FurreList.GerFurreByName(DiceMatch.Groups(3).Value)
            'Player.Message = DiceMatch.Groups(7).Value
            'MSPage.SetVariable("MESSAGE", DiceMatch.Groups(7).Value)
            'Double.TryParse(DiceMatch.Groups(4).Value, DiceSides)
            'Double.TryParse(DiceMatch.Groups(3).Value, DiceCount)
            'DiceCompnentMatch = DiceMatch.Groups(6).Value
            'DiceModifyer = 0.0R
            'Double.TryParse(DiceMatch.Groups(5).Value, DiceModifyer)
            'Double.TryParse(DiceMatch.Groups(8).Value, DiceResult)

            'If IsConnectedCharacter Then
            '    MSPage.Execute(130, 131, 132, 136)
            'Else
            '    MSPage.Execute(133, 134, 135, 136)
            'End If

        ElseIf Channel = "@dragonspeak" OrElse Channel = "@emit" OrElse Color = "emit" Then
            Try
                '(<font color='dragonspeak'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Furcadian Academy</font>

                MSpage.SetVariable("MESSAGE", Player.Message, True)
                ' Execute (0:21) When someone emits something
                MSpage.Execute(21, 22, 23)
                ' Execute (0:22) When someone emits {...}
                '' Execute (0:23) When someone emits something with {...} in it
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

            ''BCast (Advertisments, Announcments)
        ElseIf Color = "bcast" Then
            Dim AdRegEx As String = "<channel name='(.*)' />"

            Dim chan As String = Regex.Match(data, AdRegEx).Groups(1).Value
            Try

                Select Case chan
                    Case "@advertisements"
                        ' If cMain.Advertisment Then Exit Sub
                        AdRegEx = "\[(.*?)\] (.*?)</font>"
                        Dim adMessage As String = Regex.Match(data, AdRegEx).Groups(2).Value

                    Case "@announcements"
                        ' If cMain.Announcement Then Exit Sub
                        Dim u As String = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups(2).Value

                    Case Else
                End Select
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

            ''SAY
        ElseIf Color = "myspeech" Then
            Try

                MSpage.SetVariable("MESSAGE", Player.Message, True)
                ' Execute (0:5) When some one says something
                MSpage.Execute(5, 6, 7, 18, 19, 20)
                '' Execute (0:6) When some one says {...} Execute (0:7) When
                '' some one says something with {...} in it Execute (0:18)
                '' When someone says or emotes something Execute (0:19) When
                '' someone says or emotes {...} Execute (0:20) When someone
                '' says or emotes something with {...} in it
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

        ElseIf Channel = "say" Then
            Dim tt As System.Text.RegularExpressions.Match = Regex.Match(data, "\(you see(.*?)\)", RegexOptions.IgnoreCase)
            If Not tt.Success Then

                Try
                    MSpage.SetVariable("MESSAGE", Player.Message, True)
                    ' (0:5) When some one says something
                    MSpage.Execute(5, 6, 7, 18, 19, 20)
                    ' (0:6) When some one says {...}
                    '(0:7) When some one says something with {...} in it
                    ' (0:18) When someone says or emotes something
                    ' (0:19) When someone says or emotes {...}
                    ' (0:20) When someone says or emotes something with
                    ' {...} in it
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)

                End Try
            Else
                Try
                    'sndDisplay("You See '" & User & "'")
                    'Look = True
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
            End If

        ElseIf Desc <> "" Then
            Try
                Dim DescName As String = Regex.Match(data, DescFilter).Groups(1).ToString()

                MSpage.SetVariable(MS_Name, DescName, True)
                MSpage.Execute(600)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

        ElseIf Color = "shout" Then
            ''SHOUT
            Try
                Dim t As New Regex(YouSayFilter)
                Dim u As String = t.Match(data).Groups(1).ToString
                Text = t.Match(data).Groups(2).ToString
                If User = "" Then
                Else
                    Text = Regex.Match(data, "shouts: (.*)</font>").Groups(1).ToString()
                End If
                If Not IsConnectedCharacter Then
                    MSpage.SetVariable("MESSAGE", Text, True)
                    Player.Message = Text
                    ' Execute (0:8) When some one shouts something
                    MSpage.Execute(8, 9, 10)
                    ' Execute (0:9) When some one shouts {...} Execute
                    ' (0:10) When some one shouts something with {...} in it

                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

        ElseIf Color = "query" Then
            Dim QCMD As String = Regex.Match(data, "<a.*?href='command://(.*?)'>").Groups(1).ToString

            Select Case QCMD
                Case "summon"
                    ''JOIN
                    Try
                        If Not IsConnectedCharacter Then
                            MSpage.Execute(34, 35)
                        End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "join"
                    ''SUMMON
                    Try

                        If Not IsConnectedCharacter Then
                            MSpage.Execute(32, 33)
                        End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "follow"
                    ''LEAD
                    Try
                        If Not IsConnectedCharacter Then
                            MSpage.Execute(36, 37)
                        End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "lead"
                    ''FOLLOW
                    Try
                        If Not IsConnectedCharacter Then
                            MSpage.Execute(38, 39)
                        End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "cuddle"
                    Try

                        If Not IsConnectedCharacter Then
                            MSpage.Execute(40, 41)
                        End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
            End Select

            'NameFilter

        ElseIf Color = "whisper" Then
            ''WHISPER
            Try
                'TODO Refactor to FurcadiaMarkup
                Dim WhisperFrom As String = Regex.Match(data, "whispers, ""(.*?)"" to you").Groups(1).Value
                Dim WhisperTo As String = Regex.Match(data, "You whisper ""(.*?)"" to").Groups(1).Value
                Dim WhisperDir As String = Regex.Match(data, String.Format("<name shortname='(.*?)' src='whisper-(.*?)'>")).Groups(2).Value
                If WhisperDir = "from" Then

                    If Not IsConnectedCharacter Then
                        MSpage.SetVariable("NAME", Player.Name, True)
                        MSpage.SetVariable("MESSAGE", Player.Message, True)
                        ' Execute (0:15) When some one whispers something
                        MSpage.Execute(15, 16, 17)
                        ' Execute (0:16) When some one whispers {...}
                        ' Execute (0:17) When some one whispers something
                        ' with {...} in it
                    End If
                Else
                    WhisperTo = WhisperTo.Replace("<wnd>", "")

                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

        ElseIf Color = "warning" Then

            MSpage.Execute(801)

        ElseIf Color = "trade" Then
            Dim TextStr As String = Regex.Match(data, "\s<name (.*?)</name>").Groups(0).ToString()
            Text = Text.Substring(6)
            If User <> "" Then Text = " " & User & Text.Replace(TextStr, "")

            MSpage.SetVariable("MESSAGE", Player.Message, True)
            MSpage.Execute(46, 47, 48)

        ElseIf Color = "emote" Then
            Try

                MSpage.SetVariable("MESSAGE", Player.Message, True)

                If Not IsConnectedCharacter Then

                    ' Execute (0:11) When someone emotes something
                    MSpage.Execute(11, 12, 13, 18, 19, 20)
                    ' Execute (0:12) When someone emotes {...} Execute
                    ' (0:13) When someone emotes something with {...} in it
                    ' Execute (0:18) When someone says or emotes something
                    ' Execute (0:19) When someone says or emotes {...}
                    ' Execute (0:20) When someone says or emotes something
                    ' with {...} in it
                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

        ElseIf Color = "channel" Then
            'ChannelNameFilter2
            Dim chan As Regex = New Regex(ChannelNameFilter)
            Dim ChanMatch As System.Text.RegularExpressions.Match = chan.Match(data)
            Dim r As New Regex("<img src='(.*?)' alt='(.*?)' />")
            Dim ss As RegularExpressions.Match = r.Match(Text)
            If ss.Success Then Text = Text.Replace(ss.Groups(0).Value, "")
            r = New Regex(NameFilter + ":")
            ss = r.Match(Text)
            If ss.Success Then Text = Text.Replace(ss.Groups(0).Value, "")

        ElseIf Color = "notify" Then
            Dim NameStr As String = ""
            If Text.StartsWith("Players banished from your dreams: ") Then
                'Banish-List
                '[notify> Players banished from your dreams:
                '`(0:54) When the bot sees the banish list
                MSpage.SetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray), True)
                MSpage.Execute(54)

            ElseIf Text.StartsWith("The banishment of player ") Then
                'banish-off <name> (on list)
                '[notify> The banishment of player (.*?) has ended.

                '(0:56) When the bot successfully removes a furre from the banish list,
                '(0:58) When the bot successfully removes the furre named {...} from the banish list,
                Dim t As New Regex("The banishment of player (.*?) has ended.")
                NameStr = t.Match(data).Groups(1).Value
                MSpage.SetVariable("BANISHNAME", NameStr, True)
                MSpage.Execute(56, 56)
                MSpage.SetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray), True)

            End If

            MSpage.Execute(800)

            If Text.Contains("There are no furres around right now with a name starting with ") Then
                'Banish <name> (Not online)
                'Error:>>  There are no furres around right now with a name starting with (.*?) .

                '(0:50) When the Bot fails to banish a furre,
                '(0:51) When the bot fails to banish the furre named {...},
                Dim t As New Regex("There are no furres around right now with a name starting with (.*?) .")
                NameStr = t.Match(data).Groups(1).Value
                MSpage.SetVariable("BANISHNAME", NameStr, True)
                MSpage.Execute(50, 51)
                MSpage.SetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray), True)
            ElseIf Text = "Sorry, this player has not been banished from your dreams." Then
                'banish-off <name> (not on list)
                'Error:>> Sorry, this player has not been banished from your dreams.

                '(0:55) When the Bot fails to remove a furre from the banish list,
                '(0:56) When the bot fails to remove the furre named {...} from the banish list,
                MSpage.SetVariable("BANISHNAME", BanishName, True)
                MSpage.SetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray), True)
                MSpage.Execute(50, 51)
            ElseIf Text = "You have not banished anyone." Then
                'banish-off-all (empty List)
                'Error:>> You have not banished anyone.

                '(0:59) When the bot fails to see the banish list,
                MSpage.Execute(59)
                MSpage.SetVariable("BANISHLIST", "", True)
            ElseIf Text = "You do not have any cookies to give away right now!" Then
                MSpage.Execute(95)
            End If

        ElseIf data.StartsWith("Communication") Then
            ' ProcExit = False
            'LogSaveTmr.Enabled = False

        ElseIf Channel = "@cookie" Then
            ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>
            ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> All-time Cookie total: 0</font>
            ' <font color='success'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Your cookies are ready.  http://furcadia.com/cookies/ for more info!</font>
            '<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.

            Dim CookieToMe As Regex = New Regex(String.Format("{0}", CookieToMeREGEX))
            If CookieToMe.Match(data).Success Then
                MSpage.SetVariable(MS_Name, CookieToMe.Match(data).Groups(2).Value, True)
                MSpage.Execute(42, 43)
            End If
            Dim CookieToAnyone As Regex = New Regex(String.Format("<name shortname='(.*?)'>(.*?)</name> just gave <name shortname='(.*?)'>(.*?)</name> a (.*?)"))
            If CookieToAnyone.Match(data).Success Then
                MSpage.SetVariable(MS_Name, CookieToAnyone.Match(data).Groups(3).Value, True)

                If IsConnectedCharacter Then
                    MSpage.Execute(42, 43)
                Else
                    MSpage.Execute(44)
                End If

            End If
            Dim CookieFail As Regex = New Regex(String.Format("You do not have any (.*?) left!"))
            If CookieFail.Match(data).Success Then
                MSpage.Execute(45)
            End If
            Dim EatCookie As Regex = New Regex(Regex.Escape("<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.") + "(.*?)")
            If EatCookie.Match(data).Success Then
                'TODO Cookie eat message can change by Dragon Speak
                MSpage.SetVariable("MESSAGE", "You eat a cookie." + EatCookie.Replace(data, ""), True)
                Player.Message = "You eat a cookie." + EatCookie.Replace(data, "")
                MSpage.Execute(49)

            End If

        ElseIf data.StartsWith("(You enter the dream of") Then
            MSpage.SetVariable("DREAMNAME", "", True)
            MSpage.SetVariable("DREAMOWNER", data.Substring(24, data.Length - 2 - 24), True)
            MSpage.Execute(90, 91)

        End If

    End Sub

    ''' <summary>
    ''' Parse Server Data
    ''' <para>
    ''' TODO: Move this functionality to <see cref="Furcadia.Net"/>
    ''' </para>
    ''' </summary>
    ''' <param name="data">
    ''' raw server instruction
    ''' </param>
    ''' <param name="Handled">
    ''' has this instruction been handled elsewhere
    ''' </param>
    Public Overrides Sub ParseServerData(ByVal data As String, ByVal Handled As Boolean)
        MyBase.ParseServerData(data, Handled)
        MSpage.SetVariable(MS_Name, Player.ShortName, True)
        MSpage.SetVariable("MESSAGE", Player.Message, True)

        If data = "&&&&&&&&&&&&&" Then
            'We've connected to Furcadia
            'Stop the reconnection manager
            '(0:1) When the bot logs into furcadia,
            MSpage.SetVariable("BOTNAME", ConnectedFurre.ShortName, True)
            MSpage.Execute(1)

            ' Species Tags
        ElseIf data.StartsWith("]-") Then

            'DS Variables

            'Popup Dialogs!
        ElseIf data.StartsWith("]#") Then
            ']#<idstring> <style 0-17> <message that might have spaces in>
            Dim repqq As Regex = New Regex("^\]#(.*?) (\d+) (.*?)$")
            Dim m As Match = repqq.Match(data)
            Dim r As Rep
            r.ID = m.Groups(1).Value
            Dim num As Integer = 0
            Integer.TryParse(m.Groups(2).Value, r.Type)
            Repq.Enqueue(r)
            MSpage.SetVariable("MESSAGE", Player.Message, True)
            MSpage.Execute(95, 96)

            ']s(.+)1 (.*?) (.*?) 0
            'Display Dream Info
            'Portal  Names until a ]t
        ElseIf data.StartsWith("]s") Then
            Dim t As New Regex("\]s(.+)1 (.*?) (.*?) 0", RegexOptions.IgnoreCase)
            Dim m As System.Text.RegularExpressions.Match = t.Match(data)
            If Furcadia.Util.FurcadiaShortName(ConnectedCharacterName) = Furcadia.Util.FurcadiaShortName(m.Groups(2).Value) Then
                MSpage.Execute()
            End If

            'Look response
        ElseIf data.StartsWith("]f") And ServerConnectPhase = ConnectionPhase.Connected And InDream = True Then

            'Spawn Avatar
        ElseIf data.StartsWith("<") And ServerConnectPhase = ConnectionPhase.Connected Then

            If IsConnectedCharacter Then
                MSpage.Execute(28, 29, 24, 25)
            Else
                MSpage.Execute(24, 25)
            End If

            'Try

            ' ' Add New Arrivals to Dream.FurreList ' One or the other will
            ' trigger it ' IsConnectedCharacter

            ' If Player.Flag = 4 Or Not Dream.FurreList.Contains(Player)
            ' Then Dream.FurreList.Add(Player) ' If InDream Then RaiseEvent
            ' UpDateDreamList(Player.Name) If Player.Flag = 2 Then End If
            ' ElseIf Player.Flag = 2 Then

            'MSpage.Execute(28, 29)

            ' ElseIf Player.Flag = 1 Then

            ' ElseIf Player.Flag = 0 Then

            ' End If

            'Catch eX As Exception

            '    Dim logError As New ErrorLogging(eX, Me)
            '    Exit Sub
            'End Try

            'Remove Furre
        ElseIf data.StartsWith(")") And ServerConnectPhase = ConnectionPhase.Connected Then  'And loggingIn = False

            'Animated Move
        ElseIf data.StartsWith("/") And ServerConnectPhase = ConnectionPhase.Connected Then 'And loggingIn = False
            Try

                MSpage.SetVariable(MS_Name, Player.ShortName, True)
                MSpage.Execute(28, 29, 30, 31, 601, 602)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

            ' Move Avatar
        ElseIf data.StartsWith("A") And ServerConnectPhase = ConnectionPhase.Connected Then 'And loggingIn = False
            Try

                Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(ConnectedFurre.Position.x, ConnectedFurre.Position.y)
                If VisableRectangle.X <= Me.Player.Position.x And VisableRectangle.Y <= Me.Player.Position.y And VisableRectangle.height >= Me.Player.Position.y And VisableRectangle.length >= Me.Player.Position.x Then

                    Player.Visible = True
                Else
                    Player.Visible = False
                End If

                MSpage.SetVariable(MS_Name, Player.ShortName, True)
                MSpage.Execute(28, 29, 30, 31, 601, 602)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

            ' Update Color Code
        ElseIf data.StartsWith("B") And ServerConnectPhase = ConnectionPhase.Connected And InDream Then

            'Hide Avatar
        ElseIf data.StartsWith("C") <> False And ServerConnectPhase = ConnectionPhase.Connected Then
            Try

                MSpage.SetVariable(MS_Name, Player.ShortName, True)
                MSpage.Execute(30, 31)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

            'Display Disconnection Dialog
        ElseIf data.StartsWith("[") Then
            ' RaiseEvent UpDateDreamList("")

            MsgBox(data, MsgBoxStyle.Critical, "Disconnection Error")

            ';{mapfile}	Load a local map (one in the furcadia folder)
            ']q {name} {id}	Request to download a specific patch
        ElseIf data.StartsWith(";") OrElse data.StartsWith("]q") OrElse data.StartsWith("]r") Then
            MSpage.SetVariable("DREAMOWNER", "", True)
            MSpage.SetVariable("DREAMNAME", "", True)
                'RaiseEvent UpDateDreamList("")


                ElseIf data.StartsWith("]z") Then
            '   ConnectedCharacterFurcadiaID = Integer.Parse(data.Remove(0, 2))
            'Snag out UID
        ElseIf data.StartsWith("]B") Then
            ' ConnectedCharacterFurcadiaID = Integer.Parse(data.Substring(2,
            ' data.Length - ConnectedCharacterName.Length - 3))

        ElseIf data.StartsWith("]c") Then

        ElseIf data.StartsWith("]C") Then
            If data.StartsWith("]C0") Then

                MSpage.SetVariable("DREAMOWNER", Dream.Owner, True)
                MSpage.SetVariable("DREAMNAME", Dream.Name, True)
                MSpage.Execute(90, 91)
            End If

            'Process Channels Separately
        ElseIf data.StartsWith("(") Then
            If ThroatTired = True And data.StartsWith("(<font color='warning'>Your throat is tired. Try again in a few seconds.</font>") Then

                '(0:92) When the bot detects the "Your throat is tired. Please wait a few seconds" message,
                MSpage.Execute(92)

            End If
        Else

        End If

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

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' </summary>
    ''' <param name="Server_Instruction">
    ''' </param>
    ''' <returns>
    ''' </returns>
    Private Function MessagePump(ByRef Server_Instruction As String) As Boolean
        Dim objPlugin As Interfaces.msPlugin
        Dim intIndex As Integer
        Dim Handled As Boolean = False
        If Not Settings.Plugins Is Nothing Then
            For intIndex = 0 To Settings.Plugins.Count - 1
                objPlugin = DirectCast(PluginServices.CreateInstance(Settings.Plugins(intIndex)), Interfaces.msPlugin)
                If Settings.PluginList.Item(objPlugin.Name.Replace(" ", "")) Then
                    objPlugin.Initialize(objHost)
                    objPlugin.MsPage = MSpage
                    If objPlugin.MessagePump(Server_Instruction) Then Handled = True
                End If
            Next
        End If
        Return Handled
    End Function

#End Region

#Region "Dispose"

    Private disposed As Boolean

    ' IDisposable
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposed Then

            MainEngine.Dispose()

            ' Free your own state (unmanaged objects).
            ' Set large fields to null.
        End If
        Me.disposed = True
    End Sub

#Region " IDisposable Support "

    Public Overrides Sub Dispose()
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code. 
        ' Put cleanup code in
        ' Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub
#End Region



#End Region

End Class