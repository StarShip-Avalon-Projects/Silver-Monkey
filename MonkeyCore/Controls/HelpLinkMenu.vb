Imports System.IO
Imports System.Windows.Forms

Namespace Controls

    Public Class HelpLinkMenu : Inherits ToolStripMenuItem

#Region "Private Fields"

        Private HelpIni As IniFile
        Private HelpSection As IniFile.IniSection
        Private test As String = Application.ProductName

#End Region

        Private _MenuItems As List(Of ToolStripMenuItem)

        Public ReadOnly Property MenuItems As List(Of ToolStripMenuItem)
            Get
                Return _MenuItems
            End Get

        End Property

#Region "Public Constructors"

        Sub New()
            MyBase.New()
            _MenuItems = New List(Of ToolStripMenuItem)
            HelpIni = New IniFile
            HelpIni.Load(Path.Combine(Paths.ApplicationPath, "HelpLink.ini"))
            HelpSection = HelpIni.GetSection(Application.ProductName)
            If Not HelpSection Is Nothing Then
                For Each KeyVal As IniFile.IniSection.IniKey In HelpSection.Keys
                    Dim HelpItem As New ToolStripMenuItem(KeyVal.Name, Nothing, AddressOf RecentFile_click)
                    HelpItem.Name = KeyVal.Name
                    _MenuItems.Add(HelpItem)
                Next
            End If
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' click menu handler
        ''' </summary>
        ''' <param name="sender">
        ''' </param>
        ''' <param name="e">
        ''' </param>
        Public Overridable Sub RecentFile_click(sender As Object, e As EventArgs)

            Dim HelpItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim MyKey As IniFile.IniSection.IniKey = HelpSection.GetKey(HelpItem.Text)
            Process.Start(MyKey.Value.Trim)

        End Sub

#End Region

    End Class

End Namespace