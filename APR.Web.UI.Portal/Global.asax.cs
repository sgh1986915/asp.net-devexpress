using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.Web.Optimization;


namespace APR.Web.UI.Portal
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Default behavior
            // Bundles all .js files in folders such as "scripts" if URL pointed to it: http://localhost:54716/scripts/custom/js
            BundleTable.Bundles.EnableDefaultBundles();
            // Static bundle.
            // Access on url http://localhosthost:54716/StaticBundle
            Bundle b = new Bundle("~/ScriptsMin", typeof(JsMinify));
            //b.AddFile("~/scripts/aprmaster.js");
            ////b.AddFile("~/scripts/custom/GoToDefinition.js");
            ////b.AddFile("~/scripts/bundle/JScript1.js");
            b.AddDirectory("~/scripts","*.js",false);
            BundleTable.Bundles.Add(b);
            Bundle cb = new Bundle("~/CssMin", typeof(CssMinify));
            cb.AddDirectory("~/css/Custom", "*.css", false);
            BundleTable.Bundles.Add(cb);
           
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}
