
Namespace Controls
    Public Class ListBox_NoFlicker

        Inherits Windows.Forms.ListBox

        Public Sub New()
            MyBase.New()
            Me.DoubleBuffered = True
        End Sub
    End Class
End Namespace
