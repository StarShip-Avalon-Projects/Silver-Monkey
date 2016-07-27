Imports System.Text.RegularExpressions
Imports System.IO

Public Class LogStream

    Private Const Iconfilter As String = "<img src='fsh://system.fsh:([^']*)'(.*?)/>"
    Private Const NameFilter As String = "<name shortname='([^']*)' ?(.*?)?>([\x21-\x3B\=\x3F-\x7E]+)</name>"
    Private Const URLFILTER As String = "<a href='?""?(.*?)'?""?>(.*?)</a>"
    Private Const IMGFILTER As String = "<img src='?""?(.*?)'?""? ?/?>"
#Region "logging functions"
    Private Shared strErrorFilePath As String
    Private Shared Stack As New List(Of String)
    Public Sub New(FileName As String, FilePath As String)
        strErrorFilePath = Path.Combine(FilePath, FileName & ".log")
        Try
            If Not Directory.Exists(FilePath) Then
                Directory.CreateDirectory(FilePath)
            End If
        Catch
        End Try
    End Sub

    Public Shared Function IsFileInUse(ByVal filePath As String) As Boolean
        Try
            Dim contents() As String = System.IO.File.ReadAllLines(filePath)
        Catch ex As IOException
            Return (ex.Message.StartsWith("The process cannot access the file") AndAlso
                    ex.Message.EndsWith("because it is being used by another process."))
        End Try
        Return False
    End Function

    Public Shared Sub Writeline(Message As String, ByRef ObjectException As Exception)
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

        Dim URLS As MatchCollection = Regex.Matches(Message, URLFILTER)
        For Each URL As Match In URLS
            build = build.Replace(URL.ToString, "URL:" + URL.Groups(2).Value + "(" + URL.Groups(1).Value + ")")
        Next
        Dim IMGS As MatchCollection = Regex.Matches(Message, IMGFILTER)
        For Each IMG As Match In IMGS
            build = build.Replace(IMG.ToString, "img")
        Next
        Message = build.ToString

        Dim Now As String = Date.Now().ToString("MM/dd/yyyy H:mm:ss")
        Message = Now & ": " & Message
        Dim ioFile As StreamWriter = Nothing
        Try
            If Not Directory.Exists(Path.GetDirectoryName(strErrorFilePath)) Then
                Directory.CreateDirectory(Path.GetDirectoryName(strErrorFilePath))
            End If
            If Not File.Exists(strErrorFilePath) Then
                File.Create(strErrorFilePath)
            End If
            ioFile = New StreamWriter(strErrorFilePath, True)
            For Each line In Stack.ToArray
                ioFile.WriteLine(line)
            Next
            Stack.Clear()
            ioFile.WriteLine(Message)
            ioFile.WriteLine("Error: " & ObjectException.Message)
            ioFile.WriteLine("")
            If Not ObjectException.InnerException Is Nothing Then
                ioFile.WriteLine("Inner Error: " & ObjectException.InnerException.Message)
                ioFile.WriteLine("")
            End If
            ioFile.WriteLine(ObjectException.Source)
            ioFile.WriteLine(ObjectException.StackTrace)


            ioFile.Close()
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
                ioFile.Close()
            End If
        End Try
    End Sub

    Public Shared Sub Writeline(Message As String)
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

        Dim URLS As MatchCollection = Regex.Matches(Message, URLFILTER)
        For Each URL As Match In URLS
            build = build.Replace(URL.ToString, "URL:" + URL.Groups(2).Value + "(" + URL.Groups(1).Value + ")")
        Next
        Dim IMGS As MatchCollection = Regex.Matches(Message, IMGFILTER)
        For Each IMG As Match In IMGS
            build = build.Replace(IMG.ToString, "img")
        Next
        Message = build.ToString

        Dim Now As String = Date.Now().ToString("MM/dd/yyyy H:mm:ss")
        Message = Now & ": " & Message

        If Not Directory.Exists(Path.GetDirectoryName(strErrorFilePath)) Then
            Directory.CreateDirectory(Path.GetDirectoryName(strErrorFilePath))
        End If
        If Not File.Exists(strErrorFilePath) Then
            File.Create(strErrorFilePath)
        End If

        Try
            Dim ioFile As New StreamWriter(strErrorFilePath, True)
            For Each line In Stack.ToArray
                ioFile.WriteLine(line)
            Next
            Stack.Clear()
            ioFile.WriteLine(Message)

            ioFile.Close()
        Catch ex As IOException
            If (ex.Message.StartsWith("The process cannot access the file") AndAlso
                    ex.Message.EndsWith("because it is being used by another process.")) Then
                Stack.Add(Message)
            End If
        Catch ex As Exception
            Throw New Exception("there was an error with" + strErrorFilePath, ex)
        End Try

    End Sub
#End Region
End Class
