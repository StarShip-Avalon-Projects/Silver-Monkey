using System.Windows.Forms;
using System.Runtime.InteropServices;
using static Controls.NativeMethods;

namespace Controls
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ListBox" />
    public class ScrollingListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollingListBox"/> class.
        /// </summary>
        public ScrollingListBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// Occurs when [on horizontal scroll].
        /// </summary>
        public event ScrollEventHandler OnHorizontalScroll;

        /// <summary>
        /// Occurs when [on vertical scroll].
        /// </summary>
        public event ScrollEventHandler OnVerticalScroll;

        /// <summary>
        /// WNDs the proc.
        /// </summary>
        /// <param name="msg">The MSG.</param>
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