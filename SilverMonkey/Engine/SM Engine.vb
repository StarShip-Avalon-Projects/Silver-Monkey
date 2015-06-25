Imports SilverMonkey.ConfigStructs
Imports System.IO
Imports System.Text
Imports Nini.Config

Public Class SM_Engine
#Region "MonkeySpeakEngine"
    Dim Lock As New Object
    Public cBot As cBot
    Public Shared Function MS_Started() As Boolean
        ' 0 = main load
        ' 1 = engine start
        ' 2 = engine running
        If MS_Stared = 2 Then Return True Else Return False
    End Function
    Public Shared MS_Stared As Integer = 0

    Public Shared MS_Engine_Running As Boolean = False
    Public engine = New Monkeyspeak.MonkeyspeakEngine()

    Public Shared MSpage As Monkeyspeak.Page
    Public Shared EngineRestart As Boolean = False


    Public Sub New()
        MSpage = engine.LoadFromString("")
        LoadLibrary()
    End Sub
    'Bot Starts
    Public Sub ScriptStart()
        Try

            If Not cBot.MS_Engine_Enable Then Exit Sub
            Console.WriteLine("Loading:" & cBot.MS_File)
            Dim start = DateTime.Now
            cBot.MS_Script = msReader(mPath() & "/", cBot.MS_File, True)
            'Dim msReader As SilverMonkey.msUtils
            If cBot.MS_Script = "" Or IsNothing(cBot.MS_Script) Then
                Console.WriteLine("ERROR: No script loaded! Aborting")
                MS_Engine_Running = False
                Exit Sub
            End If
            MS_Stared = 1
            LoadLibrary()
            MSpage = engine.LoadFromString(True, SM_Engine.MSpage, cBot.MS_Script)
            ' Console.WriteLine("Execute (0:0)")

            PageSetVariable(Main.VarPrefix & "msPath", mPath())
            PageExecute(0)
            Console.WriteLine("Done! Executed in " & DateTime.Now.Subtract(start).ToString())
            'Console.ReadKey()
            MS_Engine_Running = True
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub

    'loads at main load
    Public Sub EngineStart()
        Try
            MSpage = engine.LoadFromString("")
            LoadLibrary()
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub

    Public Sub LoadLibrary(Optional ByRef start As Boolean = False)
        'Library Loaded?.. Get the Hell out of here

        If MS_Started() Then Exit Sub
        'MS_Engine.callbk = Me
        MS_Stared += 1
        MSpage.Reset(True)
        MSpage.SetTriggerHandler(Monkeyspeak.TriggerCategory.Cause, 0,
             Function()
                 'Console.WriteLine("Trigger (0:0)")
                 'System.Diagnostics.Debug.Print("Trigger (0:0)")
                 Return True
             End Function, "(0:0) When the bot starts,")
        Try
            'Console.WriteLine("Loading Sys  Library")
            MSpage.LoadSysLibrary()
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try

        Try
            'Console.WriteLine("Loading IO  Library")
            MSpage.LoadIOLibrary()
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
        Try
            MSpage.LoadTimerLibrary()
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
        Try

            'Console.WriteLine("Loading Default Math  Library")
            MSpage.LoadMathLibrary()
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
        'Console.WriteLine("Loading Test  Library")
        Try
            MSpage.LoadLibrary(New StringLibrary()) ' Load our new TestLibrary
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
        'Console.WriteLine("Loading Say  Library")
        Try
            MSpage.LoadLibrary(New SayLibrary())
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try

        'Banish Library
        Try
            MSpage.LoadLibrary(New Banish())
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
        'Console.WriteLine("Loading New Math  Library")
        Try
            MSpage.LoadLibrary(New MathLibrary())
            'Define our Triggers before we use them
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try

        Try
            MSpage.LoadLibrary(New MS_Time())
            'Define our Triggers before we use them
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
        'Console.WriteLine("Loading New Math  Library")
        Try
            MSpage.LoadLibrary(New MSPK_MDB())
            'Define our Triggers before we use them
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)

        End Try
        Try
            MSpage.LoadLibrary(New MSPK_Web())
            'Define our Triggers before we use them
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MS_Cookie())
            'Define our Triggers before we use them
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
        Try
            MSpage.LoadLibrary(New MS_Dice())
            'Define our Triggers before we use them
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
    End Sub

    Private msVer As Double = 2.0
    Public Function msReader(ByVal path As String, ByVal file As String, ByVal E_Mode As Boolean) As String

        Dim Data As String = String.Empty
        Try
            If Not System.IO.File.Exists(path & file) Then
                Return ""
            End If
            Dim line As String = ""
            Using objReader As New StreamReader(path & file)
                ' line = objReader.ReadLine() & Environment.NewLine
                While Not objReader.EndOfStream
                    line = objReader.ReadLine() & Environment.NewLine
                    If E_Mode = True Then
                        If line.Equals(NewMSFileFile.Header) Then
                            ' If line.Equals("*MSPK V01.00 Silver Monkey") Then
                            'Version Header
                            'line = ""
                        ElseIf line = NewMSFileFile.Footer Then
                            'ElseIf line = "*Endtriggers* 8888 *Endtriggers*" Then
                            'remainder of the file are Notes
                            Data += line
                            Exit While
                        End If
                        Data += line
                    ElseIf Not E_Mode Then
                        'Clean Data up for editor
                        Data += line
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
            If EngineRestart Then Exit Sub
            Try
                MSpage.SetVariable(Main.VarPrefix & varName, data, True) '
            Catch ex As Exception
                Dim logError As New ErrorLogging(ex, Me)
            End Try
            Try
                If Variables.ChkBxRefresh.CheckState = True And Variables.Visible Then
                    Variables.updateVariables()
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Public Sub PageSetVariable(ByVal varName As String, ByVal data As Object, ByVal Constant As Boolean)
        If cBot.MS_Engine_Enable AndAlso MS_Started() Then
            If EngineRestart Then Exit Sub
            Try
                MSpage.SetVariable(Main.VarPrefix & varName, data, Constant) '
            Catch ex As Exception
                Dim logError As New ErrorLogging(ex, Me)
            End Try
        End If
    End Sub

    Public Sub PageExecute(ByVal TriggerCause As Integer)
        If cBot.MS_Engine_Enable AndAlso MS_Started() Then
            If EngineRestart Then Exit Sub
            '  MSpage.Execute(Monkeyspeak.TriggerCategory.Cause, TriggerCause)
            SyncLock Lock
                MSpage.Execute(TriggerCause)
            End SyncLock
        End If
    End Sub

#End Region

#Region "New MS File Settings"
    Public Shared EditIni As IniConfigSource
    'Shared LocalEditIni As IniConfigSource
    '[C-General]
    'Header=*MSPK V01.00 Silver Monkey
    'Footer=*Endtriggers* 8888 *Endtriggers*
    'H0=*Silver Monkey MonkeySpeak Script File
    'H1=*Created by <name> 
    'HMax=2
    'InitLineSpaces=6

    Public Structure NewMSFileFile
        Shared _Header As String
        Shared _footer As String
        Shared _MSFile As String
        Shared _Lines As Integer
        Shared _InitLineSpaces As Integer
        Shared ReadOnly Property Header As String
            Get
                Try
                    Return EditIni.Configs("C-General").Get("Header", "*MSPK V02.00 Silver Monkey")
                Catch
                    Return "*MSPK V02.00 Silver Monkey"
                End Try
            End Get
        End Property
        Shared ReadOnly Property Footer As String
            Get
                Try
                    Return EditIni.Configs("C-General").Get("Footer", "*Endtriggers* 8888 *Endtriggers*")
                Catch
                    Return "*Endtriggers* 8888 *Endtriggers*"
                End Try

            End Get
        End Property
        Shared ReadOnly Property MSFile As String
            Get
                'header
                _MSFile = Header & vbCrLf

                Try
                    _Lines = EditIni.Configs("C-General").GetInt("HMax", 2)
                    For i As Integer = 0 To _Lines - 1
                        Dim str = "H" + i.ToString
                        _MSFile += EditIni.Configs("C-General").Get(str) & vbCrLf
                    Next
                Catch
                    _MSFile += "*Silver Monkey MonkeySpeak Script File" & vbCrLf
                    _MSFile += "*Created by <name> " & vbCrLf
                End Try
                'Blank Lines
                Try
                    _InitLineSpaces = EditIni.Configs("C-General").GetInt("InitLineSpaces", 6)
                Catch
                    _InitLineSpaces = 6
                End Try
                For i As Integer = 0 To _InitLineSpaces - 1
                    _MSFile = _MSFile & Trim(vbCrLf)
                Next

                'Footer
                _MSFile += Footer
                Return _MSFile
            End Get

        End Property
    End Structure

#End Region
End Class
