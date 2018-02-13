Imports System.IO
Imports System.Windows.Forms
Imports IO

Namespace Controls

    Public Class RecentFileMenu
        Inherits ToolStripMenuItem

#Region "Recent File List"

        Public MRUlist As Queue(Of String) = New Queue(Of String)(MRUnumber)

        ''' <summary>
        ''' how many list will save
        ''' </summary>
        Const MRUnumber As Integer = 15

        Private Shared _Menu As ToolStripMenuItem

        Public Sub New()
            MyBase.New()
        End Sub

        Public Property RecentFile As String

        ''' <summary>
        ''' load recent file list from file
        ''' </summary>
        Public Sub LoadRecentList()
            'try to load file. If file isn't found, do nothing
            MRUlist.Clear()
            Try
                If Not File.Exists(System.IO.Path.Combine(Paths.ApplicationSettingsPath, RecentFile)) Then
                    File.Create(System.IO.Path.Combine(Paths.ApplicationSettingsPath, RecentFile))
                End If
                Dim listToRead As New StreamReader(System.IO.Path.Combine(Paths.ApplicationSettingsPath, RecentFile), True)
                'read file stream
                Dim line As String = ""
                While (InlineAssignHelper(line, listToRead.ReadLine())) IsNot Nothing
                    'read each line until end of file
                    MRUlist.Enqueue(line)
                End While
                'insert to list
                'close the stream
                listToRead.Close()

                'throw;
            Catch generatedExceptionName As Exception
            End Try

        End Sub

        ''' <summary>
        ''' click menu handler
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Overridable Sub RecentFile_click(sender As Object, e As EventArgs)

        End Sub

        ''' <summary>
        ''' store a list to file and refresh list
        ''' </summary>
        ''' <param name="path"></param>
        Public Sub SaveRecentFile(path As String)
            _Menu.DropDownItems.Clear()
            'clear all recent list from menu
            LoadRecentList()
            'load list from file
            If Not (MRUlist.Contains(path)) Then
                'prevent duplication on recent list
                MRUlist.Enqueue(path)
            End If
            'insert given path into list
            While MRUlist.Count > MRUnumber
                'keep list number not exceeded given value
                MRUlist.Dequeue()
            End While
            For Each item As String In MRUlist
                Dim fileRecent As New ToolStripMenuItem(item, Nothing, AddressOf RecentFile_click)
                'create new menu for each item in list
                'add the menu to "recent" menu
                _Menu.DropDownItems.Add(fileRecent)
            Next
            'writing menu list to file
            Dim stringToWrite As New StreamWriter(System.IO.Path.Combine(Paths.ApplicationSettingsPath, RecentFile))
            'create file called "Recent.txt" located on app folder
            For Each item As String In MRUlist
                'write list to stream
                stringToWrite.WriteLine(item)
            Next
            stringToWrite.Flush()
            'write stream to file
            stringToWrite.Close()
            'close the stream and reclaim memory
        End Sub

        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function

#End Region

    End Class

End Namespace