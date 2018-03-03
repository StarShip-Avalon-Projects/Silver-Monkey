using System.Windows.Forms;

namespace Controls
{
    /// <summary>
    ///
    /// </summary>
    public class ListView_NoFlicker : ListView
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListView_NoFlicker"/> class.
        /// </summary>
        public ListView_NoFlicker() : base()
        {
            this.DoubleBuffered = true;
        }

        #endregion Public Constructors
    }
}