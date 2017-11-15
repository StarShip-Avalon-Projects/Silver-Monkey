﻿Imports Furcadia.Net.Dream
Imports Monkeyspeak

Public NotInheritable Class MsLibHelper
    Public Const MessageVariable As String = "%MESSAGE"
    Public Const ShortNameVariable = "%SHORTNAME"

    Public Shared Sub UpdateTriggerigFurreVariables(ActivePlayer As Furre, MonkeySpeakPage As Page)
        DirectCast(MonkeySpeakPage.GetVariable(MessageVariable), ConstantVariable).SetValue(ActivePlayer.Message)
        DirectCast(MonkeySpeakPage.GetVariable(ShortNameVariable), ConstantVariable).SetValue(ActivePlayer.ShortName)
        DirectCast(MonkeySpeakPage.GetVariable("%NAME"), ConstantVariable).SetValue(ActivePlayer.Name)
    End Sub

End Class