using Monkeyspeak;
using System;
using System.Collections.Generic;

namespace Libraries.Web
{
    /// <summary>
    /// web response page object
    /// </summary>
    public class WebData
    {
        #region Private Fields

        private int _Status;
        private List<IVariable> webStack;
        public List<IVariable> WebStack { get => webStack; set => webStack = value; }

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebData"/> class.
        /// </summary>
        public WebData()
        {
            webStack = new List<IVariable>();
            _Status = -1;
            ReceivedPage = false;
            ErrMsg = String.Empty;
            Packet = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebData"/> class.
        /// </summary>
        /// <param name="WebCache">The web cache.</param>
        public WebData(List<IVariable> WebCache) : this()
        {
            webStack = WebCache;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the error MSG.
        /// </summary>
        /// <value>
        /// The error MSG.
        /// </value>
        public string ErrMsg
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the packet.
        /// </summary>
        /// <value>
        /// The packet.
        /// </value>
        public string Packet { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether [received page].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [received page]; otherwise, <c>false</c>.
        /// </value>
        public bool ReceivedPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status
        {
            set => _Status = value;
            get => _Status;
        }

        /// <summary>
        /// Raw text for the received web page
        /// </summary>
        /// <returns></returns>
        public string WebPage { get; set; }

        #endregion Public Properties
    }
}