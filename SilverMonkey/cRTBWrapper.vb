'**********************************************************************************
'   Class: cRTBWrapper.vb
' Purpose: Provide minipulation of RTF documents.
'          This is a stripped down class and only applys
'          the minimum information needed to create a working
'          RTF file.  The only attribute supported is foreground 
'          color and was written to provide context hilighting.
'
'  Author: Matthew Hazlett
'    Date: Jan, 30th, 2004
'
'Changes: December 5th, 2012
'        Modified by Gerolkae
'         Removed debugging procedures
'        Fixed varias bugs and Code formatting issues



' Changes: Jan 31th 
'          Sucessfully transformed an entire document to color RTF.  
'          This turned out to be tremendiously slow in it's implementation
'          for editing for large documents.
'
'          Feb 1st
'          Changed code to provide on the fly streaming and modify
'          only a single line at a time.  Buggy Still !
'
'          Feb 2nd
'          Completly retooled the code to make it more efficent.  The class was 
'          getting to complex and needed a rewrite.
'
'          Why When I think I'm almost done do I find a pretty big snag.  
'          Everything's working great BUT the shifting headers on the RTF are
'          changing text on the other lines.  I guess I will have to make them
'          more static.  
'
'          GOT IT ! (Saved my dynamic headers !!!!)
'          Turns out after about 3 hours MS richedit was changing my RTF to their
'          prefered method.  Heh, well I ended up doing it the richedit way.  God
'          this control is tough.  So in conclusion this control is VERY PICKEY 
'          and if you change this code and screw it up don't come crying to me :-)
'
'          Feb 3nd
'          - Fixed Interface bug where control were created in wrong sequence
'          - Fixed scroll bug where text would scroll in viewport was full had had scrollbars
'          - Fixed mystery space bug where on subsequent lines the second char could not be space
'          - Fixed } and { filtering bug
'          (Maybe I should have been in pest control?)
'
'          Feb 4th
'          - Added support for mouse events
'          - Rewrote CurrentLine function to support wordwrap
'          - Fixed top line empty bug
'          - Changed coloring scheme
'
'          Feb 5th
'          - Added better scroll handeling, LockWindow wasn't cutting 
'            the mustard by itself.
'          - 546 lines of code and more stuff to add!
'
'          Feb 6th
'          - Added RegEx matching (HTML, XML etc...) [ex. "<img.*>" = "<img src=regex.gif>"]
'          - Added caseInsensitive option
'          - Added Syntax Highlighter Form
'          - Added colorDocument method
'**********************************************************************************
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices.Marshal
Imports System.Runtime.InteropServices
'Imports SilverMonkey.ConfigStructs
Imports No_Flicker
Imports System.Text
Imports System.Threading

Public Class cRTBWrapper
    ' Scrollbar direction
    Private Const SBS_HORZ As Integer = &H0
    Private Const SBS_VERT As Integer = &H1




    ' Windows Messages
    Const WM_VSCROLL As Integer = &H115
    Const WM_HSCROLL As Integer = &H114
    Const SB_THUMBPOSITION As Integer = 4

    ' This is just a class structore that holds syntax options
    Public Class tDict
        Private _Pattern As String
        Private _isRegex As Boolean
        Private _ignoreCase As Boolean
        Private _color As Integer

        Public Sub New(ByVal thispattern As String, ByVal thisregex As Boolean, ByVal thisCase As Boolean, ByVal thisColor As Integer)
            _Pattern = thispattern
            _isRegex = thisregex
            _ignoreCase = thisCase
            _color = thisColor
        End Sub

        Public Property pattern() As String
            Get
                Return _Pattern
            End Get
            Set(ByVal Value As String)
                _Pattern = Value
            End Set
        End Property

        Public Property isRegex() As Boolean
            Get
                Return _isRegex
            End Get
            Set(ByVal Value As Boolean)
                _isRegex = Value
            End Set
        End Property

        Public Property ignoreCase() As Boolean
            Get
                Return _ignoreCase
            End Get
            Set(ByVal Value As Boolean)
                _ignoreCase = Value
            End Set
        End Property

        Public Property color() As Integer
            Get
                Return _color
            End Get
            Set(ByVal Value As Integer)
                _color = Value
            End Set
        End Property

    End Class

    ' This is just a dictionary class used to store your color info
    Public Class cDict
        Inherits CollectionBase

        Sub New()
        End Sub

        Sub add(ByVal Pattern As String, ByVal isRegex As Boolean, ByVal isCase As Boolean, ByVal value As Integer)
            If exists(Pattern) = -1 Then
                List.Add(New tDict(Pattern, isRegex, isCase, value))
            End If
        End Sub

        Public Overloads Sub Clear()
            List.Clear()
        End Sub

        Public Property item(ByVal index As Integer) As Integer
            Get
                Return List(index)
            End Get
            Set(ByVal Value As Integer)
                List(index) = Value
            End Set
        End Property

        Function exists(ByVal lookup As String) As Integer
            Dim entry As tDict

            For Each entry In List
                If entry.pattern = lookup Then Return entry.color
            Next

            Return -1
        End Function

        Function index(ByVal lookup As String) As Integer
            Dim entry As tDict
            Dim thisIndex As Integer = 1

            For Each entry In List
                If entry.pattern = lookup Then Return thisIndex
                thisIndex += 1
            Next

            Return -1  ' Make it black
        End Function
    End Class

    ' This is just a list class used to store the headers color info
    Private Class cList
        Inherits CollectionBase

        Sub New()
        End Sub

        Sub add(ByVal item As Integer)
            If exists(item) = -1 Then
                List.Add(item)
                ' Console.WriteLine("New color: " & item)
            End If
        End Sub

        Sub remove(ByVal item As Integer)
            If exists(item) <> -1 Then
                List.Add(item)
                ' Console.WriteLine("New color: " & item)
            End If

        End Sub

        Public Property item(ByVal index As Integer) As Integer
            Get
                Return List(index)
            End Get
            Set(ByVal Value As Integer)
                List(index) = Value
                ' Console.WriteLine("Setting color")
            End Set
        End Property

        Function exists(ByVal lookup As Integer) As Integer
            Dim current As Integer

            If List.Count <> 0 Then
                For current = 0 To List.Count - 1
                    Dim compare As Color = Color.FromArgb(lookup)
                    Dim source As Color = Color.FromArgb(List(current))

                    ' This is very strange, the samme RGB color can have diffrent
                    ' ARGB values ???? Maybe its reporting 'A' diffrently, oh well
                    ' lets just work around that little feature

                    If compare.R = source.R And _
                       compare.G = source.G And _
                       compare.B = source.B Then
                        Return current
                    End If
                Next
            End If

            Return -1
        End Function
    End Class

    ' This is just a class used to store the position info
    Public Structure cPosition
        Public Cursor As Integer
        Public CurrentLine As Integer
        Public LinePosition As Integer
        Public xScroll As Integer
        Public yScroll As Integer
    End Structure

    ' Public events
    Public Event position(ByVal PositionInfo As cPosition)

    ' Vars
    'Dim cMain As cMain
    Private WithEvents _bind As RichTextBox2
    Public rtfSyntax As New cDict
    Private rtfColors As New cList
    Private rtfHeader As String
    Private rtfBody() As String
    Private txtBody() As String
    Private CursorPosition As cPosition
    ' Private DebugView As New cRTFDebug
    Private RTFDebug As Boolean = False

    Private oldEventMask As IntPtr
    Private updating As Integer = 0
    Private Const WM_USER As Integer = &H400
    Private Const WM_SETREDRAW As Integer = &HB
    Private Const EM_GETEVENTMASK As Integer = WM_USER + 59
    Private Const EM_SETEVENTMASK As Integer = WM_USER + 69
    '--------------------------------------------------------------------------
    '     Sub: New
    ' Purpose: This was the most chalanging part of the entire project.
    '          How to write this, humm.  Maybe i'll save it till later.
    '          Ya, later!
    '

    'Dim _bind As RichTextBox2 = Nothing
    Public Sub New()

    End Sub

    '--------------------------------------------------------------------------
    '     Sub: Bind
    ' Purpose: Provide access to the object and its events
    '
    Public Sub bind(ByRef rtb As RichTextBox2)
        _bind = rtb
        AddHandler _bind.KeyUp, AddressOf update
        AddHandler _bind.MouseUp, AddressOf update
        AddHandler _bind.TextChanged, AddressOf update

    End Sub

    Public Sub unbind()
        If IsNothing(_bind) Then Exit Sub
        RemoveHandler _bind.KeyUp, AddressOf update
        RemoveHandler _bind.MouseUp, AddressOf update
        RemoveHandler _bind.TextChanged, AddressOf update
        _bind = Nothing
    End Sub

    <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    '--------------------------------------------------------------------------
    '     API: GetScrollPos
    ' Purpose: Returns an integer of the position of the scrollbar
    '
    <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    Public Shared Function GetScrollPos(ByVal hWnd As IntPtr, ByVal nBar As Integer) As Integer
    End Function

    '--------------------------------------------------------------------------
    '     API: SetScrollPos
    ' Purpose: Sets the scrollbars to a certin value
    '
    Private Declare Function SetScrollPos Lib "user32.dll" ( _
        ByVal hWnd As IntPtr, ByVal nBar As Integer, _
        ByVal nPos As Integer, ByVal bRedraw As Boolean) As Integer

    '--------------------------------------------------------------------------
    '     API: PostMessageA
    ' Purpose: Sends a message to a control
    '
    Private Declare Function PostMessageA Lib "user32.dll" ( _
        ByVal hwnd As IntPtr, ByVal wMsg As Integer, _
        ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    '--------------------------------------------------------------------------
    '     API: LockWindowUpdate
    ' Purpose: Locks or Unlocks a window
    '

    ' Private Declare Function LockWindowUpdate Lib "user32.dll" (ByVal hWndLock As IntPtr) As Boolean
    '--------------------------------------------------------------------------
    '     Sub: Update
    ' Purpose: Reports the curssor position (Customized for word wrap support)
    '
    Private Overloads Sub update()
        CursorPosition = getCurrentPosition()
        RaiseEvent position(CursorPosition)
        ' debugprint(_bind.Rtf)
    End Sub
    Private Overloads Sub update(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        update()
    End Sub
    Private Overloads Sub update(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        update()
    End Sub
    Private Overloads Sub update(ByVal sender As Object, ByVal e As System.EventArgs)
        CursorPosition = getCurrentPosition()
        rtfColors.Clear()   ' Clear the colors
        readRTFColor()      ' Read and parse the colors in the current document
        readTXTBody()       ' Read and split the text into lines
        readRTFBody()       ' Read and split the RTF into lines


    End Sub

    '--------------------------------------------------------------------------
    '     Sub: asciiprint
    ' Purpose: Help in debugging, converts line to ascii char numbers
    '
    Private Function asciiprint(ByVal str As String) As String
        'Dim counter As Integer
        Dim retval As String = ""

        For counter As Integer = 1 To str.Length
            retval &= "{" & Asc(Mid(str, counter, 1)) & "} "
        Next

        Return retval
    End Function

    '--------------------------------------------------------------------------
    '     Sub: debugprint
    ' Purpose: Prints out the current document substitutring non-printables
    '
    'Private Sub debugprint(ByVal out As String)
    '    out = out.Replace(Chr(13) & Chr(10), Chr(182) & vbCrLf)
    '    out = out.Replace(Chr(32), Chr(183))
    '    DebugView.rtfOut.Text = out
    'End Sub

    '--------------------------------------------------------------------------
    '     Sub: toggleDebug
    ' Purpose: Hide or show the debug window
    '
    'Public Function toggleDebug() As Boolean
    '    RTFDebug = Not RTFDebug
    '    If RTFDebug Then DebugView.Show() Else DebugView.Hide()
    '    Return RTFDebug
    'End Function

    '--------------------------------------------------------------------------
    '     Sub: getCurrentPosition
    ' Purpose: Determins relitive line position
    '
    Private Function getCurrentPosition() As cPosition
        Dim retval As New cPosition
        retval.Cursor = _bind.SelectionStart

        If _bind.Text <> "" Then


            retval.CurrentLine = _bind.GetLineFromCharIndex(_bind.SelectionStart)
            retval.LinePosition = _bind.SelectionStart - _bind.GetFirstCharIndexFromLine(retval.CurrentLine)
        End If

        Return retval
    End Function

    '--------------------------------------------------------------------------
    '     Sub: saveScroll
    ' Purpose: saves the scroll info and locks the window
    '
    Private Sub saveScroll(ByVal hWnd As IntPtr)

        updating += 1

        If updating > 1 Then
            CursorPosition.xScroll = GetScrollPos(hWnd, SBS_HORZ)
            CursorPosition.yScroll = GetScrollPos(hWnd, SBS_VERT)
            Exit Sub
        End If
        'LockWindowUpdate(hWnd)
        oldEventMask = SendMessage(hWnd, EM_SETEVENTMASK, 0, 0)

        ' Prevent the control from redrawing itself.
        SendMessage(hWnd, WM_SETREDRAW, 0, 0)
        CursorPosition.xScroll = GetScrollPos(hWnd, SBS_HORZ)
        CursorPosition.yScroll = GetScrollPos(hWnd, SBS_VERT)
    End Sub

    '--------------------------------------------------------------------------
    '     Sub: restoreScroll
    ' Purpose: Resets the scroll info ans unlocks the window
    '
    Private Sub restoreScroll(ByVal hWnd As IntPtr)
        SetScrollPos(_bind.Handle, SBS_HORZ, CursorPosition.xScroll, True)
        PostMessageA(_bind.Handle, WM_HSCROLL, CursorPosition.xScroll, Nothing)
        SetScrollPos(_bind.Handle, SBS_VERT, CursorPosition.yScroll, True)
        PostMessageA(_bind.Handle, WM_VSCROLL, CursorPosition.yScroll, Nothing)


        updating -= 1

        If updating > 1 Then

            Return
        End If
        ' LockWindowUpdate(0&)

        ' Allow the control to redraw itself.
        SendMessage(hWnd, WM_SETREDRAW, 1, 0)

        ' Allow the control to raise event messages.
        SendMessage(hWnd, EM_SETEVENTMASK, 0, oldEventMask)
    End Sub

    '--------------------------------------------------------------------------
    '     Sub: _bind_TextChanged
    ' Purpose: Do the highlighting
    '
    Private Sub _bind_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles _bind.KeyUp

        If e.KeyData = Keys.Space Or e.KeyData = Keys.OemPeriod Then
            update()
            ' Freeze the windows and get the scroll nfo
            Dim line As Integer = _bind.GetLineFromCharIndex(_bind.SelectionStart - 1)
            Dim lineStart = _bind.GetFirstCharIndexFromLine(line)
            _bind.SelectionStart = lineStart
            _bind.SelectionLength = txtBody(line).Length
            saveScroll(_bind.Handle)
            applyColor(line)        ' Do any coloring
            _bind.SelectedRtf = RenderLine2(line)                          ' Update the RTF
            _bind.SelectionStart = CursorPosition.Cursor  ' Reset the cursor
            'debugprint(_bind.Rtf)                         ' Update the debug window

            restoreScroll(_bind.Handle) ' Unfreeze the windows and Restore the scroll
            ' Update the cursor 

        ElseIf e.KeyData = Keys.Enter Then
            update()
            saveScroll(_bind.Handle)     ' Freeze the windows and get the scroll nfo
            ' CursorPosition.CurrentLine -= 1
            ' Do any coloring
            Dim str As String = _bind.SelectedRtf
            Dim CharPosition As Integer = _bind.SelectionStart

            Dim line As Integer = _bind.GetLineFromCharIndex(_bind.SelectionStart)
            Dim str2 As String = _bind.Lines(line)
            Dim pos As Integer = _bind.GetFirstCharIndexFromLine(line)
            Dim str3 As String = txtBody(line - 1)
            _bind.SelectionStart = _bind.SelectionStart - txtBody(line - 1).Length - 1

            Dim i1 As Integer = _bind.Lines(line - 1).Length
            Dim i2 As Integer = _bind.Lines(line).Length
            Dim Length As Integer = i1 + i2 + 1


            _bind.SelectionLength = Length
            applyColor(line - 1)
            applyColor(line)
            _bind.SelectedRtf = RenderLine3(line)
            ' Do any coloring
            ' Update the RTF
            _bind.SelectionStart = CursorPosition.Cursor  ' Reset the cursor
            'debugprint(_bind.Rtf)                         ' Update the debug window

            restoreScroll(_bind.Handle) ' Unfreeze the windows and Restore the scroll
            ' Update the cursor
            e.Handled = True
        End If
    End Sub

    '--------------------------------------------------------------------------
    '     Sub: readRTFColor()
    ' Purpose: parse the color information in the header of the document
    '
    Private Sub readRTFColor()
        Dim strHeader As String = ""

        ' Get Header string
        ' I hate RegEx :-)
        '
        Dim colHeader As MatchCollection = Regex.Matches(_bind.Rtf, "{\\colortbl\s?;(.*?);}")

        If RTFDebug Then Console.WriteLine("Colors found: " & colHeader.Count)

        If colHeader.Count = 1 Then
            strHeader = colHeader.Item(0).Groups(1).Value
            If RTFDebug Then Console.WriteLine(colHeader.Item(0).Groups(1).Value)
        Else
            If RTFDebug Then Console.WriteLine("No color info in header")
            Exit Sub
        End If

        ' Translate the string to ARGB color values
        ' I hate RegEx :-)
        '
        Dim colColors As MatchCollection = Regex.Matches(strHeader, "(\d+)")

        If Not colColors Is Nothing Then
            Dim colorCounter As Integer

            For colorCounter = 0 To colColors.Count - 1 Step 3
                Dim newColor As Color = Color.FromArgb(0, _
                                        colColors.Item(colorCounter).Value, _
                                        colColors.Item(colorCounter + 1).Value, _
                                        colColors.Item(colorCounter + 2).Value)

                rtfColors.add(newColor.ToArgb)
            Next
        End If
    End Sub

    '--------------------------------------------------------------------------
    '     Sub: readRTFBody()
    ' Purpose: Read the RTF and strip off the header info, and split it into limes.
    '          RegEx avoided here !
    '
    Private Sub readRTFBody()

        ReDim rtfBody(txtBody.Length - 1)
    End Sub

    '--------------------------------------------------------------------------
    '     Sub: readTXTBody()
    ' Purpose: Split the text portion into lines
    '          RegEx avoided here !
    '
    Private Sub readTXTBody()
        'Dim tmpText As String
        ''Dim counter As Integer

        'tmpText = _bind.Text
        '_bind.Text = _bind.Text.Replace("\", "\\")
        '_bind.Text = _bind.Text.Replace("}", "\}")
        '_bind.Text = _bind.Text.Replace("{", "\{")

        txtBody = _bind.Lines

        'For counter As Integer = 0 To txtBody.Length - 1
        '    If txtBody(counter) Is Nothing Then
        '        txtBody(counter) = ""
        '    End If
        'Next

        If RTFDebug Then Console.WriteLine("")
        If RTFDebug Then Console.WriteLine("Text lines read: " & txtBody.Length.ToString)
    End Sub

    '--------------------------------------------------------------------------
    '     Sub: Render()
    ' Purpose: Put the moded RTF back together
    '
    Private Function Render() As String
        'Dim tmp As String = reBuildBody()
        'Dim FontSize As Integer = cMain.ApFont.Size * 2
        ''\uc1\fs" & FontSize.ToString & " \cf1
        Return reBuildHeader() & "\viewkind4 " & reBuildBody()
    End Function
    Public Function RenderLine(ByRef line As Integer) As String
        Return reBuildHeader() & "\viewkind4 " & RebuildLine(line)
    End Function
    Public Function RenderLine2(ByRef line As Integer) As String
        Return reBuildHeader() & "\viewkind4 " & RebuildLine2(line)
    End Function
    Public Function RenderLine3(ByRef line As Integer) As String
        Dim str As String = RebuildLine(line - 1)
        Dim str2 As String = RebuildLine2(line)
        Return reBuildHeader() & "\viewkind4 " & str & str2
    End Function
    '--------------------------------------------------------------------------
    '     Sub: reBuildHeader()
    ' Purpose: Using the colortable supplied by readRTFColor() rebuilds the 
    '          headers after all you might have added a color!
    '
    Private Function reBuildHeader() As String
        'Dim thisColor As Integer

        'Dim FontFace As String = cMain.ApFont.Name
        Dim DocHead As String

        DocHead = "{\rtf1\ansi\ansicpg1252\deff0\deflang1033"
        DocHead &= "{\colortbl ;"

        For Each thisColor As Integer In rtfColors
            Dim setColor As Color = Color.FromArgb(thisColor)
            DocHead &= "\red" & setColor.R
            DocHead &= "\green" & setColor.G
            DocHead &= "\blue" & setColor.B & ";"
            If RTFDebug Then Console.WriteLine("Adding Header Color")
        Next
        ''Return "{\rtf1\ansi\ansicpg1252\deff0\deflang1033" & ColorString & "{\fonttbl{\f0\fcharset0 " & FontFace & ";}}\viewkind4\uc1\fs" & FontSize.ToString & " \cf1 " & Data & " \par}"
        ''"{\fonttbl{\f0\fcharset0 " & FontFace & ";}}"
        DocHead &= "}"
        Return DocHead
    End Function

    '--------------------------------------------------------------------------
    '     Sub: reBuildBody()
    ' Purpose: Build the moded RTF Body
    '
    Private Function reBuildBody() As String

        Dim strb As New StringBuilder
        For counter As Integer = 0 To rtfBody.Length - 1
            If counter = rtfBody.Length - 1 Then
                strb.AppendLine(rtfBody(counter))
            Else
                strb.AppendLine(rtfBody(counter) & "\par")
            End If

        Next

        If RTFDebug Then Console.WriteLine("RTF body lines rendered: " & rtfBody.Length)
        Return strb.ToString
    End Function

    Private Function RebuildLine(ByRef line As Integer) As String
        Return rtfBody(line) & "\par" & vbLf
    End Function
    Private Function RebuildLine2(ByRef line As Integer) As String
        Dim str As String = rtfBody(line)
        Return str
    End Function
    '--------------------------------------------------------------------------
    '     Sub: ChangeColor
    ' Purpose: Change the color of an element document wide.  Basicly this changes
    '          the info in the color table used to build the headers.  
    '    Note: This changes a color to a color
    '
    Private Sub changeColor(ByRef srcColor As Color, ByRef toColor As Color)
        Dim index As Integer = rtfColors.exists(srcColor.ToArgb)

        If index <> -1 Then
            rtfColors.item(index) = toColor.ToArgb
        End If
    End Sub

    '--------------------------------------------------------------------------
    '     Sub: ChangeColor
    ' Purpose: Change the color of an element document wide.  Basicly this changes
    '          the info in the color table used to build the headers.  
    '    Note: This changes a index value of a color to a color
    '
    Public Sub changeColor(ByRef index As Integer, ByRef toColor As Color)
        rtfColors.item(index) = toColor.ToArgb
    End Sub



    '--------------------------------------------------------------------------
    '     Sub: applyColor
    ' Purpose: Apply a new color format to a line
    '
    Private Sub applyColor(ByVal line As Integer)
        'Dim thisMatch As Match
        Dim Style As tDict
        Dim rxOptions As New RegexOptions
        Dim colorindex As Integer

        If line = -1 Then Exit Sub
        rtfBody(line) = txtBody(line).Replace("\", "\\").Replace("}", "\}").Replace("{", "\{")
        'rtfBody(line) = txtBody(line)


        For Each Style In rtfSyntax
            If Style.ignoreCase Then rxOptions = RegexOptions.IgnoreCase Else rxOptions = RegexOptions.None

            rtfColors.add(Style.color)
            colorindex = rtfColors.exists(Style.color)

            If Style.isRegex Then
                Dim Matches As MatchCollection = Regex.Matches(rtfBody(line), Style.pattern, rxOptions)
                Dim count As Integer = 0

                For Each thisMatch As Match In Matches
                    ' Dim oldWord As String = ""

                    Dim newString As String = ""
                    'Dim oldWord As String = ""


                    newString = "\cf" & colorindex + 1 & " " & thisMatch.ToString & "\cf0 "

                    'oldWord = rtfBody(line).Substring(thisMatch.Index + count, thisMatch.Length)

                    rtfBody(line) = rtfBody(line).Remove(thisMatch.Index + count, thisMatch.Length)
                    rtfBody(line) = rtfBody(line).Insert(thisMatch.Index + count, newString)
                    If RTFDebug Then Console.WriteLine("Regex pattern match: " & Style.pattern)
                    count += 10

                Next
            Else
                Dim Matches As MatchCollection = Regex.Matches(rtfBody(line), "\b " & Style.pattern & "\b0 ", rxOptions)
                Dim count As Integer = 0

                For Each thisMatch As Match In Matches
                    Dim oldWord As String = rtfBody(line).Substring(thisMatch.Index + count, thisMatch.Length)
                    Dim newString As String = "\cf" & colorindex + 1 & " " & oldWord & "\cf0 "

                    rtfBody(line) = rtfBody(line).Remove(thisMatch.Index + count, thisMatch.Length)
                    rtfBody(line) = rtfBody(line).Insert(thisMatch.Index + count, newString)

                    count += 10
                    If RTFDebug Then Console.WriteLine("Pattern match: " & Style.pattern)
                Next
            End If

        Next

    End Sub

    Public Sub ColorLine(ByRef line As Integer)

        update()
        Dim lineStart = _bind.GetFirstCharIndexFromLine(line)
        _bind.SelectionStart = lineStart
        _bind.SelectionLength = txtBody(line).Length
        applyColor(line)
        _bind.SelectedRtf = RenderLine(line)

    End Sub

    Public Sub colorDocument()
        'Dim counter As Integer
        Dim progress As New ProgressFrm("Coloring File")
        progress.Show()
        update(Me, New System.EventArgs)

        For counter As Integer = 0 To txtBody.Length - 1
            applyColor(counter)
            progress.UpdateProgress((counter / txtBody.Length) * 100)
            'Thread.Sleep(1000)
        Next

        _bind.Rtf = Render()
        progress.Dispose()
    End Sub



End Class
