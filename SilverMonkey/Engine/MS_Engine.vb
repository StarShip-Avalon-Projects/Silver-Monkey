Imports Monkeyspeak
Imports Furcadia.Net
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.Diagnostics
Imports MonkeyCore

'StringLibrary
'(1:108) - (1:111)
'(5:110) - (5:119)
'MathLibrary

'SayLibrary


''' <summary>
''' Modules Based on SilverMonkey VB Example
''' Modified by Gerolkae
''' Script Loader Moved to SilverMonkey.Main
''' </summary>
Module MS_Engine
    Public callbk As Main
    Public MainMSEngine As MainMSEngine

    Public Structure ViewArea
        Public X As Integer
        Public Y As Integer
        Public length As Integer
        Public height As Integer
    End Structure

    ''' <summary>
    ''' Helper extension method
    ''' </summary>
    ''' <param name="reader"></param>
    ''' <param name="addIfNotExist"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Runtime.CompilerServices.Extension()> _
    Public Function ReadVariableOrNumber(ByVal reader As Monkeyspeak.TriggerReader, Optional addIfNotExist As Boolean = False) As Double
        Dim result As Double = 0
        If reader.PeekVariable Then
            Dim value As String = reader.ReadVariable(addIfNotExist).Value.ToString
            Double.TryParse(value, result)
        ElseIf reader.PeekNumber Then
            result = reader.ReadNumber
        End If
        Return result
    End Function

    'Reference http://pastebin.com/QbnjwjNc
    Public Function getTargetRectFromCenterCoord(ByRef X As Integer, ByRef Y As Integer) As ViewArea
        ' Set the size of the rectangle drawn around the player.
        ' The +1 is the tile the player is on.
        Dim rec As ViewArea
        'Dim tilesWide As UInt32 = extraTilesLeft + 7 + 1 + 7 + extraTilesRight
        'Dim tilesHigh As UInt32 = extraTilesTop + 8 + 1 + 8 + extraTilesBottom
        ' NB: these lines *look* similar, but the numbers are for *completely* different reasons!
        'tilesWide = tilesWide * 2 ' * 2 as all X values are even: we count 0, 2, 4...
        'tilesHigh = tilesHigh * 2 ' * 2 as zig-zaggy vertical cols can fit twice as many tiles to a column

        ' Set where in the map our visible (0,0) will be.
        Dim XoddOffset As Integer = 2
        Dim YoddOffset As Integer = 0
        If IsOdd(Y) Then
            XoddOffset = 0
            YoddOffset = 1
        End If
        rec.X = X - 8 + XoddOffset
        rec.Y = Y - 8 - 1 ' 1 for the tile the user is in.
        rec.length = rec.X + 14
        rec.height = rec.Y + 17 + YoddOffset
        Return rec
    End Function



    Class StringLibrary
        Inherits Libraries.AbstractBaseLibrary
        Private writer As TextBoxWriter = Nothing
        Dim FurreList As Dictionary(Of UInteger, FURRE) = DREAM.List
        Public Sub New()
            writer = New TextBoxWriter(Variables.TextBox1)
            '(1:60) and variable %Variable matches wildcard expression {...} ( ""*"" or ""?""),
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 60), AddressOf WildCard,
 "(1:60) and variable %Variable matches wildcard expression {...} ( ""*"" or ""?""),")
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 61), AddressOf NotWildCard,
 "(1:61) and variable %Variable doesn't match wildcard expression {...} ( ""*"" or ""?""),")

            '(5:110) use variable % and take word # and put it into variable %
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 120), AddressOf StringSplit,
             "(5:120) use variable %Variable and take word position # and put it into variable %Variable.")
            '(5:111) use variable % then remove character {...} and put it into variable %.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 121), AddressOf StripCharacters,
 "(5:121) use variable %Variable then remove all occurrences of character {...} and put it into variable %Variable.")
            '(5:122) chop off the beginning of variable %variable, removing the first # characters of it.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 122), AddressOf ChopStartString,
"(5:122) chop off the beginning of variable %variable, removing the first # characters of it.")
            '(5:123) chop off the end of variable %Variable, removing the last # characters of it.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 123), AddressOf ChopEndString,
           "(5:123) chop off the end of variable %Variable, removing the last # characters of it.")

            '(5:126) count the number of characters in string variable %variable and put them into variable %Variable .
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 126), AddressOf CountChars,
  "(5:126) count the number of characters in string variable %variable and put them into variable %Variable.")

            '(5:127) take variable %Variable and Convert it to Furcadia short name. (with out special Characters or spaces)
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 127), AddressOf ToShortName,
"(5:127) take variable %Variable and convert it to Furcadia short name. (without special characters or spaces or pipe ""|"").")


        End Sub
        '(1:60) and variable %Variable matches wildcard expression {...} ( ""*"" or ""?""),
        Function WildCard(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                Dim var As Monkeyspeak.Variable = reader.ReadVariable
                Dim Pattern As String = reader.ReadString
                Return MatchWildcardString(Pattern, var.Value.ToString)
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

        '(1:61) and variable %Variable doesn't match wildcard expression {...} ( ""*"" or ""?""),
        Function NotWildCard(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                Dim var As Monkeyspeak.Variable = reader.ReadVariable
                Dim Pattern As String = reader.ReadString
                Return Not MatchWildcardString(Pattern, var.Value.ToString)
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

        '(5:120) use variable % and take word # and put it into variable %
        Function StringSplit(reader As TriggerReader) As Boolean
            Try
                Dim Var As Variable = reader.ReadVariable()
                Dim i As Double = ReadVariableOrNumber(reader)
                Dim NewVar As Variable = reader.ReadVariable(True)
                Dim fields() As String = MiscUtils.Split(Var.Value.ToString, " ", """", True)
                If i < fields.Length Then
                    NewVar.Value = fields(CInt(i))
                End If
                Return True
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString.ToString
                Dim tCat As String = reader.TriggerCategory.ToString.ToString
                Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
        End Function
        '(5:121) use variable % then remove character {...} and put it into variable %.
        Public Function StripCharacters(reader As Monkeyspeak.TriggerReader) As Boolean
            Dim ch As Char = Nothing
            Dim NewVar As Variable
            Dim Var As Variable
            Try
                Var = reader.ReadVariable()
                ch = CChar(reader.ReadString)
                NewVar = reader.ReadVariable()
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
            Dim varStr As String = Var.Value.ToString
            Dim NewStr As String = varStr.Replace(ch, "")
            NewVar.Value = NewStr
            Return True
        End Function

        '(5:122) chop off the beginning of variable %variable, removing the first # characters of it.
        Public Function ChopStartString(reader As Monkeyspeak.TriggerReader) As Boolean
            Dim Var As Variable
            Dim Count As Integer = 0
            Try
                Var = reader.ReadVariable(True)
                Dim test As Boolean = Integer.TryParse(ReadVariableOrNumber(reader).ToString, Count)
                Dim str As String = Var.Value.ToString()
                If str.Length < Count Then
                    Var.Value = Nothing
                Else
                    Var.Value = str.Substring(Count)
                End If
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
        '(5:123) chop off the end of variable %Variable, removing the last # characters of it.
        Public Function ChopEndString(reader As Monkeyspeak.TriggerReader) As Boolean
            Dim Var As Variable
            Dim Count As Integer = 0
            Try
                Var = reader.ReadVariable(True)
                Dim test As Boolean = Integer.TryParse(ReadVariableOrNumber(reader).ToString, Count)
                Dim str As String = Var.Value.ToString()

                If str.Length < Count Then
                    Var.Value = str
                Else
                    Var.Value = str.Substring(0, str.Length - Count)
                End If

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



        '(5:126) count the number of characters in string variable %variable and put them into variable %Variable .
        Public Function CountChars(reader As Monkeyspeak.TriggerReader) As Boolean

            Try

                Dim var1 As Monkeyspeak.Variable = reader.ReadVariable()
                Dim var2 As Monkeyspeak.Variable = reader.ReadVariable(True)
                Dim Count As Double = Convert.ToDouble(var1.Value.ToString.Length)
                var2.Value = Count
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

        Private Function ToShortName(reader As TriggerReader) As Boolean
            Try
                If reader.PeekVariable Then
                    Dim var As Monkeyspeak.Variable = reader.ReadVariable
                    If String.IsNullOrEmpty(var.Value.ToString) Then
                        Return True
                    End If
                    var.Value = var.Value.ToString.ToFurcShortName
                    Return True
                Else
                    Return False
                End If
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


    Class SayLibrary
        Inherits Libraries.AbstractBaseLibrary
        Private writer As TextBoxWriter = Nothing

        Public Sub New()
            writer = New TextBoxWriter(Variables.TextBox1)
            Add(TriggerCategory.Cause, 1,
                Function()
                    Return True
                End Function, "(0:1) When the bot logs into furcadia,")
            Add(TriggerCategory.Cause, 2,
                Function()
                    Return True
                End Function, "(0:2) When the bot logs out of furcadia,")
            Add(TriggerCategory.Cause, 3,
                Function()
                    Return True
                End Function, "(0:3) When the Furcadia client disconnects or closes,")

            'says
            Add(TriggerCategory.Cause, 5,
                      Function()
                          '       Console.WriteLine("Cause (0:5):")
                          Return Not callbk.IsBot(callbk.Player)
                      End Function,
                   "(0:5) When someone says something,")
            Add(TriggerCategory.Cause, 6,
                 AddressOf msgIs, "(0:6) When someone says {...},")

            '(0:7) When some one says something with {...} in it
            Add(TriggerCategory.Cause, 7,
                 AddressOf msgContains, "(0:7) When someone says something with {...} in it,")

            'Shouts
            Add(TriggerCategory.Cause, 8,
           Function()
               Return Not callbk.IsBot(callbk.Player)
           End Function,
        "(0:8) When someone shouts something,")
            Add(TriggerCategory.Cause, 9,
                 AddressOf msgIs, "(0:9) When someone shouts {...},")

            '(0:10) When some one shouts something with {...} in it
            Add(TriggerCategory.Cause, 10,
                 AddressOf msgContains, "(0:10) When someone shouts something with {...} in it,")

            'emotes
            Add(TriggerCategory.Cause, 11,
           Function()
               Return Not callbk.IsBot(callbk.Player)
           End Function,
        "(0:11) When someone emotes something,")
            Add(TriggerCategory.Cause, 12,
                 AddressOf msgIs, "(0:12) When someone emotes {...},")

            '(0:13) When some one emotes something with {...} in it
            Add(TriggerCategory.Cause, 13,
                 AddressOf msgContains, "(0:13) When someone emotes something with {...} in it,")

            'Whispers
            Add(TriggerCategory.Cause, 15,
           Function()
               Return Not callbk.IsBot(callbk.Player)
           End Function,
        "(0:15) When someone whispers something,")
            Add(TriggerCategory.Cause, 16,
                 AddressOf msgIs, "(0:16) When someone whispers {...},")

            '(0:13) When some one emotes something with {...} in it
            Add(TriggerCategory.Cause, 17,
                 AddressOf msgContains, "(0:17) When someone whispers something with {...} in it,")

            'Says or Emotes
            Add(TriggerCategory.Cause, 18,
                Function()
                    Return Not callbk.IsBot(callbk.Player)
                End Function, "(0:18) When someone says or emotes something,")
            Add(TriggerCategory.Cause, 19,
                 AddressOf msgIs, "(0:19) When someone says or emotes {...},")

            '(0:13) When some one emotes something with {...} in it
            Add(TriggerCategory.Cause, 20,
                 AddressOf msgContains, "(0:20) When someone says or emotes something with {...} in it,")

            'Emits
            Add(TriggerCategory.Cause, 21,
                Function()
                    Return Not callbk.IsBot(callbk.Player)
                End Function, "(0:21) When someone emits something,")
            Add(TriggerCategory.Cause, 22,
                 AddressOf msgIs, "(0:22) When someone emits {...},")

            '(0:13) When some one emotes something with {...} in it
            Add(TriggerCategory.Cause, 23,
                 AddressOf msgContains, "(0:23) When someone emits something with {...} in it,")

            'Furre Enters
            '(0:4) When someone is added to the dream manifest,   
            Add(TriggerCategory.Cause, 24,
                Function()

                    Return True
                End Function, "(0:24) When someone enters the dream,")
            '(0:25) When a furre Named {...} enters the dream,
            Add(TriggerCategory.Cause, 25,
                AddressOf NameIs, "(0:25) When a furre Named {...} enters the dream,")
            'Furre Leaves
            '(0:25) When someone leaves the dream,
            Add(TriggerCategory.Cause, 26,
                Function()
                    Return True
                End Function, "(0:26) When someone leaves the dream, ")
            '(0:27) When a furre named {...} leaves the dream,
            Add(TriggerCategory.Cause, 27,
                AddressOf NameIs, "(0:27) When a furre named {...} leaves the dream")

            'Furre In View
            '(0:28) When someone enters the bots view,
            Add(TriggerCategory.Cause, 28,
                AddressOf EnterView, "(0:28) When someone enters the bots view, ")
            '(0:28) When a furre named {...} enters the bots view
            Add(TriggerCategory.Cause, 29,
                AddressOf FurreNamedEnterView, "(0:29) When a furre named {...} enters the bots view")
            'Furre Leave View
            '(0:30) When someone leaves the bots view,
            Add(TriggerCategory.Cause, 30,
                AddressOf LeaveView, "(0:30) When someone leaves the bots view, ")
            '(0:31) When a furre named {...} leaves the bots view
            Add(TriggerCategory.Cause, 31,
                AddressOf FurreNamedLeaveView, "(0:31) When a furre named {...} leaves the bots view")

            'Summon
            '(0:32) When someone requests to summon the bot,
            Add(TriggerCategory.Cause, 32,
                Function()
                    Return Not callbk.IsBot(callbk.Player)
                End Function, "(0:32) When someone requests to summon the bot,")

            '(0:33) When a furre named {...} requests to summon the bot,
            Add(TriggerCategory.Cause, 33,
                AddressOf NameIs, "(0:33) When a furre named {...} requests to summon the bot,")
            'Join
            '(0:34) When someone requests to join the bot,
            Add(TriggerCategory.Cause, 34,
                Function()
                    Return Not callbk.IsBot(callbk.Player)
                End Function, "(0:34) When someone requests to join the bot,")
            '(0:35) When a furre named {...} requests to join the bot,
            Add(TriggerCategory.Cause, 35,
                AddressOf NameIs, "(0:35) When a furre named {...} requests to join the bot,")
            'Follow
            '(0:36) When someone requests to follow the bot,
            Add(TriggerCategory.Cause, 36,
                Function()
                    Return Not callbk.IsBot(callbk.Player)
                End Function, "(0:36) When someone requests to follow the bot,")
            '(0:37) When a furre named {...} requests to follow the bot,
            Add(TriggerCategory.Cause, 37,
                AddressOf NameIs, "(0:37) When a furre named {...} requests to follow the bot,")
            'Lead
            '(0:38) When someone requests to lead the bot,
            Add(TriggerCategory.Cause, 38,
                Function()
                    Return Not callbk.IsBot(callbk.Player)
                End Function, "(0:38) When someone requests to lead the bot,")
            '(0:39) When a furre named {...} requests to lead the bot,
            Add(TriggerCategory.Cause, 39,
                AddressOf NameIs, "(0:39) When a furre named {...} requests to lead the bot,")
            'Cuddle
            '(0:40) When someone requests to cuddle with the bot. 
            Add(TriggerCategory.Cause, 40,
                Function()
                    Return Not callbk.IsBot(callbk.Player)
                End Function, "(0:40) When someone requests to cuddle with the bot,")
            '(0:41) When a furre named {...} requests to cuddle with the bot,
            Add(TriggerCategory.Cause, 41,
                AddressOf NameIs, "(0:41) When a furre named {...} requests to cuddle with the bot,")

            'Trade rewuests



            '(0:46) When the bot sees a trade request,
            Add(TriggerCategory.Cause, 46,
            Function()
                Return Not callbk.IsBot(callbk.Player)
            End Function, "(0:46) When the bot sees a trade request,")
            '(0:47) When the bot sees the trade request {...},
            Add(TriggerCategory.Cause, 47,
                 AddressOf msgIs, "(0:47) When the bot sees the trade request {...}")

            '(0:48) When the bot sees a trade request with {...} in it,
            Add(TriggerCategory.Cause, 48,
                 AddressOf msgContains, "(0:48) When the bot sees a trade request with {...} in it,")


            'Dream
            '(0:90) When the bot enters a dream,
            Add(TriggerCategory.Cause, 90,
                Function()
                    Return True
                End Function, "(0:90) When the bot enters a dream,")
            '(0:91) When the bot enters a dream named {...},
            Add(TriggerCategory.Cause, 91,
                AddressOf EnterDreamNamed, "(0:91) When the bot enters a dream named {...},")

            '(0:92) When the bot detects the "Your throat is tired. Please wait a few seconds" message,
            Add(TriggerCategory.Cause, 92,
                Function()
                    Return True
                End Function, "(0:92) When the bot detects the ""Your throat is tired. Please wait a few seconds"" message,")

            '(0:93) When the bot resumes processing after seeing "Your throat is tired" message,
            Add(TriggerCategory.Cause, 93,
                Function()
                    Return True
                End Function, "(0:93) When the bot resumes processing after seeing ""Your throat is tired"" message,")


            ' (1:3) and the triggering furre's name is {...},
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 5), AddressOf NameIs, "(1:5) and the triggering furre's name is {...},")
            ' (1:4) and the triggering furre's name is not {...},
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 6), AddressOf NameIsNot, "(1:6) and the triggering furre's name is not {...},")

            ' (1:5) and the Triggering Furre's message is {...}, (say, emote, shot, whisper, or emit Channels)
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 7), AddressOf msgIs, "(1:7) and the triggering furre's message is {...},")
            ' (1:8) and the triggering furre's message contains {...} in it, (say, emote, shot, whisper, or emit Channels)
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 8), AddressOf msgContains, "(1:8) and the triggering furre's message contains {...} in it,")
            '(1:9) and the triggering furre's message does not contain {...} in it, (say, emote, shot, whisper, or emit Channels)
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 9), AddressOf msgNotContain, "(1:9) and the triggering furre's message does not contain {...} in it,")
            '(1:10) and the triggering furre's message is not {...}, (say, emote, shot, whisper, or emit Channels)
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 10), AddressOf msgIsNot, "(1:10) and the triggering furre's message is not {...},")

            '(1:11) and triggering furre's message starts with {...},
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 11), AddressOf msgStartsWith, "(1:11) and triggering furre's message starts with {...},")
            '(1:12) and triggering furre's message doesn't start with {...},
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 12), AddressOf msgNotStartsWith, "(1:12) and triggering furre's message doesn't start with {...},")
            '(1:13) and triggering furre's message  ends with {...},
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 13), AddressOf msgEndsWith, "(1:13) and triggering furre's message  ends with {...},")
            '(1:14) and triggering furre's message doesn't end with {...},
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 14), AddressOf msgNotEndsWith, "(1:14) and triggering furre's message doesn't end with {...},")

            '(1:904) and the triggering furre is the Bot Controller,
            Add(New Trigger(TriggerCategory.Condition, 15), _
                Function()
                    Return IsBotControler(MainMSEngine.MSpage.GetVariable(MS_Name).Value.ToString)
                End Function, "(1:15) and the triggering furre is the Bot Controller,")

            '(1:905) and the triggering furre is not the Bot Controller,
            Add(New Trigger(TriggerCategory.Condition, 16), _
                Function()
                    Return Not IsBotControler(MainMSEngine.MSpage.GetVariable(MS_Name).Value.ToString)
                End Function, "(1:16) and the triggering furre is not the Bot Controller,")


            '(1:17) and the triggering furre's name is {...}.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 17), AddressOf TrigFurreNameIs, "(1:17) and the triggering furre's name is {...},")

            '(1:18) and the triggering furre's name is not {...}.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 18), AddressOf TrigFurreNameIsNot, "(1:18) and the triggering furre's name is not {...},")


            '(1:19) and the bot is the dream owner,
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 19), AddressOf BotIsDreamOwner, "(1:19) and the bot is the dream owner,")
            '(1:20) and the bot is not the dream owner,
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 20), AddressOf BotIsNotDreamOwner, "(1:20) and the bot is not the dream owner,")
            '(1:21) and the furre named {...} is the dream owner,
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 21), AddressOf FurreNamedIsDreamOwner, "(1:21) and the furre named {...} is the dream owner,")
            '(1:22) and the furre named {...} is not the dream owner,
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 22), AddressOf FurreNamedIsNotDreamOwner, "(1:22) and the furre named {...} is not the dream owner,")
            '(1:23) and the Dream Name is {...},
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 23), AddressOf DreamNameIs, "(1:23) and the Dream Name is {...},")
            '(1:24) and the Dream Name is not {...},
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 24), AddressOf DreamNameIsNot, "(1:24) and the Dream Name is not {...},")
            '(1:25) and the triggering furre is the dream owner
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 25), AddressOf TriggeringFurreIsDreamOwner, "(1:25) and the triggering furre is the dream owner,")
            '(1:26) and the triggering furre is not the dream owner
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 26), AddressOf TriggeringFurreIsNotDreamOwner, "(1:26) and the triggering furre is not the dream owner,")

            '(1:27) and the bot has share control of the dream or is the dream owner,
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 27),
                Function(reader As TriggerReader)
                    Dim tname As Variable = MainMSEngine.MSpage.GetVariable("DREAMOWNER")
                    If callbk.HasShare Or callbk.BotName.ToFurcShortName = tname.Value.ToString.ToFurcShortName Then
                        Return True
                    End If
                    Return False
                End Function, "(1:27) and the bot has share control of the dream or is the dream owner,")

            '(1:28) and the bot has share control of the dream,
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 28),
                Function()
                    Return callbk.HasShare
                End Function, "(1:28) and the bot has share control of the dream,")

            '(1:29) and the bot doesn't have share control in the dream,
            Add(New Monkeyspeak.Trigger(TriggerCategory.Condition, 29),
                Function()
                    Return Not callbk.HasShare
                End Function, "(1:29) and the bot doesn't have share control in the dream,")
            'Says
            ' (5:0) say {...}. 
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 0),
             Function(reader As Monkeyspeak.TriggerReader)
                 Try
                     Dim msg As String = reader.ReadString(True)
                     sndSay(msg)
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
             End Function,
             "(5:0) say {...}.")
            'emotes
            ' (5:1) emote {...}. 
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 1),
             Function(reader As Monkeyspeak.TriggerReader)
                 Try
                     Dim msg As String = reader.ReadString
                     sndEmote(msg)
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
             End Function,
             "(5:1) emote {...}.")

            'Shouts
            ' (5:2) shout {...}.         
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 2),
             Function(reader As Monkeyspeak.TriggerReader)
                 Try
                     Dim msg As String = reader.ReadString
                     sndShout(msg)
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
             End Function,
             "(5:2) shout {...}.")

            'Emits
            ' (5:3) emit {...}. 
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 3),
           Function(reader As Monkeyspeak.TriggerReader)
               Try
                   Dim msg As String = reader.ReadString
                   sndEmit(msg)
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
           End Function,
          "(5:3) Emit {...}.")
            ' (5:4) emitloud {...}. 
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 4),
           Function(reader As Monkeyspeak.TriggerReader)
               Try
                   Dim msg As String = reader.ReadString
                   sndEmitLoud(msg)
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
           End Function,
          "(5:4) Emitloud {...}.")

            'Whispers
            ' (5:5) whisper {...} to the triggering furre.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 5),
           Function(reader As Monkeyspeak.TriggerReader)
               Try
                   Dim msg As String = reader.ReadString
                   Dim tname As Variable = MainMSEngine.MSpage.GetVariable(MS_Name)
                   sndWhisper(tname.Value.ToString, msg)
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
           End Function,
         "(5:5) whisper {...} to the triggering furre.")

            ' (5:6) whisper {...} to {...}.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 6),
            Function(reader As Monkeyspeak.TriggerReader)
                Try
                    Dim msg As String = reader.ReadString(True)
                    Dim tname As String = reader.ReadString
                    sndWhisper(tname, msg)
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
            End Function,
         "(5:6) whisper {...} to {...}.")

            ' (5:7) whisper {...} to {...} even if they're offline.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 7),
          Function(reader As Monkeyspeak.TriggerReader)
              Try
                  Dim msg As String = reader.ReadString
                  Dim tname As String = reader.ReadString

                  sndOffWhisper(tname, msg)
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
          End Function,
            "(5:7) whisper {...} to {...} even if they're offline.")

            '(5:20) give share control to the triggering furre.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 20), AddressOf ShareTrigFurre, "(5:20) give share control to the triggering furre.")
            '(5:21) remove shae control from the triggering furre.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 21), AddressOf UnshareTrigFurre, "(5:21) remove share control from the triggering furre.")
            '(5:22) remove share from the furre named {...} if they're in the dream right now.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 22), AddressOf ShareFurreNamed, "(5:22) remove share from the furre named {...} if they're in the dream right now.")
            '(5:23) give share to the furre named {...} if they're in the dream right now.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 23), AddressOf UnshareFurreNamed, "(5:23) give share to the furre named {...} if they're in the dream right now.")

            '(5:40) Switch the bot to stand alone mode and close the Furcadia client.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 40), AddressOf StandAloneMode, "(5:40) Switch the bot to stand alone mode and close the Furcadia client.")

            '(5:41) Disconnect the bot from the Furcadia game server.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 41), AddressOf FurcadiaDisconnect, "(5:41) Disconnect the bot from the Furcadia game server.")

            '(5:42) start a new instance to Silver Monkey with botfile {...}.
            Add(New Monkeyspeak.Trigger(TriggerCategory.Effect, 42), AddressOf StartNewBot,
                "(5:42) start a new instance to Silver Monkey with botfile {...}.")

        End Sub

        Function NameIs(reader As TriggerReader) As Boolean
            Try
                Dim TmpName As String = reader.ReadString()
                Dim tname As Variable = MainMSEngine.MSpage.GetVariable(MS_Name)
                'add Machine Name parser
                Return TmpName.ToFurcShortName = tname.Value.ToString.ToFurcShortName
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                'Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                'Debug.Print(ErrorString)
                Return False
            End Try
        End Function

        Function TrigFurreNameIs(reader As TriggerReader) As Boolean
            Try
                Dim TmpName As String = reader.ReadString()
                Dim TrigFurreName As String = callbk.Player.ShortName
                'add Machine Name parser
                Return TmpName.ToFurcShortName = TrigFurreName
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                'Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                'Debug.Print(ErrorString)
                Return False
            End Try
        End Function

        Function TrigFurreNameIsNot(reader As TriggerReader) As Boolean
            Return Not TrigFurreNameIs(reader)
        End Function

        '(1:19) and the bot is the dream owner,
        Function BotIsDreamOwner(reader As TriggerReader) As Boolean
            Try
                Dim tname As Variable = MainMSEngine.MSpage.GetVariable("DREAMOWNER")
                Dim TrigFurreName As String = callbk.BotName
                'add Machine Name parser
                Return tname.Value.ToString().ToFurcShortName = TrigFurreName.ToFurcShortName
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                'Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                'Debug.Print(ErrorString)
                Return False
            End Try
        End Function
        Function BotIsNotDreamOwner(reader As TriggerReader) As Boolean
            Return Not BotIsDreamOwner(reader)
        End Function
        '(1:20) and the furre named {...} is the dream owner,
        Function FurreNamedIsDreamOwner(reader As TriggerReader) As Boolean
            Try
                Dim tname As Variable = MainMSEngine.MSpage.GetVariable("DREAMOWNER")
                Dim TrigFurreName As String = reader.ReadString
                'add Machine Name parser
                Return tname.Value.ToString().ToFurcShortName = TrigFurreName.ToFurcShortName
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                'Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                'Debug.Print(ErrorString)
                Return False
            End Try
        End Function
        Function FurreNamedIsNotDreamOwner(reader As TriggerReader) As Boolean
            Return Not FurreNamedIsDreamOwner(reader)
        End Function
        '(1:21) and the Dream Name is {...},
        Function DreamNameIs(reader As TriggerReader) As Boolean
            Try
                Dim tname As Variable = MainMSEngine.MSpage.GetVariable("DREAMNAME")
                Dim TrigFurreName As String = reader.ReadString
                TrigFurreName = TrigFurreName.ToLower.Replace("furc://", "")
                'add Machine Name parser
                Return tname.Value.ToString().ToFurcShortName = TrigFurreName.ToFurcShortName
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                'Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                'Debug.Print(ErrorString)
                Return False
            End Try
        End Function
        Function DreamNameIsNot(reader As TriggerReader) As Boolean
            Return Not DreamNameIs(reader)
        End Function
        '(1:22:) and the triggering furre is the dream owner
        Function TriggeringFurreIsDreamOwner(reader As TriggerReader) As Boolean
            Try
                Dim tname As String = callbk.Player.ShortName
                Dim TrigFurreName As String = MainMSEngine.MSpage.GetVariable("DREAMOWNER").Value.ToString
                'add Machine Name parser
                Return tname = TrigFurreName.ToFurcShortName
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                'Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                'Debug.Print(ErrorString)
                Return False
            End Try
        End Function
        Function TriggeringFurreIsNotDreamOwner(reader As TriggerReader) As Boolean
            Return Not TriggeringFurreIsDreamOwner(reader)
        End Function
        Function NameIsNot(reader As TriggerReader) As Boolean
            Try
                Dim tname As String = MainMSEngine.MSpage.GetVariable(MS_Name).Value.ToString
                Dim TmpName As String = reader.ReadString()
                'add Machine Name parser
                If TmpName.ToFurcShortName <> tname.ToFurcShortName Then Return True
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
            Return False
        End Function

        '(0:17) When someone whispers something with {...} in it,
        Function msgContains(reader As TriggerReader) As Boolean

            Try
                Dim msMsg As String = StripHTML(reader.ReadString())
                Dim msg As Variable = MainMSEngine.MSpage.GetVariable("MESSAGE")

                Dim test As String = StripHTML(msg.Value.ToString)
                Return test.Contains(msMsg)
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

        Function msgNotContain(reader As TriggerReader) As Boolean
            Try
                Dim msMsg As String = StripHTML(reader.ReadString())
                Dim msg As Variable = MainMSEngine.MSpage.GetVariable("MESSAGE")

                Dim test As String = StripHTML(msg.Value.ToString)
                Return test.Contains(msMsg)
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

        Private Function StripHTML(ByVal Text As String) As String

            Dim r As New Regex("<(.*?)>")
            Text = r.Replace(Text, "")
            Return Text.Replace("|", " ").ToLower
        End Function

        Public Function msgIs(reader As TriggerReader) As Boolean
            Try
                Dim safety As Boolean = Not callbk.IsBot(callbk.Player)
                Dim msMsg As String = StripHTML(reader.ReadString())
                Dim msg As Variable = MainMSEngine.MSpage.GetVariable("MESSAGE")

                Dim test As String = StripHTML(msg.Value.ToString)
                Dim test2 As Boolean = msMsg.Equals(test) And safety
                Return msMsg.Equals(test) And safety
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

        Function msgIsNot(reader As TriggerReader) As Boolean
            Try
                Dim safety As Boolean = Not callbk.IsBot(callbk.Player)
                Dim msMsg As String = StripHTML(reader.ReadString())
                Dim msg As Variable = MainMSEngine.MSpage.GetVariable("MESSAGE")

                Dim test As String = StripHTML(msg.Value.ToString)
                Return Not msMsg.Equals(test) And safety
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

        '(1:11) and triggering furre's message starts with {...},
        Function msgStartsWith(reader As TriggerReader) As Boolean
            Try
                Dim msMsg As String = StripHTML(reader.ReadString())
                Dim msg As Variable = MainMSEngine.MSpage.GetVariable("MESSAGE")

                Dim test As String = StripHTML(msg.Value.ToString)
                Return test.StartsWith(msMsg)
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
        '(1:12) and triggering furre's message doesn't start with {...},
        Function msgNotStartsWith(reader As TriggerReader) As Boolean
            Try
                Dim msMsg As String = StripHTML(reader.ReadString())
                Dim msg As Variable = MainMSEngine.MSpage.GetVariable("MESSAGE")

                Dim test As String = StripHTML(msg.Value.ToString)
                Return Not test.StartsWith(msMsg)
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
        '(1:13) and triggering furre's message  ends with {...},
        Function msgEndsWith(reader As TriggerReader) As Boolean
            Try
                Dim msMsg As String = StripHTML(reader.ReadString())
                Dim msg As Variable = MainMSEngine.MSpage.GetVariable("MESSAGE")

                Dim test As String = StripHTML(msg.Value.ToString)
                'Debug.Print("Msg = " & msg)
                Return test.EndsWith(msMsg)
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
        '(1:14) and triggering furre's message doesn't end with {...},
        Function msgNotEndsWith(reader As TriggerReader) As Boolean

            Try
                Dim msMsg As String = StripHTML(reader.ReadString())
                Dim msg As Variable = MainMSEngine.MSpage.GetVariable("MESSAGE")

                Dim test As String = StripHTML(msg.Value.ToString)
                'Debug.Print("Msg = " & msg)
                Return Not test.EndsWith(msMsg)
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

        Public Function EnterView(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = callbk.Player
                If tPlayer.Visible = tPlayer.WasVisible Then
                    Return False
                End If
                Return tPlayer.Visible = True
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
        Public Function FurreNamedEnterView(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = callbk.NametoFurre(name, False)
                If tPlayer.Visible = tPlayer.WasVisible Then
                    Return False
                End If
                Return tPlayer.Visible = True

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
        Public Function LeaveView(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = callbk.Player
                If tPlayer.Visible = tPlayer.WasVisible Then
                    Return False
                End If
                Return tPlayer.Visible = False
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
        Public Function FurreNamedLeaveView(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = callbk.NametoFurre(name, False)
                If tPlayer.Visible = tPlayer.WasVisible Then
                    Return False
                End If
                Return tPlayer.Visible = False

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
        '(0:91) When the bot enters a dream named {...},
        Public Function EnterDreamNamed(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                Dim msMsg As String = reader.ReadString()
                Dim msg As Variable = MainMSEngine.MSpage.GetVariable("DREAMNAME")
                Dim str As String = msMsg.ToLower.Replace("furc://", "").ToLower
                If str.EndsWith("/") Then str = str.TrimEnd("/"c)

                Dim str2 As String = msg.Value.ToString.ToLower.Replace("furc://", "")
                If str2.EndsWith("/") Then str2 = str2.TrimEnd("/"c)

                Return str = str2

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
        Public Sub sndSay(ByRef msg As String)

            sendServer(msg)

        End Sub
        Public Sub sndEmote(ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then sendServer(":" & msg)
        End Sub
        Public Sub sndShout(ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then sendServer("-" & msg)
        End Sub
        Public Sub sndEmit(ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then sendServer("emit " & msg)
        End Sub
        Public Sub sndEmitLoud(ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then sendServer("emitloud " & msg)
        End Sub
        Public Sub sndWhisper(ByRef name As String, ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then sendServer("/%" & name.ToFurcShortName & " " & msg)
            ' Debug.Print("wh " & name & " " & msg)
        End Sub
        Public Sub sndOffWhisper(ByRef name As String, ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then sendServer("/%%" & name.ToFurcShortName & " " & msg)
        End Sub

        Sub sendServer(ByRef var As String)
            callbk.TextToServer(var)
        End Sub

        '(5:20) give share control to the triggering furre.
        Public Function ShareTrigFurre(reader As Monkeyspeak.TriggerReader) As Boolean
            Dim furre As String = callbk.Player.Name
            sendServer("share " + furre)
            Return True
        End Function
        '(5:21) remove shae control from the triggering furre.
        Public Function UnshareTrigFurre(reader As Monkeyspeak.TriggerReader) As Boolean
            Dim furre As String = callbk.Player.ShortName
            sendServer("unshare " + furre)
            Return True
        End Function
        '(5:22) remove share from the furre named {...} if they're in the dream right now.
        Public Function ShareFurreNamed(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                Dim furre As String = reader.ReadString
                Dim Target As FURRE = callbk.NametoFurre(furre, False)
                If InDream(Target.Name) Then sendServer("share " + furre.ToFurcShortName)
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
        Private Function InDream(ByRef Name As String) As Boolean
            Dim found As Boolean = False
            For Each kvp As KeyValuePair(Of UInteger, FURRE) In DREAM.List
                If kvp.Value.Name.ToFurcShortName = Name.ToFurcShortName Then
                    Return True
                End If
            Next
            Return False
        End Function
        '(5:23) give share to the furre named {...} if they're in the dream right now.
        Public Function UnshareFurreNamed(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                Dim furre As String = reader.ReadString
                Dim Target As FURRE = callbk.NametoFurre(furre, False)
                If InDream(Target.Name) Then sendServer("unshare " + furre.ToFurcShortName)
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
        '(5:40) Switch the bot to stand alone mode and close the Furcadia client.
        Public Function StandAloneMode(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                cBot.StandAlone = True
                callbk.KillProc(callbk.ProcID)
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
            Return True
        End Function
        '(5:41) Disconnect the bot from the Furcadia game server.
        Public Function FurcadiaDisconnect(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                callbk.DisconnectBot()
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
            Return True
        End Function
        '(5:42) start a new instance to Silver Monkey with botfile {...}.
        Public Function StartNewBot(reader As Monkeyspeak.TriggerReader) As Boolean
            Try
                'Dim ps As Process = New Process()
                Dim File As String = reader.ReadString
                Dim p As New ProcessStartInfo
                p.Arguments = File
                p.FileName = "SilverMonkey.exe"
                Process.Start(p)
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try

            Return True
        End Function
    End Class

    Class MathLibrary
        Inherits Libraries.AbstractBaseLibrary
        Sub New()

        End Sub

    End Class

End Module
