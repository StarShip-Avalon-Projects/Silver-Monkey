Imports System.IO
Imports System.Reflection
Imports System.Collections.Generic

Public Class PluginServices

    Public Structure AvailablePlugin
        Public AssemblyPath As String
        Public ClassName As String

        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
                Return False
            End If
            Dim o As AvailablePlugin = CType(obj, AvailablePlugin)
            Return Me.ClassName.Equals(o.ClassName)

        End Function
        Public Overrides Function GetHashCode() As Integer
            Return Me.AssemblyPath.GetHashCode() Xor Me.ClassName.GetHashCode()
        End Function

    End Structure

    Public Shared Function FindPlugins(ByVal strPath As String, ByVal strInterface As String) As AvailablePlugin()
        Dim Plugins As List(Of AvailablePlugin) = New List(Of AvailablePlugin)
        Dim strDLLs() As String, intIndex As Integer
        Dim objDLL As [Assembly]

        'Go through all DLLs in the directory, attempting to load them
        strDLLs = Directory.GetFileSystemEntries(strPath, "*.dll")
        For intIndex = 0 To strDLLs.Length - 1
            Try
                objDLL = [Assembly].LoadFrom(strDLLs(intIndex))
                ExamineAssembly(objDLL, strInterface, Plugins)
            Catch e As Exception
                'Error loading DLL, we don't need to do anything special
            End Try
        Next

        'Return all plugins found
        Dim Results(Plugins.Count - 1) As AvailablePlugin

        If Plugins.Count <> 0 Then
            Plugins.CopyTo(Results)
            Return Results
        Else
            Return Nothing
        End If
    End Function

    Private Shared Sub ExamineAssembly(ByVal objDLL As [Assembly], ByVal strInterface As String, ByVal Plugins As List(Of AvailablePlugin))
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
                        Plugin = New AvailablePlugin()
                        Plugin.AssemblyPath = objDLL.Location
                        Plugin.ClassName = objType.FullName
                        Plugins.Add(Plugin)
                    End If

                End If
            End If
        Next
    End Sub

    Public Shared Function CreateInstance(ByVal Plugin As AvailablePlugin) As SilverMonkey.Interfaces.msPlugin
        Dim objDLL As [Assembly]
        Dim objPlugin As SilverMonkey.Interfaces.msPlugin

        'Load dll
        objDLL = [Assembly].LoadFrom(Plugin.AssemblyPath)
        If objDLL Is Nothing Then Throw New NullReferenceException("""objDLL"" failed to load assembly")

        'Create and return class instance
        objPlugin = DirectCast(objDLL.CreateInstance(Plugin.ClassName), SilverMonkey.Interfaces.msPlugin)


        Return objPlugin
    End Function

End Class
