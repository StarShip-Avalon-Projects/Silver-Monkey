Imports System.IO

Namespace Engine

    ''' <summary>
    ''' MonkeySpeak Engine settings
    ''' </summary>
    Public Class EngineOptoons : Inherits Monkeyspeak.Options

#Region "Private Fields"

        Private _BotController As String
        Private _MS_Engine_Enable As Boolean
        Private _MonkeySpeakScriptFile As String

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

        ''' <summary>
        ''' Gets or sets the monkey speak script file.
        ''' </summary>
        ''' <value>
        ''' The monkey speak script file.
        ''' </value>
        ''' <exception cref="ArgumentException">Invalid File type, Not a ""*.ms"" file.</exception>
        Public Property MonkeySpeakScriptFile() As String
            Get
                Return _MonkeySpeakScriptFile
            End Get
            Set(ByVal value As String)
                If Not Path.GetExtension(value).ToLower = ".ms" Then
                    Throw New ArgumentException("Invalid File type, Not a ""*.ms"" file.")
                End If
                _MonkeySpeakScriptFile = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the bot controller.
        ''' </summary>
        ''' <value>
        ''' The bot controller.
        ''' </value>
        Public Property BotController() As String
            Get
                Return _BotController
            End Get
            Set(ByVal value As String)
                _BotController = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the short name of the bot controller.
        ''' </summary>
        ''' <value>
        ''' The short name of the bot controller.
        ''' </value>
        Public ReadOnly Property BotControllerShortName As String
            Get
                Return _BotController.ToFurcadiaShortName
            End Get
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

#End Region

    End Class

End Namespace