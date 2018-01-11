Imports System.Diagnostics
Imports Logging
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.Devices
Imports MonkeyCore2.Logging

Namespace My

    ' The following events are available for MyApplication:
    '
    ' Startup: Raised when the application starts, before the startup form
    '          is created.
    ' Shutdown: Raised after all application forms are closed. This event is
    '           not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance
    '                      application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is
    '                             connected or disconnected.
    Partial Friend Class MyApplication

#Region "Private Methods"

        Private Sub MyApplication_NetworkAvailabilityChanged(sender As Object, e As NetworkAvailableEventArgs) Handles Me.NetworkAvailabilityChanged

        End Sub

        Private Sub MyApplication_Shutdown(sender As Object, e As EventArgs) Handles Me.Shutdown

        End Sub

        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup

        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Dim ex = e.Exception
            Dim ErrorLog = New ErrorLogging(ex, Me)
            Dim report = New BugReport(ErrorLog) With {
                .ProjectName = "MonkeyCore2Tests"
            }
            Dim ps = New ProcessStartInfo(BugReport.ToolAppName) With
                {
                    .Arguments = report.ToCommandLineArgs()
                }
            Process.Start(ps)
        End Sub

#End Region

    End Class

End Namespace