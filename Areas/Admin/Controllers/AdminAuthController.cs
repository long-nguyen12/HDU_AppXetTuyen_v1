using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class AdminAuthController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/AdminAuth
        public ActionResult Index()
        {
            if (Session["admin_login_session"] != null)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                return RedirectToAction("Index", "HomeAdmin");
            }
            return RedirectToAction("Login", "AdminAuth");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Login()
        {
            init_user();
            if (Session["admin_login_session"] != null)
            {
                return RedirectToAction("Index", "HomeAdmin");
            }
            return View();
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminAccount admin_login)
        {
            if (Session["admin_login_session"] != null)
            {
                return RedirectToAction("Index", "HomeAdmin");
            }
            string admin_password = admin_login.Admin_Pass;
            string admin_user = admin_login.Admin_Username;

            var login_details = db.AdminAccounts.Where(x => x.Admin_Username == admin_login.Admin_Username).FirstOrDefault();
            if (login_details != null)
            {
                bool verifiedPassword = Verify(admin_user, admin_password, login_details.Admin_Pass);
                if (verifiedPassword == true)
                {
                    Session["admin_login_session"] = login_details.Admin_Pass;
                    ViewBag.LoginErrorMessager = "";
                    return RedirectToAction("Index", "HomeAdmin");
                }
                else
                {
                    ViewBag.LoginErrorMessager = "Mật khẩu không chính xác";
                    return View();
                }
            }
            else
            {
                ViewBag.LoginErrorMessager = "Không tìm thấy thông tin tài khoản";
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AdminAccount admin_register)
        {
            if (Session["admin_login_session"] != null)
            {
                return RedirectToAction("Index", "HomeAdmin");
            }
            string admin_password = admin_register.Admin_Pass;
            string admin_user = admin_register.Admin_Username;

            var login_details = db.AdminAccounts.Where(x => x.Admin_Username == admin_user).FirstOrDefault();
            if (login_details == null)
            {
                var password = ComputeHash(admin_user, admin_password);
                admin_register.Admin_Pass = password;
                admin_register.Admin_Quyen = "";
                admin_register.Admin_Ho = "";
                admin_register.Admin_Ten = "";
                db.AdminAccounts.Add(admin_register);
                db.SaveChanges();
                return RedirectToAction("Login", "AdminAuth");
            }
            else
            {
                ViewBag.LoginErrorMessager = "Tài khoản đã tồn tại";
                return View();
            }
        }

        public void init_user()
        {
            AdminAccount  adminAccount = new AdminAccount();
            string admin_user= "admin";
            string admin_pass = "Admin123@";

            var login_details = db.AdminAccounts.Where(x => x.Admin_Username == admin_user).FirstOrDefault();
            if (login_details == null)
            {
                var password = ComputeHash(admin_user, admin_pass);
                adminAccount.Admin_Pass = password;
                adminAccount.Admin_Username = admin_user;
                adminAccount.Admin_Quyen = "1";
                adminAccount.Admin_Ho = "";
                adminAccount.Admin_Ten = "admin";
                adminAccount.Khoa_ID = 1;
                db.AdminAccounts.Add(adminAccount);
                db.SaveChanges();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("login_details đã tồn tại.");
            }
        }

        public ActionResult Logout()
        {
            Session["admin_login_session"] = null;
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login", "AdminAuth");
        }

        public string ComputeHash(string input_user, string input_pass)
        {
            string input = input_user.Trim() + input_pass.Trim();
            string hashedPassword = BC.HashPassword(input);
            return hashedPassword;
        }

        public bool Verify(string input_user, string input_pass, string hash_pass)
        {
            string input = input_user.Trim() + input_pass.Trim();
            bool isPasswordValid = BC.Verify(input, hash_pass);
            return isPasswordValid;
        }
    }
}
