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
        private string _webPage;
        private List<IVariable> webStack;
        public List<IVariable> WebStack { get => webStack; set => webStack = value; }

        #endregion Private Fields

        #region Public Constructors

        public WebData()
        {
            webStack = new List<IVariable>();
            _Status = -1;
            ReceivedPage = false;
            ErrMsg = String.Empty;
            Packet = String.Empty;
        }

        public WebData(List<IVariable> WebCache) : this()
        {
            webStack = WebCache;
        }

        #endregion Public Constructors

        #region Public Properties

        public string ErrMsg
        {
            get;
            set;
        }

        public string Packet { get; internal set; }

        public bool ReceivedPage
        {
            get;
            set;
        }

        public int Status
        {
            set => _Status = value;
            get => _Status;
        }

        /// <summary>
        /// Raw text for the received web page
        /// </summary>
        /// <returns></returns>
        public string WebPage
        {
            get
            {
                return _webPage;
            }
            set
            {
                _webPage = value;
            }
        }

        #endregion Public Properties
    }
}