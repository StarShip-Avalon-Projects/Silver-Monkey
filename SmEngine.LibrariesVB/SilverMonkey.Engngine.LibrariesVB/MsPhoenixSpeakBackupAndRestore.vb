Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Backup and restore a dreams
    ''' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix
    ''' Speak</see> database to Silver Monkey's built in SQLite database
    ''' system. Silver Monkey uses the Command Line interface to walk and
    ''' backup or restore the database for the dream.
    ''' <para>
    ''' NOTE: PhoenixSpeak Database is not SQL based like SQLite. Phoenix
    '''       Speak resembles an XML style system
    ''' </para>
    ''' <pra>Bot Testers: Be aware this class needs to be tested any way possible!</pra>
    ''' </summary>
    ''' <remarks>
    ''' Character List Looping.
    ''' <para>
    ''' first build the character list
    ''' </para>
    ''' <para>
    ''' Send the First PhoenixSpeak Query-set to the server Enqueue
    ''' </para>
    ''' <para>
    ''' PsReceived will read the incoming server responses and keep track
    ''' which mode we're in
    ''' </para>
    ''' <para>
    ''' Read CharacterList(0).name into a variable
    ''' </para>
    ''' <para>
    ''' Remove character at CharacterList(0)
    ''' </para>
    ''' <para>
    ''' Enqueue the Next Phoenix Speak Command
    ''' </para>
    ''' <para>
    ''' Change mode as necessary IE CharacterList.Count = 0
    ''' </para>
    ''' <para>
    ''' PSiInfoCache is the List of PhoenixSpeak.Variables last transmitted
    ''' by the server
    ''' </para>
    ''' <para>
    ''' this list does take into account Multi-Page responses from the
    ''' server and will flag an 'an overflow if page 6 is detected.
    ''' </para>
    ''' </remarks>
    Public NotInheritable Class MsPhoenixSpeakBackupAndRestore
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Overrides Sub Initialize(ParamArray args() As Object)
            MyBase.Initialize(args)
            '(0:500) When the bot starts backing up the character phoenix speak,
            Add(TriggerCategory.Cause, 500,
                Function() True,
                " When the bot starts backing up the character phoenix speak,")
            '(0:501) When the bot completes backing up the characters phoenix speak,
            Add(TriggerCategory.Cause, 501,
                Function()
                    Return True
                End Function, " When the bot completes backing up the characters phoenix speak,")
            '(0:502) When the bot starts restoring the Dreams Character phoenix speak,
            Add(TriggerCategory.Cause, 502,
                Function()
                    Return True
                End Function, " When the bot starts restoring the Dreams Character phoenix speak,")
            '(0:503) When the bot finishes restoring the dreams character phoenix speak,
            Add(TriggerCategory.Cause, 503,
                Function()
                    Return True
                End Function, " When the bot finishes restoring the dreams character phoenix speak,")
            Add(TriggerCategory.Cause, 504,
                Function()
                    Return True
                End Function, " When the bot backs up phoenix speak for any Furre.")
            Add(TriggerCategory.Cause, 505,
                AddressOf BackUpCharacterNamed, " When the bot backs up phoenix speak for the furre named {...}.")
            Add(TriggerCategory.Cause, 506,
                Function()
                    Return True
                End Function, " When the bot restores any furre's phoenix speak.")
            Add(TriggerCategory.Cause, 507,
                AddressOf BackUpCharacterNamed, " When the bot restores the  phoenix speak for the furre named {...}.")

            '(1:520) and the bot is not in the middle of a PS Backup Process
            Add(TriggerCategory.Condition, 520,
                  AddressOf BotBackup, " and the bot is not in the middle of a PS Backup Process,")

            '(1:521) and the bot is in the middle of a PS Backup Process.
            Add(TriggerCategory.Condition, 521,
                     AddressOf NotBotBackup, " and the bot is in the middle of a PS Backup Process,")

            '(1:522) and the bot is not in the middle of a PS Restore Process,
            Add(TriggerCategory.Condition, 522,
                 AddressOf BotRestore, " and the bot is not in the middle of a PS Restore Process,")
            '(1:523) and the bot is in the middle of a PS Restore Process,
            Add(TriggerCategory.Condition, 523,
                 AddressOf NotBotRestore, " and the bot is in the middle of a PS Restore Process,")

            'TODO: Add missing PS lines
            '(1:) and the backed up phoenix speak database info {...} for the triggering furre exists,
            '(1:) and the backed up phoenix speak database info {...} for the furre named {...} exists, (use ""[DREAM]"" to check specific info for this dream.)
            '(1:) and the backed up phoenix speak database info for the triggering furre exists,
            '(1:) and the backed up phoenix speak database info for the furre named {...} exists, (use ""[DREAM]"" to check specific info for this dream.)

            '(1:) and the backed up phoenix speak database info {...} for the triggering furre does not exist,
            '(1:) and the backed up phoenix speak database info {...} for the furre named {...} does not exist, (use ""[DREAM]"" to check specific info for this dream.)
            '(1:) and the backed up phoenix speak database info for the triggering furre does not exist,
            '(1:) and the backed up phoenix speak database info for the furre named {...} does not eist, (use ""[DREAM]"" to check specific info for this dream.)

            '(5:553) Backup All Character phoenixspeak for the dream
            Add(TriggerCategory.Effect, 553, AddressOf BackupAllPS,
               " backup All phoenix speak for the dream")
            '(5:554) backup Character named {...} phoenix speak
            Add(TriggerCategory.Effect, 554, AddressOf BackupSingleCharacterPS,
                   " backup character named {...} phoenix speak. (use ""[DREAM]"" to restore information specific to the dream)")
            '(5:555) restore phoenix speak for character {...}
            Add(TriggerCategory.Effect, 555, AddressOf RestoreCharacterPS,
                   " restore phoenix speak for character {...}. (use ""[DREAM]"" to restore information specific to the dream)")
            '(5:557) remove Entries older then # days from phoenix speak Character backup.
            Add(TriggerCategory.Effect, 557, AddressOf PruneCharacterBackup,
                " remove Entries older than # days from phoenix speak backup.")
            Add(TriggerCategory.Effect, 558, AddressOf RestorePS_DataOldrThanDays,
                " restore phoenix speak character records newer then # days. (zero equals all character records)")

            Add(TriggerCategory.Effect, 560, AddressOf AbortPS,
                " abort phoenix speak backup or restore process")

            '(5:x) Add Settings Info {...} to Database Settings Table {...}.
            '(5:x) update Database Info {...} for Settings Table {...} to {...}.
            '(5:x) remove Database info {...} from Settings Table{...}.
            '(5:x) clear all Settings Table Database info.
            '(5:x) remove setting table {...}.
        End Sub

        Private Function AbortPS(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function RestorePS_DataOldrThanDays(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function PruneCharacterBackup(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function RestoreCharacterPS(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function BackupSingleCharacterPS(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function BackupAllPS(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function NotBotRestore(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function BotRestore(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function NotBotBackup(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function BotBackup(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function BackUpCharacterNamed(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

#End Region

    End Class

End Namespace