Imports Furcadia.Net.Utils.ServerObjects
Imports Furcadia.Net.Utils.ServerParser
Imports Monkeyspeak
Imports SilverMonkeyEngine.Engine.Libraries.Dice

Namespace Engine.Libraries

    ''' <summary>
    ''' Cause: (0:130 - (0:136
    ''' <para>
    ''' Conditions: (1:130) - (1:131)
    ''' </para>
    ''' <para>
    ''' Effect: (5:130) - (5:139)
    ''' </para>
    ''' <para>
    ''' Furcadia Dice Role handler
    ''' </para>
    ''' </summary>
    ''' <remarks>
    ''' This Lib contains the following unnamed delegates
    ''' <para>
    ''' (0:136) When any one rolls anything,
    ''' </para>
    ''' </remarks>
    Public Class MsDice
        Inherits MonkeySpeakLibrary

        '

#Region "Private Fields"

        ''' <summary>
        ''' Last Dice roll object
        ''' </summary>
        Private Shared dice As DiceObject

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)
            dice = New DiceObject()
            '(0:130) When the bot rolls #d#,
            Add(New Trigger(TriggerCategory.Cause, 130), AddressOf RollNumber, "(0:130) When the bot rolls #d#,")
            '(0:131) When the bot rolls #d#+#,
            Add(New Trigger(TriggerCategory.Cause, 131), AddressOf RollNumberPlusModifyer, "(0:131) When the bot rolls #d#+#,")
            '(0:132) When the bot rolls #d#-#,
            Add(New Trigger(TriggerCategory.Cause, 132), AddressOf RollNumberMinusModifyer, "(0:132) When the bot rolls #d#-#,")

            '(0:133) When a furre rolls #d#,
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

            '(1:130) and the result is # or higher,
            Add(New Trigger(TriggerCategory.Condition, 130), AddressOf DiceResultNumberOrHigher, "(1:130) and the dice roll result is # or higher,")
            '(1:131) and the result is # or Lower,
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
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function DiceMinusNumber(reader As TriggerReader) As Boolean

            Dim dice As New DiceRollCollection

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
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function DicePlusNumber(reader As TriggerReader) As Boolean
            Dim Var As Variable
            Dim Number As Double
            Dim sides As Double = 0
            Dim NumberPlus As Double = 0
            Dim dice As New DiceRollCollection

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

        ''' <summary>
        ''' (1:130) and the dice roll result is # or higher,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function DiceResultNumberOrHigher(reader As TriggerReader) As Boolean
            Dim result As Double = ReadVariableOrNumber(reader)
            Return result <= dice.DiceResult
        End Function

        ''' <summary>
        ''' (1:131) and the dice roll result is # or lower,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function DiceResultNumberOrlower(reader As TriggerReader) As Boolean
            Dim result As Double = ReadVariableOrNumber(reader)
            Return result >= dice.DiceResult
        End Function

        ''' <summary>
        ''' (5:134) roll # furcadia dice with # sides.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function RollDice(reader As TriggerReader) As Boolean
            Dim count As Double = ReadVariableOrNumber(reader)
            Dim side As Double = ReadVariableOrNumber(reader)
            Dim Message As String = reader.ReadString

            Return sendServer("roll " + count.ToString + "d" + side.ToString + " " + Message)

        End Function

        ''' <summary>
        ''' (5:136) roll # furcadia dice with # sides minus #.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
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
        ''' <see cref="TriggerReader"/>
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
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Shared Function RollNumber(reader As TriggerReader) As Boolean

            If String.IsNullOrEmpty(dice.DiceCompnentMatch) Then Return False
            Dim DiceCount As Double = ReadVariableOrNumber(reader)
            Dim sides As Double = ReadVariableOrNumber(reader)
            If sides <> dice.DiceSides Then Return False
            If DiceCount <> DiceCount Then Return False

            Return True
        End Function

        ''' <summary>
        ''' (0:134) When the bot rolls #d# -# and gets # or highter,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Shared Function RollNumberMinusModifyer(reader As TriggerReader) As Boolean

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
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Shared Function RollNumberPlusModifyer(reader As TriggerReader) As Boolean

            If Not String.IsNullOrEmpty(dice.DiceCompnentMatch) Then Return False
            Dim DiceCount As Double = ReadVariableOrNumber(reader)
            Dim sides As Double = ReadVariableOrNumber(reader)
            Dim DiceModifyer As Double = ReadVariableOrNumber(reader)
            If dice.DiceModifyer <> DiceModifyer Then Return False
            If sides <> dice.DiceSides Then Return False
            If dice.DiceCount <> DiceCount Then Return False

            Return dice.DiceCompnentMatch = "+"
        End Function

        ''' <summary>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Shared Function TrigFurreRolledVariable(reader As TriggerReader) As Boolean
            Dim v As Variable = reader.ReadVariable(True)
            v.Value = dice.DiceResult
            Return True
        End Function

#End Region



#Region "Private Methods"

        ''' <summary>
        ''' Parse the dice rolls from the game server
        ''' </summary>
        ''' <param name="obj">
        ''' </param>
        ''' <param name="e">
        ''' </param>
        Private Sub OnServerChannel(obj As ChannelObject, e As EventArgs) Handles FurcadiaSession.ProcessServerChannelData
            Dim DiceObject As DiceRolls = Nothing
            If obj.GetType().Equals(GetType(DiceRolls)) Then
                DiceObject = CType(obj, DiceRolls)
            Else
                Exit Sub
            End If
            Select Case DiceObject.Channel
                Case "@roll"
                    dice = DiceObject.Dice
                    If FurcadiaSession.IsConnectedCharacter Then
                        '(0:130) When the bot rolls #d#,
                        '(0:132) When the bot rolls #d#+#,
                        '(0:134) When the bot rolls #d#-#,
                        '(0:136) When any one rolls anything,
                        MsPage.Execute(130, 131, 132, 136)
                    Else
                        '(0:136) When a furre rolls #d#,
                        '(0:138) When a fuure rolls #d#+#,
                        '(0:140) When a furre rolls #d#-#,
                        '(0:136) When any one rolls anything,
                        MsPage.Execute(133, 134, 135, 136)
                    End If
            End Select

        End Sub

#End Region

    End Class

End Namespace