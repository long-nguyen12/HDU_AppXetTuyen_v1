using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using HDU_AppXetTuyen.Models;
using HDU_AppXetTuyen.Ultils;
using PagedList;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class LienCapTHCSController : Controller
    {
        private DbConnecttion db = new DbConnecttion();
        public string DEFAULT_URL = Constant.DEFAULT_URL;

        // GET: Admin/LienCapTHCS
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var thisinhs = (from h in db.LienCapTHCSs
                            select h).OrderBy(x => x.HocSinh_ID);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(thisinhs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/LienCapTHCS/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTHCS lienCapTHCS = db.LienCapTHCSs.Find(id);
            if (lienCapTHCS == null)
            {
                return HttpNotFound();
            }
            ViewBag.URL = DEFAULT_URL;
            return View(lienCapTHCS);
        }

        // GET: Admin/LienCapTHCS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LienCapTHCS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongTH,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_ThongTinMe,HocSinh_DiemHocTap,HocSinh_MucDoNangLuc,HocSinh_MucDoPhamChat,HocSinh_MinhChungHB,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_Activation,HocSinh_TrangThai,HocSinh_GhiChu")] LienCapTHCS lienCapTHCS)
        {
            if (ModelState.IsValid)
            {
                db.LienCapTHCSs.Add(lienCapTHCS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lienCapTHCS);
        }

        // GET: Admin/LienCapTHCS/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTHCS lienCapTHCS = db.LienCapTHCSs.Find(id);
            if (lienCapTHCS == null)
            {
                return HttpNotFound();
            }
            return View(lienCapTHCS);
        }

        // POST: Admin/LienCapTHCS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongTH,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_ThongTinMe,HocSinh_DiemHocTap,HocSinh_MucDoNangLuc,HocSinh_MucDoPhamChat,HocSinh_MinhChungHB,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_Activation,HocSinh_TrangThai,HocSinh_GhiChu")] LienCapTHCS lienCapTHCS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lienCapTHCS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lienCapTHCS);
        }

        // GET: Admin/LienCapTHCS/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTHCS lienCapTHCS = db.LienCapTHCSs.Find(id);
            if (lienCapTHCS == null)
            {
                return HttpNotFound();
            }
            return View(lienCapTHCS);
        }

        // POST: Admin/LienCapTHCS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            LienCapTHCS lienCapTHCS = db.LienCapTHCSs.Find(id);
            db.LienCapTHCSs.Remove(lienCapTHCS);
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
