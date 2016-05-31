Imports System.Windows.Forms
Imports SilverMonkey.ConfigStructs
Imports Furcadia.IO
Imports System.IO
Imports System.Drawing


Public Class NewBott
    Dim pPath As String = ConfigStructs.pPath()
    Public bFile As cBot
    Dim WizIndex As Integer = 1
    Dim OverWrite As Boolean = False
#Region "Controls"
    Private Const PageCount As Integer = 2

    Public LblBotName As New Label
    Public TxtbxBotName As New TextBox
    Public LblFileLocation As New Label
    Public TxtbxFilelocation As New TextBox
    Public BtnFileLocation As New Button
    Public LblCharacterINI As New Label
    Public TxtbxCharacterINI As New TextBox
    Public BtnCharacterINI As New Button
    Public LblBotController As New Label
    Public TxtbxBotController As New TextBox

    'Value 2
    Public RadioGoMapAcropolis As New RadioButton
    'Value 1
    Public RadioGoMapAllgeriaIsland As New RadioButton
    'Value 4
    Public RadioGoDreamURL As New RadioButton
    'Value 3
    Public RadioGoMapNone As New RadioButton
    Public TxtbxDreamURL As New TextBox
#End Region

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim BotFile As String
        If Not String.IsNullOrEmpty(TxtbxFilelocation.Text) Then
            BotFile = TxtbxFilelocation.Text + Path.DirectorySeparatorChar + TxtbxBotName.Text
        Else
            BotFile = CheckMyDocFile(TxtbxBotName.Text)
        End If
        If File.Exists(BotFile + ".bini") And Not OverWrite Then
            If MessageBox.Show(BotFile + ".bini Exists! Over write settings?", "File Exists Warning", _
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, _
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

        bFile.BotFile = BotFile + ".bini"
        bFile.IniFile = TxtbxCharacterINI.Text
        bFile.MS_File = BotFile + ".ms"

        bFile.LogNameBase = TxtbxBotName.Text
        bFile.log = True

        bFile.BotController = TxtbxBotController.Text

        bFile.DreamURL = TxtbxDreamURL.Text

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

        If bFile.log Then
            callbk.LogStream = New LogStream(callbk.setLogName(bFile), bFile.LogPath)
        End If
        bFile.SaveBotSettings()
        Main.SaveRecentFile(bFile.BotFile)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Dispose()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Dispose()
    End Sub

    Private Sub Dialog1_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Main.NewBot = True

        'Default BotFile Settings
        bFile = New cBot
        bFile.MS_Engine_Enable = True
        bFile.AutoConnect = False
        bFile.GoMapIDX = 1

        GroupBox1.Text = "Basic Settings"

        'LblBotName
        LblBotName.Text = "BotName"
        LblBotName.Parent = GroupBox1
        LblBotName.Size = New Size(51, 13)
        LblBotName.Location = New Point(20, 39)
        LblBotName.Visible = True
        LblBotName.Anchor = CType(System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'TxtbxBotName
        TxtbxBotName.Parent = GroupBox1
        TxtbxBotName.Text = "New Bot"
        TxtbxBotName.Size = New Size(207, 20)
        TxtbxBotName.Location = New Point(20, 55)
        TxtbxBotName.Visible = True
        TxtbxBotName.Anchor = CType(System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'LblFileLocation
        LblFileLocation.Text = "File Location"
        LblFileLocation.Parent = GroupBox1
        LblFileLocation.Size = New Size(70, 13)
        LblFileLocation.Location = New Point(20, 77)
        LblFileLocation.Visible = True
        LblFileLocation.Anchor = CType(System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'TxtbxFilelocation
        TxtbxFilelocation.Parent = GroupBox1
        TxtbxFilelocation.Size = New Size(207, 20)
        TxtbxFilelocation.Location = New Point(20, 94)
        TxtbxFilelocation.Visible = True
        TxtbxFilelocation.Anchor = CType(System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'BtnFileLocation
        BtnFileLocation.Parent = GroupBox1
        BtnFileLocation.Text = "..."
        BtnFileLocation.Size = New Size(24, 19)
        BtnFileLocation.Location = New Point(245, 94)
        BtnFileLocation.Visible = True
        BtnFileLocation.Anchor = CType(System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)
        AddHandler BtnFileLocation.Click, AddressOf BtnFileLocation_click
        'LblCharacterINI
        LblCharacterINI.Text = "Character Ini"
        LblCharacterINI.Parent = GroupBox1
        LblCharacterINI.Size = New Size(70, 13)
        LblCharacterINI.Location = New Point(20, 116)
        LblCharacterINI.Visible = True
        LblCharacterINI.Anchor = CType(System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'TxtbxCharacterINI
        TxtbxCharacterINI.Parent = GroupBox1
        TxtbxCharacterINI.Size = New Size(207, 20)
        TxtbxCharacterINI.Location = New Point(20, 132)
        TxtbxCharacterINI.Visible = True
        TxtbxCharacterINI.Anchor = CType(System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'BtnCharacterINI
        BtnCharacterINI.Parent = GroupBox1
        BtnCharacterINI.Text = "..."
        BtnCharacterINI.Size = New Size(24, 19)
        BtnCharacterINI.Location = New Point(245, 132)
        BtnCharacterINI.Visible = True
        BtnCharacterINI.Anchor = CType(System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)
        AddHandler BtnCharacterINI.Click, AddressOf BtnCharacterINI_click
        'LblBotController
        LblBotController.Text = "Bot Controller Name"
        LblBotController.Parent = GroupBox1
        LblBotController.Size = New Size(101, 13)
        LblBotController.Location = New Point(17, 155)
        LblBotController.Visible = True
        LblBotController.Anchor = CType(System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'TxtbxBotController
        TxtbxBotController.Parent = GroupBox1
        TxtbxBotController.Size = New Size(207, 20)
        TxtbxBotController.Location = New Point(20, 171)
        TxtbxBotController.Visible = True
        TxtbxBotController.Anchor = CType(System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'Value 2
        'RadioGoMapAcropolis
        RadioGoMapAcropolis.Parent = GroupBox1
        RadioGoMapAcropolis.Text = "Acropolis"
        RadioGoMapAcropolis.Size = New Size(68, 17)
        RadioGoMapAcropolis.Location = New Point(6, 66)
        RadioGoMapAcropolis.Visible = False
        RadioGoMapAcropolis.Anchor = CType(System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'Value 1
        'RadioGoMapAllgeriaIsland
        RadioGoMapAllgeriaIsland.Parent = GroupBox1
        RadioGoMapAllgeriaIsland.Text = "Allgeria Island"
        RadioGoMapAllgeriaIsland.Size = New Size(68, 17)
        RadioGoMapAllgeriaIsland.Location = New Point(6, 89)
        RadioGoMapAllgeriaIsland.Visible = False
        RadioGoMapAllgeriaIsland.Anchor = CType(System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'Value 4
        'RadioGoDreamURL
        RadioGoDreamURL.Parent = GroupBox1
        RadioGoDreamURL.Text = "Dream URL"
        RadioGoDreamURL.Size = New Size(90, 17)
        RadioGoDreamURL.Location = New Point(6, 116)
        RadioGoDreamURL.Visible = False
        RadioGoDreamURL.Anchor = CType(System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'TxtbxCDreamURL
        TxtbxDreamURL.Parent = GroupBox1
        TxtbxDreamURL.Text = "furc://"
        TxtbxDreamURL.Size = New Size(174, 20)
        TxtbxDreamURL.Location = New Point(95, 115)
        TxtbxDreamURL.Visible = False
        TxtbxDreamURL.Anchor = CType(System.Windows.Forms.AnchorStyles.Right _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        'Value 3
        'RadioGoMapNone
        RadioGoMapNone.Parent = GroupBox1
        RadioGoMapNone.Text = "None"
        RadioGoMapNone.Size = New Size(68, 17)
        RadioGoMapNone.Location = New Point(6, 139)
        RadioGoMapNone.Visible = False
        RadioGoMapNone.Anchor = CType(System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Left _
            , System.Windows.Forms.AnchorStyles)

        Button1.Enabled = False
        Button2.Enabled = True
        WizIndex = 1
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

    Public Sub BtnFileLocation_click(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button)

        With FolderBrowserDialog1
            '.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "/Silver Monkey"

            If .ShowDialog = Windows.Forms.DialogResult.OK Then

                TxtbxFilelocation.Text = .SelectedPath
            End If
        End With
    End Sub
    Public Sub BtnCharacterINI_click(sender As Object, e As EventArgs)

        With OpenFileDialog1
            '.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "/Silver Monkey"

            .InitialDirectory = FurcPath.GetFurcadiaCharactersPath()
            If .ShowDialog = Windows.Forms.DialogResult.OK Then

                TxtbxCharacterINI.Text = .FileName
            End If
        End With

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
End Class
