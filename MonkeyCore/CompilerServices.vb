Module CompilerServices

    <Runtime.CompilerServices.Extension()>
    Public Function IsInteger(ByVal value As Object) As Boolean
        If String.IsNullOrEmpty(value.ToString) Then
            Return False
        Else
            Return Integer.TryParse(value.ToString, Nothing)
        End If
    End Function

    <Runtime.CompilerServices.Extension()>
    Public Function ToInteger(ByVal value As Type) As Integer
        If value.IsInteger() Then
            Return Integer.Parse(value.ToString)
        Else
            Return 0
        End If
    End Function

End Module