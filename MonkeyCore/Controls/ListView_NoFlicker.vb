﻿Imports System.Windows.Forms
Namespace Controls
    Public Class ListView_NoFlicker
        Inherits ListView

        Public Sub New()
            Me.DoubleBuffered = True

        End Sub
    End Class
End Namespace