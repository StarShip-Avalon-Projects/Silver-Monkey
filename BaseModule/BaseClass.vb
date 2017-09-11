Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Furcadia.Text.Base95
Imports SilverMonkeyEngine
Imports SilverMonkeyEngine.Engine

Public Class BaseClass
    Implements Interfaces.ImsPlugin

#Region "Private Fields"

    Private msHost As Interfaces.ImsHost

#End Region

#Region "Public Properties"

    Public ReadOnly Property Description() As String Implements Interfaces.ImsPlugin.Description
        Get
            Return "Base Class for building Modules"
        End Get
    End Property

    Public Property Enabled As Boolean Implements Interfaces.ImsPlugin.enabled

    Public ReadOnly Property Name() As String Implements Interfaces.ImsPlugin.Name
        Get
            Return "Base Module"
        End Get
    End Property

    Public ReadOnly Property Version() As String Implements Interfaces.ImsPlugin.Version
        Get
            Dim VersionInfo As Version = System.Reflection.Assembly.GetExecutingAssembly.GetName.Version
            Return VersionInfo.Major & "." & VersionInfo.Minor & "." & VersionInfo.Build & "." & VersionInfo.Revision
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Sub Initialize(ByVal Host As Interfaces.ImsHost) Implements Interfaces.ImsPlugin.Initialize
        msHost = Host
    End Sub

#End Region

#Region "Global Properties"

    Public Player As FURRE

    Private _MSpage As MonkeySpeakPage
    Private msDream As DREAM

    Public ReadOnly Property Dream As DREAM
        Get
            Return msHost.Dream
        End Get

    End Property

    Public Property MsPage As MonkeySpeakPage Implements Interfaces.ImsPlugin.MsPage
        Get
            Return _MSpage
        End Get
        Set(value As MonkeySpeakPage)

            _MSpage = value
            msHost.MsPage = _MSpage
        End Set
    End Property

#End Region

    Function MessagePump(ByRef ServerInstruction As String) As Boolean Implements Interfaces.ImsPlugin.MessagePump
        'Set Object At Feet
        If ServerInstruction.StartsWith("%") Then
            Player = NameToFurre(msHost.BotName, True)
            Player.FloorObjectCurrent = ConvertFromBase95(ServerInstruction.Substring(1))
            MsPage.Execute(2000, 2001)

            Return True
            'Set Object In Paws
        ElseIf ServerInstruction.StartsWith("^") Then
            Player = NameToFurre(msHost.BotName, True)
            Player.PawObjectCurrent = ConvertFromBase95(ServerInstruction.Substring(1))
            MsPage.Execute(2000, 2001)
            Return True
        End If
        Return False
    End Function

    Public Sub Start() Implements Interfaces.ImsPlugin.Start
        '(0:x) When the bot picks up or drops an object
        'Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Cause, 2000,
        '    Function()
        '        Return Player.FloorObjectOld = Player.PawObjectCurrent And Player.PawObjectOld = Player.FloorObjectCurrent
        '    End Function, "(0:2000) When the bot picks up or drops an object")

        ''(0:2001) When the bot picks up or drops the object #,
        'Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Cause, 2001,
        '    AddressOf PickUpObjectNumber, "(0:2001) When the bot picks up or drops the object #,")

    End Sub

#Region "Helper Functions"

    Public Function IsBot(ByRef p As FURRE) As Boolean
        Return p.ShortName = msHost.BotName.ToFurcShortName
    End Function

    Public Function NameToFurre(ByRef sname As String, ByRef UbdateMSVariableName As Boolean) As FURRE
        Dim p As New FURRE
        p.Name = sname
        For Each Character As FURRE In Dream.FurreList
            If Character.ShortName = sname.ToFurcShortName Then
                p = Character
                Exit For
            End If
        Next
        If UbdateMSVariableName Then MsPage.SetVariable(VarPrefix & "NAME", sname, True)
        Return p
    End Function

    Public Function ReadVariableOrNumber(ByVal reader As Monkeyspeak.TriggerReader, Optional addIfNotExist As Boolean = False) As Double
        Dim result As Double = 0
        If reader.PeekVariable Then
            Dim value As String = reader.ReadVariable(addIfNotExist).Value.ToString
            Double.TryParse(value, result)
        ElseIf reader.PeekNumber Then
            result = reader.ReadNumber
        End If
        Return result
    End Function

    Private Function fIDtoFurre(ByRef ID As UInteger) As FURRE

        For Each Character As FURRE In Dream.FurreList
            If Character.ID = ID Then
                Return Character
            End If
        Next
        Return Nothing
    End Function

#End Region

End Class