using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DesktopWindowSearchProvider
{
    public class Win32APIWindowCallsProviders
    {
        public const  UInt32 SW_HIDE = 0;
        public const  UInt32 SW_SHOWNORMAL = 1;
        public const  UInt32 SW_NORMAL = 1;
        public const  UInt32 SW_SHOWMINIMIZED = 2;
        public const  UInt32 SW_SHOWMAXIMIZED = 3;
        public const  UInt32 SW_MAXIMIZE = 3;
        public const  UInt32 SW_SHOWNOACTIVATE = 4;
        public const  UInt32 SW_SHOW = 5;
        public const  UInt32 SW_MINIMIZE = 6;
        public const  UInt32 SW_SHOWMINNOACTIVE = 7;
        public const  UInt32 SW_SHOWNA = 8;
        public const  UInt32 SW_RESTORE = 9;
        public const  UInt32 WM_GETICON = 0x007F;
        public const  UInt32 ICON_SMALL2 = 0;
        public const  UInt32 ICON_BIG = 1;
        public const UInt32 IDI_APPLICATION = 0x7F00;
        static int GCL_HICON = -14;

        [DllImport("user32.dll")]
        public static extern int SetActiveWindow(int hwnd);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(int hWnd);

        [DllImport("USER32.DLL")]
        public static extern bool ShowWindow(int hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(int hWnd, ref WINDOWPLACEMENT lpwndpl);

        //http://www.pinvoke.net/default.aspx/user32.getwindowplacement
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }


        public static WINDOWPLACEMENT GetPlacement(int Handle)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(Handle, ref placement);

            return placement;
        }

        //Code to retrieve icon taken from here: http://stackoverflow.com/questions/304109/getting-the-icon-associated-with-a-running-application
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        static extern uint GetClassLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

        /// <summary>
        /// 64 bit version maybe loses significant 64-bit specific information
        /// </summary>
        static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
                return new IntPtr((long)GetClassLong32(hWnd, nIndex));
            else
                return GetClassLong64(hWnd, nIndex);
        }


        public static Image GetSmallWindowIcon(IntPtr hWnd)
        {
            try
            {
                IntPtr hIcon = default(IntPtr);

                hIcon = SendMessage(hWnd, WM_GETICON, (IntPtr)ICON_SMALL2, IntPtr.Zero);

                if (hIcon == IntPtr.Zero)
                    hIcon = GetClassLongPtr(hWnd, GCL_HICON);

                if (hIcon == IntPtr.Zero)
                    hIcon = LoadIcon(IntPtr.Zero, (IntPtr)0x7F00/*IDI_APPLICATION*/);

                if (hIcon != IntPtr.Zero)
                    return new Bitmap(Icon.FromHandle(hIcon).ToBitmap(), 16, 16);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
