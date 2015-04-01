using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface ISwitchableWindow
    {
        void SwitchToMe();
        string WindowTitle { get; set; }

        string WindowIconPath { get; set; }

        string WindowProcessName { get; set; }
    }
}
