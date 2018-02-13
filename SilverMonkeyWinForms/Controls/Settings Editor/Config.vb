Imports System.Drawing
Imports System.Drawing.Text
Imports System.Windows.Forms
Imports IO

Public Class Config

#Region "Public Methods"

    Public Sub GetColor(ByRef ColorBX As System.Windows.Forms.PictureBox)
        Dim dlg As New ColorDialog
        dlg.Color = ColorBX.BackColor
        If dlg.ShowDialog() = DialogResult.OK Then
            ColorBX.BackColor = dlg.Color
        End If
    End Sub

    Public Sub Loadconfig()
        TxtBx_Server.Text = Main.Mainsettings.Host
        TxtSPort.Text = Main.Mainsettings.sPort.ToString
        ChkBxAutoReconnect.Checked = Main.Mainsettings.AutoReconnect

        ChkBxAutoCloseProc.Checked = Main.Mainsettings.CloseProc
        ReconnectUpDown.Value = Main.Mainsettings.ReconnectMax
        ChkTimeStamp.Checked = CBool(Main.Mainsettings.TimeStamp)
        ' Get the installed fonts collection.
        Dim installed_fonts As New InstalledFontCollection
        ' Get an array of the system's font familiies.
        Dim font_families() As FontFamily = installed_fonts.Families()
        ' Display the font families.
        For Each font_family As FontFamily In font_families
            ComboFontFace.Items.Add(font_family.Name)
        Next (font_family)
        ComboFontFace.SelectedItem = Main.Mainsettings.ApFont.Name
        For i As Integer = 3 To 50
            ComboFontSize.Items.Add(i.ToString)
        Next
        ComboFontSize.SelectedItem = Main.Mainsettings.ApFont.Size.ToString
        NumericUpDown4.Value = Main.Mainsettings.TT_TimeOut
        ReconnectUpDown.Value = Main.Mainsettings.ReconnectMax
        EmitColorBox.BackColor = Main.Mainsettings.EmitColor
        SayColorBox.BackColor = Main.Mainsettings.SayColor
        ShoutColorBox.BackColor = Main.Mainsettings.ShoutColor
        WhisperColorBox.BackColor = Main.Mainsettings.WhColor
        DefaultColorBox.BackColor = Main.Mainsettings.DefaultColor
        EmoteColorBox.BackColor = Main.Mainsettings.EmoteColor

        TxtBxFurPath.Text = Main.Mainsettings.FurcPath
        'SysTray
        ChkBxSysTray.CheckState = Main.Mainsettings.SysTray
        NumSonnectTimeOut.Value = Main.Mainsettings.ConnectTimeOut
        NumPing.Value = Main.Mainsettings.Ping

        chkBxAdvertisment.Checked = Main.Mainsettings.Advertisment
        chkBxAnnouncement.Checked = Main.Mainsettings.Announcement
        chkBxBroadcast.Checked = Main.Mainsettings.Broadcast
        chkBxAutoLoadLastBotFile.Checked = Main.Mainsettings.LoadLastBotFile
        chkBxClientDisconnectToggle.Checked = Main.Mainsettings.DisconnectPopupToggle

        CheckBox1.Checked = Main.Mainsettings.PSShowClient
        'CheckBox2.Checked = Main.MainSettings.PSShowSettingsWindow
    End Sub

#End Region

#Region "Private Methods"

    Private Sub BTN_Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_Cancel.Click
        'Close with out Saving
        Me.Close()
    End Sub

    Private Sub BTN_Ok_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_Ok.Click
        'apply the Settings
        Main.Mainsettings.Host = TxtBx_Server.Text
        Main.Mainsettings.sPort = Convert.ToInt32(TxtSPort.Text)

        Main.Mainsettings.ReconnectMax = CInt(ReconnectUpDown.Value)
        Main.Mainsettings.TT_TimeOut = CInt(NumericUpDown4.Value)
        Main.Mainsettings.TimeStamp = CUShort(ChkTimeStamp.CheckState)

        Dim face As String = ComboFontFace.SelectedItem.ToString
        Dim size As Integer = Convert.ToInt32(ComboFontSize.SelectedItem)
        Main.Mainsettings.ApFont = New Font(face, size)

        Main.Mainsettings.CloseProc = ChkBxAutoCloseProc.Checked
        Main.Mainsettings.EmitColor = EmitColorBox.BackColor
        Main.Mainsettings.SayColor = SayColorBox.BackColor
        Main.Mainsettings.ShoutColor = ShoutColorBox.BackColor
        Main.Mainsettings.WhColor = WhisperColorBox.BackColor
        Main.Mainsettings.DefaultColor = DefaultColorBox.BackColor

        Main.Mainsettings.AutoReconnect = ChkBxAutoReconnect.Checked
        Main.Mainsettings.SysTray = ChkBxSysTray.CheckState
        Main.Mainsettings.ReconnectMax = CInt(ReconnectUpDown.Value)
        Main.Mainsettings.ConnectTimeOut = CInt(NumSonnectTimeOut.Value)
        Main.Mainsettings.FurcPath = TxtBxFurPath.Text
        Main.Mainsettings.Ping = CInt(NumPing.Value)

        Main.Mainsettings.Advertisment = chkBxAdvertisment.Checked
        Main.Mainsettings.Announcement = chkBxAnnouncement.Checked
        Main.Mainsettings.Broadcast = chkBxBroadcast.Checked
        Main.Mainsettings.LoadLastBotFile = chkBxAutoLoadLastBotFile.Checked
        Main.Mainsettings.DisconnectPopupToggle = chkBxClientDisconnectToggle.Checked
        'Save the settings to the ini file

        Main.Mainsettings.PSShowClient = CheckBox1.Checked
        'Main.MainSettings.PSShowSettingsWindow = CheckBox2.Checked

        IO.Paths.FurcadiaProgramFolder = Main.Mainsettings.FurcPath
        Main.Mainsettings.SaveMainSettings()

        'Settings.InitializeTextControls()
        Me.Dispose()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        With FindFurc
            Dim ExePath As String
            .FileName = "Furcadia.exe"

            If Environment.Is64BitOperatingSystem Then
                ExePath = Environment.GetEnvironmentVariable("ProgramFiles(x86)")
            Else
                ExePath = Environment.GetEnvironmentVariable("ProgramFiles")
            End If

            .InitialDirectory = ExePath
            If .ShowDialog = DialogResult.OK Then
                TxtBxFurPath.Text = Path.GetDirectoryName(.FileName)
            End If

        End With

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles RecentBtnClear.Click

        Dim msg = "Do you want to continue?"
        Dim title = "Warning"
        Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or
            MsgBoxStyle.Exclamation

        ' Display the message box and save the response, Yes or No.
        Dim response = MsgBox(msg, style, title)

        ' Take some action based on the response.
        If response = MsgBoxResult.Yes Then
            Main.RecentToolStripMenuItem.DropDownItems.Clear()
            File.Delete(Path.Combine(IO.Paths.ApplicationSettingsPath, "Recent.txt"))

        End If

    End Sub

    Private Sub ChkTimeStamp_CheckedChanged(sender As Object, e As EventArgs) Handles ChkTimeStamp.CheckedChanged
        Main.Mainsettings.TimeStamp = CUShort(ChkTimeStamp.CheckState)
    End Sub

    Private Sub Config_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Loadconfig()
    End Sub

    Private Sub ConfigTabs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _
        ConfigTabs.SelectedIndexChanged
        Dim s As TabControl = CType(sender, TabControl)
    End Sub

    Private Sub DefaultColorBox_Click(sender As Object, e As EventArgs) Handles _
            DefaultColorBox.Click, EmitColorBox.Click, EmoteColorBox.Click, SayColorBox.Click,
            ShoutColorBox.Click, WhisperColorBox.Click

        Dim colrBox As PictureBox = CType(sender, PictureBox)
        GetColor(colrBox)
    End Sub

#End Region

End Class