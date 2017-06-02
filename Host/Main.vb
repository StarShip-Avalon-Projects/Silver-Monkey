Imports System.Collections.Generic
Imports System.IO
Imports System.Text.RegularExpressions
Imports Furcadia.Net
Imports Ionic.Zip
Imports MonkeyCore
Imports SilverMonkeyEngine
Imports SilverMonkeyEngine.Engine

Public Class Main

#Region "Fields"

    Public WithEvents smProxy As NetProxy

#End Region

#Region "Public Fields"

    Public Shared objHost As New smHost

    Public Shared Plugins As List(Of PluginServices.AvailablePlugin)

    Public DREAM As New DREAM

    Public Player As FURRE = New FURRE

#End Region

#Region "Private Fields"

    Private CauseList As Generic.List(Of String) = New List(Of String)

    Private ConditionList As Generic.List(Of String) = New Generic.List(Of String)

    Private EffectList As List(Of String) = New List(Of String)

#End Region

#Region "Public Enums"

    Enum TriggerTypes
        Cause = 0
        Condition = 1
        Effect = 5
    End Enum

#End Region

#Region "Public Methods"

    'Place holder
    Public Sub SendClientMessage(msg As String, data As String)

    End Sub

    'place holder
    Public Sub TextToServer(ByRef arg As String)

    End Sub

#End Region

#Region "Private Methods"

    Private Sub ExportCurrentToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExportCurrentToolStripMenuItem.Click
        Dim lv As ListView = ListView1
        If IsNothing(lv.FocusedItem) Then Exit Sub

        Dim objPlugin As Interfaces.ImsPlugin
        Dim Engine As New MainEngine(New Engine.EngineOptoons, Nothing)
        Dim page As MonkeySpeakPage

        page = CType(Engine.LoadFromString(""), MonkeySpeakPage)
        objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(lv.FocusedItem.Index)), Interfaces.ImsPlugin)
        objPlugin.Initialize(objHost)
        objPlugin.MsPage = page
        objPlugin.Start()
        ExportKeysIni(objPlugin.Name.Replace(" ", "") + ".ini", page)
        Dim path As String = ".\Plugins\"
#If DEBUG Then
        If File.Exists(path + objPlugin.Name.Replace(" ", "") + "_Debug.zip") Then
            File.Delete(path + objPlugin.Name.Replace(" ", "") + "_Debug.zip")
        End If
#End If
        If File.Exists(path + objPlugin.Name.Replace(" ", "") + ".zip") Then
            File.Delete(path + objPlugin.Name.Replace(" ", "") + ".zip")
        End If
        Using zip As ZipFile = New ZipFile
            zip.AddFile(path + objPlugin.Name.Replace(" ", "") + ".ini")
            zip.AddFile(path + objPlugin.Name.Replace(" ", "") + ".dll")
#If DEBUG Then
            zip.Save(path + objPlugin.Name.Replace(" ", "") + "_Debug.zip")
#Else
                zip.Save(path + objPlugin.Name.Replace(" ", "") + ".zip")
#End If
        End Using

    End Sub

    Private Sub ExportKeysIni(ByRef oFile As String, page As Monkeyspeak.Page)
        Directory.CreateDirectory("Plugins")
        oFile = "./plugins/" + oFile
        Dim Test As New List(Of String)
        For Each item As String In page.GetTriggerDescriptions()
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

        Dim w As New StreamWriter(oFile)
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
        w.Close()
    End Sub

    Private Sub ExportToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExportToolStripMenuItem.Click
        LoadPlugins()
    End Sub

    Private Sub frmMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'Plugins = PluginServices.FindPlugins(Paths.ApplicationPluginPath, "Interfaces.imsPlugin")
        'PopulatePluginList()
        'MainMSEngine = New MainEngine
        'MainMSEngine.ScriptStart()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Dim lv As ListView = DirectCast(sender, ListView)
        If IsNothing(lv.FocusedItem) Then Exit Sub
        Dim objPlugin As Interfaces.ImsPlugin
        Dim Engine As New MainEngine(New Engine.EngineOptoons, Nothing)
        Dim page As MonkeySpeakPage
        page = CType(Engine.LoadFromString(""), MonkeySpeakPage)
        objPlugin = CType(PluginServices.CreateInstance(Plugins(lv.FocusedItem.Index)), Interfaces.ImsPlugin)
        If objPlugin Is Nothing Then Exit Sub
        objPlugin.Initialize(objHost)
        objPlugin.MsPage = page
        objPlugin.Start()

        Dim Test As New List(Of String)
        For Each item As String In page.GetTriggerDescriptions()
            Test.Add(item)
        Next
        Dim cat As New Regex("\((.[0-9]*)\:(.[0-9]*)\)")
        EffectList.Clear()
        CauseList.Clear()
        ConditionList.Clear()
        For Each desc As String In Test
            ' print it or write it to file

            Dim Catagory As TriggerTypes = CType(cat.Match(desc).Groups(1).Value.ToString.ToInteger, TriggerTypes)
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
        If CauseList.Count > 0 Then
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
        End If
    End Sub

    Private Sub LoadPlugins()
        Dim objPlugin As Interfaces.ImsPlugin
        Dim intIndex As Integer
        Dim Engine As New Monkeyspeak.MonkeyspeakEngine
        Dim page As MonkeySpeakPage
        Dim path As String = ".\Plugins\"
        For Each f As String In Directory.GetFiles(path, "*.zip")
            File.Delete(f)
        Next
        For intIndex = 0 To Plugins.Count - 1
            page = CType(Engine.LoadFromString(""), MonkeySpeakPage)
            objPlugin = CType(PluginServices.CreateInstance(Plugins(intIndex)), Interfaces.ImsPlugin)

            objPlugin.Initialize(objHost)
            objPlugin.MsPage = page
            objPlugin.Start()
            ExportKeysIni(objPlugin.Name.Replace(" ", "") + ".ini", page)
            Using zip As ZipFile = New ZipFile
                zip.AddFile(path + objPlugin.Name.Replace(" ", "") + ".ini")
                zip.AddFile(path + objPlugin.Name.Replace(" ", "") + ".Plugin.dll")
#If DEBUG Then
                zip.Save(path + objPlugin.Name.Replace(" ", "") + "_Debug" + objPlugin.Version + ".zip")
#Else
                zip.Save(path + objPlugin.Name.Replace(" ", "") + objPlugin.Version + ".zip")
#End If

            End Using

        Next
    End Sub

    Private Sub PopulatePluginList()
        Dim objPlugin As Interfaces.ImsPlugin
        Dim intIndex As Integer

        'Loop through available plugins, creating instances and adding them to listbox
        For intIndex = 0 To Plugins.Count - 1
            Try
                objPlugin = TryCast(PluginServices.CreateInstance(Plugins(intIndex)), Interfaces.ImsPlugin)
                Dim item As ListViewItem = ListView1.Items.Add(intIndex.ToString)
                item.SubItems.Add(objPlugin.Name)
                item.SubItems.Add(objPlugin.Description)
                item.Checked = objPlugin.enabled
            Catch
            End Try
        Next

    End Sub

#End Region

#Region "Public Classes"

    Class CatSorter
        Implements System.Collections.Generic.IComparer(Of String)

#Region "Public Methods"

        Public Function Compare(ByVal item1 As String, ByVal item2 As String) As Integer Implements System.Collections.Generic.IComparer(Of String).Compare

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