using System.Windows.Forms;

namespace DataMonkey.Controls
{
    /// <summary>
    /// Summary description for Base.
    /// </summary>
    public class BasePopup : Form
    {
        #region Public Fields

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BasePopup.OkButton'
        public static Button OkButton;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BasePopup.OkButton'

        #endregion Public Fields

        #region Protected Fields

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BasePopup.CanButton'
        protected static Button CanButton;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BasePopup.CanButton'

        #endregion Protected Fields

        #region Private Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion Private Fields

        #region Public Constructors

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BasePopup.BasePopup()'

        public BasePopup()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BasePopup.BasePopup()'
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // TODO: Add any constructor code after InitializeComponent call
        }

        #endregion Public Constructors

        #region Protected Methods

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

        #endregion Protected Methods

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify the contents of this method with the
        /// code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CanButton = new Button();
            OkButton = new Button();
            SuspendLayout();
            // CanButton
            CanButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CanButton.Location = new System.Drawing.Point(152, 56);
            CanButton.Name = "CanButton";
            CanButton.TabIndex = 7;
            CanButton.Text = "Cancel";
            CanButton.Click += new System.EventHandler(CanButton_Click);
            // OkButton
            OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            OkButton.Location = new System.Drawing.Point(48, 56);
            OkButton.Name = "OkButton";
            OkButton.TabIndex = 6;
            OkButton.Text = "Ok";
            OkButton.Click += new System.EventHandler(OkButton_Click);
            // BasePopup
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(292, 101);
            Controls.Add(CanButton);
            Controls.Add(OkButton);
            Name = "BasePopup";
            Text = "Base";
            ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        #region Button Events

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BasePopup.tb_KeyDown(object, KeyEventArgs)'

        public static void Tb_KeyDown(object sender, KeyEventArgs e)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BasePopup.tb_KeyDown(object, KeyEventArgs)'
        {
            if (e.KeyCode == Keys.Enter)
            {
                //enter key is down
                OkButton.PerformClick();
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BasePopup.OkButton_Click(object, EventArgs)'

        public void OkButton_Click(object sender, System.EventArgs e)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BasePopup.OkButton_Click(object, EventArgs)'
        {
            Close();
        }

        private void CanButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        #endregion Button Events
    }
}