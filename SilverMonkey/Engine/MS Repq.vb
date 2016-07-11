
Imports MonkeyCore

Public Class MS_Repq
    Inherits Monkeyspeak.Libraries.AbstractBaseLibrary
    Private writer As TextBoxWriter = Nothing


    Public Sub New()
        writer = New TextBoxWriter(Variables.TextBox1)

    End Sub



End Class
