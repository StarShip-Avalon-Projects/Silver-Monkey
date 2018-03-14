using System;
using System.Runtime.Serialization;
using System.Text;

namespace Libraries.Web
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Net.WebException" />
    public class WebException : System.Net.WebException
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebException"/> class.
        /// </summary>
        /// <param name="message">The text of the error message.</param>
        public WebException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="WebObject">The web object.</param>
        public WebException(string message, WebData WebObject) : base(message)
        {
            this.WebObject = WebObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebException"/> class.
        /// </summary>
        public WebException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebException"/> class.
        /// </summary>
        /// <param name="message">The text of the error message.</param>
        /// <param name="innerException">A nested exception.</param>
        public WebException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebException"/> class.
        /// </summary>
        /// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that contains the information required to serialize the new <see cref="T:System.Net.WebException" />.</param>
        /// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the source of the serialized stream that is associated with the new <see cref="T:System.Net.WebException" />.</param>
        protected WebException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
                                base(serializationInfo, streamingContext)
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// Gets the web object.
        /// </summary>
        /// <value>
        /// The web object.
        /// </value>
        public WebData WebObject { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
        /// </PermissionSet>
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