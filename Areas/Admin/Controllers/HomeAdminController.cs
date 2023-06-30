using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class AdminSessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session == null || filterContext.HttpContext.Session["admin_login_session"] == null)
            {
                filterContext.Result = new RedirectResult("~/Admin/AdminAuth/Login");
            }
        }
    }
    public class HomeAdminController : Controller
    {
        // GET: Admin/Home
        [AdminSessionCheck]
        public ActionResult Index()
        {
            return View();
        }
    }
}