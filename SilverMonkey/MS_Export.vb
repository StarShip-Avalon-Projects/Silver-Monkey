Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports Monkeyspeak

Public Class MS_Export

#Region "Private Fields"

    Private CauseList As List(Of String) = New List(Of String)

    Private ConditionList As List(Of String) = New List(Of String)

    Private EffectList As List(Of String) = New List(Of String)

#End Region

#Region "Public Enums"

    Enum TriggerTypes
        Cause = 0
        Condition = 1
        Effect = 5
    End Enum

#End Region

#Region "Private Methods"

#End Region

#Region "Public Classes"

#End Region

    Private FurcadiaSession As FurcSession = Main.FurcadiaSession

#Region "Private Methods"

    Private Sub ExitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub ExportKeysIni(ByRef oFile As String)
        'MS_Stared = 0
        'MainMSEngine.EngineStart(False)
        Dim Test As New List(Of String)
        For Each item As String In FurcadiaSession.MainEngine.MSpage.GetTriggerDescriptions()
            Test.Add(item)
        Next
        EffectList.Clear()
        CauseList.Clear()
        ConditionList.Clear()
        Test.Sort((New CatSorter))
        Dim cat As New Regex("\((.[0-9]*)\:(.[0-9]*)\)")
        For Each desc As String In Test
            ' print it or write it to file

            Dim Catagory As TriggerTypes = CType(cat.Match(desc).Groups(1).ToString, TriggerTypes)
            Select Case Catagory
                Case TriggerTypes.Cause
                    CauseList.Add(desc)
                Case TriggerTypes.Condition
                    ConditionList.Add(desc)
                Case TriggerTypes.Effect
                    EffectList.Add(desc)
                Case Else
                    Console.Write("Catagory error " & Catagory.ToString)
            End Select
        Next
        CauseList.Sort((New CatSorter))
        ConditionList.Sort((New CatSorter))
        EffectList.Sort((New CatSorter))
        Dim w As New StreamWriter(oFile, False)
        If CauseList.Count > 0 Then
            w.WriteLine("[Causes]")
            For i As Integer = 0 To CauseList.Count - 1
                Dim Catagory As String = cat.Match(CauseList(i)).Groups(0).Value.ToString
                w.WriteLine(Catagory + "=0,0,""" + CauseList(i).Replace("""", """""") + """")
            Next
            w.WriteLine("")
            w.WriteLine("")
        End If
        If ConditionList.Count > 0 Then
            w.WriteLine("[Additional Conditions]")
            For i As Integer = 0 To ConditionList.Count - 1
                Dim Catagory As String = cat.Match(ConditionList(i)).Groups(0).Value.ToString
                w.WriteLine(Catagory + "=0,0,""" + ConditionList(i).Replace("""", """""") + """")
            Next
            w.WriteLine("")
            w.WriteLine("")
        End If
        If EffectList.Count > 0 Then
            w.WriteLine("[Effects]")
            For i As Integer = 0 To EffectList.Count - 1
                Dim Catagory As String = cat.Match(EffectList(i)).Groups(0).Value.ToString
                w.WriteLine(Catagory + "=0,0,""" + EffectList(i).Replace("""", """""") + """")
            Next
        End If
        w.WriteLine("")
        w.WriteLine("")
        w.Close()
    End Sub

    Private Sub IniToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles IniToolStripMenuItem.Click
        ExportKeysIni("out.ini")
    End Sub

    Private Sub MS_Export_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        MainMsEngine.MS_Stared = 0
        FurcadiaSession.MainEngine.EngineStart(False)
        Dim Test As New List(Of String)
        For Each item As String In FurcadiaSession.MainEngine.MSpage.GetTriggerDescriptions()
            Test.Add(item)
        Next
        Dim cat As New Regex("\((.[0-9]*)\:(.[0-9]*)\)")
        EffectList.Clear()
        CauseList.Clear()
        ConditionList.Clear()
        For Each desc As String In Test
            ' print it or write it to file
            Dim num As Integer = 0
            Integer.TryParse(cat.Match(desc).Groups(1).Value, num)
            Dim Catagory As TriggerTypes = CType(num, TriggerTypes)
            Select Case Catagory
                Case TriggerTypes.Cause
                    CauseList.Add(desc)
                Case TriggerTypes.Condition
                    ConditionList.Add(desc)
                Case TriggerTypes.Effect
                    EffectList.Add(desc)
                Case Else
                    Console.Write("Catagory error " & Catagory.ToString)
            End Select
        Next
        CauseList.Sort((New CatSorter))
        ConditionList.Sort((New CatSorter))
        EffectList.Sort((New CatSorter))

        TextBox1.Text = ""
        If ConditionList.Count > 0 Then
            TextBox1.AppendText("[Causes]" + Environment.NewLine)
            For i As Integer = 0 To CauseList.Count - 1
                TextBox1.AppendText(CauseList(i) + Environment.NewLine)
            Next
            TextBox1.AppendText(Environment.NewLine)
            TextBox1.AppendText(Environment.NewLine)
        End If
        If ConditionList.Count > 0 Then
            TextBox1.AppendText("[Additional Conditions]" + Environment.NewLine)
            For i As Integer = 0 To ConditionList.Count - 1
                TextBox1.AppendText(ConditionList(i) + Environment.NewLine)
            Next
            TextBox1.AppendText(Environment.NewLine)
            TextBox1.AppendText(Environment.NewLine)
        End If
        If EffectList.Count > 0 Then
            TextBox1.AppendText("[Effects]" + Environment.NewLine)
            For i As Integer = 0 To EffectList.Count - 1
                TextBox1.AppendText(EffectList(i) + Environment.NewLine)
            Next
            TextBox1.AppendText(Environment.NewLine)
            TextBox1.AppendText(Environment.NewLine)
        End If
    End Sub

#End Region

#Region "Public Classes"

    Class CatSorter
        Implements IComparer(Of String)

#Region "Public Methods"

        Public Function Compare(ByVal item1 As String, ByVal item2 As String) As Integer Implements IComparer(Of String).Compare

            Dim cat As New Regex("\((.[0-9]*?)\:(.[0-9]*?)\)")
            Dim num1 As Integer = 0
            Integer.TryParse(cat.Match(item1).Groups(1).ToString, num1)
            Dim num2 As Integer = 0
            Integer.TryParse(cat.Match(item2).Groups(1).ToString, num2)
            Dim num3 As Integer = 0
            Integer.TryParse(cat.Match(item1).Groups(2).ToString, num3)
            Dim num4 As Integer = 0
            Integer.TryParse(cat.Match(item2).Groups(2).ToString, num4)

            If num3 > num4 Then
                If num1 > num2 Then Return 1
                If num1 < num2 Then Return -1
                Return 1
            ElseIf num3 < num4 Then
                If num1 > num2 Then Return 1
                If num1 < num2 Then Return -1
                Return -1
            Else
                If num1 > num2 Then Return 1
                If num1 < num2 Then Return -1
                Return 0
            End If
        End Function

#End Region

    End Class

#End Region

End Class