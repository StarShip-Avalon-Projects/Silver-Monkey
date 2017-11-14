Imports System.Diagnostics
Imports System.Text
Imports Monkeyspeak.Logging

Namespace Engine

    Public Class MsLogger
        Implements ILogOutput
        Private Shared ReadOnly startTime As New DateTime()

        Public Sub New()
        End Sub

        Protected Function BuildMessage(ByRef msg As LogMessage) As LogMessage
            Dim level = msg.Level
            Dim text = msg.message
            Dim sb = New StringBuilder()
            With sb
                sb.Append("["c).Append(level.ToString().ToUpper())
                .Append("]"c)
                .Append("Thread+" + msg.Thread.ManagedThreadId.ToString)
                .Append(" "c)
                .Append(msg.TimeStamp.ToString("dd-MMM-yyyy"))
                .Append(" "c)
                .Append((msg.TimeStamp - Process.GetCurrentProcess().StartTime).ToString("hh\:mm\:ss\:fff"))
                .Append(" - ")
                .Append(text)
            End With
            msg.message = sb.ToString()
            Return msg
        End Function

        Public Overridable Sub Log(logMsg As LogMessage) Implements ILogOutput.Log
            If logMsg.message Is Nothing Then
                Return
            End If

            logMsg = BuildMessage(logMsg)
            Dim msg = logMsg.message
            Try
                Dim original As ConsoleColor = Console.ForegroundColor
                Dim color As ConsoleColor = ConsoleColor.White
                Select Case logMsg.Level
                    Case Level.Debug, Level.Warning
                        color = ConsoleColor.Yellow
                        Exit Select

                    Case Level.[Error]
                        color = ConsoleColor.Red
                        Exit Select

                    Case Level.Info
                        color = ConsoleColor.White
                        Exit Select
                End Select
                Console.ForegroundColor = color
            Catch
            End Try
            If Debugger.IsAttached Then
                Debug.WriteLine(msg)
            End If
            If Main.DebugWindow IsNot Nothing Then
                SendLogsToDemugWindow(msg)
            End If
            If logMsg.Level = Level.Info Then
                Main.SndDisplay(msg)
            End If
            Try
                Console.ResetColor()
            Catch
            End Try
        End Sub

        ''' <summary>
        ''' Send text to DebugWindow Delegate
        ''' </summary>
        ''' <param name="text"></param>
        Public Delegate Sub SendTextDelegate(text As String)

        Public Sub SendLogsToDemugWindow(Log As String)
            If Main.DebugWindow.ErrorLogTxtBx.InvokeRequired Then
                Main.DebugWindow.ErrorLogTxtBx.Invoke(New SendTextDelegate(AddressOf SendLogsToDemugWindow), Log)
            Else
                Main.DebugWindow.ErrorLogTxtBx.AppendText(Log + Environment.NewLine)
            End If
        End Sub

    End Class

End Namespace