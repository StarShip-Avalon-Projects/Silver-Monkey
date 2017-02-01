

Imports System.ComponentModel
Imports System.Drawing
Imports System.Net.WebRequestMethods
Imports FastColoredTextBoxNS
Imports Irony
Imports Irony.Parsing

Namespace Controls
    ''' <summary>
    ''' FastColoredTextBox with Irony parser support
    ''' </summary>
    ''' <see cref="https://www.codeproject.com/articles/161871/fast-colored-textbox-for-syntax-highlighting"/>
    ''' <see cref="https://github.com/PavelTorgashov/FastColoredTextBox"/>
    ''' <see cref="Http://irony.codeplex.com/"/>
    Public Class SilverMonkeyFCTB
        Inherits FastColoredTextBox
        Public Event StyleNeeded As EventHandler(Of StyleNeededEventArgs)

        Protected m_parser As Parser
        Public WavyStyle As Style = New WavyLineStyle(255, Color.Red)

        ''' <summary>
        ''' Grammar of custom language
        ''' </summary>
        <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)>
        Public Property Grammar() As Grammar
            Get
                If m_parser IsNot Nothing AndAlso m_parser.Language IsNot Nothing AndAlso m_parser.Language.Grammar IsNot Nothing Then
                    Return m_parser.Language.Grammar
                End If
                Return Nothing
            End Get
            Set
                SetParser(Value)
            End Set
        End Property

        ''' <summary>
        ''' Parser of custom language
        ''' </summary>
        <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)>
        Public Property Parser() As Parser
            Get
                Return m_parser
            End Get
            Set
                SetParser(Value)
            End Set
        End Property

        Public Sub New()
        End Sub

        ''' <summary>
        ''' Sets Irony parser (based on Grammar)
        ''' </summary>
        Public Overridable Sub SetParser(grammar As Grammar)
            SetParser(New LanguageData(grammar))
        End Sub

        ''' <summary>
        ''' Sets Irony parser (based on LanguageData)
        ''' </summary>
        Public Overridable Sub SetParser(language As LanguageData)
            SetParser(New Parser(language))
        End Sub

        ''' <summary>
        ''' Sets Irony parser
        ''' </summary>
        Public Overridable Sub SetParser(parser As Parser)
            Me.m_parser = parser
            ClearStylesBuffer()
            AddStyle(WavyStyle)
            SyntaxHighlighter.InitStyleSchema(Language.CSharp)
            InitBraces()
            OnTextChanged(Range)
        End Sub

        Public Overrides Sub OnTextChangedDelayed(changedRange As Range)
            DoHighlighting()
            MyBase.OnTextChangedDelayed(changedRange)
        End Sub

        Protected Overridable Sub DoHighlighting()
            If m_parser Is Nothing Then
                Return
            End If

            'parse text
            Dim tree As ParseTree

            Try
                tree = m_parser.Parse(Text)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                'oops...
                Return
            End Try

            'highlight errors
            If tree.Status = ParseTreeStatus.[Error] Then
                ClearStyle(GetStyleIndexMask(New Style() {WavyStyle}))
                For Each msg As LogMessage In tree.ParserMessages
                    Dim loc As SourceLocation = msg.Location
                    Dim place = New Place(loc.Column, loc.Line)
                    Dim r = New Range(Me, place, place)
                    Dim f = r.GetFragment("[\S]")
                    f.SetStyle(WavyStyle)
                Next
                Return
            End If

            'highlight syntax
            ClearStyle(StyleIndex.All)
            For Each t As Token In tree.Tokens
                Dim arg = New StyleNeededEventArgs(t)
                OnStyleNeeded(arg)

                If arg.Cancel Then
                    Continue For
                End If

                If arg.Style IsNot Nothing Then
                    GetTokenRange(t).SetStyle(arg.Style)
                    Continue For
                End If

                Select Case t.Terminal.[GetType]().Name
                    Case "KeyTerm"
                        If (t.Terminal.Flags And TermFlags.IsKeyword) <> 0 Then
                            'keywords are highlighted only
                            GetTokenRange(t).SetStyle(SyntaxHighlighter.KeywordStyle)
                        End If
                        Exit Select
                    Case "NumberLiteral"
                        GetTokenRange(t).SetStyle(SyntaxHighlighter.NumberStyle)
                        Exit Select
                    Case "StringLiteral"
                        GetTokenRange(t).SetStyle(SyntaxHighlighter.StringStyle)
                        Exit Select
                    Case "CommentTerminal"
                        GetTokenRange(t).SetStyle(SyntaxHighlighter.CommentStyle)
                        Exit Select
                End Select
            Next
        End Sub

        Public Overridable Sub OnStyleNeeded(e As StyleNeededEventArgs)
            RaiseEvent StyleNeeded(Me, e)
        End Sub

        ''' <summary>
        ''' Returns range of token
        ''' </summary>
        Public Function GetTokenRange(t As Token) As Range
            Dim loc = t.Location

            Dim place = New Place(loc.Column, loc.Line)
            Dim r = New Range(Me, place, place)

            For Each c As String In t.Text
                If c <> ControlChars.Cr Then
                    r.GoRight(True)
                End If
            Next

            Return r
        End Function

        Protected Overridable Sub InitBraces()
            LeftBracket = ControlChars.NullChar
            RightBracket = ControlChars.NullChar
            LeftBracket2 = ControlChars.NullChar
            RightBracket2 = ControlChars.NullChar

            ' select the first two pair of braces with the length of exactly one char (FCTB restrictions)
            Dim braces = m_parser.Language.Grammar.KeyTerms.[Select](Function(pair) pair.Value).Where(Function(term) term.Flags.IsSet(TermFlags.IsOpenBrace)).Where(Function(term) term.IsPairFor IsNot Nothing AndAlso TypeOf term.IsPairFor Is KeyTerm).Where(Function(term) term.Text.Length = 1).Where(Function(term) DirectCast(term.IsPairFor, KeyTerm).Text.Length = 1).Take(2)
            If braces.Any() Then
                ' first pair
                Dim brace = braces.First()
                LeftBracket = brace.Text.First()
                RightBracket = DirectCast(brace.IsPairFor, KeyTerm).Text.First()
                ' second pair
                If braces.Count() > 1 Then
                    brace = braces.Last()
                    LeftBracket2 = brace.Text.First()
                    RightBracket2 = DirectCast(brace.IsPairFor, KeyTerm).Text.First()
                End If
            Else
                LeftBracket = "("c
                RightBracket = ")"c
            End If
        End Sub
    End Class

    Public Class StyleNeededEventArgs
        Inherits EventArgs
        Public ReadOnly Token As Token
        Public Property Cancel() As Boolean
            Get
                Return m_Cancel
            End Get
            Set
                m_Cancel = Value
            End Set
        End Property
        Private m_Cancel As Boolean
        Public Property Style() As Style
            Get
                Return m_Style
            End Get
            Set
                m_Style = Value
            End Set
        End Property
        Private m_Style As Style

        Public Sub New(t As Token)
            Token = t
        End Sub
    End Class
End Namespace
