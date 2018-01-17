Imports System.Text
Imports Furcadia.IO

''' <summary>
''' Generic scripts for preloading into Monkey Speak Editor and Silver Monkey
''' </summary>
Public Class NewEditorScripts

    Sub New()
        _MS_KeysIni = New IniFile()
    End Sub

#Region "Private Fields"

    Private _MS_KeysIni As IniFile

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Dragon Speak File containing MonkeySpeak Editors lines and settings
    ''' </summary>
    ''' <returns></returns>
    Public Property MS_KeysIni As IniFile
        Get
            Return _MS_KeysIni
        End Get
        Set(value As IniFile)
            _MS_KeysIni = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' New Monkey Speak fileDefaults
    ''' <para>Actual format is set in a configuration *.ini file</para>
    ''' </summary>
    ''' <returns>Generic preformated MonkeySpeak file</returns>
    Public Shared Function NewMSFile(ver As String) As String
        Dim str As New StringBuilder
        str.AppendLine(Settings.MS_KeysIni.GetKeyValue("MS-General", "Header").Replace("[VERSION]", ver).TrimEnd(""""c))
        Dim t As String = " "
        Dim n As Integer = 0
        While t <> ""
            t = Settings.MS_KeysIni.GetKeyValue("MS-General", "H" + n.ToString()).TrimEnd(""""c)
            If t <> "" Then str.AppendLine(t)
            n += 1
        End While
        Dim iMax = 0
        Integer.TryParse(Settings.MS_KeysIni.GetKeyValue("MS-General", "InitLineSpaces").TrimEnd(""""c), iMax)
        For i = 0 To iMax
            str.AppendLine("")
        Next
        str.Append(Settings.MS_KeysIni.GetKeyValue("MS-General", "Footer").TrimEnd(""""c))
        Return str.ToString
    End Function

#End Region

End Class