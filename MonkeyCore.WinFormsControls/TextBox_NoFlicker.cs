using System.Windows.Forms;

namespace Controls
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Windows.Forms.TextBox" />
    public class TextBox_NoFlicker : TextBox
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox_NoFlicker"/> class.
        /// </summary>
        public TextBox_NoFlicker() : base()
        {
            this.DoubleBuffered = true;
        }

        #endregion Public Constructors
    }
}