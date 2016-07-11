namespace SilverMonkey.SQLiteEditor.Controls
{
    /// <summary>
    /// Summary description for AddTable.
    /// </summary>
    public class AddTable : BasePopup
	{
		private System.Windows.Forms.TextBox AddTableTextBox;
		private System.Windows.Forms.Label AddTableLabel;
	
		public AddTable()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		#region Properties

		public string TableName 
		{
			get { return AddTableTextBox.Text; }
		}
		#endregion


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.AddTableTextBox = new System.Windows.Forms.TextBox();
			this.AddTableLabel = new System.Windows.Forms.Label();
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
			this.AddTableTextBox.Location = new System.Drawing.Point(8, 32);
			this.AddTableTextBox.Name = "AddTableTextBox";
			this.AddTableTextBox.Size = new System.Drawing.Size(232, 20);
			this.AddTableTextBox.TabIndex = 5;
			this.AddTableTextBox.Text = "";
			// 
			// RenameTableLabel
			// 
			this.AddTableLabel.Location = new System.Drawing.Point(8, 8);
			this.AddTableLabel.Name = "AddTableLabel";
			this.AddTableLabel.Size = new System.Drawing.Size(232, 24);
			this.AddTableLabel.TabIndex = 4;
			this.AddTableLabel.Text = "Create table named:";
			this.AddTableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RenameTable
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(248, 101);
			this.Controls.Add(this.AddTableTextBox);
			this.Controls.Add(this.AddTableLabel);
			this.Name = "AddTable";
			this.Text = "Add Table";
			this.Controls.SetChildIndex(this.OkButton, 0);
			this.Controls.SetChildIndex(this.CanButton, 0);
			this.Controls.SetChildIndex(this.AddTableLabel, 0);
			this.Controls.SetChildIndex(this.AddTableTextBox, 0);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
