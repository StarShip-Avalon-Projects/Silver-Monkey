Namespace Engine.Libraries
    Public Class WebException
        Inherits Exception

#Region "Public Constructors"

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

#End Region

    End Class
End Namespace