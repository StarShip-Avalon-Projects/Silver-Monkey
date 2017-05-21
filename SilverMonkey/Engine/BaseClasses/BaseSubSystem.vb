Public Class BaseSubSystem

#Region "Private Fields"

    Private ParentSession As BotSession

#End Region

#Region "Public Constructors"

    Sub New(ByRef FurcSession As BotSession)
        ParentSession = FurcSession
    End Sub

#End Region

#Region "Public Properties"

    Public Property FurcadiaSession As BotSession
        Get
            Return ParentSession
        End Get
        Set(value As BotSession)
            ParentSession = value
        End Set
    End Property

#End Region

End Class