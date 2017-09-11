//-----------------------------------------------------------------------
// <copyright file="MantisSubmitApp.cs" company="Victor Boctor">
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

using SilverMonkey.BugTraqConnect.Libs;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Futureware.MantisSubmit
{
    /// <summary>
    /// Summary description for MantisConnectSampleApp.
    /// </summary>
    public sealed class MantisSubmitApp
    {
        #region Private Constructors

        private MantisSubmitApp()
        {
        }

        #endregion Private Constructors

        #region Private Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new SubmitIssueForm(new ProjectReport(args)));


        }

        #endregion Private Methods
    }
}