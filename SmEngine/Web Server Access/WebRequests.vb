Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web
Imports Monkeyspeak

'Web Module
'
Namespace Engine.Libraries

    ''' <summary>
    ''' Provide web interface for getting a list of Variables from a web server
    ''' <para>
    ''' Effects: (5:10) - (5:60)
    ''' </para>
    ''' </summary>
    Public Class MsWebRequests
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        Private Webrequest As New WebRequests()
        Private WebStack As New List(Of Variable)()
        Private WebURL As String = ""

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)
            'WebStack.Clear()
            '(0:70) When the bot receives a variable list by sending the Web-Cache.
            Add(New Trigger(TriggerCategory.Cause, 70),
                Function()
                    Return True
                End Function, "(0:70) When the bot receives a variable list by sending the Web-Cache.")

            '(1:30) and Web-Cache setting {...} is equal to {...},
            Add(New Trigger(TriggerCategory.Condition, 30), AddressOf WebArrayEqualTo, "(1:30) and Web-Cache setting {...} is equal to {...},")

            '(1:31) and Web-Cache setting {...} is not equal to {...},
            Add(New Trigger(TriggerCategory.Condition, 31), AddressOf WebArrayNotEqualTo, "(1:31) and Web-Cache setting {...} is not equal to {...},")

            '(1:32) and the Web-Cache contains field named {...},
            Add(New Trigger(TriggerCategory.Condition, 32), AddressOf WebArrayContainArrayField, "(1:32) and the Web-Cache contains field named {...},")

            '(1:33) and the Web-Cache doesn't contain field named {...},
            Add(New Trigger(TriggerCategory.Condition, 33), AddressOf WebArrayNotContainArrayField, "(1:33) and the Web-Cache doesn't contain field named {...},")

            '(5:9) remove variable %Variable from the Web-Cache
            Add(New Trigger(TriggerCategory.Effect, 9), AddressOf RemoveWebStack, "(5:9) remove variable %Variable from the Web-Cache.")

            '(5:10)  Set the web URL to {...}
            Add(New Trigger(TriggerCategory.Effect, 10), AddressOf SetURL, "(5:10)  Set the web URL to {...},")

            '(5:11)  remember setting {...} from Web-Cache and store it into variable %Variable.
            Add(New Trigger(TriggerCategory.Effect, 11), AddressOf RememberSetting, "(5:11)  remember setting {...} from Web-Cache and store it into variable %Variable.")

            '(5:12)

            '(5:13)

            '(5:14)

            '(5:15)

            '(5:16) send GET request to send the Web-Cache to URL.
            Add(New Trigger(TriggerCategory.Effect, 16), AddressOf SendGetWebStack, "(5:16) send GET request to send the Web-Cache to URL.")

            '(5:17) store variable %Variable to the Web-Cache
            Add(New Trigger(TriggerCategory.Effect, 17), AddressOf StoreWebStack, "(5:17) store variable %Variable to the Web-Cache.")
            '(5:18) send post request to send the Web-Cache to the web host.
            Add(New Trigger(TriggerCategory.Effect, 18), AddressOf SendWebStack, "(5:18) send POST request to send the Web-Cache to URL.")
            '(5:19) clear the Web-Cache.
            Add(New Trigger(TriggerCategory.Effect, 19), AddressOf ClearWebStack, "(5:19) clear the Web-Cache.")

        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' (5:19) clear the Web-Cache.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function ClearWebStack(reader As TriggerReader) As Boolean
            If Not IsNothing(WebStack) Then WebStack.Clear()
            Return True
        End Function

        ''' <summary>
        ''' (5:11) remember setting {...} from Web-Cache and store it into
        ''' variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function RememberSetting(reader As TriggerReader) As Boolean

            Dim setting = New Variable(reader.ReadString, Nothing)
            Dim var As Variable = reader.ReadVariable(True)
            If WebStack.Contains(setting) Then
                var.Value = WebStack(WebStack.IndexOf(setting))
            End If
            Return True

        End Function

        ''' <summary>
        ''' (5:60) remove variable %Variable from the Web-Cache
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function RemoveWebStack(reader As TriggerReader) As Boolean
            Dim var As Monkeyspeak.Variable = Variable.NoValue

            var = reader.ReadVariable()

            WebStack.Remove(var)
            Return True
        End Function

        ''' <summary>
        ''' (5:16) send GET request to send the Web-Cache to URL.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function SendGetWebStack(reader As TriggerReader) As Boolean

            Dim page As WebRequests.WebData = Nothing
            Dim ws As New WebRequests(WebURL)

            SyncLock Me
                page = ws.WGet(WebStack)
                WebStack = page.WebStack
                If page.ReceivedPage Then
                    FurcadiaSession.MSpage.Execute(70)
                End If
            End SyncLock
            If page.Status <> 0 Then Throw New WebException(page.ErrMsg)

            Return page.Status = 0
        End Function

        ''' <summary>
        ''' (5:18) send post request to send the Web-Cache to the web host.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function SendWebStack(reader As TriggerReader) As Boolean

            Dim page As WebRequests.WebData = Nothing
            Dim ws As New WebRequests(WebURL)

            SyncLock Me
                page = ws.WPost(WebStack)
                WebStack = page.WebStack
                If page.ReceivedPage Then
                    MsPage.Execute(70)
                End If
            End SyncLock
            If page.Status <> 0 Then Throw New WebException(page.ErrMsg, page)

            Return page.Status = 0
        End Function

        ''' <summary>
        ''' (5:10) Set the web URL to {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function SetURL(reader As TriggerReader) As Boolean

            WebURL = reader.ReadString
            Return True
        End Function

        ''' <summary>
        ''' (5:17) store variable %Variable to the Web-Cache
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function StoreWebStack(reader As TriggerReader) As Boolean

            Dim var As Monkeyspeak.Variable = reader.ReadVariable()
            If WebStack.Contains(var) Then
                WebStack.Item(WebStack.IndexOf(var)) = var
            Else
                WebStack.Add(var)
            End If
            Return True

        End Function

        Private Function WebArrayContainArrayField(reader As TriggerReader) As Boolean
            Dim var = New Variable(reader.ReadString, Nothing)
            Return WebStack.Contains(var)

        End Function

        ''' <summary>
        ''' (1:30) and Web-Cache setting {...} is equal to {...},
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function WebArrayEqualTo(reader As TriggerReader) As Boolean

            Dim setting As String
            Try
                setting = WebStack.Item(WebStack.IndexOf(New Variable(reader.ReadString, Nothing))).Value
            Catch
                setting = ""
            End Try
            Return setting = reader.ReadString

        End Function

        Private Function WebArrayNotContainArrayField(reader As TriggerReader) As Boolean

            Return Not WebStack.Contains(New Variable(reader.ReadString, Nothing))

        End Function

        ''' <summary>
        ''' (1:31) and Web-Cache setting {...} is not equal to {...},
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Private Function WebArrayNotEqualTo(reader As TriggerReader) As Boolean

            Dim setting As Variable = Nothing
            Dim value = New Variable(reader.ReadString, Nothing)
            If WebStack.Contains(value) Then
                setting = WebStack.Item(WebStack.IndexOf(value))
            End If
            Return setting.Value <> reader.ReadString
        End Function

#End Region

    End Class

    Public Class WebRequests

#Region "Private Fields"

        Dim data As New List(Of Variable)()
        Private UserAgent As String = "Silver Monkey a Furcadia Bot (gerolkae@hotmail.com)"
        Dim Ver As Integer = 1
        Private WebReferer As String = "http://silvermonkey.tsprojects.org"

        '*Value may not always return from post functions
        Dim WebURL As String

#End Region

#Region "Public Constructors"

        'Action=[Action]   (Get, Delete, Set)
        'Section=[section]
        'Key={key]
        '*Value=[Value]

        ''' <summary>
        '''
        ''' </summary>
        Public Sub New()
            'cBot = New cBot
            WebURL = String.Empty
        End Sub

        Public Sub New(Url As String)
            'cBot = New cBot
            WebURL = Url
        End Sub

#End Region

#Region "Public Methods"

        Public Function PackURLEncod(VariableList As List(Of Variable)) As String
            Dim FormattedVariables As New StringBuilder()
            For Each item As Variable In VariableList
                FormattedVariables.AppendFormat(String.Format("{0}={1}&",
                      HttpUtility.UrlEncode(item.Name), HttpUtility.UrlEncode(item.Value.ToString())))
            Next
            Return FormattedVariables.ToString()
        End Function

        Public Function WGet(ByRef array As List(Of Variable)) As WebData
            Dim result As New WebData
            Dim str As String = PackURLEncod(array)

            Try
            Catch ex As Exception
                result.Status = 1
                result.ErrMsg = ex.Message.ToString
                result.Packet = ""
                Return result
            End Try
            Dim requesttring As String = WebURL & "?" & str
            Dim request As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(requesttring), Net.HttpWebRequest)
            request.UserAgent = UserAgent
            request.Referer = WebReferer
            'request.Method = "GET"
            WebReferer = WebURL
            Dim packet As String = ""
            Dim readKVPs As Boolean = False
            Try
                Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Dim postreqreader As New StreamReader(response.GetResponseStream())
                Dim MS As Boolean = False
                Dim line As String
                Do While Not postreqreader.EndOfStream
                    line = postreqreader.ReadLine
                    If line = "" Then
                        'Loop

                    ElseIf line = "SM" And Not MS Then
                        MS = True
                    ElseIf line <> "SM" And Not MS Then
                        line += postreqreader.ReadToEnd
                        Debug.Print(line)
                        line = line.Replace("<br />", vbCrLf)
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
                                Dim var = New Variable(pos(0), pos(1))
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
            Catch ex As Exception
                result.ErrMsg = ex.Message.ToString
                result.Packet = ""
                Return result
            End Try

            Return result

        End Function

        Public Function WPost(ByRef array As List(Of Variable)) As WebData
            Dim Result As New WebData

            Dim PostData = PackURLEncod(array)

            Dim PostDataEncoding = Encoding.GetEncoding(1252)
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
            Catch ex As Exception
                Result.ErrMsg = ex.Message.ToString
                Result.Packet = ""
                Result.Status = 1
                Return Result

            End Try

            Try
                Dim postresponse = DirectCast(postReq.GetResponse(), HttpWebResponse)

                Dim postreqreader As New StreamReader(postresponse.GetResponseStream())
                Dim line As String
                Dim readKVPs As Boolean = False
                Dim MS As Boolean = False
                Dim ReceivePage As Boolean = False
                Do While Not postreqreader.EndOfStream
                    line = postreqreader.ReadLine
                    If line = "" Then
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
                        If CDbl(line.Substring(2)) > 0 Then
                            Result.ErrMsg = "The server returned error code " + Result.Status.ToString
                            Exit Do
                        End If
                        readKVPs = True
                    Else
                        If readKVPs Then
                            Dim pos() = line.Split("=", 0, 1)
                            If pos.Length > 0 Then
                                Result.ReceivedPage = True
                                Dim var = New Variable(pos(0), pos(1))

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
            Catch ex As Exception
                Result.ErrMsg = ex.Message.ToString
                Result.Packet = ""
                Result.Status = 4
                Return Result
            End Try

            Return Result

        End Function

#End Region

#Region "Public Classes"

        ''' <summary>
        ''' web response page object
        ''' </summary>
        Public Class WebData

#Region "Public Fields"

            Private _webPage As String
            Public ErrMsg As String
            Public Packet As String
            Public WebStack As New List(Of Variable)()

#End Region

#Region "Private Fields"

            Private _Status As Integer

#End Region

#Region "Public Constructors"

            Public Sub New()
                _Status = -1
                ReceivedPage = False
                ErrMsg = String.Empty
                Packet = String.Empty
            End Sub

#End Region

#Region "Public Properties"

            ''' <summary>
            ''' Raw text for the received web page
            ''' </summary>
            ''' <returns></returns>
            Public Property WebPage As String
                Get
                    Return _webPage
                End Get
                Set(value As String)
                    _webPage = value
                End Set
            End Property

            Public Property ReceivedPage As Boolean

            ''' <summary>
            ''' Web server status code
            ''' </summary>
            ''' <returns></returns>
            Public Property Status As Integer
                Get
                    Return _Status
                End Get
                Set(value As Integer)
                    _Status = value
                End Set
            End Property

#End Region

        End Class

#End Region

    End Class

End Namespace