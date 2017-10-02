Imports System.Text.RegularExpressions

Module Modunle1
    Public Const VarPrefix = "%"
    Private Const REGEX_NameFilter As String = "[^a-zA-Z0-9\0x0020_.]+"

    <System.Runtime.CompilerServices.Extension()>
    Public Function ToFurcShortName(ByVal value As String) As String
        Return Regex.Replace(value.ToLower, REGEX_NameFilter, "")
    End Function

End Module