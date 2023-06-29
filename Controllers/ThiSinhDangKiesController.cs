using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HDU_AppXetTuyen.Models;

namespace HDU_AppXetTuyen.Controllers
{
    public class Update_ThiSinh
    {
        public string ThiSinh_CCCD  { get; set; }
        public string ThiSinh_HoLot { get; set; }
        public string ThiSinh_Ten { get; set; }
        public string ThiSinh_DienThoai { get; set; }
        public string ThiSinh_Email { get; set; }
        public string ThiSinh_NgaySinh { get; set; }
        public string ThiSinh_DanToc { get; set; }
        public string ThiSinh_GioiTinh { get; set; }
        public string ThiSinh_DCNhanGiayBao { get; set; }
        public string ThiSinh_HoKhauThuongTru { get; set; }
        public string KhuVuc_ID { get; set; }
        public string DoiTuong_ID { get; set; }
        public string ThiSinh_TruongCapBa_Ma { get; set; }
        public string ThiSinh_TruongCapBa { get; set; }
        public string ThiSinh_HoKhauThuongTru_Check { get; set; }
    }
    public class ThiSinhSessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session == null || filterContext.HttpContext.Session["login_session"] == null)
            {
                filterContext.Result = new RedirectResult("~/Auth/Index");
            }
        }
    }
    public class ThiSinhDangKiesController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: ThiSinhDangKies
        [ThiSinhSessionCheck]
        public ActionResult Index()
        {
            var session = Session["login_session"].ToString();
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();
            return View(thiSinh);
        }

        // GET: ThiSinhDangKies/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThiSinhDangKy thiSinhDangKy = db.ThiSinhDangKies.Find(id);
            if (thiSinhDangKy == null)
            {
                return HttpNotFound();
            }
            return View(thiSinhDangKy);
        }

        // GET: ThiSinhDangKies/Create
        public ActionResult Create()
        {
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten");
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten");
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten");
            return View();
        }

        // POST: ThiSinhDangKies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ThiSinh_ID,ThiSinh_CCCD,ThiSinh_MatKhau,ThiSinh_DienThoai,ThiSinh_Email,ThiSinh_HoLot,ThiSinh_Ten,ThiSinh_NgaySinh,ThiSinh_NamTotNghiep,ThiSinh_ResetCode,ThiSinh_NgayDangKy,ThiSinh_DanToc,ThiSinh_GioiTinh,ThiSinh_DCNhanGiayBao,ThiSinh_HoKhauThuongTru,ThiSinh_HoKhauThuongTru_Check,KhuVuc_ID,DoiTuong_ID,ThiSinh_TruongCapBa_Ma,ThiSinh_TruongCapBa,DotXT_ID,ThiSinh_TrangThai,ThiSinh_GhiChu")] ThiSinhDangKy thiSinhDangKy)
        {
            if (ModelState.IsValid)
            {
                db.ThiSinhDangKies.Add(thiSinhDangKy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", thiSinhDangKy.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", thiSinhDangKy.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", thiSinhDangKy.KhuVuc_ID);
            return View(thiSinhDangKy);
        }

        // GET: ThiSinhDangKies/Edit/5
        [ThiSinhSessionCheck]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThiSinhDangKy thiSinhDangKy = db.ThiSinhDangKies.Find(id);
            if (thiSinhDangKy == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", thiSinhDangKy.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", thiSinhDangKy.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", thiSinhDangKy.KhuVuc_ID);
            return View(thiSinhDangKy);
        }

        // POST: ThiSinhDangKies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ThiSinh_ID,ThiSinh_CCCD,ThiSinh_MatKhau,ThiSinh_DienThoai,ThiSinh_Email,ThiSinh_HoLot,ThiSinh_Ten,ThiSinh_NgaySinh,ThiSinh_NamTotNghiep,ThiSinh_ResetCode,ThiSinh_NgayDangKy,ThiSinh_DanToc,ThiSinh_GioiTinh,ThiSinh_DCNhanGiayBao,ThiSinh_HoKhauThuongTru,ThiSinh_HoKhauThuongTru_Check,KhuVuc_ID,DoiTuong_ID,ThiSinh_TruongCapBa_Ma,ThiSinh_TruongCapBa,DotXT_ID,ThiSinh_TrangThai,ThiSinh_GhiChu")] ThiSinhDangKy thiSinhDangKy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thiSinhDangKy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", thiSinhDangKy.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", thiSinhDangKy.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", thiSinhDangKy.KhuVuc_ID);
            return View(thiSinhDangKy);
        }

        // GET: ThiSinhDangKies/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThiSinhDangKy thiSinhDangKy = db.ThiSinhDangKies.Find(id);
            if (thiSinhDangKy == null)
            {
                return HttpNotFound();
            }
            return View(thiSinhDangKy);
        }

        // POST: ThiSinhDangKies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ThiSinhDangKy thiSinhDangKy = db.ThiSinhDangKies.Find(id);
            db.ThiSinhDangKies.Remove(thiSinhDangKy);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetThiSinhInfo(string id)
        {
            int idThiSinh = int.Parse(id);
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_ID == idThiSinh).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_CCCD = n.ThiSinh_CCCD,
                ThiSinh_MatKhau = n.ThiSinh_MatKhau,
                ThiSinh_HoLot = n.ThiSinh_HoLot,
                ThiSinh_Ten = n.ThiSinh_Ten,
                ThiSinh_DienThoai = n.ThiSinh_DienThoai,
                ThiSinh_Email = n.ThiSinh_Email,
                ThiSinh_NgaySinh = n.ThiSinh_NgaySinh,
                ThiSinh_DanToc = n.ThiSinh_DanToc,
                ThiSinh_GioiTinh = n.ThiSinh_GioiTinh,
                ThiSinh_DCNhanGiayBao = n.ThiSinh_DCNhanGiayBao,
                ThiSinh_HoKhauThuongTru = n.ThiSinh_HoKhauThuongTru,
                KhuVuc_ID = n.KhuVuc_ID,
                DoiTuong_ID = n.DoiTuong_ID,
                ThiSinh_TruongCapBa_Ma = n.ThiSinh_TruongCapBa_Ma,
                ThiSinh_TruongCapBa = n.ThiSinh_TruongCapBa,
                ThiSinh_TruongCapBa_Tinh_ID = n.ThiSinh_TruongCapBa_Tinh_ID,
                ThiSinh_TrangThai = n.ThiSinh_TrangThai,
                ThiSinh_GhiChu = n.ThiSinh_GhiChu,
                ThiSinh_HoKhauThuongTru_Check = n.ThiSinh_HoKhauThuongTru_Check

            }).FirstOrDefault();
            var tinhs = db.Tinhs.Select(n => new
            {
                Tinh_ID = n.Tinh_ID,
                Tinh_Ma = n.Tinh_Ma,
                Tinh_Ten = n.Tinh_Ten,
                Tinh_MaTen = n.Tinh_MaTen,
                Tinh_GhiChu = n.Tinh_GhiChu
            }).ToList();
            var khuvucs = db.KhuVucs.Select(n => new
            {
                KhuVuc_ID = n.KhuVuc_ID,
                KhuVuc_Ten = n.KhuVuc_Ten,
                KhuVuc_DiemUuTien = n.KhuVuc_DiemUuTien,
                KhuVuc_GhiChu = n.KhuVuc_GhiChu
            }).ToList();
            var doituongs = db.DoiTuongs.Select(n => new
            {
                DoiTuong_ID = n.DoiTuong_ID,
                DoiTuong_Ten = n.DoiTuong_Ten,
                DoiTuong_DiemUuTien = n.DoiTuong_DiemUuTien,
                DoiTuong_GhiChu = n.DoiTuong_GhiChu
            }).ToList();
            return Json(new { success = true, data = thiSinh, tinhs = tinhs, khuvucs = khuvucs, doituongs = doituongs }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateInfo(Update_ThiSinh thiSinh_Update)
        {
            var session = Session["login_session"].ToString();
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();

            ts.ThiSinh_CCCD = thiSinh_Update.ThiSinh_CCCD;
            ts.ThiSinh_HoLot = thiSinh_Update.ThiSinh_HoLot;
            ts.ThiSinh_Ten = thiSinh_Update.ThiSinh_Ten;
            ts.ThiSinh_DienThoai = thiSinh_Update.ThiSinh_DienThoai;
            ts.ThiSinh_Email = thiSinh_Update.ThiSinh_Email;
            ts.ThiSinh_NgaySinh = thiSinh_Update.ThiSinh_NgaySinh;
            ts.ThiSinh_DanToc = thiSinh_Update.ThiSinh_DanToc;
            ts.ThiSinh_GioiTinh = int.Parse(thiSinh_Update.ThiSinh_GioiTinh);
            ts.ThiSinh_DCNhanGiayBao = thiSinh_Update.ThiSinh_DCNhanGiayBao;
            ts.ThiSinh_HoKhauThuongTru = thiSinh_Update.ThiSinh_HoKhauThuongTru;
            ts.ThiSinh_HoKhauThuongTru_Check = thiSinh_Update.ThiSinh_HoKhauThuongTru_Check;
            ts.KhuVuc_ID = int.Parse(thiSinh_Update.KhuVuc_ID);
            ts.DoiTuong_ID = int.Parse(thiSinh_Update.DoiTuong_ID);
            ts.ThiSinh_TruongCapBa_Ma = thiSinh_Update.ThiSinh_TruongCapBa_Ma;
            ts.ThiSinh_TruongCapBa = thiSinh_Update.ThiSinh_TruongCapBa;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
