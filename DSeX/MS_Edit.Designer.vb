Imports System.ComponentModel
Imports MonkeyCore.Controls

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MS_Edit
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MS_Edit))
        Me.MSSaveDialog = New System.Windows.Forms.SaveFileDialog()
        Me.MS_BrosweDialog = New System.Windows.Forms.OpenFileDialog()
        Me.EditMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.AutocommentOnToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AutocommentOffToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.imgList = New System.Windows.Forms.ImageList(Me.components)
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewMonkeySpeakToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MonkeySpeakFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DragonSpeakFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestartBotEngineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToHTMLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditDropCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditDropCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditDropPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.FindReplaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RedoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FixIndentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GotoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ApplyCommentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveCommentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowLineFinderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BookmakrsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GotoNextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GotoPreviousToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LanguageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DragonSpeakToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MonkeySpeakToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WindowsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DSWizardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReferenceLinksToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MokeySpeakLinesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AbutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.BtnSectionDelete = New System.Windows.Forms.Button()
        Me.BtnSectionDown = New System.Windows.Forms.Button()
        Me.BtnSectionUp = New System.Windows.Forms.Button()
        Me.BtnSectionAdd = New System.Windows.Forms.Button()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.SectionMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.NewSection = New System.Windows.Forms.ToolStripMenuItem()
        Me.InsertSectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteSection = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.ApplyCommentToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.AutoCommentOffToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.SplitContainer5 = New System.Windows.Forms.SplitContainer()
        Me.BtnTemplateDeleteMS = New System.Windows.Forms.Button()
        Me.BtnTemplateAddMS = New System.Windows.Forms.Button()
        Me.ListBox3 = New System.Windows.Forms.ListBox()
        Me.TemplateMenuMS = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MSTemplateRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.MSTemplateMenuAdd = New System.Windows.Forms.ToolStripMenuItem()
        Me.MSTemplateDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.MSTemplateRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.MSTemplateEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnTemplateAdd = New System.Windows.Forms.Button()
        Me.BtnTemplateDelete = New System.Windows.Forms.Button()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.TemplateMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RefreshTemplatesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.InsertToDSFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.AddToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.RenameToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabControl2 = New MonkeyCore.Controls.TabControlEx()
        Me.ToolBox = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolBoxNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolBoxOpen = New System.Windows.Forms.ToolStripButton()
        Me.ToolBoxSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolBoxSaveAs = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolBoxCut = New System.Windows.Forms.ToolStripButton()
        Me.ToolBoxyCopy = New System.Windows.Forms.ToolStripButton()
        Me.ToolBoxPaste = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolBoxUndo = New System.Windows.Forms.ToolStripButton()
        Me.ToolBoxRedo = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolBoxFindReplace = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.seperateor = New System.Windows.Forms.ToolStripSeparator()
        Me.BtnComment = New System.Windows.Forms.ToolStripButton()
        Me.BtnUncomment = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.lblStatus = New System.Windows.Forms.ToolStripLabel()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TxtBxFind = New System.Windows.Forms.TextBox()
        Me.BtnFind = New System.Windows.Forms.Button()
        Me.SplitContainer4 = New System.Windows.Forms.SplitContainer()
        Me.Causes = New System.Windows.Forms.TabControl()
        Me.TabControl3 = New System.Windows.Forms.TabControl()
        Me.sb = New System.Windows.Forms.StatusBar()
        Me.panelCurrentPosition = New System.Windows.Forms.StatusBarPanel()
        Me.panelCurrentLine = New System.Windows.Forms.StatusBarPanel()
        Me.panelTotalLines = New System.Windows.Forms.StatusBarPanel()
        Me.panelTotalCharacters = New System.Windows.Forms.StatusBarPanel()
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.EditMenu.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SectionMenu.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.SplitContainer5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer5.Panel1.SuspendLayout()
        Me.SplitContainer5.Panel2.SuspendLayout()
        Me.SplitContainer5.SuspendLayout()
        Me.TemplateMenuMS.SuspendLayout()
        Me.TemplateMenu.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.ToolBox.SuspendLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer4.Panel1.SuspendLayout()
        Me.SplitContainer4.Panel2.SuspendLayout()
        Me.SplitContainer4.SuspendLayout()
        CType(Me.panelCurrentPosition, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.panelCurrentLine, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.panelTotalLines, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.panelTotalCharacters, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MSSaveDialog
        '
        Me.MSSaveDialog.DefaultExt = "ms"
        Me.MSSaveDialog.Filter = "MonkeySpeak Files|*.ms|DragonSpeak Files|*.ds|Script Files|*.ini|All files|*.*"
        Me.MSSaveDialog.RestoreDirectory = True
        Me.MSSaveDialog.Title = "SaveAs"
        '
        'MS_BrosweDialog
        '
        Me.MS_BrosweDialog.DefaultExt = "ms"
        Me.MS_BrosweDialog.Filter = "MonkeySpeak Files|*.ms|DragonSpeak Files|*.ds|Script Files|*.ini|All files|*.*"
        '
        'EditMenu
        '
        Me.EditMenu.ImageScalingSize = New System.Drawing.Size(40, 40)
        Me.EditMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuCopy, Me.MenuCut, Me.PasteToolStripMenuItem, Me.ToolStripSeparator12, Me.SelectAllToolStripMenuItem, Me.ToolStripSeparator3, Me.AutocommentOnToolStripMenuItem, Me.AutocommentOffToolStripMenuItem1})
        Me.EditMenu.Name = "EditMenu"
        Me.EditMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.EditMenu.ShowImageMargin = False
        Me.EditMenu.Size = New System.Drawing.Size(307, 292)
        '
        'MenuCopy
        '
        Me.MenuCopy.Name = "MenuCopy"
        Me.MenuCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.MenuCopy.Size = New System.Drawing.Size(306, 46)
        Me.MenuCopy.Text = "Copy"
        '
        'MenuCut
        '
        Me.MenuCut.Name = "MenuCut"
        Me.MenuCut.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
        Me.MenuCut.Size = New System.Drawing.Size(306, 46)
        Me.MenuCut.Text = "Cut"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(306, 46)
        Me.PasteToolStripMenuItem.Text = "Paste"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(303, 6)
        '
        'SelectAllToolStripMenuItem
        '
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(306, 46)
        Me.SelectAllToolStripMenuItem.Text = "Select All"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(303, 6)
        '
        'AutocommentOnToolStripMenuItem
        '
        Me.AutocommentOnToolStripMenuItem.Name = "AutocommentOnToolStripMenuItem"
        Me.AutocommentOnToolStripMenuItem.Size = New System.Drawing.Size(306, 46)
        Me.AutocommentOnToolStripMenuItem.Text = "Autocomment on"
        '
        'AutocommentOffToolStripMenuItem1
        '
        Me.AutocommentOffToolStripMenuItem1.Name = "AutocommentOffToolStripMenuItem1"
        Me.AutocommentOffToolStripMenuItem1.Size = New System.Drawing.Size(306, 46)
        Me.AutocommentOffToolStripMenuItem1.Text = "Autocomment off"
        '
        'imgList
        '
        Me.imgList.ImageStream = CType(resources.GetObject("imgList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgList.TransparentColor = System.Drawing.Color.Transparent
        Me.imgList.Images.SetKeyName(0, "")
        Me.imgList.Images.SetKeyName(1, "")
        Me.imgList.Images.SetKeyName(2, "")
        Me.imgList.Images.SetKeyName(3, "")
        Me.imgList.Images.SetKeyName(4, "")
        Me.imgList.Images.SetKeyName(5, "")
        Me.imgList.Images.SetKeyName(6, "")
        Me.imgList.Images.SetKeyName(7, "")
        Me.imgList.Images.SetKeyName(8, "")
        Me.imgList.Images.SetKeyName(9, "")
        Me.imgList.Images.SetKeyName(10, "")
        Me.imgList.Images.SetKeyName(11, "")
        Me.imgList.Images.SetKeyName(12, "")
        Me.imgList.Images.SetKeyName(13, "")
        Me.imgList.Images.SetKeyName(14, "")
        Me.imgList.Images.SetKeyName(15, "")
        Me.imgList.Images.SetKeyName(16, "")
        Me.imgList.Images.SetKeyName(17, "")
        Me.imgList.Images.SetKeyName(18, "")
        Me.imgList.Images.SetKeyName(19, "")
        Me.imgList.Images.SetKeyName(20, "")
        Me.imgList.Images.SetKeyName(21, "")
        Me.imgList.Images.SetKeyName(22, "")
        Me.imgList.Images.SetKeyName(23, "")
        Me.imgList.Images.SetKeyName(24, "")
        Me.imgList.Images.SetKeyName(25, "")
        Me.imgList.Images.SetKeyName(26, "")
        Me.imgList.Images.SetKeyName(27, "")
        Me.imgList.Images.SetKeyName(28, "")
        Me.imgList.Images.SetKeyName(29, "")
        Me.imgList.Images.SetKeyName(30, "")
        Me.imgList.Images.SetKeyName(31, "")
        Me.imgList.Images.SetKeyName(32, "")
        Me.imgList.Images.SetKeyName(33, "")
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewMonkeySpeakToolStripMenuItem, Me.OpenToolStripMenuItem, Me.SaveToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.RestartBotEngineToolStripMenuItem, Me.ToolStripSeparator1, Me.ExitToolStripMenuItem, Me.CloseToolStripMenuItem, Me.CloseAllToolStripMenuItem, Me.ExportToHTMLToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(75, 45)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NewMonkeySpeakToolStripMenuItem
        '
        Me.NewMonkeySpeakToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MonkeySpeakFileToolStripMenuItem, Me.DragonSpeakFileToolStripMenuItem})
        Me.NewMonkeySpeakToolStripMenuItem.Name = "NewMonkeySpeakToolStripMenuItem"
        Me.NewMonkeySpeakToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.NewMonkeySpeakToolStripMenuItem.Size = New System.Drawing.Size(374, 46)
        Me.NewMonkeySpeakToolStripMenuItem.Text = "New File"
        '
        'MonkeySpeakFileToolStripMenuItem
        '
        Me.MonkeySpeakFileToolStripMenuItem.Name = "MonkeySpeakFileToolStripMenuItem"
        Me.MonkeySpeakFileToolStripMenuItem.Size = New System.Drawing.Size(373, 46)
        Me.MonkeySpeakFileToolStripMenuItem.Text = "MonkeySpeak File"
        '
        'DragonSpeakFileToolStripMenuItem
        '
        Me.DragonSpeakFileToolStripMenuItem.Name = "DragonSpeakFileToolStripMenuItem"
        Me.DragonSpeakFileToolStripMenuItem.Size = New System.Drawing.Size(373, 46)
        Me.DragonSpeakFileToolStripMenuItem.Text = "DragonSpeak File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(374, 46)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(374, 46)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(374, 46)
        Me.SaveAsToolStripMenuItem.Text = "SaveAs"
        '
        'RestartBotEngineToolStripMenuItem
        '
        Me.RestartBotEngineToolStripMenuItem.Name = "RestartBotEngineToolStripMenuItem"
        Me.RestartBotEngineToolStripMenuItem.Size = New System.Drawing.Size(374, 46)
        Me.RestartBotEngineToolStripMenuItem.Text = "Restart Bot Engine"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(371, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(374, 46)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(374, 46)
        Me.CloseToolStripMenuItem.Text = "Close"
        '
        'CloseAllToolStripMenuItem
        '
        Me.CloseAllToolStripMenuItem.Name = "CloseAllToolStripMenuItem"
        Me.CloseAllToolStripMenuItem.Size = New System.Drawing.Size(374, 46)
        Me.CloseAllToolStripMenuItem.Text = "Close All"
        '
        'ExportToHTMLToolStripMenuItem
        '
        Me.ExportToHTMLToolStripMenuItem.Name = "ExportToHTMLToolStripMenuItem"
        Me.ExportToHTMLToolStripMenuItem.Size = New System.Drawing.Size(374, 46)
        Me.ExportToHTMLToolStripMenuItem.Text = "Export to HTML"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditDropCopy, Me.EditDropCut, Me.EditDropPaste, Me.ToolStripSeparator8, Me.FindReplaceToolStripMenuItem, Me.ToolStripSeparator6, Me.UndoToolStripMenuItem, Me.RedoToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(80, 45)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'EditDropCopy
        '
        Me.EditDropCopy.Name = "EditDropCopy"
        Me.EditDropCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.EditDropCopy.Size = New System.Drawing.Size(458, 46)
        Me.EditDropCopy.Text = "Copy"
        '
        'EditDropCut
        '
        Me.EditDropCut.Name = "EditDropCut"
        Me.EditDropCut.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.EditDropCut.Size = New System.Drawing.Size(458, 46)
        Me.EditDropCut.Text = "Cut"
        '
        'EditDropPaste
        '
        Me.EditDropPaste.Name = "EditDropPaste"
        Me.EditDropPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.EditDropPaste.Size = New System.Drawing.Size(458, 46)
        Me.EditDropPaste.Text = "Paste"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(455, 6)
        '
        'FindReplaceToolStripMenuItem
        '
        Me.FindReplaceToolStripMenuItem.Name = "FindReplaceToolStripMenuItem"
        Me.FindReplaceToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.FindReplaceToolStripMenuItem.Size = New System.Drawing.Size(458, 46)
        Me.FindReplaceToolStripMenuItem.Text = "&Find and Replace"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(455, 6)
        '
        'UndoToolStripMenuItem
        '
        Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
        Me.UndoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(458, 46)
        Me.UndoToolStripMenuItem.Text = "Undo"
        '
        'RedoToolStripMenuItem
        '
        Me.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem"
        Me.RedoToolStripMenuItem.Size = New System.Drawing.Size(458, 46)
        Me.RedoToolStripMenuItem.Text = "Redo"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(40, 40)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.BookmakrsToolStripMenuItem, Me.LanguageToolStripMenuItem, Me.WindowsToolStripMenuItem, Me.ReferenceLinksToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(16, 5, 0, 5)
        Me.MenuStrip1.Size = New System.Drawing.Size(2403, 55)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FixIndentsToolStripMenuItem, Me.GotoToolStripMenuItem, Me.ApplyCommentToolStripMenuItem, Me.RemoveCommentToolStripMenuItem, Me.ShowLineFinderToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(99, 45)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'FixIndentsToolStripMenuItem
        '
        Me.FixIndentsToolStripMenuItem.Name = "FixIndentsToolStripMenuItem"
        Me.FixIndentsToolStripMenuItem.Size = New System.Drawing.Size(379, 46)
        Me.FixIndentsToolStripMenuItem.Text = "Fix Indents"
        '
        'GotoToolStripMenuItem
        '
        Me.GotoToolStripMenuItem.Name = "GotoToolStripMenuItem"
        Me.GotoToolStripMenuItem.Size = New System.Drawing.Size(379, 46)
        Me.GotoToolStripMenuItem.Text = "Goto Line"
        '
        'ApplyCommentToolStripMenuItem
        '
        Me.ApplyCommentToolStripMenuItem.Name = "ApplyCommentToolStripMenuItem"
        Me.ApplyCommentToolStripMenuItem.Size = New System.Drawing.Size(379, 46)
        Me.ApplyCommentToolStripMenuItem.Text = "Apply Comment"
        '
        'RemoveCommentToolStripMenuItem
        '
        Me.RemoveCommentToolStripMenuItem.Name = "RemoveCommentToolStripMenuItem"
        Me.RemoveCommentToolStripMenuItem.Size = New System.Drawing.Size(379, 46)
        Me.RemoveCommentToolStripMenuItem.Text = "Remove Comment"
        '
        'ShowLineFinderToolStripMenuItem
        '
        Me.ShowLineFinderToolStripMenuItem.Name = "ShowLineFinderToolStripMenuItem"
        Me.ShowLineFinderToolStripMenuItem.Size = New System.Drawing.Size(379, 46)
        Me.ShowLineFinderToolStripMenuItem.Text = "Show Line Finder"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConfigToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(136, 45)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'ConfigToolStripMenuItem
        '
        Me.ConfigToolStripMenuItem.Name = "ConfigToolStripMenuItem"
        Me.ConfigToolStripMenuItem.Size = New System.Drawing.Size(220, 46)
        Me.ConfigToolStripMenuItem.Text = "Config"
        '
        'BookmakrsToolStripMenuItem
        '
        Me.BookmakrsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveAllToolStripMenuItem, Me.GotoNextToolStripMenuItem, Me.GotoPreviousToolStripMenuItem})
        Me.BookmakrsToolStripMenuItem.Name = "BookmakrsToolStripMenuItem"
        Me.BookmakrsToolStripMenuItem.Size = New System.Drawing.Size(177, 45)
        Me.BookmakrsToolStripMenuItem.Text = "Bookmarks"
        '
        'RemoveAllToolStripMenuItem
        '
        Me.RemoveAllToolStripMenuItem.Name = "RemoveAllToolStripMenuItem"
        Me.RemoveAllToolStripMenuItem.Size = New System.Drawing.Size(368, 46)
        Me.RemoveAllToolStripMenuItem.Text = "Remove All"
        '
        'GotoNextToolStripMenuItem
        '
        Me.GotoNextToolStripMenuItem.Name = "GotoNextToolStripMenuItem"
        Me.GotoNextToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6
        Me.GotoNextToolStripMenuItem.Size = New System.Drawing.Size(368, 46)
        Me.GotoNextToolStripMenuItem.Text = "Goto Next"
        '
        'GotoPreviousToolStripMenuItem
        '
        Me.GotoPreviousToolStripMenuItem.Name = "GotoPreviousToolStripMenuItem"
        Me.GotoPreviousToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.GotoPreviousToolStripMenuItem.Size = New System.Drawing.Size(368, 46)
        Me.GotoPreviousToolStripMenuItem.Text = "Goto Previous"
        '
        'LanguageToolStripMenuItem
        '
        Me.LanguageToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DragonSpeakToolStripMenuItem, Me.MonkeySpeakToolStripMenuItem})
        Me.LanguageToolStripMenuItem.Name = "LanguageToolStripMenuItem"
        Me.LanguageToolStripMenuItem.Size = New System.Drawing.Size(160, 45)
        Me.LanguageToolStripMenuItem.Text = "Language"
        '
        'DragonSpeakToolStripMenuItem
        '
        Me.DragonSpeakToolStripMenuItem.Name = "DragonSpeakToolStripMenuItem"
        Me.DragonSpeakToolStripMenuItem.Size = New System.Drawing.Size(328, 46)
        Me.DragonSpeakToolStripMenuItem.Text = "Dragon Speak"
        '
        'MonkeySpeakToolStripMenuItem
        '
        Me.MonkeySpeakToolStripMenuItem.Name = "MonkeySpeakToolStripMenuItem"
        Me.MonkeySpeakToolStripMenuItem.Size = New System.Drawing.Size(328, 46)
        Me.MonkeySpeakToolStripMenuItem.Text = "Monkey Speak"
        '
        'WindowsToolStripMenuItem
        '
        Me.WindowsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DSWizardToolStripMenuItem})
        Me.WindowsToolStripMenuItem.Name = "WindowsToolStripMenuItem"
        Me.WindowsToolStripMenuItem.Size = New System.Drawing.Size(153, 45)
        Me.WindowsToolStripMenuItem.Text = "Windows"
        '
        'DSWizardToolStripMenuItem
        '
        Me.DSWizardToolStripMenuItem.Name = "DSWizardToolStripMenuItem"
        Me.DSWizardToolStripMenuItem.Size = New System.Drawing.Size(269, 46)
        Me.DSWizardToolStripMenuItem.Text = "DS Wizard"
        '
        'ReferenceLinksToolStripMenuItem
        '
        Me.ReferenceLinksToolStripMenuItem.Name = "ReferenceLinksToolStripMenuItem"
        Me.ReferenceLinksToolStripMenuItem.Size = New System.Drawing.Size(227, 45)
        Me.ReferenceLinksToolStripMenuItem.Text = "ReferenceLinks"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ContentToolStripMenuItem, Me.MokeySpeakLinesToolStripMenuItem, Me.AbutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(92, 45)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'ContentToolStripMenuItem
        '
        Me.ContentToolStripMenuItem.Name = "ContentToolStripMenuItem"
        Me.ContentToolStripMenuItem.Size = New System.Drawing.Size(386, 46)
        Me.ContentToolStripMenuItem.Text = "Contents"
        '
        'MokeySpeakLinesToolStripMenuItem
        '
        Me.MokeySpeakLinesToolStripMenuItem.Name = "MokeySpeakLinesToolStripMenuItem"
        Me.MokeySpeakLinesToolStripMenuItem.Size = New System.Drawing.Size(386, 46)
        Me.MokeySpeakLinesToolStripMenuItem.Text = "Mokey Speak Lines"
        '
        'AbutToolStripMenuItem
        '
        Me.AbutToolStripMenuItem.Name = "AbutToolStripMenuItem"
        Me.AbutToolStripMenuItem.Size = New System.Drawing.Size(386, 46)
        Me.AbutToolStripMenuItem.Text = "About"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 55)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ToolBox)
        Me.SplitContainer1.Panel1.Tag = "panel1"
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer3)
        Me.SplitContainer1.Panel2.Controls.Add(Me.sb)
        Me.SplitContainer1.Size = New System.Drawing.Size(2403, 1032)
        Me.SplitContainer1.SplitterDistance = 609
        Me.SplitContainer1.SplitterWidth = 10
        Me.SplitContainer1.TabIndex = 5
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 47)
        Me.SplitContainer2.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.TabControl1)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.TabControl2)
        Me.SplitContainer2.Size = New System.Drawing.Size(2403, 562)
        Me.SplitContainer2.SplitterDistance = 618
        Me.SplitContainer2.SplitterWidth = 11
        Me.SplitContainer2.TabIndex = 6
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(618, 562)
        Me.TabControl1.TabIndex = 9
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.BtnSectionDelete)
        Me.TabPage1.Controls.Add(Me.BtnSectionDown)
        Me.TabPage1.Controls.Add(Me.BtnSectionUp)
        Me.TabPage1.Controls.Add(Me.BtnSectionAdd)
        Me.TabPage1.Controls.Add(Me.ListBox1)
        Me.TabPage1.Location = New System.Drawing.Point(10, 48)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage1.Size = New System.Drawing.Size(598, 504)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Sections     "
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'BtnSectionDelete
        '
        Me.BtnSectionDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnSectionDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSectionDelete.Location = New System.Drawing.Point(315, 333)
        Me.BtnSectionDelete.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BtnSectionDelete.Name = "BtnSectionDelete"
        Me.BtnSectionDelete.Size = New System.Drawing.Size(83, 83)
        Me.BtnSectionDelete.TabIndex = 9
        Me.BtnSectionDelete.Text = "-"
        Me.BtnSectionDelete.UseVisualStyleBackColor = True
        '
        'BtnSectionDown
        '
        Me.BtnSectionDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnSectionDown.Font = New System.Drawing.Font("Wingdings", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnSectionDown.Location = New System.Drawing.Point(123, 333)
        Me.BtnSectionDown.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BtnSectionDown.Name = "BtnSectionDown"
        Me.BtnSectionDown.Size = New System.Drawing.Size(80, 83)
        Me.BtnSectionDown.TabIndex = 9
        Me.BtnSectionDown.Text = "â"
        Me.BtnSectionDown.UseVisualStyleBackColor = True
        '
        'BtnSectionUp
        '
        Me.BtnSectionUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnSectionUp.Font = New System.Drawing.Font("Wingdings", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnSectionUp.Location = New System.Drawing.Point(21, 333)
        Me.BtnSectionUp.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BtnSectionUp.Name = "BtnSectionUp"
        Me.BtnSectionUp.Size = New System.Drawing.Size(85, 83)
        Me.BtnSectionUp.TabIndex = 9
        Me.BtnSectionUp.Text = "á"
        Me.BtnSectionUp.UseVisualStyleBackColor = True
        '
        'BtnSectionAdd
        '
        Me.BtnSectionAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnSectionAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSectionAdd.Location = New System.Drawing.Point(219, 333)
        Me.BtnSectionAdd.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BtnSectionAdd.Name = "BtnSectionAdd"
        Me.BtnSectionAdd.Size = New System.Drawing.Size(80, 83)
        Me.BtnSectionAdd.TabIndex = 9
        Me.BtnSectionAdd.Text = "+"
        Me.BtnSectionAdd.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox1.ContextMenuStrip = Me.SectionMenu
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 31
        Me.ListBox1.Items.AddRange(New Object() {"Entire Document", "DS-Start", "   Default Section", "DS-End"})
        Me.ListBox1.Location = New System.Drawing.Point(0, 0)
        Me.ListBox1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(542, 283)
        Me.ListBox1.TabIndex = 0
        '
        'SectionMenu
        '
        Me.SectionMenu.ImageScalingSize = New System.Drawing.Size(40, 40)
        Me.SectionMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RenameToolStripMenuItem, Me.ToolStripSeparator11, Me.NewSection, Me.InsertSectionToolStripMenuItem, Me.ToolStripSeparator9, Me.DeleteSection, Me.ToolStripSeparator10, Me.ApplyCommentToolStripMenuItem1, Me.AutoCommentOffToolStripMenuItem})
        Me.SectionMenu.Name = "ContextMenuStrip1"
        Me.SectionMenu.Size = New System.Drawing.Size(350, 298)
        '
        'RenameToolStripMenuItem
        '
        Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
        Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(349, 46)
        Me.RenameToolStripMenuItem.Text = "Rename"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(346, 6)
        '
        'NewSection
        '
        Me.NewSection.Name = "NewSection"
        Me.NewSection.Size = New System.Drawing.Size(349, 46)
        Me.NewSection.Text = "New Section"
        '
        'InsertSectionToolStripMenuItem
        '
        Me.InsertSectionToolStripMenuItem.Name = "InsertSectionToolStripMenuItem"
        Me.InsertSectionToolStripMenuItem.Size = New System.Drawing.Size(349, 46)
        Me.InsertSectionToolStripMenuItem.Text = "Insert Section"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(346, 6)
        '
        'DeleteSection
        '
        Me.DeleteSection.Name = "DeleteSection"
        Me.DeleteSection.Size = New System.Drawing.Size(349, 46)
        Me.DeleteSection.Text = "Delete Section"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(346, 6)
        '
        'ApplyCommentToolStripMenuItem1
        '
        Me.ApplyCommentToolStripMenuItem1.Name = "ApplyCommentToolStripMenuItem1"
        Me.ApplyCommentToolStripMenuItem1.Size = New System.Drawing.Size(349, 46)
        Me.ApplyCommentToolStripMenuItem1.Text = "Autocomment on"
        '
        'AutoCommentOffToolStripMenuItem
        '
        Me.AutoCommentOffToolStripMenuItem.Name = "AutoCommentOffToolStripMenuItem"
        Me.AutoCommentOffToolStripMenuItem.Size = New System.Drawing.Size(349, 46)
        Me.AutoCommentOffToolStripMenuItem.Text = "Auto Comment Off"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.SplitContainer5)
        Me.TabPage2.Location = New System.Drawing.Point(10, 48)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage2.Size = New System.Drawing.Size(598, 504)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Templates"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'SplitContainer5
        '
        Me.SplitContainer5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer5.Location = New System.Drawing.Point(8, 7)
        Me.SplitContainer5.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.SplitContainer5.Name = "SplitContainer5"
        Me.SplitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer5.Panel1
        '
        Me.SplitContainer5.Panel1.Controls.Add(Me.BtnTemplateDeleteMS)
        Me.SplitContainer5.Panel1.Controls.Add(Me.BtnTemplateAddMS)
        Me.SplitContainer5.Panel1.Controls.Add(Me.ListBox3)
        '
        'SplitContainer5.Panel2
        '
        Me.SplitContainer5.Panel2.Controls.Add(Me.BtnTemplateAdd)
        Me.SplitContainer5.Panel2.Controls.Add(Me.BtnTemplateDelete)
        Me.SplitContainer5.Panel2.Controls.Add(Me.ListBox2)
        Me.SplitContainer5.Panel2Collapsed = True
        Me.SplitContainer5.Size = New System.Drawing.Size(582, 490)
        Me.SplitContainer5.SplitterDistance = 25
        Me.SplitContainer5.SplitterWidth = 10
        Me.SplitContainer5.TabIndex = 0
        '
        'BtnTemplateDeleteMS
        '
        Me.BtnTemplateDeleteMS.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnTemplateDeleteMS.Location = New System.Drawing.Point(141, 421)
        Me.BtnTemplateDeleteMS.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BtnTemplateDeleteMS.Name = "BtnTemplateDeleteMS"
        Me.BtnTemplateDeleteMS.Size = New System.Drawing.Size(136, 55)
        Me.BtnTemplateDeleteMS.TabIndex = 9
        Me.BtnTemplateDeleteMS.Text = "Delete"
        Me.BtnTemplateDeleteMS.UseVisualStyleBackColor = True
        '
        'BtnTemplateAddMS
        '
        Me.BtnTemplateAddMS.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnTemplateAddMS.Location = New System.Drawing.Point(8, 421)
        Me.BtnTemplateAddMS.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BtnTemplateAddMS.Name = "BtnTemplateAddMS"
        Me.BtnTemplateAddMS.Size = New System.Drawing.Size(136, 55)
        Me.BtnTemplateAddMS.TabIndex = 8
        Me.BtnTemplateAddMS.Text = "Add"
        Me.BtnTemplateAddMS.UseVisualStyleBackColor = True
        '
        'ListBox3
        '
        Me.ListBox3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox3.ContextMenuStrip = Me.TemplateMenuMS
        Me.ListBox3.FormattingEnabled = True
        Me.ListBox3.ItemHeight = 31
        Me.ListBox3.Location = New System.Drawing.Point(8, 7)
        Me.ListBox3.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.ListBox3.Name = "ListBox3"
        Me.ListBox3.Size = New System.Drawing.Size(538, 345)
        Me.ListBox3.TabIndex = 6
        '
        'TemplateMenuMS
        '
        Me.TemplateMenuMS.ImageScalingSize = New System.Drawing.Size(40, 40)
        Me.TemplateMenuMS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MSTemplateRefresh, Me.ToolStripSeparator16, Me.ToolStripMenuItem2, Me.ToolStripSeparator17, Me.MSTemplateMenuAdd, Me.MSTemplateDelete, Me.ToolStripSeparator18, Me.MSTemplateRename, Me.MSTemplateEdit})
        Me.TemplateMenuMS.Name = "TemplateMenu"
        Me.TemplateMenuMS.Size = New System.Drawing.Size(337, 298)
        '
        'MSTemplateRefresh
        '
        Me.MSTemplateRefresh.Name = "MSTemplateRefresh"
        Me.MSTemplateRefresh.Size = New System.Drawing.Size(336, 46)
        Me.MSTemplateRefresh.Text = "Refresh Templates"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(333, 6)
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(336, 46)
        Me.ToolStripMenuItem2.Text = "Insert to MS File"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(333, 6)
        '
        'MSTemplateMenuAdd
        '
        Me.MSTemplateMenuAdd.Name = "MSTemplateMenuAdd"
        Me.MSTemplateMenuAdd.Size = New System.Drawing.Size(336, 46)
        Me.MSTemplateMenuAdd.Text = "Add"
        '
        'MSTemplateDelete
        '
        Me.MSTemplateDelete.Name = "MSTemplateDelete"
        Me.MSTemplateDelete.Size = New System.Drawing.Size(336, 46)
        Me.MSTemplateDelete.Text = "Delete"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(333, 6)
        '
        'MSTemplateRename
        '
        Me.MSTemplateRename.Name = "MSTemplateRename"
        Me.MSTemplateRename.Size = New System.Drawing.Size(336, 46)
        Me.MSTemplateRename.Text = "Rename"
        '
        'MSTemplateEdit
        '
        Me.MSTemplateEdit.Name = "MSTemplateEdit"
        Me.MSTemplateEdit.Size = New System.Drawing.Size(336, 46)
        Me.MSTemplateEdit.Text = "Edit"
        '
        'BtnTemplateAdd
        '
        Me.BtnTemplateAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnTemplateAdd.Location = New System.Drawing.Point(13, 334)
        Me.BtnTemplateAdd.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BtnTemplateAdd.Name = "BtnTemplateAdd"
        Me.BtnTemplateAdd.Size = New System.Drawing.Size(136, 52)
        Me.BtnTemplateAdd.TabIndex = 5
        Me.BtnTemplateAdd.Text = "Add"
        Me.BtnTemplateAdd.UseVisualStyleBackColor = True
        '
        'BtnTemplateDelete
        '
        Me.BtnTemplateDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnTemplateDelete.Location = New System.Drawing.Point(147, 334)
        Me.BtnTemplateDelete.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BtnTemplateDelete.Name = "BtnTemplateDelete"
        Me.BtnTemplateDelete.Size = New System.Drawing.Size(131, 52)
        Me.BtnTemplateDelete.TabIndex = 4
        Me.BtnTemplateDelete.Text = "Delete"
        Me.BtnTemplateDelete.UseVisualStyleBackColor = True
        '
        'ListBox2
        '
        Me.ListBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox2.ContextMenuStrip = Me.TemplateMenu
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.ItemHeight = 31
        Me.ListBox2.Location = New System.Drawing.Point(13, 7)
        Me.ListBox2.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(257, 252)
        Me.ListBox2.TabIndex = 3
        '
        'TemplateMenu
        '
        Me.TemplateMenu.ImageScalingSize = New System.Drawing.Size(40, 40)
        Me.TemplateMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RefreshTemplatesToolStripMenuItem, Me.ToolStripSeparator13, Me.InsertToDSFileToolStripMenuItem, Me.ToolStripSeparator14, Me.AddToolStripMenuItem, Me.DeleteToolStripMenuItem, Me.ToolStripSeparator15, Me.RenameToolStripMenuItem1, Me.EditToolStripMenuItem1})
        Me.TemplateMenu.Name = "TemplateMenu"
        Me.TemplateMenu.Size = New System.Drawing.Size(337, 298)
        '
        'RefreshTemplatesToolStripMenuItem
        '
        Me.RefreshTemplatesToolStripMenuItem.Name = "RefreshTemplatesToolStripMenuItem"
        Me.RefreshTemplatesToolStripMenuItem.Size = New System.Drawing.Size(336, 46)
        Me.RefreshTemplatesToolStripMenuItem.Text = "Refresh Templates"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(333, 6)
        '
        'InsertToDSFileToolStripMenuItem
        '
        Me.InsertToDSFileToolStripMenuItem.Name = "InsertToDSFileToolStripMenuItem"
        Me.InsertToDSFileToolStripMenuItem.Size = New System.Drawing.Size(336, 46)
        Me.InsertToDSFileToolStripMenuItem.Text = "Insert to DS File"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(333, 6)
        '
        'AddToolStripMenuItem
        '
        Me.AddToolStripMenuItem.Name = "AddToolStripMenuItem"
        Me.AddToolStripMenuItem.Size = New System.Drawing.Size(336, 46)
        Me.AddToolStripMenuItem.Text = "Add"
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(336, 46)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(333, 6)
        '
        'RenameToolStripMenuItem1
        '
        Me.RenameToolStripMenuItem1.Name = "RenameToolStripMenuItem1"
        Me.RenameToolStripMenuItem1.Size = New System.Drawing.Size(336, 46)
        Me.RenameToolStripMenuItem1.Text = "Rename"
        '
        'EditToolStripMenuItem1
        '
        Me.EditToolStripMenuItem1.Name = "EditToolStripMenuItem1"
        Me.EditToolStripMenuItem1.Size = New System.Drawing.Size(336, 46)
        Me.EditToolStripMenuItem1.Text = "Edit"
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.TextBox2)
        Me.TabPage4.Controls.Add(Me.TextBox1)
        Me.TabPage4.Controls.Add(Me.Label1)
        Me.TabPage4.Location = New System.Drawing.Point(10, 48)
        Me.TabPage4.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabPage4.Size = New System.Drawing.Size(598, 504)
        Me.TabPage4.TabIndex = 2
        Me.TabPage4.Text = "Help Notes"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.Location = New System.Drawing.Point(16, 45)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox2.Size = New System.Drawing.Size(499, 128)
        Me.TextBox2.TabIndex = 18
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(13, 193)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(499, 270)
        Me.TextBox1.TabIndex = 17
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 7)
        Me.Label1.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(344, 32)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "MonkeySpeak Line Help"
        '
        'TabControl2
        '
        Me.TabControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl2.Location = New System.Drawing.Point(0, 0)
        Me.TabControl2.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.ShowCloseButtonOnTabs = True
        Me.TabControl2.Size = New System.Drawing.Size(1774, 562)
        Me.TabControl2.TabIndex = 0
        '
        'ToolBox
        '
        Me.ToolBox.ImageScalingSize = New System.Drawing.Size(40, 40)
        Me.ToolBox.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton2, Me.ToolBoxNew, Me.ToolBoxOpen, Me.ToolBoxSave, Me.ToolBoxSaveAs, Me.ToolStripSeparator2, Me.ToolBoxCut, Me.ToolBoxyCopy, Me.ToolBoxPaste, Me.ToolStripSeparator4, Me.ToolBoxUndo, Me.ToolBoxRedo, Me.ToolStripSeparator5, Me.ToolBoxFindReplace, Me.ToolStripButton1, Me.seperateor, Me.BtnComment, Me.BtnUncomment, Me.ToolStripSeparator7, Me.lblStatus})
        Me.ToolBox.Location = New System.Drawing.Point(0, 0)
        Me.ToolBox.Name = "ToolBox"
        Me.ToolBox.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.ToolBox.Size = New System.Drawing.Size(2403, 47)
        Me.ToolBox.TabIndex = 5
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(44, 44)
        Me.ToolStripButton2.Text = "ToolStripButton2"
        '
        'ToolBoxNew
        '
        Me.ToolBoxNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBoxNew.Image = Global.MonkeySpeakEditor.My.Resources.Resources.NewDocumentHS
        Me.ToolBoxNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolBoxNew.Name = "ToolBoxNew"
        Me.ToolBoxNew.Size = New System.Drawing.Size(44, 44)
        Me.ToolBoxNew.Text = "ToolStripButton2"
        Me.ToolBoxNew.ToolTipText = "New File"
        '
        'ToolBoxOpen
        '
        Me.ToolBoxOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBoxOpen.Image = Global.MonkeySpeakEditor.My.Resources.Resources.OpenFile
        Me.ToolBoxOpen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolBoxOpen.Name = "ToolBoxOpen"
        Me.ToolBoxOpen.Size = New System.Drawing.Size(44, 44)
        Me.ToolBoxOpen.Text = "ToolStripButton3"
        Me.ToolBoxOpen.ToolTipText = "Open File"
        '
        'ToolBoxSave
        '
        Me.ToolBoxSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBoxSave.Image = Global.MonkeySpeakEditor.My.Resources.Resources.saveHS
        Me.ToolBoxSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolBoxSave.Name = "ToolBoxSave"
        Me.ToolBoxSave.Size = New System.Drawing.Size(44, 44)
        Me.ToolBoxSave.Text = "ToolStripButton4"
        Me.ToolBoxSave.ToolTipText = "Save"
        '
        'ToolBoxSaveAs
        '
        Me.ToolBoxSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBoxSaveAs.Image = Global.MonkeySpeakEditor.My.Resources.Resources.SaveAsWebPageHS
        Me.ToolBoxSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolBoxSaveAs.Name = "ToolBoxSaveAs"
        Me.ToolBoxSaveAs.Size = New System.Drawing.Size(44, 44)
        Me.ToolBoxSaveAs.Text = "SaveAs"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 47)
        '
        'ToolBoxCut
        '
        Me.ToolBoxCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBoxCut.Image = Global.MonkeySpeakEditor.My.Resources.Resources.CutHS
        Me.ToolBoxCut.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolBoxCut.Name = "ToolBoxCut"
        Me.ToolBoxCut.Size = New System.Drawing.Size(44, 44)
        Me.ToolBoxCut.Text = "Cut"
        '
        'ToolBoxyCopy
        '
        Me.ToolBoxyCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBoxyCopy.Image = Global.MonkeySpeakEditor.My.Resources.Resources.CopyHS
        Me.ToolBoxyCopy.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolBoxyCopy.Name = "ToolBoxyCopy"
        Me.ToolBoxyCopy.Size = New System.Drawing.Size(44, 44)
        Me.ToolBoxyCopy.Text = "Copy"
        '
        'ToolBoxPaste
        '
        Me.ToolBoxPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBoxPaste.Image = Global.MonkeySpeakEditor.My.Resources.Resources.PasteHS
        Me.ToolBoxPaste.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolBoxPaste.Name = "ToolBoxPaste"
        Me.ToolBoxPaste.Size = New System.Drawing.Size(44, 44)
        Me.ToolBoxPaste.Text = "Paste"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 47)
        '
        'ToolBoxUndo
        '
        Me.ToolBoxUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBoxUndo.Image = Global.MonkeySpeakEditor.My.Resources.Resources.Edit_UndoHS
        Me.ToolBoxUndo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolBoxUndo.Name = "ToolBoxUndo"
        Me.ToolBoxUndo.Size = New System.Drawing.Size(44, 44)
        Me.ToolBoxUndo.Text = "ToolStripButton2"
        Me.ToolBoxUndo.ToolTipText = "Undo"
        '
        'ToolBoxRedo
        '
        Me.ToolBoxRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBoxRedo.Image = Global.MonkeySpeakEditor.My.Resources.Resources.Edit_RedoHS
        Me.ToolBoxRedo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolBoxRedo.Name = "ToolBoxRedo"
        Me.ToolBoxRedo.Size = New System.Drawing.Size(44, 44)
        Me.ToolBoxRedo.Text = "Redo"
        Me.ToolBoxRedo.ToolTipText = "Redo"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 47)
        '
        'ToolBoxFindReplace
        '
        Me.ToolBoxFindReplace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBoxFindReplace.Image = Global.MonkeySpeakEditor.My.Resources.Resources.CutHS
        Me.ToolBoxFindReplace.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolBoxFindReplace.Name = "ToolBoxFindReplace"
        Me.ToolBoxFindReplace.Size = New System.Drawing.Size(44, 44)
        Me.ToolBoxFindReplace.Text = "Find and Replace"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = Global.MonkeySpeakEditor.My.Resources.Resources.PageNumber
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(44, 44)
        Me.ToolStripButton1.Text = "ToolBoxGoto"
        Me.ToolStripButton1.ToolTipText = "Goto Line #"
        '
        'seperateor
        '
        Me.seperateor.Name = "seperateor"
        Me.seperateor.Size = New System.Drawing.Size(6, 47)
        '
        'BtnComment
        '
        Me.BtnComment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BtnComment.Image = CType(resources.GetObject("BtnComment.Image"), System.Drawing.Image)
        Me.BtnComment.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BtnComment.Name = "BtnComment"
        Me.BtnComment.Size = New System.Drawing.Size(44, 44)
        Me.BtnComment.Text = "ToolStripButton2"
        Me.BtnComment.ToolTipText = "Apply Comment"
        '
        'BtnUncomment
        '
        Me.BtnUncomment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BtnUncomment.Image = CType(resources.GetObject("BtnUncomment.Image"), System.Drawing.Image)
        Me.BtnUncomment.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BtnUncomment.Name = "BtnUncomment"
        Me.BtnUncomment.Size = New System.Drawing.Size(44, 44)
        Me.BtnUncomment.Text = "ToolStripButton3"
        Me.BtnUncomment.ToolTipText = "Remove Comment"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 47)
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = False
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(366, 22)
        Me.lblStatus.Text = "Status:"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.SplitContainer3.Name = "SplitContainer3"
        Me.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.Button1)
        Me.SplitContainer3.Panel1.Controls.Add(Me.TxtBxFind)
        Me.SplitContainer3.Panel1.Controls.Add(Me.BtnFind)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.SplitContainer4)
        Me.SplitContainer3.Size = New System.Drawing.Size(2403, 356)
        Me.SplitContainer3.SplitterDistance = 38
        Me.SplitContainer3.SplitterWidth = 10
        Me.SplitContainer3.TabIndex = 7
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(2328, 12)
        Me.Button1.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(43, 48)
        Me.Button1.TabIndex = 16
        Me.Button1.Text = "X"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TxtBxFind
        '
        Me.TxtBxFind.Location = New System.Drawing.Point(8, 7)
        Me.TxtBxFind.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxtBxFind.Name = "TxtBxFind"
        Me.TxtBxFind.Size = New System.Drawing.Size(393, 38)
        Me.TxtBxFind.TabIndex = 15
        '
        'BtnFind
        '
        Me.BtnFind.Location = New System.Drawing.Point(429, 7)
        Me.BtnFind.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BtnFind.Name = "BtnFind"
        Me.BtnFind.Size = New System.Drawing.Size(200, 55)
        Me.BtnFind.TabIndex = 14
        Me.BtnFind.Text = "Find"
        Me.BtnFind.UseVisualStyleBackColor = True
        '
        'SplitContainer4
        '
        Me.SplitContainer4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer4.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer4.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.SplitContainer4.Name = "SplitContainer4"
        Me.SplitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer4.Panel1
        '
        Me.SplitContainer4.Panel1.Controls.Add(Me.Causes)
        Me.SplitContainer4.Panel1Collapsed = True
        '
        'SplitContainer4.Panel2
        '
        Me.SplitContainer4.Panel2.Controls.Add(Me.TabControl3)
        Me.SplitContainer4.Size = New System.Drawing.Size(2403, 308)
        Me.SplitContainer4.SplitterDistance = 60
        Me.SplitContainer4.SplitterWidth = 10
        Me.SplitContainer4.TabIndex = 0
        '
        'Causes
        '
        Me.Causes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Causes.Location = New System.Drawing.Point(0, 0)
        Me.Causes.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.Causes.Name = "Causes"
        Me.Causes.SelectedIndex = 0
        Me.Causes.Size = New System.Drawing.Size(150, 60)
        Me.Causes.TabIndex = 10
        '
        'TabControl3
        '
        Me.TabControl3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl3.Location = New System.Drawing.Point(0, 0)
        Me.TabControl3.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TabControl3.Name = "TabControl3"
        Me.TabControl3.SelectedIndex = 0
        Me.TabControl3.Size = New System.Drawing.Size(2403, 308)
        Me.TabControl3.TabIndex = 11
        '
        'sb
        '
        Me.sb.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sb.Location = New System.Drawing.Point(0, 356)
        Me.sb.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.sb.Name = "sb"
        Me.sb.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.panelCurrentPosition, Me.panelCurrentLine, Me.panelTotalLines, Me.panelTotalCharacters})
        Me.sb.ShowPanels = True
        Me.sb.Size = New System.Drawing.Size(2403, 57)
        Me.sb.SizingGrip = False
        Me.sb.TabIndex = 6
        '
        'panelCurrentPosition
        '
        Me.panelCurrentPosition.Name = "panelCurrentPosition"
        Me.panelCurrentPosition.Text = "Cursor Position: 0"
        Me.panelCurrentPosition.ToolTipText = "Shows the current position within the document"
        Me.panelCurrentPosition.Width = 150
        '
        'panelCurrentLine
        '
        Me.panelCurrentLine.Name = "panelCurrentLine"
        Me.panelCurrentLine.Text = "Current Line:"
        Me.panelCurrentLine.ToolTipText = "Displays the current line number the cursor is on"
        '
        'panelTotalLines
        '
        Me.panelTotalLines.Name = "panelTotalLines"
        Me.panelTotalLines.Text = "Total Lines:"
        Me.panelTotalLines.ToolTipText = "Displays the total lines in the document"
        '
        'panelTotalCharacters
        '
        Me.panelTotalCharacters.Name = "panelTotalCharacters"
        Me.panelTotalCharacters.Text = "Total Characters: "
        Me.panelTotalCharacters.ToolTipText = "Displays the total length of the document in characters."
        Me.panelTotalCharacters.Width = 140
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.DisplayIndex = 0
        Me.ColumnHeader3.Text = ""
        Me.ColumnHeader3.Width = 640
        '
        'MS_Edit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(16.0!, 31.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(2403, 1087)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.Name = "MS_Edit"
        Me.Text = "Monkey Speak Editor"
        Me.EditMenu.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.SectionMenu.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.SplitContainer5.Panel1.ResumeLayout(False)
        Me.SplitContainer5.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer5.ResumeLayout(False)
        Me.TemplateMenuMS.ResumeLayout(False)
        Me.TemplateMenu.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.ToolBox.ResumeLayout(False)
        Me.ToolBox.PerformLayout()
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel1.PerformLayout()
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        Me.SplitContainer4.Panel1.ResumeLayout(False)
        Me.SplitContainer4.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer4.ResumeLayout(False)
        CType(Me.panelCurrentPosition, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.panelCurrentLine, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.panelTotalLines, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.panelTotalCharacters, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MSSaveDialog As SaveFileDialog
    Friend WithEvents MS_BrosweDialog As OpenFileDialog
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents EditMenu As ContextMenuStrip
    Friend WithEvents MenuCopy As ToolStripMenuItem
    Friend WithEvents MenuCut As ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Private WithEvents sb As StatusBar
    Friend WithEvents panelCurrentPosition As StatusBarPanel
    Friend WithEvents panelCurrentLine As StatusBarPanel
    Friend WithEvents panelTotalLines As StatusBarPanel
    Friend WithEvents panelTotalCharacters As StatusBarPanel
    Friend WithEvents imgList As ImageList
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditDropCopy As ToolStripMenuItem
    Friend WithEvents EditDropCut As ToolStripMenuItem
    Friend WithEvents EditDropPaste As ToolStripMenuItem
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ToolBox As ToolStrip
    Friend WithEvents lblStatus As ToolStripLabel
    Friend WithEvents ToolBoxNew As ToolStripButton
    Friend WithEvents ToolBoxOpen As ToolStripButton
    Friend WithEvents ToolBoxSave As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolBoxCut As ToolStripButton
    Friend WithEvents ToolBoxyCopy As ToolStripButton
    Friend WithEvents ToolBoxPaste As ToolStripButton
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolBoxUndo As ToolStripButton
    Friend WithEvents ToolBoxRedo As ToolStripButton
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolBoxFindReplace As ToolStripButton
    Friend WithEvents seperateor As ToolStripSeparator
    Friend WithEvents ToolsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AbutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GotoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents UndoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RedoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents SaveAsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolBoxSaveAs As ToolStripButton
    Friend WithEvents NewMonkeySpeakToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents WindowsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BtnComment As ToolStripButton
    Friend WithEvents BtnUncomment As ToolStripButton
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents FixIndentsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ApplyCommentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RemoveCommentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents FindReplaceToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConfigToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DSWizardToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TabCauses As TabPage
    Friend WithEvents ListCauses As ListView_NoFlicker
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents BtnSectionDelete As Button
    Friend WithEvents BtnSectionDown As Button
    Friend WithEvents BtnSectionUp As Button
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents SectionMenu As ContextMenuStrip
    Friend WithEvents CloseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseAllToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewSection As ToolStripMenuItem
    Friend WithEvents DeleteSection As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents BtnSectionAdd As Button
    Friend WithEvents ToolStripSeparator12 As ToolStripSeparator
    Friend WithEvents SelectAllToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AutocommentOnToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AutocommentOffToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents RenameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As ToolStripSeparator
    Friend WithEvents InsertSectionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As ToolStripSeparator
    Friend WithEvents ApplyCommentToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents AutoCommentOffToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TemplateMenu As ContextMenuStrip
    Friend WithEvents RefreshTemplatesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As ToolStripSeparator
    Friend WithEvents InsertToDSFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator14 As ToolStripSeparator
    Friend WithEvents AddToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator15 As ToolStripSeparator
    Friend WithEvents RenameToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents BookmakrsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RemoveAllToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GotoNextToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GotoPreviousToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SplitContainer3 As SplitContainer
    Friend WithEvents Button1 As Button
    Friend WithEvents TxtBxFind As TextBox
    Friend WithEvents BtnFind As Button
    Friend WithEvents SplitContainer4 As SplitContainer
    Friend WithEvents Causes As TabControl
    Friend WithEvents ShowLineFinderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TabControl3 As TabControl
    Friend WithEvents LanguageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DragonSpeakToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MonkeySpeakToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MonkeySpeakFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DragonSpeakFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RestartBotEngineToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripButton2 As ToolStripButton
    Friend WithEvents SplitContainer5 As SplitContainer
    Friend WithEvents BtnTemplateAddMS As Button
    Friend WithEvents ListBox3 As ListBox
    Friend WithEvents BtnTemplateAdd As Button
    Friend WithEvents BtnTemplateDelete As Button
    Friend WithEvents ListBox2 As ListBox
    Friend WithEvents BtnTemplateDeleteMS As Button
    Friend WithEvents TemplateMenuMS As ContextMenuStrip
    Friend WithEvents MSTemplateRefresh As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator16 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator17 As ToolStripSeparator
    Friend WithEvents MSTemplateMenuAdd As ToolStripMenuItem
    Friend WithEvents MSTemplateDelete As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator18 As ToolStripSeparator
    Friend WithEvents MSTemplateRename As ToolStripMenuItem
    Friend WithEvents MSTemplateEdit As ToolStripMenuItem
    Friend WithEvents TabControl2 As TabControlEx
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ContentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReferenceLinksToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MokeySpeakLinesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportToHTMLToolStripMenuItem As ToolStripMenuItem
End Class
