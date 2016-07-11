namespace SilverMonkey.SQLiteEditor.Controls
{
    /// <summary>
    /// Summary description for RenameTable.
    /// </summary>
    public class RenameTable : BasePopup
	{
		private System.Windows.Forms.TextBox RenameTableTextBox;
		private System.Windows.Forms.Label RenameTableLabel;
	
		public RenameTable()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		#region Properties

		public string NewTableName 
		{
			get { return RenameTableTextBox.Text; }
		}
		#endregion


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.RenameTableTextBox = new System.Windows.Forms.TextBox();
			this.RenameTableLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// CanButton
			// 
			this.CanButton.Location = new System.Drawing.Point(136, 64);
			this.CanButton.Name = "CanButton";
			// 
			// OkButton
			// 
			this.OkButton.Location = new System.Drawing.Point(32, 64);
			this.OkButton.Name = "OkButton";
			// 
			// RenameTableTextBox
			// 
			this.RenameTableTextBox.Location = new System.Drawing.Point(8, 32);
			this.RenameTableTextBox.Name = "RenameTableTextBox";
			this.RenameTableTextBox.Size = new System.Drawing.Size(232, 20);
			this.RenameTableTextBox.TabIndex = 5;
			this.RenameTableTextBox.Text = "";
			// 
			// RenameTableLabel
			// 
			this.RenameTableLabel.Location = new System.Drawing.Point(8, 8);
			this.RenameTableLabel.Name = "RenameTableLabel";
			this.RenameTableLabel.Size = new System.Drawing.Size(232, 24);
			this.RenameTableLabel.TabIndex = 4;
			this.RenameTableLabel.Text = "Change table name to:";
			this.RenameTableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RenameTable
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(248, 101);
			this.Controls.Add(this.RenameTableTextBox);
			this.Controls.Add(this.RenameTableLabel);
			this.Name = "RenameTable";
			this.Text = "Rename Table";
			this.Controls.SetChildIndex(this.OkButton, 0);
			this.Controls.SetChildIndex(this.CanButton, 0);
			this.Controls.SetChildIndex(this.RenameTableLabel, 0);
			this.Controls.SetChildIndex(this.RenameTableTextBox, 0);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
