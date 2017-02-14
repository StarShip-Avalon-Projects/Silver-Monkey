Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports Furcadia.Drawing
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Diagnostics
Imports System.Windows.Forms
Imports MonkeyCore
Imports MonkeyCore.Settings
Imports MonkeyCore.Controls
Imports MonkeyCore.Utils
Imports Furcadia.Net
Imports System.Threading

Public Class Main
    Inherits Form

#Region "SysTray"
    Public Shared WithEvents NotifyIcon1 As NotifyIcon = Nothing

    Public Shared Property MainSettings As cMain
#End Region

#Region "WmCpyDta"

    <DllImport("user32.dll", EntryPoint:="FindWindow")>
    Private Shared Function FindWindow(_ClassName As String, _WindowName As String) As Integer
    End Function
    Public Declare Function SetFocusAPI Lib "user32.dll" Alias "SetFocus" (ByVal hWnd As Long) As Long
    Private Declare Function SetForegroundWindow Lib "user32" (ByVal hWnd As Long) As Long

    Public Function FindProcessByName(strProcessName As String) As IntPtr
        Dim HandleOfToProcess As IntPtr = IntPtr.Zero
        Dim p As Process() = Process.GetProcesses()
        For Each p1 As Process In p
            Debug.WriteLine(p1.ProcessName.ToUpper())
            If p1.ProcessName.ToUpper() = strProcessName.ToUpper() Then
                HandleOfToProcess = p1.MainWindowHandle
                Exit For
            End If
        Next
        Return HandleOfToProcess
    End Function

    'Protected Overrides Sub WndProc(ByRef m As Message)
    '    If m.Msg = WM_COPYDATA Then
    '        'Dim mystr As COPYDATASTRUCT
    '        Dim mystr2 As COPYDATASTRUCT = CType(Marshal.PtrToStructure(m.LParam(), GetType(COPYDATASTRUCT)), COPYDATASTRUCT)

    '        ' If the size matches
    '        If mystr2.cdData = Marshal.SizeOf(GetType(MyData)) Then
    '            ' Marshal the data from the unmanaged memory block to a
    '            ' MyStruct managed struct.
    '            Dim myStr As MyData = CType(Marshal.PtrToStructure(mystr2.lpData, GetType(MyData)), MyData)

    '            Dim sName As String = myStr.lpName
    '            Dim sFID As UInteger = 0
    '            Dim sTag As String = myStr.lpTag
    '            Dim sData As String = myStr.lpMsg

    '            If sName = "~DSEX~" Then
    '                If sTag = "Restart" Then
    '                    MS_Engine.MainMSEngine.EngineRestart = True
    '                    cBot.MS_Script = MS_Engine.MainMSEngine.msReader(Path.Combine(Paths.SilverMonkeyBotPath, cBot.MS_File))
    '                    MainMSEngine.MSpage = MS_Engine.MainMSEngine.engine.LoadFromString(cBot.MS_Script)
    '                    MainMSEngine.MS_Stared = 2
    '                    ' MainMSEngine.LoadLibrary()
    '                    MS_Engine.MainMSEngine.EngineRestart = False
    '                    ' Main.ResetPrimaryVariables()
    '                    sndDisplay("<b><i>[SM]</i></b> Status: File Saved. Engine Restarted", fColorEnum.DefaultColor)
    '                    'If smProxy.IsClientConnected Then smProxy.SendClient(")<b><i>[SM]</i></b> Status: File Saved. Engine Restarted" + vbLf)
    '                    MS_Engine.MainMSEngine.PageExecute(0)
    '                End If
    '            Else
    '                'If DREAM.List.ContainsKey(sFID) Then
    '                '    Player = DREAM.List.Item(sFID)
    '                'Else
    '                '    Player.Clear()
    '                '    Player.Name = sName
    '                'End If

    '                'Player.Message = sData.ToString
    '                MainMSEngine.PageSetVariable(MS_Name, sName)
    '                MainMSEngine.PageSetVariable("MESSAGE", sData)
    '                ' Execute (0:15) When some one whispers something
    '                MS_Engine.MainMSEngine.PageExecute(75, 76, 77)
    '                'SendClientMessage("Message from: " + sName, sData)
    '            End If
    '        End If
    '    Else
    '        MyBase.WndProc(m)
    '    End If

    'End Sub

#End Region

    Private Shared _FormClose As Boolean = False

    Public Shared NewBot As Boolean = False

    <CLSCompliant(False)>
    Public writer As TextBoxWriter = Nothing

#Region "Propertes"

    Public WithEvents FurcSession As FurcSession

    Dim ActionCMD As String = ""
    Dim curWord As String

    'Input History
    Dim CMD_Max As Integer = 20
    Dim CMDList(CMD_Max) As String
    Dim CMD_Idx, CMD_Idx2 As Integer
    Dim CMD_Lck As Boolean = False

    'Dim TimeUpdater As Threading.Thread
    Public Shared FurcTime As DateTime

#End Region

#Region "Properties"
    Private _cMain As cMain
#End Region

#Region "Events"

    'UpDate Btn-Go Text and Actions Group Enable
    Private Delegate Sub Log_Scoll(ByRef rtb As RichTextBoxEx)
    Private Delegate Sub UpDateBtn_GoCallback(ByRef [text] As String)
    Private Delegate Sub UpDateBtn_GoCallback2()
    Private Delegate Function UpDateBtn_StandCallback(ByRef player As FURRE) As Boolean
    Private Delegate Sub LogSave(ByRef path As String, ByRef filename As String)
    Private Delegate Sub AddDataToListCaller(ByRef lb As RichTextBoxEx, ByRef obj As String, ByRef NewColor As fColorEnum)
    Private Delegate Sub UpDateDreamListCaller(ByRef [dummy] As String) 'ByVal [dummy] As String

    Private Delegate Sub DelTimeupdate()
    Private Delegate Function WordUnderMouse(ByRef Rtf As RichTextBoxEx, ByVal X As Integer, ByVal Y As Integer) As String

    Private LogTimer As New System.Timers.Timer()
    Private DreamUpdateTimer As New System.Timers.Timer()
    Private MSalarm As Threading.Timer

#End Region

#Region "Recent File List"
    ''' <summary>
    ''' how many list will save
    ''' </summary>
    Const MRUnumber As Integer = 15
    Private Shared MRUlist As Queue(Of String) = New Queue(Of String)(MRUnumber)

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        With BotIniOpen
            ' Select Bot ini file
            .InitialDirectory = Paths.SilverMonkeyBotPath
            If .ShowDialog = DialogResult.OK Then
                cBot = New cBot(.FileName)
                SaveRecentFile(.FileName)
                ' BotSetup.BotFile = .FileName
                ' BotSetup.ShowDialog()
                EditBotToolStripMenuItem.Enabled = True
            End If

        End With
    End Sub
    ''' <summary>
    ''' store a list to file and refresh list
    ''' </summary>
    ''' <param name="path"></param>
    Public Sub SaveRecentFile(path As String)
        RecentToolStripMenuItem.DropDownItems.Clear()
        'clear all recent list from menu
        LoadRecentList()
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
        Using stringToWrite As New StreamWriter(System.IO.Path.Combine(Paths.ApplicationSettingsPath, "Recent.txt"))
            'create file called "Recent.txt" located on app folder
            For Each item As String In MRUlist
                'write list to stream
                stringToWrite.WriteLine(item)
            Next
            stringToWrite.Flush()
            'write stream to file
            stringToWrite.Close()
        End Using
        'close the stream and reclaim memory
    End Sub
    ''' <summary>
    ''' load recent file list from file
    ''' </summary>
    Private Sub LoadRecentList()
        'try to load file. If file isn't found, do nothing
        MRUlist.Clear()
        'If Not File.Exists(Path.Combine(Paths.ApplicationSettingsPath, "Recent.txt")) Then
        '    File.Create(Path.Combine(Paths.ApplicationSettingsPath, "Recent.txt"))
        'End If
        If File.Exists(Path.Combine(Paths.ApplicationSettingsPath, "Recent.txt")) Then
            Using listToRead As New StreamReader(Path.Combine(Paths.ApplicationSettingsPath, "Recent.txt"), True)
                'read file stream
                Dim line As String = ""
                While (InlineAssignHelper(line, listToRead.ReadLine())) IsNot Nothing
                    'read each line until end of file
                    MRUlist.Enqueue(line)
                End While
                'insert to list
                'close the stream
                listToRead.Close()
            End Using
        End If

    End Sub
    ''' <summary>
    ''' click menu handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RecentFile_click(sender As Object, e As EventArgs)
        'BotSetup.BotFile =
        'BotSetup.ShowDialog()
        cBot = New cBot(sender.ToString())
        My.Settings.LastBotFile = cBot.IniFile
        EditBotToolStripMenuItem.Enabled = True
        My.Settings.Save()

        'same as open menu
    End Sub

    Private Sub NewBotToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewBotToolStripMenuItem.Click

        With NewBott
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                cBot = .bFile
                EditBotToolStripMenuItem.Enabled = True
            End If
        End With
    End Sub

    Private Sub EditBotToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditBotToolStripMenuItem.Click
        With BotSetup
            .bFile = cBot
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                cBot = .bFile
            End If
        End With
    End Sub

    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function
#End Region

#Region "Log Scroll"

    Dim Pos_Old As Integer = 0
    Private Const SBS_HORZ As Integer = 0
    Private Const SBS_VERT As Integer = 1

    <DllImport("user32.dll")>
    Public Shared Function GetScrollRange(ByVal hWnd As IntPtr, ByVal nBar As Integer,
 ByRef lpMinPos As Integer,
    ByRef lpMaxPos As Integer) As Boolean
    End Function

    <DllImport("user32.dll")>
    Public Shared Function GetScrollPos(ByVal hWnd As IntPtr,
    ByVal nBar As Integer) As Integer
    End Function
    Public Enum ScrollInfoMask As UInteger
        SIF_RANGE = &H1
        SIF_PAGE = &H2
        SIF_POS = &H4
        SIF_DISABLENOSCROLL = &H8
        SIF_TRACKPOS = &H10
        SIF_ALL = (SIF_RANGE Or SIF_PAGE Or SIF_POS Or SIF_TRACKPOS)
    End Enum
    Public Enum SBOrientation As Integer
        SB_HORZ = &H0
        SB_VERT = &H1
        SB_CTL = &H2
        SB_BOTH = &H3
    End Enum
    <Serializable(), StructLayout(LayoutKind.Sequential)>
    Structure SCROLLINFO
        Public cbSize As Integer
        <MarshalAs(UnmanagedType.U4)> Public fMask As ScrollInfoMask
        Public nMin As Integer
        Public nMax As Integer
        Public nPage As UInteger
        Public nPos As Integer
        Public nTrackPos As Integer
    End Structure
    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function GetScrollInfo(hWnd As IntPtr,
  <MarshalAs(UnmanagedType.I4)> fnBar As SBOrientation,
    <MarshalAs(UnmanagedType.Struct)> ByRef lpsi As SCROLLINFO) As Integer
    End Function
    Private Sub ScrollToEnd(ByRef rtb As RichTextBoxEx)
        If rtb.InvokeRequired Then

            Dim d As New Log_Scoll(AddressOf ScrollToEnd)
            d.Invoke(rtb)
        End If
        Dim scrollMin As Integer = 0
        Dim Sinfo As New SCROLLINFO
        Sinfo.cbSize = Marshal.SizeOf(Sinfo)
        Sinfo.fMask = ScrollInfoMask.SIF_POS
        Dim scrollMax As Integer = 0
        Dim test As Integer = GetScrollInfo(rtb.Handle, SBOrientation.SB_VERT, Sinfo)

        If (GetScrollRange(rtb.Handle, SBS_VERT, scrollMin, scrollMax)) Then
            Dim pos As Integer = GetScrollPos(rtb.Handle, SBS_VERT)
            If scrollMax = Pos_Old Then
                rtb.SelectionStart = rtb.Text.Length
            End If
            'Pos_Old = GetScrollPos(rtb.Handle, SBS_VERT)
            ' Detect if they're at the bottom
        End If
    End Sub

#End Region

    Public Function fColor(Optional ByVal MyColor As fColorEnum = fColorEnum.DefaultColor) As System.Drawing.Color
        Try
            Select Case MyColor
                Case fColorEnum.DefaultColor
                    Return MainSettings.DefaultColor
                Case fColorEnum.Emit
                    Return MainSettings.EmitColor
                Case fColorEnum.Say
                    Return MainSettings.SayColor
                Case fColorEnum.Shout
                    Return MainSettings.ShoutColor
                Case fColorEnum.Whisper
                    Return MainSettings.WhColor
                Case fColorEnum.Emote
                    Return MainSettings.EmoteColor
                Case Else
                    Return MainSettings.DefaultColor
            End Select
        Catch Ex As Exception
            Dim logError As New ErrorLogging(Ex, Me)
        End Try
        ' Return
    End Function

    Public Sub ConnectBot()
        If BTN_Go.InvokeRequired Then
            Dim d As New UpDateBtn_GoCallback2(AddressOf ConnectBot)
            Invoke(d)
        Else
            'Link to FurcadiaSession
            FurcSession.ConnectBot()

            TS_Status_Server.Image = My.Resources.images5
            TS_Status_Client.Image = My.Resources.images5
            BTN_Go.Text = "Connecting..."
            '   sndDisplay("Connecting...", fColorEnum.DefaultColor)
            'TS_Status_Server.Image = My.Resources.images2
            ConnectTrayIconMenuItem.Enabled = False
            DisconnectTrayIconMenuItem.Enabled = True
            NotifyIcon1.ShowBalloonTip(3000, "SilverMonkey", "Connecting to Furcadia.", ToolTipIcon.Info)
        End If
    End Sub
    Public Sub DisconnectBot()
        If BTN_Go.InvokeRequired Then
            Dim d As New UpDateBtn_GoCallback2(AddressOf DisconnectBot)
            Invoke(d)
        Else
            FurcSession.DisconnectBot()
            DreamList.Items.Clear()
            DreamCountTxtBx.Text = ""
            BTN_Go.Text = "Go!"
            TS_Status_Server.Image = My.Resources.images2
            TS_Status_Client.Image = My.Resources.images2
            ConnectTrayIconMenuItem.Enabled = False
            DisconnectTrayIconMenuItem.Enabled = True
            NotifyIcon1.ShowBalloonTip(3000, "SilverMonkey", "Now disconnected from Furcadia.", ToolTipIcon.Info)

        End If

    End Sub
    Public Sub BotConnecting()
        If BTN_Go.InvokeRequired Then
            Dim d As New UpDateBtn_GoCallback2(AddressOf BotConnecting)
            Invoke(d)
        Else
            BTN_Go.Text = "Connected."
            '  loggingIn = 2
            ConnectTrayIconMenuItem.Enabled = False
            DisconnectTrayIconMenuItem.Enabled = True
            SetBalloonText("Connected to Furcadia.")
            TS_Status_Server.Image = My.Resources.images3

        End If
    End Sub

#Region " Methods"
    Public Sub AddDataToList(ByRef lb As RichTextBoxEx, ByRef obj As String, ByRef newColor As fColorEnum)
        If InvokeRequired Then
            Dim dataArray() As Object = {lb, obj, newColor}
            Invoke(New AddDataToListCaller(AddressOf AddDataToList), dataArray)
        Else
            If lb.GetType().ToString.Contains("Controls.RichTextBoxEx") Then
                'Pos_Old = GetScrollPos(lb.Handle, SBS_VERT)
                Dim build As New System.Text.StringBuilder(obj)
                build = build.Replace("</b>", "\b0 ")
                build = build.Replace("<b>", "\b ")
                build = build.Replace("</i>", "\i0 ")
                build = build.Replace("<i>", "\i ")
                build = build.Replace("</ul>", "\ul0 ")
                build = build.Replace("<ul>", "\ul ")

                build = build.Replace("&lt;", "<")
                build = build.Replace("&gt;", ">")

                Dim Names As MatchCollection = Regex.Matches(obj, FurcSession.NameFilter)
                For Each Name As System.Text.RegularExpressions.Match In Names
                    build = build.Replace(Name.ToString, Name.Groups(3).Value)
                Next
                '<name shortname='acuara' forced>
                Dim MyIcon As MatchCollection = Regex.Matches(obj, FurcSession.Iconfilter)
                For Each Icon As System.Text.RegularExpressions.Match In MyIcon
                    Select Case Icon.Groups(1).Value
                        Case "91"
                            build = build.Replace(Icon.ToString, "[#]")
                        Case Else
                            build = build.Replace(Icon.ToString, "[" + Icon.Groups(1).Value + "]")
                    End Select

                Next

                'Dim myColor As System.Drawing.Color = fColor(newColor)
                lb.ReadOnly = False
                lb.BeginUpdate()

                lb.SelectionStart = lb.TextLength
                lb.SelectedRtf = FormatText(build.ToString, newColor)

                'since we Put the Data in the RTB now we Finish Setting the Links
                Dim param() As String = {"<a.*?href=['""](.*?)['""].*?>(.*?)</a>", "<a.*?href=(.*?)>(.*?)</a>"}
                For i As Integer = 0 To param.Length - 1
                    Dim links As MatchCollection = Regex.Matches(lb.Text, param(i), RegexOptions.IgnoreCase)
                    ' links = links & Regex.Matches(lb.Text, "<a.*?href='(.*?)'.*?>(.*?)</a>", RegexOptions.IgnoreCase)
                    For Each mmatch As System.Text.RegularExpressions.Match In links
                        Dim matchUrl As String = mmatch.Groups(1).Value
                        Dim matchText As String = mmatch.Groups(2).Value
                        If mmatch.Success Then
                            With lb
                                .Find(mmatch.ToString)
                                'WAIT Snag Image Links first!
                                .SelectedRtf = FormatURL(matchText & "\v #" & matchUrl & "\v0 ")
                                .Find(matchText & "#" & matchUrl, RichTextBoxFinds.WholeWord)
                                .SetSelectionLink(True)
                            End With
                        End If
                    Next
                Next

                Try
                    Dim SelStart As Integer = 0
                    While (lb.Lines.Length > 350)
                        'Array.Copy(lb.Lines, 1, lb.Lines, 0, lb.Lines.Length - 1)
                        SelStart = lb.SelectionStart
                        lb.SelectionStart = 0
                        lb.SelectionLength = lb.Text.IndexOf(vbLf, 0) + 1
                        lb.SelectedText = ""
                        lb.SelectionStart = SelStart
                    End While
                Catch
                    lb.Clear()
                    Console.WriteLine("Reset Log box due to over flow")
                End Try
                lb.EndUpdate()
                lb.ReadOnly = True

            End If

        End If
    End Sub

    Private Function IMGresize(ByRef bm_source As Bitmap, ByRef RTF As RichTextBoxEx) As Bitmap
        'Dim g As Graphics = Me.CreateGraphic
        Dim g As Drawing.Graphics = CreateGraphics()
        Dim x, y As Integer

        y = CInt(RTF.SelectionFont.Height * 72 / g.DpiX)
        Dim dy As Integer = y - bm_source.Height
        x = bm_source.Width - dy

        ' Make a bitmap for the result.
        Dim bm_dest As New Bitmap(x, y)

        ' Make a Graphics object for the result Bitmap.
        Dim gr_dest As Graphics = Graphics.FromImage(bm_dest)
        gr_dest.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        ' gr_dest.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        ' Copy the source image into the destination bitmap.
        gr_dest.DrawImage(bm_source, 0, 0, bm_dest.Width, bm_dest.Height)
        Return bm_dest

        ' Display the result.

    End Function

    Private ImageList As New Dictionary(Of String, Image)

    Private Sub log__KeyDown(sender As Object, e As KeyEventArgs) Handles log_.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        ElseIf (e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control) Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub log__LinkClicked(ByVal sender As Object, ByVal e As LinkClickedEventArgs) Handles log_.LinkClicked
        Dim Proto As String = ""
        Dim Str As String = e.LinkText
        Try
            If Str.Contains("#") Then
                Proto = Str.Substring(InStr(Str, "#"))
                Proto = Proto.Substring(0, InStr(Proto, "://") - 1)
            End If
        Catch
        End Try
        Select Case Proto.ToLower
            Case "http"
                Try
                    Cursor = Cursors.AppStarting
                    Dim url As String = Str.Substring(InStr(Str, "#"))
                    Process.Start(url)
                Catch ex As Exception
                Finally
                    Cursor = Cursors.Default
                End Try
            Case "https"
                Try
                    Cursor = Cursors.AppStarting
                    Dim url As String = Str.Substring(InStr(Str, "#"))
                    Process.Start(url)
                Catch ex As Exception
                Finally
                    Cursor = Cursors.Default
                End Try
            Case Else
                MsgBox("Protocol: """ & Proto & """ Not yet implemented")
        End Select
        'MsgBox(Proto)
    End Sub

    Public Sub UpDateDreamList(ByRef name As String) '
        Try
            If DreamList.InvokeRequired OrElse DreamCountTxtBx.InvokeRequired Then
                Invoke(New UpDateDreamListCaller(AddressOf UpDateDreamList), name)
            Else
                Dim fList As FURREList
                fList = FurcSession.Dream.FurreList

                DreamList.BeginUpdate()
                If name = "" Then
                    DreamList.Items.Clear()
                    For i As Integer = 0 To fList.Count - 1
                        If Not String.IsNullOrEmpty(fList.Item(i).Name) Then
                            DreamList.Items.Add(Web.HttpUtility.HtmlDecode(fList.Item(i).Name))
                        Else
                            DreamList.Items.RemoveAt(i)
                        End If
                    Next
                Else
                    DreamList.Items.Add(Web.HttpUtility.HtmlDecode(name))
                End If

                DreamCountTxtBx.Text = fList.Count.ToString
                DreamList.EndUpdate()
            End If
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me, FurcSession.Dream.FurreList.ToString)
            'logError = New ErrorLogging(eX, p)
        End Try
    End Sub

    Public Function setLogName(ByRef bfile As cBot) As String
        Select Case bfile.LogOption
            Case 0
                Return bfile.LogNameBase
            Case 1
                bfile.LogIdx += 1
                bfile.SaveBotSettings()
                Return bfile.LogNameBase & cBot.LogIdx.ToString
            Case 2
                Return bfile.LogNameBase & Date.Now().ToString("MM_dd_yyyy_H-mm-ss")

        End Select
        Return "Default"
    End Function

    Public Sub sndDisplay(o As Object, e As ServerReceiveEventArgs) Handles FurcSession.ServerChannelProcessed
        Try
            'data = data.Replace(vbLf, vbCrLf)
            If cBot.log Then LogStream.WriteLine(o.ToString)
            If MainSettings.TimeStamp = 1 Then
                Dim Now As String = Date.Now.ToLongTimeString
                o = Now.ToString & ": " & o.ToString
            ElseIf MainSettings.TimeStamp = 2 Then

            End If
            Dim Clr As fColorEnum = fColorEnum.DefaultColor
            Select Case e.Channel
                Case "PhoenixSpeak"
                    Clr = fColorEnum.Shout
                Case Else
                    Clr = fColorEnum.DefaultColor
            End Select

            AddDataToList(log_, o.ToString, Clr)
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub

    Private Sub DreamList_DoubleClick(sender As Object, e As EventArgs) Handles DreamList.DoubleClick
        If Not FurcSession.bConnected() Then Exit Sub
        'FurcSession.sndServer("l " + Web.HttpUtility.HtmlEncode(DreamList.SelectedItem.ToString))
    End Sub

#End Region
    Private ClientReceived As Integer = 0

    ''' <summary>
    ''' sets the Text of this form
    ''' </summary>
    ''' <param name="str"></param>
    Public Sub MainFormText(ByRef str As String)
        If InvokeRequired Then
            Dim d As New UpDateBtn_GoCallback(AddressOf MainFormText)
            Invoke(d, str)
        Else
            Me.Text = "Silver Monkey: " & str.ToString '& " " & Application.ProductVersion
            NotifyIcon1.Text = "Silver Monkey: " & str.ToString
        End If
    End Sub

    Public Function FormatText(ByVal data As String, ByVal newColor As fColorEnum) As String
        data = System.Web.HttpUtility.HtmlDecode(data)
        data = data.Replace("|", " ")

        Dim myColor As System.Drawing.Color = fColor(newColor)
        Dim ColorString As String = "{\colortbl ;"
        ColorString += "\red" & myColor.R & "\green" & myColor.G & "\blue" & myColor.B & ";}"
        Dim FontSize As Single = MainSettings.ApFont.Size
        Dim FontFace As String = MainSettings.ApFont.Name
        FontSize *= 2
        Return "{\rtf1\ansi\ansicpg1252\deff0\deflang1033" & ColorString & "{\fonttbl{\f0\fcharset0 " & FontFace & ";}}\viewkind4\uc1\fs" & FontSize.ToString & "\cf1 " & data & " \par}"
    End Function
    Public Function FormatURL(ByVal data As String) As String
        Dim FontSize As Single = MainSettings.ApFont.Size
        Dim FontFace As String = MainSettings.ApFont.Name
        FontSize *= 2
        Return "{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fcharset0 " & FontFace & ";}}\viewkind4\uc1\fs" & FontSize.ToString & " " & data & "}"
    End Function

    Private Sub FormClose()
        _FormClose = True
        My.Settings.MainFormLocation = Location
        If Not IsNothing(cBot) Then My.Settings.LastBotFile = cBot.IniFile
        'Timers.DestroyTimers()
        'Save the user settings so next time the
        'window will be the same size and location

        My.Settings.Save()
        NotifyIcon1.Visible = False
        If Not IsNothing(LogTimer) Then LogTimer.Dispose()
        If Not IsNothing(MSalarm) Then MSalarm.Dispose()
        If Not IsNothing(DreamUpdateTimer) Then DreamUpdateTimer.Dispose()

        Dispose()
    End Sub

    Private Sub Main_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try

            Select Case MainSettings.SysTray
                Case CheckState.Checked
                    Visible = False
                    e.Cancel = True
                Case CheckState.Indeterminate
                    If MessageBox.Show("Minimize to SysTray?", "", MessageBoxButtons.YesNo, Nothing,
                         MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        MainSettings.SysTray = CheckState.Checked
                        MainSettings.SaveMainSettings()
                        Visible = False
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

    Private Sub Tick(ByVal state As Object)
        If IsDisposed Then Exit Sub
        Timeupdate()
    End Sub

    Private TimeLock As New Object
    Private Sub Timeupdate()
        If _FormClose Then Exit Sub
        If MenuStrip1.InvokeRequired Then

            Dim d As New DelTimeupdate(AddressOf Timeupdate)
            If IsDisposed = False Then Invoke(d)

        Else
            SyncLock TimeLock
                FurcTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Date.Now, TimeZoneInfo.Local.Id, "Central Standard Time")
                Try

                    FurcTimeLbl.Text = "Furcadia Standard Time: " & FurcTime.ToLongTimeString
                    FurcSession.MainEngine.PageExecute(299)
                Catch ex As Exception
                    Debug.Print(ex.Message)
                End Try
            End SyncLock
        End If

    End Sub

    Private Sub Main_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If (e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control) Then

            LaunchEditor()
            'e.Handled = True
            'e.SuppressKeyPress = True
        ElseIf (e.KeyCode = Keys.F1) Then
            frmHelp.Show()
        ElseIf (e.KeyCode = Keys.N AndAlso e.Modifiers = Keys.Control) Then
            With BotSetup
                .bFile = New cBot
                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    cBot = .bFile
                End If
            End With

        End If

    End Sub
    Dim listlock As New Object
    Private Sub Main_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If NotifyIcon1 Is Nothing Then
            NotifyIcon1 = New NotifyIcon
            NotifyIcon1.ContextMenuStrip = ContextTryIcon
            NotifyIcon1.Icon = My.Resources.metal
            NotifyIcon1.BalloonTipTitle = My.Application.Info.ProductName
            NotifyIcon1.Text = My.Application.Info.ProductName + ": " + My.Application.Info.Version.ToString
            AddHandler NotifyIcon1.MouseDoubleClick, AddressOf NotifyIcon1_DoubleClick
        End If
        MainSettings = New cMain
        If Not String.IsNullOrEmpty(MainSettings.FurcPath) Then
            Paths.FurcadiaProgramFolder = MainSettings.FurcPath
        End If

        If Not NotifyIcon1.Visible Then NotifyIcon1.Visible = True
        'catch the Console messages
        _FormClose = False

        writer = New TextBoxWriter(log_)
        Console.SetOut(writer)

        MSalarm = New Threading.Timer(AddressOf Tick, True, 1000, 1000)

        ' Try to get Furcadia's path from the registry

        'MS_KeysIni.Load(Path.Combine(Paths.ApplicationPath, "Keys-MS.ini"))
        InitializeTextControls()

        Size = My.Settings.MainFormSize
        Location = My.Settings.MainFormLocation
        Text = "Silver Monkey: " & Application.ProductVersion
        Visible = True

        LoadRecentList()
        For Each item As String In MRUlist
            Dim fileRecent As New ToolStripMenuItem(item, Nothing, AddressOf RecentFile_click)
            'create new menu for each item in list
            'add the menu to "recent" menu
            RecentToolStripMenuItem.DropDownItems.Add(fileRecent)
        Next
        EditBotToolStripMenuItem.Enabled = False
        If (My.Application.CommandLineArgs.Count > 0) Then
            Dim File As String = My.Application.CommandLineArgs(0)
            Dim directoryName As String = Path.GetDirectoryName(File)
            If String.IsNullOrEmpty(directoryName) Then
                File = Path.Combine(Paths.SilverMonkeyBotPath, File)
            Else
                Paths.SilverMonkeyBotPath = directoryName
            End If
            cBot = New cBot(File)
            EditBotToolStripMenuItem.Enabled = True
            Console.WriteLine("Loaded: """ + File + """")
        ElseIf MainSettings.LoadLastBotFile And Not String.IsNullOrEmpty(My.Settings.LastBotFile) Then
            cBot = New cBot(My.Settings.LastBotFile)
            'EditBotToolStripMenuItem.Enabled = True
            Console.WriteLine("Loaded: """ + My.Settings.LastBotFile + """")
        End If

        If Not cBot Is Nothing Then
            If cBot.AutoConnect Then
                FurcSession.ConnectBot()
            End If
        End If
    End Sub

    Public Sub ConnectionControlEnable()

    End Sub
    Public Sub ConnectionControlDisEnable()
        EditBotToolStripMenuItem.Enabled = False
    End Sub

    Public Sub InitializeTextControls()
        log_.Font = MainSettings.ApFont
        toServer.Font = MainSettings.ApFont
        DreamList.Font = MainSettings.ApFont
        DreamCountTxtBx.Font = MainSettings.ApFont
    End Sub

#Region "Action Controls"

    Private Sub ActionTmr_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles ActionTmr.Tick
        If Not FurcSession.bConnected() Then Exit Sub
        FurcSession.sndServer(ActionCMD)
    End Sub
    Private Sub _ne_Click(sender As Object, e As EventArgs) Handles _ne.Click
        FurcSession.sndServer("`m 9")
    End Sub

    Private Sub _nw_Click(sender As Object, e As EventArgs) Handles _nw.Click
        FurcSession.sndServer("`m 7")
    End Sub

    Private Sub BtnSit_stand_Lie_Click(sender As Object, e As EventArgs) Handles BtnSit_stand_Lie.Click
        If Not FurcSession.bConnected() Then Exit Sub
        If BtnSit_stand_Lie.Text = "Stand" Then
            BtnSit_stand_Lie.Text = "Lay"
        ElseIf BtnSit_stand_Lie.Text = "Lay" Then
            BtnSit_stand_Lie.Text = "Sit"
        ElseIf BtnSit_stand_Lie.Text = "Sit" Then
            BtnSit_stand_Lie.Text = "Stand"
        End If
        FurcSession.sndServer("`lie")
    End Sub

    Private Sub BTN_TurnR_Click(sender As Object, e As EventArgs) Handles BTN_TurnR.Click
        FurcSession.sndServer("`>")
    End Sub

    Private Sub BTN_TurnL_Click(sender As Object, e As EventArgs) Handles BTN_TurnL.Click
        FurcSession.sndServer("`<")
    End Sub

    Private Sub use__Click(sender As Object, e As EventArgs) Handles use_.Click
        FurcSession.sndServer("`use")
    End Sub

    Private Sub get__Click(sender As Object, e As EventArgs) Handles get_.Click
        FurcSession.sndServer("`get")
    End Sub

    Private Sub sw__Click(sender As Object, e As EventArgs) Handles sw_.Click
        FurcSession.sndServer("`m 1")
    End Sub

    Private Sub sw__MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles sw_.MouseDown
        ActionTmr.Enabled = FurcSession.bConnected()
        ActionCMD = "`m 1"
    End Sub

    Private Sub sw__MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles sw_.MouseUp
        ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    Private Sub _ne_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _ne.MouseDown
        ActionTmr.Enabled = FurcSession.bConnected()
        ActionCMD = "`m 9"
    End Sub

    Private Sub _ne_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _ne.MouseUp
        ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    Private Sub _nw_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _nw.MouseDown
        ActionTmr.Enabled = FurcSession.bConnected()
        ActionCMD = "`m 7"
    End Sub

    Private Sub _nw_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _nw.MouseUp
        ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    Private Sub se__Click(sender As Object, e As EventArgs) Handles se_.Click
        FurcSession.sndServer("`m 3")
    End Sub

    Private Sub se__MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles se_.MouseDown
        ActionTmr.Enabled = FurcSession.bConnected()
        ActionCMD = "`m 3"
    End Sub

    Private Sub se__MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles se_.MouseUp
        ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub
#End Region

    Private Sub ConfigToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ConfigToolStripMenuItem.Click
        Dim test As New Config
        test.Show()
        test.Activate()
    End Sub

    Private Sub DebugToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DebugToolStripMenuItem.Click
        Variables.Show()
        Variables.Activate()
    End Sub

    Private Sub BTN_Go_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_Go.Click, ConnectTrayIconMenuItem.Click, DisconnectTrayIconMenuItem.Click
        If IsNothing(cBot) Then Exit Sub
        If String.IsNullOrEmpty(cBot.IniFile) Then Exit Sub
        If cBot.IniFile = "-pick" Then Exit Sub

        Dim p As String = Path.GetDirectoryName(cBot.IniFile)
        If String.IsNullOrEmpty(p) And Not File.Exists(cBot.IniFile) Then
            If MessageBox.Show(cBot.IniFile + " Not found, Aborting connection!", "Important Message") = DialogResult.OK Then
                Exit Sub
            End If
        End If
        If BTN_Go.Text = "Go!" Then

            If Not FurcSession.MainEngine.ScriptStart() Then Exit Sub
            My.Settings.LastBotFile = Path.Combine(Paths.SilverMonkeyBotPath, cBot.IniFile)
            My.Settings.Save()
            FurcSession.RelogCounter = 0
            FurcSession.ClientClose = False

            If Not IsNothing(MS_Export) Then MS_Export.Dispose()
            Try
                FurcSession.ConnectBot()
            Catch Ex As NetProxyException
                FurcSession.DisconnectBot()
                '   sndDisplay("Connection Aborting: " + Ex.Message, fColorEnum.DefaultColor)
            End Try

        Else

            FurcSession.DisconnectBot()

        End If
    End Sub

    Public Sub SetBalloonText(ByRef txt As String)
        'If Me.NotifyIcon1.Then Then
        '    Dim d As New UpDateBtn_GoCallback(AddressOf SetBalloonText)
        '    Me.Invoke(d, txt, {[Text]})
        'Else
        NotifyIcon1.BalloonTipText = txt
        NotifyIcon1.ShowBalloonTip(3000)
        'End If
    End Sub

    Private Sub toServer_KeyDown(sender As Object, e As KeyEventArgs) Handles toServer.KeyDown
        'Command History
        If (e.KeyCode = Keys.I AndAlso e.Modifiers = Keys.Control) Then

            If CMD_Idx2 < 0 AndAlso CMD_Lck = True Then
                CMD_Idx2 = CMD_Max - 1
            ElseIf CMD_Idx2 < 0 AndAlso CMD_Lck = False Then
                CMD_Idx2 = CMD_Idx
            End If
            toServer.Text = ""
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
            'toServer.Text = toServer.Text.Replace(vbLf, "")
            If FurcSession.bConnected() = True Then
                SendTxtToServer()
            End If
            e.SuppressKeyPress = True
            e.Handled = True
        End If

    End Sub

    Private Sub sendToServer_Click(ByVal sender As Object, ByVal e As EventArgs) Handles sendToServer.Click
        If Not FurcSession.bConnected() Then Exit Sub
        SendTxtToServer()
    End Sub

    Private Sub SendTxtToServer()

        'Filter Blank lines from triggering
        If toServer.Text = "" Then
            'toServer.Text = ""
            Exit Sub
        End If

        Dim Txt As String = toServer.Rtf
        'toServer.Rtf = Txt

        If CMD_Idx = CMD_Max Then
            CMD_Idx = 0
            CMD_Lck = True
        End If
        CMDList(CMD_Idx) = Txt
        CMD_Idx2 = CMD_Idx
        CMD_Idx += 1
        Txt = Txt.Replace("\b0 ", "</b>")
        Txt = Txt.Replace("\b ", "<b>")
        Txt = Txt.Replace("\i0 ", "</i>")
        Txt = Txt.Replace("\i ", "<i>")
        Txt = Txt.Replace("\ul0 ", "</ul>")
        Txt = Txt.Replace("\ul ", "<ul>")

        toServer.Rtf = Txt
        toServer.Text = TagCloser(toServer.Text, "b")
        toServer.Text = TagCloser(toServer.Text, "i")
        toServer.Text = TagCloser(toServer.Text, "ul")

        FurcSession.TextToServer(toServer.Text)
        toServer.Text = ""
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

    Private Sub BTN_Underline_Click(sender As Object, e As EventArgs) Handles BTN_Underline.Click
        FormatRichTextBox(toServer, System.Drawing.FontStyle.Underline)
    End Sub

    Private Sub Btn_Bold_Click(sender As Object, e As EventArgs) Handles Btn_Bold.Click
        FormatRichTextBox(toServer, System.Drawing.FontStyle.Bold)
    End Sub

    Private Sub BTN_Italic_Click(sender As Object, e As EventArgs) Handles BTN_Italic.Click
        FormatRichTextBox(toServer, System.Drawing.FontStyle.Italic)
    End Sub

    Public Sub FormatRichTextBox(ByRef TB As RichTextBoxEx,
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

    Private Sub MSEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MSEditorToolStripMenuItem.Click, EditorTrayIconMenuItem.Click
        LaunchEditor()
    End Sub

    Private Sub LaunchEditor()
        If IsNothing(cBot) OrElse String.IsNullOrEmpty(cBot.MS_File) Then
            Dim result As Integer = MessageBox.Show("No Botfile Loaded", "caption", MessageBoxButtons.OK)
            If result = DialogResult.OK Then
                Exit Sub
            End If

        End If
        Dim processStrt As New ProcessStartInfo
        processStrt.FileName = My.Application.Info.DirectoryPath + Path.DirectorySeparatorChar + "MonkeySpeakEditor.EXE"
        Dim f As String = cBot.MS_File
        If Not String.IsNullOrEmpty(f) Then
            Dim dir As String = Path.GetDirectoryName(cBot.MS_File)
            If String.IsNullOrEmpty(dir) Then
                f = Path.Combine(Paths.SilverMonkeyBotPath, cBot.MS_File)
            End If
        End If

        If Not String.IsNullOrEmpty(FurcSession.BotName) And Not String.IsNullOrEmpty(cBot.MS_File) Then
            processStrt.Arguments = "-B=""" + FurcSession.BotName + """ """ + f + """"
        ElseIf String.IsNullOrEmpty(FurcSession.BotName) And Not String.IsNullOrEmpty(cBot.MS_File) Then
            processStrt.Arguments = """" + f + """"
        End If
        Process.Start(processStrt)
    End Sub

    Private Sub NotifyIcon1_Disposed(sender As Object, e As EventArgs)
        Visible = False
    End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As EventArgs)
        If Not IsNothing(NotifyIcon1) Then

        End If
        Show()
        Activate()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click, ExitTrayIconMenuItem.Click
        FormClose()
    End Sub

    Private Sub ContextTryIcon_Opened(sender As Object, e As EventArgs) Handles ContextTryIcon.Opened
        Select Case FurcSession.LoggingIn
            Case 0
                DisconnectTrayIconMenuItem.Enabled = False
                ConnectTrayIconMenuItem.Enabled = True
            Case 1
                DisconnectTrayIconMenuItem.Enabled = True
                ConnectTrayIconMenuItem.Enabled = False
            Case 2 Or 3
                DisconnectTrayIconMenuItem.Enabled = True
                ConnectTrayIconMenuItem.Enabled = False

        End Select
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        toServer.Paste()
    End Sub

    Private Sub MenuCut_Click(sender As Object, e As EventArgs) Handles MenuCut.Click
        toServer.Cut()
    End Sub

    Private Sub MenuCopy_Click(sender As Object, e As EventArgs) Handles MenuCopy.Click
        toServer.Copy()
    End Sub

    Private Sub MenuCopy2_Click(sender As Object, e As EventArgs) Handles MenuCopy2.Click
        log_.Copy()
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem1.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub ContentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ContentsToolStripMenuItem.Click
        If File.Exists(Application.StartupPath & "/Silver Monkey.chm") Then
            Process.Start(Application.StartupPath & "/Silver Monkey.chm")
        Else

        End If

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

    Private Sub log__MouseHover(sender As Object, e As EventArgs) Handles log_.MouseHover, log_.MouseLeave, log_.MouseEnter, log_.CursorChanged
        If Cursor.Current = Cursors.Hand Then
            ToolTip1.Show(curWord, log_)
        Else
            ToolTip1.Hide(log_)
        End If
    End Sub

    Private Sub log__MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles log_.MouseMove
        If Cursor.Current = Cursors.Hand Or Cursor.Current = Cursors.Default Then
            curWord = GetWordUnderMouse(log_, e.X, e.Y)
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        log_.HideSelection = Not CheckBox1.Checked
    End Sub

    Private Sub TSTutorialsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TSTutorialsToolStripMenuItem.Click
        'Process.Start("http://www.ts-projects.org/tutorials/")
    End Sub

    Private Sub ExportMonkeySpeakToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportMonkeySpeakToolStripMenuItem.Click
        If FurcSession.LoggingIn > 0 Then Exit Sub
        MS_Export.Show()
        MS_Export.Activate()
    End Sub

    Public Function IsBot(ByRef player As FURRE) As Boolean

        If BtnSit_stand_Lie.InvokeRequired Then
            Dim d As New UpDateBtn_StandCallback(AddressOf IsBot)
            Invoke(d, [player])
            Return True
        Else
            'Update inteface
            Try

                Select Case player.FrameInfo.pose
                    Case 0
                        BtnSit_stand_Lie.Text = "Stand"
                    Case 1 To 3
                        BtnSit_stand_Lie.Text = "Lay"
                    Case 4
                        BtnSit_stand_Lie.Text = "Sit"
                End Select
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            Return FurcSession.isBot(player)
        End If
    End Function

    Sub ContentsToolStripMenuItemClick(sender As Object, e As EventArgs)

    End Sub
End Class