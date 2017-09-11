using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SilverMonkey.BugTraqConnect.Libs
{
    /// <summary>
    /// Base object for bug-report submission
    /// <para>
    /// Author: Gerolkae
    /// </para>
    /// </summary>
    public class ProjectReport
    {
        private string errorLogFile;
        private string projectName;
        private string projectVersion;
        private string reportDescription;
        private string reportSummary;
        private string severity;
        private bool projectVersionIsDefault;
        private bool projectNameIsDefault;
        private bool severityIsDefault;
        //TODO: add CPU profile detection.

        public ProjectReport()
        {
            //TODO: Use Application Informational Version
            projectVersion = "2.19.x PreAlpha 6";
            projectVersionIsDefault = true;

            projectName = Application.ProductName;
            projectNameIsDefault = true;

            severity = "minor";
            severityIsDefault = true;
        }

        /// <summary>
        /// Constructor for taking command line arguments
        /// </summary>
        /// <param name="args">Command Line arguments</param>
        public ProjectReport(string[] args) : this()
        {
            var ProjectVersion = args.SingleOrDefault(arg => arg.StartsWith("-v="));
            var ErrorLog = args.SingleOrDefault(arg => arg.StartsWith("-e="));
            var description = args.SingleOrDefault(arg => arg.StartsWith("-d="));
            var reportSubject = args.SingleOrDefault(arg => arg.StartsWith("-s="));
            var ProjectNameVar = args.SingleOrDefault(arg => arg.StartsWith("-n="));
            var ProjectSeverity = args.SingleOrDefault(arg => arg.StartsWith("-ss="));

            if (!string.IsNullOrEmpty(ErrorLog))
                errorLogFile = ErrorLog.Replace("-e=", "");
            if (!string.IsNullOrEmpty(ProjectVersion))
            {
                projectVersion = ProjectVersion.Replace("-v=", "");
                projectVersionIsDefault = false;
            }
            if (!string.IsNullOrEmpty(description))
                reportDescription = description.Replace("-d=", "");
            if (!string.IsNullOrEmpty(reportSubject))
                reportSummary = reportSubject.Replace("-s=", "");

            if (!string.IsNullOrEmpty(ProjectNameVar))
            {
                projectName = ProjectNameVar.Replace("-n=", "");
                projectNameIsDefault = false;
            }
            if (!string.IsNullOrEmpty(ProjectSeverity))
            {
                severity = ProjectSeverity.Replace("-ss=", "");
                severityIsDefault = false;
            }
        }

        /// <summary>
        /// Attachment file path to upload to bugtraq
        /// <para>
        /// Generally Unhandled exception error log files
        /// </para>
        /// </summary>
        public string AttachmentFile
        {
            get { return errorLogFile; }
            set { errorLogFile = value; }
        }

        /// <summary>
        /// Project Version to match with Bugtraq
        /// <para>
        /// Defaults to Application Informational Version of the current executing application
        /// </para>
        /// </summary>
        public string ProductVersion
        {
            get { return projectVersion; }
            set
            {
                projectVersion = value;
                projectVersionIsDefault = false;
            }
        }

        /// <summary>
        /// required by bug traq, Report description text.
        /// </summary>
        public string ReportDescription
        {
            get { return reportDescription; }
            set { reportDescription = value; }
        }

        /// <summary>
        /// Reports Subject
        /// <para>
        /// required by bugtraq
        /// </para>
        /// <para>
        /// this can be class name and Exception message by default or allow User to supply the text
        /// </para>
        /// </summary>
        public string ReportSubject
        {
            get { return reportSummary; }
            set { reportSummary = value; }
        }

        /// <summary>
        /// Project Name
        /// </summary>
        public string ProcuctName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                projectNameIsDefault = false;
            }
        }

        /// <summary>
        /// How Sever the bug is
        /// <para>
        /// For Unhandled Exceptions, mark it as 'crass'
        /// </para>
        /// </summary>
        public string Severity
        {
            get { return severity; }
            set
            {
                severity = value;
                severityIsDefault = false;
            }
        }

        /// <summary>
        /// output command line argument array
        /// </summary>
        /// <returns>string array</returns>
        public string[] ToArray()
        {
            var ArgList = new List<string>();

            if (!string.IsNullOrEmpty(errorLogFile))
                ArgList.Add("-e=\"" + errorLogFile + "\"");
            if (!string.IsNullOrEmpty(projectVersion) && !projectVersionIsDefault)
                ArgList.Add("-v=\"" + projectVersion + "\"");
            if (!string.IsNullOrEmpty(reportDescription))
                ArgList.Add("-d=\"" + reportDescription + "\"");
            if (!string.IsNullOrEmpty(reportSummary))
                ArgList.Add("-s=\"" + reportSummary + "\"");
            if (!string.IsNullOrEmpty(projectName) && !projectNameIsDefault)
                ArgList.Add("-n=\"" + projectName + "\"");
            if (!string.IsNullOrEmpty(severity) && !severityIsDefault)
                ArgList.Add("-ss=\"" + severity + "\"");

            return ArgList.ToArray<string>();
        }
    }
}