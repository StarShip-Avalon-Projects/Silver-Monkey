Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Furcadia.Text.Base95
Imports Monkeyspeak
Imports SilverMonkeyEngine
Imports SilverMonkeyEngine.Engine.Libraries.MonkeySpeakLibrary

''' <summary>
''' </summary>
Public Class TheClaaaw
    Inherits Monkeyspeak.Libraries.BaseLibrary
    Implements Interfaces.ImsPlugin

#Region "Private Fields"

    Private msHost As Interfaces.ImsHost

#End Region

#Region "Public Properties"

    Public ReadOnly Property Description() As String Implements Interfaces.ImsPlugin.Description
        Get
            Return "Allows the bot to work with own paw and feet objects."
        End Get
    End Property

    Public Property Enabled As Boolean Implements Interfaces.ImsPlugin.enabled

    Public ReadOnly Property Name() As String Implements Interfaces.ImsPlugin.Name
        Get
            Return "The Claaaw"
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

    '(5:2001) pick up the object at the bots feet.
    Function GetObject(reader As TriggerReader) As Boolean
        Try
            msHost.sendServer("`get")
        Catch ex As Exception
            msHost.logError(ex, Me)
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' </summary>
    ''' <param name="Host">
    ''' </param>
    Public Sub Initialize(ByVal Host As Interfaces.ImsHost) Implements Interfaces.ImsPlugin.Initialize
        msHost = Host
    End Sub

#End Region

#Region "Global Properties"

    Public Player As Furre

    Private _MSpage As Monkeyspeak.Page
    Private msDream As DREAM

    Public ReadOnly Property Dream As DREAM
        Get
            Return msHost.Dream
        End Get
    End Property

    Public Property MsPage As Page Implements Interfaces.ImsPlugin.MsPage
        Get
            Return _MSpage
        End Get
        Set(value As Monkeyspeak.Page)

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
            ' msHost.Player = Player
            Dream.FurreList(Player) = Player
            ServerInstruction = ServerInstruction
            Return True
            'Set Object In Paws
        ElseIf ServerInstruction.StartsWith("^") Then
            Player = NameToFurre(msHost.BotName, True)
            Player.PawObjectCurrent = ConvertFromBase95(ServerInstruction.Substring(1))
            MsPage.Execute(2000, 2001)
            ' msHost.Player = Player
            Dream.FurreList(Player) = Player
            ServerInstruction = ServerInstruction
            Return True
        End If
        ServerInstruction = ServerInstruction
        Return False
    End Function

    '(1:2003) and the bot is not standing on object #,
    Function NotObjectAtFeet(reader As TriggerReader) As Boolean
        Return Not ObjectAtFeet(reader)
    End Function

    '(1:2001) and the bot doesn't have object # in their paws,
    Function NotObjectInPaws(reader As TriggerReader) As Boolean
        Return Not ObjectInPaws(reader)
    End Function

    '(1:2002) and the bot is standing on object #,
    Function ObjectAtFeet(reader As TriggerReader) As Boolean
        Try
            Dim Obj As Double = ReadVariableOrNumber(reader)
            Return Player.FloorObjectCurrent = Obj
        Catch ex As Exception
            msHost.logError(ex, Me)
            Return False
        End Try
    End Function

    '(1:2000) and the bot has object # in their paws,
    Function ObjectInPaws(reader As TriggerReader) As Boolean
        Try
            Dim Obj As Double = ReadVariableOrNumber(reader)
            Return Player.PawObjectCurrent = Obj
        Catch ex As Exception
            msHost.logError(ex, Me)
            Return False
        End Try
    End Function

    Function PickUpObjectNumber(reader As TriggerReader) As Boolean
        Try
            Dim obj As Double = ReadVariableOrNumber(reader)
            Return Player.FloorObjectOld = Player.PawObjectCurrent And Player.PawObjectOld = Player.FloorObjectCurrent And obj = Player.PawObjectCurrent
        Catch ex As Exception
            msHost.logError(ex, Me)
            Return False
        End Try
    End Function

    '(5:2003) set the variable %Variable to the number of the object at the bots feet.
    Function SetVariableToFloorObject(reader As TriggerReader) As Boolean
        Try
            Dim V = reader.ReadVariable(True)
            V.Value = Player.FloorObjectCurrent
        Catch ex As Exception
            msHost.logError(ex, Me)
            Return False
        End Try
        Return True
    End Function

    '(5:2002) set %Variable to the number of the object in the bots paws.
    Function SetVariableToPawObject(reader As TriggerReader) As Boolean
        Try
            Dim V = reader.ReadVariable(True)
            V.Value = Player.PawObjectCurrent
        Catch ex As Exception
            msHost.logError(ex, Me)
            Return False
        End Try
        Return True
    End Function

    Public Sub Start() Implements Interfaces.ImsPlugin.Start
        '(0:x) When the bot picks up or drops an object
        Add(Monkeyspeak.TriggerCategory.Cause, 2000,
            Function()
                Return Player.FloorObjectOld = Player.PawObjectCurrent And Player.PawObjectOld = Player.FloorObjectCurrent
            End Function) ' "(0:2000) When the bot picks up or drops an object")

        '(0:2001) When the bot picks up or drops the object #,
        Add(Monkeyspeak.TriggerCategory.Cause, 2001,
            AddressOf PickUpObjectNumber, "(0:2001) When the bot picks up or drops the object #,")

        '(1:2000) and the bot has object # in their paws,
        Add(Monkeyspeak.TriggerCategory.Condition, 2000,
            AddressOf ObjectInPaws, "(1:2000) and the bot has object # in their paws,")
        '(1:2001) and the bot doesn't have object # in their paws,
        Add(Monkeyspeak.TriggerCategory.Condition, 2001,
            AddressOf NotObjectInPaws, "(1:2001) and the bot doesn't have object # in their paws,")
        '(1:2002) and the bot is standing on object #,
        Add(Monkeyspeak.TriggerCategory.Condition, 2002,
             AddressOf ObjectAtFeet, "(1:2002) and the bot is standing on object #,")
        '(1:2003) and the bot is not standing on object #,
        Add(Monkeyspeak.TriggerCategory.Condition, 2003,
                AddressOf NotObjectAtFeet, "(1:2003) and the bot is not standing on object #,")

        '(5:2000) use the object in the bots paws.
        Add(Monkeyspeak.TriggerCategory.Effect, 2000,
                AddressOf UseObject, "(5:2000) use the object in the bots paws.")
        '(5:2001) pick up the object at the bots feet.
        Add(Monkeyspeak.TriggerCategory.Effect, 2001,
                AddressOf GetObject, "(5:2001) pick up the object at the bots feet.")
        '(5:2002) set %Variable to the number of the object in the bots paws.
        Add(Monkeyspeak.TriggerCategory.Effect, 2002,
                AddressOf SetVariableToPawObject, "(5:2002) set %Variable to the number of the object in the bots paws.")
        '(5:2003) set the variable %Variable to the number of the object at the bots feet.
        Add(Monkeyspeak.TriggerCategory.Effect, 2003,
                AddressOf SetVariableToFloorObject, "(5:2003) set the variable %Variable to the number of the object at the bots feet.")
    End Sub

    '(5:2000) use the object in the bots paws.
    Function UseObject(reader As TriggerReader) As Boolean
        Try
            msHost.sendServer("`use")
        Catch ex As Exception
            msHost.logError(ex, Me)
            Return False
        End Try
        Return True
    End Function

    Public Overrides Sub Unload(page As Page)

    End Sub

#Region "Helper Functions"

    Public Function IsBot(ByRef p As Furre) As Boolean
        Return p.ShortName = msHost.BotName.ToFurcShortName
    End Function

    ''' <summary>
    ''' </summary>
    ''' <param name="sname">
    ''' </param>
    ''' <param name="UbdateMSVariableName">
    ''' </param>
    ''' <returns>
    ''' </returns>
    Public Function NameToFurre(ByRef sname As String, ByRef UbdateMSVariableName As Boolean) As Furre
        Dim p As New Furre
        p.Name = sname
        For Each Character As Furre In Dream.FurreList
            If Character.ShortName = sname.ToFurcShortName Then
                p = Character
                Exit For
            End If
        Next
        If UbdateMSVariableName Then
            MsPage.RemoveVariable("NAME")
            MsPage.SetVariable("NAME", sname, True)
        End If
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

    Private Function fIDtoFurre(ByRef ID As Integer) As Furre

        For Each Character As Furre In Dream.FurreList
            If Character.FurreID = ID Then
                Return Character
            End If
        Next
#Disable Warning BC42105 ' Function 'fIDtoFurre' doesn't return a value on all code paths. A null reference exception could occur at run time when the result is used.
    End Function

#Enable Warning BC42105 ' Function 'fIDtoFurre' doesn't return a value on all code paths. A null reference exception could occur at run time when the result is used.

#End Region

End Class