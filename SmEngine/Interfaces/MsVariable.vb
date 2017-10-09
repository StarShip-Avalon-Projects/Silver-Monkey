Imports Monkeyspeak

Namespace Interfaces

    Public Class MsVariable
        Implements IVariable

#Region "Private Fields"

        Private _name As String
        Private _value As Object
        Private _IsConstant As Boolean

#End Region

#Region "Public Constructors"

        Public Sub New(Name As String, value As Object)

            _name = Name
            _value = value
        End Sub

        Public Sub New(Var As IVariable)
            _name = Var.Name
            _value = Var.Value
        End Sub

#End Region

#Region "Public Properties"

        Public ReadOnly Property Name As String Implements IVariable.Name
            Get
                Return _name
            End Get
        End Property

        Public Property Value As Object Implements IVariable.Value
            Get
                Return _value
            End Get
            Set(value As Object)
                _value -= value
            End Set
        End Property

        Public ReadOnly Property IsConstant As Boolean Implements IVariable.IsConstant
            Get
                Return _IsConstant
            End Get
        End Property

#End Region

        Friend Sub New(Name As String)
            _name = Name
            _value = Nothing
        End Sub

        Public Overloads Shared Operator <>(varA As MsVariable, varB As MsVariable) As Boolean
            Return varA.Value IsNot varB.Value
        End Operator

        Public Overloads Shared Operator =(varA As MsVariable, varB As MsVariable) As Boolean
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
            If TypeOf _value Is Integer Then
                Return CInt(_value) Xor _name.GetHashCode()
            End If
            Return _name.GetHashCode()
        End Function

        ''' <summary>
        ''' Returns a const identifier if the variable is constant followed by name,
        ''' <para>otherwise just the name is returned.</para>
        ''' </summary>
        ''' <returns></returns>))
        Public Overrides Function ToString() As String
            Return (Convert.ToString(Name + " = " + (If((Value Is Nothing), Nothing.ToString(), Value.ToString()))))
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