<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.ContextTryIcon = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RestoreMainToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditorTrayIconMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ConnectTrayIconMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisconnectTrayIconMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitTrayIconMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ActionTmr = New System.Windows.Forms.Timer(Me.components)
        Me.TS_Main = New System.Windows.Forms.StatusStrip()
        Me.TS_Status_BotName = New System.Windows.Forms.ToolStripStatusLabel()
        Me.FurcTimeLbl = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TS_Filler = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripServerStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripClientStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.DreamCountTxtBx = New MonkeyCore.Controls.TextBox_NoFlicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DreamList = New MonkeyCore.Controls.ListBox_NoFlicker()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.toServer = New MonkeyCore.Controls.RichTextBoxEx()
        Me.EditMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.Btn_Bold = New System.Windows.Forms.Button()
        Me.BTN_Underline = New System.Windows.Forms.Button()
        Me.BTN_Italic = New System.Windows.Forms.Button()
        Me.sendToServer = New System.Windows.Forms.Button()
        Me.clientGroup = New System.Windows.Forms.GroupBox()
        Me.log_ = New MonkeyCore.Controls.RichTextBoxEx()
        Me.CopyMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuCopy2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCut2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.GrpAction = New System.Windows.Forms.GroupBox()
        Me.BtnSit_stand_Lie = New System.Windows.Forms.Button()
        Me.BTN_TurnL = New System.Windows.Forms.Button()
        Me.BTN_TurnR = New System.Windows.Forms.Button()
        Me._ne = New System.Windows.Forms.PictureBox()
        Me._nw = New System.Windows.Forms.PictureBox()
        Me.use_ = New System.Windows.Forms.Button()
        Me.get_ = New System.Windows.Forms.Button()
        Me.se_ = New System.Windows.Forms.PictureBox()
        Me.sw_ = New System.Windows.Forms.PictureBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewBotToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditBotToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RecentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportMonkeySpeakToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WindowsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DebugToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MSEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.LinksToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSTutorialsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTN_Go = New System.Windows.Forms.Button()
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.BotIniOpen = New System.Windows.Forms.OpenFileDialog()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.FURREListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ContextTryIcon.SuspendLayout()
        Me.TS_Main.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.EditMenu.SuspendLayout()
        Me.clientGroup.SuspendLayout()
        Me.CopyMenu.SuspendLayout()
        Me.GrpAction.SuspendLayout()
        CType(Me._ne, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._nw, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sw_, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.FURREListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContextTryIcon
        '
        Me.ContextTryIcon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RestoreMainToolStripMenuItem, Me.EditorTrayIconMenuItem, Me.ToolStripSeparator1, Me.ConnectTrayIconMenuItem, Me.DisconnectTrayIconMenuItem, Me.ToolStripSeparator2, Me.ExitTrayIconMenuItem})
        Me.ContextTryIcon.Name = "ContextMenuStrip1"
        Me.ContextTryIcon.Size = New System.Drawing.Size(149, 126)
        '
        'RestoreMainToolStripMenuItem
        '
        Me.RestoreMainToolStripMenuItem.Name = "RestoreMainToolStripMenuItem"
        Me.RestoreMainToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.RestoreMainToolStripMenuItem.Text = "Main Window"
        '
        'EditorTrayIconMenuItem
        '
        Me.EditorTrayIconMenuItem.Name = "EditorTrayIconMenuItem"
        Me.EditorTrayIconMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.EditorTrayIconMenuItem.Text = "Editor"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(145, 6)
        '
        'ConnectTrayIconMenuItem
        '
        Me.ConnectTrayIconMenuItem.Name = "ConnectTrayIconMenuItem"
        Me.ConnectTrayIconMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ConnectTrayIconMenuItem.Text = "Connect"
        '
        'DisconnectTrayIconMenuItem
        '
        Me.DisconnectTrayIconMenuItem.Enabled = False
        Me.DisconnectTrayIconMenuItem.Name = "DisconnectTrayIconMenuItem"
        Me.DisconnectTrayIconMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.DisconnectTrayIconMenuItem.Text = "Disconnect"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(145, 6)
        '
        'ExitTrayIconMenuItem
        '
        Me.ExitTrayIconMenuItem.Name = "ExitTrayIconMenuItem"
        Me.ExitTrayIconMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ExitTrayIconMenuItem.Text = "Exit"
        '
        'ActionTmr
        '
        Me.ActionTmr.Interval = 250
        '
        'TS_Main
        '
        Me.TS_Main.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TS_Status_BotName, Me.FurcTimeLbl, Me.TS_Filler, Me.ToolStripServerStatus, Me.ToolStripClientStatus})
        Me.TS_Main.Location = New System.Drawing.Point(0, 351)
        Me.TS_Main.Name = "TS_Main"
        Me.TS_Main.Size = New System.Drawing.Size(437, 22)
        Me.TS_Main.TabIndex = 50
        Me.TS_Main.Text = "StatusStrip1"
        '
        'TS_Status_BotName
        '
        Me.TS_Status_BotName.AutoSize = False
        Me.TS_Status_BotName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.TS_Status_BotName.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TS_Status_BotName.Name = "TS_Status_BotName"
        Me.TS_Status_BotName.Size = New System.Drawing.Size(0, 17)
        Me.TS_Status_BotName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FurcTimeLbl
        '
        Me.FurcTimeLbl.Name = "FurcTimeLbl"
        Me.FurcTimeLbl.Size = New System.Drawing.Size(156, 17)
        Me.FurcTimeLbl.Text = "Furcadia Time: ##:##:## am"
        '
        'TS_Filler
        '
        Me.TS_Filler.Name = "TS_Filler"
        Me.TS_Filler.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always
        Me.TS_Filler.Size = New System.Drawing.Size(55, 17)
        Me.TS_Filler.Spring = True
        '
        'ToolStripServerStatus
        '
        Me.ToolStripServerStatus.Image = Global.SilverMonkey.My.Resources.Resources.DisconnectedImg
        Me.ToolStripServerStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripServerStatus.Name = "ToolStripServerStatus"
        Me.ToolStripServerStatus.Size = New System.Drawing.Size(106, 17)
        Me.ToolStripServerStatus.Text = "Server Status"
        Me.ToolStripServerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolStripServerStatus.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.ToolStripServerStatus.ToolTipText = "Disconnected"
        '
        'ToolStripClientStatus
        '
        Me.ToolStripClientStatus.AutoSize = False
        Me.ToolStripClientStatus.Image = Global.SilverMonkey.My.Resources.Resources.DisconnectedImg
        Me.ToolStripClientStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripClientStatus.Name = "ToolStripClientStatus"
        Me.ToolStripClientStatus.Size = New System.Drawing.Size(105, 17)
        Me.ToolStripClientStatus.Text = "Client Status"
        Me.ToolStripClientStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolStripClientStatus.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.ToolStripClientStatus.ToolTipText = "Disconnected"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.DreamCountTxtBx)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.DreamList)
        Me.GroupBox1.Location = New System.Drawing.Point(281, 27)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(136, 151)
        Me.GroupBox1.TabIndex = 56
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Dream List"
        '
        'DreamCountTxtBx
        '
        Me.DreamCountTxtBx.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DreamCountTxtBx.BackColor = System.Drawing.SystemColors.Info
        Me.DreamCountTxtBx.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DreamCountTxtBx.Location = New System.Drawing.Point(93, 125)
        Me.DreamCountTxtBx.Name = "DreamCountTxtBx"
        Me.DreamCountTxtBx.ReadOnly = True
        Me.DreamCountTxtBx.Size = New System.Drawing.Size(33, 23)
        Me.DreamCountTxtBx.TabIndex = 14
        Me.DreamCountTxtBx.Text = "###"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 130)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 13)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Furres in Dream:"
        '
        'DreamList
        '
        Me.DreamList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)

        Me.DreamList.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DreamList.FormattingEnabled = True
        Me.DreamList.HorizontalScrollbar = True
        Me.DreamList.ItemHeight = 16
        Me.DreamList.Location = New System.Drawing.Point(5, 16)
        Me.DreamList.Name = "DreamList"
        Me.DreamList.Size = New System.Drawing.Size(124, 100)
        Me.DreamList.Sorted = True
        Me.DreamList.TabIndex = 12
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.CheckBox1)
        Me.GroupBox3.Controls.Add(Me.toServer)
        Me.GroupBox3.Controls.Add(Me.Btn_Bold)
        Me.GroupBox3.Controls.Add(Me.BTN_Underline)
        Me.GroupBox3.Controls.Add(Me.BTN_Italic)
        Me.GroupBox3.Controls.Add(Me.sendToServer)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 184)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(263, 164)
        Me.GroupBox3.TabIndex = 55
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Chat"
        '
        'CheckBox1
        '
        Me.CheckBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(6, 141)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(77, 17)
        Me.CheckBox1.TabIndex = 59
        Me.CheckBox1.Text = "Auto Scroll"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'toServer
        '
        Me.toServer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.toServer.ContextMenuStrip = Me.EditMenu
        Me.toServer.HScrollPos = 0
        Me.toServer.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.toServer.Location = New System.Drawing.Point(3, 28)
        Me.toServer.Name = "toServer"
        Me.toServer.ShortcutsEnabled = False
        Me.toServer.Size = New System.Drawing.Size(254, 106)
        Me.toServer.TabIndex = 58
        Me.toServer.Text = ""
        Me.toServer.VerticalContentAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Top
        Me.toServer.VScrollPos = 0
        '
        'EditMenu
        '
        Me.EditMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuCopy, Me.MenuCut, Me.PasteToolStripMenuItem, Me.ToolStripSeparator3})
        Me.EditMenu.Name = "EditMenu"
        Me.EditMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.EditMenu.Size = New System.Drawing.Size(145, 76)
        '
        'MenuCopy
        '
        Me.MenuCopy.Name = "MenuCopy"
        Me.MenuCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.MenuCopy.Size = New System.Drawing.Size(144, 22)
        Me.MenuCopy.Text = "Copy"
        '
        'MenuCut
        '
        Me.MenuCut.Name = "MenuCut"
        Me.MenuCut.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
        Me.MenuCut.Size = New System.Drawing.Size(144, 22)
        Me.MenuCut.Text = "Cut"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
        Me.PasteToolStripMenuItem.Text = "Paste"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(141, 6)
        '
        'Btn_Bold
        '
        Me.Btn_Bold.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Btn_Bold.Location = New System.Drawing.Point(180, 7)
        Me.Btn_Bold.Name = "Btn_Bold"
        Me.Btn_Bold.Size = New System.Drawing.Size(18, 19)
        Me.Btn_Bold.TabIndex = 60
        Me.Btn_Bold.Text = "B"
        Me.Btn_Bold.UseVisualStyleBackColor = True
        '
        'BTN_Underline
        '
        Me.BTN_Underline.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BTN_Underline.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BTN_Underline.Location = New System.Drawing.Point(228, 7)
        Me.BTN_Underline.Name = "BTN_Underline"
        Me.BTN_Underline.Size = New System.Drawing.Size(29, 19)
        Me.BTN_Underline.TabIndex = 59
        Me.BTN_Underline.Text = "UL"
        Me.BTN_Underline.UseVisualStyleBackColor = True
        '
        'BTN_Italic
        '
        Me.BTN_Italic.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BTN_Italic.Location = New System.Drawing.Point(204, 7)
        Me.BTN_Italic.Name = "BTN_Italic"
        Me.BTN_Italic.Size = New System.Drawing.Size(18, 19)
        Me.BTN_Italic.TabIndex = 58
        Me.BTN_Italic.Text = "I"
        Me.BTN_Italic.UseVisualStyleBackColor = True
        '
        'sendToServer
        '
        Me.sendToServer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.sendToServer.Location = New System.Drawing.Point(222, 138)
        Me.sendToServer.Name = "sendToServer"
        Me.sendToServer.Size = New System.Drawing.Size(35, 20)
        Me.sendToServer.TabIndex = 4
        Me.sendToServer.Text = "->"
        Me.sendToServer.UseVisualStyleBackColor = True
        '
        'clientGroup
        '
        Me.clientGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clientGroup.Controls.Add(Me.log_)
        Me.clientGroup.Location = New System.Drawing.Point(12, 27)
        Me.clientGroup.Name = "clientGroup"
        Me.clientGroup.Size = New System.Drawing.Size(263, 151)
        Me.clientGroup.TabIndex = 51
        Me.clientGroup.TabStop = False
        Me.clientGroup.Text = "Log"
        '
        'log_
        '
        Me.log_.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.log_.BackColor = System.Drawing.SystemColors.Window
        Me.log_.ContextMenuStrip = Me.CopyMenu
        Me.log_.HScrollPos = 0
        Me.log_.Location = New System.Drawing.Point(3, 18)
        Me.log_.Name = "log_"
        Me.log_.ReadOnly = True
        Me.log_.Size = New System.Drawing.Size(254, 127)
        Me.log_.TabIndex = 49
        Me.log_.Text = ""
        Me.log_.VerticalContentAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Top
        Me.log_.VScrollPos = 0
        '
        'CopyMenu
        '
        Me.CopyMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuCopy2, Me.MenuCut2, Me.ToolStripMenuItem3, Me.ToolStripSeparator4})
        Me.CopyMenu.Name = "EditMenu"
        Me.CopyMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.CopyMenu.Size = New System.Drawing.Size(145, 76)
        '
        'MenuCopy2
        '
        Me.MenuCopy2.Name = "MenuCopy2"
        Me.MenuCopy2.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.MenuCopy2.Size = New System.Drawing.Size(144, 22)
        Me.MenuCopy2.Text = "Copy"
        '
        'MenuCut2
        '
        Me.MenuCut2.Enabled = False
        Me.MenuCut2.Name = "MenuCut2"
        Me.MenuCut2.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
        Me.MenuCut2.Size = New System.Drawing.Size(144, 22)
        Me.MenuCut2.Text = "Cut"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Enabled = False
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(144, 22)
        Me.ToolStripMenuItem3.Text = "Paste"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(141, 6)
        '
        'GrpAction
        '
        Me.GrpAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GrpAction.Controls.Add(Me.BtnSit_stand_Lie)
        Me.GrpAction.Controls.Add(Me.BTN_TurnL)
        Me.GrpAction.Controls.Add(Me.BTN_TurnR)
        Me.GrpAction.Controls.Add(Me._ne)
        Me.GrpAction.Controls.Add(Me._nw)
        Me.GrpAction.Controls.Add(Me.use_)
        Me.GrpAction.Controls.Add(Me.get_)
        Me.GrpAction.Controls.Add(Me.se_)
        Me.GrpAction.Controls.Add(Me.sw_)
        Me.GrpAction.Location = New System.Drawing.Point(281, 184)
        Me.GrpAction.Name = "GrpAction"
        Me.GrpAction.Size = New System.Drawing.Size(136, 132)
        Me.GrpAction.TabIndex = 54
        Me.GrpAction.TabStop = False
        Me.GrpAction.Text = "Actions"
        '
        'BtnSit_stand_Lie
        '
        Me.BtnSit_stand_Lie.Location = New System.Drawing.Point(9, 69)
        Me.BtnSit_stand_Lie.Name = "BtnSit_stand_Lie"
        Me.BtnSit_stand_Lie.Size = New System.Drawing.Size(33, 23)
        Me.BtnSit_stand_Lie.TabIndex = 44
        Me.BtnSit_stand_Lie.Text = "Sit"
        Me.BtnSit_stand_Lie.UseVisualStyleBackColor = True
        '
        'BTN_TurnL
        '
        Me.BTN_TurnL.Location = New System.Drawing.Point(9, 19)
        Me.BTN_TurnL.Name = "BTN_TurnL"
        Me.BTN_TurnL.Size = New System.Drawing.Size(33, 23)
        Me.BTN_TurnL.TabIndex = 43
        Me.BTN_TurnL.Text = "<="
        Me.BTN_TurnL.UseVisualStyleBackColor = True
        '
        'BTN_TurnR
        '
        Me.BTN_TurnR.Location = New System.Drawing.Point(9, 43)
        Me.BTN_TurnR.Name = "BTN_TurnR"
        Me.BTN_TurnR.Size = New System.Drawing.Size(33, 23)
        Me.BTN_TurnR.TabIndex = 42
        Me.BTN_TurnR.Text = "=>"
        Me.BTN_TurnR.UseVisualStyleBackColor = True
        '
        '_ne
        '
        Me._ne.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._ne.Cursor = System.Windows.Forms.Cursors.Hand
        Me._ne.Image = CType(resources.GetObject("_ne.Image"), System.Drawing.Image)
        Me._ne.Location = New System.Drawing.Point(90, 19)
        Me._ne.Name = "_ne"
        Me._ne.Size = New System.Drawing.Size(36, 36)
        Me._ne.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me._ne.TabIndex = 41
        Me._ne.TabStop = False
        Me._ne.WaitOnLoad = True
        '
        '_nw
        '
        Me._nw.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._nw.Cursor = System.Windows.Forms.Cursors.Hand
        Me._nw.Image = CType(resources.GetObject("_nw.Image"), System.Drawing.Image)
        Me._nw.Location = New System.Drawing.Point(54, 19)
        Me._nw.Name = "_nw"
        Me._nw.Size = New System.Drawing.Size(36, 36)
        Me._nw.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me._nw.TabIndex = 40
        Me._nw.TabStop = False
        Me._nw.WaitOnLoad = True
        '
        'use_
        '
        Me.use_.Cursor = System.Windows.Forms.Cursors.Hand
        Me.use_.FlatAppearance.BorderSize = 0
        Me.use_.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.use_.Location = New System.Drawing.Point(65, 98)
        Me.use_.Name = "use_"
        Me.use_.Size = New System.Drawing.Size(35, 23)
        Me.use_.TabIndex = 38
        Me.use_.Text = "Use"
        Me.use_.UseVisualStyleBackColor = True
        '
        'get_
        '
        Me.get_.Cursor = System.Windows.Forms.Cursors.Hand
        Me.get_.FlatAppearance.BorderSize = 0
        Me.get_.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.get_.Location = New System.Drawing.Point(27, 98)
        Me.get_.Name = "get_"
        Me.get_.Size = New System.Drawing.Size(32, 23)
        Me.get_.TabIndex = 37
        Me.get_.Text = "Get"
        Me.get_.UseVisualStyleBackColor = True
        '
        'se_
        '
        Me.se_.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.se_.Cursor = System.Windows.Forms.Cursors.Hand
        Me.se_.Image = CType(resources.GetObject("se_.Image"), System.Drawing.Image)
        Me.se_.Location = New System.Drawing.Point(90, 56)
        Me.se_.Name = "se_"
        Me.se_.Size = New System.Drawing.Size(36, 36)
        Me.se_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.se_.TabIndex = 36
        Me.se_.TabStop = False
        Me.se_.WaitOnLoad = True
        '
        'sw_
        '
        Me.sw_.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.sw_.Cursor = System.Windows.Forms.Cursors.Hand
        Me.sw_.Image = CType(resources.GetObject("sw_.Image"), System.Drawing.Image)
        Me.sw_.Location = New System.Drawing.Point(54, 56)
        Me.sw_.Name = "sw_"
        Me.sw_.Size = New System.Drawing.Size(36, 36)
        Me.sw_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.sw_.TabIndex = 35
        Me.sw_.TabStop = False
        Me.sw_.WaitOnLoad = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.WindowsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(437, 24)
        Me.MenuStrip1.TabIndex = 57
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewBotToolStripMenuItem, Me.OpenToolStripMenuItem, Me.EditBotToolStripMenuItem, Me.RecentToolStripMenuItem, Me.ToolStripSeparator5, Me.CloseToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NewBotToolStripMenuItem
        '
        Me.NewBotToolStripMenuItem.Name = "NewBotToolStripMenuItem"
        Me.NewBotToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.NewBotToolStripMenuItem.Text = "New Bot"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'EditBotToolStripMenuItem
        '
        Me.EditBotToolStripMenuItem.Name = "EditBotToolStripMenuItem"
        Me.EditBotToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.EditBotToolStripMenuItem.Text = "Edit Bot"
        '
        'RecentToolStripMenuItem
        '
        Me.RecentToolStripMenuItem.Name = "RecentToolStripMenuItem"
        Me.RecentToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.RecentToolStripMenuItem.Text = "Recent"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(116, 6)
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.CloseToolStripMenuItem.Text = "Exit"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConfigToolStripMenuItem, Me.ExportMonkeySpeakToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'ConfigToolStripMenuItem
        '
        Me.ConfigToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ConfigToolStripMenuItem.Name = "ConfigToolStripMenuItem"
        Me.ConfigToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.ConfigToolStripMenuItem.Text = "Config"
        '
        'ExportMonkeySpeakToolStripMenuItem
        '
        Me.ExportMonkeySpeakToolStripMenuItem.Name = "ExportMonkeySpeakToolStripMenuItem"
        Me.ExportMonkeySpeakToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.ExportMonkeySpeakToolStripMenuItem.Text = "Export MonkeySpeak"
        '
        'WindowsToolStripMenuItem
        '
        Me.WindowsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.WindowsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DebugToolStripMenuItem, Me.MSEditorToolStripMenuItem})
        Me.WindowsToolStripMenuItem.Name = "WindowsToolStripMenuItem"
        Me.WindowsToolStripMenuItem.Size = New System.Drawing.Size(68, 20)
        Me.WindowsToolStripMenuItem.Text = "Windows"
        '
        'DebugToolStripMenuItem
        '
        Me.DebugToolStripMenuItem.Name = "DebugToolStripMenuItem"
        Me.DebugToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.DebugToolStripMenuItem.Text = "Debug"
        '
        'MSEditorToolStripMenuItem
        '
        Me.MSEditorToolStripMenuItem.Name = "MSEditorToolStripMenuItem"
        Me.MSEditorToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.MSEditorToolStripMenuItem.Text = "MS Editor"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ContentsToolStripMenuItem, Me.AboutToolStripMenuItem1, Me.LinksToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'ContentsToolStripMenuItem
        '
        Me.ContentsToolStripMenuItem.Name = "ContentsToolStripMenuItem"
        Me.ContentsToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ContentsToolStripMenuItem.Text = "Contents"
        '
        'AboutToolStripMenuItem1
        '
        Me.AboutToolStripMenuItem1.Name = "AboutToolStripMenuItem1"
        Me.AboutToolStripMenuItem1.Size = New System.Drawing.Size(122, 22)
        Me.AboutToolStripMenuItem1.Text = "About"
        '
        'LinksToolStripMenuItem
        '
        Me.LinksToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSTutorialsToolStripMenuItem})
        Me.LinksToolStripMenuItem.Name = "LinksToolStripMenuItem"
        Me.LinksToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.LinksToolStripMenuItem.Text = "Links"
        '
        'TSTutorialsToolStripMenuItem
        '
        Me.TSTutorialsToolStripMenuItem.Name = "TSTutorialsToolStripMenuItem"
        Me.TSTutorialsToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.TSTutorialsToolStripMenuItem.Text = "T-S Tutorials"
        '
        'BTN_Go
        '
        Me.BTN_Go.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BTN_Go.Location = New System.Drawing.Point(346, 322)
        Me.BTN_Go.Name = "BTN_Go"
        Me.BTN_Go.Size = New System.Drawing.Size(71, 26)
        Me.BTN_Go.TabIndex = 52
        Me.BTN_Go.Text = "Go!"
        Me.BTN_Go.UseVisualStyleBackColor = True
        '
        'BotIniOpen
        '
        Me.BotIniOpen.DefaultExt = "bini"
        Me.BotIniOpen.Filter = "Bot Files|*.bini"
        '
        'FURREListBindingSource
        '
        Me.FURREListBindingSource.DataSource = GetType(Furcadia.Net.FURREList)
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(437, 373)
        Me.Controls.Add(Me.clientGroup)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.TS_Main)
        Me.Controls.Add(Me.GrpAction)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.BTN_Go)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(445, 400)
        Me.Name = "Main"
        Me.Text = "SilverMonkey 2.0"
        Me.ContextTryIcon.ResumeLayout(False)
        Me.TS_Main.ResumeLayout(False)
        Me.TS_Main.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.EditMenu.ResumeLayout(False)
        Me.clientGroup.ResumeLayout(False)
        Me.CopyMenu.ResumeLayout(False)
        Me.GrpAction.ResumeLayout(False)
        Me.GrpAction.PerformLayout()
        CType(Me._ne, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._nw, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sw_, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.FURREListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ActionTmr As System.Windows.Forms.Timer
    Friend WithEvents ContextTryIcon As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents TS_Main As System.Windows.Forms.StatusStrip
    Friend WithEvents TS_Status_BotName As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripServerStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripClientStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents TS_Filler As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents sendToServer As System.Windows.Forms.Button
    Friend WithEvents clientGroup As System.Windows.Forms.GroupBox
    Friend WithEvents log_ As MonkeyCore.Controls.RichTextBoxEx
    Friend WithEvents GrpAction As System.Windows.Forms.GroupBox
    Friend WithEvents BtnSit_stand_Lie As System.Windows.Forms.Button
    Friend WithEvents BTN_TurnL As System.Windows.Forms.Button
    Friend WithEvents BTN_TurnR As System.Windows.Forms.Button
    Friend WithEvents _ne As System.Windows.Forms.PictureBox
    Friend WithEvents _nw As System.Windows.Forms.PictureBox
    Friend WithEvents use_ As System.Windows.Forms.Button
    Friend WithEvents get_ As System.Windows.Forms.Button
    Friend WithEvents se_ As System.Windows.Forms.PictureBox
    Friend WithEvents sw_ As System.Windows.Forms.PictureBox
    Friend WithEvents BTN_Go As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConfigToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Btn_Bold As System.Windows.Forms.Button
    Friend WithEvents BTN_Underline As System.Windows.Forms.Button
    Friend WithEvents BTN_Italic As System.Windows.Forms.Button
    Friend WithEvents DebugToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toServer As MonkeyCore.Controls.RichTextBoxEx
    Friend WithEvents MSEditorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RestoreMainToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditorTrayIconMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ConnectTrayIconMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DisconnectTrayIconMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitTrayIconMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuCut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CopyMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuCopy2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuCut2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents NewBotToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RecentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditBotToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BotIniOpen As System.Windows.Forms.OpenFileDialog
    Friend WithEvents FurcTimeLbl As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContentsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents DreamList As MonkeyCore.Controls.ListBox_NoFlicker
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents DreamCountTxtBx As MonkeyCore.Controls.TextBox_NoFlicker
    Friend WithEvents LinksToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSTutorialsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportMonkeySpeakToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FURREListBindingSource As Windows.Forms.BindingSource
End Class
