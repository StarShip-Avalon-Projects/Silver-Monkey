Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web
Imports Monkeyspeak
Imports SilverMonkeyEngine.Interfaces

'Web Module
'
Namespace Engine.Libraries.Web

    ''' <summary>
    '''Silver Monkeys web site interface
    ''' </summary>
    Public Class WebRequests
        Private WebEncoding As Encoding = Encoding.GetEncoding(1252)
        Private page As Monkeyspeak.Page

#Region "Private Fields"

        Private data As New List(Of MsVariable)()
        Private UserAgent As String = "Silver Monkey a Furcadia Bot (gerolkae@hotmail.com)"
        Private Ver As Integer = 1
        Private WebReferer As String = "https://silvermonkey.tsprojects.org"

        '*Value may not always return from post functions
        Private WebURL As Uri

#End Region

#Region "Public Constructors"

        'Action=[Action]   (Get, Delete, Set)
        'Section=[section]
        'Key={key]
        '*Value=[Value]

        ''' <summary>
        ''' Constructor Specifying the Web url to connect to
        ''' </summary>
        ''' <param name="Url"> </param>
        Public Sub New(Url As Uri, reader As TriggerReader)
            WebURL = Url
            page = reader.Page
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Pack the Variables into a URL Encoded String as ServerData
        ''' </summary>
        ''' <param name="VariableList"></param>
        ''' <returns></returns>
        Public Shared Function EncodeWebVariables(VariableList As List(Of IVariable)) As String
            Dim FormattedVariables As New StringBuilder()
            For Each item As IVariable In VariableList
                If item.Value IsNot Nothing Then
                    FormattedVariables.AppendFormat(String.Format("{0}={1}&",
                      HttpUtility.UrlEncode(item.Name.Replace("%", String.Empty)), HttpUtility.UrlEncode(item.Value.ToString())))
                End If
            Next
            Return FormattedVariables.ToString()
        End Function

        ''' <summary>
        ''' send a "GET" request to the server
        ''' </summary>
        ''' <param name="array"></param>
        ''' <returns></returns>
        Public Function WGet(ByRef array As List(Of IVariable)) As WebData
            Dim result As New WebData
            Dim EncodedWebVariables = EncodeWebVariables(array)

            Dim requesttring = New Uri(WebURL.Host + "?" + EncodedWebVariables)
            Dim request As Net.HttpWebRequest =
                DirectCast(Net.WebRequest.Create(requesttring),
                               Net.HttpWebRequest)

            request.UserAgent = UserAgent
            request.Referer = WebReferer
            'request.Method = "GET"
            WebReferer = WebURL.ToString
            Dim readKVPs As Boolean = False
            Try
                Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Dim postreqreader As New StreamReader(response.GetResponseStream())
                Dim MS As Boolean = False
                Dim line As String
                Do While Not postreqreader.EndOfStream
                    line = postreqreader.ReadLine
                    If String.IsNullOrEmpty(line) Then
                        'Loop

                    ElseIf line = "SM" And Not MS Then
                        MS = True
                    ElseIf line <> "SM" And Not MS Then
                        line += postreqreader.ReadToEnd
                        Debug.Print(line)
                        line = line.Replace("<br />", Environment.NewLine)
                        result.ErrMsg = "Invalid Format- Not Monkey Speak Response" & line
                        result.Packet = ""
                        result.Status = 2

                        Exit Do
                    ElseIf line.StartsWith("s=") Then
                        result.Status = CInt(line.Substring(2))
                        If line.Substring(2) > 0 Then
                            result.ErrMsg = "The server returned error code " + result.Status.ToString
                            Exit Do
                        End If
                        readKVPs = True
                    Else
                        If readKVPs Then
                            Dim pos() = line.Split("=", 0, 1)
                            If pos.Length > 0 Then
                                result.ReceivedPage = True
                                Dim var = New MsVariable(pos(0), pos(1))
                                'Assign Variables
                                Try
                                    result.WebStack.Add(var)
                                Catch
                                    result.WebStack.Item(result.WebStack.IndexOf(var)) = var
                                End Try
                            End If
                        End If

                    End If
                Loop
            Catch ex As WebException
                result.ErrMsg = ex.Message.ToString
                result.Packet = ""
                Dim message = New StringBuilder()
                If Not ex.Response Is Nothing Then
                    For i = 0 To ex.Response.Headers.Count - 1
                        message.AppendLine(String.Format("Header Name:{0}, Header value :{1}", ex.Response.Headers.Keys(i), ex.Response.Headers(i)))
                    Next

                    If ex.Status = WebExceptionStatus.ProtocolError Then
                        message.AppendLine(String.Format("Status Code : {0}", DirectCast(ex.Response, HttpWebResponse).StatusCode))
                        message.AppendLine(String.Format("Status Description : {0}", DirectCast(ex.Response, HttpWebResponse).StatusDescription))

                        Using reader As New StreamReader(ex.Response.GetResponseStream())
                            message.Append(reader.ReadToEnd().Replace(vbNewLine, vbCrLf))
                        End Using
                    End If
                End If
                result.WebPage = message.ToString()

            End Try

            Return result

        End Function

        ''' <summary>
        ''' send data to the website via the POST method
        ''' </summary>
        ''' <param name="WebVariables"></param>
        ''' <returns></returns>
        Public Function WPost(ByRef WebVariables As List(Of IVariable)) As WebData
            If WebVariables Is Nothing Or WebVariables.Count = 0 Then
                Throw New ArgumentNullException(NameOf(WebVariables))
            End If

            Dim Result As New WebData

            Dim PostData = EncodeWebVariables(WebVariables)

            Dim PostDataEncoding = WebEncoding
            Dim byteData = PostDataEncoding.GetBytes(PostData)

            Dim postReq = DirectCast(Net.WebRequest.Create(WebURL), Net.HttpWebRequest)
            postReq.Method = "POST"
            postReq.KeepAlive = False
            postReq.ContentType = "application/x-www-form-urlencoded"
            postReq.Referer = WebReferer
            postReq.UserAgent = UserAgent
            'postReq.ContentLength = byteData.Length
            Try
                Dim postreqstream = postReq.GetRequestStream()
                postreqstream.Write(byteData, 0, byteData.Length)
                postreqstream.Flush()
                postreqstream.Close()
            Catch ex As WebException
                Result.ErrMsg = ex.Message.ToString
                Result.Packet = ""
                Dim message = New StringBuilder()
                If Not ex.Response Is Nothing Then
                    For i = 0 To ex.Response.Headers.Count - 1
                        message.AppendLine(String.Format("Header Name:{0}, Header value :{1}", ex.Response.Headers.Keys(i), ex.Response.Headers(i)))
                    Next

                    If ex.Status = WebExceptionStatus.ProtocolError Then
                        message.AppendLine(String.Format("Status Code : {0}", DirectCast(ex.Response, HttpWebResponse).StatusCode))
                        message.AppendLine(String.Format("Status Description : {0}", DirectCast(ex.Response, HttpWebResponse).StatusDescription))

                        Using reader As New StreamReader(ex.Response.GetResponseStream())
                            message.Append(reader.ReadToEnd().Replace(vbNewLine, vbCrLf))
                        End Using
                    End If
                End If
                Result.WebPage = message.ToString()
                Return Result
            End Try

            Try
                Dim postresponse = DirectCast(postReq.GetResponse(), HttpWebResponse)

                Dim postreqreader = New StreamReader(postresponse.GetResponseStream())
                Dim line As String
                Dim readKVPs As Boolean = False
                Dim MS As Boolean = False

                Do While Not postreqreader.EndOfStream
                    line = postreqreader.ReadLine
                    If String.IsNullOrEmpty(line) Then
                        'Loop

                    ElseIf line = "SM" And Not MS Then
                        MS = True
                    ElseIf line <> "SM" And Not MS Then
                        line += postreqreader.ReadToEnd
                        Result.WebPage = line
                        Debug.Print(line)
                        line = line.Replace("<br />", vbCrLf)
                        Result.ErrMsg = "Invalid Format- Not Monkey Speak Response" & line
                        Result.Packet = ""
                        Result.Status = 2

                        Exit Do

                    ElseIf line.StartsWith("s=") Then
                        Result.Status = CInt(line.Substring(2))
                        If line.Substring(2) > 0 Then
                            Result.ErrMsg = "The server returned error code " + Result.Status.ToString
                            Exit Do
                        End If
                        readKVPs = True
                    Else
                        If readKVPs Then
                            Dim pos() = line.Split("=", 0, 1)
                            If pos.Length > 0 Then
                                Result.ReceivedPage = True
                                Dim var = New MsVariable(pos(0), pos(1))

                                'Assign Variables
                                Try
                                    Result.WebStack.Add(var)
                                Catch
                                    Result.WebStack.Item(Result.WebStack.IndexOf(var)) = var
                                End Try
                            End If
                        End If

                    End If
                Loop

                'trigger (0: )
            Catch ex As System.Net.WebException
                Result.ErrMsg = ex.Message.ToString
                Result.Packet = ""
                Dim message = New StringBuilder()
                If Not ex.Response Is Nothing Then
                    For i = 0 To ex.Response.Headers.Count - 1
                        message.AppendLine(String.Format("Header Name:{0}, Header value :{1}", ex.Response.Headers.Keys(i), ex.Response.Headers(i)))
                    Next

                    If ex.Status = WebExceptionStatus.ProtocolError Then
                        message.AppendLine(String.Format("Status Code : {0}", DirectCast(ex.Response, HttpWebResponse).StatusCode))
                        message.AppendLine(String.Format("Status Description : {0}", DirectCast(ex.Response, HttpWebResponse).StatusDescription))

                        Using reader As New StreamReader(ex.Response.GetResponseStream())
                            message.Append(reader.ReadToEnd().Replace(vbNewLine, vbCrLf))
                        End Using
                    End If
                End If
                Result.WebPage = message.ToString()
            Catch ex As Exception
                Throw ex
            End Try

            Return Result

        End Function

#End Region

    End Class

End Namespace