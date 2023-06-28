using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace HDU_AppXetTuyen.Controllers
{
    public class DangKyXetTuyensController : Controller
    {
        DbConnecttion db_tsdk = new DbConnecttion();
        DbConnecttion db_dkxt = new DbConnecttion();
        // GET: DangKyXetTuyens
        public ActionResult dkxthocba()
        {
            return View();
            /*
            if (Session["login_session"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Login");
            }*/
                        
        }
        public JsonResult dkxthocbaListAll()
        {          

            Session["login_session"] = "11111111"; // => vào chương trình k cần dòng code này

            if (Session["login_session"] != null)
            {
                string str_login_session = Session["login_session"].ToString();
                //ViewBag.str_login_session = str_login_session;

                var tsdk_Detail = db_tsdk.ThiSinhDangKies.FirstOrDefault(ts => ts.ThiSinh_MatKhau.Equals(str_login_session));

                long _thisinh_id = tsdk_Detail.ThiSinh_ID;

                var dangKyXetTuyens = db_dkxt.DangKyXetTuyens.Include(d => d.DoiTuong).
                                       Include(d => d.DotXetTuyen).
                                       Include(d => d.KhuVuc).
                                       Include(d => d.Nganh).
                                       Include(d => d.PhuongThucXetTuyen).
                                       Include(d => d.ThiSinhDangKy).
                                       Include(d => d.ToHopMon);

                //var model_dkxt_ts = db_dkxt.DangKyXetTuyens.Include().Where(x => x.ThiSinh_ID.Equals(_thisinh_id)).ToList();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult KhoiNganhListAll()
        {

            Session["login_session"] = "11111111"; // => vào chương trình k cần dòng code này

            if (Session["login_session"] != null)
            {
                string str_login_session = Session["login_session"].ToString();
                //ViewBag.str_login_session = str_login_session;

                var tsdk_Detail = db_tsdk.ThiSinhDangKies.FirstOrDefault(ts => ts.ThiSinh_MatKhau.Equals(str_login_session));

                long _thisinh_id = tsdk_Detail.ThiSinh_ID;

                var dangKyXetTuyens = db_dkxt.DangKyXetTuyens.Include(d => d.DoiTuong).
                                       Include(d => d.DotXetTuyen).
                                       Include(d => d.KhuVuc).
                                       Include(d => d.Nganh).
                                       Include(d => d.PhuongThucXetTuyen).
                                       Include(d => d.ThiSinhDangKy).
                                       Include(d => d.ToHopMon);

                //var model_dkxt_ts = db_dkxt.DangKyXetTuyens.Include().Where(x => x.ThiSinh_ID.Equals(_thisinh_id)).ToList();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}