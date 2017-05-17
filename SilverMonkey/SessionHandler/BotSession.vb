'Furcadia Servver Parser
'Event/Delegates for server instructions
'call subsystem processor

'dream info
'Furre info
'Bot info

'Furre Update events?

Imports System.Text.RegularExpressions
Imports Furcadia.Drawing
Imports Furcadia.Drawing.VisibleArea
Imports Furcadia.Net
Imports Furcadia.Net.Proxy
Imports Furcadia.Util
Imports MonkeyCore
Imports MonkeyCore.Controls
Imports MonkeyCore.Utils
Imports Monkeyspeak
Imports SilverMonkey.Engine
Imports SilverMonkey.Engine.Libraries.PhoenixSpeak

Imports Furcadia.Text.FurcadiaMarkup
Imports System.Text

''' <summary>
''' This Instance handles the current Furcadia Session.
''' <para>Part1: Manage MonkeySpeak Engine Start,Stop,Restart. System Variables, MonkeySpeak Execution Triggers</para>
''' <para>Part2: Furcadia Proxy Controls, In/Out Ports, Host, Character Ini file. Connect, Disconnect, Reconnect</para>
''' <para>Part2a: Proxy Functions do link to Monkey Speak trigger execution</para>
''' <para>Part3: This Class Links loosley to the GUI </para>
'''
''' </summary>
Public Class BotSession : Inherits ProxySession
    Implements IDisposable

#Region "Public Events"

    Public Event DisplayText(ByVal DisplayText As String, ByVal e As EventArgs)

#End Region

#Region "Private Fields"

    Private Writer As TextBoxWriter

#End Region

#Region "Public Fields"
    ''' <summary>
    ''' Main MonkeySpeak Engine
    ''' </summary>
    Public WithEvents MainEngine As Engine.MainMsEngine
    Public WithEvents MSpage As Page = Nothing
    Public objHost As New smHost(Me)
    Private MainEngineOptions As BotOptions

    Public Delegate Function GetMsPage(ByRef _msPage As Page) As Page
#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Starts the Furcadia Connection Process
    ''' </summary>
    Public Overrides Sub Connect()

        MainEngine = New Engine.MainMsEngine(MainEngineOptions.MonkeySpeakEngineOptions)
        MSpage = MainEngine.LoadFromScriptFile(MainEngineOptions.MonkeySpeakEngineOptions.MonkeySpeakScriptFile)
        MainEngine.Start()
        MyBase.Connect()

    End Sub

    ''' <summary>
    ''' Disconnect from the Game Server and Client
    ''' </summary>
    Public Overrides Sub Disconnect()
        MyBase.Disconnect()
        ' (0:2) When the bot logs off
        MainEngine.PageExecute(2)

        MainEngine.MS_Engine_Running = False
    End Sub

    ' Public implementation of Dispose pattern callable by consumers.
    Public Overloads Sub Dispose() _
              Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#Region "Private Methods"

#End Region

#Region "Public Fields"

    Public WithEvents SubSys As New SubSystem()

    'Monkey Speak Bot specific Variables
    Public BotLogStream As LogStream

    'Silver Monkey Specific Feature
#End Region

#Region "Private Fields"

    Private MainSettings As Settings.cMain

#End Region

#Region "Constructors"

    ''' <summary>
    '''
    ''' </summary>
    Sub New(ByRef Write As RichTextBoxEx)
        MyBase.New()
        Writer = New TextBoxWriter(Write)
        MainEngineOptions = New BotOptions()
    End Sub

    ''' <summary>
    ''' New BotSession with Proxy Settings
    ''' </summary>
    ''' <param name="BotSessionOptions"></param>
    Sub New(ByRef Write As RichTextBoxEx, ByRef BotSessionOptions As BotOptions)
        MyBase.New(BotSessionOptions)
        Writer = New TextBoxWriter(Write)
        MainEngineOptions = BotSessionOptions
    End Sub

#End Region

#Region "Private Methods"

#End Region

    ''' <summary>
    ''' Parse Channel Data
    ''' </summary>
    ''' <param name="data">Raw Game Server to Client instruction</param>
    ''' <param name="Handled">Is this data already handled?</param>
    Public Overloads Sub ParseServerChannel(ByRef data As String, ByVal Handled As Boolean)

        'Pass Stuff to Base Clqss before we can handle things here
        MyBase.ParseServerChannel(data, Handled)

        'Strip the trigger Character
        ' page = engine.LoadFromString(cBot.MS_Script)
        data = data.Remove(0, 1)
        Dim psCheck As Boolean = False
        Dim SpecTag As String = ""
        Channel = Regex.Match(data, ChannelNameFilter).Groups(1).Value
        Dim Color As String = Regex.Match(data, EntryFilter).Groups(1).Value
        Dim User As String = ""
        Dim Desc As String = ""
        Dim Text As String = ""
        If Not Handled Then
            Text = Regex.Match(data, EntryFilter).Groups(2).Value
            User = Regex.Match(data, NameFilter).Groups(3).Value
            If User <> "" Then Player = Dream.FurreList.GerFurreByName(User)
            Player.Message = ""
            Desc = Regex.Match(data, DescFilter).Groups(2).Value
            Dim mm As New Regex(Iconfilter)
            Dim ds As System.Text.RegularExpressions.Match = mm.Match(Text)
            Text = mm.Replace(Text, "[" + ds.Groups(1).Value + "] ")
            Dim s As New Regex(ChannelNameFilter)
            Text = s.Replace(Text, "")
        Else
            User = Player.Name
            Text = Player.Message
        End If
        DiceSides = 0
        DiceCount = 0
        DiceCompnentMatch = ""
        DiceModifyer = 0
        DiceResult = 0

        If Channel = "@news" Or Channel = "@spice" Then
            Try
                RaiseEvent DisplayText(Text, EventArgs.Empty)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        ElseIf Color = "success" Then
            Try
                If Text.Contains(" has been banished from your dreams.") Then
                    'banish <name> (online)
                    'Success: (.*?) has been banished from your dreams.

                    '(0:52) When the bot sucessfilly banishes a furre,
                    '(0:53) When the bot sucessfilly banishes the furre named {...},
                    Dim t As New Regex("(.*?) has been banished from your dreams.")
                    BanishName = t.Match(Text).Groups(1).ToString
                    MainEngine.PageSetVariable("BANISHNAME", BanishName)

                    BanishString.Add(BanishName)
                    MainEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                    MainEngine.PageExecute(52, 53)

                    ' MainEngine.PageExecute(53)
                ElseIf Text = "You have canceled all banishments from your dreams." Then
                    'banish-off-all (active list)
                    'Success: You have canceled all banishments from your dreams.

                    '(0:60) When the bot successfully clears the banish list
                    BanishString.Clear()
                    MainEngine.PageSetVariable("BANISHLIST", "")
                    MainEngine.PageSetVariable("BANISHNAME", "")
                    MainEngine.PageExecute(60)

                ElseIf Text.EndsWith(" has been temporarily banished from your dreams.") Then
                    'tempbanish <name> (online)
                    'Success: (.*?) has been temporarily banished from your dreams.

                    '(0:61) When the bot sucessfully temp banishes a Furre
                    '(0:62) When the bot sucessfully temp banishes the furre named {...}
                    Dim t As New Regex("(.*?) has been temporarily banished from your dreams.")
                    BanishName = t.Match(Text).Groups(1).Value
                    MainEngine.PageSetVariable("BANISHNAME", BanishName)
                    '  MainEngine.PageExecute(61)
                    BanishString.Add(BanishName)
                    MainEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                    MainEngine.PageExecute(61, 62)

                ElseIf Text.StartsWith("The endurance limits of player ") Then
                    Dim t As New Regex("The endurance limits of player (.*?) are now toggled off.")
                    Dim m As String = t.Match(Text).Groups(1).Value.ToString
                    If FurcadiaShortName(m) = FurcadiaShortName(ConnectedCharacterName) Then
                        NoEndurance = True
                    End If

                ElseIf Channel = "@cookie" Then
                    '(0:96) When the Bot sees "Your cookies are ready."
                    Dim CookiesReady As Regex = New Regex(String.Format("{0}", "Your cookies are ready.  http://furcadia.com/cookies/ for more info!"))
                    If CookiesReady.Match(data).Success Then
                        MainEngine.PageExecute(96)
                    End If
                End If
                RaiseEvent DisplayText(Text, EventArgs.Empty)
                If IsClientConnected Then SendToClient("(" + data)
                Exit Sub

            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
        ElseIf Channel = "@roll" Then
            Dim DiceREGEX As New Regex(DiceFilter, RegexOptions.IgnoreCase)
            Dim DiceMatch As System.Text.RegularExpressions.Match = DiceREGEX.Match(data)

            'Matches, in order:
            '1:      shortname()
            '2:      full(name)
            '3:      dice(count)
            '4:      sides()
            '5: +/-#
            '6: +/-  (component match)
            '7:      additional(Message)
            '8:      Final(result)

            Player = NametoFurre(DiceMatch.Groups(3).Value, True)
            Player.Message = DiceMatch.Groups(7).Value
            MainEngine.PageSetVariable("MESSAGE", DiceMatch.Groups(7).Value)
            Double.TryParse(DiceMatch.Groups(4).Value, DiceSides)
            Double.TryParse(DiceMatch.Groups(3).Value, DiceCount)
            DiceCompnentMatch = DiceMatch.Groups(6).Value
            DiceModifyer = 0.0R
            Double.TryParse(DiceMatch.Groups(5).Value, DiceModifyer)
            Double.TryParse(DiceMatch.Groups(8).Value, DiceResult)

            If IsConnectedCharacter Then
                MainEngine.PageExecute(130, 131, 132, 136)
            Else
                MainEngine.PageExecute(133, 134, 135, 136)
            End If

            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        ElseIf Channel = "@dragonspeak" OrElse Channel = "@emit" OrElse Color = "emit" Then
            Try
                '(<font color='dragonspeak'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Furcadian Academy</font>
                RaiseEvent DisplayText(Text, EventArgs.Empty)

                MainEngine.PageSetVariable("MESSAGE", Text.Substring(5))
                ' Execute (0:21) When someone emits something
                MainEngine.PageExecute(21, 22, 23)
                ' Execute (0:22) When someone emits {...}
                '' Execute (0:23) When someone emits something with {...} in it

            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
            ''BCast (Advertisments, Announcments)
        ElseIf Color = "bcast" Then
            Dim AdRegEx As String = "<channel name='(.*)' />"

            Dim chan As String = Regex.Match(data, AdRegEx).Groups(1).Value
            Try

                Select Case chan
                    Case "@advertisements"
                        If cMain.Advertisment Then Exit Sub
                        AdRegEx = "\[(.*?)\] (.*?)</font>"
                        Dim adMessage As String = Regex.Match(data, AdRegEx).Groups(2).Value
                        RaiseEvent DisplayText(Text, EventArgs.Empty)
                    Case "@bcast"
                        If cMain.Broadcast Then Exit Sub
                        Dim u As String = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups(2).Value
                        RaiseEvent DisplayText(Text, EventArgs.Empty)
                    Case "@announcements"
                        If cMain.Announcement Then Exit Sub
                        Dim u As String = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups(2).Value
                        RaiseEvent DisplayText(Text, EventArgs.Empty)
                    Case Else
#If DEBUG Then
                        Console.WriteLine("Unknown ")
                        Console.WriteLine("BCAST:" & data)
#End If
                End Select

            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
            ''SAY
        ElseIf Color = "myspeech" Then
            Try
                Dim t As New Regex(YouSayFilter)
                Dim u As String = t.Match(data).Groups(1).ToString
                Text = t.Match(data).Groups(2).ToString
                If SpeciesTag.Count() > 0 Then
                    Player.Color = SpeciesTag.Dequeue()
                    If Dream.FurreList.Contains(Player.ID) Then Dream.FurreList.Item(Player) = Player
                End If

                RaiseEvent DisplayText(Text, EventArgs.Empty)
                Player.Message = Text
                MainEngine.PageSetVariable("MESSAGE", Text)
                ' Execute (0:5) When some one says something
                'MainEngine.PageExecute(5, 6, 7, 18, 19, 20)
                '' Execute (0:6) When some one says {...}
                '' Execute (0:7) When some one says something with {...} in it
                '' Execute (0:18) When someone says or emotes something
                '' Execute (0:19) When someone says or emotes {...}
                '' Execute (0:20) When someone says or emotes something with {...} in it
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        ElseIf User <> "" And Channel = "" And Color = "" And Regex.Match(data, NameFilter).Groups(2).Value <> "forced" Then
            Dim tt As System.Text.RegularExpressions.Match = Regex.Match(data, "\(you see(.*?)\)", RegexOptions.IgnoreCase)
            Dim t As New Regex(NameFilter)
            If Not tt.Success Then

                Try
                    Text = t.Replace(data, "")
                    Text = Text.Remove(0, 2)

                    If SpeciesTag.Count() > 0 Then
                        SpecTag = SpeciesTag.Peek
                        SpeciesTag.Clear()
                        Player.Color = SpecTag
                        If Dream.FurreList.ContainsKey(Player.ID) Then Dream.FurreList.Item(Player.ID) = Player
                    End If
                    Channel = "say"
                    RaiseEvent DisplayText(User & " says, """ & Text & """", EventArgs.Empty)
                    MainEngine.PageSetVariable(MS_Name, User)
                    MainEngine.PageSetVariable("MESSAGE", Text)
                    Player.Message = Text
                    ' Execute (0:5) When some one says something
                    MainEngine.PageExecute(5, 6, 7, 18, 19, 20)
                    ' Execute (0:6) When some one says {...}
                    ' Execute (0:7) When some one says something with {...} in it
                    ' Execute (0:18) When someone says or emotes something
                    ' Execute (0:19) When someone says or emotes {...}
                    ' Execute (0:20) When someone says or emotes something with {...} in it

                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)

                End Try

                If IsClientConnected Then SendToClient("(" + data)
                Exit Sub
            Else
                Try
                    'sndDisplay("You See '" & User & "'")
                    Look = True
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
            End If

        ElseIf Desc <> "" Then
            Try
                Dim DescName As String = Regex.Match(data, DescFilter).Groups(1).ToString()

                Player = NametoFurre(DescName, True)
                If LookQue.Count > 0 Then
                    Dim colorcode As String = LookQue.Peek
                    If colorcode.StartsWith("t") Then
                        colorcode = colorcode.Substring(0, 14)
                    ElseIf colorcode.StartsWith("u") Then

                    ElseIf colorcode.StartsWith("v") Then
                        'RGB Values
                    End If
                    Player.Color = LookQue.Dequeue()

                End If
                If BadgeTag.Count() > 0 Then
                    SpecTag = BadgeTag.Peek
                    BadgeTag.Clear()
                    Player.Badge = SpecTag
                ElseIf Player.Badge <> "" Then
                    Player.Badge = ""
                End If
                Player.Desc = Desc.Substring(6)
                If Dream.FurreList.Contains(Player) Then Dream.FurreList.Item(Player.ID) = Player
                MainEngine.PageSetVariable(MS_Name, DescName)
                MainEngine.PageExecute(600)
                'sndDisplay)
                If Player.Tag = "" Then
                    RaiseEvent DisplayText("You See '" & Player.Name & "'\par" & Desc, EventArgs.Empty)

                Else
                    RaiseEvent DisplayText("You See '" & Player.Name & "'\par" & Player.Tag & " " & Desc, EventArgs.Empty)
                End If
                Look = False
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        ElseIf Color = "shout" Then
            ''SHOUT
            Try
                Dim t As New Regex(YouSayFilter)
                Dim u As String = t.Match(data).Groups(1).ToString
                Text = t.Match(data).Groups(2).ToString
                If User = "" Then
                    RaiseEvent DisplayText("You " & u & ", """ & Text & """", EventArgs.Empty)

                Else
                    Text = Regex.Match(data, "shouts: (.*)</font>").Groups(1).ToString()
                    RaiseEvent DisplayText(User & " shouts, """ & Text & """", EventArgs.Empty)
                End If
                If Not IsConnectedCharacter Then
                    MainEngine.PageSetVariable("MESSAGE", Text)
                    Player.Message = Text
                    ' Execute (0:8) When some one shouts something
                    MainEngine.PageExecute(8, 9, 10)
                    ' Execute (0:9) When some one shouts {...}
                    ' Execute (0:10) When some one shouts something with {...} in it

                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        ElseIf Color = "query" Then
            Dim QCMD As String = Regex.Match(data, "<a.*?href='command://(.*?)'>").Groups(1).ToString
            'Player = NametoFurre(User, True)
            Select Case QCMD
                Case "summon"
                    ''JOIN
                    Try
                        RaiseEvent DisplayText(User & " requests to join you.", EventArgs.Empty)
                        'If Not IsConnectedCharacter Then
                        MainEngine.PageExecute(34, 35)
                        'End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "join"
                    ''SUMMON
                    Try
                        RaiseEvent DisplayText(User & " requests to summon you.", EventArgs.Empty)
                        'If Not IsConnectedCharacter Then
                        MainEngine.PageExecute(32, 33)
                        'End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "follow"
                    ''LEAD
                    Try
                        RaiseEvent DisplayText(User & " requests to lead.", EventArgs.Empty)
                        'If Not IsConnectedCharacter Then
                        MainEngine.PageExecute(36, 37)
                        'End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "lead"
                    ''FOLLOW
                    Try
                        RaiseEvent DisplayText(User & " requests the bot to follow.", EventArgs.Empty)
                        'If Not IsConnectedCharacter Then
                        MainEngine.PageExecute(38, 39)
                        'End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "cuddle"
                    Try
                        RaiseEvent DisplayText(User & " requests the bot to cuddle.", EventArgs.Empty)
                        'If Not IsConnectedCharacter Then
                        MainEngine.PageExecute(40, 41)
                        'End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case Else
                    RaiseEvent DisplayText("## Unknown " & Channel & "## " & data, EventArgs.Empty)
            End Select

            'NameFilter

            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        ElseIf Color = "whisper" Then
            ''WHISPER
            Try
                Dim WhisperFrom As String = Regex.Match(data, "whispers, ""(.*?)"" to you").Groups(1).Value
                Dim WhisperTo As String = Regex.Match(data, "You whisper ""(.*?)"" to").Groups(1).Value
                Dim WhisperDir As String = Regex.Match(data, String.Format("<name shortname='(.*?)' src='whisper-(.*?)'>")).Groups(2).Value
                If WhisperDir = "from" Then
                    'Player = NametoFurre(User, True)
                    Player.Message = WhisperFrom
                    If BadgeTag.Count() > 0 Then
                        SpecTag = BadgeTag.Peek
                        BadgeTag.Clear()
                        Player.Badge = SpecTag
                    Else
                        Player.Badge = ""
                    End If

                    If Dream.FurreList.Contains(Player) Then Dream.FurreList.Item(Player.ID) = Player

                    RaiseEvent DisplayText(User & " whispers""" & WhisperFrom & """ to you.", EventArgs.Empty)

                    If Not IsConnectedCharacter Then
                        MainEngine.PageSetVariable("MESSAGE", Player.Message)
                        ' Execute (0:15) When some one whispers something
                        MainEngine.PageExecute(15, 16, 17)
                        ' Execute (0:16) When some one whispers {...}
                        ' Execute (0:17) When some one whispers something with {...} in it
                    End If

                Else
                    WhisperTo = WhisperTo.Replace("<wnd>", "")
                    RaiseEvent DisplayText("You whisper""" & WhisperTo & """ to " & User & ".", EventArgs.Empty)

                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        ElseIf Color = "warning" Then

            MainEngine.PageExecute(801)
            RaiseEvent DisplayText("::WARNING:: " & Text, EventArgs.Empty)
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        ElseIf Color = "trade" Then
            Dim TextStr As String = Regex.Match(data, "\s<name (.*?)</name>").Groups(0).ToString()
            Text = Text.Substring(6)
            If User <> "" Then Text = " " & User & Text.Replace(TextStr, "")
            RaiseEvent DisplayText(Text, EventArgs.Empty)
            MainEngine.PageSetVariable("MESSAGE", Text)
            Player.Message = Text
            MainEngine.PageExecute(46, 47, 48)
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        ElseIf Color = "emote" Then
            Try
                ' ''EMOTE
                If SpeciesTag.Count() > 0 Then

                    Player.Color = SpeciesTag.Dequeue()
                End If
                Dim usr As Regex = New Regex(NameFilter)
                Dim n As System.Text.RegularExpressions.Match = usr.Match(Text)
                Text = usr.Replace(Text, "")

                Player = NametoFurre(n.Groups(3).Value, True)
                MainEngine.PageSetVariable("MESSAGE", Text)
                Player.Message = Text
                If Dream.FurreList.ContainsKey(Player.ID) Then Dream.FurreList.Item(Player.ID) = Player

                RaiseEvent DisplayText(User & " " & Text, EventArgs.Empty)

                Dim test As Boolean = IsConnectedCharacter
                If IsConnectedCharacter = False Then

                    ' Execute (0:11) When someone emotes something
                    MainEngine.PageExecute(11, 12, 13, 18, 19, 20)
                    ' Execute (0:12) When someone emotes {...}
                    ' Execute (0:13) When someone emotes something with {...} in it
                    ' Execute (0:18) When someone says or emotes something
                    ' Execute (0:19) When someone says or emotes {...}
                    ' Execute (0:20) When someone says or emotes something with {...} in it
                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
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

            RaiseEvent DisplayText("[" + ChanMatch.Groups(1).Value + "] " + User & ": " & Text, EventArgs.Empty)

            If IsClientConnected Then SendToClient("(" + data)
        ElseIf Color = "notify" Then
            Dim NameStr As String = ""
            If Text.StartsWith("Players banished from your dreams: ") Then
                'Banish-List
                '[notify> Players banished from your dreams:
                '`(0:54) When the bot sees the banish list
                BanishString.Clear()
                Dim tmp() As String = Text.Substring(35).Split(","c)
                For Each t As String In tmp
                    BanishString.Add(t)
                Next
                MainEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                MainEngine.PageExecute(54)

            ElseIf Text.StartsWith("The banishment of player ") Then
                'banish-off <name> (on list)
                '[notify> The banishment of player (.*?) has ended.

                '(0:56) When the bot successfully removes a furre from the banish list,
                '(0:58) When the bot successfully removes the furre named {...} from the banish list,
                Dim t As New Regex("The banishment of player (.*?) has ended.")
                NameStr = t.Match(data).Groups(1).Value
                MainEngine.PageSetVariable("BANISHNAME", NameStr)
                MainEngine.PageExecute(56, 56)
                For I As Integer = 0 To BanishString.Count - 1
                    If FurcadiaShortName(BanishString.Item(I)) = FurcadiaShortName(NameStr) Then
                        BanishString.RemoveAt(I)
                        Exit For
                    End If
                Next
                MainEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
            End If

            RaiseEvent DisplayText("[notify> " & Text, EventArgs.Empty)
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub

            ErrorMsg = Text
            ErrorNum = 2

            MainEngine.PageExecute(800)
            NameStr = ""
            If Text.Contains("There are no furres around right now with a name starting with ") Then
                'Banish <name> (Not online)
                'Error:>>  There are no furres around right now with a name starting with (.*?) .

                '(0:50) When the Bot fails to banish a furre,
                '(0:51) When the bot fails to banish the furre named {...},
                Dim t As New Regex("There are no furres around right now with a name starting with (.*?) .")
                NameStr = t.Match(data).Groups(1).Value
                MainEngine.PageSetVariable("BANISHNAME", NameStr)
                MainEngine.PageExecute(50, 51)
                MainEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
            ElseIf Text = "Sorry, this player has not been banished from your dreams." Then
                'banish-off <name> (not on list)
                'Error:>> Sorry, this player has not been banished from your dreams.

                '(0:55) When the Bot fails to remove a furre from the banish list,
                '(0:56) When the bot fails to remove the furre named {...} from the banish list,
                NameStr = BanishName
                MainEngine.PageSetVariable("BANISHNAME", NameStr)
                MainEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                MainEngine.PageExecute(50, 51)
            ElseIf Text = "You have not banished anyone." Then
                'banish-off-all (empty List)
                'Error:>> You have not banished anyone.

                '(0:59) When the bot fails to see the banish list,
                BanishString.Clear()
                MainEngine.PageExecute(59)
                MainEngine.PageSetVariable("BANISHLIST", "")
            ElseIf Text = "You do not have any cookies to give away right now!" Then
                MainEngine.PageExecute(95)
            End If

            RaiseEvent DisplayText("Error:>> " & Text, EventArgs.Empty)
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        ElseIf data.StartsWith("Communication") Then
            RaiseEvent DisplayText("Error: Communication Error.  Aborting connection.", EventArgs.Empty)
            ProcExit = False
            Disconnect()
            'LogSaveTmr.Enabled = False

        ElseIf Channel = "@cookie" Then
            ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>
            ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> All-time Cookie total: 0</font>
            ' <font color='success'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Your cookies are ready.  http://furcadia.com/cookies/ for more info!</font>
            '<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.

            Dim CookieToMe As Regex = New Regex(String.Format("{0}", CookieToMeREGEX))
            If CookieToMe.Match(data).Success Then
                MainEngine.PageSetVariable(MS_Name, CookieToMe.Match(data).Groups(2).Value)
                MainEngine.PageExecute(42, 43)
            End If
            Dim CookieToAnyone As Regex = New Regex(String.Format("<name shortname='(.*?)'>(.*?)</name> just gave <name shortname='(.*?)'>(.*?)</name> a (.*?)"))
            If CookieToAnyone.Match(data).Success Then
                MainEngine.PageSetVariable(MS_Name, CookieToAnyone.Match(data).Groups(3).Value)

                If IsBot(NametoFurre(CookieToAnyone.Match(data).Groups(3).Value, True)) Then
                    MainEngine.PageExecute(42, 43)
                Else
                    MainEngine.PageExecute(44)
                End If

            End If
            Dim CookieFail As Regex = New Regex(String.Format("You do not have any (.*?) left!"))
            If CookieFail.Match(data).Success Then
                MainEngine.PageExecute(45)
            End If
            Dim EatCookie As Regex = New Regex(Regex.Escape("<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.") + "(.*?)")
            If EatCookie.Match(data).Success Then
                MainEngine.PageSetVariable("MESSAGE", "You eat a cookie." + EatCookie.Replace(data, ""))
                Player.Message = "You eat a cookie." + EatCookie.Replace(data, "")
                MainEngine.PageExecute(49)

            End If
            RaiseEvent DisplayText(Text, EventArgs.Empty)

            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        ElseIf data.StartsWith("PS") Then

        ElseIf data.StartsWith("(You enter the dream of") Then
            MainEngine.PageSetVariable("DREAMNAME", "")
            MainEngine.PageSetVariable("DREAMOWNER", data.Substring(24, data.Length - 2 - 24))
            MainEngine.PageExecute(90, 91)
            RaiseEvent DisplayText(data, EventArgs.Empty)
            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub

        Else
            RaiseEvent DisplayText(data, EventArgs.Empty)

            If IsClientConnected Then SendToClient("(" + data)
            Exit Sub
        End If
        ' If IsClientConnected Then SendToClient("(" + data )
        ' Exit Sub

    End Sub

    ''' <summary>
    ''' Parse Server Data
    ''' <para>TODO: Move this functionality to <see cref="Furcadia.Net.Utils.ParseServer"/></para>
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="Handled"></param>
    Public Overrides Sub ParseServerData(ByVal data As String, ByVal Handled As Boolean)

        MyBase.ParseServerData(data, Handled)
        Dim MyServerInstruction As ServerInstructionType = ServerInstructionType.Unknown
        ' page = engine.LoadFromString(cBot.MS_Script)
        If data = "Dragonroar" Then
            ' BotConnecting()
            '  Login Sucessful

            Exit Sub

            'Logs into Furcadia
        ElseIf data = "&&&&&&&&&&&&&" Then
            'We've connected to Furcadia
            'Stop the reconnection manager
            '(0:1) When the bot logs into furcadia,
            MainEngine.PageExecute(1)

            Exit Sub
            ' Species Tags
        ElseIf data.StartsWith("]-") Then

            Exit Sub
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
            MainEngine.PageSetVariable("MESSAGE", Player.Message, True)
            MainEngine.PageExecute(95, 96)

            Exit Sub
            ']s(.+)1 (.*?) (.*?) 0
        ElseIf data.StartsWith("]s") Then
            Dim t As New Regex("\]s(.+)1 (.*?) (.*?) 0", RegexOptions.IgnoreCase)
            Dim m As System.Text.RegularExpressions.Match = t.Match(data)
            If Furcadia.Util.FurcadiaShortName(ConnectedCharacterName) = Furcadia.Util.FurcadiaShortName(m.Groups(2).Value) Then
                MainEngine.PageExecute()
            End If

            Exit Sub
            'Look response
        ElseIf data.StartsWith("]f") And ServerConnectPhase = ConnectionPhase.Connected And InDream = True Then

            Exit Sub
            'Spawn Avatar
        ElseIf data.StartsWith("<") And ServerConnectPhase = ConnectionPhase.Connected Then

            Try

                ' Add New Arrivals to Dream.FurreList
                ' One or the other will trigger it
                IsConnectedCharacter
                MainEngine.PageSetVariable(MS_Name, Player.ShortName)

                If Player.Flag = 4 Or Not Dream.FurreList.Contains(Player) Then
                    Dream.FurreList.Add(Player)
                    '  If InDream Then RaiseEvent UpDateDreamList(Player.Name)
                    If Player.Flag = 2 Then
                        MainEngine.PageExecute(28, 29, 24, 25)
                    Else
                        MainEngine.PageExecute(24, 25)
                    End If
                ElseIf Player.Flag = 2 Then

                    MainEngine.PageExecute(28, 29)

                ElseIf Player.Flag = 1 Then

                ElseIf Player.Flag = 0 Then

                End If

            Catch eX As Exception

                Dim logError As New ErrorLogging(eX, Me)
                Exit Sub
            End Try
            Exit Sub
            'Remove Furre
        ElseIf data.StartsWith(")") And ServerConnectPhase = ConnectionPhase.Connected Then  'And loggingIn = False
            Try

            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

            Exit Sub
            'Animated Move
        ElseIf data.StartsWith("/") And ServerConnectPhase = ConnectionPhase.Connected Then 'And loggingIn = False
            Try

                MainEngine.PageSetVariable(MS_Name, Player.ShortName)
                MainEngine.PageExecute(28, 29, 30, 31, 601, 602)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

            Exit Sub
            ' Move Avatar
        ElseIf data.StartsWith("A") And ServerConnectPhase = ConnectionPhase.Connected Then 'And loggingIn = False
            Try

                Dim Bot As FURRE = Dream.FurreList.Item(ConnectedCharacterFurcadiaID)
                Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(Bot.Position.x, Bot.Position.y)
                If VisableRectangle.X <= Me.Player.Position.x And VisableRectangle.Y <= Me.Player.Position.y And VisableRectangle.height >= Me.Player.Position.y And VisableRectangle.length >= Me.Player.Position.x Then

                    Player.Visible = True
                Else
                    Player.Visible = False
                End If

                MainEngine.PageSetVariable(MS_Name, Player.ShortName)
                MainEngine.PageExecute(28, 29, 30, 31, 601, 602)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

            Exit Sub
            ' Update Color Code
        ElseIf data.StartsWith("B") And ServerConnectPhase = ConnectionPhase.Connected And InDream Then 'And loggingIn = False

            Exit Sub
            'Hide Avatar
        ElseIf data.StartsWith("C") <> False And ServerConnectPhase = ConnectionPhase.Connected Then 'And loggingIn = False
            Try

                MainEngine.PageSetVariable(MS_Name, Player.ShortName)
                MainEngine.PageExecute(30, 31)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

            Exit Sub
            'Display Disconnection Dialog
        ElseIf data.StartsWith("[") Then
#If DEBUG Then
            Console.WriteLine("Disconnection Dialog:" & data)
#End If
            InDream = False
            Dream.FurreList.Clear()
            ' RaiseEvent UpDateDreamList("")

            MsgBox(data, MsgBoxStyle.Critical, "Disconnection Error")

            Exit Sub

            ';{mapfile}	Load a local map (one in the furcadia folder)
            ']q {name} {id}	Request to download a specific patch
        ElseIf data.StartsWith(";") OrElse data.StartsWith("]q") OrElse data.StartsWith("]r") Then
            Try
#If DEBUG Then
                Console.WriteLine("Entering new Dream" & data)
#End If
                MainEngine.PageSetVariable("DREAMOWNER", "")
                MainEngine.PageSetVariable("DREAMNAME", "")
                'RaiseEvent UpDateDreamList("")
                InDream = False
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try

            Exit Sub
        ElseIf data.StartsWith("]z") Then
            ConnectedCharacterFurcadiaID = Integer.Parse(data.Remove(0, 2))
            'Snag out UID
        ElseIf data.StartsWith("]B") Then
            ConnectedCharacterFurcadiaID = Integer.Parse(data.Substring(2, data.Length - ConnectedCharacterName.Length - 3))

            Exit Sub

        ElseIf data.StartsWith("]c") Then
#If DEBUG Then
            Console.WriteLine(data)
#End If

            Exit Sub
        ElseIf data.StartsWith("]C") Then
            If data.StartsWith("]C0") Then
                Dim dname As String = data.Substring(10)
                If dname.Contains(":") Then
                    Dim NameStr As String = dname.Substring(0, dname.IndexOf(":"))

                    MainEngine.PageSetVariable("DREAMOWNER", NameStr)
                ElseIf dname.EndsWith("/") AndAlso Not dname.Contains(":") Then
                    Dim NameStr As String = dname.Substring(0, dname.IndexOf("/"))

                    MainEngine.PageSetVariable("DREAMOWNER", NameStr)
                End If

                MainEngine.PageSetVariable("DREAMNAME", dname)
                MainEngine.PageExecute(90, 91)
            End If
#If DEBUG Then
            Console.WriteLine(data)
#End If

            Exit Sub
            'Process Channels Seperatly
        ElseIf data.StartsWith("(") Then
            If ThroatTired = False And data.StartsWith("(<font color='warning'>Your throat is tired. Try again in a few seconds.</font>") Then

                'Using Furclib ServQue
                ThroatTired = True

                '(0:92) When the bot detects the "Your throat is tired. Please wait a few seconds" message,
                MainEngine.PageExecute(92)

            End If

            Exit Sub
        Else

        End If

    End Sub

    ''' <summary>
    ''' Send a formatted string to the client and log window
    ''' </summary>
    ''' <param name="msg">Channel Subsystem?</param>
    ''' <param name="data">Message to send</param>
    Public Sub SendToClientFormattedText(msg As String, data As String)
        SendToClient("(" + "<b><i>[SM]</i> - " + msg + ":</b> """ + data + """")
        Writer.WriteLine("<b><i>[SM]</i> - " + msg + ":</b> """ + data + """")

    End Sub

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="Server_Instruction"></param>
    ''' <returns></returns>
    Private Function MessagePump(ByRef Server_Instruction As String) As Boolean
        Dim objPlugin As SilverMonkey.Interfaces.msPlugin
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

#Region "Dispose"
    Private disposed As Boolean
    ' Protected implementation of Dispose pattern.
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposed Then Return

        If disposing Then
            MyBase.Dispose()
        End If

        ' Free any unmanaged objects here.
        '
        disposed = True
    End Sub
#End Region
End Class