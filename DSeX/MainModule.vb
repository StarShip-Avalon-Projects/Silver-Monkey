Imports System.Runtime.InteropServices
Imports FastColoredTextBoxNS
Imports MonkeyCore

Public Module MainModule



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



End Module