Imports System.Collections.Generic
Imports System.Diagnostics

Imports System.Windows.Forms
Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Furcadia.Net.Utils.ServerParser

Imports MonkeyCore
Imports MonkeyCore.Controls
Imports MonkeyCore.Paths
Imports MonkeyCore.Settings
Imports SilverMonkeyEngine

Public Class Main
    Inherits Form

    'TODO: implement Furcadia Session to replace NetProxy Control objects and functions
    'TODO: Change Reconnection Manager to FurcadiaSession
    'TODO: Change Server Load Balancer to FurcadiaSession

#Region "Public Fields"

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
    Public WithEvents FurcadiaSession As BotSession

    Public WithEvents NotifyIcon1 As NotifyIcon
    Private WithEvents TextDisplayer As TextDisplayManager

    'Dim TimeUpdater As Threading.Thread
    '  Public Shared FurcTime As DateTime

    ' Public Bot As FURRE
    Public LogStream As LogStream

    Public Mainsettings As cMain
    Public writer As TextBoxWriter = Nothing

#End Region

#Region "Private Fields"

    Private Const HelpFile As String = "Silver Monkey.chm"

    Private _FormClose As Boolean = False

    Private BotConfig As BotOptions

    Dim CMD_Idx, CMD_Idx2 As Integer

    Dim CMD_Lck As Boolean = False

    'Input History
    Dim CMD_Max As Integer = 20

    Dim CMDList(CMD_Max) As String

    Dim curWord As String

    ''' <summary>
    ''' Bot Debug tool
    ''' </summary>
    Private MsExport As MS_Export

    ' Private ImageList As New Dictionary(Of String, Image)

#End Region

#Region "WmCpyDta"

    'Public Function FindProcessByName(strProcessName As String) As IntPtr
    '    Dim HandleOfToProcess As IntPtr = IntPtr.Zero
    '    Dim p As Process() = Process.GetProcesses()
    '    For Each p1 As Process In p
    '        Debug.WriteLine(p1.ProcessName.ToUpper())
    '        If p1.ProcessName.ToUpper() = strProcessName.ToUpper() Then
    '            HandleOfToProcess = p1.MainWindowHandle
    '            Exit For
    '        End If
    '    Next
    '    Return HandleOfToProcess
    'End Function

    'Protected Overrides Sub WndProc(ByRef m As Message)
    '    If m.Msg = WM_COPYDATA Then
    '        ''Dim mystr As COPYDATASTRUCT
    '        'Dim mystr2 As COPYDATASTRUCT = CType(Marshal.PtrToStructure(m.LParam(), GetType(COPYDATASTRUCT)), COPYDATASTRUCT)

    ' '' If the size matches 'If mystr2.cdData =
    ' Marshal.SizeOf(GetType(MyData)) Then ' ' Marshal the data from the
    ' unmanaged memory block to a ' ' MyStruct managed struct. ' Dim myStr
    ' As MyData = DirectCast(Marshal.PtrToStructure(mystr2.lpData,
    ' GetType(MyData)), MyData)

    ' ' Dim sName As String = myStr.lpName Dim sFID As Integer = 0 Dim '
    ' sTag As String = myStr.lpTag Dim sData As String = myStr.lpMsg

    ' ' If sName = "~DSEX~" Then If sTag = "Restart" Then ' EngineRestart =
    ' True cBot.MS_Script = ' msReader(CheckBotFolder(cBot.MS_File))
    ' MainEngine.MSpage = ' engine.LoadFromString(cBot.MS_Script) MS_Stared
    ' = 2 ' ' MainMSEngine.LoadLibrary() EngineRestart = False ' '
    ' Main.ResetPrimaryVariables() sndDisplay(" '
    ' <b>
    ' ' <i>[SM]</i> '
    ' </b>
    ' ' Status: File Saved. Engine Restarted") If '
    ' FurcadiaSession.IsClientConnected Then FurcadiaSession.SendClient(") '
    ' <b>
    ' ' <i>[SM]</i> '
    ' </b>
    ' ' Status: File Saved. Engine Restarted" + vbLf) PageExecute(0) ' End
    ' If Else If DREAM.FurreList.Contains(sFID) Then ' Player =
    ' DREAM.FurreList(sFID) Else Player = New ' FURRE(sName) End If

    ' ' Player.Message = sData.ToString ' PageSetVariable(MS_Name, sName) '
    ' PageSetVariable("MESSAGE", sData) ' ' Execute (0:15) When some one
    ' whispers something ' PageExecute(75, 76, 77) '
    ' SendClientMessage("Message from: " + sName, sData) ' End If 'End If
    ' Else MyBase.WndProc(m) End If

    'End Sub

    'Private Declare Sub FindWindow Lib "user32.dll" ()

    'Private Declare Function FindWindow Lib "user32.dll" (_ClassName As String, _WindowName As String) As Integer

    'Public Declare Function SetFocusAPI Lib "user32.dll" Alias "SetFocus" (ByVal hWnd As Long) As Long
    'Private Declare Function SetForegroundWindow Lib "user32" (ByVal hWnd As Long) As Long

#End Region

#Region "Public Methods"

    Public Sub BotConnecting()
        If Me.BTN_Go.InvokeRequired Then
            Dim d As New UpDateBtn_GoCallback2(AddressOf BotConnecting)
            Me.Invoke(d)
        Else
            BTN_Go.Text = "Connected."
            ConnectTrayIconMenuItem.Enabled = False
            DisconnectTrayIconMenuItem.Enabled = True
            SetBalloonText("Connected to Furcadia.")

            ''(0:1) When the bot logs into furcadia,
            'MainMSEngine.PageExecute(1)
        End If
    End Sub

    Public Sub ConnectBot()
        If Me.BTN_Go.InvokeRequired Then
            Dim d As New UpDateBtn_GoCallback2(AddressOf ConnectBot)
            Me.Invoke(d)
        Else

            If FurcadiaSession Is Nothing Then
                FurcadiaSession = New BotSession(BotConfig)
            End If

            FurcadiaSession.Connect()
            'TooStripServerStatus.Image = My.re
            'TS_Status_Client.Image = My.Resources.images5
            BTN_Go.Text = "Connecting..."
            sndDisplay("Connecting...")
            'TS_Status_Server.Image = My.Resources.images2
            ConnectTrayIconMenuItem.Enabled = False
            DisconnectTrayIconMenuItem.Enabled = True
            NotifyIcon1.ShowBalloonTip(3000, "SilverMonkey", "Connecting to Furcadia.", ToolTipIcon.Info)
            UpDateDreamList() '
        End If
    End Sub

    Public Sub ConnectionControlDisEnable()
        Me.EditBotToolStripMenuItem.Enabled = False
    End Sub

    Public Function CountOccurrences(ByRef StToSerach As String, ByRef StToLookFor As String) As Int32
        Dim iPos As Integer = -1
        Dim iFound As Integer = 0
        Do
            iPos = StToSerach.IndexOf(StToLookFor, iPos + 1)
            If iPos <> -1 Then
                iFound += 1
            End If
        Loop Until iPos = -1
        Return iFound
    End Function

    Public Sub DisconnectBot()
        If Me.BTN_Go.InvokeRequired Then
            Dim d As New UpDateBtn_GoCallback2(AddressOf DisconnectBot)
            Me.Invoke(d)
        Else
            FurcadiaSession.Disconnect()
            BTN_Go.Text = "Go!"
            'TooStripServerStatus.Image = My.Resources.images2
            'TS_Status_Client.Image = My.Resources.images2
            ConnectTrayIconMenuItem.Enabled = False
            DisconnectTrayIconMenuItem.Enabled = True
            NotifyIcon1.ShowBalloonTip(3000, "SilverMonkey", "Now disconnected from Furcadia.", ToolTipIcon.Info)

            DreamList.Items.Clear()
            TextBox_NoFlicker1.Text = ""

            ' (0:2) When the bot logs off PageExecute(2)

        End If
    End Sub

    Public Sub FormatRichTectBox(ByRef TB As MonkeyCore.Controls.RichTextBoxEx,
         ByRef style As System.Drawing.FontStyle)
        With TB
            If .SelectionFont IsNot Nothing Then
                Dim currentFont As System.Drawing.Font = .SelectionFont
                Dim newFontStyle As System.Drawing.FontStyle

                If .SelectionFont.Bold = True Then
                    newFontStyle = CType(currentFont.Style - style, System.Drawing.FontStyle)
                ElseIf .SelectionFont.Italic = True Then
                    newFontStyle = CType(currentFont.Style - System.Drawing.FontStyle.Italic, System.Drawing.FontStyle)
                ElseIf .SelectionFont.Underline = True Then
                    newFontStyle = CType(currentFont.Style - System.Drawing.FontStyle.Underline, System.Drawing.FontStyle)
                Else
                    newFontStyle = CType(currentFont.Style + style, System.Drawing.FontStyle)
                End If
                .SelectionFont = New System.Drawing.Font(currentFont.FontFamily, currentFont.Size, newFontStyle)
            End If
        End With
    End Sub

    Public Function GetWordUnderMouse(ByRef Rtf As RichTextBoxEx, ByVal X As Integer, ByVal Y As Integer) As String
        If Rtf.InvokeRequired Then
            Dim d As New WordUnderMouse(AddressOf GetWordUnderMouse)
            d.Invoke(Rtf, X, Y)
        Else
            Try
                Dim POINT As System.Drawing.Point = New System.Drawing.Point(X, Y)
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

    Public Sub InitializeTextControls()
        Log_.Font = Mainsettings.ApFont
        toServer.Font = Mainsettings.ApFont
        DreamList.Font = Mainsettings.ApFont
        TextBox_NoFlicker1.Font = Mainsettings.ApFont
    End Sub

    Public Sub KillProc(ByRef ID As Integer)
        For Each p As Process In Process.GetProcesses
            If p.Id = ID And p.Id <> 0 Then
                p.Kill()
                Exit Sub
            End If
        Next
    End Sub

    Public Sub MainText(ByRef str As String)
        If Me.InvokeRequired Then

            Dim d As New UpDateBtn_GoCallback(AddressOf MainText)
            Me.Invoke(d, str)
        Else
            Me.Text = "Silver Monkey: " & str.ToString '& " " & Application.ProductVersion
            Me.NotifyIcon1.Text = "Silver Monkey: " & str.ToString
        End If

    End Sub

    Public Sub SendClientMessage(msg As String, data As String)
        If Not FurcadiaSession Is Nothing Then

            If FurcadiaSession.IsClientConnected Then FurcadiaSession.SendToClient("(" + "<b><i>[SM]</i> - " + msg + ":</b> """ + data + """" + vbLf)
            sndDisplay("<b><i>[SM]</i> - " + msg + ":</b> """ + data + """")
        End If
    End Sub

    ''' <summary>
    ''' Deal with Server Statuses and update our UI indicators
    ''' </summary>
    ''' <param name="Sender">
    ''' </param>
    ''' <param name="e">
    ''' </param>
    Private Sub ClientStatusUpdate(Sender As Object, e As NetClientEventArgs) Handles FurcadiaSession.ClientStatusChanged
        Select Case e.ConnectPhase

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
                ToolStripClientStatus.Image = My.Resources.ConnectingImg

            Case ConnectionPhase.MOTD
            Case Else

        End Select

    End Sub

    Private Sub ServerStatusUpdate(Sender As Object, e As NetServerEventArgs) Handles FurcadiaSession.ServerStatusChanged
        Select Case e.ConnectPhase

            Case ConnectionPhase.Connected
                ToolStripServerStatus.Image = My.Resources.ConnectedImg

            Case ConnectionPhase.Connecting
                ToolStripServerStatus.Image = My.Resources.ConnectedImg

            Case ConnectionPhase.Disconnected
                ToolStripServerStatus.Image = My.Resources.DisconnectedImg

            Case ConnectionPhase.Auth
                ToolStripServerStatus.Image = My.Resources.ConnectingImg

            Case ConnectionPhase.error
                ToolStripServerStatus.Image = My.Resources.ConnectedImg

            Case ConnectionPhase.Init
                ToolStripServerStatus.Image = My.Resources.ConnectingImg

            Case ConnectionPhase.MOTD
            Case Else
        End Select

    End Sub

#End Region

#Region "Events"

    Private Delegate Sub DelTimeupdate()

    Private Delegate Sub LogSave(ByRef path As String, ByRef filename As String)

    Private Delegate Sub UpDateBtn_GoCallback(ByRef [text] As String)

    Private Delegate Sub UpDateBtn_GoCallback2()

    Private Delegate Sub UpDateBtn_GoCallback3(ByVal Obj As Object)

    Private Delegate Sub UpDateBtn_StandCallback(ByRef [furre] As FURRE)

    Private Delegate Sub UpDateDreamListCaller() 'ByVal [dummy] As String

    Private Delegate Function WordUnderMouse(ByRef Rtf As RichTextBoxEx, ByVal X As Integer, ByVal Y As Integer) As String

#End Region

#Region "Recent File List"

    ''' <summary>
    ''' how many list will save
    ''' </summary>
    Const MRUnumber As Integer = 15

    Private MRUlist As System.Collections.Generic.Queue(Of String) = New Queue(Of String)(MRUnumber)

    ''' <summary>
    ''' store a list to file and refresh list
    ''' </summary>
    ''' <param name="path">
    ''' </param>
    Public Sub SaveRecentFile(path As String)
        RecentToolStripMenuItem.DropDownItems.Clear()
        'clear all recent list from menu
        LoadRecentList(".bini")
        'load list from file
        If Not (MRUlist.Contains(path)) Then
            'prevent duplication on recent list
            MRUlist.Enqueue(path)
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
        Using stringToWrite As New StreamWriter(ApplicationSettingsPath & "/Recent.txt")
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

    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function

    Private Sub EditBotToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EditBotToolStripMenuItem.Click
        Using Bsetup As New BotSetup(BotConfig)
            Bsetup.bFile = BotConfig
            If Bsetup.ShowDialog() = Windows.Forms.DialogResult.OK Then
                BotConfig = Bsetup.bFile
            End If
        End Using
    End Sub

    ''' <summary>
    ''' load recent file list from file
    ''' </summary>
    Private Sub LoadRecentList(Extention As String)
        'try to load file. If file isn't found, do nothing
        MRUlist.Clear()
        Try
            Using listToRead As New StreamReader(ApplicationSettingsPath & "/Recent.txt")
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

            'throw;
        Catch
        End Try

    End Sub

    Private Sub NewBotToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewBotToolStripMenuItem.Click
        Using NewBotWindow As New NewBott(BotConfig)
            With NewBotWindow
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    BotConfig = NewBotWindow.BotConfig
                    EditBotToolStripMenuItem.Enabled = True
                End If
            End With
        End Using
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
            .InitialDirectory = SilverMonkeyBotPath

            If .ShowDialog = DialogResult.OK Then
                BotConfig = New BotOptions(.FileName)
                SaveRecentFile(.FileName)
                ' BotSetup.BotFile = .FileName BotSetup.ShowDialog()
                Me.EditBotToolStripMenuItem.Enabled = True
            End If

        End With
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
        BotConfig = New BotOptions(sender.ToString())
        My.Settings.LastBotFile = sender.ToString()
        EditBotToolStripMenuItem.Enabled = True
        My.Settings.Save()

        'same as open menu
    End Sub

#End Region

#Region " Methods"

    'Public Function GetSmily(ByRef ch As Char) As String

    ' Dim RTFimg As New RTFBuilder Dim file As String = "" Dim shape As
    ' Integer = 0 If (ch >= "A") And (ch <= "P") Then file = "smileys.fsh"
    ' shape = Asc(ch) - Asc("A") ElseIf (ch >= "Q" And ch <= "Z") Then file
    ' = "smileys2.fsh" shape = Asc(ch) - Asc("Q") ElseIf (ch >= "a" And ch
    ' <= "z") Then file = "smileys2.fsh" shape = Asc(ch) - Asc("a") + 10
    ' ElseIf (ch >= "1" And ch <= "3") Then file = "smileys2.fsh" shape =
    ' Asc(ch) - Asc("1") + 35 End If
    ' RTFimg.InsertImage(IMGresize(GetFrame(shape, file), log_)) Return RTFimg.ToString

    'End Function

    Public Function setLogName(ByRef bfile As BotOptions) As String
        Select Case bfile.LogOption
            Case 0
                Return bfile.LogNameBase
            Case 1
                bfile.LogIdx += 1
                bfile.SaveBotSettings()
                Return bfile.LogNameBase & BotConfig.LogIdx.ToString
            Case 2
                Return bfile.LogNameBase & Date.Now().ToString("MM_dd_yyyy_H-mm-ss")

        End Select
        Return "Default"
    End Function

    Public Sub sndDisplay(ByRef data As String, Optional ByVal newColor As TextDisplayManager.fColorEnum = TextDisplayManager.fColorEnum.DefaultColor)
        Try
            'data = data.Replace(vbLf, vbCrLf)
            If BotConfig.log Then LogStream.WriteLine(data)
            If CBool(Mainsettings.TimeStamp) Then
                Dim Now As String = DateTime.Now.ToLongTimeString
                data = Now.ToString & ": " & data
            End If
            Dim textObject As New TextDisplayManager.TextDisplayObject(data, newColor)
            TextDisplayer.AddDataToList(textObject)
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub

    '

    Private Sub DreamList_DoubleClick(sender As Object, e As System.EventArgs) Handles DreamList.DoubleClick
        If Not FurcadiaSession Is Nothing Then
            If Not DreamList.SelectedItem Is Nothing Then
                If Not FurcadiaSession.IsServerConnected Then Exit Sub
                FurcadiaSession.sndServer("l " + CType(DreamList.SelectedItem, FURRE).ShortName)
            End If
        End If
    End Sub

    Private Sub log__KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Log_.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        ElseIf (e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control) Then
            e.SuppressKeyPress = True
        End If
    End Sub

    'Private Sub ProxyError(eX As Exception, o As Object, n As String) Handles FurcadiaSession.OnError
    '    sndDisplay(o.ToString + "- " + n + ": " + eX.Message)
    '    'sndDisplay(eX.Message)
    '    'Dim logError As New ErrorLogging(eX, Me)

    'End Sub

#End Region

    Public Sub SetBalloonText(ByRef txt As String)
        'If Me.NotifyIcon1.Then Then
        '    Dim d As New UpDateBtn_GoCallback(AddressOf SetBalloonText)
        '    Me.Invoke(d, txt, {[Text]})
        'Else
        NotifyIcon1.BalloonTipText = txt
        NotifyIcon1.ShowBalloonTip(3000)
        'End If
    End Sub

    Public Function TagCloser(ByRef Str As String, ByRef Tag As String) As String
        'Tag Counters
        Dim OpenCount, CloseCount As Integer

        Dim CloseCounter As Integer
        OpenCount = CountOccurrences(Str, "<" + Tag + ">")
        CloseCount = CountOccurrences(Str, "</" + Tag + ">")
        If OpenCount > CloseCount Then
            CloseCounter = OpenCount - CloseCount
            Dim CloseTags As String = ""
            For I As Integer = 0 To CloseCounter - 1
                CloseTags = CloseTags & "</" + Tag + ">"
            Next I
            Str = Str & CloseTags
        End If
        Return Str
    End Function

    ''' <summary>
    ''' Update Dream Furre list
    ''' </summary>
    Public Sub UpDateDreamList() '
        Try
            If Me.DreamList.InvokeRequired OrElse TextBox_NoFlicker1.InvokeRequired Then
                Me.Invoke(New UpDateDreamListCaller(AddressOf UpDateDreamList))
            Else
                DreamList.BeginUpdate()

                If Not DreamList.DataSource Is Nothing Then
                    DreamList.DataSource = Nothing
                    DreamList.Refresh()
                End If
                DreamList.DataSource = FurcadiaSession.Dream.FurreList.ToIList()
                DreamList.DisplayMember = "Name"
                DreamList.ValueMember = "ShortName"
                DreamList.EndUpdate()
                TextBox_NoFlicker1.Text = FurcadiaSession.Dream.FurreList.Count.ToString

            End If
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me, FurcadiaSession.Dream.FurreList.ToString)
            logError = New ErrorLogging(eX, p)
        End Try
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem1.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub Btn_Bold_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Bold.Click
        FormatRichTectBox(Me.toServer, System.Drawing.FontStyle.Bold)
    End Sub

    Private Sub BTN_Go_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
            ConnectTrayIconMenuItem.Click, DisconnectTrayIconMenuItem.Click, BTN_Go.Click

        If IsNothing(BotConfig) Then Exit Sub
        If String.IsNullOrEmpty(BotConfig.CharacterIniFile) Then Exit Sub
        If BotConfig.CharacterIniFile = "-pick" Then Exit Sub

        Dim p As String = Path.GetDirectoryName(BotConfig.CharacterIniFile)
        If String.IsNullOrEmpty(p) And Not File.Exists(CheckBotFolder(BotConfig.CharacterIniFile)) Then
            MessageBox.Show(BotConfig.CharacterIniFile + " Not found, Aborting connection!", "Important Message")
            Exit Sub
        End If

        If BTN_Go.Text = "Go!" Then

            If BotConfig.log Then
                LogStream = New LogStream(setLogName(BotConfig), BotConfig.LogPath)
            End If

            My.Settings.LastBotFile = CheckBotFolder(BotConfig.CharacterIniFile)
            My.Settings.Save()

            If Not IsNothing(MsExport) Then MsExport.Dispose()
            Try
                ConnectBot()
            Catch Ex As NetProxyException

                FurcadiaSession.Disconnect()
                sndDisplay("Connection Aborting: " + Ex.Message)
            End Try
        Else

            FurcadiaSession.Disconnect()

        End If
    End Sub

    Private Sub BTN_Italic_Click(sender As System.Object, e As System.EventArgs) Handles BTN_Italic.Click
        FormatRichTectBox(Me.toServer, System.Drawing.FontStyle.Italic)
    End Sub

    Private Sub BTN_Underline_Click(sender As System.Object, e As System.EventArgs) Handles BTN_Underline.Click
        FormatRichTectBox(Me.toServer, System.Drawing.FontStyle.Underline)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Log_.HideSelection = Not CheckBox1.Checked
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

    Private Sub ContextTryIcon_Opened(sender As Object, e As System.EventArgs) Handles ContextTryIcon.Opened

        Select Case FurcadiaSession.ServerStatus

            Case ConnectionPhase.Init
                DisconnectTrayIconMenuItem.Enabled = False
                ConnectTrayIconMenuItem.Enabled = True

            Case ConnectionPhase.Connecting
                DisconnectTrayIconMenuItem.Enabled = True
                ConnectTrayIconMenuItem.Enabled = False

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
        Dim Vars As New Variables(FurcadiaSession)

        Vars.Show()
        Vars.Activate()
    End Sub

    ''' <summary>
    ''' Open the MonkeySpeak Export Window
    ''' </summary>
    ''' <param name="sender">
    ''' </param>
    ''' <param name="e">
    ''' </param>
    Private Sub ExportMonkeySpeakToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExportMonkeySpeakToolStripMenuItem.Click
        'MsExport = New MS_Export()
        'MsExport.Show()
        'MsExport.Activate()
    End Sub

    Private Sub FormClose()
        _FormClose = True
        My.Settings.MainFormLocation = Me.Location
        If Not IsNothing(BotConfig) Then My.Settings.LastBotFile = BotConfig.Name
        If Not FurcadiaSession Is Nothing Then FurcadiaSession.Dispose()
        'Timers.DestroyTimers()
        'Save the user settings so next time the
        'window will be the same size and location
        Mainsettings.SaveMainSettings()
        My.Settings.Save()
        NotifyIcon1.Dispose()

        Me.Dispose()
    End Sub

    Private Sub get__Click_1(sender As Object, e As EventArgs) Handles get_.Click

    End Sub

    Private Sub LaunchEditor()
        If IsNothing(BotConfig) OrElse String.IsNullOrEmpty(BotConfig.MonkeySpeakEngineOptions.MonkeySpeakScriptFile) Then
            Dim result As Integer = MessageBox.Show("No Botfile Loaded", "caption", MessageBoxButtons.OK)
            If result = DialogResult.OK Then
                Exit Sub
            End If

        End If
        Dim processStrt As New ProcessStartInfo
        processStrt.FileName = Path.Combine(Application.StartupPath, "MonkeySpeakEditor.EXE")

        Dim f As String = CheckBotFolder(BotConfig.MonkeySpeakEngineOptions.MonkeySpeakScriptFile)
        If Not FurcadiaSession Is Nothing Then
            If Not String.IsNullOrEmpty(FurcadiaSession.ConnectedCharacterName) _
            And Not String.IsNullOrEmpty(BotConfig.MonkeySpeakEngineOptions.MonkeySpeakScriptFile) Then

                processStrt.Arguments = "-B=""" + FurcadiaSession.ConnectedCharacterName + """ """ + f + """"

            ElseIf String.IsNullOrEmpty(FurcadiaSession.ConnectedCharacterName) _
            And Not String.IsNullOrEmpty(BotConfig.MonkeySpeakEngineOptions.MonkeySpeakScriptFile) Then

                processStrt.Arguments = """" + f + """"
            End If
        End If
        Process.Start(processStrt)
    End Sub

    Private Sub log__MouseHover(sender As Object, e As System.EventArgs) Handles Log_.MouseHover
        If Cursor.Current = Cursors.Hand Then

            ToolTip1.Show(curWord, Me.Log_)
        Else
            ToolTip1.Hide(Me.Log_)
        End If
    End Sub

    Private Sub log__MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Log_.MouseMove
        If Cursor.Current = Cursors.Hand Or Cursor.Current = Cursors.Default Then

            curWord = GetWordUnderMouse(Me.Log_, e.X, e.Y)

        End If
    End Sub

    Private Sub Main_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
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

    Private Sub Main_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If (e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control) Then

            LaunchEditor()

            'e.Handled = True
            'e.SuppressKeyPress = True
        ElseIf (e.KeyCode = Keys.F1) Then
            If File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HelpFile)) Then
                Help.ShowHelp(Me, HelpFile)
            End If
        ElseIf (e.KeyCode = Keys.N AndAlso e.Modifiers = Keys.Control) Then
            Using BSetUp As New BotSetup(BotConfig)
                ' .bFile =
                If BSetUp.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    BotConfig = BSetUp.bFile
                End If
            End Using

        End If

    End Sub

    Private Sub Main_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(NotifyIcon1) Then
            NotifyIcon1 = New NotifyIcon
            NotifyIcon1.ContextMenuStrip = ContextTryIcon
            NotifyIcon1.Icon = My.Resources.metal
            NotifyIcon1.BalloonTipTitle = My.Application.Info.ProductName
            NotifyIcon1.Text = My.Application.Info.ProductName + ": " + My.Application.Info.Version.ToString
            AddHandler NotifyIcon1.MouseDoubleClick, AddressOf NotifyIcon1_DoubleClick
        End If
        If Not NotifyIcon1.Visible Then NotifyIcon1.Visible = True
        'catch the Console messages
        _FormClose = False

        Mainsettings = New cMain()

        TextDisplayer = New TextDisplayManager(Mainsettings, Log_)

        writer = New TextBoxWriter(Log_)
        Console.SetOut(writer)

        ' Plugins =
        ' PluginServices.FindPlugins(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
        ' "Plugins"), "SilverMonkey.Interfaces.msPlugin")

        ' Try to get Furcadia's path from the registry

        MS_KeysIni.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Keys-MS.ini"))
        InitializeTextControls()

        Dim HelpItems = New MonkeyCore.Controls.HelpLinkMenu
        ReferenceLinksToolStripMenuItem.DropDown.Items.AddRange(HelpItems.MenuItems.ToArray)

        'Me.Size = My.Settings.MainFormSize
        Me.Location = My.Settings.MainFormLocation
        Me.Text = "Silver Monkey: " & Application.ProductVersion
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
            Dim File As String = My.Application.CommandLineArgs(0)
            Dim directoryName As String
            directoryName = Path.GetDirectoryName(File)
            If String.IsNullOrEmpty(directoryName) Then File = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Silver Monkey", File)
            BotConfig = New BotOptions(File)
            EditBotToolStripMenuItem.Enabled = True
            Console.WriteLine("Loaded: """ + File + """")
        ElseIf Mainsettings.LoadLastBotFile And Not String.IsNullOrEmpty(My.Settings.LastBotFile) And My.Application.CommandLineArgs.Count = 0 Then
            BotConfig = New BotOptions(My.Settings.LastBotFile)
            EditBotToolStripMenuItem.Enabled = True
            Console.WriteLine("Loaded: """ + My.Settings.LastBotFile + """")
        End If
        Dim ts As TimeSpan = TimeSpan.FromSeconds(30)

        If Not IsNothing(BotConfig) Then
            If BotConfig.AutoConnect Then
                ConnectBot()
            End If
        End If

        'Paths.InstallPath = SettingsIni.GetKeyValue("Main", "FurcPath")
    End Sub

    Private Sub MenuCopy_Click(sender As System.Object, e As System.EventArgs) Handles MenuCopy.Click
        toServer.Copy()
    End Sub

    Private Sub MenuCopy2_Click(sender As System.Object, e As System.EventArgs) Handles MenuCopy2.Click
        Log_.Copy()
    End Sub

    Private Sub MenuCut_Click(sender As System.Object, e As System.EventArgs) Handles MenuCut.Click
        toServer.Cut()
    End Sub

    'Private Function MessagePump(ByRef Server_Instruction As String) As Boolean
    '    Dim objPlugin As SilverMonkey.Interfaces.msPlugin
    '    Dim intIndex As Integer
    '    Dim Handled As Boolean = False
    '    If Not Plugins Is Nothing Then
    '        For intIndex = 0 To Plugins.Length - 1
    '            objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(intIndex)), SilverMonkey.Interfaces.msPlugin)
    '            If Mainsettings.PluginList.Item(objPlugin.Name.Replace(" ", "")) Then
    '                objPlugin.Initialize(objHost)
    '                objPlugin.Page = MainEngine.MSpage
    '                If objPlugin.MessagePump(Server_Instruction) Then Handled = True
    '            End If
    '        Next
    '    End If
    '    Return Handled
    'End Function

    Private Sub MSEditorToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MSEditorToolStripMenuItem.Click, EditorTrayIconMenuItem.Click
        LaunchEditor()
    End Sub

    Private Sub NotifyIcon1_Disposed(sender As Object, e As System.EventArgs) Handles NotifyIcon1.Disposed
        Me.Visible = False
    End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        If Not IsNothing(NotifyIcon1) Then
            Me.Show()
            Me.Activate()
        End If

    End Sub

    Private Sub onClientReceived(data As String) Handles FurcadiaSession.ClientData2
        FurcadiaSession.SendToServer(data)
    End Sub

    Private Sub onProcessServerChannelData(InstructionObject As ChannelObject, Args As ParseServerArgs) _
        Handles FurcadiaSession.ProcessServerChannelData

        If Not String.IsNullOrEmpty(InstructionObject.ChannelText) Then
            sndDisplay(InstructionObject.ChannelText)
        ElseIf Not String.IsNullOrEmpty(InstructionObject.Player.Message) Then
            sndDisplay(InstructionObject.Player.Message)
        Else
            sndDisplay(InstructionObject.RawInstruction)
        End If

    End Sub

    Private Sub onServerReceive(data As String) Handles FurcadiaSession.ServerData2
        If (FurcadiaSession.ServerStatus = ConnectionPhase.MOTD) Then
            sndDisplay(data)
        End If
        FurcadiaSession.SendToClient(data)

    End Sub

    ''' <summary>
    ''' Update the Dreams Furre list as the list changes by spawn and remove instructions
    ''' </summary>
    ''' <param name="InstructionObject">
    ''' </param>
    ''' <param name="Args">
    ''' </param>
    Private Sub parseFurreList(InstructionObject As BaseServerInstruction, Args As ParseServerArgs) Handles FurcadiaSession.ProcessServerInstruction
        Select Case InstructionObject.InstructionType
            Case ServerInstructionType.SpawnAvatar
                UpDateDreamList()
            Case ServerInstructionType.RemoveAvatar
                UpDateDreamList()
        End Select

    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        toServer.Paste()
    End Sub

#Region "Action Controls"

    Private ActionCMD As String

    Private Sub _ne_Click(sender As System.Object, e As System.EventArgs) Handles _ne.Click
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            FurcadiaSession.SendToServer("`m 9")
        End If
    End Sub

    Private Sub _ne_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _ne.MouseDown
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            Me.ActionTmr.Enabled = FurcadiaSession.IsServerConnected
            ActionCMD = "`m 9"
        End If
    End Sub

    Private Sub _ne_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _ne.MouseUp
        Me.ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    Private Sub _nw_Click(sender As System.Object, e As System.EventArgs) Handles _nw.Click
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            FurcadiaSession.SendToServer("`m 7")
        End If
    End Sub

    Private Sub _nw_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _nw.MouseDown
        If Not FurcadiaSession Is Nothing Then
            Me.ActionTmr.Enabled = FurcadiaSession.IsServerConnected
            ActionCMD = "`m 7"
        End If

    End Sub

    Private Sub _nw_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _nw.MouseUp
        If Not FurcadiaSession Is Nothing Then
            Me.ActionTmr.Enabled = False
            ActionCMD = ""
        End If
    End Sub

    Private Sub ActionTmr_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActionTmr.Tick
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            FurcadiaSession.SendToServer(ActionCMD)
        End If
    End Sub

    Private Sub BTN_TurnL_Click(sender As System.Object, e As System.EventArgs) Handles BTN_TurnL.Click
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            FurcadiaSession.SendToServer("`<")
        End If
    End Sub

    Private Sub BTN_TurnR_Click(sender As System.Object, e As System.EventArgs) Handles BTN_TurnR.Click
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            FurcadiaSession.SendToServer("`>")
        End If
    End Sub

    Private Sub BtnSit_stand_Lie_Click(sender As System.Object, e As System.EventArgs) Handles BtnSit_stand_Lie.Click
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub

            If BtnSit_stand_Lie.Text = "Stand" Then
                BtnSit_stand_Lie.Text = "Lay"
            ElseIf BtnSit_stand_Lie.Text = "Lay" Then
                BtnSit_stand_Lie.Text = "Sit"
            ElseIf BtnSit_stand_Lie.Text = "Sit" Then
                BtnSit_stand_Lie.Text = "Stand"
            End If
            FurcadiaSession.SendToServer("`lie")
        End If
    End Sub

    Private Sub get__Click(sender As Object, e As System.EventArgs) Handles get_.Click
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            FurcadiaSession.SendToServer("`get")
        End If
    End Sub

    Private Sub se__Click(sender As Object, e As System.EventArgs) Handles se_.Click
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            FurcadiaSession.SendToServer("`m 3")
        End If
    End Sub

    Private Sub se__MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles se_.MouseDown
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            Me.ActionTmr.Enabled = FurcadiaSession.IsServerConnected
            ActionCMD = "`m 3"
        End If
    End Sub

    Private Sub se__MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles se_.MouseUp
        Me.ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    Private Sub sw__Click(sender As Object, e As System.EventArgs) Handles sw_.Click
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            FurcadiaSession.SendToServer("`m 1")
        End If
    End Sub

    Private Sub sw__MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles sw_.MouseDown
        If FurcadiaSession Is Nothing Then Exit Sub
        Me.ActionTmr.Enabled = FurcadiaSession.IsServerConnected
        ActionCMD = "`m 1"
    End Sub

    Private Sub sw__MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles sw_.MouseUp

        Me.ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    Private Sub use__Click(sender As Object, e As System.EventArgs) Handles use_.Click
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            FurcadiaSession.SendToServer("`use")
        End If
    End Sub

#End Region

    Private Sub sendToServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sendToServer.Click
        If Not FurcadiaSession Is Nothing Then
            If Not FurcadiaSession.IsServerConnected Then Exit Sub
            FurcadiaSession.SendToServer(toServer.Text.Replace(vbCrLf, ""))
            toServer.Clear()
        End If

    End Sub

    Private Sub toServer_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles toServer.KeyDown
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
            If Not FurcadiaSession Is Nothing Then
                If Not FurcadiaSession.IsServerConnected Then Exit Sub
                FurcadiaSession.SendToServer(toServer.Text.Replace(vbLf, "").Replace(vbCr, ""))
                toServer.Clear()
            End If

            e.SuppressKeyPress = True
            e.Handled = True
        End If

    End Sub

End Class