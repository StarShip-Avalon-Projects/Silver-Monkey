Imports Monkeyspeak

Namespace Engine.Libraries.Database

    ''' <summary>
    ''' Silver  Monkey implementation of <see cref="IVariable"/> for databases
    ''' </summary>
    <Serializable>
    <CLSCompliant(True)>
    Public Class DatabaseVariable
        Implements IVariable

#Region "Public Constructors"

        Public Sub New(Name As String, Optional value As Object = Nothing)

            _Name = Name
            _Value = value
        End Sub

        Public Sub New(Var As IVariable)
            _Name = Var.Name
            _Value = Var.Value
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

        Public Overloads Shared Operator <>(varA As DatabaseVariable, varB As IVariable) As Boolean
            Return varA.Value IsNot varB.Value
        End Operator

        Public Overloads Shared Operator =(varA As DatabaseVariable, varB As IVariable) As Boolean
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

    ''' <summary>
    '''
    ''' </summary>
    Public Class DatabaseVariableTable
        Implements IVariable, IDictionary(Of String, Object)

        Public Sub New(name As String, Optional limit As Integer = 100)
            MyBase.New(name, False, limit)

        End Sub

        Public Property Name As String Implements IVariable.Name
        Public Property Value As Object Implements IVariable.Value

        Public ReadOnly Property IsConstant As Boolean Implements IVariable.IsConstant
            Get
                Return False
            End Get
        End Property

        Default Public Property Item(key As String) As Object Implements IDictionary(Of String, Object).Item
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Object)
                Throw New NotImplementedException()
            End Set
        End Property

        Public ReadOnly Property Keys As ICollection(Of String) Implements IDictionary(Of String, Object).Keys
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property Values As ICollection(Of Object) Implements IDictionary(Of String, Object).Values
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of String, Object)).Count
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of String, Object)).IsReadOnly
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Sub Add(key As String, value As Object) Implements IDictionary(Of String, Object).Add
            Throw New NotImplementedException()
        End Sub

        Public Sub Add(item As KeyValuePair(Of String, Object)) Implements ICollection(Of KeyValuePair(Of String, Object)).Add
            Throw New NotImplementedException()
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of String, Object)).Clear
            Throw New NotImplementedException()
        End Sub

        Public Sub CopyTo(array() As KeyValuePair(Of String, Object), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of String, Object)).CopyTo
            Throw New NotImplementedException()
        End Sub

        ''' <summary>
        '''
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return $"{Name} = {(If((Values Is Nothing), Nothing.ToString(), Values.ToString()))}"
        End Function

        Public Function ContainsKey(key As String) As Boolean Implements IDictionary(Of String, Object).ContainsKey
            Throw New NotImplementedException()
        End Function

        Public Function Remove(key As String) As Boolean Implements IDictionary(Of String, Object).Remove
            Throw New NotImplementedException()
        End Function

        Public Function Remove(item As KeyValuePair(Of String, Object)) As Boolean Implements ICollection(Of KeyValuePair(Of String, Object)).Remove
            Throw New NotImplementedException()
        End Function

        Public Function TryGetValue(key As String, ByRef value As Object) As Boolean Implements IDictionary(Of String, Object).TryGetValue
            Throw New NotImplementedException()
        End Function

        Public Function Contains(item As KeyValuePair(Of String, Object)) As Boolean Implements ICollection(Of KeyValuePair(Of String, Object)).Contains
            Throw New NotImplementedException()
        End Function

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, Object)) Implements IEnumerable(Of KeyValuePair(Of String, Object)).GetEnumerator
            Throw New NotImplementedException()
        End Function

        Public Function Equals(other As IVariable) As Boolean Implements IEquatable(Of IVariable).Equals
            Throw New NotImplementedException()
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace