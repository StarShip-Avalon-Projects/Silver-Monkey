Imports System.IO
Imports IO
Imports MonkeyCore
Imports Monkeyspeak
Imports Monkeyspeak.Logging
Imports MsLibHelper

Namespace Engine.Libraries

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
        ''' Defaults to <see cref="MonkeyCore.Paths.SilverMonkeyBotPath"/>\MemberList.txt
        ''' </summary>
        ''' <returns></returns>
        Public Property MemberList As String

#End Region

#Region "Public Constructors"

        Public Overrides Sub Initialize(ParamArray args() As Object)
            MyBase.Initialize(args)
            MemberList = Paths.CheckBotFolder("MemberList.txt")
            ''(1:900) and the triggering furre is on my Dream Member List,
            'Add(TriggerCategory.Condition, 900, AddressOf TrigFurreIsMember, "and the triggering furre is on my dream Member List,")
            ''(1:901) and the furre named {...} is on my Dream Member list.
            'Add(TriggerCategory.Condition, 901, AddressOf FurreNamedIsMember, "and the furre named {...} is on my Dream Member list,")

            '(1:902) and the triggering furre is not on my Dream Member list.
            ''  Add(TriggerCategory.Condition, 902, AddressOf TrigFurreIsNotMember, "and the triggering furre is not on my Dream Member list,")
            '(1:903) and the furre named {...} is not on my Dream Member list.
            ''        Add(TriggerCategory.Condition, 903, AddressOf FurreNamedIsNotMember, "and the furre named {...} is not on my Dream Member list,")

            '(1:900) add the triggering furre to my Dream Member list if they aren't already on it.
            ''     Add(TriggerCategory.Effect, 900, AddressOf AddTrigFurre, "add the triggering furre to my Dream Member list if they aren't already on it.")
            '(5:901) add the furre named {...} to my Dream Member list if they aren't already on it.
            ''    Add(TriggerCategory.Effect, 901, AddressOf AddFurreNamed, "add the furre named {...} to my Dream Member list if they aren't already on it.")

            '(5:902) remove the triggering furre to my Dream Member list if they are on it.
            Add(TriggerCategory.Effect, 902, AddressOf RemoveTrigFurre, "remove the triggering furre to my Dream Member list if they are on it.")
            '(5:903) remove the furre named {...} from my Dream Member list if they are on it.
            Add(TriggerCategory.Effect, 903, AddressOf RemoveFurreNamed, "remove the furre named {...} from my Dream Member list if they are on it.")

            '(5:904) Use file {...} as the dream member list.
            Add(TriggerCategory.Effect, 904, AddressOf UseMemberFile, "Use file {...} as the dream member list.")
            '(5:905) store member list to variable %Variable.
            Add(TriggerCategory.Effect, 905, AddressOf ListToVariable, "store member list to variable %Variable.")

        End Sub

#End Region

#Region "Private Methods"

        '''' <summary>
        '''' (5:901) add the furre named {...} to my Dream Member list if
        '''' they aren't already on it.
        '''' </summary>
        '''' <param name="reader">
        '''' <see cref="TriggerReader"/>
        '''' </param>
        '''' <returns>
        '''' </returns>
        'Public Function AddFurreNamed(reader As TriggerReader) As Boolean

        '    Try
        '        Dim Furre = reader.ReadString
        '        If FurreNamedIsNotMember(reader) Then
        '            Using sw = New StreamWriter(MemberList, True)
        '                sw.WriteLine(Furre)
        '            End Using
        '        End If
        '        Return True
        '    Catch ex As Exception
        '        Logger.Info(Of MsMemberList)("A problem occurred checking the member-list")
        '        Logger.Error(Of MsMemberList)($"{ex.Message}")
        '    End Try
        '    Return False
        'End Function

        '''' <summary>
        '''' (1:900) add the triggering furre to my Dream Member list if they
        '''' aren't already on it.
        '''' </summary>
        '''' <param name="reader">
        '''' <see cref="TriggerReader"/>
        '''' </param>
        '''' <returns>
        '''' </returns>
        'Public Function AddTrigFurre(reader As TriggerReader) As Boolean

        '    Try
        '        Dim Furre = reader.Page.GetVariable(TriggeringFurreNameVariable).Value.ToString
        '        If TrigFurreIsMember(reader) = False AndAlso TrigFurreIsNotMember(reader) Then
        '            Dim sw = New StreamWriter(MemberList, True)
        '            sw.WriteLine(Furre)
        '            sw.Close()
        '        End If
        '        Return True
        '    Catch ex As Exception
        '        Logger.Info(Of MsMemberList)("A problem occurred checking the member-list")
        '        Logger.Error(Of MsMemberList)($"{ex.Message}")
        '    End Try
        '    Return False
        'End Function

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

        '''' <summary>
        '''' (1:901) and the furre named {...} is on my Dream Member list.
        '''' </summary>
        '''' <param name="reader">
        '''' <see cref="TriggerReader"/>
        '''' </param>
        '''' <returns>
        '''' </returns>
        'Public Function FurreNamedIsMember(reader As TriggerReader) As Boolean

        '    Dim Furre = reader.ReadString
        '    Try
        '        CheckMemberList()
        '        Dim f = File.ReadAllLines(MemberList)
        '        For Each line In f
        '            If line.ToFurcadiaShortName() = Furre.ToFurcadiaShortName() Then
        '                Return True
        '            End If
        '        Next
        '        Return ParentBotSession.IsBotController
        '    Catch ex As Exception
        '        Logger.Info(Of MsMemberList)("A problem occurred checking the member-list")
        '        Logger.Error(Of MsMemberList)($"{ex.Message}")
        '    End Try
        '    Return False
        'End Function

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

        '''' <summary>
        '''' (1:900) and the triggering furre is on my dream Member List,
        '''' </summary>
        '''' <param name="reader">
        '''' <see cref="TriggerReader"/>
        '''' </param>
        '''' <returns>
        '''' </returns>
        'Public Function TrigFurreIsMember(reader As TriggerReader) As Boolean

        '    Try
        '        CheckMemberList()

        '        Dim Furre = reader.Page.GetVariable(TriggeringFurreNameVariable).Value.ToString

        '        Dim f = New List(Of String)
        '        f.AddRange(File.ReadAllLines(MemberList))
        '        For Each line In f
        '            If line.ToFurcadiaShortName() = Furre.ToFurcadiaShortName() Then Return True
        '        Next

        '        Return ParentBotSession.IsBotController
        '    Catch ex As Exception
        '        Logger.Info(Of MsMemberList)("A problem occurred checking the member-list")
        '        Logger.Error(Of MsMemberList)($"{ex.Message}")
        '    End Try
        '    Return False
        'End Function

        '''' <summary>
        '''' (1:902) and the triggering furre is not on my Dream Member list.
        '''' </summary>
        '''' <param name="reader">
        '''' <see cref="TriggerReader"/>
        '''' </param>
        '''' <returns>
        '''' </returns>
        'Public Function TrigFurreIsNotMember(reader As TriggerReader) As Boolean
        '    Return Not TrigFurreIsMember(reader)
        'End Function

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

        Public Overrides Sub Unload(page As Page)

        End Sub

#End Region

    End Class

End Namespace