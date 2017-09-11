Imports MonkeySpeak
Imports Furcadia.Net
Imports System.Text.RegularExpressions
Imports Furcadia.Base95

Public Class SoundWave
    Implements SilverMonkey.Interfaces.msPlugin

    Private msHost As SilverMonkey.Interfaces.msHost

    Public Sub Initialize(ByVal Host As SilverMonkey.Interfaces.msHost) Implements SilverMonkey.Interfaces.msPlugin.Initialize
        msHost = Host
    End Sub

    Public ReadOnly Property Name() As String Implements SilverMonkey.Interfaces.msPlugin.Name
        Get
            Return "Sound Wave"
        End Get
    End Property
    Public ReadOnly Property Description() As String Implements SilverMonkey.Interfaces.msPlugin.Description
        Get
            Return "Plays sound files"
        End Get
    End Property

    Public ReadOnly Property Version() As String Implements SilverMonkey.Interfaces.msPlugin.Version
        Get
            Dim VersionInfo As Version = System.Reflection.Assembly.GetExecutingAssembly.GetName.Version
            Return VersionInfo.Major & "." & VersionInfo.Minor & "." & VersionInfo.Build & "." & VersionInfo.Revision
        End Get
    End Property

    Public Property Enabled As Boolean Implements SilverMonkey.Interfaces.msPlugin.enabled
#Region "Global Properties"

    Public Player As FURRE

    Private MSpage As Monkeyspeak.Page
    Public Property Page As Monkeyspeak.Page Implements SilverMonkey.Interfaces.msPlugin.Page
        Get
            Return MSpage
        End Get
        Set(value As Monkeyspeak.Page)

            MSpage = value
            msHost.Page = MSpage
        End Set
    End Property
    Private msDream As DREAM
    Public Property Dream As DREAM
        Get
            Return msHost.Dream
        End Get
        Set(value As DREAM)
            msHost.Dream = value
        End Set
    End Property

#End Region

    Public Sub Start() Implements SilverMonkey.Interfaces.msPlugin.Start
        '(0:x) When the bot picks up or drops an object
        'Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Cause, 2000,
        '    Function()
        '        Return Player.FloorObjectOld = Player.PawObjectCurrent And Player.PawObjectOld = Player.FloorObjectCurrent
        '    End Function, "(0:2000) When the bot picks up or drops an object")

        ''(0:2001) When the bot picks up or drops the object #,
        'Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Cause, 2001,
        '    AddressOf PickUpObjectNumber, "(0:2001) When the bot picks up or drops the object #,")

        '(52010)=0,0,"(5:2010) play the wave file {...}."
        '(5:2011)=0,0,"(5:2011) play the wave file {...} in a loop."
        '(5:2012)=0,0,"(5:2012) stop playing the sound file."
    End Sub



    Function MessagePump(ByRef ServerInstruction As String) As Boolean Implements SilverMonkey.Interfaces.msPlugin.MessagePump
        'Set Object At Feet
        If ServerInstruction.StartsWith("%") Then
            Player = NametoFurre(msHost.BotName, True)
            Player.FloorObjectCurrent = ConvertFromBase95(ServerInstruction.Substring(1))
            Page.Execute(2000, 2001)
            msHost.Player = Player
            Furcadia.Net.DREAM.List(Player.ID) = Player
            Return True
            'Set Object In Paws
        ElseIf ServerInstruction.StartsWith("^") Then
            Player = NametoFurre(msHost.BotName, True)
            Player.PawObjectCurrent = ConvertFromBase95(ServerInstruction.Substring(1))
            Page.Execute(2000, 2001)
            msHost.Player = Player
            Furcadia.Net.DREAM.List(Player.ID) = Player
            Return True
        End If
        Return False
    End Function

#Region "Helper Functions"
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

    Public Function IsBot(ByRef p As FURRE) As Boolean
        Return p.ShortName = msHost.BotName.ToFurcShortName
    End Function

    Private Function fIDtoFurre(ByRef ID As UInteger) As FURRE
        Dim Character As KeyValuePair(Of UInteger, FURRE)
        For Each Character In Dream.List
            If Character.Value.ID = ID Then
                Return Character.Value
            End If
        Next
    End Function

    Public Function NametoFurre(ByRef sname As String, ByRef UbdateMSVariableName As Boolean) As FURRE
        Dim p As New FURRE
        p.Name = sname
        For Each Character As KeyValuePair(Of UInteger, FURRE) In Dream.List
            If Character.Value.ShortName = sname.ToFurcShortName Then
                p = Character.Value
                Exit For
            End If
        Next
        If UbdateMSVariableName Then Page.SetVariable(VarPrefix & "NAME", sname, True)
        Return p
    End Function

#End Region
End Class
