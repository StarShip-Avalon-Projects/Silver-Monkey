using System;
using System.Runtime.Serialization;
using System.Text;

namespace Libraries.Web
{
    /// <summary>
    ///
    /// </summary>
    public class WebException : System.Net.WebException
    {
        #region Private Fields

        private WebData _WebObject;

        #endregion Private Fields

        #region Public Constructors

        public WebException(string message) : base(message)
        {
        }

        public WebException(string message, WebData WebObject) : base(message)
        {
            _WebObject = WebObject;
        }

        public WebException()
        {
        }

        public WebException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected WebException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
                                base(serializationInfo, streamingContext)
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        public WebData WebObject
        {
            get
            {
                return _WebObject;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(WebObject.ErrMsg);
            sb.AppendLine($"Status Code {WebObject.Status}");
            sb.AppendLine(WebObject.WebPage);
            return sb.ToString();
        }

        #endregion Public Methods
    }
}