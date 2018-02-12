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
        Private Options As LogSteamOptions

#Region "logging functions"

        Private Shared Stack As New List(Of String)
        Private Shared strErrorFilePath As String

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
            Me.Options = options
            strErrorFilePath = Path.Combine(Me.Options.LogPath, Me.Options.GetLogName())
        End Sub

        ''' <summary>
        ''' Write a line to the log file
        ''' </summary>
        ''' <param name="Message">
        ''' </param>
        Public Sub WriteLine(Message As String)
            If Not Options.Enabled Then Exit Sub
            Dim build As New StringBuilder(Message)
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
                Using fStream = New FileStream(strErrorFilePath, FileMode.Append)
                    Using ioFile As New StreamWriter(fStream)

                        ' Try

                        For Each line In Stack.ToArray
                            ioFile.WriteLine(line)
                        Next
                        Stack.Clear()
                        ioFile.WriteLine(Message)
                    End Using
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