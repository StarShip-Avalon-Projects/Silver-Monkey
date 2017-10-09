Namespace Engine.Libraries

    ''' <summary>
    ''' Intended as a Quote list manager
    ''' <para>
    ''' Currently not used
    ''' </para>
    ''' </summary>
    Public NotInheritable Class MS_Quotes
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)

            '(5: ) Use file {...} as quote list and put line # into Variable %.
            '(5: ) Use File {...} as quote list and put the total of lines into variable %
        End Sub

#End Region

    End Class

End Namespace