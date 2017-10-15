Imports System.Runtime.Serialization
Imports Monkeyspeak


Namespace Engine.Libraries.PhoenixSpeak

    <Serializable>
    Public Class TypeNotSupportedException
        Inherits Exception

#Region "Public Constructors"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

#End Region

#Region "Protected Constructors"

        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
        End Sub

#End Region

    End Class

    <Serializable>
    <CLSCompliant(True)>
    Public Class Variable
        Implements IVariable

#Region "Private Fields"

        Private _value As Object
        Private _IsConstant As Boolean

#End Region



#Region "Public Constructors"


        Public Sub New(Name As String, value As Object)

            _name = Name
            _value = value
        End Sub

        Public Sub New(Var As PhoenixSpeak.Variable)
            _name = Var.Name
            _value = Var.Value
        End Sub

#End Region

#Region "Public Properties"





        Public Property Name As String Implements IVariable.Name

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

        Public Overloads Shared Operator <>(varA As Variable, varB As Variable) As Boolean
            Return varA.Value IsNot varB.Value
        End Operator

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

            If TypeOf (other) Is Variable Then
                Dim ob As Variable
                ob = DirectCast(other, Variable)
                Return Me = ob
            End If
            Return False
        End Function
    End Class

End Namespace