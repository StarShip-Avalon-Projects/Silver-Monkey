Imports Monkeyspeak.Logging

Namespace Engine

    Public Class MsLogger : Implements ILogOutput

        Public Sub Log(logMsg As LogMessage) Implements ILogOutput.Log
            Select Case logMsg.Level
                Case Level.Debug

                Case Level.Error
                Case Level.Info
                Case Level.Warning
                Case Else

            End Select
        End Sub

    End Class

End Namespace