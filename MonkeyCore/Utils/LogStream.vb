Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports Furcadia.Text.FurcadiaMarkup

''' <summary>
''' Log stream for normal log messages
''' </summary>
<CLSCompliant(True)>
Public Class LogStream

#Region "logging functions"

    Private Shared Stack As New List(Of String)
    Private Shared strErrorFilePath As String

    ''' <summary>
    ''' Create a new instance of the log file
    ''' </summary>
    ''' <param name="FileName">
    ''' </param>
    ''' <param name="FilePath">
    ''' </param>
    Public Sub New(FileName As String, FilePath As String)
        strErrorFilePath = Path.Combine(FilePath, FileName & ".log")

        If Not Directory.Exists(FilePath) Then
            Directory.CreateDirectory(FilePath)
        End If

        'If Not File.Exists(strErrorFilePath) Then
        '    File.Create(strErrorFilePath)
        'End If

    End Sub

    ''' <summary>
    ''' Write a an exception line to the log file
    ''' </summary>
    ''' <param name="Message">
    ''' </param>
    ''' <param name="ObjectException">
    ''' </param>
    Public Shared Sub WriteLine(Message As String, ByRef ObjectException As Exception)
        Dim build As New Text.StringBuilder(Message)
        Dim Names As MatchCollection = Regex.Matches(Message, NameFilter)
        For Each Name As Match In Names
            build = build.Replace(Name.ToString, Name.Groups(3).Value)
        Next
        '<name shortname='acuara' forced>
        Dim MyIcon As MatchCollection = Regex.Matches(Message, Iconfilter)

        For Each Icon As Match In MyIcon
            Select Case Icon.Groups(1).Value
                Case "91"
                    build = build.Replace(Icon.ToString, "[#]")
                Case Else
                    build = build.Replace(Icon.ToString, "[" + Icon.Groups(1).Value + "]")
            End Select

        Next

        Dim URLS As MatchCollection = Regex.Matches(Message, UrlFilter)
        For Each URL As Match In URLS
            build = build.Replace(URL.ToString, "URL:" + URL.Groups(2).Value + "(" + URL.Groups(1).Value + ")")
        Next
        Dim IMGS As MatchCollection = Regex.Matches(Message, ImgTagFilter)
        For Each IMG As Match In IMGS
            build = build.Replace(IMG.ToString, "img")
        Next
        Message = build.ToString

        Dim Now As String = Date.Now().ToString("MM/dd/yyyy H:mm:ss")
        Message = Now & ": " & Message
        Dim ioFile As StreamWriter = New StreamWriter(strErrorFilePath, True)
        Try

            For Each line In Stack.ToArray
                ioFile.WriteLine(line)
            Next
            Stack.Clear()
            ioFile.WriteLine(Message)
            ioFile.WriteLine("Error: " & ObjectException.Message)
            ioFile.WriteLine("")
            Dim st As New StackTrace(ObjectException, True)
            If Not ObjectException.InnerException Is Nothing Then
                ioFile.WriteLine("Inner Error: " & ObjectException.InnerException.Message)
                ioFile.WriteLine("")
            End If
            ioFile.WriteLine(ObjectException.Source)
            ioFile.WriteLine(ObjectException.StackTrace)
        Catch ex As IOException
            If (ex.Message.StartsWith("The process cannot access the file") AndAlso
                        ex.Message.EndsWith("because it is being used by another process.")) Then
                Stack.Add(Message)
                Stack.Add("Error: " & ObjectException.Message)
                Stack.Add("")
                If Not ObjectException.InnerException Is Nothing Then
                    Stack.Add("Inner Error: " & ObjectException.InnerException.Message)
                    Stack.Add("")
                End If
                Stack.Add(ObjectException.Source)
                Stack.Add(ObjectException.StackTrace)
            End If
        Catch ex As Exception
            Throw New Exception("there was an error with" + strErrorFilePath, ex)
        Finally
            If Not ioFile.Equals(Nothing) Then
                ioFile.Flush()
                ioFile.Close()

            End If
        End Try

    End Sub

    ''' <summary>
    ''' Write a line to the log file
    ''' </summary>
    ''' <param name="Message">
    ''' </param>
    Public Shared Sub WriteLine(Message As String)
        Dim build As New StringBuilder(Message)
        Dim Names As MatchCollection = Regex.Matches(Message, NameFilter)
        For Each Name As Match In Names
            build = build.Replace(Name.ToString, Name.Groups(3).Value)
        Next
        '<name shortname='acuara' forced>
        Dim MyIcon As MatchCollection = Regex.Matches(Message, Iconfilter)

        For Each Icon As Match In MyIcon
            Select Case Icon.Groups(1).Value
                Case "91"
                    build = build.Replace(Icon.ToString, "[#]")
                Case Else
                    build = build.Replace(Icon.ToString, "[" + Icon.Groups(1).Value + "]")
            End Select

        Next

        Dim URLS As MatchCollection = Regex.Matches(Message, UrlFilter)
        For Each URL As Match In URLS
            build = build.Replace(URL.ToString, "URL:" + URL.Groups(2).Value + "(" + URL.Groups(1).Value + ")")
        Next
        Dim IMGS As MatchCollection = Regex.Matches(Message, ImgTagFilter)
        For Each IMG As Match In IMGS
            build = build.Replace(IMG.ToString, "img")
        Next
        Message = build.ToString

        Dim Now As String = Date.Now().ToString("MM/dd/yyyy H:mm:ss")
        Message = Now & ": " & Message

        Using ioFile As New StreamWriter(strErrorFilePath, True)

            ' Try

            For Each line In Stack.ToArray
                ioFile.WriteLine(line)
            Next
            Stack.Clear()
            ioFile.WriteLine(Message)
            ioFile.Flush()
            ioFile.Close()
        End Using
        'Catch ex As IOException
        '    If (ex.Message.StartsWith("The process cannot access the file") AndAlso
        '        ex.Message.EndsWith("because it is being used by another process.")) Then
        '        Stack.Add(Message)
        '    End If
        'Catch ex As Exception
        '    Throw New Exception("there was an error with" + strErrorFilePath, ex)
        'Finally

        'End Try

    End Sub

    '''' <summary>
    '''' </summary>
    '''' <param name="filePath">
    '''' </param>
    '''' <returns>
    '''' </returns>
    'Private Shared Function IsFileInUse(ByVal filePath As String) As Boolean
    '    Try
    '        Dim contents() As String = System.IO.File.ReadAllLines(filePath)
    '    Catch ex As IOException
    '        Return (ex.Message.StartsWith("The process cannot access the file") AndAlso
    '                ex.Message.EndsWith("because it is being used by another process."))
    '    End Try
    '    Return False
    'End Function

#End Region

End Class