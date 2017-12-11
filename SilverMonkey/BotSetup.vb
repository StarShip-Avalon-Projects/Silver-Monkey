Imports System.Windows.Forms
Imports MonkeyCore
Imports SilverMonkeyEngine

Public Class BotSetup

#Region "Public Constructors"

    Public Sub New(ByRef BotConfig As BotOptions, TabIndex As Integer)
        MyClass.New(BotConfig)
        _TabIndex = TabIndex
        InitializeComponent()
        ToolTip1.SetToolTip(LinkLabel1, "Please make sure you have a current Character.ini file downloaded from Furadia Services, This will override you're FurEd settings.")

    End Sub

    Public Sub New(ByRef BotConfig As BotOptions)
        bFile = BotConfig
        _TabIndex = 0
        InitializeComponent()
        ToolTip1.SetToolTip(LinkLabel1, "Please make sure you have a current Character.ini file downloaded from Furadia Services, This will override you're FurEd settings.")
    End Sub

#End Region

#Region "Private Fields"

    Private _TabIndex As Integer

#End Region

#Region "Public Fields"

    Public bFile As New BotOptions

#End Region

#Region "Public Methods"

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

    ''' <summary>
    ''' Configure Logging options
    ''' </summary>
    Public Sub SetLogOptions()
        With bFile.LogOptions
            Select Case .LogOption
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
        End With
    End Sub

#End Region

#Region "Private Methods"

    Private Sub BotSetup_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        TxtHPort.Text = bFile.LocalhostPort.ToString
        TxtBx_CharIni.Text = bFile.CharacterIniFile
        TxtBxMS_File.Text = bFile.MonkeySpeakEngineOptions.MonkeySpeakScriptFile

        TxtBxBotIni.Text = Path.GetFileName(bFile.Name)
        MSEnableChkBx.Checked = bFile.MonkeySpeakEngineOptions.MS_Engine_Enable
        TxtBxBotConroller.Text = bFile.MonkeySpeakEngineOptions.BotController
        StandAloneChkBx.Checked = bFile.Standalone
        ChkBxAutoConnect.Checked = bFile.AutoConnect

        TxtBxDreamURL.Text = bFile.DreamLink.ToString
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
        With bFile.LogOptions
            ChckSaveToLog.Checked = .log
            SetLogOptions()
            TxtBxLogName.Text = .LogNameBase
            TxtBxLogPath.Text = .LogPath
        End With
        ' Set the open tab
        TabControl1.SelectedIndex = _TabIndex
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
            .InitialDirectory = Paths.SilverMonkeyLogPath
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

        bFile = New BotOptions(Path.Combine(Paths.SilverMonkeyBotPath, TxtBxBotIni.Text))
        Integer.TryParse(TxtHPort.Text, bFile.LocalhostPort)
        Try
            bFile.CharacterIniFile = TxtBx_CharIni.Text
        Catch ex As ArgumentException
            MessageBox.Show(ex.Message, "+++ERROR+++", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        bFile.MonkeySpeakEngineOptions.MonkeySpeakScriptFile = TxtBxMS_File.Text
        bFile.MonkeySpeakEngineOptions.MS_Engine_Enable = MSEnableChkBx.Checked

        bFile.MonkeySpeakEngineOptions.BotController = TxtBxBotConroller.Text
        bFile.Standalone = Convert.ToBoolean(StandAloneChkBx.Checked)
        bFile.AutoConnect = ChkBxAutoConnect.Checked

        bFile.DreamLink = TxtBxDreamURL.Text
        If RadioButton1.Checked = True Then
            bFile.GoMapIDX = 1
        ElseIf RadioButton2.Checked = True Then
            bFile.GoMapIDX = 2
        ElseIf RadioButton3.Checked = True Then
            bFile.GoMapIDX = 3
        ElseIf RadioButton4.Checked = True Then
            bFile.GoMapIDX = 4
        End If
        With bFile.LogOptions
            .LogOption = LogOption()
            .LogNameBase = TxtBxLogName.Text
            .LogPath = TxtBxLogPath.Text
            .log = ChckSaveToLog.Checked
        End With

        bFile.SaveBotSettings()
        Main.SaveRecentFile(bFile.DestinationFile)
        Me.DialogResult = DialogResult.OK
        Me.Close()

    End Sub

    Private Sub onFurEdClick(sender As Object, e As EventArgs) Handles LinkLabel1.Click
        Diagnostics.Process.Start("http://www.furcadia.com/services/retrieve/retrieve.php4")
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton4.CheckedChanged
        TxtBxDreamURL.Enabled = RadioButton4.Checked
    End Sub

    Private Sub RadioNewFile_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioNewFile.CheckedChanged
        ChkBxTimeStampLog.Enabled = RadioNewFile.Checked
    End Sub

#End Region

End Class