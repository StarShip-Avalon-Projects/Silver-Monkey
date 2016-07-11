Imports System.IO

Public Module clsMain
    Public MainMSEngine As MainEngine
    Public callbk As Program
    Public Plugins() As PluginServices.AvailablePlugin


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