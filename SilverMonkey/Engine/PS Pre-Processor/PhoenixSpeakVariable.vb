Imports System.Runtime.Serialization

<Assembly: CLSCompliant(True)>
Namespace PhoenixSpeak
    <Serializable>
    Public Class TypeNotSupportedException
        Inherits Exception

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

    <Serializable>
    <CLSCompliant(True)>
    Public Class Variable
        Inherits Monkeyspeak.Variable

        Private _value As Object
        Private _name As String
        Public Overloads Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        Public Overloads Property Value() As Object
            Get
                Return _value
            End Get
            Set
                If CheckType(_value) = False Then
                    Throw New TypeNotSupportedException(_value.[GetType]().Name + " is not a supported type. Expecting string, short, or double.")
                End If

            End Set
        End Property



        Friend Sub New(Name As String)
            _name = Name
            _value = Nothing
        End Sub

        Public Sub New(Var As PhoenixSpeak.Variable)
            _name = Var.Name
            _value = Var.Value
        End Sub

        Friend Sub New(Name As String, value As Object)
            _name = Name
            _value = value
        End Sub

        Private Function CheckType(_value As Object) As Boolean
            If _value Is Nothing Then
                Return True
            End If

            Return TypeOf _value Is String OrElse TypeOf _value Is Short OrElse TypeOf _value Is Double
        End Function

        ''' <summary>
        ''' Returns a const identifier if the variable is constant followed by name,
        ''' <para>otherwise just the name is returned.</para>
        ''' </summary>
        ''' <returns></returns>))
        Public Overrides Function ToString() As String
            Return (Convert.ToString(Name + " = " + (If((Value Is Nothing), Nothing.ToString(), Value.ToString()))))
        End Function



        Public Overloads Shared Operator =(varA As Variable, varB As Variable) As Boolean
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

        Public Overloads Shared Operator <>(varA As Variable, varB As Variable) As Boolean
            Return varA.Value IsNot varB.Value
        End Operator

        Public Overrides Function Equals(obj As Object) As Boolean

            If TypeOf (obj) Is String Then
                Dim ob As String = Nothing
                ob = DirectCast(obj, String)
                Return Name = ob
            End If
            If TypeOf (obj) Is Variable Then
                Dim ob As Variable
                ob = DirectCast(obj, Variable)
                Return Me = ob
            End If
            Return False
        End Function
        Public Overrides Function GetHashCode() As Integer
            If TypeOf _value Is Integer Then
                Return CInt(_value) Xor _name.GetHashCode()
            End If
            Return _name.GetHashCode()
        End Function


    End Class
End Namespace