Imports Furcadia.Net.Dream

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
    Public NotInheritable Class MS_Cookie
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



        Public Overrides Sub Unload(page As Page)

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

            Dim tPlayer = Player
            Dim CookieVar = reader.ReadVariable(True)
            CookieVar.Value = tPlayer.Message
            'add Machine Name parser
            Return True

        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Return MyBase.Equals(obj)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return MyBase.GetHashCode()
        End Function

        Public Overrides Function ToString() As String
            Return MyBase.ToString()
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

        Protected Overrides Function MsgContains(reader As TriggerReader) As Boolean
            Return MyBase.MsgContains(reader)
        End Function

        Protected Overrides Function MsgEndsWith(reader As TriggerReader) As Boolean
            Return MyBase.MsgEndsWith(reader)
        End Function

        Protected Overrides Function MsgIs(reader As TriggerReader) As Boolean
            Return MyBase.MsgIs(reader)
        End Function

        Protected Overrides Function MsgIsNot(reader As TriggerReader) As Boolean
            Return MyBase.MsgIsNot(reader)
        End Function

        Protected Overrides Function MsgNotContain(reader As TriggerReader) As Boolean
            Return MyBase.MsgNotContain(reader)
        End Function

        Protected Overrides Function NameIsNot(reader As TriggerReader) As Boolean
            Return MyBase.NameIsNot(reader)
        End Function

#End Region



    End Class

End Namespace