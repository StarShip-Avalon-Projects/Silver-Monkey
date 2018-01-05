Imports System.Windows.Forms
Imports BotSession
Imports MonkeyCore
Imports SilverMonkey.Engine

Public Class BotSetup

#Region "Public Constructors"

    Public Sub New()

        _TabIndex = TabIndex
        InitializeComponent()
        _botConfig = New BotOptions()
        Initialize()

    End Sub

    Public Sub New(ByRef BotConfig As BotOptions)
        _TabIndex = 0
        InitializeComponent()
        _botConfig = BotConfig
        Initialize()
    End Sub

    Private Sub Initialize()
        HelpLinks = New LabelHotlinks()
        ToolTip1.SetToolTip(CharacterRetervialHelpLink, "Please make sure you have a current Character.ini file downloaded from Furadia Services, This will override you're FurEd settings.")
    End Sub

#End Region

#Region "Private Fields"

    Private _TabIndex As Integer
    Private HelpLinks As LabelHotlinks

#End Region

#Region "Public Fields"

    Private _botConfig As BotOptions

    Public Property BotConfig As BotOptions
        Get
            Return _botConfig
        End Get
        Set(value As BotOptions)
            _botConfig = value
        End Set
    End Property

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
        With _botConfig.LogOptions
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

        TxtHPort.Text = _botConfig.LocalhostPort.ToString
        TxtBx_CharIni.Text = _botConfig.CharacterIniFile
        TxtBxMS_File.Text = _botConfig.MonkeySpeakEngineOptions.MonkeySpeakScriptFile

        TxtBxBotIni.Text = Path.GetFileName(_botConfig.Name)
        MSEnableChkBx.Checked = _botConfig.MonkeySpeakEngineOptions.IsEnabled
        TxtBxBotConroller.Text = _botConfig.MonkeySpeakEngineOptions.BotController
        StandAloneChkBx.Checked = _botConfig.Standalone
        ChkBxAutoConnect.Checked = _botConfig.AutoConnect

        NumericRetryAttempts.Value = _botConfig.ConnectionRetries
        NumericTimeOut.Value = _botConfig.ConnectionTimeOut

        TxtBxDreamURL.Text = _botConfig.DreamLink.ToString
        Select Case _botConfig.GoMapIDX
            Case 1
                RadioButton1.Checked = True
            Case 2
                RadioButton2.Checked = True
            Case 3
                RadioButton3.Checked = True
            Case 4
                RadioButton4.Checked = True
        End Select
        With _botConfig.LogOptions
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
        Try
            _botConfig = New BotOptions(Path.Combine(Paths.SilverMonkeyBotPath, TxtBxBotIni.Text))
            Integer.TryParse(TxtHPort.Text, _botConfig.LocalhostPort)
            Try
                _botConfig.CharacterIniFile = TxtBx_CharIni.Text
            Catch ex As ArgumentException
                MessageBox.Show(ex.Message, "+++ERROR+++", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            _botConfig.MonkeySpeakEngineOptions.MonkeySpeakScriptFile = TxtBxMS_File.Text
            _botConfig.MonkeySpeakEngineOptions.IsEnabled = MSEnableChkBx.Checked

            _botConfig.MonkeySpeakEngineOptions.BotController = TxtBxBotConroller.Text
            _botConfig.Standalone = Convert.ToBoolean(StandAloneChkBx.Checked)
            _botConfig.AutoConnect = ChkBxAutoConnect.Checked

            _botConfig.ConnectionTimeOut = CInt(NumericTimeOut.Value)
            _botConfig.ConnectionRetries = CInt(NumericRetryAttempts.Value)

            _botConfig.DreamLink = TxtBxDreamURL.Text
            If RadioButton1.Checked = True Then
                _botConfig.GoMapIDX = 1
            ElseIf RadioButton2.Checked = True Then
                _botConfig.GoMapIDX = 2
            ElseIf RadioButton3.Checked = True Then
                _botConfig.GoMapIDX = 3
            ElseIf RadioButton4.Checked = True Then
                _botConfig.GoMapIDX = 4
            End If
            With _botConfig.LogOptions
                .LogOption = LogOption()
                .LogNameBase = TxtBxLogName.Text
                .LogPath = TxtBxLogPath.Text
                .log = ChckSaveToLog.Checked
            End With

            _botConfig.SaveBotSettings()
            Main.SaveRecentFile(_botConfig.BotSettingsFile)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            Furcadia.Logging.Logger.Error(ex)
        End Try
    End Sub

    Private Sub OnFurEdClick(sender As Object, e As EventArgs) Handles CharacterRetervialHelpLink.Click
        Diagnostics.Process.Start(HelpLinks.IniRetrieval)
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton4.CheckedChanged
        TxtBxDreamURL.Enabled = RadioButton4.Checked
    End Sub

    Private Sub RadioNewFile_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioNewFile.CheckedChanged
        ChkBxTimeStampLog.Enabled = RadioNewFile.Checked
    End Sub

#End Region

End Class