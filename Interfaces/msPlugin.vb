Imports Monkeyspeak
Imports Furcadia.Net

Public Interface msPlugin

    Sub Initialize(ByVal Host As SilverMonkey.Interfaces.msHost)

    ReadOnly Property Name() As String
    ReadOnly Property Description() As String
    ReadOnly Property Version() As String
    Property enabled As Boolean
    'Property Player As FURRE

    Property Page As Monkeyspeak.Page

    Sub Start()

    Function MessagePump(ByRef ServerInstruction As String) As Boolean


End Interface
