﻿Imports System.IO

Public NotInheritable Class AboutBox2

#Region "Private Fields"

    Private Const HelpFile As String = "Monkey_Speak_Editor_Help.chm"

#End Region

#Region "Private Methods"

    Private Sub AboutBox2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle)
        ' Initialize all of the text displayed on the About Box.
        ' TODO: Customize the application's assembly information in the
        '       "Application" pane of the project properties dialog (under
        '       the "Project" menu).
        Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = My.Application.Info.Description
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HelpFile)) Then
            Help.ShowHelp(Me, HelpFile, "html/baa001f9-5b6f-4416-bd04-834030e3034f.htm")
        End If
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

#End Region

End Class