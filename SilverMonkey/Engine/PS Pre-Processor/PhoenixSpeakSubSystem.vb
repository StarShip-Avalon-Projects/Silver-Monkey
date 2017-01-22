Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports MonkeyCore

Public Class PhoenixSpeakSubSystem

    Public Enum PsSystemRunning
        PsError = -1
        PsNone = 0
        PsBackup = 1
        PsRestore = 2
        PsPrune = 3
    End Enum

    Public Shared PsProcess As PsSystemRunning

    Public Class PSInfo_Struct
        Public Property name As String
        Public Property Values As Dictionary(Of String, String)
        Public Property PS_ID As Integer
        Public Sub New()
            _name = ""
            _Values = New Dictionary(Of String, String)
            _PS_ID = 0
        End Sub
    End Class

    Public Shared CharacterList As List(Of PSInfo_Struct) = New List(Of PSInfo_Struct)
    Public Enum PS_BackupStage
        off = 0
        GetList = 1
        GetAlphaNumericList
        GetTargets
        GetSingle
        RestoreAllCharacterPS
    End Enum
    Public Shared CurrentPS_Stage As PS_BackupStage = PS_BackupStage.off


    Public Shared PSinfo As New Dictionary(Of String, String)
    Public Shared PS_Page As String = ""

    Public Shared PSS_Stack As New List(Of PSS_Struct)
    Public LastPS_ID As Integer = 0
    Private Shared pslen As Integer = 0
    Private Shared NextLetter As Char = Nothing

    Public Class PSS_Struct
        Public Property Name As String
        Public Property ID As Int16
        Public Property Cmd As String
        Public Sub New()
            Name = ""
            ID = 0
            Cmd = ""
        End Sub
    End Class



    'resend PS Command if we don't get a response from the server
    'Or skip insruction after failing 4 times
    Private Shared PSLock2 As New Object
    Private Shared LastSentPS As Integer
    Public Shared Sub CheckPS_Send()
        If CurrentPS_Stage <> PS_BackupStage.RestoreAllCharacterPS Then Exit Sub
        If callbk.ThroatTired Then Exit Sub
        If PSS_Stack.Count >= psSendCounter Then Exit Sub
        SyncLock PSLock2
            Select Case LastSentPS
                Case 4 Or 8 Or 12 Or 16
                    Dim ps As New PSS_Struct
                    ps = PSS_Stack(psSendCounter - 1)
                    callbk.ServerStack.Enqueue(ps.Cmd)
                Case 20
                    LastSentPS = 0
                    psSendCounter = CShort(psSendCounter + 1)
                    psReceiveCounter = CShort(psReceiveCounter + 1)
                    Dim ps As New PSS_Struct
                    ps = PSS_Stack(psSendCounter - 1)
                    callbk.ServerStack.Enqueue(ps.Cmd)
            End Select
            LastSentPS += 1
        End SyncLock
    End Sub


    Dim backupLock As New Object

    Private Shared Function SendPStoDatabase(s As PSInfo_Struct) As Boolean

        If PsProcess <> PsSystemRunning.PsBackup Then Return False

        Dim db As New SQLiteDatabase(MSPK_MDB.SQLitefile)
        Dim Value As Dictionary(Of String, String) = s.Values
        Dim cmd As String = "SELECT [ID] FROM BACKUPMASTER Where Name ='" & s.name & "'"

        Dim idx As Integer = 0

        Dim RecordExist As Boolean = Double.TryParse(db.ExecuteScalar1(cmd), idx)

        Dim dta As New Dictionary(Of String, String)()
        If s.name.ToUpper = "[DREAM]" Then
            s.name = "[DREAM]"
        End If
        dta.Add(MS_Name, s.name)

        Dim fDate As Double = 0
        If Value.ContainsKey("sys_lastused_date") Then Double.TryParse(Value.Item("sys_lastused_date").ToString, fDate)
        If fDate = 0 Then
            dta.Add("[date modified]", Main.FurcTime.ToString)
            If s.name.ToUpper <> "[DREAM]" Then
                If Value.ContainsKey("sys_lastused_date") Then
                    Value.Item("sys_lastused_date") = DateTimeToUnixTimestamp(Main.FurcTime).ToString
                Else
                    Value.Add("sys_lastused_date", DateTimeToUnixTimestamp(Main.FurcTime).ToString)
                End If

            End If

        Else
            dta.Add("[date modified]", UnixTimeStampToDateTime(fDate).ToString)
        End If

        If RecordExist Then

            db.Update("BackupMaster", dta, "NAME ='" + s.name + "'")
            SQLiteDatabase.ExecuteNonQuery("DELETE FROM 'BACKUP' WHERE NameID=" + idx.ToString)
        Else
            db.Insert("BACKUPMASTER", dta)
            Integer.TryParse(db.ExecuteScalar1(cmd).ToString, idx)
        End If

        callbk.Player = Main.NametoFurre(s.name, True)
        callbk.Player.Message = ""
        MainMSEngine.PageSetVariable("MESSAGE", Nothing)
        MS_Engine.MainMSEngine.PageExecute(504, 505)

        Return InsertMultiRow("BACKUP", idx, Value)
    End Function

    Public Shared Sub PrunePS(NumDays As Double)
        If PsProcess <> PsSystemRunning.PsNone Then Exit Sub

        '(0:500) When the bot starts backing up the character Phoenix Speak,
        'MainEngine.MSpage.Execute(500)
        callbk.SendClientMessage("SYSTEM:", "Pruning records older than " + NumDays.ToString + " days")
        Dim cmd2 As String = "SELECT * FROM BACKUPMASTER"
        Dim db As SQLiteDatabase = New SQLiteDatabase(MSPK_MDB.SQLitefile)
        Dim dt As System.Data.DataTable = SQLiteDatabase.GetDataTable(cmd2)
        Dim result As String = ""
        Dim Counter As Integer = 0
        For Each row As System.Data.DataRow In dt.Rows
            Dim idx As Integer = Integer.Parse(row.Item("ID").ToString)
            Dim ts As TimeSpan = Main.FurcTime.Subtract(DateTime.Parse(row.Item("date modified").ToString))
            If ts.Days >= NumDays Then
                SQLiteDatabase.ExecuteNonQuery("DELETE FROM 'BACKUP' WHERE NameID=" + idx.ToString)
                SQLiteDatabase.ExecuteNonQuery("DELETE FROM 'BACKUPMASTER' WHERE ID=" + idx.ToString)
                Counter += 1
            End If

        Next

        PsProcess = PsSystemRunning.PsNone
        '(0:501) When the bot completes backing up the characters Phoenix Speak,
        'MainEngine.MSpage.Execute(501)
        callbk.SendClientMessage("SYSTEM:", "Prune Completed! Removed " + Counter.ToString + " furres from backup")
    End Sub




    Public Shared Sub RestorePS()
        If PsProcess <> PsSystemRunning.PsNone Then Exit Sub
        PsProcess = PsSystemRunning.PsRestore
        callbk.SendClientMessage("SYSTEM:", "Restoreing all Character Phoenix Speak to the dream.")
        '(0:500) When the bot starts backing up the character Phoenix Speak,
        MainMSEngine.MSpage.Execute(502)
        Dim cmd As String = "select * FROM BACKUPMASTER"
        PSS_Stack.Clear()
        CurrentPS_Stage = PS_BackupStage.RestoreAllCharacterPS
        psReceiveCounter = 0
        psSendCounter = 1
        Dim db As SQLiteDatabase = New SQLiteDatabase(MSPK_MDB.SQLitefile)
        Dim dt As System.Data.DataTable = SQLiteDatabase.GetDataTable(cmd)
        Dim result As String = ""
        For Each row As System.Data.DataRow In dt.Rows
            Build_PS_CMD(row.Item(MS_Name).ToString())
        Next

        Dim s As New PSS_Struct
        s = PSS_Stack(0)
        callbk.ServerStack.Enqueue(s.Cmd)

    End Sub
    Public Shared psSendCounter As Int16 = 0
    Public Shared psReceiveCounter As Int16 = 0
    Private ChannelLock As New Object

    Public Shared Sub RestorePS(days As Double)
        If PsProcess <> PsSystemRunning.PsNone Then Exit Sub
        PsProcess = PsSystemRunning.PsRestore
        '(0:500) When the bot starts backing up the character Phoenix Speak,
        MainMSEngine.MSpage.Execute(502)
        psReceiveCounter = 0
        psSendCounter = 1
        PSS_Stack.Clear()
        CurrentPS_Stage = PS_BackupStage.RestoreAllCharacterPS
        callbk.SendClientMessage("SYSTEM:", "Restoring characters newer than " + days.ToString + " days to the dream.")
        Dim cmd As String = "select * FROM BACKUPMASTER"
        CurrentPS_Stage = PS_BackupStage.RestoreAllCharacterPS
        Dim db As SQLiteDatabase = New SQLiteDatabase(MSPK_MDB.SQLitefile)
        Dim dt As System.Data.DataTable = SQLiteDatabase.GetDataTable(cmd)
        Dim result As String = ""
        For Each row As System.Data.DataRow In dt.Rows
            Dim ft As String = row.Item("date modified").ToString
            Dim Time As TimeSpan = Main.FurcTime.Subtract(DateTime.Parse(ft))
            If Time.Days <= days Then
                Build_PS_CMD(row.Item(MS_Name).ToString())
            End If
        Next
        Dim s As New PSS_Struct
        s = PSS_Stack(0)
        callbk.ServerStack.Enqueue(s.Cmd)
    End Sub


    Public Shared Sub Build_PS_CMD(ByRef str As String, Optional ByRef msg As Boolean = False)
        If String.IsNullOrEmpty(str) Then Exit Sub
        If str.ToUpper = "[DREAM]" Then
            str = str.ToUpper
        Else
            str = MainMSEngine.ToFurcShortName(str)
        End If

        Dim cmd As String =
            "select BACKUP.*, BACKUPMASTER.ID from BACKUP " +
            "inner join BACKUPMASTER on " +
            "BACKUPMASTER.ID = BACKUP.NameID " +
            "where BACKUPMASTER.Name = '" + str + "' "
        If msg Then callbk.SendClientMessage("SYSTEM:", "Restoring Phoenix Speak for " + str)
        Dim db As SQLiteDatabase = New SQLiteDatabase(MSPK_MDB.SQLitefile)
        Dim dt As Data.DataTable = SQLiteDatabase.GetDataTable(cmd)
        Dim result As New List(Of String)
        Dim ID As Integer = 0
        For Each row As System.Data.DataRow In dt.Rows
            result.Add(String.Format("{0}=""{1}""", row.Item("Key").ToString, row.Item("Value").ToString))
            Integer.TryParse(row.Item("NameID").ToString, ID)
        Next
        If result.Count > 0 Then
                Dim PScmd As String = ""
                Dim Var As New List(Of String)
                Dim str2 As String = ""

            For I2 As Integer = 0 To result.Count - 1

                        Dim Ok As Boolean = False

                Var.Add(result.Item(I2).ToString)

                    If str.ToUpper = "[DREAM]" Then
                            PScmd = "ps " + (PSS_Stack.Count + 1).ToString + " set dream." + String.Join(",", Var.ToArray)
                        Else
                            PScmd = "ps " + (PSS_Stack.Count + 1).ToString + " set character." + str + "." + String.Join(",", Var.ToArray)
                        End If

                If I2 = result.Count - 1 Then
                        Ok = True
                    ElseIf PScmd.Length + result.Item(I2 + 1).ToString.Length >= Main.MASS_SPEECH Then
                        Ok = True
                            End If

                If Ok Then


                    Dim struct As New PSS_Struct
                    struct.Cmd = PScmd
                    struct.Name = str
                    struct.ID = CShort(PSS_Stack.Count + 1)
                    PSS_Stack.Add(struct)
                    Var.Clear()

                End If

            Next

        End If

    End Sub

    Public Shared Sub PS_Abort()
        CurrentPS_Stage = PS_BackupStage.off
        PSS_Stack.Clear()
        CharacterList.Clear()
        PsProcess = PsSystemRunning.PsNone
        psReceiveCounter = 0
        psSendCounter = 0
        LastSentPS = 0
    End Sub

    Private Shared Function ProcessPSData(ByVal PS_Stat As Int16, ByVal ps_KV As Dictionary(Of String, String), data As String) As Boolean
        If CurrentPS_Stage = PS_BackupStage.off Then
            MS_Engine.MainMSEngine.PageExecute(80, 81, 82)
            Return False
        ElseIf CurrentPS_Stage = PS_BackupStage.GetList Or CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList Then
            CharacterList.Clear()

            '(0:500) When the bot starts backing up the character Phoenix Speak,
            MainMSEngine.MSpage.Execute(500)
            PsProcess = PsSystemRunning.PsBackup
            callbk.SendClientMessage("SYSTEM:", "Backing up Dream Characters Set.")

            Dim f As New PSInfo_Struct
            f.name = "[DREAM]"
            f.PS_ID = 1
            CharacterList.Add(f)

            For Each k As KeyValuePair(Of String, String) In ps_KV
                f = New PSInfo_Struct
                f.name = k.Key
                f.PS_ID = CharacterList.Count + 1
                CharacterList.Add(f)
            Next

            Dim str As String = "ps 1 get dream.*"
            psSendCounter = 1
            callbk.ServerStack.Enqueue(str)
            callbk.on_Tick(0)
            psReceiveCounter = 0
            CurrentPS_Stage = PS_BackupStage.GetTargets

        ElseIf CurrentPS_Stage = PS_BackupStage.GetTargets And PS_Stat <= CharacterList.Count And psSendCounter = psReceiveCounter + 1 Then
            If PS_Stat = 0 Then
                Throw New Exception()
            End If
            psReceiveCounter = PS_Stat
            Dim s As New PSInfo_Struct
            s = CharacterList(PS_Stat - 1)
            s.Values = ps_KV
            'If s.PS_ID = PS_Stat Then

            If SendPStoDatabase(s) AndAlso PS_Stat <> CharacterList.Count Then
                Dim str As String = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*"
                callbk.ServerStack.Enqueue(str)
                psSendCounter = CShort(PS_Stat + 1)

            ElseIf s.PS_ID = PS_Stat AndAlso PS_Stat = CharacterList.Count Then
                CurrentPS_Stage = PS_BackupStage.off
                PsProcess = PsSystemRunning.PsNone
                CharacterList.Clear()
                psReceiveCounter = 0
                psSendCounter = 1
                '(0:501) When the bot completes backing up the characters Phoenix Speak,
                callbk.SendClientMessage("SYSTEM:", "Completed Backing up Dream Characters set.")
                MainMSEngine.MSpage.Execute(501)
            End If
            ' End If
        ElseIf CurrentPS_Stage = PS_BackupStage.GetSingle And PS_Stat <= CharacterList.Count And psSendCounter = psReceiveCounter + 1 Then

            Dim s As New PSInfo_Struct
            s = CharacterList(PS_Stat - 1)
            callbk.SendClientMessage("SYSTEM:", "Backing up information for player " + s.name)
            s.Values = ps_KV
            s.PS_ID = PS_Stat
            psReceiveCounter = PS_Stat

            If SendPStoDatabase(s) AndAlso PS_Stat <> CharacterList.Count Then
                Dim str As String = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*"
                callbk.ServerStack.Enqueue(str)
                psSendCounter = CShort(PS_Stat + 1)

            ElseIf PS_Stat = CharacterList.Count Then
                CurrentPS_Stage = PS_BackupStage.off
                CharacterList.Clear()
                psReceiveCounter = 0
                psSendCounter = 1
                PsProcess = PsSystemRunning.PsNone
            End If


        ElseIf CurrentPS_Stage = PS_BackupStage.RestoreAllCharacterPS And PS_Stat <= PSS_Stack.Count And psSendCounter = psReceiveCounter + 1 Then


            If PS_Stat <> PSS_Stack.Count - 1 Then
                LastSentPS = 0
                Dim s As New PSS_Struct
                s = PSS_Stack(PS_Stat)
                callbk.ServerStack.Enqueue(s.Cmd)
                callbk.Player = Main.NametoFurre(s.Name, True)
                callbk.Player.Message = ""
                MainMSEngine.PageSetVariable("MESSAGE", "")
                MS_Engine.MainMSEngine.PageExecute(506, 507)

                psSendCounter = CShort(PS_Stat + 1)
                psReceiveCounter = PS_Stat

            ElseIf PS_Stat = PSS_Stack.Count - 1 Then
                PsProcess = PsSystemRunning.PsNone
                Main.SendClientMessage("SYSTEM:", "Completed Character restoration to the dream")
                '(0:501) When the bot completes backing up the characters Phoenix Speak,
                MainMSEngine.MSpage.Execute(503)
                CurrentPS_Stage = PS_BackupStage.off
            End If

        End If
        Return True
    End Function

    Private Shared psCheck As Boolean = False
    ''' <summary>
    ''' process Phoenix Speak data coming from the game server
    ''' </summary>
    ''' <param name="data">Server Data</param>
    Public Shared Sub ProcessServerPS(data As String)

        Dim PS_Stat As Int16 = 0
        '(PS Ok: get: result: bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340
        MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", data)
        callbk.Player.Message = data
        Dim psResult As Regex = New Regex(String.Format("^PS (\d+)? ?Ok: get: result: (.*)$"))          'Regex: ^\(PS Ok: get: result: (.*)$
        Dim psMatch As System.Text.RegularExpressions.Match = psResult.Match(String.Format("{0}", data))
        If psMatch.Success Then
            Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat)
            Dim psResult1 As Regex = New Regex("^<empty>$")
            Dim psMatch2 As System.Text.RegularExpressions.Match = psResult1.Match(psMatch.Groups(2).Value)
            If psMatch2.Success And CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList Then
                If NextLetter <> "{"c Then
                    callbk.ServerStack.Enqueue("ps get character." + incrementLetter(NextLetter) + "*")
                Else
                    psCheck = ProcessPSData(1, PSinfo, data)
                End If
            Else


                'Add "," to the end of match #1.
                'Input: "bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340,"
                Dim input As String = psMatch.Groups(2).Value.ToString & ","
                'Regex: ( ?([^=]+)=('?)(.+?)('?)),

                If CurrentPS_Stage <> PS_BackupStage.GetAlphaNumericList Then PSinfo.Clear()
                    Dim mc As MatchCollection = Regex.Matches(input, "\s?(.*?)=('?)(\d+|.*?)(\2),?")
                    Dim i As Integer
                    For i = 0 To mc.Count - 1
                        Dim m As System.Text.RegularExpressions.Match = mc.Item(i)
                        If Not PSinfo.ContainsKey(m.Groups(1).Value) Then PSinfo.Add(m.Groups(1).Value.ToString, m.Groups(3).Value)
                        'Match(1) : Value(Name)
                        'Match 2: Empty if number, ' if string
                        'Match(3) : Value()
                    Next
                    'Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat)
                    If CurrentPS_Stage <> PS_BackupStage.GetAlphaNumericList Then

                    psCheck = ProcessPSData(PS_Stat, PSinfo, data)

                        ElseIf CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList And NextLetter <> "{"c Then
                        Dim m As System.Text.RegularExpressions.Match = mc.Item(mc.Count - 1)
                        NextLetter = incrementLetter(m.Groups(1).Value.ToString)
                        If NextLetter <> "{"c Then
                            callbk.ServerStack.Enqueue("ps get character." + NextLetter + "*")
                        Else
                            psCheck = ProcessPSData(1, PSinfo, data)
                        End If
                    ElseIf CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList And NextLetter = "{"c Then
                    'CurrentPS_Stage = PS_BackupStage.GetList
                    psCheck = ProcessPSData(PS_Stat, PSinfo, data)

                End If

            End If
        End If


        psResult = New Regex(String.Format("^PS (\d+)? ?Ok: get: multi_result (\d+)/(\d+): (.+)$"))
        'Regex: ^\(PS Ok: get: result: (.*)$
        psMatch = psResult.Match(String.Format("{0}", data))
        If psMatch.Success Then

            Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat)
            If psMatch.Groups(2).Value.ToString = "1" And CurrentPS_Stage = PS_BackupStage.GetList Then
                pslen = 0
                PSinfo.Clear()
                PS_Page = ""
            ElseIf CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList Then
                pslen = 0
            End If

            'Add "," to the end of match #1.
            'Input: "bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340,"
            'Dim input As String = psMatch.Groups(4).Value.ToString
            PS_Page += psMatch.Groups(4).Value.ToString
            pslen += data.Length + 1
            'Regex: ( ?([^=]+)=('?)(.+?)('?)),

            If psMatch.Groups(2).Value = psMatch.Groups(3).Value Then
                'PS_Page += ","


                Dim mc As MatchCollection = Regex.Matches(String.Format(PS_Page), String.Format("\s?(.*?)=('?)(\d+|.*?)(\2),?"), RegexOptions.IgnorePatternWhitespace)
                If CurrentPS_Stage <> PS_BackupStage.GetAlphaNumericList Then PSinfo.Clear()
                For i As Integer = 0 To mc.Count - 1
                    Dim m As System.Text.RegularExpressions.Match = mc.Item(i)
                    If Not PSinfo.ContainsKey(m.Groups(1).Value) Then PSinfo.Add(m.Groups(1).Value, m.Groups(3).Value)
                    'Match(1) : Value(Name)
                    'Match 2: Empty if number, ' if string
                    'Match(3) : Value()
                Next
                Dim num As Integer = 0
                Integer.TryParse(psMatch.Groups(3).Value, num)
                If pslen > 1000 * num And CurrentPS_Stage = PS_BackupStage.GetList Then
                    CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList
                    Dim m As System.Text.RegularExpressions.Match = mc.Item(mc.Count - 1)
                    callbk.ServerStack.Enqueue("ps get character." + m.Groups(1).Value.Substring(0, 1) + "*")

                ElseIf CurrentPS_Stage <> PS_BackupStage.GetAlphaNumericList Then

                    psCheck = ProcessPSData(PS_Stat, PSinfo, data)

                        ElseIf CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList And NextLetter <> "{"c Then
                    Dim m As System.Text.RegularExpressions.Match = mc.Item(mc.Count - 1)
                    NextLetter = incrementLetter(m.Groups(1).Value.ToString)
                    If NextLetter <> "{"c Then
                        callbk.ServerStack.Enqueue("ps get character." + NextLetter + "*")
                    Else
                        psCheck = ProcessPSData(1, PSinfo, data)
                    End If
                ElseIf CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList And NextLetter = "{"c Then
                    'CurrentPS_Stage = PS_BackupStage.GetList

                    psCheck = ProcessPSData(PS_Stat, PSinfo, data)

                End If

            End If
            '(PS 5 Error: get: Query error: Field 'Bob' does not exist

        End If

        psResult = New Regex("^PS (\d+)? ?Ok: set: Ok$")
        '^PS (\d+)? ?Ok: set: Ok
        psMatch = psResult.Match(data)
        If psMatch.Success Then
            PSinfo.Clear()
            Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat)

            ProcessPSData(PS_Stat, PSinfo, data)

        End If
        'PS (\d+) Error: Sorry, PhoenixSpeak commands are currently not available in this dream.
        psResult = New Regex("^PS (\d+)? ?Error: (.*?)")
        psMatch = psResult.Match(data)
        If psMatch.Success Then
            psResult = New Regex("^PS (\d+)? ?Error: Sorry, PhoenixSpeak commands are currently not available in this dream.$")
            'Regex: ^\(PS Ok: get: result: (.*)$
            'PS (\d+)? ?Error: get: Query error: (.+) Unexpected character '(.+)' at column (\d+)
            Dim psMatch2 As System.Text.RegularExpressions.Match = psResult.Match(data)
            Dim psResult2 As Regex = New Regex("^PS (\d+)? ?Error: set")
            Dim psmatch3 As System.Text.RegularExpressions.Match = psResult2.Match(data)
            Dim psResult3 As Regex = New Regex("PS (\d+)? ?Error: set: Query error: Only (\d+) rows allowed.")
            Dim psmatch4 As System.Text.RegularExpressions.Match = psResult3.Match(data)
            If psMatch2.Success Or psmatch3.Success Or psmatch4.Success Then
                PS_Abort()
                If psmatch4.Success Then
                    MainMSEngine.MSpage.Execute(503)
                End If
            Else
                Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat)

                If CurrentPS_Stage = PS_BackupStage.off Then
                    MS_Engine.MainMSEngine.PageExecute(80, 81, 82)

                ElseIf CurrentPS_Stage = PS_BackupStage.GetList Then
                    If PS_Stat <> CharacterList.Count Then
                        Dim str As String = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*"
                        callbk.ServerStack.Enqueue(str)
                        psSendCounter = CShort(PS_Stat + 1)

                        psReceiveCounter = PS_Stat

                    ElseIf PS_Stat = CharacterList.Count Then
                        CurrentPS_Stage = PS_BackupStage.off


                    End If
                ElseIf CurrentPS_Stage = PS_BackupStage.GetTargets And psSendCounter = psReceiveCounter + 1 Then
                    If PS_Stat <> CharacterList.Count Then
                        Dim str As String = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*"
                        callbk.ServerStack.Enqueue(str)
                        psSendCounter = CShort(PS_Stat + 1)
                        psReceiveCounter = PS_Stat
                    ElseIf PS_Stat = CharacterList.Count Then
                        CurrentPS_Stage = PS_BackupStage.off
                        PsProcess = PsSystemRunning.PsNone
                        CharacterList.Clear()
                        psReceiveCounter = 0
                        psSendCounter = 1
                        '(0:501) When the bot completes backing up the characters Phoenix Speak,
                        callbk.SendClientMessage("SYSTEM:", "Completed Backing up Dream Characters set.")
                        MainMSEngine.MSpage.Execute(501)
                    End If

                ElseIf CurrentPS_Stage = PS_BackupStage.RestoreAllCharacterPS And PS_Stat <= PSS_Stack.Count - 1 And psSendCounter = psReceiveCounter + 1 Then
                    If PS_Stat <> PSS_Stack.Count - 1 Then
                        LastSentPS = 0
                        Dim ss As New PSS_Struct
                        ss = PSS_Stack(PS_Stat)
                        callbk.ServerStack.Enqueue(ss.Cmd)
                        psSendCounter = CShort(PS_Stat + 1)
                        psReceiveCounter = PS_Stat

                    ElseIf PS_Stat = PSS_Stack.Count - 1 Then
                        PsProcess = PsSystemRunning.PsNone
                        callbk.SendClientMessage("SYSTEM:", "Completed Character restoration to the dream")
                        '(0:501) When the bot completes backing up the characters Phoenix Speak,
                        MainMSEngine.MSpage.Execute(503)
                        CurrentPS_Stage = PS_BackupStage.off
                    End If
                ElseIf CurrentPS_Stage = PS_BackupStage.GetSingle And PS_Stat <= CharacterList.Count And psSendCounter = psReceiveCounter + 1 Then
                    If PS_Stat <> CharacterList.Count Then
                        Dim str As String = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*"
                        callbk.ServerStack.Enqueue(str)
                        psSendCounter = CShort(PS_Stat + 1)
                        psReceiveCounter = PS_Stat
                    ElseIf PS_Stat = CharacterList.Count Then
                        CurrentPS_Stage = PS_BackupStage.off
                        CharacterList.Clear()
                        psReceiveCounter = 0
                        psSendCounter = 1
                        PsProcess = PsSystemRunning.PsNone
                    End If
                End If
            End If
        End If


    End Sub

    Private Shared Function incrementLetter(Input As String) As Char
        Input = Input.Substring(0, 1).ToLower
        Dim i As Integer = AscW(Input)
        Select Case Input
            Case "a"c To "z"c
                Dim test As Char = ChrW(i + 1)
                Return ChrW(i + 1)
            Case "0"c To "8"c
                Dim test As Char = ChrW(i + 1)
                Return (ChrW(i + 1))
            Case "9"c
                Return "a"c
            Case Else
                Return "{"c
        End Select

    End Function

    ''' <summary>
    '''     Allows the programmer to easily insert into the DB
    ''' </summary>
    ''' <param name="tableName">The table into which we insert the data.</param>
    ''' <param name="data">A dictionary containing the column names and data for the insert.</param>
    ''' <returns>A boolean true or false to signify success or failure.</returns>
    Public Shared Function InsertMultiRow(tableName As String, ID As Integer, data As Dictionary(Of String, String)) As Boolean
        Dim values As New List(Of String)
        For Each val As KeyValuePair(Of String, String) In data
            values.Add(String.Format(" ( {0}, '{1}', '{2}' )", ID, val.Key, val.Value))
        Next


        Dim i As Integer = 0
            If values.Count > 0 Then
                Dim str As String = String.Join(", ", values.ToArray)
                'INSERT INTO 'table' ('column1', 'col2', 'col3') VALUES (1,2,3),  (1, 2, 3), (etc);
                Dim cmd As String = String.Format("INSERT into '{0}' (NameID, 'Key', 'Value') Values {1};", tableName, str)
                i = SQLiteDatabase.ExecuteNonQuery(cmd)
            End If
            Return values.Count <> 0 AndAlso i <> 0

            Return True
    End Function

End Class
