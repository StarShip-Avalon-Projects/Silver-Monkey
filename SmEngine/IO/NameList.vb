Imports System.IO

Namespace IO

    ''' <summary>
    ''' Class to read and write a list of names to a text file
    ''' <para>
    ''' Names are stored one name per live.
    ''' </para>
    ''' <para>
    ''' List is Smart enough to work with FurcadiaShortName format
    ''' </para>
    ''' </summary>
    Public Class NameList

#Region "Private Fields"

        Private _HasChanged As Boolean
        Private _PounceList As List(Of String)
        Private filename As String

#End Region

#Region "Public Constructors"

        ''' <summary>
        ''' Default constructor
        ''' </summary>
        Sub New()
            _PounceList = New List(Of String)
        End Sub

        ''' <summary>
        ''' Starts a new instance with a filename
        ''' </summary>
        ''' <param name="ListFile">
        ''' Filename
        ''' </param>
        Sub New(ListFile As String)
            _PounceList = New List(Of String)
            If Not String.IsNullOrEmpty(ListFile) Then

                filename = ListFile

                Try

                    If File.Exists(filename) Then
                        _PounceList.AddRange(File.ReadAllLines(filename))
                    Else
                        File.Create(filename)
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End If
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Has the namelist changed?
        ''' </summary>
        ''' <returns>
        ''' true if NameList has Changed
        ''' </returns>
        Public ReadOnly Property HaseChanged As Boolean
            Get
                Return _HasChanged
            End Get

        End Property

        ''' <summary>
        ''' Total number of names in the list
        ''' </summary>
        ''' <returns>
        ''' </returns>
        Public ReadOnly Property Count As Integer
            Get
                Return _PounceList.Count()
            End Get
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' adds name to the list if it does not exist
        ''' </summary>
        ''' <param name="Furre">
        ''' </param>
        Public Sub Add(Furre As String)
            If Not Contains(Furre) Then
                _HasChanged = True
                _PounceList.Add(Furre)
            End If
        End Sub

        ''' <summary>
        ''' Clears the Name List.
        ''' </summary>
        Public Sub Clear()
            _HasChanged = True
            _PounceList.Clear()
        End Sub

        ''' <summary>
        ''' Does the list contain a specified name?
        ''' </summary>
        ''' <param name="Furre">
        ''' Furre Name
        ''' </param>
        ''' <returns>
        ''' True if it does.
        ''' </returns>
        Public Function Contains(Furre As String) As Boolean

            For Each ListName In _PounceList
                If ListName.ToFurcadiaShortName() = Furre.ToFurcadiaShortName() Then Return True
            Next
            Return False
        End Function

        ''' <summary>
        ''' Reads list of names from the text file
        ''' </summary>
        ''' <param name="ListFile">
        ''' Text file containing the names (one name per line)
        ''' </param>
        Public Sub ReadList(ListFile As String)
            If Not _HasChanged Then
                filename = ListFile
                If File.Exists(ListFile) Then
                    filename = ListFile
                    _PounceList.Clear()
                    Try
                        _PounceList.AddRange(File.ReadAllLines(filename))
                    Catch ex As Exception
                        Throw ex
                    End Try
                Else
                    Throw New FileNotFoundException(String.Format("ListFile '{0}' does not exist!!!", filename))
                End If
                _HasChanged = False
            Else
                'throw exception?
            End If
        End Sub

        ''' <summary>
        ''' Removes a Name from the List
        ''' </summary>
        ''' <param name="Furre">
        ''' Furre Name
        ''' </param>
        Public Sub Remove(Furre As String)
            _HasChanged = True
            For Each ListName In _PounceList
                If ListName.ToFurcadiaShortName() = Furre.ToFurcadiaShortName() Then
                    _PounceList.Remove(ListName)
                End If
            Next

        End Sub

        ''' <summary>
        ''' Saves the Name list to the text file
        ''' </summary>
        Public Sub Save()
            If _HasChanged Then
                Try
                    File.WriteAllLines(filename, _PounceList)
                Catch Ex As Exception
                    Throw Ex
                End Try
                _HasChanged = False
            End If
        End Sub

        ''' <summary>
        ''' save the list to a new text file
        ''' </summary>
        ''' <param name="ListFile">
        ''' File Name
        ''' </param>
        Public Sub SaveAs(ListFile As String)

            If _HasChanged Then
                filename = ListFile
                Try
                    File.WriteAllLines(filename, _PounceList.ToArray())
                Catch Ex As Exception
                    Throw Ex
                Finally
                    _HasChanged = False
                End Try

            End If
        End Sub

        ''' <summary>
        ''' Converts the Name list to an Array of names
        ''' </summary>
        ''' <returns>
        ''' string Array of names
        ''' </returns>
        Public Function ToArray() As String()
            Return _PounceList.ToArray()
        End Function

        ''' <summary>
        ''' Converts the Name List to a string array
        ''' </summary>
        ''' <returns>
        ''' String array of names in Furcadia Short name format
        ''' </returns>
        Public Function ToShortNamedArray() As String()
            Dim ThisList() As String = Nothing
            _PounceList.CopyTo(ThisList)
            For I = 0 To ThisList.Length() - 1
                ThisList(I) = ThisList(I).ToFurcadiaShortName()
            Next
            Return ThisList
        End Function

#End Region

    End Class

End Namespace