using System;
using System.Runtime.InteropServices;
using static MyData;

namespace Controls
{
    /// <summary>
    /// Native Win32 Methods
    /// </summary>
    public sealed class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        #region Public Fields

        public const int CFE_AUTOCOLOR = 1073741824;

        public const int CFE_BOLD = 1;

        public const int CFE_ITALIC = 2;

        public const int CFE_LINK = 32;

        public const int CFE_PROTECTED = 16;

        public const int CFE_STRIKEOUT = 8;

        public const int CFM_LINK = 32;

        public const byte CFU_UNDERLINEWORD = 2;

        public const int EM_GETCHARFORMAT = (WM_USER + 58);

        public const int EM_GETEVENTMASK = (WM_USER + 59);

        public const int EM_SETCHARFORMAT = (WM_USER + 68);

        public const int EM_SETEVENTMASK = (WM_USER + 69);

        public const int WM_SETREDRAW = 11;

        #endregion Public Fields

        #region Internal Fields

        public const int CFE_SUBSCRIPT = 65536;

        //  Superscript and subscript are
        public const int CFE_SUPERSCRIPT = 131072;

        public const int CFE_UNDERLINE = 4;

        //   mutually exclusive
        //  (*)
        public const int CFM_ALLCAPS = 128;

        //  (*)
        public const int CFM_ANIMATION = 262144;

        public const int CFM_BACKCOLOR = 67108864;

        public const int CFM_BOLD = 1;

        public const int CFM_CHARSET = 134217728;

        public const int CFM_COLOR = 1073741824;

        //  (*)
        public const int CFM_DISABLED = 8192;

        //  (*)
        public const int CFM_EMBOSS = 2048;

        internal const int CFM_FACE = 536870912;

        //  Displayed by 3.0
        internal const int CFM_HIDDEN = 256;

        //  (*)
        internal const int CFM_IMPRINT = 4096;

        internal const int CFM_ITALIC = 2;

        //  Displayed by 3.0
        internal const int CFM_KERNING = 1048576;

        internal const int CFM_LCID = 33554432;

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

        public const byte CFU_UNDERLINETHICK = 9;

        public const byte CFU_UNDERLINEWAVE = 8;

        public const int SCF_ALL = 4;

        public const int SCF_SELECTION = 1;

        internal const int SCF_WORD = 2;

        internal const int WM_USER = 1024;

        #endregion Internal Fields

        #region Public Enums

        public enum SBOrientation : int
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3,
        }

        //  (*) displayed as ordinary underline
        public enum ScrollInfoMask : uint
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_DISABLENOSCROLL = 0x8,
            SIF_TRACKPOS = 0x10,
            SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS),
        }

        #endregion Public Enums

        #region Public Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="fnBar"></param>
        /// <param name="lpsi"></param>
        /// <returns></returns>
        [DllImport("user32,dll.dll")]
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

        // For use with WM_COPYDATA and COPYDATASTRUCT
        /// <summary>
        ///
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
        /// Sets the scroll position.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nBar">The n bar.</param>
        /// <param name="nPos">The n position.</param>
        /// <param name="bRedraw">if set to <c>true</c> [b redraw].</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

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

        public struct CHARFORMAT2_STRUCT
        {
            #region Public Fields

            public byte bAnimation;

            public byte bCharSet;

            public byte bPitchAndFamily;

            public byte bReserved1;

            public byte bRevAuthor;

            public byte bUnderlineType;

            public int cbSize;

            public int crBackColor;

            public int crTextColor;

            public int dwEffects;

            public int dwMask;

            public int dwReserved;

            //  Color.ToArgb() -> int
            public int lcid;

            public short sSpacing;

            public short sStyle;

            public char[] szFaceName;

            public short wKerning;

            public short wWeight;

            public int yHeight;

            public int yOffset;

            #endregion Public Fields
        }

        //private SBOrientation fnBar;

        //private SCROLLINFO lpsi;

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct SCROLLINFO
        {
            public int cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }

        #endregion Public Structs

        //internal static IntPtr oldEventMask;

        //internal static int updating;
    }
}