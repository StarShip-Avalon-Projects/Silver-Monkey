Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports SilverMonkeyEngine
Imports SilverMonkeyEngine.Engine

Public Class MS_Export

#Region "Private Fields"

    Private CauseList As List(Of String) = New List(Of String)

    Private ConditionList As List(Of String) = New List(Of String)

    Private EffectList As List(Of String) = New List(Of String)
    Private LoopsList As List(Of String) = New List(Of String)

#End Region

#Region "Public Enums"

    Enum TriggerTypes
        Cause = 0
        Condition = 1
        Effect = 5
        Loops = 6
    End Enum

#End Region

    Private Shared MsPage As Monkeyspeak.Page

    Private Session As BotSession

#Region "Private Methods"

    Public Sub New()
        ' This call is required by the designer.

        Dim options As New BotOptions()
        Session = New BotSession(options)
        Dim engine As New MainEngine(options.MonkeySpeakEngineOptions, Session)
        Session.MSpage = engine.LoadFromString(String.Empty)

        'Load the Monkeyspeak lins into the page
        MsPage = Session.LoadLibrary(True)

        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

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
        Test.Sort((New CatSorter))
        Dim cat As New Regex("\((.[0-9]*)\:(.[0-9]*)\)")
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
        CauseList.Sort((New CatSorter))
        ConditionList.Sort((New CatSorter))
        EffectList.Sort((New CatSorter))
        LoopsList.Sort((New CatSorter))
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
        If EffectList.Count > 0 Then
            w.WriteLine("[Loops]")
            For i As Integer = 0 To LoopsList.Count - 1
                Dim Catagory As String = cat.Match(LoopsList(i)).Groups(0).Value.ToString
                w.WriteLine(Catagory + "=0,0,""" + LoopsList(i).Replace("""", """""") + """")
            Next
        End If
        w.WriteLine("")
        w.WriteLine("")
        w.Close()
    End Sub

    Private Sub IniToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles IniToolStripMenuItem.Click
        ExportKeysIni(Path.Combine(Application.StartupPath, "out.ini"))
    End Sub

    Private Sub MS_Export_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim Test As New List(Of String)
        For Each item As String In MsPage.GetTriggerDescriptions()
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