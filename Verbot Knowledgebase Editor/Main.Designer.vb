﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.LstBxInput = New System.Windows.Forms.ListBox()
        Me.InputMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuCopyInput = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPasteInput = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewReplacementProfileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SynonymEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.LstBxOutput = New System.Windows.Forms.ListBox()
        Me.OutputMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuCopyOutputItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuPasteOutputItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.TreeMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteRuleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.ListBox4 = New System.Windows.Forms.ListBox()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.OpenFileDialog2 = New System.Windows.Forms.OpenFileDialog()
        Me.NewKnowedgebaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1.SuspendLayout()
        Me.InputMenu.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.OutputMenu.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TreeMenu.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button3)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.LstBxInput)
        Me.GroupBox1.Location = New System.Drawing.Point(266, 154)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(305, 172)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Inputs"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(168, 21)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 3
        Me.Button3.Text = "Delete"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(87, 21)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Edit"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(6, 21)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Add"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'LstBxInput
        '
        Me.LstBxInput.ContextMenuStrip = Me.InputMenu
        Me.LstBxInput.FormattingEnabled = True
        Me.LstBxInput.Location = New System.Drawing.Point(6, 50)
        Me.LstBxInput.Name = "LstBxInput"
        Me.LstBxInput.Size = New System.Drawing.Size(293, 95)
        Me.LstBxInput.TabIndex = 0
        '
        'InputMenu
        '
        Me.InputMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuCopyInput, Me.MenuPasteInput})
        Me.InputMenu.Name = "TreeMenu"
        Me.InputMenu.Size = New System.Drawing.Size(131, 48)
        '
        'MenuCopyInput
        '
        Me.MenuCopyInput.Name = "MenuCopyInput"
        Me.MenuCopyInput.Size = New System.Drawing.Size(130, 22)
        Me.MenuCopyInput.Text = "Copy Input"
        '
        'MenuPasteInput
        '
        Me.MenuPasteInput.Name = "MenuPasteInput"
        Me.MenuPasteInput.Size = New System.Drawing.Size(130, 22)
        Me.MenuPasteInput.Text = "Paste Input"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ToolsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(583, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewKnowedgebaseToolStripMenuItem, Me.OpenToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.SaveToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.SaveAsToolStripMenuItem.Text = "Save As"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewReplacementProfileToolStripMenuItem, Me.SynonymEditorToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'NewReplacementProfileToolStripMenuItem
        '
        Me.NewReplacementProfileToolStripMenuItem.Name = "NewReplacementProfileToolStripMenuItem"
        Me.NewReplacementProfileToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.NewReplacementProfileToolStripMenuItem.Text = "Replacement Profile"
        '
        'SynonymEditorToolStripMenuItem
        '
        Me.SynonymEditorToolStripMenuItem.Name = "SynonymEditorToolStripMenuItem"
        Me.SynonymEditorToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.SynonymEditorToolStripMenuItem.Text = "Synonym Editor"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button4)
        Me.GroupBox2.Controls.Add(Me.Button5)
        Me.GroupBox2.Controls.Add(Me.Button6)
        Me.GroupBox2.Controls.Add(Me.LstBxOutput)
        Me.GroupBox2.Location = New System.Drawing.Point(266, 330)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(305, 160)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Outputs"
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(168, 21)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(75, 23)
        Me.Button4.TabIndex = 3
        Me.Button4.Text = "Delete"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(87, 21)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(75, 23)
        Me.Button5.TabIndex = 2
        Me.Button5.Text = "Edit"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(6, 21)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(75, 23)
        Me.Button6.TabIndex = 1
        Me.Button6.Text = "Add"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'LstBxOutput
        '
        Me.LstBxOutput.ContextMenuStrip = Me.OutputMenu
        Me.LstBxOutput.FormattingEnabled = True
        Me.LstBxOutput.Location = New System.Drawing.Point(6, 50)
        Me.LstBxOutput.Name = "LstBxOutput"
        Me.LstBxOutput.Size = New System.Drawing.Size(293, 95)
        Me.LstBxOutput.TabIndex = 0
        '
        'OutputMenu
        '
        Me.OutputMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuCopyOutputItem, Me.menuPasteOutputItem})
        Me.OutputMenu.Name = "TreeMenu"
        Me.OutputMenu.Size = New System.Drawing.Size(139, 48)
        '
        'MenuCopyOutputItem
        '
        Me.MenuCopyOutputItem.Name = "MenuCopyOutputItem"
        Me.MenuCopyOutputItem.Size = New System.Drawing.Size(138, 22)
        Me.MenuCopyOutputItem.Text = "Copy Output"
        '
        'menuPasteOutputItem
        '
        Me.menuPasteOutputItem.Name = "menuPasteOutputItem"
        Me.menuPasteOutputItem.Size = New System.Drawing.Size(138, 22)
        Me.menuPasteOutputItem.Text = "Paste Output"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.TreeView1)
        Me.GroupBox3.Controls.Add(Me.Button10)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.Button9)
        Me.GroupBox3.Controls.Add(Me.TextBox1)
        Me.GroupBox3.Controls.Add(Me.Button8)
        Me.GroupBox3.Controls.Add(Me.Button7)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 52)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(248, 468)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Rules"
        '
        'TreeView1
        '
        Me.TreeView1.ContextMenuStrip = Me.TreeMenu
        Me.TreeView1.Location = New System.Drawing.Point(6, 87)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(236, 375)
        Me.TreeView1.TabIndex = 4
        '
        'TreeMenu
        '
        Me.TreeMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripMenuItem, Me.PasteRuleToolStripMenuItem})
        Me.TreeMenu.Name = "TreeMenu"
        Me.TreeMenu.Size = New System.Drawing.Size(126, 48)
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.CopyToolStripMenuItem.Text = "Copy Rule"
        '
        'PasteRuleToolStripMenuItem
        '
        Me.PasteRuleToolStripMenuItem.Name = "PasteRuleToolStripMenuItem"
        Me.PasteRuleToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.PasteRuleToolStripMenuItem.Text = "Paste Rule"
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(167, 29)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(75, 23)
        Me.Button10.TabIndex = 7
        Me.Button10.Text = "New Rule"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Rule Name:"
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(167, 58)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(75, 23)
        Me.Button9.TabIndex = 4
        Me.Button9.Text = "Delete Rule"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.AcceptsReturn = True
        Me.TextBox1.Location = New System.Drawing.Point(6, 32)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(155, 20)
        Me.TextBox1.TabIndex = 6
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(96, 58)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(65, 23)
        Me.Button8.TabIndex = 4
        Me.Button8.Text = "New Child"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(6, 58)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(84, 23)
        Me.Button7.TabIndex = 4
        Me.Button7.Text = "Rename Rule"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.DefaultExt = "vkb"
        Me.SaveFileDialog1.FileName = "Default"
        Me.SaveFileDialog1.Filter = "Knowedgebase Files | *.vkb"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.DefaultExt = "vkb"
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Filter = "Knowedgebase Files | *.vkb"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton2, Me.ToolStripButton3, Me.ToolStripButton4})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(583, 25)
        Me.ToolStrip1.TabIndex = 5
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "ToolStripButton1"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton2.Text = "ToolStripButton2"
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton3.Image = CType(resources.GetObject("ToolStripButton3.Image"), System.Drawing.Image)
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton3.Text = "ToolStripButton3"
        '
        'ToolStripButton4
        '
        Me.ToolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton4.Image = CType(resources.GetObject("ToolStripButton4.Image"), System.Drawing.Image)
        Me.ToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton4.Name = "ToolStripButton4"
        Me.ToolStripButton4.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton4.Text = "ToolStripButton4"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.ListBox4)
        Me.GroupBox4.Controls.Add(Me.Button12)
        Me.GroupBox4.Controls.Add(Me.Button11)
        Me.GroupBox4.Location = New System.Drawing.Point(266, 52)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(305, 100)
        Me.GroupBox4.TabIndex = 6
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Resource File Details"
        '
        'ListBox4
        '
        Me.ListBox4.FormattingEnabled = True
        Me.ListBox4.Location = New System.Drawing.Point(6, 19)
        Me.ListBox4.Name = "ListBox4"
        Me.ListBox4.Size = New System.Drawing.Size(212, 69)
        Me.ListBox4.TabIndex = 2
        '
        'Button12
        '
        Me.Button12.Location = New System.Drawing.Point(224, 65)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(75, 23)
        Me.Button12.TabIndex = 1
        Me.Button12.Text = "Remove File"
        Me.Button12.UseVisualStyleBackColor = True
        '
        'Button11
        '
        Me.Button11.Location = New System.Drawing.Point(224, 21)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(75, 23)
        Me.Button11.TabIndex = 0
        Me.Button11.Text = "Add File"
        Me.Button11.UseVisualStyleBackColor = True
        '
        'Button13
        '
        Me.Button13.Location = New System.Drawing.Point(272, 497)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(120, 23)
        Me.Button13.TabIndex = 7
        Me.Button13.Text = "Add Virtual Children"
        Me.Button13.UseVisualStyleBackColor = True
        '
        'OpenFileDialog2
        '
        Me.OpenFileDialog2.DefaultExt = "vrp"
        Me.OpenFileDialog2.FileName = "Default"
        Me.OpenFileDialog2.Filter = "Resource Files| *.rpp;*.sgp; *.csv; *.vrp;*.vsn"
        '
        'NewKnowedgebaseToolStripMenuItem
        '
        Me.NewKnowedgebaseToolStripMenuItem.Name = "NewKnowedgebaseToolStripMenuItem"
        Me.NewKnowedgebaseToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.NewKnowedgebaseToolStripMenuItem.Text = "New Knowedgebase"
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(583, 532)
        Me.Controls.Add(Me.Button13)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximumSize = New System.Drawing.Size(591, 560)
        Me.MinimumSize = New System.Drawing.Size(591, 560)
        Me.Name = "Main"
        Me.Text = "Main"
        Me.GroupBox1.ResumeLayout(False)
        Me.InputMenu.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.OutputMenu.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.TreeMenu.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents LstBxInput As System.Windows.Forms.ListBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents LstBxOutput As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents ListBox4 As System.Windows.Forms.ListBox
    Friend WithEvents Button12 As System.Windows.Forms.Button
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents Button13 As System.Windows.Forms.Button
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog2 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents NewReplacementProfileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SynonymEditorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents TreeMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteRuleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InputMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuCopyInput As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuPasteInput As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OutputMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuCopyOutputItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuPasteOutputItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewKnowedgebaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
