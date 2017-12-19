<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BotSetup
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BotSetup))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.IniBrowseDialog = New System.Windows.Forms.OpenFileDialog()
        Me.MS_BrosweDialog = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.BtnMS_File = New System.Windows.Forms.Button()
        Me.TxtBxMS_File = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtBxBotConroller = New System.Windows.Forms.TextBox()
        Me.BTN_Browse = New System.Windows.Forms.Button()
        Me.TxtBxBotIni = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.TxtBx_CharIni = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.NumericTimeOut = New System.Windows.Forms.NumericUpDown()
        Me.NumericRetryAttempts = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.StandAloneChkBx = New System.Windows.Forms.CheckBox()
        Me.ChkBxAutoConnect = New System.Windows.Forms.CheckBox()
        Me.MSEnableChkBx = New System.Windows.Forms.CheckBox()
        Me.TxtHPort = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TxtBxDreamURL = New System.Windows.Forms.TextBox()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.ChkBxTimeStampLog = New System.Windows.Forms.CheckBox()
        Me.RadioNewFile = New System.Windows.Forms.RadioButton()
        Me.RadioOverwriteLog = New System.Windows.Forms.RadioButton()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.TxtBxLogName = New System.Windows.Forms.TextBox()
        Me.ChckSaveToLog = New System.Windows.Forms.CheckBox()
        Me.TxtBxLogPath = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.IniBrowse = New System.Windows.Forms.OpenFileDialog()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.NumericTimeOut, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericRetryAttempts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(740, 568)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(389, 69)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(8, 7)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(178, 55)
        Me.OK_Button.TabIndex = 14
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(202, 7)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(179, 55)
        Me.Cancel_Button.TabIndex = 13
        Me.Cancel_Button.Text = "Cancel"
        '
        'IniBrowseDialog
        '
        Me.IniBrowseDialog.DefaultExt = "ini"
        Me.IniBrowseDialog.Filter = "Furcadia Character Files)|*.ini"
        Me.IniBrowseDialog.RestoreDirectory = True
        '
        'MS_BrosweDialog
        '
        Me.MS_BrosweDialog.DefaultExt = "ms"
        Me.MS_BrosweDialog.Filter = "MonkeySpeak Files)|*.ms"
        Me.MS_BrosweDialog.InitialDirectory = "mPath()"
        Me.MS_BrosweDialog.RestoreDirectory = True
        '
        'SaveFileDialog
        '
        Me.SaveFileDialog.DefaultExt = "bini"
        Me.SaveFileDialog.Filter = "Bot Files|*.bini"
        Me.SaveFileDialog.RestoreDirectory = True
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Location = New System.Drawing.Point(16, 7)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1118, 532)
        Me.TabControl1.TabIndex = 11
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.LinkLabel1)
        Me.TabPage1.Controls.Add(Me.BtnMS_File)
        Me.TabPage1.Controls.Add(Me.TxtBxMS_File)
        Me.TabPage1.Controls.Add(Me.Label15)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.TxtBxBotConroller)
        Me.TabPage1.Controls.Add(Me.BTN_Browse)
        Me.TabPage1.Controls.Add(Me.TxtBxBotIni)
        Me.TabPage1.Controls.Add(Me.Label20)
        Me.TabPage1.Controls.Add(Me.TxtBx_CharIni)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Location = New System.Drawing.Point(10, 48)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage1.Size = New System.Drawing.Size(1098, 474)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Main"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(992, 91)
        Me.LinkLabel1.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(90, 32)
        Me.LinkLabel1.TabIndex = 79
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Help?"
        '
        'BtnMS_File
        '
        Me.BtnMS_File.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnMS_File.Location = New System.Drawing.Point(896, 217)
        Me.BtnMS_File.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BtnMS_File.Name = "BtnMS_File"
        Me.BtnMS_File.Size = New System.Drawing.Size(64, 45)
        Me.BtnMS_File.TabIndex = 71
        Me.BtnMS_File.Text = "..."
        Me.BtnMS_File.UseVisualStyleBackColor = True
        '
        'TxtBxMS_File
        '
        Me.TxtBxMS_File.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtBxMS_File.Location = New System.Drawing.Point(320, 217)
        Me.TxtBxMS_File.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxtBxMS_File.Name = "TxtBxMS_File"
        Me.TxtBxMS_File.Size = New System.Drawing.Size(540, 38)
        Me.TxtBxMS_File.TabIndex = 70
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(43, 224)
        Me.Label15.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(256, 32)
        Me.Label15.TabIndex = 78
        Me.Label15.Text = "MonkeySpeak File "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(43, 296)
        Me.Label1.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(189, 32)
        Me.Label1.TabIndex = 77
        Me.Label1.Text = "Bot Controller"
        '
        'TxtBxBotConroller
        '
        Me.TxtBxBotConroller.Location = New System.Drawing.Point(331, 289)
        Me.TxtBxBotConroller.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxtBxBotConroller.Name = "TxtBxBotConroller"
        Me.TxtBxBotConroller.Size = New System.Drawing.Size(391, 38)
        Me.TxtBxBotConroller.TabIndex = 73
        '
        'BTN_Browse
        '
        Me.BTN_Browse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BTN_Browse.Location = New System.Drawing.Point(896, 88)
        Me.BTN_Browse.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BTN_Browse.Name = "BTN_Browse"
        Me.BTN_Browse.Size = New System.Drawing.Size(64, 45)
        Me.BTN_Browse.TabIndex = 68
        Me.BTN_Browse.Text = "..."
        Me.BTN_Browse.UseVisualStyleBackColor = True
        '
        'TxtBxBotIni
        '
        Me.TxtBxBotIni.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtBxBotIni.Location = New System.Drawing.Point(237, 155)
        Me.TxtBxBotIni.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxtBxBotIni.Name = "TxtBxBotIni"
        Me.TxtBxBotIni.Size = New System.Drawing.Size(623, 38)
        Me.TxtBxBotIni.TabIndex = 69
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(43, 162)
        Me.Label20.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(192, 32)
        Me.Label20.TabIndex = 76
        Me.Label20.Text = "Bot File *.bini:"
        '
        'TxtBx_CharIni
        '
        Me.TxtBx_CharIni.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtBx_CharIni.Location = New System.Drawing.Point(245, 88)
        Me.TxtBx_CharIni.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxtBx_CharIni.Name = "TxtBx_CharIni"
        Me.TxtBx_CharIni.Size = New System.Drawing.Size(615, 38)
        Me.TxtBx_CharIni.TabIndex = 67
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(43, 95)
        Me.Label3.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(184, 32)
        Me.Label3.TabIndex = 66
        Me.Label3.Text = "Character Ini:"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox2)
        Me.TabPage2.Controls.Add(Me.GroupBox1)
        Me.TabPage2.Location = New System.Drawing.Point(10, 48)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage2.Size = New System.Drawing.Size(1098, 474)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Connect Settings"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.NumericTimeOut)
        Me.GroupBox2.Controls.Add(Me.NumericRetryAttempts)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.StandAloneChkBx)
        Me.GroupBox2.Controls.Add(Me.ChkBxAutoConnect)
        Me.GroupBox2.Controls.Add(Me.MSEnableChkBx)
        Me.GroupBox2.Controls.Add(Me.TxtHPort)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Location = New System.Drawing.Point(19, 0)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.GroupBox2.Size = New System.Drawing.Size(1041, 236)
        Me.GroupBox2.TabIndex = 87
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Connection"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(632, 183)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(249, 32)
        Me.Label5.TabIndex = 94
        Me.Label5.Text = "Time Out Seconds"
        '
        'NumericTimeOut
        '
        Me.NumericTimeOut.Location = New System.Drawing.Point(897, 181)
        Me.NumericTimeOut.Name = "NumericTimeOut"
        Me.NumericTimeOut.Size = New System.Drawing.Size(120, 38)
        Me.NumericTimeOut.TabIndex = 93
        '
        'NumericRetryAttempts
        '
        Me.NumericRetryAttempts.Location = New System.Drawing.Point(897, 122)
        Me.NumericRetryAttempts.Name = "NumericRetryAttempts"
        Me.NumericRetryAttempts.Size = New System.Drawing.Size(120, 38)
        Me.NumericRetryAttempts.TabIndex = 91
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(660, 124)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(201, 32)
        Me.Label2.TabIndex = 92
        Me.Label2.Text = "Retry Attempts"
        '
        'StandAloneChkBx
        '
        Me.StandAloneChkBx.AutoSize = True
        Me.StandAloneChkBx.Location = New System.Drawing.Point(379, 126)
        Me.StandAloneChkBx.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.StandAloneChkBx.Name = "StandAloneChkBx"
        Me.StandAloneChkBx.Size = New System.Drawing.Size(209, 36)
        Me.StandAloneChkBx.TabIndex = 90
        Me.StandAloneChkBx.Text = "Stand Alone"
        Me.StandAloneChkBx.UseVisualStyleBackColor = True
        '
        'ChkBxAutoConnect
        '
        Me.ChkBxAutoConnect.AutoSize = True
        Me.ChkBxAutoConnect.Location = New System.Drawing.Point(59, 181)
        Me.ChkBxAutoConnect.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.ChkBxAutoConnect.Name = "ChkBxAutoConnect"
        Me.ChkBxAutoConnect.Size = New System.Drawing.Size(337, 36)
        Me.ChkBxAutoConnect.TabIndex = 89
        Me.ChkBxAutoConnect.Text = "Autoconnect at startup"
        Me.ChkBxAutoConnect.UseVisualStyleBackColor = True
        '
        'MSEnableChkBx
        '
        Me.MSEnableChkBx.AutoSize = True
        Me.MSEnableChkBx.Location = New System.Drawing.Point(59, 126)
        Me.MSEnableChkBx.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.MSEnableChkBx.Name = "MSEnableChkBx"
        Me.MSEnableChkBx.Size = New System.Drawing.Size(289, 36)
        Me.MSEnableChkBx.TabIndex = 88
        Me.MSEnableChkBx.Text = "MS Engine Enable"
        Me.MSEnableChkBx.UseVisualStyleBackColor = True
        '
        'TxtHPort
        '
        Me.TxtHPort.Location = New System.Drawing.Point(235, 64)
        Me.TxtHPort.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxtHPort.MaxLength = 5
        Me.TxtHPort.Name = "TxtHPort"
        Me.TxtHPort.Size = New System.Drawing.Size(97, 38)
        Me.TxtHPort.TabIndex = 87
        Me.TxtHPort.Text = "6500"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(51, 72)
        Me.Label4.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(151, 32)
        Me.Label4.TabIndex = 86
        Me.Label4.Text = "Local Port:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.TxtBxDreamURL)
        Me.GroupBox1.Controls.Add(Me.RadioButton4)
        Me.GroupBox1.Controls.Add(Me.RadioButton3)
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Location = New System.Drawing.Point(19, 250)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.GroupBox1.Size = New System.Drawing.Size(1033, 205)
        Me.GroupBox1.TabIndex = 86
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Go Map"
        '
        'TxtBxDreamURL
        '
        Me.TxtBxDreamURL.Enabled = False
        Me.TxtBxDreamURL.Location = New System.Drawing.Point(291, 100)
        Me.TxtBxDreamURL.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxtBxDreamURL.Name = "TxtBxDreamURL"
        Me.TxtBxDreamURL.Size = New System.Drawing.Size(455, 38)
        Me.TxtBxDreamURL.TabIndex = 91
        Me.TxtBxDreamURL.Text = "furc://"
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(27, 103)
        Me.RadioButton4.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(192, 36)
        Me.RadioButton4.TabIndex = 90
        Me.RadioButton4.Text = "DreamURL"
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Checked = True
        Me.RadioButton3.Location = New System.Drawing.Point(523, 45)
        Me.RadioButton3.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(120, 36)
        Me.RadioButton3.TabIndex = 89
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "None"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(291, 45)
        Me.RadioButton2.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(170, 36)
        Me.RadioButton2.TabIndex = 88
        Me.RadioButton2.Text = "Acropolis"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(27, 45)
        Me.RadioButton1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(232, 36)
        Me.RadioButton1.TabIndex = 87
        Me.RadioButton1.Text = "Allgeria Island"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.GroupBox6)
        Me.TabPage4.Controls.Add(Me.GroupBox4)
        Me.TabPage4.Location = New System.Drawing.Point(10, 48)
        Me.TabPage4.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage4.Size = New System.Drawing.Size(1098, 474)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Log Options"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.ChkBxTimeStampLog)
        Me.GroupBox6.Controls.Add(Me.RadioNewFile)
        Me.GroupBox6.Controls.Add(Me.RadioOverwriteLog)
        Me.GroupBox6.Location = New System.Drawing.Point(227, 248)
        Me.GroupBox6.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Padding = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.GroupBox6.Size = New System.Drawing.Size(453, 207)
        Me.GroupBox6.TabIndex = 6
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Log Options"
        '
        'ChkBxTimeStampLog
        '
        Me.ChkBxTimeStampLog.AutoSize = True
        Me.ChkBxTimeStampLog.Location = New System.Drawing.Point(88, 155)
        Me.ChkBxTimeStampLog.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.ChkBxTimeStampLog.Name = "ChkBxTimeStampLog"
        Me.ChkBxTimeStampLog.Size = New System.Drawing.Size(287, 36)
        Me.ChkBxTimeStampLog.TabIndex = 2
        Me.ChkBxTimeStampLog.Text = "Time Stamp Name"
        Me.ChkBxTimeStampLog.UseVisualStyleBackColor = True
        '
        'RadioNewFile
        '
        Me.RadioNewFile.AutoSize = True
        Me.RadioNewFile.Location = New System.Drawing.Point(16, 100)
        Me.RadioNewFile.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.RadioNewFile.Name = "RadioNewFile"
        Me.RadioNewFile.Size = New System.Drawing.Size(162, 36)
        Me.RadioNewFile.TabIndex = 1
        Me.RadioNewFile.Text = "New File"
        Me.RadioNewFile.UseVisualStyleBackColor = True
        '
        'RadioOverwriteLog
        '
        Me.RadioOverwriteLog.AutoSize = True
        Me.RadioOverwriteLog.Checked = True
        Me.RadioOverwriteLog.Location = New System.Drawing.Point(16, 45)
        Me.RadioOverwriteLog.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.RadioOverwriteLog.Name = "RadioOverwriteLog"
        Me.RadioOverwriteLog.Size = New System.Drawing.Size(151, 36)
        Me.RadioOverwriteLog.TabIndex = 0
        Me.RadioOverwriteLog.TabStop = True
        Me.RadioOverwriteLog.Text = "Append"
        Me.RadioOverwriteLog.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label14)
        Me.GroupBox4.Controls.Add(Me.TxtBxLogName)
        Me.GroupBox4.Controls.Add(Me.ChckSaveToLog)
        Me.GroupBox4.Controls.Add(Me.TxtBxLogPath)
        Me.GroupBox4.Controls.Add(Me.Button1)
        Me.GroupBox4.Controls.Add(Me.Label12)
        Me.GroupBox4.Location = New System.Drawing.Point(8, 0)
        Me.GroupBox4.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Padding = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.GroupBox4.Size = New System.Drawing.Size(957, 241)
        Me.GroupBox4.TabIndex = 5
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Log File"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(16, 129)
        Me.Label14.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(144, 32)
        Me.Label14.TabIndex = 5
        Me.Label14.Text = "File Name"
        '
        'TxtBxLogName
        '
        Me.TxtBxLogName.Location = New System.Drawing.Point(176, 112)
        Me.TxtBxLogName.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxtBxLogName.Name = "TxtBxLogName"
        Me.TxtBxLogName.Size = New System.Drawing.Size(532, 38)
        Me.TxtBxLogName.TabIndex = 6
        '
        'ChckSaveToLog
        '
        Me.ChckSaveToLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ChckSaveToLog.AutoSize = True
        Me.ChckSaveToLog.Location = New System.Drawing.Point(683, 191)
        Me.ChckSaveToLog.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.ChckSaveToLog.Name = "ChckSaveToLog"
        Me.ChckSaveToLog.Size = New System.Drawing.Size(258, 36)
        Me.ChckSaveToLog.TabIndex = 59
        Me.ChckSaveToLog.Text = "Save to Log File"
        Me.ChckSaveToLog.UseVisualStyleBackColor = True
        '
        'TxtBxLogPath
        '
        Me.TxtBxLogPath.Location = New System.Drawing.Point(160, 50)
        Me.TxtBxLogPath.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxtBxLogPath.Name = "TxtBxLogPath"
        Me.TxtBxLogPath.Size = New System.Drawing.Size(559, 38)
        Me.TxtBxLogPath.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(741, 45)
        Me.Button1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(200, 55)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Browse"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(16, 69)
        Me.Label12.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(128, 32)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "File Path"
        '
        'IniBrowse
        '
        Me.IniBrowse.DefaultExt = "Log"
        Me.IniBrowse.Filter = "Log Files | *.log"
        Me.IniBrowse.RestoreDirectory = True
        '
        'ToolTip1
        '
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning
        Me.ToolTip1.ToolTipTitle = "Legacy Feature"
        '
        'BotSetup
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(16.0!, 31.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(1161, 665)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "BotSetup"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "BotSetup"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.NumericTimeOut, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericRetryAttempts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents IniBrowseDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents MS_BrosweDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents BtnMS_File As System.Windows.Forms.Button
    Friend WithEvents TxtBxMS_File As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtBxBotConroller As System.Windows.Forms.TextBox
    Friend WithEvents BTN_Browse As System.Windows.Forms.Button
    Friend WithEvents TxtBxBotIni As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents TxtBx_CharIni As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents StandAloneChkBx As System.Windows.Forms.CheckBox
    Friend WithEvents ChkBxAutoConnect As System.Windows.Forms.CheckBox
    Friend WithEvents MSEnableChkBx As System.Windows.Forms.CheckBox
    Friend WithEvents TxtHPort As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TxtBxDreamURL As System.Windows.Forms.TextBox
    Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents ChkBxTimeStampLog As System.Windows.Forms.CheckBox
    Friend WithEvents RadioNewFile As System.Windows.Forms.RadioButton
    Friend WithEvents RadioOverwriteLog As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TxtBxLogName As System.Windows.Forms.TextBox
    Friend WithEvents ChckSaveToLog As System.Windows.Forms.CheckBox
    Friend WithEvents TxtBxLogPath As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents IniBrowse As System.Windows.Forms.OpenFileDialog
    Friend WithEvents LinkLabel1 As Windows.Forms.LinkLabel
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents NumericTimeOut As Windows.Forms.NumericUpDown
    Friend WithEvents NumericRetryAttempts As Windows.Forms.NumericUpDown
End Class
