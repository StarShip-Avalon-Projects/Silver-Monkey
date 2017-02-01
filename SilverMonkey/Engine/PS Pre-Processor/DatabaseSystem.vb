Imports System.Data.SQLite
Imports System.Collections.Generic
Imports MonkeyCore
Imports Monkeyspeak
Imports MonkeyCore.Utils

Namespace PhoenixSpeak

    ''' <summary>
    ''' Monkey Speak interface to the PS backup/restore system
    ''' </summary>
    Public Class DatabaseSystem
        Inherits Libraries.AbstractBaseLibrary
        Private Shared WithEvents SubSys As New SubSystem

        Private writer As TextBoxWriter = Nothing
        Public Shared SQLreader As SQLiteDataReader = Nothing
        Private QueryRun As Boolean = False

        Private Shared lastItemName As String = String.Empty

        Private Shared _SQLitefile As String = MSPK_MDB.SQLitefile

        ''' <summary>
        ''' Backup/Restore/Prune modes
        ''' </summary>
        <CLSCompliant(False)>
        Public Enum PsBackupStage As SByte
            [error] = -1
            off = 0
            ''' <summary>
            ''' Read Multi Page responses for character list
            ''' </summary>
            GetList = 1
            ''' <summary>
            ''' Read Character list one letter at a time
            ''' <para> Picks up where Get List left Off</para>
            ''' </summary>
            GetAlphaNumericList
            ''' <summary>
            ''' 
            ''' </summary>
            GetTargets
            ''' <summary>
            ''' 
            ''' </summary>
            GetSingle
            ''' <summary>
            ''' 
            ''' </summary>
            RestoreSibgleCharacterPs
            ''' <summary>
            ''' 
            ''' </summary>
            RestoreAllCharacterPS

            ''' <summary>
            ''' Pruning Database
            ''' </summary>
            PruneDatabase

        End Enum

        ''' <summary>
        ''' PS systems running
        ''' </summary>
        <CLSCompliant(False)>
        Public Enum PsSystemRunning As SByte
            [Error] = -1
            PsNone = 0
            PsBackup = 1
            PsRestore = 2
            PsPrune = 3
        End Enum

        ''' <summary>
        ''' Current Mode the Phoenix Speak Subsystem is in.
        ''' </summary>
        ''' <returns></returns>
        <CLSCompliant(False)>
        Public Shared Property CurrentPS_Stage As PsBackupStage

        <CLSCompliant(False)>
        Public Shared Property PsProcess As PsSystemRunning

        ''' <summary>
        ''' List of characters to back up
        ''' <para>special character [DREAM] for information to host dream</para>
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property CharacterList As New List(Of Variable)(20)

        Public Sub New()

            writer = New TextBoxWriter(Variables.TextBox1)
            '(0:500) When the bot starts backing up the character phoenix speak,
            Add(TriggerCategory.Cause, 500,
                Function()
                    Return True
                End Function, "(0:500) When the bot starts backing up the character phoenix speak,")
            '(0:501) When the bot completes backing up the characters phoenix speak,
            Add(TriggerCategory.Cause, 501,
                Function()
                    Return True
                End Function, "(0:501) When the bot completes backing up the characters phoenix speak,")
            '(0:502) When the bot starts restoring the Dreams Character phoenix speak,
            Add(TriggerCategory.Cause, 502,
                Function()
                    Return True
                End Function, "(0:502) When the bot starts restoring the Dreams Character phoenix speak,")
            '(0:503) When the bot finishes restoring the dreams character phoenix speak,
            Add(TriggerCategory.Cause, 503,
                Function()
                    Return True
                End Function, "(0:503) When the bot finishes restoring the dreams character phoenix speak,")
            Add(TriggerCategory.Cause, 504,
                Function()
                    Return True
                End Function, "(0:504) When the bot backs up phoenix speak for any Furre.")
            Add(TriggerCategory.Cause, 505,
                AddressOf BackUpCharacterNamed, "(0:505) When the bot backs up phoenix speak for the furre named {...}.")
            Add(TriggerCategory.Cause, 506,
                Function()
                    Return True
                End Function, "(0:506) When the bot restores any furre's phoenix speak.")
            Add(TriggerCategory.Cause, 507,
                AddressOf BackUpCharacterNamed, "(0:507) When the bot restores the  phoenix speak for the furre named {...}.")

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
               "(5:553) backup All phoenix speak for the dream")
            '(5:554) backup Character named {...} phoenix speak 
            Add(New Trigger(TriggerCategory.Effect, 554), AddressOf BackupSingleCharacterPS,
                   "(5:554) backup character named {...} phoenix speak. (use ""[DREAM]"" to restore information specific to the dream)")
            '(5:555) restore phoenix speak for character {...}
            Add(New Trigger(TriggerCategory.Effect, 555), AddressOf RestoreCharacterPS,
                   "(5:555) restore phoenix speak for character {...}. (use ""[DREAM]"" to restore information specific to the dream)")
            '(5:557) remove Entries older then # days from phoenix speak Character backup.
            Add(New Trigger(TriggerCategory.Effect, 557), AddressOf PruneCharacterBackup,
                "(5:557) remove Entries older than # days from phoenix speak backup.")
            Add(New Trigger(TriggerCategory.Effect, 558), AddressOf restorePS_DataOldrThanDays,
                "(5:558) restore phoenix speak character records newer then # days. (zero equals all character records)")

            Add(New Trigger(TriggerCategory.Effect, 560), AddressOf AbortPS,
        "(5:560) abort phoenix speak backup or restore process")
        End Sub

        Public Function BackUpCharacterNamed(reader As TriggerReader) As Boolean
            Dim furre As String = reader.ReadString
            Return callbk.Player.ShortName = MainMSEngine.ToFurcShortName(furre)
        End Function

        '(1:520) and the bot is not in the middle of a PS Backup Process
        Public Function BotBackup(reader As TriggerReader) As Boolean

            Try
                Return CurrentPS_Stage <> PsSystemRunning.PsBackup
            Catch ex As Exception
                MainMSEngine.LogError(reader, ex)
                Return False
            End Try
            Return False
        End Function
        '(1:521) and the bot is in the middle of a PS Backup Process
        Public Function NotBotBackup(reader As TriggerReader) As Boolean
            Try
                Return CurrentPS_Stage = PsSystemRunning.PsBackup
            Catch ex As Exception
                MainMSEngine.LogError(reader, ex)
                Return False
            End Try
            Return False
        End Function
        '(1:522) and the bot is not in the middle of a PS Restore Process
        Public Function BotRestore(reader As TriggerReader) As Boolean

            Try
                Return CurrentPS_Stage <> PsSystemRunning.PsRestore
            Catch ex As Exception
                MainMSEngine.LogError(reader, ex)
                Return False
            End Try
            Return False
        End Function
        '(1:523) and the bot is in the middle of a PS Restore Process
        Public Function NotBotRestore(reader As TriggerReader) As Boolean

            Try
                Return CurrentPS_Stage = PsSystemRunning.PsRestore
            Catch ex As Exception
                MainMSEngine.LogError(reader, ex)
                Return False
            End Try
            Return False
        End Function

        '(5:553) Backup All Character phoenixspeak for the dream
        Function BackupAllPS(reader As TriggerReader) As Boolean
            Try
                If CurrentPS_Stage = PsSystemRunning.PsNone Then
                    CurrentPS_Stage = PsBackupStage.GetList
                    SubSystem.sendServer("ps get character.*")
                    Return True
                End If
            Catch ex As Exception
                MainMSEngine.LogError(reader, ex)
                Return False
            End Try
            Return False
        End Function

        '(5:554) backup Character named {...} phoenix speak 
        Function BackupSingleCharacterPS(reader As TriggerReader) As Boolean
            Try
                If CurrentPS_Stage = PsSystemRunning.PsNone Then
                    CurrentPS_Stage = PsBackupStage.GetSingle
                    Dim str As String = reader.ReadString
                    lastItemName = str
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
                Build_PS_CMD(furre)
            Catch ex As Exception
                MainMSEngine.LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:557) remove Entries older then # days from phoenix speak Character backup.

        Public Function PruneCharacterBackup(reader As TriggerReader) As Boolean

            Try
                Dim age As Double = ReadVariableOrNumber(reader)
                If CurrentPS_Stage = PsSystemRunning.PsNone Then
                    CurrentPS_Stage = PsBackupStage.PruneDatabase
                    PrunePS(age)

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
                If CurrentPS_Stage = PsSystemRunning.PsNone Then
                    RestorePS(days)
                End If
                Return True
            Catch ex As Exception
                MainMSEngine.LogError(reader, ex)
                Return False
            End Try

        End Function

        Public Function AbortPS(reader As TriggerReader) As Boolean
            If CurrentPS_Stage <> PsSystemRunning.PsNone Then
                Abort()
                SubSystem.ClientMessage("Aborted PS Backup/Restore process")
            End If
            Return True
        End Function

#Region "Backup/Restore"

        ''' <summary>
        ''' Restores Phoenix Speak newer the specified amount a time
        ''' <para>0 Days will restore all records</para>
        ''' </summary>
        ''' <param name="days">Number of Days for new records</param>
        Public Shared Sub RestorePS(days As Double)
            If PsProcess <> PsSystemRunning.PsNone Then Exit Sub
            PsProcess = PsSystemRunning.PsRestore
            CurrentPS_Stage = PsBackupStage.RestoreAllCharacterPS
            '(0:500) When the bot starts backing up the character Phoenix Speak,
            MainMSEngine.MSpage.Execute(502)
            If days > 0 Then
                SubSystem.ClientMessage("Restoring characters newer than " + days.ToString + " days to the dream.")
            Else
                SubSystem.ClientMessage("Restoring all characters for the dream")
            End If
            Dim cmd As String = "select * FROM BACKUPMASTER"
            Dim db As SQLiteDatabase = New SQLiteDatabase(MSPK_MDB.SQLitefile)
            Dim dt As System.Data.DataTable = SQLiteDatabase.GetDataTable(cmd)
            Dim i As Integer = 0
            'Build Commands for each character
            For Each row As System.Data.DataRow In dt.Rows
                i += 1
                Dim ft As String = row.Item("date modified").ToString
                Dim Time As TimeSpan = Main.FurcTime.Subtract(DateTime.Parse(ft))
                If Time.Days <= days OrElse days = 0 Then
                    'Build PS Command sends the command set to the server Enqueue
                    CharacterList.Add(New Variable(row.Item("Name").ToString(), i))
                    Build_PS_CMD(row.Item("Name").ToString())
                End If
            Next
        End Sub

        Private Shared Sub Abort()
            'Reset all PS System Controls
            CurrentPS_Stage = PsBackupStage.off
            PsProcess = PsSystemRunning.PsNone
            CharacterList.Clear()
            SubSystem.Abort()
            LastPSId = 0
        End Sub
        ''' <summary>
        ''' resend PS Command if we don't get a response from the server
        ''' Or skip instruction after failing 4 times
        ''' </summary>
        Private Shared LastPSId As Short = 0


        ''' <summary>
        ''' Build list of Phoenix speak commands for the game server
        ''' Restores Phoenix Speak for one character at a time.
        ''' </summary>
        ''' <param name="str">Character Name</param>
        Public Shared Function Build_PS_CMD(ByRef str As String) As Short
            Dim _id As Short = 0
            Dim Ps_ID As New SubSystem.PsId(_id)

            'Is this a DataBase tool? 
            If String.IsNullOrEmpty(str) Then Return 0
            If str.ToUpper = "[DREAM]" Then
                str = str.ToUpper
            Else
                str = MainMSEngine.ToFurcShortName(str)
            End If


            Dim db As SQLiteDatabase = New SQLiteDatabase(MSPK_MDB.SQLitefile)
            Dim dt As Data.DataTable = SQLiteDatabase.GetDataTable(TableJoinSet("BACKUP", str))
            Dim PsVariableList As New List(Of String)
            Dim NameID As Integer = 0

            'Build the PS Variable List
            For Each row As System.Data.DataRow In dt.Rows
                'Table BACKUP ([NameID] [Key] [Value]
                PsVariableList.Add(String.Format("{0}=""{1}""", row.Item("Key").ToString, row.Item("Value").ToString))
                Integer.TryParse(row.Item("NameID").ToString, NameID)
            Next

            If PsVariableList.Count > 0 Then
                CharacterList.Clear()
                Dim PScmd As String = ""
                Dim Var As New List(Of String)
                Dim str2 As String = ""

                'Loop the PS Variable List until the ps set command is full
                For I As Integer = 0 To PsVariableList.Count - 1
                    Ps_ID = New SubSystem.PsId(_id)
                    Var.Add(PsVariableList.Item(I).ToString)

                    Dim PsVars As String = String.Join(",", Var.ToArray)
                    If str.ToUpper = "[DREAM]" Then
                        PScmd = "ps set dream." + PsVars
                    Else
                        PScmd = "ps set character." + str + "." + PsVars
                    End If

                    Dim CommandToSendOk As Boolean = False
                    If I = PsVariableList.Count - 1 Then
                        CommandToSendOk = True
                    ElseIf PScmd.Length + PsVariableList.Item(I + 1).ToString.Length >= Main.MASS_SPEECH Then
                        CommandToSendOk = True
                    End If
                    If CommandToSendOk Then
                        ' PS set command is full send to PS Out Enqueue
                        SubSystem.sendServer(PScmd, Ps_ID.Id)
                    End If

                Next

            End If
            Return _id
        End Function
#End Region

#Region "Backup Functions"

        'Build Character List
        'read Multipages first (Seems to cap at 6 pages)
        'read *.<letter> till *.z these could be multi pages

        Private Shared Sub PsReceived(ByRef id As Short, ByVal PsType As SubSystem.PsResponseType, ByVal Flag As SubSystem.PsFlag, PageOverFlow As Boolean) Handles SubSys.PsReveived
            'PsProcess = PsSystemRunning.PsBackup
            If PsType = SubSystem.PsResponseType.PsError Then
                Abort()
                Exit Sub
            End If
            Dim ServerCommand = String.Empty
            Select Case CurrentPS_Stage
                Case PsBackupStage.off
                    Exit Select

                Case PsBackupStage.RestoreAllCharacterPS



                Case PsBackupStage.GetList

                    SubSystem.ClientMessage("Backing up Dream Characters Set.")
                    CharacterList.Clear()

                    'Dream specific Information

                    Dim f As New Variable("[DREAM]", "'<none>'")
                    CharacterList.Add(f)
                    '(0:500) When the bot starts backing up the character Phoenix Speak,
                    MainMSEngine.MSpage.Execute(500)
                    If SubSystem.PSInfoCache.Count > 0 Then
                        CharacterList.AddRange(SubSystem.PSInfoCache)
                        lastItemName = CharacterList.Item(CharacterList.Count - 1).Name
                        If PageOverFlow Then
                            lastItemName = Utils.incrementLetter(lastItemName)
                            ServerCommand = "ps get charatcer." + lastItemName + "*"
                            CurrentPS_Stage = PsBackupStage.GetAlphaNumericList
                        Else
                            ServerCommand = "ps get dream.*"
                            CurrentPS_Stage = PsBackupStage.GetTargets
                        End If

                        Exit Select
                    Else
                        CurrentPS_Stage = PsBackupStage.off
                        lastItemName = String.Empty
                    End If
                    Exit Select
                Case PsBackupStage.GetAlphaNumericList
                    ' Grab Characters one at a time based on first letter
                    If SubSystem.PSInfoCache.Count > 0 Then
                        CharacterList.AddRange(SubSystem.PSInfoCache)
                        lastItemName = SubSystem.PSInfoCache.Item(SubSystem.PSInfoCache.Count - 1).Name
                        Utils.incrementLetter(lastItemName)
                        CurrentPS_Stage = PsBackupStage.GetAlphaNumericList
                        ServerCommand = "ps get character." + lastItemName + "*"
                    Else
                        lastItemName = String.Empty
                        CurrentPS_Stage = PsBackupStage.GetTargets
                    End If
                    Exit Select
                Case PsBackupStage.GetTargets
                    SubSystem.ClientMessage("Backing Phoenix Speak for character '" + CharacterList(0).Name + "'")
                    SendPStoDatabase(SubSystem.PSInfoCache, "BACKUP", CharacterList(0).Name)
                    CharacterList.RemoveAt(0)
                    If CharacterList.Count > 0 Then
                        If CharacterList(0).Name <> "[DREAM]" Then
                            ServerCommand = "ps get character." + CharacterList(0).Name + ".*"
                        Else
                            ServerCommand = "ps get dream.*"
                        End If
                    Else
                        CurrentPS_Stage = PsBackupStage.off
                    End If
                    Exit Select
                Case PsBackupStage.GetSingle
                    SubSystem.ClientMessage("Backing Phoenix Speak for character '" + lastItemName + "'")
                    SendPStoDatabase(SubSystem.PSInfoCache, "BACKUP", lastItemName)
                    CurrentPS_Stage = PsBackupStage.off
                    lastItemName = String.Empty
                Case Else

                    CurrentPS_Stage = PsBackupStage.off
                    LastPSId = id

            End Select

            If Not String.IsNullOrEmpty(ServerCommand) Then SubSystem.sendServer(ServerCommand)
            LastPSId = id
            SubSystem.PsId.remove(id)
        End Sub

        Private Shared Function NewPlayer(ByRef Player As String) As Dictionary(Of String, String)
            Dim Dta As New Dictionary(Of String, String)
            If Player.ToUpper = "[DREAM]" Then
                Player = "[DREAM]"
            End If
            Dta.Add("Name", Player)
            Dta.Add("date modified", Main.FurcTime.ToString)
            Return Dta
        End Function

        ''' <summary>
        ''' Sends PS Info to SQLite Database
        ''' </summary>
        ''' <param name="s"></param>
        ''' <returns>True on success</returns>
        Private Shared Function SendPStoDatabase(PsInfo As List(Of PhoenixSpeak.Variable), TableSet As String, PlayerName As String) As Boolean
            Dim dta As Dictionary(Of String, String) = NewPlayer(PlayerName)
            If PlayerName.ToUpper = "[DREAM]" Then
                PlayerName = "[DREAM]"
            Else
                PlayerName = MainMSEngine.ToFurcShortName(PlayerName)
            End If

            Dim Data As New Dictionary(Of String, String)

            For Each var As PhoenixSpeak.Variable In PsInfo
                Data.Add(var.Name, var.Value.ToString)
            Next


            Dim db As New SQLiteDatabase(MSPK_MDB.SQLitefile)

            '''Check for special character to store Dream PS tree
            Data = SystemDateFixer(TableSet, PlayerName, Data)

            Dim idx As Integer = 0
            Dim NameID As String = "SELECT [ID] FROM " + TableSet + "MASTER Where [Name]='" & PlayerName & "'"
            Dim RecordExist As Boolean = Integer.TryParse(SQLiteDatabase.ExecuteScalar1(NameID), idx)

            'Lets check the Database for a record first.
            ' If it exists we'll update the current record with new info
            If RecordExist Then
                db.Update("" + TableSet + "MASTER", dta, "[Name]='" + PlayerName + "'")
                SQLiteDatabase.ExecuteNonQuery("DELETE FROM '" + TableSet + "' WHERE [NameID]=" + idx.ToString)
            Else
                'Inserting a new record? Lets make sure it has the right name for
                ' for the MASTER Table
                db.Insert("" + TableSet + "MASTER", dta)
                Integer.TryParse(SQLiteDatabase.ExecuteScalar1(NameID), idx)
            End If
            Return SQLiteDatabase.InsertMultiRow(TableSet, idx, Data)
        End Function




        Private Shared Function TableJoinSet(ByVal TableSet As String, ByVal Name As String) As String
            'Retrieve Phoenix Speak Variables from the database
            Return _
            "select " + TableSet + ".*, " + TableSet + "MASTER.ID from " + TableSet + " " +
            "inner join " + TableSet + "MASTER on " +
            "" + TableSet + "MASTER.ID = " + TableSet + ".NameID " +
            "where " + TableSet + "MASTER.Name = '" + Name + "' "
        End Function
        '
        Private Shared Function SystemDateFixer(ByVal Table As String, ByVal PlayerName As String, ByRef dta As Dictionary(Of String, String)) As Dictionary(Of String, String)
            If Table <> "BACKUP" Then Return dta
            'retrieve characters Last Used date from PS for Use in our " + TableSet  + " tables
            Dim TimeItem As String = "sys_lastused_date"
            Dim fDate As Double = 0
            If dta.ContainsKey(TimeItem) Then
                Double.TryParse(dta.Item(TimeItem), fDate)
                If fDate = 0 Then
                    If PlayerName.ToUpper <> "[DREAM]" Then
                        dta.Item(TimeItem) = DateTimeToUnixTimestamp(Main.FurcTime).ToString
                    End If
                Else
                    'Add current Unix Time Stamp as default
                    dta.Item(TimeItem) = UnixTimeStampToDateTime(fDate).ToString
                End If
            End If
            Return dta
        End Function


#End Region

#Region "Pruning"
        ''' <summary>
        ''' Prunes Phoenix Speak Backup database
        ''' </summary>
        ''' <param name="NumDays"></param>
        ''' <returns>Number of records removed</returns>
        Public Function PrunePS(ByRef NumDays As Double) As Double
            Dim start As Date = Date.Now
            Dim Table As String = "BACKUP"
            SubSystem.ClientMessage("Pruning Backup Database")
            Dim cmd2 As String = "SELECT * FROM " + Table + "MASTER"
            Dim db As SQLiteDatabase = New SQLiteDatabase(MSPK_MDB.SQLitefile)
            Dim dt As System.Data.DataTable = SQLiteDatabase.GetDataTable(cmd2)
            Dim result As String = ""
            Dim Counter As Integer = 0
            For Each row As System.Data.DataRow In dt.Rows
                Dim idx As Integer = Integer.Parse(row.Item("ID").ToString)
                Dim ts As TimeSpan = Main.FurcTime.Subtract(DateTime.Parse(row.Item("date modified").ToString))
                If ts.Days >= NumDays Or NumDays = 0 Then
                    SQLiteDatabase.ExecuteNonQuery("DELETE FROM '" + Table + "' WHERE [NameID]=" + idx.ToString)
                    SQLiteDatabase.ExecuteNonQuery("DELETE FROM '" + Table + "MASTER' WHERE [ID]=" + idx.ToString)
                    Counter += 1
                End If
            Next
            SQLiteDatabase.ExecuteNonQuery("VACUUM")
            Dim ts2 As TimeSpan = Date.Now.Subtract(start)
            SubSystem.ClientMessage("Pruning completed. Removed " + Counter.ToString + "Characters. Time Elapsed: " + ts2.Minutes.ToString + " minutes")
            Return Counter
        End Function
#End Region
    End Class
End Namespace