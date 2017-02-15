using System.Windows.Forms;

namespace DataMonkey.Controls
{
    /// <summary>
    /// Summary description for AddColumn.
    /// </summary>
    public class AddColumn : BasePopup
    {
        private Label NameLabel;
        private TextBox NameTextBox;
        private ComboBox TypeComboBox;
        private Label TypeLabel;

        public AddColumn()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify the contents of this method with the
        /// code editor.
        /// </summary>
        private void InitializeComponent()
        {
            NameTextBox = new TextBox();
            NameLabel = new Label();
            TypeLabel = new Label();
            TypeComboBox = new ComboBox();
            SuspendLayout();
            // CanButton
            CanButton.Location = new System.Drawing.Point(144, 80);
            CanButton.Name = "CanButton";
            // OkButton
            OkButton.Location = new System.Drawing.Point(32, 80);
            OkButton.Name = "OkButton";
            // NameTextBox
            NameTextBox.Location = new System.Drawing.Point(96, 8);
            NameTextBox.Name = "NameTextBox";
            NameTextBox.Size = new System.Drawing.Size(144, 20);
            NameTextBox.TabIndex = 0;
            NameTextBox.Text = "";
            NameTextBox.KeyDown += new KeyEventHandler(tb_KeyDown);
            // NameLabel
            NameLabel.Location = new System.Drawing.Point(8, 8);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new System.Drawing.Size(80, 23);
            NameLabel.TabIndex = 1;
            NameLabel.Text = "Name:";
            NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // TypeLabel
            TypeLabel.Location = new System.Drawing.Point(8, 40);
            TypeLabel.Name = "TypeLabel";
            TypeLabel.Size = new System.Drawing.Size(80, 23);
            TypeLabel.TabIndex = 2;
            TypeLabel.Text = "Type:";
            TypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // TypeComboBox
            TypeComboBox.Items.AddRange(new object[] {
                                                              "TEXT",
                                                              "INTEGER",
                                                              "DOUBLE",
                                                              "REAL"});
            TypeComboBox.Location = new System.Drawing.Point(96, 40);
            TypeComboBox.Name = "TypeComboBox";
            TypeComboBox.Size = new System.Drawing.Size(144, 21);
            TypeComboBox.TabIndex = 3;
            // AddColumn
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(248, 117);
            Controls.Add(NameTextBox);
            Controls.Add(TypeComboBox);
            Controls.Add(TypeLabel);
            Controls.Add(NameLabel);
            Name = "AddColumn";
            Text = "     Add Column";
            Controls.SetChildIndex(NameLabel, 0);
            Controls.SetChildIndex(TypeLabel, 0);
            Controls.SetChildIndex(TypeComboBox, 0);
            Controls.SetChildIndex(NameTextBox, 0);
            Controls.SetChildIndex(OkButton, 0);
            Controls.SetChildIndex(CanButton, 0);
            ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        #region Properties

        public string ColumnName
        {
            get { return NameTextBox.Text; }
        }

        public string ColumnType
        {
            //x != 0.0 ? Math.Sin(x) / x : 1.0;
            get { return TypeComboBox.SelectedItem != null ? TypeComboBox.SelectedItem.ToString() : "TEXT"; }
        }

        #endregion Properties
    }
}