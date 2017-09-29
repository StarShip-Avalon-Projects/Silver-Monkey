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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.clientGroup = New System.Windows.Forms.GroupBox()
        Me.Log_ = New MonkeyCore.Controls.RichTextBoxEx()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.toServer = New MonkeyCore.Controls.RichTextBoxEx()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Btn_Bold = New System.Windows.Forms.Button()
        Me.BTN_Underline = New System.Windows.Forms.Button()
        Me.BTN_Italic = New System.Windows.Forms.Button()
        Me.sendToServer = New System.Windows.Forms.Button()
        Me.BTN_Go = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TextBox_NoFlicker1 = New MonkeyCore.Controls.TextBox_NoFlicker()
        Me.DreamList = New MonkeyCore.Controls.ListBox_NoFlicker()
        Me.Label1 = New System.Windows.Forms.Label()
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
        Me.FURREListBindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.FURREListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.EditMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.CopyMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuCopy2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCut2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
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
        Me.ReferenceLinksToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MonkeySpeakLibrariesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SilverMonkeyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DefaultEngineLibrariesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EngineStartProcessToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.BotIniOpen = New System.Windows.Forms.OpenFileDialog()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.clientGroup.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GrpAction.SuspendLayout()
        CType(Me._ne, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._nw, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sw_, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextTryIcon.SuspendLayout()
        Me.TS_Main.SuspendLayout()
        CType(Me.FURREListBindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FURREListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.EditMenu.SuspendLayout()
        Me.CopyMenu.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        resources.ApplyResources(Me.SplitContainer1, "SplitContainer1")
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        resources.ApplyResources(Me.SplitContainer1.Panel1, "SplitContainer1.Panel1")
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        Me.ToolTip1.SetToolTip(Me.SplitContainer1.Panel1, resources.GetString("SplitContainer1.Panel1.ToolTip"))
        '
        'SplitContainer1.Panel2
        '
        resources.ApplyResources(Me.SplitContainer1.Panel2, "SplitContainer1.Panel2")
        Me.SplitContainer1.Panel2.Controls.Add(Me.BTN_Go)
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.GrpAction)
        Me.ToolTip1.SetToolTip(Me.SplitContainer1.Panel2, resources.GetString("SplitContainer1.Panel2.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.SplitContainer1, resources.GetString("SplitContainer1.ToolTip"))
        '
        'SplitContainer2
        '
        resources.ApplyResources(Me.SplitContainer2, "SplitContainer2")
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        resources.ApplyResources(Me.SplitContainer2.Panel1, "SplitContainer2.Panel1")
        Me.SplitContainer2.Panel1.Controls.Add(Me.clientGroup)
        Me.ToolTip1.SetToolTip(Me.SplitContainer2.Panel1, resources.GetString("SplitContainer2.Panel1.ToolTip"))
        '
        'SplitContainer2.Panel2
        '
        resources.ApplyResources(Me.SplitContainer2.Panel2, "SplitContainer2.Panel2")
        Me.SplitContainer2.Panel2.Controls.Add(Me.GroupBox3)
        Me.ToolTip1.SetToolTip(Me.SplitContainer2.Panel2, resources.GetString("SplitContainer2.Panel2.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.SplitContainer2, resources.GetString("SplitContainer2.ToolTip"))
        '
        'clientGroup
        '
        resources.ApplyResources(Me.clientGroup, "clientGroup")
        Me.clientGroup.Controls.Add(Me.Log_)
        Me.clientGroup.Name = "clientGroup"
        Me.clientGroup.TabStop = False
        Me.ToolTip1.SetToolTip(Me.clientGroup, resources.GetString("clientGroup.ToolTip"))
        '
        'Log_
        '
        resources.ApplyResources(Me.Log_, "Log_")
        Me.Log_.HScrollPos = 0
        Me.Log_.Name = "Log_"
        Me.ToolTip1.SetToolTip(Me.Log_, resources.GetString("Log_.ToolTip"))
        Me.Log_.VerticalContentAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Top
        Me.Log_.VScrollPos = 0
        '
        'GroupBox3
        '
        resources.ApplyResources(Me.GroupBox3, "GroupBox3")
        Me.GroupBox3.Controls.Add(Me.toServer)
        Me.GroupBox3.Controls.Add(Me.CheckBox1)
        Me.GroupBox3.Controls.Add(Me.Btn_Bold)
        Me.GroupBox3.Controls.Add(Me.BTN_Underline)
        Me.GroupBox3.Controls.Add(Me.BTN_Italic)
        Me.GroupBox3.Controls.Add(Me.sendToServer)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox3, resources.GetString("GroupBox3.ToolTip"))
        '
        'toServer
        '
        resources.ApplyResources(Me.toServer, "toServer")
        Me.toServer.HScrollPos = 0
        Me.toServer.Name = "toServer"
        Me.ToolTip1.SetToolTip(Me.toServer, resources.GetString("toServer.ToolTip"))
        Me.toServer.VerticalContentAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Top
        Me.toServer.VScrollPos = 0
        '
        'CheckBox1
        '
        resources.ApplyResources(Me.CheckBox1, "CheckBox1")
        Me.CheckBox1.Name = "CheckBox1"
        Me.ToolTip1.SetToolTip(Me.CheckBox1, resources.GetString("CheckBox1.ToolTip"))
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Btn_Bold
        '
        resources.ApplyResources(Me.Btn_Bold, "Btn_Bold")
        Me.Btn_Bold.Name = "Btn_Bold"
        Me.ToolTip1.SetToolTip(Me.Btn_Bold, resources.GetString("Btn_Bold.ToolTip"))
        Me.Btn_Bold.UseVisualStyleBackColor = True
        '
        'BTN_Underline
        '
        resources.ApplyResources(Me.BTN_Underline, "BTN_Underline")
        Me.BTN_Underline.Name = "BTN_Underline"
        Me.ToolTip1.SetToolTip(Me.BTN_Underline, resources.GetString("BTN_Underline.ToolTip"))
        Me.BTN_Underline.UseVisualStyleBackColor = True
        '
        'BTN_Italic
        '
        resources.ApplyResources(Me.BTN_Italic, "BTN_Italic")
        Me.BTN_Italic.Name = "BTN_Italic"
        Me.ToolTip1.SetToolTip(Me.BTN_Italic, resources.GetString("BTN_Italic.ToolTip"))
        Me.BTN_Italic.UseVisualStyleBackColor = True
        '
        'sendToServer
        '
        resources.ApplyResources(Me.sendToServer, "sendToServer")
        Me.sendToServer.Name = "sendToServer"
        Me.ToolTip1.SetToolTip(Me.sendToServer, resources.GetString("sendToServer.ToolTip"))
        Me.sendToServer.UseVisualStyleBackColor = True
        '
        'BTN_Go
        '
        resources.ApplyResources(Me.BTN_Go, "BTN_Go")
        Me.BTN_Go.Name = "BTN_Go"
        Me.ToolTip1.SetToolTip(Me.BTN_Go, resources.GetString("BTN_Go.ToolTip"))
        Me.BTN_Go.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.TextBox_NoFlicker1)
        Me.GroupBox1.Controls.Add(Me.DreamList)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox1, resources.GetString("GroupBox1.ToolTip"))
        '
        'TextBox_NoFlicker1
        '
        resources.ApplyResources(Me.TextBox_NoFlicker1, "TextBox_NoFlicker1")
        Me.TextBox_NoFlicker1.Name = "TextBox_NoFlicker1"
        Me.ToolTip1.SetToolTip(Me.TextBox_NoFlicker1, resources.GetString("TextBox_NoFlicker1.ToolTip"))
        '
        'DreamList
        '
        resources.ApplyResources(Me.DreamList, "DreamList")
        Me.DreamList.FormattingEnabled = True
        Me.DreamList.Name = "DreamList"
        Me.ToolTip1.SetToolTip(Me.DreamList, resources.GetString("DreamList.ToolTip"))
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        '
        'GrpAction
        '
        resources.ApplyResources(Me.GrpAction, "GrpAction")
        Me.GrpAction.Controls.Add(Me.BtnSit_stand_Lie)
        Me.GrpAction.Controls.Add(Me.BTN_TurnL)
        Me.GrpAction.Controls.Add(Me.BTN_TurnR)
        Me.GrpAction.Controls.Add(Me._ne)
        Me.GrpAction.Controls.Add(Me._nw)
        Me.GrpAction.Controls.Add(Me.use_)
        Me.GrpAction.Controls.Add(Me.get_)
        Me.GrpAction.Controls.Add(Me.se_)
        Me.GrpAction.Controls.Add(Me.sw_)
        Me.GrpAction.Name = "GrpAction"
        Me.GrpAction.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GrpAction, resources.GetString("GrpAction.ToolTip"))
        '
        'BtnSit_stand_Lie
        '
        resources.ApplyResources(Me.BtnSit_stand_Lie, "BtnSit_stand_Lie")
        Me.BtnSit_stand_Lie.Name = "BtnSit_stand_Lie"
        Me.ToolTip1.SetToolTip(Me.BtnSit_stand_Lie, resources.GetString("BtnSit_stand_Lie.ToolTip"))
        Me.BtnSit_stand_Lie.UseVisualStyleBackColor = True
        '
        'BTN_TurnL
        '
        resources.ApplyResources(Me.BTN_TurnL, "BTN_TurnL")
        Me.BTN_TurnL.Name = "BTN_TurnL"
        Me.ToolTip1.SetToolTip(Me.BTN_TurnL, resources.GetString("BTN_TurnL.ToolTip"))
        Me.BTN_TurnL.UseVisualStyleBackColor = True
        '
        'BTN_TurnR
        '
        resources.ApplyResources(Me.BTN_TurnR, "BTN_TurnR")
        Me.BTN_TurnR.Name = "BTN_TurnR"
        Me.ToolTip1.SetToolTip(Me.BTN_TurnR, resources.GetString("BTN_TurnR.ToolTip"))
        Me.BTN_TurnR.UseVisualStyleBackColor = True
        '
        '_ne
        '
        resources.ApplyResources(Me._ne, "_ne")
        Me._ne.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._ne.Cursor = System.Windows.Forms.Cursors.Hand
        Me._ne.Name = "_ne"
        Me._ne.TabStop = False
        Me.ToolTip1.SetToolTip(Me._ne, resources.GetString("_ne.ToolTip"))
        '
        '_nw
        '
        resources.ApplyResources(Me._nw, "_nw")
        Me._nw.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._nw.Cursor = System.Windows.Forms.Cursors.Hand
        Me._nw.Name = "_nw"
        Me._nw.TabStop = False
        Me.ToolTip1.SetToolTip(Me._nw, resources.GetString("_nw.ToolTip"))
        '
        'use_
        '
        resources.ApplyResources(Me.use_, "use_")
        Me.use_.Cursor = System.Windows.Forms.Cursors.Hand
        Me.use_.FlatAppearance.BorderSize = 0
        Me.use_.Name = "use_"
        Me.ToolTip1.SetToolTip(Me.use_, resources.GetString("use_.ToolTip"))
        Me.use_.UseVisualStyleBackColor = True
        '
        'get_
        '
        resources.ApplyResources(Me.get_, "get_")
        Me.get_.Cursor = System.Windows.Forms.Cursors.Hand
        Me.get_.FlatAppearance.BorderSize = 0
        Me.get_.Name = "get_"
        Me.ToolTip1.SetToolTip(Me.get_, resources.GetString("get_.ToolTip"))
        Me.get_.UseVisualStyleBackColor = True
        '
        'se_
        '
        resources.ApplyResources(Me.se_, "se_")
        Me.se_.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.se_.Cursor = System.Windows.Forms.Cursors.Hand
        Me.se_.Name = "se_"
        Me.se_.TabStop = False
        Me.ToolTip1.SetToolTip(Me.se_, resources.GetString("se_.ToolTip"))
        '
        'sw_
        '
        resources.ApplyResources(Me.sw_, "sw_")
        Me.sw_.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.sw_.Cursor = System.Windows.Forms.Cursors.Hand
        Me.sw_.Name = "sw_"
        Me.sw_.TabStop = False
        Me.ToolTip1.SetToolTip(Me.sw_, resources.GetString("sw_.ToolTip"))
        '
        'ContextTryIcon
        '
        resources.ApplyResources(Me.ContextTryIcon, "ContextTryIcon")
        Me.ContextTryIcon.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextTryIcon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RestoreMainToolStripMenuItem, Me.EditorTrayIconMenuItem, Me.ToolStripSeparator1, Me.ConnectTrayIconMenuItem, Me.DisconnectTrayIconMenuItem, Me.ToolStripSeparator2, Me.ExitTrayIconMenuItem})
        Me.ContextTryIcon.Name = "ContextMenuStrip1"
        Me.ToolTip1.SetToolTip(Me.ContextTryIcon, resources.GetString("ContextTryIcon.ToolTip"))
        '
        'RestoreMainToolStripMenuItem
        '
        resources.ApplyResources(Me.RestoreMainToolStripMenuItem, "RestoreMainToolStripMenuItem")
        Me.RestoreMainToolStripMenuItem.Name = "RestoreMainToolStripMenuItem"
        '
        'EditorTrayIconMenuItem
        '
        resources.ApplyResources(Me.EditorTrayIconMenuItem, "EditorTrayIconMenuItem")
        Me.EditorTrayIconMenuItem.Name = "EditorTrayIconMenuItem"
        '
        'ToolStripSeparator1
        '
        resources.ApplyResources(Me.ToolStripSeparator1, "ToolStripSeparator1")
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        '
        'ConnectTrayIconMenuItem
        '
        resources.ApplyResources(Me.ConnectTrayIconMenuItem, "ConnectTrayIconMenuItem")
        Me.ConnectTrayIconMenuItem.Name = "ConnectTrayIconMenuItem"
        '
        'DisconnectTrayIconMenuItem
        '
        resources.ApplyResources(Me.DisconnectTrayIconMenuItem, "DisconnectTrayIconMenuItem")
        Me.DisconnectTrayIconMenuItem.Name = "DisconnectTrayIconMenuItem"
        '
        'ToolStripSeparator2
        '
        resources.ApplyResources(Me.ToolStripSeparator2, "ToolStripSeparator2")
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        '
        'ExitTrayIconMenuItem
        '
        resources.ApplyResources(Me.ExitTrayIconMenuItem, "ExitTrayIconMenuItem")
        Me.ExitTrayIconMenuItem.Name = "ExitTrayIconMenuItem"
        '
        'ActionTmr
        '
        Me.ActionTmr.Interval = 250
        '
        'TS_Main
        '
        resources.ApplyResources(Me.TS_Main, "TS_Main")
        Me.TS_Main.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.TS_Main.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TS_Status_BotName, Me.FurcTimeLbl, Me.TS_Filler, Me.ToolStripServerStatus, Me.ToolStripClientStatus})
        Me.TS_Main.Name = "TS_Main"
        Me.ToolTip1.SetToolTip(Me.TS_Main, resources.GetString("TS_Main.ToolTip"))
        '
        'TS_Status_BotName
        '
        resources.ApplyResources(Me.TS_Status_BotName, "TS_Status_BotName")
        Me.TS_Status_BotName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.TS_Status_BotName.Name = "TS_Status_BotName"
        '
        'FurcTimeLbl
        '
        resources.ApplyResources(Me.FurcTimeLbl, "FurcTimeLbl")
        Me.FurcTimeLbl.Name = "FurcTimeLbl"
        '
        'TS_Filler
        '
        resources.ApplyResources(Me.TS_Filler, "TS_Filler")
        Me.TS_Filler.Name = "TS_Filler"
        Me.TS_Filler.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always
        Me.TS_Filler.Spring = True
        '
        'ToolStripServerStatus
        '
        resources.ApplyResources(Me.ToolStripServerStatus, "ToolStripServerStatus")
        Me.ToolStripServerStatus.Image = Global.SilverMonkey.My.Resources.Resources.DisconnectedImg
        Me.ToolStripServerStatus.Name = "ToolStripServerStatus"
        '
        'ToolStripClientStatus
        '
        resources.ApplyResources(Me.ToolStripClientStatus, "ToolStripClientStatus")
        Me.ToolStripClientStatus.Image = Global.SilverMonkey.My.Resources.Resources.DisconnectedImg
        Me.ToolStripClientStatus.Name = "ToolStripClientStatus"
        '
        'EditMenu
        '
        resources.ApplyResources(Me.EditMenu, "EditMenu")
        Me.EditMenu.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.EditMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuCopy, Me.MenuCut, Me.PasteToolStripMenuItem, Me.ToolStripSeparator3})
        Me.EditMenu.Name = "EditMenu"
        Me.EditMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.ToolTip1.SetToolTip(Me.EditMenu, resources.GetString("EditMenu.ToolTip"))
        '
        'MenuCopy
        '
        resources.ApplyResources(Me.MenuCopy, "MenuCopy")
        Me.MenuCopy.Name = "MenuCopy"
        '
        'MenuCut
        '
        resources.ApplyResources(Me.MenuCut, "MenuCut")
        Me.MenuCut.Name = "MenuCut"
        '
        'PasteToolStripMenuItem
        '
        resources.ApplyResources(Me.PasteToolStripMenuItem, "PasteToolStripMenuItem")
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        '
        'ToolStripSeparator3
        '
        resources.ApplyResources(Me.ToolStripSeparator3, "ToolStripSeparator3")
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        '
        'CopyMenu
        '
        resources.ApplyResources(Me.CopyMenu, "CopyMenu")
        Me.CopyMenu.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.CopyMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuCopy2, Me.MenuCut2, Me.ToolStripMenuItem3, Me.ToolStripSeparator4})
        Me.CopyMenu.Name = "EditMenu"
        Me.CopyMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.ToolTip1.SetToolTip(Me.CopyMenu, resources.GetString("CopyMenu.ToolTip"))
        '
        'MenuCopy2
        '
        resources.ApplyResources(Me.MenuCopy2, "MenuCopy2")
        Me.MenuCopy2.Name = "MenuCopy2"
        '
        'MenuCut2
        '
        resources.ApplyResources(Me.MenuCut2, "MenuCut2")
        Me.MenuCut2.Name = "MenuCut2"
        '
        'ToolStripMenuItem3
        '
        resources.ApplyResources(Me.ToolStripMenuItem3, "ToolStripMenuItem3")
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        '
        'ToolStripSeparator4
        '
        resources.ApplyResources(Me.ToolStripSeparator4, "ToolStripSeparator4")
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        '
        'MenuStrip1
        '
        resources.ApplyResources(Me.MenuStrip1, "MenuStrip1")
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.WindowsToolStripMenuItem, Me.ReferenceLinksToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.ToolTip1.SetToolTip(Me.MenuStrip1, resources.GetString("MenuStrip1.ToolTip"))
        '
        'FileToolStripMenuItem
        '
        resources.ApplyResources(Me.FileToolStripMenuItem, "FileToolStripMenuItem")
        Me.FileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewBotToolStripMenuItem, Me.OpenToolStripMenuItem, Me.EditBotToolStripMenuItem, Me.RecentToolStripMenuItem, Me.ToolStripSeparator5, Me.CloseToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        '
        'NewBotToolStripMenuItem
        '
        resources.ApplyResources(Me.NewBotToolStripMenuItem, "NewBotToolStripMenuItem")
        Me.NewBotToolStripMenuItem.Name = "NewBotToolStripMenuItem"
        '
        'OpenToolStripMenuItem
        '
        resources.ApplyResources(Me.OpenToolStripMenuItem, "OpenToolStripMenuItem")
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        '
        'EditBotToolStripMenuItem
        '
        resources.ApplyResources(Me.EditBotToolStripMenuItem, "EditBotToolStripMenuItem")
        Me.EditBotToolStripMenuItem.Name = "EditBotToolStripMenuItem"
        '
        'RecentToolStripMenuItem
        '
        resources.ApplyResources(Me.RecentToolStripMenuItem, "RecentToolStripMenuItem")
        Me.RecentToolStripMenuItem.Name = "RecentToolStripMenuItem"
        '
        'ToolStripSeparator5
        '
        resources.ApplyResources(Me.ToolStripSeparator5, "ToolStripSeparator5")
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        '
        'CloseToolStripMenuItem
        '
        resources.ApplyResources(Me.CloseToolStripMenuItem, "CloseToolStripMenuItem")
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        '
        'OptionsToolStripMenuItem
        '
        resources.ApplyResources(Me.OptionsToolStripMenuItem, "OptionsToolStripMenuItem")
        Me.OptionsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConfigToolStripMenuItem, Me.ExportMonkeySpeakToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        '
        'ConfigToolStripMenuItem
        '
        resources.ApplyResources(Me.ConfigToolStripMenuItem, "ConfigToolStripMenuItem")
        Me.ConfigToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ConfigToolStripMenuItem.Name = "ConfigToolStripMenuItem"
        '
        'ExportMonkeySpeakToolStripMenuItem
        '
        resources.ApplyResources(Me.ExportMonkeySpeakToolStripMenuItem, "ExportMonkeySpeakToolStripMenuItem")
        Me.ExportMonkeySpeakToolStripMenuItem.Name = "ExportMonkeySpeakToolStripMenuItem"
        '
        'WindowsToolStripMenuItem
        '
        resources.ApplyResources(Me.WindowsToolStripMenuItem, "WindowsToolStripMenuItem")
        Me.WindowsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.WindowsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DebugToolStripMenuItem, Me.MSEditorToolStripMenuItem})
        Me.WindowsToolStripMenuItem.Name = "WindowsToolStripMenuItem"
        '
        'DebugToolStripMenuItem
        '
        resources.ApplyResources(Me.DebugToolStripMenuItem, "DebugToolStripMenuItem")
        Me.DebugToolStripMenuItem.Name = "DebugToolStripMenuItem"
        '
        'MSEditorToolStripMenuItem
        '
        resources.ApplyResources(Me.MSEditorToolStripMenuItem, "MSEditorToolStripMenuItem")
        Me.MSEditorToolStripMenuItem.Name = "MSEditorToolStripMenuItem"
        '
        'ReferenceLinksToolStripMenuItem
        '
        resources.ApplyResources(Me.ReferenceLinksToolStripMenuItem, "ReferenceLinksToolStripMenuItem")
        Me.ReferenceLinksToolStripMenuItem.Name = "ReferenceLinksToolStripMenuItem"
        '
        'HelpToolStripMenuItem
        '
        resources.ApplyResources(Me.HelpToolStripMenuItem, "HelpToolStripMenuItem")
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ContentsToolStripMenuItem, Me.MonkeySpeakLibrariesToolStripMenuItem, Me.EngineStartProcessToolStripMenuItem, Me.ToolStripSeparator6, Me.AboutToolStripMenuItem1})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        '
        'ContentsToolStripMenuItem
        '
        resources.ApplyResources(Me.ContentsToolStripMenuItem, "ContentsToolStripMenuItem")
        Me.ContentsToolStripMenuItem.Name = "ContentsToolStripMenuItem"
        '
        'MonkeySpeakLibrariesToolStripMenuItem
        '
        resources.ApplyResources(Me.MonkeySpeakLibrariesToolStripMenuItem, "MonkeySpeakLibrariesToolStripMenuItem")
        Me.MonkeySpeakLibrariesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SilverMonkeyToolStripMenuItem, Me.DefaultEngineLibrariesToolStripMenuItem})
        Me.MonkeySpeakLibrariesToolStripMenuItem.Name = "MonkeySpeakLibrariesToolStripMenuItem"
        '
        'SilverMonkeyToolStripMenuItem
        '
        resources.ApplyResources(Me.SilverMonkeyToolStripMenuItem, "SilverMonkeyToolStripMenuItem")
        Me.SilverMonkeyToolStripMenuItem.Name = "SilverMonkeyToolStripMenuItem"
        '
        'DefaultEngineLibrariesToolStripMenuItem
        '
        resources.ApplyResources(Me.DefaultEngineLibrariesToolStripMenuItem, "DefaultEngineLibrariesToolStripMenuItem")
        Me.DefaultEngineLibrariesToolStripMenuItem.Name = "DefaultEngineLibrariesToolStripMenuItem"
        '
        'EngineStartProcessToolStripMenuItem
        '
        resources.ApplyResources(Me.EngineStartProcessToolStripMenuItem, "EngineStartProcessToolStripMenuItem")
        Me.EngineStartProcessToolStripMenuItem.Name = "EngineStartProcessToolStripMenuItem"
        '
        'ToolStripSeparator6
        '
        resources.ApplyResources(Me.ToolStripSeparator6, "ToolStripSeparator6")
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        '
        'AboutToolStripMenuItem1
        '
        resources.ApplyResources(Me.AboutToolStripMenuItem1, "AboutToolStripMenuItem1")
        Me.AboutToolStripMenuItem1.Name = "AboutToolStripMenuItem1"
        '
        'MainMenu1
        '
        resources.ApplyResources(Me.MainMenu1, "MainMenu1")
        '
        'BotIniOpen
        '
        Me.BotIniOpen.DefaultExt = "bini"
        resources.ApplyResources(Me.BotIniOpen, "BotIniOpen")
        '
        'Main
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.TS_Main)
        Me.Controls.Add(Me.MenuStrip1)
        Me.DoubleBuffered = True
        Me.IsMdiContainer = True
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Main"
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.clientGroup.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GrpAction.ResumeLayout(False)
        Me.GrpAction.PerformLayout()
        CType(Me._ne, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._nw, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sw_, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextTryIcon.ResumeLayout(False)
        Me.TS_Main.ResumeLayout(False)
        Me.TS_Main.PerformLayout()
        CType(Me.FURREListBindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FURREListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.EditMenu.ResumeLayout(False)
        Me.CopyMenu.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
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
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConfigToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DebugToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
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
    Friend WithEvents ExportMonkeySpeakToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FURREListBindingSource As Windows.Forms.BindingSource
    Friend WithEvents FURREListBindingSource1 As Windows.Forms.BindingSource
    Friend WithEvents SplitContainer1 As Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As Windows.Forms.SplitContainer
    Friend WithEvents GroupBox3 As Windows.Forms.GroupBox
    Friend WithEvents CheckBox1 As Windows.Forms.CheckBox
    Friend WithEvents Btn_Bold As Windows.Forms.Button
    Friend WithEvents BTN_Underline As Windows.Forms.Button
    Friend WithEvents BTN_Italic As Windows.Forms.Button
    Friend WithEvents sendToServer As Windows.Forms.Button
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents GrpAction As Windows.Forms.GroupBox
    Friend WithEvents BtnSit_stand_Lie As Windows.Forms.Button
    Friend WithEvents BTN_TurnL As Windows.Forms.Button
    Friend WithEvents BTN_TurnR As Windows.Forms.Button
    Friend WithEvents _ne As Windows.Forms.PictureBox
    Friend WithEvents _nw As Windows.Forms.PictureBox
    Friend WithEvents use_ As Windows.Forms.Button
    Friend WithEvents get_ As Windows.Forms.Button
    Friend WithEvents se_ As Windows.Forms.PictureBox
    Friend WithEvents sw_ As Windows.Forms.PictureBox
    Friend WithEvents clientGroup As Windows.Forms.GroupBox
    Friend WithEvents BTN_Go As Windows.Forms.Button
    Friend WithEvents ReferenceLinksToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents toServer As MonkeyCore.Controls.RichTextBoxEx
    Friend WithEvents Log_ As MonkeyCore.Controls.RichTextBoxEx
    Friend WithEvents TextBox_NoFlicker1 As MonkeyCore.Controls.TextBox_NoFlicker
    Friend WithEvents DreamList As MonkeyCore.Controls.ListBox_NoFlicker
    Friend WithEvents MonkeySpeakLibrariesToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents SilverMonkeyToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents DefaultEngineLibrariesToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents EngineStartProcessToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As Windows.Forms.ToolStripSeparator
End Class
