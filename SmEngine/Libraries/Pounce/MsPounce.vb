Imports System.IO
Imports Furcadia.Net.Pounce
Imports Furcadia.Util
Imports MonkeyCore
Imports Monkeyspeak
Imports SilverMonkeyEngine.SmConstants

Namespace Engine.Libraries.Pounce

    ''' <summary>
    ''' Monkey Speak version of the Furcadia Pounce Cliet
    ''' </summary>
    Public Class MsPounce
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        Private WithEvents OnlineFurreList As IO.NameList
        Private WithEvents pFure As PounceFurre
        Private _OnlineListFile As String
        Private PounceFurreList As List(Of PounceFurre)
        Private smPounce As PounceClient

#End Region

#Region "Public Constructors"

        ''' <summary>
        ''' Default Constructor
        ''' </summary>
        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)
            PounceFurreList = New List(Of PounceFurre)
            'Setup our Default Objects
            OnlineFurreList = New IO.NameList(_OnlineListFile)

            ' (0:950) When a furre logs on,
            Add(TriggerCategory.Cause, 950,
            Function()
                Return True
            End Function, "(0:950) When a furre logs on,")

            '(0:951) When a furre logs off,
            Add(TriggerCategory.Cause, 951,
            Function()
                Return True
            End Function, "(0:951) When a furre logs off,")
            '(0:952) When the furre named {...} logs on,
            Add(TriggerCategory.Cause, 952, AddressOf NameIs, "(0:952) When the furre named {...} logs on,")
            '(0:953) When the furre named {...} logs off,
            Add(TriggerCategory.Cause, 953, AddressOf NameIs, "(0:953) When the furre named {...} logs off,")

            '(1;950) and the furre named {...} is online,
            Add(New Trigger(TriggerCategory.Condition, 950), AddressOf FurreNamedOnline, "(1:950) and the furre named {...} is online,")

            '(1:951) and the furre named {...} is offline,
            Add(New Trigger(TriggerCategory.Condition, 951), AddressOf FurreNamedNotOnline, "(1:951) and the furre named {...} is offline,")

            '(1:952) and triggering furre is on the smPounce List,
            Add(New Trigger(TriggerCategory.Condition, 952), AddressOf TrigFurreIsMember, "(1:952) and triggering furre is on the smPounce List,")
            '(1:953) and the triggering furre is not on the smPounce List,
            Add(New Trigger(TriggerCategory.Condition, 953), AddressOf TrigFurreIsNotMember, "(1:953) and the triggering furre is not on the smPounce List,")

            '(1:954) and the furren named {...} is on the smpounce list,
            Add(New Trigger(TriggerCategory.Condition, 954), AddressOf FurreNamedIsMember, "(1:954) and the furren named {...} is on the smpounce list,")

            '(1:955) and the furre named {...} is not on the smPounce list,
            Add(New Trigger(TriggerCategory.Condition, 955), AddressOf FurreNamedIsNotMember, "(1:955) and the furre named {...} is not on the smPounce list,")

            '(5:950) add the triggering furre to the smPounce List.
            Add(New Trigger(TriggerCategory.Effect, 950), AddressOf AddTrigFurre, "(5:950) add the triggering furre to the smPounce List.")
            '(5:951) add the furre named {...} to the smPounce list.
            Add(New Trigger(TriggerCategory.Effect, 951), AddressOf AddFurreNamed, "(5:951) add the furre named {...} to the smPounce list.")
            '(5:952) remove the triggering furre from the smPounce list.
            Add(New Trigger(TriggerCategory.Effect, 952), AddressOf RemoveTrigFurre, "(5:952) remove the triggering furre from the smPounce list.")
            '(5:953) remove the furre named {...} from the smPounce list.
            Add(New Trigger(TriggerCategory.Effect, 953), AddressOf RemoveFurreNamed, "(5:953) remove the furre named {...} from the smPounce list.")
            '(5:954) use the file named {...} as the smPounce list.
            Add(New Trigger(TriggerCategory.Effect, 954), AddressOf UseMemberFile, "(5:954) use the file named {...} as the smPounce list.")
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' the File of the Friends List to Check
        ''' </summary>
        ''' <returns>
        ''' </returns>
        Public Property OnlineListFile As String
            Get
                If String.IsNullOrEmpty(_OnlineListFile) Then
                    _OnlineListFile = Paths.CheckBotFolder("OnlineList.txt")
                End If
                Return _OnlineListFile
            End Get
            Set(value As String)

            End Set
        End Property

#End Region

#Region "Public Methods"

        Function FurreNamedNotOnline(reader As TriggerReader) As Boolean
            Try
                Dim TmpName As String = reader.ReadString()
                For Each Fur As PounceFurre In PounceFurreList
                    If Fur.ShortName = FurcadiaShortName(TmpName) Then
                        Return Not Fur.Online
                    End If
                Next
                'add Machine Name parser
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        Function FurreNamedOnline(reader As TriggerReader) As Boolean
            Try
                Dim TmpName As String = reader.ReadString()
                For Each Fur As PounceFurre In PounceFurreList
                    If Fur.ShortName = FurcadiaShortName(TmpName) Then
                        Return Fur.Online
                    End If
                Next
                'add Machine Name parser
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        Function NameIs(reader As TriggerReader) As Boolean
            Try
                Dim TmpName As String = reader.ReadString()
                Dim tname As Variable = MsPage.GetVariable(MS_Name)
                'add Machine Name parser
                Return FurcadiaShortName(TmpName) = FurcadiaShortName(tname.Value.ToString)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

#End Region

#Region "Private Methods"

        '(5:901) add the furre named {...} to my Dream Member list if they aren't already on it.
        Private Function AddFurreNamed(reader As TriggerReader) As Boolean
            Dim Furre As String = Nothing

            Try
                Furre = reader.ReadString
                If FurreNamedIsMember(reader) = False And FurreNamedIsNotMember(reader) Then
                    Dim sw As StreamWriter = New StreamWriter(_OnlineListFile, True)
                    sw.WriteLine(Furre)
                    sw.Close()
                End If
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try

        End Function

        '(1:900) add the triggering furre to my Dream Member list if they aren't already on it.
        Private Function AddTrigFurre(reader As TriggerReader) As Boolean
            Dim Furre As String = Nothing

            Try
                Furre = MsPage.GetVariable(MS_Name).Value.ToString
                If TrigFurreIsMember(reader) = False And TrigFurreIsNotMember(reader) Then
                    Dim sw As StreamWriter = New StreamWriter(_OnlineListFile, True)
                    sw.WriteLine(Furre)
                    sw.Close()
                End If
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try

        End Function

        Private Sub CheckCemberList()
            _OnlineListFile = Paths.CheckBotFolder(_OnlineListFile)
            If File.Exists(_OnlineListFile) = False Then
                Dim sw As StreamWriter = New StreamWriter(_OnlineListFile, False)
                sw.Close()
            End If
        End Sub

        '(1:901) and the furre named {...} is on my Dream Member list.
        Private Function FurreNamedIsMember(reader As Monkeyspeak.TriggerReader) As Boolean
            CheckCemberList()
            Dim Furre As String = Nothing
            Dim f() As String
            Try
                Furre = reader.ReadString
                f = File.ReadAllLines(_OnlineListFile)
                For Each l As String In f
                    If FurcadiaShortName(l) = FurcadiaShortName(Furre) Then Return True
                Next
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try

        End Function

        '(1:903) and the furre named {...} is not on my Dream Member list.
        Private Function FurreNamedIsNotMember(reader As Monkeyspeak.TriggerReader) As Boolean
            Return Not FurreNamedIsMember(reader)
        End Function

        '(5:903) remove the furre named {...} from my Dream Member list if they are on it.
        Private Function RemoveFurreNamed(reader As TriggerReader) As Boolean
            Dim Furre As String = Nothing
            CheckCemberList()
            Try
                Furre = reader.ReadString
                Dim line As String = Nothing
                Dim linesList As New List(Of String)(File.ReadAllLines(_OnlineListFile))
                Dim SR As New StreamReader(_OnlineListFile)
                line = SR.ReadLine()
                For i As Integer = 0 To linesList.Count - 1
                    If FurcadiaShortName(line) = FurcadiaShortName(Furre) Then
                        SR.Dispose()
                        SR.Close()
                        linesList.RemoveAt(i)
                        File.WriteAllLines(_OnlineListFile, linesList.ToArray())
                        Return True
                    End If
                    line = SR.ReadLine()
                Next i
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return False
        End Function

        '(5:902) remove the triggering furre to my Dream Member list if they are on it.
        Private Function RemoveTrigFurre(reader As TriggerReader) As Boolean
            Dim Furre As String = Nothing
            CheckCemberList()
            Try
                Furre = MsPage.GetVariable(MS_Name).Value.ToString
                Furre = FurcadiaShortName(Furre)
                Dim line As String = Nothing
                Dim linesList As New List(Of String)(File.ReadAllLines(_OnlineListFile))
                Using SR As New StreamReader(_OnlineListFile)
                    line = SR.ReadLine()
                    For i As Integer = 0 To linesList.Count - 1
                        If FurcadiaShortName(line) = Furre Then
                            SR.Dispose()
                            SR.Close()
                            linesList.RemoveAt(i)
                            File.WriteAllLines(_OnlineListFile, linesList.ToArray())
                            Return True
                        End If
                        line = SR.ReadLine()
                    Next i
                End Using
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return False
        End Function

        '(1:900) and the triggering furre is on my dream Member List,
        Private Function TrigFurreIsMember(reader As Monkeyspeak.TriggerReader) As Boolean
            CheckCemberList()
            Dim Furre As String = Nothing
            Dim f() As String
            Try
                Furre = MsPage.GetVariable(MS_Name).Value.ToString
                f = File.ReadAllLines(_OnlineListFile)
                For Each l As String In f
                    If FurcadiaShortName(l) = FurcadiaShortName(Furre) Then Return True
                Next
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try

        End Function

        '(1:902) and the triggering furre is not on my Dream Member list.
        Private Function TrigFurreIsNotMember(reader As Monkeyspeak.TriggerReader) As Boolean
            Return Not TrigFurreIsMember(reader)
        End Function

        '(5:904) Use file {...} as the dream member list.
        ''' <summary>
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' True on Success
        ''' </returns>
        Private Function UseMemberFile(reader As TriggerReader) As Boolean

            Try
                Dim FileList As String = reader.ReadString
                CheckCemberList()
                smPounce = New PounceClient(OnlineFurreList.ToArray, Nothing)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

#End Region

        ''' <summary>
        ''' MonkeySpeak Execute When a Furre Logged Off
        ''' <para>
        ''' (0:951) When a furre logs off,
        ''' </para>
        ''' <para>
        ''' (0:953) When the furre named {...} logs off,
        ''' </para>
        ''' </summary>
        ''' <param name="Furre">
        ''' PounceFurre Object
        ''' </param>
        ''' <param name="e">
        ''' EventSrgs.Empty
        ''' </param>
        Private Sub FurreLoggedOff(ByVal Furre As Object, e As EventArgs) Handles pFure.FurreLoggedOff
            Dim Furr As PounceFurre = CType(Furre, PounceFurre)
            MsPage.SetVariable("NAME", Furr.Name, True)
            MsPage.Execute(951, 953)
        End Sub

        ''' <summary>
        ''' MonkeySpeak Execute When a Furre Logged on
        ''' <para>
        ''' (0:950) When a furre logs on,
        ''' </para>
        ''' <para>
        ''' (0:952) When the furre named {...} logs on,
        ''' </para>
        ''' </summary>
        ''' <param name="Furre">
        ''' PounceFurre Object
        ''' </param>
        ''' <param name="e">
        ''' EventSrgs.Empty
        ''' </param>
        Private Sub FurreLoggedOn(ByVal Furre As Object, e As EventArgs) Handles pFure.FurreLoggedOn
            Dim Furr As PounceFurre = CType(Furre, PounceFurre)
            MsPage.SetVariable("NAME", Furr.Name, True)
            MsPage.Execute(950, 952)
        End Sub

    End Class

End Namespace