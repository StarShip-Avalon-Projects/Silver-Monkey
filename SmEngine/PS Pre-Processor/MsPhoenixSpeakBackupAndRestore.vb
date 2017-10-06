Imports System.Data.SQLite
Imports System.Threading
Imports Furcadia.Util
Imports MonkeyCore
Imports MonkeyCore.Utils
Imports Monkeyspeak
Imports SilverMonkeyEngine.Engine.Libraries.PhoenixSpeak

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
    ''' ''' <pra>Bot Testers: Be aware this class needs to be tested any way possible!</pra>
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
    Public Class MsPhoenixSpeakBackupAndRestore
        Inherits MonkeySpeakLibrary

#Region "Public Fields"

        Public Shared SQLreader As SQLiteDataReader = Nothing

#End Region

#Region "Private Fields"

        Private _SQLitefile As String = MsDatabase.SQLitefile
        Private lastItemName As String = String.Empty

        Private QueryRun As Boolean = False

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)

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
            '(1:) and the backed up phoenix speak database info {...} for the triggering furre exists,
            '(1:) and the backed up phoenix speak database info {...} for the furre named {...} exists, (use ""[DREAM]"" to check specific info for this dream.)
            '(1:) and the backed up phoenix speak database info for the triggering furre exists,
            '(1:) and the backed up phoenix speak database info for the furre named {...} exists, (use ""[DREAM]"" to check specific info for this dream.)

            '(1:) and the backed up phoenix speak database info {...} for the triggering furre does not exist,
            '(1:) and the backed up phoenix speak database info {...} for the furre named {...} does not exist, (use ""[DREAM]"" to check specific info for this dream.)
            '(1:) and the backed up phoenix speak database info for the triggering furre does not exist,
            '(1:) and the backed up phoenix speak database info for the furre named {...} does not eist, (use ""[DREAM]"" to check specific info for this dream.)

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

            '(5:x) Add Settings Info {...} to Database Settings Table {...}.
            '(5:x) update Database Info {...} for Settings Table {...} to {...}.
            '(5:x) remove Database info {...} from Settings Table{...}.
            '(5:x) clear all Settings Table Database info.
            '(5:x) remove setting table {...}.
        End Sub

#End Region

#Region "Public Enums"

        Public Enum PsBackupStage As Byte
            [error] = 0
            off = 1

            GetDream

            ''' <summary>
            ''' Read Multi Page responses for character list
            ''' </summary>
            GetList

            ''' <summary>
            ''' Read Character list one letter at a time
            ''' <para>
            ''' Picks up where Get List left Off
            ''' </para>
            ''' </summary>
            GetAlphaNumericList

            ''' <summary>
            ''' </summary>
            GetTargets

            ''' <summary>
            ''' </summary>
            GetSingle

            ''' <summary>
            ''' </summary>
            RestoreSibgleCharacterPs

            ''' <summary>
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
            [Error] = 0
            PsNone
            PsBackup
            PsRestore
            PsPrune
        End Enum

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' List of characters to back up
        ''' <para>
        ''' special character [DREAM] for information to host dream
        ''' </para>
        ''' </summary>
        ''' <returns>
        ''' </returns>
        Public Property CharacterList As New List(Of PhoenixSpeak.Variable)(20)

        Public Property CurrentPS_Stage As PsBackupStage

        <CLSCompliant(False)>
        Public Property PsProcess As PsSystemRunning

#End Region

#Region "Public Methods"

        Public Function AbortPS(reader As TriggerReader) As Boolean
            If CurrentPS_Stage <> PsSystemRunning.PsNone Then
                Abort()
                SendClientMessage("Aborted PS Backup/Restore process")
            End If
            Return True
        End Function

        ''' <summary>
        ''' (5:553) Backup All Character phoenix speak for the dream
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function BackupAllPS(reader As TriggerReader) As Boolean

            If CurrentPS_Stage = PsSystemRunning.PsNone Then
                CurrentPS_Stage = PsBackupStage.GetDream
                FurcadiaSession.SendToServer("ps get dream.*")
                Return True
            End If

            Return False
        End Function

        Public Function BackUpCharacterNamed(reader As TriggerReader) As Boolean
            Dim furre As String = reader.ReadString
            Return FurcadiaSession.Player.ShortName = FurcadiaShortName(furre)
        End Function

        ''' <summary>
        ''' (5:554) backup Character named {...} phoenix speak.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function BackupSingleCharacterPS(reader As TriggerReader) As Boolean

            If CurrentPS_Stage = PsSystemRunning.PsNone Then
                CurrentPS_Stage = PsBackupStage.GetSingle
                lastItemName = reader.ReadString
            End If

            Return True
        End Function

        ''' <summary>
        ''' (1:520) and the bot is not in the middle of a PS Backup Process
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function BotBackup(reader As TriggerReader) As Boolean

            Return CurrentPS_Stage <> PsSystemRunning.PsBackup

        End Function

        ''' <summary>
        ''' (1:522) and the bot is not in the middle of a PS Restore Process
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function BotRestore(reader As TriggerReader) As Boolean

            Return CurrentPS_Stage <> PsSystemRunning.PsRestore

        End Function

        ''' <summary>
        ''' (1:521) and the bot is in the middle of a PS Backup Process
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function NotBotBackup(reader As TriggerReader) As Boolean

            Return CurrentPS_Stage = PsSystemRunning.PsBackup

        End Function

        ''' <summary>
        ''' '(1:523) and the bot is in the middle of a PS Restore Process
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function NotBotRestore(reader As TriggerReader) As Boolean

            Return CurrentPS_Stage = PsSystemRunning.PsRestore

        End Function

        Public Function PruneCharacterBackup(reader As TriggerReader) As Boolean

            Dim age As Double = ReadVariableOrNumber(reader)
            If CurrentPS_Stage = PsSystemRunning.PsNone Then
                CurrentPS_Stage = PsBackupStage.PruneDatabase
                PrunePS(age)

            End If
            Return True

        End Function

        ''' <summary>
        ''' (5:555) restore phoenix speak for character {...}
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function RestoreCharacterPS(reader As TriggerReader) As Boolean

            Dim furre As String = reader.ReadString()
            Return 0 < Build_PS_CMD(furre)

        End Function

        ''' <summary>
        ''' (5:557) remove Entries older then # days from phoenix speak
        ''' Character backup.
        ''' <para>
        ''' (5:558) restore phoenix speak characters newer then # days.
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function restorePS_DataOldrThanDays(reader As TriggerReader) As Boolean

            Dim days As Double = ReadVariableOrNumber(reader)
            If CurrentPS_Stage = PsSystemRunning.PsNone Then
                CurrentPS_Stage = PsBackupStage.RestoreAllCharacterPS
                RestorePS(days)
            End If
            Return True

        End Function

#End Region

#Region "Backup/Restore"

        ''' <summary>
        ''' resend PS Command if we don't get a response from the server Or
        ''' skip instruction after failing 4 times
        ''' </summary>
        Private Shared LastPSId As Short = 0

        ''' <summary>
        ''' Build list of Phoenix speak commands for the game server
        ''' Restores Phoenix Speak for one character at a time.
        ''' </summary>
        ''' <param name="str">
        ''' Character Name
        ''' </param>
        Public Function Build_PS_CMD(ByRef str As String) As Short

            'Is this a DataBase tool?
            If String.IsNullOrEmpty(str) Then Return 0
            If str.ToUpper = "[DREAM]" Then
                str = str.ToUpper
            Else
                str = FurcadiaShortName(str)
            End If

            Dim db As SQLiteDatabase = New SQLiteDatabase(MsDatabase.SQLitefile)
            Dim dt As Data.DataTable = SQLiteDatabase.GetDataTable(TableJoinSet("BACKUP", str))
            Dim PsVariableList As New List(Of String)
            Dim NameID As Integer = 0

            'Build the PS Variable List
            For Each row As System.Data.DataRow In dt.Rows
                'Table BACKUP ([NameID] [Key] [Value]
                PsVariableList.Add(String.Format("{0}=""{1}""", row.Item("Key").ToString, row.Item("Value").ToString))
                Integer.TryParse(row.Item("NameID").ToString, NameID)
            Next
            Dim _id As Short = 0
            If PsVariableList.Count > 0 Then

                Dim Ps_ID As New PsId(_id)
                Dim PScmd As String = String.Empty
                Dim Var As New List(Of String)
                Dim str2 As String = String.Empty

                'Loop the PS Variable List until the ps set command is full
                For I As Integer = 0 To PsVariableList.Count - 1

                    Var.Add(PsVariableList.Item(I).ToString)

                    Dim PsVars As String = String.Join(",", Var.ToArray)
                    If str.ToUpper = "[DREAM]" Then
                        PScmd = "ps " + Ps_ID.Id.ToString + "  set dream." + PsVars
                    Else
                        PScmd = "ps " + Ps_ID.Id.ToString + " set character." + str + "." + PsVars
                    End If

                    Dim CommandToSendOk As Boolean = False
                    If I = PsVariableList.Count - 1 Then
                        CommandToSendOk = True
                    ElseIf PScmd.Length + PsVariableList.Item(I + 1).ToString.Length >= 1000 Then 'MASS_SPEECH
                        CommandToSendOk = True
                    End If
                    If CommandToSendOk Then
                        ' PS set command is full send to PS Out Enqueue
                        sendServer(PScmd)
                    End If

                Next

            End If
            Return _id
        End Function

        ''' <summary>
        ''' Restores Phoenix Speak newer the specified amount a time
        ''' <para>
        ''' 0 Days will restore all records
        ''' </para>
        ''' </summary>
        ''' <param name="days">
        ''' Number of Days for new records
        ''' </param>
        Public Sub RestorePS(ByVal days As Double)
            If PsProcess <> PsSystemRunning.PsNone Then Exit Sub
            PsProcess = PsSystemRunning.PsRestore
            '(0:500) When the bot starts backing up the character Phoenix Speak,
            FurcadiaSession.MSpage.Execute(502)
            If days > 0 Then
                SendClientMessage("Restoring characters newer than " + days.ToString + " days to the dream.")
            Else
                SendClientMessage("Restoring all characters for the dream")
            End If
            Dim cmd As String = "select * FROM BACKUPMASTER"
            Dim db As SQLiteDatabase = New SQLiteDatabase(MsDatabase.SQLitefile)
            Dim dt As System.Data.DataTable = SQLiteDatabase.GetDataTable(cmd)
            Dim i As Integer = 0
            'Build Commands for each character
            For Each row As System.Data.DataRow In dt.Rows
                i += 1
                Dim ft As String = row.Item("date modified").ToString
                Dim Time As TimeSpan = FurcTime.Subtract(DateTime.Parse(ft))

                Dim CharacternName As String = row.Item("Name").ToString()
                Dim CharId As Integer = 0
                If Time.Days <= days Then
                    CharId = Build_PS_CMD(CharacternName)
                ElseIf days = 0 Then
                    CharId = Build_PS_CMD(CharacternName)
                End If
                If CharId > 0 Then
                    CharacterList.Add(New PhoenixSpeak.Variable(CharacternName, CharId))
                End If

            Next
        End Sub

        Private Sub Abort()
            'Reset all PS System Controls
            CurrentPS_Stage = PsBackupStage.off
            PsProcess = PsSystemRunning.PsNone
            CharacterList.Clear()
            'Abort()
            LastPSId = 0
        End Sub

#End Region

#Region "Backup Functions"

        Private Shared OjbLock As New Object

        'Build Character List
        'read Multipages first (Seems to cap at 6 pages)
        'read *.<letter> till *.z these could be multi pages
        Private PSProcessingResource As Integer = 0

        Private Shared Function TableJoinSet(ByVal TableSet As String, ByVal Name As String) As String
            'Retrieve Phoenix Speak Variables from the database
            Dim str As String =
            "select " + TableSet + ".*, " + TableSet + "MASTER.ID from " + TableSet + " " +
            "inner join " + TableSet + "MASTER on " +
String.Empty + TableSet + "MASTER.ID = " + TableSet + ".NameID " +
            "where " + TableSet + "MASTER.Name = '" + Name + "' "
            Return str
        End Function

        Private Function NewPlayer(ByRef Player As String) As Dictionary(Of String, String)
            Dim Dta As New Dictionary(Of String, String)
            If Player.ToUpper = "[DREAM]" Then
                Player = "[DREAM]"
            End If
            Dta.Add("Name", Player)
            Dta.Add("date modified", FurcTime.ToString)
            Return Dta
        End Function

        Private Sub PsReceived(o As Object, e As PhoenixSpeakEventArgs) 'Handles SubSys.PsReceived
            'PsProcess = PsSystemRunning.PsBackup
            Dim ServerCommand = String.Empty
            If e.PsType = SubSystem.PsFlag.PsError Then
                Abort()
                Exit Sub
            End If
            Dim PSiInfoCache As List(Of PhoenixSpeak.Variable) = CType(o, List(Of PhoenixSpeak.Variable))

            Select Case CurrentPS_Stage
                Case PsBackupStage.off
                    Exit Select

                Case PsBackupStage.RestoreAllCharacterPS
                    SendClientMessage("Restoring Phoenix Speak for character '" + CharacterList(0).Name + "'")
                    'SendPStoDatabase(PSiInfoCache, "BACKUP", CharacterList(0).Name)
                    CharacterList.RemoveAt(0)
                    If CharacterList.Count > 0 Then
                        CurrentPS_Stage = PsBackupStage.off
                        lastItemName = String.Empty
                    End If
                Case PsBackupStage.GetDream
                    If PSiInfoCache.Count > 0 Then
                        'Dream specific Information
                        SendClientMessage("Backing up Dream Characters Set.")
                        '(0:500) When the bot starts backing up the character Phoenix Speak,
                        FurcadiaSession.MSpage.Execute(500)
                        CharacterList.Clear()
                        Dim f As New PhoenixSpeak.Variable("[DREAM]", "'<none>'")
                        CharacterList.Add(f)
                        SendClientMessage("Backing Phoenix Speak for character '" + CharacterList(0).Name + "'")
                        SendPStoDatabase(PSiInfoCache, "BACKUP", CharacterList(0).Name)
                        CharacterList.RemoveAt(0)

                    End If
                    ServerCommand = "ps get character.*"
                    CurrentPS_Stage = PsBackupStage.GetList
                    Exit Select
                Case PsBackupStage.GetList
                    If PSiInfoCache.Count > 0 Then
                        CharacterList.AddRange(PSiInfoCache)
                        lastItemName = CharacterList.Item(CharacterList.Count - 1).Name

                        If e.PageOverFlow Then
                            lastItemName = Utilities.IncrementLetter(lastItemName)
                            ServerCommand = "ps get character." + lastItemName + "*"
                            CurrentPS_Stage = PsBackupStage.GetAlphaNumericList
                        Else
                            CurrentPS_Stage = PsBackupStage.GetTargets
                            ServerCommand = "ps get character." + CharacterList(0).Name + ".*"
                        End If

                        Exit Select
                    Else
                        CurrentPS_Stage = PsBackupStage.off
                        lastItemName = String.Empty
                    End If
                    Exit Select
                Case PsBackupStage.GetAlphaNumericList
                    ' Grab Characters one at a time based on first letter
                    If PSiInfoCache.Count > 0 Then
                        CharacterList.AddRange(PSiInfoCache)
                        lastItemName = PSiInfoCache.Item(PSiInfoCache.Count - 1).Name
                        Utilities.IncrementLetter(lastItemName)
                        CurrentPS_Stage = PsBackupStage.GetAlphaNumericList
                        ServerCommand = "ps get character." + lastItemName + "*"
                    Else
                        lastItemName = String.Empty
                        CurrentPS_Stage = PsBackupStage.GetTargets
                    End If
                    Exit Select
                Case PsBackupStage.GetTargets
                    SendClientMessage("Backing Phoenix Speak for character '" + CharacterList(0).Name + "'")
                    SendPStoDatabase(PSiInfoCache, "BACKUP", CharacterList(0).Name)
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
                    SendClientMessage("Backing Phoenix Speak for character '" + lastItemName + "'")
                    SendPStoDatabase(PSiInfoCache, "BACKUP", lastItemName)
                    CurrentPS_Stage = PsBackupStage.off
                    lastItemName = String.Empty
                Case Else

                    CurrentPS_Stage = PsBackupStage.off
                    LastPSId = e.id

            End Select

            If Not String.IsNullOrEmpty(ServerCommand) Then FurcadiaSession.SendToServer(ServerCommand)
            LastPSId = e.id

            'SubSystem.PsId.remove(id)
        End Sub

        ''' <summary>
        ''' Sends PS Info to SQLite Database
        ''' </summary>
        ''' <param name="PsInfo">
        ''' </param>
        ''' <param name="TableSet">
        ''' </param>
        ''' <param name="PlayerName">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function SendPStoDatabase(PsInfo As List(Of PhoenixSpeak.Variable), TableSet As String, PlayerName As String) As Boolean
            Dim Returnval As Boolean
            Dim idx As Integer = 0
            PlayerName = PlayerName.Trim
            Dim Data As New Dictionary(Of String, String)
            Try
                Monitor.Enter(OjbLock)

                Dim dta As Dictionary(Of String, String) = NewPlayer(PlayerName)
                If PlayerName.ToUpper = "[DREAM]" Then
                    PlayerName = "[DREAM]"
                Else
                    PlayerName = FurcadiaShortName(PlayerName)
                End If

                For Each var As PhoenixSpeak.Variable In PsInfo
                    Data.Add(var.Name, var.Value.ToString)
                Next

                Dim db As New SQLiteDatabase(MsDatabase.SQLitefile)

                'Check for special character to store Dream PS tree
                Data = SystemDateFixer(TableSet, PlayerName, Data)

                Dim NameID As String = "SELECT [ID] FROM " + TableSet + "MASTER Where [Name]='" & PlayerName & "'"

                db.Insert(String.Empty + TableSet + "MASTER", dta)
                Dim RecordExist As Boolean = Integer.TryParse(SQLiteDatabase.ExecuteScalar(NameID), idx)

                'Lets check the Database for a record first.
                ' If it exists we'll update the current record with new info
                If RecordExist Then
                    db.Update(String.Empty + TableSet + "MASTER", dta, "[Name]='" + PlayerName + "'")
                    SQLiteDatabase.ExecuteNonQuery("DELETE FROM '" + TableSet + "' WHERE [NameID]=" + idx.ToString)
                Else
                    'Inserting a new record? Lets make sure it has the right name for
                    ' for the MASTER Table
                End If
                Returnval = SQLiteDatabase.InsertMultiRow(TableSet, idx, Data)
            Finally
                Monitor.Exit(OjbLock)

            End Try

            Return Returnval
        End Function

        '
        Private Function SystemDateFixer(ByVal Table As String, ByVal PlayerName As String, ByRef dta As Dictionary(Of String, String)) As Dictionary(Of String, String)
            If Table <> "BACKUP" Then Return dta
            'retrieve characters Last Used date from PS for Use in our " + TableSet  + " tables
            Dim TimeItem As String = "sys_lastused_date"
            Dim fDate As Double = 0
            If dta.ContainsKey(TimeItem) Then
                Double.TryParse(dta.Item(TimeItem), fDate)
                If fDate = 0 Then
                    If PlayerName.ToUpper <> "[DREAM]" Then
                        dta.Item(TimeItem) = Utilities.DateTimeToUnixTimestamp(FurcTime).ToString
                    End If
                Else
                    'Add current Unix Time Stamp as default
                    dta.Item(TimeItem) = Utilities.UnixTimeStampToDateTime(fDate).ToString
                End If
            End If
            Return dta
        End Function

#End Region

#Region "Pruning"

        ''' <summary>
        ''' Prunes Phoenix Speak Backup database
        ''' </summary>
        ''' <param name="NumDays">
        ''' </param>
        ''' <returns>
        ''' Number of records removed
        ''' </returns>
        Public Function PrunePS(ByRef NumDays As Double) As Double
            Dim start As Date = Date.Now
            Dim Table As String = "BACKUP"
            SendClientMessage("Pruning Backup Database")
            Dim cmd2 As String = "SELECT * FROM " + Table + "MASTER"
            Dim db As SQLiteDatabase = New SQLiteDatabase(MsDatabase.SQLitefile)
            Dim dt As System.Data.DataTable = SQLiteDatabase.GetDataTable(cmd2)
            Dim result As String = String.Empty
            Dim Counter As Integer = 0
            For Each row As System.Data.DataRow In dt.Rows
                Dim idx As Integer = Integer.Parse(row.Item("ID").ToString)
                Dim ts As TimeSpan = FurcTime.Subtract(DateTime.Parse(row.Item("date modified").ToString))
                If ts.Days >= NumDays Or NumDays = 0 Then
                    SQLiteDatabase.ExecuteNonQuery("DELETE FROM '" + Table + "' WHERE [NameID]=" + idx.ToString)
                    SQLiteDatabase.ExecuteNonQuery("DELETE FROM '" + Table + "MASTER' WHERE [ID]=" + idx.ToString)
                    Counter += 1
                End If
            Next
            SQLiteDatabase.ExecuteNonQuery("VACUUM")
            Dim ts2 As TimeSpan = Date.Now.Subtract(start)
            SendClientMessage("Pruning completed. Removed " + Counter.ToString + "Characters. Time Elapsed: " + ts2.Minutes.ToString + " minutes")
            Return Counter
        End Function

#End Region

    End Class

End Namespace