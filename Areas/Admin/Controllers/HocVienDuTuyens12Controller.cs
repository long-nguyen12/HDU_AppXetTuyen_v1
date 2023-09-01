using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HDU_AppXetTuyen.Models;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class HocVienDuTuyens12Controller : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/HocVienDuTuyens12
        public ActionResult Index()
        {
            var hocVienDuTuyens = db.HocVienDuTuyens.Include(h => h.DotXetTuyen).Include(h => h.HocVienDangKy).Include(h => h.NganhMaster);
            return View(hocVienDuTuyens.ToList());
        }

        // GET: Admin/HocVienDuTuyens12/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocVienDuTuyen hocVienDuTuyen = db.HocVienDuTuyens.Find(id);
            if (hocVienDuTuyen == null)
            {
                return HttpNotFound();
            }
            return View(hocVienDuTuyen);
        }

        // GET: Admin/HocVienDuTuyens12/Create
        public ActionResult Create()
        {
            ViewBag.Dxt_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten");
            ViewBag.HocVien_ID = new SelectList(db.HocVienDangKies, "HocVien_ID", "HocVien_HoDem");
            ViewBag.Nganh_Mt_ID = new SelectList(db.NganhMasters, "Nganh_Mt_ID", "Nganh_Mt_MaNganh");
            return View();
        }

        // POST: Admin/HocVienDuTuyens12/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DuTuyen_ID,HocVien_ID,DuTuyen_MaNghienCuu,Nganh_Mt_ID,HocVien_DKDTNgoaiNgu,HocVien_VanBangNgoaiNgu,HocVien_DoiTuongDuThi,HocVien_SoYeuLyLich,HocVien_MCBangDaiHoc,HocVien_MCBangDiem,HocVien_MCGiayKhamSucKhoe,HocVien_Anh46,HocVien_MCCCNN,HocVien_MCKhac,DuTuyen_NgayDangKy,DuTuyen_GhiChu,DuTuyen_TrangThai,Dxt_ID")] HocVienDuTuyen hocVienDuTuyen)
        {
            if (ModelState.IsValid)
            {
                db.HocVienDuTuyens.Add(hocVienDuTuyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Dxt_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", hocVienDuTuyen.Dxt_ID);
            ViewBag.HocVien_ID = new SelectList(db.HocVienDangKies, "HocVien_ID", "HocVien_HoDem", hocVienDuTuyen.HocVien_ID);
            ViewBag.Nganh_Mt_ID = new SelectList(db.NganhMasters, "Nganh_Mt_ID", "Nganh_Mt_MaNganh", hocVienDuTuyen.Nganh_Mt_ID);
            return View(hocVienDuTuyen);
        }

        // GET: Admin/HocVienDuTuyens12/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocVienDuTuyen hocVienDuTuyen = db.HocVienDuTuyens.Find(id);
            if (hocVienDuTuyen == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dxt_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", hocVienDuTuyen.Dxt_ID);
            ViewBag.HocVien_ID = new SelectList(db.HocVienDangKies, "HocVien_ID", "HocVien_HoDem", hocVienDuTuyen.HocVien_ID);
            ViewBag.Nganh_Mt_ID = new SelectList(db.NganhMasters, "Nganh_Mt_ID", "Nganh_Mt_MaNganh", hocVienDuTuyen.Nganh_Mt_ID);
            return View(hocVienDuTuyen);
        }

        // POST: Admin/HocVienDuTuyens12/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DuTuyen_ID,HocVien_ID,DuTuyen_MaNghienCuu,Nganh_Mt_ID,HocVien_DKDTNgoaiNgu,HocVien_VanBangNgoaiNgu,HocVien_DoiTuongDuThi,HocVien_SoYeuLyLich,HocVien_MCBangDaiHoc,HocVien_MCBangDiem,HocVien_MCGiayKhamSucKhoe,HocVien_Anh46,HocVien_MCCCNN,HocVien_MCKhac,DuTuyen_NgayDangKy,DuTuyen_GhiChu,DuTuyen_TrangThai,Dxt_ID")] HocVienDuTuyen hocVienDuTuyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hocVienDuTuyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dxt_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", hocVienDuTuyen.Dxt_ID);
            ViewBag.HocVien_ID = new SelectList(db.HocVienDangKies, "HocVien_ID", "HocVien_HoDem", hocVienDuTuyen.HocVien_ID);
            ViewBag.Nganh_Mt_ID = new SelectList(db.NganhMasters, "Nganh_Mt_ID", "Nganh_Mt_MaNganh", hocVienDuTuyen.Nganh_Mt_ID);
            return View(hocVienDuTuyen);
        }

        // GET: Admin/HocVienDuTuyens12/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocVienDuTuyen hocVienDuTuyen = db.HocVienDuTuyens.Find(id);
            if (hocVienDuTuyen == null)
            {
                return HttpNotFound();
            }
            return View(hocVienDuTuyen);
        }

        // POST: Admin/HocVienDuTuyens12/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HocVienDuTuyen hocVienDuTuyen = db.HocVienDuTuyens.Find(id);
            db.HocVienDuTuyens.Remove(hocVienDuTuyen);
            db.SaveChanges();
            return RedirectToAction("Index");
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
