﻿Imports Monkeyspeak
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

        Sub New()
            MyBase.New(Nothing)
            options = New EngineOptoons
            MSEngine = New MainEngine(options, New BotSession(New BotOptions()))

            MSEngine.LoadFromString("")

            Initialize()
        End Sub

        Public Sub New(engine As MainEngine, ByRef Page As Monkeyspeak.Page)
            MyBase.New(engine)

            MSEngine = engine
            options = engine.Options
            Initialize()

        End Sub

        Private Sub Initialize()
            ' Comment out Libs to Disable
            LibList = New List(Of Monkeyspeak.Libraries.AbstractBaseLibrary) From {
                New MsIO(MSEngine.FurcadiaSession),
                New StringLibrary(MSEngine.FurcadiaSession),
                New SayLibrary(MSEngine.FurcadiaSession),
                New Banish(MSEngine.FurcadiaSession),
                New MathLibrary(MSEngine.FurcadiaSession),
                New MsTime(MSEngine.FurcadiaSession),
                New MsDatabase(MSEngine.FurcadiaSession),
                New MsWebRequests(MSEngine.FurcadiaSession),
                New MS_Cookie(MSEngine.FurcadiaSession),
                New MsPhoenixSpeak(MSEngine.FurcadiaSession),
                New MsPhoenixSpeakBackupAndRestore(MSEngine.FurcadiaSession),
                New MsDice(MSEngine.FurcadiaSession),
                New MsFurreList(MSEngine.FurcadiaSession),
                New MsWarning(MSEngine.FurcadiaSession),
                New Movement(MSEngine.FurcadiaSession),
                New WmCpyDta(MSEngine.FurcadiaSession),
                New MsMemberList(MSEngine.FurcadiaSession),
                New MsPounce(MSEngine.FurcadiaSession),
                New MsVerbot(MSEngine.FurcadiaSession),
                New MsSound(MSEngine.FurcadiaSession),
                New MsTrades(MSEngine.FurcadiaSession),
                New MsDreamInfo(MSEngine.FurcadiaSession)
            }

            'LibList.Add(New MS_MemberList())
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Export MonkeySpeak descriptions
        ''' </summary>
        Public Function Export() As Page

            ' Console.WriteLine("Execute (0:0)")

            MSEngine.LoadFromString("")
            LoadLibrary(False, True)

            Return Me
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
            SetTriggerHandler(TriggerCategory.Cause, 0,
         Function()
             Return True
         End Function, "(0:0) When the bot starts,")

            LoadSysLibrary()

#If CONFIG = "Release" Then
            '(5:110) load library from file {...}.
            RemoveTriggerHandler(TriggerCategory.Effect, 110)
#ElseIf CONFIG = "Debug" Then
            '(5:105) raise an error.
            RemoveTriggerHandler(TriggerCategory.Effect, 105)
            SetTriggerHandler(TriggerCategory.Effect, 105,
     Function()
         Return False
     End Function, "(5:105) raise an error.")
#End If

            LoadTimerLibrary()
            LoadStringLibrary()
            LoadMathLibrary()

            For Each Library As Monkeyspeak.Libraries.AbstractBaseLibrary In LibList
                Try
                    LoadLibrary(Library)
                    If Not silent Then Console.WriteLine(String.Format("Loaded Monkey Speak Library: {0}", Library.GetType().Name))
                Catch ex As Exception
                    Throw New MonkeyspeakException(Library.GetType().Name + " " + ex.Message, ex)
                    Return Nothing
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
            Return Me
        End Function

        Public Sub PageSetVariable(ByVal VariableList As Dictionary(Of String, Object))

            For Each kv As KeyValuePair(Of String, Object) In VariableList
                SetVariable(kv.Key.ToUpper, kv.Value, True)
            Next '

        End Sub

        ''' <summary>
        ''' Start the Monkey Speak Engine
        ''' </summary>
        ''' <returns></returns>
        Public Function Start() As Page

            Dim TimeStart = DateTime.Now
            Dim VariableList As New Dictionary(Of String, Object)

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
            Execute(0)
            Console.WriteLine(String.Format("Done!!! Executed {0} triggers in {1} seconds.",
                                            Size, Date.Now.Subtract(TimeStart).Seconds))
            Return Me
        End Function

        Public Function [Stop]() As Page
            Reset(True)
            Return Me
        End Function

#End Region

    End Class

End Namespace