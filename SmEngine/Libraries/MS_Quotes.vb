'(5:210) - (5:220)

Imports MonkeyCore

Namespace Engine.Libraries

    ''' <summary>
    ''' Intended as a Quote list manager
    ''' <para>
    ''' Currently not used
    ''' </para>
    ''' </summary>
    Public Class MS_Quotes
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        Private writer As TextBoxWriter = Nothing

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)

            '(5: ) Use file {...} as quote list and put line # into Variable %.
            '(5: ) Use File {...} as quote list and put the total of lines into variable %
        End Sub

#End Region

    End Class

End Namespace