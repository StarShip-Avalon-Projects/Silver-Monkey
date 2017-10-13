Imports System.IO
Imports MonkeyCore
Imports MonkeyCore.Utils.Logging

Namespace My

    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

#Region "Private Methods"

        Private Sub MyApplication_UnhandledException(sender As Object, e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Dim logError As New ErrorLogging(e.Exception, sender)
            Dim args As String = logError.LogFile
            Dim Proc As String = "notepad.exe"
            Process.Start(Proc, args)
        End Sub

#End Region

    End Class

End Namespace