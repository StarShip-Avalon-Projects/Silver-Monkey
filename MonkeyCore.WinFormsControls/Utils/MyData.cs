using System;
using System.Runtime.InteropServices;

public static class MyData
{
    public const int WM_COPYDATA = 74;

    public const int WM_USER = 1024;

    // Used for WM_COPYDATA for string messages
    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;

        public int cdData;

        public IntPtr lpData;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MyDataStructure
    {
        public int fID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 78)]
        public string lpName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 78)]
        public string lpTag;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
        public string lpMsg;
    }
}