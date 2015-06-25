Imports Monkeyspeak
Imports SilverMonkey.ErrorLogging
Imports SilverMonkey.TextBoxWriter
'(5:120) - (5:119)

Public Class MS_Variables
    Inherits Monkeyspeak.Libraries.AbstractBaseLibrary
    Private writer As TextBoxWriter = Nothing

    Public Sub New()
        writer = New TextBoxWriter(Variables.TextBox1)

        ' http://www.vbforums.com/showthread.php?t=544817
        '(5: ) set variable # to the total of rolling # dice with # sides plus #.
        '(5: ) set variable # to the total of rolling # dice with # sides minus #.

    End Sub

End Class
