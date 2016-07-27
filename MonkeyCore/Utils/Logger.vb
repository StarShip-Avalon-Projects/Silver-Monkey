Imports System.IO

Public Class Logger
    Dim strErrorFilePath As String
    Dim Stack As New List(Of String)
    Public Sub New(Name As String, message As String)
        'Call Log Error
        strErrorFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Silver Monkey\Log\" & Name & Date.Now().ToString("MM_dd_yyyy_H-mm-ss") & ".txt"
        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Silver Monkey\Log\")
        LogMessage(message)
    End Sub
    Public Function IsFileInUse(ByVal filePath As String) As Boolean
        Try
            Dim contents() As String = File.ReadAllLines(filePath)
        Catch ex As IOException
            Return (ex.Message.StartsWith("The process cannot access the file") AndAlso _
                    ex.Message.EndsWith("because it is being used by another process."))
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    Public Sub LogMessage(Message As String)
        Using ioFile As StreamWriter = New StreamWriter(strErrorFilePath, True)
            Try
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

            Catch exLog As Exception
                If Not IsNothing(ioFile) Then
                    ioFile.Close()
                End If
            End Try
        End Using
    End Sub





End Class

