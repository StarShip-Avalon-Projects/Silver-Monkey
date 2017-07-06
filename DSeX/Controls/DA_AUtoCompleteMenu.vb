Imports System.Text.RegularExpressions
Imports FastColoredTextBoxNS

Namespace Controls

    Public Class DA_AUtoCompleteMenu
        Inherits AutocompleteItem

#Region "Public Fields"

        Public Shared RegexSpecSymbolsPattern As String = "[ \.\(\)]"

#End Region

#Region "Public Constructors"

        Public Sub New(snippet As String)
            MyBase.New(snippet)
        End Sub

#End Region

#Region "Public Methods"

        '[\ \^\$\\(\)\.\\\*\+\|\?]
        ''' <summary>
        ''' Compares fragment text with this item
        ''' </summary>
        Public Overrides Function Compare(fragmentText As String) As CompareResult
            fragmentText = fragmentText.Trim
            Dim pattern = Regex.Replace(fragmentText, RegexSpecSymbolsPattern, "$0").Trim
            If Regex.IsMatch(Text, pattern, RegexOptions.IgnoreCase) Then
                If Regex.IsMatch(Text, "\{" & fragmentText & "\}?", RegexOptions.IgnoreCase) Then
                    Return CompareResult.Hidden
                End If
                Return CompareResult.Visible
            End If
            Return CompareResult.Hidden
        End Function

#End Region

    End Class

End Namespace