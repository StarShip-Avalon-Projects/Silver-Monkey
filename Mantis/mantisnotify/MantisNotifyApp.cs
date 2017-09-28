//-----------------------------------------------------------------------
// <copyright file="MantisNotifyApp.cs" company="Victor Boctor">
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

using MonkeyCore;
using MonkeyCore.Utils.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Futureware.MantisNotify
{
    /// <summary>
    /// Application class for Mantis Notify application.
    /// </summary>
    public sealed class MantisNotifyApp
    {
        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.
        /// </param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            var logError = new ErrorLogging(ref ex, sender);
            var args = string.Join(" ", logError.BugReport.ToArray());
            var Proc = Path.Combine(Application.StartupPath, "BugTragSubmit.exe");
            Process.Start(Proc, args);

            //Exception ex = (Exception)e.ExceptionObject;
            //ErrorLogging logError = new ErrorLogging(ref ex, sender);
            // MessageBox.Show("An error log has been saved to" + logError.LogFile, "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Process.Start(Paths.SilverMonkeyErrorLogPath);
            // Application.Exit();
        }



        #region Private Methods

        private static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.Run(new MantisNotifyForm());
        }

        #endregion Private Methods
    }
}