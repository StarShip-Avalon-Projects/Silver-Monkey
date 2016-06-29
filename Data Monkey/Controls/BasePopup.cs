namespace SilverMonkey.SQLiteEditor.Controls
{
    /// <summary>
    /// Summary description for Base.
    /// </summary>
    public class BasePopup : System.Windows.Forms.Form
	{
		protected System.Windows.Forms.Button CanButton;
		protected System.Windows.Forms.Button OkButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BasePopup()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.CanButton = new System.Windows.Forms.Button();
			this.OkButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// CanButton
			// 
			this.CanButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CanButton.Location = new System.Drawing.Point(152, 56);
			this.CanButton.Name = "CanButton";
			this.CanButton.TabIndex = 7;
			this.CanButton.Text = "Cancel";
			this.CanButton.Click += new System.EventHandler(this.CanButton_Click);
			// 
			// OkButton
			// 
			this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkButton.Location = new System.Drawing.Point(48, 56);
			this.OkButton.Name = "OkButton";
			this.OkButton.TabIndex = 6;
			this.OkButton.Text = "Ok";
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// BasePopup
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 101);
			this.Controls.Add(this.CanButton);
			this.Controls.Add(this.OkButton);
			this.Name = "BasePopup";
			this.Text = "Base";
			this.ResumeLayout(false);

		}
		#endregion

		#region Button Events
		private void OkButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void CanButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
		#endregion
	}
}
