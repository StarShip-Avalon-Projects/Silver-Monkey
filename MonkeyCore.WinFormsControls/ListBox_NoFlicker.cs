using System.Windows.Forms;

namespace Controls
{
    /// <summary>
    /// Double Buffered ListBox
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ListBox" />
    public class ListBox_NoFlicker : ListBox
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox_NoFlicker"/> class.
        /// </summary>
        public ListBox_NoFlicker() : base()
        {
            this.DoubleBuffered = true;
        }

        #endregion Public Constructors
    }
}