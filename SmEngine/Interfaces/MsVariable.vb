Imports Monkeyspeak

Namespace Interfaces

    ''' <summary>
    ''' Silver Monkey's Custom MonkeySpeak Variable
    ''' </summary>
    Public Class MsVariable
        Implements IVariable

#Region "Public Constructors"

        ''' <summary>
        ''' Construct the Variable with Name and Value
        ''' </summary>
        ''' <param name="Name"><see cref="Monkeyspeak.Variable.Name"/></param>
        ''' <param name="value"><see cref="Monkeyspeak.Variable.Value"/></param>
        Public Sub New(Name As String, value As Object)

            Me.Name = Name
            Me.Value = value
        End Sub

        ''' <summary>
        ''' Construct the Variable with Name only
        ''' </summary>
        ''' <param name="Name"><see cref="Monkeyspeak.Variable.Name"/></param>
        Public Sub New(Name As String)
            Me.Name = Name
        End Sub

#End Region

#Region "Public Properties"

        Public Property Name As String Implements IVariable.Name
        Public Property Value As Object Implements IVariable.Value

        Public ReadOnly Property IsConstant As Boolean Implements IVariable.IsConstant
            Get
                Return False
            End Get
        End Property

#End Region

        Public Overloads Shared Operator <>(varA As MsVariable, varB As IVariable) As Boolean
            Return varA.Value IsNot varB.Value
        End Operator

        Public Overloads Shared Operator =(varA As MsVariable, varB As IVariable) As Boolean
            If varA Is Nothing OrElse varB Is Nothing Then
                If varA IsNot Nothing Then
                    Return varA.Value Is Nothing
                End If
                If varB IsNot Nothing Then
                    Return varB.Value Is Nothing
                End If
            End If
            Return varA.Value Is varB.Value
        End Operator

        Public Overrides Function GetHashCode() As Integer
            If TypeOf Me.Value Is Integer Then
                Return CInt(Me.Value) Xor Me.Name.GetHashCode()
            End If
            Return Me.Name.GetHashCode()
        End Function

        Private Function CheckType(_value As Object) As Boolean
            If _value Is Nothing Then
                Return True
            End If

            Return TypeOf _value Is String OrElse TypeOf _value Is Short OrElse TypeOf _value Is Double
        End Function

        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        Public Shadows Function Equals(other As IVariable) As Boolean Implements IEquatable(Of IVariable).Equals

            If TypeOf (other) Is IVariable Then
                Return Me = other
            End If
            Return False
        End Function

    End Class

End Namespace