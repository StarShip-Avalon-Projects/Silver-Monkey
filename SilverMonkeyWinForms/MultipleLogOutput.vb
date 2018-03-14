Imports System.Diagnostics
Imports System.Text
Imports FurcLog = Furcadia.Logging
Imports MsLog = Monkeyspeak.Logging
Imports MonkeyCore.Logging

Namespace Engine

    Public Class MultipleLogOutput
        Implements ILogOutput

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

        Public Sub Log(logMsg As LogMessage) Implements ILogOutput.Log
            If logMsg.message Is Nothing Then
                Return
            End If

            logMsg = BuildMessage(logMsg)
            If Debugger.IsAttached Then
                Debug.WriteLine(logMsg.message)
            End If
            If Main.DebugWindow IsNot Nothing Then
                Main.DebugWindow.SendLogsToDemugWindow(logMsg)
            End If
            If logMsg.Level = Level.Info _
                    Or logMsg.Level = Level.Error _
                    Or logMsg.Level = Level.Warning Then
                Main.SndDisplay(logMsg)
            End If

        End Sub

        Public Sub Log(logMsg As MsLog.LogMessage) Implements MsLog.ILogOutput.Log
            Log(logMsg)
        End Sub

        Public Sub Log(logMsg As FurcLog.LogMessage) Implements FurcLog.ILogOutput.Log
            Log(logMsg)
        End Sub

    End Class

End Namespace