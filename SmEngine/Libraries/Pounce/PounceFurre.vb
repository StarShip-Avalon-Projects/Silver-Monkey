Imports Furcadia.Util

Namespace Engine.Libraries.Pounce

    ''' <summary>
    ''' Pounce Furre object
    ''' </summary>
    Public Class MsPounceFurre
        Implements IEquatable(Of String)

#Region "Private Fields"

        Private _FurrName As String
        Private _IsOnline As Boolean
        Private _WasOnline As Boolean

#End Region

#Region "Public Constructors"

        ''' <summary>
        ''' Constructs the Furre with Furre Name
        ''' </summary>
        ''' <param name="Furre"></param>
        Public Sub New(Furre As String)
            _FurrName = Furre
        End Sub

#End Region

#Region "Public Events"

        ''' <summary>
        ''' Furre logged off the Game server
        ''' </summary>
        ''' <param name="Furre">
        ''' PounceFurre Object
        ''' </param>
        ''' <param name="e">
        ''' Event Arguments.Empty
        ''' </param>
        Public Event FurreLoggedOff(ByVal Furre As Object, e As EventArgs)

        ''' <summary>
        ''' Furre Logged onto the Gameserver
        ''' </summary>
        ''' <param name="Furre">
        ''' PounceFurre Object
        ''' </param>
        ''' <param name="e">
        ''' Event Arguments.Empty
        ''' </param>
        Public Event FurreLoggedOn(ByVal Furre As Object, e As EventArgs)

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Furre Name
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String
            Get
                Return _FurrName
            End Get
            Set(value As String)
                _FurrName = value
            End Set
        End Property

        ''' <summary>
        ''' Furre Online Status
        ''' </summary>
        ''' <returns></returns>
        Public Property Online As Boolean
            Get
                Return _IsOnline
            End Get
            Set(value As Boolean)
                _WasOnline = _IsOnline
                _IsOnline = value
                If _WasOnline = False And _WasOnline <> _IsOnline Then
                    RaiseEvent FurreLoggedOff(Me, EventArgs.Empty)
                ElseIf _IsOnline = False And _WasOnline <> _IsOnline Then
                    RaiseEvent FurreLoggedOn(Me, EventArgs.Empty)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Furre Name in short fomat
        ''' </summary>
        ''' <returns>character stripped name</returns>
        Public ReadOnly Property ShortName As String
            Get
                Return FurcadiaShortName(_FurrName)
            End Get
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Implement Equals
        ''' </summary>
        ''' <param name="other">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shadows Function Equals(other As String) As Boolean Implements IEquatable(Of String).Equals
            Return FurcadiaShortName(Me._FurrName) = FurcadiaShortName(other)
        End Function

#End Region

    End Class

End Namespace