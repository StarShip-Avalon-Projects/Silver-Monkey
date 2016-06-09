Imports SilverMonkey.IniFile
Imports SilverMonkey.ConfigStructs
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Text.RegularExpressions

Module MainModule
    'Public EditIni As New IniFile
    Public MS_KeysIni As IniFile = New IniFile
    Public SettingsIni As New IniFile
    Public BotIni As New IniFile
    Public cMain As cMain = New cMain
    Public cBot As cBot
    Public FurcPath As Furcadia.IO.Paths = New Furcadia.IO.Paths()
    Public Plugins() As PluginServices.AvailablePlugin
    Public Const My_Docs As String = "\Silver Monkey"
    Public Const REGEX_NameFilter As String = "[^a-z0-9\0x0020_;&]+"
    Public Const MS_Name As String = "NAME"

    Public Const MS_ErrWarning As String = "Error, See Debug Window"

    <System.Runtime.CompilerServices.Extension()> _
    Public Function ToFurcShortName(ByVal value As String) As String
        If String.IsNullOrEmpty(value) Then Return Nothing
        Return Regex.Replace(value.ToLower, REGEX_NameFilter, "", RegexOptions.CultureInvariant)
    End Function

    Public Function CheckMyDocFile(ByVal Filename As String) As String

        Dim f As String = Path.GetDirectoryName(Filename)
        If String.IsNullOrEmpty(f) Then
            Return mPath() + Path.DirectorySeparatorChar + Filename
        End If
        If String.IsNullOrEmpty(Path.GetPathRoot(f)) Then
            Return mPath() + Path.DirectorySeparatorChar + Filename
        End If
        Return Filename
    End Function

    Public Function IsBotControler(ByRef Name As String) As Boolean
        If String.IsNullOrEmpty(cBot.BotController) Then Return False
        Return cBot.BotController.ToFurcShortName = Name.ToFurcShortName
    End Function

    Public Const WM_USER As Integer = &H400
    Public Const WM_COPYDATA As Integer = &H4A

    'Used for WM_COPYDATA for string messages
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure COPYDATASTRUCT
        Public dwData As IntPtr
        Public cdData As Integer
        Public lpData As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
    Public Structure MyData
        Public fID As UInteger

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=78)> _
        Public lpName As String


        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=78)> _
        Public lpTag As String


        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=2048)> _
        Public lpMsg As String
    End Structure

    Public Function UnixTimeStampToDateTime(ByRef unixTimeStamp As Double) As DateTime
        ' Unix timestamp is seconds past epoch
        Dim dtDateTime As System.DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime
        Return dtDateTime
    End Function
    Public Function DateTimeToUnixTimestamp(dTime As DateTime) As Double
        Return (dTime - New DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds
    End Function
End Module
Public Module MyExtensions
    <System.Runtime.CompilerServices.Extension()> _
    Public Function IsInteger(ByVal value As String) As Boolean
        If String.IsNullOrEmpty(value) Then
            Return False
        Else
            Return Integer.TryParse(value, Nothing)
        End If
    End Function

    <System.Runtime.CompilerServices.Extension()> _
    Public Function ToInteger(ByVal value As String) As Integer
        If value.IsInteger() Then
            Return Integer.Parse(value)
        Else
            Return 0
        End If
    End Function


End Module