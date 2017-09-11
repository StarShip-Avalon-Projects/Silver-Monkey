<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> 
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wMain))
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FadeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OnToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.INIFormatToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OnToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.DsDescription = New System.Windows.Forms.RichTextBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.selecter = New MonkeyCore.Controls.ScrollingListBox()
        Me.WizMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.WizardRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.WizardEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewScriptToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.selector2 = New MonkeyCore.Controls.ScrollingListBox()
        Me.WizMenu_MS = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Refres_MS_ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.Edit_MS_ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewScriptMS_ToolStripMenuItem5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Delete_MS_ToolStripMenuItem6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Rename_MS_ToolStripMenuItem9 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer = New System.Windows.Forms.Timer(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.MenuStrip.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.WizMenu.SuspendLayout()
        Me.WizMenu_MS.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip
        '
        Me.MenuStrip.BackColor = System.Drawing.SystemColors.Control
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripMenuItem2, Me.HelpToolStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(554, 24)
        Me.MenuStrip.TabIndex = 0
        Me.MenuStrip.Text = "MenuStrip1"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem7})
        Me.ToolStripMenuItem1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(37, 20)
        Me.ToolStripMenuItem1.Text = "File"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(92, 22)
        Me.ToolStripMenuItem7.Text = "Exit"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.BackColor = System.Drawing.Color.Transparent
        Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FadeToolStripMenuItem, Me.INIFormatToolStripMenuItem})
        Me.ToolStripMenuItem2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(61, 20)
        Me.ToolStripMenuItem2.Text = "Settings"
        '
        'FadeToolStripMenuItem
        '
        Me.FadeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OnToolStripMenuItem})
        Me.FadeToolStripMenuItem.Name = "FadeToolStripMenuItem"
        Me.FadeToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.FadeToolStripMenuItem.Text = "Fade"
        '
        'OnToolStripMenuItem
        '
        Me.OnToolStripMenuItem.CheckOnClick = True
        Me.OnToolStripMenuItem.Name = "OnToolStripMenuItem"
        Me.OnToolStripMenuItem.Size = New System.Drawing.Size(90, 22)
        Me.OnToolStripMenuItem.Text = "On"
        Me.OnToolStripMenuItem.ToolTipText = "Fade in effect? Yes/No"
        '
        'INIFormatToolStripMenuItem
        '
        Me.INIFormatToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OnToolStripMenuItem1})
        Me.INIFormatToolStripMenuItem.Name = "INIFormatToolStripMenuItem"
        Me.INIFormatToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.INIFormatToolStripMenuItem.Text = "INI Format"
        '
        'OnToolStripMenuItem1
        '
        Me.OnToolStripMenuItem1.Checked = True
        Me.OnToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.OnToolStripMenuItem1.Name = "OnToolStripMenuItem1"
        Me.OnToolStripMenuItem1.Size = New System.Drawing.Size(90, 22)
        Me.OnToolStripMenuItem1.Text = "On"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem8})
        Me.HelpToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(107, 22)
        Me.ToolStripMenuItem8.Text = "About"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.DsDescription, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.SplitContainer1, 0, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 27)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.42211!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(552, 203)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'DsDescription
        '
        Me.DsDescription.BackColor = System.Drawing.SystemColors.Window
        Me.DsDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DsDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DsDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DsDescription.Location = New System.Drawing.Point(279, 3)
        Me.DsDescription.Name = "DsDescription"
        Me.DsDescription.Size = New System.Drawing.Size(270, 197)
        Me.DsDescription.TabIndex = 0
        Me.DsDescription.Text = ""
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.selecter)
        Me.SplitContainer1.Panel1Collapsed = True
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.selector2)
        Me.SplitContainer1.Size = New System.Drawing.Size(270, 197)
        Me.SplitContainer1.SplitterDistance = 89
        Me.SplitContainer1.TabIndex = 1
        '
        'selecter
        '
        Me.selecter.BackColor = System.Drawing.SystemColors.Window
        Me.selecter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.selecter.ContextMenuStrip = Me.WizMenu
        Me.selecter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.selecter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.selecter.FormattingEnabled = True
        Me.selecter.Location = New System.Drawing.Point(0, 0)
        Me.selecter.Name = "selecter"
        Me.selecter.Size = New System.Drawing.Size(150, 89)
        Me.selecter.TabIndex = 3
        '
        'WizMenu
        '
        Me.WizMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WizardRefresh, Me.ToolStripSeparator1, Me.WizardEdit, Me.NewScriptToolStripMenuItem, Me.RemoveToolStripMenuItem, Me.RenameToolStripMenuItem})
        Me.WizMenu.Name = "WizMenu"
        Me.WizMenu.Size = New System.Drawing.Size(132, 120)
        Me.WizMenu.Text = "Refresh"
        '
        'WizardRefresh
        '
        Me.WizardRefresh.Name = "WizardRefresh"
        Me.WizardRefresh.Size = New System.Drawing.Size(131, 22)
        Me.WizardRefresh.Text = "Refresh"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(128, 6)
        '
        'WizardEdit
        '
        Me.WizardEdit.Name = "WizardEdit"
        Me.WizardEdit.Size = New System.Drawing.Size(131, 22)
        Me.WizardEdit.Text = "Edit"
        '
        'NewScriptToolStripMenuItem
        '
        Me.NewScriptToolStripMenuItem.Name = "NewScriptToolStripMenuItem"
        Me.NewScriptToolStripMenuItem.Size = New System.Drawing.Size(131, 22)
        Me.NewScriptToolStripMenuItem.Text = "New Script"
        '
        'RemoveToolStripMenuItem
        '
        Me.RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem"
        Me.RemoveToolStripMenuItem.Size = New System.Drawing.Size(131, 22)
        Me.RemoveToolStripMenuItem.Text = "Delete"
        '
        'RenameToolStripMenuItem
        '
        Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
        Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(131, 22)
        Me.RenameToolStripMenuItem.Text = "Rename"
        '
        'selector2
        '
        Me.selector2.BackColor = System.Drawing.SystemColors.Window
        Me.selector2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.selector2.ContextMenuStrip = Me.WizMenu_MS
        Me.selector2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.selector2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.selector2.FormattingEnabled = True
        Me.selector2.Location = New System.Drawing.Point(0, 0)
        Me.selector2.Name = "selector2"
        Me.selector2.Size = New System.Drawing.Size(270, 197)
        Me.selector2.TabIndex = 3
        '
        'WizMenu_MS
        '
        Me.WizMenu_MS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Refres_MS_ToolStripMenuItem3, Me.ToolStripSeparator2, Me.Edit_MS_ToolStripMenuItem4, Me.NewScriptMS_ToolStripMenuItem5, Me.Delete_MS_ToolStripMenuItem6, Me.Rename_MS_ToolStripMenuItem9})
        Me.WizMenu_MS.Name = "WizMenu"
        Me.WizMenu_MS.Size = New System.Drawing.Size(132, 120)
        Me.WizMenu_MS.Text = "Refresh"
        '
        'Refres_MS_ToolStripMenuItem3
        '
        Me.Refres_MS_ToolStripMenuItem3.Name = "Refres_MS_ToolStripMenuItem3"
        Me.Refres_MS_ToolStripMenuItem3.Size = New System.Drawing.Size(131, 22)
        Me.Refres_MS_ToolStripMenuItem3.Text = "Refresh"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(128, 6)
        '
        'Edit_MS_ToolStripMenuItem4
        '
        Me.Edit_MS_ToolStripMenuItem4.Name = "Edit_MS_ToolStripMenuItem4"
        Me.Edit_MS_ToolStripMenuItem4.Size = New System.Drawing.Size(131, 22)
        Me.Edit_MS_ToolStripMenuItem4.Text = "Edit"
        '
        'NewScriptMS_ToolStripMenuItem5
        '
        Me.NewScriptMS_ToolStripMenuItem5.Name = "NewScriptMS_ToolStripMenuItem5"
        Me.NewScriptMS_ToolStripMenuItem5.Size = New System.Drawing.Size(131, 22)
        Me.NewScriptMS_ToolStripMenuItem5.Text = "New Script"
        '
        'Delete_MS_ToolStripMenuItem6
        '
        Me.Delete_MS_ToolStripMenuItem6.Name = "Delete_MS_ToolStripMenuItem6"
        Me.Delete_MS_ToolStripMenuItem6.Size = New System.Drawing.Size(131, 22)
        Me.Delete_MS_ToolStripMenuItem6.Text = "Delete"
        '
        'Rename_MS_ToolStripMenuItem9
        '
        Me.Rename_MS_ToolStripMenuItem9.Name = "Rename_MS_ToolStripMenuItem9"
        Me.Rename_MS_ToolStripMenuItem9.Size = New System.Drawing.Size(131, 22)
        Me.Rename_MS_ToolStripMenuItem9.Text = "Rename"
        '
        'Timer
        '
        Me.Timer.Interval = 1
        '
        'Timer1
        '
        Me.Timer1.Interval = 1
        '
        'Button1
        '
        Me.Button1.Enabled = False
        Me.Button1.Location = New System.Drawing.Point(100, 233)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(91, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Monkey Speak"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(3, 233)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(91, 23)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Dragon Speak"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'wMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(554, 258)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Controls.Add(Me.MenuStrip)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip
        Me.MaximizeBox = False
        Me.Name = "wMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Draconian Magic"
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.WizMenu.ResumeLayout(False)
        Me.WizMenu_MS.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetScriptsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShareScriptsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripDropDownButton1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ToolStripMenuItem8 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Timer As System.Windows.Forms.Timer
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FadeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OnToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents INIFormatToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OnToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DsDescription As System.Windows.Forms.RichTextBox
    Friend WithEvents WizMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents WizardRefresh As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents WizardEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewScriptToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RenameToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents selecter As MonkeyCore.Controls.ScrollingListBox

    Friend WithEvents selector2 As MonkeyCore.Controls.ScrollingListBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents WizMenu_MS As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Refres_MS_ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Edit_MS_ToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewScriptMS_ToolStripMenuItem5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Delete_MS_ToolStripMenuItem6 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Rename_MS_ToolStripMenuItem9 As System.Windows.Forms.ToolStripMenuItem

End Class
