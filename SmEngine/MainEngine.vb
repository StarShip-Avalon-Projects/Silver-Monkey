Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Win32.SafeHandles
Imports MonkeyCore
Imports Monkeyspeak

Namespace Engine

    ''' <summary>
    ''' Silver Monkey's MonkeySpeak Engine with our Customizations
    ''' </summary>
    Public Class MainEngine : Inherits MonkeyspeakEngine
        Implements IDisposable

#Region "Private Fields"

        ''' <summary>
        ''' Custome Options for this Engine
        ''' </summary>
        Private SilverMonkeyEngineOptions As EngineOptoons

        ''' <summary>
        ''' </summary>
        ''' <returns>
        ''' </returns>
        Public Shadows Property Options() As EngineOptoons
            Get
                Return SilverMonkeyEngineOptions
            End Get
            Set(ByVal value As EngineOptoons)
                SilverMonkeyEngineOptions = value
            End Set
        End Property

#End Region

#Region "Const"

        Private Const MS_Footer As String = "*Endtriggers* 8888 *Endtriggers*"
        Private Const MS_Header As String = "*MSPK V04.00 Silver Monkey"

#End Region

#Region "MonkeySpeakEngine"

        Public EngineRestart As Boolean = False

        Public FurcadiaSession As BotSession
        Public MS_Engine_Running As Boolean = False

        Private Const RES_MS_begin As String = "*MSPK V"

        Private Const RES_MS_end As String = "*Endtriggers* 8888 *Endtriggers*"

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

        End Sub

        ''' <summary>
        ''' Wrapper Functions to read a Monkey Speak Script File and Pass
        ''' the result to <see cref="LoadFromString"/>
        ''' </summary>
        ''' <param name="file">
        ''' MonkeySpeak filename
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function LoadFromScriptFile(ByVal file As String) As Monkeyspeak.Page
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

        'Public Sub LogError(reader As TriggerReader, ex As Exception)

        ' Console.WriteLine(MS_ErrWarning) Dim ErrorString As String =
        ' "Error: (" & reader.TriggerCategory.ToString & ":" &
        ' reader.TriggerId.ToString & ") " & ex.Message

        '    If Not IsNothing(cBot) Then
        '        If cBot.log Then
        '            LogStream.WriteLine(ErrorString, ex)
        '        End If
        '    End If
        '    Writer.WriteLine(ErrorString)
        'End Sub
        'Public Sub LogError(trigger As Trigger, ex As Exception) Handles MonkeySpeakPage.Error

        ' Console.WriteLine(MS_ErrWarning) Dim ErrorString As String =
        ' "Error: (" & trigger.Category.ToString & ":" & trigger.Id.ToString
        ' & ") " & ex.Message

        '    If Not IsNothing(cBot) Then
        '        If cBot.log Then
        '            '  BotLogStream.WriteLine(ErrorString, ex)
        '        End If
        '    End If
        '    Writer.WriteLine(ErrorString)
        'End Sub

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
            disposed = True
        End Sub

#End Region

    End Class

End Namespace