Imports MonkeyCore.Settings


Namespace My
    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Public Shared Property KeysIni As MonkeyCore.IniFile
            Get
                Return MonkeyCore.Settings.KeysIni
            End Get
            Set(value As MonkeyCore.IniFile)
                MonkeyCore.Settings.KeysIni = value
            End Set
        End Property

        Public Shared Property MS_KeysIni As MonkeyCore.IniFile
            Get
                Return MonkeyCore.Settings.MS_KeysIni
            End Get
            Set(value As MonkeyCore.IniFile)
                MonkeyCore.Settings.MS_KeysIni = value
            End Set
        End Property

        Private Sub MyApplication_NetworkAvailabilityChanged(sender As Object, e As Devices.NetworkAvailableEventArgs) Handles Me.NetworkAvailabilityChanged

        End Sub

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            ' Get the splash screen.

            Dim splash As SplashScreen1 = CType(My.Application.SplashScreen, SplashScreen1)


        End Sub

        Private Sub MyApplication_StartupNextInstance(sender As Object, e As ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            Dim filename As String = ""
            Dim BotName As String = ""
                If e.CommandLine.Count > 0 Then
                    If My.Application.CommandLineArgs.Count >= 2 Then
                        BotName = e.CommandLine.Item(0)
                    End If
                    filename = e.CommandLine.Item(e.CommandLine.Count - 1)

                    If Not String.IsNullOrEmpty(filename) And Not String.IsNullOrEmpty(BotName) Then
                        CallBk.OpenMS_File(filename, BotName)

                    ElseIf Not String.IsNullOrEmpty(filename) And String.IsNullOrEmpty(BotName) Then
                        CallBk.OpenMS_File(filename)
                    Else
                        CallBk.AddNewEditorTab(filename, "", 0)
                        CallBk.NewFile(EditStyles.ms)
                    End If
                End If

        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Dim logError As New MonkeyCore.ErrorLogging(e.Exception, sender)
            MessageBox.Show("An error log has been saved to" + logError.LogFile, "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub
    End Class


End Namespace

