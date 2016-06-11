Imports Monkeyspeak
Imports SilverMonkey.ErrorLogging
Imports SilverMonkey.TextBoxWriter

Imports System.Diagnostics
Imports System.Collections
Imports System.Collections.Generic

Public Class phoenixspeak
    Inherits Monkeyspeak.Libraries.AbstractBaseLibrary
    Private writer As TextBoxWriter = Nothing
    Public Sub New()
        writer = New TextBoxWriter(Variables.TextBox1)
        Add(Monkeyspeak.TriggerCategory.Cause, 80,
        Function()
            Return True
        End Function,
     "(0:80) When the bot sees a Phoenix Speak response")
        Add(Monkeyspeak.TriggerCategory.Cause, 81,
             AddressOf msgIs, "(0:81) When the bot sees the Phoenix Speak response {...},")

        Add(Monkeyspeak.TriggerCategory.Cause, 82,
             AddressOf msgContains, "(0:82) When the bot sees a Phoenix Speak response with {...} in it,")

        '(5:60) get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.
        Add(Monkeyspeak.TriggerCategory.Effect, 60, AddressOf RemberPSInforTrigFurre, "(5:60) get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.")

        '(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.
        Add(Monkeyspeak.TriggerCategory.Effect, 61, AddressOf RemberPSInforFurreNamed, "(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.")

        '(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.
        Add(Monkeyspeak.TriggerCategory.Effect, 62, AddressOf RemberPSInfoAllDream, "(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.")

        '(5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.
        Add(Monkeyspeak.TriggerCategory.Effect, 63, AddressOf RemberPSInfoAllCharacters, "(5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.")

        '(5:64) get All Phoenix Speak info with ID # for the triggering furre and put it into the PSInfo Cache.
        Add(Monkeyspeak.TriggerCategory.Effect, 64, AddressOf RemberPSInforTrigFurre_ID, "(5:64) get All Phoenix Speak info with ID # for the triggering furre and put it into the PSInfo Cache.")

        '(5:65) get All Phoenix Speak info with ID # for the Furre Named {...} and put it into the PSInfo Cache.
        Add(Monkeyspeak.TriggerCategory.Effect, 65, AddressOf RemberPSInfoFurreNamed_ID, "(5:65) get All Phoenix Speak info with ID # for the Furre Named {...} and put it into the PSInfo Cache.")

        '(5:66) get All Phoenix Speak info with ID # for the dream and put it into the PSInfo Cache.
        Add(Monkeyspeak.TriggerCategory.Effect, 66, AddressOf RemberPSInfoAllDream_ID, "(5:66) get All Phoenix Speak info with ID # for the dream and put it into the PSInfo Cache.")

        '(5:68) get all Phoenix Speak info with ID # for all characters and put it into the PSInfo cache.
        Add(Monkeyspeak.TriggerCategory.Effect, 68, AddressOf RemberPSInfoAllCharacters_ID, "(5:68) get all Phoenix Speak info with ID # for all characters and put it into the PSInfo cache.")

        '(5:80) retrieve  Phoenix Speak info {...} and place the value into variable %Variable.
        Add(Monkeyspeak.TriggerCategory.Effect, 80, AddressOf getPSinfo, "(5:80) retrieve  Phoenix Speak info {...} and place the value into variable %Variable.")

        '(5:81) Store PSInfo Key Names to Variable %Variable.
        Add(Monkeyspeak.TriggerCategory.Effect, 81, AddressOf PSInfoKeyToVariable, "(5:81) Store PSInfo Key Names to Variable %Variable.")

        '(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.
        Add(Monkeyspeak.TriggerCategory.Effect, 82, AddressOf MemorizeFurreNamedPS, "(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.")

        '(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.
        Add(Monkeyspeak.TriggerCategory.Effect, 83, AddressOf ForgetFurreNamedPS, "(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.")

        '(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.
        Add(Monkeyspeak.TriggerCategory.Effect, 84, AddressOf MemorizeTrigFurrePS, "(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.")

        '(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.
        Add(Monkeyspeak.TriggerCategory.Effect, 85, AddressOf ForgetTrigFurrePS, "(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.")

        '(5:86) Memorize Phoenix Speak info {...} with ID # for the Furre Named {...}.
        Add(Monkeyspeak.TriggerCategory.Effect, 86, AddressOf MemorizeFurreNamedPS_ID, "(5:86) Memorize Phoenix Speak info {...} with ID # for the Furre Named {...}.")

        '(5:87) Forget Phoenix Speak info {...} with ID # for the Furre Named {...}.
        Add(Monkeyspeak.TriggerCategory.Effect, 87, AddressOf ForgetFurreNamedPS_ID, "(5:87) Forget Phoenix Speak info {...} with ID # for the Furre Named {...}.")

        '(5:88) Memorize Phoenix Speak info {...} with ID # for the Triggering Furre.
        Add(Monkeyspeak.TriggerCategory.Effect, 88, AddressOf MemorizeTrigFurrePS_ID, "(5:88) Memorize Phoenix Speak info {...} with ID # for the Triggering Furre.")
        '(5:89) Forget Phoenix Speak info {...} with ID # for the Triggering Furre.
        Add(Monkeyspeak.TriggerCategory.Effect, 89, AddressOf ForgetTrigFurrePS_ID, "(5:89) Forget Phoenix Speak info {...} with ID # for the Triggering Furre.")

        '(5:90) Memorize Phoenix Speak info {...} for this dream.
        Add(Monkeyspeak.TriggerCategory.Effect, 90, AddressOf MemorizeDreamPS, "(5:90) Memorize Phoenix Speak info {...} for this dream.")
        '(5:91) Forget Phoenix Speak info {...} for this dream.
        Add(Monkeyspeak.TriggerCategory.Effect, 91, AddressOf ForgetDreamPS, "(5:91) Forget Phoenix Speak info {...} for this dream.")

        '(5:92) Memorize Phoenix Speak info {...} with ID # for this dream.
        Add(Monkeyspeak.TriggerCategory.Effect, 92, AddressOf MemorizeDreamPS_ID, "(5:92) Memorize Phoenix Speak info {...} with ID # for this dream.")
        '(5:93) Forget Phoenix Speak info {...} with ID # for this dream.
        Add(Monkeyspeak.TriggerCategory.Effect, 93, AddressOf ForgetDreamPS_ID, "(5:93) Forget Phoenix Speak info {...} with ID # for this dream.")

        '(5:94) execute Phoenix Speak command {...}.
        Add(Monkeyspeak.TriggerCategory.Effect, 94, AddressOf PSCommand, "(5:94) execute Phoenix Speak command {...}.")

    End Sub


    '(0:17) When someone whispers something with {...} in it,
    Function msgContains(reader As TriggerReader) As Boolean
        Dim msMsg As String = ""
        Dim msg As String = ""
        Try
            'Debug.Print("msgContains Begin Execution")
            msMsg = reader.ReadString()
            'Debug.Print("msMsg = " & msMsg)
            msg = MainEngine.MSpage.GetVariable("MESSAGE").Value.ToString
            'Debug.Print("Msg = " & msg)
            Return msg.Contains(msMsg)
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
            Dim msMsg As String = reader.ReadString()
            'Debug.Print("msMsg = " & msMsg)
            Dim msg As Variable = MainEngine.MSpage.GetVariable("MESSAGE")
            'Debug.Print("Msg = " & msg.Value.ToString)
            If msg.Value.ToString.Contains(msMsg) Then Return False
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

    Public Function msgIs(reader As TriggerReader) As Boolean
        Try
            Dim msMsg As String = reader.ReadString()
            Dim msg As Variable = MainEngine.MSpage.GetVariable("MESSAGE")
            If msMsg = msg.Value.ToString Then Return True
            Return False
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
            Dim msMsg As String = reader.ReadString()
            Dim msg As Variable = MainEngine.MSpage.GetVariable("MESSAGE")
            If msMsg <> msg.Value.ToString Then Return True
            Return False
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

    '(5:60) get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.
    Function RemberPSInforTrigFurre(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim furre As String = callbk.Player.ShortName
            sendServer("ps get characer." + furre + ".*")
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
    '(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.
    Function RemberPSInforFurreNamed(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim furre As String = reader.ReadString
            sendServer("ps get characer." + furre + ".*")
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
    '(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.
    Function RemberPSInfoAllDream(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            sendServer("ps get dream.*")
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
    '(5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.
    Function RemberPSInfoAllCharacters(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            sendServer("ps get character.*")
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
    '(5:64) get All Phoenix Speak info with ID # for the triggering furre and put it into the PSInfo Cache.
    Function RemberPSInforTrigFurre_ID(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim ID As String = ReadVariableOrNumber(reader).ToString
            Dim furre As String = callbk.Player.ShortName
            sendServer("ps " + ID + " get characer." + furre + ".*")
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
    '(5:65) get All Phoenix Speak info with ID # for the Furre Named {...} and put it into the PSInfo Cache.
    Function RemberPSInfoFurreNamed_ID(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim ID As String = ReadVariableOrNumber(reader).ToString
            Dim furre As String = reader.ReadString
            sendServer("ps " + ID + " get character." + furre + ".*")
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
    '(5:66) get All Phoenix Speak info with ID # for the dream and put it into the PSInfo Cache.
    Function RemberPSInfoAllDream_ID(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim ID As String = ReadVariableOrNumber(reader).ToString
            sendServer("ps " + ID + " get dream.*")
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
    '(5:68) get all Phoenix Speak info with ID # for all characters and put it into the PSInfo cache.
    Function RemberPSInfoAllCharacters_ID(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim ID As String = ReadVariableOrNumber(reader).ToString
            sendServer("ps " + ID + " get character.*")
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

    Public Function getPSinfo(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim Info As String = reader.ReadString()
            Dim var As Monkeyspeak.Variable = reader.ReadVariable(True)
            If Main.PSinfo.ContainsKey(Info) Then
                var.Value = Main.PSinfo.Item(Info)
            Else
                var.Value = Nothing
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

    '(5:81) Store PSInfo Key Names to Variable %Variable.
    Function PSInfoKeyToVariable(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim Var As Monkeyspeak.Variable = reader.ReadVariable(True)
            Var.Value = String.Join(" ", Main.PSinfo.Keys)
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
    '(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.
    Function MemorizeFurreNamedPS(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            Dim furre As String = reader.ReadString
            sendServer("ps set characer." + furre + "." + info)
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
    '(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.
    Function ForgetFurreNamedPS(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            Dim furre As String = reader.ReadString
            sendServer("ps clear characer." + furre + "." + info)
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
    '(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.
    Function MemorizeTrigFurrePS(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            Dim furre As String = callbk.Player.ShortName
            sendServer("ps set characer." + furre + "." + info)
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
    '(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.
    Function ForgetTrigFurrePS(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            Dim furre As String = callbk.Player.ShortName
            sendServer("ps clear characer." + furre + "." + info)
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

    '(5:86) Memorize Phoenix Speak info {...} with ID # for the Furre Named {...}.
    Function MemorizeFurreNamedPS_ID(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            Dim ID As String = ReadVariableOrNumber(reader).ToString
            Dim furre As String = reader.ReadString
            sendServer("ps " + ID + " set characer." + furre + " " + info)
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
    '(5:87) Forget Phoenix Speak info {...} with ID # for the Furre Named {...}.
    Function ForgetFurreNamedPS_ID(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            Dim ID As String = ReadVariableOrNumber(reader).ToString
            Dim furre As String = reader.ReadString
            sendServer("ps " + ID + " clear characer." + furre + " " + info)
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
    '(5:88) Memorize Phoenix Speak info {...} with ID # for the Triggering Furre.
    Function MemorizeTrigFurrePS_ID(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            Dim ID As String = ReadVariableOrNumber(reader).ToString
            Dim furre As String = callbk.Player.ShortName
            sendServer("ps " + ID + " set characer." + furre + "." + info)
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
    '(5:89) Forget Phoenix Speak info {...} with ID # for the Triggering Furre.
    Function ForgetTrigFurrePS_ID(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            Dim ID As String = ReadVariableOrNumber(reader).ToString
            Dim furre As String = callbk.Player.ShortName
            sendServer("ps " + ID + " clear characer." + furre + "." + info)
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

    '(5:90) Memorize Phoenix Speak info {...} for this dream.
    Function MemorizeDreamPS(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            sendServer("ps  set dream." + info)
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
    '(5:91) Forget Phoenix Speak info {...} for this dream.
    Function ForgetDreamPS(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            sendServer("ps  clear dream." + info)
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

    '(5:92) Memorize Phoenix Speak info {...} with ID # for this dream.
    Function MemorizeDreamPS_ID(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            Dim ID As String = ReadVariableOrNumber(reader).ToString
            sendServer("ps " + ID + " set dream." + info)
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
    '(5:93) Forget Phoenix Speak info {...} with ID # for this dream.
    Function ForgetDreamPS_ID(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            Dim ID As String = ReadVariableOrNumber(reader).ToString
            sendServer("ps " + ID + " clear dream." + info)
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
    '(5:94) execute Phoenix Speak command {...}.
    Function PSCommand(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim info As String = reader.ReadString
            sendServer("ps  " + info)
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

    Sub sendServer(ByRef var As String)
        callbk.sndServer(var)
    End Sub

End Class
