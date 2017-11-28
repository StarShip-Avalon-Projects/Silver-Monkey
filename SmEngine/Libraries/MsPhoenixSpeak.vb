Imports System.Text
Imports System.Text.RegularExpressions
Imports Monkeyspeak
Imports SilverMonkeyEngine.Engine.Libraries.PhoenixSpeak

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
    Public NotInheritable Class MsPhoenixSpeak
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Sub New(session As BotSession)
            MyBase.New(session)
        End Sub

        Public Overrides Sub Initialize(ParamArray args() As Object)
            Add(TriggerCategory.Cause, 80,
                Function() True, " When the bot sees a Phoenix Speak response")
            Add(TriggerCategory.Cause, 81,
                AddressOf MsgIs, " When the bot sees the Phoenix Speak response {...},")

            Add(TriggerCategory.Cause, 82,
                AddressOf MsgContains, " When the bot sees a Phoenix Speak response with {...} in it,")

            'TODO: Add Monkeu Speak
            '(1xxx) And the Database info {...} about the triggerig furre exists.
            '(1:xxx) And the Database info {...} about the triggerig furre does Not exist.
            '(1:xxx) And the Database info {...} about the furre named {...} exists.
            '(1:xxx) And the Database info {...} about the furre named {...} does Not exist.

            '(5:60) get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect, 60, AddressOf RemberPSInforTrigFurre,
                " get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.")

            '(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect, 61, AddressOf RemberPSInforFurreNamed,
                " get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.")

            '(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect, 62, AddressOf RemberPSInfoAllDream,
                " get All Phoenix Speak info for the dream and put it into the PSInfo Cache.")

            '(5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.
            Add(TriggerCategory.Effect, 63, AddressOf RemberPSInfoAllCharacters,
                " get all list of all characters and put it into the PSInfo cache.")

            '(5:80) retrieve  Phoenix Speak info {...} and place the value into variable %Variable.
            Add(TriggerCategory.Effect, 80, AddressOf getPSinfo,
                " retrieve  Phoenix Speak info {...} and place the value into variable %Variable.")

            '(5:81) Store PSInfo Key Names to Variable %Variable.
            Add(TriggerCategory.Effect, 81, AddressOf PSInfoKeyToVariable,
                " Store PSInfo Key Names to Variable %Variable.")

            '(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.
            Add(TriggerCategory.Effect, 82, AddressOf MemorizeFurreNamedPS,
                " Memorize Phoenix Speak info {...} for the Furre Named {...}.")

            '(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.
            Add(TriggerCategory.Effect, 83, AddressOf ForgetFurreNamedPS,
                " Forget Phoenix Speak info {...} for the Furre Named {...}.")

            '(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.
            Add(TriggerCategory.Effect, 84, AddressOf MemorizeTrigFurrePS,
                " Memorize Phoenix Speak info {...} for the Triggering Furre.")

            '(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.
            Add(TriggerCategory.Effect, 85, AddressOf ForgetTrigFurrePS,
                " Forget Phoenix Speak info {...} for the Triggering Furre.")

            '(5:90) Memorize Phoenix Speak info {...} for this dream.
            Add(TriggerCategory.Effect, 90, AddressOf MemorizeDreamPS,
                " Memorize Phoenix Speak info {...} for this dream.")
            '(5:91) Forget Phoenix Speak info {...} for this dream.
            Add(TriggerCategory.Effect, 91, AddressOf ForgetDreamPS,
                " Forget Phoenix Speak info {...} for this dream.")

            '(5:94) execute Phoenix Speak command {...}.
            Add(TriggerCategory.Effect, 94,
                AddressOf PSCommand, " execute Phoenix Speak command {...}.")
            Add(TriggerCategory.Effect, 95,
                AddressOf PSForgetTriggeringFurre,
                " Forget ALL Phoenix Speak info for the triggering furre")

            Add(TriggerCategory.Effect, 96,
                AddressOf PSForgetFurreNamed,
                " Forget ALL Phoenix Speak info for the furre named {...}.")
            Add(TriggerCategory.Effect, 97,
                AddressOf PSForgetDream,
                " Forget ALL Phoenix Speak info for this dream.")

        End Sub

#End Region

#Region "Public Methods"

        Private Function PSForgetDream(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function PSForgetFurreNamed(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function PSForgetTriggeringFurre(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function PSCommand(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function ForgetDreamPS(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function MemorizeDreamPS(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function ForgetTrigFurrePS(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function MemorizeTrigFurrePS(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function ForgetFurreNamedPS(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function MemorizeFurreNamedPS(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function PSInfoKeyToVariable(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function getPSinfo(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function RemberPSInfoAllCharacters(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function RemberPSInfoAllDream(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function RemberPSInforFurreNamed(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function RemberPSInforTrigFurre(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

#End Region

        Public Overrides Sub Unload(page As Page)

        End Sub

    End Class

End Namespace