using System;
using System.Runtime.InteropServices;
using static Controls.NativeMethods;

namespace Controls
{
    /// <summary>
    ///
    /// </summary>
    public static class WindowsMessageing
    {
        /// <summary>
        /// Used for WM_COPYDATA for string messages
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            /// <summary>
            /// Any value the sender chooses.  Perhaps its main window handle?
            /// </summary>
            public IntPtr dwData;

            /// <summary>
            /// The count of bytes in the message.
            /// </summary>
            public int cbData;

            /// <summary>
            /// The address of the message.
            /// </summary>
            public IntPtr lpData;
        }

        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct FurreDataStructure
        {
            /// <summary>
            /// Furre Gameserver ID
            /// </summary>
            public int FurreId;

            /// <summary>
            /// The furre name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string FurreName;

            /// <summary>
            /// The lp tag
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 78)]
            public string lpTag;

            /// <summary>
            /// The furre message
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
            public string FurreMessage;
        }

        /// <summary>
        /// Allocate a pointer to an arbitrary structure on the global heap.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public static IntPtr IntPtrAlloc<T>(T param)
        {
            IntPtr retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, retval, false);
            return retval;
        }

        /// <summary>
        /// Free a pointer to an arbitrary structure from the global heap.
        /// </summary>
        /// <param name="preAllocated">The pre allocated.</param>
        /// <exception cref="NullReferenceException">Go Home</exception>
        public static void IntPtrFree(ref IntPtr preAllocated)
        {
            if (IntPtr.Zero == preAllocated)
                throw (new NullReferenceException("Go Home"));
            Marshal.FreeHGlobal(preAllocated);
            preAllocated = IntPtr.Zero;
        }

        /// <summary>
        /// Brings the application to front.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <returns></returns>
        public static bool BringAppToFront(int hWnd)
        {
            return SetForegroundWindow(hWnd);
        }

        /// <summary>
        /// Gets the window identifier.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="windowName">Name of the window.</param>
        /// <returns></returns>
        public static IntPtr GetWindowId(string className, string windowName)
        {
            return FindWindow(className, windowName);
        }

        /// <summary>
        /// Sends the windows message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="Msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns></returns>
        public static IntPtr SendWindowsMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr result = IntPtr.Zero;
            if (hWnd != IntPtr.Zero)
            {
                result = SendMessage(hWnd, Msg, wParam, lParam);
            }

            return result;
        }

        /// <summary>
        /// Sends the windows string message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="Name">The name.</param>
        /// <param name="fID">The f identifier.</param>
        /// <param name="Tag">The tag.</param>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public static IntPtr SendWindowsStringMessage(IntPtr hWnd, IntPtr wParam, string Name, int fID, string Tag, string msg)
        {
            IntPtr result = IntPtr.Zero;
            if (hWnd != IntPtr.Zero)
            {
                FurreDataStructure FurreData;
                FurreData.FurreName = Name;
                FurreData.FurreId = fID;
                FurreData.lpTag = Tag;
                FurreData.FurreMessage = msg;

                IntPtr pData = Marshal.AllocHGlobal(Marshal.SizeOf(FurreData));
                Marshal.StructureToPtr(FurreData, pData, true);
                //  Create the COPYDATASTRUCT you'll use to shuttle the data.
                COPYDATASTRUCT copy;
                copy.dwData = IntPtr.Zero;
                copy.lpData = pData;
                copy.cbData = Marshal.SizeOf(FurreData);
                IntPtr pCopy = Marshal.AllocHGlobal(Marshal.SizeOf(copy));
                Marshal.StructureToPtr(copy, pCopy, true);
                //  Send the message to the other application.
                result = SendMessage(hWnd, WM_COPYDATA, wParam, pCopy);
                Marshal.FreeHGlobal(pData);
                Marshal.FreeHGlobal(pCopy);
            }

            return result;
        }
    }
}