Module HelperModule
    Public Function IsOdd(ByRef num As Integer) As Boolean
        Return num Mod 2 <> 0
    End Function

    Public Function IsEven(ByRef num As Integer) As Boolean
        Return num Mod 2 = 0
    End Function

    Public Function IsOdd(ByRef num As Double) As Boolean
        Return num Mod 2 <> 0
    End Function

    Public Function IsEven(ByRef num As Double) As Boolean
        Return num Mod 2 = 0
    End Function

    Public Function IsOdd(ByRef num As UInteger) As Boolean
        Return num Mod 2 <> 0
    End Function

    Public Function IsEven(ByRef num As UInteger) As Boolean
        Return num Mod 2 = 0
    End Function
End Module