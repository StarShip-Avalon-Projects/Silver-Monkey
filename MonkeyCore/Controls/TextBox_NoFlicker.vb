Imports System.Windows.Forms
Namespace Controls
    Public Class TextBox_NoFlicker
        Inherits TextBox

#Region "Public Constructors"

        Public Sub New()
            MyBase.New()
            Me.DoubleBuffered = True
        End Sub

#End Region

    End Class
End Namespace