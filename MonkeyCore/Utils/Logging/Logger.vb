Imports System.IO

Namespace Utils.Logging

    ''' <summary>
    ''' trouble shooting Class for tracing what functions do
    ''' </summary>
    Public Class Logger

#Region "Private Fields"

        Private Stack As New List(Of String)
        Private strErrorFilePath As String

#End Region

#Region "Public Constructors"

        ''' <summary>
        ''' Constructor
        ''' <para>
        ''' Sets the name of the log file for the duration of the instance
        ''' </para>
        ''' </summary>
        ''' <param name="Name">
        ''' </param>
        ''' <param name="message">
        ''' First string to prime the log with
        ''' </param>
        Public Sub New(Name As String, message As String)
            'Call Log Error
            strErrorFilePath = Path.Combine(Paths.SilverMonkeyLogPath, Name & Date.Now().ToString("MM_dd_yyyy_H-mm-ss") & ".txt")
            If Not Directory.Exists(Paths.SilverMonkeyLogPath) Then
                Directory.CreateDirectory(Paths.SilverMonkeyLogPath)
            End If
            LogMessage(message)
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Sends a string to the log file
        ''' </summary>
        ''' <param name="Message">
        ''' </param>
        Public Sub LogMessage(Message As String)
            Using LogFile As StreamWriter = New StreamWriter(strErrorFilePath, True)
                Try
                    For Each line In Stack.ToArray
                        LogFile.WriteLine(line)
                    Next
                    Stack.Clear()
                    LogFile.WriteLine(Message)
                Catch ex As IOException
                    If (ex.Message.StartsWith("The process cannot access the file") AndAlso
                        ex.Message.EndsWith("because it is being used by another process.")) Then
                        Stack.Add(Message)
                    End If
                End Try
            End Using
        End Sub

#End Region

    End Class

End Namespace