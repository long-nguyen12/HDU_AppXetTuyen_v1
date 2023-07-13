using System.Web.Mvc;

namespace HDU_AppXetTuyen.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "AdminAuth", action = "Login", id = UrlParameter.Optional },
                new[] { "HDU_AppXetTuyen.Areas.Admin.Controllers" }
            );
        }
    }
}