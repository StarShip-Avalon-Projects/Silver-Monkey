Imports Monkeyspeak

Imports System.Diagnostics
Imports MonkeyCore

Public Class Warning
    Inherits Libraries.AbstractBaseLibrary
    Private writer As TextBoxWriter = Nothing
    Dim Lock As New Object()

    Public Sub New()
        writer = New TextBoxWriter(Variables.TextBox1)

        '(0:800) When the bot sees error message {...},
        Add(TriggerCategory.Cause, 800,
AddressOf ErrorIs, "(0:800) When the bot sees error message {...},")

        '(0:801) when the bot sees warning message{...},
        Add(TriggerCategory.Cause, 801,
AddressOf ErrorIs, "(0:801) when the bot sees warning message{...},")

        '(1:800) and the last command sent returned an error or warning # (zero = none, one = warning, two = error)
        Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 800), AddressOf CommandWariningOrError,
"(1:800) and the last command sent returned an error or warning # (zero = none, one = warning, two = error)")

        '(1:801) and the last command sent didn't return an error or warning # (zero = none, one = warning, two = error)
        Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 801), AddressOf CommandNotWariningOrError,
"(1:801) and the last command sent didn't return an error or warning # (zero = none, one = warning, two = error)")

        '(5:800) set %Variable to the value of the message returned by the last command line. (zero = none, one = warning, two = error)
        Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 800), AddressOf CommandNotWariningOrError,
"(5:800) set %Variable to the value of the message returned by the last command line. (zero = none, one = warning, two = error)")


    End Sub

    '(0:800) When the bot sees error message {...},
    '(0:801) when the bot sees warning message{...},
    Function ErrorIs(reader As TriggerReader) As Boolean
        Try
            Dim TmpName As String = reader.ReadString()
            Dim errstr As String = ""
            SyncLock Lock
                errstr = callbk.ErrorMsg
            End SyncLock
            'add Machine Name parser
            Return TmpName = errstr
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
    End Function


    '(1:800) and the last command sent returned an error or warning # (zero = none, one = warning, two = error)
    Function CommandWariningOrError(reader As TriggerReader) As Boolean
        Try
            Dim err As Double = ReadVariableOrNumber(reader, False)
            Dim errNum As Short = 0
            SyncLock Lock
                errNum = callbk.ErrorNum
            End SyncLock
            'add Machine Name parser
            Return Convert.ToDouble(errNum) = err
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
    End Function

    '(1:801) and the last command sent didn't return an error or warning # (zero = none, one = warning, two = error)
    Function CommandNotWariningOrError(reader As TriggerReader) As Boolean
        Try
            Dim err As Double = ReadVariableOrNumber(reader, False)
            Dim errNum As Short = 0
            SyncLock Lock
                errNum = callbk.ErrorNum
            End SyncLock
            'add Machine Name parser
            Return Convert.ToDouble(errNum) <> err
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
    End Function

    '(5:800) set %Variable to the value of the message returned by the last command line. (zero = none, one = warning, two = error)
    Function WarningVar(reader As TriggerReader) As Boolean
        Try
            Dim Var As Monkeyspeak.Variable = reader.ReadVariable(True)
            Dim errNum As Short = 0
            SyncLock Lock
                errNum = callbk.ErrorNum
            End SyncLock
            'add Machine Name parser
            Var.Value = Convert.ToDouble(errNum)
            Return True
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
    End Function

End Class
