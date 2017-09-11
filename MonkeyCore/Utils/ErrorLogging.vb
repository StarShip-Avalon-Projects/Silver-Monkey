﻿Imports System.IO
Imports MonkeyCore.Paths

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

#End Region

#Region "Public Constructors"

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="Ex"></param>
    ''' <param name="ObjectThrowingError"></param>
    Public Sub New(ByRef Ex As System.Exception, ByVal ObjectThrowingError As Object)
        'Call Log Error

        'CHANGE FILEPATH/STRUCTURE HERE TO CHANGE FILE NAME & SAVING LOCATION
        strErrorFilePath = Path.Combine(SilverMonkeyErrorLogPath, My.Application.Info.ProductName & "_Error_" & Date.Now().ToString("MM_dd_yyyy_H-mm-ss") & ".txt")
        LogError(Ex, ObjectThrowingError.ToString())
    End Sub

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="Ex"></param>
    ''' <param name="ObjectThrowingError"></param>
    ''' <param name="ObJectCheck"></param>
    Public Sub New(ByRef Ex As System.Exception, ByRef ObjectThrowingError As Object, ByRef ObJectCheck As Object)
        'Call Log Error
        'CHANGE FILEPATH/STRUCTURE HERE TO CHANGE FILE NAME & SAVING LOCATION
        strErrorFilePath = Path.Combine(SilverMonkeyErrorLogPath, My.Application.Info.ProductName & "_Error_" & Date.Now().ToString("MM_dd_yyyy_H-mm-ss") & ".txt")
        LogError(Ex, ObjectThrowingError.ToString(), ObJectCheck.ToString)
    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Fullpath the error document was written to.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property LogFile As String
        Get
            Return strErrorFilePath
        End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="ex"></param>
    ''' <param name="ObjectThrowingError"></param>
    Public Sub LogError(ByRef ex As System.Exception, ByRef ObjectThrowingError As Object)

        Dim ioFile As System.IO.StreamWriter = Nothing
        Try
            ioFile = New System.IO.StreamWriter(strErrorFilePath, False)

            '***********************************************************
            '* Error Log Formatting
            '***********************************************************
            ioFile.WriteLine("-------------------------------------------------------")
                ioFile.WriteLine("")
                ioFile.WriteLine(My.Application.Info.ProductName & " Error Report")
                ioFile.WriteLine(My.Application.Info.Version.ToString & " Product Version")
                ioFile.WriteLine("")
                ioFile.WriteLine("Date: " & Date.Now().ToString("d"))
                ioFile.WriteLine("")
                ioFile.WriteLine("System Details")
                ioFile.WriteLine("-------------------------------------------------------")
                ioFile.WriteLine("Windows Version: " & My.Computer.Info.OSFullName)
                ioFile.WriteLine("Version Number: " & My.Computer.Info.OSVersion)
                'Determine if 64-bit
                If Environment.Is64BitOperatingSystem Then
                    ioFile.WriteLine("64-Bit OS")
                Else
                    ioFile.WriteLine("32-Bit OS")
                End If

                If Environment.Is64BitProcess = True Then
                    ioFile.WriteLine("64-Bit Process")
                Else
                    ioFile.WriteLine("32-Bit Process")
                End If

                ioFile.WriteLine("")
                ioFile.WriteLine("Program Location: " & My.Application.Info.DirectoryPath)

                ioFile.WriteLine("")
                ioFile.WriteLine("")
                ioFile.WriteLine("Error Details")
                ioFile.WriteLine("-------------------------------------------------------")
                ioFile.WriteLine("Error: " & ex.Message)
                ioFile.WriteLine("")
                If Not ex.InnerException Is Nothing Then
                    ioFile.WriteLine("Inner Error: " & ex.InnerException.Message)
                    ioFile.WriteLine("")
                End If
                ioFile.WriteLine("Source: " & ObjectThrowingError.ToString)
                ioFile.WriteLine("")
                Dim st As New StackTrace(ex, True)
                ioFile.WriteLine("-------------------------------------------------------")

                ioFile.WriteLine("Stack Trace: " & st.ToString())
                ioFile.WriteLine("")
                ioFile.WriteLine("-------------------------------------------------------")
                If Not ex.InnerException Is Nothing Then
                    Dim stInner As New StackTrace(ex.InnerException, True)
                    ioFile.WriteLine("Inner Stack Trace: " & stInner.ToString())
                    ioFile.WriteLine("")
                    ioFile.WriteLine("-------------------------------------------------------")
                End If

                '***********************************************************

            Finally
                If Not IsNothing(ioFile) Then
                    ioFile.Close()
                End If
            End Try

    End Sub
    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="ex"></param>
    ''' <param name="ObjectThrowingError"></param>
    ''' <param name="ObJectCheck"></param>
    Public Sub LogError(ByRef ex As System.Exception, ByRef ObjectThrowingError As Object, ByRef ObJectCheck As Object)
        'CHANGE FILEPATH/STRUCTURE HERE TO CHANGE FILE NAME & SAVING LOCATION
        Dim ioFile As StreamWriter = Nothing
        Try

            ioFile = New StreamWriter(strErrorFilePath, False)

            '***********************************************************
            '* Error Log Formatting
            '***********************************************************
            ioFile.WriteLine("-------------------------------------------------------")
            ioFile.WriteLine("")
            ioFile.WriteLine(My.Application.Info.ProductName & " Error Report")
            ioFile.WriteLine("")
            ioFile.WriteLine("Date: " & Date.Now().ToString("d"))
            ioFile.WriteLine("")
            ioFile.WriteLine("System Details")
            ioFile.WriteLine("-------------------------------------------------------")
            ioFile.WriteLine("Windows Version: " & My.Computer.Info.OSFullName)
            ioFile.WriteLine("Version Number: " & My.Computer.Info.OSVersion)
            'Determine if 64-bit
            If Environment.Is64BitOperatingSystem Then
                ioFile.WriteLine("64-Bit OS")
            Else
                ioFile.WriteLine("32-Bit OS")
            End If
            If Environment.Is64BitProcess = True Then
                ioFile.WriteLine("64-Bit Process")
            Else
                ioFile.WriteLine("32-Bit Process")
            End If

            ioFile.WriteLine("")
            ioFile.WriteLine("Program Location: " & My.Application.Info.DirectoryPath)

            ioFile.WriteLine("")
            ioFile.WriteLine("")
            ioFile.WriteLine("Error Details")
            ioFile.WriteLine("-------------------------------------------------------")
            ioFile.WriteLine("Error: " & ex.Message)
            ioFile.WriteLine("")
            If Not IsNothing(ex.InnerException) Then
                ioFile.WriteLine("Inner Error: " & ex.InnerException.Message)
                ioFile.WriteLine("")
            End If
            ioFile.WriteLine("Source: " & ObjectThrowingError.ToString)
            ioFile.WriteLine("")
            ioFile.WriteLine("Object Check: " & ObJectCheck.ToString)
            ioFile.WriteLine("")
            Dim st As New StackTrace(ex, True)
            ioFile.WriteLine("Stack Frames: ")
            For Each Frame As StackFrame In st.GetFrames()
                ioFile.WriteLine("Line:" + Frame.GetFileLineNumber().ToString + Frame.GetFileName().ToString, Frame.GetMethod().ToString)
            Next
            ioFile.WriteLine("-------------------------------------------------------")

            ioFile.WriteLine("Stack Trace: " & st.ToString())
            ioFile.WriteLine("")
            ioFile.WriteLine("-------------------------------------------------------")
            If Not ex.InnerException Is Nothing Then
                Dim stInner As New StackTrace(ex.InnerException, True)
                ioFile.WriteLine("Inner Stack Trace: " & stInner.ToString())
                ioFile.WriteLine("")
                ioFile.WriteLine("-------------------------------------------------------")
            End If

            '***********************************************************
        Finally
            If Not IsNothing(ioFile) Then
                ioFile.Close()
            End If
        End Try
    End Sub

#End Region

End Class