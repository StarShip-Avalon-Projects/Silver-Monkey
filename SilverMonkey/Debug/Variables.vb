Imports System.Windows.Forms
Imports SilverMonkeyEngine

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

#End Region

#Region "Public Methods"

    Public Sub updateVariables()

        If ListView1.InvokeRequired Then
            Dim d As New dlgUpdateUI(AddressOf updateVariables)
            ListView1?.Invoke(d)
        Else

            SyncLock Lock

                For i As Integer = 0 To FurcadiaSession.MSpage.Scope.Count - 1
                    Dim Var As Monkeyspeak.Variable = FurcadiaSession.MSpage.Scope.Item(i)

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
        updateVariables()

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
        updateVariables()
    End Sub

    Private Sub Variables_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If Not IsNothing(Me.alarm) Then Me.alarm.Dispose()
    End Sub

    Private Sub Variables_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

#End Region

End Class