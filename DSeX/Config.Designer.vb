<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class Config
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Config))
        Me.BTN_Ok = New System.Windows.Forms.Button()
        Me.BTN_Cancel = New System.Windows.Forms.Button()
        Me.IniBrowseDialog = New System.Windows.Forms.OpenFileDialog()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.MS_IDPictureBox = New System.Windows.Forms.PictureBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.MS_NumberPictureBox = New System.Windows.Forms.PictureBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.MS_VariablePictureBox = New System.Windows.Forms.PictureBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.MS_CommentPictureBox = New System.Windows.Forms.PictureBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.MS_StringPictureBox = New System.Windows.Forms.PictureBox()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.StringVariableClrBx = New System.Windows.Forms.PictureBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.IDPictureBox = New System.Windows.Forms.PictureBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.NumberPictureBox = New System.Windows.Forms.PictureBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.VariablePictureBox = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CommentPictureBox = New System.Windows.Forms.PictureBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.StringPictureBox = New System.Windows.Forms.PictureBox()
        Me.ConfigTabs = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.ChkBxAutoComplete = New System.Windows.Forms.CheckBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage5.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.MS_IDPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MS_NumberPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MS_VariablePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MS_CommentPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MS_StringPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox8.SuspendLayout()
        CType(Me.StringVariableClrBx, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IDPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumberPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VariablePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CommentPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StringPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ConfigTabs.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'BTN_Ok
        '
        Me.BTN_Ok.Location = New System.Drawing.Point(201, 274)
        Me.BTN_Ok.Name = "BTN_Ok"
        Me.BTN_Ok.Size = New System.Drawing.Size(75, 23)
        Me.BTN_Ok.TabIndex = 10
        Me.BTN_Ok.Text = "Ok"
        Me.BTN_Ok.UseVisualStyleBackColor = True
        '
        'BTN_Cancel
        '
        Me.BTN_Cancel.Location = New System.Drawing.Point(287, 274)
        Me.BTN_Cancel.Name = "BTN_Cancel"
        Me.BTN_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.BTN_Cancel.TabIndex = 9
        Me.BTN_Cancel.Text = "Cancel"
        Me.BTN_Cancel.UseVisualStyleBackColor = True
        '
        'IniBrowseDialog
        '
        Me.IniBrowseDialog.DefaultExt = "ini"
        Me.IniBrowseDialog.Filter = "Furcadia Character Files)|*.ini"
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.GroupBox3)
        Me.TabPage5.Controls.Add(Me.GroupBox8)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(371, 230)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Highlighter Settings"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.MS_IDPictureBox)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.MS_NumberPictureBox)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.MS_VariablePictureBox)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.MS_CommentPictureBox)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.MS_StringPictureBox)
        Me.GroupBox3.Location = New System.Drawing.Point(199, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(166, 160)
        Me.GroupBox3.TabIndex = 45
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Monkey Speak"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(58, 110)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(68, 13)
        Me.Label6.TabIndex = 42
        Me.Label6.Text = "Line ID Color"
        '
        'MS_IDPictureBox
        '
        Me.MS_IDPictureBox.BackColor = System.Drawing.Color.Lime
        Me.MS_IDPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.MS_IDPictureBox.Location = New System.Drawing.Point(132, 110)
        Me.MS_IDPictureBox.Name = "MS_IDPictureBox"
        Me.MS_IDPictureBox.Size = New System.Drawing.Size(15, 14)
        Me.MS_IDPictureBox.TabIndex = 41
        Me.MS_IDPictureBox.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(53, 70)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(71, 13)
        Me.Label7.TabIndex = 40
        Me.Label7.Text = "Number Color"
        '
        'MS_NumberPictureBox
        '
        Me.MS_NumberPictureBox.BackColor = System.Drawing.Color.Lime
        Me.MS_NumberPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.MS_NumberPictureBox.Location = New System.Drawing.Point(132, 70)
        Me.MS_NumberPictureBox.Name = "MS_NumberPictureBox"
        Me.MS_NumberPictureBox.Size = New System.Drawing.Size(15, 14)
        Me.MS_NumberPictureBox.TabIndex = 39
        Me.MS_NumberPictureBox.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(43, 90)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(83, 13)
        Me.Label8.TabIndex = 38
        Me.Label8.Text = "% Variable Color"
        '
        'MS_VariablePictureBox
        '
        Me.MS_VariablePictureBox.BackColor = System.Drawing.Color.Lime
        Me.MS_VariablePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.MS_VariablePictureBox.Location = New System.Drawing.Point(132, 90)
        Me.MS_VariablePictureBox.Name = "MS_VariablePictureBox"
        Me.MS_VariablePictureBox.Size = New System.Drawing.Size(15, 14)
        Me.MS_VariablePictureBox.TabIndex = 37
        Me.MS_VariablePictureBox.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(48, 31)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(78, 13)
        Me.Label9.TabIndex = 36
        Me.Label9.Text = "Comment Color"
        '
        'MS_CommentPictureBox
        '
        Me.MS_CommentPictureBox.BackColor = System.Drawing.Color.Lime
        Me.MS_CommentPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.MS_CommentPictureBox.Location = New System.Drawing.Point(132, 30)
        Me.MS_CommentPictureBox.Name = "MS_CommentPictureBox"
        Me.MS_CommentPictureBox.Size = New System.Drawing.Size(15, 14)
        Me.MS_CommentPictureBox.TabIndex = 35
        Me.MS_CommentPictureBox.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(64, 50)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(61, 13)
        Me.Label10.TabIndex = 34
        Me.Label10.Text = "String Color"
        '
        'MS_StringPictureBox
        '
        Me.MS_StringPictureBox.BackColor = System.Drawing.Color.Lime
        Me.MS_StringPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.MS_StringPictureBox.Location = New System.Drawing.Point(132, 50)
        Me.MS_StringPictureBox.Name = "MS_StringPictureBox"
        Me.MS_StringPictureBox.Size = New System.Drawing.Size(15, 14)
        Me.MS_StringPictureBox.TabIndex = 33
        Me.MS_StringPictureBox.TabStop = False
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.Label1)
        Me.GroupBox8.Controls.Add(Me.StringVariableClrBx)
        Me.GroupBox8.Controls.Add(Me.Label20)
        Me.GroupBox8.Controls.Add(Me.IDPictureBox)
        Me.GroupBox8.Controls.Add(Me.Label15)
        Me.GroupBox8.Controls.Add(Me.NumberPictureBox)
        Me.GroupBox8.Controls.Add(Me.Label19)
        Me.GroupBox8.Controls.Add(Me.VariablePictureBox)
        Me.GroupBox8.Controls.Add(Me.Label3)
        Me.GroupBox8.Controls.Add(Me.CommentPictureBox)
        Me.GroupBox8.Controls.Add(Me.Label4)
        Me.GroupBox8.Controls.Add(Me.StringPictureBox)
        Me.GroupBox8.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(166, 160)
        Me.GroupBox8.TabIndex = 1
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Drqagon Speak"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 129)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 13)
        Me.Label1.TabIndex = 44
        Me.Label1.Text = "~String Variable Color"
        '
        'StringVariableClrBx
        '
        Me.StringVariableClrBx.BackColor = System.Drawing.Color.Lime
        Me.StringVariableClrBx.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.StringVariableClrBx.Location = New System.Drawing.Point(132, 129)
        Me.StringVariableClrBx.Name = "StringVariableClrBx"
        Me.StringVariableClrBx.Size = New System.Drawing.Size(15, 14)
        Me.StringVariableClrBx.TabIndex = 43
        Me.StringVariableClrBx.TabStop = False
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(58, 110)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(68, 13)
        Me.Label20.TabIndex = 42
        Me.Label20.Text = "Line ID Color"
        '
        'IDPictureBox
        '
        Me.IDPictureBox.BackColor = System.Drawing.Color.Lime
        Me.IDPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.IDPictureBox.Location = New System.Drawing.Point(132, 110)
        Me.IDPictureBox.Name = "IDPictureBox"
        Me.IDPictureBox.Size = New System.Drawing.Size(15, 14)
        Me.IDPictureBox.TabIndex = 41
        Me.IDPictureBox.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(53, 70)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(71, 13)
        Me.Label15.TabIndex = 40
        Me.Label15.Text = "Number Color"
        '
        'NumberPictureBox
        '
        Me.NumberPictureBox.BackColor = System.Drawing.Color.Lime
        Me.NumberPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.NumberPictureBox.Location = New System.Drawing.Point(132, 70)
        Me.NumberPictureBox.Name = "NumberPictureBox"
        Me.NumberPictureBox.Size = New System.Drawing.Size(15, 14)
        Me.NumberPictureBox.TabIndex = 39
        Me.NumberPictureBox.TabStop = False
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(43, 90)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(83, 13)
        Me.Label19.TabIndex = 38
        Me.Label19.Text = "% Variable Color"
        '
        'VariablePictureBox
        '
        Me.VariablePictureBox.BackColor = System.Drawing.Color.Lime
        Me.VariablePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.VariablePictureBox.Location = New System.Drawing.Point(132, 90)
        Me.VariablePictureBox.Name = "VariablePictureBox"
        Me.VariablePictureBox.Size = New System.Drawing.Size(15, 14)
        Me.VariablePictureBox.TabIndex = 37
        Me.VariablePictureBox.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(48, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 13)
        Me.Label3.TabIndex = 36
        Me.Label3.Text = "Comment Color"
        '
        'CommentPictureBox
        '
        Me.CommentPictureBox.BackColor = System.Drawing.Color.Lime
        Me.CommentPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.CommentPictureBox.Location = New System.Drawing.Point(132, 30)
        Me.CommentPictureBox.Name = "CommentPictureBox"
        Me.CommentPictureBox.Size = New System.Drawing.Size(15, 14)
        Me.CommentPictureBox.TabIndex = 35
        Me.CommentPictureBox.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(64, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 34
        Me.Label4.Text = "String Color"
        '
        'StringPictureBox
        '
        Me.StringPictureBox.BackColor = System.Drawing.Color.Lime
        Me.StringPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.StringPictureBox.Location = New System.Drawing.Point(132, 50)
        Me.StringPictureBox.Name = "StringPictureBox"
        Me.StringPictureBox.Size = New System.Drawing.Size(15, 14)
        Me.StringPictureBox.TabIndex = 33
        Me.StringPictureBox.TabStop = False
        '
        'ConfigTabs
        '
        Me.ConfigTabs.Controls.Add(Me.TabPage5)
        Me.ConfigTabs.Controls.Add(Me.TabPage1)
        Me.ConfigTabs.Controls.Add(Me.TabPage2)
        Me.ConfigTabs.Controls.Add(Me.TabPage3)
        Me.ConfigTabs.Location = New System.Drawing.Point(12, 12)
        Me.ConfigTabs.Name = "ConfigTabs"
        Me.ConfigTabs.SelectedIndex = 0
        Me.ConfigTabs.Size = New System.Drawing.Size(379, 256)
        Me.ConfigTabs.TabIndex = 11
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.Button1)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.NumericUpDown1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(371, 230)
        Me.TabPage1.TabIndex = 5
        Me.TabPage1.Text = "Line Indents"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ListBox2)
        Me.GroupBox2.Location = New System.Drawing.Point(192, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(173, 182)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Monkey Speak"
        '
        'ListBox2
        '
        Me.ListBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(3, 16)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(167, 163)
        Me.ListBox2.TabIndex = 7
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ListBox1)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(169, 182)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Dragon Speak"
        '
        'ListBox1
        '
        Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(3, 16)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(163, 163)
        Me.ListBox1.TabIndex = 7
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(240, 199)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(106, 25)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Reset to Defaults"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(75, 188)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(111, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Spaces of Indentation"
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(78, 204)
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(120, 20)
        Me.NumericUpDown1.TabIndex = 2
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.ChkBxAutoComplete)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(371, 230)
        Me.TabPage2.TabIndex = 6
        Me.TabPage2.Text = "General Settings"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'ChkBxAutoComplete
        '
        Me.ChkBxAutoComplete.AutoSize = True
        Me.ChkBxAutoComplete.Checked = True
        Me.ChkBxAutoComplete.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkBxAutoComplete.Location = New System.Drawing.Point(120, 107)
        Me.ChkBxAutoComplete.Name = "ChkBxAutoComplete"
        Me.ChkBxAutoComplete.Size = New System.Drawing.Size(131, 17)
        Me.ChkBxAutoComplete.TabIndex = 3
        Me.ChkBxAutoComplete.Text = "Enable Auto Complete"
        Me.ChkBxAutoComplete.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.ListView1)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(371, 230)
        Me.TabPage3.TabIndex = 7
        Me.TabPage3.Text = "Plugins"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.CheckBoxes = True
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(3, 3)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(365, 224)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Enabled"
        Me.ColumnHeader1.Width = 79
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Name"
        Me.ColumnHeader2.Width = 300
        '
        'Config
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(405, 309)
        Me.Controls.Add(Me.ConfigTabs)
        Me.Controls.Add(Me.BTN_Ok)
        Me.Controls.Add(Me.BTN_Cancel)
        Me.DataBindings.Add(New System.Windows.Forms.Binding("Location", Global.MonkeySpeakEditor.My.MySettings.Default, "ConfigFormLocation", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = Global.MonkeySpeakEditor.My.MySettings.Default.ConfigFormLocation
        Me.Name = "Config"
        Me.Text = "Configuration"
        Me.TabPage5.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.MS_IDPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MS_NumberPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MS_VariablePictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MS_CommentPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MS_StringPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        CType(Me.StringVariableClrBx, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IDPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumberPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VariablePictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CommentPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StringPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ConfigTabs.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BTN_Ok As System.Windows.Forms.Button
    Friend WithEvents BTN_Cancel As System.Windows.Forms.Button
    Friend WithEvents IniBrowseDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents ConfigTabs As System.Windows.Forms.TabControl
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents IDPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents NumberPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents VariablePictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CommentPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents StringPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents StringVariableClrBx As System.Windows.Forms.PictureBox
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents MS_IDPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents MS_NumberPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents MS_VariablePictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents MS_CommentPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents MS_StringPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents ChkBxAutoComplete As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
End Class

