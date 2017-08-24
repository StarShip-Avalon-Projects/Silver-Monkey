Imports MonkeyCore
Imports Monkeyspeak
Imports SilverMonkeyEngine.Engine.Libraries
Imports SilverMonkeyEngine.SmConstants

Namespace Engine

    ''' <summary>
    ''' Silver Monkey's <see cref="Monkeyspeak.Page"/> handler
    ''' <para>
    ''' In here we handle loading the Monkey Speak script file and load the
    ''' Monkey Speak Default Libraries and the Silver Monkey Libraries
    ''' </para>
    ''' </summary>
    Public Class MonkeySpeakPage : Inherits Monkeyspeak.Page

        'Public Sub LoadPluginLibrary(Optional ByRef start As Boolean = False)
        '    'Library Loaded?.. Get the Hell out of here

        ' If MS_Started() Then Exit Sub MS_Stared += 1

        ' Dim objPlugin As Interfaces.msPlugin

        ' 'Loop through available plugins, creating instances and adding
        ' them to listbox

        ' For intIndex As Integer = 0 To Plugins.Count - 1 objPlugin =
        ' CType(PluginServices.CreateInstance(Main.Plugins(intIndex)),
        ' Interfaces.msPlugin) objPlugin.Initialize(Main.objHost)
        ' objPlugin.MsPage = MSpage objPlugin.Start() Next

        'End Sub

#Region "Public Fields"

        Public Shared MS_Stared As Integer = 0
        Public MS_Engine_Running As Boolean

#End Region

#Region "Private Fields"

        ''' <summary>
        ''' Library Objects to load into the Engine
        ''' </summary>
        Private LibList As List(Of Monkeyspeak.Libraries.AbstractBaseLibrary)

        ''' <summary>
        ''' The Main Monkey Speak Script engine for Silver Monkey
        ''' </summary>
        Private MSEngine As MainEngine

        ''' <summary>
        ''' MonkeySpeak Engine configuration options
        ''' </summary>
        Private options As EngineOptoons

#End Region

#Region "Public Constructors"

        Private MsPage As Monkeyspeak.Page

        Sub New()
            MyBase.New(Nothing)
            options = New EngineOptoons
            MSEngine = New MainEngine(options, New BotSession(New BotOptions()))

            MsPage = MSEngine.LoadFromString("")
            Initialize()
        End Sub

        Public Sub New(engine As MainEngine, ByRef Page As Monkeyspeak.Page)

            MyBase.New(engine)
            MsPage = Page

            MSEngine = engine
            options = engine.Options
            Initialize()

        End Sub

        Private Sub Initialize()
            LibList = New List(Of Monkeyspeak.Libraries.AbstractBaseLibrary)
            ' Comment out Libs to Disable
            LibList.Add(New MsIO(MSEngine.FurcadiaSession))
            LibList.Add(New StringLibrary(MSEngine.FurcadiaSession))
            LibList.Add(New SayLibrary(MSEngine.FurcadiaSession))
            LibList.Add(New Banish(MSEngine.FurcadiaSession))
            LibList.Add(New MathLibrary(MSEngine.FurcadiaSession))
            LibList.Add(New MsTime(MSEngine.FurcadiaSession))
            LibList.Add(New MsDatabase(MSEngine.FurcadiaSession))
            LibList.Add(New MsWebRequests(MSEngine.FurcadiaSession))
            LibList.Add(New MS_Cookie(MSEngine.FurcadiaSession))
            LibList.Add(New MsPhoenixSpeak(MSEngine.FurcadiaSession))
            LibList.Add(New MsPhoenixSpeakBackupAndRestore(MSEngine.FurcadiaSession))
            LibList.Add(New MsDice(MSEngine.FurcadiaSession))
            LibList.Add(New MsFurreList(MSEngine.FurcadiaSession))
            LibList.Add(New MsWarning(MSEngine.FurcadiaSession))
            LibList.Add(New Movement(MSEngine.FurcadiaSession))
            LibList.Add(New WmCpyDta(MSEngine.FurcadiaSession))
            LibList.Add(New MsMemberList(MSEngine.FurcadiaSession))
            LibList.Add(New MsPounce(MSEngine.FurcadiaSession))
            LibList.Add(New MsVerbot(MSEngine.FurcadiaSession))
            LibList.Add(New MsSound(MSEngine.FurcadiaSession))
            'LibList.Add(New MS_MemberList())
            'LibList.Add(New MS_MemberList())
            'LibList.Add(New MS_MemberList())
        End Sub

#End Region

#Region "Public Methods"

        Public Shared Function MS_Started() As Boolean
            ' 0 = main load 1 = engine start 2 = engine running
            Return MS_Stared >= 2
        End Function

        ''' <summary>
        ''' Export MonkeySpeak descriptions
        ''' </summary>
        Public Function Export() As Page

            Try

                ' Console.WriteLine("Execute (0:0)")
                MS_Stared = 0

                MsPage = MSEngine.LoadFromString("")
                LoadLibrary(False, True)

                MS_Engine_Running = False
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)

            End Try
            Return MsPage
        End Function

        ''' <summary>
        ''' Loads monkey speak libraries
        ''' </summary>
        ''' <param name="Library">
        ''' </param>
        Public Shadows Sub LoadLibrary(ByRef Library As Monkeyspeak.Libraries.AbstractBaseLibrary)
            MyBase.LoadLibrary(Library)
        End Sub

        ''' <summary>
        ''' Load Libraries into the engine
        ''' </summary>
        ''' <param name="LoadPlugins">
        ''' </param>
        ''' <returns>
        ''' reference to the active <see cref="MonkeySpeak.Page"/>
        ''' </returns>
        Public Shadows Function LoadLibrary(ByRef LoadPlugins As Boolean, ByVal silent As Boolean) As Page
            'Library Loaded?.. Get the Hell out of here
            If MS_Started() Then Return Me
            MS_Stared += 1

            MsPage.Reset()
            MsPage.SetTriggerHandler(TriggerCategory.Cause, 0,
         Function()
             Return True
         End Function, "(0:0) When the bot starts,")
            Try
                MsPage.LoadSysLibrary()

#If CONFIG = "Release" Then
            '(5:110) load library from file {...}.
            MsPage.RemoveTriggerHandler(TriggerCategory.Effect, 110)
#ElseIf CONFIG = "Debug" Then
                '(5:105) raise an error.
                MsPage.RemoveTriggerHandler(TriggerCategory.Effect, 105)
                MsPage.SetTriggerHandler(TriggerCategory.Effect, 105,
         Function()
             Return False
         End Function, "(5:105) raise an error.")
#End If
            Catch ex As Exception
                Dim e As New ErrorLogging(ex, Me)
            End Try
            Try
                MsPage.LoadTimerLibrary()
                MsPage.LoadStringLibrary()
                MsPage.LoadMathLibrary()
            Catch ex As Exception
                Dim e As New ErrorLogging(ex, Me)
            End Try

            For Each Library As Monkeyspeak.Libraries.AbstractBaseLibrary In LibList
                Try
                    MsPage.LoadLibrary(Library)
                    If Not silent Then Console.WriteLine(String.Format("Loaded Monkey Speak Library: {0}", Library.GetType().Name))
                Catch ex As Exception

                    Dim e As New ErrorLogging(ex, Library)
                    ' Console.WriteLine(String.Format("Error loading Monkey
                    ' Speak Library: {0}", Library.GetType().Name))
                End Try
            Next

            'Define our Triggers before we use them
            'TODO Check for Duplicate and use that one instead
            'we don't want this to cause a memory leak.. its prefered to run this one time and thats  it except for checking for new plugins
            'Loop through available plugins, creating instances and adding them to listbox
            'If Not Plugins Is Nothing And LoadPlugins Then

            'End If
            'Dim objPlugin As Interfaces.msPlugin
            'Dim newPlugin As Boolean = False
            'For intIndex As Integer = 0 To Plugins.Count - 1
            '    Try
            '        objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(intIndex)), Interfaces.msPlugin)
            '        If Not PluginList.ContainsKey(objPlugin.Name.Replace(" ", "")) Then
            '            PluginList.Add(objPlugin.Name.Replace(" ", ""), True)
            '            newPlugin = True
            '        End If

            ' If PluginList.Item(objPlugin.Name.Replace(" ", "")) = True
            ' Then Console.WriteLine("Loading Plugin: " + objPlugin.Name)
            ' objPlugin.Initialize(objHost) objPlugin.MonkeySpeakPage =
            ' MonkeySpeakPage objPlugin.Start() End If Catch ex As Exception
            ' Dim e As New ErrorLogging(ex, Me) End Try Next 'TODO: Add to
            '' Delegate? 'If newPlugin Then Main.MainSettings.SaveMainSettings()

            'End If
            Return MsPage
        End Function

        Public Sub PageSetVariable(ByVal VariableList As Dictionary(Of String, Object))
            If options.MS_Engine_Enable Then

                For Each kv As KeyValuePair(Of String, Object) In VariableList
                    MsPage.SetVariable(kv.Key.ToUpper, kv.Value, True)
                Next '

            End If
        End Sub

        'Bot Starts
        Public Function Start() As Page

            Try
                Dim TimeStart As DateTime = DateTime.Now
                Dim VariableList As New Dictionary(Of String, Object)

                ' Console.WriteLine("Execute (0:0)")
                MS_Stared = 1
                MsPage.Reset()
                LoadLibrary(False, False)

                VariableList.Add("DREAMOWNER", Nothing)
                VariableList.Add("DREAMNAME", Nothing)
                VariableList.Add("BOTNAME", Nothing)
                VariableList.Add("BOTCONTROLLER", options.BotController)
                VariableList.Add(MS_Name, Nothing)
                VariableList.Add("MESSAGE", Nothing)
                VariableList.Add("BANISHNAME", Nothing)
                VariableList.Add("BANISHLIST", Nothing)
                PageSetVariable(VariableList)
                '(0:0) When the bot starts,
                MsPage.Execute(0)
                Console.WriteLine(String.Format("Done!!! Executed {0} triggers in {1} seconds.",
                                                MsPage.Size, Date.Now.Subtract(TimeStart).Seconds))
                MS_Engine_Running = True
            Catch eX As Exception
                Dim logError As New ErrorLogging(eX, Me)

            End Try
            Return MsPage
        End Function

#End Region

    End Class

End Namespace