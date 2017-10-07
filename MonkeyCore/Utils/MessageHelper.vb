Imports System.Runtime.InteropServices
Imports MonkeyCore.Controls.NativeMethods
Imports MonkeyCore.MyData

Namespace Utils

    ''' <summary>
    ''' Managed Windows Messaging 
    ''' </summary>
    Public Class MessageHelper

#Region "Public Methods"

        Public Function bringAppToFront(hWnd As Integer) As Boolean
            Return SetForegroundWindow(hWnd)
        End Function

        Public Function getWindowId(className As String, windowName As String) As Integer
            Return FindWindow(className, windowName)
        End Function

        Public Function SendWindowsMessage(hWnd As IntPtr, Msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
            Dim result As IntPtr = IntPtr.Zero

            If hWnd <> IntPtr.Zero Then
                result = SendMessage(hWnd, Msg, wParam, lParam)
            End If

            Return result
        End Function

        Public Function SendWindowsStringMessage(hWnd As IntPtr, wParam As IntPtr, Name As String, fID As UInteger, Tag As String, msg As String) As IntPtr
            Dim result As IntPtr = IntPtr.Zero

            If hWnd <> IntPtr.Zero Then

                Dim cds As MyDataStructure
                cds.lpName = Name
                cds.fID = fID
                cds.lpTag = Tag
                cds.lpMsg = msg

                Dim pData As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds))
                Marshal.StructureToPtr(cds, pData, True)

                ' Create the COPYDATASTRUCT you'll use to shuttle the data.
                Dim copy As COPYDATASTRUCT
                copy.dwData = IntPtr.Zero
                copy.lpData = pData
                copy.cdData = Marshal.SizeOf(cds)
                Dim pCopy As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(copy))
                Marshal.StructureToPtr(copy, pCopy, True)

                ' Send the message to the other application.

                result = SendMessage(hWnd, WM_COPYDATA, wParam, pCopy)
                Marshal.FreeHGlobal(pData)
                Marshal.FreeHGlobal(pCopy)

            End If

            Return result
        End Function

#End Region

    End Class

End Namespace