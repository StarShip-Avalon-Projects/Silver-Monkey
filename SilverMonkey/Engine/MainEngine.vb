Imports Monkeyspeak

Imports MonkeyCore
Imports System.Collections.Generic
Imports MonkeyCore.Settings
Imports System.Text.RegularExpressions
Imports System.Diagnostics
Imports SilverMonkey.PhoenixSpeak
Imports Furcadia.Net
Imports System.Threading
Imports Microsoft.Win32.SafeHandles
Imports System.Runtime.InteropServices
Imports Conversive.Verbot5

Public Class MainMSEngine
    Implements IDisposable


#Region "Const"
    Private Const MS_Header As String = "*MSPK V04.00 Silver Monkey"
    Private Const MS_Footer As String = "*Endtriggers* 8888 *Endtriggers*"
#End Region

#Region "Verbot"
    <CLSCompliant(False)>
    Public verbot As Verbot5Engine
    <CLSCompliant(False)>
    Public state As State
    <CLSCompliant(False)>
    Public kb As KnowledgeBase = New KnowledgeBase()
    <CLSCompliant(False)>
    Public kbi As KnowledgeBaseItem = New KnowledgeBaseItem()
#End Region
    Public Const REGEX_NameFilter As String = "[^a-z0-9\0x0020_;&|]+"
    Public Shared Function ToFurcShortName(ByVal value As String) As String
        If String.IsNullOrEmpty(value) Then Return Nothing
        Return Regex.Replace(value.ToLower, REGEX_NameFilter, "", RegexOptions.CultureInvariant)
    End Function

    Public Shared Function IsBotControler(ByRef Name As String) As Boolean
        If String.IsNullOrEmpty(cBot.BotController) Then Return False
        Return ToFurcShortName(cBot.BotController) = ToFurcShortName(Name)
    End Function
#Region "MonkeySpeakEngine"
    Public Shared Function MS_Started() As Boolean
        ' 0 = main load
        ' 1 = engine start
        ' 2 = engine running
        Return MS_Stared >= 2
    End Function
    Public Shared MS_Stared As Integer = 0
    Private Shared Writer As TextBoxWriter = New TextBoxWriter(Variables.TextBox1)
    Private Const RES_MS_begin As String = "*MSPK V"
    Private Const RES_MS_end As String = "*Endtriggers* 8888 *Endtriggers*"

    Public EngineRestart As Boolean = False

    Public MS_Engine_Running As Boolean = False
    Public engine As MonkeyspeakEngine = New MonkeyspeakEngine()
    Public Shared WithEvents MSpage As Page = Nothing
    Public Sub New()

        EngineStart(True)
    End Sub
    'Bot Starts
    Public Function ScriptStart() As Boolean
        Try
            Dim VariableList As New Dictionary(Of String, Object)

            If Not cBot.MS_Engine_Enable Then
                Return False
            End If

            Console.WriteLine("Loading:" & cBot.MS_File)
            Dim start As DateTime = DateTime.Now
            If Not File.Exists(cBot.MS_File) Then
                Directory.Exists(Path.GetDirectoryName(cBot.MS_File))
                cBot.MS_File = Path.Combine(Paths.SilverMonkeyBotPath, cBot.MS_File)
            End If
            cBot.MS_Script = msReader(cBot.MS_File)
            If String.IsNullOrEmpty(cBot.MS_Script) Then
                Console.WriteLine("ERROR: No script loaded! Loading Default MonkeySpeak.")
                MS_Engine_Running = False
                msReader(IO.NewMSFile)
                VariableList.Add("MSPATH", "!!! Not Specified !!!")
            Else
                VariableList.Add("MSPATH", Paths.SilverMonkeyBotPath)
            End If
            Try
                MSpage = engine.LoadFromString(cBot.MS_Script)
            Catch ex As MonkeyspeakException
                Console.WriteLine(ex.Message)
                Return False
            Catch ex As Exception
                Console.WriteLine("There's an error loading the bot script")
                Return False
            End Try
            ' Console.WriteLine("Execute (0:0)")
            MS_Stared = 1
            LoadLibrary(True)


            VariableList.Add("DREAMOWNER", "")
            VariableList.Add("DREAMNAME", "")
            VariableList.Add("BOTNAME", "")
            VariableList.Add("BOTCONTROLLER", cBot.BotController)
            VariableList.Add(MS_Name, "")
            VariableList.Add("MESSAGE", "")
            VariableList.Add("BANISHNAME", "")
            VariableList.Add("BANISHLIST", "")
            PageSetVariable(VariableList)
            '(0:0) When the bot starts,
            PageExecute(0)
            Console.WriteLine("Done! Executed in " & DateTime.Now.Subtract(start).ToString())
            'Console.ReadKey()
            MS_Engine_Running = True
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
            Return False
        End Try
        Return True
    End Function

    'loads at main load
    Public Sub EngineStart(ByRef LoadPlugins As Boolean)

        MSpage = engine.LoadFromString("")
        LoadLibrary(LoadPlugins)

    End Sub



    Public Sub LoadLibrary(ByRef LoadPlugins As Boolean)
        'Library Loaded?.. Get the Hell out of here
        If MS_Started() Then Exit Sub
        MS_Stared += 1
        MSpage.SetTriggerHandler(TriggerCategory.Cause, 0,
             Function()
                 Return True
             End Function, "(0:0) When the bot starts,")
        Try
            MSpage.LoadSysLibrary()
#If CONFIG = "Release" Then
            '(5:105) raise an error.
            MSpage.RemoveTriggerHandler(TriggerCategory.Effect, 105)
            '(5:110) load library from file {...}.
            MSpage.RemoveTriggerHandler(TriggerCategory.Effect, 110)
#ElseIf CONFIG = "Debug" Then

#End If
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MS_IO())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadTimerLibrary()
            MSpage.LoadStringLibrary()
            MSpage.LoadMathLibrary()
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New StringLibrary()) ' Load our new TestLibrary
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New SayLibrary())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New Banish())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MathLibrary())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MS_Time())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MSPK_MDB())
        Catch ex As FileNotFoundException
            Console.WriteLine(ex.Message)
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MSPK_Web())

        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MS_Cookie())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MsPhoenixSpeak())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New DatabaseSystem())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MS_Dice())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New Description())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try

            MSpage.LoadLibrary(New FurreList())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New Warning())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New Movement())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New WmCpyDta())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            PounceTimer = New Threading.Timer(AddressOf smPounceSend, Nothing, TimeSpan.Zero, TimeSpan.FromSeconds(30))
            PounceTimer.InitializeLifetimeService()
            MSpage.LoadLibrary(New MS_Pounce())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MS_MemberList())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MS_Verbot())
        Catch ex As Exception
            Dim e As New ErrorLogging(ex, Me)
        End Try

        'Define our Triggers before we use them
        'TODO Check for Duplicate and use that one instead
        'we don't want this to cause a memory leak.. its prefered to run this one time and thats  it except for checking for new plugins
        'Loop through available plugins, creating instances and adding them to listbox
        If Not Plugins Is Nothing And LoadPlugins Then
            Dim objPlugin As Interfaces.msPlugin
            Dim newPlugin As Boolean = False
            For intIndex As Integer = 0 To Plugins.Count - 1
                Try
                    objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(intIndex)), Interfaces.msPlugin)
                    If Not PluginList.ContainsKey(objPlugin.Name.Replace(" ", "")) Then
                        PluginList.Add(objPlugin.Name.Replace(" ", ""), True)
                        newPlugin = True
                    End If

                    If PluginList.Item(objPlugin.Name.Replace(" ", "")) = True Then
                        Console.WriteLine("Loading Plugin: " + objPlugin.Name)
                        objPlugin.Initialize(Main.FurcSession.objHost)
                        objPlugin.Page = MSpage
                        objPlugin.Start()
                    End If
                Catch ex As Exception
                    Dim e As New ErrorLogging(ex, Me)
                End Try
            Next
            If newPlugin Then Main.MainSettings.SaveMainSettings()


        End If
    End Sub

    Private msVer As Double = 3.0
    Public Function msReader(ByVal file As String) As String
        file = file.Trim
        Dim Data As String = String.Empty
        Try
            If Not System.IO.File.Exists(file.Trim) Then
                Return ""
            End If
            Dim line As String = ""
            Using objReader As New StreamReader(file)
                ' line = objReader.ReadLine() & Environment.NewLine
                While objReader.Peek <> -1
                    line = objReader.ReadLine()
                    If Not line.StartsWith(RES_MS_begin) Then
                        Data += line + Environment.NewLine
                    End If

                    If line = RES_MS_end Then
                        Exit While
                    End If



                End While
                objReader.Close()
                objReader.Dispose()
            End Using
            Return Data
        Catch eX As Exception
            Dim LogError As New ErrorLogging(eX, Me)
            Return ""
        End Try
    End Function

    Public Shared Sub PageSetVariable(ByVal varName As String, ByVal data As Object)
        If cBot.MS_Engine_Enable AndAlso MS_Started() Then
            If data Is Nothing Then data = String.Empty
            Debug.Print("Settingg Variable: " + varName + ":" + data.ToString)
            MSpage.SetVariable(varName.ToUpper, data, True) '

        End If
    End Sub

    Public Shared Sub PageSetVariable(ByVal VariableList As Dictionary(Of String, Object))
        If cBot.MS_Engine_Enable Then

            For Each kv As KeyValuePair(Of String, Object) In VariableList
                MSpage.SetVariable(kv.Key.ToUpper, kv.Value, True)
            Next '

        End If
    End Sub

    Public Shared Sub PageSetVariable(ByVal varName As String, ByVal data As Object, ByVal Constant As Boolean)
        If Not IsNothing(cBot) Then
            If cBot.MS_Engine_Enable AndAlso MS_Started() Then
                Debug.Print("Settingg Variable: " + varName + ":" + data.ToString)
                MSpage.SetVariable(varName.ToUpper, data, Constant) '
            End If
        End If
    End Sub

    Public Sub PageExecute(ParamArray ID() As Integer)
        If Not IsNothing(cBot) Then
            If cBot.MS_Engine_Enable AndAlso MS_Started() Then
                MSpage.Execute(ID)

            End If
        End If

    End Sub

    Public Shared Sub LogError(trigger As Trigger, ex As Exception) Handles MSpage.Error

        Console.WriteLine(MS_ErrWarning)
        Dim ErrorString As String = "Error: (" & trigger.Category.ToString & ":" & trigger.Id.ToString & ") " & ex.Message

        If Not IsNothing(cBot) Then
            If cBot.log Then
                LogStream.Writeline(ErrorString, ex)
            End If
        End If
        Writer.WriteLine(ErrorString)
    End Sub

    Public Shared Sub LogError(reader As TriggerReader, ex As Exception)

        Console.WriteLine(MS_ErrWarning)
        Dim ErrorString As String = "Error: (" & reader.TriggerCategory.ToString & ":" & reader.TriggerId.ToString & ") " & ex.Message

        If Not IsNothing(cBot) Then
            If cBot.log Then
                LogStream.Writeline(ErrorString, ex)
            End If
        End If
        Writer.WriteLine(ErrorString)
    End Sub

#End Region

#Region "smPounce"
    Private WithEvents smPounce As PounceConnection
    Private PounceTimer As Threading.Timer
    Public Structure pFurre
        Public WasOnline As Boolean
        Public Online As Boolean
    End Structure

    Public FurreList As New Dictionary(Of String, pFurre)


    Dim lastaccess As Date

    Private Function ReadOnlineList() As Boolean
        Dim result As Boolean = False
        If File.Exists(MS_Pounce.OnlineList) Then
            If File.GetLastWriteTime(MS_Pounce.OnlineList) <> lastaccess Then
                lastaccess = File.GetLastWriteTime(MS_Pounce.OnlineList)


                Dim NameList() As String = File.ReadAllLines(MS_Pounce.OnlineList)
                For i As Integer = 0 To NameList.Length - 1
                    If Not FurreList.ContainsKey(NameList(i)) Then FurreList.Add(NameList(i), New pFurre)
                Next
                Dim Namelist2(FurreList.Count - 1) As String
                FurreList.Keys.CopyTo(Namelist2, 0)
                For i As Integer = 0 To Namelist2.Length - 1
                    Dim found As Boolean = False
                    For j As Integer = 0 To NameList.Length - 1
                        If MainMSEngine.ToFurcShortName(NameList(j)) = MainMSEngine.ToFurcShortName(Namelist2(i)) Then
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
            '   If _FormClose Then Exit Sub
            '   If Not bConnected() Then Exit Sub
            If Not ReadOnlineList() Then Exit Sub
            smPounce = New PounceConnection("http://on.furcadia.com/q/", Nothing)

            smPounce.RemoveFriends()
            For Each kv As KeyValuePair(Of String, pFurre) In FurreList
                If Not String.IsNullOrEmpty(kv.Key) Then
                    smPounce.AddFriend(MainMSEngine.ToFurcShortName(kv.Key))
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
                If MainMSEngine.ToFurcShortName(_furre) = MainMSEngine.ToFurcShortName([friend]) Then
                    test.Online = True
                    Exit For
                End If
            Next
            FurreList.Item(_furre) = test
            If test.WasOnline = True And test.Online = False Then
                'Furre Logged off
                callbk.FurcSession.SendClientMessage("smPounce", _furre + " has logged out.")
                FurcadiaSession.Player = FurcadiaSession.NametoFurre(_furre, True)
                PageExecute(951, 953)
            ElseIf test.WasOnline = False And test.Online = True Then
                'Furre logged on
                callbk.FurcSession.SendClientMessage("smPounce", _furre + " has logged on.")
                FurcadiaSession.Player = FurcadiaSession.NametoFurre(_furre, True)
                PageExecute(950, 952)
            End If

        Next
    End Sub

#End Region

#Region "Dispose"

    Dim disposed As Boolean = False
    ' Instantiate a SafeHandle instance.
    Dim handle As SafeHandle = New SafeFileHandle(IntPtr.Zero, True)

    ' Public implementation of Dispose pattern callable by consumers.
    Public Sub Dispose() _
               Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ' Protected implementation of Dispose pattern.
    Protected Overridable Sub Dispose(disposing As Boolean)
        If disposed Then Return

        If disposing Then
            handle.Dispose()
            ' Free any other managed objects here.
            If Not IsNothing(PounceTimer) Then PounceTimer.Dispose()
        End If

        ' Free any unmanaged objects here.
        '
        disposed = True
    End Sub
#End Region

End Class
