Public Class MiscUtils

    Public Shared Function Split(expression As String, delimiter As String, qualifier As String, ignoreCase As Boolean) As String()
        Dim _QualifierState As Boolean = False
        Dim _StartIndex As Integer = 0
        Dim _Values As New System.Collections.ArrayList()

        For _CharIndex As Integer = 0 To expression.Length - 2
            If (qualifier IsNot Nothing) And (String.Compare(expression.Substring(_CharIndex, qualifier.Length), qualifier, ignoreCase) = 0) Then
                _QualifierState = Not (_QualifierState)
            ElseIf Not (_QualifierState) And (delimiter IsNot Nothing) And (String.Compare(expression.Substring(_CharIndex, delimiter.Length), delimiter, ignoreCase) = 0) Then
                _Values.Add(expression.Substring(_StartIndex, _CharIndex - _StartIndex))
                _StartIndex = _CharIndex + 1
            End If
        Next

        If _StartIndex < expression.Length Then
            _Values.Add(expression.Substring(_StartIndex, expression.Length - _StartIndex))
        End If

        Dim _returnValues As String() = New String(_Values.Count - 1) {}
        _Values.CopyTo(_returnValues)
        Return _returnValues
    End Function

    Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
        Dim cnt As Integer = 0
        For Each c As Char In value
            If c = ch Then cnt += 1
        Next
        Return cnt
    End Function

End Class