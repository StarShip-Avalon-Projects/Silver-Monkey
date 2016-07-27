using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using SilverMonkey.SQLiteEditor.Controls;
using MonkeyCore;
using FastColoredTextBoxNS;
using MonkeyCore.Controls;

namespace SQLiteEditor
{

    public class frmExplorer : Form
    {
        #region Members
        MainMenu mainMenu1;
        MenuItem menuItem1;
        MenuItem OpenDBmenu;
        private System.ComponentModel.IContainer components;
        private MenuItem menuItem2;
        private MenuItem AddAreaMenu;

        //Database String
        static string ActiveDatabaseLocation;

        //Menu Members
        ContextMenu TreeViewContextMenu;
        ContextMenu TreeViewTablesMenu;

        //Text Box Menu
        MenuItem objExecuteSQL;

        //Treeview Menu
        MenuItem objOpenTableSQL;
        MenuItem objRenameTableSQL;
        MenuItem objAddColumnSQL;
        MenuItem objRemoveColumnSQL;
        MenuItem objDeleteTableSQL;
        MenuItem objCreateTableSQL;

        //Listview Menu
        MenuItem objDeleteRowSQL;
        private MenuItem ExitAppMenu;
        private ToolBar toolBar1;
        private TreeView DatabaseTreeView;
        private Splitter splitter1;
        private ImageList ToolBarImages;
        private ToolBarButton OpenDatabase;
        private ToolBarButton ExecuteSQL;
        private ToolBarButton StopSQL;
        private ToolBarButton Separator;
        private ToolBarButton IntegrityCheck;
        private MenuItem CheckIntegrity;
        private MenuItem menuItem3;
        private MenuItem CreateDBMenu;
        private SplitContainer splitContainer1;
        public ListView_NoFlicker SqlResultsListView;

        //Only access from BuildSqlResultsListView
        string TableName = "";
        private MonkeyCore.Controls.TabControlEx SQLAreaTabControl;
        private TabPage tabPage1;
        private SilverMonkeyFCTB silverMonkeyFCTB1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel StatusStripLog;
        private FastColoredTextBox sqlStatementTextBox;
        #endregion

        #region Constructor / Destructor

        public frmExplorer()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            //sqlStatementTextBox 
            //
            GenerateTabPage();

            //  this.sqlStatementTextBox.ContextMenu = new ContextMenu();
            //this.sqlStatementTextBox.ContextMenu.MenuItems.Add(objExecuteSQL);
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

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExplorer));
            mainMenu1 = new System.Windows.Forms.MainMenu(components);
            menuItem1 = new System.Windows.Forms.MenuItem();
            OpenDBmenu = new System.Windows.Forms.MenuItem();
            CreateDBMenu = new System.Windows.Forms.MenuItem();
            CheckIntegrity = new System.Windows.Forms.MenuItem();
            menuItem3 = new System.Windows.Forms.MenuItem();
            ExitAppMenu = new System.Windows.Forms.MenuItem();
            menuItem2 = new System.Windows.Forms.MenuItem();
            AddAreaMenu = new System.Windows.Forms.MenuItem();
            objExecuteSQL = new System.Windows.Forms.MenuItem();
            objOpenTableSQL = new System.Windows.Forms.MenuItem();
            objRenameTableSQL = new System.Windows.Forms.MenuItem();
            objAddColumnSQL = new System.Windows.Forms.MenuItem();
            objRemoveColumnSQL = new System.Windows.Forms.MenuItem();
            objDeleteRowSQL = new System.Windows.Forms.MenuItem();
            objCreateTableSQL = new System.Windows.Forms.MenuItem();
            objDeleteTableSQL = new System.Windows.Forms.MenuItem();
            TreeViewContextMenu = new System.Windows.Forms.ContextMenu();
            TreeViewTablesMenu = new System.Windows.Forms.ContextMenu();
            toolBar1 = new System.Windows.Forms.ToolBar();
            OpenDatabase = new System.Windows.Forms.ToolBarButton();
            IntegrityCheck = new System.Windows.Forms.ToolBarButton();
            Separator = new System.Windows.Forms.ToolBarButton();
            ExecuteSQL = new System.Windows.Forms.ToolBarButton();
            StopSQL = new System.Windows.Forms.ToolBarButton();
            ToolBarImages = new System.Windows.Forms.ImageList(components);
            sqlStatementTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            DatabaseTreeView = new System.Windows.Forms.TreeView();
            splitter1 = new System.Windows.Forms.Splitter();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            SQLAreaTabControl = new MonkeyCore.Controls.TabControlEx();
            tabPage1 = new System.Windows.Forms.TabPage();
            silverMonkeyFCTB1 = new MonkeyCore.Controls.SilverMonkeyFCTB();
            SqlResultsListView = new MonkeyCore.Controls.ListView_NoFlicker();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            StatusStripLog = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(sqlStatementTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SQLAreaTabControl.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(silverMonkeyFCTB1)).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // mainMenu1
            // 
            mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            menuItem1,
            menuItem2});
            // 
            // menuItem1
            // 
            menuItem1.Index = 0;
            menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            OpenDBmenu,
            CreateDBMenu,
            CheckIntegrity,
            menuItem3,
            ExitAppMenu});
            menuItem1.Text = "File";
            // 
            // OpenDBmenu
            // 
            OpenDBmenu.Index = 0;
            OpenDBmenu.Text = "Open Database";
            OpenDBmenu.Click += new System.EventHandler(OpenDBmenu_Click);
            // 
            // CreateDBMenu
            // 
            CreateDBMenu.Index = 1;
            CreateDBMenu.Text = "Create Database";
            CreateDBMenu.Click += new System.EventHandler(CreateDBMenu_Click);
            // 
            // CheckIntegrity
            // 
            CheckIntegrity.Index = 2;
            CheckIntegrity.Text = "Check DB Integrity";
            CheckIntegrity.Click += new System.EventHandler(CheckIntegrity_Click);
            // 
            // menuItem3
            // 
            menuItem3.Index = 3;
            menuItem3.Text = "-";
            // 
            // ExitAppMenu
            // 
            ExitAppMenu.Index = 4;
            ExitAppMenu.Text = "Exit";
            ExitAppMenu.Click += new System.EventHandler(ExitAppMenu_Click);
            // 
            // menuItem2
            // 
            menuItem2.Index = 1;
            menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            AddAreaMenu});
            menuItem2.Text = "SQLArea";
            // 
            // AddAreaMenu
            // 
            AddAreaMenu.Index = 0;
            AddAreaMenu.Text = "Add Area";
            AddAreaMenu.Click += new System.EventHandler(AddAreaMenu_Click);
            // 
            // objExecuteSQL
            // 
            objExecuteSQL.Index = -1;
            objExecuteSQL.Text = "Execute";
            objExecuteSQL.Click += new System.EventHandler(objExecuteSQL_Click);
            // 
            // objOpenTableSQL
            // 
            objOpenTableSQL.Index = 0;
            objOpenTableSQL.Text = "Open Table";
            objOpenTableSQL.Click += new System.EventHandler(objOpenTableSQL_Click);
            // 
            // objRenameTableSQL
            // 
            objRenameTableSQL.Index = 1;
            objRenameTableSQL.Text = "Rename";
            objRenameTableSQL.Click += new System.EventHandler(objRenameTableSQL_Click);
            // 
            // objAddColumnSQL
            // 
            objAddColumnSQL.Index = 2;
            objAddColumnSQL.Text = "Add Column";
            objAddColumnSQL.Click += new System.EventHandler(objAddColumnSQL_Click);
            // 
            // objRemoveColumnSQL
            // 
            objRemoveColumnSQL.Index = 3;
            objRemoveColumnSQL.Text = "Remove Column";
            objRemoveColumnSQL.Click += new System.EventHandler(objRemoveColumnSQL_Click);
            // 
            // objDeleteRowSQL
            // 
            objDeleteRowSQL.Index = -1;
            objDeleteRowSQL.Text = "Delete Row";
            objDeleteRowSQL.Click += new System.EventHandler(objDeleteRowSQL_Click);
            // 
            // objCreateTableSQL
            // 
            objCreateTableSQL.Index = 0;
            objCreateTableSQL.Text = "Create Table";
            objCreateTableSQL.Click += new System.EventHandler(objCreateTableSQL_Click);
            // 
            // objDeleteTableSQL
            // 
            objDeleteTableSQL.Index = 4;
            objDeleteTableSQL.Text = "Delete Table";
            objDeleteTableSQL.Click += new System.EventHandler(objDeleteTableSQL_Click);
            // 
            // TreeViewContextMenu
            // 
            TreeViewContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            objOpenTableSQL,
            objRenameTableSQL,
            objAddColumnSQL,
            objRemoveColumnSQL,
            objDeleteTableSQL});
            // 
            // TreeViewTablesMenu
            // 
            TreeViewTablesMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            objCreateTableSQL});
            // 
            // toolBar1
            // 
            toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            OpenDatabase,
            IntegrityCheck,
            Separator,
            ExecuteSQL,
            StopSQL});
            toolBar1.DropDownArrows = true;
            toolBar1.ImageList = ToolBarImages;
            toolBar1.Location = new System.Drawing.Point(0, 0);
            toolBar1.Name = "toolBar1";
            toolBar1.ShowToolTips = true;
            toolBar1.Size = new System.Drawing.Size(840, 28);
            toolBar1.TabIndex = 11;
            toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(toolBar1_ButtonClick);
            // 
            // OpenDatabase
            // 
            OpenDatabase.ImageIndex = 0;
            OpenDatabase.Name = "OpenDatabase";
            OpenDatabase.Tag = "OpenDatabase";
            OpenDatabase.ToolTipText = "Open Database";
            // 
            // IntegrityCheck
            // 
            IntegrityCheck.ImageIndex = 3;
            IntegrityCheck.Name = "IntegrityCheck";
            IntegrityCheck.Tag = "IntegrityCheck";
            IntegrityCheck.ToolTipText = "Integrity Check";
            // 
            // Separator
            // 
            Separator.Name = "Separator";
            Separator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // ExecuteSQL
            // 
            ExecuteSQL.ImageIndex = 1;
            ExecuteSQL.Name = "ExecuteSQL";
            ExecuteSQL.Tag = "ExecuteSQL";
            ExecuteSQL.ToolTipText = "Execute SQL";
            // 
            // StopSQL
            // 
            StopSQL.ImageIndex = 2;
            StopSQL.Name = "StopSQL";
            StopSQL.Tag = "StopSQL";
            StopSQL.ToolTipText = "Stop SQL";
            // 
            // ToolBarImages
            // 
            ToolBarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ToolBarImages.ImageStream")));
            ToolBarImages.TransparentColor = System.Drawing.Color.Transparent;
            ToolBarImages.Images.SetKeyName(0, "");
            ToolBarImages.Images.SetKeyName(1, "");
            ToolBarImages.Images.SetKeyName(2, "");
            ToolBarImages.Images.SetKeyName(3, "");
            // 
            // sqlStatementTextBox
            // 
            sqlStatementTextBox.AutoCompleteBracketsList = new char[] {
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
            sqlStatementTextBox.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            sqlStatementTextBox.BackBrush = null;
            sqlStatementTextBox.CharHeight = 14;
            sqlStatementTextBox.CharWidth = 8;
            sqlStatementTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            sqlStatementTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            sqlStatementTextBox.Font = new System.Drawing.Font("Courier New", 9.75F);
            sqlStatementTextBox.IsReplaceMode = false;
            sqlStatementTextBox.Location = new System.Drawing.Point(0, 0);
            sqlStatementTextBox.Name = "sqlStatementTextBox";
            sqlStatementTextBox.Paddings = new System.Windows.Forms.Padding(0);
            sqlStatementTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            sqlStatementTextBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("sqlStatementTextBox.ServiceColors")));
            sqlStatementTextBox.Size = new System.Drawing.Size(150, 150);
            sqlStatementTextBox.TabIndex = 0;
            sqlStatementTextBox.Zoom = 100;
            // 
            // DatabaseTreeView
            // 
            DatabaseTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            DatabaseTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            DatabaseTreeView.Location = new System.Drawing.Point(0, 28);
            DatabaseTreeView.Name = "DatabaseTreeView";
            DatabaseTreeView.Size = new System.Drawing.Size(216, 355);
            DatabaseTreeView.TabIndex = 12;
            DatabaseTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(DatabaseTreeView_AfterExpand);
            DatabaseTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(DatabaseTreeView_BeforeSelect);
            DatabaseTreeView.LostFocus += new System.EventHandler(DatabaseTreeView_LostFocus);
            DatabaseTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(DatabaseTreeView_MouseDown);
            // 
            // splitter1
            // 
            splitter1.Location = new System.Drawing.Point(216, 28);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(8, 355);
            splitter1.TabIndex = 16;
            splitter1.TabStop = false;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(224, 28);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(SQLAreaTabControl);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(SqlResultsListView);
            splitContainer1.Size = new System.Drawing.Size(616, 355);
            splitContainer1.SplitterDistance = 170;
            splitContainer1.TabIndex = 17;
            // 
            // SQLAreaTabControl
            // 
            SQLAreaTabControl.Controls.Add(tabPage1);
            SQLAreaTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            SQLAreaTabControl.Location = new System.Drawing.Point(0, 0);
            SQLAreaTabControl.Name = "SQLAreaTabControl";
            SQLAreaTabControl.SelectedIndex = 0;
            SQLAreaTabControl.Size = new System.Drawing.Size(616, 170);
            SQLAreaTabControl.TabIndex = 0;
            SQLAreaTabControl.CloseButtonClick += SQLAreaTabControl_CloseButtonClick;
            SQLAreaTabControl.MouseDown += SQLAreaTabControl_MouseDown;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(silverMonkeyFCTB1);
            tabPage1.Location = new System.Drawing.Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(608, 144);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "SQL     ";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // silverMonkeyFCTB1
            // 
            silverMonkeyFCTB1.AutoCompleteBracketsList = new char[] {
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
            silverMonkeyFCTB1.AutoIndentCharsPatterns = "";
            silverMonkeyFCTB1.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            silverMonkeyFCTB1.BackBrush = null;
            silverMonkeyFCTB1.CharHeight = 14;
            silverMonkeyFCTB1.CharWidth = 8;
            silverMonkeyFCTB1.CommentPrefix = "--";
            silverMonkeyFCTB1.Cursor = System.Windows.Forms.Cursors.IBeam;
            silverMonkeyFCTB1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            silverMonkeyFCTB1.Dock = System.Windows.Forms.DockStyle.Fill;
            silverMonkeyFCTB1.Font = new System.Drawing.Font("Courier New", 9.75F);
            silverMonkeyFCTB1.IsReplaceMode = false;
            silverMonkeyFCTB1.Language = FastColoredTextBoxNS.Language.SQL;
            silverMonkeyFCTB1.LeftBracket = '(';
            silverMonkeyFCTB1.Location = new System.Drawing.Point(3, 3);
            silverMonkeyFCTB1.Name = "silverMonkeyFCTB1";
            silverMonkeyFCTB1.Paddings = new System.Windows.Forms.Padding(0);
            silverMonkeyFCTB1.RightBracket = ')';
            silverMonkeyFCTB1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            silverMonkeyFCTB1.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("silverMonkeyFCTB1.ServiceColors")));
            silverMonkeyFCTB1.Size = new System.Drawing.Size(602, 138);
            silverMonkeyFCTB1.TabIndex = 0;
            silverMonkeyFCTB1.Zoom = 100;
            // 
            // SqlResultsListView
            // 
            SqlResultsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            SqlResultsListView.FullRowSelect = true;
            SqlResultsListView.GridLines = true;
            SqlResultsListView.LabelEdit = true;
            SqlResultsListView.LargeImageList = ToolBarImages;
            SqlResultsListView.Location = new System.Drawing.Point(0, 0);
            SqlResultsListView.Name = "SqlResultsListView";
            SqlResultsListView.Size = new System.Drawing.Size(616, 181);
            SqlResultsListView.TabIndex = 0;
            SqlResultsListView.UseCompatibleStateImageBehavior = false;
            SqlResultsListView.View = System.Windows.Forms.View.Details;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            StatusStripLog});
            statusStrip1.Location = new System.Drawing.Point(224, 361);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(616, 22);
            statusStrip1.TabIndex = 18;
            statusStrip1.Text = "statusStrip1";
            // 
            // StatusStripLog
            // 
            StatusStripLog.Name = "StatusStripLog";
            StatusStripLog.Size = new System.Drawing.Size(50, 17);
            StatusStripLog.Text = "Execute:";
            // 
            // frmExplorer
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(840, 383);
            Controls.Add(statusStrip1);
            Controls.Add(splitContainer1);
            Controls.Add(splitter1);
            Controls.Add(DatabaseTreeView);
            Controls.Add(toolBar1);
            Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            Menu = mainMenu1;
            Name = "frmExplorer";
            Text = "Starship Avalon Projects: Data Monkey";
            ((System.ComponentModel.ISupportInitialize)(sqlStatementTextBox)).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            SQLAreaTabControl.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(silverMonkeyFCTB1)).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }


        #endregion


        #region Private Methods

        #region PopulateDatabaseTreeView
        public void PopulateDatabaseTreeView()
        {
            DatabaseTreeView.Nodes.Clear();

            int LastSlash = ActiveDatabaseLocation.LastIndexOf("\\");
            string DatabaseName = ActiveDatabaseLocation.Substring(LastSlash + 1, ActiveDatabaseLocation.Length - LastSlash - 1);

            TreeNode topNode = new TreeNode();
            topNode.Text = DatabaseName;
            topNode.Tag = "DatabaseName";

            TreeNode tablesNode = new TreeNode();
            tablesNode.Text = "Tables";
            tablesNode.Tag = "Tables";

            DataSet ds = null;
            StatementParser.ReturnResults(StatementBuilder.BuildMasterQuery(), ActiveDatabaseLocation, ref ds);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string TableName = dr[0].ToString();
                    TreeNode tableNode = new TreeNode();
                    tableNode.Text = TableName;
                    tableNode.Tag = TableName;
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
        #endregion

        #region GenerateSQLArea TabPage

        private TabPage GenerateTabPage()
        {
            FastColoredTextBox tempTextBox = new SilverMonkeyFCTB();
            TabPage tempTabPage = new TabPage();
            // 
            // sqlStatementTextBox
            // 
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
            tempTabPage.Text = (SQLAreaTabControl.TabCount + 1).ToString();
            tempTabPage.Tag = SQLAreaTabControl.TabCount;
            return tempTabPage;
        }
        #endregion

        #region BuildSqlResultsListView

        private void BuildSqlResultsListView(DataSet ds, string tableName)
        {
            SqlResultsListView.Items.Clear();
            SqlResultsListView.Columns.Clear();

            if (ds != null)
            {
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
        }
        #endregion

        #region IntegrityCheck
        private void IntegrityCheckSQL()
        {
            //GetTable Names
            DataSet ds = null;
            string sqlStatement = StatementBuilder.BuildIntegrityCheckSQL();

            //Place sqlstatement into the text box
            ((SilverMonkeyFCTB)SQLAreaTabControl.SelectedTab.Controls[0]).Text = sqlStatement;

            //Parse Results
            StatementParser.ReturnResults(sqlStatement, ActiveDatabaseLocation, ref ds);

            //Build ListView
            BuildSqlResultsListView(ds, (DatabaseTreeView.SelectedNode == null ? "" : DatabaseTreeView.SelectedNode.Text));
        }
        #endregion

        #region ExecuteTextBoxSQL

        private void ExecuteTextBoxSQL()
        {
            //GetTable Names
            DataSet ds = null;
            if (SQLAreaTabControl.SelectedTab == null)
                SQLAreaTabControl.TabPages.Add(GenerateTabPage());
            string sqlStatement = ((SilverMonkeyFCTB)SQLAreaTabControl.SelectedTab.Controls[0]).Text;

            //Parse Results
            string message = null;
            StatementParser.ReturnResults(sqlStatement, ActiveDatabaseLocation, ref ds, out message);

            //Get the tablename out of the txtbox Sqlstatement
            string TableName = ParseTableName(sqlStatement);

            //Build ListView
            BuildSqlResultsListView(ds, TableName);
            StatusStripLog.Text = message;
        }

        #endregion

        #region OpenDBFileLocator
        private void OpenDBFileLocator()
        {
            using (OpenFileDialog oFile = new OpenFileDialog())
            {
                oFile.Title = "Data Monkey Database Locator";
                oFile.InitialDirectory = Paths.SilverMonkeyBotPath;
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
        #endregion

        #region CreateDBFile()

        private void CreateDBFile()
        {
            using (SaveFileDialog oFile = new SaveFileDialog())
            {
                oFile.Title = "Data Monkey Database Locator";
                oFile.InitialDirectory = Paths.SilverMonkeyBotPath;
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

        #endregion


        #endregion

        #region Menu Event
        private void OpenDBmenu_Click(object sender, System.EventArgs e)
        {
            OpenDBFileLocator();
        }

        private void CreateDBMenu_Click(object sender, System.EventArgs e)
        {
            CreateDBFile();
        }
        private void AddAreaMenu_Click(object sender, System.EventArgs e)
        {
            SQLAreaTabControl.Controls.Add(GenerateTabPage());
        }

        private void ExitAppMenu_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void CheckIntegrity_Click(object sender, System.EventArgs e)
        {
            IntegrityCheckSQL();
        }
        #endregion

        #region TreeView Events
        private void DatabaseTreeView_LostFocus(object sender, EventArgs e)
        {
            if (DatabaseTreeView.SelectedNode != null)
                DatabaseTreeView.SelectedNode.BackColor = Color.LightGray;
        }

        private void DatabaseTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (DatabaseTreeView.SelectedNode != null)
                DatabaseTreeView.SelectedNode.BackColor = Color.White;
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

        private void DatabaseTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null && e.Node.Parent.Text == "Tables")
            {
                string SQLStatement = "PRAGMA table_info(" + e.Node.Text + ")";

                //GetTable Names
                DataSet ds = null;

                StatementParser.ReturnResults(SQLStatement, ActiveDatabaseLocation, ref ds);

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
                        columnNode.Text = ColumnName != null ? ColumnName + ", " + ColumnType : ColumnName  ;
                        columnNode.Tag = ColumnName;

                        columnsNode.Nodes.Add(columnNode);
                    }
                }

                columnsNode.Expand();
            }
        }
        private void objCreateTableSQL_Click(object sender, EventArgs e)
        {
            using (AddTable pAddTable = new AddTable())
            {
                pAddTable.ShowInTaskbar = false;
                if (DialogResult.OK == pAddTable.ShowDialog())
                {
                    DataSet ds = null;
                    string message;
                    StatementParser.ReturnResults(StatementBuilder.BuildAddTableSQL(pAddTable.TableName), ActiveDatabaseLocation, ref ds, out message);

                    //Build TreeView
                    PopulateDatabaseTreeView();
                    StatusStripLog.Text = message;
                }
            }
        }

        private void objOpenTableSQL_Click(object sender, EventArgs e)
        {
            //GetTable Names
            DataSet ds = null;
            string sqlStatement = StatementBuilder.BuildTableOpenSql(DatabaseTreeView.SelectedNode.Text);

            //Place sqlstatement into the text box

            if (SQLAreaTabControl.SelectedTab == null)
                SQLAreaTabControl.TabPages.Add(GenerateTabPage());
            ((SilverMonkeyFCTB)SQLAreaTabControl.SelectedTab.Controls[0]).Text = sqlStatement;

            //Parse Results
            string LogMessage;
            StatementParser.ReturnResults(sqlStatement, ActiveDatabaseLocation, ref ds, out LogMessage);

            //Build ListView
            BuildSqlResultsListView(ds, DatabaseTreeView.SelectedNode.Text);
            StatusStripLog.Text = LogMessage;
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

        private void objRemoveColumnSQL_Click(object sender, EventArgs e)
        {
            using (RemoveColumn pRemoveColumn = new RemoveColumn())
            {
                pRemoveColumn.ShowInTaskbar = false;
                if (DialogResult.OK == pRemoveColumn.ShowDialog() & !string.IsNullOrEmpty(pRemoveColumn.ColumnName))
                {
                    // StatementParser.ReturnResults(StatementBuilder.BuildAddColumnSQL(DatabaseTreeView.SelectedNode.Text, pRemoveColumn.ColumnName, pRemoveColumn.ColumnType), ActiveDatabaseLocation);
                    SQLiteDatabase db = new SQLiteDatabase(ActiveDatabaseLocation);
                    if (db.isColumnExist(pRemoveColumn.ColumnName, DatabaseTreeView.SelectedNode.Text ))
                    {
                        db.removeColumn(DatabaseTreeView.SelectedNode.Text, pRemoveColumn.ColumnName);
                        //Add new column to the tree if it is expanded
                        if (DatabaseTreeView.SelectedNode.IsExpanded)
                        {
                            TreeNode columnNode = new TreeNode();
                            columnNode.Text = pRemoveColumn.ColumnName ;
                            columnNode.Tag = pRemoveColumn.ColumnName;

                            DatabaseTreeView.SelectedNode.Nodes[0].Nodes.Remove(columnNode);
                          
                            DatabaseTreeView.SelectedNode.Collapse();
                           DatabaseTreeView.SelectedNode.Expand();
                           
                        }
                    }
                }
            }
        }

        private void objRenameTableSQL_Click(object sender, EventArgs e)
        {
            using (RenameTable pRenameTable = new RenameTable())
            {
                pRenameTable.ShowInTaskbar = false;
                if (DialogResult.OK == pRenameTable.ShowDialog())
                {
                    StatementParser.ReturnResults(StatementBuilder.BuildRenameTableSQL(DatabaseTreeView.SelectedNode.Text, pRenameTable.NewTableName), ActiveDatabaseLocation);

                    DatabaseTreeView.SelectedNode.Text = pRenameTable.NewTableName;
                }
            }
        }


        private void objDeleteTableSQL_Click(object sender, EventArgs e)
        {
            string TableName = DatabaseTreeView.SelectedNode.Text;
            StatementParser.ReturnResults(StatementBuilder.BuildTableDeleteSQL(TableName), ActiveDatabaseLocation);
            DatabaseTreeView.SelectedNode.Remove();
        }
        #endregion

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

        #endregion

        #region ListView Events
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

        private void objDeleteRowSQL_Click(object sender, EventArgs e)
        {
            string ColumnName = SqlResultsListView.Columns[0].Text;
            foreach (ListViewItem lvi in SqlResultsListView.SelectedItems)
            {
                StatementParser.ReturnResults(StatementBuilder.BuildRowDeleteSQL(TableName, ColumnName, lvi.Text), ActiveDatabaseLocation);
                lvi.Remove();
            }
        }

        #endregion

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
                    //case "StopSQL":
                    //StatementParser.InterruptQuery();
                    //break;
                    default:
                        break;
                }

            }
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
                        break; // TODO: might not be correct. Was : Exit For
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
                        CloseTab(i);
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
        }

        private void FNewTab_Click(object sender, EventArgs e)
        {
            SQLAreaTabControl.TabPages.Add(GenerateTabPage());
        }

        private void CloseAllButThis(ref int i)
        {
            int j = 0;
            for (j = SQLAreaTabControl.TabPages.Count - 1; j >= 0; j += -1)
            {
                if (i != j)
                    CloseTab(j);
            }
        }

        private void FCloseAllTab_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem t = (ToolStripMenuItem)sender;
            int i = int.Parse((string)t.Tag);
            CloseAllButThis(ref i);
        }

        private void FCloseTab_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem t = (ToolStripMenuItem)sender;
            int i = (int)t.Tag;
            CloseTab(i);
        }

        private void SQLAreaTabControl_CloseButtonClick(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Button t = (Button)sender;
            CloseTab(t.TabIndex);
            SQLAreaTabControl.RePositionCloseButtons();
        }

        private void CloseTab(int i)
        {
            if (i > SQLAreaTabControl.TabCount - 1)
                return;
            SQLAreaTabControl.TabPages.RemoveAt(i);
            if (SQLAreaTabControl.TabPages.Count == 0 & Disposing == false)
            {
                SQLAreaTabControl.TabPages.Add(GenerateTabPage());
            }

        }

    }
}
