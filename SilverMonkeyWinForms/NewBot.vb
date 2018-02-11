Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Engine.BotSession
Imports MonkeyCore2.IO

Public Class NewBott

#Region "Public Properties"

    Public ReadOnly Property BotConfig As BotOptions
        Get
            Return bFile
        End Get
    End Property

#End Region

#Region "Public Fields"

    Private bFile As BotOptions

#End Region

#Region "Public Constructors"

    Public Sub New(ByRef options As BotOptions)
        Dim Prt As New Random(Date.Now.Millisecond)
        options = New BotOptions()
        HelpLinks = New LabelHotlinks()
        options.LocalhostPort = Prt.Next(options.LocalhostPort, options.LocalhostPort + 1100)
        bFile = options
        ' This call is required by the designer.
        InitializeComponent()
        ToolTip1.SetToolTip(LinkLabel1, "Please make sure you have a current Character.ini file downloaded from Furadia Services, This will override you're FurEd settings.")
        ' Add any initialization after the InitializeComponent() call.

    End Sub

#End Region

#Region "Private Fields"

    Dim OverWrite As Boolean = False
    Dim WizIndex As Integer = 1
    Private HelpLinks As LabelHotlinks

#End Region

#Region "Controls"

    Public BtnCharacterINI As New Button
    Public BtnFileLocation As New Button
    Public LblBotController As New Label
    Public LblBotName As New Label
    Public LblCharacterINI As New Label
    Public LblFileLocation As New Label

    'Value 4
    Public RadioGoDreamURL As New RadioButton

    'Value 2
    Public RadioGoMapAcropolis As New RadioButton

    'Value 1
    Public RadioGoMapAllgeriaIsland As New RadioButton

    'Value 3
    Public RadioGoMapNone As New RadioButton

    Public TxtbxBotController As New TextBox
    Public TxtbxBotName As New TextBox
    Public TxtbxCharacterINI As New TextBox
    Public TxtbxDreamURL As New TextBox
    Public TxtbxFilelocation As New TextBox
    Private Const PageCount As Integer = 2

#End Region

#Region "Public Methods"

    Public Sub BtnCharacterINI_click(sender As Object, e As EventArgs)

        With OpenFileDialog1
            '.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "/Silver Monkey"

            .InitialDirectory = MonkeyCore2.IO.Paths.FurcadiaCharactersFolder
            If .ShowDialog = Windows.Forms.DialogResult.OK Then

                TxtbxCharacterINI.Text = .FileName
            End If
        End With

    End Sub

    Public Sub BtnFileLocation_click(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button)

        With FolderBrowserDialog1

            If .ShowDialog = Windows.Forms.DialogResult.OK Then

                TxtbxFilelocation.Text = .SelectedPath
            End If
        End With
    End Sub

#End Region

#Region "Private Methods"

    'Button Previous
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If WizIndex <= 1 Then Exit Sub
        ClearForm()
        WizIndex -= 1
        SetForm(WizIndex)
    End Sub

    'Button Next
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If WizIndex >= 2 Then Exit Sub
        ClearForm()
        WizIndex += 1
        SetForm(WizIndex)
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Dispose()
    End Sub

    Private Sub ClearForm()
        LblBotName.Visible = False
        TxtbxBotName.Visible = False
        LblFileLocation.Visible = False
        TxtbxFilelocation.Visible = False
        BtnFileLocation.Visible = False
        LblCharacterINI.Visible = False
        TxtbxCharacterINI.Visible = False
        BtnCharacterINI.Visible = False
        LinkLabel1.Visible = False
        LblBotController.Visible = False
        TxtbxBotController.Visible = False

        'Value 2
        RadioGoMapAcropolis.Visible = False
        'Value 1
        RadioGoMapAllgeriaIsland.Visible = False
        'Value 4
        RadioGoDreamURL.Visible = False
        'Value 3
        RadioGoMapNone.Visible = False
        TxtbxDreamURL.Visible = False

        Button1.Enabled = True
        Button2.Enabled = True
    End Sub

    Private Sub Dialog1_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        bFile = New BotOptions()

        GroupBox1.Text = "Basic Settings"

        'LblBotName
        LblBotName.Text = "BotName"
        LblBotName.Parent = GroupBox1
        LblBotName.Size = New Size(51, 13)
        LblBotName.Location = New Point(20, 39)
        LblBotName.Visible = True
        LblBotName.Anchor = AnchorStyles.Top Or AnchorStyles.Left

        'TxtbxBotName
        TxtbxBotName.Parent = GroupBox1
        TxtbxBotName.Text = "New Bot"
        TxtbxBotName.Size = New Size(207, 20)
        TxtbxBotName.Location = New Point(20, 55)
        TxtbxBotName.Visible = True
        TxtbxBotName.Anchor = System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left

        'LblFileLocation
        LblFileLocation.Text = "File Location"
        LblFileLocation.Parent = GroupBox1
        LblFileLocation.Size = New Size(70, 13)
        LblFileLocation.Location = New Point(20, 77)
        LblFileLocation.Visible = True
        LblFileLocation.Anchor = System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left

        'TxtbxFilelocation
        TxtbxFilelocation.Parent = GroupBox1
        TxtbxFilelocation.Size = New Size(207, 20)
        TxtbxFilelocation.Location = New Point(20, 94)
        TxtbxFilelocation.Visible = True
        TxtbxFilelocation.Text = MonkeyCore2.IO.Paths.SilverMonkeyBotPath
        TxtbxFilelocation.Anchor = System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left

        'BtnFileLocation
        BtnFileLocation.Parent = GroupBox1
        BtnFileLocation.Text = "..."
        BtnFileLocation.Size = New Size(24, 19)
        BtnFileLocation.Location = New Point(245, 94)
        BtnFileLocation.Visible = True
        BtnFileLocation.Anchor = System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left
        AddHandler BtnFileLocation.Click, AddressOf BtnFileLocation_click
        'LblCharacterINI
        LblCharacterINI.Text = "Character Ini"
        LblCharacterINI.Parent = GroupBox1
        LblCharacterINI.Size = New Size(70, 13)
        LblCharacterINI.Location = New Point(20, 116)
        LblCharacterINI.Visible = True
        LblCharacterINI.Anchor = System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left

        'TxtbxCharacterINI
        TxtbxCharacterINI.Parent = GroupBox1
        TxtbxCharacterINI.Size = New Size(207, 20)
        TxtbxCharacterINI.Location = New Point(20, 133)
        TxtbxCharacterINI.Visible = True
        TxtbxCharacterINI.Anchor = System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left

        'BtnCharacterINI
        BtnCharacterINI.Parent = GroupBox1
        BtnCharacterINI.Text = "..."
        BtnCharacterINI.Size = New Size(24, 19)
        BtnCharacterINI.Location = New Point(245, 133)
        BtnCharacterINI.Visible = True
        BtnCharacterINI.Anchor = System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left
        AddHandler BtnCharacterINI.Click, AddressOf BtnCharacterINI_click

        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        LinkLabel1.Parent = GroupBox1
        Me.LinkLabel1.Location = New System.Drawing.Point(270, 133)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(90, 32)
        Me.LinkLabel1.TabIndex = 2
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Help?"
        LinkLabel1.Anchor = System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left

        'LblBotController
        LblBotController.Text = "Bot Controller Name"
        LblBotController.Parent = GroupBox1
        LblBotController.Size = New Size(101, 13)
        LblBotController.Location = New Point(17, 155)
        LblBotController.Visible = True
        LblBotController.Anchor = System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left

        'TxtbxBotController
        TxtbxBotController.Parent = GroupBox1
        TxtbxBotController.Size = New Size(207, 20)
        TxtbxBotController.Location = New Point(20, 171)
        TxtbxBotController.Visible = True
        TxtbxBotController.Anchor = System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left

        'Value 2
        'RadioGoMapAcropolis
        RadioGoMapAcropolis.Parent = GroupBox1
        RadioGoMapAcropolis.Text = "Acropolis"
        RadioGoMapAcropolis.Size = New Size(68, 17)
        RadioGoMapAcropolis.Location = New Point(6, 66)
        RadioGoMapAcropolis.Visible = False
        RadioGoMapAcropolis.Anchor = System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left

        'Value 1
        'RadioGoMapAllgeriaIsland
        RadioGoMapAllgeriaIsland.Parent = GroupBox1
        RadioGoMapAllgeriaIsland.Text = "Allgeria Island"
        RadioGoMapAllgeriaIsland.Size = New Size(68, 17)
        RadioGoMapAllgeriaIsland.Location = New Point(6, 89)
        RadioGoMapAllgeriaIsland.Visible = False
        RadioGoMapAllgeriaIsland.Anchor = System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left

        'Value 4
        'RadioGoDreamURL
        RadioGoDreamURL.Parent = GroupBox1
        RadioGoDreamURL.Text = "Dream URL"
        RadioGoDreamURL.Size = New Size(90, 17)
        RadioGoDreamURL.Location = New Point(6, 116)
        RadioGoDreamURL.Visible = False
        RadioGoDreamURL.Anchor = System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left

        'TxtbxCDreamURL
        TxtbxDreamURL.Parent = GroupBox1
        TxtbxDreamURL.Text = "furc://"
        TxtbxDreamURL.Size = New Size(174, 20)
        TxtbxDreamURL.Location = New Point(95, 115)
        TxtbxDreamURL.Visible = False
        TxtbxDreamURL.Anchor = System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left

        'Value 3
        'RadioGoMapNone
        RadioGoMapNone.Parent = GroupBox1
        RadioGoMapNone.Text = "None"
        RadioGoMapNone.Size = New Size(68, 17)
        RadioGoMapNone.Location = New Point(6, 139)
        RadioGoMapNone.Visible = False
        RadioGoMapNone.Anchor = System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left

        Button1.Enabled = False
        Button2.Enabled = True
        WizIndex = 1
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If String.IsNullOrEmpty(TxtbxBotName.Text) Then
            If MessageBox.Show("Botname cannot be empty!!!", "Missing Botname",
                MessageBoxButtons.OK, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
        End If
        If String.IsNullOrEmpty(TxtbxFilelocation.Text) Then
            If MessageBox.Show("Bot path cannot be empty!!!", "Missing Botpath",
                MessageBoxButtons.OK, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
        End If
        Dim BotFile As String
        Dim MsFile As String = String.Empty
        If Not String.IsNullOrEmpty(TxtbxFilelocation.Text) Then
            BotFile = Path.Combine(TxtbxFilelocation.Text, TxtbxBotName.Text)
            MsFile = BotFile
        ElseIf Not Directory.Exists(TxtbxFilelocation.Text) Then
            BotFile = Path.Combine(MonkeyCore2.IO.Paths.SilverMonkeyBotPath, TxtbxBotName.Text)
            MsFile = BotFile
        Else
            BotFile = Path.Combine(MonkeyCore2.IO.Paths.SilverMonkeyBotPath, TxtbxBotName.Text)
            MsFile = BotFile
        End If

        Dim ext As String = Path.GetExtension(BotFile)
        If String.IsNullOrEmpty(ext) Then
            BotFile += ".bini"
            MsFile += ".ms"
        End If

        If File.Exists(BotFile) And Not OverWrite Then
            If MessageBox.Show(BotFile + " Exists! Over write settings?", "File Exists Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then

                ClearForm()
                WizIndex = 1
                SetForm(WizIndex)
                TxtbxBotName.Select()
                Exit Sub
            Else
                OverWrite = True
            End If
        End If
        bFile.BotSettingsFile = BotFile
        bFile.CharacterIniFile = TxtbxCharacterINI.Text

        bFile.MonkeySpeakEngineOptions.MonkeySpeakScriptFile = MsFile

        bFile.LogOptions.LogNameBase = TxtbxBotName.Text
        bFile.LogOptions.log = True

        bFile.MonkeySpeakEngineOptions.BotController = TxtbxBotController.Text

        bFile.DreamLink = TxtbxDreamURL.Text

        If RadioGoMapAllgeriaIsland.Checked = True Then
            bFile.GoMapIDX = 1
        ElseIf RadioGoMapAcropolis.Checked = True Then
            bFile.GoMapIDX = 2
        ElseIf RadioGoMapNone.Checked = True Then
            bFile.GoMapIDX = 3
        ElseIf RadioGoDreamURL.Checked = True Then
            bFile.GoMapIDX = 4
        Else
            'pop up error and load that pare of the form Page 2
            MessageBox.Show("Please select which map you want the bot to go to when connected to Furcadia.")
            ClearForm()
            WizIndex = 2
            SetForm(WizIndex)
            Exit Sub
        End If
        Dim ScriptFile As String = bFile.MonkeySpeakEngineOptions.MonkeySpeakScriptFile
        If Not File.Exists(ScriptFile) Then
            Dim Script = New EditorScript(Path.Combine(Paths.ApplicationPath, "MS Scripts.ini"))
            Script.ReadScriptTemplate("Default New Script", bFile.MonkeySpeakEngineOptions.Version.ToString())
            Script.WriteScriptToFile(ScriptFile)
        End If
        Try
            bFile.SaveBotSettings()
            Main.SaveRecentFile(bFile.BotSettingsFile)
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Dispose()
        Catch ex As Exception

            MessageBox.Show(ex.Message, "error")
        End Try
    End Sub

    Private Sub SetForm(Index As Integer)

        Select Case Index
            Case 1
                GroupBox1.Text = "Basic Settings"

                LblBotName.Visible = True
                TxtbxBotName.Visible = True
                LblFileLocation.Visible = True
                TxtbxFilelocation.Visible = True
                BtnFileLocation.Visible = True
                LblCharacterINI.Visible = True
                TxtbxCharacterINI.Visible = True
                LinkLabel1.Visible = True
                BtnCharacterINI.Visible = True
                LblBotController.Visible = True
                TxtbxBotController.Visible = True
                Button1.Enabled = False
            Case 2
                GroupBox1.Text = "GoMap Settings"

                'Value 2
                RadioGoMapAcropolis.Visible = True
                'Value 1
                RadioGoMapAllgeriaIsland.Visible = True
                'Value 4
                RadioGoDreamURL.Visible = True
                'Value 3
                RadioGoMapNone.Visible = True

                TxtbxDreamURL.Visible = True
                Button2.Enabled = False
            Case Else
                Throw New InvalidDataException("Invalid Wizard Form index")
        End Select

    End Sub

#End Region

    Private Sub OnFurEdClick(sender As Object, e As EventArgs) Handles LinkLabel1.Click
        Process.Start(HelpLinks.IniRetrieval)
    End Sub

End Class