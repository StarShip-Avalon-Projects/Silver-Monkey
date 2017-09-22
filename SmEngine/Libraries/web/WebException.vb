Imports System.Runtime.Serialization
Imports System.Text
Imports SilverMonkeyEngine.Engine.Libraries.Web.WebRequests

Namespace Engine.Libraries.Web

    Public Class WebException
        Inherits Net.WebException

#Region "Public Constructors"

        Private _WebObject As WebData

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal message As String, WebObject As WebData)
            MyBase.New(message)
            _WebObject = WebObject
        End Sub

        Protected Sub New(serializationInfo As SerializationInfo,
                            streamingContext As StreamingContext)
            MyBase.New(serializationInfo, streamingContext)
        End Sub

#End Region

        Public ReadOnly Property WebObject As WebData
            Get
                Return _WebObject
            End Get
        End Property

        ''' <summary>
        '''
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder()
            sb.AppendLine(WebObject.ErrMsg)
            sb.AppendLine("Status Code" + WebObject.Status.ToString())
            sb.AppendLine(WebObject.WebPage)
            Return sb.ToString()

        End Function

    End Class

End Namespace