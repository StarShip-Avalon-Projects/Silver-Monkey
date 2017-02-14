Imports System.Reflection
Imports System.IO
Imports System.Windows.Forms

Public Class PluginServices

    Public Class AvailablePlugin
        Public ReadOnly Property AssemblyPath As String
            Get
                Return Library.Location
            End Get
        End Property
        Public Property ClassName As String
        Public Property Library As [Assembly]
        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing OrElse Not obj.GetType().Equals(GetType(AvailablePlugin)) Then
                Return False
            End If
            Dim var As AvailablePlugin = DirectCast(obj, AvailablePlugin)
            Return ClassName = var.ClassName
            Return False
        End Function
        Public Overrides Function GetHashCode() As Integer
            Return ClassName.GetHashCode()
        End Function
    End Class
    Public Shared Plugins As New List(Of AvailablePlugin)

    Public Shared Function FindPlugins(ByVal strPath As String, ByVal strInterface As String) As List(Of AvailablePlugin)
        Dim strDLLs() As String, intIndex As Integer
        Dim objDLL As [Assembly] = Nothing

        'Go through all DLLs in the directory, attempting to load them
        strDLLs = Directory.GetFileSystemEntries(strPath, "*.Plugin.dll", SearchOption.TopDirectoryOnly)
        For intIndex = 0 To strDLLs.Length - 1
            Try
                objDLL = [Assembly].LoadFrom(strDLLs(intIndex))
                ExamineAssembly(objDLL, strInterface, Plugins)
            Catch e As Exception
                MessageBox.Show("there was a problem" + objDLL.ToString + e.Message)
                'Error loading DLL, we don't need to do anything special
            End Try
        Next

        'Return all plugins found

        If Plugins.Count > 0 Then
            Return Plugins
        Else
            Return Nothing
        End If
    End Function

    Private Shared Sub ExamineAssembly(ByVal objDLL As [Assembly], ByVal strInterface As String, ByRef Plugins As List(Of AvailablePlugin))
        Dim objType As Type
        Dim objInterface As Type
        Dim Plugin As AvailablePlugin

        'Loop through each type in the DLL
        For Each objType In objDLL.GetTypes
            'Only look at public types
            If objType.IsPublic = True Then
                'Ignore abstract classes
                If Not ((objType.Attributes And TypeAttributes.Abstract) = TypeAttributes.Abstract) Then

                    'See if this type implements our interface
                    objInterface = objType.GetInterface(strInterface, True)

                    If Not (objInterface Is Nothing) Then
                        'It does
                        Try
                            Plugin = New AvailablePlugin()
                            Plugin.ClassName = objType.FullName
                            Plugin.Library = objDLL
                            Debug.Print("Loading: " + Plugin.ClassName)
                            Plugins.Add(Plugin)
                        Catch e As Exception

                        End Try
                    End If

                End If
            End If
        Next
    End Sub

    Public Shared Function CreateInstance(ByVal Plugin As AvailablePlugin) As Object
        Dim objDLL As [Assembly]
        Dim objPlugin As Object

        Try
            'Load dll
            objDLL = [Assembly].LoadFrom(Plugin.AssemblyPath)

            'Create and return class instance
            objPlugin = objDLL.CreateInstance(Plugin.ClassName)
        Catch e As Exception
            Return Nothing
        End Try

        Return objPlugin
    End Function

End Class