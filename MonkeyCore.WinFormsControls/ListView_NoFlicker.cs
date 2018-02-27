using System.Windows.Forms;

namespace Controls
{
    public class ListView_NoFlicker : ListView
    {
        public ListView_NoFlicker() : base()
        {
            this.DoubleBuffered = true;
        }
    }
}