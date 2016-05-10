Imports RTF
Imports SilverMonkey.Controls
Imports Furcadia.Net.Movement
Imports Furcadia.IO
Imports Furcadia.Net
Imports Furcadia.Base220
Imports SilverMonkey.ConfigStructs
Imports System.Net
Imports System.Web.HttpUtility
Imports System.Text.RegularExpressions
Imports System.IO
Imports Furcadia.Drawing.Graphics
Imports Furcadia.Drawing
Imports System.Drawing
Imports System.Net.NetworkInformation
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports SilverMonkey.MSPK_MDB
Imports Conversive.Verbot5

Public Class Main
    Inherits System.Windows.Forms.Form

#Region "Constants"

    Private Const CookieToMeREGEX As String = "<name shortname='(.*?)'>(.*?)</name> just gave you"
#End Region
#Region "SysTray"
    Public WithEvents NotifyIcon1 As NotifyIcon
#End Region
#Region "smPounce"
    Private WithEvents smPounce As PounceConnection
    Private PounceTimer As Threading.Timer
    Public Structure pFurre
        Public WasOnline As Boolean
        Public Online As Boolean
    End Structure

    Public OnlineList As String = CheckMyDocFile("OnlineList.txt")

    Public FurreList As New Dictionary(Of String, pFurre)

    Public HasShare As Boolean = False
    Public NoEndurance As Boolean = False
    Dim lastaccess As Date

    Private Function ReadOnlineList() As Boolean
        Dim result As Boolean = False
        If File.Exists(CheckMyDocFile(OnlineList)) Then
            If File.GetLastWriteTime(CheckMyDocFile(OnlineList)) <> lastaccess Then
                lastaccess = File.GetLastWriteTime(CheckMyDocFile(OnlineList))


                Dim NameList() As String = File.ReadAllLines(CheckMyDocFile(OnlineList))
                For i As Integer = 0 To NameList.Length - 1
                    If Not FurreList.ContainsKey(NameList(i)) Then FurreList.Add(NameList(i), New pFurre)
                Next
                Dim Namelist2(FurreList.Count - 1) As String
                FurreList.Keys.CopyTo(Namelist2, 0)
                For i As Integer = 0 To Namelist2.Length - 1
                    Dim found As Boolean = False
                    For j As Integer = 0 To NameList.Length - 1
                        If NameList(j).ToFurcShortName = Namelist2(i).ToFurcShortName Then
                            found = True
                            Exit For
                        End If
                    Next
                    If Not found Then FurreList.Remove(Namelist2(i))
                Next
                result = True
            End If
        End If
        Return result
    End Function
    Dim usingPounce As Integer = 0
    Private Sub smPounceSend(sender As Object)
        If (0 = Interlocked.Exchange(usingPounce, 1)) Then
            If _FormClose Then Exit Sub
            If Not bConnected() Then Exit Sub
            If Not ReadOnlineList() Then Exit Sub
            smPounce = New PounceConnection("http://on.furcadia.com/q/", Nothing)

            smPounce.RemoveFriends()
            For Each kv As KeyValuePair(Of String, pFurre) In FurreList
                If Not [String].IsNullOrEmpty(kv.Key) Then
                    smPounce.AddFriend(kv.Key.ToFurcShortName())
                End If
            Next
            smPounce.ConnectAsync()
            Interlocked.Exchange(usingPounce, 0)
        End If
    End Sub

    Sub Response(friends As String(), dreams As String()) Handles smPounce.Response

        Dim myKeysArray(FurreList.Keys.Count - 1) As String
        FurreList.Keys.CopyTo(myKeysArray, 0)

        For Each _furre As String In myKeysArray
            Dim test As pFurre = FurreList.Item(_furre)
            test.WasOnline = test.Online
            test.Online = False
            For Each [friend] As String In friends
                If _furre.ToFurcShortName = [friend].ToFurcShortName Then
                    test.Online = True
                    Exit For
                End If
            Next
            FurreList.Item(_furre) = test
            If test.WasOnline = True And test.Online = False Then
                'Furre Logged off
                SendClientMessage("smPounce", _furre + " has logged out.")
                Player = NametoFurre(_furre, True)
                MainMSEngine.PageExecute(951, 953)
            ElseIf test.WasOnline = False And test.Online = True Then
                'Furre logged on
                SendClientMessage("smPounce", _furre + " has logged on.")
                Player = NametoFurre(_furre, True)
                MainMSEngine.PageExecute(950, 952)
            End If

        Next
    End Sub

#End Region

#Region "Verbot"
    Public verbot As Verbot5Engine
    Public state As State
    Public kb As KnowledgeBase = New KnowledgeBase()
    Public kbi As KnowledgeBaseItem = New KnowledgeBaseItem()
#End Region

#Region "WmCpyDta"

    <DllImport("user32.dll", EntryPoint:="FindWindow")> _
    Private Shared Function FindWindow(_ClassName As String, _WindowName As String) As Integer
    End Function
    Public Declare Function SetFocusAPI Lib "user32.dll" Alias "SetFocus" (ByVal hWnd As Long) As Long
    Private Declare Function SetForegroundWindow Lib "user32" (ByVal hWnd As Long) As Long


    Public Function FindProcessByName(strProcessName As String) As IntPtr
        Dim HandleOfToProcess As IntPtr = IntPtr.Zero
        Dim p As Process() = Process.GetProcesses()
        For Each p1 As Process In p
            Debug.WriteLine(p1.ProcessName.ToUpper())
            If p1.ProcessName.ToUpper() = strProcessName.ToUpper() Then
                HandleOfToProcess = p1.MainWindowHandle
                Exit For
            End If
        Next
        Return HandleOfToProcess
    End Function

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = WM_COPYDATA Then
            'Dim mystr As COPYDATASTRUCT
            Dim mystr2 As COPYDATASTRUCT = CType(Marshal.PtrToStructure(m.LParam(), GetType(COPYDATASTRUCT)), COPYDATASTRUCT)

            ' If the size matches
            If mystr2.cdData = Marshal.SizeOf(GetType(MyData)) Then
                ' Marshal the data from the unmanaged memory block to a 
                ' MyStruct managed struct.
                Dim myStr As MyData = DirectCast(Marshal.PtrToStructure(mystr2.lpData, GetType(MyData)), MyData)



                Dim sName As String = myStr.lpName
                Dim sFID As UInteger = 0
                Dim sTag As String = myStr.lpTag
                Dim sData As String = myStr.lpMsg


                If sName = "~DSEX~" Then
                    If sTag = "Restart" Then
                        MainMSEngine.EngineRestart = True
                        cBot.MS_Script = MainMSEngine.msReader(CheckMyDocFile(cBot.MS_File))
                        MainEngine.MSpage = MainMSEngine.engine.LoadFromString(cBot.MS_Script)
                        MainMSEngine.MS_Stared = 2
                        ' MainMSEngine.LoadLibrary()
                        MainMSEngine.EngineRestart = False
                        ' Main.ResetPrimaryVariables()
                        sndDisplay("<b><i>[SM]</i></b> Status: File Saved. Engine Restarted")
                        If smProxy.IsClientConnected Then smProxy.SendClient(")<b><i>[SM]</i></b> Status: File Saved. Engine Restarted" + vbLf)
                        MainMSEngine.PageExecute(0)
                    End If
                Else
                    If DREAM.List.ContainsKey(sFID) Then
                        Player = DREAM.List.Item(sFID)
                    Else
                        Player.Clear()
                        Player.Name = sName
                    End If

                    Player.Message = sData.ToString
                    MainMSEngine.PageSetVariable(MS_Name, sName)
                    MainMSEngine.PageSetVariable("MESSAGE", sData)
                    ' Execute (0:15) When some one whispers something
                    MainMSEngine.PageExecute(75, 76, 77)
                    SendClientMessage("Message from: " + sName, sData)
                End If
            End If
        Else
            MyBase.WndProc(m)
        End If

    End Sub

#End Region
    Public Sub SendClientMessage(msg As String, data As String)
        If smProxy.IsClientConnected Then smProxy.SendClient("(" + "<b><i>[SM]</i> - " + msg + ":</b> """ + data + """" + vbLf)
        sndDisplay("<b><i>[SM]</i> - " + msg + ":</b> """ + data + """")
    End Sub

    Dim _FormClose As Boolean = False
    Private FurcMutex As System.Threading.Mutex
    Private Const FurcProcess As String = "Furcadia"
    Private ReconnectTimer, ReconnectTimeOutTimer As Threading.Timer
    Public Shared NewBot As Boolean = False
    Public writer As TextBoxWriter = Nothing
    Private ClientClose As Boolean = False
#Region "Propertes"
    Dim Lock As New Object
    Public objHost As New smHost
    Public Player As FURRE = New FURRE
    Public Shared loggingIn As UShort = 0
    Public DREAM As New DREAM
    Dim ActionCMD As String = ""
    Dim InDream As Boolean = False
    Dim curWord As String

    Public Shared [SQLitefile] As [String]
    'Public PS_Que As New Queue(Of String)(100)
    '0 = no error
    '1 = warning
    '2 = error
    Public ErrorNum As Short = 0
    Public ErrorMsg As String = ""

    Private ThroatTired As Boolean = False

    Public ServerStack As Queue(Of String) = New Queue(Of String)(500)
    Private SpeciesTag As Queue(Of String) = New Queue(Of String)()
    Private BadgeTag As Queue(Of String) = New Queue(Of String)()
    Private LookQue As Queue(Of String) = New Queue(Of String)()

    Private Sub ClearQues()
        If Not IsNothing(Me.TroatTiredDelay) Then Me.TroatTiredDelay.Dispose()
        SyncLock Lock
            ThroatTired = False
        End SyncLock
        ServerStack.Clear()
        SpeciesTag.Clear()
        LookQue.Clear()
        BadgeTag.Clear()
        g_mass = 0
        PS_Abort()


    End Sub

    'Monkey Speak Bot specific Variables
    Public Const VarPrefix As String = ""
    Public ProcID As Integer
    Private ProcExit As Boolean
    'Boolean Hack as Controls need a Boolean for enable
    Public Function bConnected() As Boolean
        If loggingIn >= 2 Then
            Return True
        End If
        Return False
    End Function
    ' Public Bot As FURRE

    Public BotUID As UInteger = 0
    Public BotName As String = ""
    Public LogStream As LogStream
    Public Look As Boolean = False
    Dim newData As Boolean = False
    Public BanishName As String = ""
    Public BanishString As New ArrayList
    'Input History
    Dim CMD_Max As Integer = 20
    Dim CMDList(CMD_Max) As String
    Dim CMD_Idx, CMD_Idx2 As Integer
    Dim CMD_Lck As Boolean = False

    Private Shared ReLogCounter As Integer = 0
    Public Class PSInfo_Struct
        Public Property name As String
        Public Property Values As Dictionary(Of String, String)
        Public Property PS_ID As Integer
        Public Sub New()
            name = ""
            Values = New Dictionary(Of String, String)
            PS_ID = 0
        End Sub
    End Class


    Public Shared PSinfo As New Dictionary(Of String, String)
    Public Shared PS_Page As String = ""
    'Dim TimeUpdater As Threading.Thread
    Public Shared FurcTime As DateTime



#End Region

#Region "Events"
    Public WithEvents smProxy As NetProxy

    'UpDate Btn-Go Text and Actions Group Enable
    Private Delegate Sub Log_Scoll(ByRef rtb As RichTextBoxEx)
    Private Delegate Sub UpDateBtn_GoCallback(ByRef [text] As String)
    Private Delegate Sub UpDateBtn_GoCallback2()
    Private Delegate Sub UpDateBtn_GoCallback3(ByVal Obj As Object)
    Private Delegate Sub UpDateBtn_StandCallback(ByRef [furre] As FURRE)
    Private Delegate Sub LogSave(ByRef path As String, ByRef filename As String)
    Private Delegate Sub AddDataToListCaller(ByRef lb As RichTextBoxEx, ByRef obj As String, ByRef NewColor As fColorEnum)
    Private Delegate Sub UpDateDreamListCaller(ByRef [dummy] As String) 'ByVal [dummy] As String

    Private Delegate Sub DelTimeupdate()
    Private Delegate Function WordUnderMouse(ByRef Rtf As RichTextBoxEx, ByVal X As Integer, ByVal Y As Integer) As String

    Dim LogTimer As New System.Timers.Timer()
    Dim DreamUpdateTimer As New System.Timers.Timer()
    Private MSalarm As Threading.Timer

    Dim PingTimer As Threading.Timer
    Dim usingPing As Integer = 0
    Private Sub PingTimerTick(ByVal state As Object)
        If (0 = Interlocked.Exchange(usingPing, 1)) Then
            If g_mass + MASS_SPEECH <= MASS_CRITICAL Then
                ServerStack.Enqueue("Ping")
            End If
            Interlocked.Exchange(usingPing, 0)
        End If
    End Sub

    Public CharacterList As List(Of PSInfo_Struct) = New List(Of PSInfo_Struct)
    Public Enum PS_BackupStage
        off = 0
        GetList = 1
        GetAlphaNumericList
        GetTargets
        GetSingle
        RestoreAllCharacterPS
    End Enum
    Public CurrentPS_Stage As PS_BackupStage = PS_BackupStage.off

    Public TroatTiredProc As Threading.Timer
    Private TickTime As DateTime = DateTime.Now
    Private usingResource As Integer = 0
    Public Sub TroatTiredProcTick(ByVal state As Object)
        If (0 = Interlocked.Exchange(usingResource, 1)) Then
            Dim seconds As Double = DateTime.Now.Subtract(TickTime).Milliseconds
            on_Tick(seconds)
            CheckPS_Send()
            TickTime = DateTime.Now
            Interlocked.Exchange(usingResource, 0)
        End If
    End Sub

    Private g_mass As Double = 0
    Private ps_mass As Double = 0
    Private pss_mass As Double = 0

    Public PSS_Stack As New List(Of PSS_Struct)
    Public LastPS_ID As Integer = 0

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

    Public Const MASS_DEFAULT As Integer = 80
    Public Const MASS_SPEECH As Integer = 1000
    Public Const MASS_CRITICAL As Integer = 1600
    Public Const MASS_NOENDURANCE As Integer = 2048
    Public Const MASS_DECAYPS As Integer = 400

    Public Sub on_Tick(ByVal dt As Double)
        If IsNothing(smProxy) Then Exit Sub
        If Not smProxy.IsServerConnected Then Exit Sub
        If ServerStack.Count = 0 Then Exit Sub
        If dt <> 0 Then
            dt = Math.Round(dt, 0) + 1
        End If


        '/* Send buffered speech. */
        Dim decay As Double = Math.Round(dt * MASS_DECAYPS / 1000.0F, 0)
        If (decay > g_mass) Then
            g_mass = 0
        Else
            g_mass = g_mass - decay
        End If

        If NoEndurance Then
            SyncLock Lock
                '/* just send everything right away */
                While ServerStack.Count > 0 And g_mass <= MASS_CRITICAL
                    g_mass += ServerStack.Peek.Length + MASS_DEFAULT
                    smProxy.SendServer(ServerStack.Peek & vbLf)
                    ServerStack.Dequeue()
                End While
            End SyncLock
        ElseIf Not ThroatTired Then
            SyncLock Lock
                ' Only send a speech line if the mass will be under the limit. */
                While ServerStack.Count > 0 And g_mass + MASS_SPEECH <= MASS_CRITICAL
                    g_mass += ServerStack.Peek.Length + MASS_DEFAULT
                    smProxy.SendServer(ServerStack.Peek & vbLf)

                    ServerStack.Dequeue()
                End While
            End SyncLock
        End If
    End Sub


    'resend PS Command if we don't get a response from the server
    'Or skip insruction after failing 4 times
    Dim PSLock2 As New Object
    Dim LastSentPS As Integer
    Private Sub CheckPS_Send()
        If CurrentPS_Stage <> PS_BackupStage.RestoreAllCharacterPS Then Exit Sub
        If ThroatTired Then Exit Sub
        If PSS_Stack.Count >= psSendCounter Then Exit Sub
        SyncLock PSLock2
            Select Case LastSentPS
                Case 4 Or 8 Or 12 Or 16
                    Dim ps As New PSS_Struct
                    ps = PSS_Stack(psSendCounter - 1)
                    ServerStack.Enqueue(ps.Cmd)
                Case 20
                    LastSentPS = 0
                    psSendCounter = CShort(psSendCounter + 1)
                    psReceiveCounter = CShort(psReceiveCounter + 1)
                    Dim ps As New PSS_Struct
                    ps = PSS_Stack(psSendCounter - 1)
                    ServerStack.Enqueue(ps.Cmd)
            End Select
            LastSentPS += 1
        End SyncLock
    End Sub


    Dim backupLock As New Object
    Public PSBackupRunning As Boolean = False
    Private Function SendPStoDatabase(s As PSInfo_Struct) As Boolean

        If PSRestoreRunning Then Return False

        Dim db As New SQLiteDatabase(Main.SQLitefile)
        db.CreateTbl("BACKUPMASTER", "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [date modified] TEXT")
        db.CreateTbl("BACKUP", "[NameID] INTEGER, [Key] TEXT, [Value] TEXT")
        Dim Value As Dictionary(Of String, String) = s.Values
        Dim cmd As String = "SELECT [ID] FROM BACKUPMASTER Where Name ='" & s.name & "'"

        Dim idx As Integer = 0
        Dim RecordExist As Boolean = Integer.TryParse(SQLiteDatabase.ExecuteScalar1(cmd).ToString, idx)

        Dim dta As New Dictionary(Of [String], [String])()
        If s.name.ToUpper = "[DREAM]" Then
            s.name = "[DREAM]"
        End If
        dta.Add(MS_Name, s.name)

        Dim fDate As Double = 0
        If Value.ContainsKey("sys_lastused_date") Then Double.TryParse(Value.Item("sys_lastused_date").ToString, fDate)
        If fDate = 0 Then
            dta.Add("[date modified]", FurcTime.ToString)
            If s.name.ToUpper <> "[DREAM]" Then
                If Value.ContainsKey("sys_lastused_date") Then
                    Value.Item("sys_lastused_date") = DateTimeToUnixTimestamp(FurcTime).ToString
                Else
                    Value.Add("sys_lastused_date", DateTimeToUnixTimestamp(FurcTime).ToString)
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
            Integer.TryParse(SQLiteDatabase.ExecuteScalar1(cmd).ToString, idx)
        End If

        Player = NametoFurre(s.name, True)
        Player.Message = ""
        MainMSEngine.PageSetVariable("MESSAGE", "")
        MainMSEngine.PageExecute(504, 505)

        Return InsertMultiRow("BACKUP", idx, Value)
    End Function

    Public PSPruneRunning As Boolean = False
    Public Sub PrunePS(NumDays As Double)
        If PSBackupRunning Or PSRestoreRunning Or PSPruneRunning Then Exit Sub
        PSPruneRunning = True
        '(0:500) When the bot starts backing up the character Phoenix Speak,
        'MainEngine.MSpage.Execute(500)
        SendClientMessage("SYSTEM:", "Pruning records older than " + NumDays.ToString + " days")
        Dim cmd2 As String = "SELECT * FROM BACKUPMASTER"
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        Dim dt As System.Data.DataTable = SQLiteDatabase.GetDataTable(cmd2)
        Dim result As String = ""
        Dim Counter As Integer = 0
        For Each row As System.Data.DataRow In dt.Rows
            Dim idx As Integer = Integer.Parse(row.Item("ID").ToString)
            Dim ts As TimeSpan = FurcTime.Subtract(DateTime.Parse(row.Item("date modified").ToString))
            If ts.Days >= NumDays Then
                SQLiteDatabase.ExecuteNonQuery("DELETE FROM 'BACKUP' WHERE NameID=" + idx.ToString)
                SQLiteDatabase.ExecuteNonQuery("DELETE FROM 'BACKUPMASTER' WHERE ID=" + idx.ToString)
                Counter += 1
            End If

        Next

        PSPruneRunning = False
        '(0:501) When the bot completes backing up the characters Phoenix Speak,
        'MainEngine.MSpage.Execute(501)
        SendClientMessage("SYSTEM:", "Prune Completed! Removed " + Counter.ToString + " furres from backup")
    End Sub


    Public PSRestoreRunning As Boolean = False

    Public Sub RestorePS()
        If PSRestoreRunning Or PSBackupRunning Or PSPruneRunning Then Exit Sub
        PSRestoreRunning = True
        SendClientMessage("SYSTEM:", "Restoreing all Character Phoenix Speak to the dream.")
        '(0:500) When the bot starts backing up the character Phoenix Speak,
        MainEngine.MSpage.Execute(502)
        Dim cmd As String = "select * FROM BACKUPMASTER"
        PSS_Stack.Clear()
        CurrentPS_Stage = PS_BackupStage.RestoreAllCharacterPS
        psReceiveCounter = 0
        psSendCounter = 1
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        Dim dt As System.Data.DataTable = SQLiteDatabase.GetDataTable(cmd)
        Dim result As String = ""
        For Each row As System.Data.DataRow In dt.Rows
            Build_PS_CMD(row.Item(MS_Name).ToString())
        Next

        Dim s As New PSS_Struct
        s = PSS_Stack(0)
        ServerStack.Enqueue(s.Cmd)

    End Sub
    Public psSendCounter As Int16 = 0
    Public psReceiveCounter As Int16 = 0
    Public Sub RestorePS(days As Double)
        If PSRestoreRunning Or PSBackupRunning Or PSPruneRunning Then Exit Sub
        PSRestoreRunning = True
        '(0:500) When the bot starts backing up the character Phoenix Speak,
        MainEngine.MSpage.Execute(502)
        psReceiveCounter = 0
        psSendCounter = 1
        PSS_Stack.Clear()
        CurrentPS_Stage = PS_BackupStage.RestoreAllCharacterPS
        SendClientMessage("SYSTEM:", "Restoring characters newer than " + days.ToString + " days to the dream.")
        Dim cmd As String = "select * FROM BACKUPMASTER"
        CurrentPS_Stage = Main.PS_BackupStage.RestoreAllCharacterPS
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        Dim dt As System.Data.DataTable = SQLiteDatabase.GetDataTable(cmd)
        Dim result As String = ""
        For Each row As System.Data.DataRow In dt.Rows
            Dim ft As String = row.Item("date modified").ToString
            Dim Time As TimeSpan = FurcTime.Subtract(DateTime.Parse(ft))
            If Time.Days <= days Then
                Build_PS_CMD(row.Item(MS_Name).ToString())
            End If
        Next
        Dim s As New PSS_Struct
        s = PSS_Stack(0)
        ServerStack.Enqueue(s.Cmd)
    End Sub


    Public Sub Build_PS_CMD(ByRef str As String, Optional ByRef msg As Boolean = False)
        If String.IsNullOrEmpty(str) Then Exit Sub
        If str.ToUpper = "[DREAM]" Then
            str = str.ToUpper
        Else
            str = str.ToFurcShortName
        End If

        Dim cmd As String = _
            "select BACKUP.*, BACKUPMASTER.ID from BACKUP " + _
            "inner join BACKUPMASTER on " + _
            "BACKUPMASTER.ID = BACKUP.NameID " + _
            "where BACKUPMASTER.Name = '" + str + "' "
        If msg Then SendClientMessage("SYSTEM:", "Restoring Phoenix Speak for " + str)
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        Dim dt As System.Data.DataTable = SQLiteDatabase.GetDataTable(cmd)
        Dim result As New ArrayList
        Dim ID As Integer = 0
        SyncLock (Me)
            For Each row As System.Data.DataRow In dt.Rows
                result.Add(String.Format("{0}=""{1}""", row.Item("Key").ToString, row.Item("Value").ToString))
                Integer.TryParse(row.Item("NameID").ToString, ID)
            Next
        End SyncLock
        If result.Count > 0 Then
            Dim PScmd As String = ""
            Dim Var As New ArrayList
            Dim str2 As String = ""
            SyncLock (Me)
                For I2 As Integer = 0 To result.Count - 1

                    Dim Ok As Boolean = False
                    Try
                        Var.Add(result.Item(I2).ToString)
                    Catch ex As Exception
                        Dim E As New ErrorLogging(ex, Me)
                    End Try
                    If str.ToUpper = "[DREAM]" Then
                        PScmd = "ps " + (PSS_Stack.Count + 1).ToString + " set dream." + String.Join(",", Var.ToArray)
                    Else
                        PScmd = "ps " + (PSS_Stack.Count + 1).ToString + " set character." + str + "." + String.Join(",", Var.ToArray)
                    End If
                    Try
                        If I2 = result.Count - 1 Then
                            Ok = True
                        ElseIf PScmd.Length + result.Item(I2 + 1).ToString.Length >= MASS_SPEECH Then
                            Ok = True
                        End If
                    Catch ex As Exception
                        Dim E As New ErrorLogging(ex, Me)
                    End Try
                    If Ok Then
                        Try

                            Dim struct As New PSS_Struct
                            struct.Cmd = PScmd
                            struct.Name = str
                            struct.ID = CShort(PSS_Stack.Count + 1)
                            PSS_Stack.Add(struct)
                            Var.Clear()

                        Catch ex As Exception
                            Dim E As New ErrorLogging(ex, Me)
                        End Try
                    End If

                Next
            End SyncLock
        End If

    End Sub
    Public TroatTiredDelay As Threading.Timer
    Private Sub TroatTiredDelayTick(ByVal state As Object)
        ThroatTired = False
        Me.TroatTiredDelay.Dispose()
    End Sub

#End Region

#Region "Recent File List"
    ''' <summary>
    ''' how many list will save
    ''' </summary>
    Const MRUnumber As Integer = 15
    Private MRUlist As System.Collections.Generic.Queue(Of String) = New Queue(Of String)(MRUnumber)

    Private Sub OpenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        With BotIniOpen
            ' Select Bot ini file
            .InitialDirectory = mPath()
            If .ShowDialog = DialogResult.OK Then
                cBot = New cBot(.FileName)
                SaveRecentFile(.FileName)
                ' BotSetup.BotFile = .FileName
                ' BotSetup.ShowDialog()
                Me.EditBotToolStripMenuItem.Enabled = True
            End If

        End With
    End Sub
    ''' <summary>
    ''' store a list to file and refresh list
    ''' </summary>
    ''' <param name="path"></param>
    Public Sub SaveRecentFile(path As String)
        RecentToolStripMenuItem.DropDownItems.Clear()
        'clear all recent list from menu
        LoadRecentList()
        'load list from file
        If Not (MRUlist.Contains(path)) Then
            'prevent duplication on recent list
            MRUlist.Enqueue(path)
        End If
        'insert given path into list
        While MRUlist.Count > MRUnumber
            'keep list number not exceeded given value
            MRUlist.Dequeue()
        End While
        For Each item As String In MRUlist
            Dim fileRecent As New ToolStripMenuItem(item, Nothing, AddressOf RecentFile_click)
            'create new menu for each item in list
            'add the menu to "recent" menu
            RecentToolStripMenuItem.DropDownItems.Add(fileRecent)
        Next
        'writing menu list to file
        Dim stringToWrite As New StreamWriter(pPath() & "/Recent.txt")
        'create file called "Recent.txt" located on app folder
        For Each item As String In MRUlist
            'write list to stream
            stringToWrite.WriteLine(item)
        Next
        stringToWrite.Flush()
        'write stream to file
        stringToWrite.Close()
        'close the stream and reclaim memory
    End Sub
    ''' <summary>
    ''' load recent file list from file
    ''' </summary>
    Private Sub LoadRecentList()
        'try to load file. If file isn't found, do nothing
        MRUlist.Clear()
        Try
            Dim listToRead As New StreamReader(pPath() & "/Recent.txt")
            'read file stream
            Dim line As String = ""
            While (InlineAssignHelper(line, listToRead.ReadLine())) IsNot Nothing
                'read each line until end of file
                MRUlist.Enqueue(line)
            End While
            'insert to list
            'close the stream
            listToRead.Close()

            'throw;
        Catch generatedExceptionName As Exception
        End Try

    End Sub
    ''' <summary>
    ''' click menu handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RecentFile_click(sender As Object, e As EventArgs)
        'BotSetup.BotFile =
        'BotSetup.ShowDialog()
        cBot = New cBot(sender.ToString())
        My.Settings.LastBotFile = cBot.BotFile
        EditBotToolStripMenuItem.Enabled = True
        My.Settings.Save()

        'same as open menu
    End Sub

    Private Sub NewBotToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewBotToolStripMenuItem.Click

        With NewBott
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                cBot = .bFile
                EditBotToolStripMenuItem.Enabled = True
            End If
        End With
    End Sub

    Private Sub EditBotToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EditBotToolStripMenuItem.Click
        With BotSetup
            .bFile = cBot
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                cBot = .bFile
            End If
        End With
    End Sub


    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function
#End Region

#Region "Log Scroll"

    Dim Pos_Old As Integer = 0
    Private Const SBS_HORZ As Integer = 0
    Private Const SBS_VERT As Integer = 1

    <DllImport("user32.dll")> _
    Public Shared Function GetScrollRange(ByVal hWnd As IntPtr, ByVal nBar As Integer, _
 ByRef lpMinPos As Integer, _
    ByRef lpMaxPos As Integer) As Boolean
    End Function

    <DllImport("user32.dll")> _
    Public Shared Function GetScrollPos(ByVal hWnd As IntPtr, _
    ByVal nBar As Integer) As Integer
    End Function
    Public Enum ScrollInfoMask As UInteger
        SIF_RANGE = &H1
        SIF_PAGE = &H2
        SIF_POS = &H4
        SIF_DISABLENOSCROLL = &H8
        SIF_TRACKPOS = &H10
        SIF_ALL = (SIF_RANGE Or SIF_PAGE Or SIF_POS Or SIF_TRACKPOS)
    End Enum
    Public Enum SBOrientation As Integer
        SB_HORZ = &H0
        SB_VERT = &H1
        SB_CTL = &H2
        SB_BOTH = &H3
    End Enum
    <Serializable(), StructLayout(LayoutKind.Sequential)> _
    Structure SCROLLINFO
        Public cbSize As Integer
        <MarshalAs(UnmanagedType.U4)> Public fMask As ScrollInfoMask
        Public nMin As Integer
        Public nMax As Integer
        Public nPage As UInteger
        Public nPos As Integer
        Public nTrackPos As Integer
    End Structure
    <DllImport("user32.dll", SetLastError:=True)> _
    Public Shared Function GetScrollInfo(hWnd As IntPtr, _
  <MarshalAs(UnmanagedType.I4)> fnBar As SBOrientation, _
    <MarshalAs(UnmanagedType.Struct)> ByRef lpsi As SCROLLINFO) As Integer
    End Function
    Private Sub ScrollToEnd(ByRef rtb As RichTextBoxEx)
        If rtb.InvokeRequired Then

            Dim d As New Log_Scoll(AddressOf ScrollToEnd)
            d.Invoke(rtb)
        End If
        Dim scrollMin As Integer = 0
        Dim Sinfo As New SCROLLINFO
        Sinfo.cbSize = Marshal.SizeOf(Sinfo)
        Sinfo.fMask = ScrollInfoMask.SIF_POS
        Dim scrollMax As Integer = 0
        Dim test As Integer = GetScrollInfo(rtb.Handle, SBOrientation.SB_VERT, Sinfo)

        If (GetScrollRange(rtb.Handle, SBS_VERT, scrollMin, scrollMax)) Then
            Dim pos As Integer = GetScrollPos(rtb.Handle, SBS_VERT)
            If scrollMax = Pos_Old Then
                rtb.SelectionStart = rtb.Text.Length
            End If
            'Pos_Old = GetScrollPos(rtb.Handle, SBS_VERT)
            ' Detect if they're at the bottom
        End If
    End Sub

#End Region

    Public Enum fColorEnum
        DefaultColor
        Say
        Shout
        Whisper
        Emote
        Emit
    End Enum

    Public Function fColor(Optional ByVal MyColor As fColorEnum = fColorEnum.DefaultColor) As System.Drawing.Color
        Try
            Select Case MyColor
                Case fColorEnum.DefaultColor
                    Return cMain.DefaultColor
                Case fColorEnum.Emit
                    Return cMain.EmitColor
                Case fColorEnum.Say
                    Return cMain.SayColor
                Case fColorEnum.Shout
                    Return cMain.ShoutColor
                Case fColorEnum.Whisper
                    Return cMain.WhColor
                Case fColorEnum.Emote
                    Return cMain.EmoteColor
                Case Else
                    Return cMain.DefaultColor
            End Select
        Catch Ex As Exception
            Dim logError As New ErrorLogging(Ex, Me)
        End Try
        ' Return 
    End Function


#Region "RegEx filters"
    Private Const EntryFilter As String = "^<font color='([^']*?)'>(.*?)</font>$"
    Private Const NameFilter As String = "<name shortname='([^']*)' ?(.*?)?>([\x21-\x3B\=\x3F-\x7E]+)</name>"
    Private Const DescFilter As String = "<desc shortname='([^']*)' />(.*)"
    Private Const ChannelNameFilter As String = "<channel name='(.*?)' />"
    Private Const Iconfilter As String = "<img src='fsh://system.fsh:([^']*)'(.*?)/>"
    Private Const YouSayFilter As String = "You ([\x21-\x3B\=\x3F-\x7E]+), ""([^']*)"""
    Private Const DiceFilter As String = "^<font color='roll'><img src='fsh://system.fsh:101' alt='@roll' /><channel name='@roll' /> <name shortname='([^ ]+)'>([^ ]+)</name> rolls (\d+)d(\d+)((-|\+)\d+)? ?(.*) & gets (\d+)\.</font>$"
#End Region

    Public Function PortOpen(ByRef port As Integer) As Boolean

        ' Evaluate current system tcp connections. This is the same information provided
        ' by the netstat command line application, just in .Net strongly-typed object
        ' form.  We will look through the list, and if our port we would like to use
        ' in our TcpClient is occupied, we will set isAvailable to false.
        Dim ipGlobalProperties__1 As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties()
        Dim tcpConnInfoArray As TcpConnectionInformation() = ipGlobalProperties__1.GetActiveTcpConnections()

        For Each tcpi As TcpConnectionInformation In tcpConnInfoArray
            If tcpi.LocalEndPoint.Port = port Then
                Return False
            End If
        Next
        Return True
        ' At this point, if isAvailable is true, we can proceed accordingly.
    End Function


#Region " Methods"

    Private Sub ProxyError(eX As Exception, o As Object, n As String) Handles smProxy.Error
        sndDisplay(o.ToString + "- " + n + ": " + eX.Message)
        'sndDisplay(eX.Message)
        'Dim logError As New ErrorLogging(eX, Me)

    End Sub


    Public Sub AddDataToList(ByRef lb As Controls.RichTextBoxEx, ByRef obj As String, ByRef newColor As fColorEnum)
        If InvokeRequired Then
            Dim dataArray() As Object = {lb, obj, newColor}
            Me.Invoke(New AddDataToListCaller(AddressOf AddDataToList), dataArray)
        Else
            If lb.GetType().ToString.Contains("Controls.RichTextBoxEx") Then
                'Pos_Old = GetScrollPos(lb.Handle, SBS_VERT)
                Dim build As New System.Text.StringBuilder(obj)
                build = build.Replace("</b>", "\b0 ")
                build = build.Replace("<b>", "\b ")
                build = build.Replace("</i>", "\i0 ")
                build = build.Replace("<i>", "\i ")
                build = build.Replace("</ul>", "\ul0 ")
                build = build.Replace("<ul>", "\ul ")

                build = build.Replace("&lt;", "<")
                build = build.Replace("&gt;", ">")


                Dim Names As MatchCollection = Regex.Matches(obj, NameFilter)
                For Each Name As System.Text.RegularExpressions.Match In Names
                    build = build.Replace(Name.ToString, Name.Groups(3).Value)
                Next
                '<name shortname='acuara' forced>
                Dim MyIcon As MatchCollection = Regex.Matches(obj, Iconfilter)
                For Each Icon As System.Text.RegularExpressions.Match In MyIcon
                    Select Case Icon.Groups(1).Value
                        Case "91"
                            build = build.Replace(Icon.ToString, "[#]")
                        Case Else
                            build = build.Replace(Icon.ToString, "[" + Icon.Groups(1).Value + "]")
                    End Select

                Next

                'Dim myColor As System.Drawing.Color = fColor(newColor)
                lb.ReadOnly = False
                lb.BeginUpdate()

                lb.SelectionStart = lb.TextLength
                lb.SelectedRtf = FormatText(build.ToString, newColor)

                'since we Put the Data in the RTB now we Finish Setting the Links
                Dim param() As String = {"<a.*?href=['""](.*?)['""].*?>(.*?)</a>", "<a.*?href=(.*?)>(.*?)</a>"}
                For i As Integer = 0 To param.Length - 1
                    Dim links As MatchCollection = Regex.Matches(lb.Text, param(i), RegexOptions.IgnoreCase)
                    ' links = links & Regex.Matches(lb.Text, "<a.*?href='(.*?)'.*?>(.*?)</a>", RegexOptions.IgnoreCase)
                    For Each mmatch As System.Text.RegularExpressions.Match In links
                        Dim matchUrl As String = mmatch.Groups(1).Value
                        Dim matchText As String = mmatch.Groups(2).Value
                        If mmatch.Success Then
                            With lb
                                .Find(mmatch.ToString)
                                'WAIT Snag Image Links first!
                                'Dim snag As Match = Regex.Match(matchText, "IMG:(.*?)::")
                                'If snag.Success Then
                                '    Dim RTFimg As New RTFBuilder
                                '    RTFimg.InsertImage(LoadImageFromUrl(snag.Groups(1).ToString))
                                '    .SelectedRtf = RTFimg.ToString
                                'Else
                                .SelectedRtf = FormatURL(matchText & "\v #" & matchUrl & "\v0 ")
                                .Find(matchText & "#" & matchUrl, RichTextBoxFinds.WholeWord)
                                .SetSelectionLink(True)
                                'End If
                                'Put the Link in

                            End With
                        End If
                    Next
                Next
                'Dim Images As MatchCollection = Regex.Matches(lb.Text, "<img .*?src=[""']?([^'"">]+)[""']?.*?>", RegexOptions.IgnoreCase)
                'For Each Image As Match In Images
                '    Dim img As String = Image.Groups(1).Value
                '    Dim alt As String = Image.Groups(2).Value
                '    With lb
                '        .SelectionStart = lb.Text.IndexOf(Image.ToString)
                '        .SelectionLength = Image.ToString.Length
                '        Dim RTFimg As New RTFBuilder
                '        'RTFimg.Append("IMG:" & img & "::")
                '        RTFimg.InsertImage(LoadImageFromUrl(img))
                '        .SelectedRtf = RTFimg.ToString
                '    End With
                'Next

                'Dim SysImages As MatchCollection = Regex.Matches(lb.Text, "\$(.[0-9]+)\$")
                'For Each SysMatch As Match In SysImages
                '    Dim idx As Integer = Convert.ToInt32(SysMatch.Groups(1).ToString)
                '    With lb
                '        .Find(SysMatch.ToString)
                '        Dim RTFimg As New RTFBuilder
                '        RTFimg.InsertImage(IMGresize(GetFrame(idx), log_))
                '        .SelectedRtf = RTFimg.ToString
                '    End With
                'Next
                '' 
                'SysImages = Regex.Matches(lb.Text, "#C(.?)?")
                'For Each SysMatch As Match In SysImages
                '    Dim idx As Integer = Helper.CharToDescTag(SysMatch.Groups(1).ToString)
                '    With lb
                '        .Find(SysMatch.ToString)
                '        Dim RTFimg As New RTFBuilder
                '        RTFimg.InsertImage(IMGresize(GetFrame(idx, "desctags.fox"), log_))
                '        .SelectedRtf = RTFimg.ToString
                '    End With
                'Next
                'SysImages = Regex.Matches(lb.Text, "#S(.?)?")
                'For Each SysMatch As Match In SysImages
                '    With lb
                '        .Find(SysMatch.ToString)
                '        .SelectedRtf = GetSmily(SysMatch.Groups(1).Value)
                '    End With
                'Next


                Try
                    Dim SelStart As Integer = 0
                    While (lb.Lines.Length > 350)
                        'Array.Copy(lb.Lines, 1, lb.Lines, 0, lb.Lines.Length - 1)
                        SelStart = lb.SelectionStart
                        lb.SelectionStart = 0
                        lb.SelectionLength = lb.Text.IndexOf(vbLf, 0) + 1
                        lb.SelectedText = ""
                        lb.SelectionStart = SelStart
                    End While
                Catch
                    lb.Clear()
                    Console.WriteLine("Reset Log box due to over flow")
                End Try
                lb.EndUpdate()
                lb.ReadOnly = True

            End If



        End If
    End Sub

    Private Function GetFrame(ByRef Img As Integer, Optional ByRef FSH As String = "system.fsh") As Bitmap
        If Not File.Exists(FurcPath.GetDefaultPatchPath() & FSH) Then
            Dim pos As Integer = FSH.LastIndexOf(".")
            FSH = FSH.Substring(0, pos) & ".fox"
        End If
        Dim shape As New FurcadiaShapes(FurcPath.GetDefaultPatchPath() & FSH)
        Dim anims As Bitmap() = Helper.ToBitmapArray(shape)
        ' pic.Image = anims(Img)

        Return anims(Img)
    End Function

    Private Function IMGresize(ByRef bm_source As Bitmap, ByRef RTF As RichTextBoxEx) As Bitmap
        'Dim g As Graphics = Me.CreateGraphic
        Dim g As Drawing.Graphics = Me.CreateGraphics()
        Dim x, y As Integer

        y = CInt(RTF.SelectionFont.Height * 72 / g.DpiX)
        Dim dy As Integer = y - bm_source.Height
        x = bm_source.Width - dy

        ' Make a bitmap for the result.
        Dim bm_dest As New Bitmap(x, y)

        ' Make a Graphics object for the result Bitmap.
        Dim gr_dest As Graphics = Graphics.FromImage(bm_dest)
        gr_dest.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        ' gr_dest.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        ' Copy the source image into the destination bitmap.
        gr_dest.DrawImage(bm_source, 0, 0, bm_dest.Width, bm_dest.Height)
        Return bm_dest

        ' Display the result.


    End Function
    Public Function GetSmily(ByRef ch As Char) As String

        Dim RTFimg As New RTFBuilder
        Dim file As String = ""
        Dim shape As Integer = 0
        If (ch >= "A") And (ch <= "P") Then
            file = "smileys.fsh"
            shape = Asc(ch) - Asc("A")
        ElseIf (ch >= "Q" And ch <= "Z") Then
            file = "smileys2.fsh"
            shape = Asc(ch) - Asc("Q")
        ElseIf (ch >= "a" And ch <= "z") Then
            file = "smileys2.fsh"
            shape = Asc(ch) - Asc("a") + 10
        ElseIf (ch >= "1" And ch <= "3") Then
            file = "smileys2.fsh"
            shape = Asc(ch) - Asc("1") + 35
        End If
        RTFimg.InsertImage(IMGresize(GetFrame(shape, file), log_))
        Return RTFimg.ToString

    End Function

    Private ImageList As New Dictionary(Of String, Image)
    ''' <summary>
    ''' Downloads an image file from a web server and Stores it in a temp file location
    ''' if it doesn't already exist at that location
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns>image</returns>
    ''' <remarks></remarks>
    Public Function LoadImageFromUrl(ByRef url As String) As Image
        Dim img As Image = Nothing
        Dim ImageFile As String = System.IO.Path.GetFileName(url)
        Dim TempPath As String = Path.GetTempPath()
        Dim Filename As String = ""
        If url.StartsWith("fsh://") = False Then Filename = ImageFile.Substring(0, ImageFile.Length - 4)
        If url.StartsWith("fsh://") Then
            Dim start As Integer = url.LastIndexOf("/") + 1
            Dim last As Integer = url.LastIndexOf(":") - start
            Dim FSH As String = url.Substring(start, last)

            img = GetFrame(CInt(ImageFile), FSH)
        ElseIf ImageList.ContainsKey(Filename) Then
            img = ImageList.Item(Filename)
        ElseIf Not File.Exists(TempPath & "/" & ImageFile) Then
            Try
                Dim request As HttpWebRequest = DirectCast(HttpWebRequest.Create(url), HttpWebRequest)
                Dim response As HttpWebResponse = DirectCast(request.GetResponse, HttpWebResponse)
                img = Image.FromStream(response.GetResponseStream())
                response.Close()
                img.Save(TempPath & "/" & ImageFile)
            Catch
                Console.WriteLine("Error 404: Could not read " & ImageFile & " from Apollo.furcadia.com")
            End Try
            If ImageList.ContainsKey(Filename) = False Then ImageList.Add(Filename, img)
        Else
            img = Image.FromFile(TempPath & "/" & ImageFile)
            If ImageList.ContainsKey(Filename) = False Then ImageList.Add(Filename, img)
        End If
        Return IMGresize(CType(img, Bitmap), log_)
    End Function

    Private Sub log__KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles log_.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        ElseIf (e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control) Then
            e.SuppressKeyPress = True
        End If
    End Sub


    Private Sub log__LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles log_.LinkClicked
        Dim Proto As String = ""
        Dim Str As String = e.LinkText
        Try
            If Str.Contains("#") Then
                Proto = Str.Substring(InStr(Str, "#"))
                Proto = Proto.Substring(0, InStr(Proto, "://") - 1)
            End If
        Catch
        End Try
        Select Case Proto.ToLower
            Case "http"
                Try
                    Me.Cursor = System.Windows.Forms.Cursors.AppStarting
                    Dim url As String = Str.Substring(InStr(Str, "#"))
                    Process.Start(url)
                Catch ex As Exception
                Finally
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                End Try
            Case "https"
                Try
                    Me.Cursor = System.Windows.Forms.Cursors.AppStarting
                    Dim url As String = Str.Substring(InStr(Str, "#"))
                    Process.Start(url)
                Catch ex As Exception
                Finally
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                End Try
            Case Else
                MsgBox("Protocol: """ & Proto & """ Not yet implemented")
        End Select
        'MsgBox(Proto)
    End Sub



    Public Sub UpDateDreamList(ByRef name As String) '
        Try
            If Me.DreamList.InvokeRequired OrElse DreamCountTxtBx.InvokeRequired Then
                Me.Invoke(New UpDateDreamListCaller(AddressOf UpDateDreamList), name)
            Else
                Dim fList As New Dictionary(Of UInteger, FURRE)
                fList = DREAM.List
                Dim p As KeyValuePair(Of UInteger, FURRE)
                DreamList.BeginUpdate()
                If name = "" Then
                    DreamList.Items.Clear()
                    For Each p In fList
                        If Not String.IsNullOrEmpty(fList.Item(p.Key).Name) Then
                            DreamList.Items.Add(Web.HttpUtility.HtmlDecode(fList.Item(p.Key).Name))
                        Else
                            DreamList.Items.Remove(p.Key)
                        End If
                    Next
                Else
                    DreamList.Items.Add(Web.HttpUtility.HtmlDecode(name))
                End If

                DreamCountTxtBx.Text = fList.Count.ToString
                DreamList.EndUpdate()
            End If
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me, DREAM.List.ToString)
            'logError = New ErrorLogging(eX, p)
        End Try
    End Sub

    Public Function setLogName(ByRef bfile As cBot) As String
        Select Case bfile.LogOption
            Case 0
                Return bfile.LogNameBase
            Case 1
                bfile.LogIdx += 1
                bfile.SaveBotSettings()
                Return bfile.LogNameBase & cBot.LogIdx.ToString
            Case 2
                Return bfile.LogNameBase & Date.Now().ToString("MM_dd_yyyy_H-mm-ss")

        End Select
        Return "Default"
    End Function

    Public Sub sndDisplay(ByRef data As String, Optional ByRef newColor As fColorEnum = fColorEnum.DefaultColor)
        Try
            'data = data.Replace(vbLf, vbCrLf)
            If cBot.log Then LogStream.Writeline(data)
            If CBool(cMain.TimeStamp) Then
                Dim Now As String = DateTime.Now.ToLongTimeString
                data = Now.ToString & ": " & data
            End If
            AddDataToList(log_, data, newColor)
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub



    Private Sub DreamList_DoubleClick(sender As Object, e As System.EventArgs) Handles DreamList.DoubleClick
        If Not bConnected() Then Exit Sub
        sndServer("l " + Web.HttpUtility.HtmlEncode(DreamList.SelectedItem.ToString))
    End Sub
    Public Sub sndServer(ByRef data As String)
        Try
            If Not bConnected() Then Exit Sub
            If data.StartsWith("`m ") Then
                ' TriggerCmd(MS0_MOVE)
                Select Case data.Substring(2, 1)
                    Case "7"
                        '     TriggerCmd(MS0_MOVENW)
                    Case "9"
                        '   TriggerCmd(MS0_MOVENE)
                    Case "1"
                        '  TriggerCmd(MS0_MOVESW)
                    Case "3"
                        '  TriggerCmd(MS0_MOVESE)
                End Select
            ElseIf data = "`use" Then

            ElseIf data = "`get" Then

            ElseIf data = "`>" Then

            ElseIf data = "`<" Then

            ElseIf data = "`lie" Then

            ElseIf data = "`liedown" Then

            ElseIf data = "`sit" Then

            ElseIf data = "`stand" Then

            ElseIf data.StartsWith("banish ") Then
                BanishName = data.Substring(7)
                MainMSEngine.PageSetVariable("BANISHNAME", BanishName)
            ElseIf data.StartsWith("banish-off ") Or data.StartsWith("tempbanish ") Then
                BanishName = data.Substring(11)
                MainMSEngine.PageSetVariable("BANISHNAME", BanishName)
            ElseIf data = "banish-list" Then
                BanishName = ""
                MainMSEngine.PageSetVariable("BANISHNAME", "")
            End If

            TextToServer(data)
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub

    Public Sub TextToServer(ByRef arg As String)
        Try

            If String.IsNullOrWhiteSpace(arg) Then Exit Sub
            'Clean Text input to match Client 
            Dim result As String = ""
            Select Case arg.Substring(0, 1)
                Case "`"
                    result = arg.Remove(0, 1)
                Case "/"
                    result = "wh " & arg.Substring(1)
                Case ":"
                    result = arg

                Case "-"
                    result = arg

                Case Else
                    result = Chr(34) & arg
            End Select
            SndToServer(result)

        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me, arg)
        End Try
    End Sub

    Public Sub SndToServer(ByVal data As String)
        If String.IsNullOrEmpty(data) Then Exit Sub
        Try
            SyncLock Lock
                ServerStack.Enqueue(data)
                If g_mass + MASS_SPEECH <= MASS_CRITICAL Then
                    'g_mass = data.Length - 2
                    on_Tick(0)
                End If
            End SyncLock


        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me, data)
            Debug.Print("SndToServer: " & data)
            Debug.Print(eX.Message)
        End Try
    End Sub

    Public DiceSides As Double = 0.0R
    Public DiceCount As Double = 0.0R
    Public DiceCompnentMatch As String
    Public DiceModifyer As Double = 0.0R
    Public DiceResult As Double = 0.0R
    Public Structure Rep
        Public ID As String
        Public type As Integer
    End Structure
    Public Repq As Queue(Of Rep) = New Queue(Of Rep)
    Private Sub ParseServerData(ByVal data As String, ByVal Handled As Boolean)

        ' page = engine.LoadFromString(cBot.MS_Script)
        If data = "Dragonroar" Then
            BotConnecting()
            '  Login Sucessful
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data = "&&&&&&&&&&&&&" Then
            loggingIn = 2
            TS_Status_Client.Image = My.Resources.images3
            ReLogCounter = 0
            '(0:1) When the bot logs into furcadia,
            MainMSEngine.PageExecute(1)
            If Not IsNothing(ReconnectTimeOutTimer) Then ReconnectTimeOutTimer.Dispose()
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            ' Species Tags
        ElseIf data.StartsWith("]-") Then
            If data.StartsWith("]-#A") Then
                SpeciesTag.Enqueue(data.Substring(4))
            ElseIf data.StartsWith("]-#B") Then
                BadgeTag.Enqueue(data.Substring(2))
            End If

            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'DS Variables

            'Popup Dialogs!
        ElseIf data.StartsWith("]#") Then
            ']#<idstring> <style 0-17> <message that might have spaces in>
            Dim repqq As Regex = New Regex("^\]#(.*?) (\d+) (.*?)$")
            Dim m As System.Text.RegularExpressions.Match = repqq.Match(data)
            Dim r As Rep
            r.ID = m.Groups(1).Value
            r.type = m.Groups(2).Value.ToInteger
            Repq.Enqueue(r)
            MainMSEngine.PageSetVariable("MESSAGE", m.Groups(3).Value, True)
            Player.Message = m.Groups(3).Value
            MainMSEngine.PageExecute(95, 96)
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("0") Then
            InDream = True
            'Phoenix Speak event
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("3") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'self Induced Dragon Speak Event
        ElseIf data.StartsWith("6") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Dragon Speak event
        ElseIf data.StartsWith("7") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Dragon Speak Addon (Follows Instructions 6 and 7
        ElseIf data.StartsWith("8") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            ']s(.+)1 (.*?) (.*?) 0
        ElseIf data.StartsWith("]s") Then
            Dim t As New Regex("\]s(.+)1 (.*?) (.*?) 0", RegexOptions.IgnoreCase)
            Dim m As System.Text.RegularExpressions.Match = t.Match(data)
            If BotName.ToFurcShortName = m.Groups(2).Value.ToFurcShortName Then

                MainMSEngine.PageExecute()
            End If
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Look response
        ElseIf data.StartsWith("]f") And bConnected() And InDream = True Then
            Dim length As Short = 14
            If Look Then
                LookQue.Enqueue(data.Substring(2))
            Else
                If data.Substring(2, 1) <> "t" Then
                    length = 30
                Else
                    length = 14
                End If
                Try
                    Player = NametoFurre(data.Remove(0, length + 2), True)
                    ' If Player.ID = 0 Then Exit Sub
                    Player.Color = data.Substring(2, length)
                    If IsBot(Player) Then Look = False
                    If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try

            End If
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Spawn Avatar
        ElseIf data.StartsWith("<") And bConnected() Then
            Try
                If data.Length < 29 Then Exit Sub
                ' Debug.Print(data)
                Player.ID = ConvertFromBase220(data.Substring(1, 4))

                If DREAM.List.ContainsKey(Player.ID) Then
                    Player = DREAM.List.Item(Player.ID)
                End If


                Player.X = CUInt(ConvertFromBase220(data.Substring(5, 2)) * 2)
                Player.Y = ConvertFromBase220(data.Substring(7, 2))
                Player.Shape = ConvertFromBase220(data.Substring(9, 2))



                Dim NameLength As UInteger = ConvertFromBase220(data.Substring(11, 1))
                Player.Name = data.Substring(12, CInt(NameLength)).Replace("|", " ")

                Dim ColTypePos As UInteger = CUInt(12 + NameLength)
                Player.ColorType = CChar(data.Substring(CInt(ColTypePos), 1))
                Dim ColorSize As UInteger = 10
                If Player.ColorType <> "t" Then
                    ColorSize = 30
                End If
                Dim sColorPos As Integer = CInt(ColTypePos + 1)

                Player.Color = data.Substring(sColorPos, CInt(ColorSize))

                Dim FlagPos As Integer = data.Length - 6
                Player.Flag = CInt(ConvertFromBase220(data.Substring(FlagPos, 1)))
                Dim AFK_Pos As Integer = data.Length - 5
                Dim AFKStr As String = data.Substring(AFK_Pos, 4)
                Player.AFK = ConvertFromBase220(data.Substring(AFK_Pos, 4))
                Dim FlagCheck As Integer = CType(Flags.CHAR_FLAG_NEW_AVATAR, Integer) - Player.Flag

                ' Add New Arrivals to Dream List
                ' One or the other will trigger it
                IsBot(Player)
                MainMSEngine.PageSetVariable(MS_Name, Player.ShortName)

                If Player.Flag = 4 Or Not DREAM.List.ContainsKey(Player.ID) Then
                    DREAM.List.Add(Player.ID, Player)
                    If InDream Then UpDateDreamList(Player.Name)
                    If Player.Flag = 2 Then
                        Dim Bot As FURRE = fIDtoFurre(CUInt(BotUID))
                        Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(CInt(Bot.X), CInt(Bot.Y))
                        If VisableRectangle.X <= Player.X And VisableRectangle.Y <= Player.Y And VisableRectangle.height >= Player.Y And VisableRectangle.length >= Player.X Then
                            Player.Visible = True
                        Else
                            Player.Visible = False
                        End If
                        MainMSEngine.PageExecute(28, 29, 24, 25)
                    Else
                        MainMSEngine.PageExecute(24, 25)
                    End If
                ElseIf Player.Flag = 2 Then
                    Dim Bot As FURRE = fIDtoFurre(CUInt(BotUID))
                    Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(CInt(Bot.X), CInt(Bot.Y))
                    If VisableRectangle.X <= Player.X And VisableRectangle.Y <= Player.Y And VisableRectangle.height >= Player.Y And VisableRectangle.length >= Player.X Then
                        Player.Visible = True
                    Else
                        Player.Visible = False
                    End If
                    MainMSEngine.PageExecute(28, 29)

                ElseIf Player.Flag = 1 Then

                ElseIf Player.Flag = 0 Then

                End If
                If DREAM.List.ContainsKey(Player.ID) Then
                    DREAM.List.Item(Player.ID) = Player
                End If
            Catch eX As Exception

                Dim logError As New ErrorLogging(eX, Me)
                Exit Sub
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Remove Furre
        ElseIf data.StartsWith(")") And bConnected() Then 'And loggingIn = False
            Try
                Dim remID As UInteger = ConvertFromBase220(data.Substring(1, 4))
                ' remove departure from List
                If DREAM.List.ContainsKey(remID) = True Then
                    Player = DREAM.List.Item(remID)
                    MainMSEngine.PageSetVariable(MS_Name, Player.Name)
                    MainMSEngine.PageExecute(26, 27, 30, 31)
                    DREAM.List.Remove(remID)
                    UpDateDreamList("")
                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Animated Move
        ElseIf data.StartsWith("/") And bConnected() Then 'And loggingIn = False
            Try
                Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)))
                Player.X = CUInt(ConvertFromBase220(data.Substring(5, 2)) * 2)
                Player.Y = ConvertFromBase220(data.Substring(7, 2))
                Player.Shape = ConvertFromBase220(data.Substring(9, 2))
                Dim Bot As FURRE = fIDtoFurre(CUInt(BotUID))
                Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(CInt(Bot.X), CInt(Bot.Y))
                If VisableRectangle.X <= Player.X And VisableRectangle.Y <= Player.Y And VisableRectangle.height >= Player.Y And VisableRectangle.length >= Player.X Then
                    Player.Visible = True
                Else
                    Player.Visible = False
                End If
                If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                IsBot(Player)
                MainMSEngine.PageSetVariable(MS_Name, Player.ShortName)
                MainMSEngine.PageExecute(28, 29, 30, 31, 601, 602)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            ' Move Avatar
        ElseIf data.StartsWith("A") And bConnected() Then 'And loggingIn = False
            Try
                Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)))
                Player.X = CUInt(ConvertFromBase220(data.Substring(5, 2)) * 2)
                Player.Y = ConvertFromBase220(data.Substring(7, 2))
                Player.Shape = ConvertFromBase220(data.Substring(9, 2))

                Dim Bot As FURRE = fIDtoFurre(CUInt(BotUID))
                Dim VisableRectangle As ViewArea = getTargetRectFromCenterCoord(CInt(Bot.X), CInt(Bot.Y))
                If VisableRectangle.X <= Player.X And VisableRectangle.Y <= Player.Y And VisableRectangle.height >= Player.Y And VisableRectangle.length >= Player.X Then

                    Player.Visible = True
                Else
                    Player.Visible = False
                End If
                If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player

                IsBot(Player)
                MainMSEngine.PageSetVariable(MS_Name, Player.ShortName)
                MainMSEngine.PageExecute(28, 29, 30, 31, 601, 602)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            ' Update Color Code
        ElseIf data.StartsWith("B") <> False And bConnected() And InDream Then 'And loggingIn = False
            Try
                Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)))
                Player.Shape = ConvertFromBase220(data.Substring(5, 2))
                Dim ColTypePos As UInteger = 7
                Player.ColorType = CChar(data.Substring(CInt(ColTypePos), 1))
                Dim ColorSize As UInteger = 10
                If Player.ColorType <> "t" Then
                    ColorSize = 30
                End If
                Dim sColorPos As UInteger = CUInt(ColTypePos + 1)
                Player.Color = data.Substring(CInt(sColorPos), CInt(ColorSize))
                If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                IsBot(Player)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Hide Avatar
        ElseIf data.StartsWith("C") <> False And bConnected() Then 'And loggingIn = False
            Try
                Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)))
                Player.X = CUInt(ConvertFromBase220(data.Substring(5, 2)) * 2)
                Player.Y = ConvertFromBase220(data.Substring(7, 2))
                Player.Visible = False
                If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                IsBot(Player)
                MainMSEngine.PageSetVariable(MS_Name, Player.Name)
                MainMSEngine.PageExecute(30, 31)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Display Disconnection Dialog
        ElseIf data.StartsWith("[") Then
#If DEBUG Then
            Console.WriteLine("Disconnection Dialog:" & data)
#End If
            InDream = False
            DREAM.List.Clear()
            UpDateDreamList("")

            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            MsgBox(data, MsgBoxStyle.Critical, "Disconnection Error")

            Exit Sub


            ';{mapfile}	Load a local map (one in the furcadia folder)
            ']q {name} {id}	Request to download a specific patch
        ElseIf data.StartsWith(";") OrElse data.StartsWith("]q") OrElse data.StartsWith("]r") Then
            Try
#If DEBUG Then
                Debug.Print("Entering new Dream" & data)
#End If
                MainMSEngine.PageSetVariable("DREAMOWNER", "")
                MainMSEngine.PageSetVariable("DREAMNAME", "")
                HasShare = False
                NoEndurance = False

                DREAM.List.Clear()
                UpDateDreamList("")
                InDream = False
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("]z") Then
            BotUID = UInteger.Parse(data.Remove(0, 2))
            'Snag out UID
        ElseIf data.StartsWith("]B") Then
            Try
                BotUID = UInteger.Parse(data.Remove(0, 2))
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("~") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("=") Then
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("]c") Then
#If DEBUG Then
            Console.WriteLine(data)
#End If
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("]C") Then
            If data.StartsWith("]C0") Then
                Dim dname As String = data.Substring(10)
                If dname.Contains(":") Then
                    Dim NameStr As String = dname.Substring(0, dname.IndexOf(":"))
                    If NameStr.ToFurcShortName = BotName.ToFurcShortName Then
                        HasShare = True
                    End If
                    MainMSEngine.PageSetVariable(Main.VarPrefix & "DREAMOWNER", NameStr)
                ElseIf dname.EndsWith("/") AndAlso Not dname.Contains(":") Then
                    Dim NameStr As String = dname.Substring(0, dname.IndexOf("/"))
                    If NameStr.ToFurcShortName = BotName.ToFurcShortName Then
                        HasShare = True
                    End If
                    MainMSEngine.PageSetVariable("DREAMOWNER", NameStr)
                End If

                MainMSEngine.PageSetVariable("DREAMNAME", dname)
                MainMSEngine.PageExecute(90, 91)
            End If
#If DEBUG Then
            Console.WriteLine(data)
#End If
            If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
            Exit Sub
            'Process Channels Seperatly
        ElseIf data.StartsWith("(") Then
            If ThroatTired = False And data.StartsWith("(<font color='warning'>Your throat is tired. Try again in a few seconds.</font>") Then
                ThroatTired = True

                'Throat Tired Syndrome, Halt all out going data for a few seconds
                Dim Ts As TimeSpan = TimeSpan.FromSeconds(cMain.TT_TimeOut)
                TroatTiredDelay = New Threading.Timer(AddressOf TroatTiredDelayTick, _
                   Nothing, Ts, Ts)
                '(0:92) When the bot detects the "Your throat is tired. Please wait a few seconds" message,
                MainMSEngine.PageExecute(92)
                'Exit Sub
                If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)
                Exit Sub
            End If

            ChannelProcess(data, Handled)
            Exit Sub
        End If

        If smProxy.IsClientConnected Then smProxy.SendClient(data + vbLf)

    End Sub

    Dim pslen As Integer = 0
    Dim NextLetter As Char = Nothing
    Dim ChannelLock As New Object
    Public Channel As String
    ''' <summary>
    ''' Channel Parser
    ''' RegEx Style Processing here
    ''' </summary>
    ''' <param name="data"></param>
    ''' <remarks></remarks>
    Public Sub ChannelProcess(ByRef data As String, ByVal Handled As Boolean)
        'Strip the trigger Character
        ' page = engine.LoadFromString(cBot.MS_Script)
        data = data.Remove(0, 1)
        Dim SpecTag As String = ""
        Channel = Regex.Match(data, ChannelNameFilter).Groups(1).Value
        Dim Color As String = Regex.Match(data, EntryFilter).Groups(1).Value
        Dim User As String = ""
        Dim Desc As String = ""
        Dim Text As String = ""
        If Not Handled Then
            Text = Regex.Match(data, EntryFilter).Groups(2).Value
            User = Regex.Match(data, NameFilter).Groups(3).Value
            If User <> "" Then Player = NametoFurre(User, True)
            Player.Message = ""
            Desc = Regex.Match(data, DescFilter).Groups(2).Value
            Dim mm As New Regex(Iconfilter)
            Dim ds As System.Text.RegularExpressions.Match = mm.Match(Text)
            Text = mm.Replace(Text, "[" + ds.Groups(1).Value + "] ")
            Dim s As New Regex(ChannelNameFilter)
            Text = s.Replace(Text, "")
        Else
            User = Player.Name
            Text = Player.Message
        End If
        DiceSides = 0
        DiceCount = 0
        DiceCompnentMatch = ""
        DiceModifyer = 0
        DiceResult = 0

        SyncLock Lock
            ErrorMsg = ""
            ErrorNum = 0
        End SyncLock
        If Channel = "@news" Or Channel = "@spice" Then
            Try
                sndDisplay(Text)
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf Color = "success" Then
            Try
                If Text.Contains(" has been banished from your dreams.") Then
                    'banish <name> (online)
                    'Success: (.*?) has been banished from your dreams. 

                    '(0:52) When the bot sucessfilly banishes a furre,
                    '(0:53) When the bot sucessfilly banishes the furre named {...},
                    Dim t As New Regex("(.*?) has been banished from your dreams.")
                    BanishName = t.Match(Text).Groups(1).ToString
                    MainMSEngine.PageSetVariable("BANISHNAME", BanishName)

                    BanishString.Add(BanishName)
                    MainMSEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                    MainMSEngine.PageExecute(52, 53)

                    ' MainMSEngine.PageExecute(53)
                ElseIf Text = "You have canceled all banishments from your dreams." Then
                    'banish-off-all (active list)
                    'Success: You have canceled all banishments from your dreams. 

                    '(0:60) When the bot successfully clears the banish list
                    BanishString.Clear()
                    MainMSEngine.PageSetVariable("BANISHLIST", "")
                    MainMSEngine.PageSetVariable("BANISHNAME", "")
                    MainMSEngine.PageExecute(60)

                ElseIf Text.EndsWith(" has been temporarily banished from your dreams.") Then
                    'tempbanish <name> (online)
                    'Success: (.*?) has been temporarily banished from your dreams. 

                    '(0:61) When the bot sucessfully temp banishes a Furre
                    '(0:62) When the bot sucessfully temp banishes the furre named {...}
                    Dim t As New Regex("(.*?) has been temporarily banished from your dreams.")
                    BanishName = t.Match(Text).Groups(1).Value
                    MainMSEngine.PageSetVariable(Main.VarPrefix & "BANISHNAME", BanishName)
                    '  MainMSEngine.PageExecute(61)
                    BanishString.Add(BanishName)
                    MainMSEngine.PageSetVariable(VarPrefix & "BANISHLIST", String.Join(" ", BanishString.ToArray))
                    MainMSEngine.PageExecute(61, 62)

                ElseIf Text = "Control of this dream is now being shared with you." Then
                    HasShare = True

                ElseIf Text.EndsWith("is now sharing control of this dream with you.") Then
                    HasShare = True

                ElseIf Text.EndsWith("has stopped sharing control of this dream with you.") Then
                    HasShare = False

                ElseIf Text.StartsWith("The endurance limits of player ") Then
                    Dim t As New Regex("The endurance limits of player (.*?) are now toggled off.")
                    Dim m As String = t.Match(Text).Groups(1).Value.ToString
                    If m.ToFurcShortName = BotName.ToFurcShortName Then
                        NoEndurance = True
                    End If

                ElseIf Channel = "@cookie"
                	'(0:96) When the Bot sees "Your cookies are ready."
					Dim CookiesReady As Regex = New Regex(String.Format( "{0}", "Your cookies are ready.  http://furcadia.com/cookies/ for more info!"))
					If CookiesReady.Match(data).Success Then
                		MainMSEngine.PageExecute(96)
            		End If
                End If
                sndDisplay(Text)
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub

            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
        ElseIf Channel = "@roll" Then
            Dim DiceREGEX As New Regex(DiceFilter, RegexOptions.IgnoreCase)
            Dim DiceMatch As System.Text.RegularExpressions.Match = DiceREGEX.Match(data)

            'Matches, in order:
            '1:      shortname()
            '2:      full(name)
            '3:      dice(count)
            '4:      sides()
            '5: +/-#
            '6: +/-  (component match)
            '7:      additional(Message)
            '8:      Final(result)

            Player = NametoFurre(DiceMatch.Groups(3).Value, True)
            Player.Message = DiceMatch.Groups(7).Value
            MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", DiceMatch.Groups(7).Value)
            Double.TryParse(DiceMatch.Groups(4).Value, DiceSides)
            Double.TryParse(DiceMatch.Groups(3).Value, DiceCount)
            DiceCompnentMatch = DiceMatch.Groups(6).Value
            DiceModifyer = 0.0R
            Double.TryParse(DiceMatch.Groups(5).Value, DiceModifyer)
            Double.TryParse(DiceMatch.Groups(8).Value, DiceResult)

            If IsBot(Player) Then
                MainMSEngine.PageExecute(130, 131, 132, 136)
            Else
                MainMSEngine.PageExecute(133, 134, 135, 136)
            End If

            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf Channel = "@dragonspeak" OrElse Channel = "@emit" OrElse Color = "emit" Then
            Try
                '(<font color='dragonspeak'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Furcadian Academy</font>
                sndDisplay(Text, fColorEnum.Emit)

                MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", Text.Substring(5))
                ' Execute (0:21) When someone emits something
                MainMSEngine.PageExecute(21, 22, 23)
                ' Execute (0:22) When someone emits {...}
                '' Execute (0:23) When someone emits something with {...} in it

            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
            ''BCast (Advertisments, Announcments)
        ElseIf Color = "bcast" Then
            Dim AdRegEx As String = "<channel name='(.*)' />"

            Dim chan As String = Regex.Match(data, AdRegEx).Groups(1).Value
            Try

                Select Case chan
                    Case "@advertisements"
                        If cMain.Advertisment Then Exit Sub
                        AdRegEx = "\[(.*?)\] (.*?)</font>"
                        Dim adMessage As String = Regex.Match(data, AdRegEx).Groups(2).Value
                        sndDisplay(Text)
                    Case "@bcast"
                        If cMain.Broadcast Then Exit Sub
                        Dim u As String = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups(2).Value
                        sndDisplay(Text)
                    Case "@announcements"
                        If cMain.Announcement Then Exit Sub
                        Dim u As String = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups(2).Value
                        sndDisplay(Text)
                    Case Else
#If DEBUG Then
                        Console.WriteLine("Unknown ")
                        Console.WriteLine("BCAST:" & data)
#End If
                End Select


            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
            ''SAY
        ElseIf Color = "myspeech" Then
            Try
                Dim t As New Regex(YouSayFilter)
                Dim u As String = t.Match(data).Groups(1).ToString
                Text = t.Match(data).Groups(2).ToString
                If SpeciesTag.Count() > 0 Then
                    SpecTag = SpeciesTag.Peek
                    SpeciesTag.Dequeue()
                    Player.Color = SpecTag
                    If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                End If

                sndDisplay("You " & u & ", """ & Text & """", fColorEnum.Say)
                Player.Message = Text
                MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", Text)
                ' Execute (0:5) When some one says something
                'MainMSEngine.PageExecute(5, 6, 7, 18, 19, 20)
                '' Execute (0:6) When some one says {...}
                '' Execute (0:7) When some one says something with {...} in it
                '' Execute (0:18) When someone says or emotes something
                '' Execute (0:19) When someone says or emotes {...}
                '' Execute (0:20) When someone says or emotes something with {...} in it
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf User <> "" And Channel = "" And Color = "" And Regex.Match(data, NameFilter).Groups(2).Value <> "forced" Then
            Dim tt As System.Text.RegularExpressions.Match = Regex.Match(data, "\(you see(.*?)\)", RegexOptions.IgnoreCase)
            Dim t As New Regex(NameFilter)
            If Not tt.Success Then

                Try
                    Text = t.Replace(data, "")
                    Text = Text.Remove(0, 2)

                    If SpeciesTag.Count() > 0 Then
                        SpecTag = SpeciesTag.Peek
                        SpeciesTag.Clear()
                        Player.Color = SpecTag
                        If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                    End If
                    Channel = "say"
                    sndDisplay(User & " says, """ & Text & """", fColorEnum.Say)
                    MainMSEngine.PageSetVariable(MS_Name, User)
                    MainMSEngine.PageSetVariable("MESSAGE", Text)
                    Player.Message = Text
                    ' Execute (0:5) When some one says something
                    MainMSEngine.PageExecute(5, 6, 7, 18, 19, 20)
                    ' Execute (0:6) When some one says {...}
                    ' Execute (0:7) When some one says something with {...} in it
                    ' Execute (0:18) When someone says or emotes something
                    ' Execute (0:19) When someone says or emotes {...}
                    ' Execute (0:20) When someone says or emotes something with {...} in it

                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)

                End Try

                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
                Exit Sub
            Else
                Try
                    'sndDisplay("You See '" & User & "'")
                    Look = True
                Catch eX As Exception
                    Dim logError As New ErrorLogging(eX, Me)
                End Try
            End If

        ElseIf Desc <> "" Then
            Try
                Dim DescName As String = Regex.Match(data, DescFilter).Groups(1).ToString()

                Player = NametoFurre(DescName, True)
                If LookQue.Count > 0 Then
                    Dim colorcode As String = LookQue.Peek
                    If colorcode.StartsWith("t") Then
                        colorcode = colorcode.Substring(0, 14)
                    ElseIf colorcode.StartsWith("u") Then

                    ElseIf colorcode.StartsWith("v") Then
                        'RGB Values
                    End If
                    Player.Color = colorcode
                    LookQue.Dequeue()
                End If
                If BadgeTag.Count() > 0 Then
                    SpecTag = BadgeTag.Peek
                    BadgeTag.Clear()
                    Player.Badge = SpecTag
                ElseIf Player.Badge <> "" Then
                    Player.Badge = ""
                End If
                Player.Desc = Desc.Substring(6)
                If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                MainMSEngine.PageSetVariable(MS_Name, DescName)
                MainMSEngine.PageExecute(600)
                'sndDisplay)
                If Player.Tag = "" Then
                    sndDisplay("You See '" & Player.Name & "'\par" & Desc)
                Else
                    sndDisplay("You See '" & Player.Name & "'\par" & Player.Tag & " " & Desc)
                End If
                Look = False
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf Color = "shout" Then
            ''SHOUT
            Try
                Dim t As New Regex(YouSayFilter)
                Dim u As String = t.Match(data).Groups(1).ToString
                Text = t.Match(data).Groups(2).ToString
                If User = "" Then
                    sndDisplay("You " & u & ", """ & Text & """", fColorEnum.Shout)
                Else
                    Text = Regex.Match(data, "shouts: (.*)</font>").Groups(1).ToString()
                    sndDisplay(User & " shouts, """ & Text & """", fColorEnum.Shout)
                End If
                If Not IsBot(Player) Then
                    MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", Text)
                    Player.Message = Text
                    ' Execute (0:8) When some one shouts something
                    MainMSEngine.PageExecute(8, 9, 10)
                    ' Execute (0:9) When some one shouts {...}
                    ' Execute (0:10) When some one shouts something with {...} in it


                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf Color = "query" Then
            Dim QCMD As String = Regex.Match(data, "<a.*?href='command://(.*?)'>").Groups(1).ToString
            'Player = NametoFurre(User, True)
            Select Case QCMD
                Case "summon"
                    ''JOIN
                    Try
                        sndDisplay(User & " requests to join you.")
                        'If Not IsBot(Player) Then
                        MainMSEngine.PageExecute(34, 35)
                        'End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "join"
                    ''SUMMON
                    Try
                        sndDisplay(User & " requests to summon you.")
                        'If Not IsBot(Player) Then
                        MainMSEngine.PageExecute(32, 33)
                        'End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "follow"
                    ''LEAD
                    Try
                        sndDisplay(User & " requests to lead.")
                        'If Not IsBot(Player) Then
                        MainMSEngine.PageExecute(36, 37)
                        'End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "lead"
                    ''FOLLOW
                    Try
                        sndDisplay(User & " requests the bot to follow.")
                        'If Not IsBot(Player) Then
                        MainMSEngine.PageExecute(38, 39)
                        'End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case "cuddle"
                    Try
                        sndDisplay(User & " requests the bot to cuddle.")
                        'If Not IsBot(Player) Then
                        MainMSEngine.PageExecute(40, 41)
                        'End If
                    Catch eX As Exception
                        Dim logError As New ErrorLogging(eX, Me)
                    End Try
                Case Else
                    sndDisplay("## Unknown " & Channel & "## " & data)
            End Select

            'NameFilter

            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf Color = "whisper" Then
            ''WHISPER
            Try
                Dim WhisperFrom As String = Regex.Match(data, "whispers, ""(.*?)"" to you").Groups(1).Value
                Dim WhisperTo As String = Regex.Match(data, "You whisper ""(.*?)"" to").Groups(1).Value
                Dim WhisperDir As String = Regex.Match(data, String.Format("<name shortname='(.*?)' src='whisper-(.*?)'>")).Groups(2).Value
                If WhisperDir = "from" Then
                    'Player = NametoFurre(User, True)
                    Player.Message = WhisperFrom
                    If BadgeTag.Count() > 0 Then
                        SpecTag = BadgeTag.Peek
                        BadgeTag.Clear()
                        Player.Badge = SpecTag
                    Else
                        Player.Badge = ""
                    End If

                    If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player


                    sndDisplay(User & " whispers""" & WhisperFrom & """ to you.", fColorEnum.Whisper)
                    If Not IsBot(Player) Then
                        MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", Player.Message)
                        ' Execute (0:15) When some one whispers something
                        MainMSEngine.PageExecute(15, 16, 17)
                        ' Execute (0:16) When some one whispers {...}
                        ' Execute (0:17) When some one whispers something with {...} in it
                    End If


                Else
                    WhisperTo = WhisperTo.Replace("<wnd>", "")
                    sndDisplay("You whisper""" & WhisperTo & """ to " & User & ".", fColorEnum.Whisper)
                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf Color = "warning" Then
            SyncLock ChannelLock
                ErrorMsg = Text
                ErrorNum = 1
            End SyncLock
            MainMSEngine.PageExecute(801)
            sndDisplay("::WARNING:: " & Text, fColorEnum.DefaultColor)
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf Color = "trade" Then
            Dim TextStr As String = Regex.Match(data, "\s<name (.*?)</name>").Groups(0).ToString()
            Text = Text.Substring(6)
            If User <> "" Then Text = " " & User & Text.Replace(TextStr, "")
            sndDisplay(Text, fColorEnum.DefaultColor)
            MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", Text)
            Player.Message = Text
            MainMSEngine.PageExecute(46, 47, 48)
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf Color = "emote" Then
            Try
                ' ''EMOTE
                If SpeciesTag.Count() > 0 Then
                    SpecTag = SpeciesTag.Peek
                    SpeciesTag.Dequeue()
                    Player.Color = SpecTag
                End If
                Dim usr As Regex = New Regex(NameFilter)
                Dim n As System.Text.RegularExpressions.Match = usr.Match(Text)
                Text = usr.Replace(Text, "")

                Player = NametoFurre(n.Groups(3).Value, True)
                MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", Text)
                Player.Message = Text
                If DREAM.List.ContainsKey(Player.ID) Then DREAM.List.Item(Player.ID) = Player
                sndDisplay(User & " " & Text, fColorEnum.Emote)
                Dim test As Boolean = IsBot(Player)
                If IsBot(Player) = False Then

                    ' Execute (0:11) When someone emotes something
                    MainMSEngine.PageExecute(11, 12, 13, 18, 19, 20)
                    ' Execute (0:12) When someone emotes {...}
                    ' Execute (0:13) When someone emotes something with {...} in it
                    ' Execute (0:18) When someone says or emotes something
                    ' Execute (0:19) When someone says or emotes {...}
                    ' Execute (0:20) When someone says or emotes something with {...} in it
                End If
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf Color = "channel" Then
            'ChannelNameFilter2
            Dim chan As Regex = New Regex(ChannelNameFilter)
            Dim ChanMatch As System.Text.RegularExpressions.Match = chan.Match(data)
            Dim r As New Regex("<img src='(.*?)' alt='(.*?)' />")
            Dim ss As RegularExpressions.Match = r.Match(Text)
            If ss.Success Then Text = Text.Replace(ss.Groups(0).Value, "")
            r = New Regex(NameFilter + ":")
            ss = r.Match(Text)
            If ss.Success Then Text = Text.Replace(ss.Groups(0).Value, "")
            sndDisplay("[" + ChanMatch.Groups(1).Value + "] " + User & ": " & Text)
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
        ElseIf Color = "notify" Then
            Dim NameStr As String = ""
            If Text.StartsWith("Players banished from your dreams: ") Then
                'Banish-List
                '[notify> Players banished from your dreams:  
                '`(0:54) When the bot sees the banish list
                BanishString.Clear()
                Dim tmp() As String = Text.Substring(35).Split(","c)
                For Each t As String In tmp
                    BanishString.Add(t)
                Next
                MainMSEngine.PageSetVariable(VarPrefix & "BANISHLIST", String.Join(" ", BanishString.ToArray))
                MainMSEngine.PageExecute(54)

            ElseIf Text.StartsWith("The banishment of player ") Then
                'banish-off <name> (on list)
                '[notify> The banishment of player (.*?) has ended.  

                '(0:56) When the bot successfully removes a furre from the banish list,
                '(0:58) When the bot successfully removes the furre named {...} from the banish list,
                Dim t As New Regex("The banishment of player (.*?) has ended.")
                NameStr = t.Match(data).Groups(1).Value
                MainMSEngine.PageSetVariable("BANISHNAME", NameStr)
                MainMSEngine.PageExecute(56, 56)
                For I As Integer = 0 To BanishString.Count - 1
                    If BanishString.Item(I).ToString.ToFurcShortName = NameStr.ToFurcShortName Then
                        BanishString.RemoveAt(I)
                        Exit For
                    End If
                Next
                MainMSEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
            End If

            sndDisplay("[notify> " & Text, fColorEnum.DefaultColor)
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf Color = "error" Then
            SyncLock Lock
                ErrorMsg = Text
                ErrorNum = 2
            End SyncLock
            MainMSEngine.PageExecute(800)
            Dim NameStr As String = ""
            If Text.Contains("There are no furres around right now with a name starting with ") Then
                'Banish <name> (Not online)
                'Error:>>  There are no furres around right now with a name starting with (.*?) . 

                '(0:50) When the Bot fails to banish a furre,
                '(0:51) When the bot fails to banish the furre named {...},
                Dim t As New Regex("There are no furres around right now with a name starting with (.*?) .")
                NameStr = t.Match(data).Groups(1).Value
                MainMSEngine.PageSetVariable("BANISHNAME", NameStr)
                MainMSEngine.PageExecute(50, 51)
                MainMSEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
            ElseIf Text = "Sorry, this player has not been banished from your dreams." Then
                'banish-off <name> (not on list)
                'Error:>> Sorry, this player has not been banished from your dreams.

                '(0:55) When the Bot fails to remove a furre from the banish list,
                '(0:56) When the bot fails to remove the furre named {...} from the banish list,
                NameStr = BanishName
                MainMSEngine.PageSetVariable("BANISHNAME", NameStr)
                MainMSEngine.PageSetVariable("BANISHLIST", String.Join(" ", BanishString.ToArray))
                MainMSEngine.PageExecute(50, 51)
            ElseIf Text = "You have not banished anyone." Then
                'banish-off-all (empty List)
                'Error:>> You have not banished anyone. 

                '(0:59) When the bot fails to see the banish list,
                BanishString.Clear()
                MainMSEngine.PageExecute(59)
                MainMSEngine.PageSetVariable(VarPrefix & "BANISHLIST", "")
            ElseIf Text = "You do not have any cookies to give away right now!"
                MainMSEngine.PageExecute(95)
            End If

            sndDisplay("Error:>> " & Text)
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("Communication") Then
            sndDisplay("Error: Communication Error.  Aborting connection.")
            ProcExit = False
            DisconnectBot()
            'LogSaveTmr.Enabled = False

        ElseIf Channel = "@cookie" Then
            ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>
            ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> All-time Cookie total: 0</font>
            ' <font color='success'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Your cookies are ready.  http://furcadia.com/cookies/ for more info!</font>
            '<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.

			Dim CookieToMe As Regex = New Regex(String.Format("{0}", CookieToMeREGEX))
            If CookieToMe.Match(data).Success Then
                MainMSEngine.PageSetVariable(MS_Name, CookieToMe.Match(data).Groups(2).Value)
                MainMSEngine.PageExecute(42, 43)
            End If
            Dim CookieToAnyone As Regex = New Regex(String.Format("<name shortname='(.*?)'>(.*?)</name> just gave <name shortname='(.*?)'>(.*?)</name> a (.*?)"))
            If CookieToAnyone.Match(data).Success Then
                'MainMSEngine.PageSetVariable(VarPrefix & MS_Name, CookieToAnyone.Match(data).Groups(3).Value)
                If callbk.IsBot(NametoFurre(CookieToAnyone.Match(data).Groups(3).Value, True)) Then
                    MainMSEngine.PageExecute(42, 43)
                Else
                    MainMSEngine.PageExecute(44)
                End If


            End If
            Dim CookieFail As Regex = New Regex(String.Format("You do not have any (.*?) left!"))
            If CookieFail.Match(data).Success Then
                MainMSEngine.PageExecute(45)
            End If
            Dim EatCookie As Regex = New Regex(Regex.Escape("<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.") + "(.*?)")
            If EatCookie.Match(data).Success Then
                MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", "You eat a cookie." + EatCookie.Replace(data, ""))
                Player.Message = "You eat a cookie." + EatCookie.Replace(data, "")
                MainMSEngine.PageExecute(49)

            End If
            sndDisplay(Text, fColorEnum.DefaultColor)
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        ElseIf data.StartsWith("PS") Then
            Dim PS_Stat As Int16 = 0
            '(PS Ok: get: result: bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340
            MainMSEngine.PageSetVariable(Main.VarPrefix & "MESSAGE", data)
            Player.Message = data
            Dim psResult As Regex = New Regex(String.Format("^PS (\d+)? ?Ok: get: result: (.*)$"))          'Regex: ^\(PS Ok: get: result: (.*)$
            Dim psMatch As System.Text.RegularExpressions.Match = psResult.Match(String.Format("{0}", data))
            If psMatch.Success Then
                Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat)
                Dim psResult1 As Regex = New Regex("^<empty>$")
                Dim psMatch2 As System.Text.RegularExpressions.Match = psResult1.Match(psMatch.Groups(2).Value)
                If psMatch2.Success And CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList Then
                    If NextLetter <> "{"c Then
                        ServerStack.Enqueue("ps get character." + incrementLetter(NextLetter) + "*")
                    Else
                        psCheck = ProcessPSData(1, PSinfo, data)
                    End If
                Else


                    'Add "," to the end of match #1.
                    'Input: "bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340,"
                    Dim input As String = psMatch.Groups(2).Value.ToString & ","
                    'Regex: ( ?([^=]+)=('?)(.+?)('?)),
                    SyncLock ChannelLock
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
                            Try
                                psCheck = ProcessPSData(PS_Stat, PSinfo, data)
                            Catch ex As Exception
                                Dim e As New ErrorLogging(ex, Me)
                            End Try
                        ElseIf CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList And NextLetter <> "{"c Then
                            Dim m As System.Text.RegularExpressions.Match = mc.Item(mc.Count - 1)
                            NextLetter = incrementLetter(m.Groups(1).Value.ToString)
                            If NextLetter <> "{"c Then
                                ServerStack.Enqueue("ps get character." + NextLetter + "*")
                            Else
                                psCheck = ProcessPSData(1, PSinfo, data)
                            End If
                        ElseIf CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList And NextLetter = "{"c Then
                            'CurrentPS_Stage = PS_BackupStage.GetList
                            Try
                                psCheck = ProcessPSData(PS_Stat, PSinfo, data)
                            Catch ex As Exception
                                Dim e As New ErrorLogging(ex, Me)
                            End Try
                        End If
                    End SyncLock
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

                    SyncLock ChannelLock
                        Dim mc As MatchCollection = Regex.Matches(String.Format(PS_Page), String.Format("\s?(.*?)=('?)(\d+|.*?)(\2),?"), RegexOptions.IgnorePatternWhitespace)
                        If CurrentPS_Stage <> PS_BackupStage.GetAlphaNumericList Then PSinfo.Clear()
                        For i As Integer = 0 To mc.Count - 1
                            Dim m As System.Text.RegularExpressions.Match = mc.Item(i)
                            If Not PSinfo.ContainsKey(m.Groups(1).Value) Then PSinfo.Add(m.Groups(1).Value, m.Groups(3).Value)
                            'Match(1) : Value(Name)
                            'Match 2: Empty if number, ' if string
                            'Match(3) : Value()
                        Next
                        Dim test As Integer = 1000 * psMatch.Groups(3).Value.ToInteger
                        If pslen > 1000 * psMatch.Groups(3).Value.ToInteger And CurrentPS_Stage = PS_BackupStage.GetList Then
                            CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList
                            Dim m As System.Text.RegularExpressions.Match = mc.Item(mc.Count - 1)
                            ServerStack.Enqueue("ps get character." + m.Groups(1).Value.Substring(0, 1) + "*")

                        ElseIf CurrentPS_Stage <> PS_BackupStage.GetAlphaNumericList Then
                            Try
                                psCheck = ProcessPSData(PS_Stat, PSinfo, data)
                            Catch ex As Exception
                                Dim e As New ErrorLogging(ex, Me)
                            End Try
                        ElseIf CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList And NextLetter <> "{"c Then
                            Dim m As System.Text.RegularExpressions.Match = mc.Item(mc.Count - 1)
                            NextLetter = incrementLetter(m.Groups(1).Value.ToString)
                            If NextLetter <> "{"c Then
                                ServerStack.Enqueue("ps get character." + NextLetter + "*")
                            Else
                                psCheck = ProcessPSData(1, PSinfo, data)
                            End If
                        ElseIf CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList And NextLetter = "{"c Then
                            'CurrentPS_Stage = PS_BackupStage.GetList
                            Try
                                psCheck = ProcessPSData(PS_Stat, PSinfo, data)
                            Catch ex As Exception
                                Dim e As New ErrorLogging(ex, Me)
                            End Try
                        End If
                    End SyncLock
                End If
                '(PS 5 Error: get: Query error: Field 'Bob' does not exist

            End If

            psResult = New Regex("^PS (\d+)? ?Ok: set: Ok$")
            '^PS (\d+)? ?Ok: set: Ok
            psMatch = psResult.Match(data)
            If psMatch.Success Then
                PSinfo.Clear()
                Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat)
                Try
                    ProcessPSData(PS_Stat, PSinfo, data)
                Catch ex As Exception
                    Dim e As New ErrorLogging(ex, Me)
                End Try
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
                        MainEngine.MSpage.Execute(503)
                    End If
                Else
                    Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat)

                    If CurrentPS_Stage = PS_BackupStage.off Then
                        MainMSEngine.PageExecute(80, 81, 82)

                    ElseIf CurrentPS_Stage = PS_BackupStage.GetList Then
                        If PS_Stat <> CharacterList.Count Then
                            Dim str As String = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*"
                            ServerStack.Enqueue(str)
                            psSendCounter = CShort(PS_Stat + 1)

                            psReceiveCounter = PS_Stat

                        ElseIf PS_Stat = CharacterList.Count Then
                            CurrentPS_Stage = PS_BackupStage.off


                        End If
                    ElseIf CurrentPS_Stage = PS_BackupStage.GetTargets And psSendCounter = psReceiveCounter + 1 Then
                        If PS_Stat <> CharacterList.Count Then
                            Dim str As String = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*"
                            ServerStack.Enqueue(str)
                            psSendCounter = CShort(PS_Stat + 1)
                            psReceiveCounter = PS_Stat
                        ElseIf PS_Stat = CharacterList.Count Then
                            CurrentPS_Stage = PS_BackupStage.off
                            PSBackupRunning = False
                            CharacterList.Clear()
                            psReceiveCounter = 0
                            psSendCounter = 1
                            '(0:501) When the bot completes backing up the characters Phoenix Speak,
                            SendClientMessage("SYSTEM:", "Completed Backing up Dream Characters set.")
                            MainEngine.MSpage.Execute(501)
                        End If

                    ElseIf CurrentPS_Stage = PS_BackupStage.RestoreAllCharacterPS And PS_Stat <= PSS_Stack.Count - 1 And psSendCounter = psReceiveCounter + 1 Then
                        If PS_Stat <> PSS_Stack.Count - 1 Then
                            LastSentPS = 0
                            Dim ss As New PSS_Struct
                            ss = PSS_Stack(PS_Stat)
                            ServerStack.Enqueue(ss.Cmd)
                            psSendCounter = CShort(PS_Stat + 1)
                            psReceiveCounter = PS_Stat

                        ElseIf PS_Stat = PSS_Stack.Count - 1 Then
                            PSRestoreRunning = False
                            SendClientMessage("SYSTEM:", "Completed Character restoration to the dream")
                            '(0:501) When the bot completes backing up the characters Phoenix Speak,
                            MainEngine.MSpage.Execute(503)
                            CurrentPS_Stage = PS_BackupStage.off
                        End If
                    ElseIf CurrentPS_Stage = PS_BackupStage.GetSingle And PS_Stat <= CharacterList.Count And psSendCounter = psReceiveCounter + 1 Then
                        If PS_Stat <> CharacterList.Count Then
                            Dim str As String = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*"
                            ServerStack.Enqueue(str)
                            psSendCounter = CShort(PS_Stat + 1)
                            psReceiveCounter = PS_Stat
                        ElseIf PS_Stat = CharacterList.Count Then
                            CurrentPS_Stage = PS_BackupStage.off
                            CharacterList.Clear()
                            psReceiveCounter = 0
                            psSendCounter = 1
                            PSBackupRunning = False
                        End If
                    End If
                End If
            End If
            If cMain.PSShowMainWindow Then
                sndDisplay(data)
            End If
            If cMain.PSShowClient Then
                If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            End If
            Exit Sub

        ElseIf data.StartsWith("(You enter the dream of") Then
            MainMSEngine.PageSetVariable("DREAMNAME", "")
            MainMSEngine.PageSetVariable("DREAMOWNER", data.Substring(24, data.Length - 2 - 24))
            MainMSEngine.PageExecute(90, 91)
            sndDisplay(data)
            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub


        Else
            sndDisplay(data)

            If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
            Exit Sub
        End If
        ' If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
        ' Exit Sub
    End Sub
    Public Sub PS_Abort()
        CurrentPS_Stage = PS_BackupStage.off
        PSS_Stack.Clear()
        CharacterList.Clear()
        PSRestoreRunning = False
        PSBackupRunning = False
        PSPruneRunning = False
        psReceiveCounter = 0
        psSendCounter = 0
        LastSentPS = 0
    End Sub
    Function incrementLetter(Input As String) As Char
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
    Private Function ProcessPSData(ByVal PS_Stat As Int16, ByVal ps_KV As Dictionary(Of String, String), data As String) As Boolean
        If CurrentPS_Stage = PS_BackupStage.off Then
            MainMSEngine.PageExecute(80, 81, 82)
            Return False
        ElseIf CurrentPS_Stage = PS_BackupStage.GetList Or CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList Then
            CharacterList.Clear()

            '(0:500) When the bot starts backing up the character Phoenix Speak,
            MainEngine.MSpage.Execute(500)
            PSBackupRunning = True
            SendClientMessage("SYSTEM:", "Backing up Dream Characters Set.")

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
            ServerStack.Enqueue(str)
            on_Tick(0)
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
                ServerStack.Enqueue(str)
                psSendCounter = CShort(PS_Stat + 1)

            ElseIf s.PS_ID = PS_Stat AndAlso PS_Stat = CharacterList.Count Then
                CurrentPS_Stage = PS_BackupStage.off
                PSBackupRunning = False
                CharacterList.Clear()
                psReceiveCounter = 0
                psSendCounter = 1
                '(0:501) When the bot completes backing up the characters Phoenix Speak,
                SendClientMessage("SYSTEM:", "Completed Backing up Dream Characters set.")
                MainEngine.MSpage.Execute(501)
            End If
            ' End If
        ElseIf CurrentPS_Stage = PS_BackupStage.GetSingle And PS_Stat <= CharacterList.Count And psSendCounter = psReceiveCounter + 1 Then

            Dim s As New PSInfo_Struct
            s = CharacterList(PS_Stat - 1)
            SendClientMessage("SYSTEM:", "Backing up information for player " + s.name)
            s.Values = ps_KV
            s.PS_ID = PS_Stat
            psReceiveCounter = PS_Stat

            If SendPStoDatabase(s) AndAlso PS_Stat <> CharacterList.Count Then
                Dim str As String = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*"
                ServerStack.Enqueue(str)
                psSendCounter = CShort(PS_Stat + 1)

            ElseIf PS_Stat = CharacterList.Count Then
                CurrentPS_Stage = PS_BackupStage.off
                CharacterList.Clear()
                psReceiveCounter = 0
                psSendCounter = 1
                PSBackupRunning = False
            End If


        ElseIf CurrentPS_Stage = PS_BackupStage.RestoreAllCharacterPS And PS_Stat <= PSS_Stack.Count And psSendCounter = psReceiveCounter + 1 Then


            If PS_Stat <> PSS_Stack.Count - 1 Then
                LastSentPS = 0
                Dim s As New PSS_Struct
                s = PSS_Stack(PS_Stat)
                ServerStack.Enqueue(s.Cmd)
                Player = NametoFurre(s.Name, True)
                Player.Message = ""
                MainMSEngine.PageSetVariable("MESSAGE", "")
                MainMSEngine.PageExecute(506, 507)

                psSendCounter = CShort(PS_Stat + 1)
                psReceiveCounter = PS_Stat

            ElseIf PS_Stat = PSS_Stack.Count - 1 Then
                PSRestoreRunning = False
                SendClientMessage("SYSTEM:", "Completed Character restoration to the dream")
                '(0:501) When the bot completes backing up the characters Phoenix Speak,
                MainEngine.MSpage.Execute(503)
                CurrentPS_Stage = PS_BackupStage.off
            End If

        End If
        Return True
    End Function




    ''' <summary>
    '''     Allows the programmer to easily insert into the DB
    ''' </summary>
    ''' <param name="tableName">The table into which we insert the data.</param>
    ''' <param name="data">A dictionary containing the column names and data for the insert.</param>
    ''' <returns>A boolean true or false to signify success or failure.</returns>
    Public Function InsertMultiRow(tableName As [String], ID As Integer, data As Dictionary(Of [String], [String])) As Boolean
        Dim values As New ArrayList
        For Each val As KeyValuePair(Of [String], [String]) In data
            values.Add([String].Format(" ( {0}, '{1}', '{2}' )", ID, val.Key, val.Value))
        Next

        Try
            Dim i As Integer = 0
            If values.Count > 0 Then
                Dim str As String = String.Join(", ", values.ToArray)
                'INSERT INTO 'table' ('column1', 'col2', 'col3') VALUES (1,2,3),  (1, 2, 3), (etc);
                Dim cmd As String = [String].Format("INSERT into '{0}' (NameID, 'Key', 'Value') Values {1};", tableName, str)
                i = SQLiteDatabase.ExecuteNonQuery(cmd)
            End If
            Return values.Count <> 0 AndAlso i <> 0
        Catch fail As Exception
            Dim er As New ErrorLogging(fail, Me)
            Return False
        End Try
        Return True
    End Function

    Private Function fIDtoFurre(ByRef ID As UInteger) As FURRE
        Dim Character As KeyValuePair(Of UInteger, FURRE)
        For Each Character In DREAM.List
            If Character.Value.ID = ID Then
                Return Character.Value
            End If
        Next
    End Function

    Public Function NametoFurre(ByRef sname As String, ByRef UbdateMSVariableName As Boolean) As FURRE
        Dim p As New FURRE
        p.Name = sname
        For Each Character As KeyValuePair(Of UInteger, FURRE) In DREAM.List
            If Character.Value.ShortName = sname.ToFurcShortName Then
                p = Character.Value
                Exit For
            End If
        Next
        If UbdateMSVariableName Then MainMSEngine.PageSetVariable(MS_Name, sname)

        Return p
    End Function



    Public Function IsBot(ByRef player As FURRE) As Boolean
        Try
            Dim str As String = BotName.ToFurcShortName
            If player.ShortName <> BotName.ToFurcShortName Then Return False
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
        If BtnSit_stand_Lie.InvokeRequired Then
            Dim d As New UpDateBtn_StandCallback(AddressOf IsBot)
            Me.Invoke(d, [player])
            Return True
        Else
            'Update inteface
            Try

                Select Case player.FrameInfo.pose
                    Case 0
                        BtnSit_stand_Lie.Text = "Stand"
                    Case 1 To 3
                        BtnSit_stand_Lie.Text = "Lay"
                    Case 4
                        BtnSit_stand_Lie.Text = "Sit"
                End Select
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)
            End Try
            Return True
        End If
    End Function

#End Region
    Private ClientReceived As Integer = 0
    Private Sub onClientDataReceived(ByVal data As String) Handles smProxy.ClientData2
        Dim temp As Object = data
        Try

            If (Monitor.TryEnter(temp)) Then
                'If data.StartsWith("desc") = True Or data.StartsWith("chdesc") = True Then
                '    data += " [<a href='http://www.ts-projects.org/smIdx.html'>SilverMonkey</a> with <a href='http://Furcadia.codeplex.com'>Furcadia Framework</a> for Third Party Programs]"
                'End If

                If data.StartsWith("quit") And cBot.StandAlone Then
                    Exit Sub

                    'Capture The Bots Name
                ElseIf data.StartsWith("connect") Then


                    Dim test As String = data.Replace("connect ", "").TrimStart(" "c)
                    BotName = test.Substring(0, test.IndexOf(" "))

                    BotName = BotName.Replace("|", " ")
                    MainText(BotName)
                    BotName = BotName.Replace("[^a-zA-Z0-9\0x0020_.| ]+", "").ToLower()
                    MainMSEngine.PageSetVariable("BOTNAME", BotName)
                ElseIf data = "vascodagama" And loggingIn = 2 Then
                    sndServer("`uid")
                    Select Case cBot.GoMapIDX
                        Case 1
                            sndServer("`gomap #")
                        Case 2
                            sndServer("`gomap *")
                        Case 3

                        Case 4
                            sndServer("`fdl " + cBot.DreamURL)
                    End Select
                    loggingIn = 3
                End If
                SndToServer(data)
            End If
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)

        Finally
            Monitor.Exit(temp)
        End Try






    End Sub

    Public Sub MainText(ByRef str As String)
        If Me.InvokeRequired Then

            Dim d As New UpDateBtn_GoCallback(AddressOf MainText)
            Me.Invoke(d, str)
        Else
            Me.Text = "Silver Monkey: " & str.ToString '& " " & Application.ProductVersion
            Me.NotifyIcon1.Text = "Silver Monkey: " & str.ToString
        End If


    End Sub

    Private Sub ClientExit() Handles smProxy.ClientExited
        ' If loggingIn = 0 Then Exit Sub

        'TS_Status_Client.Image = My.Resources.images2
        MainMSEngine.PageExecute(3)

        'loggingIn = 0
        If cBot.StandAlone = False Then

            ClientClose = True
            If cMain.CloseProc Then
                ProcExit = False
            Else
                ProcExit = True
            End If
            DisconnectBot()
        ElseIf bConnected() Then
            smProxy.CloseClient()
            TS_Status_Client.Image = My.Resources.images2
        End If
        If cMain.DisconnectPopupToggle Then SetBalloonText("Furcadia Closed or disconnected")
    End Sub
    Private Sub ServerClose() Handles smProxy.ServerDisConnected
        If smProxy.IsClientConnected Then
            ProcExit = False

        End If
        DisconnectBot()
    End Sub

    Public serverData As String
    Private Sub onServerDataReceived(ByVal data As String) Handles smProxy.ServerData2
        Dim temp As Object = data
        Try

            If (Monitor.TryEnter(temp)) Then
                Player.Clear()
                Channel = ""
                MainMSEngine.PageSetVariable(MS_Name, "")
                MainMSEngine.PageSetVariable("MESSAGE", "")
                serverData = data
                Dim test As Boolean = MessagePump(serverData)
                ParseServerData(serverData, test)
            End If
        Finally
            Monitor.Exit(temp)
        End Try
    End Sub

    Private Function MessagePump(ByRef Server_Instruction As String) As Boolean
        Dim objPlugin As SilverMonkey.Interfaces.msPlugin
        Dim intIndex As Integer
        Dim Handled As Boolean = False
        If Not Plugins Is Nothing Then
            For intIndex = 0 To Plugins.Length - 1
                objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(intIndex)), SilverMonkey.Interfaces.msPlugin)
                If cMain.PluginList.Item(objPlugin.Name.Replace(" ", "")) Then
                    objPlugin.Initialize(objHost)
                    objPlugin.Page = MainEngine.MSpage
                    If objPlugin.MessagePump(Server_Instruction) Then Handled = True
                End If
            Next
        End If
        Return Handled
    End Function

    Protected Overrides Sub Finalize()
        Try
            If Not IsNothing(smProxy) Then
                smProxy.Kill()
                ClearQues()
            End If

        Catch
        End Try
        MyBase.Finalize()
    End Sub

    Public Sub OnConnected() Handles smProxy.Connected
If Not IsNothing(ReconnectTimeOutTimer) Then ReconnectTimeOutTimer.Dispose()

    End Sub



    Public Function FormatText(ByVal data As String, ByVal newColor As fColorEnum) As String
        data = System.Web.HttpUtility.HtmlDecode(data)
        data = data.Replace("|", " ")

        Dim myColor As System.Drawing.Color = fColor(newColor)
        Dim ColorString As String = "{\colortbl ;"
        ColorString += "\red" & myColor.R & "\green" & myColor.G & "\blue" & myColor.B & ";}"
        Dim FontSize As Single = cMain.ApFont.Size
        Dim FontFace As String = cMain.ApFont.Name
        FontSize *= 2
        Return "{\rtf1\ansi\ansicpg1252\deff0\deflang1033" & ColorString & "{\fonttbl{\f0\fcharset0 " & FontFace & ";}}\viewkind4\uc1\fs" & FontSize.ToString & "\cf1 " & data & " \par}"
    End Function
    Public Function FormatURL(ByVal data As String) As String
        Dim FontSize As Single = cMain.ApFont.Size
        Dim FontFace As String = cMain.ApFont.Name
        FontSize *= 2
        Return "{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fcharset0 " & FontFace & ";}}\viewkind4\uc1\fs" & FontSize.ToString & " " & data & "}"
    End Function

    Private Sub FormClose()
        _FormClose = True
        My.Settings.MainFormLocation = Me.Location
        If Not IsNothing(cBot) Then My.Settings.LastBotFile = cBot.BotFile
        'Timers.DestroyTimers()
        'Save the user settings so next time the
        'window will be the same size and location
        SettingsIni.Save(SetFile)
        My.Settings.Save()
        NotifyIcon1.Visible = False
        If Not IsNothing(Me.TroatTiredDelay) Then Me.TroatTiredDelay.Dispose()
        If Not IsNothing(Me.TroatTiredProc) Then Me.TroatTiredProc.Dispose()
        If Not IsNothing(Me.LogTimer) Then Me.LogTimer.Dispose()
        If Not IsNothing(Me.MSalarm) Then Me.MSalarm.Dispose()
        If Not IsNothing(Me.DreamUpdateTimer) Then Me.DreamUpdateTimer.Dispose()
        If Not IsNothing(Me.ReconnectTimeOutTimer) Then Me.ReconnectTimeOutTimer.Dispose()
        If Not IsNothing(Me.PingTimer) Then Me.PingTimer.Dispose()
        If Not IsNothing(Me.PounceTimer) Then Me.PounceTimer.Dispose()
        Me.Dispose()
    End Sub



    Private Sub Main_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try

            Select Case cMain.SysTray
                Case CheckState.Checked
                    Me.Visible = False
                    e.Cancel = True
                Case CheckState.Indeterminate
                    If MessageBox.Show("Minimize to SysTray?", "", MessageBoxButtons.YesNo, Nothing, _
                     MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        cMain.SysTray = CheckState.Checked
                        cMain.SaveMainSettings()
                        Me.Visible = False
                        e.Cancel = True
                    Else
                        e.Cancel = False
                        FormClose()
                    End If
                Case CheckState.Unchecked
                    FormClose()

            End Select
            'TimeUpdater.Abort()

        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub

    Private Sub Tick(ByVal state As Object)
        If Me.IsDisposed Then Exit Sub
        Timeupdate()
    End Sub
    Private Sub Timeupdate()
        Try
            If _FormClose Then Exit Sub
            If MenuStrip1.InvokeRequired Then

                Dim d As New DelTimeupdate(AddressOf Timeupdate)
                If Me.IsDisposed = False Then Me.Invoke(d)

            Else
                Dim FTime As Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Date.Now, TimeZoneInfo.Local.Id, "Central Standard Time")
                SyncLock Lock
                    FurcTime = FTime
                End SyncLock

                Try

                    FurcTimeLbl.Text = "Furcadia Standard Time: " & FTime.ToLongTimeString
                    MainMSEngine.PageExecute(299)
                Catch ex As Exception
                    Debug.Print(ex.Message)
                End Try
            End If
        Catch
        End Try


    End Sub

    Private Sub Main_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If (e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control) Then

            LaunchEditor()


            'e.Handled = True
            'e.SuppressKeyPress = True
        ElseIf (e.KeyCode = Keys.F1) Then
            frmHelp.Show()
        ElseIf (e.KeyCode = Keys.N AndAlso e.Modifiers = Keys.Control) Then
            With BotSetup
                .bFile = New cBot
                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    cBot = .bFile
                End If
            End With

        End If

    End Sub

    Private Sub Main_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(NotifyIcon1) Then
            NotifyIcon1 = New NotifyIcon
            NotifyIcon1.ContextMenuStrip = ContextTryIcon
            NotifyIcon1.Icon = My.Resources.metal
            NotifyIcon1.BalloonTipTitle = My.Application.Info.ProductName
            NotifyIcon1.Text = My.Application.Info.ProductName + ": " + My.Application.Info.Version.ToString
            AddHandler NotifyIcon1.MouseDoubleClick, AddressOf NotifyIcon1_DoubleClick
        End If
        If Not NotifyIcon1.Visible Then NotifyIcon1.Visible = True
        'catch the Console messages
        _FormClose = False
        FurcPath = New Paths(cMain.FurcPath)
        writer = New TextBoxWriter(log_)
        Console.SetOut(writer)
        MainMSEngine = New MainEngine
        Me.MSalarm = New Threading.Timer(AddressOf Tick, True, 1000, 1000)
        FurreList.Clear()
        Plugins = PluginServices.FindPlugins(Path.GetDirectoryName(Application.ExecutablePath) + "\Plugins\", "SilverMonkey.Interfaces.msPlugin")

        ' Try to get Furcadia's path from the registry
        cMain = New cMain
        MS_KeysIni.Load(Path.GetDirectoryName(Application.ExecutablePath) + "\Keys-MS.ini")
        InitializeTextControls()

        'Me.Size = My.Settings.MainFormSize
        Me.Location = My.Settings.MainFormLocation
        Me.Text = "Silver Monkey: " & Application.ProductVersion
        Me.Visible = True


        LoadRecentList()
        For Each item As String In MRUlist
            Dim fileRecent As New ToolStripMenuItem(item, Nothing, AddressOf RecentFile_click)
            'create new menu for each item in list
            'add the menu to "recent" menu
            RecentToolStripMenuItem.DropDownItems.Add(fileRecent)
        Next
        EditBotToolStripMenuItem.Enabled = False
        callbk = Me
        If (My.Application.CommandLineArgs.Count > 0) Then
            Dim File As String = My.Application.CommandLineArgs(0)
            Dim directoryName As String
            directoryName = Path.GetDirectoryName(File)
            If String.IsNullOrEmpty(directoryName) Then File = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Silver Monkey/" + File
            cBot = New cBot(File)
            EditBotToolStripMenuItem.Enabled = True
            Console.WriteLine("Loaded: """ + File + """")
        ElseIf cMain.LoadLastBotFile And Not String.IsNullOrEmpty(My.Settings.LastBotFile) And My.Application.CommandLineArgs.Count = 0 Then
            cBot = New cBot(My.Settings.LastBotFile)
            EditBotToolStripMenuItem.Enabled = True
            Console.WriteLine("Loaded: """ + My.Settings.LastBotFile + """")
        End If
        Dim ts As TimeSpan = TimeSpan.FromSeconds(30)
        Me.PounceTimer = New Threading.Timer(AddressOf smPounceSend, Nothing, TimeSpan.Zero, ts)
        Me.PounceTimer.InitializeLifetimeService()
        Me.TroatTiredProc = New Threading.Timer(AddressOf TroatTiredProcTick, Nothing, 3000, 100)

        If Not IsNothing(cBot) Then
            If cBot.AutoConnect Then
                ConnectBot()
            End If
        End If

        'Paths.InstallPath = SettingsIni.GetKeyValue("Main", "FurcPath")
    End Sub



    Public Sub ConnectionControlEnable()

    End Sub
    Public Sub ConnectionControlDisEnable()
        Me.EditBotToolStripMenuItem.Enabled = False
    End Sub

    Public Sub InitializeTextControls()
        log_.Font = cMain.ApFont
        toServer.Font = cMain.ApFont
        DreamList.Font = cMain.ApFont
        DreamCountTxtBx.Font = cMain.ApFont
    End Sub

#Region "Action Controls"

    Private Sub ActionTmr_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActionTmr.Tick
        If Not bConnected() Then Exit Sub
        sndServer(ActionCMD)
    End Sub
    Private Sub _ne_Click(sender As System.Object, e As System.EventArgs) Handles _ne.Click
        sndServer("`m 9")
    End Sub

    Private Sub _nw_Click(sender As System.Object, e As System.EventArgs) Handles _nw.Click
        sndServer("`m 7")
    End Sub

    Private Sub BtnSit_stand_Lie_Click(sender As System.Object, e As System.EventArgs) Handles BtnSit_stand_Lie.Click
        If Not bConnected() Then Exit Sub
        If BtnSit_stand_Lie.Text = "Stand" Then
            BtnSit_stand_Lie.Text = "Lay"
        ElseIf BtnSit_stand_Lie.Text = "Lay" Then
            BtnSit_stand_Lie.Text = "Sit"
        ElseIf BtnSit_stand_Lie.Text = "Sit" Then
            BtnSit_stand_Lie.Text = "Stand"
        End If
        sndServer("`lie")
    End Sub

    Private Sub BTN_TurnR_Click(sender As System.Object, e As System.EventArgs) Handles BTN_TurnR.Click
        sndServer("`>")
    End Sub

    Private Sub BTN_TurnL_Click(sender As System.Object, e As System.EventArgs) Handles BTN_TurnL.Click
        sndServer("`<")
    End Sub

    Private Sub use__Click(sender As Object, e As System.EventArgs) Handles use_.Click
        sndServer("`use")
    End Sub

    Private Sub get__Click(sender As Object, e As System.EventArgs) Handles get_.Click
        sndServer("`get")
    End Sub

    Private Sub sw__Click(sender As Object, e As System.EventArgs) Handles sw_.Click
        sndServer("`m 1")
    End Sub

    Private Sub sw__MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles sw_.MouseDown
        Me.ActionTmr.Enabled = bConnected()
        ActionCMD = "`m 1"
    End Sub

    Private Sub sw__MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles sw_.MouseUp
        Me.ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    Private Sub _ne_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _ne.MouseDown
        Me.ActionTmr.Enabled = bConnected()
        ActionCMD = "`m 9"
    End Sub

    Private Sub _ne_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _ne.MouseUp
        Me.ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    Private Sub _nw_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _nw.MouseDown
        Me.ActionTmr.Enabled = bConnected()
        ActionCMD = "`m 7"
    End Sub

    Private Sub _nw_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles _nw.MouseUp
        Me.ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub

    Private Sub se__Click(sender As Object, e As System.EventArgs) Handles se_.Click
        sndServer("`m 3")
    End Sub

    Private Sub se__MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles se_.MouseDown
        Me.ActionTmr.Enabled = bConnected()
        ActionCMD = "`m 3"
    End Sub

    Private Sub se__MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles se_.MouseUp
        Me.ActionTmr.Enabled = False
        ActionCMD = ""
    End Sub
#End Region

    Private Sub ConfigToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigToolStripMenuItem.Click
        Config.Show()
        Config.Activate()
    End Sub

    Private Sub DebugToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DebugToolStripMenuItem.Click
        Variables.Show()
        Variables.Activate()
    End Sub

    Private Sub BTN_Go_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_Go.Click, ConnectTrayIconMenuItem.Click, DisconnectTrayIconMenuItem.Click
        If IsNothing(cBot) Then Exit Sub
        If String.IsNullOrEmpty(cBot.IniFile) Then Exit Sub
        If cBot.IniFile = "-pick" Then Exit Sub

        Dim p As String = Path.GetDirectoryName(cBot.IniFile)
        If String.IsNullOrEmpty(p) And Not File.Exists(FurcPath.GetFurcadiaCharactersPath() + Path.DirectorySeparatorChar + Path.GetFileName(cBot.IniFile)) Then
            MessageBox.Show(cBot.IniFile + " Not found, Aborting connection!", "Important Message")
            Exit Sub
        End If

        If BTN_Go.Text = "Go!" Then

            If cBot.log Then
                LogStream = New LogStream(setLogName(cBot), cBot.LogPath)
            End If
            If Not MainMSEngine.ScriptStart() Then Exit Sub
            My.Settings.LastBotFile = CheckMyDocFile(cBot.BotFile)
            My.Settings.Save()
            ReLogCounter = 0
            ClientClose = False
            Dim Ts As TimeSpan = TimeSpan.FromSeconds(cMain.ConnectTimeOut)
            Me.ReconnectTimeOutTimer = New Threading.Timer(AddressOf ReconnectTimeOutTick, _
             Nothing, Ts, Ts)
            Dim Tss As TimeSpan = TimeSpan.FromSeconds(cMain.Ping)
            If cMain.Ping > 0 Then Me.PingTimer = New Threading.Timer(AddressOf PingTimerTick, _
             Nothing, Tss, Tss)
            If Not IsNothing(MS_Export) Then MS_Export.Dispose()
            Try
            	ConnectBot()
            Catch Ex As NetProxyException
            	Me.ReconnectTimeOutTimer.Dispose()
            	If Not IsNothing(Me.PingTimer) Then Me.PingTimer.Dispose()
               	If Not IsNothing(FurcMutex) Then '
                	FurcMutex.Close()
                	FurcMutex.Dispose()
               	End If
               	DisconnectBot()
               	sndDisplay("Connection Aborting: " + eX.Message)
            End Try

        Else
            ProcExit = False
            ClientClose = True
            If Not IsNothing(FurcMutex) Then '
                FurcMutex.Close()
                FurcMutex.Dispose()
            End If
            DisconnectBot()
            If Not IsNothing(ReconnectTimer) Then Me.ReconnectTimer.Dispose()
            If Not IsNothing(ReconnectTimeOutTimer) Then ReconnectTimeOutTimer.Dispose()
        End If
    End Sub

    Public Sub KillProc(ByRef ID As Integer)
        For Each p As Process In Process.GetProcesses
            If p.Id = ID And p.Id <> 0 Then
                p.Kill()
                Exit Sub
            End If
        Next
    End Sub

    Public Sub ConnectBot()
        If Me.BTN_Go.InvokeRequired Then
            Dim d As New UpDateBtn_GoCallback2(AddressOf ConnectBot)
            Me.Invoke(d)
        Else
            FurcMutex = New System.Threading.Mutex(False, FurcProcess)
            If FurcMutex.WaitOne(0, False) = False Then
                FurcMutex.Close()
                FurcMutex.Dispose()
                Console.WriteLine("Another copy  of Silver Monkey is Currently Connecting")
            Else
                Dim port As Integer = cBot.lPort

                If Not PortOpen(cBot.lPort) Then
                    For i As Integer = cBot.lPort To cBot.lPort + 100
                        If PortOpen(i) Then
                            port = i
                            Exit For
                        End If
                    Next
                    'MsgBox("Local Port: " & cBot.lPort.ToString & " is in use, Aborting connection")
                    'Exit Sub
                End If
                Try
                    g_mass = 0
                    If Not IsNothing(smProxy) Then smProxy.Dispose()
                    smProxy = New NetProxy(cMain.Host, cMain.sPort, port)
                    With smProxy
                        .ProcessCMD = cBot.IniFile
                        .ProcessPath = cMain.FurcPath
                        .StandAloneMode = cBot.StandAlone
                        .Connect()
                        ProcID = .ProcID
                        loggingIn = 1
                        NewLogFile = True
                    End With
                Catch ex As Exception
                	'Debug.WriteLine(ex.Message)
                	Throw ex
                End Try
                loggingIn = 1
                TS_Status_Server.Image = My.Resources.images5
                TS_Status_Client.Image = My.Resources.images5
                BTN_Go.Text = "Connecting..."
                sndDisplay("Connecting...")
                'TS_Status_Server.Image = My.Resources.images2
                ConnectTrayIconMenuItem.Enabled = False
                DisconnectTrayIconMenuItem.Enabled = True
                NotifyIcon1.ShowBalloonTip(3000, "SilverMonkey", "Connecting to Furcadia.", ToolTipIcon.Info)
            End If
        End If
    End Sub
    Public Sub DisconnectBot()
        If Me.BTN_Go.InvokeRequired Then
            Dim d As New UpDateBtn_GoCallback2(AddressOf DisconnectBot)
            Me.Invoke(d)
        Else
            If cMain.AutoReconnect And Not ClientClose Then
                Me.ReconnectTimer = New Threading.Timer(AddressOf ReconnectTick, _
                  Nothing, 45000, 45000)
                SetBalloonText("Connection Lost. Reconnecting in 45 Seconds.")
                Console.WriteLine("Connection Lost. Reconnecting in 45 Seconds.")
            End If

            BTN_Go.Text = "Go!"
            TS_Status_Server.Image = My.Resources.images2
            TS_Status_Client.Image = My.Resources.images2
            ConnectTrayIconMenuItem.Enabled = False
            DisconnectTrayIconMenuItem.Enabled = True
            NotifyIcon1.ShowBalloonTip(3000, "SilverMonkey", "Now disconnected from Furcadia.", ToolTipIcon.Info)
            Try
                If cMain.CloseProc And ProcExit = False Then

                    KillProc(ProcID)
                    SndToServer("quit")

                    'smProxy.Kill()
                End If
                If Not IsNothing(smProxy) Then
                    If smProxy.IsServerConnected Or smProxy.IsClientConnected Then
                        PingTimer.Dispose()
                        smProxy.Kill()
                        ClearQues()
                    End If

                End If
                sndDisplay("Disconnected.", fColorEnum.DefaultColor)
            Catch ex As Exception
                Dim logError As New ErrorLogging(ex, Me)
            End Try
            InDream = False
            DreamList.Items.Clear()
            DreamCountTxtBx.Text = ""

            ' (0:2) When the bot logs off
            MainMSEngine.PageExecute(2)
            loggingIn = 0
            Monkeyspeak.Libraries.Timers.DestroyTimers()
            MainMSEngine.MS_Engine_Running = False
        End If
    End Sub
    Public Sub BotConnecting()
        If Me.BTN_Go.InvokeRequired Then
            Dim d As New UpDateBtn_GoCallback2(AddressOf BotConnecting)
            Me.Invoke(d)
        Else
            BTN_Go.Text = "Connected."
            ClientClose = False
            '  loggingIn = 2
            ConnectTrayIconMenuItem.Enabled = False
            DisconnectTrayIconMenuItem.Enabled = True
            SetBalloonText("Connected to Furcadia.")
            TS_Status_Server.Image = My.Resources.images3
            ''(0:1) When the bot logs into furcadia,
            'MainMSEngine.PageExecute(1)
            If Not IsNothing(FurcMutex) Then '
                FurcMutex.Close()
                FurcMutex.Dispose()
            End If
        End If
    End Sub
    
    
    Private Sub FurcSettingsRestored() Handles smProxy.FurcSettingsRestored
    	 		FurcMutex.Close()
                FurcMutex.Dispose()
    End Sub
    
    Private Sub ReconnectTick(ByVal state As Object)

#If DEBUG Then
        Console.WriteLine("ReconnectTick()")
#End If
        Dim Ts As TimeSpan = TimeSpan.FromSeconds(45)
        Me.ReconnectTimeOutTimer = New Threading.Timer(AddressOf ReconnectTimeOutTick, _
         Nothing, Ts, Ts)
        If cMain.CloseProc And ProcExit = False Then
            KillProc(ProcID)
        End If
        If Not IsNothing(smProxy) Then
            smProxy.Kill()
            ClearQues()
        End If
            Try
            	ConnectBot()
            Catch Ex As NetProxyException
            	Me.ReconnectTimeOutTimer.Dispose()
            	If Not IsNothing(Me.PingTimer) Then Me.PingTimer.Dispose()
               	If Not IsNothing(FurcMutex) Then '
                	FurcMutex.Close()
                	FurcMutex.Dispose()
               	End If
               	DisconnectBot()
               	sndDisplay("Connection Aborting: " + eX.Message)
            Finally
            	Me.ReconnectTimer.Dispose()
            End Try
        
    End Sub
    Private Sub ReconnectTimeOutTick(ByVal Obj As Object)

        If InvokeRequired Then
            Dim dataArray() As Object = {Obj}
            Me.Invoke(New UpDateBtn_GoCallback3(AddressOf ReconnectTimeOutTick), dataArray)

#If DEBUG Then
            Console.WriteLine("ReconnectTimeOutTick()")
            Console.WriteLine("ReLogCounter: " + ReLogCounter.ToString)
#End If


            'DisconnectBot()
            If cMain.CloseProc And ProcExit = False Then
                KillProc(ProcID)

            End If
            If Not IsNothing(smProxy) Then
                smProxy.Kill()
                ClearQues()
            End If

		try
            ConnectBot()
            sndDisplay("Reconnect attempt: " + ReLogCounter.ToString)
            If ReLogCounter = cMain.ReconnectMax Then
                Me.ReconnectTimeOutTimer.Dispose()
                sndDisplay("Reconnect attempts exceeded.")
                BTN_Go.Text = "Go!"
                TS_Status_Server.Image = My.Resources.images2
                TS_Status_Client.Image = My.Resources.images2
                ConnectTrayIconMenuItem.Enabled = False
                DisconnectTrayIconMenuItem.Enabled = True
                If Not IsNothing(FurcMutex) Then
                    FurcMutex.Close()
                    FurcMutex.Dispose()
                End If

            End If
            ReLogCounter += 1
		Catch Ex As NetProxyException
            	Me.ReconnectTimeOutTimer.Dispose()
            	If Not IsNothing(Me.PingTimer) Then Me.PingTimer.Dispose()
               	If Not IsNothing(FurcMutex) Then '
                	FurcMutex.Close()
                	FurcMutex.Dispose()
               	End If
               	DisconnectBot()
               	sndDisplay("Connection Aborting: " + eX.Message)

			End try
        End If
    End Sub

    Public Sub ResetInterface()

    End Sub
    Public Sub SetBalloonText(ByRef txt As String)
        'If Me.NotifyIcon1.Then Then
        '    Dim d As New UpDateBtn_GoCallback(AddressOf SetBalloonText)
        '    Me.Invoke(d, txt, {[Text]})
        'Else
        NotifyIcon1.BalloonTipText = txt
        NotifyIcon1.ShowBalloonTip(3000)
        'End If
    End Sub

    Private Sub toServer_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles toServer.KeyDown
        'Command History
        If (e.KeyCode = Keys.I AndAlso e.Modifiers = Keys.Control) Then

            If CMD_Idx2 < 0 AndAlso CMD_Lck = True Then
                CMD_Idx2 = CMD_Max - 1
            ElseIf CMD_Idx2 < 0 AndAlso CMD_Lck = False Then
                CMD_Idx2 = CMD_Idx
            End If
            toServer.Text = ""
            toServer.Rtf = CMDList(CMD_Idx2)
            toServer.SelectionStart = toServer.Text.Length


            CMD_Idx2 -= 1
            e.SuppressKeyPress = True
            e.Handled = True

            'Ctrl Cut
        ElseIf (e.KeyCode = Keys.X AndAlso e.Modifiers = Keys.Control) Then
            toServer.Cut()
            e.Handled = True
            'Ctrl Copy
        ElseIf (e.KeyCode = Keys.C AndAlso e.Modifiers = Keys.Control) Then
            toServer.Copy()
            e.Handled = True
            'Ctrl Paste
        ElseIf (e.KeyCode = Keys.V AndAlso e.Modifiers = Keys.Control) Then
            toServer.Paste()
            e.Handled = True
            'Ctrl Undo
        ElseIf (e.KeyCode = Keys.Z AndAlso e.Modifiers = Keys.Control) Then
            toServer.Undo()
            e.Handled = True
            'trl Redo
        ElseIf (e.KeyCode = Keys.Enter) Then
            'toServer.Text = toServer.Text.Replace(vbLf, "")
            If bConnected() = True Then
                SendTxtToServer()
            End If
            e.SuppressKeyPress = True
            e.Handled = True
        End If

    End Sub

    Private Sub sendToServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sendToServer.Click
        If Not bConnected() Then Exit Sub
        SendTxtToServer()
    End Sub

    Private Sub SendTxtToServer()

        'Filter Blank lines from triggering
        If toServer.Text = "" Then
            'toServer.Text = ""
            Exit Sub
        End If

        Dim Txt As String = toServer.Rtf
        'toServer.Rtf = Txt

        If CMD_Idx = CMD_Max Then
            CMD_Idx = 0
            CMD_Lck = True
        End If
        CMDList(CMD_Idx) = Txt
        CMD_Idx2 = CMD_Idx
        CMD_Idx += 1
        Txt = Txt.Replace("\b0 ", "</b>")
        Txt = Txt.Replace("\b ", "<b>")
        Txt = Txt.Replace("\i0 ", "</i>")
        Txt = Txt.Replace("\i ", "<i>")
        Txt = Txt.Replace("\ul0 ", "</ul>")
        Txt = Txt.Replace("\ul ", "<ul>")

        toServer.Rtf = Txt
        toServer.Text = TagCloser(toServer.Text, "b")
        toServer.Text = TagCloser(toServer.Text, "i")
        toServer.Text = TagCloser(toServer.Text, "ul")

        TextToServer(toServer.Text)
        toServer.Text = ""
    End Sub

    Public Function TagCloser(ByRef Str As String, ByRef Tag As String) As String
        'Tag Counters
        Dim OpenCount, CloseCount As Integer

        Dim CloseCounter As Integer
        OpenCount = CountOccurrences(Str, "<" + Tag + ">")
        CloseCount = CountOccurrences(Str, "</" + Tag + ">")
        If OpenCount > CloseCount Then
            CloseCounter = OpenCount - CloseCount
            Dim CloseTags As String = ""
            For I As Integer = 0 To CloseCounter - 1
                CloseTags = CloseTags & "</" + Tag + ">"
            Next I
            Str = Str & CloseTags
        End If
        Return Str
    End Function

    Public Function CountOccurrences(ByRef StToSerach As String, ByRef StToLookFor As String) As Int32
        Dim iPos As Integer = -1
        Dim iFound As Integer = 0
        Do
            iPos = StToSerach.IndexOf(StToLookFor, iPos + 1)
            If iPos <> -1 Then
                iFound += 1
            End If
        Loop Until iPos = -1
        Return iFound
    End Function

    Private Sub BTN_Underline_Click(sender As System.Object, e As System.EventArgs) Handles BTN_Underline.Click
        FormatRichTectBox(Me.toServer, System.Drawing.FontStyle.Underline)
    End Sub

    Private Sub Btn_Bold_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Bold.Click
        FormatRichTectBox(Me.toServer, System.Drawing.FontStyle.Bold)
    End Sub

    Private Sub BTN_Italic_Click(sender As System.Object, e As System.EventArgs) Handles BTN_Italic.Click
        FormatRichTectBox(Me.toServer, System.Drawing.FontStyle.Italic)
    End Sub

    Public Sub FormatRichTectBox(ByRef TB As SilverMonkey.Controls.RichTextBoxEx, _
     ByRef style As System.Drawing.FontStyle)
        With TB
            If .SelectionFont IsNot Nothing Then
                Dim currentFont As System.Drawing.Font = .SelectionFont
                Dim newFontStyle As System.Drawing.FontStyle

                If .SelectionFont.Bold = True Then
                    newFontStyle = CType(currentFont.Style - style, System.Drawing.FontStyle)
                ElseIf .SelectionFont.Italic = True Then
                    newFontStyle = CType(currentFont.Style - System.Drawing.FontStyle.Italic, System.Drawing.FontStyle)
                ElseIf .SelectionFont.Underline = True Then
                    newFontStyle = CType(currentFont.Style - System.Drawing.FontStyle.Underline, System.Drawing.FontStyle)
                Else
                    newFontStyle = CType(currentFont.Style + style, System.Drawing.FontStyle)
                End If
                .SelectionFont = New System.Drawing.Font(currentFont.FontFamily, currentFont.Size, newFontStyle)
            End If
        End With
    End Sub

    Private Sub MSEditorToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MSEditorToolStripMenuItem.Click, EditorTrayIconMenuItem.Click
        LaunchEditor()
    End Sub

    Private Sub LaunchEditor()
        If IsNothing(cBot) OrElse String.IsNullOrEmpty(cBot.MS_File) Then
            Dim result As Integer = MessageBox.Show("No Botfile Loaded", "caption", MessageBoxButtons.OK)
            If result = DialogResult.OK Then
                Exit Sub
            End If

        End If
        Dim processStrt As New ProcessStartInfo
        processStrt.FileName = My.Application.Info.DirectoryPath + Path.DirectorySeparatorChar + "MonkeySpeakEditor.EXE"
        Dim f As String = cBot.MS_File
        If Not String.IsNullOrEmpty(f) Then
            Dim dir As String = Path.GetDirectoryName(cBot.MS_File)
            If String.IsNullOrEmpty(dir) Then
                f = mPath() + Path.DirectorySeparatorChar + cBot.MS_File
            End If
        End If


        If Not String.IsNullOrEmpty(BotName) And Not String.IsNullOrEmpty(cBot.MS_File) Then
            processStrt.Arguments = "-B=""" + BotName + """ """ + f + """"
        ElseIf String.IsNullOrEmpty(BotName) And Not String.IsNullOrEmpty(cBot.MS_File) Then
            processStrt.Arguments = """" + f + """"
        End If
        Process.Start(processStrt)
    End Sub

    Private Sub NotifyIcon1_Disposed(sender As Object, e As System.EventArgs)
        Me.Visible = False
    End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As System.EventArgs)
        If Not IsNothing(NotifyIcon1) Then

        End If
        Me.Show()
        Me.Activate()
    End Sub


    Private Sub CloseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CloseToolStripMenuItem.Click, ExitTrayIconMenuItem.Click
        FormClose()
    End Sub


    Private Sub ContextTryIcon_Opened(sender As Object, e As System.EventArgs) Handles ContextTryIcon.Opened
        Select Case loggingIn
            Case 0
                DisconnectTrayIconMenuItem.Enabled = False
                ConnectTrayIconMenuItem.Enabled = True
            Case 1
                DisconnectTrayIconMenuItem.Enabled = True
                ConnectTrayIconMenuItem.Enabled = False
            Case 2 Or 3
                DisconnectTrayIconMenuItem.Enabled = True
                ConnectTrayIconMenuItem.Enabled = False

        End Select
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        toServer.Paste()
    End Sub

    Private Sub MenuCut_Click(sender As System.Object, e As System.EventArgs) Handles MenuCut.Click
        toServer.Cut()
    End Sub

    Private Sub MenuCopy_Click(sender As System.Object, e As System.EventArgs) Handles MenuCopy.Click
        toServer.Copy()
    End Sub

    Private Sub MenuCopy2_Click(sender As System.Object, e As System.EventArgs) Handles MenuCopy2.Click
        log_.Copy()
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem1.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub ContentsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ContentsToolStripMenuItem.Click
        If File.Exists(Application.StartupPath & "/Silver Monkey.chm") Then
            Process.Start(Application.StartupPath & "/Silver Monkey.chm")
        Else

        End If

    End Sub

    Public Function GetWordUnderMouse(ByRef Rtf As RichTextBoxEx, ByVal X As Integer, ByVal Y As Integer) As String
        If Rtf.InvokeRequired Then
            Dim d As New WordUnderMouse(AddressOf GetWordUnderMouse)
            d.Invoke(Rtf, X, Y)
        Else
            Try
                Dim POINT As System.Drawing.Point = New System.Drawing.Point(X, Y)
                Dim Pos As Integer, i As Integer, lStart As Integer, lEnd As Integer
                Dim lLen As Integer, sTxt As String, sChr As String

                GetWordUnderMouse = vbNullString
                '
                With Rtf
                    lLen = .Text.Length
                    sTxt = .Text
                    Pos = .GetCharIndexFromPosition(POINT)
                End With
                If Pos > 0 Then
                    For i = Pos To 1 Step -1
                        sChr = sTxt.Substring(i, 1)
                        If sChr = " " Or sChr = Chr(10) Or i = 1 Then
                            'if the starting character is vbcrlf then
                            'we want to chop that off
                            If sChr = Chr(10) Then
                                lStart = (i + 1)
                            Else
                                lStart = i
                            End If
                            Exit For
                        End If
                    Next i
                    For i = Pos To lLen
                        If sTxt.Substring(i, 1) = " " Or sTxt.Substring(i, 1) = Chr(10) Or i = lLen Then
                            lEnd = i + 1
                            Exit For
                        End If
                    Next i
                    If lEnd >= lStart Then
                        Dim test As String = sTxt.Substring(lStart, lEnd - lStart).Trim
                        Return sTxt.Substring(lStart, lEnd - lStart).Trim
                    End If
                End If

            Catch ex As Exception
                Return ""
            End Try
        End If
        Return ""
    End Function

    Private Sub log__MouseHover(sender As Object, e As System.EventArgs) Handles log_.MouseHover, log_.MouseLeave, log_.MouseEnter, log_.CursorChanged
        If Cursor.Current = Cursors.Hand Then
            SyncLock Lock
                ToolTip1.Show(curWord, Me.log_)
            End SyncLock
        Else
            ToolTip1.Hide(Me.log_)
        End If
    End Sub

    Private Sub log__MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles log_.MouseMove
        If Cursor.Current = Cursors.Hand Or Cursor.Current = Cursors.Default Then
            SyncLock Lock

                curWord = GetWordUnderMouse(Me.log_, e.X, e.Y)
            End SyncLock
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        log_.HideSelection = Not CheckBox1.Checked
    End Sub

    Private Sub TSTutorialsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TSTutorialsToolStripMenuItem.Click
        'Process.Start("http://www.ts-projects.org/tutorials/")
    End Sub

    Private Sub ExportMonkeySpeakToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExportMonkeySpeakToolStripMenuItem.Click
        If loggingIn > 0 Then Exit Sub
        MS_Export.Show()
        MS_Export.Activate()
    End Sub


    
    Sub ContentsToolStripMenuItemClick(sender As Object, e As EventArgs)
    	
    End Sub
End Class

