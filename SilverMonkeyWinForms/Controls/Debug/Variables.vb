Imports System.ComponentModel
Imports System.Windows.Forms
Imports Monkeyspeak
Imports MonkeyCore.Logging

''' <summary>
'''
''' </summary>
''' <seealso cref="System.Windows.Forms.Form" />
Public Class Variables
    Inherits Form

#Region "Private Fields"

    Private alarm As Threading.Timer
    Private MsPage As Page

    'Dim RefreshList As New Thread(AddressOf RefreshMe)
    Dim Lock As New Object

    Private Shared Shadows IsDisposed As Boolean

    Public Sub New(ByRef msPage As Page)

        ' This call is required by the designer.
        InitializeComponent()
        Me.MsPage = msPage
        ' Add any initialization after the InitializeComponent() call.

    End Sub

#End Region

#Region "Private Delegates"

    Private Delegate Sub dlgUpdateUI()

    ''' <summary>
    ''' Send text to DebugWindow Delegate
    ''' </summary>
    ''' <param name="text"></param>
    Private Delegate Sub LogMessageDelegate(text As Object)

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Updates the variables.
    ''' </summary>
    Public Sub UpdateVariables()

        If ListView1.InvokeRequired Then
            Dim d As New dlgUpdateUI(AddressOf UpdateVariables)
            ListView1?.Invoke(d)
        Else

            SyncLock Lock

                If MsPage Is Nothing Then
                    Exit Sub
                End If
                For i As Integer = 0 To MsPage.Scope.Count - 1
                    Dim Var = MsPage.Scope.Item(i)

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
        Using SetVars As New SetVariables(MsPage)
            Try
                If ListView1.Items.Count > 0 Then
                    SetVars.VarName = ListView1.SelectedItems.Item(0).SubItems(0).Text
                    'SetVars.Show()
                    'SetVars.Activate()

                    If SetVars.ShowDialog = DialogResult.OK Then
                        UpdateVariables()

                    End If
                End If
            Catch
            End Try

        End Using
    End Sub

    Private Sub Tick(ByVal state As Object)
        UpdateVariables()
    End Sub

    Private Sub Variables_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If Not IsNothing(Me.alarm) Then Me.alarm.Dispose()
    End Sub

    Public Sub SendLogsToDemugWindow(Log As Object)
        If IsDisposed Then Exit Sub
        If Me.InvokeRequired Then
            ErrorLogTxtBx.Invoke(New LogMessageDelegate(AddressOf SendLogsToDemugWindow), Log)
        ElseIf Log.GetType() Is GetType(LogMessage) Then
            ErrorLogTxtBx.AppendText(DirectCast(Log, LogMessage).message + Environment.NewLine)
        ElseIf Log.GetType() Is GetType(String) Then
            ErrorLogTxtBx.AppendText(Log.ToString + Environment.NewLine)
        End If
    End Sub

    Private Sub Variables_Load(sender As Object, e As EventArgs) Handles Me.Load
        IsDisposed = False
        Logger.DebugEnabled = True

    End Sub

    Private Sub Variables_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Logger.DebugEnabled = False
    End Sub

    Private Sub Variables_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        IsDisposed = True
    End Sub

#End Region

End Class