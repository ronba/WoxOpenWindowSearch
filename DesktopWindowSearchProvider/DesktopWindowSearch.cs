using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Wox.Plugin;

//Some code here taken from: Mike Corcoran
//http://stackoverflow.com/questions/10819575/how-to-list-active-application-windows-using-c-sharp


namespace DesktopWindowSearchProvider
{
    /// <summary>
    /// This class is used to select windows from handles
    /// </summary>
    public class DesktopWindowSearch : IWindowSearchProvider
    {

        private List<Result> windowList = new List<Result>();

        public DesktopWindowSearch()
        {
            Initialize();
        }

        public List<Result> FindWindow(string titleSearchString)
        {
            List<Result> output = new List<Result>();
            
            foreach (Result window in windowList)
            {
                bool isMatch = false;
                bool processWindowTitleMatch= window.Title.ToLower().Contains(titleSearchString.ToLower());
                bool processNameMatch = window.SubTitle.ToLower().Contains(titleSearchString.ToLower());
                if (processWindowTitleMatch || processNameMatch)
                {
                    isMatch = true;
                    output.Add(window);
                }
                else
                {
                    try
                    {
                        Regex r = new Regex(titleSearchString, RegexOptions.IgnoreCase);
                        //string is sometimes not good enough for regex...
                        if (r.IsMatch(window.Title))
                        {
                            isMatch = true;
                            output.Add(window);
                        }
                    }
                    catch
                    {
                        //go to the next one, nothing to see here.
                    }
                }

            }
            return output;
        }

        public void Initialize()
        {
            EnumWindows(new WindowEnumCallback(this.AddWnd), 0);
        }

        public delegate bool WindowEnumCallback(int hwnd, int lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(WindowEnumCallback lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        public static extern void GetWindowText(int h, StringBuilder s, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(int h);

        private List<string> Windows = new List<string>();
        private bool AddWnd(int hwnd, int lparam)
        {
            if (IsWindowVisible(hwnd))
            {
                StringBuilder sb = new StringBuilder(255);
                GetWindowText(hwnd, sb, sb.Capacity);
                if (sb.Length > 0)
                {
                    var p = Win32APIWindowCallsProviders.GetProcessObjectFromHandle(hwnd);
                    Result result = new Result()
                    {
                        Action = e =>
                        {
                            SwitchToWindow(hwnd);
                            return true;
                        },
                        Title = sb.ToString(),
                        SubTitle = p.ProcessName + ".exe - " + p.Id
                    };

                    //This thing takes a LOT of time...
                    try {
                        //result.IcoPath = p.Modules[0].FileName;
                    }
                    catch
                    {
                        //if we're here - it means we won't be able to switch to this process ! 
                        //it's running in a higher level context! let's assign it a super low priority
                        //also add a caption
                        result.Score = -1000;
                        result.IcoPath = string.Empty;
                        result.SubTitle += " < Process is running in a higher security context - Consider running Wox as Administrator >";
                    }
                    windowList.Add(result);
                }
            }
            return true;
        }

        public void SwitchToWindow(int Handle)
        {
            Win32APIWindowCallsProviders.SetActiveWindow(Handle);
            Win32APIWindowCallsProviders.ShowWindow(Handle, (int)Win32APIWindowCallsProviders.SW_SHOW); //SW_SHOW
            var placement = Win32APIWindowCallsProviders.GetPlacement(Handle);

            //Check if the window is minimized
            if (placement.showCmd == Win32APIWindowCallsProviders.SW_SHOWMINIMIZED)
            {
                Win32APIWindowCallsProviders.ShowWindow(Handle, (int)Win32APIWindowCallsProviders.SW_RESTORE); //SW_RESTORE
            }
            Win32APIWindowCallsProviders.SetForegroundWindow(Handle);

        }
    }
}
