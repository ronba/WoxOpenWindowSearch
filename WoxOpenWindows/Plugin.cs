using Common;
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

            List<IWindowSearchProvider> Providers = new List<IWindowSearchProvider>();

            Providers.Add(new DesktopWindowSearch());

            string queryString = string.Empty;
            if (query.ActionParameters.Count > 0)
            {
                queryString = query.ActionParameters.First();
            }


            var providerEnumerator = Providers.GetEnumerator();
            providerEnumerator.MoveNext();

            var results = providerEnumerator.Current.FindWindow(queryString);
            while(providerEnumerator.MoveNext())
            {
                results.AddRange(providerEnumerator.Current.FindWindow(queryString));
            }

            return results;
        }


    }
}

