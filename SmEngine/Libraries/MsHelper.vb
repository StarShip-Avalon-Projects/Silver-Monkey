Imports Furcadia.Net.Dream
Imports Monkeyspeak

Public NotInheritable Class MsLibHelper
    Private Const MessageVariable As String = "%MESSAGE"

    Public Shared Sub UpdateTriggerigFurreFariabled(ActivePlayer As Furre, MonkeySpeakPage As Page)
        DirectCast(MonkeySpeakPage.GetVariable(MessageVariable), ConstantVariable).SetValue(ActivePlayer.Message)
        DirectCast(MonkeySpeakPage.GetVariable("%SHORTNAME"), ConstantVariable).SetValue(ActivePlayer.ShortName)
        DirectCast(MonkeySpeakPage.GetVariable("%NAME"), ConstantVariable).SetValue(ActivePlayer.Name)
    End Sub

End Class