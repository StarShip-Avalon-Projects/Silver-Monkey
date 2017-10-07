Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports Furcadia.Text.FurcadiaMarkup
Imports MonkeyCore
Imports MonkeyCore.Controls
Imports MonkeyCore.Controls.NativeMethods
Imports MonkeyCore.Settings
Imports MonkeyCore.Utils.Logging
Imports SilverMonkey.HelperClasses.NativeMethods

Namespace HelperClasses

    Public Class TextDisplayManager

#Region "Private Fields"

        ''' <summary>
        ''' RichTextBoxEx to work with
        ''' </summary>
        Public WithEvents LogDisplayBox As RichTextBoxEx

        Private Mainsettings As cMain

#End Region

#Region "Public Constructors"

        Sub New(settings As cMain, ByRef logBox As RichTextBoxEx)
            LogDisplayBox = logBox

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

        Private data As New System.Text.StringBuilder()

#Region "Public Methods"

        ''' <summary>
        ''' Add paragraph text to Log Box
        ''' </summary>
        ''' <param name="TextObject"></param>
        Public Sub AddDataToList(TextObject As TextDisplayObject)
            If LogDisplayBox.InvokeRequired Then
                Dim dataArray() As Object = {TextObject}
                LogDisplayBox.Invoke(New AddDataToListCaller(AddressOf AddDataToList), dataArray)
            Else
                Dim RtfReadonly As Boolean

                If LogDisplayBox.GetType().ToString.Contains("Controls.RichTextBoxEx") Then
                    'Pos_Old = GetScrollPos(lb.Handle, SBS_VERT)

                    'Dim myColor As System.Drawing.Color = fColor(newColor)
                    RtfReadonly = LogDisplayBox.ReadOnly
                    LogDisplayBox.ReadOnly = False
                    ' lb.BeginUpdate()

                    If LogDisplayBox.TextLength > 1 AndAlso LogDisplayBox.SelectionStart <> LogDisplayBox.TextLength - 1 Then
                        LogDisplayBox.SelectionStart = LogDisplayBox.TextLength - 1
                    End If
                    LogDisplayBox.BeginUpdate()
                    LogDisplayBox.SelectedRtf = FormatText(TextObject.Data, TextObject.TextColor)

                    ''since we Put the Data in the RTB now we Finish Setting the Links

                    Dim links As New Regex(UrlFilter, RegexOptions.IgnoreCase)
                    ' links = links & Regex.Matches(lb.Text,
                    ' "<a.*?href='(.*?)'.*?>(.*?)</a>", RegexOptions.IgnoreCase)
                    For Each mmatch As System.Text.RegularExpressions.Match In links.Matches(LogDisplayBox.Text)
                        Dim matchUrl As String = mmatch.Groups(1).Value
                        Dim matchText As String = mmatch.Groups(2).Value
                        If mmatch.Success Then
                            With LogDisplayBox

                                Dim Idx = .Text.IndexOf(mmatch.Groups(0).Value)
                                Dim link As New Regex(UrlFilter, RegexOptions.IgnoreCase)
                                .Text = link.Replace(.Text, "", 1)

                                '  .Text.Replace(mmatch.Groups(0).Value, "")

                                .InsertLink(matchText, matchUrl, Idx)
                                '   .SelectionStart = Idx

                            End With
                        End If
                    Next

                    ' lb.EndUpdate()
                    LogDisplayBox.ReadOnly = RtfReadonly
                    LogDisplayBox.EndUpdate()
                End If

            End If
        End Sub

        ''' <summary>
        ''' </summary>
        ''' <param name="MyColor">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function FColor(Optional ByVal MyColor As fColorEnum = fColorEnum.DefaultColor) As System.Drawing.Color
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
                    Case fColorEnum.Error
                        Return Mainsettings.ErrorColor
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

            SystemFshIcon(data, "")
            ChannelTag(data, "[$1]")
            Dim RftData As New StringBuilder(System.Web.HttpUtility.HtmlDecode(data))
            Dim Names As MatchCollection = Regex.Matches(data, NameFilter)
            For Each Name As System.Text.RegularExpressions.Match In Names
                RftData.Replace(Name.Value, Name.Groups(2).Value)
            Next
            '<name shortname='acuara' forced>
            Dim MyIcon As MatchCollection = Regex.Matches(data, Iconfilter)

            RftData.Replace("|", " ")
            RftData.Replace("\", "\\")
            RftData.Replace("</b>", "\b0 ")
            RftData.Replace("<b>", "\b ")
            RftData.Replace("</i>", "\i0 ")
            RftData.Replace("<i>", "\i ")
            RftData.Replace("</ul>", "\ul0 ")
            RftData.Replace("<ul>", "\ul ")

            Dim myColor As System.Drawing.Color = FColor(newColor)
            Dim ColorString As String = "{\colortbl ;"
            ColorString += "\red" & myColor.R & "\green" & myColor.G & "\blue" & myColor.B & ";}"
            Dim FontSize As Single = Mainsettings.ApFont.Size
            Dim FontFace As String = Mainsettings.ApFont.Name
            FontSize *= 2
            Return "{\rtf1\ansi\ansicpg1252\deff0\deflang1033" & ColorString & "{\fonttbl{\f0\fcharset0 " & FontFace & ";}}\viewkind4\uc1\fs" & FontSize.ToString & "\cf1 " & RftData.ToString & " \par}"
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

            Private _data As String
            Private _TextColor As fColorEnum

#End Region

#Region "Public Constructors"

            Sub New(ByRef TextToDisplay As String, ByRef NewColor As fColorEnum)
                _data = TextToDisplay
                _TextColor = NewColor
            End Sub

#End Region

#Region "Public Properties"

            Public Property Data() As String
                Get
                    Return _data
                End Get
                Set(ByVal value As String)
                    _data = value
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

        Private Sub log__LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles LogDisplayBox.LinkClicked
            'Dim Proto As String = ""
            'Dim Str As String = e.LinkText
            'Try
            '    If Str.Contains("#") Then
            '        Proto = Str.Substring(Str.IndexOf("#"), Str.IndexOf("://"))

            '    End If
            'Catch
            'End Try
            'Select Case Proto.ToLower
            '    Case "http"
            '        Try
            '            lb.Cursor = System.Windows.Forms.Cursors.AppStarting
            '            Dim url As String = Str.Substring(Str.IndexOf("#"))
            '            Process.Start(url)
            '        Catch ex As Exception
            '        Finally
            '            lb.Cursor = System.Windows.Forms.Cursors.Default
            '        End Try
            '    Case "https"
            '        Try
            '            lb.Cursor = System.Windows.Forms.Cursors.AppStarting
            '            Dim url As String = Str.Substring(Str.IndexOf("#"))
            '            Process.Start(url)
            '        Catch ex As Exception
            '        Finally
            '            lb.Cursor = System.Windows.Forms.Cursors.Default
            '        End Try

            '    Case Else
            '        MsgBox("Protocol: """ & Proto & """ Not yet implemented")
            'End Select
            'MsgBox(Proto)
        End Sub

#End Region

#Region "Log Scroll"

        Private Const SBS_HORZ As Integer = 0
        Private Const SBS_VERT As Integer = 1
        Dim Pos_Old As Integer = 0

        Private Sub ScrollToEnd()
            If LogDisplayBox.InvokeRequired Then

                Dim d As New Log_Scoll(AddressOf ScrollToEnd)
                d.Invoke()
            End If
            Dim scrollMin As Integer = 0
            Dim Sinfo As New SCROLLINFO
            Sinfo.CbSize = Marshal.SizeOf(Sinfo)
            Sinfo.fMask = ScrollInfoMask.SIF_POS
            Dim scrollMax As Integer = 0
            Dim test As Integer = GetScrollInfo(LogDisplayBox.Handle, SBOrientation.SB_VERT, Sinfo)

            If (GetScrollRange(LogDisplayBox.Handle, SBS_VERT, scrollMin, scrollMax)) Then
                Dim pos As Integer = GetScrollPos(LogDisplayBox.Handle, SBS_VERT)
                If scrollMax = Pos_Old Then
                    LogDisplayBox.SelectionStart = LogDisplayBox.Text.Length
                End If
                'Pos_Old = GetScrollPos(rtb.Handle, SBS_VERT)
                ' Detect if they're at the bottom
            End If
        End Sub

#End Region

    End Class

End Namespace