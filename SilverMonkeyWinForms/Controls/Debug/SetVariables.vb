Imports System.Windows.Forms
Imports Monkeyspeak

Public Class SetVariables
    Inherits Form

    Sub New(ByRef msPage As Page)

        ' This call is required by the designer.
        InitializeComponent()
        Me.MsPage = msPage
        ' Add any initialization after the InitializeComponent() call.

    End Sub

#Region "Public Fields"

    Public Var As Monkeyspeak.IVariable

#End Region

#Region "Private Fields"

    Private _VarName As String

    Private MsPage As Page

#End Region

#Region "Public Properties"

    Public Property VarName As String
        Get
            Return _VarName
        End Get
        Set(value As String)
            _VarName = value
        End Set
    End Property

#End Region

#Region "Private Methods"

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Var.Value = TxtBxValue.Text
        MsPage.RemoveVariable(VarName)
        '   new Variable($"%{reader.ReadString()}",false)
        MsPage.SetVariable(New Variable(VarName, Var.Value, Var.IsConstant))
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub SetVariables_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Var = MsPage.GetVariable(VarName)

        If Var.IsConstant Then
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()
            Exit Sub
        End If

        TxtBxName.Text = Var.Name.ToString
        TxtBxValue.Text = Var.Value.ToString
    End Sub

#End Region

End Class