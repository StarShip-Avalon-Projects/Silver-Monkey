Imports System.Text.RegularExpressions
Imports Furcadia.Net.Utils.ServerObjects
Imports SilverMonkeyEngine.MsLibHelper

Namespace Engine.Libraries.PhoenixSpeak

    ''' <summary>
    ''' Store a Phoenix Speak 'Get' response and send it to the MonkeySpeak
    ''' Library for PhonixSpeak Processing.
    ''' <para>
    ''' MonkeySpeak library grabs the PS Page and transforms it into a
    ''' PsInfo object. A PsInfo object is a list of PhoenixSpeak Variables
    ''' retrieved from the Furcadia Game-Server via the command line
    ''' interface.
    ''' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix Speak</see>
    ''' </para>
    ''' </summary>
    Public Class PhoenicSpeakDataObject
        Inherits DataObject

#Region "Private Fields"

        Private _CurrentPage As Integer
        Private _PageCount As Integer
        Private _PhoenixSpeakId As Short
        Private _PS_Page As String

#End Region

#Region "Public Properties"

        Public Property CurrentPage() As Integer
            Get
                Return _CurrentPage
            End Get
            Set(ByVal value As Integer)
                _CurrentPage = value
            End Set
        End Property

        Public ReadOnly Property MultiPage() As Boolean
            Get
                Return _PageCount = 6
            End Get

        End Property

        Public Property PhoenixSpeakID() As Short
            Get
                Return _PhoenixSpeakId
            End Get
            Set(ByVal value As Short)
                _PhoenixSpeakId = value
            End Set
        End Property

        Public ReadOnly Property PS_Page() As String
            Get
                Return _PS_Page
            End Get
        End Property

#End Region

#Region "Public Constructors"

        Public Sub New()

        End Sub

        Public Sub New(ByRef data As String)
            Dim PsPage As New Regex($"multi_result?(\ d +)?/(\d+)?", SmRegExOptions)
            Integer.TryParse(PsPage.Match(data, 0).Groups(1).Value(), _CurrentPage)
            Integer.TryParse(PsPage.Match(data, 0).Groups(2).Value(), _PageCount)
            _PS_Page = PsPage.Replace(data, "", 1)
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' returns number of Phoenix Speak pages remaining
        ''' </summary>
        Public ReadOnly Property PagesRemaining() As Integer
            Get
                Return _PageCount - _CurrentPage
                'Add "," to the end of match #1.
                'Input: "bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340,"
            End Get
        End Property

#End Region

    End Class

End Namespace