Imports Furcadia.Net
Imports System.IO
Imports Ionic.Zip
Imports System.Text.RegularExpressions

'
' Created by SharpDevelop.
' User: Gerolkae
' Date: 5/4/2016
' Time: 1:51 AM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Module Program
	
	Class CatSorter
        Implements System.Collections.Generic.IComparer(Of String)

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

    End Class
	
	Public objHost As New smHost
    Public Player As FURRE = New FURRE
    Public DREAM As New DREAM
    Public WithEvents smProxy As NetProxy

    'Place holder
    Public Sub SendClientMessage(msg As String, data As String)

    End Sub


    'place holder
    Public Sub TextToServer(ByRef arg As String)

    End Sub

    Private EffectList As List(Of String) = New List(Of String)
    Private CauseList As List(Of String) = New List(Of String)
    Private ConditionList As List(Of String) = New List(Of String)
    Enum TriggerTypes
        Cause = 0
        Condition = 1
        Effect = 5
    End Enum
	
	Sub Main()
		Plugins = PluginServices.FindPlugins(Path.GetDirectoryName(Application.ExecutablePath) + "\Plugins\", "SilverMonkey.Interfaces.msPlugin")
        'PopulatePluginList()
        MainMSEngine = New MainEngine
        MainMSEngine.ScriptStart()
        
		Console.WriteLine("Hello World!")
		
		' TODO: Implement Functionality Here
		
		Console.Write("Press any key to continue . . . ")
		Console.ReadKey(True)
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
        ''Test.Sort((New CatSorter))
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

    Private Sub LoadPlugins()
        Dim objPlugin As SilverMonkey.Interfaces.msPlugin
        Dim intIndex As Integer
        Dim Engine As New Monkeyspeak.MonkeyspeakEngine
        Dim page As Monkeyspeak.Page
        Dim path As String = ".\Plugins\"
        For Each f As String In Directory.GetFiles(path, "*.zip")
            File.Delete(f)
        Next
        For intIndex = 0 To Plugins.Length - 1
            page = Engine.LoadFromString("")
            objPlugin = DirectCast(PluginServices.CreateInstance(Plugins(intIndex)), SilverMonkey.Interfaces.msPlugin)
            objPlugin.Initialize(objHost)
            objPlugin.Page = page
            objPlugin.Start()
            ExportKeysIni(objPlugin.Name.Replace(" ", "") + ".ini", page)
            Using zip As ZipFile = New ZipFile
                zip.AddFile(path + objPlugin.Name.Replace(" ", "") + ".ini")
                zip.AddFile(path + objPlugin.Name.Replace(" ", "") + ".dll")
#If DEBUG Then
                zip.Save(path + objPlugin.Name.Replace(" ", "") + "_Debug" + objPlugin.Version + ".zip")
#Else
                zip.Save(path + objPlugin.Name.Replace(" ", "") + objPlugin.Version + ".zip")
#End If

            End Using
        Next
    End Sub
End Module
