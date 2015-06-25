' Basic badge detection demonstration
' Author: Arden
' Not for real world usage

Imports Monkeyspeak
Imports SilverMonkey.ErrorLogging
Imports SilverMonkey.TextBoxWriter
Imports System.Reflection


Public Class Misc

    Inherits Monkeyspeak.Libraries.AbstractBaseLibrary
    Private writer As TextBoxWriter = Nothing


    Public Sub New()
        writer = New TextBoxWriter(Variables.TextBox1)


        '(1:1000) and the triggering furre doesn't have a badge on,
        '(1:1001) and the triggering furre has a badge on,
        '(1:1002) and the triggering furre isn't in group #, (URL)
        '(1:1003) and they have an active badge and are level # of group #, (URL)
        '(1:1004) and they are in beekin group #, (URL)
        '(1:1005) and Furre Named {...} is in beekin group #, (URL)
        '(1:1006) and they have group level #, (URL)
        '(1:1007) and Furre Named {...} has group level #, (URL)
        '(5:1000) set variable to the beekin group number of the triggering furre (URL)
        '(5:1001) set variable to the beekin group number of the furre named {...} (URL)
        '(5:1002) set variable to the beekin group level of the triggering furre. (URL)
        '(5:1003) set variable to the beekin group level of the furre named {...}. (URL)
    End Sub

    '(1:1000) and the triggering furre doesn't have a badge on,
    '(1:1001) and the triggering furre has a badge on,
    '(1:1002) and the triggering furre isn't in group #, (URL)
    '(1:1003) and they have an active badge and are level # of group #, (URL)
    '(1:1004) and they are in beekin group #, (URL)
    '(1:1005) and Furre Named {...} is in beekin group #, (URL)
    '(1:1006) and they have group level #, (URL)
    '(1:1007) and Furre Named {...} has group level #, (URL)
    '(5:1000) set variable to the beekin group number of the triggering furre (URL)
    '(5:1001) set variable to the beekin group number of the furre named {...} (URL)
    '(5:1002) set variable to the beekin group level of the triggering furre. (URL)
    '(5:1003) set variable to the beekin group level of the furre named {...}. (URL)

#Region "Helper Functions"

#End Region
End Class
