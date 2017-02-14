'(5:210) - (5:220)

Imports MonkeyCore

Public Class MS_Quotes
    Inherits Monkeyspeak.Libraries.AbstractBaseLibrary
    Private writer As TextBoxWriter = Nothing

    Public Sub New()
        writer = New TextBoxWriter(Variables.TextBox1)

        '(5: ) Use file {...} as quote list and put line # into Variable %.
        '(5: ) Use File {...} as quote list and put the total of lines into variable %
    End Sub

End Class