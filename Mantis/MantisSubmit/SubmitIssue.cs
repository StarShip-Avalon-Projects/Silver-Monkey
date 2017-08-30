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

using SilverMonkey.BugTraqConnect;
using SilverMonkey.BugTraqConnect.Libs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Futureware.MantisSubmit
{
    /// <summary>
    /// A Windows form that allows submitting of issues in a Mantis installation.
    /// </summary>
    public class SubmitIssueForm : System.Windows.Forms.Form
    {
        #region Private Fields

        private TextBox attachmentTextBox;
        private Button browseButton;
        private System.Windows.Forms.ComboBox categoryComboBox;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private System.Windows.Forms.TextBox descriptionTextBox;
        private TextBox firstCustomFieldTextBox;
        private System.Windows.Forms.Label label1;
        private Label label10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private Label lblCustomField1;
        private Label lblCustomField2;
        private OpenFileDialog openFileDialog1;

        /// <summary>
        /// Tracks whether the projects combobox is currently being
        /// populated or not.
        /// </summary>
        /// <remarks>
        /// If being populated, then selection change event for the current
        /// project is ignored.
        /// </remarks>
        private bool populating;

        private System.Windows.Forms.ComboBox priorityComboBox;
        private System.Windows.Forms.ComboBox reproducibilityComboBox;
        private TextBox secondCustomFieldTextBox;

        /// <summary>
        /// Session used to communicate with MantisConnect.
        /// </summary>
        private Session session;

        private System.Windows.Forms.ComboBox severityComboBox;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.TextBox summaryTextBox;
        private TreeView treeView1;
        private StatusBarPanel statusBarPanel;
        private StatusBar statusBar;
        private System.Windows.Forms.ComboBox versionComboBox;

        #endregion Private Fields

        #region Public Constructors

        public SubmitIssueForm()
        {
            populating = false;
            // Required for Windows Form Designer support
            InitializeComponent();
            ErrorLog = new ProjectReport();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="errorLog"></param>
        public SubmitIssueForm(ProjectReport errorLog) : this()
        {
            // Required for Windows Form Designer support
            ErrorLog = errorLog;
        }

        private ProjectReport ErrorLog;

        #endregion Public Constructors

        #region Public Methods

        public void walkNode(List<Project> projects, ref TreeNode Tn, string SearchText, ref TreeNode FoundNode)
        {
            for (int i = 0; i < projects.Count; i++)
            {
                TreeNode Node = new TreeNode(projects[i].Name);
                Node.Tag = projects[i].Id;
                Tn.Nodes.Add(Node);
                if (projects[i].Name == SearchText)
                    FoundNode = Node;
                if (projects[i].Subprojects.Count > 0)
                    walkNode(projects[i].Subprojects, ref Tn, SearchText, ref FoundNode);
            }
        }

        #endregion Public Methods

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
            this.summaryTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.priorityComboBox = new System.Windows.Forms.ComboBox();
            this.severityComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.reproducibilityComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.versionComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblCustomField1 = new System.Windows.Forms.Label();
            this.firstCustomFieldTextBox = new System.Windows.Forms.TextBox();
            this.secondCustomFieldTextBox = new System.Windows.Forms.TextBox();
            this.lblCustomField2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.attachmentTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.statusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.statusBar = new System.Windows.Forms.StatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // summaryTextBox
            // 
            this.summaryTextBox.Location = new System.Drawing.Point(136, 282);
            this.summaryTextBox.Name = "summaryTextBox";
            this.summaryTextBox.Size = new System.Drawing.Size(646, 26);
            this.summaryTextBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 282);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "Summary";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // submitButton
            // 
            this.submitButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.submitButton.Location = new System.Drawing.Point(533, 604);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(120, 33);
            this.submitButton.TabIndex = 9;
            this.submitButton.Text = "Submit";
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(317, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 31);
            this.label2.TabIndex = 3;
            this.label2.Text = "Priority";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // priorityComboBox
            // 
            this.priorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityComboBox.Location = new System.Drawing.Point(483, 161);
            this.priorityComboBox.Name = "priorityComboBox";
            this.priorityComboBox.Size = new System.Drawing.Size(281, 28);
            this.priorityComboBox.TabIndex = 4;
            // 
            // severityComboBox
            // 
            this.severityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.severityComboBox.Location = new System.Drawing.Point(483, 195);
            this.severityComboBox.Name = "severityComboBox";
            this.severityComboBox.Size = new System.Drawing.Size(281, 28);
            this.severityComboBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(317, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 31);
            this.label3.TabIndex = 5;
            this.label3.Text = "Severity";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // reproducibilityComboBox
            // 
            this.reproducibilityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reproducibilityComboBox.Location = new System.Drawing.Point(483, 229);
            this.reproducibilityComboBox.Name = "reproducibilityComboBox";
            this.reproducibilityComboBox.Size = new System.Drawing.Size(281, 28);
            this.reproducibilityComboBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(317, 227);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 31);
            this.label4.TabIndex = 7;
            this.label4.Text = "Reproducibility";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 34);
            this.label5.TabIndex = 10;
            this.label5.Text = "Description";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(136, 319);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(634, 177);
            this.descriptionTextBox.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(52, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(160, 30);
            this.label6.TabIndex = 11;
            this.label6.Text = "Project";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(51, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(578, 57);
            this.label7.TabIndex = 13;
            this.label7.Text = "Ts Projects Bugtraq Submit";
            // 
            // versionComboBox
            // 
            this.versionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.versionComboBox.Location = new System.Drawing.Point(483, 93);
            this.versionComboBox.Name = "versionComboBox";
            this.versionComboBox.Size = new System.Drawing.Size(281, 28);
            this.versionComboBox.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(317, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(160, 31);
            this.label8.TabIndex = 16;
            this.label8.Text = "Version";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.categoryComboBox.Location = new System.Drawing.Point(483, 127);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(281, 28);
            this.categoryComboBox.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(317, 123);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(160, 31);
            this.label9.TabIndex = 18;
            this.label9.Text = "Category";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomField1
            // 
            this.lblCustomField1.Location = new System.Drawing.Point(7, 502);
            this.lblCustomField1.Name = "lblCustomField1";
            this.lblCustomField1.Size = new System.Drawing.Size(123, 33);
            this.lblCustomField1.TabIndex = 19;
            this.lblCustomField1.Text = "Custom Field 1";
            this.lblCustomField1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // firstCustomFieldTextBox
            // 
            this.firstCustomFieldTextBox.Enabled = false;
            this.firstCustomFieldTextBox.Location = new System.Drawing.Point(136, 502);
            this.firstCustomFieldTextBox.Name = "firstCustomFieldTextBox";
            this.firstCustomFieldTextBox.Size = new System.Drawing.Size(646, 26);
            this.firstCustomFieldTextBox.TabIndex = 20;
            // 
            // secondCustomFieldTextBox
            // 
            this.secondCustomFieldTextBox.Enabled = false;
            this.secondCustomFieldTextBox.Location = new System.Drawing.Point(136, 539);
            this.secondCustomFieldTextBox.Name = "secondCustomFieldTextBox";
            this.secondCustomFieldTextBox.Size = new System.Drawing.Size(646, 26);
            this.secondCustomFieldTextBox.TabIndex = 22;
            // 
            // lblCustomField2
            // 
            this.lblCustomField2.Location = new System.Drawing.Point(7, 535);
            this.lblCustomField2.Name = "lblCustomField2";
            this.lblCustomField2.Size = new System.Drawing.Size(123, 34);
            this.lblCustomField2.TabIndex = 21;
            this.lblCustomField2.Text = "Custom Field 2";
            this.lblCustomField2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // attachmentTextBox
            // 
            this.attachmentTextBox.Location = new System.Drawing.Point(136, 572);
            this.attachmentTextBox.Name = "attachmentTextBox";
            this.attachmentTextBox.Size = new System.Drawing.Size(646, 26);
            this.attachmentTextBox.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(7, 569);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(121, 33);
            this.label10.TabIndex = 23;
            this.label10.Text = "Attachment";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(807, 535);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(46, 33);
            this.browseButton.TabIndex = 25;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(47, 123);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(248, 135);
            this.treeView1.TabIndex = 26;
            // 
            // statusBarPanel
            // 
            this.statusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel.Name = "statusBarPanel";
            this.statusBarPanel.Width = 840;
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 651);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(865, 32);
            this.statusBar.TabIndex = 14;
            // 
            // SubmitIssueForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
            this.ClientSize = new System.Drawing.Size(865, 683);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.attachmentTextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.secondCustomFieldTextBox);
            this.Controls.Add(this.lblCustomField2);
            this.Controls.Add(this.firstCustomFieldTextBox);
            this.Controls.Add(this.lblCustomField1);
            this.Controls.Add(this.categoryComboBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.versionComboBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.summaryTextBox);
            this.Controls.Add(this.reproducibilityComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.severityComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.priorityComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(887, 739);
            this.MinimumSize = new System.Drawing.Size(887, 739);
            this.Name = "SubmitIssueForm";
            this.Text = "Mantis Connect - Submit Issue";
            this.Load += new System.EventHandler(this.SubmitIssue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion Windows Form Designer generated code

        #region Private Methods

        private void browseButton_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FileName = this.attachmentTextBox.Text;
            this.openFileDialog1.CheckFileExists = true;

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.attachmentTextBox.Text = this.openFileDialog1.FileName;
            }
        }

        /// <summary>
        /// Populates the list of categories and versions based on the
        /// currently selected projects.
        /// </summary>
        private void PopulateProjectDependentFields()
        {
            //if (populating == true)
            //    return;
            try
            {
                int projectId = (int)treeView1.SelectedNode.Tag;

                this.lblCustomField1.Text = "Custom Field 1";
                this.lblCustomField2.Text = "Custom Field 2";

                if (projectId == 0)
                {
                    categoryComboBox.DataSource = null;
                    versionComboBox.DataSource = null;
                }
                else
                {
                    categoryComboBox.DataSource = session.Request.ProjectGetCategories(projectId);
                    categoryComboBox.DisplayMember = "Name";
                    versionComboBox.DataSource = session.Request.ProjectGetVersionsReleased(projectId);
                    versionComboBox.DisplayMember = "Name";
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
            summaryTextBox.Clear();
            summaryTextBox.Focus();
            descriptionTextBox.Clear();
            attachmentTextBox.Clear();
        }

        /// <summary>
        /// Event handler for clicking the submit button.
        /// </summary>
        /// <param name="sender">
        /// not used
        /// </param>
        /// <param name="e">
        /// not used
        /// </param>
        private void submitButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                string attachment = this.attachmentTextBox.Text;

                if (attachment.Length > 0 && !File.Exists(attachment))
                {
                    MessageBox.Show(String.Format("File '{0}' doesn't exist", attachment));
                    return;
                }

                statusBar.Panels[0].Text = "Checking if issue already reported...";

                // Check if issue was previously logged in Mantis.
                int issueId = session.Request.IssueGetIdFromSummary(summaryTextBox.Text);
                if (issueId > 0)
                {
                    statusBar.Panels[0].Text = string.Format("'{0}' already reported in issue {1}", summaryTextBox.Text, issueId);
                    return;
                }

                // Create the issue in memory
                Issue issue = new Issue();

                issue.Project = new ObjectRef((int)treeView1.SelectedNode.Tag);
                issue.Priority = new ObjectRef(priorityComboBox.Text);
                issue.Severity = new ObjectRef(severityComboBox.Text);
                issue.Reproducibility = new ObjectRef(reproducibilityComboBox.Text);
                issue.Category = new ObjectRef(categoryComboBox.Text);
                issue.ProductVersion = versionComboBox.Text;
                issue.Summary = summaryTextBox.Text;
                issue.Description = descriptionTextBox.Text;
                issue.ReportedBy = new User();
                issue.ReportedBy.Name = session.Username;

                statusBar.Panels[0].Text = "Submitting issue...";

                int newIssueId = session.Request.IssueAdd(issue);

                statusBar.Panels[0].Text = String.Format("Submitting attachment to issue {0}...", newIssueId);

                if (attachment.Length > 0)
                {
                    session.Request.IssueAttachmentAdd(newIssueId, attachment, null);
                }

                // Submit the issue and show its id in the status bar
                statusBar.Panels[0].Text = string.Format("Issued added as {0}.", newIssueId);

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

                string mantisConnectUrl = appSettings["MantisConnectUrl"];
                string mantisUserName = appSettings["MantisUserName"];
                string mantisPassword = appSettings["MantisPassword"];

                session = new Session(mantisConnectUrl, mantisUserName, mantisPassword, nc);

                session.Connect();

                populating = true;
                int i = 0;
                TreeNode SmNode = null;
                // Project project2 = session.Request.
                foreach (Project project in session.Request.UserGetAccessibleProjects())
                {
                    TreeNode Node = new TreeNode(project.Name);

                    Node.Tag = project.Id;
                    treeView1.Nodes.Add(Node);
                    if (project.Subprojects.Count > 0)
                    {
                        TreeNode customerNode = new TreeNode(project.Name);

                        customerNode.Tag = project.Id;
                        walkNode(project.Subprojects, ref customerNode, ErrorLog.ProcuctName, ref SmNode);
                        treeView1.Nodes[i].Nodes.Add(customerNode);
                        if (customerNode.Text == ErrorLog.ProcuctName && SmNode == null)
                        {
                            SmNode = customerNode;
                        }
                    }

                    i++;
                }

                this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterSelect);
                if (SmNode != null)
                    treeView1.SelectedNode = SmNode;

                PopulateProjectDependentFields();
                treeView1.Update();
                priorityComboBox.DataSource = session.Config.PriorityEnum.GetLabels();
                severityComboBox.DataSource = session.Config.SeverityEnum.GetLabels();

                reproducibilityComboBox.DataSource = session.Config.ReproducibilityEnum.GetLabels();

                descriptionTextBox.Text = ErrorLog.ReportDescription;
                summaryTextBox.Text = ErrorLog.ReportSubject;
                versionComboBox.SelectedItem = ErrorLog.ProductVersion;
                attachmentTextBox.Text = ErrorLog.AttachmentFile;
                if (!string.IsNullOrEmpty(ErrorLog.Severity))
                    severityComboBox.SelectedText = ErrorLog.Severity;
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


        #endregion Private Methods


    }
}