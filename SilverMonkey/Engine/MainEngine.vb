Imports System.IO
Imports System.Text
Imports SilverMonkey.ConfigStructs
Imports Monkeyspeak

Imports System.Diagnostics
Imports System.Collections
Imports System.Collections.Generic

Public Class MainEngine


#Region "Const"
    Private Const MS_Header As String = "*MSPK V04.00 Silver Monkey"
    Private Const MS_Footer As String = "*Endtriggers* 8888 *Endtriggers*"
#End Region

#Region "MonkeySpeakEngine"
    Public Function MS_Started() As Boolean
        ' 0 = main load
        ' 1 = engine start
        ' 2 = engine running
        Return MS_Stared >= 2
    End Function
    Public MS_Stared As Integer = 0
    Private Shared Writer As TextBoxWriter = New TextBoxWriter(Variables.TextBox1)
    Private Const RES_MS_begin As String = "*MSPK V"
    Private Const RES_MS_end As String = "*Endtriggers* 8888 *Endtriggers*"

    Public EngineRestart As Boolean = False

    Public MS_Engine_Running As Boolean = False
    Public engine As Monkeyspeak.MonkeyspeakEngine = New Monkeyspeak.MonkeyspeakEngine()
    Public Shared WithEvents MSpage As Monkeyspeak.Page = Nothing
    Public Sub New()
        EngineStart(True)
    End Sub
    'Bot Starts
    Public Function ScriptStart() As Boolean
        Try

            If Not cBot.MS_Engine_Enable Then Exit Function
            Console.WriteLine("Loading:" & cBot.MS_File)
            Dim start As DateTime = DateTime.Now
            cBot.MS_Script = msReader(CheckMyDocFile(cBot.MS_File))
            Dim p As String = mPath()
            If String.IsNullOrEmpty(cBot.MS_Script) Then
                Console.WriteLine("ERROR: No script loaded! Aborting")
                MS_Engine_Running = False
                Return False
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
            Dim VariableList As New Dictionary(Of String, Object)
            VariableList.Add("MSPATH", mPath())
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
        MSpage.SetTriggerHandler(Monkeyspeak.TriggerCategory.Cause, 0,
             Function()
                 Return True
             End Function, "(0:0) When the bot starts,")
        Try
            MSpage.LoadSysLibrary()
#If Config = "Release" Then
            '(5:105) raise an error.
            MSpage.RemoveTriggerHandler(TriggerCategory.Effect, 105)
            '(5:110) load library from file {...}.
            MSpage.RemoveTriggerHandler(TriggerCategory.Effect, 110)
#ElseIf Config = "Debug" Then

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
            MSpage.LoadLibrary(New phoenixspeak())
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

        'Loop through available plugins, creating instances and adding them to listbox
        If Not Plugins Is Nothing And LoadPlugins Then
            Dim objPlugin As SilverMonkey.Interfaces.msPlugin
            Dim newPlugin As Boolean = False
            For intIndex As Integer = 0 To Plugins.Length - 1
                Try
                    objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(intIndex)), SilverMonkey.Interfaces.msPlugin)
                    If Not cMain.PluginList.ContainsKey(objPlugin.Name.Replace(" ", "")) Then
                        cMain.PluginList.Add(objPlugin.Name.Replace(" ", ""), True)
                        newPlugin = True
                    End If

                    If cMain.PluginList.Item(objPlugin.Name.Replace(" ", "")) = True Then
                        Console.WriteLine("Loading Plugin: " + objPlugin.Name)
                        objPlugin.Initialize(Main.objHost)
                        objPlugin.Page = MSpage
                        objPlugin.Start()
                    End If
                Catch ex As Exception
                    Dim e As New ErrorLogging(ex, Me)
                End Try
            Next
            If newPlugin Then cMain.SaveMainSettings()


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
            Dim logError As New ErrorLogging(eX, Me)
            Return ""
        End Try
    End Function

    Public Sub PageSetVariable(ByVal varName As String, ByVal data As Object)
        If cBot.MS_Engine_Enable AndAlso MS_Started() Then
            Try
                MSpage.SetVariable(Main.VarPrefix & varName.ToUpper, data, True) '
            Catch ex As Exception
                Dim logError As New ErrorLogging(ex, Me)
            End Try
        End If
    End Sub

    Public Sub PageSetVariable(ByVal VariableList As Dictionary(Of String, Object))
        If cBot.MS_Engine_Enable Then
            Try
                For Each kv As KeyValuePair(Of String, Object) In VariableList
                    MSpage.SetVariable(Main.VarPrefix & kv.Key.ToUpper, kv.Value, True)
                Next '
            Catch ex As Exception
                Dim logError As New ErrorLogging(ex, Me)
            End Try
        End If
    End Sub

    Public Sub PageSetVariable(ByVal varName As String, ByVal data As Object, ByVal Constant As Boolean)
        If Not IsNothing(cBot) Then
            If cBot.MS_Engine_Enable AndAlso MS_Started() Then
                Try
                    MSpage.SetVariable(Main.VarPrefix & varName.ToUpper, data, Constant) '
                Catch ex As Exception
                    Dim logError As New ErrorLogging(ex, Me)
                End Try
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

    Public Shared Sub MS_Error(trigger As Monkeyspeak.Trigger, ex As Exception) Handles MSpage.Error

        Console.WriteLine(MS_ErrWarning)
        Dim ErrorString As String = "Error: (" & trigger.Category.ToString & ":" & trigger.Id.ToString & ") " & ex.Message
        writer.WriteLine(ErrorString)
    End Sub

#End Region

End Class
