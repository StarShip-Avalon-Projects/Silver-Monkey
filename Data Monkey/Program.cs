using MonkeyCore;
using SilverMonkey.BugTraqConnect;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DataMonkey
{
    public class Program
    {
        #region Private Methods

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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.Run(new frmExplorer());
        }

        #endregion Private Methods
    }
}