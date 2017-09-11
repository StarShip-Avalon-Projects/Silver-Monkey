//-----------------------------------------------------------------------
// <copyright file="MantisNotifyForm.cs" company="Victor Boctor">
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

using CustomUIControls;
using SilverMonkey.BugTraqConnect;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace Futureware.MantisNotify
{
    /// <summary>
    /// Main form for Mantis Notify sample application.
    /// </summary>
    public class MantisNotifyForm : Form
    {
        #region Private Fields

        /// <summary>
        /// Eventually this should be saved between runs of the application.
        /// Till then a notification will always be generated on the first
        /// check after running the tool.
        /// </summary>
        private static int lastIssueId;

        private TextBox basicPasswordTextBox;
        private TextBox basicUsernameTextBox;
        private Button cancelButton;
        private Timer checkMantisTimer;
        private MenuItem checkNowMenuItem;
        private IContainer components;
        private ContextMenu contextMenu;
        private MenuItem exitMenuItem;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox mantisPasswordTextBox;
        private TextBox mantisUsernameTextBox;
        private NotifyIcon notifyIcon;
        private Button okButton;
        private Session session;
        private MenuItem ShowLastIssueMenuItem;
        private TaskbarNotifier taskbarNotifier;
        private int timeToHideNotificationInMs;
        private int timeToShowNotificationInMs;
        private int timeToStayNotificationInMs;
        private TextBox urlTextBox;

        #endregion Private Fields

        #region Public Constructors

        public MantisNotifyForm()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Properties

        private string MantisBugtrackerUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["BugtrackerUrl"];
            }
        }

        #endregion Private Properties

        #region Protected Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion Protected Methods

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify the
        /// contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MantisNotifyForm));
            this.checkMantisTimer = new System.Windows.Forms.Timer(this.components);
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.ShowLastIssueMenuItem = new System.Windows.Forms.MenuItem();
            this.checkNowMenuItem = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mantisPasswordTextBox = new System.Windows.Forms.TextBox();
            this.mantisUsernameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.basicPasswordTextBox = new System.Windows.Forms.TextBox();
            this.basicUsernameTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkMantisTimer
            // 
            this.checkMantisTimer.Interval = 10000;
            this.checkMantisTimer.Tick += new System.EventHandler(this.checkMantisTimer_Tick);
            // 
            // urlTextBox
            // 
            this.urlTextBox.Location = new System.Drawing.Point(333, 38);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(977, 38);
            this.urlTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(42, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 38);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mantis Connect Url";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenu = this.contextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Mantis Notifier";
            this.notifyIcon.Visible = true;
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ShowLastIssueMenuItem,
            this.checkNowMenuItem,
            this.exitMenuItem});
            // 
            // ShowLastIssueMenuItem
            // 
            this.ShowLastIssueMenuItem.Index = 0;
            this.ShowLastIssueMenuItem.Text = "Show Last Issue";
            this.ShowLastIssueMenuItem.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // checkNowMenuItem
            // 
            this.checkNowMenuItem.Index = 1;
            this.checkNowMenuItem.Text = "Check Now";
            this.checkNowMenuItem.Click += new System.EventHandler(this.checkNowMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 2;
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(458, 515);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(195, 55);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(686, 515);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(195, 55);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.mantisPasswordTextBox);
            this.groupBox1.Controls.Add(this.mantisUsernameTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(42, 114);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1268, 172);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mantis Authentication";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(686, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(260, 55);
            this.label3.TabIndex = 3;
            this.label3.Text = "Password";
            // 
            // mantisPasswordTextBox
            // 
            this.mantisPasswordTextBox.Location = new System.Drawing.Point(978, 76);
            this.mantisPasswordTextBox.Name = "mantisPasswordTextBox";
            this.mantisPasswordTextBox.Size = new System.Drawing.Size(260, 38);
            this.mantisPasswordTextBox.TabIndex = 2;
            // 
            // mantisUsernameTextBox
            // 
            this.mantisUsernameTextBox.Location = new System.Drawing.Point(250, 76);
            this.mantisUsernameTextBox.Name = "mantisUsernameTextBox";
            this.mantisUsernameTextBox.Size = new System.Drawing.Size(260, 38);
            this.mantisUsernameTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(42, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 55);
            this.label2.TabIndex = 0;
            this.label2.Text = "Username";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.basicPasswordTextBox);
            this.groupBox2.Controls.Add(this.basicUsernameTextBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(42, 324);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1268, 172);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Basic Authentication";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(686, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(260, 55);
            this.label4.TabIndex = 3;
            this.label4.Text = "Password";
            // 
            // basicPasswordTextBox
            // 
            this.basicPasswordTextBox.Location = new System.Drawing.Point(978, 76);
            this.basicPasswordTextBox.Name = "basicPasswordTextBox";
            this.basicPasswordTextBox.Size = new System.Drawing.Size(260, 38);
            this.basicPasswordTextBox.TabIndex = 2;
            // 
            // basicUsernameTextBox
            // 
            this.basicUsernameTextBox.Location = new System.Drawing.Point(250, 76);
            this.basicUsernameTextBox.Name = "basicUsernameTextBox";
            this.basicUsernameTextBox.Size = new System.Drawing.Size(260, 38);
            this.basicUsernameTextBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(42, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(166, 55);
            this.label5.TabIndex = 0;
            this.label5.Text = "Username";
            // 
            // MantisNotifyForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(13, 31);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1347, 603);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.urlTextBox);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1379, 691);
            this.MinimumSize = new System.Drawing.Size(1379, 691);
            this.Name = "MantisNotifyForm";
            this.ShowInTaskbar = false;
            this.Text = "Mantis Notify Configuration";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.MantisNotifyForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion Windows Form Designer generated code

        protected override void OnResize(EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();

            base.OnResize(e);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
        }

        private void CheckForNewIssues()
        {
            try
            {
                checkMantisTimer.Enabled = false;

                if (!Connect())
                    return;

                int issueId;

                try
                {
                    issueId = session.Request.IssueGetLastId(0);
                }
                catch (System.Net.WebException)
                {
                    Disconnect();
                    return;
                }

                if (issueId != lastIssueId)
                {
                    // set it here so that if user clicks on the text, the
                    // handler can open the issue.
                    lastIssueId = issueId;

                    ShowIssue(issueId);
                }
            }
            finally
            {
                checkMantisTimer.Enabled = true;
            }
        }

        private void checkMantisTimer_Tick(object sender, System.EventArgs e)
        {
            CheckForNewIssues();
        }

        private void checkNowMenuItem_Click(object sender, System.EventArgs e)
        {
            CheckForNewIssues();
        }

        private void CloseClick(object sender, EventArgs e)
        {
        }

        private bool Connect()
        {
            if (session == null)
            {
                try
                {
                    NetworkCredential nc = null;

                    NameValueCollection appSettings = ConfigurationManager.AppSettings;
                    string basicHttpAuthUserName = appSettings["BasicHttpAuthUserName"];
                    string basicHttpAuthPassword = appSettings["BasicHttpAuthPassword"];
                    if (!String.IsNullOrEmpty(basicHttpAuthUserName) && basicHttpAuthPassword != null)
                    {
                        nc = new NetworkCredential(basicHttpAuthUserName, basicHttpAuthPassword);
                    }

                    string mantisConnectUrl = appSettings["MantisConnectUrl"];
                    string mantisUserName = appSettings["MantisUserName"];
                    string mantisPassword = appSettings["MantisPassword"];

                    session = new Session(mantisConnectUrl, mantisUserName, mantisPassword, nc);
                    session.Connect();

                    notifyIcon.Text = "Mantis Notifier";
                }
                catch (System.Net.WebException)
                {
                    Disconnect();
                }
            }

            return session != null;
        }

        private void ContentClick(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = string.Format("{0}/view.php?id={1}", MantisBugtrackerUrl, lastIssueId);

            try
            {
                System.Diagnostics.Process.Start(startInfo);
            }
            catch
            {
            }
        }

        private void Disconnect()
        {
            notifyIcon.Text = "Mantis Notifier (offline)";
            session = null;
        }

        private void exitMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void lastIssueIdTimer_Tick(object sender, System.EventArgs e)
        {
        }

        private void MantisNotifyForm_Load(object sender, System.EventArgs e)
        {
            Hide();

            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            checkMantisTimer.Interval = Convert.ToInt32(appSettings["CheckForNewIssuesEverySecs"]) * 1000;
            timeToShowNotificationInMs = Convert.ToInt32(appSettings["TimeToShowNotificationSecs"]) * 1000;
            timeToStayNotificationInMs = Convert.ToInt32(appSettings["TimeToStayNotificationSecs"]) * 1000;
            timeToHideNotificationInMs = Convert.ToInt32(appSettings["TimeToHideNotificationSecs"]) * 1000;

            taskbarNotifier = new TaskbarNotifier();
            taskbarNotifier.SetBackgroundBitmap(new Bitmap(GetType(), "skin.bmp"), Color.FromArgb(255, 0, 255));
            taskbarNotifier.SetCloseBitmap(new Bitmap(GetType(), "close.bmp"), Color.FromArgb(255, 0, 255), new Point(280, 55));
            taskbarNotifier.TitleRectangle = new Rectangle(150, 57, 125, 28);
            taskbarNotifier.ContentRectangle = new Rectangle(75, 92, 215, 55);
            taskbarNotifier.TitleClick += new EventHandler(TitleClick);
            taskbarNotifier.ContentClick += new EventHandler(ContentClick);
            taskbarNotifier.CloseClick += new EventHandler(CloseClick);

            Connect();

            checkMantisTimer.Enabled = true;
        }

        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            ShowLastIssue();
        }

        private void ShowIssue(int issueId)
        {
            if (!Connect())
                return;

            Issue issue;
            try
            {
                issue = session.Request.IssueGet(issueId);
            }
            catch (System.Net.WebException)
            {
                Disconnect();
                return;
            }

            taskbarNotifier.CloseClickable = true;
            taskbarNotifier.TitleClickable = true;
            taskbarNotifier.ContentClickable = true;
            taskbarNotifier.EnableSelectionRectangle = true;
            taskbarNotifier.KeepVisibleOnMousOver = true;
            taskbarNotifier.ReShowOnMouseOver = true;

            taskbarNotifier.Show("Mantis Notifier", string.Format("{0}: {1}",
                issueId, issue.Summary), timeToShowNotificationInMs, timeToStayNotificationInMs,
                timeToHideNotificationInMs);
        }

        private void ShowLastIssue()
        {
            try
            {
                checkMantisTimer.Enabled = false;

                if (!Connect())
                    return;

                int issueId;
                try
                {
                    issueId = session.Request.IssueGetLastId(0);
                }
                catch (System.Net.WebException)
                {
                    Disconnect();
                    return;
                }

                // set it here so that if user clicks on the text, the
                // handler can open the issue.
                lastIssueId = issueId;

                if (issueId > 0)
                    ShowIssue(issueId);
            }
            finally
            {
                checkMantisTimer.Enabled = true;
            }
        }

        private void TitleClick(object sender, EventArgs e)
        {
            string url = ConfigurationManager.AppSettings["MantisConnectWebsite"];

            if (url != null)
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.FileName = url;
                System.Diagnostics.Process.Start(startInfo);
            }
        }
    }
}