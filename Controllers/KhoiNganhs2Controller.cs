﻿using System;
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
    public class KhoiNganhs2Controller : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: KhoiNganhs2
        public ActionResult Index()
        {
            return View(db.KhoiNganhs.ToList());
        }

        // GET: KhoiNganhs2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoiNganh khoiNganh = db.KhoiNganhs.Find(id);
            if (khoiNganh == null)
            {
                return HttpNotFound();
            }
            return View(khoiNganh);
        }

        // GET: KhoiNganhs2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KhoiNganhs2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KhoiNganh_ID,KhoiNganh_Ten,KhoiNganh_GhiChu,KhoiNganh_TrangThai")] KhoiNganh khoiNganh)
        {
            if (ModelState.IsValid)
            {
                db.KhoiNganhs.Add(khoiNganh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khoiNganh);
        }

        // GET: KhoiNganhs2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoiNganh khoiNganh = db.KhoiNganhs.Find(id);
            if (khoiNganh == null)
            {
                return HttpNotFound();
            }
            return View(khoiNganh);
        }

        // POST: KhoiNganhs2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KhoiNganh_ID,KhoiNganh_Ten,KhoiNganh_GhiChu,KhoiNganh_TrangThai")] KhoiNganh khoiNganh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khoiNganh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khoiNganh);
        }

        // GET: KhoiNganhs2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoiNganh khoiNganh = db.KhoiNganhs.Find(id);
            if (khoiNganh == null)
            {
                return HttpNotFound();
            }
            return View(khoiNganh);
        }

        // POST: KhoiNganhs2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhoiNganh khoiNganh = db.KhoiNganhs.Find(id);
            db.KhoiNganhs.Remove(khoiNganh);
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
