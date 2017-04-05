''' <summary>
''' General Utility functions we haven't found a home for yet
''' </summary>
<CLSCompliant(True)>
Public Class Utils

#Region "Public Enums"

    Public Enum fColorEnum
        DefaultColor = 0
        Say
        Shout
        Whisper
        Emote
        Emit
    End Enum

#End Region

#Region "Public Methods"

    Public Shared Function DateTimeToUnixTimestamp(dTime As DateTime) As Double
        Return (dTime - New DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds
    End Function

    ''' <summary>
    ''' Gets the file version information.
    ''' </summary>
    ''' <param name="filename">The filename.</param>
    ''' <returns></returns>
    Public Shared Function GetFileVersionInfo(ByVal filename As String) As Version
        Return Version.Parse(FileVersionInfo.GetVersionInfo(filename).FileVersion)
    End Function

    ''' <summary>
    ''' Letter increment for walking PS tables
    ''' </summary>
    ''' <param name="Input"></param>
    ''' <returns></returns>
    Public Shared Function incrementLetter(ByRef Input As String) As Char
        Input = Input.Substring(0, 1)
        Dim i As Integer = AscW(Input)
        Dim test As Char
        Select Case Input
            Case "A"c To "Z"c
                test = ChrW(i + 1)
            Case "a"c To "z"c
                test = ChrW(i + 1)
            Case "0"c To "8"c
                test = ChrW(i + 1)
            Case "9"c
                test = "a"c
            Case Else
                test = "{"c
        End Select
        Input = test
        Return test
    End Function

    'DateTime functions
    ''' <summary>
    ''' Converts a number representing a Unix Time stamp and converts it to a usable DateTime format
    ''' </summary>
    ''' <param name="unixTimeStamp"></param>
    ''' <returns>DataTime</returns>
    Public Shared Function UnixTimeStampToDateTime(ByRef unixTimeStamp As Double) As DateTime
        ' Unix timestamp is seconds past epoch
        Dim dtDateTime As System.DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime
        Return dtDateTime
    End Function

#End Region

End Class