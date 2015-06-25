Imports Monkeyspeak
Public Class ms


    Dim testScript As String = vbCr & vbLf & "*This is a comment." & vbCr & vbLf & "(0:0) when the script is started," & vbCr & vbLf & vbTab & "(1:0) and %hello.World is not defined," & vbCr & vbLf & vbTab & vbTab & "(5:1) set %hello.World to {Hello World}." & vbCr & vbLf & vbTab & vbTab & "(5:0) print {%hello.World} to the console." & vbCr & vbLf

    Dim engine As New Monkeyspeak.MonkeyspeakEngine()
    Dim page As Monkeyspeak.Page = engine.LoadFromString(testScript)

page.SetTriggerHandler(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Cause, 0), Function() True)

page.SetTriggerHandler(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 0), Function(reader As Monkeyspeak.TriggerReader) Do
    Dim variable = TriggerReader.ReadVariable()
	If variable = Monkeyspeak.Variable.NoValue Then
		Return True
	End If
    ' it is not defined so return true.
	Return False
End Function)

page.SetTriggerHandler(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 0), Function(reader As Monkeyspeak.TriggerReader) Do
    Dim str = TriggerReader.ReadString()
	System.Diagnostics.Debug.WriteLine(str)
	Return True
End Function)

page.SetTriggerHandler(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 1), Function(reader As Monkeyspeak.TriggerReader) Do
    Dim variable = TriggerReader.ReadVariable()
    Dim str = TriggerReader.ReadString()

	variable.AssignValue(str)

	reader.Page.SetVariable(variable)
	Return True
End Function)

page.Execute(Monkeyspeak.TriggerCategory.Cause, 0)



End Class
