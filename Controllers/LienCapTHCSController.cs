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
    public class LienCapTHCSController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: LienCapTHCS
        public ActionResult Index()
        {
            return View(db.LienCapTHCSs.ToList());
        }

        // GET: LienCapTHCS/Details/5
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
            return View(lienCapTHCS);
        }

        // GET: LienCapTHCS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LienCapTHCS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongTH,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_ThongTinMe,HocSinh_DiemHocTap,HocSinh_MucDoNangLuc,HocSinh_MucDoPhamChat,HocSinh_MinhChungHB,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_TrangThai,HocSinh_GhiChu")] LienCapTHCS lienCapTHCS)
        {
            if (ModelState.IsValid)
            {
                db.LienCapTHCSs.Add(lienCapTHCS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lienCapTHCS);
        }

        // GET: LienCapTHCS/Edit/5
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

        // POST: LienCapTHCS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongTH,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_ThongTinMe,HocSinh_DiemHocTap,HocSinh_MucDoNangLuc,HocSinh_MucDoPhamChat,HocSinh_MinhChungHB,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_TrangThai,HocSinh_GhiChu")] LienCapTHCS lienCapTHCS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lienCapTHCS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lienCapTHCS);
        }

        // GET: LienCapTHCS/Delete/5
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

        // POST: LienCapTHCS/Delete/5
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
