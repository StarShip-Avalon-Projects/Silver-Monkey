Imports System.Diagnostics
Imports MonkeyCore
Imports Monkeyspeak

Public Class MS_Dice
    Inherits MonkeySpeakLibrary
    Private writer As TextBoxWriter = Nothing
    '(5:130) - (5:139)

    Public Sub New()

        '(0:130) When the bot rolls #d#,
        Add(New Trigger(TriggerCategory.Cause, 130), AddressOf RollNumber, "(0:130) When the bot rolls #d#,")
        '(0:132) When the bot rolls #d#+#,
        Add(New Trigger(TriggerCategory.Cause, 131), AddressOf RollNumberPlusModifyer, "(0:131) When the bot rolls #d#+#,")
        '(0:134) When the bot rolls #d#-#,
        Add(New Trigger(TriggerCategory.Cause, 132), AddressOf RollNumberMinusModifyer, "(0:132) When the bot rolls #d#-#,")

        '(0:136) When a furre rolls #d#,
        Add(New Trigger(TriggerCategory.Cause, 133), AddressOf RollNumber, "(0:133) When a furre rolls #d#,")
        '(0:138) When a fuure rolls #d#+#,
        Add(New Trigger(TriggerCategory.Cause, 134), AddressOf RollNumberPlusModifyer, "(0:134) When a furre rolls #d#+#,")
        '(0:140) When a furre rolls #d#-#,
        Add(New Trigger(TriggerCategory.Cause, 135), AddressOf RollNumberMinusModifyer, "(0:135) When a furre rolls #d#-#,")

        '(0:136) When any one rolls anything,
        Add(New Trigger(TriggerCategory.Cause, 136),
            Function()
                Return True
            End Function, "(0:136) When any one rolls anything,")

        '(1:130) and the rresult is # or higher,
        Add(New Trigger(TriggerCategory.Condition, 130), AddressOf DiceResultNumberOrHigher, "(1:130) and the dice roll result is # or higher,")
        '(1:131) and the rresult is # or Lower,
        Add(New Trigger(TriggerCategory.Condition, 131), AddressOf DiceResultNumberOrlower, "(1:131) and the dice roll result is # or lower,")

        '  (5:130) set variable % to the total of rolling # dice with # sides plus #.
        Add(New Trigger(TriggerCategory.Effect, 130), AddressOf DicePlusNumber, "(5:130) set variable % to the total of rolling # dice with # sides plus #.")

        '  (5:131) set variable % to the total of rolling # dice with # sides minus #.
        Add(New Trigger(TriggerCategory.Effect, 131), AddressOf DiceMinusNumber, "(5:131) set variable % to the total of rolling # dice with # sides minus #.")

        ' (5:132) set variable %Variable to the number of the dice reult that the triggering furre rolled.
        Add(New Trigger(TriggerCategory.Effect, 132), AddressOf TrigFurreRolledVariable, "(5:132) set variable %Variable to the number of the dice result that the triggering furre rolled.")

        ' (5:133) roll # furcadia dice with # sides. (optional Message {...})
        Add(New Trigger(TriggerCategory.Effect, 133), AddressOf RollDice, "(5:133) roll # furcadia dice with # sides. (optional Message {...})")

        ' (5:134) roll # furcadia dice with # sides plus #. (optional Message {...})
        Add(New Trigger(TriggerCategory.Effect, 134), AddressOf RollDicePlus, "(5:134) roll # furcadia dice with # sides plus #. (optional Message {...})")

        ' (5:135) roll # furcadia dice with # sides minus #. (optional Message {...})
        Add(New Trigger(TriggerCategory.Effect, 135), AddressOf RollDiceMinus, "(5:135) roll # furcadia dice with # sides minus #. (optional Message {...})")

    End Sub

    '(0:130) When the bot rolls #d# and gets # or highter,
    Function RollNumber(reader As TriggerReader) As Boolean
        Try
            If String.IsNullOrEmpty(FurcSession.DiceCompnentMatch) Then Return False
            Dim DiceCount As Double = reader.ReadVariableOrNumber
            Dim sides As Double = reader.ReadVariableOrNumber
            Dim DiceModifyer As Double = reader.ReadVariableOrNumber
            If sides <> FurcSession.DiceSides Then Return False
            If FurcSession.DiceCount <> DiceCount Then Return False
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    '(0:132) When the bot rolls #d# +# and gets # or highter,
    Function RollNumberPlusModifyer(reader As TriggerReader) As Boolean
        Try
            If Not String.IsNullOrEmpty(FurcSession.DiceCompnentMatch) Then Return False
            Dim DiceCount As Double = reader.ReadVariableOrNumber
            Dim sides As Double = reader.ReadVariableOrNumber
            Dim DiceModifyer As Double = reader.ReadVariableOrNumber
            If FurcSession.DiceModifyer <> DiceModifyer Then Return False
            If sides <> FurcSession.DiceSides Then Return False
            If FurcSession.DiceCount <> DiceCount Then Return False

        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return FurcSession.DiceCompnentMatch = "+"
    End Function

    '(0:134) When the bot rolls #d# -# and gets # or highter,
    Function RollNumberMinusModifyer(reader As TriggerReader) As Boolean
        Try
            If Not String.IsNullOrEmpty(FurcSession.DiceCompnentMatch) Then Return False
            Dim DiceCount As Double = reader.ReadVariableOrNumber
            Dim sides As Double = reader.ReadVariableOrNumber
            Dim DiceModifyer As Double = reader.ReadVariableOrNumber
            If FurcSession.DiceModifyer <> DiceModifyer Then Return False
            If sides <> FurcSession.DiceSides Then Return False
            If FurcSession.DiceCount <> DiceCount Then Return False

        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return FurcSession.DiceCompnentMatch = "-"
    End Function

    Public Function DiceResultNumberOrHigher(reader As TriggerReader) As Boolean
        Dim result As Double = reader.ReadVariableOrNumber
        Return result <= FurcSession.DiceResult
    End Function
    Public Function DiceResultNumberOrlower(reader As TriggerReader) As Boolean
        Dim result As Double = reader.ReadVariableOrNumber
        Return result >= FurcSession.DiceResult
    End Function
    '  (5:312) set variable % to the total of rolling # dice with # sides plus #.
    Public Function DicePlusNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Var As Monkeyspeak.Variable
        Dim Number As Double
        Dim sides As Double = 0
        Dim NumberPlus As Double = 0
        Dim dice As New DieCollection

        Try
            Var = reader.ReadVariable(True)
            Number = ReadVariableOrNumber(reader)
            sides = ReadVariableOrNumber(reader)
            NumberPlus = ReadVariableOrNumber(reader)
            ' dice.Clear()
            For I As Double = 0 To Number - 1
                dice.Add(New Die(CInt(sides)))
            Next
            Var.Value = dice.RollAll() + NumberPlus
            Return True
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
    End Function

    '  (5:313) set variable % to the total of rolling # dice with # sides minus #.
    Public Function DiceMinusNumber(reader As Monkeyspeak.TriggerReader) As Boolean

        Dim dice As New DieCollection

        Try
            Dim Var As Variable = reader.ReadVariable(True)
            Dim Number As Double = reader.ReadVariableOrNumber
            Dim sides As Double = reader.ReadVariableOrNumber
            Dim NumberPlus As Double = reader.ReadVariableOrNumber

            dice.Clear()
            For I As Double = 0 To Number - 1
                dice.Add(New Die(sides))
            Next
            Var.Value = dice.RollAll() - NumberPlus
            Return True
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
    End Function

    Function TrigFurreRolledVariable(reader As TriggerReader) As Boolean
        Dim v As Variable = reader.ReadVariable(True)
        v.Value = FurcSession.DiceResult
        Return True
    End Function
    ' (5:134) roll # furcadia dice with # sides.
    Function RollDice(reader As TriggerReader) As Boolean
        Dim count As Double = reader.ReadVariableOrNumber
        Dim side As Double = reader.ReadVariableOrNumber
        Dim Message As String = ""
        If (reader.TryReadString(Message)) Then
            sendServer("roll " + count.ToString + "d" + side.ToString + " " + Message)
        Else
            sendServer("roll " + count.ToString + "d" + side.ToString)
        End If
        Return True
    End Function

    ' (5:135) roll # furcadia dice with # sides plus #.
    Function RollDicePlus(reader As TriggerReader) As Boolean
        Dim count As Double = reader.ReadVariableOrNumber
        Dim side As Double = reader.ReadVariableOrNumber
        Dim modifyer As Double = reader.ReadVariableOrNumber
        Dim Message As String = ""
        If (reader.TryReadString(Message)) Then
            sendServer("roll " + count.ToString + "d" + side.ToString + "+" + modifyer.ToString + " " + Message)
        Else
            sendServer("roll " + count.ToString + "d" + side.ToString + "+" + modifyer.ToString)
        End If
        Return True
    End Function
    ' (5:136) roll # furcadia dice with # sides minus #.
    Function RollDiceMinus(reader As TriggerReader) As Boolean
        Dim count As Double = reader.ReadVariableOrNumber
        Dim side As Double = reader.ReadVariableOrNumber
        Dim modifyer As Double = reader.ReadVariableOrNumber
        Dim Message As String = ""
        If (reader.TryReadString(Message)) Then
            sendServer("roll " + count.ToString + "d" + side.ToString + "-" + modifyer.ToString + " " + Message)

        Else
            sendServer("roll " + count.ToString + "d" + side.ToString + "-" + modifyer.ToString)
        End If
        Return True
    End Function
#Region "Helper functions"

#End Region

    Public Class Die

        Private Shared faceSelector As New Random

        Private _faceCount As Double
        Private _value As Double

        Public Property FaceCount() As Double
            Get
                Return Me._faceCount
            End Get
            Set(ByVal value As Double)
                Me._faceCount = value
            End Set
        End Property

        Public ReadOnly Property Value() As Double
            Get
                Return Me._value
            End Get
        End Property

        Public Sub New(ByVal faceCount As Double)
            If faceCount <= 0 Then
                Throw New ArgumentOutOfRangeException("faceCount", "Dice must have one or more faces.")
            End If

            Me._faceCount = faceCount
        End Sub

        Public Function Roll() As Double
            Me._value = CDbl(faceSelector.Next(1, CInt(Me.FaceCount)))
            Return Me.Value
        End Function

    End Class

    Public Class DieCollection
        Inherits System.Collections.ObjectModel.Collection(Of Die)

        Public Function RollAll() As Double
            Dim total As Double = 0

            For Each die As Die In Me.Items
                total += die.Roll()
            Next

            Return total
        End Function

    End Class

End Class