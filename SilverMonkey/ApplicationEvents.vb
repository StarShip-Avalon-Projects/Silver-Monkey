Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.Devices
Imports System.Windows.Forms
Imports MonkeyCore
Imports SilverMonkey.BugTraqConnect

Namespace My
    ' The following events are available for MyApplication:
    '
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private Sub MyApplication_NetworkAvailabilityChanged(sender As Object, e As NetworkAvailableEventArgs) Handles Me.NetworkAvailabilityChanged

        End Sub

        Private Sub MyApplication_Shutdown(sender As Object, e As EventArgs) Handles Me.Shutdown

        End Sub

        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup

        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Exit Sub
            Dim logError As New ErrorLogging(e.Exception, sender)
            Dim SubmitError As New SubmitIssueForm(logError.LogFile)
            'Dim dialog As DialogResult = MessageBox.Show("An error log has been saved to" + logError.LogFile + " Press Ok To continue ", "Unhandled Exception", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'e.ExitApplication = (dialog = DialogResult.Cancel)
            If SubmitError.ShowDialog = DialogResult.OK Then
                File.Delete(logError.LogFile)
            End If
        End Sub
    End Class

End Namespace