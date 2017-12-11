Imports Monkeyspeak
Imports SilverMonkeyEngine.Engine.Libraries.MsLibHelper

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
    Public NotInheritable Class StringLibrary
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)
        End Sub

        Public Overrides Sub Initialize(ParamArray args() As Object)
            '(1:60) and variable %Variable matches wild-card expression {.} ( ""*"" or ""?""),
            Add(TriggerCategory.Condition, 60, AddressOf WildCard,
              " and variable %Variable matches wild-card expression {.} ( ""*"" or ""?""),")

            Add(TriggerCategory.Condition, 61, AddressOf NotWildCard,
             " and variable %Variable doesn't match wild-card expression {.} ( ""*"" or ""?""),")

            Add(TriggerCategory.Condition, 62, AddressOf AndVariableContains,
             " and variable %variable contains text {...},")

            Add(TriggerCategory.Condition, 63, AddressOf AndVariableNotContains,
             " and variable %variable does not contain text {...},")

            '(5:110) use variable % and take word # and put it into variable %
            Add(TriggerCategory.Effect, 120, AddressOf StringSplit,
                 " use variable %Variable and take word position # and put it into variable %Variable.")

            '(5:111) use variable % then remove character {.} and put it into variable %.
            Add(TriggerCategory.Effect, 121, AddressOf StripCharacters,
                 " use variable %Variable then remove all occurrences of character {.} and put it into variable %Variable.")

            '(5:122) chop off the beginning of variable %variable, removing the first # characters of it.
            Add(TriggerCategory.Effect, 122, AddressOf ChopStartString,
                 " chop off the beginning of variable %variable, removing the first # characters of it.")

            '(5:123) chop off the end of variable %Variable, removing the last # characters of it.
            Add(TriggerCategory.Effect, 123, AddressOf ChopEndString,
                " chop off the end of variable %Variable, removing the last # characters of it.")

            '(5:126) count the number of characters in string variable %variable and put them into variable %Variable .
            Add(TriggerCategory.Effect, 126, AddressOf CountChars,
             " count the number of characters in string variable %variable and put them into variable %Variable.")

            '(5:127) take variable %Variable and Convert it to Furcadia short name. (with out special Characters or spaces)
            Add(TriggerCategory.Effect, 127, AddressOf ToShortName,
            " take variable %Variable and convert it to Furcadia short name. (without special characters or spaces or pipe ""|"").")

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (5:123) chop off the end of variable %Variable, removing the
        ''' last # characters of it.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function ChopEndString(reader As TriggerReader) As Boolean

            Dim Count As Integer = 0

            Dim Var = reader.ReadVariable(True)
            Dim test = Integer.TryParse(ReadVariableOrNumber(reader).ToString, Count)
            Dim str = Var.Value.ToString()

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
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function ChopStartString(reader As TriggerReader) As Boolean
            Dim Count As Integer = 0

            Dim Var = reader.ReadVariable(True)
            Dim test = Integer.TryParse(ReadVariableOrNumber(reader).ToString, Count)
            Dim str = Var.Value.ToString()
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
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function CountChars(reader As TriggerReader) As Boolean

            Dim var1 = reader.ReadVariable()
            Dim var2 = reader.ReadVariable(True)
            Dim Count = Convert.ToDouble(var1.Value.ToString.Length)
            var2.Value = Count
            Return True

        End Function

        ''' <summary>
        ''' (1:61) and variable %Variable doesn't match wild-card expression
        ''' {.} ( ""*"" or ""?""),
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function NotWildCard(reader As TriggerReader) As Boolean

            Dim var = reader.ReadVariable
            Dim Pattern = reader.ReadString
            Return Not MatchWildcardString(Pattern, var.Value.ToString)

        End Function

        ''' <summary>
        ''' (5:120) use variable % and take word # and put it into variable %
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function StringSplit(reader As TriggerReader) As Boolean

            Dim Var = reader.ReadVariable()
            Dim i = ReadVariableOrNumber(reader)
            Dim NewVar = reader.ReadVariable(True)
            Dim fields() = Split(Var.Value.ToString, " ")
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
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function StripCharacters(reader As TriggerReader) As Boolean

            Dim Var = reader.ReadVariable()
            Dim ch = CChar(reader.ReadString)
            Dim NewVar = reader.ReadVariable()

            Dim varStr = Var.Value.ToString
            Dim NewStr = varStr.Replace(ch, String.Empty)
            NewVar.Value = NewStr
            Return True
        End Function

        ''' <summary>
        ''' (1:60) and variable %Variable matches wild-card expression {.} (
        ''' ""*"" or ""?""),
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function WildCard(reader As TriggerReader) As Boolean

            Dim var = reader.ReadVariable
            Dim Pattern = reader.ReadString
            Return MatchWildcardString(Pattern, var.Value.ToString)

        End Function

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' (1:62) and variable %variable contains text {...},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function AndVariableContains(reader As TriggerReader) As Boolean

            Dim VariableToCheck = reader.ReadVariable()
            Dim Argument As String
            If reader.PeekVariable Then
                Argument = reader.ReadVariable.Value
            ElseIf reader.PeekNumber Then
                Argument = reader.ReadNumber.ToString()
            Else
                Argument = reader.ReadString
            End If

            Return VariableToCheck.Value.Contains(Argument)

        End Function

        ''' <summary>
        ''' (1:63) and variable %variable does not contain text {...},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function AndVariableNotContains(reader As TriggerReader) As Boolean

            Dim VariableToCheck = reader.ReadVariable()
            Dim Argument As String
            If reader.PeekVariable Then
                Argument = reader.ReadVariable.Value
            ElseIf reader.PeekNumber Then
                Argument = reader.ReadNumber.ToString()
            Else
                Argument = reader.ReadString
            End If
            Return Not VariableToCheck.Value.Contains(Argument)

        End Function

        Private Function MatchWildcardString(pattern As String, input As String) As Boolean
            If String.Compare(pattern, input) = 0 Then
                Return True
            ElseIf String.IsNullOrEmpty(input) Then
                If String.IsNullOrEmpty(pattern.Trim(New Char() {"*"c})) Then
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
        ''' (5:127) take variable %Variable and convert it to Furcadia short
        ''' name. (without special characters or spaces or pipe ""|"").
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Private Shared Function ToShortName(reader As TriggerReader) As Boolean

            If reader.PeekVariable Then
                Dim var = reader.ReadVariable
                If String.IsNullOrEmpty(var.Value.ToString) Then
                    Return True
                End If
                var.Value = var.Value.ToString.ToFurcadiaShortName()
                Return True
            Else
                Return False
            End If

        End Function

        Public Overrides Sub Unload(page As Page)

        End Sub

#End Region

    End Class

End Namespace