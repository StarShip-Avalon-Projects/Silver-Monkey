Imports Furcadia.Net.Dream
Imports Monkeyspeak

Public NotInheritable Class MsLibHelper

    ''' <summary>
    ''' %MESSAGE
    ''' </summary>
    Public Const MessageVariable As String = "%MESSAGE"

    ''' <summary>
    ''' %SHORTNAME
    ''' </summary>
    Public Const ShortNameVariable = "%SHORTNAME"

    Public Const TriggeringFurreNameVariable = "%NAME"

    Public Const DreamOwnerVariable = "%DREAMOWNER"

    Public Const DreamNameVariable = "%DREAMNAME"

    ''' <summary>
    ''' updates Bot Constant Variables for the Current Triggering  Furre
    ''' </summary>
    ''' <param name="ActivePlayer"></param>
    ''' <param name="MonkeySpeakPage"></param>
    Public Shared Sub UpdateTriggerigFurreVariables(ActivePlayer As Furre, MonkeySpeakPage As Page)
        DirectCast(MonkeySpeakPage.GetVariable(MessageVariable), ConstantVariable).SetValue(ActivePlayer.Message)
        DirectCast(MonkeySpeakPage.GetVariable(ShortNameVariable), ConstantVariable).SetValue(ActivePlayer.ShortName)
        DirectCast(MonkeySpeakPage.GetVariable(TriggeringFurreNameVariable), ConstantVariable).SetValue(ActivePlayer.Name)
    End Sub

    ''' <summary>
    ''' updates Bot Constant Variables for the Current Triggering  Furre
    ''' </summary>
    ''' <param name="ActiveDream"></param>
    ''' <param name="MonkeySpeakPage"></param>
    Public Shared Sub UpdateCurrentDreamVariables(ActiveDream As DREAM, MonkeySpeakPage As Page)
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
        Dim result As Double = 0
        If reader.PeekVariable Then
            Dim value As String = reader.ReadVariable(addIfNotExist).Value.ToString
            Double.TryParse(value, result)
        ElseIf reader.PeekNumber Then
            result = reader.ReadNumber
        End If
        Return result
    End Function

End Class