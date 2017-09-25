Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports FastColoredTextBoxNS
Imports MonkeyCore
Imports MonkeyCore.Controls
Imports MonkeyCore.IniFile
Imports MonkeySpeakEditor.Controls
Imports MonkeySpeakEditor.Controls.LineFinder

''' <summary>
''' Silver Monkey Main Form
''' </summary>
Public Class MS_Edit

#Region "Private Fields"

    Private Const HelpFile As String = "Monkey_Speak_Editor_Help.chm"
    'Private Const MonkeySpeakLineHelp As String = "Silver Monkey.chm"

#End Region

#Region "Public Fields"

    Public Shared EditSettings As MonkeyCore.Settings.EditSettings

    Public DS_autoCompleteList As New List(Of AutocompleteItem)

    Public MS_autoCompleteList As New List(Of AutocompleteItem)

#End Region



#Region "Private Fields"

    Private frm As frmSearch = New frmSearch

    Dim LastFoundIndex As Integer = 0

    Dim LastSearchString As String = String.Empty

    Dim lisView As Integer = 1

    Private popupMenu As AutocompleteMenu

    Dim SectionLstIdx As Integer = 0

    Dim SectionLstIdxOld As Integer = 0

    Dim tablock As New Object

#End Region

#Region "Public Constructors"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

#End Region

#Region "Private Delegates"

    Private Delegate Sub ListBoxInvoker(ByVal Sender As Object, e As EventArgs)

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' </summary>
    ''' <returns>
    ''' </returns>
    Public Shared Property ini As IniFile
        Get
            Return Settings.ini
        End Get
        Set(value As IniFile)
            Settings.ini = value
        End Set
    End Property

    Public Shared Property KeysIni As IniFile
        Get
            Return Settings.KeysIni
        End Get
        Set(value As IniFile)
            Settings.KeysIni = value
        End Set
    End Property

    Public Shared Property MS_KeysIni As IniFile
        Get
            Return Settings.MS_KeysIni
        End Get
        Set(value As IniFile)
            Settings.MS_KeysIni = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Sub AddNewEditorTab(ByRef FileName As String, FilePath As String, ByRef n As Integer)
        SyncLock tablock
            Dim tp As New TabPage

            tp.Text = New_File_Tag + "     "
            TabControl2.TabPages.Add(tp)
            Dim intLastTabIndex As Integer = TabControl2.TabPages.Count - 1
            tp.Name = "tbpageBrowser" & intLastTabIndex.ToString
            'Adds a new tab to your tab control
            CanOpen.Add(True)
            WorkFileName.Add(FileName)
            WorkPath.Add(FilePath)
            BotName.Add("none")

            frmTitle.Add(AppName + " - New DragonSpeak File")
            SectionIdx.Add(0)
            TabEditStyles.Add(EditStyles.ds)
            FullFile.Add(New List(Of String))
            'Gets the index number of the last tab
            TabSections.Add(New List(Of TDSSegment))

            'Creates the listview and displays it in the new tab
            Dim lstView As FastColoredTextBox = New FastColoredTextBox()

            lstView.ContextMenuStrip = EditMenu
            lstView.AcceptsTab = True
            lstView.Parent = tp
            lstView.Anchor = CType(AnchorStyles.Left & AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Right, AnchorStyles)
            lstView.Name = "edit" + n.ToString
            lstView.AutoIndent = False
            lstView.Dock = DockStyle.Fill
            lstView.CommentPrefix = "*"
            lstView.Language = Language.Custom
            lstView.Show()
            lstView.ContextMenuStrip = SectionMenu
            TabControl2.SelectedTab = TabControl2.TabPages(intLastTabIndex)

            AddHandler lstView.TextChangedDelayed, AddressOf MS_Editor_TextChangedDelayed
            AddHandler lstView.TextChanged, AddressOf MS_Editor_TextChanged
            AddHandler lstView.MouseUp, AddressOf MS_EditRightClick
            AddHandler lstView.CursorChanged, AddressOf MS_Editor_CursorChanged
            AddHandler lstView.MouseClick, AddressOf MS_Editor_CursorChanged
            AddHandler lstView.KeyUp, AddressOf MS_Editor_CursorChanged
            AddHandler lstView.MouseDoubleClick, AddressOf MS_Editor_MouseDoubleClick

        End SyncLock
    End Sub

    Public Function FileTab(ByRef File As String) As Integer
        Dim f As String = Path.GetFileName(File)
        Dim p As String = Path.GetDirectoryName(File)
        For I = 0 To TabControl2.TabPages.Count - 1
            If WorkFileName(I) = f And WorkPath(I) = p Then
                Return I
            End If
        Next
        Return -1
    End Function

    Public Function IsEditorOpen(ByRef File As String) As Boolean
        Dim f As String = Path.GetFileName(File)
        Dim p As String = Path.GetDirectoryName(File)
        For I = 0 To TabControl2.TabPages.Count - 1
            If WorkFileName(I) = f And WorkPath(I) = p Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function IsEditorOpen(ByRef File As String, ByRef path As String) As Boolean
        For I = 0 To TabControl2.TabPages.Count - 1
            If WorkFileName(I) = File And WorkPath(I) = path Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub NewFile(ByVal style As EditStyles)
        TabSections(TabControl2.SelectedIndex).Clear()
        WorkFileName(TabControl2.SelectedIndex) = ""
        TabEditStyles(TabControl2.SelectedIndex) = style

        If style = EditStyles.ms Then
            MS_Editor.Text = MonkeyCore.IO.NewMSFile()
            lblStatus.Text = "Status: Opened New MonkeySpeak  File "
            popupMenu = New AutocompleteMenu(MS_Editor)
            popupMenu.Enabled = True

            popupMenu.SearchPattern = AutoCompleteSearchPattern
            popupMenu.Items.MaximumSize = New Size(600, 300)
            popupMenu.Items.Width = 600
            popupMenu.Items.SetAutocompleteItems(MS_autoCompleteList)
        ElseIf style = EditStyles.ds Then
            MS_Editor.Text = MonkeyCore.IO.NewDSFile()
            lblStatus.Text = "Status: Opened New DragonSpeak  File "
            popupMenu = New AutocompleteMenu(MS_Editor)
            popupMenu.Enabled = True

            popupMenu.SearchPattern = AutoCompleteSearchPattern
            popupMenu.Items.MaximumSize = New Size(600, 300)
            popupMenu.Items.Width = 600
            popupMenu.Items.SetAutocompleteItems(DS_autoCompleteList)
        ElseIf style = EditStyles.ini Then
            MS_Editor.Text = IO.NewDMScript()
            lblStatus.Text = "Status: Opened new Wizard Script"
        Else
            MS_Editor.Text = ""
            lblStatus.Text = "Status: Opened new blank file"
        End If

        frmTitle(TabControl2.SelectedIndex) = AppName + " - " + New_File_Tag
        FullFile(TabControl2.SelectedIndex).Clear()
        Text = frmTitle(TabControl2.SelectedIndex)
        For i = 0 To MS_Editor.Lines.Count - 1
            FullFile(TabControl2.SelectedIndex).Add(MS_Editor.Lines.Item(i).Trim(charsToTrim))
        Next
        TabControl2.SelectedTab.Text = New_File_Tag
        CanOpen(TabControl2.SelectedIndex) = True
        TabControl2.RePositionCloseButtons(TabControl2.SelectedTab)
        UpdateSegments()
        UpdateSegmentList()

        SetLineTabs(TabControl2.SelectedIndex)

    End Sub

    Public Sub OpenMS_File(ByRef filename As String, Optional ByRef bName As String = "none")
        Dim f As String = Path.GetFileName(filename)
        Dim p As String = Path.GetDirectoryName(filename)

        If IsEditorOpen(filename) Then
            TabControl2.SelectedIndex = FileTab(filename)
            BotName(FileTab(filename)) = bName
            If Not CanOpen(TabControl2.SelectedIndex) Then
                Dim msg = "File Contents have changed. Reload the file discarding changes?"
                Dim title = "Refresh Page"
                Dim style = MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton2 Or
                            MsgBoxStyle.Information
                Dim response = MsgBox(msg, style, title)
                If response = MsgBoxResult.Cancel Then
                    Exit Sub
                Else

                End If

            End If
        Else
            AddNewEditorTab(f, p, TabControl2.TabPages.Count)
        End If
        FullFile(TabControl2.SelectedIndex).Clear()
        TabSections(TabControl2.SelectedIndex).Clear()
        SectionIdx(TabControl2.SelectedIndex) = 0
        SectionLstIdxOld = 0
        SectionLstIdx = 0

        WorkFileName(TabControl2.SelectedIndex) = f
        WorkPath(TabControl2.SelectedIndex) = p
        BotName(TabControl2.SelectedIndex) = bName
        Dim ext As String = Path.GetExtension(filename)

        frmTitle(TabControl2.SelectedIndex) = AppName + " - " & WorkFileName(TabControl2.SelectedIndex)
        Text = frmTitle(TabControl2.SelectedIndex)

        Try
            Dim reader As New StreamReader(Path.Combine(WorkPath(TabControl2.SelectedIndex), WorkFileName(TabControl2.SelectedIndex)))
            MS_Editor.Text = ""
            Do While reader.Peek <> -1
                Dim line As String = reader.ReadLine
                FullFile(TabControl2.SelectedIndex).Add(line)
            Loop
            MS_Editor.Text = String.Join(vbCrLf, FullFile(TabControl2.SelectedIndex).ToArray)
            reader.Close()
            lblStatus.Text = "Status: opened " & WorkFileName(TabControl2.SelectedIndex)
            UpdateSegments()
            TabControl2.SelectedTab.Text = WorkFileName(TabControl2.SelectedIndex)
            CanOpen(TabControl2.SelectedIndex) = True
        Catch ex As Exception
            Select Case ext.ToLower
                Case ".ds"
                    MS_Editor.Text = MonkeyCore.IO.NewDSFile()
                Case ".ms"
                    MS_Editor.Text = MonkeyCore.IO.NewMSFile()
                Case Else
                    MS_Editor.Text = ""
            End Select
            TabControl2.SelectedTab.Text = "*" + WorkFileName(TabControl2.SelectedIndex)
            CanOpen(TabControl2.SelectedIndex) = False
            lblStatus.Text = "New File: " & WorkFileName(TabControl2.SelectedIndex) & " doesn't exist Creating new File"
        End Try

        If ext.ToLower = ".ds" Then
            TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ds
            popupMenu = New AutocompleteMenu(MS_Editor)
            popupMenu.Enabled = True

            popupMenu.SearchPattern = AutoCompleteSearchPattern
            popupMenu.Items.MaximumSize = New System.Drawing.Size(600, 300)
            popupMenu.Items.Width = 600
            popupMenu.Items.SetAutocompleteItems(DS_autoCompleteList)
        ElseIf ext.ToLower = ".ms" Then
            TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ms
            popupMenu = New AutocompleteMenu(MS_Editor)
            popupMenu.Enabled = True

            popupMenu.SearchPattern = AutoCompleteSearchPattern
            popupMenu.Items.MaximumSize = New System.Drawing.Size(600, 300)
            popupMenu.Items.Width = 600
            popupMenu.Items.SetAutocompleteItems(MS_autoCompleteList)
        ElseIf ext.ToLower = ".ini" Then
            TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ini
        Else
            TabEditStyles(TabControl2.SelectedIndex) = EditStyles.none
        End If

        SetLineTabs(TabControl2.SelectedIndex)
        TabControl2.RePositionCloseButtons(TabControl2.SelectedTab)
        UpdateSegments()
        UpdateSegmentList()
    End Sub

    Public Function RegExEscapedSring(ByVal text As String) As String
        text = text.Replace("\", "\\")
        text = text.Replace(".", "\.")
        text = text.Replace("$", "\$")
        text = text.Replace("^", "\^")
        text = text.Replace("{", "\{")
        text = text.Replace("[", "\[")
        text = text.Replace("(", "\(")
        text = text.Replace("|", "\|")
        text = text.Replace("}", "\}")
        text = text.Replace(")", "\)")
        text = text.Replace("]", "\]")
        text = text.Replace("*", "\*")
        text = text.Replace("+", "\+")
        text = text.Replace("?", "\?")
        Return text

    End Function

    Public Sub Reset()
        If TabControl2.TabPages.Count = 0 Then Exit Sub
        SetDSHilighter()
        SetMSHilighter()
        For i = 0 To TabControl2.TabPages.Count - 1
            MS_Editor(i).Invalidate()
        Next

    End Sub

    Public Sub SetLineTabs(ByRef Idx As Integer)
        'Debug.Print("SetLineTabs()")
        If TabEditStyles(Idx) = EditStyles.ds Then
            'line Tabs
            SplitContainer4.Panel1Collapsed = False
            SplitContainer4.Panel2Collapsed = True

            'Templates
            SplitContainer5.Panel1Collapsed = True
            SplitContainer5.Panel2Collapsed = False

            ListBox2.Location = New Point(5, 3)
            ListBox2.Size = New Size(99, 147)
            TabControl1.TabPages.Item(2).Text = "DragonSpeak Help"
            Label1.Text = "DragonSpeak Line Help"

        ElseIf TabEditStyles(Idx) = EditStyles.ms Then
            'line tabs
            SplitContainer4.Panel1Collapsed = True
            SplitContainer4.Panel2Collapsed = False

            'templates
            SplitContainer5.Panel1Collapsed = False
            SplitContainer5.Panel2Collapsed = True

            ListBox3.Location = New Point(5, 3)
            ListBox3.Size = New Size(99, 147)

            TabControl1.TabPages.Item(2).Text = "MonkeySpeak Help"
            Label1.Text = "MonkeySpeak Line Help"
        End If

        'Debug.Print("Setting Template Lists Size/Location")
        'Debug.Print("TemplateList.Size: " + Templatelist.Size.ToString)

    End Sub

    ''' <summary>
    ''' Read a CSV line
    ''' </summary>
    ''' <param name="input">
    ''' </param>
    ''' <returns>
    ''' </returns>
    Public Function SplitCSV(ByRef input As String) As String()

        Try
            Dim afile As New FileIO.TextFieldParser(New StringReader(input))
            afile.TextFieldType = FileIO.FieldType.Delimited
            afile.Delimiters = New String() {","}
            afile.HasFieldsEnclosedInQuotes = True
            Return afile.ReadFields
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try

        Return Nothing
    End Function

    Public Sub UpdateSegmentList()
        UpdateSegmentList(TabControl2.SelectedIndex)
    End Sub

    Public Sub UpdateSegmentList(ByRef idx As Integer)
        Dim tseg As TDSSegment
        Dim Sects_Indent As String = Space(4)
        With ListBox1
            .Items.Clear()
            .Items.Add(RES_DSS_All)
            For i = 0 To TabSections(idx).Count - 1

                tseg = TabSections(idx)(i)
                If (tseg.Title <> RES_DSS_begin) And (tseg.Title <> RES_DSS_End) And (tseg.Title <> RES_MSS_begin) And (tseg.Title <> RES_MSS_End) Then
                    If ((tseg.Title = RES_Def_section) And ((i <= 1))) Then

                        .Items.Add(TabSections(idx)(i).Title)
                    Else
                        .Items.Add(Sects_Indent + TabSections(idx)(i).Title)
                    End If
                Else
                    .Items.Add(TabSections(idx)(i).Title)
                End If

            Next
        End With
        ListBox1.SelectedIndex = SectionIdx(TabControl2.SelectedIndex)
    End Sub

    Public Sub UpdateSegments()
        UpdateSegments(TabControl2.SelectedIndex)
    End Sub

    Public Sub UpdateSegments(ByRef idx As Integer)
        Debug.Print("UpdateSegments()")
        Dim tmpsec As TDSSegment = New TDSSegment
        Dim sec2 As TDSSegment = New TDSSegment
        Dim bypass As Boolean = False
        Dim t1 As String = ""
        Dim blank As Boolean = False
        'Dim TabSections As List(Of Dictionary(Of String, TDSSegment))
        If Not IsNothing(TabSections) Then TabSections(idx).Clear()

        'Build from the basics

        tmpsec.Title = RES_Def_section

        TabSections(idx).Add(tmpsec)
        bypass = False

        For i = 0 To FullFile(idx).Count - 1

            If FullFile(idx)(i).StartsWith(RES_DS_begin) Then
                sec2.Title = RES_DSS_begin
                sec2.lines.Add(FullFile(idx)(i))
                sec2.SecType = TSecType.SecFixed
                bypass = True
                TabSections(idx).Insert(0, sec2)
            ElseIf FullFile(idx)(i).StartsWith(RES_MS_begin) Then
                sec2.Title = RES_MSS_begin
                sec2.lines.Add(FullFile(idx)(i))
                sec2.SecType = TSecType.SecFixed
                bypass = True
                TabSections(idx).Insert(0, sec2)
            End If
            'Ending segment

            If FullFile(idx)(i) = RES_DS_end Then
                tmpsec = New TDSSegment
                tmpsec.Title = RES_DSS_End
                tmpsec.SecType = TSecType.SecEnd
                'tmpsec.lines.Add(FullFile(idx)(i))
                TabSections(idx).Add(tmpsec)
                bypass = False
            ElseIf FullFile(idx)(i) = RES_MS_end Then
                tmpsec = New TDSSegment
                tmpsec.Title = RES_MSS_End
                tmpsec.SecType = TSecType.SecEnd
                'tmpsec.lines.Add(FullFile(idx)(i))
                TabSections(idx).Add(tmpsec)
                bypass = False

            End If
            If (Not IsNothing(FullFile(idx)(i))) And (tmpsec.Title = RES_Def_section) And i = 1 Then
                blank = False
                bypass = False
            End If
            ' Section marker
            If FullFile(idx)(i).StartsWith(RES_SEC_Marker) Then

                t1 = FullFile(idx)(i).Substring(RES_SEC_Marker.Length)

                tmpsec = New TDSSegment
                tmpsec.Title = t1
                TabSections(idx).Add(tmpsec)

                bypass = False
            ElseIf Not bypass Then
                tmpsec.lines.Add(FullFile(idx)(i))
                bypass = False
            End If
        Next

    End Sub

#End Region

#Region "Private Methods"

    Private Sub AbutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AbutToolStripMenuItem.Click
        AboutBox2.Show()
        AboutBox2.Activate()
    End Sub

    ''' <summary>
    ''' Add a new MS/DS editor tab
    ''' </summary>
    ''' <param name="n">
    ''' </param>
    ''' <param name="VL_Name">
    ''' </param>
    ''' <param name="lst">
    ''' </param>
    ''' <param name="LineTab">
    ''' </param>
    Private Sub AddNewTab(ByRef n As String, ByRef VL_Name As String, ByRef lst As List(Of String), ByRef LineTab As TabControl)
        LineTab.TabPages.Add(n)
        'Adds a new tab to your tab control

        Dim intLastTabIndex As Integer = LineTab.TabPages.Count - 1
        'Gets the index number of the last tab

        LineTab.TabPages(intLastTabIndex).Name = "tbpageBrowser" & LineTab.TabPages.Count
        'LineTab.SelectedTab = LineTab.TabPages(intLastTabIndex)

        'Creates the listview and displays it in the new tab
        Dim LineFinderListView As New ListView_NoFlicker()

        LineFinderListView.Tag = n
        LineFinderListView.Dock = DockStyle.Fill
        LineFinderListView.Sorting = SortOrder.Ascending
        LineFinderListView.Columns.Add(n)
        LineFinderListView.Location = New System.Drawing.Point(6, 3)
        LineFinderListView.Height = LineTab.Height
        LineFinderListView.Width = LineTab.Width
        LineFinderListView.MultiSelect = False
        LineFinderListView.BeginUpdate()
        For Each t In lst
            LineFinderListView.Items.Add(t)
        Next
        LineFinderListView.EndUpdate()
        LineFinderListView.Parent = LineTab.TabPages(intLastTabIndex)
        LineFinderListView.ListViewItemSorter = New MsDsCustomeSorter
        LineFinderListView.HeaderStyle = ColumnHeaderStyle.None
        LineFinderListView.Name = VL_Name
        LineFinderListView.FullRowSelect = True
        LineFinderListView.View = View.Details
        LineFinderListView.Columns(0).Width() = LineFinderListView.Width
        AddHandler LineFinderListView.DoubleClick, AddressOf ListCauses_DoubleClick
        AddHandler LineFinderListView.Resize, AddressOf ListView_resize
        AddHandler LineFinderListView.MouseClick, AddressOf ListCauses_MouseClick
        AddHandler LineFinderListView.SelectedIndexChanged, AddressOf ListCauses_MouseClick
        'This covers a very strange order bug involving sorted listboxes
        'Without this (or a .Show()) it will never actually update the internal list order
        'until it becomes visible in the application. (Triggering .Sort() doesn't work)
        LineFinderListView.Visible = True

    End Sub

    Private Sub ApplyCommentToolStripMenuItem1_Click(sender As Object, e As EventArgs) _
        Handles BtnComment.Click, ApplyCommentToolStripMenuItem.Click, AutocommentOnToolStripMenuItem.Click

        If IsNothing(MS_Editor) Then Exit Sub

        Dim str() As String = MS_Editor.SelectedText.Replace(vbCr, "").Split(Convert.ToChar(vbLf))
        If str.Length > 1 Then
            For i As Integer = 0 To str.Length - 1
                If str(i).Length > 0 Then
                    str(i) = "*" + str(i)
                End If
            Next
            MS_Editor.SelectedText = String.Join(Environment.NewLine, str)

        End If
    End Sub

    Private Sub AutoCommentOffToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles AutoCommentOffToolStripMenuItem.Click, AutocommentOffToolStripMenuItem1.Click

        If IsNothing(MS_Editor) Then Exit Sub

        Dim str() As String = MS_Editor.SelectedText.Replace(vbCr, "").Split(Convert.ToChar(vbLf))
        If str.Length > 1 Then
            For i As Integer = 0 To str.Length - 1
                If str(i).StartsWith("*") Then str(i) = str(i).Remove(0, 1)
            Next
            MS_Editor.SelectedText = String.Join(Environment.NewLine, str)

        End If
    End Sub

    Private Sub BtnFind_Click(sender As Object, e As EventArgs) Handles BtnFind.Click
        'Reset the starting index to Zero as the Search has changed

        Dim Test2 As Boolean = False

        Dim tbc As TabControl = Causes
        ' First check DS File
        Dim iFile As IniFile = KeysIni
        If TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ms Then
            'Then we check MS File
            tbc = TabControl3
            iFile = MS_KeysIni
        End If
        Debug.WriteLine(tbc.Name + ".TabPages.Count: " & tbc.TabPages.Count)
        Debug.WriteLine("Control count: " & tbc.Controls.Find("1", True).Count)
        Dim LV1 As ListView_NoFlicker = CType(tbc.TabPages.Item(lisView - 1).Controls.Find(lisView.ToString, True)(0), ListView_NoFlicker)

        LV1.Items(LastFoundIndex).Selected = False

        LV1.Items(LastFoundIndex).BackColor = Color.White
        LV1.Items(LastFoundIndex).ForeColor = Color.Black
        If TxtBxFind.Text <> LastSearchString Then
            LastFoundIndex = 0
            lisView = 1
        Else
            LastFoundIndex += 1
        End If
        For lis As Integer = lisView To Integer.Parse(iFile.GetKeyValue("Init-Types", "Count"))
            LV1 = CType(tbc.TabPages.Item(lis - 1).Controls.Find(lis.ToString, True)(0), ListView_NoFlicker)

            With LV1
                For i As Integer = LastFoundIndex To .Items.Count - 1

                    Dim ItemStr As String = .Items(i).SubItems(0).Text
                    If ItemStr.Trim.ToLower.Contains(TxtBxFind.Text.Trim.ToLower) Then

                        Dim tmp As ListViewItem = .Items(i)
                        'Debug.Print(i.ToString() + ":" + TxtBxFind.Text.ToLower + ":" + .Items(i).SubItems(0).Text + ":" + .Items(i).Text)
                        'Debug.Print(tmp.Text)

                        tbc.SelectedTab = tbc.TabPages.Item(lis - 1)

                        .Items(i).EnsureVisible()
                        .Items(i).BackColor = Color.Blue
                        .Items(i).ForeColor = Color.White
                        .Visible = True
                        LastFoundIndex = i
                        LastSearchString = TxtBxFind.Text
                        lisView = lis
                        Test2 = True
                        .Items(i).Selected = True
                        Exit For
                    End If
                Next
            End With
            If Test2 Then
                lisView = lis
                Exit For
            End If
            LastFoundIndex = 0

        Next
        If Not Test2 Then
            MessageBox.Show("No items found.")
        End If
    End Sub

    Private Sub BtnSectionDelete_Click(sender As Object, e As EventArgs) Handles BtnSectionDelete.Click, DeleteSection.Click
        RemoveSection(ListBox1.SelectedIndex - 1)
    End Sub

    Private Sub BtnSectionDown_Click(sender As Object, e As EventArgs) Handles BtnSectionDown.Click
        If ListBox1.Items.Count = 0 Then Exit Sub
        If ListBox1.SelectedIndex <= 1 Then Exit Sub
        If ListBox1.SelectedIndex = ListBox1.Items.Count - 1 Then Exit Sub
        Dim idx As Integer = ListBox1.SelectedIndex - 1

        Dim item As TDSSegment ' = New TDSSegment

        item = TabSections(TabControl2.SelectedIndex)(idx)

        If idx < TabSections(TabControl2.SelectedIndex).Count - 1 Then
            TabSections(TabControl2.SelectedIndex).RemoveAt(idx)
            idx += 1
            TabSections(TabControl2.SelectedIndex).Insert(idx, item)
            UpdateSegmentList()
            SectionLstIdxOld = idx + 1
            ListBox1.SelectedIndex = idx + 1
            SectionLstIdx = idx + 1
            SectionIdx(TabControl2.SelectedIndex) = SectionLstIdx
        End If

    End Sub

    Private Sub BtnSectionUp_Click(sender As Object, e As EventArgs) Handles BtnSectionUp.Click
        If ListBox1.Items.Count = 0 Then Exit Sub
        If ListBox1.SelectedIndex <= 1 Then Exit Sub
        Dim idx As Integer = ListBox1.SelectedIndex - 1

        Dim item As TDSSegment
        item = TabSections(TabControl2.SelectedIndex)(idx)

        If idx <> 0 Then
            TabSections(TabControl2.SelectedIndex).RemoveAt(idx)
            idx -= 1
            TabSections(TabControl2.SelectedIndex).Insert(idx, item)
            UpdateSegmentList()
            SectionLstIdxOld = idx + 1
            ListBox1.SelectedIndex = idx + 1
            SectionLstIdx = idx + 1
            SectionIdx(TabControl2.SelectedIndex) = SectionLstIdx
        Else
            UpdateSegmentList()
            ListBox1.SelectedIndex = idx
        End If

    End Sub

    Private Sub BtnTemplateAdd_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem.Click, BtnTemplateAdd.Click
        Dim TemplatePath As String = (Path.Combine(Paths.SilverMonkeyDocumentsPath, "Templates"))
        Dim message, title As String
        Dim myValue As String
        message = "Name of file?"
        title = "Template Name"
        myValue = InputBox(message, title, "")
        If String.IsNullOrEmpty(myValue) Then Exit Sub
        TemplatePaths.Add(TemplatePath)
        ListBox2.Items.Add(myValue)
        File.WriteAllText(TemplatePath + myValue.ToString + ".ds", MS_Editor.Selection.Text)
    End Sub

    Private Sub BtnTemplateAddMS_Click(sender As Object, e As EventArgs) Handles BtnTemplateAddMS.Click, MSTemplateMenuAdd.Click

        Dim message, title As String
        Dim myValue As String
        message = "Name of file?"
        title = "Template Name"
        myValue = InputBox(message, title, "")
        If String.IsNullOrEmpty(myValue) Then Exit Sub
        TemplatePathsMS.Add(Paths.MonkeySpeakEditorDocumentsTemplatesPath)
        ListBox3.Items.Add(myValue)
        File.WriteAllText(Path.Combine(Paths.MonkeySpeakEditorDocumentsTemplatesPath, myValue.ToString + ".ms"), MS_Editor.Selection.Text)
    End Sub

    Private Sub BtnTemplateDelete_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        Dim reply As DialogResult = MessageBox.Show("Really delete this template?", "Caption",
     MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If reply = DialogResult.OK Then
            File.Delete(TemplatePaths(ListBox2.SelectedIndex) + ListBox2.SelectedItem.ToString + ".ds")
            TemplatePaths.RemoveAt(ListBox2.SelectedIndex)
            ListBox2.Items.RemoveAt(ListBox2.SelectedIndex)
        End If

    End Sub

    Private Sub BtnTemplateDeleteMS_Click(sender As Object, e As EventArgs) Handles BtnTemplateDeleteMS.Click, MSTemplateDelete.Click
        Dim reply As DialogResult = MessageBox.Show("Really delete this template?", "Caption",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If reply = DialogResult.OK Then
            File.Delete(TemplatePaths(ListBox3.SelectedIndex) + ListBox3.SelectedItem.ToString + ".ms")
            TemplatePathsMS.RemoveAt(ListBox3.SelectedIndex)
            ListBox3.Items.RemoveAt(ListBox3.SelectedIndex)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SplitContainer3.Panel1Collapsed = True
    End Sub

    Private Sub CloseTab(TabBtn As Button)

        If TabControl2.TabCount = 0 Then Exit Sub
        Dim tb As TabPage = DirectCast(TabBtn.Tag, TabPage)

        CloseTab(tb)
    End Sub

    Private Sub CloseTab(tb As TabPage)

        Dim TabPageIndex As Integer = TabControl2.TabPages.IndexOf(tb)
        Dim fname As String = WorkFileName(TabPageIndex)
        If fname = "" Then
            fname = New_File_Tag
        End If

        If Not CanOpen(TabPageIndex) Then
            Dim reply As DialogResult = MessageBox.Show(fname + " has been modified." + Environment.NewLine + "Save the changes?", "Warning",
      MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)

            If reply = DialogResult.Yes Then
                SaveMS_File(TabPageIndex)
            ElseIf reply = DialogResult.Cancel Then
                Exit Sub
            End If

        End If

        TabControl2.TabPages.Remove(tb)
        CanOpen.RemoveAt(TabPageIndex)
        WorkFileName.RemoveAt(TabPageIndex)
        WorkPath.RemoveAt(TabPageIndex)
        frmTitle.RemoveAt(TabPageIndex)
        SectionIdx.RemoveAt(TabPageIndex)
        FullFile.RemoveAt(TabPageIndex)
        TabSections.RemoveAt(TabPageIndex)
        TabEditStyles.RemoveAt(TabPageIndex)
        TabControl2.RePositionCloseButtons()

        If TabControl2.TabPages.Count = 0 And Disposing = False Then
            AddNewEditorTab("", "", 0)
            NewFile(EditStyles.ms)
        End If

    End Sub

    Private Sub ConfigToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigToolStripMenuItem.Click
        Config.Show()
        Config.Activate()
    End Sub

    Private Sub ContentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ContentToolStripMenuItem.Click
        If File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HelpFile)) Then
            Help.ShowHelp(Me, HelpFile)
        End If
    End Sub

    Private Sub DisplaySection(ByRef j As Integer)
        MS_Editor.Text = ""
        MS_Editor.Text = String.Join(vbCrLf, TabSections(TabControl2.SelectedIndex)(j).lines.ToArray)
    End Sub

    Private Sub DragonSpeakFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DragonSpeakFileToolStripMenuItem.Click
        AddNewEditorTab("", Paths.FurcadiaDocumentsFolder, TabControl2.TabCount)
        NewFile(EditStyles.ds)
    End Sub

    Private Sub DragonSpeakToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DragonSpeakToolStripMenuItem.Click
        TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ds

        SetLineTabs(TabControl2.SelectedIndex)
        If Not popupMenu Is Nothing Then popupMenu.Dispose()
        popupMenu.Enabled = True

        popupMenu.SearchPattern = AutoCompleteSearchPattern
        popupMenu.Items.MaximumSize = New System.Drawing.Size(600, 300)
        popupMenu.Items.Width = 600
        popupMenu.Items.SetAutocompleteItems(DS_autoCompleteList)
        MS_Editor.OnTextChanged()
        SetLineTabs(TabControl2.SelectedIndex)
    End Sub

    Private Sub DSWizardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DSWizardToolStripMenuItem.Click
        WMain.Show()
        WMain.Activate()
    End Sub

    Private Sub EditToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem1.Click
        OpenMS_File(TemplatePaths.Item(ListBox2.SelectedIndex) + "/" + ListBox2.SelectedItem.ToString + ".ds")
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    ''' <summary>
    ''' Export Highlighted code to HTML file
    ''' </summary>
    ''' <param name="sender">
    ''' </param>
    ''' <param name="e">
    ''' </param>
    Private Sub ExportToHTMLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportToHTMLToolStripMenuItem.Click

        With MSSaveDialog
            .Filter = "HTML files|*.html"
            If .ShowDialog = DialogResult.OK Then
                Dim HtmlTitle As String = Path.GetFileNameWithoutExtension(.FileName)
                Using Writer As New StreamWriter(.FileName)
                    '' Dim body As String = System.Web.HttpUtility.HtmlDecode()
                    Dim HtmlExportTemplate = String.Format(
"<html>
    <head>
        <title>{0}</title>
    </head>
    <body>
    {1}
    </body>
</html>", HtmlTitle, MS_Editor.Html)

                    Writer.Write(HtmlExportTemplate)
                End Using

            End If
        End With
    End Sub

    Private Sub FCloseAllTab_Click(sender As Object, e As EventArgs)
        Dim t As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim CurrentTab As TabPage = TabControl2.SelectedTab
        For Each tb As TabPage In TabControl2.TabPages
            If Not tb Is CurrentTab Then
                CloseTab(tb)
            End If
        Next
    End Sub

    Private Sub FCloseTab_Click(sender As Object, e As EventArgs)
        Dim t As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        CloseTab(TabControl2.SelectedTab)
    End Sub

    Private Sub FindReplace()
        If IsNothing(MS_Editor) Then Exit Sub
        Try
            If IsNothing(frm) Or frm.IsDisposed() Then frm = New frmSearch
            If frm.Visible Then
                frm.Activate()
            Else
                frm.Show() 'Dialog()
            End If
        Catch exc As Exception

            MessageBox.Show(exc.Message, exc.Source, MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    ''' <summary>
    ''' Fix line indents for Dragon Speak and MonkeySpeak
    ''' </summary>
    ''' <param name="sender">
    ''' </param>
    ''' <param name="e">
    ''' </param>
    Private Sub FindReplaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindReplaceToolStripMenuItem.Click
        FindReplace()
    End Sub

    Private Sub FixIndentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FixIndentsToolStripMenuItem.Click
        If IsNothing(MS_Editor) Then Exit Sub
        Dim StrArray() As String = MS_Editor.Lines.ToArray
        Dim str As String
        Dim Count As Integer = 0
        If TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ds Then
            Count = ini.GetKeyValue("Init-Types", "Count").ToInteger
        ElseIf TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ms Then
            Count = ini.GetKeyValue("MS-Init-Types", "Count").ToInteger
        End If

        Dim pattern(Count - 1) As String
        Dim pat(Count - 1) As Integer
        Dim chr As String = " "

        Dim T As String = " "
        If TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ds Then
            For I As Integer = 1 To Count
                T = ini.GetKeyValue("Init-Types", I.ToString)
                Dim s As String = ini.GetKeyValue("Indent-Lookup", T)
                Dim u As String = ini.GetKeyValue("C-Indents", T)
                pattern(I - 1) = "(" + s
                pat(I - 1) = u.ToInteger
            Next
        ElseIf TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ms Then
            For I As Integer = 1 To Count
                T = ini.GetKeyValue("MS-Init-Types", I.ToString)
                Dim s As String = ini.GetKeyValue("MS-Indent-Lookup", T)
                Dim u As String = ini.GetKeyValue("MS-C-Indents", T)
                pattern(I - 1) = "(" + s
                pat(I - 1) = u.ToInteger
            Next
        End If

        Dim insertPos As Integer = MS_Editor.SelectionStart
        For I As Integer = 0 To StrArray.Length - 1
            str = StrArray(I).Trim

            For N As Integer = 0 To pattern.Length - 1
                If str.StartsWith(pattern(N)) Then
                    Dim c As Integer = pattern(N).Substring(1, 1).ToInteger
                    StrArray(I) = StrDup(pat(N), " ") & str
                    Exit For
                End If
            Next
        Next

        MS_Editor.Text = String.Join(Environment.NewLine, StrArray)

        MS_Editor.SelectionStart = insertPos
    End Sub

    Private Sub FNewTab_Click(sender As Object, e As EventArgs)
        Dim c As Integer = TabControl2.TabCount
        AddNewEditorTab("", Paths.SilverMonkeyBotPath, c)
        NewFile(EditStyles.ms)
    End Sub

    Private Sub FSave_Click(sender As Object, e As EventArgs)
        SaveMS_File(TabControl2.SelectedIndex)
    End Sub

    Private Sub GetTemplates()
        Try
            Dim p As String = Path.Combine(Paths.ApplicationPath, "Templates")

            ListBox3.BeginUpdate()
            TemplatePaths.Clear()
            ListBox2.Items.Clear()

            Dim fileEntries As String()
            If Directory.Exists(p) Then
                fileEntries = Directory.GetFiles(p, "*.ds", SearchOption.TopDirectoryOnly)
                For Each s As String In fileEntries
                    s = Path.GetFileNameWithoutExtension(s)
                    ListBox2.BeginUpdate()
                    ListBox2.Items.Add(s)
                    TemplatePaths.Add(p)
                    ListBox2.EndUpdate()
                Next
            End If
            p = Path.Combine(Paths.FurcadiaDocumentsFolder, "Templates")
            If Directory.Exists(p) Then
                fileEntries = Directory.GetFiles(p, "*.ds", SearchOption.TopDirectoryOnly)
                For Each s As String In fileEntries
                    s = Path.GetFileNameWithoutExtension(s)
                    ListBox2.Items.Add(s)
                    TemplatePaths.Add(p)

                Next
            End If
            p = Path.Combine(Paths.SilverMonkeyDocumentsPath, "Templates")
            If Directory.Exists(p) Then
                fileEntries = Directory.GetFiles(p, "*.ds", SearchOption.TopDirectoryOnly)
                For Each s As String In fileEntries
                    s = Path.GetFileNameWithoutExtension(s)
                    ListBox2.BeginUpdate()
                    ListBox2.Items.Add(s)
                    TemplatePaths.Add(p)
                    ListBox2.EndUpdate()
                Next
            End If
            ListBox2.EndUpdate()

            TemplatePathsMS.Clear()
            ListBox3.Items.Clear()

            p = Paths.MonkeySpeakEditorDocumentsTemplatesPath
            If Directory.Exists(p) Then
                fileEntries = Directory.GetFiles(p, "*.ms", SearchOption.TopDirectoryOnly)
                For Each s As String In fileEntries
                    s = Path.GetFileNameWithoutExtension(s)
                    ListBox3.BeginUpdate()
                    ListBox3.Items.Add(s)
                    TemplatePathsMS.Add(p)
                    ListBox3.EndUpdate()
                Next
            End If
            p = Paths.MonKeySpeakEditorTemplatesPath
            If Directory.Exists(p) Then

                fileEntries = Directory.GetFiles(p, "*.ms", SearchOption.TopDirectoryOnly)
                For Each s As String In fileEntries
                    s = Path.GetFileNameWithoutExtension(s)
                    ListBox3.BeginUpdate()
                    ListBox3.Items.Add(s)
                    TemplatePathsMS.Add(p)
                    ListBox3.EndUpdate()
                Next
            End If
            ListBox3.EndUpdate()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub GotoBookMark()
        If IsNothing(MS_Editor) Then Exit Sub
        Dim l As Integer = MS_Editor.Selection.Start.iLine
        MS_Editor.GotoNextBookmark(l)
    End Sub

    Private Sub GotoBookPrevMark()
        If IsNothing(MS_Editor) Then Exit Sub
        Dim l As Integer = MS_Editor.Selection.Start.iLine
        MS_Editor.GotoPrevBookmark(l)

    End Sub

    Private Sub GotoNextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GotoNextToolStripMenuItem.Click
        GotoBookMark()
    End Sub

    Private Sub GotoPreviousToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GotoPreviousToolStripMenuItem.Click
        GotoBookPrevMark()
    End Sub

    Private Sub GotoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GotoToolStripMenuItem.Click, ToolStripButton1.Click
        If IsNothing(MS_Editor) Then Exit Sub
        Dim i As String =
                    InputBox("What line within the document do you want to send the cursor to?",
                            " Location to send the Cursor?", "0")
        If String.IsNullOrEmpty(i) Then Exit Sub
        If IsInteger(i) And i.ToInteger > 0 Then
            If CInt(i) > MS_Editor.Lines.Count - 1 Then i = CStr(MS_Editor.Lines.Count - 1)
            MS_Editor.Selection.Start = New Place(0, i.ToInteger - 1)
            MS_Editor.Selection.Expand()
            MS_Editor.DoSelectionVisible()

            UpdateStatusBar()

        End If
    End Sub

    Private Sub InsertSectionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InsertSectionToolStripMenuItem.Click
        If ListBox1.SelectedIndex > 1 Then NewSec(ListBox1.SelectedIndex)
    End Sub

    Private Sub ListBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles ListBox1.MouseDown
        Dim l As ListBox = CType(sender, ListBox)
        If l.Items.Count = 0 Then Exit Sub
        SectionLstIdx = l.IndexFromPoint(New Point(e.X, e.Y))
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If SectionLstIdx = -1 Then SectionLstIdx = l.SelectedIndex
            l.SelectedIndex = SectionLstIdx
        End If
        Debug.Print("ListBox1_MouseDown()")
        If SectionLstIdx <> SectionLstIdxOld Then SaveSections(TabControl2.SelectedIndex)

    End Sub

    Private Sub ListBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles ListBox1.MouseUp
        Dim l As ListBox = CType(sender, ListBox)
        If ListBox1.Items.Count = 0 Or l.SelectedIndex = -1 Then Exit Sub
        Debug.Print("ListBox1_MouseUp()")
        Dim test As Integer = l.SelectedIndex
        If SectionLstIdx = SectionLstIdxOld Then Exit Sub

        SectionChange = True
        If ListBox1.SelectedIndex = 0 Then
            'Rebuild FullFile list first
            RebuildFullFile()
            MS_Editor.Text = ""
            MS_Editor.Text = String.Join(vbCrLf, FullFile(TabControl2.SelectedIndex).ToArray)

            UpdateSegments(TabControl2.SelectedIndex)
        Else
            DisplaySection(ListBox1.SelectedIndex - 1)
        End If
        MS_Editor.ClearUndo()

        Dim j As Integer = ListBox1.SelectedIndex
        SectionIdx(TabControl2.SelectedIndex) = l.SelectedIndex
        SectionChange = False
        SectionLstIdxOld = l.SelectedIndex
        UpdateSegmentList()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim l As ListBox = CType(sender, ListBox)
        If IsNothing(l.SelectedItem) Then Exit Sub
        ToolTip1.SetToolTip(l, l.SelectedItem.ToString)
    End Sub

    Private Sub ListBox2_DoubleClick(sender As Object, e As EventArgs) Handles InsertToDSFileToolStripMenuItem.Click, ListBox2.DoubleClick
        If IsNothing(MS_Editor) Then Exit Sub
        Dim p As String = TemplatePaths.Item(ListBox2.SelectedIndex) + "\" + ListBox2.SelectedItem.ToString + ".ds"
        Dim reader As New StreamReader(p)
        Dim str As String = ""
        Do While reader.Peek <> -1
            str += reader.ReadLine + vbLf
        Loop
        reader.Close()
        Dim pos As Integer = MS_Editor.SelectionStart
        MS_Editor.InsertText(str)
        MS_Editor.SelectionStart = pos + str.Length

    End Sub

    Private Sub ListBox2_MouseDown(sender As Object, e As MouseEventArgs) Handles ListBox2.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            ListBox2.SelectedIndex = ListBox2.IndexFromPoint(New Point(e.X, e.Y))
        End If
    End Sub

    Private Sub ListBox3_DoubleClick(sender As Object, e As EventArgs) Handles ListBox3.DoubleClick
        If IsNothing(MS_Editor) Then Exit Sub
        Dim p As String = TemplatePathsMS.Item(ListBox3.SelectedIndex) + "\" + ListBox3.SelectedItem.ToString + ".ms"
        Dim reader As New StreamReader(p)
        Dim str As String = ""
        Do While reader.Peek <> -1
            str += reader.ReadLine + vbLf
        Loop
        reader.Close()
        Dim pos As Integer = MS_Editor.SelectionStart
        MS_Editor.InsertText(str)
        MS_Editor.SelectionStart = pos + str.Length
    End Sub

    Private Sub ListBox3_Resize(sender As Object, e As EventArgs) Handles ListBox3.Resize, ListBox2.Resize

        If IsNothing(TabControl2) Then Exit Sub
        If TabControl2.TabPages.Count = 0 Then Exit Sub

        Dim lb As ListBox = DirectCast(sender, ListBox)
        Dim p As SplitterPanel = DirectCast(lb.Parent, SplitterPanel)

        If lb.Width <> p.Width - 5 AndAlso p.Height <> p.Height - 30 Then
            lb.BeginUpdate()
            lb.Size = New Size(p.Width - 5, p.Height - 30)
            lb.EndUpdate()
        End If

    End Sub

    Private Sub ListCauses_DoubleClick(sender As Object, e As EventArgs)
        Dim lv As ListView = CType(sender, ListView)
        Dim p As TabPage = CType(lv.Parent, TabPage)
        Dim r As TabControl = CType(p.Parent, TabControl)
        Dim q As SplitterPanel = CType(r.Parent, SplitterPanel)
        Dim test2 As String
        Dim test As Integer
        If q.Handle = SplitContainer4.Panel1.Handle Then
            test2 = ini.GetKeyValue("Init-Types", lv.Name)
            test = ini.GetKeyValue("C-Indents", test2).ToInteger
        Else
            test2 = ini.GetKeyValue("MS-Init-Types", lv.Name)
            test = ini.GetKeyValue("MS-C-Indents", test2).ToInteger
        End If
        TextInsert(lv, test)
    End Sub

    Private Sub ListCauses_MouseClick(sender As Object, e As EventArgs)
        Dim lv As ListView = CType(sender, ListView)
        If lv.FocusedItem Is Nothing Then Exit Sub
        Dim p As TabPage = CType(lv.Parent, TabPage)
        Dim r As TabControl = CType(p.Parent, TabControl)
        Dim q As SplitterPanel = CType(r.Parent, SplitterPanel)
        Dim test2 As String

        Dim t As New Regex("(\(\d+\:\d+\))")
        Dim u As Match = t.Match(lv.FocusedItem.Text)
        If q.Handle = SplitContainer4.Panel1.Handle Then
            test2 = KeysHelpIni.GetKeyValue(lv.Tag.ToString, u.Groups(1).Value)
        Else
            test2 = KeysHelpMSIni.GetKeyValue(lv.Tag.ToString, u.Groups(1).Value)
        End If
        If String.IsNullOrEmpty(test2) Then test2 = "No Help found for this item"
        TextBox2.Text = lv.FocusedItem.Text
        TextBox1.Text = test2
    End Sub

    Private Sub ListView_resize(sender As Object, e As EventArgs)
        Dim lv As ListView = CType(sender, ListView)
        lv.Columns(0).Width() = lv.Width
    End Sub

    Private Sub MenuCopy_Click(sender As Object, e As EventArgs) Handles MenuCopy.Click, EditDropCopy.Click
        MS_Editor.Copy()
    End Sub

    Private Sub MenuCut_Click(sender As Object, e As EventArgs) Handles MenuCut.Click, EditDropCut.Click
        MS_Editor.Cut()
    End Sub

    Private Sub MonkeySpeakFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MonkeySpeakFileToolStripMenuItem.Click, ToolBoxNew.Click
        AddNewEditorTab("", Paths.SilverMonkeyBotPath, TabControl2.TabCount)
        NewFile(EditStyles.ms)
    End Sub

    Private Sub MonkeySpeakToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MonkeySpeakToolStripMenuItem.Click
        TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ms

        SetLineTabs(TabControl2.SelectedIndex)
        If Not popupMenu Is Nothing Then popupMenu.Dispose()

        popupMenu = New AutocompleteMenu(MS_Editor)
        popupMenu.Enabled = True

        popupMenu.SearchPattern = AutoCompleteSearchPattern
        popupMenu.Items.MaximumSize = New Size(600, 300)
        popupMenu.Items.Width = 600
        popupMenu.Items.SetAutocompleteItems(MS_autoCompleteList)
        MS_Editor.OnTextChanged()
        SetLineTabs(TabControl2.SelectedIndex)
    End Sub

    Private Sub MS_Edit_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        For i = TabControl2.TabPages.Count - 1 To 0 Step -1
            If Not CanOpen(i) Then
                Dim fname As String = WorkFileName(i)
                If fname = "" Then
                    fname = New_File_Tag
                End If
                Dim result = MessageBox.Show(fname + " was modified." + Environment.NewLine + "Save your work before closing?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                If result = DialogResult.Cancel Then
                    e.Cancel = True
                    CanOpen(i) = False
                    Exit Sub
                ElseIf result = DialogResult.No Then
                    Try
                        TabControl2.TabPages.RemoveAt(i)
                        CanOpen.RemoveAt(i)
                        WorkFileName.RemoveAt(i)
                        WorkPath.RemoveAt(i)
                        frmTitle.RemoveAt(i)
                        SectionIdx.RemoveAt(i)
                        TabEditStyles.RemoveAt(i)
                        BotName.RemoveAt(i)

                        ' MsgBox(e.CloseReason.ToString)
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                ElseIf result = DialogResult.Yes Then
                    SaveMS_File(i)
                    Try
                        TabControl2.TabPages.RemoveAt(i)
                        CanOpen.RemoveAt(i)
                        WorkFileName.RemoveAt(i)
                        WorkPath.RemoveAt(i)
                        frmTitle.RemoveAt(i)
                        SectionIdx.RemoveAt(i)
                        TabEditStyles.RemoveAt(i)
                        BotName.RemoveAt(i)
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                End If
            Else
                Try
                    TabControl2.TabPages.RemoveAt(i)
                    CanOpen.RemoveAt(i)
                    WorkFileName.RemoveAt(i)
                    WorkPath.RemoveAt(i)
                    frmTitle.RemoveAt(i)
                    SectionIdx.RemoveAt(i)
                    TabEditStyles.RemoveAt(i)
                    BotName.RemoveAt(i)
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
            End If
        Next
        'Set my user setting MainFormLocation to
        'the current form's location
        If Not IsNothing(EditSettings) Then EditSettings.SaveEditorSettings()
        My.Settings.EditFormLocation = Location
        My.Settings.Save()
        Dispose()
    End Sub

    Private Sub MS_Edit_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If (e.KeyCode = Keys.O AndAlso e.Modifiers = Keys.Control) Then
            OpenMS_File()
        ElseIf (e.KeyCode = Keys.W AndAlso e.Modifiers = Keys.Control) Then
            WMain.Show()
        ElseIf (e.KeyCode = Keys.S AndAlso e.Modifiers = Keys.Control) Then
            ' SaveMS_File(WorkPath(TabControl2.SelectedIndex), WorkFileName(TabControl2.SelectedIndex))
        ElseIf (e.KeyCode = Keys.F1) Then
            If File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HelpFile)) Then
                Help.ShowHelp(Me, HelpFile)
            End If
        ElseIf e.KeyCode = Keys.F5 Then
            GotoBookPrevMark()
        ElseIf e.KeyCode = Keys.F5 Then
            GotoBookMark()
        ElseIf (e.KeyCode = Keys.F AndAlso e.Modifiers = Keys.Control) Then
            FindReplace()
        End If

    End Sub

    Private Sub MS_Edit_Load(sender As Object, e As EventArgs) Handles Me.Load

        KeysIni.Load(Path.Combine(Paths.ApplicationPath, "Keys.ini"))
        MS_KeysIni.Load(Path.Combine(Paths.ApplicationPath, "Keys-MS.ini"))
        KeysHelpMSIni.Load(Path.Combine(Paths.ApplicationPath, "KeysHelp-MS.ini"))
        KeysHelpIni.Load(Path.Combine(Paths.ApplicationPath, "KeysHelp.ini"))

        EditSettings = New MonkeyCore.Settings.EditSettings

        Dim HelpItems = New MonkeyCore.Controls.HelpLinkToolStripMenu
        ReferenceLinksToolStripMenuItem.DropDown.Items.AddRange(HelpItems.MenuItems.ToArray)

        Dim splash As SplashScreen1 = CType(My.Application.SplashScreen, SplashScreen1)
        Dim filename As String = ""

        If My.Application.CommandLineArgs.Count > 0 Then
            filename = My.Application.CommandLineArgs.Last()
        End If

        Dim BotName As String = ""
        If My.Application.CommandLineArgs.Count >= 2 Then
            BotName = My.Application.CommandLineArgs(0)
        End If

        DoubleBuffered = True

        Dim PluginFound As Boolean = False
        For Each s As String In FileIO.FileSystem.GetFiles(Paths.ApplicationPluginPath, FileIO.SearchOption.SearchTopLevelOnly, "*.ini")
            Dim FName As String = Path.GetFileNameWithoutExtension(s)
            If IsNothing(EditSettings.PluginList) Then EditSettings.PluginList = New Dictionary(Of String, Boolean)
            If Not EditSettings.PluginList.ContainsKey(FName) Then
                EditSettings.PluginList.Add(FName, True)
                PluginFound = True
            End If
            If EditSettings.PluginList.Item(FName) = True Then
                MS_KeysIni.Load(s, True)
            End If
        Next
        If PluginFound Then EditSettings.SaveEditorSettings()

        Location = My.Settings.EditFormLocation
        Visible = True

        Dim items As List(Of AutocompleteItem) = New List(Of AutocompleteItem)()
        Dim DsKeyCount As Integer = Integer.Parse(KeysIni.GetKeyValue("Init-Types", "Count"))
        Dim MsKeyCount As Integer = Integer.Parse(MS_KeysIni.GetKeyValue("Init-Types", "Count"))
        For i As Integer = 1 To DsKeyCount
            items.Clear()
            Dim DSLines As New List(Of String)
            Dim key As String = KeysIni.GetKeyValue("Init-Types", i.ToString)
            splash.UpdateProgress("Loading DS " + key + "...", i / (DsKeyCount + MsKeyCount + 2) * 100)
            Dim DSSection As IniSection = KeysIni.GetSection(key)

            For Each K As IniSection.IniKey In DSSection.Keys
                Dim fields As String() = SplitCSV(K.Value)
                If Not IsNothing(fields) Then
                    DSLines.Add(fields(2))
                    items.Add(New DA_AUtoCompleteMenu(fields(2)))
                End If

            Next

            AddNewTab(key, i.ToString, DSLines, Causes)
            DS_autoCompleteList.AddRange(items)

        Next

        For i As Integer = 1 To MsKeyCount
            items.Clear()
            Dim MsLineFinderSet As New List(Of String)
            Dim key As String = MS_KeysIni.GetKeyValue("Init-Types", i.ToString)
            splash.UpdateProgress("Loading MS " + key + "...", i + DsKeyCount / (DsKeyCount + MsKeyCount + 2) * 100)
            Dim MsSection As IniSection = MS_KeysIni.GetSection(key)

            For Each K As IniSection.IniKey In MsSection.Keys

                Dim MsIniKeyFields As String() = SplitCSV(K.Value)

                If Not IsNothing(MsIniKeyFields) Then
                    MsLineFinderSet.Add(MsIniKeyFields(2))
                    items.Add(New DA_AUtoCompleteMenu(MsIniKeyFields(2)))
                End If

            Next

            AddNewTab(key, i.ToString, MsLineFinderSet, TabControl3)
            MS_autoCompleteList.AddRange(items)

        Next

        splash.UpdateProgress("Finishing up...", DsKeyCount + MsKeyCount + 1 / (DsKeyCount + MsKeyCount + 2) * 100)

        SetDSHilighter()
        SetMSHilighter()

        Debug.Print("MS_Edit_Load()")
        Debug.Print("Setting Template Lists Size/Location")

        If (My.Application.CommandLineArgs.Count > 0) Then
            'AddNewEditorTab(filename, mPath, 0)
            OpenMS_File(filename)
        Else
            AddNewEditorTab("", Paths.SilverMonkeyDocumentsPath, 0)
            NewFile(EditStyles.ms)
        End If

        SetLineTabs(0)

        GetTemplates()

        splash.UpdateProgress("Complete!", 100)
    End Sub

    Private Sub MS_Editor_CursorChanged(sender As Object, e As EventArgs)
        UpdateStatusBar()
    End Sub

    Private Sub MS_Editor_MouseDoubleClick(sender As Object, e As MouseEventArgs)
        If e.X < MS_Editor.LeftIndent Then
            Dim place = MS_Editor.PointToPlace(e.Location)
            If MS_Editor.Bookmarks.Contains(place.iLine) Then
                MS_Editor.Bookmarks.Remove(place.iLine)
            Else
                MS_Editor.Bookmarks.Add(place.iLine)
            End If
        End If
    End Sub

    Private Sub MS_Editor_TextChanged(sender As Object, e As EventArgs)

        If SectionChange = False Then CanOpen(TabControl2.SelectedIndex) = False

        UpdateStatusBar()
        If CanOpen(TabControl2.SelectedIndex) = False Then Text = frmTitle(TabControl2.SelectedIndex) & "*"
        If WorkFileName(TabControl2.SelectedIndex) = "" And CanOpen(TabControl2.SelectedIndex) = False Then
            TabControl2.SelectedTab.Text = New_File_Tag + "*"
            TabControl2.RePositionCloseButtons()
        ElseIf CanOpen(TabControl2.SelectedIndex) = False Then
            TabControl2.SelectedTab.Text = WorkFileName(TabControl2.SelectedIndex) + "*"
            TabControl2.RePositionCloseButtons()
        End If

        If CanOpen(TabControl2.SelectedIndex) = False Then lblStatus.Text = "Status: A change has occured since you last saved the document."
    End Sub

    Private Sub MS_Editor_TextChangedDelayed(sender As Object, e As TextChangedEventArgs)
        Dim s As FastColoredTextBox = CType(sender, FastColoredTextBox)
        e.ChangedRange.ClearStyle(StyleIndex.All)
        If TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ds Or TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ini Then
            s.CommentPrefix = "*"
            'clear style of changed range
            'Header
            e.ChangedRange.SetStyle(DS_Header_Style, "(" + RegExEscapedSring(DS_HEADER) + ")", RegexOptions.IgnoreCase)
            'Footer
            e.ChangedRange.SetStyle(DS_Footer_Style, "(" + RegExEscapedSring(DS_FOOTER) + ")", RegexOptions.IgnoreCase)
            'comment highlighting
            e.ChangedRange.SetStyle(DS_Comment_Style, "^\*(.*)$", RegexOptions.Multiline)
            'Line ID highlighting
            e.ChangedRange.SetStyle(DS_Line_ID_Style, "(\([0-9#]+):[0-9]+\)")
            'number Variable highlighting
            e.ChangedRange.SetStyle(DS_Num_Var_Style, "%([A-Za-z0-9_]+)")
            'number Variable highlighting
            e.ChangedRange.SetStyle(DS_Str_Var_Style, "~([A-Za-z0-9_]+)")
            'string highlighting
            e.ChangedRange.SetStyle(DS_String_Style, "\{.*?\}")
            'number highlighting
            e.ChangedRange.SetStyle(DS_Num_Style, "([\-\+0-9#]+)")
            'clear folding markers
            ' sender.Range.ClearFoldingMarkers()

        ElseIf TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ms Then
            s.CommentPrefix = "*"
            'clear style of changed range

            'Header
            e.ChangedRange.SetStyle(MS_Header_Style, "(" + RegExEscapedSring(MS_HEADER) + ")", RegexOptions.IgnoreCase)
            'Footer
            e.ChangedRange.SetStyle(MS_Footer_Style, "(" + RegExEscapedSring(MS_FOOTER) + ")", RegexOptions.IgnoreCase)
            'comment highlighting
            'e.ChangedRange.SetStyle(MS_Comment_Style, "^\*([^\n]*)")
            e.ChangedRange.SetStyle(MS_Comment_Style, "^\*(.*)$", RegexOptions.Multiline)
            'Line ID highlighting
            e.ChangedRange.SetStyle(MS_Line_ID_Style, "(\([0-9#]+):[0-9]+\)")
            'number Variable highlighting
            e.ChangedRange.SetStyle(MS_Num_Var_Style, "%([A-Za-z0-9_]+)")
            'string highlighting
            e.ChangedRange.SetStyle(MS_String_Style, "\{.*?\}")
            'number highlighting
            e.ChangedRange.SetStyle(MS_Num_Style, "([0-9#]+)")
            'clear folding markers
            ' sender.Range.ClearFoldingMarkers()
        ElseIf TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ini Then
            s.CommentPrefix = "*"
            'clear style of changed range
            'comment highlighting
            ' e.ChangedRange.SetStyle(INI_Comment_Style, "^;(.*)$", RegexOptions.Multiline)

            'clear folding markers
            ' sender.Range.ClearFoldingMarkers()
        End If
    End Sub

    Private Sub MS_EditRightClick(sender As Object, e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Right Then
            EditMenu.Show(MS_Editor, New Point(e.X, e.Y))
        End If
    End Sub

    Private Sub NewSec(ByRef i As Integer)
        If ListBox1.Items.Count = 0 Then Exit Sub
        If ListBox1.SelectedIndex < 1 Or i < 1 Then Exit Sub
        'If i >= 1 Then i = 2
        i -= 1
        Debug.Print("NewSection_Click()")
        Dim s As String = InputBox("Add Section")
        If String.IsNullOrEmpty(s) Then Exit Sub

        SaveSections(TabControl2.SelectedIndex)
        Dim section As New TDSSegment
        'Dim sec As TDSSegment = TabSections(TabControl2.SelectedIndex)(i)
        section.Title = s
        section.lines.Add("")

        TabSections(TabControl2.SelectedIndex).Insert(i, section)
        'TabSections(TabControl2.SelectedIndex)(i + 1) = sec
        UpdateSegmentList()

        SectionChange = False

        i += 1
        ListBox1.SelectedIndex = i
        SectionLstIdx = i
        SectionLstIdxOld = i
        SectionIdx(TabControl2.SelectedIndex) = SectionLstIdx

        DisplaySection(i - 1)
        SectionChange = True

    End Sub

    Private Sub NewSection_Click(sender As Object, e As EventArgs) Handles NewSection.Click, BtnSectionAdd.Click
        If TabControl2.TabCount > 0 Then NewSec((TabSections(TabControl2.SelectedIndex).Count))
    End Sub

    Private Sub OpenMS_File()
        With MS_BrosweDialog
            ' Select Character ini file
            .InitialDirectory = Paths.SilverMonkeyBotPath

            If .ShowDialog = DialogResult.OK Then
                Dim f As String = Path.GetFileName(.FileName)
                AddNewEditorTab(f, "", 0)
                TabSections(TabControl2.SelectedIndex).Clear()
                Dim slashPosition As Integer = .FileName.LastIndexOf("\")
                Dim ext As String = Path.GetExtension(.FileName)

                WorkFileName(TabControl2.SelectedIndex) = .FileName.Substring(slashPosition + 1)
                WorkPath(TabControl2.SelectedIndex) = .FileName.Replace(WorkFileName(TabControl2.SelectedIndex), "")

                frmTitle(TabControl2.SelectedIndex) = AppName + " - " & WorkFileName(TabControl2.SelectedIndex)
                Text = frmTitle(TabControl2.SelectedIndex)
                lblStatus.Text = "Status: opened " & WorkFileName(TabControl2.SelectedIndex)

                Dim reader As New StreamReader(WorkPath(TabControl2.SelectedIndex) + "/" + WorkFileName(TabControl2.SelectedIndex))
                MS_Editor.Text = ""
                FullFile(TabControl2.SelectedIndex).Clear()
                Do While reader.Peek <> -1
                    Dim line As String = reader.ReadLine
                    FullFile(TabControl2.SelectedIndex).Add(line)
                Loop
                MS_Editor.Text = String.Join(vbCrLf, FullFile(TabControl2.SelectedIndex).ToArray)
                reader.Close()

                UpdateSegments()
                UpdateSegmentList()
                CanOpen(TabControl2.SelectedIndex) = True
                TabControl2.SelectedTab.Text = WorkFileName(TabControl2.SelectedIndex)
                TabControl2.RePositionCloseButtons(TabControl2.SelectedTab)
                If ext.ToLower = ".ds" Then
                    TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ds
                    popupMenu = New AutocompleteMenu(MS_Editor)
                    popupMenu.Enabled = True

                    popupMenu.SearchPattern = AutoCompleteSearchPattern
                    popupMenu.Items.MaximumSize = New System.Drawing.Size(600, 300)
                    popupMenu.Items.Width = 600
                    popupMenu.Items.SetAutocompleteItems(DS_autoCompleteList)
                ElseIf ext.ToLower = ".ms" Then
                    TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ms
                    popupMenu = New AutocompleteMenu(MS_Editor)
                    popupMenu.Enabled = True

                    popupMenu.SearchPattern = AutoCompleteSearchPattern
                    popupMenu.Items.MaximumSize = New System.Drawing.Size(600, 300)
                    popupMenu.Items.Width = 600
                    popupMenu.Items.SetAutocompleteItems(MS_autoCompleteList)
                ElseIf ext.ToLower = ".ini" Then
                    TabEditStyles(TabControl2.SelectedIndex) = EditStyles.ini
                Else
                    TabEditStyles(TabControl2.SelectedIndex) = EditStyles.none
                End If
                'MS_Editor.OnTextChanged()
                SetLineTabs(TabControl2.SelectedIndex)
            End If

        End With

    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click

        OpenMS_File()
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click, EditDropPaste.Click
        MS_Editor.Paste()
    End Sub

    Private Sub RebuildFullFile()
        RebuildFullFile(TabControl2.SelectedIndex)
    End Sub

    Private Sub RebuildFullFile(ByRef Tab As Integer)
        Debug.Print("RebuildFullFile()")
        FullFile(Tab).Clear()
        For i = 0 To TabSections(Tab).Count - 1
            'RES_SEC_Marker
            If TabSections(Tab)(i).Title <> RES_DSS_begin And
                TabSections(Tab)(i).Title <> RES_DSS_End And
                TabSections(Tab)(i).Title <> RES_MSS_begin And
                TabSections(Tab)(i).Title <> RES_MSS_End And
                (TabSections(Tab)(i).Title <> RES_Def_section Or TabSections(Tab)(i).Title = RES_Def_section And i > 1) Then
                FullFile(Tab).Add(RES_SEC_Marker + TabSections(Tab)(i).Title)
            End If
            FullFile(Tab).AddRange(TabSections(Tab)(i).lines)
        Next
    End Sub

    Private Sub RedoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RedoToolStripMenuItem.Click
        If IsNothing(MS_Editor) Then Exit Sub
        MS_Editor.Redo()
    End Sub

    Private Sub RefreshTemplatesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshTemplatesToolStripMenuItem.Click, MSTemplateRefresh.Click
        GetTemplates()
    End Sub

    Private Sub RemoveAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveAllToolStripMenuItem.Click
        If IsNothing(MS_Editor) Then Exit Sub
        MS_Editor.Bookmarks.Clear()
    End Sub

    Private Sub RemoveComment()
        If IsNothing(MS_Editor) Then Exit Sub

        Dim this As String = MS_Editor.Selection.Text
        Dim str() As String = this.Replace(vbCr, "").Split(Chr(10))
        If str.Length > 1 Then
            For i As Integer = 0 To str.Length - 1
                If str(i).StartsWith("*") Then str(i) = str(i).Remove(0, 1)
            Next
            MS_Editor.SelectedText = String.Join(vbCrLf, str)

        End If
    End Sub

    Private Sub RemoveCommentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveCommentToolStripMenuItem.Click, BtnUncomment.Click
        RemoveComment()
    End Sub

    Private Sub RemoveSection(ByRef i As Integer)
        If ListBox1.SelectedIndex < 2 Then Exit Sub
        If ListBox1.Items.Count = 0 Then Exit Sub
        Dim reply As DialogResult = MessageBox.Show("Really delete this section?", "Caption",
MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If reply = DialogResult.Cancel Then Exit Sub

        TabSections(TabControl2.SelectedIndex).RemoveAt(i)
        'RebuildFullFile()
        UpdateSegmentList()
        'UpdateSegments(i) 'm Goin bald trying to figure this
        ListBox1.SelectedIndex = i + 1
        DisplaySection(i)

    End Sub

    Private Sub RenameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RenameToolStripMenuItem.Click
        If ListBox1.Items.Count = 0 Then Exit Sub
        Dim idx As Integer = ListBox1.SelectedIndex
        Dim section As TDSSegment = TabSections(TabControl2.SelectedIndex)(idx - 1)
        Dim s As String = InputBox("New Name?")
        If String.IsNullOrEmpty(s) Then Exit Sub
        section.Title = s
        UpdateSegmentList()
        ListBox1.SelectedIndex = idx
    End Sub

    Private Sub RenameToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RenameToolStripMenuItem1.Click
        Dim s As String = Microsoft.VisualBasic.InputBox("New Name?")
        If String.IsNullOrEmpty(s) Then Exit Sub
        My.Computer.FileSystem.RenameFile(TemplatePaths.Item(ListBox2.SelectedIndex) + ListBox2.SelectedItem.ToString + ".ds", TemplatePaths(ListBox2.SelectedIndex) + s + ".ds")
        GetTemplates()
    End Sub

    Private Sub RestartBotEngineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestartBotEngineToolStripMenuItem.Click, ToolStripButton2.Click
        Dim msg As MessageHelper = New MessageHelper
        'save if modified
        SaveMS_File(TabControl2.SelectedIndex)

        'send restart command to bot
        Dim cstrReceiverWindowName As String = "Silver Monkey: " + BotName(TabControl2.SelectedIndex)
        Dim WindowHandleOfToProcess As Integer = FindWindow(Nothing, cstrReceiverWindowName)
        'find by window name
        Dim WindowHandle As New IntPtr(WindowHandleOfToProcess)

        'Step 2.
        'Create some information to send.
        Dim strTag As String = "Restart"
        Dim msMsg As String = WorkFileName(TabControl2.SelectedIndex)

        Dim iResult As IntPtr = IntPtr.Zero
        If WindowHandle.ToInt32() <> 0 Then
            iResult = msg.sendWindowsStringMessage(WindowHandle, Handle, "~DSEX~", 0, strTag, msMsg)
            lblStatus.Text = "Status: Engine restart command sent to " + BotName(TabControl2.SelectedIndex)
        End If
    End Sub

    Private Sub SaveAs()

        With MSSaveDialog
            ' Select Character ini file
            .InitialDirectory = MonkeyCore.Paths.SilverMonkeyBotPath
            If .ShowDialog = DialogResult.OK Then
                WorkFileName(TabControl2.SelectedIndex) = Path.GetFileName(.FileName)
                WorkPath(TabControl2.SelectedIndex) = Path.GetDirectoryName(.FileName)
                CanOpen(TabControl2.SelectedIndex) = False
                SaveMS_File(TabControl2.SelectedIndex)
                lblStatus.Text = "Status: Saved " & WorkFileName(TabControl2.SelectedIndex)
                frmTitle(TabControl2.SelectedIndex) = AppName + " - " & WorkFileName(TabControl2.SelectedIndex)
                Text = frmTitle(TabControl2.SelectedIndex)
                CanOpen(TabControl2.SelectedIndex) = True
            End If
        End With
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click, ToolBoxSaveAs.Click
        If IsNothing(MS_Editor) Then Exit Sub
        SaveAs()
    End Sub

    Private Sub SaveMS_File(ByRef TabIdx As Integer)
        If MS_Editor.InvokeRequired Then
            Dim d As New FileSave(AddressOf SaveMS_File)
            Invoke(d, TabIdx)
        Else

            If Not CanOpen(TabIdx) Then

                If String.IsNullOrEmpty(WorkFileName(TabIdx)) Then
                    SaveAs()
                    Exit Sub
                End If
                SaveSections(TabIdx)
                RebuildFullFile(TabIdx)

                Try
                    Dim Writer As New StreamWriter(WorkPath(TabIdx) & "/" & WorkFileName(TabIdx))
                    For j = 0 To FullFile(TabIdx).Count - 1
                        Writer.WriteLine(FullFile(TabIdx)(j))
                    Next
                    Writer.Close()
                    lblStatus.Text = "Status: File Saved."

                    CanOpen(TabIdx) = True
                    If TabIdx = TabControl2.SelectedIndex Then
                        Text = frmTitle(TabIdx)
                        TabControl2.SelectedTab.Text = WorkFileName(TabIdx)
                        TabControl2.RePositionCloseButtons()
                    End If
                Catch ex As Exception
                    MessageBox.Show("There was an error writing to " + WorkFileName(TabIdx))
                End Try

            End If
        End If

    End Sub

    Private Sub SaveSections(ByVal tabidx As Integer)
        If ListBox1.SelectedIndex = -1 Then ListBox1.SelectedIndex = 0
        Debug.Print("SaveSections()")

        If SectionIdx(tabidx) = 0 And MS_Editor(tabidx).Text <> "" Then
            'Debug.Print("SectionIdx(" + tabidx.ToString + ")")
            FullFile(tabidx).Clear()
            For i = 0 To MS_Editor.Lines.Count - 1
                FullFile(tabidx).Add(MS_Editor(tabidx).Lines.Item(i).TrimEnd(charsToTrim))
            Next
            UpdateSegments()

        ElseIf SectionIdx(tabidx) > 0 Then
            Dim section As TDSSegment = TabSections(tabidx)(SectionIdx(tabidx) - 1)
            Debug.Print("Saving to section " + section.Title)
            section.lines.Clear()
            For i = 0 To MS_Editor(tabidx).Lines.Count - 1
                section.lines.Add(MS_Editor(tabidx).Lines.Item(i))
            Next

        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        SaveMS_File(TabControl2.SelectedIndex)
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectAllToolStripMenuItem.Click
        MS_Editor.SelectAll()
    End Sub

    Private Sub SetDSHilighter()

        DS_String_Style.ForeBrush = New SolidBrush(EditSettings.StringVariableColor)
        DS_Str_Var_Style.ForeBrush = New SolidBrush(EditSettings.StringVariableColor)
        DS_Num_Var_Style.ForeBrush = New SolidBrush(EditSettings.VariableColor)
        DS_Comment_Style.ForeBrush = New SolidBrush(EditSettings.CommentColor)
        DS_Num_Style.ForeBrush = New SolidBrush(EditSettings.NumberColor)
        DS_Line_ID_Style.ForeBrush = New SolidBrush(EditSettings.IDColor)
        DS_HEADER = KeysIni.GetKeyValue("MS-General", "Header")
        DS_FOOTER = KeysIni.GetKeyValue("MS-General", "Footer")
    End Sub

    Private Sub SetMSHilighter()
        MS_Num_Var_Style.ForeBrush = New SolidBrush(EditSettings.MS_VariableColor)
        MS_Comment_Style.ForeBrush = New SolidBrush(EditSettings.MS_CommentColor)
        MS_Num_Style.ForeBrush = New SolidBrush(EditSettings.MS_NumberColor)
        MS_Line_ID_Style.ForeBrush = New SolidBrush(EditSettings.MS_IDColor)
        MS_HEADER = MS_KeysIni.GetKeyValue("MS-General", "Header")
        MS_FOOTER = MS_KeysIni.GetKeyValue("MS-General", "Footer")
    End Sub

    Private Sub ShowLineFinderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowLineFinderToolStripMenuItem.Click
        SplitContainer3.Panel1Collapsed = Not SplitContainer3.Panel1Collapsed
    End Sub

    Private Sub TabControl2_CloseButtonClick(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TabControl2.CloseButtonClick
        e.Cancel = True
        Dim t As Button = DirectCast(sender, Button)
        CloseTab(DirectCast(t.Tag, TabPage))
    End Sub

    Private Sub TabControl2_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles TabControl2.MouseDown
        Dim z As Control = CType(sender, Control)
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim x As New ContextMenuStrip
            Dim s As ToolStripItem = x.Items.Add("New Tab", Nothing, AddressOf FNewTab_Click)
            s.Tag = sender
            Dim t As ToolStripItem = x.Items.Add("Close All Other Tabs", Nothing, AddressOf FCloseAllTab_Click)
            Dim v As ToolStripItem = x.Items.Add("Close Tab", Nothing, AddressOf FCloseTab_Click)
            x.Items.Add(New ToolStripSeparator)
            Dim u As ToolStripItem = x.Items.Add("Save File", Nothing, AddressOf FSave_Click)

            x.Show(z, e.Location)
            Dim tabPageIndex As Integer = 0
            For i As Integer = 0 To TabControl2.TabPages.Count - 1
                If TabControl2.GetTabRect(i).Contains(e.X, e.Y) Then
                    tabPageIndex = i
                    Exit For
                End If

            Next
            t.Tag = tabPageIndex
            u.Tag = tabPageIndex
        ElseIf e.Button = Windows.Forms.MouseButtons.Middle Then
            For i As Integer = 0 To TabControl2.TabPages.Count - 1
                If TabControl2.GetTabRect(i).Contains(e.X, e.Y) Then
                    CloseTab(TabControl2.TabPages.Item(i))
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub TabControl2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl2.SelectedIndexChanged

        If TabControl2.SelectedIndex = -1 Or TabControl2.SelectedIndex > TabControl2.TabPages.Count Then Exit Sub
        ListBox1.Items.Clear()
        If CanOpen(TabControl2.SelectedIndex) = False Then
            Text = frmTitle(TabControl2.SelectedIndex) + "*"
        Else
            Text = frmTitle(TabControl2.SelectedIndex)
        End If
        UpdateSegmentList()
        If SectionIdx(TabControl2.SelectedIndex) <> ListBox1.SelectedIndex Then ListBox1.SelectedIndex = SectionIdx(TabControl2.SelectedIndex)
        SetLineTabs(TabControl2.SelectedIndex)

    End Sub

    Private Sub TextInsert(ByRef LB As ListView, Optional ByVal Spaces As Integer = 0)

        If IsNothing(MS_Editor) Then Exit Sub
        Dim ch As String = " "
        Dim insertText = StrDup(Spaces, ch) & LB.SelectedItems(0).Text

        MS_Editor.InsertText(insertText + vbCrLf)
        UpdateStatusBar()
    End Sub

    Private Sub ToolBoxCut_Click(sender As Object, e As EventArgs) Handles ToolBoxCut.Click
        If IsNothing(MS_Editor) Then Exit Sub
        MS_Editor.Cut()
    End Sub

    Private Sub ToolBoxFindReplace_Click(sender As Object, e As EventArgs) Handles ToolBoxFindReplace.Click

        If IsNothing(MS_Editor) Then Exit Sub
        Try
            If IsNothing(frm) Or frm.IsDisposed() Then frm = New frmSearch
            If frm.Visible Then
                frm.Activate()
            Else
                frm.Show() 'Dialog()
            End If
        Catch exc As Exception

            MessageBox.Show(exc.Message, exc.Source, MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub ToolBoxOpen_Click(sender As Object, e As EventArgs) Handles ToolBoxOpen.Click
        OpenMS_File()
    End Sub

    Private Sub ToolBoxPaste_Click(sender As Object, e As EventArgs) Handles ToolBoxPaste.Click
        If IsNothing(MS_Editor) Then Exit Sub
        MS_Editor.Paste()
    End Sub

    Private Sub ToolBoxRedo_Click(sender As Object, e As EventArgs) Handles ToolBoxRedo.Click
        If IsNothing(MS_Editor) Then Exit Sub
        MS_Editor.Redo()
    End Sub

    Private Sub ToolBoxSave_Click(sender As Object, e As EventArgs) Handles ToolBoxSave.Click
        SaveMS_File(TabControl2.SelectedIndex)
    End Sub

    Private Sub ToolBoxUndo_Click(sender As Object, e As EventArgs) Handles ToolBoxUndo.Click
        If IsNothing(MS_Editor) Then Exit Sub
        MS_Editor.Undo()
    End Sub

    Private Sub ToolBoxyCopy_Click(sender As Object, e As EventArgs) Handles ToolBoxyCopy.Click
        If IsNothing(MS_Editor) Then Exit Sub
        MS_Editor.Copy()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        If IsNothing(MS_Editor) Then Exit Sub
        Dim p As String = TemplatePathsMS.Item(ListBox3.SelectedIndex) + Path.DirectorySeparatorChar + ListBox3.SelectedItem.ToString + ".ms"
        Dim reader As New StreamReader(p)
        Dim str As String = ""
        Do While reader.Peek <> -1
            str += reader.ReadLine + vbLf
        Loop
        reader.Close()
        Dim pos As Integer = MS_Editor.SelectionStart
        MS_Editor.InsertText(str)
        MS_Editor.SelectionStart = pos + str.Length
    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles MSTemplateRename.Click
        Dim s As String = InputBox("New Name?")
        If String.IsNullOrEmpty(s) Then Exit Sub
        My.Computer.FileSystem.RenameFile(TemplatePathsMS.Item(ListBox3.SelectedIndex) + ListBox3.SelectedItem.ToString + ".ms", TemplatePathsMS(ListBox3.SelectedIndex) + s + ".ms")
        GetTemplates()
    End Sub

    Private Sub ToolStripMenuItem6_Click(sender As Object, e As EventArgs) Handles MSTemplateEdit.Click
        OpenMS_File(TemplatePathsMS.Item(ListBox3.SelectedIndex) + Path.DirectorySeparatorChar + ListBox3.SelectedItem.ToString + ".ms")
    End Sub

    Private Sub TxtBxFind_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtBxFind.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            BtnFind.PerformClick()
        End If
    End Sub

    Private Sub UndoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UndoToolStripMenuItem.Click
        If IsNothing(MS_Editor) Then Exit Sub
        MS_Editor.Undo()
    End Sub

    Private Sub UpdateStatusBar()
        sb.Panels.Item(0).Text = "Cursor Position: " & MS_Editor.Selection.Start.iChar.ToString
        sb.Panels.Item(1).Text = "Current Line: " & MS_Editor.Selection.Start.iLine.ToString
        sb.Panels.Item(2).Text = "Total Lines: " & MS_Editor.Lines.Count.ToString
        sb.Panels.Item(3).Text = "Total Characters: " & MS_Editor.Text.Length.ToString
    End Sub

#End Region

#Region "Properties"

    Public Shared TemplatePaths As List(Of String) = New List(Of String)
    Public Shared TemplatePathsMS As List(Of String) = New List(Of String)
    Public BotName As List(Of String) = New List(Of String)
    Public CanOpen As List(Of Boolean) = New List(Of Boolean)
    Public TabEditStyles As List(Of EditStyles) = New List(Of EditStyles)

    ' Public SettingsChanged As List(Of Boolean) = New List(Of Boolean)
    Public WorkFileName As List(Of String) = New List(Of String)

    Public WorkPath As List(Of String) = New List(Of String)
    Private Const AppName As String = "Monkey Speak Editor"
    Private Const AutoCompleteSearchPattern As String = "[ \w\.:=!<>\{\}]"
    Private Const BotProcessName As String = "Silver Monkey: "
    Private Const MyProcessName As String = "MonkeySpeakEditor"
    Private Const New_File_Tag As String = "(New File)"
    Private Const RES_Def_section As String = "Default Section"
    Private Const RES_DS_begin As String = "DSPK V"
    Private Const RES_DS_end As String = "*Endtriggers* 8888 *Endtriggers*"
    Private Const RES_DSS_All As String = "Entire Document"
    Private Const RES_DSS_begin As String = "DS-START"
    Private Const RES_DSS_End As String = "DS-END"
    Private Const RES_MS_begin As String = "*MSPK V"
    Private Const RES_MS_end As String = "*Endtriggers* 8888 *Endtriggers*"
    Private Const RES_MSS_begin As String = "MS-START"
    Private Const RES_MSS_End As String = "MS-END"
    Private Const RES_SEC_Marker As String = "**SECTION**  "
    Private Shared DS_FOOTER As String = ""
    Private Shared DS_HEADER As String = ""
    Private Shared MS_FOOTER As String = ""
    Private Shared MS_HEADER As String = ""
    Private AutoCompleteMenu1 As AutocompleteMenu
    Private charsToTrim() As Char = {CChar(vbCr), CChar(vbLf)}

    'Dim lastTab As Integer = 0
    Dim Flag As Boolean = False

    'mPath()
    Dim frmTitle As List(Of String) = New List(Of String)

    Dim FullFile As List(Of List(Of String)) = New List(Of List(Of String))
    Private objMutex As System.Threading.Mutex
    Dim SectionChange As Boolean = False
    Dim SectionIdx As List(Of Integer) = New List(Of Integer)
    Dim TabSections As List(Of List(Of TDSSegment)) = New List(Of List(Of TDSSegment))

    'Private Templatelist As ListBox = New ListBox
    Enum TSecType
        SecNormal
        SecEnd
        SecFixed
        SecDefault
    End Enum

    Public Function FindControl(parent As Control, ident As String) As Control
        Dim control As Control = New Control
        For Each child As Control In parent.Controls
            If child.Name.Contains(ident) Then
                control = child
                Exit For
            End If
        Next
        Return control
    End Function

    ''' <summary>
    ''' Selected FCTB by Current Selected Tab
    ''' </summary>
    ''' <returns>
    ''' </returns>
    Public Function MS_Editor() As FastColoredTextBox
        If TabControl2.TabCount = 0 Then Return Nothing
        Return MS_Editor(TabControl2.SelectedIndex)
    End Function

    Public Function MS_Editor(i As Integer) As FastColoredTextBox
        If TabControl2.TabCount < i Then Return Nothing
        Return CType(FindControl(TabControl2.TabPages.Item(i), "edit"), FastColoredTextBox)
    End Function

    Public Class TDSSegment

#Region "Public Constructors"

        Sub New()
            Title = ""
            lines.Clear()
            SecType = TSecType.SecDefault
        End Sub

#End Region

#Region "Public Properties"

        Public Property lines As List(Of String) = New List(Of String)
        Public Property SecType As TSecType
        Public Property Title As String

#End Region

    End Class

    '+ TabControl2.SelectedIndex.ToString

#End Region

#Region "WmCpyDta"

    Public Function FindProcessByName(strProcessName As String) As IntPtr
        Dim HandleOfToProcess As IntPtr = IntPtr.Zero
        Dim MyProcess As Process = Process.GetCurrentProcess()
        Dim p As Process() = Process.GetProcessesByName(strProcessName)
        For Each p1 As Process In p
            Debug.WriteLine(p1.ProcessName.ToUpper())
            If p1.ProcessName.ToUpper() = strProcessName.ToUpper() And p1.Id <> MyProcess.Id Then
                HandleOfToProcess = p1.MainWindowHandle
                Exit For
            End If
        Next
        Return HandleOfToProcess
    End Function

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = WM_COPYDATA Then

            Dim mystr2 As COPYDATASTRUCT = Nothing
            Marshal.PtrToStructure(m.LParam(), [mystr2])
            ' If the size matches
            If mystr2.cdData = Marshal.SizeOf(GetType(MyData)) Then
                ' Marshal the data from the unmanaged memory block to a
                ' MyStruct managed struct.
                Dim myStr As MyData = Nothing
                Marshal.PtrToStructure(mystr2.lpData, [myStr])

                Dim sName As String = myStr.lpName
                Dim sFID As UInteger = myStr.fID
                Dim sTag As String = myStr.lpTag
                Dim sData As String = myStr.lpMsg              'Sample Data

                'sName = ~DSEX~
                'sFID = 0 "n/a"
                'sTag = -b=BotName
                'sData = "path/Filename.ms"

                'sName = ~DSEX~
                'sFID = 0 "n/a"
                'sTag =
                'sData = "path/Filename.ms"

                Dim bName As String = ""
                If sTag.StartsWith("-B=") Then
                    bName = sTag.Substring(3)
                    'store Botname to List at NewTab index
                End If

                '
                If Not String.IsNullOrEmpty(sData) And Not String.IsNullOrEmpty(bName) Then
                    OpenMS_File(sData, bName)

                ElseIf Not String.IsNullOrEmpty(sData) And String.IsNullOrEmpty(bName) Then
                    OpenMS_File(sData)
                Else
                    AddNewEditorTab(sData.ToString, "", 0)
                    NewFile(EditStyles.ms)
                End If
            End If
        Else
            MyBase.WndProc(m)
        End If

    End Sub

    ''' <summary>
    ''' </summary>
    ''' <param name="_ClassName">
    ''' </param>
    ''' <param name="_WindowName">
    ''' </param>
    ''' <returns>
    ''' </returns>
    <DllImport("user32.dll", EntryPoint:="FindWindow")>
    Private Shared Function FindWindow(_ClassName As String, _WindowName As String) As Integer
    End Function

    Public Declare Function SetFocusAPI Lib "user32.dll" Alias "SetFocus" (ByVal hWnd As Long) As Long
    Private Declare Function SetForegroundWindow Lib "user32" (ByVal hWnd As Long) As Long

    Private Declare Auto Function SendMessage Lib "user32" _
(ByVal hWnd As IntPtr,
ByVal Msg As Integer,
ByVal wParam As IntPtr,
ByRef lParam As COPYDATASTRUCT) As Boolean

#End Region

#Region "Event Handlers"

    Delegate Sub FileSave(ByRef IabIDX As Integer)

    Private Sub MokeySpeakLinesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MokeySpeakLinesToolStripMenuItem.Click
        If File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HelpFile)) Then
            Help.ShowHelp(Me, HelpFile, "/html/N_SilverMonkeyEngine_Engine_Libraries.htm")

        End If
    End Sub

#End Region

End Class