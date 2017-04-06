'Furcadia Servver Parser
'Event/Delegates for server instructions
'call subsystem processor

'dream info
'Furre info
'Bot info

'Furre Update events?

Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Threading
Imports Furcadia.Net
Imports Furcadia.Text.Base220
Imports Microsoft.Win32.SafeHandles
Imports MonkeyCore
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices
Imports System.Net.NetworkInformation
Imports MonkeyCore.Utils
Imports Furcadia.Util
Imports Furcadia.Drawing
Imports Furcadia.Drawing.VisibleArea
Imports Furcadia.Net.Utils.Movement

''' <summary>
''' This Instance handles the current Furcadia Session.
''' <para>Part1: Manage MonkeySpeak Engine Start,Stop,Restart. System Variables, MonkeySpeak Execution Triggers</para>
''' <para>Part2: Furcadia Proxy Controls, In/Out Ports, Host, Character Ini file. Connect, Disconnect, Reconnect</para>
''' <para>Part2a: Proxy Functions do link to Monkey Speak trigger execution</para>
''' <para>Part3: This Class Links loosley to the GUI </para>
'''
''' </summary>
Public Class FurcSession : Inherits NetProxy
    Implements IDisposable

#Region "Private Fields"
    ''' <summary>
    ''' Manage out Auto reconnects
    ''' </summary>
    Private WithEvents ReconnectionManager As New Furcadia.Net.Utils.ProxyReconnect

    ''' <summary>
    ''' Balance thhe out going load to server
    ''' <para> Throat Tired Syndrome and No Endurance Control</para>
    ''' </summary>
    Private WithEvents ServerBalancer As New Furcadia.Net.Utils.ServerQue

    ''' <summary>
    ''' Connection Status to server
    ''' </summary>
    Private LoggingIn As Integer
#End Region

#Region "Public Events"
    ''' <summary>
    ''' Track the Furcadia Client status
    ''' </summary>
    ''' <param name="Sender"></param>
    ''' <param name="e"></param>
    Public Event ClientStatusChanged(ByRef Sender As Object, e As EventArgs)

    ''' <summary>
    ''' Error Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Event OnError(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Track the Server Status
    ''' </summary>
    ''' <param name="Sender"></param>
    ''' <param name="e"></param>
    Public Event ServerStatusChanged(ByRef Sender As Object, e As EventArgs)
#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Server Connection status
    ''' </summary>
    ''' <returns>Status to the server</returns>
    Public ReadOnly Property ServerStatus As Furcadia.Net.Utils.NetServerArgs
        Get
            Return CType(LoggingIn, ConnectionStats)
        End Get
    End Property
#End Region

#Region "Public Methods"

#End Region

#Region "Fields"

#End Region

#Region "Public Fields"

#End Region

#Region "Protected Destructors"

#End Region

#Region "Private Methods"

#End Region

#Region "Public Methods"

    Public Sub BotConnecting()
        ClientClose = False
        '(0:1) When the bot logs into furcadia,
        MainEngine.PageExecute(1)
    End Sub

    ''' <summary>
    ''' Starts the Furcadia Connection Process
    ''' </summary>
    Public Sub ConnectBot()
        FurcMutex = New Mutex(False, Process + Environment.UserName)
        If FurcMutex.WaitOne(0, False) = False Then
            FurcMutex.Close()
            FurcMutex.Dispose()
            Console.WriteLine("Another copy  of Silver Monkey is Currently Connecting")
        Else
            Dim port As Integer = cBot.lPort

            If Not Furcadia.Net.Utils.Utilities.PortOpen(cBot.lPort) Then
                For i As Integer = cBot.lPort To cBot.lPort + 100
                    If Furcadia.Net.Utils.Utilities.PortOpen(i) Then
                        port = i
                        Exit For
                    End If
                Next
                'MsgBox("Local Port: " & cBot.lPort.ToString & " is in use, Aborting connection")
                'Exit Sub
            End If
        End If
    End Sub

    Public Sub DisconnectBot()

        Try
            If MainSettings.CloseProc And ProcExit = False Then

                KillProc(_FurcProcessId)
                SndToServer("quit")
                MyBase.Kill()
            End If
            SendDisplay("Disconnected.", fColorEnum.DefaultColor)
        Catch ex As Exception
            Dim logError As New ErrorLogging(ex, Me)
        End Try
        InDream = False

        ' (0:2) When the bot logs off
        MainEngine.PageExecute(2)
        Monkeyspeak.Libraries.Timers.DestroyTimers()
        MainEngine.MS_Engine_Running = False
    End Sub

    ' Public implementation of Dispose pattern callable by consumers.
    Public Overloads Sub Dispose() _
              Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Public Function GetEnumInt(Of T)(enumVal As T) As Integer
        Return Convert.ToInt32(enumVal)
    End Function

#End Region

    'TODO: Add Server Load Balancer
    'TODO: add Reconnection Manager
    'TODO: Implement this Class in restored from back up Main.vb "Main GUI Form"

#Region "Public Fields"

#End Region

#Region "Protected Destructors"

#End Region

#Region "Private Methods"

#End Region

#Region "Public Fields"

    Public WithEvents MainEngine As MainMsEngine

    Public WithEvents SubSys As New PhoenixSpeak.SubSystem()

    Public Shared BanishName As String = ""

    Public Shared BanishString As New List(Of String)

    Public Shared BotName As String

    'Bot specific Settings for Silver
    Public Shared BotUID As Integer = 0

    Public Shared Channel As String

    Public Shared ClientClose As Boolean = False

    Public Shared ErrorMsg As String = ""

    Public Shared ErrorNum As Short = 0

    Public Shared HasShare As Boolean

    'Property?
    Public Shared InDream As Boolean = False

    'Silver Monkey Specific Feature
    Public Shared objHost As New smHost
    'Monkey Speak Bot specific Variables

    Public Shared ProcExit As Boolean

    Public BotLogStream As LogStream

#End Region

#Region "Protected Destructors"

#End Region

#Region "Private Methods"

#End Region

#Region "Private Fields"

    Dim ChannelLock As New Object

    Dim clientlock As New Object

    Private DataReceived As Integer = 0

    Dim disposed As Boolean = False

    ' Instantiate a SafeHandle instance.
    Dim handle As SafeHandle = New SafeFileHandle(IntPtr.Zero, True)

    Private Look As Boolean

    Private MainSettings As Settings.MainConfigSettings

    ' Public Bot As FURRE
    Dim newData As Boolean = False

#End Region

#Region "Protected Destructors"

    Protected Overrides Sub Finalize()
        Try
            MyBase.Kill()

        Catch
        End Try
        MyBase.Finalize()
    End Sub

#End Region

#Region "Public Methods"

    Public Function isBot(ByRef player As FURRE) As Boolean
        Return player.ShortName <> FurcadiaShortName(BotName)
    End Function

    Public Sub KillProc(ByRef ID As Integer)
        GetProcessById(ID).Kill()
    End Sub

    Public Function NameToFurre(ByRef sname As String, ByRef UbdateMSVariableName As Boolean) As FURRE
        Dim p As New FURRE
        p.Name = sname

        For Each Character As FURRE In Dream.FurreList
            If Character.ShortName = Furcadia.Util.FurcadiaShortName(sname) Then
                p = Character
            End If
        Next
        If UbdateMSVariableName Then MainEngine.PageSetVariable(MS_Name, sname)
        Return p
    End Function
#End Region

    'TODO put this in Engine Start
    'If bFile.log Then
    'If String.IsNullOrEmpty(bFile.LogPath) Or Not Directory.Exists(bFile.LogPath) Then
    '            bFile.LogPath = Paths.SilverMonkeyLogPath
    '        End If
    '        callbk.LogStream = New LogStream(callbk.setLogName(bFile), bFile.LogPath)
    '    End If

    'If cBot.log Then
    'If String.IsNullOrEmpty(cBot.LogPath) Then
    '                cBot.LogPath = Paths.SilverMonkeyLogPath
    '            End If
    '            LogStream = New LogStream(setLogName(cBot), cBot.LogPath)
    '        End If

#Region "RegEx filters"
    Public Const ChannelNameFilter As String = "<channel name='(.*?)' />"
    Public Const DescFilter As String = "<desc shortname='([^']*)' />(.*)"
    Public Const EntryFilter As String = "^<font color='([^']*?)'>(.*?)</font>$"
    Public Const Iconfilter As String = "<img src='fsh://system.fsh:([^']*)'(.*?)/>"
    Public Const NameFilter As String = "<name shortname='([^']*)' ?(.*?)?>([\x21-\x3B\=\x3F-\x7E]+)</name>"
    Public Const YouSayFilter As String = "You ([\x21-\x3B\=\x3F-\x7E]+), ""([^']*)"""
    Private Const CookieToMeREGEX As String = "<name shortname='(.*?)'>(.*?)</name> just gave you"

#End Region

#Region "Dice Rolls"
    Public Const DiceFilter As String = "^<font color='roll'><img src='fsh://system.fsh:101' alt='@roll' /><channel name='@roll' /> <name shortname='([^ ]+)'>([^ ]+)</name> rolls (\d+)d(\d+)((-|\+)\d+)? ?(.*) & gets (\d+)\.</font>$"

    Public Shared DiceCompnentMatch As String

    Public Shared DiceCount As Double = 0.0R

    Public Shared DiceModifyer As Double = 0.0R

    Public Shared DiceResult As Double = 0.0R

    'TODO Check MS Engine Dice lines
    Public Shared DiceSides As Double = 0.0R
#End Region

#Region "Popup Dialogs"
    Public Repq As Queue(Of Rep) = New Queue(Of Rep)

    'TODO Check Furcadoia Popup Windows
    Public Structure Rep

#Region "Public Fields"

        Public ID As String
        Public type As Integer

#End Region

    End Structure
#End Region

#Region "Constructors"

    Friend Dream As DREAM
    Friend Player As FURRE
    Private Shared _FurcProcessId As Integer
    Private FurcMutex As Mutex
    Private LookQue As New Queue(Of String)
    Private SpeciesTag As New Queue(Of String)

    ''' <summary>
    '''
    ''' </summary>
    Sub New()
        MyBase.New()
        MainEngine = New MainMsEngine()
        ReconnectionManager = New Furcadia.Net.Utils.ProxyReconnect()
    End Sub

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="MainSettings"></param>
    ''' <param name="BotSettings"></param>
    Sub New(MainSettings As Settings.MainConfigSettings, BotSettings As Settings.cBot)
        MyBase.New(MainSettings.Host, MainSettings.sPort, BotSettings.lPort)

        MainEngine = New MainMsEngine()
        ReconnectionManager = New Furcadia.Net.Utils.ProxyReconnect()

        Try
            'TODO Convert to MainSetting and cBot for Setup.
            'Process
            '1 Start new instance of MonkeySprak Engine
            MainEngine = New MainMsEngine()
            '2 Start New Proxy Connection

            'Here we should move MyBase config to
            'ConnectBot()

        Catch ex As Exception
            'Debug.WriteLine(ex.Message)
            Throw ex
        End Try

    End Sub

    Public Property BadgeTag As New Queue(Of String)
    Public Property NoEndurance As Boolean
    Public Property ThroatTired As Boolean
        Get
            Return ServerBalancer.ThroatTired
        End Get
        Set(value As Boolean)
            ServerBalancer.ThroatTired = value
        End Set
    End Property
#End Region

#Region "Private Methods"

#End Region
    ''' <summary>
    ''' Connect to Furcadia and Launch our engine objects
    ''' <list type="bullet">
    ''' <listheader></listheader>
    ''' <item>Reconnection Manager</item>
    ''' <item>Launch MonkeySpeak Engine</item>
    ''' </list>
    ''' </summary>
    Public Overloads Sub Connect()

        'TODO: Reconnection.Manager.Start
        MainEngine.EngineStart(True)
        MyBase.Connect()
    End Sub

    ''' <summary>
    ''' Base Proxy has connected
    ''' </summary>
    Public Sub OnConnected() Handles MyBase.Connected

    End Sub

    ''' <summary>
    ''' Parse Channel Data
    ''' </summary>
    ''' <param name="data">Raw Game Server to Client instruction</param>
    ''' <param name="Handled">Is this data already handled?</param>
    Public Sub ParseServerChannel(ByRef data As String, ByVal Handled As Boolean)
        'Strip the trigger Character
        ' page = engine.LoadFromString(cBot.MS_Script)
        SyncLock ChannelLock
            data = data.Remove(0, 1)

            Dim SpecTag As String = ""
            Channel = Regex.Match(data, ChannelNameFilter).Groups(1).Value
            Dim Color As String = Regex.Match(data, EntryFilter).Groups(1).Value
            Dim User As String = ""
            Dim Desc As String = ""
            Dim Text As String = ""
            If Not Handled Then
                Text = Regex.Match(data, EntryFilter).Groups(2).Value
                User = Regex.Match(data, NameFilter).Groups(3).Value
                If User <> "" Then Player = NameToFurre(User, True)
                Player.Message = ""
                Desc = Regex.Match(data, DescFilter).Groups(2).Value
                Dim mm As New Regex(Iconfilter)
                Dim ds As Match = mm.Match(Text)
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

            ErrorMsg = ""
            ErrorNum = 0

            If Channel = "@news" Or Channel = "@spice" Then
                Try
                    sndDisplay(Text)
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf Color = "success" Then
                Try
                    If Text.Contains(" has been banished from your dreams.") Then
                        'banish <name> (online)
                        'Success: (.*?) has been banished from your dreams.

                        '(0:52) When the bot successfully banishes a furre,
                        '(0:53) When the bot successfully banishes the furre named {...},
                        Dim t As New Regex("(.*?) has been banished from your dreams.")
                        BanishName = t.Match(Text).Groups(1).ToString
                        MainEngine.PageSetVariable("BANISHNAME", BanishName)

                        BanishString.Add(BanishName)
                        MainEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                        MainEngine.PageExecute(52, 53)

                        ' MainMSEngine.PageExecute(53)
                    ElseIf Text = "You have canceled all banishments from your dreams." Then
                        'banish-off-all (active list)
                        'Success: You have canceled all banishments from your dreams.

                        '(0:60) When the bot successfully clears the banish list
                        BanishString.Clear()
                        MainEngine.PageSetVariable("BANISHLIST", Nothing)
                        MainEngine.PageSetVariable("BANISHNAME", Nothing)
                        MainEngine.PageExecute(60)

                    ElseIf Text.EndsWith(" has been temporarily banished from your dreams.") Then
                        'tempbanish <name> (online)
                        'Success: (.*?) has been temporarily banished from your dreams.

                        '(0:61) When the bot successfully temp banishes a Furre
                        '(0:62) When the bot successfully temp banishes the furre named {...}
                        Dim t As New Regex("(.*?) has been temporarily banished from your dreams.")
                        BanishName = t.Match(Text).Groups(1).Value
                        MainEngine.PageSetVariable("BANISHNAME", BanishName)
                        '  MainMSEngine.PageExecute(61)
                        BanishString.Add(BanishName)
                        MainEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                        MainEngine.PageExecute(61, 62)

                    ElseIf Text = "Control of this dream is now being shared with you." Then
                        HasShare = True

                    ElseIf Text.EndsWith("is now sharing control of this dream with you.") Then
                        HasShare = True

                    ElseIf Text.EndsWith("has stopped sharing control of this dream with you.") Then
                        HasShare = False

                    ElseIf Text.StartsWith("The endurance limits of player ") Then
                        Dim t As New Regex("The endurance limits of player (.*?) are now toggled off.")
                        Dim m As String = t.Match(Text).Groups(1).Value.ToString
                        If FurcadiaShortName(m) = FurcadiaShortName(BotName) Then
                            NoEndurance = True
                        End If

                    ElseIf Channel = "@cookie" Then
                        '(0:96) When the Bot sees "Your cookies are ready."
                        Dim CookiesReady As Regex = New Regex(String.Format("{0}", "Your cookies are ready.  http://furcadia.com/cookies/ for more info!"))
                        If CookiesReady.Match(data).Success Then
                            MainEngine.PageExecute(96)
                        End If
                    End If
                    sndDisplay(Text)
                    If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
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

                Player = NameToFurre(DiceMatch.Groups(3).Value, True)
                Player.Message = DiceMatch.Groups(7).Value
                MainEngine.PageSetVariable("MESSAGE", DiceMatch.Groups(7).Value)
                Double.TryParse(DiceMatch.Groups(4).Value, DiceSides)
                Double.TryParse(DiceMatch.Groups(3).Value, DiceCount)
                DiceCompnentMatch = DiceMatch.Groups(6).Value
                DiceModifyer = 0.0R
                Double.TryParse(DiceMatch.Groups(5).Value, DiceModifyer)
                Double.TryParse(DiceMatch.Groups(8).Value, DiceResult)

                If isBot(Player) Then
                    MainEngine.PageExecute(130, 131, 132, 136)
                Else
                    MainEngine.PageExecute(133, 134, 135, 136)
                End If

                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf Channel = "@dragonspeak" OrElse Channel = "@emit" OrElse Color = "emit" Then
                Try
                    '(<font color='dragonspeak'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Furcadian Academy</font>
                    '  SendDisplay(Text, fColorEnum.Emit)

                    MainEngine.PageSetVariable("MESSAGE", Text.Substring(5))
                    ' Execute (0:21) When someone emits something
                    MainEngine.PageExecute(21, 22, 23)
                    ' Execute (0:22) When someone emits {...}
                    '' Execute (0:23) When someone emits something with {...} in it

                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
                ''BCast (Advertisments, Announcments)
            ElseIf Color = "bcast" Then
                Dim AdRegEx As String = "<channel name='(.*)' />"

                Dim chan As String = Regex.Match(data, AdRegEx).Groups(1).Value
                Try

                    Select Case chan
                        Case "@advertisements"
                            If MainSettings.Advertisment Then Exit Sub
                            AdRegEx = "\[(.*?)\] (.*?)</font>"
                            Dim adMessage As String = Regex.Match(data, AdRegEx).Groups(2).Value
                            sndDisplay(Text)
                        Case "@bcast"
                            If MainSettings.Broadcast Then Exit Sub
                            Dim u As String = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups(2).Value
                            sndDisplay(Text)
                        Case "@announcements"
                            If MainSettings.Announcement Then Exit Sub
                            Dim u As String = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups(2).Value
                            sndDisplay(Text)
                        Case Else
#If DEBUG Then
                            Console.WriteLine("Unknown ")
                            Console.WriteLine("BCAST:" & data)
#End If
                    End Select

                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
                ''SAY
            ElseIf Color = "myspeech" Then
                Try
                    Dim t As New Regex(YouSayFilter)
                    Dim u As String = t.Match(data).Groups(1).ToString
                    Text = t.Match(data).Groups(2).ToString
                    If SpeciesTag.Count() > 0 Then
                        Player.Color = SpeciesTag.Dequeue()
                        If Dream.FurreList.Contains(Player) Then Dream.FurreList.Item(Player) = Player
                    End If

                    SendDisplay("You " & u & ", """ & Text & """", fColorEnum.Say)
                    Player.Message = Text
                    MainEngine.PageSetVariable("MESSAGE", Text)
                    ' Execute (0:5) When some one says something
                    'MainMSEngine.PageExecute(5, 6, 7, 18, 19, 20)
                    '' Execute (0:6) When some one says {...}
                    '' Execute (0:7) When some one says something with {...} in it
                    '' Execute (0:18) When someone says or emotes something
                    '' Execute (0:19) When someone says or emotes {...}
                    '' Execute (0:20) When someone says or emotes something with {...} in it
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf User <> "" And Channel = "" And Color = "" And Regex.Match(data, NameFilter).Groups(2).Value <> "forced" Then
                Dim tt As System.Text.RegularExpressions.Match = Regex.Match(data, "\(you see(.*?)\)", RegexOptions.IgnoreCase)
                Dim t As New Regex(NameFilter)
                If Not tt.Success Then

                    Try
                        Text = t.Replace(data, "")
                        Text = Text.Remove(0, 2)

                        If SpeciesTag.Count() > 0 Then
                            Player.Color = SpeciesTag.Dequeue()
                            If Dream.FurreList.Contains(Player) Then Dream.FurreList.Item(Player) = Player
                        End If
                        Channel = "say"
                        SendDisplay(User & " says, """ & Text & """", fColorEnum.Say)
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

                    If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
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

                    Player = NameToFurre(DescName, True)
                    If LookQue.Count > 0 Then
                        Dim colorcode As String = LookQue.Peek
                        If colorcode.StartsWith("t") Then
                            colorcode = colorcode.Substring(0, 14)
                        ElseIf colorcode.StartsWith("u") Then

                        ElseIf colorcode.StartsWith("v") Then
                            'RGB Values
                        End If
                        Player.Color = colorcode
                        LookQue.Dequeue()
                    End If
                    If BadgeTag.Count() > 0 Then
                        SpecTag = BadgeTag.Peek
                        BadgeTag.Clear()
                        Player.Badge = SpecTag
                    ElseIf Player.Badge <> "" Then
                        Player.Badge = ""
                    End If
                    Player.Desc = Desc.Substring(6)
                    If Dream.FurreList.Contains(Player) Then Dream.FurreList.Item(Player) = Player
                    MainEngine.PageSetVariable(MS_Name, DescName)
                    MainEngine.PageExecute(600)
                    'sndDisplay)
                    If Player.Tag = "" Then
                        sndDisplay("You See '" & Player.Name & "'\par" & Desc)
                    Else
                        sndDisplay("You See '" & Player.Name & "'\par" & Player.Tag & " " & Desc)
                    End If
                    Look = False
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf Color = "shout" Then
                ''SHOUT
                Try
                    Dim t As New Regex(YouSayFilter)
                    Dim u As String = t.Match(data).Groups(1).ToString
                    Text = t.Match(data).Groups(2).ToString
                    If User = "" Then
                        SendDisplay("You " & u & ", """ & Text & """", fColorEnum.Shout)
                    Else
                        Text = Regex.Match(data, "shouts: (.*)</font>").Groups(1).ToString()
                        SendDisplay(User & " shouts, """ & Text & """", fColorEnum.Shout)
                    End If
                    If Not isBot(Player) Then
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
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf Color = "query" Then
                Dim QCMD As String = Regex.Match(data, "<a.*?href='command://(.*?)'>").Groups(1).ToString
                'Player = NameToFurre(User, True)
                Select Case QCMD
                    Case "summon"
                        ''JOIN
                        Try
                            sndDisplay(User & " requests to join you.")
                            'If Not IsBot(Player) Then
                            MainEngine.PageExecute(34, 35)
                            'End If
                        Catch eX As Exception
                            Dim logError As New ErrorLogging(eX, Me)
                        End Try
                    Case "join"
                        ''SUMMON
                        Try
                            sndDisplay(User & " requests to summon you.")
                            'If Not IsBot(Player) Then
                            MainEngine.PageExecute(32, 33)
                            'End If
                        Catch eX As Exception
                            Dim logError As New ErrorLogging(eX, Me)
                        End Try
                    Case "follow"
                        ''LEAD
                        Try
                            sndDisplay(User & " requests to lead.")
                            'If Not IsBot(Player) Then
                            MainEngine.PageExecute(36, 37)
                            'End If
                        Catch eX As Exception
                            Dim logError As New ErrorLogging(eX, Me)
                        End Try
                    Case "lead"
                        ''FOLLOW
                        Try
                            sndDisplay(User & " requests the bot to follow.")
                            'If Not IsBot(Player) Then
                            MainEngine.PageExecute(38, 39)
                            'End If
                        Catch eX As Exception
                            Dim logError As New ErrorLogging(eX, Me)
                        End Try
                    Case "cuddle"
                        Try
                            sndDisplay(User & " requests the bot to cuddle.")
                            'If Not IsBot(Player) Then
                            MainEngine.PageExecute(40, 41)
                            'End If
                        Catch eX As Exception
                            Dim logError As New ErrorLogging(eX, Me)
                        End Try
                    Case Else
                        sndDisplay("## Unknown " & Channel & "## " & data)
                End Select

                'NameFilter

                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf Color = "whisper" Then
                ''WHISPER
                Try
                    Dim WhisperFrom As String = Regex.Match(data, "whispers, ""(.*?)"" to you").Groups(1).Value
                    Dim WhisperTo As String = Regex.Match(data, "You whisper ""(.*?)"" to").Groups(1).Value
                    Dim WhisperDir As String = Regex.Match(data, String.Format("<name shortname='(.*?)' src='whisper-(.*?)'>")).Groups(2).Value
                    If WhisperDir = "from" Then
                        'Player = NameToFurre(User, True)
                        Player.Message = WhisperFrom
                        If BadgeTag.Count() > 0 Then
                            SpecTag = BadgeTag.Peek
                            BadgeTag.Clear()
                            Player.Badge = SpecTag
                        Else
                            Player.Badge = ""
                        End If

                        If Dream.FurreList.Contains(Player) Then Dream.FurreList.Item(Player) = Player

                        SendDisplay(User & " whispers""" & WhisperFrom & """ to you.", fColorEnum.Whisper)
                        If Not isBot(Player) Then
                            MainEngine.PageSetVariable("MESSAGE", Player.Message)
                            ' Execute (0:15) When some one whispers something
                            MainEngine.PageExecute(15, 16, 17)
                            ' Execute (0:16) When some one whispers {...}
                            ' Execute (0:17) When some one whispers something with {...} in it
                        End If

                    Else
                        WhisperTo = WhisperTo.Replace("<wnd>", "")
                        SendDisplay("You whisper""" & WhisperTo & """ to " & User & ".", fColorEnum.Whisper)
                    End If
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf Color = "warning" Then

                ErrorMsg = Text
                ErrorNum = 1
                MainEngine.PageExecute(801)
                SendDisplay("::WARNING:: " & Text, fColorEnum.DefaultColor)
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf Color = "trade" Then
                Dim TextStr As String = Regex.Match(data, "\s<name (.*?)</name>").Groups(0).ToString()
                Text = Text.Substring(6)
                If User <> "" Then Text = " " & User & Text.Replace(TextStr, "")
                SendDisplay(Text, fColorEnum.DefaultColor)
                MainEngine.PageSetVariable("MESSAGE", Text)
                Player.Message = Text
                MainEngine.PageExecute(46, 47, 48)
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf Color = "emote" Then
                Try
                    ' ''EMOTE
                    If SpeciesTag.Count() > 0 Then
                        SpecTag = SpeciesTag.Peek
                        SpeciesTag.Dequeue()
                        Player.Color = SpecTag
                    End If
                    Dim usr As Regex = New Regex(NameFilter)
                    Dim n As System.Text.RegularExpressions.Match = usr.Match(Text)
                    Text = usr.Replace(Text, "")

                    Player = NameToFurre(n.Groups(3).Value, True)
                    MainEngine.PageSetVariable("MESSAGE", Text)
                    Player.Message = Text
                    If Dream.FurreList.Contains(Player) Then Dream.FurreList.Item(Player) = Player
                    SendDisplay(User & " " & Text, fColorEnum.Emote)
                    Dim test As Boolean = isBot(Player)
                    If isBot(Player) = False Then

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
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
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
                sndDisplay("[" + ChanMatch.Groups(1).Value + "] " + User & ": " & Text)
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
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
                        If Furcadia.Util.FurcadiaShortName(BanishString.Item(I).ToString) = Furcadia.Util.FurcadiaShortName(NameStr) Then
                            BanishString.RemoveAt(I)
                            Exit For
                        End If
                    Next
                    MainEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                End If

                SendDisplay("[notify> " & Text, fColorEnum.DefaultColor)
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf Color = "error" Then

                ErrorMsg = Text
                ErrorNum = 2

                MainEngine.PageExecute(800)
                Dim NameStr As String = ""
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
                    MainEngine.PageSetVariable("BANISHLIST", Nothing)
                ElseIf Text = "You do not have any cookies to give away right now!" Then
                    MainEngine.PageExecute(95)
                End If

                sndDisplay("Error:>> " & Text)
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf data.StartsWith("Communication") Then
                sndDisplay("Error: Communication Error.  Aborting connection.")
                ProcExit = False
                DisconnectBot()
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
                    'MainMSEngine.PageSetVariable( MS_Name, CookieToAnyone.Match(data).Groups(3).Value)
                    If isBot(NameToFurre(CookieToAnyone.Match(data).Groups(3).Value, True)) Then
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
                'Dim args As New ServerReceiveEventArgs
                'args.Channel = Channel
                'args.Text = data
                'args.Handled = True
                'RaiseEvent ServerChannelProcessed(data, args)
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            ElseIf data.StartsWith("PS") Then
                Color = "PhoenixSpeak"
                ParseServerData(data, Handled)

                If MainSettings.PSShowMainWindow Then
                    'Dim args As New Furcadia.Net.Utils.NetServerArgs
                    'args.ConnectionPhase.
                    'args.Channel = "PhoenixSpeak"
                    'args.Text = data
                    'args.Handled = True
                    'RaiseEvent ServerChannelProcessed(data, args)
                End If
                If MainSettings.PSShowClient Then
                    If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                End If
                Exit Sub
            ElseIf data.StartsWith("(You enter the dream of") Then
                MainEngine.PageSetVariable("DREAMNAME", Nothing)
                MainEngine.PageSetVariable("DREAMOWNER", data.Substring(24, data.Length - 2 - 24))
                MainEngine.PageExecute(90, 91)
                sndDisplay(data)
                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub

            Else
                sndDisplay(data)

                If MyBase.IsClientConnected Then MyBase.SendClient("(" + data)
                Exit Sub
            End If
            ' If MyBase.IsClientConnected Then MyBase.SendClient("(" + data )
            ' Exit Sub
        End SyncLock
    End Sub

    ''' <summary>
    ''' Parse Server Data
    ''' <para>TODO: Move this functionality to <see cref="Furcadia.Net.Utils.ParseServer"/></para>
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="Handled"></param>
    Public Sub ParseServerData(ByVal data As String, ByVal Handled As Boolean)

        ' page = engine.LoadFromString(cBot.MS_Script)
        If data = "Dragonroar" Then
            BotConnecting()
            '  Login Sucessful
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub

            'Logs into Furcadia
        ElseIf data = "&&&&&&&&&&&&&" Then
            'We've connected to Furcadia
            'Stop the reconnection manager
            LoggingIn = 2
            RelogCounter = 0
            '(0:1) When the bot logs into furcadia,
            MainEngine.PageExecute(1)
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
            ' Species Tags
        ElseIf data.StartsWith("]-") Then
            If data.StartsWith("]-#A") Then
                SpeciesTag.Enqueue(data.Substring(4))
            ElseIf data.StartsWith("]-#B") Then
                BadgeTag.Enqueue(data.Substring(2))
            End If

            If MyBase.IsClientConnected Then MyBase.SendClient(data)
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
            Integer.TryParse(m.Groups(2).Value, r.type)
            Repq.Enqueue(r)
            MainEngine.PageSetVariable("MESSAGE", m.Groups(3).Value, True)
            Player.Message = m.Groups(3).Value
            MainEngine.PageExecute(95, 96)
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
            ']s(.+)1 (.*?) (.*?) 0
        ElseIf data.StartsWith("]s") Then
            Dim t As New Regex("\]s(.+)1 (.*?) (.*?) 0", RegexOptions.IgnoreCase)
            Dim m As System.Text.RegularExpressions.Match = t.Match(data)
            If Furcadia.Util.FurcadiaShortName(BotName) = Furcadia.Util.FurcadiaShortName(m.Groups(2).Value) Then
                MainEngine.PageExecute()
            End If
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
            'Look response
        ElseIf data.StartsWith("]f") And bConnected() And InDream = True Then
            Dim length As Short = 14
            If Look Then
                LookQue.Enqueue(data.Substring(2))
            Else
                If data.Substring(2, 1) <> "t" Then
                    length = 30
                Else
                    length = 14
                End If
                Try
                    Player = NameToFurre(data.Remove(0, length + 2), True)
                    ' If Player.ID = 0 Then Exit Sub
                    Player.Color = data.Substring(2, length)
                    If isBot(Player) Then Look = False
                    If Dream.FurreList.Contains(Player) Then Dream.FurreList(Dream.FurreList.IndexOf(Player)) = Player
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try

            End If
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
            'Spawn Avatar
        ElseIf data.StartsWith("<") And bConnected() Then
            Try
                If data.Length < 29 Then Exit Sub
                ' Debug.Print(data)
                Player = New FURRE(ConvertFromBase220(data.Substring(1, 4)))

                If Dream.FurreList.Contains(Player) Then
                    Player = Dream.FurreList.Item(Player)
                End If
                Player.Position = New Furcadia.Drawing.FurrePosition(ConvertFromBase220(data.Substring(5, 2)) * 2, ConvertFromBase220(data.Substring(7, 2)))
                Player.Shape = ConvertFromBase220(data.Substring(9, 2))

                Dim NameLength As Integer = ConvertFromBase220(data.Substring(11, 1))
                Me.Player.Name = data.Substring(12, NameLength).Replace("|", " ")

                Dim ColTypePos As Integer = 12 + NameLength
                Me.Player.ColorType = CChar(data.Substring(ColTypePos, 1))
                Dim ColorSize As UInteger = 10
                'If Player.ColorType <> "t" Then
                '    ColorSize = 30
                'End If
                Dim sColorPos As Integer = ColTypePos + 1

                Player.Color = data.Substring(sColorPos, CInt(ColorSize))

                Dim FlagPos As Integer = data.Length - 6
                Player.Flag = CInt(ConvertFromBase220(data.Substring(FlagPos, 1)))
                Dim AFK_Pos As Integer = data.Length - 5
                Dim AFKStr As String = data.Substring(AFK_Pos, 4)
                Player.AFK = ConvertFromBase220(data.Substring(AFK_Pos, 4))
                Dim FlagCheck As Integer = Flags.CHAR_FLAG_NEW_AVATAR - Player.Flag

                ' Add New Arrivals to Dream List
                ' One or the other will trigger it
                isBot(Player)
                MainEngine.PageSetVariable(MS_Name, Player.ShortName)

                If Player.Flag = 4 Or Not Dream.FurreList.Contains(Player) Then
                    Dream.FurreList.add(Player)
                    '  If InDream Then RaiseEvent UpDateDreamList(Player.Name)
                    If Player.Flag = 2 Then
                        Dim Bot As FURRE = NameToFurre(BotName, False)
                        Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(Bot.Position.x, Bot.Position.y)
                        If VisableRectangle.X <= Me.Player.Position.y And VisableRectangle.Y <= Me.Player.Position.y And VisableRectangle.height >= Me.Player.Position.y And VisableRectangle.length >= Me.Player.Position.x Then
                            Player.Visible = True
                        Else
                            Player.Visible = False
                        End If
                        MainEngine.PageExecute(28, 29, 24, 25)
                    Else
                        MainEngine.PageExecute(24, 25)
                    End If
                ElseIf Player.Flag = 2 Then
                    Dim Bot As FURRE = NameToFurre(BotName, False)
                    Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(Bot.Position.x, Bot.Position.y)
                    If VisableRectangle.X <= Me.Player.Position.x And VisableRectangle.Y <= Me.Player.Position.y And VisableRectangle.height >= Me.Player.Position.y And VisableRectangle.length >= Me.Player.Position.x Then
                        Player.Visible = True
                    Else
                        Player.Visible = False
                    End If
                    MainEngine.PageExecute(28, 29)

                ElseIf Player.Flag = 1 Then

                ElseIf Player.Flag = 0 Then

                End If
                If Dream.FurreList.Contains(Player) Then
                    Dream.FurreList.Item(Dream.FurreList.IndexOf(Player)) = Player
                End If
            Catch eX As Exception

                Dim logError As New ErrorLogging(eX, Me)
                Exit Sub
            End Try
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
            'Remove Furre
        ElseIf data.StartsWith(")") And bConnected() Then 'And loggingIn = False
            Try
                Dim remID As Integer = ConvertFromBase220(data.Substring(1, 4))
                ' remove departure from List
                If Dream.FurreList.Contains(remID) = True Then
                    Player = Dream.FurreList.Item(remID)
                    MainEngine.PageSetVariable(MS_Name, Player.Name)
                    MainEngine.PageExecute(26, 27, 30, 31)
                    Dream.FurreList.Remove(remID)
                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
            'Animated Move
        ElseIf data.StartsWith("/") And bConnected() Then 'And loggingIn = False
            Try
                Player = Dream.FurreList.Item(ConvertFromBase220(data.Substring(1, 4)))
                Player.Position.x = ConvertFromBase220(data.Substring(5, 2)) * 2
                Player.Position.y = ConvertFromBase220(data.Substring(7, 2))
                Player.Shape = ConvertFromBase220(data.Substring(9, 2))
                Dim Bot As FURRE = Dream.FurreList.Item(BotUID)
                Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(Bot.Position.x, Bot.Position.y)
                If VisableRectangle.X <= Me.Player.Position.x And VisableRectangle.Y <= Me.Player.Position.y And VisableRectangle.height >= Me.Player.Position.y And VisableRectangle.length >= Me.Player.Position.x Then
                    Player.Visible = True
                Else
                    Player.Visible = False
                End If
                If Dream.FurreList.Contains(Player) Then Dream.FurreList.Item(Player) = Player
                isBot(Player)
                MainEngine.PageSetVariable(MS_Name, Player.ShortName)
                MainEngine.PageExecute(28, 29, 30, 31, 601, 602)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
            ' Move Avatar
        ElseIf data.StartsWith("A") And bConnected() Then 'And loggingIn = False
            Try
                Player = Dream.FurreList.Item(ConvertFromBase220(data.Substring(1, 4)))
                Player.Position.x = ConvertFromBase220(data.Substring(5, 2)) * 2
                Player.Position.y = ConvertFromBase220(data.Substring(7, 2))
                Player.Shape = ConvertFromBase220(data.Substring(9, 2))

                Dim Bot As FURRE = Dream.FurreList.Item(BotUID)
                Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(Bot.Position.x, Bot.Position.y)
                If VisableRectangle.X <= Me.Player.Position.x And VisableRectangle.Y <= Me.Player.Position.y And VisableRectangle.height >= Me.Player.Position.y And VisableRectangle.length >= Me.Player.Position.x Then

                    Player.Visible = True
                Else
                    Player.Visible = False
                End If
                isBot(Player)
                MainEngine.PageSetVariable(MS_Name, Player.ShortName)
                MainEngine.PageExecute(28, 29, 30, 31, 601, 602)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
            ' Update Color Code
        ElseIf data.StartsWith("B") And bConnected() And InDream Then 'And loggingIn = False
            Try
                Player = Dream.FurreList.Item(ConvertFromBase220(data.Substring(1, 4)))
                Player.Shape = ConvertFromBase220(data.Substring(5, 2))
                Dim ColTypePos As UInteger = 7
                Player.ColorType = CChar(data.Substring(CInt(ColTypePos), 1))
                Dim ColorSize As UInteger = 10

                Dim sColorPos As UInteger = CUInt(ColTypePos + 1)
                Player.Color = data.Substring(CInt(sColorPos), CInt(ColorSize))

                isBot(Player)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
            'Hide Avatar
        ElseIf data.StartsWith("C") <> False And bConnected() Then 'And loggingIn = False
            Try
                Player = Dream.FurreList.Item(ConvertFromBase220(data.Substring(1, 4)))
                Player.Position.x = ConvertFromBase220(data.Substring(5, 2)) * 2
                Player.Position.y = ConvertFromBase220(data.Substring(7, 2))
                Player.Visible = False
                If Dream.FurreList.Contains(Player) Then
                    Dream.FurreList.Item(Player) = Player
                End If
                isBot(Player)
                MainEngine.PageSetVariable(MS_Name, Player.Name)
                MainEngine.PageExecute(30, 31)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
            'Display Disconnection Dialog
        ElseIf data.StartsWith("[") Then
#If DEBUG Then
            Console.WriteLine("Disconnection Dialog:" & data)
#End If
            InDream = False
            Dream.FurreList.Clear()
            ' RaiseEvent UpDateDreamList("")

            If MyBase.IsClientConnected Then MyBase.SendClient(data)
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
                HasShare = False
                NoEndurance = False

                Dream.FurreList.Clear()
                'RaiseEvent UpDateDreamList("")
                InDream = False
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
        ElseIf data.StartsWith("]z") Then
            BotUID = Integer.Parse(data.Remove(0, 2))
            'Snag out UID
        ElseIf data.StartsWith("]B") Then
            BotUID = Integer.Parse(data.Substring(2, data.Length - BotName.Length - 3))
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub

        ElseIf data.StartsWith("]c") Then
#If DEBUG Then
            Console.WriteLine(data)
#End If
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
        ElseIf data.StartsWith("]C") Then
            If data.StartsWith("]C0") Then
                Dim dname As String = data.Substring(10)
                If dname.Contains(":") Then
                    Dim NameStr As String = dname.Substring(0, dname.IndexOf(":"))
                    If FurcadiaShortName(NameStr) = FurcadiaShortName(BotName) Then
                        HasShare = True
                    End If
                    MainEngine.PageSetVariable("DREAMOWNER", NameStr)
                ElseIf dname.EndsWith("/") AndAlso Not dname.Contains(":") Then
                    Dim NameStr As String = dname.Substring(0, dname.IndexOf("/"))
                    If Furcadia.Util.FurcadiaShortName(NameStr) = Furcadia.Util.FurcadiaShortName(BotName) Then
                        HasShare = True
                    End If
                    MainEngine.PageSetVariable("DREAMOWNER", NameStr)
                End If

                MainEngine.PageSetVariable("DREAMNAME", dname)
                MainEngine.PageExecute(90, 91)
            End If
#If DEBUG Then
            Console.WriteLine(data)
#End If
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
            Exit Sub
            'Process Channels Seperatly
        ElseIf data.StartsWith("(") Then
            If ThroatTired = False And data.StartsWith("(<font color='warning'>Your throat is tired. Try again in a few seconds.</font>") Then

                'Using Furclib ServQue
                ThroatTired = True

                '(0:92) When the bot detects the "Your throat is tired. Please wait a few seconds" message,
                MainEngine.PageExecute(92)
                If MyBase.IsClientConnected Then MyBase.SendClient(data)
            End If

            Exit Sub
        Else
            If MyBase.IsClientConnected Then MyBase.SendClient(data)
        End If

    End Sub

    ''' <summary>
    ''' Send a formatted string to the client and log window
    ''' </summary>
    ''' <param name="msg">Channel Subsystem?</param>
    ''' <param name="data">Message to send</param>
    Public Sub SendClientMessage(msg As String, data As String)
        If MyBase.IsClientConnected Then MyBase.SendClient("(" + "<b><i>[SM]</i> - " + msg + ":</b> """ + data + """")
        sndDisplay("<b><i>[SM]</i> - " + msg + ":</b> """ + data + """")
    End Sub

    ''' <summary>
    ''' Format basic furcadia commands and send to server
    ''' <para>
    ''' We also mirror the client to server banish system.
    ''' </para>
    ''' <para>
    ''' This maybe a good place to place Proxy/Bot commands for controls
    ''' </para>
    ''' </summary>
    ''' <param name="data">Raw Client to Server instruction</param>
    Public Sub sndServer(ByRef data As String)
        Try
            If Not MyBase.IsServerConnected Then Exit Sub
            If data.StartsWith("`m ") Then

                Select Case data.Substring(2, 1)
                    Case "7"

                    Case "9"

                    Case "1"

                    Case "3"

                End Select
            ElseIf data = "`use" Then

            ElseIf data = "`get" Then

            ElseIf data = "`>" Then

            ElseIf data = "`<" Then

            ElseIf data = "`lie" Then

            ElseIf data = "`liedown" Then

            ElseIf data = "`sit" Then

            ElseIf data = "`stand" Then

            ElseIf data.StartsWith("banish ") Then
                BanishName = data.Substring(7)
                MainEngine.PageSetVariable("BANISHNAME", BanishName)
            ElseIf data.StartsWith("banish-off ") Or data.StartsWith("tempbanish ") Then
                BanishName = data.Substring(11)
                MainEngine.PageSetVariable("BANISHNAME", BanishName)
            ElseIf data = "banish-list" Then
                BanishName = ""
                MainEngine.PageSetVariable("BANISHNAME", Nothing)
            End If

            TextToServer(data)
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub

    ''' <summary>
    ''' Text Channel Prefixes (shout,whisper emote, Raw Server command)
    ''' </summary>
    ''' <param name="arg"></param>
    Public Sub TextToServer(ByRef arg As String)
        Try

            If String.IsNullOrWhiteSpace(arg) Then Exit Sub
            'Clean Text input to match Client
            Dim result As String = ""
            Select Case arg.Substring(0, 1)
                Case "`"
                    result = arg.Remove(0, 1)
                Case "/"
                    result = "wh " & arg.Substring(1)
                Case ":"
                    result = arg

                Case "-"
                    result = arg

                Case Else
                    result = Chr(34) & arg
            End Select
            SndToServer(result)

        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me, arg.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' Furcadia Client lost Connection (was it intentionally closed?)
    ''' </summary>
    Private Sub ClientExit() Handles MyBase.ClientExited
        If LoggingIn = 0 Then Exit Sub
        MainEngine.PageExecute(3)

        LoggingIn = 0
        If cBot.StandAlone = False Then

            ClientClose = True
            If MainSettings.CloseProc Then
                ProcExit = False
            Else
                ProcExit = True
            End If
            DisconnectBot()
        ElseIf MyBase.IsClientConnected() Then
            MyBase.CloseClient()
        End If

    End Sub

    ''' <summary>
    ''' Pump the server to client instructions through Plugins
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
                    objPlugin.Page = MainEngine.MSpage
                    If objPlugin.MessagePump(Server_Instruction) Then Handled = True
                End If
            Next
        End If
        Return Handled
    End Function

    ''' <summary>
    ''' Client sent us some data, Let's deal with it
    ''' </summary>
    ''' <param name="data"></param>
    Private Sub onClientDataReceived(ByVal data As String) Handles MyBase.ClientData2

        Try

            If (Monitor.TryEnter(clientlock)) Then

                If data.StartsWith("quit") And cBot.StandAlone Then
                    Exit Sub

                    'Capture The Bots Name
                ElseIf data.StartsWith("connect") Then

                    Dim test As String = data.Replace("connect ", "").TrimStart(" "c)
                    BotName = test.Substring(0, test.IndexOf(" "))
                    BotName = BotName.Replace("|", " ")

                    'Hack to Keep Main Form Text Current
                    'callbk.MainFormText(BotName)

                    BotName = BotName.Replace("[^a-zA-Z0-9\0x0020_.| ]+", "").ToLower()
                    MainEngine.PageSetVariable("BOTNAME", BotName)
                ElseIf data = "vascodagama" And LoggingIn = 2 Then
                    sndServer("`uid")
                    Select Case cBot.GoMapIDX
                        Case 1
                            sndServer("`gomap #")
                        Case 2
                            sndServer("`gomap *")
                        Case 3

                        Case 4
                            sndServer("`fdl " + cBot.DreamURL)
                    End Select
                    LoggingIn = 3
                End If
                MyBase.SendServer(data)
            End If
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)

        Finally
            Monitor.Exit(clientlock)
        End Try

    End Sub

    Private Sub onServerDataReceived(ByVal data As String) Handles MyBase.ServerData2
        Try
            Monitor.Enter(DataReceived)
            Player = New FURRE
            Channel = ""
            MainEngine.PageSetVariable(MS_Name, "")
            MainEngine.PageSetVariable("MESSAGE", "")
            Dim test As Boolean = MessagePump(data)
            ParseServerData(data, test)
        Finally
            Monitor.Exit(DataReceived)
        End Try
    End Sub

    Private Sub OnServerDisconnect() Handles MyBase.ServerDisConnected
        If MyBase.IsClientConnected Then
            ProcExit = False

        End If
        DisconnectBot()
    End Sub

    Private Shadows Sub ProxyError(eX As Exception, o As Object, n As String) Handles MyBase.Error

        'TODO: add custom event args

        'Dim args As New Furcadia.Net.Utils..ConnectionEventArgs
        'args.Status = ConnectionStats.error
        'RaiseEvent OnError(o, args)
    End Sub

    Private Sub SendDisplay(v As String, defaultColor As fColorEnum)
        Throw New NotImplementedException()
    End Sub
    Private Sub sndDisplay(v As String)

    End Sub
    Private Sub SndToServer(v As String)
        SendServer(v)
    End Sub
#Region "Dispose"
    'Dim Ts As TimeSpan = TimeSpan.FromSeconds(MainSettings.ConnectTimeOut)
    '        ReconnectTimeOutTimer = New Threading.Timer(AddressOf ReconnectTimeOutTick,
    '         Nothing, Ts, Ts)
    '        Dim Tss As TimeSpan = TimeSpan.FromSeconds(MainSettings.Ping)
    'If MainSettings.Ping > 0 Then PingTimer = New Threading.Timer(AddressOf PingTimerTick,
    '         Nothing, Tss, Tss)
    Public Overrides Sub SendServer(message As String)
        ServerBalancer.SendToServer(message)
    End Sub

    ' Protected implementation of Dispose pattern.
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposed Then Return

        If disposing Then
            handle.Dispose()
        End If

        ' Free any unmanaged objects here.
        '
        disposed = True
    End Sub
#End Region
End Class