Imports System.Threading
Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Monkeyspeak

Namespace Engine.Libraries

    Public Class MonkeySpeakLibrary
        Inherits Monkeyspeak.Libraries.AbstractBaseLibrary

#Region "Public Properties"

        Public ReadOnly Property FurcTime As DateTime
            Get
                Return _FurcTime
            End Get
        End Property

#End Region

#Region "Private Methods"

        Private Sub TimeUpdate(obj As Object)
            _FurcTime = Now
        End Sub

#End Region

#Region "Private Fields"

        Private _FurcTime As DateTime
        Private _HasHelp As Boolean
        Private _SkillLevel As Integer
        Private FurcTimeTimer As Timer
        Private FuscariaSession As BotSession

#End Region

#Region "Public Fields"

        ''' <summary>
        ''' Current Dream the Bot is in
        ''' </summary>
        Public Dream As DREAM

        Public MsEngine As MonkeyspeakEngine

        Public MsPage As Page

        ''' <summary>
        ''' Current Triggering Furre
        ''' </summary>
        Public Player As FURRE

#End Region

#Region "Public Delegates"

        Public WithEvents FurcadiaSession As BotSession

        Public Delegate Sub MessageDelegate(ByRef message As String)

        'Public Sub LogError(reader As TriggerReader, ex As Exception)
        '    '   FurcadiaSession.LogError(reader, ex)

        'End Sub

        ''' <summary>
        ''' Send a raw instruction to the client
        ''' </summary>
        ''' <param name="message">
        ''' Message sring
        ''' </param>
        Public Sub SendClientMessage(ByRef message As String)
            FurcadiaSession.SendToClient(message)
        End Sub

        ''' <summary>
        ''' Send Formated Text to Server
        ''' </summary>
        ''' <param name="message">
        ''' Client to server instruction
        ''' </param>
        ''' <returns>
        ''' True is the Server is Connected
        ''' </returns>
        Public Function sendServer(ByRef message As String) As Boolean
            If FurcadiaSession.IsServerConnected Then
                FurcadiaSession.SendFormattedTextToServer(message)
                Return True
            Else
                Return False
            End If
        End Function

#End Region

#Region "Public Constructors"

        ''' <summary>
        ''' Default Constructor
        ''' <para>
        ''' Loads default Text Writer
        ''' </para>
        ''' </summary>
        Sub New(ByRef Session As BotSession)
            MyBase.New()
            If Session Is Nothing Then
                Exit Sub
            End If
            _SkillLevel = 0
            _HasHelp = False
            MsPage = Session.MSpage
            Player = Session.Player
            Dream = Session.Dream
            MsEngine = Session.MainEngine
            FurcadiaSession = Session
            FurcTimeTimer = New Timer(AddressOf TimeUpdate, Nothing, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500))
        End Sub

#End Region

#Region "Protected Methods"

        ''' <summary>
        ''' Registers a Trigger to the TriggerHandler with optional description
        ''' </summary>
        ''' <param name="trigger">
        ''' MonkeySpeak Trigger catagory
        ''' </param>
        ''' <param name="handler">
        ''' MonkeySpeak Handler
        ''' </param>
        ''' <param name="description">
        ''' Optional Description
        ''' <para>
        ''' Inherited from Base
        ''' </para>
        ''' </param>
        ''' <param name="HasHelp">
        ''' Is Help provided in the help file?
        ''' </param>
        ''' <param name="SkilLevel">
        ''' Skill levels 1-5
        ''' </param>
        Protected Overloads Sub Add(trigger As Trigger, handler As TriggerHandler, SkilLevel As Integer, HasHelp As Boolean, Optional description As String = Nothing)
            MyBase.Add(trigger, handler, description)
            _HasHelp = HasHelp
            _SkillLevel = _SkillLevel
        End Sub

        ''' <summary>
        ''' </summary>
        ''' <param name="TriggerIDs">
        ''' MonkeySpeak Triggers
        ''' </param>
        Protected Sub PageExecute(ParamArray TriggerIDs() As Integer)
            MsPage.Execute(TriggerIDs)
        End Sub

        ''' <summary>
        ''' </summary>
        ''' <param name="Message">
        ''' </param>
        ''' <param name="ServerData">
        ''' </param>
        Protected Sub PageSetVariable(Message As String, ServerData As String)
            MsPage.SetVariable(Message, ServerData, True)
        End Sub

#End Region

#Region "Common Library Methods"

        ''' <summary>
        ''' Reads a Double or a MonkeySpeak Variable
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <param name="addIfNotExist">
        ''' Add Variable to Variable Scope is it Does not exist
        ''' </param>
        ''' <returns>
        ''' <see cref="Double"/>
        ''' </returns>
        Public Function ReadVariableOrNumber(ByVal reader As TriggerReader, Optional addIfNotExist As Boolean = False) As Double
            Dim result As Double = 0
            If reader.PeekVariable Then
                Dim value As String = reader.ReadVariable(addIfNotExist).Value.ToString
                Double.TryParse(value, result)
            ElseIf reader.PeekNumber Then
                result = reader.ReadNumber
            End If
            Return result
        End Function

#End Region

    End Class

End Namespace