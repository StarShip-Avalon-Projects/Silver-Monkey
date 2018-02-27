using System.Windows.Forms;

namespace Controls
{
    public class ListBox_NoFlicker : ListBox
    {
        public ListBox_NoFlicker() : base()
        {
            this.DoubleBuffered = true;
        }
    }
}