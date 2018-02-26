Imports System.Text
Imports System.Windows.Forms
Imports MonkeyCore.Controls
Imports MonkeyCore.WinForms.Controls

<CLSCompliant(True)>
Public Class TextBoxWriter
    Inherits System.IO.TextWriter

#Region "Private Fields"

    Private Builder As StringBuilder
    Private control As TextBoxBase

#End Region

#Region "Public Constructors"

    Public Sub New(ByVal control As TextBox)
        Me.control = control
        AddHandler control.HandleCreated,
           New EventHandler(AddressOf OnHandleCreated)
        Builder = New StringBuilder()
    End Sub

    Public Sub New(ByVal control As RichTextBox)
        Me.control = control
        AddHandler control.HandleCreated,
           New EventHandler(AddressOf OnHandleCreated)
        Builder = New StringBuilder()
    End Sub

#End Region

#Region "Public Delegates"

    Delegate Sub AppendTextDelegate(ByRef Text As String)

#End Region

#Region "Public Properties"

    Public Overrides ReadOnly Property Encoding() As System.Text.Encoding
        Get
            Return Encoding.Default
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Sub Write(ByVal ch As Char)
        Write(ch.ToString())
    End Sub

    Public Overrides Sub Write(ByVal s As String)
        If (control.IsHandleCreated) Then
            AppendText(s)
        Else
            BufferText(s)
        End If
    End Sub

    Public Overrides Sub WriteLine(ByVal s As String)
        Write(s + Environment.NewLine)
    End Sub

#End Region

#Region "Private Methods"

    Private Sub AppendText(ByRef Text As String)
        If control.InvokeRequired Then
            control.Invoke(New AppendTextDelegate(AddressOf AppendText), Text)
        Else
            If Builder.Length > 0 Then
                control.AppendText(Builder.ToString())
                Builder.Clear()
            End If
            control.AppendText(Text)
            ' (ByRef lb As Object, ByRef obj As Object, ByRef newColor As fColorEnum)
            'Main.AddDataToList(Main.log_, s, 0)

        End If
    End Sub

    Private Sub BufferText(ByRef s As String)

        Builder.Append(s)

    End Sub

    Private Sub OnHandleCreated(ByVal sender As Object,
       ByVal e As EventArgs)
        If Builder.Length > 0 Then
            control.AppendText(Builder.ToString())
            Builder.Clear()
        End If
    End Sub

#End Region

End Class