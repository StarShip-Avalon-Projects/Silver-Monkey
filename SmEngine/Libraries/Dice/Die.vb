Namespace Engine.Libraries.Dice

    ''' <summary>
    ''' Single Die object for Silver Monkey generating a Dice Roll
    ''' </summary>
    Public Class Die

#Region "Private Fields"

        Private Shared faceSelector As New Random

        Private _faceCount As Double
        Private _value As Double

#End Region

#Region "Public Constructors"

        Public Sub New(ByVal faceCount As Double)
            If faceCount < 1 Then
                Throw New ArgumentOutOfRangeException("faceCount", "Dice must have one or more faces.")
            End If

            _faceCount = faceCount
        End Sub

#End Region

#Region "Public Properties"

        Public Property FaceCount() As Double
            Get
                Return Me._faceCount
            End Get
            Set(ByVal value As Double)
                Me._faceCount = value
            End Set
        End Property

        Public ReadOnly Property Value() As Double
            Get
                Return Me._value
            End Get
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Roll a single die
        ''' </summary>
        ''' <returns>
        ''' <see cref="Double"/>
        ''' </returns>
        Public Function Roll() As Double
            Me._value = CDbl(faceSelector.Next(1, CInt(Me.FaceCount)))
            Return Me.Value
        End Function

#End Region

    End Class

End Namespace