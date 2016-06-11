Imports Monkeyspeak
Imports SilverMonkey.ErrorLogging
Imports SilverMonkey.TextBoxWriter
Imports System.Reflection
Imports System.IO
Imports Conversive.Verbot5

Imports System.Diagnostics
Imports System.Collections
Imports System.Collections.Generic

Public Class MS_Verbot
	Inherits Monkeyspeak.Libraries.AbstractBaseLibrary
    Private writer As TextBoxWriter = Nothing
	Private ChatCMD As String

    Public Sub New()
    	writer = New TextBoxWriter(Variables.TextBox1)
    	Main.verbot = New Verbot5Engine()
        Main.state = New State()
    	
 '(0:1500) When the chat engine executes command {...},
    Add(Monkeyspeak.TriggerCategory.Cause, 1500,
AddressOf ChatExecute, "(0:1500) When the chat engine executes command {...},")	
    	
'(1:1500) and the Chat Engine State variable {...} is equal to {..},
'(1:1501) and the Chat Engine State variable {...} is not equal to {..},
'(1:1502) and the Chat Engine State variable {...} is equal to #, 
'(1:1503) and the Chat Engine State variable {...} is not equal to #, 
'(1:1504) and the Chat Engine State variable {...} is greater than #,
'(1:1505) and the Chat Engine State variable {...} is greater than or equal to #,
'(1:1506) and the Chat Engine State variable {...} is less than #,
'(1:1507) And the Chat Engine State variable {...} Is less than Or equal To #,


'(5:1500) use knowledgbase file {...} (*.vkb) and start the chat engine.
    Add(Monkeyspeak.TriggerCategory.Effect, 1500,
AddressOf useKB_File, "(5:1500) use knowledgbase file {...} (*.vkb) and start the chat engine.")	

 

'(5:1501) send text {...} to chat engine and put the response in variable %Variable.
    Add(Monkeyspeak.TriggerCategory.Effect, 1501,
AddressOf getReply, "(5:1501) send text {...} to chat engine and put the response in variable %Variable.")	

'(5:1502) send text {...} and Name {...} to chat engine and put the response in variable %Variable
    Add(Monkeyspeak.TriggerCategory.Effect, 1502,
AddressOf getReplyName, "(5:1502) send text {...} and Name {...} to chat engine and put the response in variable %Variable.")	

'(5:1503) Set Chat Engine State Vairable {...} to {...}.
    Add(Monkeyspeak.TriggerCategory.Effect, 1503,
AddressOf setStateVariable, "(5:1503) Set Chat Engine State Vairable {...} to {...}.")	

'(5:1504) Get chat engine state variable {...} and put it into variable %Variable.
    Add(Monkeyspeak.TriggerCategory.Effect, 1504,
AddressOf getStateVariable, "(5:1504) Get chat engine state variable {...} and put it into variable %Variable.")	
    	
   	End sub
   	
   	#Region "Chat interface"
   	
   	'(5:1501) send text {...} to chat engine and put the response in variable %Variable
   	Function getReply(reader As TriggerReader) As boolean
   		Dim SayText As String
   		Dim ResponceText As Monkeyspeak.Variable
   		Try
   			SayText= reader.ReadString
   			ResponceText = reader.ReadVariable(True)
        	If callbk.state.Vars.ContainsKey("botname") Then
            	callbk.state.Vars.Item("botname") = callbk.BotName
        	Else
            	callbk.state.Vars.Add("botname", callbk.BotName)
        	End If
        	If callbk.state.Vars.ContainsKey("channel") Then
            	callbk.state.Vars.Item("channel") = callbk.Channel
        	Else
            	callbk.state.Vars.Add("channel", callbk.Channel)
        	End If
        	Dim reply As Reply = callbk.verbot.GetReply(callbk.Player, SayText, callbk.state)
        	
        	If reply Is Nothing Then Return false
        	
        	ResponceText.Value = reply.Text
       	    Me.parseEmbeddedOutputCommands(reply.AgentText)
       	    Return true
       	Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
    End Function
    
    '(5:1502) send text {...} and Name {...} to chat engine and put the response in variable %Variable
   	Function getReplyName(reader As TriggerReader) As boolean
   		Dim SayText As String
   		Dim SayName As String 
   		Dim ResponceText As Monkeyspeak.Variable
   		Try
   			SayText= reader.ReadString
   			SayName= reader.ReadString
   			ResponceText = reader.ReadVariable(True)
        	If callbk.state.Vars.ContainsKey("botname") Then
            	callbk.state.Vars.Item("botname") = callbk.BotName
        	Else
            	callbk.state.Vars.Add("botname", callbk.BotName)
        	End If
        	If callbk.state.Vars.ContainsKey("channel") Then
            	callbk.state.Vars.Item("channel") = callbk.Channel
        	Else
            	callbk.state.Vars.Add("channel", callbk.Channel)
        	End If
        	Dim reply As Reply = callbk.verbot.GetReply(callbk.NametoFurre(SayName, False), SayText, callbk.state)
        	
        	If reply Is Nothing Then Return false
        	
        	ResponceText.Value = reply.Text
       	    Me.parseEmbeddedOutputCommands(reply.AgentText)
       	    Return true
       	Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
    End Function
    
    Private Sub parseEmbeddedOutputCommands(text As String)
        Dim startCommand As String = "<"
        Dim endCommand As String = ">"

        Dim start As Integer = text.IndexOf(startCommand)
        Dim [end] As Integer = -1

        While start <> -1
            [end] = text.IndexOf(endCommand, start)
            If [end] <> -1 Then
                Dim command As String = text.Substring(start + 1, [end] - start - 1).Trim()
                If command <> "" Then
                    Me.runEmbeddedOutputCommand(command)
                End If
            End If
            start = text.IndexOf(startCommand, start + 1)
        End While
    End Sub
    'parseEmbeddedOutputCommands(string text)
    Private Sub runEmbeddedOutputCommand(command As String)
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
        	Select Case [Function]
        		Case "setmsvariable"
        			Dim VarIndex As Integer = args.IndexOf(" ")
        			Dim VarDataIndex As integer = args.IndexOf("=")
        			Dim VarName As String = nothing
        			Dim VarData As String = nothing
        			        
        			If VarIndex <> -1 Then
        				VarName = args.Substring(0, VarIndex)
        				 If VarDataIndex <> -1 Then
        				 	VarData = args.Substring(VarDataIndex + 1)
        				 End If
        				 MainMsEngine.PageSetVariable(VarName, VarData)
        			End If
        			                			
        			'ChatExecute
        		Case "executechatcmd"
        			ChatCMD = args
        			MainMSEngine.PageExecute(1500)

                Case Else
                    Exit Select
                    'switch
            End Select
        Catch
        End Try
    End Sub
   	
	#End Region
   	
   	'(0:1500) When the chat engine executes command {...},
   	Function ChatExecute(reader As TriggerReader) As Boolean
   		
   		Try
   			Dim cmd As String = reader.ReadString()
   			Return ChatCMD.ToLower() = cmd.ToLower()
   		Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
   	End Function
   	
   	'(5:1500) use knowledgbase file {...} (*.vkb) and start the chat engine.
   	Function useKB_File (reader As TriggerReader) As Boolean
   		
   		Try
   			Dim FileName As String = reader.ReadString
   			FileName = CheckMyDocFile(FileName)
   			
   			Dim xToolbox As XMLToolbox = New XMLToolbox(GetType(KnowledgeBase))
            callbk.kb = CType(xToolbox.LoadXML(FileName),KnowledgeBase)
            callbk.kbi.Filename = Path.GetFileName(FileName)
            callbk.kbi.Fullpath = Path.GetDirectoryName(FileName) + "\"
            callbk.verbot.AddKnowledgeBase(callbk.kb, callbk.kbi)
            callbk.state.CurrentKBs.Clear()
            callbk.state.CurrentKBs.Add(FileName)
            
            Return True
   			
   		Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
   	End Function
   	
   	
   	'(5:1503) Set Chat Engine State Vairable {...} to {...}.
   	Function setStateVariable(reader As TriggerReader) As Boolean
   		
   		Try
   			
   			Dim EngineVar As String = reader.ReadString()
   			Dim EngineValue As String = reader.ReadString()
   			If callbk.state.Vars.ContainsKey(EngineVar) Then
            	callbk.state.Vars.Item(EngineVar) = EngineValue
        	Else
            	callbk.state.Vars.Add(EngineVar, EngineValue)
        	End If
        	Return True
   		Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
   	End Function
   	
'(5:1504) Get chat engine state variable {...} and put it into variable %Variable.
	Function getStateVariable(reader As TriggerReader)As Boolean
		Try
			Dim EngineVar As String = reader.readstring()
			Dim MS_Var As Variable = reader.ReadVariable(True)
			
			MS_Var.Value = callbk.state.Vars.Item(EngineVar)
			Return true
		

   		Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
	End Function   		
End Class
