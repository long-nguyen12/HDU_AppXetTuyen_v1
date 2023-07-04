using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HDU_AppXetTuyen.Controllers
{
    public class DangKyXetTuyenThangController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: DangKyXetTuyenThang
        [ThiSinhSessionCheck]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllKhoiNganh()
        {
            var khoinganhs = db.KhoiNganhs.Select(n => new
            {
                KhoiNganh_ID = n.KhoiNganh_ID,
                KhoiNganh_Ten = n.KhoiNganh_Ten
            });
            return Json(khoinganhs.ToList(), JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllNganhs(string idKhoiNganh)
        {
            int id = int.Parse(idKhoiNganh);
            var nganhs = db.Nganhs.Where(n => n.KhoiNganh_ID == id).Select(n => new
            {
                Nganh_ID = n.Nganh_ID,
                Nganh_MaNganh = n.Nganh_MaNganh,
                NganhTenNganh = n.NganhTenNganh
            });
            if (nganhs != null)
                return Json(new { success = true, data = nganhs.ToList() }, JsonRequestBehavior.AllowGet);
            else return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [ThiSinhSessionCheck]
        public JsonResult GetAllNguyenVongs()
        {
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_HocLucLop12 = n.ThiSinh_HocLucLop12,
                ThiSinh_HanhKiemLop12 = n.ThiSinh_HanhKiemLop12
            }).FirstOrDefault();

            return Json(new
            {
                success = true,
                thisinh = ts,
                data = "",
            }, JsonRequestBehavior.AllowGet);
        }
    }
}