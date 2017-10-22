Imports System.IO
Imports Conversive.Verbot5
Imports MonkeyCore
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Chatter Bot interface using the Verbot SDK
    ''' </summary>
    Public NotInheritable Class MsVerbot
        Inherits MonkeySpeakLibrary

#Region "Public Fields"

        <CLSCompliant(False)>
        Public _state As State

        <CLSCompliant(False)>
        Public kb As KnowledgeBase = New KnowledgeBase()

        <CLSCompliant(False)>
        Public kbi As KnowledgeBaseItem = New KnowledgeBaseItem()

        <CLSCompliant(False)>
        Public verbot As Verbot5Engine

        ' <CLSCompliant(False)>

#End Region

#Region "Private Fields"

        Private ChatCMD As String

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)
            verbot = New Verbot5Engine()
            _state = New State()

        End Sub

        Public Overrides Sub Initialize()
            '(0:1500) When the chat engine executes command {...},
            Add(TriggerCategory.Cause, 1500,
                 AddressOf ChatExecute, " When the chat engine executes command {...},")

            '(1:1500) and the Chat Engine State variable {...} is equal to {..},
            '(1:1501) and the Chat Engine State variable {...} is not equal to {..},
            '(1:1502) and the Chat Engine State variable {...} is equal to #,
            '(1:1503) and the Chat Engine State variable {...} is not equal to #,
            '(1:1504) and the Chat Engine State variable {...} is greater than #,
            '(1:1505) and the Chat Engine State variable {...} is greater than or equal to #,
            '(1:1506) and the Chat Engine State variable {...} is less than #,
            '(1:1507) And the Chat Engine State variable {...} Is less than Or equal To #,

            '(5:1500) use knowledgbase file {...} (*.vkb) and start the chat engine.
            Add(TriggerCategory.Effect, 1500,
                AddressOf UseKkbFile, " use knowledge base file {...} (*.vkb) and start the chat engine.")

            '(5:1501) send text {...} to chat engine and put the response in variable %Variable.
            Add(TriggerCategory.Effect, 1501,
                AddressOf GetReply, " send text {...} to chat engine and put the response in variable %Variable.")

            '(5:1502) send text {...} and Name {...} to chat engine and put the response in variable %Variable
            Add(TriggerCategory.Effect, 1502,
                 AddressOf GetReplyName, " send text {...} and Name {...} to chat engine and put the response in variable %Variable.")

            '(5:1503) Set Chat Engine State Vairable {...} to {...}.
            Add(TriggerCategory.Effect, 1503,
                AddressOf SetStateVariable, " Set Chat Engine State Vairable {...} to {...}.")

            '(5:1504) Get chat engine _state variable {...} and put it into variable %Variable.
            Add(TriggerCategory.Effect, 1504,
                 AddressOf GetStateVariable, " Get chat engine state variable {...} and put it into variable %Variable.")

        End Sub

#End Region

#Region "Chat interface"

        ''' <summary>
        ''' (5:1501) send text {...} to chat engine and put the response in
        ''' variable %Variable
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function GetReply(reader As TriggerReader) As Boolean
            Dim SayText As String
            Dim ResponceText

            SayText = reader.ReadString
            ResponceText = reader.ReadVariable(True)
            If _state.Vars.ContainsKey("botname") Then
                _state.Vars.Item("botname") = FurcadiaSession.ConnectedFurre.Name
            Else
                _state.Vars.Add("botname", FurcadiaSession.ConnectedFurre.Name)
            End If
            If _state.Vars.ContainsKey("channel") Then
                _state.Vars.Item("channel") = FurcadiaSession.Channel
            Else
                _state.Vars.Add("channel", FurcadiaSession.Channel)
            End If
            Dim reply As Reply = verbot.GetReply(Player, SayText, _state)

            If reply Is Nothing Then Return False

            ResponceText.Value = reply.Text
            ParseEmbeddedOutputCommands(reply.AgentText, reader)
            Return True

        End Function

        ''' <summary>
        ''' (5:1502) send text {...} and Name {...} to chat engine and put
        ''' the response in variable %Variable
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function GetReplyName(reader As TriggerReader) As Boolean

            Dim SayText = reader.ReadString
            Dim ResponceText = reader.ReadVariable(True)
            If _state.Vars.ContainsKey("botname") Then
                _state.Vars.Item("botname") = FurcadiaSession.ConnectedFurre.Name
            Else
                _state.Vars.Add("botname", FurcadiaSession.ConnectedFurre.Name)
            End If
            If _state.Vars.ContainsKey("channel") Then
                _state.Vars.Item("channel") = FurcadiaSession.Channel
            Else
                _state.Vars.Add("channel", FurcadiaSession.Channel)
            End If
            Dim reply As Reply = verbot.GetReply(SayText, _state)

            If reply Is Nothing Then Return False

            ResponceText.Value = reply.Text
            Me.ParseEmbeddedOutputCommands(reply.AgentText, reader)
            Return True

        End Function

        Private Sub ParseEmbeddedOutputCommands(text As String, reader As TriggerReader)
            Dim startCommand As String = "<"
            Dim endCommand As String = ">"

            Dim start As Integer = text.IndexOf(startCommand)
            Dim [end] As Integer = -1

            While start <> -1
                [end] = text.IndexOf(endCommand, start)
                If [end] <> -1 Then
                    Dim command As String = text.Substring(start + 1, [end] - start - 1).Trim()
                    If String.IsNullOrEmpty(command) Then
                        RunEmbeddedOutputCommand(command, reader)
                    End If
                End If
                start = text.IndexOf(startCommand, start + 1)
            End While
        End Sub

        'parseEmbeddedOutputCommands(string text)
        Private Sub RunEmbeddedOutputCommand(command As String, reader As TriggerReader)
            Dim spaceIndex As Integer = command.IndexOf(" ")

            Dim [function] As String
            Dim args As String
            If spaceIndex = -1 Then
                [function] = command.ToLower()
                args = ""
            Else
                [function] = command.Substring(0, spaceIndex).ToLower()
                args = command.Substring(spaceIndex + 1)
            End If

            Try
                Select Case [function]
                    Case "setmsvariable"
                        Dim VarIndex = args.IndexOf(" ")
                        Dim VarDataIndex = args.IndexOf("=")
                        Dim VarName As String = Nothing
                        Dim VarData As String = Nothing

                        If VarIndex <> -1 Then
                            VarName = args.Substring(0, VarIndex)
                            If VarDataIndex <> -1 Then
                                VarData = args.Substring(VarDataIndex + 1)
                            End If
                            reader.Page.SetVariable(VarName, VarData, False)
                        End If

                    'ChatExecute
                    Case "executechatcmd"
                        ChatCMD = args
                        reader.Page.Execute(1500)

                    Case Else
                        Exit Select
                        'switch
                End Select
            Catch
            End Try
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (0:1500) When the chat engine executes command {...},
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function ChatExecute(reader As TriggerReader) As Boolean

            Dim cmd As String = reader.ReadString()
            Return ChatCMD.ToLower() = cmd.ToLower()

        End Function

        ''' <summary>
        ''' (5:1504) Get chat engine _state variable {...} and put it into
        ''' variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function GetStateVariable(reader As TriggerReader) As Boolean

            Dim EngineVar = reader.ReadString()
            Dim MS_Var = reader.ReadVariable(True)

            MS_Var.Value = _state.Vars.Item(EngineVar)
            Return True

        End Function

        ''' <summary>
        ''' (5:1503) Set Chat Engine State Vairable {...} to {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function SetStateVariable(reader As TriggerReader) As Boolean

            Dim EngineVar = reader.ReadString()
            Dim EngineValue = reader.ReadString()
            If _state.Vars.ContainsKey(EngineVar) Then
                _state.Vars.Item(EngineVar) = EngineValue
            Else
                _state.Vars.Add(EngineVar, EngineValue)
            End If
            Return True

        End Function

        ''' <summary>
        ''' (5:1500) use knowledgbase file {...} (*.vkb) and start the chat engine.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function UseKkbFile(reader As TriggerReader) As Boolean

            Dim FileName = reader.ReadString
            FileName = Path.Combine(Paths.SilverMonkeyBotPath, FileName)

            Dim xToolbox = New XMLToolbox(GetType(KnowledgeBase))
            kb = DirectCast(xToolbox.LoadXML(FileName), KnowledgeBase)
            kbi.Filename = Path.GetFileName(FileName)
            kbi.Fullpath = Path.GetDirectoryName(FileName) + "\"
            verbot.AddKnowledgeBase(kb, kbi)
            _state.CurrentKBs.Clear()
            _state.CurrentKBs.Add(FileName)

            Return True

        End Function

        Public Overrides Sub Unload(page As Page)

        End Sub

#End Region

    End Class

End Namespace