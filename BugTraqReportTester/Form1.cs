using Logging;

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BugTraqReportTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var brokenClass = new BrokenClass();
            try
            {
                brokenClass.MakeError();
            }
            catch (Exception ex)
            {
                var ErrorLog = new ErrorLogging(ex, this);
                var report = new BugReport(ErrorLog);
                report.ProjectName = "MonkeyCore2Tests";
                var ps = new ProcessStartInfo(BugReport.ToolAppName)
                {
                    Arguments = report.ToCommandLineArgs()
                };
                Process.Start(ps);
            }
        }
    }
}