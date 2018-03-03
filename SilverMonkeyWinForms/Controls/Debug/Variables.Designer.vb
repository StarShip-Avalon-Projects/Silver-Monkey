Imports Controls

<CompilerServices.DesignerGenerated()>
Partial Class Variables
    Inherits System.Windows.Forms.Form


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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ListView1 = New Controls.ListView_NoFlicker()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChkBxRefresh = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ErrorLogTxtBx = New System.Windows.Forms.TextBox()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
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
        Me.SplitContainer1.Size = New System.Drawing.Size(879, 624)
        Me.SplitContainer1.SplitterDistance = 372
        Me.SplitContainer1.SplitterWidth = 11
        Me.SplitContainer1.TabIndex = 4
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(38, 87)
        Me.ListView1.Margin = New System.Windows.Forms.Padding(10, 9, 10, 9)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(785, 194)
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
        Me.ChkBxRefresh.Location = New System.Drawing.Point(38, 322)
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
        Me.Button1.Location = New System.Drawing.Point(604, 298)
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
        Me.ErrorLogTxtBx.Size = New System.Drawing.Size(879, 241)
        Me.ErrorLogTxtBx.TabIndex = 4
        Me.ErrorLogTxtBx.WordWrap = False
        '
        'Variables
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(879, 624)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Margin = New System.Windows.Forms.Padding(10, 9, 10, 9)
        Me.Name = "Variables"
        Me.Text = "Debug Window"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ChkBxRefresh As System.Windows.Forms.CheckBox
    Friend WithEvents ListView1 As ListView_NoFlicker
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Public WithEvents ErrorLogTxtBx As Windows.Forms.TextBox
End Class
