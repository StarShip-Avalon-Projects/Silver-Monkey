﻿Imports System.Windows.Forms

Namespace Controls

    Public Class ListBox_NoFlicker

        Inherits ListBox

#Region "Public Constructors"

        Public Sub New()
            MyBase.New()
            Me.DoubleBuffered = True
        End Sub

#End Region

    End Class

End Namespace