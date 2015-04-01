using DesktopWindowSearchProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wox.Plugin;

namespace WoxOpenWindows
{
    public class WoxOpenWindowsPlugin : IPlugin
    {
        private PluginInitContext context;

        public void Init(PluginInitContext context)
        {
            this.context = context;
        }

        public List<Result> Query(Query query)
        {

            List<Result> results = new List<Result>();
            DesktopWindowSearch desktop = new DesktopWindowSearch();

            string queryString = string.Empty;
            if (query.ActionParameters.Count > 0)
            {
                queryString = query.ActionParameters.First();
            }
            
            foreach (var window in desktop.FindWindow(queryString).ToList())
            {
                results.Add(new Result()
                {
                    Title = window.WindowTitle,
                    Action = e =>
                    {
                        window.SwitchToMe();

                        return true;
                    }
                });
            }
            return results;
        }


    }
}

