Imports System.IO
Imports Furcadia.Net.Pounce
Imports Furcadia.Util
Imports MonkeyCore
Imports Monkeyspeak
Imports SilverMonkeyEngine.Engine.Libraries.Pounce
Imports SilverMonkeyEngine.SmConstants

Namespace Engine.Libraries

    ''' <summary>
    ''' Pounce Server interface with a list of furres contained in a simple text file. This system is styled after <see cref="MsMemberList"/>
    ''' </summary>
    ''' <remarks>
    ''' This classe is the Monkey Speak interface for using the Furcadia Pounce server. It does not read the online.ini located in the Furcadia App-Data Folder.
    ''' Instead it uses a text file With a list Of Furcadia Names(Long Or Short formats Don't matter). Furcadia.Net.Pounce is a HTTP(S)  Async Post Method
    ''' class that sends the requests to the server once every 30 seconds.
    ''' <para/>
    ''' Why 30 seconds? Because the Furcadia pounce server runs on a 30 second cron job, Therefore it makes sense to stick with it update time.
    ''' </remarks>
    Public NotInheritable Class MsPounce
        Inherits MonkeySpeakLibrary

        ''' <summary>
        ''' Default File we use
        ''' </summary>
        Public Const ListFile As String = "onlineList.txt"

        Private WithEvents OnlineFurreList As IO.NameList
        Private WithEvents PFure As MsPounceFurre
        Private _onlineListFile As String
        Private PounceFurreList As List(Of MsPounceFurre)
        Private smPounce As PounceClient

        ''' <summary>
        ''' Default Constructor
        ''' </summary>
        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)
            PounceFurreList = New List(Of MsPounceFurre)
            'Setup our Default Objects
            _onlineListFile = Paths.CheckBotFolder(ListFile)
            OnlineFurreList = New IO.NameList(_onlineListFile)

            ' (0:950) When a furre logs on,
            Add(TriggerCategory.Cause, 950,
            Function() True, "(0:950) When a furre logs on,")

            '(0:951) When a furre logs off,
            Add(TriggerCategory.Cause, 951,
            Function() True, "(0:951) When a furre logs off,")
            '(0:952) When the furre named {...} logs on,
            Add(TriggerCategory.Cause, 952, AddressOf NameIs, "(0:952) When the furre named {...} logs on,")
            '(0:953) When the furre named {...} logs off,
            Add(TriggerCategory.Cause, 953, AddressOf NameIs, "(0:953) When the furre named {...} logs off,")

            '(1;950) and the furre named {...} is on-line,
            Add(New Trigger(TriggerCategory.Condition, 950), AddressOf FurreNamedonline, "(1:950) and the furre named {...} is on-line,")

            '(1:951) and the furre named {...} is off-line,
            Add(New Trigger(TriggerCategory.Condition, 951), AddressOf FurreNamedNotOnline, "(1:951) and the furre named {...} is off-line,")

            '(1:952) and triggering furre is on the smPounce List,
            Add(New Trigger(TriggerCategory.Condition, 952), AddressOf TrigFurreIsMember, "(1:952) and triggering furre is on the smPounce List,")
            '(1:953) and the triggering furre is not on the smPounce List,
            Add(New Trigger(TriggerCategory.Condition, 953), AddressOf TrigFurreIsNotMember, "(1:953) and the triggering furre is not on the smPounce List,")

            '(1:954) and the furre named {...} is on the smPounce list,
            Add(New Trigger(TriggerCategory.Condition, 954), AddressOf FurreNamedIsMember, "(1:954) and the furre named {...} is on the smPounce list,")

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

        ''' <summary>
        ''' the File of the Friends List to Check
        ''' <para/>
        ''' Defaults to 'BotFolder\onlineList.txt"
        ''' </summary>
        ''' <returns>
        ''' </returns>
        Public ReadOnly Property OnlineListFile As String
            Get
                Return _onlineListFile
            End Get

        End Property

        ''' <summary>
        ''' (5:951) add the furre named {...} to the smPounce list.
        ''' they aren't already on it.
        ''' </summary>
        ''' <param name="reader"> <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function AddFurreNamed(reader As TriggerReader) As Boolean
            Dim Furre As String = Nothing

            Furre = reader.ReadString
            If FurreNamedIsMember(reader) = False And FurreNamedIsNotMember(reader) Then
                Using sw = New StreamWriter(_onlineListFile, True)
                    sw.WriteLine(Furre)
                End Using
            End If
            Return True

        End Function

        ''' <summary>
        ''' (5:950) add the triggering furre to the smPounce List.
        ''' aren't already on it.
        ''' <para/>
        ''' Defaults to 'BotFolder\on-lineList.txt"
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function AddTrigFurre(reader As TriggerReader) As Boolean

            Dim Furre = reader.Page.GetVariable(MS_Name).Value.ToString
            If TrigFurreIsMember(reader) = False And TrigFurreIsNotMember(reader) Then
                Using sw = New StreamWriter(_onlineListFile, True)
                    sw.WriteLine(Furre)
                    sw.Close()
                End Using
            End If
            Return True

        End Function

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
        ''' <see cref="Eventargs.Empty"/>
        ''' </param>
        Public Sub FurreLoggedOff(ByVal Furre As Object, e As EventArgs) Handles PFure.FurreLoggedOff
            Dim Furr As PounceFurre = CType(Furre, PounceFurre)
            FurcadiaSession.MSpage.RemoveVariable("%NAME")
            FurcadiaSession.MSpage.SetVariable("%NAME", Furr.Name, True)
            FurcadiaSession.MSpage.Execute(951, 953)
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
        Public Sub FurreLoggedOn(ByVal Furre As Object, e As EventArgs) Handles PFure.FurreLoggedOn
            Dim Furr = DirectCast(Furre, PounceFurre)
            FurcadiaSession.MSpage.RemoveVariable("%NAME")
            FurcadiaSession.MSpage.SetVariable("%NAME", Furr.Name, True)
            FurcadiaSession.MSpage.Execute(950, 952)
        End Sub

        ''' <summary>
        ''' (1:954) and the furre named {...} is on the smPounce list,
        ''' </summary>
        ''' <param name="reader"><see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function FurreNamedIsMember(reader As Monkeyspeak.TriggerReader) As Boolean
            CheckonlineList()

            Dim Furre = reader.ReadString
            Dim f = File.ReadAllLines(_onlineListFile)
            For Each l As String In f
                If FurcadiaShortName(l) = FurcadiaShortName(Furre) Then Return True
            Next
            Return False

        End Function

        ''' <summary>
        ''' (1:955) and the furre named {...} is not on the smPounce list,
        ''' </summary>
        ''' <param name="reader"> <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function FurreNamedIsNotMember(reader As Monkeyspeak.TriggerReader) As Boolean
            Return Not FurreNamedIsMember(reader)
        End Function

        ''' <summary>
        ''' (1:951) and the furre named {...} is off-line,
        ''' </summary>
        ''' <param name="reader"><see cref="TriggerReader"/></param>
        ''' <returns></returns>
        Public Function FurreNamedNotOnline(reader As TriggerReader) As Boolean

            Dim TmpName = reader.ReadString()
            For Each Fur In PounceFurreList
                If Fur.ShortName = FurcadiaShortName(TmpName) Then
                    Return Not Fur.Online
                End If
            Next
            'add Machine Name parser
            Return False

        End Function

        ''' <summary>
        ''' (1:950) and the furre named {...} is on-line,
        ''' </summary>
        ''' <param name="reader"><see cref="TriggerReader"/></param>
        ''' <returns></returns>
        Public Function FurreNamedonline(reader As TriggerReader) As Boolean

            Dim TmpName = reader.ReadString()
            For Each Fur In PounceFurreList
                If Fur.ShortName = FurcadiaShortName(TmpName) Then
                    Return Fur.Online
                End If
            Next
            'add Machine Name parser
            Return False

        End Function

        ''' <summary>
        ''' (5:953) remove the furre named {...} from the smPounce list.
        ''' </summary>
        ''' <param name="reader"><see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function RemoveFurreNamed(reader As TriggerReader) As Boolean

            CheckonlineList()

            Dim Furre = reader.ReadString
            Dim linesList = New List(Of String)(File.ReadAllLines(_onlineListFile))
            Using SR = New StreamReader(_onlineListFile)
                Dim line = SR.ReadLine()
                For i As Integer = 0 To linesList.Count - 1
                    If FurcadiaShortName(line) = FurcadiaShortName(Furre) Then
                        linesList.RemoveAt(i)
                        File.WriteAllLines(_onlineListFile, linesList.ToArray())
                        Return True
                    End If
                    line = SR.ReadLine()
                Next i
            End Using

            Return False
        End Function

        ''' <summary>
        ''' (5:952) remove the triggering furre from the smPounce list.
        ''' </summary>
        ''' <param name="reader"> <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function RemoveTrigFurre(reader As TriggerReader) As Boolean

            CheckonlineList()

            Dim Furre = reader.Page.GetVariable(MS_Name).Value.ToString
            Furre = FurcadiaShortName(Furre)
            Dim line As String = Nothing
            Dim linesList = New List(Of String)(File.ReadAllLines(_onlineListFile))
            Using SR = New StreamReader(_onlineListFile)
                line = SR.ReadLine()
                For i As Integer = 0 To linesList.Count - 1
                    If FurcadiaShortName(line) = Furre Then
                        linesList.RemoveAt(i)
                        File.WriteAllLines(_onlineListFile, linesList.ToArray())
                        Return True
                    End If
                    line = SR.ReadLine()
                Next i
            End Using

            Return False
        End Function

        ''' <summary>
        ''' (1:952) and triggering furre is on the smPounce List,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function TrigFurreIsMember(reader As Monkeyspeak.TriggerReader) As Boolean
            CheckonlineList()

            Dim Furre = reader.Page.GetVariable(MS_Name).Value.ToString
            Dim f = File.ReadAllLines(_onlineListFile)
            For Each l As String In f
                If FurcadiaShortName(l) = FurcadiaShortName(Furre) Then Return True
            Next
            Return False

        End Function

        ''' <summary>
        ''' (1:953) and the triggering furre is not on the smPounce List,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function TrigFurreIsNotMember(reader As Monkeyspeak.TriggerReader) As Boolean
            Return Not TrigFurreIsMember(reader)
        End Function

        ''' <summary>
        ''' (5:904) Use file {...} as the dream member list.
        ''' <para/>
        ''' Defaults to 'BotFolder\on-lineList.txt"
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' True on Success
        ''' </returns>
        Public Function UseMemberFile(reader As TriggerReader) As Boolean

            Dim FileList = reader.ReadString
            CheckonlineList()
            smPounce = New PounceClient(OnlineFurreList.ToArray, Nothing)

            Return True
        End Function

        ''' <summary>
        ''' (0:953) When the furre named {...} logs off,
        ''' <para>
        ''' (0:952) When the furre named {...} logs on,
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Protected Overrides Function NameIs(reader As TriggerReader) As Boolean

            Return MyBase.NameIs(reader)

        End Function

        Private Sub CheckonlineList()
            _onlineListFile = Paths.CheckBotFolder(_onlineListFile)
            If File.Exists(_onlineListFile) = False Then
                Console.WriteLine("On-line List File: " + _onlineListFile + " Doesn't Exist, Creating new file")
                Using sw = New StreamWriter(_onlineListFile, False)
                    sw.Close()
                End Using
            End If
        End Sub

        Public Overrides Sub Unload(page As Page)

        End Sub

    End Class

End Namespace