Imports System.Data.SQLite
Imports MonkeyCore
Imports Monkeyspeak
Imports SilverMonkey.PhoenixSpeakSubSystem

Public Class PhoenixSpeakDatabaseSystem
    Inherits Libraries.AbstractBaseLibrary

    Private writer As TextBoxWriter = Nothing
    Public Shared SQLreader As SQLiteDataReader = Nothing
    Private QueryRun As Boolean = False
    Private Shared _SQLitefile As String

    Public Sub New()

        writer = New TextBoxWriter(Variables.TextBox1)
        '(0:500) When the bot starts backing up the character Phoenix Speak,
        Add(TriggerCategory.Cause, 500,
                Function()
                    Return True
                End Function, "(0:500) When the bot starts backing up the character Phoenix Speak,")
        '(0:501) When the bot completes backing up the characters Phoenix Speak,
        Add(TriggerCategory.Cause, 501,
                Function()
                    Return True
                End Function, "(0:501) When the bot completes backing up the characters Phoenix Speak,")
        '(0:502) When the bot starts restoring the Dreams Character Phoenix Speak,
        Add(TriggerCategory.Cause, 502,
                Function()
                    Return True
                End Function, "(0:502) When the bot starts restoring the Dreams Character Phoenix Speak,")
        '(0:503) When the bot finishes restoring the dreams character Phoenix Speak,
        Add(TriggerCategory.Cause, 503,
                Function()
                    Return True
                End Function, "(0:503) When the bot finishes restoring the dreams character Phoenix Speak,")
        Add(TriggerCategory.Cause, 504,
                Function()
                    Return True
                End Function, "(0:504) When the bot backs up Phoenix Speak for any Furre.")
        Add(TriggerCategory.Cause, 505,
                AddressOf BackUpCharacterNamed, "(0:505) When the bot backs up Phoenix Speak for the furre named {...}.")
        Add(TriggerCategory.Cause, 506,
                Function()
                    Return True
                End Function, "(0:506) When the bot restores any furre's Phoenix Speak.")
        Add(TriggerCategory.Cause, 507,
                AddressOf BackUpCharacterNamed, "(0:507) When the bot restores the  Phoenix Speak for the furre named {...}.")

        '(1:520) and the bot is not in the middle of a PS Backup Process
        Add(New Trigger(TriggerCategory.Condition, 520),
                  AddressOf BotBackup, "(1:520) and the bot is not in the middle of a PS Backup Process,")

        '(1:521) and the bot is in the middle of a PS Backup Process.
        Add(New Trigger(TriggerCategory.Condition, 521),
                     AddressOf NotBotBackup, "(1:521) and the bot is in the middle of a PS Backup Process,")

        '(1:522) and the bot is not in the middle of a PS Restore Process,
        Add(New Trigger(TriggerCategory.Condition, 522),
                 AddressOf BotRestore, "(1:522) and the bot is not in the middle of a PS Restore Process,")
        '(1:523) and the bot is in the middle of a PS Restore Process,
        Add(New Trigger(TriggerCategory.Condition, 523),
                 AddressOf NotBotRestore, "(1:523) and the bot is in the middle of a PS Restore Process,")

        'TODO: Add missing PS lines
        'and the backed up phoenix speak database info {...} for the triggering furre exists,
        'and the backed up phoenix speak database info {...} for the furre named {...} exists,  (use ""[DREAM]"" to check specific info for this dream.)
        'and the backed up phoenix speak database info for the triggering furre exists,
        'and the backed up phoenix speak database info for the furre named {...} exists,  (use ""[DREAM]"" to check specific info for this dream.)

        'and the backed up phoenix speak database info {...} for the triggering furre does not exist,
        'and the backed up phoenix speak database info {...} for the furre named {...} does not exist,  (use ""[DREAM]"" to check specific info for this dream.)
        'and the backed up phoenix speak database info for the triggering furre does not exist,
        'and the backed up phoenix speak database info for the furre named {...} does not eist,  (use ""[DREAM]"" to check specific info for this dream.)




        '(5:553) Backup All Character phoenixspeak for the dream
        Add(New Trigger(TriggerCategory.Effect, 553), AddressOf BackupAllPS,
               "(5:553) backup All Phoenix Speak for the dream")
        '(5:554) backup Character named {...} phoenix speak 
        Add(New Trigger(TriggerCategory.Effect, 554), AddressOf BackupSingleCharacterPS,
                   "(5:554) backup character named {...} Phoenix Speak. (use ""[DREAM]"" to restore information specific to the dream)")
        '(5:555) restore phoenix speak for character {...}
        Add(New Trigger(TriggerCategory.Effect, 555), AddressOf RestoreCharacterPS,
                   "(5:555) restore Phoenix Speak for character {...}. (use ""[DREAM]"" to restore information specific to the dream)")
        '(5:556) restore all phoenxi speak characters for this dream.
        Add(New Trigger(TriggerCategory.Effect, 556), AddressOf restoreAllPSData,
                 "(5:556) restore all Phoenix Speak records for this dream.")
        '(5:557) remove Entries older then # days from Phoenix Speak Character backup.
        Add(New Trigger(TriggerCategory.Effect, 557), AddressOf PruneCharacterBackup,
                "(5:557) remove Entries older than # days from Phoenix Speak backup.")
        Add(New Trigger(TriggerCategory.Effect, 558), AddressOf restorePS_DataOldrThanDays,
                "(5:558) restore Phoenix Speak records newer then # days.")

        Add(New Trigger(TriggerCategory.Effect, 560), AddressOf AbortPS,
        "(5:560) abort Phoenix Speak backup or restore process")
    End Sub

    Public Function BackUpCharacterNamed(reader As TriggerReader) As Boolean
        Dim furre As String = reader.ReadString
        Return callbk.Player.ShortName = MainMSEngine.ToFurcShortName(furre)
    End Function

    '(1:520) and the bot is not in the middle of a PS Backup Process
    Public Function BotBackup(reader As TriggerReader) As Boolean

        Try
            Return PhoenixSpeakSubSystem.PsProcess <> PsSystemRunning.PsBackup
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return False
    End Function
    '(1:521) and the bot is in the middle of a PS Backup Process
    Public Function NotBotBackup(reader As TriggerReader) As Boolean
        Try
            Return PhoenixSpeakSubSystem.PsProcess = PsSystemRunning.PsBackup
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return False
    End Function
    '(1:522) and the bot is not in the middle of a PS Restore Process
    Public Function BotRestore(reader As TriggerReader) As Boolean

        Try
            Return PhoenixSpeakSubSystem.PsProcess <> PsSystemRunning.PsRestore
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return False
    End Function
    '(1:523) and the bot is in the middle of a PS Restore Process
    Public Function NotBotRestore(reader As TriggerReader) As Boolean

        Try
            Return PhoenixSpeakSubSystem.PsProcess = PsSystemRunning.PsRestore
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return False
    End Function

    '(5:553) Backup All Character phoenixspeak for the dream
    Function BackupAllPS(reader As TriggerReader) As Boolean
        Try
            If PhoenixSpeakSubSystem.PsProcess <> PsSystemRunning.PsNone Then
                PhoenixSpeakSubSystem.CurrentPS_Stage = PhoenixSpeakSubSystem.PS_BackupStage.GetList
                sendServer("ps get character.*")
            End If
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    '(5:554) backup Character named {...} phoenix speak 
    Function BackupSingleCharacterPS(reader As TriggerReader) As Boolean
        Try
            If PhoenixSpeakSubSystem.PsProcess = PsSystemRunning.PsNone Then

                Dim str As String = reader.ReadString
                If str.ToUpper <> "[DREAM]" Then
                    str = MainMSEngine.ToFurcShortName(str)
                Else
                    str = str.ToUpper
                End If

                Dim f As New PhoenixSpeakSubSystem.PSInfo_Struct
                f.name = str
                f.PS_ID = PhoenixSpeakSubSystem.CharacterList.Count + 1
                PhoenixSpeakSubSystem.CharacterList.Add(f)
                If PhoenixSpeakSubSystem.CurrentPS_Stage <> PhoenixSpeakSubSystem.PS_BackupStage.GetSingle Then
                    PhoenixSpeakSubSystem.CurrentPS_Stage = PhoenixSpeakSubSystem.PS_BackupStage.GetSingle
                    PhoenixSpeakSubSystem.psReceiveCounter = 0
                    PhoenixSpeakSubSystem.psSendCounter = 1
                    PhoenixSpeakSubSystem.PsProcess = PhoenixSpeakSubSystem.PsSystemRunning.PsBackup
                    If str <> "[DREAM]" Then
                        callbk.ServerStack.Enqueue("ps " + PhoenixSpeakSubSystem.CharacterList.Count.ToString + " get character." + str + ".*")
                    Else
                        callbk.ServerStack.Enqueue("ps " + PhoenixSpeakSubSystem.CharacterList.Count.ToString + " get dream.*")
                    End If
                End If
            End If
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function
    '(5:555) restore phoenix speak for character {...}
    Public Function RestoreCharacterPS(reader As TriggerReader) As Boolean

        Try
            Dim furre As String = reader.ReadString()
            PhoenixSpeakSubSystem.Build_PS_CMD(furre, True)
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function
    '(5:556) restore all phoenxi speak characters for this dream.
    Public Function restoreAllPSData(reader As TriggerReader) As Boolean

        Dim str As String = ""

        Try
            If PhoenixSpeakSubSystem.PsProcess = PsSystemRunning.PsNone Then
                PhoenixSpeakSubSystem.RestorePS()
            End If
            Return True
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try

    End Function
    '(5:557) remove Entries older then # days from Phoenix Speak Character backup.

    Public Function PruneCharacterBackup(reader As TriggerReader) As Boolean

        Try
            Dim age As Double = ReadVariableOrNumber(reader)
            If PhoenixSpeakSubSystem.PsProcess = PsSystemRunning.PsNone Then
                PhoenixSpeakSubSystem.PrunePS(age)
            End If
            Return True
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try

    End Function

    '(5:558) restore phoenix speak characters newer then # days.
    Public Function restorePS_DataOldrThanDays(reader As TriggerReader) As Boolean

        Try
            Dim days As Double = ReadVariableOrNumber(reader)
            If PhoenixSpeakSubSystem.PsProcess = PsSystemRunning.PsNone Then
                PhoenixSpeakSubSystem.RestorePS(days)
            End If
            Return True
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try

    End Function

    Public Function AbortPS(reader As TriggerReader) As Boolean
        If PhoenixSpeakSubSystem.PsProcess <> PhoenixSpeakSubSystem.PsSystemRunning.PsNone Then
            PhoenixSpeakSubSystem.PS_Abort()
            callbk.SendClientMessage("SYSTEM:", "Aborted PS Backup/Restore process")
        End If
        Return True
    End Function

#Region "Effects Helper Functions"



    Sub sendServer(ByRef var As String)
        Try
            callbk.sndServer(var)
        Catch ex As Exception
            Dim log As New ErrorLogging(ex, Me)
        End Try
    End Sub

#End Region
End Class
