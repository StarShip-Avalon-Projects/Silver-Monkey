Namespace Interfaces

    Public Interface ImsPlugin

#Region "Public Properties"

        ReadOnly Property Description() As String

        Property enabled As Boolean

        Property MsPage As Monkeyspeak.Page
        ReadOnly Property Name() As String
        ReadOnly Property Version() As String

#End Region

#Region "Public Methods"

        Sub Initialize(ByVal Host As Interfaces.ImsHost)

        Function MessagePump(ByRef ServerInstruction As String) As Boolean

        'Property Player As FURRE
        Sub Start()

#End Region

    End Interface

End Namespace