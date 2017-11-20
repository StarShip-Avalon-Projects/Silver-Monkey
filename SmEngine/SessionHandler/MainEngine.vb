Imports System.IO
Imports System.Text
Imports MonkeyCore
Imports Monkeyspeak

Namespace Engine

    ''' <summary>
    ''' Silver Monkey's MonkeySpeak Engine with our Customizations
    ''' </summary>
    Public NotInheritable Class MsEngineExtentionFunctions

#Region "Const"

        Private Const MS_Footer As String = "*Endtriggers* 8888 *Endtriggers*"
        Private Const MS_Header As String = "*MSPK V04.00 Silver Monkey"

#End Region

#Region "MonkeySpeakEngine"

        Public FurcadiaSession As BotSession

        Private Const RES_MS_begin As String = "*MSPK V"

        Private Const RES_MS_end As String = "*Endtriggers* 8888 *Endtriggers*"

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
        Public Shared Function LoadFromScriptFile(ByVal file As String) As String
            Try
                Dim ScriptContents As New StringBuilder()
                If Not System.IO.File.Exists(Paths.CheckBotFolder(file)) Then
                    Throw New FileNotFoundException($"MonkeySpeak file ({file}) not found. Did you forget to define on or check the file path?")
                End If

                Using MonkeySpeakScriptReader As New StreamReader(file)
                    Dim line As String = ""

                    While MonkeySpeakScriptReader.Peek <> -1
                        line = MonkeySpeakScriptReader.ReadLine()
                        If Not line.StartsWith(RES_MS_begin) Then
                            ScriptContents.AppendLine(line)
                        ElseIf line.StartsWith(RES_MS_begin) Then
                            'MonkeySpeak Script Version Check

                        End If

                        If line = RES_MS_end Then
                            Exit While
                        End If

                    End While
                End Using
                Return ScriptContents.ToString()
            Catch eX As Exception
                Throw eX
            End Try

            Return Nothing

        End Function

        Public Shared Async Function LoadFromScriptFileAsync(FilePath As String) As Task(Of String)
            Return Await Task.Run(Function() LoadFromScriptFile(FilePath))
        End Function

#End Region

    End Class

End Namespace