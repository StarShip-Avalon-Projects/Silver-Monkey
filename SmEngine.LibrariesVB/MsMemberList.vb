Imports System.IO
Imports IO
Imports Libraries
Imports Libraries.MsLibHelper
Imports MonkeyCore2.IO
Imports Monkeyspeak
Imports Monkeyspeak.Logging

Namespace Libraries

    ''' <summary>
    ''' Dream Member List management
    ''' <para>
    ''' a Simple dream administration system using a text file to contain a
    ''' list of Furre as staff
    ''' </para>
    ''' <para>
    ''' NOTE: The BotController is considered to be on the list even if the
    '''       furres name is not in the text file
    ''' </para>
    ''' <para>
    ''' Default Member-List file: <see cref="Paths.SilverMonkeyBotPath"/>\MemberList.txt
    ''' </para>
    ''' <conceptualLink target="d1358c3d-d6d3-4063-a0ef-259e13752a0f"/>
    ''' <para/>
    ''' Credits: Drake for assistance with designing this system
    ''' </summary>
    Public Class MsMemberList
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        ''' <summary>
        ''' Member List file path
        ''' <para/>
        ''' Defaults to <see cref="IO.Paths.SilverMonkeyBotPath"/>\MemberList.txt
        ''' </summary>
        ''' <returns></returns>
        Public Property MemberList As String

#End Region

#Region "Public Constructors"

        Public Overrides ReadOnly Property BaseId As Integer
            Get
                Return 900
            End Get
        End Property

        Public Overrides Sub Initialize(ParamArray args() As Object)
            MyBase.Initialize(args)
            MemberList = Paths.CheckBotFolder("MemberList.txt")
            '(1:900) and the triggering furre is on my Dream Member List,
            Add(TriggerCategory.Condition,
                AddressOf TrigFurreIsMember,
                "and the triggering furre is on my dream Member List,")
            '(1:901) and the furre named {...} is on my Dream Member list.
            Add(TriggerCategory.Condition,
                AddressOf FurreNamedIsMember,
                "and the furre named {...} is on my Dream Member list,")

            '(1902) And the triggering furre Is Not on my Dream Member list.
            Add(TriggerCategory.Condition,
                AddressOf TrigFurreIsNotMember,
                "and the triggering furre is not on my Dream Member list,")
            '(1903) And the furre named {...} Is Not on my Dream Member list.
            Add(TriggerCategory.Condition,
                AddressOf FurreNamedIsNotMember,
                "and the furre named {...} is not on my Dream Member list,")

            '(1900) add the triggering furre to my Dream Member list if they aren't already on it.
            Add(TriggerCategory.Effect,
                AddressOf AddTrigFurre,
                "add the triggering furre to my Dream Member list if they aren't already on it.")
            '(5901) add the furre named {...} to my Dream Member list if they aren't already on it.
            Add(TriggerCategory.Effect,
                AddressOf AddFurreNamed,
                "add the furre named {...} to my Dream Member list if they aren't already on it.")

            '(5:902) remove the triggering furre to my Dream Member list if they are on it.
            Add(TriggerCategory.Effect,
                AddressOf RemoveTrigFurre,
                "remove the triggering furre to my Dream Member list if they are on it.")
            '(5:903) remove the furre named {...} from my Dream Member list if they are on it.
            Add(TriggerCategory.Effect,
                AddressOf RemoveFurreNamed,
                "remove the furre named {...} from my Dream Member list if they are on it.")

            '(5:904) Use file {...} as the dream member list.
            Add(TriggerCategory.Effect,
                AddressOf UseMemberFile,
                "Use file {...} as the dream member list.")
            '(5:905) store member list to variable %Variable.
            Add(TriggerCategory.Effect,
                AddressOf ListToVariable,
                "store member list to variable %Variable.")

        End Sub

        Private Function AddFurreNamed(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function AddTrigFurre(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function FurreNamedIsNotMember(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function TrigFurreIsNotMember(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function FurreNamedIsMember(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

        Private Function TrigFurreIsMember(reader As TriggerReader) As Boolean
            Throw New NotImplementedException()
        End Function

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Check for the member list file, If it doesn't exist created it.
        ''' <para>
        ''' Checks default <see cref="Paths.SilverMonkeyBotPath"/>
        ''' </para>
        ''' </summary>
        Public Sub CheckMemberList()
            MemberList = Paths.CheckBotFolder(MemberList)
            If Not File.Exists(MemberList) Then
                Using f As New StreamWriter(MemberList)
                    f.WriteLine("")
                End Using
            End If
        End Sub

        ''' <summary>
        ''' (5:905) store member list to variable %Variable.
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <returns></returns>
        Public Function ListToVariable(reader As TriggerReader) As Boolean

            Try
                CheckMemberList()
                Dim Furre = reader.ReadVariable(True)

                Dim f = New List(Of String)
                f.AddRange(File.ReadAllLines(MemberList))
                Furre.Value = String.Join("", f.ToArray)

                Return True
            Catch ex As Exception
                Logger.Info(Of MsMemberList)("A problem occurred checking the member-list")
                Logger.Error(Of MsMemberList)($"{ex.Message}")
            End Try
            Return False
        End Function

        ''' <summary>
        ''' (5:903) remove the furre named {...} from my Dream Member list
        ''' if they are on it.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function RemoveFurreNamed(reader As TriggerReader) As Boolean

            Try
                CheckMemberList()

                Dim Furre = reader.ReadString
                Dim line As String
                Dim linesList = New List(Of String)(File.ReadAllLines(MemberList))
                Using SR = New StreamReader(MemberList)
                    While SR.Peek() <> -1
                        line = SR.ReadLine()
                        For i As Integer = 0 To linesList.Count - 1
                            If line.ToFurcadiaShortName() = Furre.ToFurcadiaShortName() Then
                                linesList.RemoveAt(i)
                                File.WriteAllLines(MemberList, linesList.ToArray())
                                Exit For
                            End If
                        Next i
                    End While
                End Using

                Return True
            Catch ex As Exception
                Logger.Info(Of MsMemberList)("A problem occurred checking the member-list")
                Logger.Error(Of MsMemberList)($"{ex.Message}")
            End Try
            Return False
        End Function

        ''' <summary>
        ''' (5:902) remove the triggering furre to my Dream Member list if
        ''' they are on it.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function RemoveTrigFurre(reader As TriggerReader) As Boolean

            Try
                CheckMemberList()

                Dim Furre = reader.Page.GetVariable(TriggeringFurreNameVariable).Value.ToString

                Dim linesList As New List(Of String)(File.ReadAllLines(MemberList))
                Using SR As New StreamReader(MemberList)
                    While SR.Peek() <> -1
                        Dim line = SR.ReadLine()
                        For i As Integer = 0 To linesList.Count - 1
                            If line.ToFurcadiaShortName() = Furre.ToFurcadiaShortName() Then
                                linesList.RemoveAt(i)
                                File.WriteAllLines(MemberList, linesList.ToArray())
                                Exit For
                            End If
                        Next i
                    End While
                End Using

                Return True
            Catch ex As Exception
                Logger.Info(Of MsMemberList)("A problem occurred checking the member-list")
                Logger.Error(Of MsMemberList)($"{ex.Message}")
            End Try
            Return False
        End Function

        ''' <summary>
        ''' (5:904) Use file {...} as the dream member list.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function UseMemberFile(reader As TriggerReader) As Boolean
            Try
                MemberList = reader.ReadString
                CheckMemberList()
                Return True
            Catch ex As Exception
                Logger.Info(Of MsMemberList)("A problem occurred checking the member-list")
                Logger.Error(Of MsMemberList)($"{ex.Message}")
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Called when page is disposing or resetting.
        ''' </summary>
        ''' <param name="page">The page.</param>
        Public Overrides Sub Unload(page As Page)

        End Sub

#End Region

    End Class

End Namespace