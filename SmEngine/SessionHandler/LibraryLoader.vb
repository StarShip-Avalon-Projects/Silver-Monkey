Imports Monkeyspeak
Imports SilverMonkeyEngine.Engine.Libraries

Namespace Engine

    ''' <summary>
    ''' Functions for linking components between windows forms
    ''' 
    ''' </summary>
    Public NotInheritable Class LibraryUtils

        ''' <summary>
        ''' Load Libraries into the engine
        ''' </summary>
        ''' <param name="Session">BotSession</param>
        ''' <param name="silent"> Announce Loaded Libraries</param>
        Public Shared Function LoadLibrary(Session As BotSession, silent As Boolean) As Page
            Dim MSpage = Session.MSpage
            'Library Loaded?.. Get the Hell out of here
            MSpage.SetTriggerHandler(TriggerCategory.Cause, 0, Function(reader) True,
            "(0:0) When the bot starts,")

            MSpage.LoadSysLibrary()

#If CONFIG = "Release" Then
            '(5:110) load library from file {...}.
            MSpage.RemoveTriggerHandler(TriggerCategory.Effect, 110)
#ElseIf CONFIG = "Debug" Then
            '
            MSpage.RemoveTriggerHandler(TriggerCategory.Effect, 105)
            MSpage.SetTriggerHandler(TriggerCategory.Effect, 105,
         Function() False, "(5:105) raise an error.")
            ',
#End If

            MSpage.LoadTimerLibrary(100)
            MSpage.LoadIOLibrary(Session.MainEngineOptions.BotPath)
            MSpage.LoadStringLibrary()
            MSpage.LoadMathLibrary()
            Dim LibList = InitializeEngineLibraries(Session)

            For Each Library As Monkeyspeak.Libraries.BaseLibrary In LibList
                Try
                    MSpage.LoadLibrary(Library)
                    If Not silent Then Console.WriteLine(String.Format("Loaded Monkey Speak Library: {0}", Library.GetType().Name))
                Catch ex As Exception
                    Throw New MonkeyspeakException(Library.GetType().Name + " " + ex.Message, ex)

                End Try
            Next

            Return MSpage

        End Function

        Private Shared Function InitializeEngineLibraries(Session As BotSession) As List(Of Monkeyspeak.Libraries.BaseLibrary)
            ' Comment out Libs to Disable

            Dim LibList = New List(Of Monkeyspeak.Libraries.BaseLibrary) From {
                                New StringLibrary(Session),
                    New MsSayLibrary(Session),
                    New MsBanish(Session),
                    New MsTime(Session),
                    New MsDatabase(Session),
                    New MsWebRequests(Session),
                    New MS_Cookie(Session),
                    New MsPhoenixSpeak(Session),
                    New MsPhoenixSpeakBackupAndRestore(Session),
                    New MsDice(Session),
                    New MsFurreList(Session),
                    New MsWarning(Session),
                    New MsMovement(Session),
                    New WmCpyDta(Session),
                    New MsMemberList(Session),
                    New MsPounce(Session),
                    New MsVerbot(Session),
                    New MsSound(Session),
                    New MsTrades(Session),
                    New MsDreamInfo(Session)
                }
            ' New MathLibrary(Me),
            'New MsIO(Me),
            'LibList.Add(New MS_MemberList())
            Return LibList
        End Function

    End Class

End Namespace