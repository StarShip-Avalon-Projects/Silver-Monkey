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
    Public Class MainEngine
        Implements IDisposable

#Region "Private Fields"

        ''' <summary>
        ''' Custome Options for this Engine
        ''' </summary>
        Private SilverMonkeyEngineOptions As EngineOptoons

        Private SmEngine As Monkeyspeak.MonkeyspeakEngine

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

        Public FurcadiaSession As BotSession

        Private Const RES_MS_begin As String = "*MSPK V"

        Private Const RES_MS_end As String = "*Endtriggers* 8888 *Endtriggers*"

        ''' <summary>
        ''' Default Constructlor.
        ''' <para>
        ''' This Loads our MonkeyBeak Libraries
        ''' </para>
        ''' </summary>
        Public Sub New(ByRef Options As EngineOptoons, ByRef FurcSession As BotSession)
            SilverMonkeyEngineOptions = Options
            FurcadiaSession = FurcSession
            SmEngine = New MonkeyspeakEngine(Options)
        End Sub

        ''' <summary>
        ''' Wrapper Functions to read a Monkey Speak Script File and Pass
        ''' the result to <see cref="MonkeySpeakEngine.LoadFromString"/>
        ''' </summary>
        ''' <param name="file">
        ''' MonkeySpeak filename
        ''' </param>
        ''' <exception cref="FileNotFoundException"/>
        ''' <returns>
        ''' </returns>
        Public Function LoadFromScriptFile(ByVal file As String) As Page
            Dim MonkeySpeakScript = New StringBuilder()
            Try

                If Not System.IO.File.Exists(Paths.CheckBotFolder(file)) Then
                    Throw New FileNotFoundException("MonkeySpeak file (" + file + ") not found. Did you forget to define on or check the file path?")
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
                End Using
                Return SmEngine.LoadFromString(MonkeySpeakScript.ToString())
            Catch eX As Exception
                Throw eX
            End Try

            Return Nothing

        End Function

        ''' <summary>
        ''' Loads a Monkeyspeak script from a string into a <see cref="Monkeyspeak.Page"/>.
        ''' </summary>
        ''' <param name="MonkeySpeakScript">MonkeySpeak as string</param>
        ''' <returns></returns>
        Public Function LoadFromString(MonkeySpeakScript As String) As Page
            Return SmEngine.LoadFromString(MonkeySpeakScript)
        End Function

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

        End Sub

        ' Protected implementation of Dispose pattern.
        Protected Overridable Sub Dispose(disposing As Boolean)
            If disposed Then Return

            If disposing Then

                handle.Dispose()

            End If

            ' Free any unmanaged objects here.
            disposed = True
        End Sub

#End Region

    End Class

End Namespace