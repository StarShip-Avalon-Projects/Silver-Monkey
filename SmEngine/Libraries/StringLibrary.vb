﻿Imports Furcadia.Util
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' <para>
    ''' Conditions: (1:60) - (1:61)
    ''' </para>
    ''' <para>
    ''' Effects: (5:120)- (5:127)
    ''' </para>
    ''' Tied width a string!!
    ''' <para>
    ''' well not really but, this class extends monkey Speak ability to work
    ''' with strings in <see cref="Monkeyspeak.Variable"/> s
    ''' </para>
    ''' </summary>
    Class StringLibrary
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)

            '(1:60) and variable %Variable matches wild-card expression {.} ( ""*"" or ""?""),
            Add(New Trigger(TriggerCategory.Condition, 60), AddressOf WildCard,
              "(1:60) and variable %Variable matches wild-card expression {.} ( ""*"" or ""?""),")

            Add(New Trigger(TriggerCategory.Condition, 61), AddressOf NotWildCard,
             "(1:61) and variable %Variable doesn't match wild-card expression {.} ( ""*"" or ""?""),")

            '(5:110) use variable % and take word # and put it into variable %
            Add(New Trigger(TriggerCategory.Effect, 120), AddressOf StringSplit,
                 "(5:120) use variable %Variable and take word position # and put it into variable %Variable.")

            '(5:111) use variable % then remove character {.} and put it into variable %.
            Add(New Trigger(TriggerCategory.Effect, 121), AddressOf StripCharacters,
                 "(5:121) use variable %Variable then remove all occurrences of character {.} and put it into variable %Variable.")

            '(5:122) chop off the beginning of variable %variable, removing the first # characters of it.
            Add(New Trigger(TriggerCategory.Effect, 122), AddressOf ChopStartString,
                 "(5:122) chop off the beginning of variable %variable, removing the first # characters of it.")

            '(5:123) chop off the end of variable %Variable, removing the last # characters of it.
            Add(New Trigger(TriggerCategory.Effect, 123), AddressOf ChopEndString,
                "(5:123) chop off the end of variable %Variable, removing the last # characters of it.")

            '(5:126) count the number of characters in string variable %variable and put them into variable %Variable .
            Add(New Trigger(TriggerCategory.Effect, 126), AddressOf CountChars,
             "(5:126) count the number of characters in string variable %variable and put them into variable %Variable.")

            '(5:127) take variable %Variable and Convert it to Furcadia short name. (with out special Characters or spaces)
            Add(New Trigger(TriggerCategory.Effect, 127), AddressOf ToShortName,
            "(5:127) take variable %Variable and convert it to Furcadia short name. (without special characters or spaces or pipe ""|"").")

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (5:123) chop off the end of variable %Variable, removing the
        ''' last # characters of it.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function ChopEndString(reader As TriggerReader) As Boolean
            Dim Var As Variable
            Dim Count As Integer = 0

            Var = reader.ReadVariable(True)
            Dim test As Boolean = Integer.TryParse(ReadVariableOrNumber(reader).ToString, Count)
            Dim str As String = Var.Value.ToString()

            If str.Length < Count Then
                Var.Value = str
            Else
                Var.Value = str.Substring(0, str.Length - Count)
            End If

            Return True

        End Function

        ''' <summary>
        ''' (5:122) chop off the beginning of variable %variable, removing
        ''' the first # characters of it.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function ChopStartString(reader As TriggerReader) As Boolean
            Dim Var As Variable
            Dim Count As Integer = 0

            Var = reader.ReadVariable(True)
            Dim test As Boolean = Integer.TryParse(ReadVariableOrNumber(reader).ToString, Count)
            Dim str As String = Var.Value.ToString()
            If str.Length < Count Then
                Var.Value = Nothing
            Else
                Var.Value = str.Substring(Count)
            End If
            Return True

        End Function

        ''' <summary>
        ''' (5:126) count the number of characters in string variable
        ''' %variable and put them into variable %Variable .
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function CountChars(reader As TriggerReader) As Boolean

            Dim var1 As Variable = reader.ReadVariable()
            Dim var2 As Variable = reader.ReadVariable(True)
            Dim Count As Double = Convert.ToDouble(var1.Value.ToString.Length)
            var2.Value = Count
            Return True

        End Function

        Public Function MatchWildcardString(pattern As String, input As String) As [Boolean]
            If [String].Compare(pattern, input) = 0 Then
                Return True
            ElseIf [String].IsNullOrEmpty(input) Then
                If [String].IsNullOrEmpty(pattern.Trim(New [Char](0) {"*"c})) Then
                    Return True
                Else
                    Return False
                End If
            ElseIf pattern.Length = 0 Then
                Return False
            ElseIf pattern(0) = "?"c Then
                Return MatchWildcardString(pattern.Substring(1), input.Substring(1))
            ElseIf pattern(pattern.Length - 1) = "?"c Then
                Return MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input.Substring(0, input.Length - 1))
            ElseIf pattern(0) = "*"c Then
                If MatchWildcardString(pattern.Substring(1), input) Then
                    Return True
                Else
                    Return MatchWildcardString(pattern, input.Substring(1))
                End If
            ElseIf pattern(pattern.Length - 1) = "*"c Then
                If MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input) Then
                    Return True
                Else
                    Return MatchWildcardString(pattern, input.Substring(0, input.Length - 1))
                End If
            ElseIf pattern(0) = input(0) Then
                Return MatchWildcardString(pattern.Substring(1), input.Substring(1))
            End If
            Return False
        End Function

        ''' <summary>
        ''' (1:61) and variable %Variable doesn't match wild-card expression
        ''' {.} ( ""*"" or ""?""),
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function NotWildCard(reader As TriggerReader) As Boolean

            Dim var As Variable = reader.ReadVariable
            Dim Pattern As String = reader.ReadString
            Return Not MatchWildcardString(Pattern, var.Value.ToString)

        End Function

        ''' <summary>
        ''' (5:120) use variable % and take word # and put it into variable %
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function StringSplit(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable()
            Dim i As Double = ReadVariableOrNumber(reader)
            Dim NewVar As Variable = reader.ReadVariable(True)
            Dim fields() As String = Split(Var.Value.ToString, " ")
            If i < fields.Length Then
                NewVar.Value = fields(i)
            End If
            Return True

        End Function

        ''' <summary>
        ''' (5:121) use variable % then remove character {.} and put it into
        ''' variable %.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function StripCharacters(reader As TriggerReader) As Boolean
            Dim ch As Char = Nothing
            Dim NewVar As Variable
            Dim Var As Variable

            Var = reader.ReadVariable()
            ch = CChar(reader.ReadString)
            NewVar = reader.ReadVariable()

            Dim varStr As String = Var.Value.ToString
            Dim NewStr As String = varStr.Replace(ch, String.Empty)
            NewVar.Value = NewStr
            Return True
        End Function

        ''' <summary>
        ''' (1:60) and variable %Variable matches wild-card expression {.} (
        ''' ""*"" or ""?""),
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function WildCard(reader As TriggerReader) As Boolean

            Dim var As Variable = reader.ReadVariable
            Dim Pattern As String = reader.ReadString
            Return MatchWildcardString(Pattern, var.Value.ToString)

        End Function

#End Region

#Region "Private Methods"

        Private Function ToShortName(reader As TriggerReader) As Boolean

            If reader.PeekVariable Then
                Dim var As Variable = reader.ReadVariable
                If String.IsNullOrEmpty(var.Value.ToString) Then
                    Return True
                End If
                var.Value = FurcadiaShortName(var.Value.ToString)
                Return True
            Else
                Return False
            End If

        End Function

#End Region

    End Class

End Namespace