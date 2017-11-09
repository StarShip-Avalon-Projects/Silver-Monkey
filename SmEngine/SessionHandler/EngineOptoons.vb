Imports System.IO

Namespace Engine

    ''' <summary>
    ''' MonkeySpeak Engine settings
    ''' </summary>
    Public Class EngineOptoons : Inherits Monkeyspeak.Options

#Region "Private Fields"

        Private _BotController As String
        Private _MS_Engine_Enable As Boolean
        Private _MsFileName As String

#End Region

#Region "Public Constructors"

        ''' <summary>
        ''' Default Constructor
        ''' </summary>
        Sub New()
            MyBase.New()
            _BotController = Nothing
            CanOverrideTriggerHandlers = False
            StringBeginSymbol = "{"c
            StringEndSymbol = "}"c
            VariableDeclarationSymbol = "%"c
            LineCommentSymbol = "*"c
            BlockCommentBeginSymbol = "/*"
            BlockCommentEndSymbol = "*/"
            TriggerLimit = 6000
            VariableCountLimit = 1000
            StringLengthLimit = Int32.MaxValue
            _MS_Engine_Enable = True
        End Sub

#End Region

#Region "Public Properties"

        Private _MonkeySpeakScriptFile As String

        Private _MonkeySpeakScriptVersion As Double

        ''' <summary>
        ''' Monkey Speak Script File Name
        ''' </summary>
        ''' <returns>
        ''' </returns>
        Public Property MonkeySpeakScriptFile() As String
            Get
                Return _MonkeySpeakScriptFile
            End Get
            Set(ByVal value As String)
                _MonkeySpeakScriptFile = value
            End Set
        End Property

        ''' <summary>
        ''' Current Monkey Speak Script Version the Bot Supports
        ''' </summary>
        ''' <returns>
        ''' </returns>
        Public Property MonkeySpeakScriptVersion() As Double
            Get
                Return _MonkeySpeakScriptVersion
            End Get
            Set(ByVal value As Double)
                _MonkeySpeakScriptVersion = value
            End Set
        End Property

#End Region

        Public Property BotController() As String
            Get
                Return _BotController
            End Get
            Set(ByVal value As String)
                _BotController = value
            End Set
        End Property

        ''' <summary>
        ''' Enable the Monkey Speak Engine
        ''' </summary>
        ''' <returns>
        ''' </returns>
        Public Property MS_Engine_Enable As Boolean
            Get
                Return _MS_Engine_Enable
            End Get
            Set(ByVal value As Boolean)
                _MS_Engine_Enable = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the m s_ file.
        ''' </summary>
        ''' <value>
        ''' The ms file name.
        ''' </value>
        Public Property MS_File As String
            Get
                Return _MsFileName
            End Get
            Set(value As String)
                If Not Path.GetExtension(value).ToLower = ".ms" Then
                    Throw New ArgumentException("Invalid File type, Not a ""*.ms"" file.")
                End If
                _MsFileName = value
            End Set
        End Property

    End Class

End Namespace