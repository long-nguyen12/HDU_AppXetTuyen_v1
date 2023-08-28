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
    public class KinhPhis2Controller : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/KinhPhis2
        public ActionResult Index()
        {
            var kinhPhis = db.KinhPhis.Include(k => k.DotXetTuyen).Include(k => k.PhuongThucXetTuyen).Include(k => k.ThiSinhDangKy);
            return View(kinhPhis.ToList());
        }

        // GET: Admin/KinhPhis2/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KinhPhi kinhPhi = db.KinhPhis.Find(id);
            if (kinhPhi == null)
            {
                return HttpNotFound();
            }
            return View(kinhPhi);
        }

        // GET: Admin/KinhPhis2/Create
        public ActionResult Create()
        {
            ViewBag.Dxt_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten");
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc");
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD");
            return View();
        }

        // POST: Admin/KinhPhis2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KinhPhi_ID,ThiSinh_ID,Dkxt_ID,Dxt_ID,Ptxt_ID,KinhPhi_NguyenVong,KinhPhi_SoTC,KinhPhi_NgayThang_NopMC,KinhPhi_NgayThang_CheckMC,KinhPhi_TepMinhChung,KinhPhi_TrangThai,KinhPhi_GhiChu")] KinhPhi kinhPhi)
        {
            if (ModelState.IsValid)
            {
                db.KinhPhis.Add(kinhPhi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Dxt_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", kinhPhi.Dxt_ID);
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc", kinhPhi.Ptxt_ID);
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", kinhPhi.ThiSinh_ID);
            return View(kinhPhi);
        }

        // GET: Admin/KinhPhis2/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KinhPhi kinhPhi = db.KinhPhis.Find(id);
            if (kinhPhi == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dxt_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", kinhPhi.Dxt_ID);
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc", kinhPhi.Ptxt_ID);
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", kinhPhi.ThiSinh_ID);
            return View(kinhPhi);
        }

        // POST: Admin/KinhPhis2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KinhPhi_ID,ThiSinh_ID,Dkxt_ID,Dxt_ID,Ptxt_ID,KinhPhi_NguyenVong,KinhPhi_SoTC,KinhPhi_NgayThang_NopMC,KinhPhi_NgayThang_CheckMC,KinhPhi_TepMinhChung,KinhPhi_TrangThai,KinhPhi_GhiChu")] KinhPhi kinhPhi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kinhPhi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dxt_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", kinhPhi.Dxt_ID);
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc", kinhPhi.Ptxt_ID);
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", kinhPhi.ThiSinh_ID);
            return View(kinhPhi);
        }

        // GET: Admin/KinhPhis2/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KinhPhi kinhPhi = db.KinhPhis.Find(id);
            if (kinhPhi == null)
            {
                return HttpNotFound();
            }
            return View(kinhPhi);
        }

        // POST: Admin/KinhPhis2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            KinhPhi kinhPhi = db.KinhPhis.Find(id);
            db.KinhPhis.Remove(kinhPhi);
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
