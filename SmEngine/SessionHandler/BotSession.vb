'Furcadia Servver Parser
'Event/Delegates for server instructions
'call subsystem processor

'dream info
'Furre info
'Bot info

'Furre Update events?

Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports Furcadia.Net.Proxy
Imports Furcadia.Text.FurcadiaMarkup
Imports Microsoft.Win32.SafeHandles
Imports MonkeyCore
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

    Private handle As SafeHandle = New SafeFileHandle(IntPtr.Zero, True)

#Region "Constructors"

    ''' <summary>
    ''' </summary>
    Sub New()
        MyBase.New()
        MainEngineOptions = New BotOptions()
        Initiaize()
    End Sub

    ''' <summary>
    ''' New BotSession with Proxy Settings
    ''' </summary>
    ''' <param name="BotSessionOptions">
    ''' </param>
    Sub New(ByRef BotSessionOptions As BotOptions)
        MyBase.New(BotSessionOptions)
        MainEngineOptions = BotSessionOptions
        Initiaize()
    End Sub

    Private Sub Initiaize()
        MainEngine = New MainEngine(MainEngineOptions.MonkeySpeakEngineOptions, Me)
        MSpage = MainEngine.LoadFromScriptFile(MainEngineOptions.MonkeySpeakEngineOptions.MonkeySpeakScriptFile)
    End Sub

#End Region

#Region "Public Properties"

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
    ''' Name of the controller furre
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property BotController As String
        Get
            Return MainEngineOptions.BotController
        End Get
    End Property

#End Region

#Region "Private Fields"

    Private MainSettings As Settings.cMain
    Private MainEngineOptions As BotOptions
#End Region

#Region "Public Fields"

    ''' <summary>
    ''' Main MonkeySpeak Engine
    ''' </summary>
    Public WithEvents MainEngine As Engine.MainEngine

    Public WithEvents MSpage As Monkeyspeak.Page = Nothing

    'Monkey Speak Bot specific Variables

    Public objHost As New smHost(Me)


#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Starts the Furcadia Connection Process
    ''' </summary>
    Public Overrides Sub Connect()

        Try

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

        ' Channel = Regex.Match(data, ChannelNameFilter).Groups(1).Value
        Dim Color As String = Regex.Match(data, EntryFilter).Groups(1).Value
        Dim Desc As String = ""
        Dim Text As String = ""

        MSpage.SetVariable(MS_Name, Player.Name, True)
        MSpage.SetVariable("MESSAGE", Player.Message, True)

        If Not String.IsNullOrEmpty(Desc) Then

            Dim DescName As String = Regex.Match(data, DescFilter).Groups(1).ToString()

            MSpage.SetVariable(MS_Name, DescName, True)
            MSpage.Execute(600)

            'NameFilter

        ElseIf Color = "warning" Then

            MSpage.Execute(801)

        ElseIf Color = "channel" Then
            'ChannelNameFilter2

            Dim r As New Regex("<img src='(.*?)' alt='(.*?)' />")
            Dim ss As RegularExpressions.Match = r.Match(Text)
            If ss.Success Then Text = Text.Replace(ss.Groups(0).Value, "")
            r = New Regex(NameFilter + ":")
            ss = r.Match(Text)
            If ss.Success Then Text = Text.Replace(ss.Groups(0).Value, "")

            'ElseIf Color = "notify" Then

        End If

    End Sub

    '''' <summary>
    '''' Parse Server Data
    '''' <para>
    '''' TODO: Move this functionality to <see cref="Furcadia.Net"/>
    '''' </para>
    '''' </summary>
    '''' <param name="data">
    '''' raw server instruction
    '''' </param>
    '''' <param name="Handled">
    '''' has this instruction been handled elsewhere
    '''' </param>
    'Public Overrides Sub ParseServerData(ByVal data As String, ByVal Handled As Boolean)
    '    MyBase.ParseServerData(data, Handled)
    '    MSpage.SetVariable(MS_Name, Player.ShortName, True)
    '    MSpage.SetVariable("MESSAGE", Player.Message, True)

    '    If data = "&&&&&&&&&&&&&" Then
    '        'We've connected to Furcadia
    '        'Stop the reconnection manager
    '        '(0:1) When the bot logs into furcadia,
    '        MSpage.SetVariable("BOTNAME", ConnectedFurre.ShortName, True)
    '        MSpage.Execute(1)

    '        ' Species Tags
    '    ElseIf data.StartsWith("]-") Then

    '        'DS Variables

    '        'Popup Dialogs!
    '    ElseIf data.StartsWith("]#") Then
    '        ']#<idstring> <style 0-17> <message that might have spaces in>
    '        Dim repqq As Regex = New Regex("^\]#(.*?) (\d+) (.*?)$")
    '        Dim m As Match = repqq.Match(data)
    '        Dim r As Rep
    '        r.ID = m.Groups(1).Value
    '        Dim num As Integer = 0
    '        Integer.TryParse(m.Groups(2).Value, r.Type)
    '        Repq.Enqueue(r)
    '        MSpage.SetVariable("MESSAGE", Player.Message, True)
    '        MSpage.Execute(95, 96)

    '        ']s(.+)1 (.*?) (.*?) 0
    '        'Display Dream Info
    '        'Portal  Names until a ]t
    '    ElseIf data.StartsWith("]s") Then
    '        Dim t As New Regex("\]s(.+)1 (.*?) (.*?) 0", RegexOptions.IgnoreCase)
    '        Dim m As System.Text.RegularExpressions.Match = t.Match(data)
    '        If Furcadia.Util.FurcadiaShortName(ConnectedCharacterName) = Furcadia.Util.FurcadiaShortName(m.Groups(2).Value) Then
    '            MSpage.Execute()
    '        End If

    '        'Look response
    '    ElseIf data.StartsWith("]f") And ServerConnectPhase = ConnectionPhase.Connected And InDream = True Then

    '        'Spawn Avatar
    '    ElseIf data.StartsWith("<") And ServerConnectPhase = ConnectionPhase.Connected Then

    '        If IsConnectedCharacter Then
    '            MSpage.Execute(28, 29, 24, 25)
    '        Else
    '            MSpage.Execute(24, 25)
    '        End If

    '        'Try

    '        ' ' Add New Arrivals to Dream.FurreList ' One or the other will
    '        ' trigger it ' IsConnectedCharacter

    '        ' If Player.Flag = 4 Or Not Dream.FurreList.Contains(Player)
    '        ' Then Dream.FurreList.Add(Player) ' If InDream Then RaiseEvent
    '        ' UpDateDreamList(Player.Name) If Player.Flag = 2 Then End If
    '        ' ElseIf Player.Flag = 2 Then

    '        'MSpage.Execute(28, 29)

    '        ' ElseIf Player.Flag = 1 Then

    '        ' ElseIf Player.Flag = 0 Then

    '        ' End If

    '        'Catch eX As Exception

    '        '    Dim logError As New ErrorLogging(eX, Me)
    '        '    Exit Sub
    '        'End Try

    '        'Remove Furre
    '    ElseIf data.StartsWith(")") And ServerConnectPhase = ConnectionPhase.Connected Then  'And loggingIn = False

    '        'Animated Move
    '    ElseIf data.StartsWith("/") And ServerConnectPhase = ConnectionPhase.Connected Then 'And loggingIn = False
    '        Try

    '            MSpage.SetVariable(MS_Name, Player.ShortName, True)
    '            MSpage.Execute(28, 29, 30, 31, 601, 602)
    '        Catch eX As Exception
    '            Dim logError As New ErrorLogging(eX, Me)
    '        End Try

    '        ' Move Avatar
    '    ElseIf data.StartsWith("A") And ServerConnectPhase = ConnectionPhase.Connected Then 'And loggingIn = False
    '        Try

    '            Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(ConnectedFurre.Position.x, ConnectedFurre.Position.y)
    '            If VisableRectangle.X <= Me.Player.Position.x And VisableRectangle.Y <= Me.Player.Position.y And VisableRectangle.height >= Me.Player.Position.y And VisableRectangle.length >= Me.Player.Position.x Then

    '                Player.Visible = True
    '            Else
    '                Player.Visible = False
    '            End If

    '            MSpage.SetVariable(MS_Name, Player.ShortName, True)
    '            MSpage.Execute(28, 29, 30, 31, 601, 602)
    '        Catch eX As Exception
    '            Dim logError As New ErrorLogging(eX, Me)
    '        End Try

    '        ' Update Color Code
    '    ElseIf data.StartsWith("B") And ServerConnectPhase = ConnectionPhase.Connected And InDream Then

    '        'Hide Avatar
    '    ElseIf data.StartsWith("C") <> False And ServerConnectPhase = ConnectionPhase.Connected Then
    '        Try

    '            MSpage.SetVariable(MS_Name, Player.ShortName, True)
    '            MSpage.Execute(30, 31)
    '        Catch eX As Exception
    '            Dim logError As New ErrorLogging(eX, Me)
    '        End Try

    '        'Display Disconnection Dialog
    '    ElseIf data.StartsWith("[") Then
    '        ' RaiseEvent UpDateDreamList("")

    '        MsgBox(data, MsgBoxStyle.Critical, "Disconnection Error")

    '        ';{mapfile}	Load a local map (one in the furcadia folder)
    '        ']q {name} {id}	Request to download a specific patch
    '    ElseIf data.StartsWith(";") OrElse data.StartsWith("]q") OrElse data.StartsWith("]r") Then
    '        MSpage.SetVariable("DREAMOWNER", "", True)
    '        MSpage.SetVariable("DREAMNAME", "", True)
    '        'RaiseEvent UpDateDreamList("")

    '    ElseIf data.StartsWith("]z") Then
    '        '   ConnectedCharacterFurcadiaID = Integer.Parse(data.Remove(0, 2))
    '        'Snag out UID
    '    ElseIf data.StartsWith("]B") Then
    '        ' ConnectedCharacterFurcadiaID = Integer.Parse(data.Substring(2,
    '        ' data.Length - ConnectedCharacterName.Length - 3))

    '    ElseIf data.StartsWith("]c") Then

    '    ElseIf data.StartsWith("]C") Then
    '        If data.StartsWith("]C0") Then

    '            MSpage.SetVariable("DREAMOWNER", Dream.Owner, True)
    '            MSpage.SetVariable("DREAMNAME", Dream.Name, True)
    '            MSpage.Execute(90, 91)
    '        End If

    '        'Process Channels Separately
    '    ElseIf data.StartsWith("(") Then
    '        If ThroatTired = True And data.StartsWith("(<font color='warning'>Your throat is tired. Try again in a few seconds.</font>") Then

    '            '(0:92) When the bot detects the "Your throat is tired. Please wait a few seconds" message,
    '            MSpage.Execute(92)

    '        End If
    '    Else

    '    End If

    'End Sub

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

    ''' <summary>
    ''' Dispose components
    ''' </summary>
    ''' <param name="disposing"></param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If Me.disposed Then Exit Sub
        If disposing Then
            handle.Dispose()
            MainEngine.Dispose()

            ' Free your own state (unmanaged objects).
            ' Set large fields to null.

        End If
        Me.disposed = True
        Me.Finalize()
    End Sub

#Region " IDisposable Support "

    ''' <summary>
    ''' Implement IDisposable Support
    ''' </summary>
    Public Overrides Sub Dispose() Implements IDisposable.Dispose

        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#End Region

End Class