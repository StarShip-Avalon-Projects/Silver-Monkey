Imports System.Text.RegularExpressions
Imports System.Threading
Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Furcadia.Util
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' The base library in which all Silver Monkey's Monkey Speak Libraries
    ''' are built on. This Library contains the commonly used functions for
    ''' all the other libraries
    ''' </summary>
    Public Class MonkeySpeakLibrary
        Inherits Monkeyspeak.Libraries.AbstractBaseLibrary

#Region "Protected Methods"

        ''' <summary>
        ''' <para>
        ''' Comparasons are done with Fucadia Markup Stripped
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True if the MESSAGE system variable contains the specified string
        ''' </returns>
        Protected Overridable Function msgContains(reader As TriggerReader) As Boolean

            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            Return test.Contains(msMsg)

        End Function

        ''' <summary>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the System MESSAGE varible ends with the specified string
        ''' </returns>
        Protected Overridable Function msgEndsWith(reader As TriggerReader) As Boolean

            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            'Debug.Print("Msg = " & msg)
            Return test.EndsWith(msMsg)

        End Function

        ''' <summary>
        ''' the Main Message is Comparason function
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Protected Overridable Function msgIs(reader As TriggerReader) As Boolean

            Dim safety As Boolean = Not FurcadiaSession.IsConnectedCharacter
            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            Dim test2 As Boolean = msMsg.Equals(test) And safety
            Return msMsg.Equals(test) And safety

        End Function

        ''' <summary>
        ''' Generic Message is Not Functions
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the last message seen is not the specified message
        ''' </returns>
        ''' <remarks>
        ''' Message Comparason is done with Markup stripped
        ''' <para>
        ''' The Bot ignores self messages
        ''' </para>
        ''' </remarks>
        Protected Overridable Function msgIsNot(reader As TriggerReader) As Boolean

            Dim safety As Boolean = Not FurcadiaSession.IsConnectedCharacter
            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            Return Not msMsg.Equals(test) And safety

        End Function

        ''' <summary>
        ''' Generic Message does not contain text function
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the last message seen does not contain the specified text
        ''' </returns>
        ''' <remarks>
        ''' Message Comparason is done with Markup stripped
        ''' <para>
        ''' The Bot ignores self messages
        ''' </para>
        ''' </remarks>
        Protected Overridable Function msgNotContain(reader As TriggerReader) As Boolean

            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            Return test.Contains(msMsg)

        End Function

        ''' <summary>
        ''' (1:14) and triggering furre's message doesn't end with {.},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Protected Function msgNotEndsWith(reader As TriggerReader) As Boolean

            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            'Debug.Print("Msg = " & msg)
            Return Not test.EndsWith(msMsg)

        End Function

        ''' <summary>
        ''' (1:12) and triggering furre's message doesn't start with {.},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Protected Function msgNotStartsWith(reader As TriggerReader) As Boolean

            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            Return Not test.StartsWith(msMsg)

        End Function

        ''' <summary>
        ''' (1:11) and triggering furre's message starts with {.},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Protected Function msgStartsWith(reader As TriggerReader) As Boolean

            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            Return test.StartsWith(msMsg)
        End Function

        ''' <summary>
        ''' Generic base Furre named {...} is Triggering Furre
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on Name match
        ''' </returns>
        ''' <remarks>
        ''' any name is acepted and converted to Furcadia Machine name
        ''' (ShortName version, lowercase with special characters stripped)
        ''' </remarks>
        Protected Overridable Function NameIs(reader As TriggerReader) As Boolean

            Dim TmpName As String = reader.ReadString()
            Return FurcadiaShortName(TmpName) = Player.ShortName

        End Function

        ''' <summary>
        ''' Generic base Furre named {...} is not the Triggering Furre
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on Name match
        ''' </returns>
        ''' <remarks>
        ''' any name is acepted and converted to Furcadia Machine name
        ''' (ShortName version, lowercase with special characters stripped)
        ''' </remarks>
        Protected Overridable Function NameIsNot(reader As TriggerReader) As Boolean

            Dim TmpName As String = reader.ReadString()
            Return FurcadiaShortName(TmpName) <> Player.ShortName
        End Function

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Strip Markup
        ''' </summary>
        ''' <param name="Text">
        ''' </param>
        ''' <returns>
        ''' Markup stripped string
        ''' </returns>
        Private Function StripHTML(ByVal Text As String) As String

            Dim r As New Regex("<(.*?)>")
            Text = r.Replace(Text, String.Empty)
            Return Text.Replace("|", " ").ToLower
        End Function

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Current Furcadia Standard Time (fst)
        ''' </summary>
        ''' <returns>
        ''' Furcadia Time Object in Furcadia Standard Time (fst)
        ''' </returns>
        Public ReadOnly Property FurcTime As DateTime
            Get
                Return _FurcTime
            End Get
        End Property

#End Region



#Region "Private Methods"

        ''' <summary>
        ''' Furcadia Clock updater
        ''' </summary>
        ''' <param name="obj">
        ''' Nothing
        ''' </param>
        Private Sub TimeUpdate(obj As Object)
            _FurcTime = Now
        End Sub

#End Region

#Region "Private Fields"

        Private _FurcTime As DateTime
        Private _HasHelp As Boolean
        Private _SkillLevel As Integer
        Private FurcTimeTimer As Timer

#End Region

#Region "Public Fields"

        ''' <summary>
        ''' Reference to the Main Bot Session for the bot
        ''' </summary>
        Public WithEvents FurcadiaSession As BotSession

        ''' <summary>
        ''' Reference to the Main Engine in the bot
        ''' </summary>
        Public WithEvents MsEngine As MonkeyspeakEngine

        ''' <summary>
        ''' Reference to the Main Monkey Speak Page for the bot
        ''' </summary>
        Public WithEvents MsPage As Page

        ''' <summary>
        ''' Current Dream the Bot is in
        ''' </summary>
        Public Dream As DREAM

        ''' <summary>
        ''' Current Triggering Furre
        ''' </summary>
        Public Player As FURRE

#End Region

#Region "Public Delegates"

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
        ''' References the main components cfrom <see cref="BotSession"/>
        ''' </para>
        ''' </summary>
        ''' <exception cref="ArgumentException">
        ''' Thrown when Session is not provided
        ''' </exception>
        Sub New(ByRef Session As BotSession)
            MyBase.New()
            If Session Is Nothing Then
                Throw New ArgumentException("Session cannot be null")
            End If
            _SkillLevel = 0
            _HasHelp = False
            MsPage = Session.MSpage
            Player = Session.Player
            Dream = Session.Dream
            MsEngine = Session.MainEngine
            FurcadiaSession = Session
            FurcTimeTimer = New Timer(AddressOf TimeUpdate, Nothing,
                                      TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500))
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
        Protected Overloads Sub Add(trigger As Trigger, handler As TriggerHandler,
                                    SkilLevel As Integer, HasHelp As Boolean,
                                    Optional description As String = Nothing)
            MyBase.Add(trigger, handler, description)
            _HasHelp = HasHelp
            _SkillLevel = _SkillLevel
        End Sub

        ''' <summary>
        ''' Execute array of Trigger cause integers from the Current
        ''' monkeyspeak page
        ''' </summary>
        ''' <param name="TriggerIDs">
        ''' MonkeySpeak Triggers
        ''' </param>
        Protected Sub PageExecute(ParamArray TriggerIDs() As Integer)
            MsPage.Execute(TriggerIDs)
        End Sub

        ''' <summary>
        ''' Set Bot assigned moneky speak variables as Constant
        ''' </summary>
        ''' <param name="Name">
        ''' <see cref="Monkeyspeak.Variable.Name"/>
        ''' </param>
        ''' <param name="Value">
        ''' <see cref="Monkeyspeak.Variable.Value"/>
        ''' </param>
        Protected Sub PageSetVariable(Name As String, Value As String)
            MsPage.SetVariable(Name.ToUpper, Value, True)
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
        ''' Add Variable to Variable Scope is it Does not exist,
        ''' <para>
        ''' Default Value is False
        ''' </para>
        ''' </param>
        ''' <returns>
        ''' <see cref="Double"/>
        ''' </returns>
        Public Function ReadVariableOrNumber(ByVal reader As TriggerReader,
                                             Optional addIfNotExist As Boolean = False) As Double
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