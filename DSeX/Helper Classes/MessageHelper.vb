Imports System.Runtime.InteropServices

Public Class MessageHelper

#Region "Public Methods"

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="lpClassName"></param>
    ''' <param name="lpWindowName"></param>
    ''' <returns></returns>
    <DllImport("User32.dll", EntryPoint:="FindWindow")>
    Public Shared Function FindWindow(lpClassName As [String], lpWindowName As [String]) As Int32
    End Function

    'For use with WM_COPYDATA and COPYDATASTRUCT

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="hWnd"></param>
    ''' <param name="Msg"></param>
    ''' <param name="wParam"></param>
    ''' <param name="lParam"></param>
    ''' <returns></returns>
    <DllImport("User32.dll", EntryPoint:="PostMessage")>
    Public Shared Function PostMessage(hWnd As IntPtr, Msg As Integer, wParam As IntPtr, ByRef lParam As COPYDATASTRUCT) As IntPtr
    End Function

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="hWnd"></param>
    ''' <param name="Msg"></param>
    ''' <param name="wParam"></param>
    ''' <param name="lParam"></param>
    ''' <returns></returns>
    <DllImport("User32.dll", EntryPoint:="PostMessage")>
    Public Shared Function PostMessage(hWnd As Integer, Msg As Integer, wParam As IntPtr, lParam As Integer) As IntPtr
    End Function

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="hWnd"></param>
    ''' <param name="Msg"></param>
    ''' <param name="wParam"></param>
    ''' <param name="lParam"></param>
    ''' <returns></returns>
    <DllImport("User32.dll", EntryPoint:="SendMessage")>
    Public Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As IntPtr
    End Function

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="hWnd"></param>
    ''' <returns></returns>
    <DllImport("User32.dll", EntryPoint:="SetForegroundWindow")>
    Public Shared Function SetForegroundWindow(hWnd As Integer) As Boolean
    End Function

    Public Function bringAppToFront(hWnd As Integer) As Boolean
        Return SetForegroundWindow(hWnd)
    End Function

    Public Function getWindowId(className As String, windowName As String) As Integer
        Return FindWindow(className, windowName)
    End Function

    Public Function sendWindowsMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As IntPtr
        Dim result As IntPtr = IntPtr.Zero

        If hWnd <> IntPtr.Zero Then
            result = SendMessage(hWnd, Msg, wParam, lParam)
        End If

        Return result
    End Function

    Public Function sendWindowsStringMessage(hWnd As IntPtr, wParam As IntPtr, Name As String, fID As UInteger, Tag As String, msg As String) As IntPtr
        Dim result As IntPtr = IntPtr.Zero

        If hWnd <> IntPtr.Zero Then

            Dim cds As New MyData

            cds.lpName = Name
            cds.fID = fID
            cds.lpTag = Tag
            cds.lpMsg = msg

            Dim pData As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds))
            Marshal.StructureToPtr(cds, pData, True)

            ' Create the COPYDATASTRUCT you'll use to shuttle the data.
            Dim copy As New COPYDATASTRUCT
            copy.dwData = IntPtr.Zero
            copy.lpData = pData
            copy.cdData = Marshal.SizeOf(cds)
            Dim pCopy As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(copy))
            Marshal.StructureToPtr(copy, pCopy, True)

            ' Send the message to the other application.

            result = SendMessage(hWnd, WM_COPYDATA, wParam, copy)
            Marshal.FreeHGlobal(pData)
            Marshal.FreeHGlobal(pCopy)

        End If

        Return result
    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="lpString"></param>
    ''' <returns></returns>
    <DllImport("user32.dll", EntryPoint:="SRegisterWindowMessage")>
    Private Shared Function RegisterWindowMessage(lpString As String) As Integer
    End Function

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="hWnd"></param>
    ''' <param name="Msg"></param>
    ''' <param name="wParam"></param>
    ''' <param name="lParam"></param>
    ''' <returns></returns>
    <DllImport("User32.dll", EntryPoint:="SendMessage")>
    Private Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, ByVal wParam As IntPtr, ByRef lParam As COPYDATASTRUCT) As IntPtr
    End Function

#End Region

End Class