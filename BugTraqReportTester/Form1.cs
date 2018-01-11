using Logging;
using MonkeyCore2.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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