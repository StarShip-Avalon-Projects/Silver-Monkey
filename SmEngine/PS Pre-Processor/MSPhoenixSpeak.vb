Imports System.Text
Imports System.Text.RegularExpressions
Imports Furcadia.Net.Utils.ServerParser
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Monkey Speak Interface to the PhoenixSpeak server interface
    ''' <para>
    ''' Checks and executes Basic PhoenixSpeak response triggers
    ''' </para>
    ''' </summary>
    Public Class MsPhoenixSpeak
        Inherits MonkeySpeakLibrary

#Region "Public Fields"

        Public Const SmRegExOptions As RegexOptions = RegexOptions.CultureInvariant _
            Or RegexOptions.IgnoreCase

#End Region

#Region "Private Fields"

        Private PSInfoCache As List(Of PhoenixSpeak.Variable)

        Private PSPage As StringBuilder

#End Region

#Region "Public Constructors"

        Public Sub New(session As BotSession)
            MyBase.New(session)
            PSPage = New StringBuilder()
            PSInfoCache = New List(Of PhoenixSpeak.Variable)(100)
            Add(TriggerCategory.Cause, 80,
        Function()
            Return True
        End Function,
     "(0:80) When the bot sees a Phoenix Speak response")
            Add(TriggerCategory.Cause, 81,
             AddressOf msgIs, "(0:81) When the bot sees the Phoenix Speak response {...},")

            Add(TriggerCategory.Cause, 82,
             AddressOf msgContains, "(0:82) When the bot sees a Phoenix Speak response with {...} in it,")

            '(5:60) get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect, 60, AddressOf RemberPSInforTrigFurre, "(5:60) get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.")

            '(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect, 61, AddressOf RemberPSInforFurreNamed, "(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.")

            '(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect, 62, AddressOf RemberPSInfoAllDream, "(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.")

            '(5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.
            Add(TriggerCategory.Effect, 63, AddressOf RemberPSInfoAllCharacters, "(5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.")

            '(5:80) retrieve  Phoenix Speak info {...} and place the value into variable %Variable.
            Add(TriggerCategory.Effect, 80, AddressOf getPSinfo, "(5:80) retrieve  Phoenix Speak info {...} and place the value into variable %Variable.")

            '(5:81) Store PSInfo Key Names to Variable %Variable.
            Add(TriggerCategory.Effect, 81, AddressOf PSInfoKeyToVariable, "(5:81) Store PSInfo Key Names to Variable %Variable.")

            '(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.
            Add(TriggerCategory.Effect, 82, AddressOf MemorizeFurreNamedPS, "(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.")

            '(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.
            Add(TriggerCategory.Effect, 83, AddressOf ForgetFurreNamedPS, "(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.")

            '(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.
            Add(TriggerCategory.Effect, 84, AddressOf MemorizeTrigFurrePS, "(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.")

            '(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.
            Add(TriggerCategory.Effect, 85, AddressOf ForgetTrigFurrePS, "(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.")

            '(5:90) Memorize Phoenix Speak info {...} for this dream.
            Add(TriggerCategory.Effect, 90, AddressOf MemorizeDreamPS, "(5:90) Memorize Phoenix Speak info {...} for this dream.")
            '(5:91) Forget Phoenix Speak info {...} for this dream.
            Add(TriggerCategory.Effect, 91, AddressOf ForgetDreamPS, "(5:91) Forget Phoenix Speak info {...} for this dream.")

            '(5:94) execute Phoenix Speak command {...}.
            Add(TriggerCategory.Effect, 94, AddressOf PSCommand, "(5:94) execute Phoenix Speak command {...}.")
            Add(TriggerCategory.Effect, 95, AddressOf PSForgetTriggeringFurre, "(5:95) Forget ALL Phoenix Speak info for the triggering furre")
            Add(TriggerCategory.Effect, 96, AddressOf PSForgetFurreNamed, "(5:96) Forget ALL Phoenix Speak info for the furre named {...}.")
            Add(TriggerCategory.Effect, 97, AddressOf PSForgetDream, "(5:97) Forget ALL Phoenix Speak info for this dream.")

        End Sub

#End Region

#Region "Public Methods"

        '(5:91) Forget Phoenix Speak info {...} for this dream.
        Function ForgetDreamPS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                sendServer("ps clear dream." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.
        Function ForgetFurreNamedPS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                Dim furre As String = reader.ReadString
                sendServer("ps clear characer." + furre + "." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.
        Function ForgetTrigFurrePS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                Dim furre As String = FurcadiaSession.Player.ShortName
                sendServer("ps clear characer." + furre + "." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        Public Function getPSinfo(reader As TriggerReader) As Boolean
            Try
                Dim Info As New PhoenixSpeak.Variable(reader.ReadString())
                Dim var As Monkeyspeak.Variable = reader.ReadVariable(True)
                If PSInfoCache.Contains(Info) Then
                    var = Info
                Else
                    var.Value = Nothing
                End If
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:90) Memorize Phoenix Speak info {...} for this dream.
        Function MemorizeDreamPS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                sendServer("ps set dream." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.
        Function MemorizeFurreNamedPS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                Dim furre As String = reader.ReadString
                sendServer("ps set characer." + furre + "." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.
        Function MemorizeTrigFurrePS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                Dim furre As String = FurcadiaSession.Player.ShortName
                sendServer("ps set characer." + furre + "." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(0:17) When someone whispers something with {...} in it,
        Function msgContains(reader As TriggerReader) As Boolean
            Dim msMsg As String = ""
            Dim msg As String = ""
            Try
                'Debug.Print("msgContains Begin Execution")
                msMsg = reader.ReadString()
                'Debug.Print("msMsg = " & msMsg)
                msg = MsPage.GetVariable("MESSAGE").Value.ToString
                'Debug.Print("Msg = " & msg)
                Return msg.Contains(msMsg)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        Public Function msgIs(reader As TriggerReader) As Boolean
            Try
                Dim msMsg As String = reader.ReadString()
                Dim msg As Monkeyspeak.Variable = MsPage.GetVariable("MESSAGE")
                If msMsg = msg.Value.ToString Then Return True
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        Function msgIsNot(reader As TriggerReader) As Boolean
            Try
                Dim msMsg As String = reader.ReadString()
                Dim msg As Monkeyspeak.Variable = MsPage.GetVariable("MESSAGE")
                If msMsg <> msg.Value.ToString Then Return True
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        Function msgNotContain(reader As TriggerReader) As Boolean
            Try
                Dim msMsg As String = reader.ReadString()
                'Debug.Print("msMsg = " & msMsg)
                Dim msg As Monkeyspeak.Variable = MsPage.GetVariable("MESSAGE")
                'Debug.Print("Msg = " & msg.Value.ToString)
                If msg.Value.ToString.Contains(msMsg) Then Return False
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:94) execute Phoenix Speak command {...}.
        Function PSCommand(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                sendServer("ps " + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        Function PSForgetDream(reader As TriggerReader) As Boolean
            Try
                sendServer("ps clear dream ")
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        Function PSForgetFurreNamed(reader As TriggerReader) As Boolean
            Try
                Dim Furre As String = reader.ReadString
                sendServer("ps clear character. " + Furre)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        Function PSForgetTriggeringFurre(reader As TriggerReader) As Boolean
            Try
                sendServer("ps clear character." + FurcadiaSession.Player.ShortName)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:81) Store PSInfo Key Names to Variable %Variable.
        Function PSInfoKeyToVariable(reader As TriggerReader) As Boolean
            Try
                Dim Var As Monkeyspeak.Variable = reader.ReadVariable(True)
                Dim L As New List(Of String)
                For Each Name As PhoenixSpeak.Variable In PSInfoCache
                    L.Add(Name.Name)
                Next
                Var.Value = String.Join(" ", L)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.
        Function RemberPSInfoAllCharacters(reader As TriggerReader) As Boolean
            Try
                sendServer("ps get character.*")
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.
        Function RemberPSInfoAllDream(reader As TriggerReader) As Boolean
            Try
                sendServer("ps get dream.*")
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.
        Function RemberPSInforFurreNamed(reader As TriggerReader) As Boolean
            Try
                Dim furre As String = reader.ReadString
                sendServer("ps get characer." + furre + ".*")
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:60) get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.
        Function RemberPSInforTrigFurre(reader As TriggerReader) As Boolean
            Try
                Dim furre As String = FurcadiaSession.Player.ShortName
                sendServer("ps set characer." + furre + ".*")
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Handle server data
        ''' </summary>
        ''' <param name="obj">
        ''' </param>
        ''' <param name="e">
        ''' </param>
        Private Shared Sub ParseData(obj As ChannelObject, e As EventArgs) Handles FurcadiaSession.ProcessServerChannelData
            Dim DiceObject As DiceRolls = Nothing
            If obj.GetType().Equals(GetType(DiceRolls)) Then
                DiceObject = CType(obj, DiceRolls)
            Else
                Exit Sub
            End If

        End Sub

#End Region

        ''' <summary>
        ''' returns number of Phoenix Speak pages remaining
        ''' </summary>
        ''' <param name="data">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function ProcessPage(ByRef data As String) As Short
            Dim PsPage As New Regex(String.Format("{0}", "multi_result?(\ d +)?/(\d+)?"), SmRegExOptions)
            Dim CurrentPage As Short = 0
            Dim TotalPages As Short = 0
            Short.TryParse(PsPage.Match(data, 0).Groups(1).Value(), CurrentPage)
            Short.TryParse(PsPage.Match(data, 0).Groups(2).Value(), TotalPages)
            data = PsPage.Replace(data, "", 1)
            Return TotalPages - CurrentPage
            'Add "," to the end of match #1.
            'Input: "bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340,"
        End Function

        ''' <summary>
        ''' Get a Phoenix-Speak Variable List from a REGEX match collection.
        ''' Process only if there is a single page or if we have collected
        ''' all pages
        ''' <para>
        ''' Match(1) : Value(Name)
        ''' </para>
        ''' <para>
        ''' Match 2: Empty if number, ' if string
        ''' </para>
        ''' <para>
        ''' Match(3) : Value()
        ''' </para>
        ''' </summary>
        ''' <param name="data">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function ProcessVariables(ByRef data As String) As List(Of PhoenixSpeak.Variable)
            Dim mc As New Regex(" (.*?)=('?)(\d+|.*?)(\2),?", SmRegExOptions)

            Dim PsVarList As New List(Of PhoenixSpeak.Variable)
            For Each M As Match In mc.Matches(data)
                PsVarList.Add(New PhoenixSpeak.Variable(M.Groups(1).Value.Trim, M.Groups(3).Value))
            Next

            Return PsVarList
        End Function

    End Class

End Namespace