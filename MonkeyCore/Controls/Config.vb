Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Drawing.Text
Imports System.Drawing

Imports MonkeyCore


Public Class Config

    Dim MyConfig As New Settings

    Private Sub BTN_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_Cancel.Click
        'Close with out Saving
        Me.Close()
    End Sub

    Private Sub BTN_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_Ok.Click
        'apply the Settings
        Main.cMain.Host = TxtBx_Server.Text
        Main.cMain.sPort = Convert.ToInt32(TxtSPort.Text)

        Main.cMain.ReconnectMax = CInt(ReconnectUpDown.Value)
        Main.cMain.TT_TimeOut = CInt(NumericUpDown4.Value)
        Main.cMain.TimeStamp = CUShort(ChkTimeStamp.CheckState)
        Dim face As String = ComboFontFace.SelectedItem.ToString
        Dim size As Integer = Convert.ToInt32(ComboFontSize.SelectedItem)
        Main.cMain.CloseProc = ChkBxAutoCloseProc.Checked
        Main.cMain.ApFont = New Font(face, size)
        Main.cMain.EmitColor = EmitColorBox.BackColor
        Main.cMain.SayColor = SayColorBox.BackColor
        Main.cMain.ShoutColor = ShoutColorBox.BackColor
        Main.cMain.WhColor = WhisperColorBox.BackColor
        Main.cMain.DefaultColor = DefaultColorBox.BackColor

        Main.cMain.AutoReconnect = ChkBxAutoReconnect.Checked
        Main.cMain.SysTray = ChkBxSysTray.CheckState
        Main.cMain.ReconnectMax = CInt(ReconnectUpDown.Value)
        Main.cMain.ConnectTimeOut = CInt(NumSonnectTimeOut.Value)
        Main.cMain.FurcPath = TxtBxFurPath.Text
        Main.cMain.Ping = CInt(NumPing.Value)

        Main.cMain.Advertisment = chkBxAdvertisment.Checked
        Main.cMain.Announcement = chkBxAnnouncement.Checked
        Main.cMain.Broadcast = chkBxBroadcast.Checked
        Main.cMain.LoadLastBotFile = chkBxAutoLoadLastBotFile.Checked
        Main.cMain.DisconnectPopupToggle = chkBxClientDisconnectToggle.Checked
        'Save the settings to the ini file

        Main.cMain.PSShowClient = CheckBox1.Checked
        Main.cMain.PSShowMainWindow = CheckBox2.Checked
        Dim Plugins As Dictionary(Of String, Boolean) = New Dictionary(Of String, Boolean)
        For Each lvItem As ListViewItem In LstVwPlugin.Items
            Plugins.Add(lvItem.SubItems.Item(1).Text.Replace(" ", ""), lvItem.Checked)
        Next
        Paths.FurcadiaProgramFolder = Main.cMain.FurcPath
        Main.cMain.PluginList = Plugins
        Main.cMain.SaveMainSettings()

        Main.InitializeTextControls()
        Me.Dispose()

    End Sub

    Private Sub Config_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.ConfigFormLocation = Me.Location
        My.Settings.Save()
    End Sub

    Private Sub Config_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ConfigTabs.SelectedIndex = My.Settings.ConfigSelectedTab
        PopulatePluginList()
        Loadconfig()
    End Sub

    Public Sub Loadconfig()
        TxtBx_Server.Text = Main.cMain.Host
        TxtSPort.Text = Main.cMain.sPort.ToString
        ChkBxAutoReconnect.Checked = Main.cMain.AutoReconnect

        ChkBxAutoCloseProc.Checked = Main.cMain.CloseProc
        ReconnectUpDown.Value = Main.cMain.ReconnectMax
        ChkTimeStamp.Checked = CBool(Main.cMain.TimeStamp)
        ' Get the installed fonts collection.
        Dim installed_fonts As New InstalledFontCollection
        ' Get an array of the system's font familiies.
        Dim font_families() As FontFamily = installed_fonts.Families()
        ' Display the font families.
        For Each font_family As FontFamily In font_families
            ComboFontFace.Items.Add(font_family.Name)
        Next (font_family)
        ComboFontFace.SelectedItem = Main.cMain.ApFont.Name
        For i As Integer = 3 To 50
            ComboFontSize.Items.Add(i.ToString)
        Next
        ComboFontSize.SelectedItem = Main.cMain.ApFont.Size.ToString
        NumericUpDown4.Value = Main.cMain.TT_TimeOut
        ReconnectUpDown.Value = Main.cMain.ReconnectMax
        EmitColorBox.BackColor = Main.cMain.EmitColor
        SayColorBox.BackColor = Main.cMain.SayColor
        ShoutColorBox.BackColor = Main.cMain.ShoutColor
        WhisperColorBox.BackColor = Main.cMain.WhColor
        DefaultColorBox.BackColor = Main.cMain.DefaultColor
        EmoteColorBox.BackColor = Main.cMain.EmoteColor

        TxtBxFurPath.Text = Main.cMain.FurcPath
        'SysTray
        ChkBxSysTray.CheckState = Main.cMain.SysTray
        NumSonnectTimeOut.Value = Main.cMain.ConnectTimeOut
        NumPing.Value = Main.cMain.Ping

        chkBxAdvertisment.Checked = Main.cMain.Advertisment
        chkBxAnnouncement.Checked = Main.cMain.Announcement
        chkBxBroadcast.Checked = Main.cMain.Broadcast
        chkBxAutoLoadLastBotFile.Checked = Main.cMain.LoadLastBotFile
        chkBxClientDisconnectToggle.Checked = Main.cMain.DisconnectPopupToggle
        Me.Location = My.Settings.ConfigFormLocation


        CheckBox1.Checked = Main.cMain.PSShowClient
        CheckBox2.Checked = Main.cMain.PSShowMainWindow
    End Sub

    Private Sub PopulatePluginList()
        Dim objPlugin As SilverMonkey.Interfaces.msPlugin
        Dim intIndex As Integer

        'Loop through available plugins, creating instances and adding them to listbox
        If Not Plugins Is Nothing Then
            For intIndex = 0 To Plugins.Length - 1
                objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(intIndex)), SilverMonkey.Interfaces.msPlugin)
                Dim item As ListViewItem = LstVwPlugin.Items.Add(intIndex.ToString)
                item.SubItems.Add(objPlugin.Name)
                item.SubItems.Add(objPlugin.Description)
                item.SubItems.Add(objPlugin.Version)
                If Not Main.cMain.PluginList.ContainsKey(objPlugin.Name.Replace(" ", "")) Then
                    Main.cMain.PluginList.Add(objPlugin.Name.Replace(" ", ""), True)
                End If
                item.Checked = Main.cMain.PluginList.Item(objPlugin.Name.Replace(" ", ""))
            Next
        End If

    End Sub




    Private Sub WhisperColorBox_Click(sender As System.Object, e As System.EventArgs) Handles WhisperColorBox.Click
        GetColor(WhisperColorBox)
    End Sub

    Private Sub ShoutColorBox_Click(sender As System.Object, e As System.EventArgs) Handles ShoutColorBox.Click
        GetColor(ShoutColorBox)
    End Sub

    Private Sub EmitColorBox_Click(sender As System.Object, e As System.EventArgs) Handles EmitColorBox.Click
        GetColor(EmitColorBox)
    End Sub

    Private Sub SayColorBox_Click(sender As System.Object, e As System.EventArgs) Handles SayColorBox.Click
        GetColor(SayColorBox)
    End Sub

    Private Sub DefaultColorBox_Click(sender As System.Object, e As System.EventArgs) Handles DefaultColorBox.Click
        GetColor(DefaultColorBox)
    End Sub

    Private Sub EmoteColorBox_Click(sender As System.Object, e As System.EventArgs) Handles EmoteColorBox.Click
        GetColor(EmoteColorBox)
    End Sub

    Public Sub GetColor(ByRef ColorBX As System.Windows.Forms.PictureBox)
        Dim dlg As New ColorDialog
        dlg.Color = ColorBX.BackColor
        If dlg.ShowDialog() = DialogResult.OK Then
            ColorBX.BackColor = dlg.Color
        End If
    End Sub

    Private Sub ChkTimeStamp_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ChkTimeStamp.CheckedChanged
        Main.cMain.TimeStamp = CUShort(ChkTimeStamp.CheckState)
    End Sub




    Private Sub ConfigTabs_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ConfigTabs.SelectedIndexChanged
        Dim s As TabControl = CType(sender, TabControl)
        My.Settings.ConfigSelectedTab = s.SelectedIndex
    End Sub



    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
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

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        callbk.RecentToolStripMenuItem.DropDownItems.Clear()
        File.Delete(Path.Combine(MonkeyCore.Paths.ApplicationSettingsPath, "Recent.txt"))
    End Sub
End Class
