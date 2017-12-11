Imports System.Text.RegularExpressions
Imports Furcadia.Net.DreamInfo
Imports Monkeyspeak

Namespace Engine.Libraries

    Public NotInheritable Class MsLibHelper

        ''' <summary>
        ''' the last text the bot has seen, Usually the Triggering furre's message
        ''' </summary>
        Public Const MessageVariable As String = "%MESSAGE"

        ''' <summary>
        ''' the triggering furres shortname
        ''' </summary>
        Public Const ShortNameVariable = "%SHORTNAME"

        ''' <summary>
        ''' the triggering furres full name
        ''' </summary>
        Public Const TriggeringFurreNameVariable = "%NAME"

        ''' <summary>
        ''' the Uploaders name
        ''' </summary>
        Public Const DreamOwnerVariable = "%DREAMOWNER"

        ''' <summary>
        ''' the name of the dream we're in
        ''' </summary>
        Public Const DreamNameVariable = "%DREAMNAME"

        Public Const SmRegExOptions As RegexOptions = RegexOptions.Compiled

        ''' <summary>
        ''' Name of the connected furre, IE: the Bots name
        ''' </summary>
        Public Const BotNameVariable As String = "%BOTNAME"

        ''' <summary>
        ''' designated furre the bot recognizes as its controller
        ''' </summary>
        Public Const BotControllerVariable As String = "%BOTCONTROLLER"

        ''' <summary>
        ''' name of the furre getting banished ot unbanished
        ''' </summary>
        Public Const BanishNameVariable As String = "%BANISHNAME"

        ''' <summary>
        ''' comma seperated list of the furres on the banisl list
        ''' </summary>
        Public Const BanishListVariable As String = "%BANISHLIST"

        ''' <summary>
        ''' update Bot Constant Variables for the Current Triggering  Furre
        ''' <para/>
        ''' <see cref="TriggeringFurreNameVariable"/>
        ''' <para/>
        ''' <see cref="ShortNameVariable"/>
        ''' <para/>
        ''' <see cref="MessageVariable"/>
        ''' </summary>
        ''' <param name="ActivePlayer"></param>
        ''' <param name="MonkeySpeakPage"></param>
        Public Shared Sub UpdateTriggerigFurreVariables(ByRef ActivePlayer As IFurre, ByRef MonkeySpeakPage As Page)
            DirectCast(MonkeySpeakPage.GetVariable(MessageVariable), ConstantVariable).SetValue(ActivePlayer.Message)
            DirectCast(MonkeySpeakPage.GetVariable(ShortNameVariable), ConstantVariable).SetValue(ActivePlayer.ShortName)
            DirectCast(MonkeySpeakPage.GetVariable(TriggeringFurreNameVariable), ConstantVariable).SetValue(ActivePlayer.Name)
        End Sub

        ''' <summary>
        ''' update Bot Constant Variables for the Current Dream
        ''' <para/>
        ''' <see cref="DreamOwnerVariable"/>
        ''' <para/>
        ''' <see cref="DreamNameVariable"/>
        ''' </summary>
        ''' <param name="ActiveDream"></param>
        ''' <param name="MonkeySpeakPage"></param>
        Public Shared Sub UpdateCurrentDreamVariables(ByRef ActiveDream As Dream, ByRef MonkeySpeakPage As Page)
            DirectCast(MonkeySpeakPage.GetVariable(DreamOwnerVariable), ConstantVariable).SetValue(ActiveDream.Owner)
            DirectCast(MonkeySpeakPage.GetVariable(DreamNameVariable), ConstantVariable).SetValue(ActiveDream.Name)
        End Sub

        ''' <summary>
        ''' Reads a Double or a MonkeySpeak Variable
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <param name="addIfNotExist">
        ''' Add Variable to Variable Scope is it Does not exist,
        ''' <para>
        ''' Default Value is False
        ''' </para>
        ''' </param>
        ''' <returns>
        ''' <see cref="Double"/>
        ''' </returns>
        Public Shared Function ReadVariableOrNumber(ByVal reader As TriggerReader,
                                         Optional addIfNotExist As Boolean = False) As Double
            Dim result = 0
            If reader.PeekVariable Then
                Dim value = reader.ReadVariable(addIfNotExist).Value.ToString
                Double.TryParse(value, result)
            ElseIf reader.PeekNumber Then
                result = reader.ReadNumber
            End If
            Return result
        End Function

    End Class

End Namespace