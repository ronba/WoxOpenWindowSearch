using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

//Some code here taken from: Mike Corcoran
//http://stackoverflow.com/questions/10819575/how-to-list-active-application-windows-using-c-sharp


namespace DesktopWindowSearchProvider
{
    /// <summary>
    /// This class is used to select windows from handles
    /// </summary>
    public class DesktopWindowSearch : IWindowSearchProvider
    {

        private List<DesktopWindow> windowList = new List<DesktopWindow>();

        public DesktopWindowSearch()
        {
            Initialize();
        }

        public List<ISwitchableWindow> FindWindow(string titleSearchString)
        {
            List<ISwitchableWindow> output = new List<ISwitchableWindow>();
            foreach(ISwitchableWindow window in windowList)
            {
                //If we have a direct match go ahead and add
                //lets disregard capital letters here
                if (window.WindowTitle.ToLower().Contains(titleSearchString.ToLower()))
                {
                    output.Add(window);
                }
                else
                {
                    try
                    {
                        Regex r = new Regex(titleSearchString, RegexOptions.IgnoreCase);
                        //string is sometimes not good enough for regex...
                        if (r.IsMatch(window.WindowTitle)) {
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

        public void SwitchToWindow(ISwitchableWindow Window)
        {
            Window.SwitchToMe();
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
                    windowList.Add(new DesktopWindow()
                    {
                        Handle = hwnd,
                        WindowTitle = sb.ToString()
                    });
                    
                }

                
                
            }
            return true;
        }
    }
}
