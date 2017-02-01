Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Windows.Forms

Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles
Imports MonkeyCore.Controls.Win32

Namespace Controls
    Public Class RichTextBoxEx
        Inherits System.Windows.Forms.RichTextBox

        'Declaration
        <BindableAttribute(True)>
        Public Property VerticalContentAlignment As VerticalAlignment
        Private _protocols As List(Of String)

        Dim instance As Control
        Dim value As VerticalAlignment


        Public Sub New()
            MyBase.New()
            Me.DoubleBuffered = True
            ' Otherwise, non-standard links get lost When user starts typing
            ' next to a non-standard link
            Me.DetectUrls = True
            Me._protocols = New List(Of String)
            Me._protocols.AddRange(New String() {"http://", "furc://", "file://", "mailto://", "ftp://", "https://", "gopher://", "nntp://", "prospero://", "telnet://", "news://", "wais://", "outlook://", "\\"})


        End Sub

        ''' <summary>
        ''' Gets and Sets the Horizontal Scroll position of the control.
        ''' </summary>
        Public Property HScrollPos() As Integer
            Get
                Return GetScrollPos(Me.Handle, SB_HORZ)
            End Get
            Set(ByVal value As Integer)
                SetScrollPos(Me.Handle, SB_HORZ, value, True)
            End Set
        End Property

        ''' <summary>
        ''' Gets and Sets the Vertical Scroll position of the control.
        ''' </summary>
        Public Property VScrollPos() As Integer
            Get
                Return GetScrollPos(Me.Handle, SB_VERT)
            End Get
            Set(ByVal value As Integer)
                SetScrollPos(Me.Handle, SB_VERT, value, True)
            End Set
        End Property

        <Editor(("System.Windows.Forms.Design.StringCollectionEditor," _
           + "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
           GetType(System.Drawing.Design.UITypeEditor))>
        Public ReadOnly Property Protocols() As List(Of String)
            Get

                Return Me._protocols
            End Get
        End Property

        Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
            BeginUpdate()
            For Each p As String In _protocols
                Dim matches As MatchCollection = Regex.Matches(Me.Text, p & "(.*?)\s", RegexOptions.IgnoreCase)
                For Each m As Match In matches
                    If m.Success Then
                        Me.Select(m.Index, m.Length - 1)
                        Me.SetSelectionStyle(CFM_LINK, CFE_LINK)
                    End If
                Next
            Next
            EndUpdate()
        End Sub

        <DefaultValue(False)> _
        Public Shadows Property DetectUrls() As Boolean
            Get
                Return MyBase.DetectUrls
            End Get
            Set(ByVal value As Boolean)
                MyBase.DetectUrls = value
            End Set
        End Property

        ''' <summary>
        ''' Insert a given text as a link into the RichTextBox at the current insert position.
        ''' </summary>
        ''' <param name="text">Text to be inserted</param>
        Public Sub InsertLink(ByVal text As String)
            InsertLink(text, Me.SelectionStart)
        End Sub

        ''' <summary>
        ''' Insert a given text at a given position as a link. 
        ''' </summary>
        ''' <param name="text">Text to be inserted</param>
        ''' <param name="position">Insert position</param>
        Public Sub InsertLink(ByVal text As String, ByVal position As Integer)
            If position < 0 OrElse position > Me.Text.Length Then
                Throw New ArgumentOutOfRangeException("position")
            End If

            Me.SelectionStart = position
            Me.SelectedText = text
            Me.[Select](position, text.Length)
            Me.SetSelectionLink(True)
            Me.[Select](position + text.Length, 0)
        End Sub

        ''' <summary>
        ''' Insert a given text at at the current input position as a link.
        ''' The link text is followed by a hash (#) and the given hyperlink text, both of
        ''' them invisible.
        ''' When clicked on, the whole link text and hyperlink string are given in the
        ''' LinkClickedEventArgs.
        ''' </summary>
        ''' <param name="text">Text to be inserted</param>
        ''' <param name="hyperlink">Invisible hyperlink string to be inserted</param>
        Public Sub InsertLink(ByVal text As String, ByVal hyperlink As String)
            InsertLink(text, hyperlink, Me.SelectionStart)
        End Sub

        ''' <summary>
        ''' Insert a given text at a given position as a link. The link text is followed by
        ''' a hash (#) and the given hyperlink text, both of them invisible.
        ''' When clicked on, the whole link text and hyperlink string are given in the
        ''' LinkClickedEventArgs.
        ''' </summary>
        ''' <param name="text">Text to be inserted</param>
        ''' <param name="hyperlink">Invisible hyperlink string to be inserted</param>
        ''' <param name="position">Insert position</param>
        Public Sub InsertLink(ByVal text As String, ByVal hyperlink As String, ByVal position As Integer)
            If position < 0 OrElse position > Me.Text.Length Then
                Throw New ArgumentOutOfRangeException("position")
            End If
            BeginUpdate()
            Me.SelectionStart = position
            Me.SelectedRtf = "{\rtf1\ansi\ansicpg1252\deff0\deflang1033 " & text & "\v #" & hyperlink & "\v0}"
            Me.[Select](position, text.Length + 1 + hyperlink.Length)
            Me.SetSelectionLink(True)
            Me.[Select](position + text.Length + 1 + hyperlink.Length, 0)
            EndUpdate()
        End Sub

        ''' <summary>
        ''' Set the current selection's link style
        ''' </summary>
        ''' <param name="link">true: set link style, false: clear link style</param>
        Public Sub SetSelectionLink(ByVal link As Boolean)
            SetSelectionStyle(CFM_LINK, If(link, CFE_LINK, 0))
        End Sub
        ''' <summary>
        ''' Get the link style for the current selection
        ''' </summary>
        ''' <returns>0: link style not set, 1: link style set, -1: mixed</returns>
        Public Function GetSelectionLink() As Integer
            Return GetSelectionStyle(CFM_LINK, CFE_LINK)
        End Function

        Private Sub SetSelectionStyle(ByVal mask As Integer, ByVal effect As Integer)
            Dim cf As New CHARFORMAT2_STRUCT()
            cf.cbSize = CType(Marshal.SizeOf(cf), Integer)
            cf.dwMask = mask
            cf.dwEffects = effect

            Dim wpar As New IntPtr(SCF_SELECTION)
            Dim lpar As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf))
            Marshal.StructureToPtr(cf, lpar, False)

            Dim res As IntPtr = SendMessage(Handle, EM_SETCHARFORMAT, wpar, lpar)

            Marshal.FreeCoTaskMem(lpar)
        End Sub

        Private Function GetSelectionStyle(ByVal mask As Integer, ByVal effect As Integer) As Integer
            Dim cf As New CHARFORMAT2_STRUCT()
            cf.cbSize = CType(Marshal.SizeOf(cf), Integer)
            cf.szFaceName = New Char(31) {}

            Dim wpar As New IntPtr(SCF_SELECTION)
            Dim lpar As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf))
            Marshal.StructureToPtr(cf, lpar, False)

            Dim res As IntPtr = SendMessage(Handle, EM_GETCHARFORMAT, wpar, lpar)

            cf = CType(Marshal.PtrToStructure(lpar, GetType(CHARFORMAT2_STRUCT)), CHARFORMAT2_STRUCT)

            Dim state As Integer
            ' dwMask holds the information which properties are consistent throughout the selection:
            If (cf.dwMask And mask) = mask Then
                If (cf.dwEffects And effect) = effect Then
                    state = 1
                Else
                    state = 0
                End If
            Else
                state = -1
            End If

            Marshal.FreeCoTaskMem(lpar)
            Return state
        End Function

        'Public Sub urlClicked(ByVal sender As Object, ByVal e As LinkClickedEventArgs) Handles Me.LinkClicked
        '    MessageBox.Show(e.LinkText)
        'End Sub

        ''' <summary>
        ''' Maintains performance while updating.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' It is recommended to call this method before doing
        ''' any major updates that you do not wish the user to
        ''' see. Remember to call EndUpdate When you are finished
        ''' with the update. Nested calls are supported.
        ''' </para>
        ''' <para>
        ''' Calling this method will prevent redrawing. It will
        ''' also setup the event mask of the underlying richedit
        ''' control so that no events are sent.
        ''' </para>
        ''' </remarks>
        Public Sub BeginUpdate()
            ' Deal with nested calls.
            updating += 1

            If updating > 1 Then
                Return
            End If

            ' Prevent the control from raising any events.
            oldEventMask = SendMessage(Me.Handle, EM_SETEVENTMASK, IntPtr.Zero, IntPtr.Zero)

            ' Prevent the control from redrawing itself.
            SendMessage(Me.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero)
        End Sub

        ''' <summary>
        ''' Resumes drawing and event handling.
        ''' </summary>
        ''' <remarks>
        ''' This method should be called every time a call is made
        ''' made to BeginUpdate. It resets the event mask to it's
        ''' original value and enables redrawing of the control.
        ''' </remarks>
        Public Sub EndUpdate()
            ' Deal with nested calls.
            updating -= 1

            If updating > 0 Then
                Return
            End If

            ' Allow the control to redraw itself.
            SendMessage(Me.Handle, WM_SETREDRAW, New IntPtr(1), IntPtr.Zero)

            ' Allow the control to raise event messages.
            SendMessage(Me.Handle, EM_SETEVENTMASK, IntPtr.Zero, oldEventMask)
        End Sub








    End Class
End Namespace
