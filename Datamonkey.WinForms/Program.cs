using Logging;
using MonkeyCore2.Logging;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DataMonkey
{
    /// <summary>
    /// Data Monkey Program launcher
    /// </summary>
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
            var ErrorLog = new ErrorLogging(ex, e.ExceptionObject);
            var report = new BugReport(ErrorLog);
            report.ProjectName = "MonkeyCore2Tests";
            var ps = new ProcessStartInfo(BugReport.ToolAppName)
            {
                Arguments = report.ToCommandLineArgs()
            };
            Process.Start(ps);
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