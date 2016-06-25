Imports System.IO
Imports System.Drawing.Text
Imports System.Drawing
Imports Furcadia.IO
Imports MonkeyCore.Settings



Public Class Config

    Dim CurrentListBox As ListBox
    Private Sub BTN_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_Cancel.Click
        'Close with out Saving
        Me.Close()
    End Sub

    Private Sub BTN_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_Ok.Click
        'Editor settings

        MS_Edit.EditSettings.FurcPath = TxtBxFurPath.Text

        MS_Edit.EditSettings.AutoCompleteEnable = ChkBxAutoComplete.Checked
        ' MS_Edit.AutocompleteMenu1.Enabled = ChkBxAutoComplete.Checked


        MS_Edit.EditSettings.CommentColor = CommentPictureBox.BackColor
        MS_Edit.EditSettings.StringColor = StringPictureBox.BackColor
        MS_Edit.EditSettings.NumberColor = NumberPictureBox.BackColor
        MS_Edit.EditSettings.VariableColor = VariablePictureBox.BackColor
        MS_Edit.EditSettings.IDColor = IDPictureBox.BackColor
        MS_Edit.EditSettings.StringVariableColor = StringVariableClrBx.BackColor

        For i As Integer = 0 To ListBox1.Items.Count - 1
            Dim item As String = ListBox1.Items(i).ToString
            Dim KV() As String = item.Split("="c)
            ini.SetKeyValue("C-Indents", KV(0), KV(1))
        Next

        MS_Edit.EditSettings.MS_CommentColor = MS_CommentPictureBox.BackColor
        MS_Edit.EditSettings.MS_StringColor = MS_StringPictureBox.BackColor
        MS_Edit.EditSettings.MS_NumberColor = MS_NumberPictureBox.BackColor
        MS_Edit.EditSettings.MS_VariableColor = MS_VariablePictureBox.BackColor
        MS_Edit.EditSettings.MS_IDColor = MS_IDPictureBox.BackColor
        For i As Integer = 0 To ListBox2.Items.Count - 1
            Dim item As String = ListBox1.Items(i).ToString
            Dim KV() As String = item.Split("="c)
            ini.SetKeyValue("MS-C-Indents", KV(0), KV(1))
        Next
        Dim Plugins As Dictionary(Of String, Boolean) = New Dictionary(Of String, Boolean)
        For Each lvItem As ListViewItem In ListView1.Items
            Plugins.Add(lvItem.SubItems.Item(1).Text.Replace(" ", ""), lvItem.Checked)
        Next
        MS_Edit.EditSettings.PluginList = Plugins
        'Save the settings to the ini file
        MS_Edit.EditSettings.SaveEditorSettings()

        If MS_Edit.Visible Then
            MS_Edit.Reset()
        End If



        Me.Dispose()

    End Sub

    Private Sub PopulatePluginList()

        Dim intIndex As Integer

        'Loop through available plugins, creating instances and adding them to listbox

        For Each F In Directory.GetFiles(Application.StartupPath + "/Plugins/", "*.ini")
            Dim fname As String = Path.GetFileNameWithoutExtension(F)
            Dim item As ListViewItem = ListView1.Items.Add(intIndex.ToString)
            item.SubItems.Add(fname)
            item.Checked = MS_Edit.EditSettings.PluginList.Item(fname.Replace(" ", ""))
        Next


    End Sub

    Private Sub Config_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.ConfigFormLocation = Me.Location
        My.Settings.Save()
    End Sub

    Private Sub Config_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ConfigTabs.SelectedIndex = My.Settings.ConfigSelectedTab
        CurrentListBox = ListBox1
        PopulatePluginList()
        Loadconfig()
        CurrentListBox.Select()
    End Sub

    Public Sub Loadconfig()


        'Editor

        ChkBxAutoComplete.Checked = CType(MS_Edit.EditSettings.AutoCompleteEnable, Boolean)

        CommentPictureBox.BackColor = MS_Edit.EditSettings.CommentColor
        StringPictureBox.BackColor = MS_Edit.EditSettings.StringColor
        NumberPictureBox.BackColor = MS_Edit.EditSettings.NumberColor
        VariablePictureBox.BackColor = MS_Edit.EditSettings.VariableColor
        IDPictureBox.BackColor = MS_Edit.EditSettings.IDColor
        StringVariableClrBx.BackColor = MS_Edit.EditSettings.StringVariableColor

        MS_CommentPictureBox.BackColor = MS_Edit.EditSettings.MS_CommentColor
        MS_StringPictureBox.BackColor = MS_Edit.EditSettings.MS_StringColor
        MS_NumberPictureBox.BackColor = MS_Edit.EditSettings.MS_NumberColor
        MS_VariablePictureBox.BackColor = MS_Edit.EditSettings.MS_VariableColor
        MS_IDPictureBox.BackColor = MS_Edit.EditSettings.MS_IDColor

        TxtBxFurPath.Text = MS_Edit.EditSettings.FurcPath

        Dim count As Integer = ini.GetKeyValue("Init-Types", "Count").ToInteger
        For i = 1 To count
            Dim key As String = ini.GetKeyValue("Init-Types", i.ToString)
            Dim s As String = ini.GetKeyValue("C-Indents", key)
            ListBox1.Items.Add(key + "=" + s)
        Next

        count = ini.GetKeyValue("MS-Init-Types", "Count").ToInteger
        For i = 1 To count
            Dim key As String = ini.GetKeyValue("MS-Init-Types", i.ToString)
            Dim s As String = ini.GetKeyValue("MS-C-Indents", key)
            ListBox2.Items.Add(key + "=" + s)
        Next

        Me.Location = My.Settings.ConfigFormLocation
    End Sub

    Private Sub ConfigTabs_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ConfigTabs.SelectedIndexChanged
        Dim t As TabControl = CType(sender, TabControl)
        My.Settings.ConfigSelectedTab = t.SelectedIndex
    End Sub


    Public Sub GetColor(ByRef ColorBX As System.Windows.Forms.PictureBox)
        Dim dlg As New ColorDialog
        dlg.Color = ColorBX.BackColor
        If dlg.ShowDialog() = DialogResult.OK Then
            ColorBX.BackColor = dlg.Color
        End If
    End Sub

    Private Sub CommentPictureBox_Click(sender As System.Object, e As System.EventArgs) Handles CommentPictureBox.Click, StringPictureBox.Click, NumberPictureBox.Click, VariablePictureBox.Click, IDPictureBox.Click, StringVariableClrBx.Click, _
                                                                                        MS_CommentPictureBox.Click, MS_StringPictureBox.Click, MS_NumberPictureBox.Click, MS_VariablePictureBox.Click, MS_IDPictureBox.Click
        GetColor(CType(sender, PictureBox))
    End Sub

    Private Sub ChkBxAutoComplete_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ChkBxAutoComplete.CheckedChanged
        MS_Edit.EditSettings.AutoCompleteEnable = ChkBxAutoComplete.Checked
    End Sub

    Private Sub NumericUpDown1_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles NumericUpDown1.KeyUp

        Dim i As Integer = CurrentListBox.SelectedIndex
        If i = -1 Then Exit Sub
        Dim Key As String = ""
        If CurrentListBox.Name = ListBox1.Name Then
            Key = ini.GetKeyValue("Init-Types", i.ToString)
        Else
            Key = ini.GetKeyValue("MS-Init-Types", i.ToString)
        End If

        CurrentListBox.Items.RemoveAt(i)
        CurrentListBox.Items.Insert(i, Key + "=" + NumericUpDown1.Value.ToString)

        CurrentListBox.SelectedIndex = i
    End Sub

    Private Sub ListBox1_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles ListBox2.MouseUp, ListBox1.MouseUp
        Dim l As ListBox = CType(sender, ListBox)
        Dim str As String = l.SelectedItem.ToString
        Dim Val As Integer = str.Split("="c)(1).ToInteger
        NumericUpDown1.Value = Val
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As System.EventArgs) Handles NumericUpDown1.ValueChanged

        Dim i As Integer = CurrentListBox.SelectedIndex
        If i = -1 Then Exit Sub
        Dim i2 As Integer = i + 1
        Dim Key As String = ""
        If CurrentListBox.Name = ListBox1.Name Then
            Key = ini.GetKeyValue("Init-Types", i2.ToString)
        Else
            Key = ini.GetKeyValue("MS-Init-Types", i2.ToString)
        End If

        CurrentListBox.Items.RemoveAt(i)
        CurrentListBox.Items.Insert(i, Key + "=" + NumericUpDown1.Value.ToString)
        CurrentListBox.SelectedIndex = i
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ListBox2.SelectedIndexChanged, ListBox1.SelectedIndexChanged
        CurrentListBox = CType(sender, ListBox)
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        With FindFurc
            Dim ExePath As String
            .FileName = "Furcadia.exe"

            If Environment.Is64BitOperatingSystem Then
                ExePath = Environment.GetEnvironmentVariable("ProgramFiles(x86)")
            Else
                ExePath = Environment.GetEnvironmentVariable("ProgramFiles")
            End If

            .InitialDirectory = ExePath
            If .ShowDialog = DialogResult.OK Then
                TxtBxFurPath.Text = Path.GetDirectoryName(.FileName)
            End If

        End With
    End Sub
End Class
