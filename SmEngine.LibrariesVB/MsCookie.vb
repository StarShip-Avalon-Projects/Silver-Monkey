Imports Monkeyspeak

''' <summary>
''' Furcadia Cookie Interface
''' <para>
''' <see href="http://furcadia.com/cookies/Cookie%20Economy.html"/>
''' </para>
''' <para>
''' <see href="http://www.furcadia.com/cookies/"/>
''' </para>
''' </summary>
Public NotInheritable Class MsCookie
    Inherits MonkeySpeakLibrary

#Region "Public Constructors"

    Public Overrides Sub Initialize(ParamArray args() As Object)
        MyBase.Initialize(args)
        '(0:42) When some one gives a cookie to the bot,
        Add(TriggerCategory.Cause, 42,
             Function(reader) ReadTriggeringFurreParams(reader),
              " When some one gives a cookie to the bot,")

        '(0:43) When a furre named {...} gives a cookie to the bot,
        Add(TriggerCategory.Cause, 43,
            AddressOf NameIs, " When a furre named {...} gives a cookie to the bot,")

        '(0:44) When anyone gives a cookie to someone the bot can see,
        Add(TriggerCategory.Cause, 44, Function(reader) ReadTriggeringFurreParams(reader),
            " When anyone gives a cookie to someone the bot can see,")

        '(0:49) When bot eats a cookie,
        Add(TriggerCategory.Cause, 49,
          Function(reader) ReadTriggeringFurreParams(reader),
            " When bot eats a cookie,")

        '(0:95) When the Bot sees ""You do not have any cookies to give away right now!",
        Add(TriggerCategory.Cause, 95,
            Function()
                Return True
            End Function,
            " When the Bot sees ""You do not have any cookies to give away right now!"",")

        '(0:46) When bot eats a cookie,
        Add(TriggerCategory.Cause, 96,
            Function()
                Return True
            End Function,
            " When the Bot sees ""Your cookies are ready."",")

        Add(TriggerCategory.Effect, 45,
             AddressOf EatCookie,
            " set variable %Variable to the cookie message the bot received.")
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
    Public Function EatCookie(reader) As Boolean

        Dim CookieVar = reader.ReadVariable(True)
        CookieVar.Value = Player.Message
        'add Machine Name parser
        Return True

    End Function

#End Region

End Class