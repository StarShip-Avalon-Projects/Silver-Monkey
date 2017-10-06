Imports System.Text.RegularExpressions
Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Furcadia.Net.Utils.ServerParser
Imports Furcadia.Text.FurcadiaMarkup
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Furcadia Cookie Interface
    ''' <para>
    ''' <see href="http://furcadia.com/cookies/Cookie%20Economy.html"/>
    ''' </para>
    ''' <para>
    ''' <see href="http://www.furcadia.com/cookies/"/>
    ''' </para>
    ''' </summary>
    Public Class MS_Cookie
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)

            '(0:42) When some one gives a cookie to the bot,
            Add(TriggerCategory.Cause, 42,
                 Function()
                     Return True
                 End Function,
                  "(0:42) When some one gives a cookie to the bot,")

            '(0:43) When a furre named {...} gives a cookie to the bot,
            Add(New Trigger(TriggerCategory.Cause, 43),
                AddressOf NameIs, "(0:43) When a furre named {...} gives a cookie to the bot,")

            '(0:44) When anyone gives a cookie to someone the bot can see,
            Add(TriggerCategory.Cause, 44,
            Function()
                Return True
            End Function,
                 "(0:44) When anyone gives a cookie to someone the bot can see,")

            '(0:49) When bot eats a cookie,
            Add(TriggerCategory.Cause, 49,
                Function()
                    Return True
                End Function,
                "(0:49) When bot eats a cookie,")

            '(0:95) When the Bot sees ""You do not have any cookies to give away right now!",
            Add(TriggerCategory.Cause, 95,
                Function()
                    Return True
                End Function,
                "(0:95) When the Bot sees ""You do not have any cookies to give away right now!"",")

            '(0:46) When bot eats a cookie,
            Add(TriggerCategory.Cause, 96,
                Function()
                    Return True
                End Function,
                "(0:96) When the Bot sees ""Your cookies are ready."",")

            Add(TriggerCategory.Effect, 45,
                 AddressOf EatCookie,
                "(5:45) set variable %Variable to the cookie message the bot received.")
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (5:45) set variable %Variable to the cookie message the bot received.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="triggerreader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function EatCookie(reader As TriggerReader) As Boolean

            Dim tPlayer As FURRE = Player
            Dim CookieVar As Variable = reader.ReadVariable(True)
            CookieVar.Value = tPlayer.Message
            'add Machine Name parser
            Return True

        End Function

        ''' <summary>
        ''' (0:43) When a furre named {...} gives a cookie to the bot,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Protected Overrides Function NameIs(reader As TriggerReader) As Boolean
            Return MyBase.NameIs(reader)
        End Function

#End Region

        Private Sub OnServerChannel(InstructionObject As ChannelObject, Args As ParseServerArgs) Handles FurcadiaSession.ProcessServerChannelData
            '   If FurcadiaSession.IsConnectedCharacter Then Exit Sub
            Player = InstructionObject.Player
            FurcadiaSession.MSpage.SetVariable("NAME", Player.Name, True)
            FurcadiaSession.MSpage.SetVariable("MESSAGE", Player.Message, True)

            Dim Text = InstructionObject.ChannelText
            Select Case InstructionObject.Channel
                Case "@cookie"
                    ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>
                    ' <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> All-time Cookie total: 0</font>
                    ' <font color='success'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Your cookies are ready.  http://furcadia.com/cookies/ for more info!</font>
                    '<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.

                    Dim CookieToMe As Regex = New Regex(String.Format("{0}", CookieToMeREGEX))
                    If CookieToMe.Match(Text).Success Then

                        FurcadiaSession.MSpage.Execute(42, 43)
                    End If
                    Dim CookieToAnyone As Regex = New Regex(String.Format("<name shortname='(.*?)'>(.*?)</name> just gave <name shortname='(.*?)'>(.*?)</name> a (.*?)"))
                    If CookieToAnyone.Match(Text).Success Then

                        If FurcadiaSession.IsConnectedCharacter Then
                            FurcadiaSession.MSpage.Execute(42, 43)
                        Else
                            FurcadiaSession.MSpage.Execute(44)
                        End If

                    End If
                    Dim CookieFail As Regex = New Regex(String.Format("You do not have any (.*?) left!"))
                    If CookieFail.Match(Text).Success Then
                        FurcadiaSession.MSpage.Execute(45)
                    End If
                    Dim EatCookie As Regex = New Regex(Regex.Escape("<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.") + "(.*?)")
                    If EatCookie.Match(Text).Success Then
                        'TODO Cookie eat message can change by Dragon Speak

                        FurcadiaSession.MSpage.Execute(49)

                    End If
                    '(0:96) When the Bot sees "Your cookies are ready."
                    Dim CookiesReady As Regex = New Regex(<a>"Your cookies are ready.  http://furcadia.com/cookies/ for more info!"</a>)
                    If CookiesReady.Match(Text).Success Then
                        FurcadiaSession.MSpage.Execute(96)
                    End If

            End Select
        End Sub

    End Class

End Namespace