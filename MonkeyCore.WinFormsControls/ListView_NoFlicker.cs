using System.Windows.Forms;

namespace Controls
{
    public class ListView_NoFlicker : ListView
    {
        #region Public Constructors

        public ListView_NoFlicker() : base()
        {
            this.DoubleBuffered = true;
        }

        #endregion Public Constructors
    }
}