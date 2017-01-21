Public Class Utils

    ''' <summary>
    ''' Gets the file version information.
    ''' </summary>
    ''' <param name="filename">The filename.</param>
    ''' <returns></returns>
    Public Shared Function GetFileVersionInfo(ByVal filename As String) As Version
        Return Version.Parse(FileVersionInfo.GetVersionInfo(filename).FileVersion)
    End Function
End Class
