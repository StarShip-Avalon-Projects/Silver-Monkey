Imports MonkeyCore.Controls
Imports SilverMonkeyEngine

<CompilerServices.DesignerGenerated()>
Partial Class Variables
    Inherits System.Windows.Forms.Form

    Sub New(ByRef session As BotSession)
        FurcadiaSession = session
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Variables))
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ListView1 = New MonkeyCore.Controls.ListView_NoFlicker()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChkBxRefresh = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ErrorLogTxtBx = New System.Windows.Forms.TextBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripSplitButton1 = New System.Windows.Forms.ToolStripSplitButton()
        Me.DebugEnableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ErrorEnableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InfoEnableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WarningEnableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(48, 48)
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(48, 48)
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(19, 6, 0, 6)
        Me.MenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.MenuStrip1.Size = New System.Drawing.Size(879, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(10, 9, 10, 9)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListView1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ChkBxRefresh)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Button1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ErrorLogTxtBx)
        Me.SplitContainer1.Size = New System.Drawing.Size(879, 600)
        Me.SplitContainer1.SplitterDistance = 358
        Me.SplitContainer1.SplitterWidth = 11
        Me.SplitContainer1.TabIndex = 4
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(38, 87)
        Me.ListView1.Margin = New System.Windows.Forms.Padding(10, 9, 10, 9)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(785, 180)
        Me.ListView1.TabIndex = 6
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 254
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "IsConstant"
        Me.ColumnHeader2.Width = 195
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Value"
        Me.ColumnHeader3.Width = 1142
        '
        'ChkBxRefresh
        '
        Me.ChkBxRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkBxRefresh.AutoSize = True
        Me.ChkBxRefresh.Location = New System.Drawing.Point(38, 308)
        Me.ChkBxRefresh.Margin = New System.Windows.Forms.Padding(10, 9, 10, 9)
        Me.ChkBxRefresh.Name = "ChkBxRefresh"
        Me.ChkBxRefresh.Size = New System.Drawing.Size(218, 36)
        Me.ChkBxRefresh.TabIndex = 5
        Me.ChkBxRefresh.Text = "Auto Refresh"
        Me.ChkBxRefresh.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(38, 46)
        Me.Label1.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(173, 32)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Variable List"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(604, 284)
        Me.Button1.Margin = New System.Windows.Forms.Padding(10, 9, 10, 9)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(238, 65)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Refresh"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(38, 0)
        Me.Label2.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(285, 32)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Monkey Speak Errors"
        '
        'ErrorLogTxtBx
        '
        Me.ErrorLogTxtBx.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ErrorLogTxtBx.Location = New System.Drawing.Point(0, 0)
        Me.ErrorLogTxtBx.Margin = New System.Windows.Forms.Padding(10, 9, 10, 9)
        Me.ErrorLogTxtBx.Multiline = True
        Me.ErrorLogTxtBx.Name = "ErrorLogTxtBx"
        Me.ErrorLogTxtBx.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.ErrorLogTxtBx.Size = New System.Drawing.Size(879, 231)
        Me.ErrorLogTxtBx.TabIndex = 4
        Me.ErrorLogTxtBx.WordWrap = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(40, 40)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSplitButton1})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(879, 47)
        Me.ToolStrip1.TabIndex = 5
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripSplitButton1
        '
        Me.ToolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripSplitButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DebugEnableToolStripMenuItem, Me.ErrorEnableToolStripMenuItem, Me.InfoEnableToolStripMenuItem, Me.WarningEnableToolStripMenuItem})
        Me.ToolStripSplitButton1.Image = CType(resources.GetObject("ToolStripSplitButton1.Image"), System.Drawing.Image)
        Me.ToolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripSplitButton1.Name = "ToolStripSplitButton1"
        Me.ToolStripSplitButton1.Size = New System.Drawing.Size(73, 44)
        Me.ToolStripSplitButton1.Text = "ToolStripSplitButton1"
        '
        'DebugEnableToolStripMenuItem
        '
        Me.DebugEnableToolStripMenuItem.Name = "DebugEnableToolStripMenuItem"
        Me.DebugEnableToolStripMenuItem.Size = New System.Drawing.Size(331, 46)
        Me.DebugEnableToolStripMenuItem.Text = "DebugEnable"
        '
        'ErrorEnableToolStripMenuItem
        '
        Me.ErrorEnableToolStripMenuItem.Name = "ErrorEnableToolStripMenuItem"
        Me.ErrorEnableToolStripMenuItem.Size = New System.Drawing.Size(331, 46)
        Me.ErrorEnableToolStripMenuItem.Text = "Error Enable"
        '
        'InfoEnableToolStripMenuItem
        '
        Me.InfoEnableToolStripMenuItem.Name = "InfoEnableToolStripMenuItem"
        Me.InfoEnableToolStripMenuItem.Size = New System.Drawing.Size(331, 46)
        Me.InfoEnableToolStripMenuItem.Text = "InfoEnable"
        '
        'WarningEnableToolStripMenuItem
        '
        Me.WarningEnableToolStripMenuItem.Name = "WarningEnableToolStripMenuItem"
        Me.WarningEnableToolStripMenuItem.Size = New System.Drawing.Size(331, 46)
        Me.WarningEnableToolStripMenuItem.Text = "WarningEnable"
        '
        'Variables
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(879, 624)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(10, 9, 10, 9)
        Me.Name = "Variables"
        Me.Text = "Debug Window"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ErrorLogTxtBx As System.Windows.Forms.TextBox
    Friend WithEvents ChkBxRefresh As System.Windows.Forms.CheckBox
    Friend WithEvents ListView1 As ListView_NoFlicker
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ToolStrip1 As Windows.Forms.ToolStrip
    Friend WithEvents ToolStripSplitButton1 As Windows.Forms.ToolStripSplitButton
    Friend WithEvents DebugEnableToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ErrorEnableToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents InfoEnableToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents WarningEnableToolStripMenuItem As Windows.Forms.ToolStripMenuItem
End Class
