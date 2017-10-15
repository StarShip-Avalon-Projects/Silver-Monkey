Imports Monkeyspeak

Public Class ConstantVariable
    Implements IVariable

    Sub New(ByVal var As Variable)
        Name = var.Name
        _value = var.Value
    End Sub

    Sub New(ByVal Name As String)
        Me._Name = Name
    End Sub

    Sub New(ByVal name As String, ByVal value As Object)
        Me._Name = name
        _value = value
    End Sub

    Public Property Name As String Implements IVariable.Name
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
        End Set
    End Property

    Private _value As Object
    Private _Name As String

    Public Sub ForceAssignValue(value As Object)
        Me._value = value
    End Sub

    Public Property Value As Object Implements IVariable.Value
        Get
            If _value = Nothing Then
                Return "null"
            End If
            Return _value
        End Get
        Set(value As Object)
            Throw New VariableIsConstantException($"Attempt to assign a value to constant '{Name}'")
        End Set
    End Property

    Public ReadOnly Property IsConstant As Boolean Implements IVariable.IsConstant
        Get
            Return True
        End Get
    End Property

    Public Shadows Function Equals(other As IVariable) As Boolean Implements IEquatable(Of IVariable).Equals
        Return Me.Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase) And Me.Value.Equals(other.Value)
    End Function

    'Private Function CheckType(_value As Object) As Boolean

    '    If (_value = Nothing) Then Return True

    '    Return _value.GetType() Is GetType(String) Or
    '               _value.GetType() Is GetType(Double)
    'End Function

    'Public Shared Widening Operator CType(ByVal d As Variable) As ConstantVariable
    '    Return New ConstantVariable(d)
    'End Operator

    'Public Shared Narrowing Operator CType(ByVal b As ConstantVariable) As Variable
    '    Return New Variable(b)
    'End Operator

End Class