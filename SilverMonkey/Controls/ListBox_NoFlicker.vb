Public Class ListBox_NoFlicker

    Inherits ListBox

    Public Sub New()
        MyBase.New()
        Me.DoubleBuffered = True
    End Sub
End Class
