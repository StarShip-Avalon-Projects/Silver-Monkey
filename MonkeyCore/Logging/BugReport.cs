using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MonkeyCore.Logging
{
    public sealed class BugReport
    {
        #region Private Fields

        private string _logPath;

        private string _projectName;
        private string _severity;

        private string _summary;

        private Version version;
        private AssemblyInformationalVersionAttribute _InfoVersion;

        #endregion Private Fields

        #region Public Constructors

        public BugReport()
        {
            Initialize();
        }

        public const string ToolAppName = "Bugtraq Submit.exe";

        /// <summary>
        /// Initializes a new instance of the <see cref="BugReport"/> class.
        /// </summary>
        /// <param name="log">The error log.</param>
        public BugReport(ErrorLogging log)
        {
            Initialize();
            _logPath = log.LogFile;
            Summary = log.Summary;
            _severity = "crash";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BugReport"/> class
        /// from command-line Arguments
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public BugReport(string[] args)
        {
            Initialize();
            var param1 = args.SingleOrDefault(arg => arg.StartsWith("-s="));
            if (!string.IsNullOrEmpty(param1))
            {
                Summary = param1.Replace("-s=", "").Trim('"');
            }
            var param3 = args.SingleOrDefault(arg => arg.StartsWith("-l="));
            if (!string.IsNullOrEmpty(param3))
            {
                _logPath = param3.Replace("-l=", "").Trim('"');
            }
            var param4 = args.SingleOrDefault(arg => arg.StartsWith("-N="));
            if (!string.IsNullOrEmpty(param4))
            {
                _projectName = param4.Replace("-N=", "").Trim('"');
            }
            var param5 = args.SingleOrDefault(arg => arg.StartsWith("-S="));
            if (!string.IsNullOrEmpty(param5))
            {
                Severity = param5.Replace("-S=", "").Trim('"').Trim('"');
            }
            var param6 = args.SingleOrDefault(arg => arg.StartsWith("-v="));
            if (!string.IsNullOrEmpty(param6))
            {
                if (!Version.TryParse(param6.Replace("-v=", "").Trim('"'), out version))
                    _InfoVersion = new AssemblyInformationalVersionAttribute(param6.Replace("-v=", "").Trim('"'));
            }
        }

        private void Initialize()
        {
            _logPath = "";
            _severity = "major";
            _summary = "";

            _projectName = Assembly.GetEntryAssembly().GetName().Name;
            version = Assembly.GetEntryAssembly().GetName().Version;
            _InfoVersion = Attribute
                .GetCustomAttribute(
                    Assembly.GetEntryAssembly(),
                    typeof(AssemblyInformationalVersionAttribute))
                as AssemblyInformationalVersionAttribute;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the log file-path.
        /// <para/>
        /// Not Required but preloaded
        /// </summary>
        /// <value>
        /// The log path.
        /// </value>
        public string LogPath
        {
            get { return _logPath; }
        }

        /// <summary>
        /// Gets or sets the name of the project.
        /// <para/>
        /// Required
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        public Version ProjectVersion
        {
            get { return version; }
        }

        /// <summary>
        /// Gets or sets the severity.
        /// <para/>
        /// Required
        /// </summary>
        /// <value>
        /// The severity.
        /// </value>
        public string Severity
        {
            get { return _severity; }
            set { _severity = value; }
        }

        /// <summary>
        /// Gets or sets the summary.
        /// <para/>
        /// Required
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        #endregion Public Properties

        /// <summary>

        /// To the command line arguments.
        /// </summary>
        /// <returns></returns>
        public string ToCommandLineArgs()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(_severity))
                sb.Append($"-S=\"{_severity}\"");
            sb.Append(" ");
            if (!string.IsNullOrWhiteSpace(_summary))
                sb.Append($"-s=\"{_summary}\"");
            sb.Append(" ");
            if (!string.IsNullOrWhiteSpace(_logPath))
                sb.Append($"-l=\"{_logPath}\"");
            sb.Append(" ");
            if (_InfoVersion != null)
            {
                sb.Append($"-v=\"{_InfoVersion.InformationalVersion}\"");
                sb.Append(" ");
            }
            else
            {
                sb.Append($"-v=\"{version.ToString()}\"");
                sb.Append(" ");
            }
            sb.Append($"-n=\"{_projectName}\"");
            //sb.Append(" ");
            return sb.ToString();
        }
    }
}