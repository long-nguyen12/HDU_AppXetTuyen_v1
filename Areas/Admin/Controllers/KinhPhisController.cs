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
    public class KinhPhisController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/KinhPhis
        public ActionResult Index()
        {
            //var kinhPhis = db.KinhPhis.Include(k => k.ThiSinhDangKy);
            return View();
        }

        // GET: Admin/KinhPhis/Details/5
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

        // GET: Admin/KinhPhis/Create
        public ActionResult Create()
        {
            ViewBag.Dkdt_NK_ID = new SelectList(db.DangKyDuThiNangKhieus, "Dkdt_NK_ID", "Dkdt_NK_GhiChu");
            ViewBag.Dkxt_ID = new SelectList(db.DangKyXetTuyenHBs, "Dkxt_ID", "Dkxt_GhiChu");
            ViewBag.Dkxt_Khac_ID = new SelectList(db.DangKyXetTuyenKhacs, "Dkxt_ID", "Dkxt_GhiChu");
            ViewBag.Dkxt_KQTQG_ID = new SelectList(db.DangKyXetTuyenKQTQGs, "Dkxt_KQTQG_ID", "Dkxt_KQTQG_GhiChu");
            ViewBag.Dkxt_TT_ID = new SelectList(db.DangKyXetTuyenThangs, "Dkxt_ID", "Dkxt_GhiChu");
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD");
            return View();
        }

        // POST: Admin/KinhPhis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KinhPhi_ID,ThiSinh_ID,Dkxt_ID,Dkxt_KQTQG_ID,Dkxt_TT_ID,Dkxt_Khac_ID,Dkdt_NK_ID,Ptxt_ID,Dxt_ID,KinhPhi_NgayThang,KinhPhi_SoTC,KinhPhi_TepMinhChung,KinhPhi_TrangThai,KinhPhi_GhiChu")] KinhPhi kinhPhi)
        {
            if (ModelState.IsValid)
            {
                db.KinhPhis.Add(kinhPhi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", kinhPhi.ThiSinh_ID);
            return View(kinhPhi);
        }

        // GET: Admin/KinhPhis/Edit/5
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

            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", kinhPhi.ThiSinh_ID);
            return View(kinhPhi);
        }

        // POST: Admin/KinhPhis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KinhPhi_ID,ThiSinh_ID,Dkxt_ID,Dkxt_KQTQG_ID,Dkxt_TT_ID,Dkxt_Khac_ID,Dkdt_NK_ID,Ptxt_ID,Dxt_ID,KinhPhi_NgayThang,KinhPhi_SoTC,KinhPhi_TepMinhChung,KinhPhi_TrangThai,KinhPhi_GhiChu")] KinhPhi kinhPhi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kinhPhi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", kinhPhi.ThiSinh_ID);
            return View(kinhPhi);
        }

        // GET: Admin/KinhPhis/Delete/5
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

        // POST: Admin/KinhPhis/Delete/5
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
