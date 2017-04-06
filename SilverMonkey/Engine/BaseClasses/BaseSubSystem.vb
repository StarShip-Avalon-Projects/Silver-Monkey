Imports Furcadia.Net

''' <summary>
''' Base Class for Pluginable SubSystems
''' <para>These are Optional things to plug-in.. Things like a Phoenix Speak Parser</para>
''' </summary>
Public Class BaseSubSystem
    Inherits Utils.ParseServer



#Region "Public Constructors"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Dream"></param>
    ''' <param name="Player"></param>
    Public Sub New(ByRef Dream As DREAM, ByRef Player As FURRE)
        MyBase.New(Dream, Player)

    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Public Methods"
    ''' <summary>
    ''' Parse Serer Instructions
    ''' </summary>
    ''' <param name="data">Raw Server Instruction</param>
    ''' <param name="handled"> Handled</param>
    Public Overrides Sub ParseServerChannel(ByVal data As String, ByRef handled As Boolean)
        'parse data
        MyBase.ParseServerChannel(data, handled)
    End Sub

    ''' <summary>
    ''' Parse Server Channels
    ''' 
    ''' </summary>
    ''' <param name="data">Raw Server Instruction</param>
    ''' <param name="handled"></param>
    Public Overrides Sub ParseServerData(data As String, ByRef handled As Boolean)

        'Parse Data
        MyBase.ParseServerData(data, handled)
    End Sub

#End Region

End Class