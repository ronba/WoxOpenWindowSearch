using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Common
{
    public interface IWindowSearchProvider
    {
        List<ISwitchableWindow> FindWindow(string titleSearchString);
    }
}
