Option Explicit On

Imports Microsoft.VisualBasic

Public Class PictureToRTF
    'Attribute VB_Name = "modRTFpic"


    Private Declare Function CreateDC Lib "gdi32" Alias "CreateDCA" _
       (ByVal lpDriverName As String, ByVal lpDeviceName As String, _
       ByVal lpOutput As Long, ByVal lpInitData As Long) As Long


    Private Declare Function BitBlt Lib "gdi32" ( _
        ByVal hDestDC As Long, _
        ByVal x As Long, _
        ByVal y As Long, _
        ByVal nWidth As Long, _
        ByVal nHeight As Long, _
        ByVal hSrcDC As Long, _
        ByVal xSrc As Long, _
        ByVal ySrc As Long, _
        ByVal dwRop As Long _
    ) As Long

    Private Declare Function CreateCompatibleDC Lib "gdi32" ( _
        ByVal hdc As Long _
    ) As Long

    Private Declare Function CreateMetaFile Lib "gdi32" Alias "CreateMetaFileA" ( _
        ByVal lpString As String _
    ) As Long

    Private Declare Function CloseMetaFile Lib "gdi32" ( _
        ByVal hDCMF As Long _
    ) As Long

    Private Declare Function DeleteMetaFile Lib "gdi32" ( _
        ByVal hMF As Long _
    ) As Long

    Private Declare Function DeleteDC Lib "gdi32" ( _
        ByVal hdc As Long _
    ) As Long

    Private Declare Function DeleteObject Lib "gdi32" ( _
        ByVal hObject As Long _
    ) As Long

    Private Declare Function GetObject Lib "gdi32" Alias "GetObjectA" ( _
        ByVal hObject As Long, _
        ByVal nCount As Long, _
        ByVal lpObject As Any _
    ) As Long

    Private Declare Function GetDC Lib "user32" ( _
        ByVal hwnd As Long _
    ) As Long

    Private Declare Function ReleaseDC Lib "user32" ( _
        ByVal hwnd As Long, _
        ByVal hdc As Long _
    ) As Long

    Private Declare Function RestoreDC Lib "gdi32" ( _
        ByVal hdc As Long, _
        ByVal nSavedDC As Long _
    ) As Long

    Private Declare Function SetMapMode Lib "gdi32" ( _
        ByVal hdc As Long, _
        ByVal nMapMode As Long _
    ) As Long

    Private Declare Function SetWindowExtEx Lib "gdi32" ( _
        ByVal hdc As Long, _
        ByVal nX As Long, _
        ByVal nY As Long, _
        ByVal lpSize As Size _
    ) As Long

    Private Declare Function SetWindowOrgEx Lib "gdi32" ( _
        ByVal hdc As Long, _
        ByVal nX As Long, _
        ByVal nY As Long, _
        ByVal lpPoint As POINTAPI _
    ) As Long

    Private Declare Function SaveDC Lib "gdi32" ( _
        ByVal hdc As Long _
    ) As Long

    Private Declare Function SelectObject Lib "gdi32" ( _
        ByVal hdc As Long, _
        ByVal hObject As Long _
    ) As Long

    Private Const MM_ANISOTROPIC = 8

    Private Structure Size
        Private x As Long
        Private y As Long
    End Structure

    Private Structure POINTAPI
        Private x As Long
        Private y As Long
    End Structure

    Private Structure BITMAP


        Private Type As Long
        Private Width As Long
        Private Height As Long
        Private WidthB As Long
        Private Planes As Long
        Private BitsPx As Long
        Private Bits As Long
    End Structure

    'Inserts the picture at the current insertion point
    Public Function InsertPicture(ByVal RTB As RichTextBox, ByVal pic As StdPicture)
        Dim strRTFall As String
        Dim lStart As Long
        With RTB
            .SelText = Chr(&H9D) & .SelText & Chr(&H81)
            strRTFall = .TextRTF
            strRTFall = Replace(strRTFall, "\'9d", PictureToRTF(pic))
            .TextRTF = strRTFall
            'position cursor past new insertion
            lStart = .Find(Chr(&H81))
            strRTFall = Replace(strRTFall, "\'81", "")
            .TextRTF = strRTFall
            .SelStart = lStart
        End With
    End Function

    'returns the RTF string representation of our picture
    Public Function PictureToRTF(ByVal pic As StdPicture) As String
        Dim hMetaDC As Long, hMeta As Long, hPicDC As Long, hOldBmp As Long
        Dim Bmp As BITMAP, Sz As Size, Pt As POINTAPI
        Dim sTempFile As String, screenDC As Long
        Dim headerStr As String, retStr As String, byteStr As String
        Dim ByteArr() As Byte, nBytes As Long
        Dim fn As Long, i As Long, j As Long

        sTempFile = App.Path & "\~pic" & ((Rnd * 1000000) + 1000000) \ 1 & ".tmp"  'some temprory file
        If Dir(sTempFile) <> "" Then Kill(sTempFile)

        'Create a metafile which is a collection of structures that store a
        'picture in a device-independent format.
        hMetaDC = CreateMetaFile(sTempFile)

        'set size of Metafile window
        SetMapMode(hMetaDC, MM_ANISOTROPIC)
        SetWindowOrgEx(hMetaDC, 0, 0, Pt)
        GetObject(pic.Handle, Len(Bmp), Bmp)
        SetWindowExtEx(hMetaDC, Bmp.Width, Bmp.Height, Sz)
        'save sate for later retrieval
        SaveDC(hMetaDC)

        'get DC compatible to screen
        screenDC = GetDC(0)
        hPicDC = CreateCompatibleDC(screenDC)
        ReleaseDC(0, screenDC)

        'set out picture as new DC picture
        hOldBmp = SelectObject(hPicDC, pic.Handle)

        'copy our picture to metafile
        BitBlt(hMetaDC, 0, 0, Bmp.Width, Bmp.Height, hPicDC, 0, 0, vbSrcCopy)

        'cleanup - close metafile
        SelectObject(hPicDC, hOldBmp)
        DeleteDC(hPicDC)
        DeleteObject(hOldBmp)
        'retrieve saved state
        RestoreDC(hMetaDC, True)
        hMeta = CloseMetaFile(hMetaDC)
        DeleteMetaFile(hMeta)

        'header to string we want to insert
        headerStr = "{\pict\wmetafile8" & _
                    "\picw" & pic.Width & "\pich" & pic.Height & _
                    "\picwgoal" & Bmp.Width * Screen.TwipsPerPixelX & _
                    "\pichgoal" & Bmp.Height * Screen.TwipsPerPixelY & _
                    " "

        'read metafile from disk into byte array
        nBytes = FileLen(sTempFile)
        ReDim ByteArr(0 To nBytes)

        fn = FreeFile()
        Microsoft.VisualBasic.Open(fn, sTempFile, openmode.input)
        Microsoft.VisualBasic.Get(fn, , ByteArr)
        Microsoft.VisualBasic.Close(fn)


        Dim nlines As Long

        'turn each byte into two char hex value
        i = 0
        byteStr = ""
        Do
            byteStr = byteStr & vbCrLf
            For j = 1 To 39
                i = i + 1
                If i > nBytes Then Exit For
                byteStr = byteStr & Hex00(ByteArr(i))
            Next j
        Loop While i < nBytes

        'string we will be inserting
        retStr = headerStr & LCase(byteStr) & vbCrLf & "}"
        PictureToRTF = retStr

        'remove temp metafile
        Kill(sTempFile)

    End Function

    'adds leading zero to hex value if needed.
    Public Function Hex00(ByVal icolor As Byte) As String
        Hex00 = Right("0" & Hex(icolor), 2)
    End Function






End Class
