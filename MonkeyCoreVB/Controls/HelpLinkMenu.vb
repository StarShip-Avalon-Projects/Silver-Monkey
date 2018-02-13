Imports System.IO
Imports System.Windows.Forms
Imports Furcadia.IO

Namespace Controls

    ''' <summary>
    ''' Standard menu class for HelpLink Menu Items
    ''' </summary>
    Public Class HelpLinkMenu : Inherits MenuItem

#Region "Private Fields"

        Private HelpIni As IniFile
        Private HelpSection As IniFile.IniSection

#End Region

        Private _MenuItems As List(Of MenuItem)

        ''' <summary>
        ''' Help Link list for the help menu. Initiliaze the Help Menu first
        ''' and then add range this.ToArray()
        ''' </summary>
        ''' <returns>
        ''' The Lst of links read from the ini file
        ''' </returns>
        Public Overloads ReadOnly Property MenuItems As List(Of MenuItem)
            Get
                Return _MenuItems
            End Get

        End Property

#Region "Public Constructors"

        ''' <summary>
        ''' Read the default Section by <see cref="Application.ProductName"/>
        ''' </summary>
        Sub New()
            MyClass.New(Application.ProductName)
        End Sub

        ''' <summary>
        ''' Read section from the ini file and converit it to a list of ToolStripMenuItems
        ''' </summary>
        ''' <param name="SectionName">
        ''' Arbitrary Secion Name
        ''' </param>
        Sub New(SectionName As String)
            MyBase.New()
            _MenuItems = New List(Of MenuItem)
            HelpIni = New IniFile
            HelpIni.Load(Path.Combine(IO.Paths.ApplicationPath, "HelpLink.ini"))
            HelpSection = HelpIni.GetSection(SectionName)
            If Not HelpSection Is Nothing Then
                For Each KeyVal As IniFile.IniSection.IniKey In HelpSection.Keys
                    Dim HelpItem As New MenuItem(KeyVal.Name, AddressOf RecentFile_click)
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
        ''' the triggering ToolstripMenu object
        ''' </param>
        ''' <param name="e">
        ''' event arguments (not needed)
        ''' </param>
        Public Overridable Sub RecentFile_click(sender As Object, e As EventArgs)

            Dim HelpItem As MenuItem = CType(sender, MenuItem)
            Dim MyKey As IniFile.IniSection.IniKey = HelpSection.GetKey(HelpItem.Text)
            Process.Start(MyKey.Value.Trim)

        End Sub

#End Region

    End Class

End Namespace