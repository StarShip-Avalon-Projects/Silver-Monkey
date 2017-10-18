Imports System.IO
Imports System.Text
Imports MonkeyCore
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Create, Delete, Modify text files
    ''' <para>
    ''' Conditions (1:200) - (1:203)
    ''' </para>
    ''' <para>
    ''' Effects (5:200) - (5:125)
    ''' </para>
    ''' </summary>
    Public NotInheritable Class MsIO
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)
            ' (1:200) and the file {...} exists,
            Add(New Trigger(TriggerCategory.Condition, 200), AddressOf FileExists, " and the file {...} exists,")

            ' (1:201) and the file {...} does not exist,
            Add(New Trigger(TriggerCategory.Condition, 201), AddressOf FileNotExists, " and the file {...} does not exist,")

            ' (1:202) and the file {...} can be read from,
            Add(New Trigger(TriggerCategory.Condition, 202), AddressOf CanReadFile, " and the file {...} can be read from,")

            ' (1:203) and the file {...} can be written to,
            Add(New Trigger(TriggerCategory.Condition, 203), AddressOf CanWriteFile, " and the file {...} can be written to,")

            ' (5:200) append {...} to file {...}.
            Add(New Trigger(TriggerCategory.Effect, 200), AddressOf AppendToFile, " append {...} to file {...}.")

            ' (5:201) read from file {...} and put it into variable %Variable.
            Add(New Trigger(TriggerCategory.Effect, 201), AddressOf ReadFileIntoVariable, " read from file {...} and put it into variable %Variable.")

            ' (5:202) delete file {...}.
            Add(New Trigger(TriggerCategory.Effect, 202), AddressOf DeleteFile, " delete file {...}.")

            '(5:203) create file {...}.
            Add(New Trigger(TriggerCategory.Effect, 203), AddressOf CreateFile, " create file {...}.")

            '(5:124) read line number # from text file {...} and put it into variable %Variable.
            Add(New Trigger(TriggerCategory.Effect, 124), AddressOf ReadTextLine,
            " read line number # from text file {...} and put it into variable %Variable.")

            '(5:125) count the number of lines in text file {...} and put it into variable %Variable .
            Add(New Trigger(TriggerCategory.Effect, 125), AddressOf CountLines,
            " count the number of lines in text file {...} and put it into variable %Variable.")

        End Sub

        Public Overrides Sub Unload(page As Page)

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (5:125) count the number of lines in text file {...} and put it
        ''' into variable %Variable .
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function CountLines(reader As TriggerReader) As Boolean

            Dim FilePath = Paths.CheckBotFolder(reader.ReadString)
            Dim Var = reader.ReadVariable(True)
            Var.Value = 0.0R
            If File.Exists(FilePath) Then
                Dim test As New List(Of String)
                test.AddRange(File.ReadAllLines(FilePath))
                Var.Value = test.Count

            End If

            Return True
        End Function

        ''' <summary>
        ''' (5:124) read line number # from text file {...} and put it into
        ''' variable %Variable.
        ''' </summary>
        ''' <exception cref="FileNotFoundException">
        ''' </exception>
        ''' <exception cref="IndexOutOfRangeException">
        ''' </exception>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True
        ''' </returns>
        Shared Function ReadTextLine(reader As TriggerReader) As Boolean

            Dim num = ReadVariableOrNumber(reader, False)
            Dim F = Paths.CheckBotFolder(reader.ReadString)
            Dim var = reader.ReadVariable(True)
            If File.Exists(F) Then
                Dim lines() As String = File.ReadAllLines(F)
                If lines.Count < 0 Then
                    var = Variable.NoValue
                    Throw New MonkeySpeakException(var.Name + " Is less then the number of lines in file " + F)
                ElseIf num < lines.Count - 1 Then
                    var.Value = lines(CInt(num))
                    Return True
                Else
                    var = Variable.NoValue
                    Throw New MonkeySpeakException(var.Name + " Is larger then the number of lines in file " + F)
                End If
            Else
                Throw New MonkeySpeakException("File """ + F + """ Does not exist.")
            End If
            Return True
        End Function

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' (5:200) append {...} to file {...}."
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function AppendToFile(reader As TriggerReader) As Boolean
            Dim data = reader.ReadString()
            Dim f = Paths.CheckBotFolder(reader.ReadString())

            Using SW As New StreamWriter(f, True)
                SW.WriteLine(data)
            End Using

            Return True
        End Function

        ''' <summary>
        ''' (1:202) and the file {...} can be read from,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function CanReadFile(reader As TriggerReader) As Boolean
            Dim f = Paths.CheckBotFolder(reader.ReadString())

            Using Stream As New File.Open(f, FileMode.Open, FileAccess.Read)
                Return Stream.CanRead
            End Using

        End Function

        ''' <summary>
        ''' (1:203) and the file {...} can be written to,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function CanWriteFile(reader As TriggerReader) As Boolean
            Dim f = Paths.CheckBotFolder(reader.ReadString())

            Using stream As New File.Open(f, FileMode.Open, FileAccess.Write)
                Return stream.CanWrite
            End Using

        End Function

        ''' <summary>
        ''' (5:203) create file {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function CreateFile(reader As TriggerReader) As Boolean
            If reader.PeekString() = False Then
                Return False
            End If
            Dim f As String = Paths.CheckBotFolder(reader.ReadString())
            File.Create(f).Close()
            Return True
        End Function

        ''' <summary>
        ''' (5:202) delete file {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function DeleteFile(reader As TriggerReader) As Boolean

            If reader.PeekString() = False Then
                Return False
            End If
            Dim f = Paths.CheckBotFolder(reader.ReadString())
            If File.Exists(f) Then
                File.Delete(f)
                Return True
            End If
            Return False
        End Function

        ''' <summary>
        ''' (1:200) and the file {...} exists,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function FileExists(reader As TriggerReader) As Boolean
            Dim f As String = If(reader.PeekString(), Paths.CheckBotFolder(reader.ReadString()), "")
            Return File.Exists(f)
        End Function

        ''' <summary>
        ''' (1:201) and the file {...} does not exist,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function FileNotExists(reader As TriggerReader) As Boolean
            Return FileExists(reader) = False
        End Function

        ''' <summary>
        ''' (5:201) read from file {...} and put it into variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function ReadFileIntoVariable(reader As TriggerReader) As Boolean

            Dim OutVar = reader.ReadVariable(True)
            Dim sb = New StringBuilder()

            Using SR = New StreamReader(Paths.CheckBotFolder(reader.ReadString(True)))
                While Not SR.EndOfStream
                    sb.AppendLine(SR.ReadLine)
                End While
            End Using

            OutVar.Value = sb.ToString()
            Return True

        End Function

#End Region

    End Class

End Namespace