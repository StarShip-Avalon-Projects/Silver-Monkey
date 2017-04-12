Imports Monkeyspeak

Imports System.Collections.Generic
Imports SilverMonkey.Engine.Libraries.PhoenixSpeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Monkey Speak Interface to the PhoenixSpeak server interface
    ''' <para>Checks and executes Basic PhoenixSpeak response triggers</para>
    ''' </summary>
    Public Class MsPhoenixSpeak
        Inherits MonkeySpeakLibrary

#Region "Fields"

        Private WithEvents SubSys As New SubSystem()

#End Region

#Region "Public Constructors"

        Public Sub New()
            MyBase.New()

            Add(TriggerCategory.Cause, 80,
        Function()
            Return True
        End Function,
     "(0:80) When the bot sees a Phoenix Speak response")
            Add(TriggerCategory.Cause, 81,
             AddressOf msgIs, "(0:81) When the bot sees the Phoenix Speak response {...},")

            Add(TriggerCategory.Cause, 82,
             AddressOf msgContains, "(0:82) When the bot sees a Phoenix Speak response with {...} in it,")

            '(5:60) get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect, 60, AddressOf RemberPSInforTrigFurre, "(5:60) get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.")

            '(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect, 61, AddressOf RemberPSInforFurreNamed, "(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.")

            '(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect, 62, AddressOf RemberPSInfoAllDream, "(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.")

            '(5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.
            Add(TriggerCategory.Effect, 63, AddressOf RemberPSInfoAllCharacters, "(5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.")

            '(5:80) retrieve  Phoenix Speak info {...} and place the value into variable %Variable.
            Add(TriggerCategory.Effect, 80, AddressOf getPSinfo, "(5:80) retrieve  Phoenix Speak info {...} and place the value into variable %Variable.")

            '(5:81) Store PSInfo Key Names to Variable %Variable.
            Add(TriggerCategory.Effect, 81, AddressOf PSInfoKeyToVariable, "(5:81) Store PSInfo Key Names to Variable %Variable.")

            '(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.
            Add(TriggerCategory.Effect, 82, AddressOf MemorizeFurreNamedPS, "(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.")

            '(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.
            Add(TriggerCategory.Effect, 83, AddressOf ForgetFurreNamedPS, "(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.")

            '(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.
            Add(TriggerCategory.Effect, 84, AddressOf MemorizeTrigFurrePS, "(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.")

            '(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.
            Add(TriggerCategory.Effect, 85, AddressOf ForgetTrigFurrePS, "(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.")

            '(5:90) Memorize Phoenix Speak info {...} for this dream.
            Add(TriggerCategory.Effect, 90, AddressOf MemorizeDreamPS, "(5:90) Memorize Phoenix Speak info {...} for this dream.")
            '(5:91) Forget Phoenix Speak info {...} for this dream.
            Add(TriggerCategory.Effect, 91, AddressOf ForgetDreamPS, "(5:91) Forget Phoenix Speak info {...} for this dream.")

            '(5:94) execute Phoenix Speak command {...}.
            Add(TriggerCategory.Effect, 94, AddressOf PSCommand, "(5:94) execute Phoenix Speak command {...}.")
            Add(TriggerCategory.Effect, 95, AddressOf PSForgetTriggeringFurre, "(5:95) Forget ALL Phoenix Speak info for the triggering furre")
            Add(TriggerCategory.Effect, 96, AddressOf PSForgetFurreNamed, "(5:96) Forget ALL Phoenix Speak info for the furre named {...}.")
            Add(TriggerCategory.Effect, 97, AddressOf PSForgetDream, "(5:97) Forget ALL Phoenix Speak info for this dream.")

        End Sub

#End Region

#Region "Public Methods"

        '(5:91) Forget Phoenix Speak info {...} for this dream.
        Function ForgetDreamPS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                sendServer("ps clear dream." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:83) Forget Phoenix Speak info {...} for the Furre Named {...}.
        Function ForgetFurreNamedPS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                Dim furre As String = reader.ReadString
                sendServer("ps clear characer." + furre + "." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:85) Forget Phoenix Speak info {...} for the Triggering Furre.
        Function ForgetTrigFurrePS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                Dim furre As String = FurcadiaSession.Player.ShortName
                sendServer("ps clear characer." + furre + "." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        Public Function getPSinfo(reader As TriggerReader) As Boolean
            Try
                Dim Info As New PhoenixSpeak.Variable(reader.ReadString())
                Dim var As Monkeyspeak.Variable = reader.ReadVariable(True)
                If SubSys.PSInfoCache.Contains(Info) Then
                    var = Info
                Else
                    var.Value = Nothing
                End If
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:90) Memorize Phoenix Speak info {...} for this dream.
        Function MemorizeDreamPS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                sendServer("ps set dream." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:82) Memorize Phoenix Speak info {...} for the Furre Named {...}.
        Function MemorizeFurreNamedPS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                Dim furre As String = reader.ReadString
                sendServer("ps set characer." + furre + "." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:84) Memorize Phoenix Speak info {...} for the Triggering Furre.
        Function MemorizeTrigFurrePS(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                Dim furre As String = FurcadiaSession.Player.ShortName
                sendServer("ps set characer." + furre + "." + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(0:17) When someone whispers something with {...} in it,
        Function msgContains(reader As TriggerReader) As Boolean
            Dim msMsg As String = ""
            Dim msg As String = ""
            Try
                'Debug.Print("msgContains Begin Execution")
                msMsg = reader.ReadString()
                'Debug.Print("msMsg = " & msMsg)
                msg = MsPage.GetVariable("MESSAGE").Value.ToString
                'Debug.Print("Msg = " & msg)
                Return msg.Contains(msMsg)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        Public Function msgIs(reader As TriggerReader) As Boolean
            Try
                Dim msMsg As String = reader.ReadString()
                Dim msg As Monkeyspeak.Variable = MsPage.GetVariable("MESSAGE")
                If msMsg = msg.Value.ToString Then Return True
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        Function msgIsNot(reader As TriggerReader) As Boolean
            Try
                Dim msMsg As String = reader.ReadString()
                Dim msg As Monkeyspeak.Variable = MsPage.GetVariable("MESSAGE")
                If msMsg <> msg.Value.ToString Then Return True
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        Function msgNotContain(reader As TriggerReader) As Boolean
            Try
                Dim msMsg As String = reader.ReadString()
                'Debug.Print("msMsg = " & msMsg)
                Dim msg As Monkeyspeak.Variable = MsPage.GetVariable("MESSAGE")
                'Debug.Print("Msg = " & msg.Value.ToString)
                If msg.Value.ToString.Contains(msMsg) Then Return False
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function
        '(5:94) execute Phoenix Speak command {...}.
        Function PSCommand(reader As TriggerReader) As Boolean
            Try
                Dim info As String = reader.ReadString
                sendServer("ps " + info)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        Function PSForgetDream(reader As TriggerReader) As Boolean
            Try
                sendServer("ps clear dream ")
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        Function PSForgetFurreNamed(reader As TriggerReader) As Boolean
            Try
                Dim Furre As String = reader.ReadString
                sendServer("ps clear character. " + Furre)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        Function PSForgetTriggeringFurre(reader As TriggerReader) As Boolean
            Try
                sendServer("ps clear character." + FurcadiaSession.Player.ShortName)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:81) Store PSInfo Key Names to Variable %Variable.
        Function PSInfoKeyToVariable(reader As TriggerReader) As Boolean
            Try
                Dim Var As Monkeyspeak.Variable = reader.ReadVariable(True)
                Dim L As New List(Of String)
                For Each Name As PhoenixSpeak.Variable In SubSys.PSInfoCache
                    L.Add(Name.Name)
                Next
                Var.Value = String.Join(" ", L)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.
        Function RemberPSInfoAllCharacters(reader As TriggerReader) As Boolean
            Try
                sendServer("ps get character.*")
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:62) get All Phoenix Speak info for the dream and put it into the PSInfo Cache.
        Function RemberPSInfoAllDream(reader As TriggerReader) As Boolean
            Try
                sendServer("ps get dream.*")
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:61) get All Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.
        Function RemberPSInforFurreNamed(reader As TriggerReader) As Boolean
            Try
                Dim furre As String = reader.ReadString
                sendServer("ps get characer." + furre + ".*")
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

        '(5:60) get All Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.
        Function RemberPSInforTrigFurre(reader As TriggerReader) As Boolean
            Try
                Dim furre As String = FurcadiaSession.Player.ShortName
                sendServer("ps set characer." + furre + ".*")
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return True
        End Function

#End Region

    End Class
End Namespace