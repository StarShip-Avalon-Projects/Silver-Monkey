namespace SilverMonkey.SQLiteEditor.Controls
{
    /// <summary>
    /// Summary description for AddColumn.
    /// </summary>
    public class RemoveColumn : BasePopup
	{
		private System.Windows.Forms.TextBox NameTextBox;
		private System.Windows.Forms.Label NameLabel;

		public RemoveColumn()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CanButton
            // 
            this.CanButton.Location = new System.Drawing.Point(144, 80);
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(32, 80);
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(96, 8);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(144, 20);
            this.NameTextBox.TabIndex = 0;
            // 
            // NameLabel
            // 
            this.NameLabel.Location = new System.Drawing.Point(8, 8);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(80, 23);
            this.NameLabel.TabIndex = 1;
            this.NameLabel.Text = "Name:";
            this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RemoveColumn
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(248, 117);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.NameLabel);
            this.Name = "RemoveColumn";
            this.Text = "     Remove Column";
            this.Controls.SetChildIndex(this.NameLabel, 0);
            this.Controls.SetChildIndex(this.NameTextBox, 0);
            this.Controls.SetChildIndex(this.OkButton, 0);
            this.Controls.SetChildIndex(this.CanButton, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Properties
		public string ColumnName 
		{
			get { return this.NameTextBox.Text; }
		}

		#endregion
	}
}
