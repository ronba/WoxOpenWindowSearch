using System;
using Common;
using System.Runtime.InteropServices;
using System.Drawing;

//some code taken from here:
//http://stackoverflow.com/questions/2636721/bring-another-processes-window-to-foreground-when-it-has-showintaskbar-false

namespace DesktopWindowSearchProvider
{
    public class DesktopWindow : ISwitchableWindow
    {

        public int Handle { get; set; }
        public string WindowTitle { get; set; }

        public string WindowProcessName { get; set; }

        public string WindowIconPath { get; set; }

        public void SwitchToMe()
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