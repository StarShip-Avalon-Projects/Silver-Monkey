Imports MonkeyCore.Controls

<CompilerServices.DesignerGenerated()>
Partial Class Main
    Inherits Form

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
        Me.Label1 = New Label()
        Me.ListView1 = New ListView()
        Me.ColumnHeader1 = New ColumnHeader()
        Me.ColumnHeader2 = New ColumnHeader()
        Me.ColumnHeader3 = New ColumnHeader()
        Me.MenuStrip1 = New MenuStrip()
        Me.ExportToolStripMenuItem = New ToolStripMenuItem()
        Me.ExportCurrentToolStripMenuItem = New ToolStripMenuItem()
        Me.TextBox1 = New TextBox_NoFlicker()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(144, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Plugins:"
        '
        'ListView1
        '
        Me.ListView1.BorderStyle = BorderStyle.FixedSingle
        Me.ListView1.CheckBoxes = True
        Me.ListView1.Columns.AddRange(New ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(15, 43)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(473, 106)
        Me.ListView1.TabIndex = 6
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Enabled"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Name"
        Me.ColumnHeader2.Width = 121
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Description"
        Me.ColumnHeader3.Width = 289
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New ToolStripItem() {Me.ExportToolStripMenuItem, Me.ExportCurrentToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(514, 24)
        Me.MenuStrip1.TabIndex = 7
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ExportToolStripMenuItem
        '
        Me.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem"
        Me.ExportToolStripMenuItem.Size = New System.Drawing.Size(65, 20)
        Me.ExportToolStripMenuItem.Text = "Export All"
        '
        'ExportCurrentToolStripMenuItem
        '
        Me.ExportCurrentToolStripMenuItem.Name = "ExportCurrentToolStripMenuItem"
        Me.ExportCurrentToolStripMenuItem.Size = New System.Drawing.Size(91, 20)
        Me.ExportCurrentToolStripMenuItem.Text = "Export Current"
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(15, 155)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(473, 155)
        Me.TextBox1.TabIndex = 8
        Me.TextBox1.WordWrap = False
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(514, 316)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Main"
        Me.Text = "Silver Monkey Module Explorer"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As Label
    Friend WithEvents ListView1 As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ExportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TextBox1 As TextBox_NoFlicker
    Friend WithEvents ExportCurrentToolStripMenuItem As ToolStripMenuItem
End Class
