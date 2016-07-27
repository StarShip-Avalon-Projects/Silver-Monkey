Imports System.Windows.Forms
Imports FastColoredTextBoxNS
Imports MonkeyCore.Controls

Public Class TabControleExTabs
    Inherits TabPage

    Private TabControl2 As TabControlEx
    Private popupMenu As AutocompleteMenu

    Public Sub New(ByRef TabControlObject As TabControlEx, name As String)
        TabControl2 = TabControlObject
        Dim tp As New TabPage

        'tp.Text = New_File_Tag + "     "
        TabControl2.TabPages.Add(tp)
        Dim intLastTabIndex As Integer = TabControl2.TabPages.Count - 1
        tp.Name = "tbpageBrowser" & intLastTabIndex.ToString
        'Adds a new tab to your tab control

        'Creates the listview and displays it in the new tab
        Dim lstView As FastColoredTextBox = New FastColoredTextBox()
        lstView.AcceptsTab = True
        lstView.Parent = tp
        lstView.Anchor = CType(AnchorStyles.Left + AnchorStyles.Top + AnchorStyles.Bottom + AnchorStyles.Right, AnchorStyles)
        lstView.Name = name
        lstView.AutoIndent = False
        lstView.Dock = DockStyle.Fill
        lstView.CommentPrefix = "*"
        lstView.Language = Language.Custom
        lstView.Show()
        'lstView.ContextMenuStrip = SectionMenu
        TabControl2.SelectedTab = TabControl2.TabPages(intLastTabIndex)

        'AddHandler lstView.TextChangedDelayed, AddressOf MS_Editor_TextChangedDelayed
        'AddHandler lstView.TextChanged, AddressOf MS_Editor_TextChanged
        'AddHandler lstView.MouseUp, AddressOf MS_EditRightClick
        'AddHandler lstView.CursorChanged, AddressOf MS_Editor_CursorChanged
        'AddHandler lstView.MouseClick, AddressOf MS_Editor_CursorChanged
        'AddHandler lstView.KeyUp, AddressOf MS_Editor_CursorChanged
        'AddHandler lstView.MouseDoubleClick, AddressOf MS_Editor_MouseDoubleClick

    End Sub
End Class
