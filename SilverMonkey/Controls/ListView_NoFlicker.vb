Public Class ListView_NoFlicker
    Inherits ListView

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        Me.DoubleBuffered = True

    End Sub
End Class