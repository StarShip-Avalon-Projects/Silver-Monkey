Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports Furcadia.Text.FurcadiaMarkup
Imports MonkeyCore2.IO

Namespace Utils.Logging

    ''' <summary>
    ''' Log stream for normal log messages
    ''' </summary>
    <CLSCompliant(True)>
    Public Class LogStream
        Implements IDisposable
        Private Shared Options As LogSteamOptions

#Region "logging functions"

        Private Shared Stack As New List(Of String)
        Private Shared strErrorFilePath As String

        ''' <summary>
        ''' Create a new instance of the log file
        ''' </summary>
        Public Sub New()
            Options = New LogSteamOptions

            strErrorFilePath = MonkeyCore2.IO.Paths.SilverMonkeyLogPath

        End Sub

        ''' <summary>
        ''' Create a new instance of the log file
        ''' </summary>
        ''' <param name="FilePath">
        ''' </param>
        Public Sub New(FilePath As String)
            Options = New LogSteamOptions With {
                .LogPath = FilePath,
                .LogNameBase = "Default"
            }

            strErrorFilePath = Path.Combine(FilePath, Options.GetLogName())

        End Sub

        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="options"></param>
        Sub New(options As LogSteamOptions)
            options = options
            strErrorFilePath = Path.Combine(options.LogPath, options.GetLogName())
        End Sub

        ''' <summary>
        ''' Write a an exception line to the log file
        ''' </summary>
        ''' <param name="Message">
        ''' </param>
        ''' <param name="ObjectException">
        ''' </param>
        Public Shared Sub WriteLine(Message As String, ByRef ObjectException As Exception)
            If Not Options.log Then Exit Sub
            Dim build As New Text.StringBuilder(Message)
            'Dim Names As MatchCollection = NameRegex.Matches(Message)
            'For Each Name As Match In Names
            '    build = build.Replace(Name.Value, Name.Groups(3).Value)
            'Next

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
                    ioFile.Write(PrintStackTrace(ObjectException, Nothing))
                Catch ex As IOException
                    If (ex.Message.StartsWith("The process cannot access the file") AndAlso
                            ex.Message.EndsWith("because it is being used by another process.")) Then
                        Stack.Add(Message)
                        Stack.AddRange(PrintStackTrace(ObjectException, Nothing).Split(CChar(Environment.NewLine)))
                        Stack.Add(ObjectException.Source)
                        Stack.Add(ObjectException.StackTrace)
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Sub

        Shared Function PrintStackTrace(ex As Exception, ObjectThrowingError As Object) As String
            Dim LogFile = New StringBuilder()
            If Not ex.InnerException Is Nothing Then
                LogFile.AppendLine("Inner Error: " & ex.InnerException.Message)
                LogFile.AppendLine("")
            End If
            LogFile.AppendLine("Source: " & ObjectThrowingError.ToString)
            LogFile.AppendLine("")
            Dim st As New StackTrace(ex, True)
            LogFile.AppendLine("-------------------------------------------------------")

            LogFile.AppendLine("Stack Trace: " & st.ToString())
            LogFile.AppendLine("")
            LogFile.AppendLine("-------------------------------------------------------")
            If Not ex.InnerException Is Nothing Then
                Dim stInner As New StackTrace(ex.InnerException, True)
                LogFile.AppendLine("Inner Stack Trace: " & stInner.ToString())
                LogFile.AppendLine("")
                LogFile.AppendLine("-------------------------------------------------------")
            End If
            Return LogFile.ToString
        End Function

        ''' <summary>
        ''' Write a line to the log file
        ''' </summary>
        ''' <param name="Message">
        ''' </param>
        Public Shared Sub WriteLine(Message As String)
            If Not Options.log Then Exit Sub
            Dim build As New StringBuilder(Message)
            'Dim Names As MatchCollection = NameRegex.Matches(Message)
            'For Each Name As Match In Names
            '    build = build.Replace(Name.Value, Name.Groups(3).Value)
            'Next
            '<name shortname='acuara' forced>
            Dim MyIcon As MatchCollection = Regex.Matches(Message, Iconfilter)

            For Each Icon As Match In MyIcon
                build = build.Replace(Icon.ToString, "[" + Icon.Groups(1).Value + "]")
            Next

            Dim URLS As MatchCollection = Regex.Matches(Message, UrlFilter)
            For Each URL As Match In URLS
                build = build.Replace(URL.ToString, "URL:" + URL.Groups(2).Value + "(" + URL.Groups(1).Value + ")")
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

        End Sub

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Stack.Clear()
                End If

            End If
            disposedValue = True
        End Sub

#End Region

#End Region

    End Class

End Namespace