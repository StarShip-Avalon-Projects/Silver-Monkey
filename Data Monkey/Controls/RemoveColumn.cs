using System;
using System.Windows.Forms;

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
            NameTextBox = new System.Windows.Forms.TextBox();
            NameLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // CanButton
            // 
            CanButton.Location = new System.Drawing.Point(144, 80);
            // 
            // OkButton
            // 
            OkButton.Location = new System.Drawing.Point(32, 80);
            OkButton.Click -= OkButton_Click;
            OkButton.Click += OkButton_Click2;
            // 
            // NameTextBox
            // 
            NameTextBox.AcceptsReturn = true;
            NameTextBox.Location = new System.Drawing.Point(96, 8);
            NameTextBox.Name = "NameTextBox";
            NameTextBox.Size = new System.Drawing.Size(144, 20);
            NameTextBox.TabIndex = 0;
            NameTextBox.KeyDown += new KeyEventHandler(tb_KeyDown);
            // 
            // NameLabel
            // 
            NameLabel.Location = new System.Drawing.Point(8, 8);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new System.Drawing.Size(80, 23);
            NameLabel.TabIndex = 1;
            NameLabel.Text = "Name:";
            NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RemoveColumn
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(248, 117);
            Controls.Add(NameTextBox);
            Controls.Add(NameLabel);
            Name = "RemoveColumn";
            Text = "     Remove Column";
            Controls.SetChildIndex(NameLabel, 0);
            Controls.SetChildIndex(NameTextBox, 0);
            Controls.SetChildIndex(OkButton, 0);
            Controls.SetChildIndex(CanButton, 0);
            ResumeLayout(false);
            PerformLayout();

		}
        private void OkButton_Click2(object sender, EventArgs e)
        {

            OkButton.DialogResult = MessageBox.Show("Do you want to delete this column?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            Close();
        }
        #endregion

        #region Properties
        public string ColumnName 
		{
			get { return NameTextBox.Text; }
		}

		#endregion
	}
}
