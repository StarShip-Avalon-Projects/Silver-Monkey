Imports System.Runtime.InteropServices

Public Class MyData
    Public Const WM_COPYDATA As Integer = &H4A
    Public Const WM_USER As Integer = &H400

    'Used for WM_COPYDATA for string messages
    <StructLayout(LayoutKind.Sequential)>
    Public Structure COPYDATASTRUCT
        Public dwData As IntPtr
        Public cdData As Integer
        Public lpData As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure MyDataStructure
        Public fID As UInteger

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=78)>
        Public lpName As String

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=78)>
        Public lpTag As String

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=2048)>
        Public lpMsg As String

    End Structure

End Class