Imports System.IO
Imports MonkeyCore
Imports Monkeyspeak


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
        If MS_Stared = 2 Then Return True Else Return False
    End Function
    Public MS_Stared As Integer = 0
    Private Const RES_MS_begin As String = "*MSPK V"
    Private Const RES_MS_end As String = "*Endtriggers* 8888 *Endtriggers*"

    Public EngineRestart As Boolean = False

    Public MS_Engine_Running As Boolean = False
    Public engine As MonkeyspeakEngine = New MonkeyspeakEngine()
    Public Shared WithEvents MSpage As Page = Nothing
    Public Sub New()
        EngineStart()
    End Sub
    'Bot Starts
    Public Sub ScriptStart()
        Try

            MSpage = engine.LoadFromString("")
            ' Console.WriteLine("Execute (0:0)")
            MS_Stared = 1
            LoadLibrary()

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
        MS_Stared += 1

        Dim objPlugin As SilverMonkey.Interfaces.msPlugin

        'Loop through available plugins, creating instances and adding them to listbox

        For intIndex As Integer = 0 To Main.Plugins.Count - 1
            MSpage = engine.LoadFromString("")
            objPlugin = CType(PluginServices.CreateInstance(Main.Plugins(intIndex)), SilverMonkey.Interfaces.msPlugin)
            objPlugin.Initialize(Main.objHost)
            objPlugin.Page = MSpage
            objPlugin.Start()
        Next


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

        Try
            MSpage.SetVariable("%" & varName.ToUpper, data, True) '
        Catch ex As Exception
            Dim logError As New ErrorLogging(ex, Me)
        End Try

    End Sub
    Public Sub PageSetVariable(ByVal varName As String, ByVal data As Object, ByVal Constant As Boolean)
        Try
            MSpage.SetVariable("%" & varName.ToUpper, data, Constant) '
        Catch ex As Exception
            Dim logError As New ErrorLogging(ex, Me)
        End Try

    End Sub

    Public Sub PageExecute(ParamArray ID() As Integer)

        MSpage.Execute(ID)

    End Sub

    Public Shared Sub MS_Error(trigger As Trigger, ex As Exception) Handles MSpage.Error

        Console.WriteLine("Error, See Debug Window")
        Dim ErrorString As String = "Error: (" & trigger.Category.ToString & ":" & trigger.Id.ToString & ") " & ex.Message
    End Sub



#End Region

End Class
