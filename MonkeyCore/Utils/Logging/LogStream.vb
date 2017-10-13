Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports Furcadia.Text.FurcadiaMarkup


Namespace Utils.Logging
    ''' <summary>
    ''' Log stream for normal log messages
    ''' </summary>
    <CLSCompliant(True)>
    Public Class LogStream
        Implements IDisposable
        Private Logoptions As LogSteamOptions
#Region "logging functions"

        Private Shared Stack As New List(Of String)
        Private Shared strErrorFilePath As String

        ''' <summary>
        ''' Create a new instance of the log file
        ''' </summary>
        ''' <param name="FilePath">
        ''' </param>
        Public Sub New(FilePath As String)
            Logoptions = New LogSteamOptions With {
                .LogPath = FilePath,
                .LogNameBase = "Default"
            }

            strErrorFilePath = Path.Combine(FilePath, Logoptions.GetLogName())

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="options"></param>
        Sub New(options As LogSteamOptions)
            Logoptions = options
            strErrorFilePath = Path.Combine(Logoptions.LogPath, Logoptions.GetLogName())
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
            Using ioFile As StreamWriter = New StreamWriter(strErrorFilePath, True)
                Try

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
                    Throw ex
                End Try
            End Using
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
            Try
                Using ioFile As New StreamWriter(strErrorFilePath, True)

                    ' Try

                    For Each line In Stack.ToArray
                        ioFile.WriteLine(line)
                    Next
                    Stack.Clear()
                    ioFile.WriteLine(Message)
                End Using
            Catch ex As IOException
                If (ex.Message.StartsWith("The process cannot access the file") AndAlso
                        ex.Message.EndsWith("because it is being used by another process.")) Then
                    Stack.Add(Message)
                End If
            End Try

        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Stack.Clear()
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub
#End Region



#End Region

    End Class
End Namespace