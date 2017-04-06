Imports System.Windows.Forms
Imports System.Drawing.Text
Imports System.Drawing
Imports MonkeyCore.Settings
Imports MonkeyCore

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
        TxtBx_Server.Text = Main.MainSettings.Host
        TxtSPort.Text = Main.MainSettings.sPort.ToString
        ChkBxAutoReconnect.Checked = Main.MainSettings.AutoReconnect

        ChkBxAutoCloseProc.Checked = Main.MainSettings.CloseProc
        ReconnectUpDown.Value = Main.MainSettings.ReconnectMax
        ChkTimeStamp.Checked = CBool(Main.MainSettings.TimeStamp)
        ' Get the installed fonts collection.
        Dim installed_fonts As New InstalledFontCollection
        ' Get an array of the system's font familiies.
        Dim font_families() As FontFamily = installed_fonts.Families()
        ' Display the font families.
        For Each font_family As FontFamily In font_families
            ComboFontFace.Items.Add(font_family.Name)
        Next (font_family)
        ComboFontFace.SelectedItem = Main.MainSettings.ApFont.Name
        For i As Integer = 3 To 50
            ComboFontSize.Items.Add(i.ToString)
        Next
        ComboFontSize.SelectedItem = Main.MainSettings.ApFont.Size.ToString
        NumericUpDown4.Value = Main.MainSettings.TT_TimeOut
        ReconnectUpDown.Value = Main.MainSettings.ReconnectMax
        EmitColorBox.BackColor = Main.MainSettings.EmitColor
        SayColorBox.BackColor = Main.MainSettings.SayColor
        ShoutColorBox.BackColor = Main.MainSettings.ShoutColor
        WhisperColorBox.BackColor = Main.MainSettings.WhColor
        DefaultColorBox.BackColor = Main.MainSettings.DefaultColor
        EmoteColorBox.BackColor = Main.MainSettings.EmoteColor

        TxtBxFurPath.Text = Main.MainSettings.FurcPath
        'SysTray
        ChkBxSysTray.CheckState = Main.MainSettings.SysTray
        NumSonnectTimeOut.Value = Main.MainSettings.ConnectTimeOut
        NumPing.Value = Main.MainSettings.Ping

        chkBxAdvertisment.Checked = Main.MainSettings.Advertisment
        chkBxAnnouncement.Checked = Main.MainSettings.Announcement
        chkBxBroadcast.Checked = Main.MainSettings.Broadcast
        chkBxAutoLoadLastBotFile.Checked = Main.MainSettings.LoadLastBotFile
        chkBxClientDisconnectToggle.Checked = Main.MainSettings.DisconnectPopupToggle
        Me.Location = My.Settings.ConfigFormLocation

        CheckBox1.Checked = Main.MainSettings.PSShowClient
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
        Main.MainSettings.Host = TxtBx_Server.Text
        Main.MainSettings.sPort = Convert.ToInt32(TxtSPort.Text)

        Main.MainSettings.ReconnectMax = CInt(ReconnectUpDown.Value)
        Main.MainSettings.TT_TimeOut = CInt(NumericUpDown4.Value)
        Main.MainSettings.TimeStamp = CUShort(ChkTimeStamp.CheckState)

        Dim face As String = ComboFontFace.SelectedItem.ToString
        Dim size As Integer = Convert.ToInt32(ComboFontSize.SelectedItem)
        Main.MainSettings.ApFont = New Font(face, size)

        Main.MainSettings.CloseProc = ChkBxAutoCloseProc.Checked
        Main.MainSettings.EmitColor = EmitColorBox.BackColor
        Main.MainSettings.SayColor = SayColorBox.BackColor
        Main.MainSettings.ShoutColor = ShoutColorBox.BackColor
        Main.MainSettings.WhColor = WhisperColorBox.BackColor
        Main.MainSettings.DefaultColor = DefaultColorBox.BackColor

        Main.MainSettings.AutoReconnect = ChkBxAutoReconnect.Checked
        Main.MainSettings.SysTray = ChkBxSysTray.CheckState
        Main.MainSettings.ReconnectMax = CInt(ReconnectUpDown.Value)
        Main.MainSettings.ConnectTimeOut = CInt(NumSonnectTimeOut.Value)
        Main.MainSettings.FurcPath = TxtBxFurPath.Text
        Main.MainSettings.Ping = CInt(NumPing.Value)

        Main.MainSettings.Advertisment = chkBxAdvertisment.Checked
        Main.MainSettings.Announcement = chkBxAnnouncement.Checked
        Main.MainSettings.Broadcast = chkBxBroadcast.Checked
        Main.MainSettings.LoadLastBotFile = chkBxAutoLoadLastBotFile.Checked
        Main.MainSettings.DisconnectPopupToggle = chkBxClientDisconnectToggle.Checked
        'Save the settings to the ini file

        Main.MainSettings.PSShowClient = CheckBox1.Checked
        'Main.MainSettings.PSShowSettingsWindow = CheckBox2.Checked

        For Each lvItem As ListViewItem In LstVwPlugin.Items
            If PluginList.ContainsKey(lvItem.SubItems.Item(1).Text.Replace(" ", "")) Then
                PluginList(lvItem.SubItems.Item(1).Text.Replace(" ", "")) = lvItem.Checked
            Else
                PluginList.Add(lvItem.SubItems.Item(1).Text.Replace(" ", ""), lvItem.Checked)
            End If
        Next
        Paths.FurcadiaProgramFolder = Main.MainSettings.FurcPath
        Main.MainSettings.SaveMainSettings()

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

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' RecentToolStripMenuItem.DropDownItems.Clear()
        'File.Delete(Path.Combine(Paths.ApplicationSettingsPath, "Recent.txt"))
    End Sub

    Private Sub ChkTimeStamp_CheckedChanged(sender As Object, e As EventArgs) Handles ChkTimeStamp.CheckedChanged
        Main.MainSettings.TimeStamp = CUShort(ChkTimeStamp.CheckState)
    End Sub

    Private Sub Config_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.ConfigFormLocation = Me.Location
        My.Settings.Save()
    End Sub

    Private Sub Config_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ConfigTabs.SelectedIndex = My.Settings.ConfigSelectedTab
        PopulatePluginList()
        Loadconfig()
    End Sub
    Private Sub ConfigTabs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ConfigTabs.SelectedIndexChanged
        Dim s As TabControl = CType(sender, TabControl)
        My.Settings.ConfigSelectedTab = s.SelectedIndex
    End Sub

    Private Sub DefaultColorBox_Click(sender As Object, e As EventArgs) Handles DefaultColorBox.Click
        GetColor(DefaultColorBox)
    End Sub

    Private Sub EmitColorBox_Click(sender As Object, e As EventArgs) Handles EmitColorBox.Click
        GetColor(EmitColorBox)
    End Sub

    Private Sub EmoteColorBox_Click(sender As Object, e As EventArgs) Handles EmoteColorBox.Click
        GetColor(EmoteColorBox)
    End Sub

    Private Sub PopulatePluginList()
        Dim objPlugin As SilverMonkey.Interfaces.msPlugin
        Dim intIndex As Integer
        'Loop through available plugins, creating instances and adding them to listbox

        For intIndex = 0 To Plugins.Count - 1
            objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(intIndex)), SilverMonkey.Interfaces.msPlugin)
            Dim item As ListViewItem = LstVwPlugin.Items.Add(intIndex.ToString)
            item.SubItems.Add(objPlugin.Name)
            item.SubItems.Add(objPlugin.Description)
            item.SubItems.Add(objPlugin.Version)
            If Not PluginList.ContainsKey(objPlugin.Name.Replace(" ", "")) Then
                PluginList.Add(objPlugin.Name.Replace(" ", ""), True)
            End If
            item.Checked = PluginList.Item(objPlugin.Name.Replace(" ", ""))
        Next

    End Sub

    Private Sub SayColorBox_Click(sender As Object, e As EventArgs) Handles SayColorBox.Click
        GetColor(SayColorBox)
    End Sub

    Private Sub ShoutColorBox_Click(sender As Object, e As EventArgs) Handles ShoutColorBox.Click
        GetColor(ShoutColorBox)
    End Sub

    Private Sub WhisperColorBox_Click(sender As Object, e As EventArgs) Handles WhisperColorBox.Click
        GetColor(WhisperColorBox)
    End Sub

#End Region

End Class