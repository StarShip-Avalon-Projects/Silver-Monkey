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
Imports Furcadia.Base220
Imports Microsoft.Win32.SafeHandles
Imports MonkeyCore
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices
Imports System.Net.NetworkInformation
Imports MonkeyCore.Utils
Imports Furcadia.Net.Movement

Public Class FurcadiaSession : Implements IDisposable

    Private Const CookieToMeREGEX As String = "<name shortname='(.*?)'>(.*?)</name> just gave you"

    Public Shared objHost As New smHost

    <CLSCompliant(False)>
    Public Shared Player As New FURRE
    <CLSCompliant(False)>
    Public Shared loggingIn As UShort = 0
    <CLSCompliant(False)>
    Public DREAM As New DREAM
    Public FurcMutex As Mutex

    Public Const FurcProcess As String = "Furcadia"
    Public Const VarPrefix As String = "%"

    'Property?
    Public InDream As Boolean = False
    Public ClientClose As Boolean = False
    Public ErrorNum As Short = 0
    Public ErrorMsg As String = ""
    'Monkey Speak Bot specific Variables

    Public Shared ProcExit As Boolean
    'Boolean Hack as Controls need a Boolean for enable
    Public Shared Function bConnected() As Boolean
        If loggingIn >= 2 Then
            Return True
        End If
        Return False
    End Function
    ' Public Bot As FURRE

    'Properties?
    <CLSCompliant(False)>
    Public BotUID As UInteger = 0
    Public BotName As String = ""
    Public HasShare As Boolean = False
    'Public NoEndurance As Boolean = False
    '\Properties?

    Public BanishName As String = ""
    Public BanishString As New List(Of String)
    Public Shared ReLogCounter As Integer = 0
    Dim newData As Boolean = False

    <CLSCompliant(False)>
    Public WithEvents smProxy As NetProxy

#Region "RegEx filters"
    Public Const EntryFilter As String = "^<font color='([^']*?)'>(.*?)</font>$"
    Public Const NameFilter As String = "<name shortname='([^']*)' ?(.*?)?>([\x21-\x3B\=\x3F-\x7E]+)</name>"
    Public Const DescFilter As String = "<desc shortname='([^']*)' />(.*)"
    Public Const ChannelNameFilter As String = "<channel name='(.*?)' />"
    Public Const Iconfilter As String = "<img src='fsh://system.fsh:([^']*)'(.*?)/>"
    Public Const YouSayFilter As String = "You ([\x21-\x3B\=\x3F-\x7E]+), ""([^']*)"""

#End Region

#Region "Dice Rolls"
    'TODO Check MS Engine Dice lines
    Public DiceSides As Double = 0.0R
    Public DiceCount As Double = 0.0R
    Public DiceCompnentMatch As String
    Public DiceModifyer As Double = 0.0R
    Public DiceResult As Double = 0.0R
    Public Const DiceFilter As String = "^<font color='roll'><img src='fsh://system.fsh:101' alt='@roll' /><channel name='@roll' /> <name shortname='([^ ]+)'>([^ ]+)</name> rolls (\d+)d(\d+)((-|\+)\d+)? ?(.*) & gets (\d+)\.</font>$"
#End Region

#Region "Popup Dialogs"
    'TODO Check Furcadoia Popup Windows
    Public Structure Rep
        Public ID As String
        Public type As Integer
    End Structure

    Public Repq As Queue(Of Rep) = New Queue(Of Rep)
#End Region

#Region "Constructors"
    Sub New(ByVal FurcHost As String, ByVal FurcPort As Integer, ByVal LocalPort As Integer, ByVal FurcClient As String)
        Try
            g_mass = 0
            If Not IsNothing(smProxy) Then smProxy.Dispose()
            smProxy = New NetProxy(FurcHost, FurcPort, LocalPort)
            With smProxy
                .ProcessCMD = cBot.IniFile
                .ProcessPath = FurcClient
                .StandAloneMode = cBot.StandAlone
                .Connect()
                FurcProcessId = .ProcID
                loggingIn = 1
                'NewLogFile = True
            End With
        Catch ex As Exception
            'Debug.WriteLine(ex.Message)
            Throw ex
        End Try
        loggingIn = 1
    End Sub

    Private Shared _FurcProcessId As Integer
    ''' <summary>
    ''' Current Process ID of the Furcadia Client we're connected too
    ''' </summary>
    ''' <returns></returns>
    Property FurcProcessId As Integer
        Get
            Return _FurcProcessId
        End Get
        Set(value As Integer)
            _FurcProcessId = value
        End Set
    End Property
#End Region

#Region "Connection Timers"

    Private NoEndurance As Boolean
    Private ThroatTired As Boolean
    Private WithEvents SubSys As New PhoenixSpeak.SubSystem
    Private ServerStack As Queue(Of String) = New Queue(Of String)(500)
    Private SpeciesTag As Queue(Of String) = New Queue(Of String)()
    Private BadgeTag As Queue(Of String) = New Queue(Of String)()
    Private LookQue As Queue(Of String) = New Queue(Of String)()

    Private Shared ReconnectTimer, ReconnectTimeOutTimer As Threading.Timer

    Private PingTimer As Threading.Timer
    Private usingPing As Integer = 0
    Private Sub PingTimerTick(ByVal state As Object)
        If (0 = Interlocked.Exchange(usingPing, 1)) Then
            If g_mass + MASS_SPEECH <= MASS_CRITICAL Then
                ServerStack.Enqueue("Ping")
            End If
            Interlocked.Exchange(usingPing, 0)
        End If
    End Sub


    Public TroatTiredProc As Threading.Timer
    Private TickTime As DateTime = DateTime.Now
    Private usingResource As Integer = 0
    Public Sub TroatTiredProcTick(ByVal state As Object)
        If (0 = Interlocked.Exchange(usingResource, 1)) Then
            Dim seconds As Double = DateTime.Now.Subtract(TickTime).Milliseconds
            on_Tick(seconds)
            'PhoenixSpeak.CheckPS_Send()
            TickTime = DateTime.Now
            Interlocked.Exchange(usingResource, 0)
        End If
    End Sub

    Private g_mass As Double = 0

    Public Const MASS_DEFAULT As Integer = 80
    Public Const MASS_SPEECH As Integer = 1000
    Public Const MASS_CRITICAL As Integer = 1600
    Public Const MASS_NOENDURANCE As Integer = 2048
    Public Const MASS_DECAYPS As Integer = 400


    ''' <summary>
    ''' Load Balancing Function
    ''' <para>this makes sure we don't over load what the server can handle</para>
    ''' <para>Bot has 2 modes of operation</para>
    ''' <para>Mode 1 Normal. Prepare for Throat Tired syndrome</para>
    ''' <para>Mode 2 NoEndurance. Send data to server as fast as it can handle with lout overloading its buffer</para>
    ''' </summary>
    ''' <param name="dt"></param>
    Public Sub on_Tick(ByVal dt As Double)
        If IsNothing(smProxy) Then Exit Sub
        If Not smProxy.IsServerConnected Then Exit Sub
        If ServerStack.Count = 0 Then Exit Sub
        If dt <> 0 Then
            dt = Math.Round(dt, 0) + 1
        End If

        '/* Send buffered speech. */
        Dim decay As Double = Math.Round(dt * MASS_DECAYPS / 1000.0F, 0)
        If (decay > g_mass) Then
            g_mass = 0
        Else
            g_mass = g_mass - decay
        End If

        If NoEndurance Then

            '/* just send everything right away */
            While ServerStack.Count > 0 And g_mass <= MASS_CRITICAL
                g_mass += ServerStack.Peek.Length + MASS_DEFAULT
                smProxy.SendServer(ServerStack.Dequeue() & vbLf)
            End While

        ElseIf Not ThroatTired Then

            ' Only send a speech line if the mass will be under the limit. */
            While ServerStack.Count > 0 And g_mass + MASS_SPEECH <= MASS_CRITICAL
                g_mass += ServerStack.Peek.Length + MASS_DEFAULT
                smProxy.SendServer(ServerStack.Dequeue() & vbLf)
            End While

        End If
    End Sub

    Private Function fIDtoFurre(ByRef ID As UInteger) As FURRE
        Dim Character As KeyValuePair(Of UInteger, FURRE)
        For Each Character In DREAM.List
            If Character.Value.ID = ID Then
                Return Character.Value
            End If
        Next
        Return Nothing
    End Function

    Public Shared Function NametoFurre(ByRef sname As String, ByRef UbdateMSVariableName As Boolean) As FURRE
        Dim p As New FURRE
        p.Name = sname
        For Each Character As KeyValuePair(Of UInteger, FURRE) In DREAM.List
            If Character.Value.ShortName = MainMSEngine.ToFurcShortName(sname) Then
                p = Character.Value
                Exit For
            End If
        Next
        If UbdateMSVariableName Then MainMSEngine.PageSetVariable(MS_Name, sname)

        Return p
    End Function

    Public Function isBot(ByRef player As FURRE) As Boolean
        Try

            Return player.ShortName <> MainMSEngine.ToFurcShortName(BotName)
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Function



    Private TroatTiredDelay As Threading.Timer
    Private Sub TroatTiredDelayTick(ByVal state As Object)
        ThroatTired = False
        TroatTiredDelay.Dispose()
    End Sub

    Private Sub ClearQues()
        If Not IsNothing(TroatTiredDelay) Then TroatTiredDelay.Dispose()
        ThroatTired = False
        ServerStack.Clear()
        SpeciesTag.Clear()
        LookQue.Clear()
        BadgeTag.Clear()
        g_mass = 0
        'PhoenixSpeak.SubSystem.Abort()


    End Sub

#End Region

#Region "Delegate"
    Public Event sndDisplay(ByVal Message As String)
    Public Event SendDisplay(ByVal Message As String, ByRef newColor As fColorEnum)
    Public Event Connecting()
    Public Event UpDateDreamList(v As String)

    'sndDisplay
#End Region

    Private Sub ProxyError(eX As Exception, o As Object, n As String) Handles smProxy.Error
        RaiseEvent sndDisplay(o.ToString + "- " + n + ": " + eX.Message)
        LogStream.Writeline(n, eX)
    End Sub

    Public Sub SendClientMessage(msg As String, data As String)
        If smProxy.IsClientConnected Then smProxy.SendClient("(" + "<b><i>[SM]</i> - " + msg + ":</b> """ + data + """" + vbLf)
        RaiseEvent sndDisplay("<b><i>[SM]</i> - " + msg + ":</b> """ + data + """")
    End Sub

    Dim clientlock As New Object
    Private Sub onClientDataReceived(ByVal data As String) Handles smProxy.ClientData2

        Try

            If (Monitor.TryEnter(clientlock)) Then
                'If data.StartsWith("desc") = True Or data.StartsWith("chdesc") = True Then
                '    data += " [<a href='http://www.ts-projects.org/smIdx.html'>SilverMonkey</a> with <a href='http://Furcadia.codeplex.com'>Furcadia Framework</a> for Third Party Programs]"
                'End If

                If data.StartsWith("quit") And cBot.StandAlone Then
                    Exit Sub

                    'Capture The Bots Name
                ElseIf data.StartsWith("connect") Then


                    Dim test As String = data.Replace("connect ", "").TrimStart(" "c)
                    BotName = test.Substring(0, test.IndexOf(" "))

                    BotName = BotName.Replace("|", " ")
                    MainText(BotName)
                    BotName = BotName.Replace("[^a-zA-Z0-9\0x0020_.| ]+", "").ToLower()
                    MainMSEngine.PageSetVariable("BOTNAME", BotName)
                ElseIf data = "vascodagama" And loggingIn = 2 Then
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
                    loggingIn = 3
                End If
                SndToServer(data)
            End If
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)

        Finally
            Monitor.Exit(clientlock)
        End Try

    End Sub

    Private Sub ClientExit() Handles smProxy.ClientExited
        If loggingIn = 0 Then Exit Sub

        TS_Status_Client.Image = My.Resources.images2
        MS_Engine.MainMSEngine.PageExecute(3)

        'loggingIn = 0
        If cBot.StandAlone = False Then

            ClientClose = True
            If Main.MainSettings.CloseProc Then
                ProcExit = False
            Else
                ProcExit = True
            End If
            DisconnectBot()
        ElseIf bConnected() Then
            smProxy.CloseClient()
            TS_Status_Client.Image = My.Resources.images2
        End If
        If MainSettings.DisconnectPopupToggle Then SetBalloonText("Furcadia Closed or disconnected")
    End Sub
    Private Sub ServerClose() Handles smProxy.ServerDisConnected
        If smProxy.IsClientConnected Then
            ProcExit = False

        End If
        DisconnectBot()
    End Sub

    Private DataReceived As Integer = 0
    Private Sub onServerDataReceived(ByVal data As String) Handles smProxy.ServerData2
        Try
            Monitor.Enter(DataReceived)
            Player.Clear()
            Channel = ""
            MainMSEngine.PageSetVariable(MS_Name, "")
            MainMSEngine.PageSetVariable("MESSAGE", "")
            Dim test As Boolean = MessagePump(data)
            ParseServerData(data, test)
        Finally
            Monitor.Exit(DataReceived)
        End Try
    End Sub

    Private Function MessagePump(ByRef Server_Instruction As String) As Boolean
        Dim objPlugin As SilverMonkey.Interfaces.msPlugin
        Dim intIndex As Integer
        Dim Handled As Boolean = False
        If Not Plugins Is Nothing Then
            For intIndex = 0 To Plugins.Count - 1
                objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(intIndex)), Interfaces.msPlugin)
                If PluginList.Item(objPlugin.Name.Replace(" ", "")) Then
                    objPlugin.Initialize(objHost)
                    objPlugin.Page = MainMSEngine.MSpage
                    If objPlugin.MessagePump(Server_Instruction) Then Handled = True
                End If
            Next
        End If
        Return Handled
    End Function

    Private Function PortOpen(ByRef port As Integer) As Boolean

        ' Evaluate current system tcp connections. This is the same information provided
        ' by the netstat command line application, just in .Net strongly-typed object
        ' form.  We will look through the list, and if our port we would like to use
        ' in our TcpClient is occupied, we will set isAvailable to false.
        Dim ipGlobalProperties__1 As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties()
        Dim tcpConnInfoArray As TcpConnectionInformation() = ipGlobalProperties__1.GetActiveTcpConnections()

        For Each tcpi As TcpConnectionInformation In tcpConnInfoArray
            If tcpi.LocalEndPoint.Port = port Then
                Return False
            End If
        Next
        Return True
        ' At this point, if isAvailable is true, we can proceed accordingly.
    End Function

    Protected Overrides Sub Finalize()
        Try
            If Not IsNothing(smProxy) Then
                smProxy.Kill()
                ClearQues()
            End If

        Catch
        End Try
        MyBase.Finalize()
    End Sub

    Public Sub OnConnected() Handles smProxy.Connected
        If Not IsNothing(ReconnectTimeOutTimer) Then ReconnectTimeOutTimer.Dispose()

    End Sub

    Private Sub FurcSettingsRestored() Handles smProxy.FurcSettingsRestored
        FurcMutex.Close()
        FurcMutex.Dispose()
    End Sub

    Private Sub ReconnectTick(ByVal state As Object)

#If DEBUG Then
        Console.WriteLine("ReconnectTick()")
#End If
        Dim Ts As TimeSpan = TimeSpan.FromSeconds(45)
        ReconnectTimeOutTimer = New Threading.Timer(AddressOf ReconnectTimeOutTick,
         Nothing, Ts, Ts)
        If Main.MainSettings.CloseProc And ProcExit = False Then
            KillProc(FurcProcessId)
        End If
        If Not IsNothing(smProxy) Then
            smProxy.Kill()
            ClearQues()
        End If
        Try
            ConnectBot()
        Catch Ex As NetProxyException
            ReconnectTimeOutTimer.Dispose()
            If Not IsNothing(PingTimer) Then PingTimer.Dispose()
            If Not IsNothing(FurcMutex) Then '
                FurcMutex.Close()
                FurcMutex.Dispose()
            End If
            DisconnectBot()
            RaiseEvent sndDisplay("Connection Aborting: " + Ex.Message)
        Finally
            ReconnectTimer.Dispose()
        End Try

    End Sub
    Private Sub ReconnectTimeOutTick(ByVal Obj As Object)

        If InvokeRequired Then
            Dim dataArray() As Object = {Obj}
            Invoke(New UpDateBtn_GoCallback3(AddressOf ReconnectTimeOutTick), dataArray)

#If DEBUG Then
            Console.WriteLine("ReconnectTimeOutTick()")
            Console.WriteLine("ReLogCounter: " + ReLogCounter.ToString)
#End If


            'DisconnectBot()
            If MainSettings.CloseProc And ProcExit = False Then
                KillProc(FurcProcessId)

            End If
            If Not IsNothing(smProxy) Then
                smProxy.Kill()
                ClearQues()
            End If

            Try
                ConnectBot()
                RaiseEvent sndDisplay("Reconnect attempt: " + ReLogCounter.ToString)
                If ReLogCounter = MainSettings.ReconnectMax Then
                    ReconnectTimeOutTimer.Dispose()
                    RaiseEvent sndDisplay("Reconnect attempts exceeded.")
                    BTN_Go.Text = "Go!"
                    TS_Status_Server.Image = My.Resources.images2
                    TS_Status_Client.Image = My.Resources.images2
                    ConnectTrayIconMenuItem.Enabled = False
                    DisconnectTrayIconMenuItem.Enabled = True
                    If Not IsNothing(FurcMutex) Then
                        FurcMutex.Close()
                        FurcMutex.Dispose()
                    End If

                End If
                ReLogCounter += 1
            Catch Ex As NetProxyException
                ReconnectTimeOutTimer.Dispose()
                If Not IsNothing(PingTimer) Then PingTimer.Dispose()
                If Not IsNothing(FurcMutex) Then '
                    FurcMutex.Close()
                    FurcMutex.Dispose()
                End If
                DisconnectBot()
                RaiseEvent sndDisplay("Connection Aborting: " + Ex.Message)

            End Try
        End If
    End Sub


    Public Sub sndServer(ByRef data As String)
        Try
            If Not bConnected() Then Exit Sub
            If data.StartsWith("`m ") Then
                ' TriggerCmd(MS0_MOVE)
                Select Case data.Substring(2, 1)
                    Case "7"
                        '     TriggerCmd(MS0_MOVENW)
                    Case "9"
                        '   TriggerCmd(MS0_MOVENE)
                    Case "1"
                        '  TriggerCmd(MS0_MOVESW)
                    Case "3"
                        '  TriggerCmd(MS0_MOVESE)
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
                MainMSEngine.PageSetVariable("BANISHNAME", BanishName)
            ElseIf data.StartsWith("banish-off ") Or data.StartsWith("tempbanish ") Then
                BanishName = data.Substring(11)
                MainMSEngine.PageSetVariable("BANISHNAME", BanishName)
            ElseIf data = "banish-list" Then
                BanishName = ""
                MainMSEngine.PageSetVariable("BANISHNAME", Nothing)
            End If

            TextToServer(data)
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub

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

    Public Sub SndToServer(ByVal data As String)
        If String.IsNullOrEmpty(data) Then Exit Sub
        Try
            ServerStack.Enqueue(data)
            If g_mass + MASS_SPEECH <= MASS_CRITICAL Then
                'g_mass = data.Length - 2
                on_Tick(0)
            End If
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me, data.ToString)
            Debug.Print("SndToServer: " & data)
            Debug.Print(eX.Message)
        End Try
    End Sub


    Private Sub ParseServerData(ByVal data As String, ByVal Handled As Boolean)

        ' page = engine.LoadFromString(cBot.MS_Script)
        If data = "Dragonroar" Then
            BotConnecting()
            '  Login Sucessful
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data = "&&&&&&&&&&&&&" Then
            loggingIn = 2
            TS_Status_Client.Image = My.Resources.images3
            ReLogCounter = 0
            '(0:1) When the bot logs into furcadia,
            MS_Engine.MainMSEngine.PageExecute(1)
            If Not IsNothing(ReconnectTimeOutTimer) Then ReconnectTimeOutTimer.Dispose()
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            ' Species Tags
        ElseIf data.StartsWith("]-") Then
            If data.StartsWith("]-#A") Then
                SpeciesTag.Enqueue(data.Substring(4))
            ElseIf data.StartsWith("]-#B") Then
                BadgeTag.Enqueue(data.Substring(2))
            End If

            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'DS Variables

            'Popup Dialogs!
        ElseIf data.StartsWith("]#") Then
            ']#<idstring> <style 0-17> <message that might have spaces in>
            Dim repqq As Regex = New Regex("^\]#(.*?) (\d+) (.*?)$")
            Dim m As System.Text.RegularExpressions.Match = repqq.Match(data)
            Dim r As Rep
            r.ID = m.Groups(1).Value
            Dim num As Integer = 0
            Integer.TryParse(m.Groups(2).Value, r.type)
            Repq.Enqueue(r)
            MainMSEngine.PageSetVariable("MESSAGE", m.Groups(3).Value, True)
            Player.Message = m.Groups(3).Value
            MS_Engine.MainMSEngine.PageExecute(95, 96)
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("0") Then
            InDream = True
            'Phoenix Speak event
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("3") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'self Induced Dragon Speak Event
        ElseIf data.StartsWith("6") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Dragon Speak event
        ElseIf data.StartsWith("7") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Dragon Speak Addon (Follows Instructions 6 and 7
        ElseIf data.StartsWith("8") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            ']s(.+)1 (.*?) (.*?) 0
        ElseIf data.StartsWith("]s") Then
            Dim t As New Regex("\]s(.+)1 (.*?) (.*?) 0", RegexOptions.IgnoreCase)
            Dim m As System.Text.RegularExpressions.Match = t.Match(data)
            If MainMSEngine.ToFurcShortName(BotName) = MainMSEngine.ToFurcShortName(m.Groups(2).Value) Then

                MS_Engine.MainMSEngine.PageExecute()
            End If
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
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
                    Player = NametoFurre(data.Remove(0, length + 2), True)
                    ' If Player.ID = 0 Then Exit Sub
                    Player.Color = data.Substring(2, length)
                    If isBot(Player) Then Look = False
                    If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try

            End If
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Spawn Avatar
        ElseIf data.StartsWith("<") And bConnected() Then
            Try
                If data.Length < 29 Then Exit Sub
                ' Debug.Print(data)
                Player.ID = ConvertFromBase220(data.Substring(1, 4))

                If DREAM.List.ContainsKey(Player.ID) Then
                    Player = DREAM.List.Item(Player.ID)
                End If


                Player.X = CUInt(ConvertFromBase220(data.Substring(5, 2)) * 2)
                Player.Y = ConvertFromBase220(data.Substring(7, 2))
                Player.Shape = ConvertFromBase220(data.Substring(9, 2))



                Dim NameLength As UInteger = ConvertFromBase220(data.Substring(11, 1))
                Player.Name = data.Substring(12, CInt(NameLength)).Replace("|", " ")

                Dim ColTypePos As UInteger = CUInt(12 + NameLength)
                Player.ColorType = CChar(data.Substring(CInt(ColTypePos), 1))
                Dim ColorSize As UInteger = 10
                'If Player.ColorType <> "t" Then
                '    ColorSize = 30
                'End If
                Dim sColorPos As Integer = CInt(ColTypePos + 1)

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
                MainMSEngine.PageSetVariable(MS_Name, Player.ShortName)

                If Player.Flag = 4 Or Not DREAM.List.ContainsKey(Player.ID) Then
                    DREAM.List.Add(Player.ID, Player)
                    If InDream Then RaiseEvent UpDateDreamList(Player.Name)
                    If Player.Flag = 2 Then
                        Dim Bot As FURRE = NametoFurre(BotName, False)
                        Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(CInt(Bot.X), CInt(Bot.Y))
                        If VisableRectangle.X <= Player.X And VisableRectangle.Y <= Player.Y And VisableRectangle.height >= Player.Y And VisableRectangle.length >= Player.X Then
                            Player.Visible = True
                        Else
                            Player.Visible = False
                        End If
                        MS_Engine.MainMSEngine.PageExecute(28, 29, 24, 25)
                    Else
                        MS_Engine.MainMSEngine.PageExecute(24, 25)
                    End If
                ElseIf Player.Flag = 2 Then
                    Dim Bot As FURRE = NametoFurre(BotName, False)
                    Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(CInt(Bot.X), CInt(Bot.Y))
                    If VisableRectangle.X <= Player.X And VisableRectangle.Y <= Player.Y And VisableRectangle.height >= Player.Y And VisableRectangle.length >= Player.X Then
                        Player.Visible = True
                    Else
                        Player.Visible = False
                    End If
                    MS_Engine.MainMSEngine.PageExecute(28, 29)

                ElseIf Player.Flag = 1 Then

                ElseIf Player.Flag = 0 Then

                End If
                If DREAM.List.ContainsKey(Player.ID) Then
                    DREAM.List.Item(Player.ID) = Player
                End If
            Catch eX As Exception

                Dim logError As New ErrorLogging(eX, Me)
                Exit Sub
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Remove Furre
        ElseIf data.StartsWith(")") And bConnected() Then 'And loggingIn = False
            Try
                Dim remID As UInteger = ConvertFromBase220(data.Substring(1, 4))
                ' remove departure from List
                If DREAM.List.ContainsKey(remID) = True Then
                    Player = DREAM.List.Item(remID)
                    MainMSEngine.PageSetVariable(MS_Name, Player.Name)
                    MS_Engine.MainMSEngine.PageExecute(26, 27, 30, 31)
                    DREAM.List.Remove(remID)
                    RaiseEvent UpDateDreamList("")
                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Animated Move
        ElseIf data.StartsWith("/") And bConnected() Then 'And loggingIn = False
            Try
                Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)))
                Player.X = CUInt(ConvertFromBase220(data.Substring(5, 2)) * 2)
                Player.Y = ConvertFromBase220(data.Substring(7, 2))
                Player.Shape = ConvertFromBase220(data.Substring(9, 2))
                Dim Bot As FURRE = fIDtoFurre((BotUID))
                Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(CInt(Bot.X), CInt(Bot.Y))
                If VisableRectangle.X <= Player.X And VisableRectangle.Y <= Player.Y And VisableRectangle.height >= Player.Y And VisableRectangle.length >= Player.X Then
                    Player.Visible = True
                Else
                    Player.Visible = False
                End If
                If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                isBot(Player)
                MainMSEngine.PageSetVariable(MS_Name, Player.ShortName)
                MS_Engine.MainMSEngine.PageExecute(28, 29, 30, 31, 601, 602)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            ' Move Avatar
        ElseIf data.StartsWith("A") And bConnected() Then 'And loggingIn = False
            Try
                Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)))
                Player.X = CUInt(ConvertFromBase220(data.Substring(5, 2)) * 2)
                Player.Y = ConvertFromBase220(data.Substring(7, 2))
                Player.Shape = ConvertFromBase220(data.Substring(9, 2))

                Dim Bot As FURRE = fIDtoFurre((BotUID))
                Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(CInt(Bot.X), CInt(Bot.Y))
                If VisableRectangle.X <= Player.X And VisableRectangle.Y <= Player.Y And VisableRectangle.height >= Player.Y And VisableRectangle.length >= Player.X Then

                    Player.Visible = True
                Else
                    Player.Visible = False
                End If
                If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player

                isBot(Player)
                MainMSEngine.PageSetVariable(MS_Name, Player.ShortName)
                MS_Engine.MainMSEngine.PageExecute(28, 29, 30, 31, 601, 602)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            ' Update Color Code
        ElseIf data.StartsWith("B") <> False And bConnected() And InDream Then 'And loggingIn = False
            Try
                Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)))
                Player.Shape = ConvertFromBase220(data.Substring(5, 2))
                Dim ColTypePos As UInteger = 7
                Player.ColorType = CChar(data.Substring(CInt(ColTypePos), 1))
                Dim ColorSize As UInteger = 10

                Dim sColorPos As UInteger = CUInt(ColTypePos + 1)
                Player.Color = data.Substring(CInt(sColorPos), CInt(ColorSize))
                If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                isBot(Player)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Hide Avatar
        ElseIf data.StartsWith("C") <> False And bConnected() Then 'And loggingIn = False
            Try
                Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)))
                Player.X = CUInt(ConvertFromBase220(data.Substring(5, 2)) * 2)
                Player.Y = ConvertFromBase220(data.Substring(7, 2))
                Player.Visible = False
                If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                isBot(Player)
                MainMSEngine.PageSetVariable(MS_Name, Player.Name)
                MS_Engine.MainMSEngine.PageExecute(30, 31)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Display Disconnection Dialog
        ElseIf data.StartsWith("[") Then
#If DEBUG Then
            Console.WriteLine("Disconnection Dialog:" & data)
#End If
            InDream = False
            DREAM.List.Clear()
            RaiseEvent UpDateDreamList("")

            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            MsgBox(data, MsgBoxStyle.Critical, "Disconnection Error")

            Exit Sub


            ';{mapfile}	Load a local map (one in the furcadia folder)
            ']q {name} {id}	Request to download a specific patch
        ElseIf data.StartsWith(";") OrElse data.StartsWith("]q") OrElse data.StartsWith("]r") Then
            Try
#If DEBUG Then
                Debug.Print("Entering new Dream" & data)
#End If
                MainMSEngine.PageSetVariable("DREAMOWNER", "")
                MainMSEngine.PageSetVariable("DREAMNAME", "")
                HasShare = False
                NoEndurance = False

                DREAM.List.Clear()
                RaiseEvent UpDateDreamList("")
                InDream = False
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("]z") Then
            BotUID = UInteger.Parse(data.Remove(0, 2))
            'Snag out UID
        ElseIf data.StartsWith("]B") Then
            BotUID = UInteger.Parse(data.Substring(2, data.Length - BotName.Length - 3))
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("~") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("=") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("]c") Then
#If DEBUG Then
            Console.WriteLine(data)
#End If
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("]C") Then
            If data.StartsWith("]C0") Then
                Dim dname As String = data.Substring(10)
                If dname.Contains(":") Then
                    Dim NameStr As String = dname.Substring(0, dname.IndexOf(":"))
                    If MainMSEngine.ToFurcShortName(NameStr) = MainMSEngine.ToFurcShortName(BotName) Then
                        HasShare = True
                    End If
                    MainMSEngine.PageSetVariable(VarPrefix & "DREAMOWNER", NameStr)
                ElseIf dname.EndsWith("/") AndAlso Not dname.Contains(":") Then
                    Dim NameStr As String = dname.Substring(0, dname.IndexOf("/"))
                    If MainMSEngine.ToFurcShortName(NameStr) = MainMSEngine.ToFurcShortName(BotName) Then
                        HasShare = True
                    End If
                    MainMSEngine.PageSetVariable("DREAMOWNER", NameStr)
                End If

                MainMSEngine.PageSetVariable("DREAMNAME", dname)
                MS_Engine.MainMSEngine.PageExecute(90, 91)
            End If
#If DEBUG Then
            Console.WriteLine(data)
#End If
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Process Channels Seperatly
        ElseIf data.StartsWith("(") Then
            If ThroatTired = False And data.StartsWith("(<font color='warning'>Your throat is tired. Try again in a few seconds.</font>") Then
                ThroatTired = True

                'Throat Tired Syndrome, Halt all out going data for a few seconds
                Dim Ts As TimeSpan = TimeSpan.FromSeconds(MainSettings.TT_TimeOut)
                TroatTiredDelay = New Threading.Timer(AddressOf TroatTiredDelayTick,
                   Nothing, Ts, Ts)
                '(0:92) When the bot detects the "Your throat is tired. Please wait a few seconds" message,
                MS_Engine.MainMSEngine.PageExecute(92)
                'Exit Sub
                If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
                Exit Sub
            End If

            ChannelProcess(data, Handled)
            Exit Sub
        End If

        If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)

    End Sub



    Dim ChannelLock As New Object
    Public Shared Channel As String
    ''' <summary>
    ''' Channel Parser
    ''' RegEx Style Processing here
    ''' </summary>
    ''' <param name="data"></param>
    ''' <remarks></remarks>
    Public Sub ChannelProcess(ByRef data As String, ByVal Handled As Boolean)
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
                If User <> "" Then Player = NametoFurre(User, True)
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

            ErrorMsg = ""
            ErrorNum = 0

            If Channel = "@news" Or Channel = "@spice" Then
                Try
                    RaiseEvent sndDisplay(Text)
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
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
                        MainMSEngine.PageSetVariable("BANISHNAME", BanishName)

                        BanishString.Add(BanishName)
                        MainMSEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                        MS_Engine.MainMSEngine.PageExecute(52, 53)

                        ' MainMSEngine.PageExecute(53)
                    ElseIf Text = "You have canceled all banishments from your dreams." Then
                        'banish-off-all (active list)
                        'Success: You have canceled all banishments from your dreams. 

                        '(0:60) When the bot successfully clears the banish list
                        BanishString.Clear()
                        MainMSEngine.PageSetVariable("BANISHLIST", Nothing)
                        MainMSEngine.PageSetVariable("BANISHNAME", Nothing)
                        MS_Engine.MainMSEngine.PageExecute(60)

                    ElseIf Text.EndsWith(" has been temporarily banished from your dreams.") Then
                        'tempbanish <name> (online)
                        'Success: (.*?) has been temporarily banished from your dreams. 

                        '(0:61) When the bot sucessfully temp banishes a Furre
                        '(0:62) When the bot sucessfully temp banishes the furre named {...}
                        Dim t As New Regex("(.*?) has been temporarily banished from your dreams.")
                        BanishName = t.Match(Text).Groups(1).Value
                        MainMSEngine.PageSetVariable(VarPrefix & "BANISHNAME", BanishName)
                        '  MainMSEngine.PageExecute(61)
                        BanishString.Add(BanishName)
                        MainMSEngine.PageSetVariable(VarPrefix & "BANISHLIST", String.Join(" ", BanishString.ToArray))
                        MS_Engine.MainMSEngine.PageExecute(61, 62)

                    ElseIf Text = "Control of this dream is now being shared with you." Then
                        HasShare = True

                    ElseIf Text.EndsWith("is now sharing control of this dream with you.") Then
                        HasShare = True

                    ElseIf Text.EndsWith("has stopped sharing control of this dream with you.") Then
                        HasShare = False

                    ElseIf Text.StartsWith("The endurance limits of player ") Then
                        Dim t As New Regex("The endurance limits of player (.*?) are now toggled off.")
                        Dim m As String = t.Match(Text).Groups(1).Value.ToString
                        If MainMSEngine.ToFurcShortName(m) = MainMSEngine.ToFurcShortName(BotName) Then
                            NoEndurance = True
                        End If

                    ElseIf Channel = "@cookie" Then
                        '(0:96) When the Bot sees "Your cookies are ready."
                        Dim CookiesReady As Regex = New Regex(String.Format("{0}", "Your cookies are ready.  http://furcadia.com/cookies/ for more info!"))
                        If CookiesReady.Match(data).Success Then
                            MS_Engine.MainMSEngine.PageExecute(96)
                        End If
                    End If
                    RaiseEvent sndDisplay(Text)
                    If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
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
                MainMSEngine.PageSetVariable(VarPrefix & "MESSAGE", DiceMatch.Groups(7).Value)
                Double.TryParse(DiceMatch.Groups(4).Value, DiceSides)
                Double.TryParse(DiceMatch.Groups(3).Value, DiceCount)
                DiceCompnentMatch = DiceMatch.Groups(6).Value
                DiceModifyer = 0.0R
                Double.TryParse(DiceMatch.Groups(5).Value, DiceModifyer)
                Double.TryParse(DiceMatch.Groups(8).Value, DiceResult)

                If isBot(Player) Then
                    MS_Engine.MainMSEngine.PageExecute(130, 131, 132, 136)
                Else
                    MS_Engine.MainMSEngine.PageExecute(133, 134, 135, 136)
                End If

                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub
            ElseIf Channel = "@dragonspeak" OrElse Channel = "@emit" OrElse Color = "emit" Then
                Try
                    '(<font color='dragonspeak'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Furcadian Academy</font>
                    RaiseEvent SendDisplay(Text, fColorEnum.Emit)

                    MainMSEngine.PageSetVariable(VarPrefix & "MESSAGE", Text.Substring(5))
                    ' Execute (0:21) When someone emits something
                    MS_Engine.MainMSEngine.PageExecute(21, 22, 23)
                    ' Execute (0:22) When someone emits {...}
                    '' Execute (0:23) When someone emits something with {...} in it

                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
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
                            RaiseEvent sndDisplay(Text)
                        Case "@bcast"
                            If MainSettings.Broadcast Then Exit Sub
                            Dim u As String = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups(2).Value
                            RaiseEvent sndDisplay(Text)
                        Case "@announcements"
                            If MainSettings.Announcement Then Exit Sub
                            Dim u As String = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups(2).Value
                            RaiseEvent sndDisplay(Text)
                        Case Else
#If DEBUG Then
                            Console.WriteLine("Unknown ")
                            Console.WriteLine("BCAST:" & data)
#End If
                    End Select


                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub
                ''SAY
            ElseIf Color = "myspeech" Then
                Try
                    Dim t As New Regex(YouSayFilter)
                    Dim u As String = t.Match(data).Groups(1).ToString
                    Text = t.Match(data).Groups(2).ToString
                    If SpeciesTag.Count() > 0 Then
                        SpecTag = SpeciesTag.Peek
                        SpeciesTag.Dequeue()
                        Player.Color = SpecTag
                        If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                    End If

                    RaiseEvent SendDisplay("You " & u & ", """ & Text & """", fColorEnum.Say)
                    Player.Message = Text
                    MainMSEngine.PageSetVariable(VarPrefix & "MESSAGE", Text)
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
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
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
                            If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                        End If
                        Channel = "say"
                        RaiseEvent SendDisplay(User & " says, """ & Text & """", fColorEnum.Say)
                        MainMSEngine.PageSetVariable(MS_Name, User)
                        MainMSEngine.PageSetVariable("MESSAGE", Text)
                        Player.Message = Text
                        ' Execute (0:5) When some one says something
                        MS_Engine.MainMSEngine.PageExecute(5, 6, 7, 18, 19, 20)
                        ' Execute (0:6) When some one says {...}
                        ' Execute (0:7) When some one says something with {...} in it
                        ' Execute (0:18) When someone says or emotes something
                        ' Execute (0:19) When someone says or emotes {...}
                        ' Execute (0:20) When someone says or emotes something with {...} in it

                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)

                    End Try

                    If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
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
                    If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                    MainMSEngine.PageSetVariable(MS_Name, DescName)
                    MS_Engine.MainMSEngine.PageExecute(600)
                    'sndDisplay)
                    If Player.Tag = "" Then
                        RaiseEvent sndDisplay("You See '" & Player.Name & "'\par" & Desc)
                    Else
                        RaiseEvent sndDisplay("You See '" & Player.Name & "'\par" & Player.Tag & " " & Desc)
                    End If
                    Look = False
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub
            ElseIf Color = "shout" Then
                ''SHOUT
                Try
                    Dim t As New Regex(YouSayFilter)
                    Dim u As String = t.Match(data).Groups(1).ToString
                    Text = t.Match(data).Groups(2).ToString
                    If User = "" Then
                        RaiseEvent SendDisplay("You " & u & ", """ & Text & """", fColorEnum.Shout)
                    Else
                        Text = Regex.Match(data, "shouts: (.*)</font>").Groups(1).ToString()
                        RaiseEvent SendDisplay(User & " shouts, """ & Text & """", fColorEnum.Shout)
                    End If
                    If Not isBot(Player) Then
                        MainMSEngine.PageSetVariable(VarPrefix & "MESSAGE", Text)
                        Player.Message = Text
                        ' Execute (0:8) When some one shouts something
                        MS_Engine.MainMSEngine.PageExecute(8, 9, 10)
                        ' Execute (0:9) When some one shouts {...}
                        ' Execute (0:10) When some one shouts something with {...} in it


                    End If
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub
            ElseIf Color = "query" Then
                Dim QCMD As String = Regex.Match(data, "<a.*?href='command://(.*?)'>").Groups(1).ToString
                'Player = NametoFurre(User, True)
                Select Case QCMD
                    Case "summon"
                        ''JOIN
                        Try
                            RaiseEvent sndDisplay(User & " requests to join you.")
                            'If Not IsBot(Player) Then
                            MS_Engine.MainMSEngine.PageExecute(34, 35)
                            'End If
                        Catch eX As Exception
                            Dim logError As New ErrorLogging(eX, Me)
                        End Try
                    Case "join"
                        ''SUMMON
                        Try
                            RaiseEvent sndDisplay(User & " requests to summon you.")
                            'If Not IsBot(Player) Then
                            MS_Engine.MainMSEngine.PageExecute(32, 33)
                            'End If
                        Catch eX As Exception
                            Dim logError As New ErrorLogging(eX, Me)
                        End Try
                    Case "follow"
                        ''LEAD
                        Try
                            RaiseEvent sndDisplay(User & " requests to lead.")
                            'If Not IsBot(Player) Then
                            MS_Engine.MainMSEngine.PageExecute(36, 37)
                            'End If
                        Catch eX As Exception
                            Dim logError As New ErrorLogging(eX, Me)
                        End Try
                    Case "lead"
                        ''FOLLOW
                        Try
                            RaiseEvent sndDisplay(User & " requests the bot to follow.")
                            'If Not IsBot(Player) Then
                            MS_Engine.MainMSEngine.PageExecute(38, 39)
                            'End If
                        Catch eX As Exception
                            Dim logError As New ErrorLogging(eX, Me)
                        End Try
                    Case "cuddle"
                        Try
                            RaiseEvent sndDisplay(User & " requests the bot to cuddle.")
                            'If Not IsBot(Player) Then
                            MS_Engine.MainMSEngine.PageExecute(40, 41)
                            'End If
                        Catch eX As Exception
                            Dim logError As New ErrorLogging(eX, Me)
                        End Try
                    Case Else
                        RaiseEvent sndDisplay("## Unknown " & Channel & "## " & data)
                End Select

                'NameFilter

                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
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

                        If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player


                        RaiseEvent SendDisplay(User & " whispers""" & WhisperFrom & """ to you.", fColorEnum.Whisper)
                        If Not isBot(Player) Then
                            MainMSEngine.PageSetVariable(VarPrefix & "MESSAGE", Player.Message)
                            ' Execute (0:15) When some one whispers something
                            MS_Engine.MainMSEngine.PageExecute(15, 16, 17)
                            ' Execute (0:16) When some one whispers {...}
                            ' Execute (0:17) When some one whispers something with {...} in it
                        End If


                    Else
                        WhisperTo = WhisperTo.Replace("<wnd>", "")
                        RaiseEvent SendDisplay("You whisper""" & WhisperTo & """ to " & User & ".", fColorEnum.Whisper)
                    End If
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub
            ElseIf Color = "warning" Then

                ErrorMsg = Text
                ErrorNum = 1
                MS_Engine.MainMSEngine.PageExecute(801)
                RaiseEvent SendDisplay("::WARNING:: " & Text, fColorEnum.DefaultColor)
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub
            ElseIf Color = "trade" Then
                Dim TextStr As String = Regex.Match(data, "\s<name (.*?)</name>").Groups(0).ToString()
                Text = Text.Substring(6)
                If User <> "" Then Text = " " & User & Text.Replace(TextStr, "")
                RaiseEvent SendDisplay(Text, fColorEnum.DefaultColor)
                MainMSEngine.PageSetVariable(VarPrefix & "MESSAGE", Text)
                Player.Message = Text
                MS_Engine.MainMSEngine.PageExecute(46, 47, 48)
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
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

                    Player = NametoFurre(n.Groups(3).Value, True)
                    MainMSEngine.PageSetVariable(VarPrefix & "MESSAGE", Text)
                    Player.Message = Text
                    If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                    RaiseEvent SendDisplay(User & " " & Text, fColorEnum.Emote)
                    Dim test As Boolean = isBot(Player)
                    If isBot(Player) = False Then

                        ' Execute (0:11) When someone emotes something
                        MS_Engine.MainMSEngine.PageExecute(11, 12, 13, 18, 19, 20)
                        ' Execute (0:12) When someone emotes {...}
                        ' Execute (0:13) When someone emotes something with {...} in it
                        ' Execute (0:18) When someone says or emotes something
                        ' Execute (0:19) When someone says or emotes {...}
                        ' Execute (0:20) When someone says or emotes something with {...} in it
                    End If
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
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
                RaiseEvent sndDisplay("[" + ChanMatch.Groups(1).Value + "] " + User & ": " & Text)
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
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
                    MainMSEngine.PageSetVariable(VarPrefix & "BANISHLIST", String.Join(" ", BanishString.ToArray))
                    MS_Engine.MainMSEngine.PageExecute(54)

                ElseIf Text.StartsWith("The banishment of player ") Then
                    'banish-off <name> (on list)
                    '[notify> The banishment of player (.*?) has ended.  

                    '(0:56) When the bot successfully removes a furre from the banish list,
                    '(0:58) When the bot successfully removes the furre named {...} from the banish list,
                    Dim t As New Regex("The banishment of player (.*?) has ended.")
                    NameStr = t.Match(data).Groups(1).Value
                    MainMSEngine.PageSetVariable("BANISHNAME", NameStr)
                    MS_Engine.MainMSEngine.PageExecute(56, 56)
                    For I As Integer = 0 To BanishString.Count - 1
                        If MainMSEngine.ToFurcShortName(BanishString.Item(I).ToString) = MainMSEngine.ToFurcShortName(NameStr) Then
                            BanishString.RemoveAt(I)
                            Exit For
                        End If
                    Next
                    MainMSEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                End If

                RaiseEvent SendDisplay("[notify> " & Text, fColorEnum.DefaultColor)
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub
            ElseIf Color = "error" Then

                ErrorMsg = Text
                ErrorNum = 2

                MS_Engine.MainMSEngine.PageExecute(800)
                Dim NameStr As String = ""
                If Text.Contains("There are no furres around right now with a name starting with ") Then
                    'Banish <name> (Not online)
                    'Error:>>  There are no furres around right now with a name starting with (.*?) . 

                    '(0:50) When the Bot fails to banish a furre,
                    '(0:51) When the bot fails to banish the furre named {...},
                    Dim t As New Regex("There are no furres around right now with a name starting with (.*?) .")
                    NameStr = t.Match(data).Groups(1).Value
                    MainMSEngine.PageSetVariable("BANISHNAME", NameStr)
                    MS_Engine.MainMSEngine.PageExecute(50, 51)
                    MainMSEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                ElseIf Text = "Sorry, this player has not been banished from your dreams." Then
                    'banish-off <name> (not on list)
                    'Error:>> Sorry, this player has not been banished from your dreams.

                    '(0:55) When the Bot fails to remove a furre from the banish list,
                    '(0:56) When the bot fails to remove the furre named {...} from the banish list,
                    NameStr = BanishName
                    MainMSEngine.PageSetVariable("BANISHNAME", NameStr)
                    MainMSEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                    MS_Engine.MainMSEngine.PageExecute(50, 51)
                ElseIf Text = "You have not banished anyone." Then
                    'banish-off-all (empty List)
                    'Error:>> You have not banished anyone. 

                    '(0:59) When the bot fails to see the banish list,
                    BanishString.Clear()
                    MS_Engine.MainMSEngine.PageExecute(59)
                    MainMSEngine.PageSetVariable(VarPrefix & "BANISHLIST", Nothing)
                ElseIf Text = "You do not have any cookies to give away right now!" Then
                    MS_Engine.MainMSEngine.PageExecute(95)
                End If

                RaiseEvent sndDisplay("Error:>> " & Text)
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub
            ElseIf data.StartsWith("Communication") Then
                RaiseEvent sndDisplay("Error: Communication Error.  Aborting connection.")
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
                    MainMSEngine.PageSetVariable(MS_Name, CookieToMe.Match(data).Groups(2).Value)
                    MS_Engine.MainMSEngine.PageExecute(42, 43)
                End If
                Dim CookieToAnyone As Regex = New Regex(String.Format("<name shortname='(.*?)'>(.*?)</name> just gave <name shortname='(.*?)'>(.*?)</name> a (.*?)"))
                If CookieToAnyone.Match(data).Success Then
                    'MainMSEngine.PageSetVariable(VarPrefix & MS_Name, CookieToAnyone.Match(data).Groups(3).Value)
                    If callbk.IsBot(NametoFurre(CookieToAnyone.Match(data).Groups(3).Value, True)) Then
                        MS_Engine.MainMSEngine.PageExecute(42, 43)
                    Else
                        MS_Engine.MainMSEngine.PageExecute(44)
                    End If


                End If
                Dim CookieFail As Regex = New Regex(String.Format("You do not have any (.*?) left!"))
                If CookieFail.Match(data).Success Then
                    MS_Engine.MainMSEngine.PageExecute(45)
                End If
                Dim EatCookie As Regex = New Regex(Regex.Escape("<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.") + "(.*?)")
                If EatCookie.Match(data).Success Then
                    MainMSEngine.PageSetVariable(VarPrefix & "MESSAGE", "You eat a cookie." + EatCookie.Replace(data, ""))
                    Player.Message = "You eat a cookie." + EatCookie.Replace(data, "")
                    MS_Engine.MainMSEngine.PageExecute(49)

                End If
                RaiseEvent SendDisplay(Text, fColorEnum.DefaultColor)
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub
            ElseIf data.StartsWith("PS") Then
                Color = "PhoenixSpeak"
                SubSys.ProcessServerPS(data)
                If MainSettings.PSShowMainWindow Then
                    RaiseEvent sndDisplay(data)
                End If
                If MainSettings.PSShowClient Then
                    If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                End If
                Exit Sub
            ElseIf data.StartsWith("(You enter the dream of") Then
                MainMSEngine.PageSetVariable("DREAMNAME", Nothing)
                MainMSEngine.PageSetVariable("DREAMOWNER", data.Substring(24, data.Length - 2 - 24))
                MS_Engine.MainMSEngine.PageExecute(90, 91)
                RaiseEvent sndDisplay(data)
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub


            Else
                RaiseEvent sndDisplay(data)

                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub
            End If
            ' If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            ' Exit Sub
        End SyncLock
    End Sub


    Dim disposed As Boolean = False
    ' Instantiate a SafeHandle instance.
    Dim handle As SafeHandle = New SafeFileHandle(IntPtr.Zero, True)

    ' Public implementation of Dispose pattern callable by consumers.
    Public Sub Dispose() _
              Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


#Region "Dispose"
    ' Protected implementation of Dispose pattern.
    Protected Overridable Sub Dispose(disposing As Boolean)
        If disposed Then Return

        If disposing Then
            handle.Dispose()
            ' Free any other managed objects here.
            If Not IsNothing(TroatTiredDelay) Then TroatTiredDelay.Dispose()
            If Not IsNothing(TroatTiredProc) Then TroatTiredProc.Dispose()
            If Not IsNothing(ReconnectTimeOutTimer) Then ReconnectTimeOutTimer.Dispose()
            If Not IsNothing(PingTimer) Then PingTimer.Dispose()

            '
        End If

        ' Free any unmanaged objects here.
        '
        disposed = True
    End Sub

    'Dim Ts As TimeSpan = TimeSpan.FromSeconds(MainSettings.ConnectTimeOut)
    '        ReconnectTimeOutTimer = New Threading.Timer(AddressOf ReconnectTimeOutTick,
    '         Nothing, Ts, Ts)
    '        Dim Tss As TimeSpan = TimeSpan.FromSeconds(MainSettings.Ping)
    'If MainSettings.Ping > 0 Then PingTimer = New Threading.Timer(AddressOf PingTimerTick,
    '         Nothing, Tss, Tss)


#End Region



    Public Sub KillProc(ByRef ID As Integer)
        For Each p As Process In Process.GetProcesses
            If p.Id = ID And p.Id <> 0 Then
                p.Kill()
                Exit Sub
            End If
        Next
    End Sub
End Class
