Public Class SetVariables

    Public Var As Monkeyspeak.Variable
    Private _VarName As String
    Public Property VarName As String
        Get
            Return _VarName
        End Get
        Set(value As String)
            _VarName = value
        End Set
    End Property
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Var.Value = TxtBxValue.Text
        FurcSession.MainEngine.MSpage.SetVariable(VarName, Var.Value, Var.IsConstant)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SetVariables_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Var = FurcSession.MainEngine.MSpage.GetVariable(VarName)
        If Var.IsConstant Then
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If

        TxtBxName.Text = Var.Name.ToString
        TxtBxValue.Text = Var.Value.ToString
    End Sub
End Class