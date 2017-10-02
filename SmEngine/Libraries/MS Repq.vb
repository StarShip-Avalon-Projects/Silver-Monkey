Imports Furcadia.Net
Imports Furcadia.Net.Utils.ServerParser

Namespace Engine.Libraries

    ''' <summary>
    ''' Furcadia Popup Windows
    ''' <para>
    ''' TODO: Complete Class
    ''' </para>
    ''' </summary>
    Public Class MsRepQ
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)

        End Sub

#End Region

        ''' <summary>
        ''' Trade trigger handler
        ''' </summary>
        ''' <param name="InstructionObject">Server Instruction</param>
        ''' <param name="Args">Server Event Arguments</param>
        Private Shared Sub OnServerChannel(InstructionObject As ChannelObject, Args As ParseServerArgs) Handles FurcadiaSession.ProcessServerChannelData
            'Player = InstructionObject.Player
            'Dim Text = InstructionObject.ChannelText

            'Select Case Text
            '    Case "trade"
            '        MsPage.Execute(46, 47, 48)
            'End Select
        End Sub

        ''' <summary>
        ''' Server Instruction handler
        ''' </summary>
        ''' <param name="InstructionObject"></param>
        ''' <param name="Args"></param>
        Private Sub OnServerInstruction(InstructionObject As BaseServerInstruction, Args As ParseServerArgs) Handles FurcadiaSession.ProcessServerInstruction
            Player = FurcadiaSession.Player
        End Sub

    End Class

End Namespace