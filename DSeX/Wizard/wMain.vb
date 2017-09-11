Imports System.IO
Imports MonkeyCore
Imports MonkeyCore.Paths

'######### Please Read ##############
'
' This Project was originally not intended to be "open-source".
' However the source is released and if you use a lot of the source
' provided please, in your project, give credit where credit is due.
'
' Sincerely,
' Squizzle (in-game)
'####################################

'Begin the main Form!
Public Class wMain

#Region "Public Methods"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    ''' <summary>
    ''' </summary>
    ''' <param name="sIni">
    ''' </param>
    Public Sub GetInfo(ByVal sIni As String)
        Try
            ScriptIni.Load(sIni)
            dsdesc.Text = ""
            Dim s As String = " "
            Dim i As Integer = 1
            s = ScriptIni.GetKeyValue("main", "name")
            If s <> "" Then dsdesc.AppendText("Name: " + s + vbLf)
            s = ScriptIni.GetKeyValue("main", "Author")
            If s <> "" Then dsdesc.AppendText("Author: " + s + vbLf)
            Do While s <> ""
                s = ScriptIni.GetKeyValue("main", "d" + i.ToString)
                dsdesc.AppendText(s + vbLf)
                i += 1
            Loop
            s = ScriptIni.GetKeyValue("main", "DefaultRepeat")
            If IsInteger(s) Then
                wUI.NumericUpDown1.Value = s.ToInteger
            Else
                wUI.NumericUpDown1.Value = 0
            End If
        Catch ex As Exception
            Dim x As New ErrorLogging(ex, Me)
        End Try

    End Sub

    Public Sub GetParams(ByVal sIni As String)
        Try

            wUI.Code = ScriptIni.Code
            wUI.selecter2.Items.Clear()
            wUI.ListBox1.Items.Clear()
            Dim s As String = " "
            Dim j As String = ""
            Dim t As String = ""
            Dim i As Integer = 1
            Do While s <> ""
                s = ScriptIni.GetKeyValue("main", "v" + i.ToString)
                If s <> "" Then
                    wUI.selecter2.Items.Add(s)
                End If
                j = ScriptIni.GetKeyValue("main", "b" + i.ToString)
                If j <> "" And s <> "" Then
                    wUI.ListBox1.Items.Add(s)
                ElseIf s <> "" Then
                    wUI.ListBox1.Items.Add("")
                End If
                'wUI.selecter2.Items.Add(s)
                t = ScriptIni.GetKeyValue("main", "m" + i.ToString)
                'If t = "" And s <> "" Then t = "text"
                'If t <> "" Then wUI.ListBox1.Items.Add(t)
                i += 1
            Loop
            wUI.SetUI()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Exclamation, "Error!")
        End Try
        wUI.Text = FileIO.FileSystem.GetName(sIni)
    End Sub

#End Region

#Region "Private Methods"

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click, Button2.Click
        Dim btn As Button = CType(sender, Button)
        Button1.Enabled = Not Button1.Enabled
        Button2.Enabled = Not Button2.Enabled
        If btn.Name = "Button1" Then
            SplitContainer1.Panel2Collapsed = False
            SplitContainer1.Panel1Collapsed = True
        Else
            SplitContainer1.Panel2Collapsed = True
            SplitContainer1.Panel1Collapsed = False
        End If

    End Sub

    Private Sub Delete_MS_ToolStripMenuItem6_Click(sender As System.Object, e As System.EventArgs) Handles Delete_MS_ToolStripMenuItem6.Click
        Dim reply As DialogResult = MessageBox.Show("Really delete this script?", "Caption",
MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If reply = DialogResult.OK Then
            File.Delete(ScriptPaths_MS.Item(selector2.SelectedIndex) + selector2.SelectedItem.ToString + ".ini")
            ScriptPaths_MS.RemoveAt(selector2.SelectedIndex)
            selector2.Items.RemoveAt(selector2.SelectedIndex)
        End If
    End Sub

    Private Sub Edit_MS_ToolStripMenuItem4_Click(sender As System.Object, e As System.EventArgs) Handles Edit_MS_ToolStripMenuItem4.Click
        MS_Edit.OpenMS_File(ScriptPaths_MS.Item(selector2.SelectedIndex) + "/" + selector2.SelectedItem.ToString + ".ini")
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        LoadOptions()
        If OnToolStripMenuItem.Checked = True Then
            MyBase.Opacity = 0.0
            Timer1.Enabled = True
        End If
        'Gets the scripts.ini files in your "Scripts" folder
        GetScriptIni()
    End Sub

    Private Sub GetScriptIni()
        selecter.Items.Clear()
        selecter.BeginUpdate()
        Dim p As String = Path.Combine(ApplicationPath, "Scripts")
        Directory.CreateDirectory(p)
        ScriptPaths.Clear()
        For Each s As String In FileIO.FileSystem.GetFiles(p, FileIO.SearchOption.SearchTopLevelOnly, "*.ini")
            s = Path.GetFileNameWithoutExtension(s)
            selecter.Items.Add(s)
            ScriptPaths.Add(p)
        Next

        p = Path.Combine(Path.Combine(Paths.FurcadiaDocumentsFolder, "Scripts"))
        'path = Enviroment.GetFolderPath(Enviroment.SpecialFolderMyDocuments) + My_Docs + "/Scripts"
        Directory.CreateDirectory(p)
        For Each s As String In FileIO.FileSystem.GetFiles(p, FileIO.SearchOption.SearchTopLevelOnly, "*.ini")
            s = Path.GetFileNameWithoutExtension(s)
            selecter.Items.Add(s)
            ScriptPaths.Add(p)
        Next
        selecter.EndUpdate()

        selector2.Items.Clear()
        selector2.BeginUpdate()
        p = MonKeySpeakEditorScriptsPath
        Directory.CreateDirectory(p)
        ScriptPaths_MS.Clear()
        For Each s As String In FileIO.FileSystem.GetFiles(p, FileIO.SearchOption.SearchTopLevelOnly, "*.ini")
            s = Path.GetFileNameWithoutExtension(s)
            selector2.Items.Add(s)
            ScriptPaths_MS.Add(p)
        Next

        'p = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Scripts-MS/"
        p = MonKeySpeakEditorDocumentsScriptsPath
        Directory.CreateDirectory(p)
        For Each s As String In FileIO.FileSystem.GetFiles(p, FileIO.SearchOption.SearchTopLevelOnly, "*.ini")
            Dim e As String = Path.GetExtension(s)
            s = Path.GetFileNameWithoutExtension(s)
            selector2.Items.Add(s)
            ScriptPaths_MS.Add(p)
        Next
        selector2.EndUpdate()
    End Sub

    Private Sub LoadOptions()
        Dim a As Integer = 0
        'Dim a = IniRead(AppPath & "\Settings", "Main", "Fade", 1)
        If a = 0 Then
            OnToolStripMenuItem.Checked = False
            OnToolStripMenuItem.CheckState = CheckState.Unchecked
        Else
            OnToolStripMenuItem.Checked = True
            OnToolStripMenuItem.CheckState = CheckState.Checked
        End If

    End Sub

    Private Sub NewScriptToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewScriptToolStripMenuItem.Click, NewScriptMS_ToolStripMenuItem5.Click
        MS_Edit.AddNewEditorTab("", "", 0)
        MS_Edit.NewFile(EditStyles.ini)
    End Sub

    Private Sub OnToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OnToolStripMenuItem.Click
        If OnToolStripMenuItem.Checked = True And OnToolStripMenuItem.CheckState = CheckState.Checked Then
            ' IniWrite(AppPath & "\Settings", "Main", "Fade", 1)
        Else
            ' IniWrite(AppPath & "\Settings", "Main", "Fade", 0)
        End If

    End Sub

    Private Sub OnToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OnToolStripMenuItem1.Click
        MsgBox(sender.ToString)
    End Sub

    Private Sub RemoveToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RemoveToolStripMenuItem.Click
        Dim reply As DialogResult = MessageBox.Show("Really delete this script?", "Caption",
MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If reply = DialogResult.OK Then
            File.Delete(ScriptPaths.Item(selecter.SelectedIndex) + selecter.SelectedItem.ToString + ".ini")
            ScriptPaths.RemoveAt(selecter.SelectedIndex)
            selecter.Items.RemoveAt(selecter.SelectedIndex)
        End If
    End Sub

    Private Sub Rename_MS_ToolStripMenuItem9_Click(sender As System.Object, e As System.EventArgs) Handles Rename_MS_ToolStripMenuItem9.Click
        Dim s As String = Microsoft.VisualBasic.InputBox("New Name?")
        If String.IsNullOrEmpty(s) Then Exit Sub
        My.Computer.FileSystem.RenameFile(ScriptPaths_MS.Item(selector2.SelectedIndex) + "/" + selector2.SelectedItem.ToString + ".ini", ScriptPaths_MS(selector2.SelectedIndex) + s + ".ini")

    End Sub

    Private Sub RenameToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RenameToolStripMenuItem.Click
        Dim s As String = Microsoft.VisualBasic.InputBox("New Name?")
        If String.IsNullOrEmpty(s) Then Exit Sub
        My.Computer.FileSystem.RenameFile(ScriptPaths.Item(selecter.SelectedIndex) + "/" + selecter.SelectedItem.ToString + ".ini", ScriptPaths(selecter.SelectedIndex) + s + ".ini")

    End Sub

    Private Sub selecter_DoubleClick(sender As Object, e As System.EventArgs) Handles selecter.DoubleClick, selector2.DoubleClick
        Dim lb As ListBox = CType(sender, ListBox)
        Dim lst As List(Of String)
        If lb.Name = "selecter" Then
            lst = ScriptPaths
        Else
            lst = ScriptPaths_MS
        End If

        Dim sIni = lb.GetItemText(lb.SelectedItem)
        If wUI.IsDisposed = False Then
            wUI.Dispose()
        End If
        If System.IO.File.Exists(Path.Combine(ApplicationPath, "help.txt")) Then
            wUI.dsdesc2.Text = FileIO.FileSystem.ReadAllText(Path.Combine(ApplicationPath, "help.txt"))
        Else
            wUI.dsdesc2.Text = "Error: " & Path.Combine(ApplicationPath, "help.txt") & " doesn't exist.  Help contents cannot be displayed."
        End If
        wUI.PathIndex = lb.SelectedIndex
        GetParams(lst(lb.SelectedIndex) & sIni & ".ini")
        wUI.wVariables.Clear()
        wUI.NumericUpDown1.Value = 1
        wUI.wVariables.Clear()
        wUI.Show()
    End Sub

    Private Sub selecter_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles selecter.MouseDown, selector2.MouseDown
        Dim lb As ListBox = CType(sender, ListBox)
        If e.Button = Windows.Forms.MouseButtons.Right Then
            lb.SelectedIndex = lb.IndexFromPoint(New Point(e.X, e.Y))
        End If
    End Sub

    Private Sub selecter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles selecter.SelectedIndexChanged, selector2.SelectedIndexChanged
        Dim lb As ListBox = CType(sender, ListBox)
        Dim lst As List(Of String) = New List(Of String)
        If lb.Name = "selecter" Then
            lst = ScriptPaths
        Else
            lst = ScriptPaths_MS
        End If
        If System.IO.File.Exists(lst(lb.SelectedIndex) & lb.GetItemText(lb.SelectedItem) & ".ini") Then
            Dim sIni = selecter.GetItemText(lb.SelectedItem)
            GetInfo(lst(lb.SelectedIndex) & sIni & ".ini")
        End If
    End Sub

    Private Sub Timer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer.Tick
        'fade stuffs
        Me.Opacity -= 0.01
        If Me.Opacity = 0 Then Me.Dispose()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        'fade stuffs
        Me.Opacity += 0.01
        If Me.Opacity = 100 Then
            Me.Show()
        End If
    End Sub

    Private Sub ToolStripMenuItem7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem7.Click
        'ExitToolStrip
        If wUI.IsDisposed = False Then
            wUI.Dispose()
            If Me.OnToolStripMenuItem.Checked = True Then
                If OnToolStripMenuItem.Checked = True Then
                    Timer1.Enabled = False
                    Timer.Enabled = True
                End If
            Else
                Me.Dispose()
            End If
        Else
            If Me.OnToolStripMenuItem.Checked = True Then
                If OnToolStripMenuItem.Checked = True Then
                    Timer1.Enabled = False
                    Timer.Enabled = True
                End If
            Else
                Me.Dispose()
            End If
        End If

    End Sub

    Private Sub ToolStripMenuItem8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
                                                                                ToolStripMenuItem8.Click, AboutToolStripMenuItem.Click
        'AboutToolStrip
        AboutBox2.Show()
        AboutBox2.Activate()
    End Sub

    Private Sub WizardEdit_Click(sender As System.Object, e As System.EventArgs) Handles WizardEdit.Click
        'MS_Edit.AddNewEditorTab("", "", 0)
        MS_Edit.OpenMS_File(ScriptPaths.Item(selecter.SelectedIndex) + "/" + selecter.SelectedItem.ToString + ".ini")
    End Sub

    Private Sub WizardRefresh_Click(sender As System.Object, e As System.EventArgs) Handles WizardRefresh.Click, Refres_MS_ToolStripMenuItem3.Click
        GetScriptIni()
    End Sub

#End Region

End Class