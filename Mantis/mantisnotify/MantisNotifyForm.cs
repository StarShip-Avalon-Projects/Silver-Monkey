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
        private Timer checkMantisTimer;
        private Label label1;
        private TextBox urlTextBox;
        private NotifyIcon notifyIcon;
        private ContextMenu contextMenu;
        private Button okButton;
        private Button cancelButton;
        private GroupBox groupBox1;
        private Label label2;
        private Label label3;
        private TextBox mantisUsernameTextBox;
        private TextBox mantisPasswordTextBox;
        private GroupBox groupBox2;
        private Label label4;
        private TextBox basicPasswordTextBox;
        private TextBox basicUsernameTextBox;
        private Label label5;
        private MenuItem exitMenuItem;
        private MenuItem checkNowMenuItem;
        private MenuItem ShowLastIssueMenuItem;
        private IContainer components;

        public MantisNotifyForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MantisNotifyForm));
            checkMantisTimer = new Timer(components);
            urlTextBox = new TextBox();
            label1 = new Label();
            notifyIcon = new NotifyIcon(components);
            contextMenu = new ContextMenu();
            ShowLastIssueMenuItem = new MenuItem();
            checkNowMenuItem = new MenuItem();
            exitMenuItem = new MenuItem();
            okButton = new Button();
            cancelButton = new Button();
            groupBox1 = new GroupBox();
            label3 = new Label();
            mantisPasswordTextBox = new TextBox();
            mantisUsernameTextBox = new TextBox();
            label2 = new Label();
            groupBox2 = new GroupBox();
            label4 = new Label();
            basicPasswordTextBox = new TextBox();
            basicUsernameTextBox = new TextBox();
            label5 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            //
            // checkMantisTimer
            //
            checkMantisTimer.Interval = 10000;
            checkMantisTimer.Tick += new System.EventHandler(checkMantisTimer_Tick);
            //
            // urlTextBox
            //
            urlTextBox.Location = new System.Drawing.Point(128, 16);
            urlTextBox.Name = "urlTextBox";
            urlTextBox.Size = new System.Drawing.Size(376, 20);
            urlTextBox.TabIndex = 0;
            urlTextBox.Text = "";
            //
            // label1
            //
            label1.Location = new System.Drawing.Point(16, 16);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(100, 16);
            label1.TabIndex = 1;
            label1.Text = "Mantis Connect Url";
            //
            // notifyIcon
            //
            notifyIcon.ContextMenu = contextMenu;
            notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            notifyIcon.Text = "Mantis Notifier";
            notifyIcon.Visible = true;
            //
            // contextMenu
            //
            contextMenu.MenuItems.AddRange(new MenuItem[] {
                                                                                        ShowLastIssueMenuItem,
                                                                                        checkNowMenuItem,
                                                                                        exitMenuItem});
            //
            // ShowLastIssueMenuItem
            //
            ShowLastIssueMenuItem.Index = 0;
            ShowLastIssueMenuItem.Text = "Show Last Issue";
            ShowLastIssueMenuItem.Click += new System.EventHandler(menuItem1_Click);
            //
            // checkNowMenuItem
            //
            checkNowMenuItem.Index = 1;
            checkNowMenuItem.Text = "Check Now";
            checkNowMenuItem.Click += new System.EventHandler(checkNowMenuItem_Click);
            //
            // exitMenuItem
            //
            exitMenuItem.Index = 2;
            exitMenuItem.Text = "Exit";
            exitMenuItem.Click += new System.EventHandler(exitMenuItem_Click);
            //
            // okButton
            //
            okButton.Location = new System.Drawing.Point(176, 216);
            okButton.Name = "okButton";
            okButton.TabIndex = 1;
            okButton.Text = "OK";
            //
            // cancelButton
            //
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(264, 216);
            cancelButton.Name = "cancelButton";
            cancelButton.TabIndex = 0;
            cancelButton.Text = "Cancel";
            //
            // groupBox1
            //
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(mantisPasswordTextBox);
            groupBox1.Controls.Add(mantisUsernameTextBox);
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new System.Drawing.Point(16, 48);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(488, 72);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Mantis Authentication";
            //
            // label3
            //
            label3.Location = new System.Drawing.Point(264, 32);
            label3.Name = "label3";
            label3.TabIndex = 3;
            label3.Text = "Password";
            //
            // mantisPasswordTextBox
            //
            mantisPasswordTextBox.Location = new System.Drawing.Point(376, 32);
            mantisPasswordTextBox.Name = "mantisPasswordTextBox";
            mantisPasswordTextBox.TabIndex = 2;
            mantisPasswordTextBox.Text = "";
            //
            // mantisUsernameTextBox
            //
            mantisUsernameTextBox.Location = new System.Drawing.Point(96, 32);
            mantisUsernameTextBox.Name = "mantisUsernameTextBox";
            mantisUsernameTextBox.TabIndex = 1;
            mantisUsernameTextBox.Text = "";
            //
            // label2
            //
            label2.Location = new System.Drawing.Point(16, 32);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(64, 23);
            label2.TabIndex = 0;
            label2.Text = "Username";
            //
            // groupBox2
            //
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(basicPasswordTextBox);
            groupBox2.Controls.Add(basicUsernameTextBox);
            groupBox2.Controls.Add(label5);
            groupBox2.Location = new System.Drawing.Point(16, 136);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(488, 72);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Basic Authentication";
            //
            // label4
            //
            label4.Location = new System.Drawing.Point(264, 32);
            label4.Name = "label4";
            label4.TabIndex = 3;
            label4.Text = "Password";
            //
            // basicPasswordTextBox
            //
            basicPasswordTextBox.Location = new System.Drawing.Point(376, 32);
            basicPasswordTextBox.Name = "basicPasswordTextBox";
            basicPasswordTextBox.TabIndex = 2;
            basicPasswordTextBox.Text = "";
            //
            // basicUsernameTextBox
            //
            basicUsernameTextBox.Location = new System.Drawing.Point(96, 32);
            basicUsernameTextBox.Name = "basicUsernameTextBox";
            basicUsernameTextBox.TabIndex = 1;
            basicUsernameTextBox.Text = "";
            //
            // label5
            //
            label5.Location = new System.Drawing.Point(16, 32);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(64, 23);
            label5.TabIndex = 0;
            label5.Text = "Username";
            //
            // MantisNotifyForm
            //
            AcceptButton = okButton;
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(520, 256);
            Controls.Add(groupBox1);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(label1);
            Controls.Add(urlTextBox);
            Controls.Add(groupBox2);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            MaximizeBox = false;
            Name = "MantisNotifyForm";
            ShowInTaskbar = false;
            Text = "Mantis Notify Configuration";
            WindowState = System.Windows.Forms.FormWindowState.Minimized;
            Load += new System.EventHandler(MantisNotifyForm_Load);
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        protected override void OnResize(EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();

            base.OnResize(e);
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

        private void Disconnect()
        {
            notifyIcon.Text = "Mantis Notifier (offline)";
            session = null;
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

        private string MantisBugtrackerUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["BugtrackerUrl"];
            }
        }

        private void ContentClick(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = string.Format("{0}view.php?id={1}", MantisBugtrackerUrl, lastIssueId);

            try
            {
                System.Diagnostics.Process.Start(startInfo);
            }
            catch
            {
            }
        }

        private void CloseClick(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
        }

        private void lastIssueIdTimer_Tick(object sender, System.EventArgs e)
        {
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

                // set it here so that if user clicks on the text, the handler
                // can open the issue.
                lastIssueId = issueId;

                if (issueId > 0)
                    ShowIssue(issueId);
            }
            finally
            {
                checkMantisTimer.Enabled = true;
            }
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
                    // set it here so that if user clicks on the text, the handler
                    // can open the issue.
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

        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            ShowLastIssue();
        }

        private void exitMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Eventually this should be saved between runs of the application.  Till then
        /// a notification will always be generated on the first check after running the
        /// tool.
        /// </summary>
        private static int lastIssueId;

        private Session session;
        private TaskbarNotifier taskbarNotifier;
        private int timeToShowNotificationInMs;
        private int timeToStayNotificationInMs;
        private int timeToHideNotificationInMs;
    }
}