Imports System.IO

''' <summary>
''' trouble shooting Class for tracing what functions do
''' </summary>
Public Class Logger
    Private strErrorFilePath As String
    Private Stack As New List(Of String)

    ''' <summary>
    ''' Constructor
    ''' <para>Sets the name of the log file for the duration of the instance</para>
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <param name="message"> First string to prime the log with</param>
    Public Sub New(Name As String, message As String)
        'Call Log Error
        strErrorFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Silver Monkey\Log\" & Name & Date.Now().ToString("MM_dd_yyyy_H-mm-ss") & ".txt"
        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Silver Monkey\Log\")
        LogMessage(message)
    End Sub

    ''' <summary>
    ''' Checks to see if a file is in use
    ''' </summary>
    ''' <param name="filePath"></param>
    ''' <returns></returns>
    Private Function IsFileInUse(ByVal filePath As String) As Boolean
        Try
            Dim contents() As String = File.ReadAllLines(filePath)
        Catch ex As IOException
            Return (ex.Message.StartsWith("The process cannot access the file") AndAlso
                    ex.Message.EndsWith("because it is being used by another process."))
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Sends a string to the log file
    ''' </summary>
    ''' <param name="Message"></param>
    Public Sub LogMessage(Message As String)
        Using ioFile As StreamWriter = New StreamWriter(strErrorFilePath, True)
            Try
                For Each line In Stack.ToArray
                    ioFile.WriteLine(line)
                Next
                Stack.Clear()
                ioFile.WriteLine(Message)

            Catch ex As IOException
                If (ex.Message.StartsWith("The process cannot access the file") AndAlso
                        ex.Message.EndsWith("because it is being used by another process.")) Then
                    Stack.Add(Message)
                End If

            Finally
                If Not IsNothing(ioFile) Then
                    ioFile.Close()
                End If
            End Try
        End Using
    End Sub

End Class