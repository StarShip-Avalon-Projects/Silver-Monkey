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

using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Futureware.MantisNotify
{
    /// <summary>
    /// Application class for Mantis Notify application.
    /// </summary>
    public sealed class MantisNotifyApp
    {
        #region Private Constructors

        /// <summary>
        /// Private Constructor, no need to create instances of this class.
        /// </summary>
        private MantisNotifyApp()
        {
        }

        #endregion Private Constructors

        #region Private Methods

        private static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var param1 = args.SingleOrDefault(arg => arg.StartsWith("-v="));
            var ErrorLox = args.Single(arg => arg.StartsWith("-e="));
            //if (mutex.WaitOne(TimeSpan.Zero, true))
            //{
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MantisNotifyForm());
            //    mutex.ReleaseMutex();
            //}
        }

        #endregion Private Methods
    }
}