Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports Furcadia.Text.FurcadiaMarkup
Imports MonkeyCore
Imports MonkeyCore.Controls
Imports MonkeyCore.Settings

Public Class TextDisplayManager

#Region "Private Fields"

    Public WithEvents lb As RichTextBoxEx
    Private Mainsettings As cMain

#End Region

#Region "Public Constructors"

    Sub New(settings As cMain, ByRef logBox As RichTextBoxEx)
        lb = logBox

        Mainsettings = settings
    End Sub

#End Region

#Region "Private Delegates"

    Private Delegate Sub AddDataToListCaller(TextObject As TextDisplayObject)

    'UpDate Btn-Go Text and Actions Group Enable
    Private Delegate Sub Log_Scoll()

#End Region

#Region "Public Enums"

    ''' <summary>
    ''' Color enums for rtf display
    ''' </summary>
    Public Enum fColorEnum

        ''' <summary>
        ''' Errors
        ''' </summary>
        [Error] = -1

        ''' <summary>
        ''' Deneral Default color for everything
        ''' </summary>
        DefaultColor

        ''' <summary>
        ''' say channels... MySpeech
        ''' </summary>
        Say

        ''' <summary>
        ''' Shout Channel
        ''' </summary>
        Shout

        ''' <summary>
        ''' Whispers to the bot
        ''' </summary>
        Whisper

        ''' <summary>
        ''' Emote Channel
        ''' </summary>
        Emote

        ''' <summary>
        ''' Emit Channel
        ''' </summary>
        Emit

    End Enum

#End Region

#Region "Public Methods"

    Public Sub AddDataToList(TextObject As TextDisplayObject)
        If lb.InvokeRequired Then
            Dim dataArray() As Object = {TextObject}
            lb.Invoke(New AddDataToListCaller(AddressOf AddDataToList), dataArray)
        Else
            Dim RtfReadonly As Boolean

            If lb.GetType().ToString.Contains("Controls.RichTextBoxEx") Then
                'Pos_Old = GetScrollPos(lb.Handle, SBS_VERT)
                Dim build As New System.Text.StringBuilder(TextObject.DisplayText)
                build = build.Replace("</b>", "\b0 ")
                build = build.Replace("<b>", "\b ")
                build = build.Replace("</i>", "\i0 ")
                build = build.Replace("<i>", "\i ")
                build = build.Replace("</ul>", "\ul0 ")
                build = build.Replace("<ul>", "\ul ")

                build = build.Replace("&lt;", "<")
                build = build.Replace("&gt;", ">")

                Dim Names As MatchCollection = Regex.Matches(TextObject.DisplayText, NameFilter)
                For Each Name As System.Text.RegularExpressions.Match In Names
                    build = build.Replace(Name.Value, Name.Groups(3).Value)
                Next
                '<name shortname='acuara' forced>
                Dim MyIcon As MatchCollection = Regex.Matches(TextObject.DisplayText, Iconfilter)
                For Each Icon As System.Text.RegularExpressions.Match In MyIcon
                    Select Case Icon.Groups(1).Value
                        Case "91"
                            build = build.Replace(Icon.Value, "[#]")
                        Case Else
                            build = build.Replace(Icon.Value, "[" + Icon.Groups(1).Value + "]")
                    End Select

                Next

                'Dim myColor As System.Drawing.Color = fColor(newColor)
                RtfReadonly = lb.ReadOnly
                lb.ReadOnly = False
                ' lb.BeginUpdate()

                lb.SelectionStart = lb.TextLength
                lb.SelectedRtf = FormatText(build.ToString, TextObject.TextColor)

                'since we Put the Data in the RTB now we Finish Setting the Links
                Dim param() As String = {"<a.*?href=['""](.*?)['""].*?>(.*?)</a>", "<a.*?href=(.*?)>(.*?)</a>"}
                For i As Integer = 0 To param.Length - 1
                    Dim links As MatchCollection = Regex.Matches(lb.Text, param(i), RegexOptions.IgnoreCase)
                    ' links = links & Regex.Matches(lb.Text,
                    ' "<a.*?href='(.*?)'.*?>(.*?)</a>", RegexOptions.IgnoreCase)
                    For Each mmatch As System.Text.RegularExpressions.Match In links
                        Dim matchUrl As String = mmatch.Groups(1).Value
                        Dim matchText As String = mmatch.Groups(2).Value
                        If mmatch.Success Then
                            With lb
                                .BeginUpdate()
                                .Find(mmatch.Value)
                                'WAIT Snag Image Links first!
                                'Dim snag As Match = Regex.Match(matchText, "IMG:(.*?)::")
                                'If snag.Success Then
                                '    Dim RTFimg As New RTFBuilder
                                '    RTFimg.InsertImage(LoadImageFromUrl(snag.Groups(1).ToString))
                                '    .SelectedRtf = RTFimg.ToString
                                'Else
                                .SelectedRtf = FormatURL(matchText & "\v #" & matchUrl & "\v0 ")
                                .Find(matchText & "#" & matchUrl, RichTextBoxFinds.WholeWord)
                                .SetSelectionLink(True)
                                .EndUpdate()
                                'End If
                                'Put the Link in

                            End With
                        End If
                    Next
                Next

                Try
                    Dim SelStart As Integer = 0
                    While (lb.Lines.Length > 350)
                        'Array.Copy(lb.Lines, 1, lb.Lines, 0, lb.Lines.Length - 1)
                        SelStart = lb.SelectionStart
                        lb.SelectionStart = 0
                        lb.SelectionLength = lb.Text.IndexOf(vbLf, 0) + 1
                        lb.SelectedText = ""
                        lb.SelectionStart = SelStart
                    End While
                Catch
                    lb.Clear()
                    Console.WriteLine("Reset Log box due to over flow")
                End Try
                ' lb.EndUpdate()
                lb.ReadOnly = RtfReadonly

            End If

        End If
    End Sub

    ''' <summary>
    ''' </summary>
    ''' <param name="MyColor">
    ''' </param>
    ''' <returns>
    ''' </returns>
    Public Function fColor(Optional ByVal MyColor As fColorEnum = fColorEnum.DefaultColor) As System.Drawing.Color
        Try
            Select Case MyColor
                Case fColorEnum.DefaultColor
                    Return Mainsettings.DefaultColor
                Case fColorEnum.Emit
                    Return Mainsettings.EmitColor
                Case fColorEnum.Say
                    Return Mainsettings.SayColor
                Case fColorEnum.Shout
                    Return Mainsettings.ShoutColor
                Case fColorEnum.Whisper
                    Return Mainsettings.WhColor
                Case fColorEnum.Emote
                    Return Mainsettings.EmoteColor
                Case Else
                    Return Mainsettings.DefaultColor
            End Select
        Catch Ex As Exception
            Dim logError As New ErrorLogging(Ex, Me)
        End Try
        ' Return
    End Function

    ''' <summary>
    ''' Format text coming from Client and server
    ''' </summary>
    ''' <param name="data">
    ''' </param>
    ''' <param name="newColor">
    ''' </param>
    ''' <returns>
    ''' </returns>
    Public Function FormatText(ByVal data As String, ByVal newColor As fColorEnum) As String
        data = System.Web.HttpUtility.HtmlDecode(data)
        data = data.Replace("|", " ")

        Dim myColor As System.Drawing.Color = fColor(newColor)
        Dim ColorString As String = "{\colortbl ;"
        ColorString += "\red" & myColor.R & "\green" & myColor.G & "\blue" & myColor.B & ";}"
        Dim FontSize As Single = Mainsettings.ApFont.Size
        Dim FontFace As String = Mainsettings.ApFont.Name
        FontSize *= 2
        Return "{\rtf1\ansi\ansicpg1252\deff0\deflang1033" & ColorString & "{\fonttbl{\f0\fcharset0 " & FontFace & ";}}\viewkind4\uc1\fs" & FontSize.ToString & "\cf1 " & data & " \par}"
    End Function

    Public Function FormatURL(ByVal data As String) As String
        Dim FontSize As Single = Mainsettings.ApFont.Size
        Dim FontFace As String = Mainsettings.ApFont.Name
        FontSize *= 2
        Return "{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fcharset0 " & FontFace & ";}}\viewkind4\uc1\fs" & FontSize.ToString & " " & data & "}"
    End Function

#End Region

#Region "Public Classes"

    ''' <summary>
    ''' Object to Display Texxt with Color for Furcadia Markup
    ''' </summary>
    Public Class TextDisplayObject

#Region "Private Fields"

        Private _DisplayText As String
        Private _TextColor As fColorEnum

#End Region

#Region "Public Constructors"

        Sub New(ByRef TextToDisplay As String, ByRef NewColor As fColorEnum)
            _DisplayText = TextToDisplay
        End Sub

#End Region

#Region "Public Properties"

        Public Property DisplayText() As String
            Get
                Return _DisplayText
            End Get
            Set(ByVal value As String)
                _DisplayText = value
            End Set
        End Property

        Public Property TextColor() As fColorEnum
            Get
                Return _TextColor
            End Get
            Set(ByVal value As fColorEnum)
                _TextColor = value
            End Set
        End Property

#End Region

    End Class

#End Region

#Region "Private Methods"

    Private Sub log__LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles lb.LinkClicked
        Dim Proto As String = ""
        Dim Str As String = e.LinkText
        Try
            If Str.Contains("#") Then
                Proto = Str.Substring(InStr(Str, "#"))
                Proto = Proto.Substring(0, InStr(Proto, "://") - 1)
            End If
        Catch
        End Try
        Select Case Proto.ToLower
            Case "http"
                Try
                    lb.Cursor = System.Windows.Forms.Cursors.AppStarting
                    Dim url As String = Str.Substring(InStr(Str, "#"))
                    Process.Start(url)
                Catch ex As Exception
                Finally
                    lb.Cursor = System.Windows.Forms.Cursors.Default
                End Try
            Case "https"
                Try
                    lb.Cursor = System.Windows.Forms.Cursors.AppStarting
                    Dim url As String = Str.Substring(InStr(Str, "#"))
                    Process.Start(url)
                Catch ex As Exception
                Finally
                    lb.Cursor = System.Windows.Forms.Cursors.Default
                End Try

            Case Else
                MsgBox("Protocol: """ & Proto & """ Not yet implemented")
        End Select
        'MsgBox(Proto)
    End Sub

#End Region

#Region "Log Scroll"

    Private Const SBS_HORZ As Integer = 0
    Private Const SBS_VERT As Integer = 1
    Dim Pos_Old As Integer = 0

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

    Public Shared Function GetScrollInfo(hWnd As IntPtr,
            <MarshalAs(UnmanagedType.I4)> fnBar As SBOrientation,
            <MarshalAs(UnmanagedType.Struct)> ByRef lpsi As SCROLLINFO) As Integer
    End Function

    Public Declare Function GetScrollPos Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nBar As Integer) As Integer

    Public Declare Function GetScrollRange Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nBar As Integer,
            ByRef lpMinPos As Integer,
            ByRef lpMaxPos As Integer) As Boolean

    Private Sub ScrollToEnd()
        If lb.InvokeRequired Then

            Dim d As New Log_Scoll(AddressOf ScrollToEnd)
            d.Invoke()
        End If
        Dim scrollMin As Integer = 0
        Dim Sinfo As New SCROLLINFO
        Sinfo.cbSize = Marshal.SizeOf(Sinfo)
        Sinfo.fMask = ScrollInfoMask.SIF_POS
        Dim scrollMax As Integer = 0
        Dim test As Integer = GetScrollInfo(lb.Handle, SBOrientation.SB_VERT, Sinfo)

        If (GetScrollRange(lb.Handle, SBS_VERT, scrollMin, scrollMax)) Then
            Dim pos As Integer = GetScrollPos(lb.Handle, SBS_VERT)
            If scrollMax = Pos_Old Then
                lb.SelectionStart = lb.Text.Length
            End If
            'Pos_Old = GetScrollPos(rtb.Handle, SBS_VERT)
            ' Detect if they're at the bottom
        End If
    End Sub

    Structure SCROLLINFO

#Region "Public Fields"

        Public cbSize As Integer
        <MarshalAs(UnmanagedType.U4)> Public fMask As ScrollInfoMask
        Public nMax As Integer
        Public nMin As Integer
        Public nPage As UInteger
        Public nPos As Integer
        Public nTrackPos As Integer

#End Region

    End Structure

#End Region

End Class