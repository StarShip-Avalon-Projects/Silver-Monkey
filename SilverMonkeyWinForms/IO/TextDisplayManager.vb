Imports System.Drawing
Imports System.Text
Imports System.Text.RegularExpressions
Imports Furcadia.Text.FurcadiaMarkup

Imports MonkeyCore.WinForms.Controls
Imports Settings

Namespace HelperClasses

    ''' <summary>
    '''
    ''' </summary>
    Public Class TextDisplayManager

#Region "Fields"

        ''' <summary>
        ''' RichTextBoxEx to work with
        ''' </summary>
        Private WithEvents LogDisplayBox As RichTextBoxEx

#End Region

#Region "Private Fields"

        Private Mainsettings As CMain

        Private Pos_Old As Integer = 0

#End Region

#Region "Public Constructors"

        ''' <summary>
        ''' Initializes a new instance of the <see cref="TextDisplayManager"/> class.
        ''' </summary>
        ''' <param name="settings">The settings.</param>
        ''' <param name="logBox">The log box.</param>
        Sub New(settings As CMain, ByRef logBox As RichTextBoxEx)
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
        Public Enum DisplayColors

            ''' <summary>
            ''' Errors
            ''' </summary>
            [Error] = -1

            ''' <summary>
            ''' Generic Warning
            ''' </summary>
            Warning

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

        ''' <summary>
        ''' Add paragraph text to Log Box
        ''' </summary>
        ''' <param name="TextObject"></param>
        Public Sub AddDataToList(TextObject As TextDisplayObject)
            If LogDisplayBox.InvokeRequired Then
                Dim dataArray() As TextDisplayObject = {TextObject}
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
                    'links = links & Regex.Matches(lb.Text,
                    '"<a.*?href='(.*?)'.*?>(.*?)</a>", RegexOptions.IgnoreCase)
                    For Each mmatch As System.Text.RegularExpressions.Match In links.Matches(LogDisplayBox.SelectedText)
                        Dim matchUrl As String = mmatch.Groups(1).Value
                        Dim matchText As String = mmatch.Groups(2).Value
                        If mmatch.Success Then
                            With LogDisplayBox

                                Dim Idx = .SelectedText.IndexOf(mmatch.Groups(0).Value)
                                Dim link As New Regex(UrlFilter, RegexOptions.IgnoreCase)
                                .SelectedText = link.Replace(.SelectedText, "", 1)

                                '  .Text.Replace(mmatch.Groups(0).Value, "")

                                .InsertLink(matchText, matchUrl, Idx)
                                '   .SelectionStart = Idx

                            End With
                        End If
                    Next

                    'lb.EndUpdate()
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
        Public Function FColor(Optional ByVal MyColor As DisplayColors = DisplayColors.DefaultColor) As System.Drawing.Color
            Dim thisColor = Mainsettings.DefaultColor
            Select Case MyColor
                Case DisplayColors.Emit
                    thisColor = Mainsettings.EmitColor
                Case DisplayColors.Say
                    thisColor = Mainsettings.SayColor
                Case DisplayColors.Shout
                    thisColor = Mainsettings.ShoutColor
                Case DisplayColors.Whisper
                    thisColor = Mainsettings.WhColor
                Case DisplayColors.Emote
                    thisColor = Mainsettings.EmoteColor
                Case DisplayColors.Error
                    thisColor = Mainsettings.ErrorColor
                Case DisplayColors.Warning
                    thisColor = Color.DarkOrange
            End Select
            Return thisColor
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
        Public Function FormatText(ByVal data As String, ByVal newColor As DisplayColors) As String

            ChannelTag(data, "[$1]")
            Dim RftData As New StringBuilder(System.Web.HttpUtility.HtmlDecode(data))
            'Dim Names As MatchCollection = NameRegex.Matches(data)
            'For Each Name As Match In Names
            '    buiRftDatald = RftData.Replace(Name.Value, Name.Groups(3).Value)
            'Next
            ''<name shortname='acuara' forced>
            'Dim MyIcon As MatchCollection = Regex.Matches(data, Iconfilter)

            RftData.Replace("|", " ")
            RftData.Replace("\", "\\")
            RftData.Replace("</b>", "\b0 ")
            RftData.Replace("<b>", "\b ")
            RftData.Replace("</i>", "\i0 ")
            RftData.Replace("<i>", "\i ")
            RftData.Replace("</ul>", "\ul0 ")
            RftData.Replace("<ul>", "\ul ")

            Dim myColor As Color = FColor(newColor)
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
            Private _TextColor As DisplayColors

#End Region

#Region "Public Constructors"

            Sub New(ByRef TextToDisplay As String, ByRef NewColor As DisplayColors)

                If Not TextToDisplay.EndsWith(Environment.NewLine) Then TextToDisplay += Environment.NewLine
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

            Public Property TextColor() As DisplayColors
                Get
                    Return _TextColor
                End Get
                Set(ByVal value As DisplayColors)
                    _TextColor = value
                End Set
            End Property

#End Region

        End Class

#End Region

    End Class

End Namespace