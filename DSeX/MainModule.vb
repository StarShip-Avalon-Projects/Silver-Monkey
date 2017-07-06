Imports System.Runtime.InteropServices
Imports FastColoredTextBoxNS
Imports MonkeyCore

Public Module MainModule

    Public Const WM_COPYDATA As Integer = &H4A
    Public Const WM_USER As Integer = &H400

    Public DS_Comment_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Regular)
    Public DS_Default_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Regular)
    Public DS_Footer_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Bold)
    Public DS_Header_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Bold)
    Public DS_Line_ID_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Regular)
    Public DS_Num_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Regular)
    Public DS_Num_Var_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Regular)
    Public DS_Str_Var_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Regular)
    Public DS_String_Style As TextStyle = New TextStyle(Brushes.Brown, Nothing, FontStyle.Italic)
    Public KeysHelpIni As IniFile = New IniFile
    Public KeysHelpMSIni As IniFile = New IniFile
    Public MS_Comment_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Regular)
    Public MS_Default_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Regular)
    Public MS_Footer_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Bold)
    Public MS_Header_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Bold)
    Public MS_Line_ID_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Regular)
    Public MS_Num_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Regular)
    Public MS_Num_Var_Style As TextStyle = New TextStyle(New SolidBrush(Color.Green), Nothing, FontStyle.Regular)
    Public MS_String_Style As TextStyle = New TextStyle(Brushes.Brown, Nothing, FontStyle.Italic)

    Public Enum EditStyles
        none = 0
        ini
        ds
        ms
    End Enum

    'Used for WM_COPYDATA for string messages
    <StructLayout(LayoutKind.Sequential)>
    Public Structure COPYDATASTRUCT
        Public dwData As IntPtr
        Public cdData As Integer
        Public lpData As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure MyData
        Public fID As UInteger

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=78)>
        Public lpName As String

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=78)>
        Public lpTag As String

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=2048)>
        Public lpMsg As String

    End Structure

End Module