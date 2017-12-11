Imports Furcadia.Net
Imports Furcadia.Net.DreamInfo

Namespace Interfaces

    Public Class smHost
        Implements ImsHost

#Region "Private Fields"

        Private FurcadiaSession As BotSession

#End Region

#Region "Public Properties"

        Public ReadOnly Property BotName As String Implements Interfaces.ImsHost.BotName
            Get
                Return FurcadiaSession.ConnectedFurre.Name
            End Get
        End Property

        Public Property data As String Implements Interfaces.ImsHost.Data
            Get
                Return Nothing
                'Return FurcSession.
            End Get
            Set(value As String)
                ' FurcSession.serverData = value
            End Set
        End Property

        Public Property Dream As Dream Implements Interfaces.ImsHost.Dream
            Get
                Return FurcadiaSession.Dream
            End Get
            Set(value As Dream)
                FurcadiaSession.Dream = value
            End Set

        End Property

        Public WriteOnly Property MsPage() As Monkeyspeak.Page Implements Interfaces.ImsHost.MsPage
            Set(value As Monkeyspeak.Page)
                FurcadiaSession.MSpage = value
            End Set
        End Property

        Public ReadOnly Property Player() As Furre Implements Interfaces.ImsHost.Player
            Get
                Return FurcadiaSession.Player
            End Get

        End Property

#End Region

#Region "Public Methods"

        Public Sub New(Session As BotSession)
            FurcadiaSession = Session
        End Sub

        Sub logError(ByRef Ex As System.Exception, ByRef ObjectThrowingError As Object) Implements Interfaces.ImsHost.logError

        End Sub

        Public Sub SendClientMessage(Tag As String, data As String) Implements Interfaces.ImsHost.SendClientMessage
            SendClientMessage(Tag, data)
        End Sub

        Sub sendServer(ByRef var As String) Implements Interfaces.ImsHost.sendServer

        End Sub

        Sub Start(ByRef page As Monkeyspeak.Page) Implements Interfaces.ImsHost.start
            FurcadiaSession.MSpage = page
        End Sub

#End Region

    End Class

End Namespace