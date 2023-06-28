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
    public class NganhsController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/Nganhs
        public ActionResult Index()
        {
            var nganh = db.Nganh.Include(n => n.KhoiNganh);
            return View(nganh.ToList());
        }

        // GET: Admin/Nganhs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganh.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            return View(nganh);
        }

        // GET: Admin/Nganhs/Create
        public ActionResult Create()
        {
            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten");
            return View();
        }

        // POST: Admin/Nganhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nganh_ID,Nganh_MaNganh,NganhTenNganh,Khoa_ID,Nganh_GhiChu,KhoiNganh_ID")] Nganh nganh)
        {
            if (ModelState.IsValid)
            {
                db.Nganh.Add(nganh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten", nganh.KhoiNganh_ID);
            return View(nganh);
        }

        // GET: Admin/Nganhs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganh.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten", nganh.KhoiNganh_ID);
            return View(nganh);
        }

        // POST: Admin/Nganhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nganh_ID,Nganh_MaNganh,NganhTenNganh,Khoa_ID,Nganh_GhiChu,KhoiNganh_ID")] Nganh nganh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nganh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten", nganh.KhoiNganh_ID);
            return View(nganh);
        }

        // GET: Admin/Nganhs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganh.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            return View(nganh);
        }

        // POST: Admin/Nganhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nganh nganh = db.Nganh.Find(id);
            db.Nganh.Remove(nganh);
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
