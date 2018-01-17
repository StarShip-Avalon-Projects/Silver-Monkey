Imports Furcadia.Net.Utils.ServerObjects
Imports Libraries
Imports Monkeyspeak
Imports SilverMonkey.Engine.Libraries.Engine.Libraries.Dice
Imports SilverMonkeyEngine.Engine.Libraries.Dice
Imports SilverMonkeyEngine.Engine.Libraries.MsLibHelper

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
        Private dice As DiceObject

#Region "Public Constructors"

        Public Overrides ReadOnly Property BaseId As Integer
            Get
                Return 130
            End Get
        End Property

        Public Overrides Sub Initialize(ParamArray args() As Object)
            MyBase.Initialize(args)
            dice = New DiceObject()
            '(0:130) When the bot rolls #d#,
            Add(TriggerCategory.Cause,
                AddressOf RollNumber,
                "When the bot rolls #d#,")
            '(0:131) When the bot rolls #d#+#,
            Add(TriggerCategory.Cause,
                AddressOf RollNumberPlusModifyer,
                "When the bot rolls #d#+#,")
            '(0:132) When the bot rolls #d#-#,
            Add(TriggerCategory.Cause,
                AddressOf RollNumberMinusModifyer,
                "When the bot rolls #d#-#,")

            '(0:133) When a furre rolls #d#,
            Add(TriggerCategory.Cause,
                AddressOf RollNumber,
                "When a furre rolls #d#,")
            '(0:138) When a fuure rolls #d#+#,
            Add(TriggerCategory.Cause,
                AddressOf RollNumberPlusModifyer,
                "When a furre rolls #d#+#,")
            '(0:140) When a furre rolls #d#-#,
            Add(TriggerCategory.Cause,
                AddressOf RollNumberMinusModifyer,
                "When a furre rolls #d#-#,")

            '(0:136) When any one rolls anything,
            Add(TriggerCategory.Cause,
                 Function(reader)
                     Dim DiceParam = reader.GetParameter(Of DiceObject)
                     If DiceParam IsNot Nothing Then
                         dice = DiceParam
                     End If
                     Return True
                 End Function, "When any one rolls anything,")

            '(1:130) and the result is # or higher,
            Add(TriggerCategory.Condition,
                AddressOf DiceResultNumberOrHigher,
                "and the dice roll result is # or higher,")
            '(1:131) and the result is # or Lower,
            Add(TriggerCategory.Condition,
                AddressOf DiceResultNumberOrlower,
                "and the dice roll result is # or lower,")

            ' (5:130) set variable % to the total of rolling # dice with #
            ' sides plus #.
            Add(TriggerCategory.Effect,
                AddressOf DicePlusNumber,
                "set variable % to the total of rolling # dice with # sides plus #.")

            ' (5:131) set variable % to the total of rolling # dice with #
            ' sides minus #.
            Add(TriggerCategory.Effect,
                AddressOf DiceMinusNumber,
                "set variable % to the total of rolling # dice with # sides minus #.")

            ' (5:132) set variable %Variable to the number of the dice reult
            ' that the triggering furre rolled.
            Add(TriggerCategory.Effect,
                AddressOf TrigFurreRolledVariable,
                "set variable %Variable to the number of the dice result that the triggering furre rolled.")

            ' (5:133) roll # furcadia dice with # sides. (optional Message {...})
            Add(TriggerCategory.Effect,
                AddressOf RollDice,
                "roll # furcadia dice with # sides. (optional Message {...})")

            ' (5:134) roll # furcadia dice with # sides plus #. (optional
            ' Message {...})
            Add(TriggerCategory.Effect,
                AddressOf RollDicePlus,
                "roll # furcadia dice with # sides plus #. (optional Message {...})")

            ' (5:135) roll # furcadia dice with # sides minus #. (optional
            ' Message {...})
            Add(TriggerCategory.Effect,
                AddressOf RollDiceMinus,
                "roll # furcadia dice with # sides minus #. (optional Message {...})")

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

            Dim Var = reader.ReadVariable(True)
            Dim Number = reader.ReadNumber()
            Dim sides = reader.ReadNumber()
            Dim NumberPlus = reader.ReadNumber()

            Dim dice = New DiceRollCollection
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
            Dim Var = reader.ReadVariable(True)
            Dim Number = reader.ReadNumber()
            Dim sides = reader.ReadNumber()
            Dim NumberPlus = reader.ReadNumber()
            Dim dice = New DiceRollCollection
            For I = 0 To Number - 1
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
        Public Function DiceResultNumberOrHigher(reader As TriggerReader) As Boolean
            Dim result = reader.ReadNumber()
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
        Public Function DiceResultNumberOrlower(reader As TriggerReader) As Boolean
            Dim result = reader.ReadNumber()
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
        Public Function RollDice(reader As TriggerReader) As Boolean
            Dim count = reader.ReadNumber()
            Dim side = reader.ReadNumber()
            Dim Message = reader.ReadString

            Return SendServer("roll " + count.ToString + "d" + side.ToString + "" + Message)

        End Function

        ''' <summary>
        ''' (5:136) roll # furcadia dice with # sides minus #.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function RollDiceMinus(reader As TriggerReader) As Boolean
            Dim count = reader.ReadNumber()
            Dim side = reader.ReadNumber()
            Dim modifyer = reader.ReadNumber()

            Return SendServer("roll " + count.ToString + "d" + side.ToString + "-" + modifyer.ToString)

        End Function

        ''' <summary>
        ''' (5:135) roll # furcadia dice with # sides plus #.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function RollDicePlus(reader As TriggerReader) As Boolean
            Dim count = reader.ReadNumber()
            Dim side = reader.ReadNumber()
            Dim modifyer = reader.ReadNumber()

            Return SendServer("roll " + count.ToString + "d" + side.ToString + "+" + modifyer.ToString)

        End Function

        ''' <summary>
        ''' (0:130) When the bot rolls #d# and gets # or highter,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function RollNumber(reader As TriggerReader) As Boolean
            Dim DiceParam = reader.GetParameter(Of DiceObject)
            If DiceParam IsNot Nothing Then
                dice = DiceParam
            End If
            If String.IsNullOrEmpty(dice.DiceCompnentMatch) Then Return False

            Dim DiceCount = reader.ReadNumber()
            Dim sides = reader.ReadNumber()
            If sides <> dice.DiceSides Then Return False
            If DiceCount <> DiceCount Then Return False

            Return dice.DiceResult >= reader.ReadNumber()
        End Function

        ''' <summary>
        ''' (0:134) When the bot rolls #d# -# and gets # or highter,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function RollNumberMinusModifyer(reader As TriggerReader) As Boolean
            Dim DiceParam = reader.GetParameter(Of DiceObject)
            If DiceParam IsNot Nothing Then
                dice = DiceParam
            End If

            If Not String.IsNullOrEmpty(dice.DiceCompnentMatch) Then Return False
            Dim DiceCount = reader.ReadNumber()
            Dim sides = reader.ReadNumber()
            Dim DiceModifyer = reader.ReadNumber()
            If dice.DiceModifyer <> DiceModifyer Then Return False
            If sides <> dice.DiceSides Then Return False
            If dice.DiceCount <> DiceCount Then Return False

            Return dice.DiceCompnentMatch = "-" AndAlso dice.DiceResult >= reader.ReadNumber()
        End Function

        ''' <summary>
        ''' (0:132) When the bot rolls #d# +# and gets # or highter,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function RollNumberPlusModifyer(reader As TriggerReader) As Boolean
            Dim DiceParam = reader.GetParameter(Of DiceObject)
            If DiceParam IsNot Nothing Then
                dice = DiceParam
            End If

            If Not String.IsNullOrEmpty(dice.DiceCompnentMatch) Then Return False
            Dim DiceCount = reader.ReadNumber()
            Dim sides = reader.ReadNumber()
            Dim DiceModifyer = reader.ReadNumber()
            If dice.DiceModifyer <> DiceModifyer Then Return False
            If sides <> dice.DiceSides Then Return False
            If dice.DiceCount <> DiceCount Then Return False

            Return dice.DiceCompnentMatch = "+" And dice.DiceResult >= reader.ReadNumber()
        End Function

        ''' <summary>
        ''' (5:132) set variable %Variable to the number of the dice result that the triggering furre rolled.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function TrigFurreRolledVariable(reader As TriggerReader) As Boolean
            reader.ReadVariable(True).Value = dice.DiceResult
            Return True
        End Function

        Public Overrides Sub Unload(page As Page)

        End Sub

#End Region

    End Class

End Namespace