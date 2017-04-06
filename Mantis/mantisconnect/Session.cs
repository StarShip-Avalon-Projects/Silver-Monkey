//-----------------------------------------------------------------------
// <copyright file="Session.cs" company="Victor Boctor">
//     Copyright (C) All Rights Reserved
// </copyright>
// <summary>
// MantisConnect is copyrighted to Victor Boctor
//
// This program is distributed under the terms and conditions of the GPL
// See LICENSE file for details.
//
// For commercial applications to link with or modify MantisConnect, they require the
// purchase of a MantisConnect commercial license.
// </summary>
//-----------------------------------------------------------------------

namespace SilverMonkey.BugTraqConnect
{
    using System;
    using System.Net;

    /// <summary>
    /// Represents a connection session between the webservice client and server.
    /// </summary>
    public sealed class Session
    {
        #region Private Fields

        /// <summary>
        /// The config instance.
        /// </summary>
        private readonly Config config;

        /// <summary>
        /// The network credential (e.g. basic http auth).
        /// </summary>
        private readonly NetworkCredential networkCredential;

        /// <summary>
        /// The password.
        /// </summary>
        private readonly string password;

        /// <summary>
        /// The request instance.
        /// </summary>
        private readonly Request request;

        /// <summary>
        /// The MantisBT url.
        /// </summary>
        private readonly string url;

        /// <summary>
        /// The user name.
        /// </summary>
        private readonly string username;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructs a session given a url, username and password.
        /// </summary>
        /// <param name="url">
        /// URL of MantisConnect webservice (eg: http://www.example.com/mantis/)
        /// </param>
        /// <param name="username">
        /// User name to connect as.
        /// </param>
        /// <param name="password">
        /// Password for the specified user.
        /// </param>
        /// <param name="networkCredential">
        /// The network credentials to use (e.g. basic http password).
        /// </param>
        public Session(string url, string username, string password, NetworkCredential networkCredential)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (url.IndexOf("mantisconnect.php", StringComparison.OrdinalIgnoreCase) == -1)
            {
                if (!url.EndsWith("/"))
                {
                    url += "/";
                }

                url += "api/soap/mantisconnect.php";
            }

            this.username = username;
            this.password = password;
            this.url = url;
            this.networkCredential = networkCredential;

            this.config = new Config(this);
            this.request = new Request(this);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Config object to be used to retrieve information about the configuration of the Mantis installation.
        /// </summary>
        public Config Config
        {
            get { return this.config; }
        }

        /// <summary>
        /// Gets the network credential specified during construction time.
        /// </summary>
        /// <remarks>
        /// This is use to support connecting to Mantis installation that are protected with basic
        /// http authentication.
        /// </remarks>
        public NetworkCredential NetworkCredential
        {
            get { return this.networkCredential; }
        }

        /// <summary>
        /// Gets the user password specified on construction of the session.
        /// </summary>
        public string Password
        {
            get { return this.password; }
        }

        /// <summary>
        /// Request is a wrapper around the MantisConnect webservice which makes calling the methods
        /// more natural for the rest of the C# code.
        /// </summary>
        public Request Request
        {
            get { return this.request; }
        }

        /// <summary>
        /// Gets the MantisConnect webservice URL
        /// </summary>
        /// <remarks>
        /// eg: http://www.example.com/mantis/mantisconnect/mantisconnect.php
        /// </remarks>
        public string Url
        {
            get { return this.url; }
        }

        /// <summary>
        /// Gets the user name specified on construction of the session.
        /// </summary>
        public string Username
        {
            get { return this.username; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Connects to the webservice.
        /// </summary>
        public void Connect()
        {
            // call any method to trigger authentication, if url, username, password are valid, then
            // nothing will happen, otherwise an exception will be raised.
            MantisEnum temp = Config.StatusEnum;
        }

        #endregion Public Methods
    }
}