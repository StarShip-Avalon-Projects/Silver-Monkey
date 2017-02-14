Imports System.Windows.Forms
Imports MonkeyCore
Imports MonkeyCore.Settings

Public Class BotSetup

    Public bFile As New cBot

    Private Sub BotSetup_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If String.IsNullOrEmpty(bFile.BiniFile) Then
            Main.NewBot = True
        End If

        TxtHPort.Text = bFile.lPort.ToString
        TxtBx_CharIni.Text = bFile.IniFile
        TxtBxMS_File.Text = bFile.MS_File

        TxtBxBotIni.Text = Path.GetFileName(bFile.BiniFile)
        MSEnableChkBx.Checked = bFile.MS_Engine_Enable
        TxtBxBotConroller.Text = bFile.BotController
        StandAloneChkBx.Checked = bFile.StandAlone
        ChkBxAutoConnect.Checked = bFile.AutoConnect

        TxtBxDreamURL.Text = bFile.DreamURL
        Select Case bFile.GoMapIDX
            Case 1
                RadioButton1.Checked = True
            Case 2
                RadioButton2.Checked = True
            Case 3
                RadioButton3.Checked = True
            Case 4
                RadioButton4.Checked = True
        End Select
        ChckSaveToLog.Checked = bFile.log
        setLogOptions()
        TxtBxLogName.Text = bFile.LogNameBase
        TxtBxLogPath.Text = bFile.LogPath
    End Sub

    Private Sub BTN_Browse_Click(sender As System.Object, e As System.EventArgs) Handles BTN_Browse.Click
        With IniBrowseDialog
            ' Select Character ini file

            .InitialDirectory = Paths.FurcadiaCharactersFolder

            If .ShowDialog = DialogResult.OK Then
                TxtBx_CharIni.Text = .FileName
            End If
        End With
    End Sub

    Private Sub BtnMS_File_Click(sender As System.Object, e As System.EventArgs) Handles BtnMS_File.Click
        With MS_BrosweDialog
            ' Select Character ini file
            .InitialDirectory = Paths.SilverMonkeyDocumentsPath
            If .ShowDialog = DialogResult.OK Then
                TxtBxMS_File.Text = .FileName
            End If
        End With
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        With IniBrowse
            ' Select Character ini file
            .InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
         "/Silver Monkey/Logs")
            .RestoreDirectory = True
            If .ShowDialog = DialogResult.OK Then
                Dim filenameOnly As String = Path.GetFileNameWithoutExtension(.FileName)
                TxtBxLogName.Text = filenameOnly
                TxtBxLogPath.Text = Path.GetFullPath(.FileName)
            End If
        End With
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If String.IsNullOrEmpty(TxtBxBotIni.Text) Then
            TxtBxBotIni.Text = "New Bot.bini"
        End If

        bFile.BiniFile = Path.Combine(Paths.SilverMonkeyBotPath, TxtBxBotIni.Text)
        bFile.lPort = Convert.ToInt32(TxtHPort.Text)
        Try
            bFile.IniFile = TxtBx_CharIni.Text
        Catch ex As ArgumentException
            MessageBox.Show(ex.Message, "+++ERROR+++", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        bFile.MS_File = TxtBxMS_File.Text
        bFile.MS_Engine_Enable = CBool(MSEnableChkBx.CheckState)
        bFile.BotController = TxtBxBotConroller.Text
        bFile.StandAlone = Convert.ToBoolean(StandAloneChkBx.Checked)
        bFile.AutoConnect = ChkBxAutoConnect.Checked

        bFile.DreamURL = TxtBxDreamURL.Text
        If RadioButton1.Checked = True Then
            bFile.GoMapIDX = 1
        ElseIf RadioButton2.Checked = True Then
            bFile.GoMapIDX = 2
        ElseIf RadioButton3.Checked = True Then
            bFile.GoMapIDX = 3
        ElseIf RadioButton4.Checked = True Then
            bFile.GoMapIDX = 4
        End If

        bFile.LogOption = LogOption()
        bFile.LogNameBase = TxtBxLogName.Text
        bFile.LogPath = TxtBxLogPath.Text
        bFile.log = ChckSaveToLog.Checked

        bFile.SaveBotSettings()
        Main.SaveRecentFile(bFile.IniFile)
        Me.DialogResult = DialogResult.OK
        Me.Close()

    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton4.CheckedChanged
        TxtBxDreamURL.Enabled = RadioButton4.Checked
    End Sub

    Private Sub RadioNewFile_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioNewFile.CheckedChanged
        ChkBxTimeStampLog.Enabled = RadioNewFile.Checked
    End Sub

    Public Function LogOption() As Short
        Dim Opt As Short = 0
        If RadioOverwriteLog.Checked Then
            Return Opt
        ElseIf RadioNewFile.Checked Then
            Opt = 1
            If ChkBxTimeStampLog.Checked Then
                Opt = 2
                Return Opt
            End If
            Return Opt
        End If
        Return Opt
    End Function

    Public Sub setLogOptions()
        Select Case bFile.LogOption
            Case 0
                RadioOverwriteLog.Checked = True
                ChkBxTimeStampLog.Checked = False
                ChkBxTimeStampLog.Enabled = False
            Case 1
                RadioNewFile.Checked = True
                ChkBxTimeStampLog.Checked = False
                ChkBxTimeStampLog.Enabled = True
            Case 2
                RadioNewFile.Checked = True
                ChkBxTimeStampLog.Checked = True
                ChkBxTimeStampLog.Enabled = True
        End Select
    End Sub

End Class