Imports System.Collections.Generic

Imports Furcadia.Util

Public Class PounceList

#Region "Private Fields"

    Private _PounceList As New List(Of String)

#End Region

#Region "Public Constructors"

    Sub New()

    End Sub

    Sub New(PounceListFile As String)

    End Sub

#End Region

#Region "Public Methods"

    Public Sub Add(Furre As String)
        _PounceList.Add(Furre)
    End Sub

    Public Sub Clear()
        _PounceList.Clear()
    End Sub
    Public Sub Remove(Furre As String)
        _PounceList.Remove(Furre)
    End Sub

    Public Function ToArray() As String()
        Return _PounceList.ToArray()
    End Function

    Public Function ToFormatedArray() As String()
        Dim ThisList() As String = Nothing
        _PounceList.CopyTo(ThisList)
        For I As Integer = 0 To ThisList.Length() - 1
            ThisList(I) = FurcadiaShortName(ThisList(I))
        Next
        Return ThisList
    End Function

#End Region

#Region "Private Methods"

    Private Sub ReadList(File As String)

    End Sub

    Private Sub SaveList(File As String)

    End Sub

#End Region

End Class