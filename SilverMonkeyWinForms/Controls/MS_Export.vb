Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports BotSession
Imports Monkeyspeak

Public Class MS_Export

#Region "Private Fields"

    Private Const LineIDRegex As String = "\((.[0-9]*)\:(.[0-9]*)\)"
    Private CauseList As List(Of String)

    Private ConditionList As List(Of String)

    Private EffectList As List(Of String)
    Private LoopsList As List(Of String)

#End Region

#Region "Public Enums"

    Enum TriggerTypes As Byte
        Cause = 0
        Condition
        Effect = 5
        Loops
    End Enum

#End Region

    Private Shared MsPage As Page

#Region "Private Methods"

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        CauseList = New List(Of String)
        ConditionList = New List(Of String)
        EffectList = New List(Of String)
        LoopsList = New List(Of String)

        Dim options As New BotOptions()
        Dim engine As New MonkeyspeakEngine(options.MonkeySpeakEngineOptions)
        MsPage = engine.LoadFromString("* dummy")
        MsPage.LoadAllLibraries()

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub ExportKeysIni(ByRef oFile As String)
        Dim Test As New List(Of String)
        Dim TriggerDescs = MsPage.GetTriggerDescriptions(True)
        Dim InputList As IEnumerator(Of String) = TriggerDescs.GetEnumerator()
        While InputList.MoveNext
            Test.AddRange(InputList.Current.Replace(vbCr, String.Empty).Split(CType(vbLf, Char())))
        End While

        EffectList.Clear()
        CauseList.Clear()
        ConditionList.Clear()
        LoopsList.Clear()
        Test.Sort(New CatSorter)

        Dim cat As New Regex(LineIDRegex)
        For Each desc As String In Test
            ' print it or write it to file

            Dim Catagory As TriggerTypes
            If cat.Match(desc).Success Then
                Catagory = CType(cat.Match(desc).Groups(1).Value, TriggerTypes)
                Select Case Catagory
                    Case TriggerTypes.Cause
                        CauseList.Add(desc)
                    Case TriggerTypes.Condition
                        ConditionList.Add(desc)
                    Case TriggerTypes.Effect
                        EffectList.Add(desc)
                    Case TriggerTypes.Loops
                        LoopsList.Add(desc)
                    Case Else
                        Console.Write("Catagory error " & Catagory.ToString)
                End Select
            End If
        Next
        CauseList.Sort(New CatSorter)
        ConditionList.Sort(New CatSorter)
        EffectList.Sort(New CatSorter)
        LoopsList.Sort(New CatSorter)
        Dim LineList As New StringBuilder()
        If CauseList.Count > 0 Then
            LineList.AppendLine("[Causes]")
            For i As Integer = 0 To CauseList.Count - 1
                Dim Catagory As String = cat.Match(CauseList(i)).Groups(0).Value.ToString
                LineList.AppendLine(Catagory + "=0,0,""" + CauseList(i).Replace("""", """""") + """")
            Next
            LineList.AppendLine("")
            LineList.AppendLine("")
        End If
        If ConditionList.Count > 0 Then
            LineList.AppendLine("[Additional Conditions]")
            For i As Integer = 0 To ConditionList.Count - 1
                Dim Catagory As String = cat.Match(ConditionList(i)).Groups(0).Value.ToString
                LineList.AppendLine(Catagory + "=0,0,""" + ConditionList(i).Replace("""", """""") + """")
            Next
            LineList.AppendLine("")
            LineList.AppendLine("")
        End If
        If EffectList.Count > 0 Then
            LineList.AppendLine("[Effects]")
            For i As Integer = 0 To EffectList.Count - 1
                Dim Catagory As String = cat.Match(EffectList(i)).Groups(0).Value.ToString
                LineList.AppendLine(Catagory + "=0,0,""" + EffectList(i).Replace("""", """""") + """")
            Next
        End If
        LineList.AppendLine("")
        LineList.AppendLine("")
        If EffectList.Count > 0 Then
            LineList.AppendLine("[Loops]")
            For i As Integer = 0 To LoopsList.Count - 1
                Dim Catagory As String = cat.Match(LoopsList(i)).Groups(0).Value.ToString
                LineList.AppendLine(Catagory + "=0,0,""" + LoopsList(i).Replace("""", """""") + """")
            Next
        End If
        LineList.AppendLine("")
        LineList.AppendLine("")
        Try
            File.WriteAllText(oFile, LineList.ToString)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub IniToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles IniToolStripMenuItem.Click
        ExportKeysIni(Path.Combine(Application.StartupPath, "out.ini"))
    End Sub

    Private Sub MS_Export_Load(sender As Object, e As System.EventArgs) Handles Me.Load

#If DEBUG Then
        If Not Debugger.IsAttached Then
            IniToolStripMenuItem.Enabled = False

        End If
#Else
        IniToolStripMenuItem.Enabled = false
#End If

        Dim Test As New List(Of String)
        For Each item As String In MsPage.GetTriggerDescriptions()
            Test.Add(item)
        Next
        Dim cat As New Regex(LineIDRegex)
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
        CauseList.Sort(New CatSorter)
        ConditionList.Sort(New CatSorter)
        EffectList.Sort(New CatSorter)

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
        If LoopsList.Count > 0 Then
            TextBox1.AppendText("[Loops]" + Environment.NewLine)
            For i As Integer = 0 To LoopsList.Count - 1
                TextBox1.AppendText(LoopsList(i) + Environment.NewLine)
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

            Dim cat As New Regex(LineIDRegex)
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