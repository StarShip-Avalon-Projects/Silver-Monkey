Imports System.Windows.Forms

Public Class InputBoxFrm

#Region "Public Properties"

    Public Property Section As MS_Edit.TDSSegment = New MS_Edit.TDSSegment

#End Region

#Region "Private Methods"

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Section.Title = SectionTitle.Text
        Section.lines.Add("")
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

#End Region

End Class