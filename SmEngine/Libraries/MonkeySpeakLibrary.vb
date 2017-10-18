Imports System.Text.RegularExpressions
Imports System.Threading
Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Furcadia.Util
Imports Monkeyspeak
Imports Monkeyspeak.Libraries

Namespace Engine.Libraries

    ''' <summary>
    ''' The base library in which all Silver Monkey's Monkey Speak Libraries
    ''' are built on. This Library contains the commonly used functions for
    ''' all the other libraries
    ''' </summary>
    Public Class MonkeySpeakLibrary
        Inherits BaseLibrary

#Region "Protected Methods"

        ''' <summary>
        ''' <para>
        ''' Comparisons are done with Fucadia Markup Stripped
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True if the %MESSAGE system variable contains the specified string
        ''' </returns>
        Protected Overridable Function MsgContains(reader As TriggerReader) As Boolean

            Dim msMsg = StripHTML(reader.ReadString())
            Dim msg = reader.Page.GetVariable("%MESSAGE")

            Dim test = StripHTML(msg.Value.ToString)
            Return test.ToLower.Contains(msMsg.ToLower)

        End Function

        ''' <summary>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the System %MESSAGE varible ends with the specified string
        ''' </returns>
        Protected Overridable Function MsgEndsWith(reader As TriggerReader) As Boolean

            Dim msMsg = StripHTML(reader.ReadString())
            Dim msg = reader.Page.GetVariable("%MESSAGE")

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
        Protected Overridable Function MsgIs(reader As TriggerReader) As Boolean

            Dim safety = Not FurcadiaSession.IsConnectedCharacter
            Dim msMsg = StripHTML(reader.ReadString())
            Dim msg = reader.Page.GetVariable("%MESSAGE")

            Dim test = StripHTML(msg.Value.ToString)

            Return msMsg.ToLower.Equals(test.ToLower) And safety

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
        Protected Overridable Function MsgIsNot(reader As TriggerReader) As Boolean

            Dim safety = Not FurcadiaSession.IsConnectedCharacter
            Dim msMsg = StripHTML(reader.ReadString())
            Dim msg = reader.Page.GetVariable("%MESSAGE")

            Dim test = StripHTML(msg.Value.ToString)
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
        Protected Overridable Function MsgNotContain(reader As TriggerReader) As Boolean

            Dim msMsg = StripHTML(reader.ReadString())
            Dim msg = reader.Page.GetVariable("%MESSAGE")

            Dim test = StripHTML(msg.Value.ToString)
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
        Protected Function MsgNotEndsWith(reader As TriggerReader) As Boolean

            Dim msMsg = StripHTML(reader.ReadString())
            Dim msg = reader.Page.GetVariable("%MESSAGE")

            Dim test = StripHTML(msg.Value.ToString)
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
        Protected Function MsgNotStartsWith(reader As TriggerReader) As Boolean

            Dim msMsg = StripHTML(reader.ReadString())
            Dim msg = reader.Page.GetVariable("%MESSAGE")

            Dim test = StripHTML(msg.Value.ToString)
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
        Protected Function MsgStartsWith(reader As TriggerReader) As Boolean

            Dim msMsg = StripHTML(reader.ReadString())
            Dim msg = reader.Page.GetVariable("%MESSAGE")

            Dim test = StripHTML(msg.Value.ToString)
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

            Dim TmpName = reader.ReadString()
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

            Dim TmpName = reader.ReadString()
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
        Private Shared Function StripHTML(ByVal Text As String) As String

            Dim r = New Regex("<(.*?)>")
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
        ''' Current Dream the Bot is in
        ''' </summary>
        Public Property Dream As DREAM

        ''' <summary>
        ''' Current Triggering Furre
        ''' </summary>
        Public Property Player

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
        Public Function SendServer(ByRef message As String) As Boolean
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
            Player = Session.Player
            Dream = Session.Dream
            FurcadiaSession = Session
            FurcTimeTimer = New Timer(AddressOf TimeUpdate, Nothing,
                                      TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500))
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
        Public Shared Function ReadVariableOrNumber(ByVal reader As TriggerReader,
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

        ''' <summary>
        ''' Is the specified furre in the dream?
        ''' </summary>
        ''' <param name="Name">
        ''' Furre Name
        ''' </param>
        ''' <returns>
        ''' True if the furre is in the dream
        ''' </returns>
        Public Function InDream(ByRef Name As String) As Boolean
            Dim found As Boolean = False
            For Each Fur In Dream.FurreList
                If Fur.ShortName = FurcadiaShortName(Name) Then
                    found = True
                    Exit For
                End If
            Next
            Return found
        End Function

        Public Overrides Sub Unload(page As Page)

        End Sub

#End Region

    End Class

End Namespace