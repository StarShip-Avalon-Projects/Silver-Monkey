using System.Windows.Forms;

namespace DataMonkey.Controls
{
    /// <summary>
    /// Summary description for AddTable.
    /// </summary>
    public class AddTable : BasePopup
    {
        private Label AddTableLabel;
        private TextBox AddTableTextBox;

        public AddTable()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }

        #region Properties

        public string TableName
        {
            get { return AddTableTextBox.Text; }
        }

        #endregion Properties

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify the contents of this method with the
        /// code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AddTableTextBox = new TextBox();
            AddTableLabel = new Label();
            SuspendLayout();
            // CanButton
            CanButton.Location = new System.Drawing.Point(136, 64);
            // OkButton
            OkButton.Location = new System.Drawing.Point(32, 64);
            // AddTableTextBox
            AddTableTextBox.AcceptsReturn = true;
            AddTableTextBox.Location = new System.Drawing.Point(8, 32);
            AddTableTextBox.Name = "AddTableTextBox";
            AddTableTextBox.Size = new System.Drawing.Size(232, 20);
            AddTableTextBox.TabIndex = 5;
            AddTableTextBox.KeyDown += new KeyEventHandler(tb_KeyDown);
            // AddTableLabel
            AddTableLabel.Location = new System.Drawing.Point(8, 8);
            AddTableLabel.Name = "AddTableLabel";
            AddTableLabel.Size = new System.Drawing.Size(232, 24);
            AddTableLabel.TabIndex = 4;
            AddTableLabel.Text = "Create table named:";
            AddTableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // AddTable
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(248, 101);
            Controls.Add(AddTableTextBox);
            Controls.Add(AddTableLabel);
            Name = "AddTable";
            Text = "Add Table";
            Controls.SetChildIndex(OkButton, 0);
            Controls.SetChildIndex(CanButton, 0);
            Controls.SetChildIndex(AddTableLabel, 0);
            Controls.SetChildIndex(AddTableTextBox, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion Windows Form Designer generated code
    }
}