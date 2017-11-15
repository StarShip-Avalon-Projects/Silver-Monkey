Imports System.Windows.Forms
Imports SilverMonkeyEngine
Imports Monkeyspeak.Logging
Imports System.ComponentModel

Public Class Variables
    Inherits Form

#Region "Private Fields"

    Private alarm As Threading.Timer
    Private FurcadiaSession As BotSession

    'Dim RefreshList As New Thread(AddressOf RefreshMe)
    Dim Lock As New Object

#End Region

#Region "Private Delegates"

    Private Delegate Sub dlgUpdateUI()

    ''' <summary>
    ''' Send text to DebugWindow Delegate
    ''' </summary>
    ''' <param name="text"></param>
    Private Delegate Sub SendTextDelegate(text As String)

    ''' <summary>
    ''' Send text to DebugWindow Delegate
    ''' </summary>
    ''' <param name="text"></param>
    Private Delegate Sub LogMessageDelegate(text As LogMessage)

#End Region

#Region "Public Methods"

    Public Sub UpdateVariables()

        If ListView1.InvokeRequired Then
            Dim d As New dlgUpdateUI(AddressOf UpdateVariables)
            ListView1?.Invoke(d)
        Else

            SyncLock Lock

                If FurcadiaSession Is Nothing OrElse FurcadiaSession.MSpage Is Nothing Then
                    Exit Sub
                End If
                For i As Integer = 0 To FurcadiaSession.MSpage.Scope.Count - 1
                    Dim Var = FurcadiaSession.MSpage.Scope.Item(i)

                    Dim Variable() As String = {"", "", ""}
                    Variable(0) = Var.Name.ToString
                    Variable(1) = Var.IsConstant.ToString
                    If Not IsNothing(Var.Value) Then
                        Variable(2) = Var.Value.ToString
                    End If
                    Dim itm As ListViewItem = New ListViewItem(Variable)
                    If ListView1.Items.Count > i Then

                        If ListView1.Items.Item(i).SubItems(2).Text <> itm.SubItems.Item(2).Text Then
                            ListView1.Items.Item(i) = itm
                        End If
                    Else
                        ListView1.Items.Add(itm)
                    End If

                    ' ListView1.Groups.

                Next
            End SyncLock
        End If
    End Sub

#End Region

#Region "Private Methods"

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        UpdateVariables()

    End Sub

    Private Sub ChkBxRefresh_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ChkBxRefresh.CheckedChanged
        If ChkBxRefresh.Checked Then
            Me.alarm = New Threading.Timer(AddressOf Tick, Nothing, 1000, 1000)
        Else
            Me.alarm.Dispose()
        End If
    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As System.EventArgs) Handles ListView1.DoubleClick
        With SetVariables
            Try
                If ListView1.Items.Count > 0 Then
                    .VarName = ListView1.SelectedItems.Item(0).SubItems(0).Text
                    .Show()
                End If
            Catch
            End Try

        End With
    End Sub

    Private Sub Tick(ByVal state As Object)
        UpdateVariables()
    End Sub

    Private Sub Variables_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If Not IsNothing(Me.alarm) Then Me.alarm.Dispose()
    End Sub

    Public Shared Sub SendLogsToDemugWindow(Log As LogMessage)
        If ErrorLogTxtBx.InvokeRequired Then
            ErrorLogTxtBx.Invoke(New LogMessageDelegate(AddressOf SendLogsToDemugWindow), Log)
        Else
            ErrorLogTxtBx.AppendText(Log.message + Environment.NewLine)
        End If
    End Sub

    Public Shared Sub SendLogsToDemugWindow(Log As String)
        If ErrorLogTxtBx.InvokeRequired Then
            ErrorLogTxtBx.Invoke(New SendTextDelegate(AddressOf SendLogsToDemugWindow), Log)
        Else
            ErrorLogTxtBx.AppendText(Log + Environment.NewLine)
        End If
    End Sub

    Private Sub Variables_Load(sender As Object, e As EventArgs) Handles Me.Load
        Logger.DebugEnabled = True
    End Sub

    Private Sub Variables_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Logger.DebugEnabled = False

    End Sub

#End Region

End Class