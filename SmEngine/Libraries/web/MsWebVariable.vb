Imports Monkeyspeak

Namespace Engine.Libraries.Web

    ''' <summary>
    ''' Silver  Monkey implementation of <see cref="IVariable"/> for web requests
    ''' </summary>
    <Serializable>
    <CLSCompliant(True)>
    Public Class MsWebVariable
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

        Public Overloads Shared Operator <>(varA As MsWebVariable, varB As IVariable) As Boolean
            Return varA.Value IsNot varB.Value
        End Operator

        Public Overloads Shared Operator =(varA As MsWebVariable, varB As IVariable) As Boolean
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
    Public Class MsWebVariableTable
        Inherits VariableTable
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

        ''' <summary>
        '''
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return $"{Name} = {(If((Values Is Nothing), Nothing.ToString(), Values.ToString()))}"
        End Function

    End Class

End Namespace