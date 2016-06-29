using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using SilverMonkey.SQLiteEditor.Controls;
using MonkeyCore;

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
		MenuItem objDeleteTableSQL;
		MenuItem objCreateTableSQL;

		//Listview Menu
		MenuItem objDeleteRowSQL;
		private MenuItem ExitAppMenu;
		private TextBox sqlStatementTextBox;
		private ToolBar toolBar1;
		private TreeView DatabaseTreeView;
		private Splitter splitter1;
		private TabControl SQLAreaTabControl;
		private TabPage tabPage1;
		private Splitter splitter2;
		private ImageList ToolBarImages;
		private ToolBarButton OpenDatabase;
		private ToolBarButton ExecuteSQL;
		private ToolBarButton StopSQL;
		private ToolBarButton Separator;
		private ToolBarButton toolBarButton1;
		private ToolBarButton toolBarButton2;
		private ToolBarButton toolBarButton3;
		private ToolBarButton toolBarButton4;
		private ToolBarButton toolBarButton5;
		private ToolBarButton toolBarButton6;
		private ToolBarButton toolBarButton7;
		private ListView SqlResultsListView;
		private ToolBarButton IntegrityCheck;
		private MenuItem CheckIntegrity;
		private MenuItem menuItem3;
		private MenuItem CreateDBMenu;

		//Only access from BuildSqlResultsListView
		string TableName = "";
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
			this.sqlStatementTextBox.ContextMenu = new ContextMenu();
			this.sqlStatementTextBox.ContextMenu.MenuItems.Add(objExecuteSQL);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmExplorer));
			this.mainMenu1 = new MainMenu();
			this.menuItem1 = new MenuItem();
			this.OpenDBmenu = new MenuItem();
			this.CreateDBMenu = new MenuItem();
			this.CheckIntegrity = new MenuItem();
			this.menuItem3 = new MenuItem();
			this.ExitAppMenu = new MenuItem();
			this.menuItem2 = new MenuItem();
			this.AddAreaMenu = new MenuItem();
			this.objExecuteSQL = new MenuItem();
			this.objOpenTableSQL = new MenuItem();
			this.objRenameTableSQL = new MenuItem();
			this.objAddColumnSQL = new MenuItem();
			this.objDeleteRowSQL = new MenuItem();
			this.objCreateTableSQL = new MenuItem();
			this.objDeleteTableSQL = new MenuItem();
			this.TreeViewContextMenu = new ContextMenu();
			this.TreeViewTablesMenu = new ContextMenu();
			this.sqlStatementTextBox = new TextBox();
			this.toolBar1 = new ToolBar();
			this.OpenDatabase = new ToolBarButton();
			this.IntegrityCheck = new ToolBarButton();
			this.Separator = new ToolBarButton();
			this.toolBarButton1 = new ToolBarButton();
			this.toolBarButton2 = new ToolBarButton();
			this.toolBarButton3 = new ToolBarButton();
			this.toolBarButton4 = new ToolBarButton();
			this.toolBarButton5 = new ToolBarButton();
			this.toolBarButton6 = new ToolBarButton();
			this.toolBarButton7 = new ToolBarButton();
			this.ExecuteSQL = new ToolBarButton();
			this.StopSQL = new ToolBarButton();
			this.ToolBarImages = new ImageList(this.components);
			this.DatabaseTreeView = new TreeView();
			this.splitter1 = new Splitter();
			this.SQLAreaTabControl = new TabControl();
			this.tabPage1 = new TabPage();
			this.splitter2 = new Splitter();
			this.SqlResultsListView = new ListView();
			this.SQLAreaTabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new MenuItem[] {
																					  this.OpenDBmenu,
																					  this.CreateDBMenu,
																					  this.CheckIntegrity,
																					  this.menuItem3,
																					  this.ExitAppMenu});
			this.menuItem1.Text = "File";
			// 
			// OpenDBmenu
			// 
			this.OpenDBmenu.Index = 0;
			this.OpenDBmenu.Text = "Open Database";
			this.OpenDBmenu.Click += new System.EventHandler(this.OpenDBmenu_Click);
			// 
			// CreateDBMenu
			// 
			this.CreateDBMenu.Index = 1;
			this.CreateDBMenu.Text = "Create Database";
			this.CreateDBMenu.Click += new System.EventHandler(this.CreateDBMenu_Click);
			// 
			// CheckIntegrity
			// 
			this.CheckIntegrity.Index = 2;
			this.CheckIntegrity.Text = "Check DB Integrity";
			this.CheckIntegrity.Click += new System.EventHandler(this.CheckIntegrity_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 3;
			this.menuItem3.Text = "-";
			// 
			// ExitAppMenu
			// 
			this.ExitAppMenu.Index = 4;
			this.ExitAppMenu.Text = "Exit";
			this.ExitAppMenu.Click += new System.EventHandler(this.ExitAppMenu_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new MenuItem[] {
																					  this.AddAreaMenu});
			this.menuItem2.Text = "SQLArea";
			// 
			// AddAreaMenu
			// 
			this.AddAreaMenu.Index = 0;
			this.AddAreaMenu.Text = "Add Area";
			this.AddAreaMenu.Click += new System.EventHandler(this.AddAreaMenu_Click);
			// 
			// objExecuteSQL
			// 
			this.objExecuteSQL.Index = -1;
			this.objExecuteSQL.Text = "Execute";
			this.objExecuteSQL.Click += new System.EventHandler(this.objExecuteSQL_Click);
			// 
			// objOpenTableSQL
			// 
			this.objOpenTableSQL.Index = 0;
			this.objOpenTableSQL.Text = "Open Table";
			this.objOpenTableSQL.Click += new System.EventHandler(this.objOpenTableSQL_Click);
			// 
			// objRenameTableSQL
			// 
			this.objRenameTableSQL.Index = 1;
			this.objRenameTableSQL.Text = "Rename";
			this.objRenameTableSQL.Click += new System.EventHandler(this.objRenameTableSQL_Click);
			// 
			// objCreateTableSQL
			// 
			this.objCreateTableSQL.Index = 1;
			this.objCreateTableSQL.Text = "Create Table";
			this.objCreateTableSQL.Click += new EventHandler(objCreateTableSQL_Click);
			// 
			// objAddColumnSQL
			// 
			this.objAddColumnSQL.Index = 2;
			this.objAddColumnSQL.Text = "Add Column";
			this.objAddColumnSQL.Click += new System.EventHandler(this.objAddColumnSQL_Click);
			// 
			// objDeleteRowSQL
			// 
			this.objDeleteRowSQL.Index = -1;
			this.objDeleteRowSQL.Text = "Delete Row";
			this.objDeleteRowSQL.Click += new System.EventHandler(this.objDeleteRowSQL_Click);
			// 
			// objDeleteTableSQL
			// 
			this.objDeleteTableSQL.Index = 3;
			this.objDeleteTableSQL.Text = "Delete Table";
			this.objDeleteTableSQL.Click += new System.EventHandler(this.objDeleteTableSQL_Click);
			// 
			// TreeViewContextMenu
			// 
			this.TreeViewContextMenu.MenuItems.AddRange(new MenuItem[] {
																								this.objOpenTableSQL,
																								this.objRenameTableSQL,
																								this.objAddColumnSQL,
																								this.objDeleteTableSQL});
			// 
			// TreeViewTablesMenu
			// 
			this.TreeViewTablesMenu.MenuItems.AddRange(new MenuItem[] {this.objCreateTableSQL});
			// 
			// sqlStatementTextBox
			// 
			this.sqlStatementTextBox.Dock = DockStyle.Top;
			this.sqlStatementTextBox.Location = new System.Drawing.Point(0, 0);
			this.sqlStatementTextBox.Multiline = true;
			this.sqlStatementTextBox.Name = "sqlStatementTextBox";
			this.sqlStatementTextBox.Size = new System.Drawing.Size(608, 200);
			this.sqlStatementTextBox.TabIndex = 7;
			this.sqlStatementTextBox.Text = "";
			// 
			// toolBar1
			// 
			this.toolBar1.Buttons.AddRange(new ToolBarButton[] {
																						this.OpenDatabase,
																						this.IntegrityCheck,
																						this.Separator,
																						this.toolBarButton1,
																						this.toolBarButton2,
																						this.toolBarButton3,
																						this.toolBarButton4,
																						this.toolBarButton5,
																						this.toolBarButton6,
																						this.toolBarButton7,
																						this.ExecuteSQL,
																						this.StopSQL});
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.ImageList = this.ToolBarImages;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(840, 28);
			this.toolBar1.TabIndex = 11;
			this.toolBar1.ButtonClick += new ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// OpenDatabase
			// 
			this.OpenDatabase.ImageIndex = 0;
			this.OpenDatabase.Tag = "OpenDatabase";
			this.OpenDatabase.ToolTipText = "Open Database";
			// 
			// IntegrityCheck
			// 
			this.IntegrityCheck.ImageIndex = 3;
			this.IntegrityCheck.Tag = "IntegrityCheck";
			this.IntegrityCheck.ToolTipText = "Integrity Check";
			// 
			// Separator
			// 
			this.Separator.Style = ToolBarButtonStyle.Separator;
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = ToolBarButtonStyle.Separator;
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Style = ToolBarButtonStyle.Separator;
			// 
			// toolBarButton3
			// 
			this.toolBarButton3.Style = ToolBarButtonStyle.Separator;
			// 
			// toolBarButton4
			// 
			this.toolBarButton4.Style = ToolBarButtonStyle.Separator;
			// 
			// toolBarButton5
			// 
			this.toolBarButton5.Style = ToolBarButtonStyle.Separator;
			// 
			// toolBarButton6
			// 
			this.toolBarButton6.Style = ToolBarButtonStyle.Separator;
			// 
			// toolBarButton7
			// 
			this.toolBarButton7.Style = ToolBarButtonStyle.Separator;
			// 
			// ExecuteSQL
			// 
			this.ExecuteSQL.ImageIndex = 1;
			this.ExecuteSQL.Tag = "ExecuteSQL";
			this.ExecuteSQL.ToolTipText = "Execute SQL";
			// 
			// StopSQL
			// 
			this.StopSQL.ImageIndex = 2;
			this.StopSQL.Tag = "StopSQL";
			this.StopSQL.ToolTipText = "Stop SQL";
			// 
			// ToolBarImages
			// 
			this.ToolBarImages.ImageSize = new System.Drawing.Size(16, 16);
			this.ToolBarImages.ImageStream = ((ImageListStreamer)(resources.GetObject("ToolBarImages.ImageStream")));
			this.ToolBarImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// DatabaseTreeView
			// 
			this.DatabaseTreeView.Cursor = Cursors.Default;
			this.DatabaseTreeView.Dock = DockStyle.Left;
			this.DatabaseTreeView.ImageIndex = -1;
			this.DatabaseTreeView.Location = new System.Drawing.Point(0, 28);
			this.DatabaseTreeView.Name = "DatabaseTreeView";
			this.DatabaseTreeView.SelectedImageIndex = -1;
			this.DatabaseTreeView.Size = new System.Drawing.Size(216, 397);
			this.DatabaseTreeView.TabIndex = 12;
			this.DatabaseTreeView.MouseDown += new MouseEventHandler(this.DatabaseTreeView_MouseDown);
			this.DatabaseTreeView.AfterExpand += new TreeViewEventHandler(this.DatabaseTreeView_AfterExpand);
			this.DatabaseTreeView.BeforeSelect += new TreeViewCancelEventHandler(this.DatabaseTreeView_BeforeSelect);
			this.DatabaseTreeView.LostFocus += new System.EventHandler(this.DatabaseTreeView_LostFocus);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(216, 28);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(8, 397);
			this.splitter1.TabIndex = 16;
			this.splitter1.TabStop = false;
			// 
			// SQLAreaTabControl
			// 
			this.SQLAreaTabControl.Controls.Add(this.tabPage1);
			this.SQLAreaTabControl.Dock = DockStyle.Top;
			this.SQLAreaTabControl.HotTrack = true;
			this.SQLAreaTabControl.ItemSize = new System.Drawing.Size(42, 18);
			this.SQLAreaTabControl.Location = new System.Drawing.Point(224, 28);
			this.SQLAreaTabControl.Name = "SQLAreaTabControl";
			this.SQLAreaTabControl.SelectedIndex = 0;
			this.SQLAreaTabControl.Size = new System.Drawing.Size(616, 172);
			this.SQLAreaTabControl.TabIndex = 17;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.sqlStatementTextBox);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(608, 146);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "1";
			// 
			// splitter2
			// 
			this.splitter2.Dock = DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(224, 200);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(616, 8);
			this.splitter2.TabIndex = 18;
			this.splitter2.TabStop = false;
			// 
			// SqlResultsListView
			// 
			this.SqlResultsListView.Dock = DockStyle.Fill;
			this.SqlResultsListView.FullRowSelect = true;
			this.SqlResultsListView.GridLines = true;
			this.SqlResultsListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.SqlResultsListView.Location = new System.Drawing.Point(224, 208);
			this.SqlResultsListView.Name = "SqlResultsListView";
			this.SqlResultsListView.Size = new System.Drawing.Size(616, 217);
			this.SqlResultsListView.TabIndex = 19;
			this.SqlResultsListView.View = View.Details;
			this.SqlResultsListView.MouseDown += new MouseEventHandler(this.SqlResultsListView_MouseDown);
			// 
			// frmExplorer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(840, 425);
			this.Controls.Add(this.SqlResultsListView);
			this.Controls.Add(this.splitter2);
			this.Controls.Add(this.SQLAreaTabControl);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.DatabaseTreeView);
			this.Controls.Add(this.toolBar1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "frmExplorer";
			this.Text = "Starship Avalon Projects: Data Monkey";
			this.SQLAreaTabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);

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
				foreach(DataRow dr in ds.Tables[0].Rows)
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
			TextBox tempTextBox = new TextBox();
			TabPage tempTabPage = new TabPage();
			// 
			// sqlStatementTextBox
			// 
			tempTextBox.Dock = DockStyle.Top;
			tempTextBox.Location = new System.Drawing.Point(0, 0);
			tempTextBox.Multiline = true;
			tempTextBox.Size = new System.Drawing.Size(608, 200);
			tempTextBox.ContextMenu = new ContextMenu();
			tempTextBox.ContextMenu.MenuItems.Add(objExecuteSQL.CloneMenu());

			tempTabPage.Controls.Add(tempTextBox);
			tempTabPage.Location = new System.Drawing.Point(4, 22);
			tempTabPage.Size = new System.Drawing.Size(608, 158);
			tempTabPage.Text = (SQLAreaTabControl.TabCount + 1).ToString();

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
				foreach(DataColumn dc in ds.Tables[0].Columns)
				{
					SqlResultsListView.Columns.Add(dc.ColumnName, 50, HorizontalAlignment.Left);
				}

				int iCounter = 0;

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					SqlResultsListView.Items.Add(dr[0].ToString(),0);

					for(int i = 1; i < dr.ItemArray.Length; i++)
					{
						SqlResultsListView.Items[iCounter].SubItems.Add(dr[i].ToString());					
					}

					//-- Assign alternating backcolor
					if (iCounter%2 == 0)
					{
						SqlResultsListView.Items[iCounter].BackColor = Color.AliceBlue;					
					}

					iCounter++;
				}

				foreach(ColumnHeader ch in SqlResultsListView.Columns)
				{
					ch.Width = -2;
				}
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
			((TextBox) SQLAreaTabControl.SelectedTab.Controls[0]).Text = sqlStatement;

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
			string sqlStatement = ((TextBox) SQLAreaTabControl.SelectedTab.Controls[0]).Text;

			//Parse Results
			StatementParser.ReturnResults(sqlStatement, ActiveDatabaseLocation, ref ds);

			//Get the tablename out of the txtbox Sqlstatement
			string TableName = ParseTableName(sqlStatement);

			//Build ListView
			BuildSqlResultsListView(ds, TableName);
		}

		#endregion

		#region OpenDBFileLocator
		private void OpenDBFileLocator()
		{
			using (OpenFileDialog oFile = new OpenFileDialog()) 
			{
				oFile.Title = "Data Monkey Database Locator" ; 
				oFile.InitialDirectory = Paths.SilverMonkeyBotPath ; 
				oFile.Filter = "All files (*.*)|*.*|DB Files (*.db)|*.db" ; 
				oFile.FilterIndex = 2 ; 
				oFile.RestoreDirectory = true ;
                if (oFile.ShowDialog() == DialogResult.OK ) 
				{ 
					ActiveDatabaseLocation = oFile.FileName ;
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
                if (oFile.ShowDialog() == DialogResult.OK )
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
			SQLAreaTabControl.Controls.Add(this.GenerateTabPage());
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
				DatabaseTreeView.SelectedNode.BackColor = System.Drawing.Color.LightGray;
		}
		
		private void DatabaseTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (DatabaseTreeView.SelectedNode != null)
				DatabaseTreeView.SelectedNode.BackColor = System.Drawing.Color.White;
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
				this.DatabaseTreeView.ContextMenu = TreeViewContextMenu;
			}
			else if (DatabaseTreeView.SelectedNode != null &&
				DatabaseTreeView.SelectedNode.Text.ToLower().Equals("tables"))
			{
				this.DatabaseTreeView.ContextMenu = this.TreeViewTablesMenu;
			}
			else
			{
				this.DatabaseTreeView.ContextMenu = null;
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
					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						string ColumnName = dr[1].ToString();
						string ColumnType = dr[2].ToString();
						TreeNode columnNode = new TreeNode();
						columnNode.Text = ColumnName + ", " + ColumnType;
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
					StatementParser.ReturnResults(StatementBuilder.BuildAddTableSQL(pAddTable.TableName), ActiveDatabaseLocation);

					//Build TreeView
					PopulateDatabaseTreeView();
				}
			}
		}

		private void objOpenTableSQL_Click(object sender, EventArgs e)
		{
			//GetTable Names
			DataSet ds = null;
			string sqlStatement = StatementBuilder.BuildTableOpenSql(DatabaseTreeView.SelectedNode.Text);

			//Place sqlstatement into the text box
			((TextBox) SQLAreaTabControl.SelectedTab.Controls[0]).Text = sqlStatement;

			//Parse Results
			StatementParser.ReturnResults(sqlStatement, ActiveDatabaseLocation, ref ds);

			//Build ListView
			BuildSqlResultsListView(ds, DatabaseTreeView.SelectedNode.Text);
		}

		private void objAddColumnSQL_Click(object sender, EventArgs e)
		{
			using (AddColumn pAddColumn = new AddColumn()) 
			{
				pAddColumn.ShowInTaskbar = false;
				if (DialogResult.OK == pAddColumn.ShowDialog())
				{
					StatementParser.ReturnResults(StatementBuilder.BuildAddColumnSQL(DatabaseTreeView.SelectedNode.Text, pAddColumn.ColumnName, pAddColumn.ColumnType), ActiveDatabaseLocation);

					//Add new column to the tree if it is expanded
					if (DatabaseTreeView.SelectedNode.IsExpanded)
					{
						TreeNode columnNode = new TreeNode();
						columnNode.Text = pAddColumn.ColumnName + ", " + pAddColumn.ColumnType;
						columnNode.Tag = pAddColumn.ColumnName;
						
						DatabaseTreeView.SelectedNode.Nodes[0].Nodes.Add(columnNode);
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
					StatementParser.ReturnResults(StatementBuilder.BuildRenameTableSQL(DatabaseTreeView.SelectedNode.Text, pRenameTable.NewTableName),ActiveDatabaseLocation);

					DatabaseTreeView.SelectedNode.Text = pRenameTable.NewTableName;
				}
			}
		}

		
		private void objDeleteTableSQL_Click(object sender, EventArgs e)
		{
			string TableName = DatabaseTreeView.SelectedNode.Text;
			StatementParser.ReturnResults(StatementBuilder.BuildTableDeleteSQL(TableName),ActiveDatabaseLocation);
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
			foreach(ListViewItem lvi in SqlResultsListView.SelectedItems)
			{
				StatementParser.ReturnResults(StatementBuilder.BuildRowDeleteSQL(this.TableName, ColumnName, lvi.Text), ActiveDatabaseLocation);
				lvi.Remove();
			}
		}

		#endregion

		private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			if(e.Button.Tag != null)
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
					case "StopSQL":
						//StatementParser.InterruptQuery();
						break;
					default:
						break;
				}

			}
		}

	}

}
