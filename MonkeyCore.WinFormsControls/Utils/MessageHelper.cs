using static MyData;
using static Controls.NativeMethods;
using System.Runtime.InteropServices;
using System;

namespace Utils
{
    // '' <summary>
    // '' Managed Windows Messaging
    // '' </summary>
    public class MessageHelper
    {
        public bool bringAppToFront(int hWnd)
        {
            return SetForegroundWindow(hWnd);
        }

        public IntPtr getWindowId(string className, string windowName)
        {
            return FindWindow(className, windowName);
        }

        public IntPtr SendWindowsMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr result = IntPtr.Zero;
            if ((hWnd != IntPtr.Zero))
            {
                result = SendMessage(hWnd, Msg, wParam, lParam);
            }

            return result;
        }

        public IntPtr SendWindowsStringMessage(IntPtr hWnd, IntPtr wParam, string Name, int fID, string Tag, string msg)
        {
            IntPtr result = IntPtr.Zero;
            if (hWnd != IntPtr.Zero)
            {
                MyDataStructure cds;
                cds.lpName = Name;
                cds.fID = fID;
                cds.lpTag = Tag;
                cds.lpMsg = msg;
                IntPtr pData = Marshal.AllocHGlobal(Marshal.SizeOf(cds));
                Marshal.StructureToPtr(cds, pData, true);
                //  Create the COPYDATASTRUCT you'll use to shuttle the data.
                COPYDATASTRUCT copy;
                copy.dwData = IntPtr.Zero;
                copy.lpData = pData;
                copy.cdData = Marshal.SizeOf(cds);
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