using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HDU_AppXetTuyen.Models;
using HDU_AppXetTuyen.Ultils;
using PagedList;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class LienCapTieuHocsController : Controller
    {
        public string DEFAULT_URL = Constant.DEFAULT_URL;

        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/LienCapTieuHocs
        [AdminSessionCheck]
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var thisinhs = (from h in db.LienCapTieuHocs
                            select h).OrderBy(x => x.HocSinh_ID);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(thisinhs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/LienCapTieuHocs/Details/5
        [AdminSessionCheck]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            if (lienCapTieuHoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.URL = DEFAULT_URL;
            return View(lienCapTieuHoc);
        }

        // GET: Admin/LienCapTieuHocs/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LienCapTieuHocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminSessionCheck]
        public ActionResult Create([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongMN,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_NgheNghiepCha,HocSinh_DienThoaiCha,HocSinh_ThongTinMe,HocSinh_NgheNghiepMe,HocSinh_DienThoaiMe,HocSinh_MinhChungMN,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_TrangThai,HocSinh_GhiChu,HocSinh_MinhChungLePhi,HocSinh_Activation")] LienCapTieuHoc lienCapTieuHoc)
        {
            if (ModelState.IsValid)
            {
                db.LienCapTieuHocs.Add(lienCapTieuHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lienCapTieuHoc);
        }

        // GET: Admin/LienCapTieuHocs/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            if (lienCapTieuHoc == null)
            {
                return HttpNotFound();
            }
            return View(lienCapTieuHoc);
        }

        // POST: Admin/LienCapTieuHocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminSessionCheck]
        public ActionResult Edit([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongMN,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_NgheNghiepCha,HocSinh_DienThoaiCha,HocSinh_ThongTinMe,HocSinh_NgheNghiepMe,HocSinh_DienThoaiMe,HocSinh_MinhChungMN,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_TrangThai,HocSinh_GhiChu,HocSinh_MinhChungLePhi,HocSinh_Activation")] LienCapTieuHoc lienCapTieuHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lienCapTieuHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lienCapTieuHoc);
        }

        // GET: Admin/LienCapTieuHocs/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            if (lienCapTieuHoc == null)
            {
                return HttpNotFound();
            }
            return View(lienCapTieuHoc);
        }

        // POST: Admin/LienCapTieuHocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminSessionCheck]
        public ActionResult DeleteConfirmed(long id)
        {
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            db.LienCapTieuHocs.Remove(lienCapTieuHoc);
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
