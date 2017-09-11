Public NotInheritable Class SplashScreen1

#Region "Private Delegates"

    Private Delegate Sub CloseDelegate()

    Private Delegate Sub UpdateProgressDelegate(ByVal msg As String, ByVal percentage As Double)

#End Region

#Region "Public Methods"

    Public Overloads Sub Close()
        If Me.InvokeRequired Then
            Me.Invoke(New CloseDelegate(AddressOf Close))
        Else
            Me.Hide()
        End If
    End Sub

    Public Sub UpdateProgress(ByVal msg As String, ByVal percentage As Double)
        If Me.InvokeRequired Then
            Me.Invoke(New UpdateProgressDelegate(AddressOf UpdateProgress), New Object() {msg, percentage})
        Else
            Me.Label2.Text = msg
            If percentage >= Me.Status.Minimum AndAlso percentage <= Me.Status.Maximum Then
                Me.Status.Value = CInt(percentage)
            End If
        End If
    End Sub

#End Region

#Region "Private Methods"

    Private Sub frmSplashScreen_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        ' Draw a Black Border around the Borderless Form:
        Dim rc As New Rectangle(0, 0, Me.ClientRectangle.Width - 1, Me.ClientRectangle.Height - 1)
        e.Graphics.DrawRectangle(Pens.Black, rc)
    End Sub

    Private Sub SplashScreen1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Set up the dialog text at runtime according to the application's assembly information.

        'Application title
        If My.Application.Info.Title <> "" Then
            ApplicationTitle.Text = "MonkeySpeak Editor"
        Else
            'If the application title is missing, use the application name, without the extension
            ApplicationTitle.Text = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.TopMost = True
        'Format the version information using the text set into the Version control at design time as the
        '  formatting string.  This allows for effective localization if desired.
        '  Build and revision information could be included by using the following code and changing the
        '  Version control's designtime text to "Version {0}.{1:00}.{2}.{3}" or something similar.  See
        '  String.Format() in Help for more information.
        '
        '    Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)

        Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)

        'Copyright info
        Copyright.Text = My.Application.Info.Copyright
    End Sub

#End Region

End Class