Imports Monkeyspeak
Imports Furcadia.Util
Imports MonkeyCore
Imports System.Collections.Generic
Imports MonkeyCore.Settings
Imports System.Diagnostics

Imports Microsoft.Win32.SafeHandles
Imports System.Runtime.InteropServices
Imports SilverMonkey.Engine.Libraries
Imports System.Text

Namespace Engine
    ''' <summary>
    ''' Silver Monkey's MonkeySpeak Engine with our Customizations
    ''' </summary>
    Public Class MainMsEngine : Inherits MonkeyspeakEngine
        Implements IDisposable

#Region "Public Fields"

#End Region

#Region "Private Fields"

        ''' <summary>
        '''
        ''' </summary>
        Private WithEvents MonkeySpeakPage As Page

        ''' <summary>
        ''' Library Objects to load into the Engine
        ''' </summary>
        Private LibList As List(Of Monkeyspeak.Libraries.AbstractBaseLibrary)

        ''' <summary>
        ''' Custome Options for this Engine
        ''' </summary>
        Private SilverMonkeyEngineOptions As EngineOptoons
#End Region

#Region "Const"
        Private Const MS_Footer As String = "*Endtriggers* 8888 *Endtriggers*"
        Private Const MS_Header As String = "*MSPK V04.00 Silver Monkey"
#End Region

#Region "MonkeySpeakEngine"

        Public Shared MS_Stared As Integer = 0

        Public EngineRestart As Boolean = False

        Public FurcadiaSession As BotSession
        Public MS_Engine_Running As Boolean = False

        Private Const RES_MS_begin As String = "*MSPK V"

        Private Const RES_MS_end As String = "*Endtriggers* 8888 *Endtriggers*"

        Private Shared Writer As TextBoxWriter = New TextBoxWriter(Variables.TextBox1)

        Private msVer As Double = 3.0
        ''' <summary>
        ''' Default Constructlor.
        ''' <para>
        ''' This Loads our MonkeyBeak Libraries
        ''' </para>
        ''' </summary>
        Public Sub New(ByRef Options As EngineOptoons, ByRef FurcSession As BotSession)
            MyBase.New(Options)
            SilverMonkeyEngineOptions = Options
            FurcadiaSession = FurcSession
            'EngineStart(True)
            LibList = New List(Of Monkeyspeak.Libraries.AbstractBaseLibrary)
            ' Comment out Libs to Disable
            LibList.Add(New MS_IO())
            LibList.Add(New StringLibrary())
            LibList.Add(New SayLibrary())
            LibList.Add(New Banish())
            LibList.Add(New MathLibrary())
            LibList.Add(New MS_Time())
            LibList.Add(New MSPK_MDB())
            LibList.Add(New MSPK_Web())
            LibList.Add(New MS_Cookie())
            LibList.Add(New MsPhoenixSpeak())
            LibList.Add(New DatabaseSystem())
            LibList.Add(New MS_Dice())
            LibList.Add(New MonkeySpeakFurreList())
            LibList.Add(New Warning())
            LibList.Add(New Movement())
            LibList.Add(New WmCpyDta())
            LibList.Add(New MS_MemberList())
            LibList.Add(New Pounce.MsPounce())
            LibList.Add(New MS_Verbot())
            LibList.Add(New MS_MemberList())
            LibList.Add(New MS_MemberList())
            LibList.Add(New MS_MemberList())
            LibList.Add(New MS_MemberList())

        End Sub

        Public Shared Function MS_Started() As Boolean
            ' 0 = main load
            ' 1 = engine start
            ' 2 = engine running
            Return MS_Stared >= 2
        End Function

        'loads at main load
        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="LoadPlugins"></param>
        Public Sub EngineStart(ByRef LoadPlugins As Boolean)
            MonkeySpeakPage = New Page(Me)
            MonkeySpeakPage = LoadFromString("")
            LoadLibrary(LoadPlugins)

        End Sub

        ''' <summary>
        ''' Wrapper Functions to read a Monkey Speak Script File and Pass the result to <see cref="LoadFromString"/>
        ''' </summary>
        ''' <param name="file">MonkeySpeak filename</param>
        ''' <returns></returns>
        Public Function LoadFromScriptFile(ByVal file As String) As Page
            Dim MonkeySpeakScript As New StringBuilder()
            Try

                If Not System.IO.File.Exists(file) Then
                    Throw New FileNotFoundException("MonkeySpeak script file not found.")
                End If

                Using MonkeySpeakScriptReader As New StreamReader(file)
                    Dim line As String = ""

                    While MonkeySpeakScriptReader.Peek <> -1
                        line = MonkeySpeakScriptReader.ReadLine()
                        If Not line.StartsWith(RES_MS_begin) Then
                            MonkeySpeakScript.AppendLine(line)
                        ElseIf line.StartsWith(RES_MS_begin) Then
                            'MonkeySpeak Script Version Check

                        End If

                        If line = RES_MS_end Then
                            Exit While
                        End If

                    End While
                    MonkeySpeakScriptReader.Close()
                End Using

            Catch eX As Exception
                Dim LogError As New ErrorLogging(eX, Me)
            End Try
            Return LoadFromString(MonkeySpeakScript.ToString())
        End Function

        ''' <summary>
        ''' Load Libraries into the engine
        ''' </summary>
        ''' <param name="LoadPlugins"></param>
        Public Sub LoadLibrary(ByRef LoadPlugins As Boolean)
            'Library Loaded?.. Get the Hell out of here
            If MS_Started() Then Exit Sub
            MS_Stared += 1

            MonkeySpeakPage.Reset()
            MonkeySpeakPage.SetTriggerHandler(TriggerCategory.Cause, 0,
             Function()
                 Return True
             End Function, "(0:0) When the bot starts,")
            Try
                MonkeySpeakPage.LoadSysLibrary()
#If CONFIG = "Release" Then
            '(5:105) raise an error.
            MonkeySpeakPage.RemoveTriggerHandler(TriggerCategory.Effect, 105)
            '(5:110) load library from file {...}.
            MonkeySpeakPage.RemoveTriggerHandler(TriggerCategory.Effect, 110)
#ElseIf CONFIG = "Debug" Then
                MonkeySpeakPage.SetTriggerHandler(TriggerCategory.Effect, 105,
             Function()
                 Return False
             End Function, "(5:105) raise an error.")
#End If
            Catch ex As Exception
                Dim e As New ErrorLogging(ex, Me)
            End Try
            Try
                MonkeySpeakPage.LoadTimerLibrary()
                MonkeySpeakPage.LoadStringLibrary()
                MonkeySpeakPage.LoadMathLibrary()
            Catch ex As Exception
                Dim e As New ErrorLogging(ex, Me)
            End Try

            For Each Library As Monkeyspeak.Libraries.AbstractBaseLibrary In LibList
                Try
                    MonkeySpeakPage.LoadLibrary(Library)
                    Console.WriteLine(String.Format("Loaded Monkey Speak Library: {0}", Library.GetType().Name))
                Catch ex As Exception
                    Dim e As New ErrorLogging(ex, Library)
                    Console.WriteLine(String.Format("Error loading Monkey Speak Library: {0}", Library.GetType().Name))
                End Try
            Next

            'Define our Triggers before we use them
            'TODO Check for Duplicate and use that one instead
            'we don't want this to cause a memory leak.. its prefered to run this one time and thats  it except for checking for new plugins
            'Loop through available plugins, creating instances and adding them to listbox
            'If Not Plugins Is Nothing And LoadPlugins Then
            '    Dim objPlugin As Interfaces.msPlugin
            '    Dim newPlugin As Boolean = False
            '    For intIndex As Integer = 0 To Plugins.Count - 1
            '        Try
            '            objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(intIndex)), Interfaces.msPlugin)
            '            If Not PluginList.ContainsKey(objPlugin.Name.Replace(" ", "")) Then
            '                PluginList.Add(objPlugin.Name.Replace(" ", ""), True)
            '                newPlugin = True
            '            End If

            '            If PluginList.Item(objPlugin.Name.Replace(" ", "")) = True Then
            '                Console.WriteLine("Loading Plugin: " + objPlugin.Name)
            '                objPlugin.Initialize(objHost)
            '                objPlugin.MonkeySpeakPage = MonkeySpeakPage
            '                objPlugin.Start()
            '            End If
            '        Catch ex As Exception
            '            Dim e As New ErrorLogging(ex, Me)
            '        End Try
            '    Next
            '    'TODO: Add to delegate?
            '    'If newPlugin Then Main.MainSettings.SaveMainSettings()

            'End If
        End Sub

        'Public Sub LogError(reader As TriggerReader, ex As Exception)

        '    Console.WriteLine(MS_ErrWarning)
        '    Dim ErrorString As String = "Error: (" & reader.TriggerCategory.ToString & ":" & reader.TriggerId.ToString & ") " & ex.Message

        '    If Not IsNothing(cBot) Then
        '        If cBot.log Then
        '            LogStream.WriteLine(ErrorString, ex)
        '        End If
        '    End If
        '    Writer.WriteLine(ErrorString)
        'End Sub
        'Public Sub LogError(trigger As Trigger, ex As Exception) Handles MonkeySpeakPage.Error

        '    Console.WriteLine(MS_ErrWarning)
        '    Dim ErrorString As String = "Error: (" & trigger.Category.ToString & ":" & trigger.Id.ToString & ") " & ex.Message

        '    If Not IsNothing(cBot) Then
        '        If cBot.log Then
        '            '  BotLogStream.WriteLine(ErrorString, ex)
        '        End If
        '    End If
        '    Writer.WriteLine(ErrorString)
        'End Sub
        ''' <summary>
        ''' Execute the Triggers safely
        ''' </summary>
        ''' <param name="ID"></param>
        Public Sub PageExecute(ParamArray ID() As Integer)
            If SilverMonkeyEngineOptions.EngineEnable AndAlso MS_Started() Then
                MonkeySpeakPage.Execute(ID)
            End If

        End Sub
        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="varName"></param>
        ''' <param name="data"></param>
        Public Sub PageSetVariable(ByVal varName As String, ByVal data As Object)
            If SilverMonkeyEngineOptions.EngineEnable AndAlso MS_Started() Then
                If data Is Nothing Then data = String.Empty
                Debug.Print("Settingg Variable: " + varName + ":" + data.ToString)
                MonkeySpeakPage.SetVariable(varName.ToUpper, data, True) '

            End If
        End Sub

        Public Sub PageSetVariable(ByVal VariableList As Dictionary(Of String, Object))
            If SilverMonkeyEngineOptions.EngineEnable Then

                For Each kv As KeyValuePair(Of String, Object) In VariableList
                    PageSetVariable(kv.Key.ToUpper, kv.Value, True)
                Next '

            End If
        End Sub

        Public Sub PageSetVariable(ByVal varName As String, ByVal data As Object, ByVal Constant As Boolean)

            If SilverMonkeyEngineOptions.EngineEnable AndAlso MS_Started() Then
                Debug.Print("Settingg Variable: " + varName + ":" + data.ToString)
                MonkeySpeakPage.SetVariable(varName.ToUpper, data, Constant) '
            End If

        End Sub
        'Bot Starts
        Public Sub Start()
            Try
                Dim TimeStart As DateTime = DateTime.Now
                Dim VariableList As New Dictionary(Of String, Object)

                Console.WriteLine("Loading:" & SilverMonkeyEngineOptions.MonkeySpeakScriptFile)

                If String.IsNullOrEmpty(SilverMonkeyEngineOptions.MonkeySpeakScriptFile) Then
                    Console.WriteLine("ERROR: No script loaded! Loading Default ")
                    MS_Engine_Running = False
                    LoadFromString(MonkeyCore.IO.NewMSFile)
                    VariableList.Add("MSPATH", "!!! Not Specified !!!")
                Else
                    VariableList.Add("MSPATH", Paths.SilverMonkeyBotPath)
                End If
                Try

                Catch ex As MonkeyspeakException
                    Console.WriteLine(ex.Message)
                Catch ex As Exception
                    Console.WriteLine("There's an error loading the bot script")

                End Try
                ' Console.WriteLine("Execute (0:0)")
                MS_Stared = 1
                LoadLibrary(True)

                VariableList.Add("DREAMOWNER", "")
                VariableList.Add("DREAMNAME", "")
                VariableList.Add("BOTNAME", "")
                VariableList.Add("BOTCONTROLLER", FurcadiaSession.BotController)
                VariableList.Add(MS_Name, "")
                VariableList.Add("MESSAGE", "")
                VariableList.Add("BANISHNAME", "")
                VariableList.Add("BANISHLIST", "")
                PageSetVariable(VariableList)
                '(0:0) When the bot starts,
                PageExecute(0)
                Console.WriteLine(String.Format("Done!!! Executed {0} triggers in {1} seconds.", MonkeySpeakPage.Size, Date.Now.Subtract(TimeStart).Seconds))
                MS_Engine_Running = True
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)

            End Try

        End Sub
#End Region

#Region "Dispose"
        'need Timer Library disposal here and any other Libs that need to be disposed

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
                Monkeyspeak.Libraries.Timers.DestroyTimers()
                handle.Dispose()

            End If

            ' Free any unmanaged objects here.
            '
            disposed = True
        End Sub
#End Region

    End Class
End Namespace