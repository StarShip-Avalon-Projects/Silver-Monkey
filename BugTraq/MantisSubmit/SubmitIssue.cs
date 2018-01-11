//-----------------------------------------------------------------------
// <copyright file="SubmitIssue.cs" company="Victor Boctor">
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

using Futureware.MantisConnect;
using MonkeyCore2.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Futureware.MantisSubmit
{
    /// <summary>
    /// A Windows form that allows submitting of issues in a Mantis installation.
    /// </summary>
    public class SubmitIssueForm : Form
    {
        #region Private Fields

        private TextBox attachmentTextBox;
        private Button browseButton;
        private BugReport bugReport;
        private System.Windows.Forms.ComboBox categoryComboBox;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private TextBox descriptionTextBox;
        private Label label1;
        private Label label10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private TextBox notedTextBox;
        private OpenFileDialog openFileDialog1;

        /// <summary>
        /// Tracks whether the projects combobox is currently being populated or not.
        /// </summary>
        /// <remarks>
        /// If being populated, then selection change event for the current project is
        /// ignored.
        /// </remarks>
        private bool populating = false;

        private System.Windows.Forms.ComboBox priorityComboBox;
        private TextBox repoStepsTextBox;
        private System.Windows.Forms.ComboBox reproducibilityComboBox;

        /// <summary>
        /// Session used to communicate with MantisConnect.
        /// </summary>
        private Session session;

        private System.Windows.Forms.ComboBox severityComboBox;
        private StatusStrip statusBar;
        private Button submitButton;
        private System.Windows.Forms.TextBox summaryTextBox;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private ToolStripStatusLabel ToolStripNoticeLable;
        private TreeView treeView1;
        private System.Windows.Forms.ComboBox versionComboBox;

        #endregion Private Fields

        #region Public Constructors

        public SubmitIssueForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitIssueForm"/> class
        /// with command-line argument initialization
        /// </summary>
        /// <param name="args">The arguments.</param>
        public SubmitIssueForm(string[] args)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            bugReport = new BugReport(args);
        }

        #endregion Public Constructors

        #region Private Properties

        private string MantisBugtrackerUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["MantisConnectUrl"];
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

        #region Private Methods

        private void browseButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = attachmentTextBox.Text;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.InitialDirectory = IO.Paths.SilverMonkeyErrorLogPath;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                attachmentTextBox.Text = openFileDialog1.FileName;
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.summaryTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.priorityComboBox = new System.Windows.Forms.ComboBox();
            this.severityComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.reproducibilityComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.versionComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.attachmentTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.ToolStripNoticeLable = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.repoStepsTextBox = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.notedTextBox = new System.Windows.Forms.TextBox();
            this.statusBar.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            //
            // summaryTextBox
            //
            this.summaryTextBox.Location = new System.Drawing.Point(426, 581);
            this.summaryTextBox.Name = "summaryTextBox";
            this.summaryTextBox.Size = new System.Drawing.Size(1227, 38);
            this.summaryTextBox.TabIndex = 7;
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(93, 581);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 54);
            this.label1.TabIndex = 1;
            this.label1.Text = "Summary";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // submitButton
            //
            this.submitButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.submitButton.Location = new System.Drawing.Point(820, 1202);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(195, 66);
            this.submitButton.TabIndex = 9;
            this.submitButton.Text = "Submit";
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(841, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(260, 50);
            this.label2.TabIndex = 3;
            this.label2.Text = "Priority";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // priorityComboBox
            //
            this.priorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityComboBox.Location = new System.Drawing.Point(1174, 336);
            this.priorityComboBox.Name = "priorityComboBox";
            this.priorityComboBox.Size = new System.Drawing.Size(457, 39);
            this.priorityComboBox.TabIndex = 4;
            //
            // severityComboBox
            //
            this.severityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.severityComboBox.Location = new System.Drawing.Point(1174, 412);
            this.severityComboBox.Name = "severityComboBox";
            this.severityComboBox.Size = new System.Drawing.Size(457, 39);
            this.severityComboBox.TabIndex = 5;
            //
            // label3
            //
            this.label3.Location = new System.Drawing.Point(841, 412);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(260, 50);
            this.label3.TabIndex = 5;
            this.label3.Text = "Severity";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // reproducibilityComboBox
            //
            this.reproducibilityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reproducibilityComboBox.Location = new System.Drawing.Point(1174, 488);
            this.reproducibilityComboBox.Name = "reproducibilityComboBox";
            this.reproducibilityComboBox.Size = new System.Drawing.Size(457, 39);
            this.reproducibilityComboBox.TabIndex = 6;
            //
            // label4
            //
            this.label4.Location = new System.Drawing.Point(841, 488);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(260, 50);
            this.label4.TabIndex = 7;
            this.label4.Text = "Reproducibility";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // label6
            //
            this.label6.Location = new System.Drawing.Point(71, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(260, 50);
            this.label6.TabIndex = 11;
            this.label6.Text = "Projects";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // label7
            //
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(83, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1602, 102);
            this.label7.TabIndex = 13;
            this.label7.Text = "TS Projects BugTrag Submit Tool";
            //
            // versionComboBox
            //
            this.versionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.versionComboBox.Location = new System.Drawing.Point(1174, 183);
            this.versionComboBox.Name = "versionComboBox";
            this.versionComboBox.Size = new System.Drawing.Size(457, 39);
            this.versionComboBox.TabIndex = 2;
            //
            // label8
            //
            this.label8.Location = new System.Drawing.Point(841, 183);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(260, 50);
            this.label8.TabIndex = 16;
            this.label8.Text = "Version";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // categoryComboBox
            //
            this.categoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.categoryComboBox.Location = new System.Drawing.Point(1174, 259);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(457, 39);
            this.categoryComboBox.TabIndex = 3;
            //
            // label9
            //
            this.label9.Location = new System.Drawing.Point(841, 259);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(260, 50);
            this.label9.TabIndex = 18;
            this.label9.Text = "Category";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // openFileDialog1
            //
            this.openFileDialog1.FileName = "openFileDialog1";
            //
            // attachmentTextBox
            //
            this.attachmentTextBox.Location = new System.Drawing.Point(433, 1119);
            this.attachmentTextBox.Name = "attachmentTextBox";
            this.attachmentTextBox.Size = new System.Drawing.Size(1134, 38);
            this.attachmentTextBox.TabIndex = 24;
            //
            // label10
            //
            this.label10.Location = new System.Drawing.Point(100, 1119);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(260, 54);
            this.label10.TabIndex = 23;
            this.label10.Text = "Attachment";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // browseButton
            //
            this.browseButton.Location = new System.Drawing.Point(1585, 1109);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 55);
            this.browseButton.TabIndex = 25;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            //
            // treeView1
            //
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(79, 195);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(662, 343);
            this.treeView1.TabIndex = 26;
            //
            // statusBar
            //
            this.statusBar.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripNoticeLable});
            this.statusBar.Location = new System.Drawing.Point(0, 1297);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1711, 22);
            this.statusBar.TabIndex = 27;
            this.statusBar.Text = "statusStrip1";
            //
            // toolStripStatusLabel1
            //
            this.ToolStripNoticeLable.Name = "toolStripStatusLabel1";
            this.ToolStripNoticeLable.Size = new System.Drawing.Size(0, 17);
            //
            // tabControl1
            //
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(77, 651);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1594, 426);
            this.tabControl1.TabIndex = 28;
            //
            // tabPage1
            //
            this.tabPage1.Controls.Add(this.descriptionTextBox);
            this.tabPage1.Location = new System.Drawing.Point(10, 48);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1574, 368);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Description";
            this.tabPage1.UseVisualStyleBackColor = true;
            //
            // descriptionTextBox
            //
            this.descriptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionTextBox.Location = new System.Drawing.Point(3, 3);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(1568, 362);
            this.descriptionTextBox.TabIndex = 10;
            //
            // tabPage2
            //
            this.tabPage2.Controls.Add(this.repoStepsTextBox);
            this.tabPage2.Location = new System.Drawing.Point(10, 48);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1574, 368);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Reproduction Steps";
            this.tabPage2.UseVisualStyleBackColor = true;
            //
            // repoStepsTextBox
            //
            this.repoStepsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.repoStepsTextBox.Location = new System.Drawing.Point(3, 3);
            this.repoStepsTextBox.Multiline = true;
            this.repoStepsTextBox.Name = "repoStepsTextBox";
            this.repoStepsTextBox.Size = new System.Drawing.Size(1568, 362);
            this.repoStepsTextBox.TabIndex = 11;
            //
            // tabPage3
            //
            this.tabPage3.Controls.Add(this.notedTextBox);
            this.tabPage3.Location = new System.Drawing.Point(10, 48);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1574, 368);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Additional Information";
            this.tabPage3.UseVisualStyleBackColor = true;
            //
            // notedTextBox
            //
            this.notedTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notedTextBox.Location = new System.Drawing.Point(0, 0);
            this.notedTextBox.Multiline = true;
            this.notedTextBox.Name = "notedTextBox";
            this.notedTextBox.Size = new System.Drawing.Size(1574, 368);
            this.notedTextBox.TabIndex = 11;
            //
            // SubmitIssueForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(13, 31);
            this.ClientSize = new System.Drawing.Size(1711, 1319);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.attachmentTextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.categoryComboBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.versionComboBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.summaryTextBox);
            this.Controls.Add(this.reproducibilityComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.severityComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.priorityComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(1743, 1407);
            this.MinimumSize = new System.Drawing.Size(1743, 1407);
            this.Name = "SubmitIssueForm";
            this.Text = "Mantis Connect - Submit Issue";
            this.Load += new System.EventHandler(this.SubmitIssue_Load);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Populates the list of categories and versions based on the currently
        /// selected projects.
        /// </summary>
        private void PopulateProjectDependentFields()
        {
            try
            {
                int projectId = (int)treeView1.SelectedNode.Tag;
                if (projectId == 0)
                {
                    categoryComboBox.DataSource = null;
                    versionComboBox.DataSource = null;
                }
                else
                {
                    categoryComboBox.DataSource = session.Request.ProjectGetCategories(projectId);
                    categoryComboBox.DisplayMember = "Name";
                    versionComboBox.DataSource = session.Request.ProjectGetVersions(projectId);
                    versionComboBox.DisplayMember = "Name";
                    try
                    {
                        versionComboBox.SelectedIndex = versionComboBox.FindStringExact(bugReport.ProjectVersion.ToString());
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Webservice Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// Prepare the form for the next issue.
        /// </summary>
        private void ResetForm()
        {
            notedTextBox.Clear();
            repoStepsTextBox.Clear();
            summaryTextBox.Clear();
            summaryTextBox.Focus();
            descriptionTextBox.Clear();
        }

        /// <summary>
        /// Event handler for clicking the submit button.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void submitButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(repoStepsTextBox.Text))
                {
                    MessageBox.Show("Reproduction Steps are required, Please fill in the box", "Warning", MessageBoxButtons.OK);
                    tabControl1.SelectedIndex = 1;
                    repoStepsTextBox.Focus();
                    return;
                }

                string attachment = this.attachmentTextBox.Text;

                if (attachment.Length > 0 && !File.Exists(attachment))
                {
                    MessageBox.Show($"File '{attachment}' doesn't exist");
                    return;
                }

                ToolStripNoticeLable.Text = "Checking if issue already reported...";

                // Check if issue was previously logged in Mantis.
                int issueId = session.Request.IssueGetIdFromSummary(summaryTextBox.Text);
                if (issueId > 0)
                {
                    ToolStripNoticeLable.Text = $"'{summaryTextBox.Text}' already reported in issue {issueId}";
                    Process.Start($"{ MantisBugtrackerUrl }/view.php?id={ issueId }");
                    return;
                }

                // Create the issue in memory
                Issue issue = new Issue
                {
                    Project = new ObjectRef(treeView1.SelectedNode.Index),
                    Priority = new ObjectRef(priorityComboBox.Text),
                    Severity = new ObjectRef(severityComboBox.Text),
                    Reproducibility = new ObjectRef(reproducibilityComboBox.Text),
                    Category = new ObjectRef(categoryComboBox.Text),
                    ProductVersion = versionComboBox.Text,

                    Summary = summaryTextBox.Text,
                    StepsToReproduce = repoStepsTextBox.Text,
                    AdditionalInformation = notedTextBox.Text,

                    Description = descriptionTextBox.Text,
                    ReportedBy = new User()
                };
                issue.ReportedBy.Name = session.Username;

                ToolStripNoticeLable.Text = "Submitting issue...";

                int newIssueId = session.Request.IssueAdd(issue);

                ToolStripNoticeLable.Text = $"Submitting attachment to issue {newIssueId}...";

                if (attachment.Length > 0)
                {
                    session.Request.IssueAttachmentAdd(newIssueId, attachment, null);
                }

                // Submit the issue and show its id in the status bar
                ToolStripNoticeLable.Text = $"Issued added as {newIssueId}.";
                Process.Start($"{MantisBugtrackerUrl}/view.php?id={newIssueId}");
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Webservice Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                statusBar.Text = string.Empty;
            }
        }

        private void SubmitIssue_Load(object sender, System.EventArgs e)
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

                string mantisUserName = appSettings["MantisUserName"];
                string mantisPassword = appSettings["MantisPassword"];

                session = new Session(MantisBugtrackerUrl, mantisUserName, mantisPassword, nc);

                session.Connect();

                populating = true;
                int i = 0;
                foreach (Project project in session.Request.UserGetAccessibleProjects())
                {
                    TreeNode FounNode = null;
                    TreeNode Node = new TreeNode(project.Name)
                    {
                        Tag = project.Id
                    };
                    treeView1.Nodes.Add(Node);
                    if (project.Subprojects.Count > 0)
                    {
                        TreeNode customerNode = new TreeNode(project.Name)
                        {
                            Tag = project.Id
                        };
                        WalkNode(project.Subprojects, ref customerNode, ref FounNode, bugReport.ProjectName);
                        treeView1.Nodes[i].Nodes.Add(customerNode);
                    }

                    if (i == 1 || Node.Text == bugReport.ProjectName)
                    {
                        treeView1.SelectedNode = Node;
                        treeView1.ExpandAll();
                    }
                    if (FounNode != null && FounNode.Text == bugReport.ProjectName)
                    {
                        treeView1.SelectedNode = FounNode;
                        treeView1.ExpandAll();
                    }
                    i++;
                }

                this.treeView1.AfterSelect += new TreeViewEventHandler(TreeView1_AfterSelect);
                populating = false;

                PopulateProjectDependentFields();

                priorityComboBox.DataSource = session.Config.PriorityEnum.GetLabels();
                severityComboBox.DataSource = session.Config.SeverityEnum.GetLabels();
                reproducibilityComboBox.DataSource = session.Config.ReproducibilityEnum.GetLabels();
                if (!string.IsNullOrWhiteSpace(bugReport.LogPath))
                    attachmentTextBox.Text = bugReport.LogPath;
                if (!string.IsNullOrWhiteSpace(bugReport.Summary))
                    summaryTextBox.Text = bugReport.Summary;
                if (!string.IsNullOrWhiteSpace(bugReport.Severity))
                    severityComboBox.SelectedIndex = severityComboBox.FindStringExact(bugReport.Severity);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Webservice Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        // Handle the After_Select event.
        private void TreeView1_AfterSelect(Object sender, TreeViewEventArgs e)
        {
            TreeView TV = (TreeView)sender;
            treeView1.SelectedNode = TV.SelectedNode;

            PopulateProjectDependentFields();
        }

        /// <summary>
        /// Walks the nodes to populate Projects
        /// </summary>
        /// <param name="projects">The projects.</param>
        /// <param name="Tn">The tn.</param>
        private void WalkNode(List<Project> projects, ref TreeNode Tn, ref TreeNode FounNode, string NameToFind)
        {
            for (int i = 0; i < projects.Count; i++)
            {
                TreeNode Node = new TreeNode(projects[i].Name)
                {
                    Tag = projects[i].Id
                };
                Tn.Nodes.Add(Node);
                if (projects[i].Name == NameToFind)
                    FounNode = Node;
                if (projects[i].Subprojects.Count > 0)
                    WalkNode(projects[i].Subprojects, ref Tn, ref FounNode, NameToFind);
            }
        }

        #endregion Private Methods
    }
}