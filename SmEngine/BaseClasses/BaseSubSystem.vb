Imports Furcadia.Net

<CLSCompliant(True)>
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

#Region "Private Delegates"

    Private Delegate Sub ParseChannel(obj As ParseChannelArgs, e As NetServerEventArgs)

#End Region

#Region "Private Events"

    Private Event ChannelParsed As ParseChannel

#End Region

#Region "Public Methods"

    Public Overridable Sub ParseServerChannel(ServerData As String, ByRef Handled As Boolean)

    End Sub

#End Region

End Class