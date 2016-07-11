Imports Monkeyspeak
Imports Furcadia.Net

Imports Furcadia.Base95

Public Class TheClaaaw
    Implements Interfaces.msPlugin

    Private msHost As Interfaces.msHost

    Public Sub Initialize(ByVal Host As Interfaces.msHost) Implements Interfaces.msPlugin.Initialize
        msHost = Host
    End Sub

    Public ReadOnly Property Name() As String Implements Interfaces.msPlugin.Name
        Get
            Return "The Claaaw"
        End Get
    End Property
    Public ReadOnly Property Description() As String Implements Interfaces.msPlugin.Description
        Get
            Return "Allows the bot to work with own paw and feet objects."
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
        Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Cause, 2000,
            Function()
                Return Player.FloorObjectOld = Player.PawObjectCurrent And Player.PawObjectOld = Player.FloorObjectCurrent
            End Function, "(0:2000) When the bot picks up or drops an object")

        '(0:2001) When the bot picks up or drops the object #,
        Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Cause, 2001,
            AddressOf PickUpObjectNumber, "(0:2001) When the bot picks up or drops the object #,")

        '(1:2000) and the bot has object # in their paws,
        Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Condition, 2000,
            AddressOf ObjectInPaws, "(1:2000) and the bot has object # in their paws,")
        '(1:2001) and the bot doesn't have object # in their paws,
        Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Condition, 2001,
            AddressOf NotObjectInPaws, "(1:2001) and the bot doesn't have object # in their paws,")
        '(1:2002) and the bot is standing on object #,
        Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Condition, 2002,
             AddressOf ObjectAtFeet, "(1:2002) and the bot is standing on object #,")
        '(1:2003) and the bot is not standing on object #,
        Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Condition, 2003,
                AddressOf NotObjectAtFeet, "(1:2003) and the bot is not standing on object #,")

        '(5:2000) use the object in the bots paws.
        Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Effect, 2000,
                AddressOf UseObject, "(5:2000) use the object in the bots paws.")
        '(5:2001) pick up the object at the bots feet.
        Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Effect, 2001,
                AddressOf GetObject, "(5:2001) pick up the object at the bots feet.")
        '(5:2002) set %Variable to the number of the object in the bots paws.
        Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Effect, 2002,
                AddressOf SetVariableToPawObject, "(5:2002) set %Variable to the number of the object in the bots paws.")
        '(5:2003) set the variable %Variable to the number of the object at the bots feet.
        Page.SetTriggerHandler(Monkeyspeak.TriggerCategory.Effect, 2003,
                AddressOf SetVariableToFloorObject, "(5:2003) set the variable %Variable to the number of the object at the bots feet.")
    End Sub

    Function PickUpObjectNumber(reader As TriggerReader) As Boolean
        Try
            Dim obj As Double = ReadVariableOrNumber(reader)
            Return Player.FloorObjectOld = Player.PawObjectCurrent And Player.PawObjectOld = Player.FloorObjectCurrent And obj = Player.PawObjectCurrent
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
    '(1:2003) and the bot is not standing on object #,
    Function NotObjectAtFeet(reader As TriggerReader) As Boolean
        Return Not ObjectAtFeet(reader)
    End Function


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
    '(5:2002) set %Variable to the number of the object in the bots paws.
    Function SetVariableToPawObject(reader As TriggerReader) As Boolean
        Try
            Dim V As Variable = reader.ReadVariable(True)
            V.Value = Player.PawObjectCurrent
        Catch ex As Exception
            msHost.logError(ex, Me)
            Return False
        End Try
        Return True
    End Function

    '(5:2003) set the variable %Variable to the number of the object at the bots feet.
    Function SetVariableToFloorObject(reader As TriggerReader) As Boolean
        Try
            Dim V As Variable = reader.ReadVariable(True)
            V.Value = Player.FloorObjectCurrent
        Catch ex As Exception
            msHost.logError(ex, Me)
            Return False
        End Try
        Return True
    End Function

    Function MessagePump(ByRef ServerInstruction As String) As Boolean Implements SilverMonkey.Interfaces.msPlugin.MessagePump
        'Set Object At Feet
        If ServerInstruction.StartsWith("%") Then
            Player = NametoFurre(msHost.BotName, True)
            Player.FloorObjectCurrent = ConvertFromBase95(ServerInstruction.Substring(1))
            Page.Execute(2000, 2001)
            msHost.Player = Player
            Furcadia.Net.DREAM.List(Player.ID) = Player
            ServerInstruction = ServerInstruction
            Return True
            'Set Object In Paws
        ElseIf ServerInstruction.StartsWith("^") Then
            Player = NametoFurre(msHost.BotName, True)
            Player.PawObjectCurrent = ConvertFromBase95(ServerInstruction.Substring(1))
            Page.Execute(2000, 2001)
            msHost.Player = Player
            Furcadia.Net.DREAM.List(Player.ID) = Player
            ServerInstruction = ServerInstruction
            Return True
        End If
        ServerInstruction = ServerInstruction
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

    Private Function fIDtoFurre(ByRef ID As String) As FURRE
        Dim Character As KeyValuePair(Of UInteger, FURRE)
        For Each Character In Dream.List
            If Character.Value.ID = CDbl(ID) Then
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
