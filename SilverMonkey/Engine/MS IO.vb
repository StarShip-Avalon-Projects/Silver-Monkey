Imports System.Linq
Imports System.Text
Imports MonkeyCore.IO
Imports Monkeyspeak
Imports Monkeyspeak.Libraries
Imports MonkeyCore
Imports System.Collections.Generic

Friend Class MS_IO
    Inherits AbstractBaseLibrary

    Private writer As TextBoxWriter = Nothing

    Public Sub New()
        writer = New TextBoxWriter(Variables.TextBox1)
        ' (1:200) and the file {...} exists,
        Add(New Trigger(TriggerCategory.Condition, 200), AddressOf FileExists, "(1:200) and the file {...} exists,")

        ' (1:201) and the file {...} does not exist,
        Add(New Trigger(TriggerCategory.Condition, 201), AddressOf FileNotExists, "(1:201) and the file {...} does not exist,")

        ' (1:202) and the file {...} can be read from,
        Add(New Trigger(TriggerCategory.Condition, 202), AddressOf CanReadFile, "(1:202) and the file {...} can be read from,")

        ' (1:203) and the file {...} can be written to,
        Add(New Trigger(TriggerCategory.Condition, 203), AddressOf CanWriteFile, "(1:203) and the file {...} can be written to,")

        ' (5:200) append {...} to file {...}.
        Add(New Trigger(TriggerCategory.Effect, 200), AddressOf AppendToFile, "(5:200) append {...} to file {...}.")

        ' (5:201) read from file {...} and put it into variable %Variable.
        Add(New Trigger(TriggerCategory.Effect, 201), AddressOf ReadFileIntoVariable, "(5:201) read from file {...} and put it into variable %Variable.")

        ' (5:202) delete file {...}.
        Add(New Trigger(TriggerCategory.Effect, 202), AddressOf DeleteFile, "(5:202) delete file {...}.")

        '(5:203) create file {...}.
        Add(New Trigger(TriggerCategory.Effect, 203), AddressOf CreateFile, "(5:203) create file {...}.")

        '(5:124) read line number # from text file {...} and put it into variable %Variable.
        Add(New Trigger(TriggerCategory.Effect, 124), AddressOf ReadTextLine,
            "(5:124) read line number # from text file {...} and put it into variable %Variable.")

        '(5:125) count the number of lines in text file {...} and put it into variable %Variable .
        Add(New Trigger(TriggerCategory.Effect, 125), AddressOf CountLines,
            "(5:125) count the number of lines in text file {...} and put it into variable %Variable.")

    End Sub

    Private Function FileExists(reader As TriggerReader) As Boolean
        Dim f As String = If((reader.PeekString()), Paths.CheckBotFolder(reader.ReadString()), "")
        Return File.Exists(f)
    End Function

    Private Function FileNotExists(reader As TriggerReader) As Boolean
        Return FileExists(reader) = False
    End Function

    Private Function CanReadFile(reader As TriggerReader) As Boolean
        Dim f As String = Paths.CheckBotFolder(reader.ReadString())
        Try
            Using stream As FileStream = File.Open(f, FileMode.Open, FileAccess.Read)
                Return stream.CanRead
            End Using
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
    End Function

    Private Function CanWriteFile(reader As TriggerReader) As Boolean
        Dim f As String = Paths.CheckBotFolder(reader.ReadString())
        Try
            Using stream As FileStream = File.Open(f, FileMode.Open, FileAccess.Write)
                Return stream.CanWrite
            End Using
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
    End Function

    Private Function AppendToFile(reader As TriggerReader) As Boolean
        Dim data As String = reader.ReadString()
        Dim f As String = Paths.CheckBotFolder(reader.ReadString())

        Try
            Using SW As StreamWriter = New StreamWriter(f, True)
                SW.WriteLine(data)
            End Using
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    Private Function ReadFileIntoVariable(reader As TriggerReader) As Boolean
        Try
            Dim f As String = Paths.CheckBotFolder(reader.ReadString(True))
            Dim var As Variable = reader.ReadVariable(True)
            Dim sb As New StringBuilder()
            Using stream As FileStream = File.Open(f, FileMode.Open, FileAccess.Read)
                Using SR As StreamReader = New StreamReader(stream)
                    sb.AppendLine(SR.ReadToEnd)
                End Using
            End Using
            var.Value = sb.ToString()
            Return True
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
    End Function

    Private Function DeleteFile(reader As TriggerReader) As Boolean
        Try
            If reader.PeekString() = False Then
                Return False
            End If
            Dim f As String = Paths.CheckBotFolder(reader.ReadString())
            File.Delete(f)
            Return True
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
    End Function

    Private Function CreateFile(reader As TriggerReader) As Boolean
        If reader.PeekString() = False Then
            Return False
        End If
        Dim f As String = Paths.CheckBotFolder(reader.ReadString())
        File.Create(f).Close()
        Return True
    End Function

    '(5:124) read line number # from text file {...} and put it into variable %Variable.
    Function ReadTextLine(reader As TriggerReader) As Boolean
        Try
            Dim num As Double = ReadVariableOrNumber(reader, False)
            Dim F As String = Paths.CheckBotFolder(reader.ReadString)
            Dim var As Variable = reader.ReadVariable(True)
            If File.Exists(F) Then
                Dim lines() As String = File.ReadAllLines(F)
                If lines.Count < 0 Then
                    var = Variable.NoValue
                    Throw New IndexOutOfRangeException(var.Name + " Is less then the number of lines in file " + F)
                ElseIf num < lines.Count - 1 Then
                    var.Value = lines(CInt(num))
                    Return True
                Else
                    var = Variable.NoValue
                    Throw New IndexOutOfRangeException(var.Name + " Is larger then the number of lines in file " + F)
                End If
            Else
                Throw New Exception("File """ + F + """ Does not exist.")
            End If

        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
    End Function

    '(5:125) count the number of lines in text file {...} and put it into variable %Variable .
    Public Function CountLines(reader As TriggerReader) As Boolean
        Dim F As String = ""
        Dim Var As Variable
        Dim count As Double = 0
        Try
            F = Paths.CheckBotFolder(reader.ReadString)
            Var = reader.ReadVariable(True)
            Var.Value = 0.0R
            If File.Exists(F) Then
                Dim test As New List(Of String)
                test.AddRange(File.ReadAllLines(F))
                Var.Value = test.Count.ToString()

            End If
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

End Class