Imports Furcadia.IO

Public Class LabelHotlinks
    Private HelpLink As IniFile
    Private Const HelpIniFile As String = "ControlHelpLinks.ini"

#Region "Constructors"

    Sub New()
        HelpLink = New IniFile()
        Try
            HelpLink.Load(Path.Combine(MonkeyCore2.IO.Paths.ApplicationPath, HelpIniFile))
            Dim HelpSection = HelpLink.GetSection("BotSettings")
            Dim s = HelpSection.GetKey("IniRetrieve").GetValue()
            If Not String.IsNullOrWhiteSpace(s) Then _iniRetrieval = s
        Catch
        End Try
    End Sub

    Sub Initialize()

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