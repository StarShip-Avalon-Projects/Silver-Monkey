Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web
Imports Monkeyspeak

'Web Module
'(5:10) - (5:60)
Namespace Engine.Libraries

    Public Class MSPK_Web
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        Private Webrequest As New WebRequests("")
        Dim WebStack As New Dictionary(Of String, String)
        Private WebURL As String = ""

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)
            'WebStack.Clear()
            '(0:70) When the bot receives a variable list by sending the Web-Array.
            Add(New Trigger(TriggerCategory.Cause, 70),
        Function()
            Return True
        End Function, "(0:70) When the bot receives a variable list by sending the Web-Array.")

            '(1:30) and Web-Array setting {...} is equal to {...},
            Add(New Trigger(TriggerCategory.Condition, 30), AddressOf WebArrayEqualTo, "(1:30) and Web-Array setting {...} is equal to {...},")

            '(1:31) and Web-Array setting {...} is not equal to {...},
            Add(New Trigger(TriggerCategory.Condition, 31), AddressOf WebArrayNotEqualTo, "(1:31) and Web-Array setting {...} is not equal to {...},")

            '(1:32) and the Web-Array contains field named {...},
            Add(New Trigger(TriggerCategory.Condition, 32), AddressOf WebArrayContainArrayField, "(1:32) and the Web-Array contains field named {...},")

            '(1:33) and the Web-Array doesn't contain field named {...},
            Add(New Trigger(TriggerCategory.Condition, 33), AddressOf WebArrayNotContainArrayField, "(1:33) and the Web-Array doesn't contain field named {...},")

            '(5:9) remove variable %Variable from the Web-Array
            Add(New Trigger(TriggerCategory.Effect, 9), AddressOf RemoveWebStack, "(5:9) remove variable %Variable from the Web-Array.")

            '(5:10)  Set the web URL to {...}
            Add(New Trigger(TriggerCategory.Effect, 10), AddressOf SetURL, "(5:10)  Set the web URL to {...},")

            '(5:11)  remember setting {...} from Web-Array and store it into variable %Variable.
            Add(New Trigger(TriggerCategory.Effect, 11), AddressOf RememberSetting, "(5:11)  remember setting {...} from Web-Array and store it into variable %Variable.")

            '(5:12)

            '(5:13)

            '(5:14)

            '(5:15)

            '(5:16) send GET request to send the Web-Array to URL.
            Add(New Trigger(TriggerCategory.Effect, 16), AddressOf SendGetWebStack, "(5:16) send GET request to send the Web-Array to URL.")

            '(5:17) store variable %Variable to the Web-Array
            Add(New Trigger(TriggerCategory.Effect, 17), AddressOf StoreWebStack, "(5:17) store variable %Variable to the Web-Array.")
            '(5:18) send post request to send the Web-Array to the web host.
            Add(New Trigger(TriggerCategory.Effect, 18), AddressOf SendWebStack, "(5:18) send POST request to send the Web-Array to URL.")
            '(5:19) clear the Web-Array.
            Add(New Trigger(TriggerCategory.Effect, 19), AddressOf ClearWebStack, "(5:19) clear the Web-Array.")

        End Sub

#End Region

#Region "Private Methods"

        ''(5:19) clear the Web-Array.
        Private Function ClearWebStack(reader As TriggerReader) As Boolean
            If Not IsNothing(WebStack) Then WebStack.Clear()
            Return True
        End Function

        '(5:11)  remember setting {...} from Web-Array and store it into variable %Variable.
        Private Function RememberSetting(reader As TriggerReader) As Boolean
            Try
                Dim setting As String = reader.ReadString
                Dim var As Variable = reader.ReadVariable(True)
                If WebStack.ContainsKey(setting) Then
                    var.Value = WebStack.Item(setting)
                End If
                Return True
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
        End Function

        '(5:60) remove variable %Variable from the Web-Array
        Private Function RemoveWebStack(reader As TriggerReader) As Boolean
            Dim var As Monkeyspeak.Variable = Variable.NoValue
            Try
                var = reader.ReadVariable()
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
            WebStack.Remove(var.Name)
            Return True
        End Function

        '(5:16) send GET request to send the Web-Array to URL.
        Private Function SendGetWebStack(reader As TriggerReader) As Boolean

            Dim page As WebRequests.WebData = Nothing
            Dim ws As New WebRequests(WebURL)
            Try
                SyncLock Me
                    page = ws.WGet(WebStack)
                    WebStack = page.WebStack
                    If page.ReceivedPage Then
                        FurcadiaSession.MSpage.Execute(70)
                    End If
                End SyncLock
                If page.Status <> 0 Then Throw New WebException(page.ErrMsg)
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
            Return page.Status = 0
        End Function

        '(5:18) send post request to send the Web-Array to the web host.
        Private Function SendWebStack(reader As TriggerReader) As Boolean

            Dim page As WebRequests.WebData = Nothing
            Dim ws As New WebRequests(WebURL)
            Try
                SyncLock Me
                    page = ws.WPost(WebStack)
                    WebStack = page.WebStack
                    If page.ReceivedPage Then
                        PageExecute(70)
                    End If
                End SyncLock
                If page.Status <> 0 Then Throw New WebException(page.ErrMsg)
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
            Return page.Status = 0
        End Function

        '(5:10)  Set the web URL to {...}
        Private Function SetURL(reader As TriggerReader) As Boolean
            Try
                WebURL = reader.ReadString
                Return True
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
        End Function

        '(5:17) store variable %Variable to the Web-Array
        Private Function StoreWebStack(reader As TriggerReader) As Boolean

            Try
                Dim var As Monkeyspeak.Variable = reader.ReadVariable()
                If WebStack.ContainsKey(var.Name) = False Then
                    WebStack.Add(var.Name.Substring(1), var.Value.ToString)
                End If
                Return True
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try

        End Function

        Private Function WebArrayContainArrayField(reader As TriggerReader) As Boolean
            Try
                Return WebStack.ContainsKey(reader.ReadString)
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
        End Function

        '(1:30) and Web-Array setting {...} is equal to {...},
        Private Function WebArrayEqualTo(reader As TriggerReader) As Boolean
            Try

                Dim setting As String
                Try
                    setting = WebStack.Item(reader.ReadString)
                Catch
                    setting = ""
                End Try
                Dim Check As String = reader.ReadString
                Return setting = Check
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
        End Function

        Private Function WebArrayNotContainArrayField(reader As TriggerReader) As Boolean
            Try
                Return Not WebStack.ContainsKey(reader.ReadString)
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
        End Function

        '(1:31) and Web-Array setting {...} is not equal to {...},
        Private Function WebArrayNotEqualTo(reader As TriggerReader) As Boolean
            Try
                Dim setting As String = Nothing
                Dim value As String = reader.ReadString
                If WebStack.ContainsKey(value) Then
                    setting = WebStack.Item(value)
                End If
                Dim Check As String = reader.ReadString
                Dim b As Boolean = setting <> Check
                Return setting <> Check
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
        End Function

#End Region

    End Class

    Public Class WebRequests

#Region "Private Fields"

        Dim data As New Dictionary(Of [String], [String])()
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
        Public Sub New(ByRef URL As String)
            'cBot = New cBot
            WebURL = URL
        End Sub

#End Region

#Region "Public Methods"

        ' Sample Data
        Sub add(ByVal Key As String, ByVal Value As String)
            Try
                data.Add(Key, Value)
            Catch ex As Exception
                data.Item(Key) = Value
            End Try

        End Sub

        Public Function PackURLEncod() As String
            Dim str As String = ""
            For Each row As KeyValuePair(Of String, String) In data
                Dim temp As String = ""
                If row.Value <> "" Then
                    temp = HttpUtility.UrlEncode(row.Value)
                End If
                str += String.Format("{0}={1}&", row.Key, temp)
            Next
            Return str.Substring(0, str.Length - 1)
        End Function

        Public Function WGet(ByRef array As Dictionary(Of String, String)) As WebData
            Dim result As New WebData

            For Each item As KeyValuePair(Of String, String) In array
                Dim Key As String = item.Key
                If Key.StartsWith("WWW") Then Key = Key.Substring(3)
                data.Add(Key, item.Value)
            Next
            Dim str As String = PackURLEncod()

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
                        If CDbl(line.Substring(2)) > 0 Then
                            result.ErrMsg = "The server returned error code " + result.Status.ToString
                            Exit Do
                        End If
                        readKVPs = True
                    Else
                        If readKVPs Then
                            Dim pos As Integer = line.IndexOf("=")
                            If pos > 0 Then
                                result.ReceivedPage = True
                                Dim key As String = line.Substring(0, pos)
                                Dim Value As String = line.Substring(pos + 1)

                                'Assign Variables
                                Try
                                    result.WebStack.Add(key, Value)
                                Catch
                                    result.WebStack.Item(key) = Value
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

        Public Function WPost(ByRef array As Dictionary(Of String, String)) As WebData
            Dim Result As New WebData

            For Each item As KeyValuePair(Of String, String) In array
                Dim Key As String = item.Key
                If Key.StartsWith("WWW") Then Key = Key.Substring(3)
                data.Add(Key, item.Value)
            Next
            Dim str As String = PackURLEncod()

            Dim encoding As New UTF8Encoding
            Dim byteData As Byte() = encoding.GetBytes(str)

            Try
            Catch ex As Exception
                Result.Status = 1
                Result.ErrMsg = ex.Message.ToString
                Result.Packet = ""
                Return Result
            End Try
            Dim postReq As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(WebURL), Net.HttpWebRequest)
            postReq.Method = "POST"
            postReq.KeepAlive = False
            postReq.ContentType = "application/x-www-form-urlencoded"
            postReq.Referer = WebReferer
            postReq.UserAgent = UserAgent
            'postReq.ContentLength = byteData.Length
            Try
                Dim postreqstream As Stream = postReq.GetRequestStream()
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
                Dim postresponse As HttpWebResponse

                postresponse = DirectCast(postReq.GetResponse(), HttpWebResponse)

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
                            Dim pos As Integer = line.IndexOf("=")
                            If pos > 0 Then
                                Result.ReceivedPage = True
                                Dim key As String = line.Substring(0, pos)
                                Dim Value As String = line.Substring(pos + 1)

                                'Assign Variables
                                Try
                                    Result.WebStack.Add(key, Value)
                                Catch
                                    Result.WebStack.Item(key) = Value
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

        Public Class WebData

#Region "Public Fields"

            Public ErrMsg As String = ""
            Public Packet As String = ""
            Public WebStack As New Dictionary(Of String, String)

#End Region

#Region "Private Fields"

            Private _Status As Integer = 0

#End Region

#Region "Public Constructors"

            Public Sub New()

            End Sub

#End Region

#Region "Public Properties"

            Public Property ReceivedPage As Boolean = False

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