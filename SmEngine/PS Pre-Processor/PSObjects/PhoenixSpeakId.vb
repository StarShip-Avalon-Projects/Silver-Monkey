Namespace Engine.Libraries.PhoenixSpeak

    ''' <summary>
    ''' Phoenix Speak ID manager
    '''<see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix Speak</see>
    ''' </summary>
    <CLSCompliant(True)>
    Public Class PsId

#Region "Private Fields"

        Private Shared idLock As New Object

        'Recycle ids as they're needed
        'on Receiving Server Data remove Id
        'If theres more then one set to an ID Count Down first

        'On sending a Command to the Server Set Available IDs for the out going Enqueue
        'Use same ID with counter for Multiple Commands sent
        'IE Restoring multi-page character data

        Private Shared List As New Dictionary(Of Short, PsId)

        Private _Count As Integer
        Private _id As Short

#End Region

#Region "Public Constructors"

        ''' <summary>
        ''' </summary>
        Sub New()
            _id = getID(0)
            _Count = 1
            If Not List.ContainsKey(_id) Then
                List.Add(_id, Me)
            End If
        End Sub

        ''' <summary>
        ''' Creates a New PHoenix Speak ID
        ''' <para>
        ''' If the ID exists, Its Number of uses increases by one
        ''' </para>
        ''' </summary>
        ''' <param name="NewId">
        ''' </param>
        Sub New(ByRef NewId As Short)

            If List.ContainsKey(NewId) Then
                List.Item(NewId).Count += 1
            Else

                _id = getID(NewId)
                _Count = 1
                List.Add(NewId, Me)
            End If

        End Sub

#End Region

#Region "Public Properties"

        Public Property Count As Integer
            Get
                Return _Count
            End Get
            Private Set(ByVal value As Integer)
                _Count = value
            End Set
        End Property

        Public Property Id As Short
            Get
                Return _id
            End Get
            Set(ByVal value As Short)
                _id = value
                '_Count += 1
            End Set
        End Property

#End Region

#Region "Public Methods"

        Public Function HasId(ByVal id As Short) As Boolean
            Return List.ContainsKey(id)
        End Function

        ''' <summary>
        ''' Removes the specified Phoenix Speak ID
        ''' </summary>
        ''' <param name="id">
        ''' </param>
        Public Sub remove(id As Short)
            Dim PS As PsId
            If List.ContainsKey(id) Then
                PS = List.Item(id)
                PS.Count -= 1
                If PS.Count = 0 Then
                    List.Remove(id)
                End If
            End If
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Gets an available PS ID
        ''' </summary>
        ''' <param name="v">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Shared Function getID(ByRef v As Short) As Short
            If v = 0 Then v = 1
            SyncLock idLock
                While List.ContainsKey(v)
                    v += CShort(1)
                    If v = Short.MaxValue - 1 Then
                        v = 1
                    End If
                End While
            End SyncLock
            Return v
        End Function

#End Region

    End Class

End Namespace