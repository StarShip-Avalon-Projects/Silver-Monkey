Imports System.Runtime.InteropServices

Namespace HelperClasses

    ''' <summary>
    ''' Native Win32 Methods
    ''' </summary>
    Public NotInheritable Class NativeMethods
        Public Enum SBOrientation As Integer
            SB_HORZ = &H0
            SB_VERT = &H1
            SB_CTL = &H2
            SB_BOTH = &H3
        End Enum

        Public Enum ScrollInfoMask As UInteger
            SIF_RANGE = &H1
            SIF_PAGE = &H2
            SIF_POS = &H4
            SIF_DISABLENOSCROLL = &H8
            SIF_TRACKPOS = &H10
            SIF_ALL = (SIF_RANGE Or SIF_PAGE Or SIF_POS Or SIF_TRACKPOS)
        End Enum

        Public Declare Function GetScrollInfo Lib "user32.dll" (hWnd As IntPtr,
                <MarshalAs(UnmanagedType.I4)> fnBar As SBOrientation,
                <MarshalAs(UnmanagedType.Struct)> ByRef lpsi As SCROLLINFO) As Integer

        Public Declare Function GetScrollPos Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nBar As Integer) As Integer

        Public Declare Function GetScrollRange Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nBar As Integer,
                ByRef lpMinPos As Integer,
                ByRef lpMaxPos As Integer) As Boolean

        Structure SCROLLINFO

#Region "Public Fields"

            Public Property CbSize As Integer
            <MarshalAs(UnmanagedType.U4)> Public fMask As ScrollInfoMask
            Public Property NMax As Integer
            Public Property NMin As Integer
            Public Property NPage As UInteger
            Public Property NPos As Integer
            Public Property NTrackPos As Integer

#End Region

        End Structure

    End Class
End Namespace
