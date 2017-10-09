Namespace Engine.Libraries

    ''' <summary>
    ''' Future support for Dream uploads
    ''' <para>
    ''' Currently not used
    ''' </para>
    ''' </summary>
    Public NotInheritable Class MS_DreamTransfer
        Inherits MonkeySpeakLibrary
        ' Ctrl+T and Editor.ini Dream Name+location
        'http://www.furcadia.com/download/dream.html

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)
            '(0:960) When the bot has finished uploading a dream,
            '(0:961) When the bot gets an error from the server about uploading a dream,
            '(0:962) When the bot gets an error from the server about an `fdl teleport attempt,

            '(5:960) Go to Acropolis.
            '(5:961) Go to Allegria Island.
            '(5:962) CTRL-T (dream upload) the current cued map.
            '(5:963) Set current cued map to {...} and CTRL-T (dream upload). (default "My Documents/Silver Monkey/Dreams")
            '(5:964) `fdl to my dream (already uploaded).
            '(5:965) `fdl to my dream, subtitled {...} (already uploaded).
            '(5:966) `fdl to the dream named {...} .
            '(5:967) unload owned dream on the current map.
            '(5:968) unload all dreams in bot's or Shared bot's dream.
            '(5:969) unload the dream {...} in Bot's Dream or if the bot has share.
        End Sub

#End Region

    End Class

End Namespace