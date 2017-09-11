Imports System.Runtime.InteropServices
Imports MonkeyCore

Module MainModule
    'Public EditIni As New IniFile
    'Public MS_KeysIni As IniFile = New IniFile
    Public BotIni As New IniFile


    Public Const MS_Name As String = "NAME"

    Public Const MS_ErrWarning As String = "Error, See Debug Window"

    Public Const WM_USER As Integer = &H400
    Public Const WM_COPYDATA As Integer = &H4A

    'Used for WM_COPYDATA for string messages
    <StructLayout(LayoutKind.Sequential)>
    Public Structure COPYDATASTRUCT
        Public dwData As IntPtr
        Public cdData As Integer
        Public lpData As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure MyData
        Public fID As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=78)>
        Public lpName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=78)>
        Public lpTag As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=2048)>
        Public lpMsg As String
    End Structure

End Module
Public Module MyExtensions
    <Runtime.CompilerServices.Extension()>
    Public Function IsInteger(ByVal value As Type) As Boolean
        If String.IsNullOrEmpty(value.ToString) Then
            Return False
        Else
            Return Integer.TryParse(value.ToString, Nothing)
        End If
    End Function

    <Runtime.CompilerServices.Extension()>
    Public Function ToInteger(ByVal value As Type) As Integer
        If value.IsInteger() Then
            Return Integer.Parse(value.ToString)
        Else
            Return 0
        End If
    End Function

End Module