using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Wox.Plugin;

namespace Common
{
    public interface IWindowSearchProvider
    {
        List<Result> FindWindow(string titleSearchString);
    }
}
