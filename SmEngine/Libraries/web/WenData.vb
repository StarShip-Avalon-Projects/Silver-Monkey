﻿'
Imports Monkeyspeak

Namespace Engine.Libraries.Web

    ''' <summary>
    ''' web response page object
    ''' </summary>
    Public Class WebData

#Region "Public Fields"

        Private _WebStack As List(Of IVariable)
        Private _webPage As String
        Public Property ErrMsg As String
        Public Property Packet As String

        ''' <summary>
        ''' Web Cache to process
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property WebStack As List(Of IVariable)
            Get
                Return _WebStack
            End Get

        End Property

#End Region

#Region "Private Fields"

        Private _Status As Integer

#End Region

#Region "Public Constructors"

        Public Sub New()
            _WebStack = New List(Of IVariable)
            _Status = -1
            ReceivedPage = False
            ErrMsg = String.Empty
            Packet = String.Empty
        End Sub

        Sub New(WebCache As List(Of IVariable))
            Me.New()
            _WebStack = WebCache
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Raw text for the received web page
        ''' </summary>
        ''' <returns></returns>
        Public Property WebPage As String
            Get
                Return _webPage
            End Get
            Set(value As String)
                _webPage = value
            End Set
        End Property

        Public Property ReceivedPage As Boolean

        ''' <summary>
        ''' Web server status code
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        '''
        ''' </remarks>
        Public Property Status As Integer
            Get
                Return _Status
            End Get
            Set(value As Integer)
                _Status = value
            End Set
        End Property

#End Region

    End Class

End Namespace