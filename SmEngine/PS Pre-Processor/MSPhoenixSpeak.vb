Imports System.Text
Imports System.Text.RegularExpressions
Imports Furcadia.Net.Utils.ServerParser
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Monkey Speak Interface to the
    ''' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix
    ''' Speak</see> server command line interface
    ''' <para>
    ''' Checks and executes predefined Phoenix Speak commands to manages a
    ''' dreams database.
    ''' </para>
    ''' <pra>Bot Testers: Be aware this class needs to be tested any way possible!</pra>
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
            Add(TriggerCategory.Effect, 60, AddressOf RemberPSInforTrigFurre,
                "(5:60) get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.")

            '(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect, 61, AddressOf RemberPSInforFurreNamed,
                "(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.")

            '(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect, 62, AddressOf RemberPSInfoAllDream,
                "(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.")

            '(5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.
            Add(TriggerCategory.Effect, 63, AddressOf RemberPSInfoAllCharacters,
                "(5:63) get all list of all characters and put it into the PSInfo cache.")

            '(5:80) retrieve  Phoenix Speak info {...} and place the value into variable %Variable.
            Add(TriggerCategory.Effect, 80, AddressOf getPSinfo,
                "(5:80) retrieve  Phoenix Speak info {...} and place the value into variable %Variable.")

            '(5:81) Store PSInfo Key Names to Variable %Variable.
            Add(TriggerCategory.Effect, 81, AddressOf PSInfoKeyToVariable,
                "(5:81) Store PSInfo Key Names to Variable %Variable.")

            '(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.
            Add(TriggerCategory.Effect, 82, AddressOf MemorizeFurreNamedPS,
                "(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.")

            '(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.
            Add(TriggerCategory.Effect, 83, AddressOf ForgetFurreNamedPS,
                "(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.")

            '(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.
            Add(TriggerCategory.Effect, 84, AddressOf MemorizeTrigFurrePS,
                "(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.")

            '(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.
            Add(TriggerCategory.Effect, 85, AddressOf ForgetTrigFurrePS,
                "(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.")

            '(5:90) Memorize Phoenix Speak info {...} for this dream.
            Add(TriggerCategory.Effect, 90, AddressOf MemorizeDreamPS,
                "(5:90) Memorize Phoenix Speak info {...} for this dream.")
            '(5:91) Forget Phoenix Speak info {...} for this dream.
            Add(TriggerCategory.Effect, 91, AddressOf ForgetDreamPS,
                "(5:91) Forget Phoenix Speak info {...} for this dream.")

            '(5:94) execute Phoenix Speak command {...}.
            Add(TriggerCategory.Effect, 94, AddressOf PSCommand, "(5:94) execute Phoenix Speak command {...}.")
            Add(TriggerCategory.Effect, 95, AddressOf PSForgetTriggeringFurre,
                "(5:95) Forget ALL Phoenix Speak info for the triggering furre")

            Add(TriggerCategory.Effect, 96, AddressOf PSForgetFurreNamed,
                "(5:96) Forget ALL Phoenix Speak info for the furre named {...}.")
            Add(TriggerCategory.Effect, 97, AddressOf PSForgetDream,
                "(5:97) Forget ALL Phoenix Speak info for this dream.")

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (5:91) Forget Phoenix Speak info {...} for this dream.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function ForgetDreamPS(reader As TriggerReader) As Boolean

            Dim info As String = reader.ReadString
            Return sendServer("ps clear dream." + info)

        End Function

        ''' <summary>
        ''' (5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function ForgetFurreNamedPS(reader As TriggerReader) As Boolean

            Dim info As String = reader.ReadString
            Dim furre As String = reader.ReadString
            Return sendServer("ps clear characer." + furre + "." + info)

        End Function

        ''' <summary>
        ''' (5:85) Forget Phoenix Speak info {...} for the Triggering Furre.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function ForgetTrigFurrePS(reader As TriggerReader) As Boolean

            Dim info As String = reader.ReadString
            Dim furre As String = Player.ShortName
            Return sendServer("ps clear characer." + furre + "." + info)
        End Function

        ''' <summary>
        ''' (5:80) retrieve Phoenix Speak info {...} and place the value
        ''' into variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function getPSinfo(reader As TriggerReader) As Boolean

            Dim Info As New PhoenixSpeak.Variable(reader.ReadString())
            Dim var As Monkeyspeak.Variable = reader.ReadVariable(True)
            If PSInfoCache.Contains(Info) Then
                var = Info
            Else
                var.Value = Nothing
            End If
            Return True

        End Function

        ''' <summary>
        ''' (5:90) Memorize Phoenix Speak info {...} for this dream.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function MemorizeDreamPS(reader As TriggerReader) As Boolean
            Dim info As String = reader.ReadString
            Return sendServer("ps set dream." + info)

        End Function

        ''' <summary>
        ''' (5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function MemorizeFurreNamedPS(reader As TriggerReader) As Boolean

            Dim info As String = reader.ReadString
            Dim furre As String = reader.ReadString
            Return sendServer("ps set characer." + furre + "." + info)

        End Function

        ''' <summary>
        ''' (5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.
        ''' <para>
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function MemorizeTrigFurrePS(reader As TriggerReader) As Boolean

            Dim info As String = reader.ReadString
            Dim furre As String = FurcadiaSession.Player.ShortName
            Return sendServer("ps set characer." + furre + "." + info)
        End Function

        ''' <summary>
        ''' (5:94) execute Phoenix Speak command {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function PSCommand(reader As TriggerReader) As Boolean

            Dim info As String = reader.ReadString
            Return sendServer("ps " + info)

        End Function

        ''' <summary>
        ''' (5:97) Forget ALL Phoenix Speak info for this dream.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function PSForgetDream(reader As TriggerReader) As Boolean

            Return sendServer("ps clear dream ")

        End Function

        ''' <summary>
        ''' (5:96) Forget ALL Phoenix Speak info for the furre named {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function PSForgetFurreNamed(reader As TriggerReader) As Boolean

            Dim Furre As String = reader.ReadString
            Return sendServer("ps clear character. " + Furre)

        End Function

        ''' <summary>
        ''' (5:95) Forget ALL Phoenix Speak info for the triggering furre
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function PSForgetTriggeringFurre(reader As TriggerReader) As Boolean

            Return sendServer("ps clear character." + Player.ShortName)

        End Function

        ''' <summary>
        ''' (5:81) Store PSInfo Key Names to Variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function PSInfoKeyToVariable(reader As TriggerReader) As Boolean

            Dim Var As Monkeyspeak.Variable = reader.ReadVariable(True)
            Dim L As New List(Of String)
            For Each Name As PhoenixSpeak.Variable In PSInfoCache
                L.Add(Name.Name)
            Next
            Var.Value = String.Join(" ", L)

            Return True
        End Function

        ''' <summary>
        ''' (5:63) get all list of all characters and put it into the PSInfo cache.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function RemberPSInfoAllCharacters(reader As TriggerReader) As Boolean

            Return sendServer("ps get character.*")

        End Function

        ''' <summary>
        ''' (5:62) get All Phoenix Speak info for the dream and put it into
        ''' the PSInfo Cache.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function RemberPSInfoAllDream(reader As TriggerReader) As Boolean

            Return sendServer("ps get dream.*")

        End Function

        ''' <summary>
        ''' (5:61) get All Phoenix Speak info for the Furre Named {...} and
        ''' put it into the PSInfo Cache.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function RemberPSInforFurreNamed(reader As TriggerReader) As Boolean

            Dim furre As String = reader.ReadString
            Return sendServer("ps get characer." + furre + ".*")

        End Function

        ''' <summary>
        ''' (5:60) get All Phoenix Speak info for the triggering furre and
        ''' put it into the PSInfo Cache.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function RemberPSInforTrigFurre(reader As TriggerReader) As Boolean

            Dim furre As String = FurcadiaSession.Player.ShortName
            Return sendServer("ps set characer." + furre + ".*")

        End Function

        ''' <summary>
        ''' (0:17) When someone whispers something with {...} in it,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Protected Overrides Function msgContains(reader As TriggerReader) As Boolean
            Return MyBase.msgContains(reader)
        End Function

        ''' <summary>
        ''' (0:81) When the bot sees the Phoenix Speak response {...},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Protected Overrides Function msgIs(reader As TriggerReader) As Boolean

            Return MyBase.msgIs(reader)

        End Function

        'Protected Overrides Function msgIsNot(reader As TriggerReader) As Boolean

        ' Return MyBase.msgIsNot(reader)

        'End Function

        'Protected Overrides Function msgNotContain(reader As TriggerReader) As Boolean
        '    Return MyBase.msgNotContain(reader)

        'End Function

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