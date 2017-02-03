'Move All DLL imports here


'Author Gerolkae
Imports System.Runtime.InteropServices

Namespace Controls
    Public Class Win32

#Region "Interop-Defines"
        <StructLayout(LayoutKind.Sequential)>
        Friend Structure CHARFORMAT2_STRUCT
            Public cbSize As Integer
            Public dwMask As Integer
            Public dwEffects As Integer
            Public yHeight As Integer
            Public yOffset As Integer
            Public crTextColor As Integer
            Public bCharSet As Byte
            Public bPitchAndFamily As Byte
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)>
            Public szFaceName As Char()
            Public wWeight As Short
            Public sSpacing As Short
            Public crBackColor As Integer
            ' Color.ToArgb() -> int
            Public lcid As Integer
            Public dwReserved As Integer
            Public sStyle As Short
            Public wKerning As Short
            Public bUnderlineType As Byte
            Public bAnimation As Byte
            Public bRevAuthor As Byte
            Public bReserved1 As Byte
        End Structure

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Friend Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        End Function


        Friend Shared updating As Integer
        Friend Shared oldEventMask As IntPtr
        Friend Const WM_USER As Integer = &H400
        Friend Const EM_GETCHARFORMAT As Integer = WM_USER + 58
        Friend Const EM_SETCHARFORMAT As Integer = WM_USER + 68

        Friend Const SCF_SELECTION As Integer = &H1
        Friend Const SCF_WORD As Integer = &H2
        Friend Const SCF_ALL As Integer = &H4

        Friend Const WM_SETREDRAW As Integer = &HB
        Friend Const EM_GETEVENTMASK As Integer = WM_USER + 59
        Friend Const EM_SETEVENTMASK As Integer = WM_USER + 69

#Region "CHARFORMAT2 Flags"
        Friend Const CFE_BOLD As Integer = &H1
        Friend Const CFE_ITALIC As Integer = &H2
        Friend Const CFE_UNDERLINE As Integer = &H4
        Friend Const CFE_STRIKEOUT As Integer = &H8
        Friend Const CFE_PROTECTED As Integer = &H10
        Friend Const CFE_LINK As Integer = &H20
        Friend Const CFE_AUTOCOLOR As Integer = &H40000000
        Friend Const CFE_SUBSCRIPT As Integer = &H10000
        ' Superscript and subscript are 
        Friend Const CFE_SUPERSCRIPT As Integer = &H20000
        '  mutually exclusive			 

        Friend Const CFM_SMALLCAPS As Integer = &H40
        ' (*)	
        Friend Const CFM_ALLCAPS As Integer = &H80
        ' Displayed by 3.0	
        Friend Const CFM_HIDDEN As Integer = &H100
        ' Hidden by 3.0 
        Friend Const CFM_OUTLINE As Integer = &H200
        ' (*)	
        Friend Const CFM_SHADOW As Integer = &H400
        ' (*)	
        Friend Const CFM_EMBOSS As Integer = &H800
        ' (*)	
        Friend Const CFM_IMPRINT As Integer = &H1000
        ' (*)	
        Friend Const CFM_DISABLED As Integer = &H2000
        Friend Const CFM_REVISED As Integer = &H4000

        Friend Const CFM_BACKCOLOR As Integer = &H4000000
        Friend Const CFM_LCID As Integer = &H2000000
        Friend Const CFM_UNDERLINETYPE As Integer = &H800000
        ' Many displayed by 3.0 
        Friend Const CFM_WEIGHT As Integer = &H400000
        Friend Const CFM_SPACING As Integer = &H200000
        ' Displayed by 3.0	
        Friend Const CFM_KERNING As Integer = &H100000
        ' (*)	
        Friend Const CFM_STYLE As Integer = &H80000
        ' (*)	
        Friend Const CFM_ANIMATION As Integer = &H40000
        ' (*)	
        Friend Const CFM_REVAUTHOR As Integer = &H8000


        Friend Const CFM_BOLD As Integer = &H1
        Friend Const CFM_ITALIC As Integer = &H2
        Friend Const CFM_UNDERLINE As Integer = &H4
        Friend Const CFM_STRIKEOUT As Integer = &H8
        Friend Const CFM_PROTECTED As Integer = &H10
        Friend Const CFM_LINK As Integer = &H20
        Friend Const CFM_SIZE As Integer = &H80000000
        Friend Const CFM_COLOR As Integer = &H40000000
        Friend Const CFM_FACE As Integer = &H20000000
        Friend Const CFM_OFFSET As Integer = &H10000000
        Friend Const CFM_CHARSET As Integer = &H8000000
        Friend Const CFM_SUBSCRIPT As Integer = CFE_SUBSCRIPT Or CFE_SUPERSCRIPT
        Friend Const CFM_SUPERSCRIPT As Integer = CFM_SUBSCRIPT

        Friend Const CFU_UNDERLINENONE As Byte = &H0
        Friend Const CFU_UNDERLINE As Byte = &H1
        Friend Const CFU_UNDERLINEWORD As Byte = &H2
        ' (*) displayed as ordinary underline	
        Friend Const CFU_UNDERLINEDOUBLE As Byte = &H3
        ' (*) displayed as ordinary underline	
        Friend Const CFU_UNDERLINEDOTTED As Byte = &H4
        Friend Const CFU_UNDERLINEDASH As Byte = &H5
        Friend Const CFU_UNDERLINEDASHDOT As Byte = &H6
        Friend Const CFU_UNDERLINEDASHDOTDOT As Byte = &H7
        Friend Const CFU_UNDERLINEWAVE As Byte = &H8
        Friend Const CFU_UNDERLINETHICK As Byte = &H9
        Friend Const CFU_UNDERLINEHAIRLINE As Byte = &HA
        ' (*) displayed as ordinary underline	

#End Region

#Region "Scrollbar position"
        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Shared Function GetScrollPos(ByVal hWnd As IntPtr, ByVal nBar As Integer) As Integer
        End Function

        <DllImport("user32.dll")>
        Public Shared Function SetScrollPos(ByVal hWnd As IntPtr, ByVal nBar As Integer, ByVal nPos As Integer, ByVal bRedraw As Boolean) As Integer
        End Function

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
        <Serializable(), StructLayout(LayoutKind.Sequential)>
        Structure SCROLLINFO
            Public cbSize As UInteger
            <MarshalAs(UnmanagedType.U4)> Public fMask As ScrollInfoMask
            Public nMin As Integer
            Public nMax As Integer
            Public nPage As UInteger
            Public nPos As Integer
            Public nTrackPos As Integer
        End Structure
        Public Function GetScrollInfo(hWnd As IntPtr,
        <MarshalAs(UnmanagedType.I4)> fnBar As SBOrientation,
        <MarshalAs(UnmanagedType.Struct)> ByRef lpsi As SCROLLINFO) As Integer
        End Function

        Friend Const SB_HORZ As Integer = &H0
        Friend Const SB_VERT As Integer = &H1


#End Region



#End Region

    End Class

End Namespace
