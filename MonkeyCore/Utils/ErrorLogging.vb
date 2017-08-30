Imports System.IO
Imports MonkeyCore.Paths
Imports SilverMonkey.BugTraqConnect.Libs
''' <summary>
'''Error Logging Class
'''<para>Author: Tim Wilson</para>
'''<para>Created: Sept 10, 2011</para>
'''<para>Updated and maintained by Gerolkae</para>
'''<para>To Call Class </para>
'''<para>Example of calling Custom Error Logging Code</para>
'''<example>
'''   Try
'''        Throw New Exception("This is an example exception demonstrating the Error
'''Logging Exception Routine") 'Don't require this... this is just manually throwing an error
''' to demo the class, actual code you'd just have the try/catch
'''    Catch ex As Exception
'''        Dim logError As New ErrorLogging(ex, Me) 'Passes the new constructor in the Error Logging Class the exception and 'me' (the calling object)
'''    End Try</example>
'''<para>To provide more details for individual object types create a new constructor by copying and pasting one below and then adjusting the argument. </para>
''' </summary>
'''
Public Class ErrorLogging

#Region "Private Fields"

    Private ReadOnly strErrorFilePath As String
    Public BugReport As ProjectReport

#End Region

#Region "Public Constructors"

    ''' <summary>
    ''' </summary>
    ''' <param name="Ex">
    ''' </param>
    ''' <param name="ObjectThrowingError">
    ''' </param>
    Public Sub New(ByRef Ex As System.Exception, ByVal ObjectThrowingError As Object)
        'Call Log Error
        BugReport = New ProjectReport
        'CHANGE FILEPATH/STRUCTURE HERE TO CHANGE FILE NAME & SAVING LOCATION
        strErrorFilePath = Path.Combine(SilverMonkeyErrorLogPath, My.Application.Info.ProductName & "_Error_" & Date.Now().ToString("MM_dd_yyyy_H-mm-ss") & ".txt")
        LogError(Ex, ObjectThrowingError.ToString())
    End Sub

    ''' <summary>
    ''' </summary>
    ''' <param name="Ex">
    ''' </param>
    ''' <param name="ObjectThrowingError">
    ''' </param>
    ''' <param name="ObJectCheck">
    ''' </param>
    Public Sub New(ByRef Ex As System.Exception, ByRef ObjectThrowingError As Object, ByRef ObJectCheck As Object)
        'Call Log Error
        'CHANGE FILEPATH/STRUCTURE HERE TO CHANGE FILE NAME & SAVING LOCATION
        BugReport = New ProjectReport

        strErrorFilePath = Path.Combine(SilverMonkeyErrorLogPath, My.Application.Info.ProductName & "_Error_" & Date.Now().ToString("MM_dd_yyyy_H-mm-ss") & ".txt")
        LogError(Ex, ObjectThrowingError.ToString(), ObJectCheck.ToString)
    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Fullpath the error document was written to.
    ''' </summary>
    ''' <returns>
    ''' </returns>
    Public ReadOnly Property LogFile As String
        Get
            Return strErrorFilePath
        End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' </summary>
    ''' <param name="ex">
    ''' </param>
    ''' <param name="ObjectThrowingError">
    ''' </param>
    Public Sub LogError(ByRef ex As System.Exception, ByRef ObjectThrowingError As Object)

        BugReport.ProcuctName = My.Application.Info.ProductName
        BugReport.AttachmentFile = strErrorFilePath
        BugReport.ReportSubject = ex.Message
        BugReport.Severity = "crash"


        Using LogFile As System.IO.StreamWriter = New System.IO.StreamWriter(strErrorFilePath, False)
            Try

                '***********************************************************
                '* Error Log Formatting
                '***********************************************************
                LogFile.WriteLine("-------------------------------------------------------")
                LogFile.WriteLine("")
                LogFile.WriteLine(My.Application.Info.ProductName & " Error Report")

                LogFile.WriteLine(My.Application.Info.Version.ToString & " Product Version")

                LogFile.WriteLine(FileVersionInfo.GetVersionInfo(My.Application.Info.AssemblyName + ".exe").ProductVersion & " Informational Version")
                LogFile.WriteLine("")
                LogFile.WriteLine("Date: " & Date.Now().ToString("d"))
                LogFile.WriteLine("")
                LogFile.WriteLine("System Details")
                LogFile.WriteLine("-------------------------------------------------------")
                LogFile.WriteLine("Windows Version: " & My.Computer.Info.OSFullName)
                LogFile.WriteLine("Version Number: " & My.Computer.Info.OSVersion)
                'Determine if 64-bit
                If Environment.Is64BitOperatingSystem Then
                    LogFile.WriteLine("64-Bit OS")
                Else
                    LogFile.WriteLine("32-Bit OS")
                End If

                If Environment.Is64BitProcess = True Then
                    LogFile.WriteLine("64-Bit Process")
                Else
                    LogFile.WriteLine("32-Bit Process")
                End If

                LogFile.WriteLine("")
                LogFile.WriteLine("Program Location: " & My.Application.Info.DirectoryPath)

                LogFile.WriteLine("")
                LogFile.WriteLine("")
                LogFile.WriteLine("Error Details")
                LogFile.WriteLine("-------------------------------------------------------")
                LogFile.WriteLine("Error: " & ex.Message)
                LogFile.WriteLine("")
                If Not ex.InnerException Is Nothing Then
                    LogFile.WriteLine("Inner Error: " & ex.InnerException.Message)
                    LogFile.WriteLine("")
                End If
                LogFile.WriteLine("Source: " & ObjectThrowingError.ToString)
                LogFile.WriteLine("")
                Dim st As New StackTrace(ex, True)
                LogFile.WriteLine("-------------------------------------------------------")

                LogFile.WriteLine("Stack Trace: " & st.ToString())
                LogFile.WriteLine("")
                LogFile.WriteLine("-------------------------------------------------------")
                If Not ex.InnerException Is Nothing Then
                    Dim stInner As New StackTrace(ex.InnerException, True)
                    LogFile.WriteLine("Inner Stack Trace: " & stInner.ToString())
                    LogFile.WriteLine("")
                    LogFile.WriteLine("-------------------------------------------------------")
                End If

                '***********************************************************
            Finally
                If Not IsNothing(LogFile) Then
                    LogFile.Close()
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' </summary>
    ''' <param name="ex">
    ''' </param>
    ''' <param name="ObjectThrowingError">
    ''' </param>
    ''' <param name="ObJectCheck">
    ''' </param>
    Public Sub LogError(ByRef ex As System.Exception, ByRef ObjectThrowingError As Object, ByRef ObJectCheck As Object)
        'CHANGE FILEPATH/STRUCTURE HERE TO CHANGE FILE NAME & SAVING LOCATION

        BugReport.ProcuctName = My.Application.Info.ProductName
        BugReport.AttachmentFile = strErrorFilePath
        BugReport.ReportSubject = ex.Message
        BugReport.Severity = "crash"

        Using LogFile As StreamWriter = New StreamWriter(strErrorFilePath, False)
            Try

                '***********************************************************
                '* Error Log Formatting
                '***********************************************************
                LogFile.WriteLine("-------------------------------------------------------")
                LogFile.WriteLine("")
                LogFile.WriteLine(My.Application.Info.ProductName & " Error Report")
                LogFile.WriteLine(My.Application.Info.Version.ToString & " Product Version")
                LogFile.WriteLine(FileVersionInfo.GetVersionInfo(My.Application.Info.AssemblyName + ".exe").ProductVersion & " Informational Version")
                LogFile.WriteLine("")
                LogFile.WriteLine("Date: " & Date.Now().ToString("d"))
                LogFile.WriteLine("")
                LogFile.WriteLine("System Details")
                LogFile.WriteLine("-------------------------------------------------------")
                LogFile.WriteLine("Windows Version: " & My.Computer.Info.OSFullName)
                LogFile.WriteLine("Version Number: " & My.Computer.Info.OSVersion)
                'Determine if 64-bit
                If Environment.Is64BitOperatingSystem Then
                    LogFile.WriteLine("64-Bit OS")
                Else
                    LogFile.WriteLine("32-Bit OS")
                End If
                If Environment.Is64BitProcess = True Then
                    LogFile.WriteLine("64-Bit Process")
                Else
                    LogFile.WriteLine("32-Bit Process")
                End If

                LogFile.WriteLine("")
                LogFile.WriteLine("Program Location: " & My.Application.Info.DirectoryPath)

                LogFile.WriteLine("")
                LogFile.WriteLine("")
                LogFile.WriteLine("Error Details")
                LogFile.WriteLine("-------------------------------------------------------")
                LogFile.WriteLine("Error: " & ex.Message)
                LogFile.WriteLine("")
                If Not IsNothing(ex.InnerException) Then
                    LogFile.WriteLine("Inner Error: " & ex.InnerException.Message)
                    LogFile.WriteLine("")
                End If
                LogFile.WriteLine("Source: " & ObjectThrowingError.ToString)
                LogFile.WriteLine("")
                LogFile.WriteLine("Object Check: " & ObJectCheck.ToString)
                LogFile.WriteLine("")
                Dim st As New StackTrace(ex, True)
                LogFile.WriteLine("Stack Frames: ")
                For Each Frame As StackFrame In st.GetFrames()
                    LogFile.WriteLine("Line:" + Frame.GetFileLineNumber().ToString + Frame.GetFileName().ToString, Frame.GetMethod().ToString)
                Next
                LogFile.WriteLine("-------------------------------------------------------")

                LogFile.WriteLine("Stack Trace: " & st.ToString())
                LogFile.WriteLine("")
                LogFile.WriteLine("-------------------------------------------------------")
                If Not ex.InnerException Is Nothing Then
                    Dim stInner As New StackTrace(ex.InnerException, True)
                    LogFile.WriteLine("Inner Stack Trace: " & stInner.ToString())
                    LogFile.WriteLine("")
                    LogFile.WriteLine("-------------------------------------------------------")
                End If

                '***********************************************************
            Finally
                If Not IsNothing(LogFile) Then
                    LogFile.Close()
                End If
            End Try
        End Using
    End Sub

#End Region

End Class