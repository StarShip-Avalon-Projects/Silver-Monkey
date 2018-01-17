Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports BotSession
Imports Furcadia.Logging
Imports Furcadia.Net
Imports Furcadia.Net.DreamInfo
Imports Furcadia.Net.Utils.ServerParser
Imports Logging
Imports MonkeyCore
Imports MonkeyCore.Controls
Imports MonkeyCore.Settings
Imports MonkeyCore.Utils.Logging

Imports SilverMonkey.Engine
Imports SilverMonkey.HelperClasses
Imports SilverMonkey.HelperClasses.TextDisplayManager

Public Class Main
    Inherits Form

#Region "Fields"

    Public Shared WithEvents DebugWindow As Variables
    Private Shared WithEvents TextDisplayer As TextDisplayManager

    ''' <summary>
    ''' Decouple Our Bot Stuff from the GUI
    ''' <para>
    ''' Move all the Proxy functions to FurcSessilon
    ''' </para>
    ''' <para>
    ''' Move MonkeySpeak Engine to BotSession
    ''' </para>
    ''' <para>
    ''' Move SubSystems to FurcLib (pounce, PhoenixSpeak, Dice ETC)
    ''' </para>
    ''' </summary>
    Public WithEvents FurcadiaSession As Bot

    Public WithEvents NotifyIcon1 As NotifyIcon

#End Region

#Region "Public Fields"

    Public Shared Mainsettings As cMain
    Public writer As TextBoxWriter = Nothing

#End Region

#Region "Private Fields"

    Private Const MsEditor As String = "Monkeyspeak Editor.exe"
    Private Const HelpFile As String = "Silver Monkey.chm"

    ''' <summary>
    ''' how many list will save
    ''' </summary>
    Private Const MRUnumber As Integer = 15

    Private _FormClose As Boolean = False

    Private ActionCMD As String

    Private BotConfig As BotOptions

    Private CMD_Idx, CMD_Idx2 As Integer

    Dim CMD_Lck As Boolean = False

    'Input History
    Private CMD_Max As Integer = 20

    Private CMDList(CMD_Max) As String

    Private curWord As String

    ''' <summary>
    ''' Debug Window cache
    ''' </summary>
    Private DebugLogs As StringBuilder

    Private Shared FileLogWriter As LogStream
    Private MRUlist As Queue(Of String)

#End Region

#Region "Public Constructors"

    Public Sub New()
        _FormClose = False
        ' This call is required by the designer.
        InitializeComponent()
        FileLogWriter = New LogStream
        ' Add any initialization after the InitializeComponent() call.
        Dim HelpItems = New HelpLinkToolStripMenu()
        ReferenceLinksToolStripMenuItem.DropDown.Items.AddRange(HelpItems.MenuItems.ToArray)

        BotConfig = New BotOptions()
        FurcadiaSession = New Bot(BotConfig)
        MRUlist = New Queue(Of String)(MRUnumber)
        DebugLogs = New StringBuilder()
        Mainsettings = New cMain()

        ''  MS_KeysIni.Load(Path.Combine(ApplicationPath, "Keys-MS.ini"))

        InitializeTextControls()
        Monkeyspeak.Logging.Logger.InfoEnabled = True
        Monkeyspeak.Logging.Logger.SuppressSpam = False
        Monkeyspeak.Logging.Logger.WarningEnabled = True
        Monkeyspeak.Logging.Logger.SingleThreaded = True

        Furcadia.Logging.Logger.InfoEnabled = True
        Furcadia.Logging.Logger.SuppressSpam = False
        Furcadia.Logging.Logger.WarningEnabled = True
        Furcadia.Logging.Logger.SingleThreaded = True
        Furcadia.Logging.Logger.LogOutput = New MultiLogOutput(New FileLogger(), New Engine.MultipleLogOutput())

        Monkeyspeak.Logging.Logger.LogOutput = New Monkeyspeak.Logging.MultiLogOutput(New Monkeyspeak.Logging.FileLogger(), New Engine.MultipleLogOutput())
    End Sub

#End Region

#Region "Private Delegates"

    Private Delegate Sub UpDateBtn_StandCallback(ByRef [furre] As Furre)

    Private Delegate Sub UpdateUiDelegate(ByVal Obj As Object)

    Private Delegate Sub UpdateUiDelegate1()

    Private Delegate Function WordUnderMouse(ByRef Rtf As RichTextBoxEx, ByVal X As Integer, ByVal Y As Integer) As String

#End Region

#Region "Public Properties"

    Public ReadOnly Property CanConnect As Boolean
        Get
            Return Not String.IsNullOrWhiteSpace(BotConfig.BotSettingsFile)
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Shared Sub FormatRichTectBox(ByRef TB As MonkeyCore.Controls.RichTextBoxEx,
         ByRef style As System.Drawing.FontStyle)
        With TB
            If .SelectionFont IsNot Nothing Then
                Dim currentFont As System.Drawing.Font = .SelectionFont
                Dim newFontStyle As System.Drawing.FontStyle

                If .SelectionFont.Bold = True Then
                    newFontStyle = DirectCast(currentFont.Style - style, System.Drawing.FontStyle)
                ElseIf .SelectionFont.Italic = True Then
                    newFontStyle = DirectCast(currentFont.Style - System.Drawing.FontStyle.Italic, System.Drawing.FontStyle)
                ElseIf .SelectionFont.Underline = True Then
                    newFontStyle = DirectCast(currentFont.Style - System.Drawing.FontStyle.Underline, System.Drawing.FontStyle)
                Else
                    newFontStyle = DirectCast(currentFont.Style + style, Drawing.FontStyle)
                End If
                .SelectionFont = New Drawing.Font(currentFont.FontFamily, currentFont.Size, newFontStyle)
            End If
        End With
    End Sub

    Public Function GetWordUnderMouse(ByRef Rtf As RichTextBoxEx, ByVal X As Integer, ByVal Y As Integer) As String
        If Rtf.InvokeRequired Then
            Dim d As New WordUnderMouse(AddressOf GetWordUnderMouse)
            d.Invoke(Rtf, X, Y)
        Else
            Try
                Dim POINT = New Drawing.Point(X, Y)
                Dim Pos As Integer, i As Integer, lStart As Integer, lEnd As Integer
                Dim lLen As Integer, sTxt As String, sChr As String

                GetWordUnderMouse = vbNullString
                '
                With Rtf
                    lLen = .Text.Length
                    sTxt = .Text
                    Pos = .GetCharIndexFromPosition(POINT)
                End With
                If Pos > 0 Then
                    For i = Pos To 1 Step -1
                        sChr = sTxt.Substring(i, 1)
                        If sChr = " " Or sChr = Chr(10) Or i = 1 Then
                            'if the starting character is vbcrlf then
                            'we want to chop that off
                            If sChr = Chr(10) Then
                                lStart = (i + 1)
                            Else
                                lStart = i
                            End If
                            Exit For
                        End If
                    Next i
                    For i = Pos To lLen
                        If sTxt.Substring(i, 1) = " " Or sTxt.Substring(i, 1) = Chr(10) Or i = lLen Then
                            lEnd = i + 1
                            Exit For
                        End If
                    Next i
                    If lEnd >= lStart Then
                        Dim test As String = sTxt.Substring(lStart, lEnd - lStart).Trim
                        Return sTxt.Substring(lStart, lEnd - lStart).Trim
                    End If
                End If
            Catch ex As Exception
                Return ""
            End Try
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Update Main window title bar
    ''' </summary>
    ''' <param name="str">The string.</param>
    Public Sub MainTitleText(str As Object)
        If Me.InvokeRequired Then

            Dim d As New UpdateUiDelegate(AddressOf MainTitleText)
            Me.Invoke(d, str)
        Else
            Me.Text = str.ToString
            Me.NotifyIcon1.Text = str.ToString
        End If

    End Sub

    Public Sub OnFurcadiaSessionError(ex As Exception, o As Object) _
        Handles FurcadiaSession.[Error]

        If ex.GetType() Is GetType(Monkeyspeak.MonkeyspeakException) Then
            If o.GetType Is GetType(Monkeyspeak.TriggerHandler) Then
                SendTextToDebugWindow(DirectCast(o, Monkeyspeak.TriggerHandler).Target.ToString)
            End If
            SndDisplay("MonkeySpeak Error:" + ex.Message + Environment.NewLine + o.ToString, DisplayColors.Error)
            If ex.InnerException IsNot Nothing Then
                If ex.InnerException.GetType() Is GetType(Libraries.Web.WebException) Then
                    Dim InnerEx = DirectCast(ex.InnerException, Libraries.Web.WebException)
                    SendTextToDebugWindow(InnerEx.ToString)
                Else
                    SendTextToDebugWindow(ex.InnerException.ToString)
                End If

            End If
        Else
            If o Is Nothing Then o = "null"
            SndDisplay("Furcadia Session error:" + ex.Message + Environment.NewLine + o.ToString, DisplayColors.Error)
        End If

    End Sub

    ''' <summary>
    ''' Saves the recent file.
    ''' </summary>
    ''' <param name="FilePath">The file path.</param>
    Public Sub SaveRecentFile(FilePath As String)
        RecentToolStripMenuItem.DropDownItems.Clear()
        'clear all recent list from menu
        LoadRecentList(".bini")
        'load list from file
        If Not MRUlist.Contains(FilePath) Then
            'prevent duplication on recent list
            MRUlist.Enqueue(FilePath)
        End If
        'insert given path into list
        While MRUlist.Count > MRUnumber
            'keep list number not exceeded given value
            MRUlist.Dequeue()
        End While
        For Each item As String In MRUlist
            Dim fileRecent As New ToolStripMenuItem(item, Nothing, AddressOf RecentFile_click)
            'create new menu for each item in list
            'add the menu to "recent" menu
            RecentToolStripMenuItem.DropDownItems.Add(fileRecent)
        Next
        'writing menu list to file
        Using stringToWrite = New StreamWriter(Path.Combine(MonkeyCore2.IO.Paths.ApplicationSettingsPath, "Recent.txt"))
            'create file called "Recent.txt" located on app folder
            For Each item As String In MRUlist
                'write list to stream
                stringToWrite.WriteLine(item)
            Next
            'write stream to file
            stringToWrite.Close()
        End Using
        'close the stream and reclaim memory
    End Sub

    ''' <summary>
    ''' Send text like MonkeySpeak inner exception errors to the Debug Window
    ''' <para>
    ''' Some Monkey Speak lines expect errors under certain conditions. These are what we want to send to the DebugWindow
    ''' so the Bot User can handle then accordingly
    ''' </para>
    ''' </summary>
    ''' <param name="text"></param>
    Public Sub SendTextToDebugWindow(ByVal text As String)
        If DebugWindow Is Nothing OrElse DebugWindow.IsDisposed() Then
            DebugLogs.AppendLine(text)
        Else
            DebugWindow.SendLogsToDemugWindow(text)
        End If

    End Sub

    ''' <summary>
    ''' Send formatted text to log box
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="newColor"></param>
    Public Sub SndDisplay(data As String, Optional newColor As DisplayColors = DisplayColors.DefaultColor)

        If BotConfig.LogOptions.log Then LogStream.WriteLine(data)
        If CBool(Mainsettings.TimeStamp) Then
            Dim Now As String = DateTime.Now.ToLongTimeString
            data = Now.ToString & ": " & data
        End If
        Dim textObject As New TextDisplayObject(data, newColor)
        TextDisplayer.AddDataToList(textObject)

    End Sub

    ''' <summary>
    ''' Send formatted text to log box
    ''' </summary>
    ''' <param name="Message"></param>
    Public Sub SndDisplay(Message As Monkeyspeak.Logging.LogMessage)

        If BotConfig.LogOptions.log Then LogStream.WriteLine(Message.message)
        Dim newColor = DisplayColors.DefaultColor
        Select Case Message.Level
            Case Monkeyspeak.Logging.Level.Warning
                newColor = DisplayColors.Warning
            Case Monkeyspeak.Logging.Level.Debug
                newColor = DisplayColors.Warning
            Case Monkeyspeak.Logging.Level.Error
                newColor = DisplayColors.Error
        End Select
        SndDisplay(Message.message, newColor)
    End Sub

    ''' <summary>
    ''' Send formatted text to log box
    ''' </summary>
    ''' <param name="Message"></param>
    Public Sub SndDisplay(Message As Furcadia.Logging.LogMessage)

        If BotConfig.LogOptions.log Then LogStream.WriteLine(Message.message)
        Dim newColor = DisplayColors.DefaultColor
        Select Case Message.Level
            Case Furcadia.Logging.Level.Warning
                newColor = DisplayColors.Error
            Case Furcadia.Logging.Level.Debug Or Furcadia.Logging.Level.Error
                newColor = DisplayColors.Warning
        End Select
        SndDisplay(Message.message, newColor)
    End Sub

    Public Sub UpDatButtonGoText(str As Object)
        If Me.InvokeRequired Then

            Dim d As New UpdateUiDelegate(AddressOf UpDatButtonGoText)
            Me.Invoke(d, str.ToString)
        Else
            BTN_Go.Text = str.ToString
        End If

    End Sub

    Public Function UpdateBotConfig(BotFilePath As String, Optional BConfig As BotOptions = Nothing) As Boolean

        If BConfig Is Nothing Then

            BotConfig = New BotOptions(BotFilePath)
        Else
            BotConfig = BConfig
        End If
        FurcadiaSession.SetOptions(BotConfig)

        SilverMonkeyBotPath = BotConfig.BotPath

        Return True

    End Function

    ''' <summary>
    ''' Update Dream Furre list
    ''' </summary>
    Public Sub UpDateDreamList() '

        Try
            If Me.DreamList.InvokeRequired OrElse FurreCountTxtBx.InvokeRequired Then
                Me.Invoke(New UpdateUiDelegate1(AddressOf UpDateDreamList))
            Else
                Try
                    DreamList.BeginUpdate()

                    If Not DreamList.DataSource Is Nothing Then
                        DreamList.DataSource = Nothing
                        DreamList.Refresh()
                    End If
                    DreamList.DataSource = FurcadiaSession.Dream.Furres.ToIList()
                    DreamList.DisplayMember = "Name"
                    DreamList.ValueMember = "ShortName"
                Finally
                    DreamList.EndUpdate()
                End Try
                Try
                    FurreCountTxtBx.Text = FurcadiaSession.Dream.Furres.Count.ToString
                Catch
                End Try

            End If
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub

#End Region

#Region "Private Methods"

    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function

    Private Sub _ne_Click(sender As System.Object, e As System.EventArgs) Handles _ne.Click
        SendCommandToServer("`m 9")
    End Sub

    Private Sub _ne_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _ne.MouseDown

        If Not FurcadiaSession.IsServerSocketConnected Then Exit Sub
        Me.ActionTmr.Enabled = FurcadiaSession.IsServerSocketConnected
        ActionCMD = "`m 9"

    End Sub

    Private Sub _ne_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _ne.MouseUp
        Me.ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    Private Sub _nw_Click(sender As System.Object, e As System.EventArgs) Handles _nw.Click
        SendCommandToServer("`m 7")
    End Sub

    Private Sub _nw_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _nw.MouseDown

        Me.ActionTmr.Enabled = FurcadiaSession.IsServerSocketConnected
        ActionCMD = "`m 7"

    End Sub

    Private Sub _nw_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _nw.MouseUp

        Me.ActionTmr.Enabled = False
        ActionCMD = ""

    End Sub

    Private Sub AboutToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem1.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub ActionTmr_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActionTmr.Tick
        SendCommandToServer(ActionCMD)
    End Sub

    Private Sub Btn_Bold_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Bold.Click
        FormatRichTectBox(Me.toServer, System.Drawing.FontStyle.Bold)
    End Sub

    Private Async Sub BTN_Go_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
            ConnectTrayIconMenuItem.Click, DisconnectTrayIconMenuItem.Click, BTN_Go.Click
        BTN_Go.Enabled = False
        Try
            FurcadiaSession.Options = BotConfig

            If Not CanConnect Then Exit Sub

            If FurcadiaSession.IsServerSocketConnected Or FurcadiaSession.ServerStatus = ConnectionPhase.Connecting Then
                BTN_Go.Text = "Disconnecting..."
                Await Task.Run(Sub() FurcadiaSession.Disconnect())
                ConnectTrayIconMenuItem.Enabled = False
                DisconnectTrayIconMenuItem.Enabled = True
                NotifyIcon1.ShowBalloonTip(3000, "SilverMonkey", "Now disconnected from Furcadia.", ToolTipIcon.Info)
                FurreCountTxtBx.Text = ""
            Else
                BTN_Go.Text = "Connecting..."
                My.Settings.LastBotFile = MonkeyCore2.IO.Paths.CheckBotFolder(BotConfig.BotSettingsFile)
                My.Settings.Save()
                FurcadiaSession.SetOptions(BotConfig)
                If BotConfig.LogOptions.log Then
                    Try

                        FileLogWriter = New LogStream(BotConfig.LogOptions)
                    Catch
                        Furcadia.Logging.Logger.Error($"There's an error with log-file {BotConfig.LogOptions.GetLogName}")
                        Exit Sub
                    End Try
                End If

                Monkeyspeak.Logging.Logger.Info("New Session Started")
                Await FurcadiaSession.ConnetAsync()
                If FurcadiaSession.IsServerSocketConnected Then
                    ConnectTrayIconMenuItem.Enabled = False
                    DisconnectTrayIconMenuItem.Enabled = True
                    Await Task.Run(Sub() UpDateDreamList()) '
                End If

            End If
        Catch ex As Exception
            SndDisplay($"Error: {ex.Message}{Environment.NewLine}", DisplayColors.Error)
        Finally
            BTN_Go.Enabled = True
        End Try
    End Sub

    Private Sub BTN_Italic_Click(sender As System.Object, e As System.EventArgs) Handles BTN_Italic.Click
        FormatRichTectBox(Me.toServer, System.Drawing.FontStyle.Italic)
    End Sub

    Private Sub BTN_TurnL_Click(sender As System.Object, e As System.EventArgs) Handles BTN_TurnL.Click
        SendCommandToServer("`<")
    End Sub

    Private Sub BTN_TurnR_Click(sender As System.Object, e As System.EventArgs) Handles BTN_TurnR.Click
        SendCommandToServer("`>")
    End Sub

    Private Sub BTN_Underline_Click(sender As System.Object, e As System.EventArgs) Handles BTN_Underline.Click
        FormatRichTectBox(Me.toServer, System.Drawing.FontStyle.Underline)
    End Sub

    Private Sub BtnSit_stand_Lie_Click(sender As System.Object, e As System.EventArgs) Handles BtnSit_stand_Lie.Click

        If Not FurcadiaSession.IsServerSocketConnected Then Exit Sub

        If BtnSit_stand_Lie.Text = "Stand" Then
            BtnSit_stand_Lie.Text = "Lay"
        ElseIf BtnSit_stand_Lie.Text = "Lay" Then
            BtnSit_stand_Lie.Text = "Sit"
        ElseIf BtnSit_stand_Lie.Text = "Sit" Then
            BtnSit_stand_Lie.Text = "Stand"
        End If
        SendCommandToServer("`lie")

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Log_.HideSelection = Not CheckBox1.Checked
    End Sub

    ''' <summary>
    ''' Deal with Client Statuses and update our UI indicators
    ''' </summary>
    ''' <param name="Sender">
    ''' </param>
    ''' <param name="e">
    ''' </param>
    Private Sub ClientStatusUpdate(Sender As Object, e As NetClientEventArgs) _
        Handles FurcadiaSession.ClientStatusChanged

        UpdateUiClientStatus(e.ConnectPhase)

    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CloseToolStripMenuItem.Click, ExitTrayIconMenuItem.Click
        FormClose()
    End Sub

    Private Sub ConfigToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigToolStripMenuItem.Click
        Config.Show()
        Config.Activate()
    End Sub

    Private Sub ContentsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ContentsToolStripMenuItem.Click
        If File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HelpFile)) Then
            Help.ShowHelp(Me, HelpFile)
        End If

    End Sub

    Private Async Sub ContextTryIcon_Opened(sender As Object, e As System.EventArgs) Handles ContextTryIcon.Opened
        If FurcadiaSession Is Nothing Then Exit Sub
        Select Case FurcadiaSession.ServerStatus

            Case ConnectionPhase.Init
                Await Meep(DisconnectTrayIconMenuItem, False)
                Await Meep(ConnectTrayIconMenuItem, True)

            Case ConnectionPhase.MOTD Or ConnectionPhase.Connecting
                DisconnectTrayIconMenuItem.Enabled = True
                ConnectTrayIconMenuItem.Enabled = False

        End Select

    End Sub

    ''' <summary>
    ''' Open the Bot Debugging window
    ''' </summary>
    ''' <param name="sender">
    ''' </param>
    ''' <param name="e">
    ''' </param>
    Private Sub DebugToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DebugToolStripMenuItem.Click
        If DebugWindow Is Nothing OrElse DebugWindow.IsDisposed() Then
            DebugWindow = New Variables(FurcadiaSession.MSpage)
        End If

        DebugWindow.Show()
        DebugWindow.Activate()
        If DebugLogs.Length > 0 Then
            DebugWindow.SendLogsToDemugWindow(DebugLogs)
            DebugLogs.Clear()
        End If
    End Sub

    Private Sub DreamList_DoubleClick(sender As Object, e As System.EventArgs) Handles DreamList.DoubleClick

        If Not DreamList.SelectedItem Is Nothing Then
            If Not FurcadiaSession.IsServerSocketConnected Then Exit Sub
            FurcadiaSession.SendFormattedTextToServer("l " + DirectCast(DreamList.SelectedItem, Furre).ShortName)
        End If

    End Sub

    Private Sub EditBotToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EditBotToolStripMenuItem.Click
        Using Bsetup = New BotSetup()
            Bsetup.BotConfig = BotConfig
            If Bsetup.ShowDialog() = Windows.Forms.DialogResult.OK Then
                BotConfig = Bsetup.BotConfig
            End If
        End Using
    End Sub

    Private Sub Follow_Click(sender As Object, e As EventArgs) _
        Handles Follow.Click, Lead.Click, Summon.Click, Join.Click
        If Not DreamList.SelectedItem Is Nothing Then
            If Not FurcadiaSession.IsServerSocketConnected Then Exit Sub
            FurcadiaSession.SendFormattedTextToServer($"`{DirectCast(sender, ToolStripMenuItem).Tag} {DirectCast(DreamList.SelectedItem, Furre).ShortName}")
        End If
    End Sub

    Private Sub FormClose()
        _FormClose = True
        My.Settings.MainFormLocation = Me.Location
        My.Settings.LastBotFile = BotConfig.Name
        FurcadiaSession.Dispose()
        'Save the user settings so next time the
        'window will be the same size and location
        Mainsettings.SaveMainSettings()
        My.Settings.Save()
        NotifyIcon1.Dispose()

        Me.Dispose()
    End Sub

    Private Sub Get__Click(sender As Object, e As System.EventArgs) Handles get_.Click
        SendCommandToServer("`get")
    End Sub

    Private Sub InitializeTextControls()

        Log_.Font = Mainsettings.ApFont
        toServer.Font = Mainsettings.ApFont
        DreamList.Font = Mainsettings.ApFont
        FurreCountTxtBx.Font = Mainsettings.ApFont
    End Sub

    Private Sub LaunchEditor()
        If String.IsNullOrEmpty(BotConfig.MonkeySpeakEngineOptions.MonkeySpeakScriptFile) Then
            Dim result As Integer = MessageBox.Show("No Botfile Loaded", "caption", MessageBoxButtons.OK)
            If result = DialogResult.OK Then
                Exit Sub
            End If

        End If

        Dim processStrt As New ProcessStartInfo With {
            .FileName = Path.Combine(MonkeyCore2.IO.Paths.ApplicationPath, MsEditor)
        }

        Dim f As String = MonkeyCore2.IO.Paths.CheckBotFolder(BotConfig.MonkeySpeakEngineOptions.MonkeySpeakScriptFile)

        If Not String.IsNullOrEmpty(FurcadiaSession.ConnectedFurre.Name) _
            And Not String.IsNullOrEmpty(BotConfig.MonkeySpeakEngineOptions.MonkeySpeakScriptFile) Then

            processStrt.Arguments = "-B=""" + FurcadiaSession.ConnectedFurre.Name + """ """ + f + """"

        ElseIf String.IsNullOrEmpty(FurcadiaSession.ConnectedFurre.Name) _
        And Not String.IsNullOrEmpty(BotConfig.MonkeySpeakEngineOptions.MonkeySpeakScriptFile) Then

            processStrt.Arguments = """" + f + """"
        End If
        Process.Start(processStrt)
    End Sub

    ''' <summary>
    ''' load recent file list from file
    ''' </summary>
    Private Sub LoadRecentList(Extention As String)
        'try to load file. If file isn't found, do nothing
        MRUlist.Clear()
        Try
            Using fStream As New FileStream(Path.Combine(MonkeyCore2.IO.Paths.ApplicationSettingsPath, "Recent.txt"), FileMode.OpenOrCreate)
                Using listToRead = New StreamReader(fStream)
                    'read file stream
                    Dim line As String = ""
                    While (InlineAssignHelper(line, listToRead.ReadLine())) IsNot Nothing
                        Dim ext As String = Path.GetExtension(line)
                        If ext = Extention Then
                            'read each line until end of file
                            MRUlist.Enqueue(line)
                        End If
                    End While
                    'insert to list
                    'close the stream
                    listToRead.Close()
                End Using
            End Using
            'throw;
        Catch
        End Try

    End Sub

    '
    Private Sub Log__KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Log_.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        ElseIf (e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control) Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub Log__LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles Log_.LinkClicked
        Dim Proto As String = ""
        Dim Str As String = e.LinkText

        If Str.Contains("#") Then
            Str = Str.Substring(0, Str.IndexOf("#"))
        End If
        Proto = Str.Substring(0, Str.IndexOf("://"))
        Select Case Proto.ToLower
            Case "http"
                Try
                    Me.Cursor = System.Windows.Forms.Cursors.AppStarting

                    Process.Start(Str)
                Catch
                Finally
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                End Try
            Case "https"
                Try
                    Me.Cursor = System.Windows.Forms.Cursors.AppStarting

                    Process.Start(Str)
                Catch
                Finally
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                End Try

            Case Else
                MsgBox("Protocol: """ & Proto & """ Not yet implemented")
        End Select
        'MsgBox(Proto)
    End Sub

    Private Sub Log__MouseHover(sender As Object, e As System.EventArgs) Handles Log_.MouseHover
        If Cursor.Current = Cursors.Hand Then

            ToolTip1.Show(curWord, Me.Log_)
        Else
            ToolTip1.Hide(Me.Log_)
        End If
    End Sub

    Private Sub Log__MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Log_.MouseMove
        If Cursor.Current = Cursors.Hand Or Cursor.Current = Cursors.Default Then

            curWord = GetWordUnderMouse(Me.Log_, e.X, e.Y)

        End If
    End Sub

    Private Sub Look_Click(sender As Object, e As EventArgs) Handles Look.Click
        If Not DreamList.SelectedItem Is Nothing Then
            If Not FurcadiaSession.IsServerSocketConnected Then Exit Sub
            FurcadiaSession.SendFormattedTextToServer("l " + DirectCast(DreamList.SelectedItem, Furre).ShortName)
        End If
    End Sub

    Private Sub Main_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try

            Select Case Mainsettings.SysTray
                Case CheckState.Checked
                    Me.Visible = False
                    e.Cancel = True
                Case CheckState.Indeterminate
                    If MessageBox.Show("Minimize to SysTray?", "", MessageBoxButtons.YesNo, Nothing,
                     MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        Mainsettings.SysTray = CheckState.Checked
                        Mainsettings.SaveMainSettings()
                        Me.Visible = False
                        e.Cancel = True
                    Else
                        e.Cancel = False
                        FormClose()
                    End If
                Case CheckState.Unchecked
                    FormClose()

            End Select
            'TimeUpdater.Abort()
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub

    Private Sub Main_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub Main_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If (e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control) Then

            LaunchEditor()

            'e.Handled = True
            'e.SuppressKeyPress = True
        ElseIf (e.KeyCode = Keys.F1) Then
            If File.Exists(Path.Combine(MonkeyCore2.IO.Paths.ApplicationPath, HelpFile)) Then
                Help.ShowHelp(Me, HelpFile)
            End If
        ElseIf (e.KeyCode = Keys.N AndAlso e.Modifiers = Keys.Control) Then
            Using BSetUp As New BotSetup(BotConfig)
                ' .BotConfig =
                If BSetUp.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    UpdateBotConfig(BSetUp.BotConfig.BotPath, BSetUp.BotConfig)

                End If
            End Using

        End If

    End Sub

    Private Sub Main_Leave(sender As Object, e As EventArgs) Handles Me.Leave

    End Sub

    Private Sub Main_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not NotifyIcon1 Is Nothing Then
                RemoveHandler NotifyIcon1.MouseDoubleClick, AddressOf NotifyIcon1_DoubleClick
                NotifyIcon1.Dispose()
            End If
            If NotifyIcon1 Is Nothing Then
                NotifyIcon1 = New NotifyIcon With {
                .ContextMenuStrip = ContextTryIcon,
                .Icon = My.Resources.metal,
                .BalloonTipTitle = My.Application.Info.ProductName,
                .Text = My.Application.Info.ProductName + ": " + My.Application.Info.Version.ToString
            }
                AddHandler NotifyIcon1.MouseDoubleClick, AddressOf NotifyIcon1_DoubleClick
            End If
            TextDisplayer = New TextDisplayManager(Mainsettings, Log_)
            If Not NotifyIcon1.Visible Then NotifyIcon1.Visible = True

            writer = New TextBoxWriter(Log_)
            Console.SetOut(writer)

            'Me.Size = My.Settings.MainFormSize
            Me.Location = My.Settings.MainFormLocation
            MainTitleText($"Silver Monkey: {Application.ProductVersion}")
            Me.Visible = True

            LoadRecentList(".bini")
            For Each item As String In MRUlist
                Dim fileRecent As New ToolStripMenuItem(item, Nothing, AddressOf RecentFile_click)
                'create new menu for each item in list
                'add the menu to "recent" menu
                RecentToolStripMenuItem.DropDownItems.Add(fileRecent)
            Next
            EditBotToolStripMenuItem.Enabled = False
            If (My.Application.CommandLineArgs.Count > 0) Then
                Dim File As String = String.Join(" ", My.Application.CommandLineArgs())
                Dim directoryName As String
                directoryName = Path.GetDirectoryName(File)
                If String.IsNullOrEmpty(directoryName) Then
                    File = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Silver Monkey", File)
                End If

                UpdateBotConfig(File)
                EditBotToolStripMenuItem.Enabled = True
                Monkeyspeak.Logging.Logger.Info($"Loaded: ""{ File }""")
            ElseIf Mainsettings.LoadLastBotFile And Not String.IsNullOrEmpty(My.Settings.LastBotFile) And My.Application.CommandLineArgs.Count = 0 Then
                UpdateBotConfig(My.Settings.LastBotFile)
                EditBotToolStripMenuItem.Enabled = True
                Monkeyspeak.Logging.Logger.Info($"Loaded: ""{My.Settings.LastBotFile}""")
            End If

            'If Not IsNothing(BotConfig) Then
            '    UpdateBotConfig(BotConfig.BotSettingsFile, BotConfig)
            '    If BotConfig.AutoConnect Then
            '        '' BTN_Go.PerformClick()
            '    End If
            'End If
        Catch ex As Exception
            Furcadia.Logging.Logger.Error(ex)
        End Try
    End Sub

    Private Function Meep(disconnectTrayIconMenuItem As ToolStripMenuItem, v As Boolean) As Task
        disconnectTrayIconMenuItem.Enabled = v
    End Function

    Private Sub MenuCopy_Click(sender As System.Object, e As System.EventArgs) Handles MenuCopy.Click
        toServer.Copy()
    End Sub

    Private Sub MenuCopy2_Click(sender As System.Object, e As System.EventArgs) Handles MenuCopy2.Click
        Log_.Copy()
    End Sub

    Private Sub MenuCut_Click(sender As System.Object, e As System.EventArgs) Handles MenuCut.Click
        toServer.Cut()
    End Sub

    Private Sub MSEditorToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MSEditorToolStripMenuItem.Click, EditorTrayIconMenuItem.Click
        LaunchEditor()
    End Sub

    Private Sub NewBotToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewBotToolStripMenuItem.Click
        Using NewBotWindow As New NewBott(BotConfig)
            With NewBotWindow
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    UpdateBotConfig(BotConfig.BotPath, NewBotWindow.BotConfig)
                    EditBotToolStripMenuItem.Enabled = True
                End If
            End With
        End Using
    End Sub

    Private Sub NotifyIcon1_Disposed(sender As Object, e As System.EventArgs) Handles NotifyIcon1.Disposed
        Me.Visible = False
    End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        If Not NotifyIcon1 Is Nothing Then
            Me.Show()
            Me.Activate()
        End If

    End Sub

    ''' <summary>
    ''' Parse the Server Channels
    ''' </summary>
    ''' <param name="sender">
    ''' <see cref="ChannelObject"/>
    ''' </param>
    ''' <param name="Args">
    ''' <see cref="ParseChannelArgs"/>
    ''' </param>
    Private Sub OnProcessServerChannelData(sender As Object, Args As ParseChannelArgs) _
        Handles FurcadiaSession.ProcessServerChannelData

        Dim InstructionObject = DirectCast(sender, ChannelObject)
        Dim color = DisplayColors.DefaultColor
        Select Case Args.Channel
            Case "@emit"
                color = DisplayColors.Emit
            Case "say"
                color = DisplayColors.Say
            Case "myspeech"
                color = DisplayColors.Say
            Case "emote"
                color = DisplayColors.Emote
            Case "whisper"
                color = DisplayColors.Whisper
            Case "shout"
                color = DisplayColors.Shout
        End Select
        If Not String.IsNullOrEmpty(InstructionObject.FormattedChannelText) Then
            SndDisplay(InstructionObject.FormattedChannelText, color)
        ElseIf Not String.IsNullOrEmpty(InstructionObject.Player.Message) Then
            SndDisplay(InstructionObject.Player.Message.ToStrippedFurcadiaMarkupString, color)
        Else
            SndDisplay(InstructionObject.RawInstruction, color)
        End If

    End Sub

    Private Sub OnServerReceive(data As String) Handles FurcadiaSession.ServerData2
        If (FurcadiaSession.ServerStatus = ConnectionPhase.MOTD) Then
            SndDisplay(data)
        End If

    End Sub

    ''' <summary>
    ''' </summary>
    ''' <param name="sender">
    ''' </param>
    ''' <param name="e">
    ''' </param>
    Private Sub OpenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        With BotIniOpen
            ' Select Bot ini file
            .InitialDirectory = MonkeyCore2.IO.Paths.SilverMonkeyBotPath

            If .ShowDialog = DialogResult.OK Then
                UpdateBotConfig(.FileName)
                SaveRecentFile(.FileName)
                UpdateBotConfig(.FileName)
                ' BotSetup.BotFile = .FileName BotSetup.ShowDialog()
                Me.EditBotToolStripMenuItem.Enabled = True
            End If

        End With
    End Sub

    ''' <summary>
    ''' Update the Dreams Furre list as the list changes by spawn and remove instructions
    ''' </summary>
    ''' <param name="sender">InstructionObject
    ''' </param>
    ''' <param name="Args">
    ''' </param>
    Private Sub ParseFurres(sender As Object, Args As ParseServerArgs) Handles FurcadiaSession.ProcessServerInstruction

        Select Case Args.ServerInstruction
            Case ServerInstructionType.SpawnAvatar
                UpDateDreamList()
            Case ServerInstructionType.RemoveAvatar
                UpDateDreamList()
            Case ServerInstructionType.BookmarkDream
                UpDateDreamList()
            Case ServerInstructionType.LoadDreamEvent
                UpDateDreamList()
        End Select

    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        toServer.Paste()
    End Sub

    ''' <summary>
    ''' click menu handler
    ''' </summary>
    ''' <param name="sender">
    ''' </param>
    ''' <param name="e">
    ''' </param>
    Private Sub RecentFile_click(sender As Object, e As EventArgs) Handles RecentToolStripMenuItem.Click
        'BotSetup.BotFile =
        'BotSetup.ShowDialog()

        If Not FurcadiaSession.IsServerSocketConnected Then
            Try
                UpdateBotConfig(sender.ToString())
                My.Settings.LastBotFile = sender.ToString()
                EditBotToolStripMenuItem.Enabled = True
                My.Settings.Save()
            Catch ex As FileNotFoundException

            End Try
        End If

        'same as open menu
    End Sub

    Private Sub Se__Click(sender As Object, e As System.EventArgs) Handles se_.Click
        SendCommandToServer("`m 3")
    End Sub

    Private Sub Se__MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles se_.MouseDown

        If Not FurcadiaSession.IsServerSocketConnected Then Exit Sub
        Me.ActionTmr.Enabled = FurcadiaSession.IsServerSocketConnected
        ActionCMD = "`m 3"

    End Sub

    Private Sub Se__MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles se_.MouseUp
        Me.ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    ''' <summary>
    ''' Sends a text command to the server if we are connected to server
    ''' </summary>
    ''' <param name="command">
    ''' </param>
    Private Sub SendCommandToServer(ByVal command As String)

        If Not FurcadiaSession.IsServerSocketConnected Then Exit Sub
        FurcadiaSession.SendFormattedTextToServer(command)

    End Sub

    Private Sub SendToServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles sendToServer.Click
        SendCommandToServer(toServer.Text.Replace(vbCrLf, ""))
        toServer.Clear()
    End Sub

    Private Sub ServerStatusUpdate(Sender As Object, e As NetServerEventArgs) Handles FurcadiaSession.ServerStatusChanged
        UpdateUiServerStatus(e.ConnectPhase)

    End Sub

    Private Sub StartupProcessToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HelpFile)) Then
            Help.ShowHelp(Me, HelpFile, "/html/4c192ea5-9a9c-4dae-927f-7581b05c0f65.htm")
        End If
    End Sub

    Private Sub Sw__Click(sender As Object, e As System.EventArgs) Handles sw_.Click
        SendCommandToServer("`m 1")
    End Sub

    Private Sub Sw__MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles sw_.MouseDown
        Me.ActionTmr.Enabled = FurcadiaSession.IsServerSocketConnected
        ActionCMD = "`m 1"
    End Sub

    Private Sub Sw__MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles sw_.MouseUp

        Me.ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    Private Sub ToServer_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles toServer.KeyDown
        'Command History
        If (e.KeyCode = Keys.I AndAlso e.Modifiers = Keys.Control) Then

            If CMD_Idx2 < 0 AndAlso CMD_Lck = True Then
                CMD_Idx2 = CMD_Max - 1
            ElseIf CMD_Idx2 < 0 AndAlso CMD_Lck = False Then
                CMD_Idx2 = CMD_Idx
            End If
            toServer.Clear()
            toServer.Rtf = CMDList(CMD_Idx2)
            toServer.SelectionStart = toServer.Text.Length

            CMD_Idx2 -= 1
            e.SuppressKeyPress = True
            e.Handled = True

            'Ctrl Cut
        ElseIf (e.KeyCode = Keys.X AndAlso e.Modifiers = Keys.Control) Then
            toServer.Cut()
            e.Handled = True
            'Ctrl Copy
        ElseIf (e.KeyCode = Keys.C AndAlso e.Modifiers = Keys.Control) Then
            toServer.Copy()
            e.Handled = True
            'Ctrl Paste
        ElseIf (e.KeyCode = Keys.V AndAlso e.Modifiers = Keys.Control) Then
            toServer.Paste()
            e.Handled = True
            'Ctrl Undo
        ElseIf (e.KeyCode = Keys.Z AndAlso e.Modifiers = Keys.Control) Then
            toServer.Undo()
            e.Handled = True
            'trl Redo
        ElseIf (e.KeyCode = Keys.Enter) Then
            SendCommandToServer(toServer.Text.Replace(vbLf, "").Replace(vbCr, ""))
            toServer.Clear()
            e.SuppressKeyPress = True
            e.Handled = True
        End If

    End Sub

    Private Sub UpdateUiClientStatus(ConnectPhase As Object)
        If Me.InvokeRequired Then
            Dim d As New UpdateUiDelegate(AddressOf UpdateUiClientStatus)
            Me.Invoke(d, ConnectPhase)
        Else
            Dim Phase = DirectCast(ConnectPhase, ConnectionPhase)
            Select Case Phase

                Case ConnectionPhase.Connected
                    ToolStripClientStatus.Image = My.Resources.ConnectedImg

                Case ConnectionPhase.Connecting
                    ToolStripClientStatus.Image = My.Resources.ConnectedImg

                Case ConnectionPhase.Disconnected
                    ToolStripClientStatus.Image = My.Resources.DisconnectedImg

                Case ConnectionPhase.Auth
                    ToolStripClientStatus.Image = My.Resources.ConnectingImg

                Case ConnectionPhase.error
                    ToolStripClientStatus.Image = My.Resources.ConnectedImg

                Case ConnectionPhase.Init
                    ToolStripClientStatus.Image = My.Resources.DisconnectedImg
            End Select
        End If
    End Sub

    Private Sub UpdateUiServerStatus(ConnectPhase As Object)
        If Me.InvokeRequired Then
            Dim d As New UpdateUiDelegate(AddressOf UpdateUiServerStatus)
            Me.Invoke(d, ConnectPhase)
        Else
            Dim Phase = DirectCast(ConnectPhase, ConnectionPhase)
            Select Case Phase

                Case ConnectionPhase.Connected
                    ToolStripServerStatus.Image = My.Resources.ConnectedImg
                    UpDatButtonGoText("Connected.")
                    MainTitleText($"Silver Monkey: {FurcadiaSession.ConnectedFurre.Name}")
                Case ConnectionPhase.Connecting
                    ToolStripServerStatus.Image = My.Resources.ConnectedImg
                    UpDatButtonGoText("Connecting...")
                Case ConnectionPhase.Disconnected
                    ToolStripServerStatus.Image = My.Resources.DisconnectedImg
                    UpDatButtonGoText("Go!")
                    MainTitleText($"Silver Monkey: {Application.ProductVersion}")
                Case ConnectionPhase.Auth
                    ToolStripServerStatus.Image = My.Resources.ConnectingImg

                Case ConnectionPhase.error
                    ToolStripServerStatus.Image = My.Resources.ConnectedImg

                Case ConnectionPhase.Init
                    ToolStripServerStatus.Image = My.Resources.DisconnectedImg
                    UpDatButtonGoText("Go!")
            End Select
        End If

    End Sub

    Private Sub Use__Click(sender As Object, e As System.EventArgs) Handles use_.Click
        SendCommandToServer("`use")
    End Sub

    Private Sub Whisper_Click(sender As Object, e As EventArgs) Handles Whisper.Click
        If Not DreamList.SelectedItem Is Nothing Then
            If Not FurcadiaSession.IsServerSocketConnected Then Exit Sub
            toServer.Focus()
            toServer.Text = $"/%{DirectCast(DreamList.SelectedItem, Furre).ShortName} "
            toServer.SelectionStart = toServer.Text.Length
        End If
    End Sub

#End Region

End Class