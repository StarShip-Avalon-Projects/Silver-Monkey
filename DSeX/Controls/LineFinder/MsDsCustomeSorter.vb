Imports System.Text.RegularExpressions

Namespace Controls.LineFinder

    ''' <summary>
    ''' ListView sorter for Monkey Speak and Dragon Speak
    ''' </summary>
    Class MsDsCustomeSorter
        Implements IComparer

#Region "Public Constructors"

        Sub New()

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' </summary>
        ''' <param name="x">
        ''' </param>
        ''' <param name="y">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = DirectCast(x, ListViewItem)
            Dim item2 As ListViewItem = DirectCast(y, ListViewItem)
            Dim cat As New Regex("\((.[0-9]*?)\:(.[0-9]*?)\)")
            Dim num1 As Integer = 0
            Dim num2 As Integer = 0
            Dim num3 As Integer = 0
            Dim num4 As Integer = 0

            Integer.TryParse(cat.Match(item1.Text).Groups(1).Value, num1)
            Integer.TryParse(cat.Match(item2.Text).Groups(1).Value, num2)
            Integer.TryParse(cat.Match(item1.Text).Groups(2).Value, num3)
            Integer.TryParse(cat.Match(item2.Text).Groups(2).Value, num4)

            If num3 > num4 Then
                If num1 > num2 Then Return 1
                If num1 < num2 Then Return -1
                Return 1
            ElseIf num3 < num4 Then
                If num1 > num2 Then Return 1
                If num1 < num2 Then Return -1
                Return -1
            Else
                If num1 > num2 Then Return 1
                If num1 < num2 Then Return -1
                Return 0
            End If

            Return 0

        End Function

#End Region

    End Class

End Namespace