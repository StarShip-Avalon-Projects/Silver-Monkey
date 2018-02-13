Imports System.Windows.Forms

Namespace Controls

    Public Class ListView_NoFlicker
        Inherits ListView

#Region "Public Constructors"

        Public Sub New()
            Me.DoubleBuffered = True

        End Sub

#End Region

    End Class

End Namespace