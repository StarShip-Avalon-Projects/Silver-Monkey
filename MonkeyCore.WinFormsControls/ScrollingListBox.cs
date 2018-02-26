using System.Windows.Forms;
using System.Runtime.InteropServices;
using static Controls.NativeMethods;

namespace Controls
{
    public class ScrollingListBox : ListBox
    {
        public ScrollingListBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public event ScrollEventHandler OnHorizontalScroll;

        public event ScrollEventHandler OnVerticalScroll;

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_HSCROLL)
            {
                // If OnHorizontalScroll IsNot Nothing Then
                SCROLLINFO si = new SCROLLINFO
                {
                    fMask = ScrollInfoMask.SIF_ALL
                };
                si.cbSize = Marshal.SizeOf(si);
                GetScrollInfo(msg.HWnd, 0, ref si);
                if (msg.WParam.ToInt32() == SB_ENDSCROLL)
                {
                    ScrollEventArgs sargs = new ScrollEventArgs(ScrollEventType.EndScroll, si.nPos);
                    OnHorizontalScroll(this, sargs);
                }

                // End If
                if (msg.Msg == WM_VSCROLL)
                {
                    si.cbSize = Marshal.SizeOf(si);
                    GetScrollInfo(msg.HWnd, 0, ref si);
                    if (msg.WParam.ToInt32() == SB_ENDSCROLL)
                    {
                        ScrollEventArgs sargs = new ScrollEventArgs(ScrollEventType.EndScroll, si.nPos);
                        OnVerticalScroll(this, sargs);
                    }

                    // End If
                }
            }
            else base.WndProc(ref msg);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Size = new System.Drawing.Size(120, 95);
            ResumeLayout(false);
        }
    }
}