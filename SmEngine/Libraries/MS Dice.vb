Imports Furcadia.Net.Utils.ServerObjects
Imports Furcadia.Net.Utils.ServerParser
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Furcadia Drice Role handler
    ''' </summary>
    Public Class MS_Dice
        Inherits MonkeySpeakLibrary

        '(5:130) - (5:139)

#Region "Private Fields"

        Private Shared dice As DiceObject

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)
            dice = New DiceObject()
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

            ' (5:130) set variable % to the total of rolling # dice with #
            ' sides plus #.
            Add(New Trigger(TriggerCategory.Effect, 130), AddressOf DicePlusNumber, "(5:130) set variable % to the total of rolling # dice with # sides plus #.")

            ' (5:131) set variable % to the total of rolling # dice with #
            ' sides minus #.
            Add(New Trigger(TriggerCategory.Effect, 131), AddressOf DiceMinusNumber, "(5:131) set variable % to the total of rolling # dice with # sides minus #.")

            ' (5:132) set variable %Variable to the number of the dice reult
            ' that the triggering furre rolled.
            Add(New Trigger(TriggerCategory.Effect, 132), AddressOf TrigFurreRolledVariable, "(5:132) set variable %Variable to the number of the dice result that the triggering furre rolled.")

            ' (5:133) roll # furcadia dice with # sides. (optional Message {...})
            Add(New Trigger(TriggerCategory.Effect, 133), AddressOf RollDice, "(5:133) roll # furcadia dice with # sides. (optional Message {...})")

            ' (5:134) roll # furcadia dice with # sides plus #. (optional
            ' Message {...})
            Add(New Trigger(TriggerCategory.Effect, 134), AddressOf RollDicePlus, "(5:134) roll # furcadia dice with # sides plus #. (optional Message {...})")

            ' (5:135) roll # furcadia dice with # sides minus #. (optional
            ' Message {...})
            Add(New Trigger(TriggerCategory.Effect, 135), AddressOf RollDiceMinus, "(5:135) roll # furcadia dice with # sides minus #. (optional Message {...})")

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (5:313) set variable % to the total of rolling # dice with #
        ''' sides minus #.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function DiceMinusNumber(reader As TriggerReader) As Boolean

            Dim dice As New DieCollection

            Dim Var As Variable = reader.ReadVariable(True)
            Dim Number As Double = ReadVariableOrNumber(reader)
            Dim sides As Double = ReadVariableOrNumber(reader)
            Dim NumberPlus As Double = ReadVariableOrNumber(reader)

            dice.Clear()
            For I As Double = 0 To Number - 1
                dice.Add(New Die(sides))
            Next
            Var.Value = dice.RollAll() - NumberPlus
            Return True

        End Function

        ''' <summary>
        ''' (5:312) set variable % to the total of rolling # dice with #
        ''' sides plus #.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function DicePlusNumber(reader As TriggerReader) As Boolean
            Dim Var As Variable
            Dim Number As Double
            Dim sides As Double = 0
            Dim NumberPlus As Double = 0
            Dim dice As New DieCollection

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

        End Function

        Public Function DiceResultNumberOrHigher(reader As TriggerReader) As Boolean
            Dim result As Double = ReadVariableOrNumber(reader)
            Return result <= dice.DiceResult
        End Function

        Public Function DiceResultNumberOrlower(reader As TriggerReader) As Boolean
            Dim result As Double = ReadVariableOrNumber(reader)
            Return result >= dice.DiceResult
        End Function

        ''' <summary>
        ''' (5:134) roll # furcadia dice with # sides.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function RollDice(reader As TriggerReader) As Boolean
            Dim count As Double = ReadVariableOrNumber(reader)
            Dim side As Double = ReadVariableOrNumber(reader)
            Dim Message As String = ""

            Return sendServer("roll " + count.ToString + "d" + side.ToString + " " + Message)

        End Function

        ''' <summary>
        ''' (5:136) roll # furcadia dice with # sides minus #.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function RollDiceMinus(reader As TriggerReader) As Boolean
            Dim count As Double = ReadVariableOrNumber(reader)
            Dim side As Double = ReadVariableOrNumber(reader)
            Dim modifyer As Double = ReadVariableOrNumber(reader)
            Dim Message As String = ""

            Return sendServer("roll " + count.ToString + "d" + side.ToString + "-" + modifyer.ToString + " " + Message)

        End Function

        ''' <summary>
        ''' (5:135) roll # furcadia dice with # sides plus #.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function RollDicePlus(reader As TriggerReader) As Boolean
            Dim count As Double = ReadVariableOrNumber(reader)
            Dim side As Double = ReadVariableOrNumber(reader)
            Dim modifyer As Double = ReadVariableOrNumber(reader)
            Dim Message As String = ""

            Return sendServer("roll " + count.ToString + "d" + side.ToString + "+" + modifyer.ToString + " " + Message)

        End Function

        ''' <summary>
        ''' (0:130) When the bot rolls #d# and gets # or highter,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function RollNumber(reader As TriggerReader) As Boolean

            If String.IsNullOrEmpty(dice.DiceCompnentMatch) Then Return False
            Dim DiceCount As Double = ReadVariableOrNumber(reader)
            Dim sides As Double = ReadVariableOrNumber(reader)
            Dim DiceModifyer As Double = ReadVariableOrNumber(reader)
            If sides <> dice.DiceSides Then Return False
            If DiceCount <> DiceCount Then Return False

            Return True
        End Function

        ''' <summary>
        ''' (0:134) When the bot rolls #d# -# and gets # or highter,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function RollNumberMinusModifyer(reader As TriggerReader) As Boolean

            If Not String.IsNullOrEmpty(dice.DiceCompnentMatch) Then Return False
            Dim DiceCount As Double = ReadVariableOrNumber(reader)
            Dim sides As Double = ReadVariableOrNumber(reader)
            Dim DiceModifyer As Double = ReadVariableOrNumber(reader)
            If dice.DiceModifyer <> DiceModifyer Then Return False
            If sides <> dice.DiceSides Then Return False
            If dice.DiceCount <> DiceCount Then Return False

            Return dice.DiceCompnentMatch = "-"
        End Function

        ''' <summary>
        ''' (0:132) When the bot rolls #d# +# and gets # or highter,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function RollNumberPlusModifyer(reader As TriggerReader) As Boolean

            If Not String.IsNullOrEmpty(dice.DiceCompnentMatch) Then Return False
            Dim DiceCount As Double = ReadVariableOrNumber(reader)
            Dim sides As Double = ReadVariableOrNumber(reader)
            Dim DiceModifyer As Double = ReadVariableOrNumber(reader)
            If dice.DiceModifyer <> DiceModifyer Then Return False
            If sides <> dice.DiceSides Then Return False
            If dice.DiceCount <> DiceCount Then Return False

            Return dice.DiceCompnentMatch = "+"
        End Function

        Function TrigFurreRolledVariable(reader As TriggerReader) As Boolean
            Dim v As Variable = reader.ReadVariable(True)
            v.Value = dice.DiceResult
            Return True
        End Function

#End Region

#Region "Public Classes"

        Public Class Die

#Region "Private Fields"

            Private Shared faceSelector As New Random

            Private _faceCount As Double
            Private _value As Double

#End Region

#Region "Public Constructors"

            Public Sub New(ByVal faceCount As Double)
                If faceCount <= 0 Then
                    Throw New ArgumentOutOfRangeException("faceCount", "Dice must have one or more faces.")
                End If

                Me._faceCount = faceCount
            End Sub

#End Region

#Region "Public Properties"

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

#End Region

#Region "Public Methods"

            Public Function Roll() As Double
                Me._value = CDbl(faceSelector.Next(1, CInt(Me.FaceCount)))
                Return Me.Value
            End Function

#End Region

        End Class

        Public Class DieCollection
            Inherits System.Collections.ObjectModel.Collection(Of Die)

#Region "Public Methods"

            Public Function RollAll() As Double
                Dim total As Double = 0

                For Each die As Die In Me.Items
                    total += die.Roll()
                Next

                Return total
            End Function

#End Region

        End Class

#End Region

#Region "Private Methods"

        Private Shared Sub ParseData(obj As ChannelObject, e As EventArgs) 'Handles FurcadiaSession.ProcessServerChannelData
            Dim DiceObject As DiceRolls = Nothing
            If obj.GetType().Equals(GetType(DiceRolls)) Then
                DiceObject = CType(obj, DiceRolls)
            Else
                Exit Sub
            End If
            dice = DiceObject.Dice

        End Sub

#End Region

    End Class

End Namespace