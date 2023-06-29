using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HDU_AppXetTuyen.Controllers
{
    public class KetQuaXetTuyensController : Controller
    {
        private DbConnecttion db = new DbConnecttion();
        // GET: KetQuaXetTuyens
        [ThiSinhSessionCheck]
        public ActionResult Index()
        {
            var session = Session["login_session"].ToString();
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();
            var nguyenvongs = db.DangKyXetTuyens.Include(l => l.Nganh).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            return View(nguyenvongs);
        }
    }
}