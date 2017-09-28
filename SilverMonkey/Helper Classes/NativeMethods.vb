Imports System.Runtime.InteropServices

Namespace HelperClasses

    ''' <summary>
    ''' Native Win32 Methods
    ''' </summary>
    Public NotInheritable Class NativeMethods
        Public Enum SBOrientation As Integer
            SB_HORZ = &H0
            SB_VERT = &H1
            SB_CTL = &H2
            SB_BOTH = &H3
        End Enum

        Public Enum ScrollInfoMask As UInteger
            SIF_RANGE = &H1
            SIF_PAGE = &H2
            SIF_POS = &H4
            SIF_DISABLENOSCROLL = &H8
            SIF_TRACKPOS = &H10
            SIF_ALL = (SIF_RANGE Or SIF_PAGE Or SIF_POS Or SIF_TRACKPOS)
        End Enum

        Public Declare Function GetScrollInfo Lib "user32.dll" (hWnd As IntPtr,
                <MarshalAs(UnmanagedType.I4)> fnBar As SBOrientation,
                <MarshalAs(UnmanagedType.Struct)> ByRef lpsi As SCROLLINFO) As Integer

        Public Declare Function GetScrollPos Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nBar As Integer) As Integer

        Public Declare Function GetScrollRange Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nBar As Integer,
                ByRef lpMinPos As Integer,
                ByRef lpMaxPos As Integer) As Boolean

        Structure SCROLLINFO

#Region "Public Fields"

            Public Property CbSize As Integer
            <MarshalAs(UnmanagedType.U4)> Public fMask As ScrollInfoMask
            Public Property NMax As Integer
            Public Property NMin As Integer
            Public Property NPage As UInteger
            Public Property NPos As Integer
            Public Property NTrackPos As Integer

#End Region

        End Structure

#Region "WmCpyDta"

        'Public Function FindProcessByName(strProcessName As String) As IntPtr
        '    Dim HandleOfToProcess As IntPtr = IntPtr.Zero
        '    Dim p As Process() = Process.GetProcesses()
        '    For Each p1 As Process In p
        '        Debug.WriteLine(p1.ProcessName.ToUpper())
        '        If p1.ProcessName.ToUpper() = strProcessName.ToUpper() Then
        '            HandleOfToProcess = p1.MainWindowHandle
        '            Exit For
        '        End If
        '    Next
        '    Return HandleOfToProcess
        'End Function

        'Protected Overrides Sub WndProc(ByRef m As Message)
        '    If m.Msg = WM_COPYDATA Then
        '        ''Dim mystr As COPYDATASTRUCT
        '        'Dim mystr2 As COPYDATASTRUCT = CType(Marshal.PtrToStructure(m.LParam(), GetType(COPYDATASTRUCT)), COPYDATASTRUCT)

        ' '' If the size matches 'If mystr2.cdData =
        ' Marshal.SizeOf(GetType(MyData)) Then ' ' Marshal the data from the
        ' unmanaged memory block to a ' ' MyStruct managed struct. ' Dim myStr
        ' As MyData = DirectCast(Marshal.PtrToStructure(mystr2.lpData,
        ' GetType(MyData)), MyData)

        ' ' Dim sName As String = myStr.lpName Dim sFID As Integer = 0 Dim '
        ' sTag As String = myStr.lpTag Dim sData As String = myStr.lpMsg

        ' ' If sName = "~DSEX~" Then If sTag = "Restart" Then ' EngineRestart =
        ' True cBot.MS_Script = ' msReader(CheckBotFolder(cBot.MS_File))
        ' MainEngine.MSpage = ' engine.LoadFromString(cBot.MS_Script) MS_Stared
        ' = 2 ' ' MainMSEngine.LoadLibrary() EngineRestart = False ' '
        ' Main.ResetPrimaryVariables() sndDisplay(" '
        ' <b>
        ' ' <i>[SM]</i> '
        ' </b>
        ' ' Status: File Saved. Engine Restarted") If '
        ' FurcadiaSession.IsClientConnected Then FurcadiaSession.SendClient(") '
        ' <b>
        ' ' <i>[SM]</i> '
        ' </b>
        ' ' Status: File Saved. Engine Restarted" + vbLf) PageExecute(0) ' End
        ' If Else If DREAM.FurreList.Contains(sFID) Then ' Player =
        ' DREAM.FurreList(sFID) Else Player = New ' FURRE(sName) End If

        ' ' Player.Message = sData.ToString ' PageSetVariable(MS_Name, sName) '
        ' PageSetVariable("MESSAGE", sData) ' ' Execute (0:15) When some one
        ' whispers something ' PageExecute(75, 76, 77) '
        ' SendClientMessage("Message from: " + sName, sData) ' End If 'End If
        ' Else MyBase.WndProc(m) End If

        'End Sub

        'Private Declare Sub FindWindow Lib "user32.dll" ()

        'Private Declare Function FindWindow Lib "user32.dll" (_ClassName As String, _WindowName As String) As Integer

        'Public Declare Function SetFocusAPI Lib "user32.dll" Alias "SetFocus" (ByVal hWnd As Long) As Long
        'Private Declare Function SetForegroundWindow Lib "user32" (ByVal hWnd As Long) As Long

#End Region

    End Class
End Namespace
