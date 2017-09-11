﻿Imports Conversive.Verbot5
Imports System.IO

Public Class Main

#Region "Public Fields"

    Public CopyInput As Input
    Public CopyOutput As Output
    Public CopyRule As Rule
    Public CurrentResourceFile As ResourceFile
    Public CurrentRule As Rule
    Public kb As KnowledgeBase = New KnowledgeBase()

#End Region

#Region "Private Fields"

    Private FileName As String = ""
    Dim KB_Changed As Boolean = False
    Private verbot As Verbot5Engine = New Verbot5Engine()

#End Region

#Region "Public Methods"

    Public Function TreeView_GetRootNode(objTV As TreeView) As TreeNode
        Dim xnode As TreeNode = Nothing

        If objTV.Nodes.Count > 0 Then
            ' Will err here if there are no treeview nodes
            xnode = objTV.Nodes(1)

            ' Get the first node's highest parent node
            Do While (xnode.Parent Is Nothing) = False
                xnode = xnode.Parent
            Loop

            ' Return the highest parent node's first sibling

        End If
        Return xnode.FirstNode
    End Function

    Sub walkNode(ByRef node As Rule, ByRef TV As TreeView, ByVal Tn As TreeNode)
        TV.SelectedNode = Tn
        If String.IsNullOrEmpty(node.Name) Then
            Tn = TV.SelectedNode.Nodes.Add(node.Id)
        Else
            Tn = TV.SelectedNode.Nodes.Add(node.Name)
        End If
        For i As Integer = 0 To node.Children.Count - 1

            walkNode(node.Children.Item(i), TV, Tn)
        Next
    End Sub

#End Region

#Region "Private Methods"

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim IPW As New InputWindow()
        With IPW
            .ShowDialog()
            If .DialogResult = Windows.Forms.DialogResult.OK Then
                CurrentRule.AddInput(IPW._CurrentInput.Text, IPW._CurrentInput.Condition)
                LstBxInput.Items.Add(CurrentRule.Inputs.Item(CurrentRule.Inputs.Count - 1).Text)
            End If
        End With
    End Sub

    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click
        Dim vRule As Rule = New Rule()
        vRule.Id = kb.GetNewRuleId()
        vRule.Name = TextBox1.Text
        TreeView1.Nodes.Add(TextBox1.Text)
        TextBox1.Text = ""
        kb.Rules.Add(vRule)
    End Sub

    Private Sub Button11_Click(sender As System.Object, e As System.EventArgs) Handles Button11.Click
        With OpenFileDialog2
            .InitialDirectory = MonkeyCore.Paths.SilverMonkeyBotPath
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim I As ResourceFile = New ResourceFile
                I.Filename = Path.GetFileName(.FileName)
                Select Case Path.GetExtension(.FileName).ToLower
                    Case ".rpp"
                        I.Filetype = ResourceFileType.ReplacementProfileFile
                    Case ".vrp"
                        I.Filetype = ResourceFileType.ReplacementProfileFile
                    Case ".sgp"
                        I.Filetype = ResourceFileType.SynonymFile
                    Case ".vsn"
                        I.Filetype = ResourceFileType.SynonymFile
                    Case ".csv"
                        I.Filetype = ResourceFileType.TemplateDataFile
                    Case Else
                        I.Filetype = ResourceFileType.Other
                End Select
                CurrentResourceFile = I
                kb.ResourceFiles.Add(I)
                ListBox4.Items.Add(Path.GetFileName(.FileName))
            End If
        End With
    End Sub

    Private Sub Button12_Click(sender As System.Object, e As System.EventArgs) Handles Button12.Click
        If ListBox4.SelectedIndex = -1 Then Exit Sub
        kb.ResourceFiles.RemoveAt(ListBox4.SelectedIndex)
        ListBox4.Items.RemoveAt(ListBox4.SelectedIndex)
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click, LstBxInput.DoubleClick
        If IsNothing(CurrentRule) Or LstBxInput.SelectedIndex = -1 Then Exit Sub
        Dim IPW As New InputWindow(CurrentRule.Inputs.Item(LstBxInput.SelectedIndex))
        With IPW
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim Idx As Integer = LstBxInput.SelectedIndex
                CurrentRule.Inputs.Item(Idx) = IPW._CurrentInput
                LstBxInput.Items.RemoveAt(Idx)
                LstBxInput.Items.Insert(Idx, IPW._CurrentInput.Text)
            End If
        End With
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If LstBxInput.SelectedIndex = -1 Then Exit Sub
        CurrentRule.Inputs.RemoveAt(LstBxInput.SelectedIndex)
        LstBxInput.Items.RemoveAt(LstBxInput.SelectedIndex)
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        If LstBxOutput.SelectedIndex = -1 Then Exit Sub
        CurrentRule.Outputs.RemoveAt(LstBxOutput.SelectedIndex)
        LstBxOutput.Items.RemoveAt(LstBxOutput.SelectedIndex)
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click, LstBxOutput.DoubleClick
        If IsNothing(CurrentRule) Or LstBxOutput.SelectedIndex = -1 Then Exit Sub
        Dim IPW As New OutputWindow(CurrentRule.Outputs.Item(LstBxOutput.SelectedIndex))
        With IPW
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim idx As Integer = LstBxOutput.SelectedIndex
                CurrentRule.Outputs.Item(idx) = IPW._CurrentOutput
                LstBxOutput.Items.RemoveAt(idx)
                LstBxOutput.Items.Insert(idx, IPW._CurrentOutput.Text)
            End If
        End With
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        Dim IPW As New OutputWindow()
        With IPW

            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                CurrentRule.AddOutput(IPW._CurrentOutput.Text, IPW._CurrentOutput.Condition, IPW._CurrentOutput.Cmd)
                LstBxOutput.Items.Add(CurrentRule.Outputs.Item(CurrentRule.Outputs.Count - 1).Text)
            End If
        End With
    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        If TreeView1.SelectedNode.Index = -1 Then Exit Sub
        CurrentRule.Name = TextBox1.Text
        TreeView1.SelectedNode.Text = TextBox1.Text
    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click
        Dim tv As TreeView = TreeView1
        Dim tn As TreeNode = tv.SelectedNode
        tn.Nodes.Add(TextBox1.Text)
        Dim ParentRule As Rule = kb.GetRuleByNameOrId(tn.FullPath)
        Dim vRule As Rule = New Rule()
        vRule.Id = kb.GetNewRuleId()
        vRule.Name = TextBox1.Text
        ParentRule.Children.Add(vRule)

    End Sub

    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click
        If TreeView1.SelectedNode.Index = -1 Then Exit Sub
        LstBxOutput.Items.Clear()
        LstBxInput.Items.Clear()
        kb.DeleteRule(CurrentRule.Id)
        TreeView1.Nodes.Remove(TreeView1.SelectedNode)
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        CopyRule = kb.GetRuleByNameOrId(TreeView1.SelectedNode.FullPath)
    End Sub

    Private Sub ListBox3_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles TreeView1.AfterSelect

        If IsNothing(TreeView1.SelectedNode) Then Exit Sub
        If TreeView1.SelectedNode.Index = -1 Then Exit Sub
        LstBxOutput.Items.Clear()
        LstBxInput.Items.Clear()
        CurrentRule = kb.GetRuleByNameOrId(TreeView1.SelectedNode.FullPath)
        If IsNothing(CurrentRule) Then
            CurrentRule = kb.GetRuleByNameOrId(TreeView1.SelectedNode.Parent.FullPath)
            Dim R As New Rule
            R.Id = kb.GetNewRuleId
            R.Name = TreeView1.SelectedNode.Text
            CurrentRule = R
        End If

        For i As Integer = 0 To CurrentRule.Inputs.Count - 1
            LstBxInput.Items.Add(CurrentRule.Inputs.Item(i).Text)
        Next

        For i As Integer = 0 To CurrentRule.Outputs.Count - 1
            LstBxOutput.Items.Add(CurrentRule.Outputs.Item(i).Text)
        Next
    End Sub

    Private Sub ListBox4_DoubleClick(sender As Object, e As System.EventArgs) Handles ListBox4.DoubleClick
        CurrentResourceFile = kb.ResourceFiles.Item(ListBox4.SelectedIndex)
        Select Case Path.GetExtension(kb.ResourceFiles.Item(ListBox4.SelectedIndex).Filename).ToLower
            Case ".rpp"

                With ReplacementEditor
                    .FilePath = Path.GetDirectoryName(FileName)
                    .FileName = ListBox4.SelectedItem.ToString
                    If .ShowDialog = Windows.Forms.DialogResult.OK Then
                        CurrentResourceFile = kb.ResourceFiles.Item(ListBox4.SelectedIndex)
                        CurrentResourceFile.Filename = Path.GetFileName(.FileName)
                        CurrentResourceFile.Filetype = ResourceFileType.ReplacementProfileFile
                    End If
                End With
            Case ".vrp"

                With ReplacementEditor
                    .FilePath = Path.GetDirectoryName(FileName)
                    .FileName = ListBox4.SelectedItem.ToString
                    If .ShowDialog = Windows.Forms.DialogResult.OK Then
                        CurrentResourceFile = kb.ResourceFiles.Item(ListBox4.SelectedIndex)
                        CurrentResourceFile.Filename = Path.GetFileName(.FileName)
                        CurrentResourceFile.Filetype = ResourceFileType.ReplacementProfileFile
                    End If
                End With
            Case ".sgp"
                Dim Test As New SynonymEditor(Path.GetDirectoryName(FileName) & "\" & kb.ResourceFiles.Item(ListBox4.SelectedIndex).Filename)
                Test.Show()
                Test.Activate()
                CurrentResourceFile.Filetype = ResourceFileType.SynonymFile
            Case ".vsn"

                Dim Test As New SynonymEditor(Path.GetDirectoryName(FileName) & "\" & kb.ResourceFiles.Item(ListBox4.SelectedIndex).Filename)
                Test.Show()
                Test.Activate()
                CurrentResourceFile.Filetype = ResourceFileType.SynonymFile
            Case Else
                CurrentResourceFile.Filetype = ResourceFileType.Other
        End Select

    End Sub

    Private Sub Main_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        OpenFileDialog1.InitialDirectory = MonkeyCore.Paths.SilverMonkeyBotPath
        OpenFileDialog2.InitialDirectory = MonkeyCore.Paths.SilverMonkeyBotPath
        SaveFileDialog1.InitialDirectory = MonkeyCore.Paths.SilverMonkeyBotPath
    End Sub

    Private Sub MenuCopyInput_Click(sender As System.Object, e As System.EventArgs) Handles MenuCopyInput.Click
        CopyInput = CurrentRule.Inputs.Item(LstBxInput.SelectedIndex)
    End Sub

    Private Sub MenuCopyOutputItem_Click(sender As System.Object, e As System.EventArgs) Handles MenuCopyOutputItem.Click
        CopyOutput = New Output
        CopyOutput.Id = CurrentRule.GetNewOutputId
        CopyOutput.Text = CurrentRule.Outputs.Item(LstBxOutput.SelectedIndex).Text
        CopyOutput.Cmd = CurrentRule.Outputs.Item(LstBxOutput.SelectedIndex).Cmd
        CopyOutput.Condition = CurrentRule.Outputs.Item(LstBxOutput.SelectedIndex).Condition
    End Sub

    Private Sub MenuPasteInput_Click(sender As System.Object, e As System.EventArgs) Handles MenuPasteInput.Click
        '
        Dim InputCopy As New Input
        InputCopy.Id = CurrentRule.GetNewInputId
        InputCopy.Text = CopyInput.Text
        InputCopy.Condition = CopyInput.Condition
        CurrentRule.Inputs.Add(InputCopy)

        LstBxInput.Items.Add(InputCopy.Text)
    End Sub

    Private Sub menuPasteOutputItem_Click(sender As System.Object, e As System.EventArgs) Handles menuPasteOutputItem.Click
        Dim outputcopy As New Output
        outputcopy.Id = CurrentRule.GetNewOutputId
        outputcopy.Text = CopyOutput.Text
        outputcopy.Cmd = CopyOutput.Cmd
        outputcopy.Condition = CopyOutput.Condition
        CurrentRule.Outputs.Add(outputcopy)
        LstBxOutput.Items.Add(outputcopy.Text)

    End Sub

    Private Sub NewReplacementProfileToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewReplacementProfileToolStripMenuItem.Click
        With ReplacementEditor

            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                CurrentResourceFile = New ResourceFile

                CurrentResourceFile.Filename = .FileName
                Select Case Path.GetExtension(.FileName).ToLower
                    Case ".rpp"
                        CurrentResourceFile.Filetype = ResourceFileType.ReplacementProfileFile
                    Case ".vrp"
                        CurrentResourceFile.Filetype = ResourceFileType.ReplacementProfileFile
                    Case ".sgp"
                        CurrentResourceFile.Filetype = ResourceFileType.SynonymFile
                    Case ".vsn"
                        CurrentResourceFile.Filetype = ResourceFileType.SynonymFile
                    Case ".csv"
                        CurrentResourceFile.Filetype = ResourceFileType.TemplateDataFile
                    Case Else
                        CurrentResourceFile.Filetype = ResourceFileType.Other
                End Select
                kb.ResourceFiles.Add(CurrentResourceFile)
                ListBox4.Items.Add(.FileName)
            End If
        End With
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OpenToolStripMenuItem.Click

        With OpenFileDialog1
            ' Select Character ini file
            If .ShowDialog = DialogResult.OK Then
                Dim xToolbox As XMLToolbox = New XMLToolbox(GetType(KnowledgeBase))
                kb = CType(xToolbox.LoadXML(.FileName), KnowledgeBase)
                FileName = .FileName
                LstBxInput.Items.Clear()
                LstBxOutput.Items.Clear()
                TreeView1.Nodes.Clear()
                ListBox4.Items.Clear()
                For i As Integer = 0 To kb.Rules.Count - 1
                    If String.IsNullOrEmpty(kb.Rules.Item(i).Name) Then

                        Dim tn As TreeNode = TreeView1.Nodes.Add(kb.Rules.Item(i).Id)
                        For j As Integer = 0 To kb.Rules.Item(i).Children.Count - 1
                            walkNode(kb.Rules.Item(i).Children.Item(j), TreeView1, tn)
                        Next
                    Else
                        Dim tn As TreeNode = TreeView1.Nodes.Add(kb.Rules.Item(i).Name)
                        For j As Integer = 0 To kb.Rules.Item(i).Children.Count - 1
                            walkNode(kb.Rules.Item(i).Children.Item(j), TreeView1, tn)
                        Next

                    End If
                Next
                For i As Integer = 0 To kb.ResourceFiles.Count - 1
                    ListBox4.Items.Add(kb.ResourceFiles.Item(i))
                Next
            End If
        End With
    End Sub
    Private Sub PasteRuleToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PasteRuleToolStripMenuItem.Click
        CurrentRule = kb.GetRuleByNameOrId(TreeView1.SelectedNode.FullPath)
        CopyRule.Id = kb.GetNewRuleId
        CurrentRule.Children.Add(CopyRule)
        TreeView1.SelectedNode.Nodes.Add(CopyRule.Name)
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        With SaveFileDialog1
            ' Select Character ini file
            If .ShowDialog = DialogResult.OK Then
                Dim xToolbox As XMLToolbox = New XMLToolbox(GetType(KnowledgeBase))
                FileName = .FileName
                xToolbox.SaveXML(kb, .FileName)
            End If
        End With
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveToolStripMenuItem.Click

        Dim xToolbox As XMLToolbox = New XMLToolbox(GetType(KnowledgeBase))
        xToolbox.SaveXML(kb, FileName)

    End Sub
    Private Sub SynonymEditorToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SynonymEditorToolStripMenuItem.Click

        If Not IsNothing(SynonymEditor) Then
            SynonymEditor.Dispose()
        End If
        SynonymEditor.Show()
        SynonymEditor.Activate()
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        Dim t As TextBox = CType(sender, TextBox)

        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            Dim vRule As Rule = New Rule()
            vRule.Id = kb.GetNewRuleId()
            vRule.Name = t.Text
            TreeView1.Nodes.Add(TextBox1.Text)
            t.Text = ""
            kb.Rules.Add(vRule)

        End If
    End Sub

#End Region

End Class