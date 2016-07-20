﻿using System;
using System.Windows.Forms;
using MonkeyCore;
using System.Diagnostics;

namespace SQLiteEditor
{
    public class Program
	{
		/// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException +=  new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.Run(new frmExplorer());
        }

        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            ErrorLogging logError = new ErrorLogging(ref ex, sender);
            MessageBox.Show("An error log has been saved to" + logError.LogFile, "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Process.Start(Paths.SilverMonkeyErrorLogPath);
            Application.Exit();
        }
    }

}
