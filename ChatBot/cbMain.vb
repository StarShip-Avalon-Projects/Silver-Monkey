Imports System.IO
Imports Conversive.Verbot5
Imports Furcadia.Net.Dream

Public Class cbMain
    Inherits Form

#Region "Public Fields"

    Public Player As FURRE

#End Region

#Region "Private Fields"

    Private _kb As KnowledgeBase = New KnowledgeBase()
    Dim kbi As KnowledgeBaseItem = New KnowledgeBaseItem()
    Private state As State
    Private stCKBFileFilter As String = "Verbot Knowledge Bases (*.vkb)|*.vkb"
    Private stFormName As String = "Verbot SDK Windows App Sample"
    Private verbot As Verbot5Engine

#End Region

#Region "Public Constructors"

    Public Sub New()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        verbot = New Verbot5Engine()
        state = New State()

        openFileDialog1.Filter = stCKBFileFilter
        openFileDialog1.InitialDirectory = MonkeyCore.Paths.SilverMonkeyBotPath
        Text = stFormName
    End Sub

#End Region

#Region "Public Properties"

    Public Property kb As KnowledgeBase
        Get
            Return _kb
        End Get
        Set(value As KnowledgeBase)

            _kb = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    'runProgram(string filename, string args)
    Public Function splitOnFirstUnquotedSpace(text As String) As String()
        Dim pieces As String() = New String(1) {}
        Dim index As Integer = -1
        Dim isQuoted As Boolean = False
        'find the first unquoted space
        For i As Integer = 0 To text.Length - 1
            If text(i) = """"c Then
                isQuoted = Not isQuoted
            ElseIf text(i) = " "c AndAlso Not isQuoted Then
                index = i
                Exit For
            End If
        Next

        'break up the string
        If index <> -1 Then
            pieces(0) = text.Substring(0, index)
            pieces(1) = text.Substring(index + 1)
        Else
            pieces(0) = text
            pieces(1) = ""
        End If

        Return pieces
    End Function

#End Region

#Region "Private Methods"

    Private Sub cbMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Player = New FURRE
    End Sub

    Private Sub exitMenuItem_Click(sender As Object, e As System.EventArgs) Handles exitMenuItem.Click
        Close()
    End Sub

    Private Sub getReply()
        Dim stInput As String = inputTextBox.Text.Trim()
        inputTextBox.Text = ""
        If state.Vars.ContainsKey("botname") Then
            state.Vars.Item("botname") = TxtBxBotName.Text
        Else
            state.Vars.Add("botname", TxtBxBotName.Text)
        End If
        Dim reply As Reply = verbot.GetReply(Player, stInput, state)
        If reply IsNot Nothing Then
            outputTextBox.Text = reply.Text
            parseEmbeddedOutputCommands(reply.AgentText)
            runProgram(reply.Cmd)
        Else
            outputTextBox.Text = "No reply found."
        End If
    End Sub

    Private Sub getReplyButton_Click(sender As Object, e As System.EventArgs) Handles getReplyButton.Click
        getReply()
    End Sub

    Private Sub inputTextBox_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles inputTextBox.KeyPress
        If e.KeyChar = ControlChars.Cr Then
            e.Handled = True
            getReply()
        End If
    End Sub

    Private Sub loadMenuItem_Click(sender As Object, e As EventArgs) Handles loadMenuItem.Click
        With openFileDialog1
            If .ShowDialog() = DialogResult.OK Then
                Dim xToolbox As XMLToolbox = New XMLToolbox(GetType(KnowledgeBase))
                kb = CType(xToolbox.LoadXML(.FileName), KnowledgeBase)
                kbi.Filename = Path.GetFileName(.FileName)
                kbi.Fullpath = Path.GetDirectoryName(.FileName) + "\"
                verbot.AddKnowledgeBase(kb, kbi)
                state.CurrentKBs.Clear()
                state.CurrentKBs.Add(.FileName)
            End If
        End With
    End Sub

    Private Sub parseEmbeddedOutputCommands(text As String)
        Dim startCommand As String = "<"
        Dim endCommand As String = ">"

        Dim start As Integer = text.IndexOf(startCommand)
        Dim [end] As Integer = -1

        While start <> -1
            [end] = text.IndexOf(endCommand, start)
            If [end] <> -1 Then
                Dim command As String = text.Substring(start + 1, [end] - start - 1).Trim()
                If command <> "" Then
                    runEmbeddedOutputCommand(command)
                End If
            End If
            start = text.IndexOf(startCommand, start + 1)
        End While
    End Sub

    'parseEmbeddedOutputCommands(string text)
    Private Sub runEmbeddedOutputCommand(command As String)
        Dim spaceIndex As Integer = command.IndexOf(" ")

        Dim [function] As String
        Dim args As String
        If spaceIndex = -1 Then
            [function] = command.ToLower()
            args = ""
        Else
            [function] = command.Substring(0, spaceIndex).ToLower()
            args = command.Substring(spaceIndex + 1)
        End If

        Try
            Select Case [function]
                Case "quit", "exit"

                    Close()
                    Exit Select
                Case "run"
                    runProgram(args)
                    Exit Select
                Case Else
                    Exit Select
                    'switch
            End Select
        Catch
        End Try
    End Sub

    'runOutputEmbeddedCommand(string command)
    Private Sub runProgram(command As String)
        Try
            Dim pieces As String() = splitOnFirstUnquotedSpace(command)

            Dim proc As New System.Diagnostics.Process()
            proc.StartInfo.FileName = pieces(0).Trim()
            proc.StartInfo.Arguments = pieces(1)
            proc.StartInfo.CreateNoWindow = True
            proc.StartInfo.UseShellExecute = True
            proc.Start()
        Catch
        End Try
    End Sub

#End Region

    'splitOnFirstUnquotedSpace(string text)
End Class