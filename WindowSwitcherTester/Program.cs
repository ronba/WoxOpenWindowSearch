using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace WindowSwitcherTester
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var desktop = new DesktopWindowSearchProvider.DesktopWindowSearch();
                var a = desktop.FindWindow(args[0]);

                foreach (var window in a)
                {
                    window.SwitchToMe();
                }
            }           
        }
    }
}
