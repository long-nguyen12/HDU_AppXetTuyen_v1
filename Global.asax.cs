using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HDU_AppXetTuyen
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 30;
            Session["login_session"] = false;           
            Session["login_session"] = null;
            // lưu họ tên, điện thoại, email, đơn vị vào log
        }
        protected void Session_End(object sender, EventArgs e)
        {
            Session["login_session"] = false;
            Session["login_session"] = null;          
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
        }
    }
}
