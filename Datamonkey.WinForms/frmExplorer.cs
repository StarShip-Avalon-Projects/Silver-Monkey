using DataMonkey.Controls;
using FastColoredTextBoxNS;
using MonkeyCore;
using MonkeyCore.Controls;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MonkeyCore.Data;
using Controls;

namespace DataMonkey
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'frmExplorer'

    public class frmExplorer : Form
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'frmExplorer'
    {
        #region Members

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'frmExplorer.SqlResultsListView'
        public ListView_NoFlicker SqlResultsListView;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'frmExplorer.SqlResultsListView'

        //Database String
        private static string ActiveDatabaseLocation;

        private MenuItem AddAreaMenu;
        private MenuItem CheckIntegrity;
        private System.ComponentModel.IContainer components;
        private MenuItem CreateDBMenu;
        private TreeView DatabaseTreeView;
        private ToolBarButton ExecuteSQL;
        private MenuItem ExitAppMenu;
        private ToolBarButton IntegrityCheck;
        private MainMenu mainMenu1;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private MenuItem menuItem6;
        private MenuItem objAddColumnSQL;
        private MenuItem objCreateTableSQL;

        //Listview Menu
        private MenuItem objDeleteRowSQL;

        private MenuItem objDeleteTableSQL;

        //Text Box Menu
        private MenuItem objExecuteSQL;

        //Treeview Menu
        private MenuItem objOpenTableSQL;

        private MenuItem objRemoveColumnSQL;
        private MenuItem objRenameTableSQL;
        private ToolBarButton OpenDatabase;
        private MenuItem OpenDBmenu;
        private ToolBarButton Separator;

        private SplitContainer splitContainer1;

        private SplitContainer splitContainer2;

        private TabControlEx SQLAreaTabControl;

        private FastColoredTextBox sqlStatementTextBox;

        private StatusStrip statusStrip1;

        private ToolStripStatusLabel StatusStripLog;

        private TabControl tabControl1;

        //Only access from BuildSqlResultsListView
        private string TableName = "";

        private TabPage tabPage2;

        private TabPage tabPage3;

        private ToolBar toolBar1;

        private ImageList ToolBarImages;

        //Menu Members
        private ContextMenu TreeViewContextMenu;

        private ContextMenu TreeViewTablesMenu;

        #endregion Members

        #region Constructor / Destructor

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'frmExplorer.frmExplorer()'

        public frmExplorer()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'frmExplorer.frmExplorer()'
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            //
            //sqlStatementTextBox
            //
            SQLAreaTabControl.TabPages.Add(GenerateTabPage());

            sqlStatementTextBox.ContextMenu = new ContextMenu();
            sqlStatementTextBox.ContextMenu.MenuItems.Add(objExecuteSQL);
            var HelpItems = new MonkeyCore.Controls.HelpLinkMenu();
            menuItem6.MenuItems.AddRange(HelpItems.MenuItems.ToArray());
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

        #endregion Constructor / Destructor

        #region Windows Form Designer generated code

        private void CloseAllButThis()
        {
            TabPage tb = this.SQLAreaTabControl.SelectedTab;
            foreach (TabPage tab in SQLAreaTabControl.TabPages)
                if (!tab.Equals(tb))
                {
                    CloseTab(tab);
                }
        }

        private void CloseTab(Button btn)
        {
            CloseTab((TabPage)btn.Tag);
        }

        private void CloseTab(TabPage tb)
        {
            if (SQLAreaTabControl.TabCount == 0)
                return;

            SQLAreaTabControl.TabPages.Remove(tb);
            SQLAreaTabControl.RePositionCloseButtons();
            GC.Collect();
            if (SQLAreaTabControl.TabPages.Count == 0 & Disposing == false)
            {
                SQLAreaTabControl.TabPages.Add(GenerateTabPage());
            }
        }

        private void DatabaseTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (DatabaseTreeView.SelectedNode != null &&
                DatabaseTreeView.SelectedNode.Parent != null &&
                DatabaseTreeView.SelectedNode.Parent.Text.ToLower().Equals("tables"))
            {
                objOpenTableSQL.PerformClick();
            }
        }

        private void FCloseAllTab_Click(object sender, EventArgs e)
        {
            CloseAllButThis();
        }

        private void FCloseTab_Click(object sender, EventArgs e)
        {
            CloseTab(SQLAreaTabControl.SelectedTab);
        }

        private void FNewTab_Click(object sender, EventArgs e)
        {
            SQLAreaTabControl.TabPages.Add(GenerateTabPage());
        }

        /// <summary>
        /// Required method for Designer support - do not modify the
        /// contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExplorer));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.OpenDBmenu = new System.Windows.Forms.MenuItem();
            this.CreateDBMenu = new System.Windows.Forms.MenuItem();
            this.CheckIntegrity = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.ExitAppMenu = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.AddAreaMenu = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.objExecuteSQL = new System.Windows.Forms.MenuItem();
            this.objOpenTableSQL = new System.Windows.Forms.MenuItem();
            this.objRenameTableSQL = new System.Windows.Forms.MenuItem();
            this.objAddColumnSQL = new System.Windows.Forms.MenuItem();
            this.objRemoveColumnSQL = new System.Windows.Forms.MenuItem();
            this.objDeleteRowSQL = new System.Windows.Forms.MenuItem();
            this.objCreateTableSQL = new System.Windows.Forms.MenuItem();
            this.objDeleteTableSQL = new System.Windows.Forms.MenuItem();
            this.TreeViewContextMenu = new System.Windows.Forms.ContextMenu();
            this.TreeViewTablesMenu = new System.Windows.Forms.ContextMenu();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.OpenDatabase = new System.Windows.Forms.ToolBarButton();
            this.IntegrityCheck = new System.Windows.Forms.ToolBarButton();
            this.Separator = new System.Windows.Forms.ToolBarButton();
            this.ExecuteSQL = new System.Windows.Forms.ToolBarButton();
            this.ToolBarImages = new System.Windows.Forms.ImageList(this.components);
            this.sqlStatementTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusStripLog = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.DatabaseTreeView = new System.Windows.Forms.TreeView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.SQLAreaTabControl = new TabControlEx();
            this.SqlResultsListView = new MonkeyCore.Controls.ListView_NoFlicker();
            ((System.ComponentModel.ISupportInitialize)(this.sqlStatementTextBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // mainMenu1
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem6,
            this.menuItem4});
            // menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OpenDBmenu,
            this.CreateDBMenu,
            this.CheckIntegrity,
            this.menuItem3,
            this.ExitAppMenu});
            this.menuItem1.Text = "File";
            // OpenDBmenu
            this.OpenDBmenu.Index = 0;
            this.OpenDBmenu.Text = "Open Database";
            this.OpenDBmenu.Click += new System.EventHandler(this.OpenDBmenu_Click);
            // CreateDBMenu
            this.CreateDBMenu.Index = 1;
            this.CreateDBMenu.Text = "Create Database";
            this.CreateDBMenu.Click += new System.EventHandler(this.CreateDBMenu_Click);
            // CheckIntegrity
            this.CheckIntegrity.Index = 2;
            this.CheckIntegrity.Text = "Check DB Integrity";
            this.CheckIntegrity.Click += new System.EventHandler(this.CheckIntegrity_Click);
            // menuItem3
            this.menuItem3.Index = 3;
            this.menuItem3.Text = "-";
            // ExitAppMenu
            this.ExitAppMenu.Index = 4;
            this.ExitAppMenu.Text = "Exit";
            this.ExitAppMenu.Click += new System.EventHandler(this.ExitAppMenu_Click);
            // menuItem2
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.AddAreaMenu});
            this.menuItem2.Text = "SQLArea";
            // AddAreaMenu
            this.AddAreaMenu.Index = 0;
            this.AddAreaMenu.Text = "Add Area";
            this.AddAreaMenu.Click += new System.EventHandler(this.AddAreaMenu_Click);
            // menuItem6
            this.menuItem6.Index = 2;
            this.menuItem6.Text = "ReferenceLinks";
            // menuItem4
            this.menuItem4.Index = 3;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem5});
            this.menuItem4.Text = "Help";
            // menuItem5
            this.menuItem5.Index = 0;
            this.menuItem5.Text = "Contents";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // objExecuteSQL
            this.objExecuteSQL.Index = -1;
            this.objExecuteSQL.Text = "Execute";
            this.objExecuteSQL.Click += new System.EventHandler(this.objExecuteSQL_Click);
            // objOpenTableSQL
            this.objOpenTableSQL.Index = 0;
            this.objOpenTableSQL.Text = "Open Table";
            this.objOpenTableSQL.Click += new System.EventHandler(this.objOpenTableSQL_Click);
            // objRenameTableSQL
            this.objRenameTableSQL.Index = 1;
            this.objRenameTableSQL.Text = "Rename";
            this.objRenameTableSQL.Click += new System.EventHandler(this.objRenameTableSQL_Click);
            // objAddColumnSQL
            this.objAddColumnSQL.Index = 2;
            this.objAddColumnSQL.Text = "Add Column";
            this.objAddColumnSQL.Click += new System.EventHandler(this.objAddColumnSQL_Click);
            // objRemoveColumnSQL
            this.objRemoveColumnSQL.Index = 3;
            this.objRemoveColumnSQL.Text = "Remove Column";
            this.objRemoveColumnSQL.Click += new System.EventHandler(this.objRemoveColumnSQL_Click);
            // objDeleteRowSQL
            this.objDeleteRowSQL.Index = -1;
            this.objDeleteRowSQL.Text = "Delete Row";
            this.objDeleteRowSQL.Click += new System.EventHandler(this.objDeleteRowSQL_Click);
            // objCreateTableSQL
            this.objCreateTableSQL.Index = 0;
            this.objCreateTableSQL.Text = "Create Table";
            this.objCreateTableSQL.Click += new System.EventHandler(this.objCreateTableSQL_Click);
            // objDeleteTableSQL
            this.objDeleteTableSQL.Index = 4;
            this.objDeleteTableSQL.Text = "Delete Table";
            this.objDeleteTableSQL.Click += new System.EventHandler(this.objDeleteTableSQL_Click);
            // TreeViewContextMenu
            this.TreeViewContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.objOpenTableSQL,
            this.objRenameTableSQL,
            this.objAddColumnSQL,
            this.objRemoveColumnSQL,
            this.objDeleteTableSQL});
            // TreeViewTablesMenu
            this.TreeViewTablesMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.objCreateTableSQL});
            // toolBar1
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.OpenDatabase,
            this.IntegrityCheck,
            this.Separator,
            this.ExecuteSQL});
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.ToolBarImages;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(840, 28);
            this.toolBar1.TabIndex = 11;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // OpenDatabase
            this.OpenDatabase.ImageIndex = 0;
            this.OpenDatabase.Name = "OpenDatabase";
            this.OpenDatabase.Tag = "OpenDatabase";
            this.OpenDatabase.ToolTipText = "Open Database";
            // IntegrityCheck
            this.IntegrityCheck.ImageIndex = 3;
            this.IntegrityCheck.Name = "IntegrityCheck";
            this.IntegrityCheck.Tag = "IntegrityCheck";
            this.IntegrityCheck.ToolTipText = "Integrity Check";
            // Separator
            this.Separator.Name = "Separator";
            this.Separator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // ExecuteSQL
            this.ExecuteSQL.ImageIndex = 1;
            this.ExecuteSQL.Name = "ExecuteSQL";
            this.ExecuteSQL.Tag = "ExecuteSQL";
            this.ExecuteSQL.ToolTipText = "Execute SQL";
            // ToolBarImages
            this.ToolBarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ToolBarImages.ImageStream")));
            this.ToolBarImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ToolBarImages.Images.SetKeyName(0, "");
            this.ToolBarImages.Images.SetKeyName(1, "");
            this.ToolBarImages.Images.SetKeyName(2, "");
            this.ToolBarImages.Images.SetKeyName(3, "");
            // sqlStatementTextBox
            this.sqlStatementTextBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.sqlStatementTextBox.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.sqlStatementTextBox.BackBrush = null;
            this.sqlStatementTextBox.CharHeight = 14;
            this.sqlStatementTextBox.CharWidth = 8;
            this.sqlStatementTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.sqlStatementTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.sqlStatementTextBox.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.sqlStatementTextBox.IsReplaceMode = false;
            this.sqlStatementTextBox.Location = new System.Drawing.Point(0, 0);
            this.sqlStatementTextBox.Name = "sqlStatementTextBox";
            this.sqlStatementTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.sqlStatementTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.sqlStatementTextBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("sqlStatementTextBox.ServiceColors")));
            this.sqlStatementTextBox.Size = new System.Drawing.Size(150, 150);
            this.sqlStatementTextBox.TabIndex = 0;
            this.sqlStatementTextBox.Zoom = 100;
            // statusStrip1
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusStripLog});
            this.statusStrip1.Location = new System.Drawing.Point(0, 390);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(840, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // StatusStripLog
            this.StatusStripLog.Name = "StatusStripLog";
            this.StatusStripLog.Size = this.statusStrip1.Size;
            this.StatusStripLog.Text = "Execute: Ready";
            // splitContainer1
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // splitContainer1.Panel1
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // splitContainer1.Panel2
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(840, 362);
            this.splitContainer1.SplitterDistance = 280;
            this.splitContainer1.TabIndex = 19;
            // tabControl1
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(280, 362);
            this.tabControl1.TabIndex = 1;
            // tabPage2
            this.tabPage2.Controls.Add(this.DatabaseTreeView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(272, 336);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Database";
            this.tabPage2.UseVisualStyleBackColor = true;
            // DatabaseTreeView
            this.DatabaseTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DatabaseTreeView.Location = new System.Drawing.Point(3, 3);
            this.DatabaseTreeView.Name = "DatabaseTreeView";
            this.DatabaseTreeView.Size = new System.Drawing.Size(266, 330);
            this.DatabaseTreeView.TabIndex = 1;
            this.DatabaseTreeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.objOpenTableSQL_Click);
            this.DatabaseTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DatabaseTreeView_MouseDown);
            // tabPage3
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(272, 336);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Templates";
            this.tabPage3.UseVisualStyleBackColor = true;
            // splitContainer2
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // splitContainer2.Panel1
            this.splitContainer2.Panel1.Controls.Add(this.SQLAreaTabControl);
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(5);
            // splitContainer2.Panel2
            this.splitContainer2.Panel2.Controls.Add(this.SqlResultsListView);
            this.splitContainer2.Size = new System.Drawing.Size(556, 362);
            this.splitContainer2.SplitterDistance = 170;
            this.splitContainer2.TabIndex = 21;
            // SQLAreaTabControl
            this.SQLAreaTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SQLAreaTabControl.Location = new System.Drawing.Point(5, 5);
            this.SQLAreaTabControl.Name = "SQLAreaTabControl";
            this.SQLAreaTabControl.SelectedIndex = 0;
            this.SQLAreaTabControl.ShowCloseButtonOnTabs = true;
            this.SQLAreaTabControl.Size = new System.Drawing.Size(546, 160);
            this.SQLAreaTabControl.TabIndex = 0;
            this.SQLAreaTabControl.CloseButtonClick += new System.ComponentModel.CancelEventHandler(this.SQLAreaTabControl_CloseButtonClick);
            this.SQLAreaTabControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SQLAreaTabControl_MouseDown);
            // SqlResultsListView
            this.SqlResultsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SqlResultsListView.FullRowSelect = true;
            this.SqlResultsListView.GridLines = true;
            this.SqlResultsListView.LabelEdit = true;
            this.SqlResultsListView.LargeImageList = this.ToolBarImages;
            this.SqlResultsListView.Location = new System.Drawing.Point(0, 0);
            this.SqlResultsListView.Name = "SqlResultsListView";
            this.SqlResultsListView.Size = new System.Drawing.Size(556, 188);
            this.SqlResultsListView.TabIndex = 0;
            this.SqlResultsListView.UseCompatibleStateImageBehavior = false;
            this.SqlResultsListView.View = System.Windows.Forms.View.Details;
            // frmExplorer
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(840, 412);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "frmExplorer";
            this.Text = "TSProjects: Data Monkey";
            ((System.ComponentModel.ISupportInitialize)(this.sqlStatementTextBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void SQLAreaTabControl_CloseButtonClick(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Button t = (Button)sender;
            CloseTab(t);
        }

        private void SQLAreaTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            Control z = (Control)sender;
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip x = new ContextMenuStrip();
                ToolStripItem s = x.Items.Add("New Tab", null, FNewTab_Click);
                s.Tag = sender;
                ToolStripItem t = x.Items.Add("Close All Other Tabs", null, FCloseAllTab_Click);
                ToolStripItem v = x.Items.Add("Close Tab", null, FCloseTab_Click);

                x.Show(z, e.Location);
                int tabPageIndex = 0;
                for (int i = 0; i <= SQLAreaTabControl.TabPages.Count - 1; i++)
                {
                    if (SQLAreaTabControl.GetTabRect(i).Contains(e.X, e.Y))
                    {
                        tabPageIndex = i;
                        break;
                    }
                }
                t.Tag = tabPageIndex;
                v.Tag = tabPageIndex;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                for (int i = 0; i <= SQLAreaTabControl.TabPages.Count - 1; i++)
                {
                    if (SQLAreaTabControl.GetTabRect(i).Contains(e.X, e.Y))
                    {
                        CloseTab(SQLAreaTabControl.TabPages[i]);
                        break;
                    }
                }
            }
        }

        #endregion Windows Form Designer generated code

        #region Private Methods

        #region PopulateDatabaseTreeView

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'frmExplorer.PopulateDatabaseTreeView()'

        public void PopulateDatabaseTreeView()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'frmExplorer.PopulateDatabaseTreeView()'
        {
            DatabaseTreeView.Nodes.Clear();

            string DatabaseName = Path.GetFileNameWithoutExtension(ActiveDatabaseLocation);

            TreeNode topNode = new TreeNode();
            topNode.Text = DatabaseName;
            topNode.Tag = "DatabaseName";

            TreeNode tablesNode = new TreeNode();
            tablesNode.Text = "Tables";
            tablesNode.Tag = "Tables";

            DataSet ds = null;
            string message;
            StatementParser.ReturnResults(StatementBuilder.BuildMasterQuery(), ActiveDatabaseLocation, ref ds, out message);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string TableName = dr[0].ToString();
                    TreeNode tableNode = new TreeNode();
                    tableNode.Text = TableName;
                    tableNode.Tag = "Table";
                    tableNode.Nodes.Add(new TreeNode("Columns"));

                    tablesNode.Nodes.Add(tableNode);
                }
            }

            tablesNode.Expand();
            topNode.Expand();

            //Add the Tables Node to the Top Node
            topNode.Nodes.Add(tablesNode);

            //Add the topNode to the TreeView
            DatabaseTreeView.Nodes.Add(topNode);
        }

        #endregion PopulateDatabaseTreeView

        #region GenerateSQLArea TabPage

        private TabPage GenerateTabPage()
        {
            FastColoredTextBox tempTextBox = new SilverMonkeyFCTB();
            TabPage tempTabPage = new TabPage();
            // sqlStatementTextBox
            tempTextBox.Dock = DockStyle.Fill;
            tempTextBox.Location = new Point(0, 0);
            tempTextBox.Multiline = true;
            tempTextBox.Size = new Size(608, 200);
            tempTextBox.ContextMenu = new ContextMenu();
            tempTextBox.ContextMenu.MenuItems.Add(objExecuteSQL.CloneMenu());
            tempTextBox.Language = Language.SQL;

            tempTabPage.Controls.Add(tempTextBox);
            tempTabPage.Location = new Point(4, 22);
            tempTabPage.Size = new Size(608, 158);
            tempTabPage.Text = string.Format("SQL Command {0}", SQLAreaTabControl.TabCount + 1);

            return tempTabPage;
        }

        #endregion GenerateSQLArea TabPage

        #region BuildSqlResultsListView

        private void BuildSqlResultsListView(DataSet ds, string tableName)
        {
            SqlResultsListView.Items.Clear();
            SqlResultsListView.Columns.Clear();

            if (ds == null || ds.Tables.Count == 0)
                return;

            TableName = tableName;
            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                SqlResultsListView.Columns.Add(dc.ColumnName, 50, HorizontalAlignment.Left);
            }

            int iCounter = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SqlResultsListView.Items.Add(dr[0].ToString(), 0);

                for (int i = 1; i < dr.ItemArray.Length; i++)
                {
                    SqlResultsListView.Items[iCounter].SubItems.Add(dr[i].ToString());
                }

                //-- Assign alternating backcolor
                if (iCounter % 2 == 0)
                {
                    SqlResultsListView.Items[iCounter].BackColor = Color.AliceBlue;
                }

                iCounter++;
            }

            foreach (ColumnHeader ch in SqlResultsListView.Columns)
            {
                ch.Width = -2;
            }
            SqlResultsListView.Visible = true;
        }

        #endregion BuildSqlResultsListView

        #region IntegrityCheck

        private void IntegrityCheckSQL()
        {
            //GetTable Names
            DataSet ds = null;
            string sqlStatement = StatementBuilder.BuildIntegrityCheckSQL();

            //Place sqlstatement into the text box
            ((SilverMonkeyFCTB)SQLAreaTabControl.SelectedTab.Controls[0]).Text = sqlStatement;

            //Parse Results
            string message;
            StatementParser.ReturnResults(sqlStatement, ActiveDatabaseLocation, ref ds, out message);
            StatusStripLog.Text = message;

            //Build ListView
            BuildSqlResultsListView(ds, (DatabaseTreeView.SelectedNode == null ? "" : DatabaseTreeView.SelectedNode.Text));
        }

        #endregion IntegrityCheck

        #region ExecuteTextBoxSQL

        private void ExecuteTextBoxSQL()
        {
            //GetTable Names
            DataSet ds = null;
            string sqlStatement = ((SilverMonkeyFCTB)SQLAreaTabControl.SelectedTab.Controls[0]).Text;
            if (string.IsNullOrEmpty(sqlStatement))
                return;
            //Parse Results
            string message = null;
            string message2 = null;
            StatementParser.ReturnResults(sqlStatement, ActiveDatabaseLocation, ref ds, out message);

            //Get the tablename out of the txtbox Sqlstatement
            string TableName = ParseTableName(sqlStatement);
            if (ds == null)
            {
                //reload the ListView to see changes
                StatementParser.ReturnResults(string.Format("SELECT * FROM {0}", TableName), ActiveDatabaseLocation, ref ds, out message2);
                message2 = String.Format("Refresh Display: {0}", message2);
            }
            //Build ListView
            BuildSqlResultsListView(ds, TableName);
            StatusStripLog.Text = message + " " + message2;
        }

        #endregion ExecuteTextBoxSQL

        #region OpenDBFileLocator

        private void OpenDBFileLocator()
        {
            using (OpenFileDialog oFile = new OpenFileDialog())
            {
                oFile.Title = "Data Monkey Database Locator";
                oFile.InitialDirectory = IO.Paths.SilverMonkeyBotPath;
                oFile.Filter = "All files (*.*)|*.*|DB Files (*.db)|*.db";
                oFile.FilterIndex = 2;
                oFile.RestoreDirectory = true;
                if (oFile.ShowDialog() == DialogResult.OK)
                {
                    ActiveDatabaseLocation = oFile.FileName;
                    SQLiteDatabase db = new SQLiteDatabase(ActiveDatabaseLocation);
                    PopulateDatabaseTreeView();
                }
            }
        }

        #endregion OpenDBFileLocator

        #region CreateDBFile()

        private void CreateDBFile()
        {
            using (SaveFileDialog oFile = new SaveFileDialog())
            {
                oFile.Title = "Data Monkey Database Locator";
                oFile.InitialDirectory = IO.Paths.SilverMonkeyBotPath;
                oFile.Filter = "All files (*.*)|*.*|DB Files (*.db)|*.db";
                oFile.FilterIndex = 2;
                oFile.RestoreDirectory = true;
                if (oFile.ShowDialog() == DialogResult.OK)
                {
                    ActiveDatabaseLocation = oFile.FileName;
                    SQLiteDatabase db = new SQLiteDatabase(ActiveDatabaseLocation);
                    PopulateDatabaseTreeView();
                }
            }
        }

        #endregion CreateDBFile()

        #endregion Private Methods

        #region Menu Event

        private void AddAreaMenu_Click(object sender, EventArgs e)
        {
            SQLAreaTabControl.Controls.Add(GenerateTabPage());
        }

        private void CheckIntegrity_Click(object sender, EventArgs e)
        {
            IntegrityCheckSQL();
        }

        private void CreateDBMenu_Click(object sender, EventArgs e)
        {
            CreateDBFile();
        }

        private void ExitAppMenu_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenDBmenu_Click(object sender, EventArgs e)
        {
            OpenDBFileLocator();
        }

        #endregion Menu Event

        #region TreeView Events

        private void DatabaseTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null && e.Node.Parent.Text == "Tables")
            {
                string SQLStatement = "PRAGMA table_info(" + e.Node.Text + ")";

                //GetTable Names
                DataSet ds = null;

                string message;
                StatementParser.ReturnResults(SQLStatement, ActiveDatabaseLocation, ref ds, out message);
                StatusStripLog.Text = message;

                TreeNode columnsNode = e.Node.Nodes[0];
                columnsNode.Tag = "Columns";

                columnsNode.Nodes.Clear();

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string ColumnName = dr[1].ToString();
                        string ColumnType = dr[2].ToString();
                        TreeNode columnNode = new TreeNode();
                        columnNode.Text = ColumnName != null ? ColumnName + ", " + ColumnType : ColumnName;
                        columnNode.Tag = ColumnName;

                        columnsNode.Nodes.Add(columnNode);
                    }
                }

                columnsNode.Expand();
            }
        }

        private void DatabaseTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (DatabaseTreeView.SelectedNode != null)
                DatabaseTreeView.SelectedNode.BackColor = Color.White;
        }

        private void DatabaseTreeView_LostFocus(object sender, EventArgs e)
        {
            if (DatabaseTreeView.SelectedNode != null)
                DatabaseTreeView.SelectedNode.BackColor = Color.LightGray;
        }

        private void DatabaseTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                DatabaseTreeView.SelectedNode = DatabaseTreeView.GetNodeAt(p);
            }

            if (DatabaseTreeView.SelectedNode != null &&
                DatabaseTreeView.SelectedNode.Parent != null &&
                DatabaseTreeView.SelectedNode.Parent.Text.ToLower().Equals("tables"))
            {
                DatabaseTreeView.ContextMenu = TreeViewContextMenu;
            }
            else if (DatabaseTreeView.SelectedNode != null &&
                DatabaseTreeView.SelectedNode.Text.ToLower().Equals("tables"))
            {
                DatabaseTreeView.ContextMenu = TreeViewTablesMenu;
            }
            else
            {
                DatabaseTreeView.ContextMenu = null;
            }
        }

        private void objAddColumnSQL_Click(object sender, EventArgs e)
        {
            using (AddColumn pAddColumn = new AddColumn())
            {
                pAddColumn.ShowInTaskbar = false;
                if (DialogResult.OK == pAddColumn.ShowDialog() & !string.IsNullOrEmpty(pAddColumn.ColumnName))
                {
                    string LogMessage;
                    StatementParser.ReturnResults(StatementBuilder.BuildAddColumnSQL(DatabaseTreeView.SelectedNode.Text, pAddColumn.ColumnName, pAddColumn.ColumnType), ActiveDatabaseLocation, out LogMessage);

                    //Add new column to the tree if it is expanded
                    if (DatabaseTreeView.SelectedNode.IsExpanded)
                    {
                        TreeNode columnNode = new TreeNode();
                        columnNode.Text = pAddColumn.ColumnName + ", " + pAddColumn.ColumnType;
                        columnNode.Tag = pAddColumn.ColumnName;

                        DatabaseTreeView.SelectedNode.Nodes[0].Nodes.Add(columnNode);
                    }
                    StatusStripLog.Text = LogMessage;
                }
            }
        }

        private void objCreateTableSQL_Click(object sender, EventArgs e)
        {
            using (AddTable pAddTable = new AddTable())
            {
                string message;
                pAddTable.ShowInTaskbar = false;
                if (DialogResult.OK == pAddTable.ShowDialog())
                {
                    DataSet ds = null;

                    StatementParser.ReturnResults(StatementBuilder.BuildAddTableSQL(pAddTable.TableName), ActiveDatabaseLocation, ref ds, out message);

                    //Build TreeView
                    PopulateDatabaseTreeView();
                }
                else
                    message = "Create Table Aborted";
                StatusStripLog.Text = message;
            }
        }

        private void objDeleteTableSQL_Click(object sender, EventArgs e)
        {
            string TableName = DatabaseTreeView.SelectedNode.Text;
            string message;
            if (MessageBox.Show("Do you want to delete this table?", "Waring", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                StatementParser.ReturnResults(StatementBuilder.BuildTableDeleteSQL(TableName), ActiveDatabaseLocation, out message);
                DatabaseTreeView.SelectedNode.Remove();
            }
            else
                message = "Delete Table Aborted";
            StatusStripLog.Text = message;
        }

        private void objOpenTableSQL_Click(object sender, EventArgs e)
        {
            if (DatabaseTreeView.SelectedNode.Tag != null)
                if (DatabaseTreeView.SelectedNode.Tag.ToString() == "Table")
                {
                    //GetTable Names
                    DataSet ds = null;
                    string sqlStatement = StatementBuilder.BuildTableOpenSql(DatabaseTreeView.SelectedNode.Text);

                    //Place sqlstatement into the text box
                    if (!string.IsNullOrEmpty(((SilverMonkeyFCTB)SQLAreaTabControl.SelectedTab.Controls[0]).Text))
                    {
                        SQLAreaTabControl.TabPages.Add(GenerateTabPage());
                        SQLAreaTabControl.SelectTab(SQLAreaTabControl.TabCount - 1);
                    }
                    ((SilverMonkeyFCTB)SQLAreaTabControl.SelectedTab.Controls[0]).Text = sqlStatement;

                    //Parse Results
                    string LogMessage;
                    StatementParser.ReturnResults(sqlStatement, ActiveDatabaseLocation, ref ds, out LogMessage);

                    //Build ListView
                    BuildSqlResultsListView(ds, DatabaseTreeView.SelectedNode.Text);
                    StatusStripLog.Text = LogMessage;
                }
        }

        private void objRemoveColumnSQL_Click(object sender, EventArgs e)
        {
            using (RemoveColumn pRemoveColumn = new RemoveColumn())
            {
                pRemoveColumn.ShowInTaskbar = false;
                string message = null;
                if (DialogResult.OK == pRemoveColumn.ShowDialog() & !string.IsNullOrEmpty(pRemoveColumn.ColumnName))
                {
                    // StatementParser.ReturnResults(StatementBuilder.BuildAddColumnSQL(DatabaseTreeView.SelectedNode.Text,
                    // pRemoveColumn.ColumnName, pRemoveColumn.ColumnType), ActiveDatabaseLocation);
                    SQLiteDatabase db = new SQLiteDatabase(ActiveDatabaseLocation);
                    int Records = db.RemoveColumn(DatabaseTreeView.SelectedNode.Text, pRemoveColumn.ColumnName);

                    //Add new column to the tree if it is expanded
                    if (DatabaseTreeView.SelectedNode.IsExpanded & Records > -1)
                    {
                        TreeNode columnNode = new TreeNode();
                        columnNode.Text = pRemoveColumn.ColumnName;
                        columnNode.Tag = pRemoveColumn.ColumnName;
                        DatabaseTreeView.SelectedNode.Nodes[0].Nodes.Remove(columnNode);

                        DatabaseTreeView.SelectedNode.Collapse();
                        DatabaseTreeView.SelectedNode.Expand();
                    }
                    message = String.Format("ExecuteNonQurey: Records updated {0}", Records);
                }
                else
                    message = "Remove Column Aborted";
                StatusStripLog.Text = message;
            }
        }

        private void objRenameTableSQL_Click(object sender, EventArgs e)
        {
            using (RenameTable pRenameTable = new RenameTable())
            {
                pRenameTable.ShowInTaskbar = false;
                string message;
                if (DialogResult.OK == pRenameTable.ShowDialog())
                {
                    if (StatementParser.ReturnResults(StatementBuilder.BuildRenameTableSQL(DatabaseTreeView.SelectedNode.Text, pRenameTable.NewTableName), ActiveDatabaseLocation, out message))
                        DatabaseTreeView.SelectedNode.Text = pRenameTable.NewTableName;
                }
                else
                    message = "Rename Table aborted";
                StatusStripLog.Text = message;
            }
        }

        #endregion TreeView Events

        #region TextBox Events

        private void objExecuteSQL_Click(object sender, EventArgs e)
        {
            ExecuteTextBoxSQL();
            PopulateDatabaseTreeView();
        }

        private string ParseTableName(string SQLStatement)
        {
            string tableName = "";
            int iofTable = 0;
            int iofEndTableName = 0;

            iofTable = SQLStatement.ToLower().IndexOf(" from ");
            if (iofTable > -1)
                iofTable += 6;
            else
            {
                iofTable = SQLStatement.ToLower().IndexOf(" table ");
                if (iofTable > -1)
                    iofTable += 7;
            }

            if (iofTable > -1)
            {
                string t = SQLStatement.Substring(iofTable, SQLStatement.Length - iofTable);
                iofEndTableName = t.IndexOf(" ");

                if (iofEndTableName > -1)
                    tableName = SQLStatement.Substring(iofTable, iofEndTableName);
                else
                    tableName = SQLStatement.Substring(iofTable, SQLStatement.Length - iofTable);
            }

            return tableName;
        }

        #endregion TextBox Events

        #region ListView Events

        private void objDeleteRowSQL_Click(object sender, EventArgs e)
        {
            string ColumnName = SqlResultsListView.Columns[0].Text;
            foreach (ListViewItem lvi in SqlResultsListView.SelectedItems)
            {
                string message;
                StatementParser.ReturnResults(StatementBuilder.BuildRowDeleteSQL(TableName, ColumnName, lvi.Text), ActiveDatabaseLocation, out message);
                StatusStripLog.Text = message;
                lvi.Remove();
            }
        }

        private void SqlResultsListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewItem lvi = SqlResultsListView.GetItemAt(e.X, e.Y);
                if (lvi != null)
                {
                    SqlResultsListView.ContextMenu = new ContextMenu();
                    SqlResultsListView.ContextMenu.MenuItems.Add(objDeleteRowSQL);
                }
                else
                {
                    SqlResultsListView.ContextMenu = null;
                }
            }
        }

        #endregion ListView Events

        private const string HelpFile = "Data_Monkey_Help.chm";

        private void menuItem5_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HelpFile)))
            {
                Help.ShowHelp(this, HelpFile);
            }
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button.Tag != null)
            {
                switch (e.Button.Tag.ToString())
                {
                    case "OpenDatabase":
                        OpenDBFileLocator();
                        break;

                    case "IntegrityCheck":
                        IntegrityCheckSQL();
                        break;

                    case "ExecuteSQL":
                        ExecuteTextBoxSQL();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}