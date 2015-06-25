Public Class TextBox_NoFlicker
    Inherits TextBox

    Public Sub New()
        MyBase.New()
        Me.DoubleBuffered = True
    End Sub
End Class
