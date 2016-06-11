using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SilverMonkey.SQLiteEditor.Controls
{
	/// <summary>
	/// Summary description for AddColumn.
	/// </summary>
	public class AddColumn : BasePopup
	{
		private System.Windows.Forms.TextBox NameTextBox;
		private System.Windows.Forms.Label NameLabel;
		private System.Windows.Forms.Label TypeLabel;
		private System.Windows.Forms.ComboBox TypeComboBox;

		public AddColumn()
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
			this.TypeLabel = new System.Windows.Forms.Label();
			this.TypeComboBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// CanButton
			// 
			this.CanButton.Location = new System.Drawing.Point(144, 80);
			this.CanButton.Name = "CanButton";
			// 
			// OkButton
			// 
			this.OkButton.Location = new System.Drawing.Point(32, 80);
			this.OkButton.Name = "OkButton";
			// 
			// NameTextBox
			// 
			this.NameTextBox.Location = new System.Drawing.Point(96, 8);
			this.NameTextBox.Name = "NameTextBox";
			this.NameTextBox.Size = new System.Drawing.Size(144, 20);
			this.NameTextBox.TabIndex = 0;
			this.NameTextBox.Text = "";
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
			// TypeLabel
			// 
			this.TypeLabel.Location = new System.Drawing.Point(8, 40);
			this.TypeLabel.Name = "TypeLabel";
			this.TypeLabel.Size = new System.Drawing.Size(80, 23);
			this.TypeLabel.TabIndex = 2;
			this.TypeLabel.Text = "Type:";
			this.TypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TypeComboBox
			// 
			this.TypeComboBox.Items.AddRange(new object[] {
															  "TEXT",
															  "INTEGER",
															  "REAL",
															  "BLOB"});
			this.TypeComboBox.Location = new System.Drawing.Point(96, 40);
			this.TypeComboBox.Name = "TypeComboBox";
			this.TypeComboBox.Size = new System.Drawing.Size(144, 21);
			this.TypeComboBox.TabIndex = 3;
			// 
			// AddColumn
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(248, 117);
			this.Controls.Add(this.NameTextBox);
			this.Controls.Add(this.TypeComboBox);
			this.Controls.Add(this.TypeLabel);
			this.Controls.Add(this.NameLabel);
			this.Name = "AddColumn";
			this.Text = "     Add Column";
			this.Controls.SetChildIndex(this.NameLabel, 0);
			this.Controls.SetChildIndex(this.TypeLabel, 0);
			this.Controls.SetChildIndex(this.TypeComboBox, 0);
			this.Controls.SetChildIndex(this.NameTextBox, 0);
			this.Controls.SetChildIndex(this.OkButton, 0);
			this.Controls.SetChildIndex(this.CanButton, 0);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties
		public string ColumnName 
		{
			get { return this.NameTextBox.Text; }
		}

		public string ColumnType
		{
			get { return this.TypeComboBox.SelectedItem.ToString(); }
		}
		#endregion
	}
}
