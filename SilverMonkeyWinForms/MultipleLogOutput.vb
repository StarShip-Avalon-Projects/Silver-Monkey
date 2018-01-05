Imports System.Diagnostics
Imports System.Text

Namespace Engine

    Public Enum Level As Byte
        Info = 1
        Warning = 2
        [Error] = 3
        Debug = 4
    End Enum

    Public Class MultipleLogOutput
        Implements Furcadia.Logging.ILogOutput
        Implements Monkeyspeak.Logging.ILogOutput

        Public Sub New()
        End Sub

        Protected Function BuildMessage(ByRef msg As Furcadia.Logging.LogMessage) As Furcadia.Logging.LogMessage
            Dim level = msg.Level
            Dim text = msg.Message
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
            msg.Message = sb.ToString()
            Return msg
        End Function

        Public Sub Log(logMsg As Furcadia.Logging.LogMessage) Implements Furcadia.Logging.ILogOutput.Log
            If logMsg.Message Is Nothing Then
                Return
            End If

            logMsg = BuildMessage(logMsg)
            If Debugger.IsAttached Then
                Debug.WriteLine(logMsg.Message)
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

        Protected Function BuildMessage(ByRef msg As Monkeyspeak.Logging.LogMessage) As Monkeyspeak.Logging.LogMessage
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

        Public Overridable Sub Log(logMsg As Monkeyspeak.Logging.LogMessage) Implements Monkeyspeak.Logging.ILogOutput.Log
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

    End Class

End Namespace