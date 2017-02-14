using System;
using System.Windows.Forms;

namespace SilverMonkey.DataMonkey.Controls
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

        #endregion Properties

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            RenameTableTextBox = new System.Windows.Forms.TextBox();
            RenameTableLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            //
            // CanButton
            //
            CanButton.Location = new System.Drawing.Point(136, 64);
            CanButton.Name = "CanButton";
            //
            // OkButton
            //
            OkButton.Location = new System.Drawing.Point(32, 64);
            OkButton.Name = "OkButton";
            OkButton.Click -= new System.EventHandler(OkButton_Click);
            OkButton.Click += new System.EventHandler(OkButton_Click2);
            //
            // RenameTableTextBox
            //
            RenameTableTextBox.Location = new System.Drawing.Point(8, 32);
            RenameTableTextBox.Name = "RenameTableTextBox";
            RenameTableTextBox.Size = new System.Drawing.Size(232, 20);
            RenameTableTextBox.TabIndex = 5;
            RenameTableTextBox.Text = "";
            RenameTableTextBox.KeyDown += new KeyEventHandler(tb_KeyDown);
            //
            // RenameTableLabel
            //
            RenameTableLabel.Location = new System.Drawing.Point(8, 8);
            RenameTableLabel.Name = "RenameTableLabel";
            RenameTableLabel.Size = new System.Drawing.Size(232, 24);
            RenameTableLabel.TabIndex = 4;
            RenameTableLabel.Text = "Change table name to:";
            RenameTableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // RenameTable
            //
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(248, 101);
            Controls.Add(RenameTableTextBox);
            Controls.Add(RenameTableLabel);
            Name = "RenameTable";
            Text = "Rename Table";
            Controls.SetChildIndex(OkButton, 0);
            Controls.SetChildIndex(CanButton, 0);
            Controls.SetChildIndex(RenameTableLabel, 0);
            Controls.SetChildIndex(RenameTableTextBox, 0);
            ResumeLayout(false);
        }

        private void OkButton_Click2(object sender, EventArgs e)
        {
            OkButton.DialogResult = MessageBox.Show("Do you want to rename this table?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            Close();
        }

        #endregion Windows Form Designer generated code
    }
}