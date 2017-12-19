Imports Furcadia.IO

Public Class LabelHotlinks
    Private HelpLink As IniFile
    Private Const HelpIniFile As String = "ControlHelpLinks.ini"

#Region "Constructors"

    Sub New()
        HelpLink = New IniFile()
        HelpLink.Load(HelpIniFile)
        Dim HelpSection = HelpLink.GetSection("BotSettings")
        _iniRetrieval = HelpSection.GetKey("IniRetrieve").Value.Trim(""""c, " "c)
    End Sub

#End Region

#Region "BotSettings"

    Private _iniRetrieval As String

    Public ReadOnly Property IniRetrieval() As String
        Get
            Return _iniRetrieval
        End Get
    End Property

#End Region

End Class