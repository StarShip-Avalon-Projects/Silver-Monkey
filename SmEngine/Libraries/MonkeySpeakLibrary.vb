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
        ''' (0:17) When someone whispers something with {.} in it,
        ''' <para>
        ''' Comparasons are done with Fucadia Markup Stripped
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True is the MESSAGE system variable contains the specified string
        ''' </returns>
        Protected Function msgContains(reader As TriggerReader) As Boolean

            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            Return test.Contains(msMsg)

        End Function

        ''' <summary>
        ''' (1:13) and triggering furre's message ends with {.},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the System MESSAGE varible ends with the specified string
        ''' </returns>
        Protected Function msgEndsWith(reader As TriggerReader) As Boolean

            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            'Debug.Print("Msg = " & msg)
            Return test.EndsWith(msMsg)

        End Function

        ''' <summary>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Protected Function msgIs(reader As TriggerReader) As Boolean

            Dim safety As Boolean = Not FurcadiaSession.IsConnectedCharacter
            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            Dim test2 As Boolean = msMsg.Equals(test) And safety
            Return msMsg.Equals(test) And safety

        End Function

        ''' <summary>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Protected Function msgIsNot(reader As TriggerReader) As Boolean

            Dim safety As Boolean = Not FurcadiaSession.IsConnectedCharacter
            Dim msMsg As String = StripHTML(reader.ReadString())
            Dim msg As Variable = MsPage.GetVariable("MESSAGE")

            Dim test As String = StripHTML(msg.Value.ToString)
            Return Not msMsg.Equals(test) And safety

        End Function

        ''' <summary>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Protected Function msgNotContain(reader As TriggerReader) As Boolean

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
        ''' (0:25) When a furre Named {.} enters the Dream,
        ''' <para>
        ''' (0:27) When a furre named {.} leaves the Dream
        ''' </para>
        ''' (0:33) When a furre named {.} requests to summon the bot,"
        ''' <para>
        ''' (0:35) When a furre named {.} requests to join the bot,
        ''' </para>
        ''' <para>
        ''' (0:37) When a furre named {.} requests to follow the bot,
        ''' </para>
        ''' <para>
        ''' (0:39) When a furre named {.} requests to lead the bot,
        ''' </para>
        ''' <para>
        ''' (0:41) When a furre named {.} requests to cuddle with the bot,
        ''' </para>
        ''' <para>
        ''' (1:5) and the triggering furre's name is {.},
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on Name match
        ''' </returns>
        Protected Function NameIs(reader As TriggerReader) As Boolean

            Dim TmpName As String = reader.ReadString()
            Dim tname As Variable = MsPage.GetVariable("NAME")
            'add Machine Name parser
            Return FurcadiaShortName(TmpName) = FurcadiaShortName(tname.Value.ToString)

        End Function

        ''' <summary>
        ''' (1:6) and the triggering furre's name is not {.},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on Name Match
        ''' </returns>
        Protected Function NameIsNot(reader As TriggerReader) As Boolean

            Dim tname As String = MsPage.GetVariable("NAME").Value.ToString
            Dim TmpName As String = reader.ReadString()
            'add Machine Name parser
            If FurcadiaShortName(TmpName) <> FurcadiaShortName(tname) Then Return True

            Return False
        End Function

        ''' <summary>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Protected Function TrigFurreNameIs(reader As TriggerReader) As Boolean

            Dim TmpName As String = reader.ReadString()
            Dim TrigFurreName As String = FurcadiaSession.Player.ShortName
            'add Machine Name parser
            Return Furcadia.Util.FurcadiaShortName(TmpName) = TrigFurreName

        End Function

        ''' <summary>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Protected Function TrigFurreNameIsNot(reader As TriggerReader) As Boolean
            Return Not TrigFurreNameIs(reader)
        End Function

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' </summary>
        ''' <param name="Text">
        ''' </param>
        ''' <returns>
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
        ''' </returns>
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