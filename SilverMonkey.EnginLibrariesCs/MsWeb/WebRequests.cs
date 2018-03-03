using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Libraries.Variables;
using Monkeyspeak;

namespace Libraries.Web
{
    /// <summary>
    ///Silver Monkeys web site interface
    /// </summary>
    public class WebRequests
    {
        #region Private Fields

        private Monkeyspeak.Page page;
        private string UserAgent = "Silver Monkey a Furcadia Bot (gerolkae@gmail.com.com)";

        // private int Ver = 1;
        private Encoding WebEncoding = Encoding.GetEncoding(1252);

        private string WebReferer = "https://silvermonkey.tsprojects.org";

        private Uri WebURL;

        #endregion Private Fields

        #region Public Constructors

        // Action=[Action]   (Get, Delete, Set)
        // Section=[section]
        // Key={key]
        // *Value=[Value]
        /// <summary>
        /// Constructor Specifying the Web url to connect to
        /// </summary>
        /// <param name="Url"> </param>
        public WebRequests(Uri Url, TriggerReader reader)
        {
            WebURL = Url;
            page = reader.Page;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Pack the Variables into a URL Encoded String as ServerData
        /// </summary>
        /// <param name="VariableList"></param>
        /// <returns></returns>
        public static string EncodeWebVariables(List<IVariable> VariableList)
        {
            StringBuilder FormattedVariables = new StringBuilder();
            foreach (IVariable item in VariableList)
            {
                if (item.Value != null)
                {
                    FormattedVariables.AppendFormat($"{HttpUtility.UrlEncode(item.Name.Replace("%", String.Empty))}={HttpUtility.UrlEncode(item.Value.ToString())}&");
                }
            }

            return FormattedVariables.ToString();
        }

        /// <summary>
        /// send a "GET"request to the server
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public WebData WGet(List<IVariable> array)
        {
            bool MS = false;
            string line;
            bool readKVPs = false;
            var message = new StringBuilder();
            WebData result = new WebData();
            var EncodedWebVariables = EncodeWebVariables(array);
            var requesttring = new Uri($"{WebURL.Host}?{EncodedWebVariables}");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requesttring);

            request.UserAgent = UserAgent;
            request.Referer = WebReferer;
            // request.Method = "GET"
            WebReferer = WebURL.ToString();

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader postreqreader = new StreamReader(response.GetResponseStream());

                while (!postreqreader.EndOfStream)
                {
                    line = postreqreader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        continue;
                    else if (line == "SM" && !MS)
                    {
                        MS = true;
                    }
                    else if (line != "SM" && !MS)
                    {
                        line = $"{line}{postreqreader.ReadToEnd()}";

                        line = line.Replace("<br />", Environment.NewLine);
                        result.ErrMsg = ("Invalid Format- Not Monkey Speak Response" + line);
                        result.Packet = "";
                        result.Status = 2;
                        break;
                    }
                    else if (line.StartsWith("s="))
                    {
                        result.Status = int.Parse(line.Substring(2));
                        if (int.Parse(line.Substring(2)) > 0)
                        {
                            result.ErrMsg = $"The server returned error code {result.Status.ToString()}";
                            break;
                        }

                        readKVPs = true;
                    }
                    else if (readKVPs)
                    {
                        var pos = line.Split(new char[] { '=' }, 1);
                        if (pos.Length > 0)
                        {
                            result.ReceivedPage = true;
                            var var = new WebVariable(pos[0], pos[1]);
                            // Assign Variables

                            result.WebStack.Add(var);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                result.ErrMsg = ex.Message.ToString();
                result.Packet = "";
                if (!(ex.Response == null))
                {
                    for (int i = 0; i <= ex.Response.Headers.Count; i++)
                    {
                        message.AppendLine("Header Name:{ex.Response.Headers.Keys(i)}, Header value :{ex.Response.Headers(i)}");
                    }

                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        message.AppendLine("Status Code : {DirectCast(ex.Response, HttpWebResponse).StatusCode}");
                        message.AppendLine("Status Description : {DirectCast(ex.Response, HttpWebResponse).StatusDescription}");
                        using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                            message.Append(reader.ReadToEnd().Replace("\r\n", "\r\n"));
                    }
                }
            }

            result.WebPage = message.ToString();
            return result;
        }

        public WebData WPost(List<IVariable> WebVariables)
        {
            string line;
            bool readKVPs = false;
            bool MS = false;

            WebData result = new WebData();
            var message = new StringBuilder();

            var PostData = EncodeWebVariables(WebVariables);
            var PostDataEncoding = WebEncoding;
            var byteData = PostDataEncoding.GetBytes(PostData);

            var postReq = (HttpWebRequest)WebRequest.Create(WebURL);
            var postresponse = (HttpWebResponse)postReq.GetResponse();
            var postreqreader = new StreamReader(postresponse.GetResponseStream());

            postReq.Method = "POST";
            postReq.KeepAlive = false;
            postReq.ContentType = "application/x-www-form-urlencoded";
            postReq.Referer = WebReferer;
            postReq.UserAgent = UserAgent;
            // postReq.ContentLength = byteData.Length
            try
            {
                var postreqstream = postReq.GetRequestStream();

                postreqstream.Write(byteData, 0, byteData.Length);
                postreqstream.Flush();
                postreqstream.Close();
            }
            catch (WebException ex)
            {
                result.ErrMsg = ex.Message.ToString();
                result.Packet = "";
                if (ex.Response != null)
                {
                    for (int i = 0; i <= ex.Response.Headers.Count; i++)
                    {
                        message.AppendLine("Header Name:{ex.Response.Headers.Keys(i)}, Header value :{ex.Response.Headers(i)}");
                    }

                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        message.AppendLine("Status Code : {DirectCast(ex.Response, HttpWebResponse).StatusCode}");
                        message.AppendLine("Status Description : {DirectCast(ex.Response, HttpWebResponse).StatusDescription}");
                        using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                            message.Append(reader.ReadToEnd().Replace("\r\n", "\r\n"));
                    }
                }

                result.WebPage = message.ToString();
                return result;
            }

            try
            {
                while (!postreqreader.EndOfStream)
                {
                    line = postreqreader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        continue;
                    else if (line == "SM" && !MS)
                    {
                        MS = true;
                    }
                    else if (line != "SM" && !MS)
                    {
                        line = $"{line}{postreqreader.ReadToEnd()}";

                        line = line.Replace("<br />", Environment.NewLine);
                        result.ErrMsg = ("Invalid Format- Not Monkey Speak Response" + line);
                        result.Packet = "";
                        result.Status = 2;
                        break;
                    }
                    else if (line.StartsWith("s="))
                    {
                        result.Status = int.Parse(line.Substring(2));
                        if (int.Parse(line.Substring(2)) > 0)
                        {
                            result.ErrMsg = $"The server returned error code {result.Status.ToString()}";
                            break;
                        }

                        readKVPs = true;
                    }
                    else if (readKVPs)
                    {
                        var pos = line.Split(new char[] { '=' }, 1);
                        if (pos.Length > 0)
                        {
                            result.ReceivedPage = true;
                            var var = new WebVariable(pos[0], pos[1]);
                            // Assign Variables

                            result.WebStack.Add(var);
                        }
                    }
                }
            }
            catch (System.Net.WebException ex)
            {
                result.ErrMsg = ex.Message.ToString();
                result.Packet = "";
                if (ex.Response != null)
                {
                    for (int i = 0; i <= ex.Response.Headers.Count; i++)
                    {
                        message.AppendLine("Header Name:{ex.Response.Headers.Keys(i)}, Header value :{ ex.Response.Headers(i)}");
                    }

                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        message.AppendLine("Status Code : {DirectCast(ex.Response, HttpWebResponse).StatusCode}");
                        message.AppendLine("Status Description : {DirectCast(ex.Response, HttpWebResponse).StatusDescription}");
                        using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                            message.Append(reader.ReadToEnd().Replace("\r\n", "\r\n"));
                    }
                }
            }

            result.WebPage = message.ToString();
            return result;
        }

        #endregion Public Methods
    }
}