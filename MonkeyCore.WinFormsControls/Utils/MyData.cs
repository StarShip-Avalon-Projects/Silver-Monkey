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
        public const int WM_COPYDATA = 74;

        public const int WM_USER = 1024;

        /// <summary>
        /// Used for WM_COPYDATA for string messages
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            // Any value the sender chooses.  Perhaps its main window handle?
            public IntPtr dwData;

            // The count of bytes in the message.
            public int cbData;

            // The address of the message.
            public IntPtr lpData;
        }

        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct FurreDataStructure
        {
            public int FurreId;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string FurreName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 78)]
            public string lpTag;

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