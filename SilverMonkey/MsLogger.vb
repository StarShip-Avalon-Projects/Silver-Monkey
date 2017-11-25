Imports System.Diagnostics
Imports System.Text
Imports Monkeyspeak.Logging
Imports SilverMonkey.HelperClasses.TextDisplayManager

Namespace Engine

    Public Class MsLogger
        Implements ILogOutput
        '    Private Shared ReadOnly startTime As New DateTime()

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
            If Debugger.IsAttached Then
                Debug.WriteLine(logMsg.message)
            End If
            If Main.DebugWindow IsNot Nothing Then
                Variables.SendLogsToDemugWindow(logMsg)
            End If
            Select Case logMsg.Level
                Case Level.Info Or Level.Error
                    Main.SndDisplay(logMsg)
            End Select

        End Sub

    End Class

End Namespace