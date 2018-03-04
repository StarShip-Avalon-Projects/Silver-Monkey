using System;
using System.Runtime.InteropServices;
using static Controls.WindowsMessageing;

namespace Controls
{
    /// <summary>
    /// Native Win32 Methods
    /// </summary>
    public sealed class NativeMethods
    {
        #region Public Fields

        /// <summary>
        /// The wm copydata
        /// </summary>
        public const int WM_COPYDATA = 74;

        #endregion Public Fields

        #region Internal Fields

        internal const int CFE_AUTOCOLOR = 1073741824;

        internal const int CFE_BOLD = 1;

        internal const int CFE_ITALIC = 2;

        internal const int CFE_LINK = 32;

        internal const int CFE_PROTECTED = 16;

        internal const int CFE_STRIKEOUT = 8;

        internal const int CFE_SUBSCRIPT = 65536;

        //  Superscript and subscript are
        internal const int CFE_SUPERSCRIPT = 131072;

        internal const int CFE_UNDERLINE = 4;

        //   mutually exclusive
        //  (*)
        internal const int CFM_ALLCAPS = 128;

        //  (*)
        internal const int CFM_ANIMATION = 262144;

        internal const int CFM_BACKCOLOR = 67108864;

        internal const int CFM_BOLD = 1;

        internal const int CFM_CHARSET = 134217728;

        internal const int CFM_COLOR = 1073741824;

        //  (*)
        internal const int CFM_DISABLED = 8192;

        //  (*)
        internal const int CFM_EMBOSS = 2048;

        internal const int CFM_FACE = 536870912;

        //  Displayed by 3.0
        internal const int CFM_HIDDEN = 256;

        //  (*)
        internal const int CFM_IMPRINT = 4096;

        internal const int CFM_ITALIC = 2;

        //  Displayed by 3.0
        internal const int CFM_KERNING = 1048576;

        internal const int CFM_LCID = 33554432;
        internal const int CFM_LINK = 32;

        internal const int CFM_OFFSET = 268435456;

        //  Hidden by 3.0
        internal const int CFM_OUTLINE = 512;

        internal const int CFM_PROTECTED = 16;

        //  (*)
        internal const int CFM_REVAUTHOR = 32768;

        internal const int CFM_REVISED = 16384;

        //  (*)
        internal const int CFM_SHADOW = 1024;

        internal const uint CFM_SIZE = 2_147_483_648;
        internal const int CFM_SMALLCAPS = 64;
        internal const int CFM_SPACING = 2097152;
        internal const int CFM_STRIKEOUT = 8;

        //  (*)
        internal const int CFM_STYLE = 524288;

        internal const int CFM_SUBSCRIPT = (CFE_SUBSCRIPT | CFE_SUPERSCRIPT);
        internal const int CFM_SUPERSCRIPT = CFM_SUBSCRIPT;
        internal const int CFM_UNDERLINE = 4;
        internal const int CFM_UNDERLINETYPE = 8388608;

        //  Many displayed by 3.0
        internal const int CFM_WEIGHT = 4194304;

        internal const byte CFU_UNDERLINE = 1;
        internal const byte CFU_UNDERLINEDASH = 5;
        internal const byte CFU_UNDERLINEDASHDOT = 6;
        internal const byte CFU_UNDERLINEDASHDOTDOT = 7;

        //  (*) displayed as ordinary underline
        internal const byte CFU_UNDERLINEDOTTED = 4;

        //  (*) displayed as ordinary underline
        internal const byte CFU_UNDERLINEDOUBLE = 3;

        internal const byte CFU_UNDERLINEHAIRLINE = 10;
        internal const byte CFU_UNDERLINENONE = 0;
        internal const byte CFU_UNDERLINETHICK = 9;

        internal const byte CFU_UNDERLINEWAVE = 8;

        internal const byte CFU_UNDERLINEWORD = 2;

        internal const int EM_GETCHARFORMAT = (WM_USER + 58);

        internal const int EM_GETEVENTMASK = (WM_USER + 59);

        internal const int EM_SETCHARFORMAT = (WM_USER + 68);

        internal const int EM_SETEVENTMASK = (WM_USER + 69);

        internal const int SB_ENDSCROLL = 8;
        internal const int SB_LEFT = 6;
        internal const int SB_LINEDOWN = 1;
        internal const int SB_LINELEFT = 0;
        internal const int SB_LINERIGHT = 1;
        internal const int SB_LINEUP = 0;
        internal const int SB_PAGEBOTTOM = 7;
        internal const int SB_PAGEDOWN = 3;
        internal const int SB_PAGELEFT = 2;
        internal const int SB_PAGERIGHT = 3;
        internal const int SB_PAGETOP = 6;
        internal const int SB_PAGEUP = 2;
        internal const int SB_RIGHT = 7;
        internal const int SB_THUMBPOSITION = 4;
        internal const int SB_THUMBTRACK = 5;
        internal const int SCF_ALL = 4;

        internal const int SCF_SELECTION = 1;

        internal const int SCF_WORD = 2;
        internal const int WM_HSCROLL = 276;
        internal const int WM_SETREDRAW = 11;
        internal const int WM_USER = 1024;
        internal const int WM_VSCROLL = 277;

        #endregion Internal Fields

        #region Public Enums

        /// <summary>
        ///
        /// </summary>
        public enum SBOrientation : int
        {
            /// <summary>
            /// The sb horz
            /// </summary>
            SB_HORZ = 0,

            /// <summary>
            /// The sb vert
            /// </summary>
            SB_VERT = 1,

            /// <summary>
            /// The sb control
            /// </summary>
            SB_CTL = 2,

            /// <summary>
            /// The sb both
            /// </summary>
            SB_BOTH = 3,
        }

        /// <summary>
        /// (*) displayed as ordinary underline
        /// </summary>
        public enum ScrollInfoMask : uint
        {
            /// <summary>
            /// The sif range
            /// </summary>
            SIF_RANGE = 0x1,

            /// <summary>
            /// The sif page
            /// </summary>
            SIF_PAGE = 0x2,

            /// <summary>
            /// The sif position
            /// </summary>
            SIF_POS = 0x4,

            /// <summary>
            /// The sif disablenoscroll
            /// </summary>
            SIF_DISABLENOSCROLL = 0x8,

            /// <summary>
            /// The sif trackpos
            /// </summary>
            SIF_TRACKPOS = 0x10,

            /// <summary>
            /// The sif all
            /// </summary>
            SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS),
        }

        #endregion Public Enums

        #region Public Methods

        /// <summary>
        /// Finds the window.
        /// </summary>
        /// <param name="lpClassName">Name of the lp class.</param>
        /// <param name="lpWindowName">Name of the lp window.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Finds the window ex.
        /// </summary>
        /// <param name="parentHandle">The parent handle.</param>
        /// <param name="childAfter">The child after.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="windowTitle">The window title.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        /// <summary>
        /// Gets the scroll information.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="fnBar">The function bar.</param>
        /// <param name="lpsi">The lpsi.</param>
        [DllImport("user32.dll")]
        public static extern void GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

        /// <summary>
        /// Gets the scroll position.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nBar">The n bar.</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetScrollPos(IntPtr hWnd, SBOrientation nBar);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nBar"></param>
        /// <param name="lpMinPos"></param>
        /// <param name="lpMaxPos"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetScrollRange(IntPtr hWnd, int nBar, ref int lpMinPos, ref int lpMaxPos);

        /// <summary>
        ///For use with WM_COPYDATA and COPYDATASTRUCT
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        /// <summary>
        /// Posts the message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="Msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Sets the foreground window.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(int hWnd);

        /// <summary>
        /// Sets the parent.
        /// </summary>
        /// <param name="hWndChild">The h WND child.</param>
        /// <param name="hWndNewParent">The h WND new parent.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        /// <summary>
        /// Sets the scroll position.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nBar">The n bar.</param>
        /// <param name="nPos">The n position.</param>
        /// <param name="bRedraw">if set to <c>true</c> [b redraw].</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        /// <summary>
        /// Sets the scroll position.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nBar">The n bar.</param>
        /// <param name="nPos">The n position.</param>
        /// <param name="bRedraw">if set to <c>true</c> [b redraw].</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, SBOrientation nBar, int nPos, bool bRedraw);

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Registers the window message.
        /// </summary>
        /// <param name="lpString">The lp string.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int RegisterWindowMessage(string lpString);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="Msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns></returns>
        [DllImport("USER32.DLL")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        #endregion Private Methods

        #region Public Structs

        /// <summary>
        ///
        /// </summary>
        public struct CHARFORMAT2_STRUCT
        {
            #region Public Fields

            /// <summary>
            /// The b animation
            /// </summary>
            public byte bAnimation;

            /// <summary>
            /// The b character set
            /// </summary>
            public byte bCharSet;

            /// <summary>
            /// The b pitch and family
            /// </summary>
            public byte bPitchAndFamily;

            /// <summary>
            /// The b reserved1
            /// </summary>
            public byte bReserved1;

            /// <summary>
            /// The b rev author
            /// </summary>
            public byte bRevAuthor;

            /// <summary>
            /// The b underline type
            /// </summary>
            public byte bUnderlineType;

            /// <summary>
            /// The cb size
            /// </summary>
            public int cbSize;

            /// <summary>
            /// The cr back color
            /// </summary>
            public int crBackColor;

            /// <summary>
            /// The cr text color
            /// </summary>
            public int crTextColor;

            /// <summary>
            /// The dw effects
            /// </summary>
            public int dwEffects;

            /// <summary>
            /// The dw mask
            /// </summary>
            public int dwMask;

            /// <summary>
            /// The dw reserved
            /// </summary>
            public int dwReserved;

            /// <summary>
            /// Color.ToArgb() -> int
            /// </summary>
            public int lcid;

            /// <summary>
            /// The s spacing
            /// </summary>
            public short sSpacing;

            /// <summary>
            /// The s style
            /// </summary>
            public short sStyle;

            /// <summary>
            /// The sz face name
            /// </summary>
            public char[] szFaceName;

            /// <summary>
            /// The w kerning
            /// </summary>
            public short wKerning;

            /// <summary>
            /// The w weight
            /// </summary>
            public short wWeight;

            /// <summary>
            /// The y height
            /// </summary>
            public int yHeight;

            /// <summary>
            /// The y offset
            /// </summary>
            public int yOffset;

            #endregion Public Fields
        }

        /// <summary>
        ///
        /// </summary>
        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct SCROLLINFO
        {
            /// <summary>
            /// The cb size
            /// </summary>
            public int cbSize;

            /// <summary>
            /// The f mask
            /// </summary>
            public ScrollInfoMask fMask;

            /// <summary>
            /// The n minimum
            /// </summary>
            public int nMin;

            /// <summary>
            /// The n maximum
            /// </summary>
            public int nMax;

            /// <summary>
            /// The n page
            /// </summary>
            public uint nPage;

            /// <summary>
            /// The n position
            /// </summary>
            public int nPos;

            /// <summary>
            /// The n track position
            /// </summary>
            public int nTrackPos;
        }

        #endregion Public Structs
    }
}