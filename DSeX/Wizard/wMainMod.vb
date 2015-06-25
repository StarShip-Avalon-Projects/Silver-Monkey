Imports MonkeySpeakEditor.IniFile

Module wMainMod
    Public ScriptIni As IniFile = New IniFile

    Public ScriptPaths As List(Of String) = New List(Of String)
    Public ScriptPaths_MS As List(Of String) = New List(Of String)

    Public Function IsOdd(ByRef num As Integer) As Boolean
        Return num Mod 2 <> 0
    End Function

    Public Function IsEven(ByRef num As Integer) As Boolean
        Return num Mod 2 = 0
    End Function
End Module
