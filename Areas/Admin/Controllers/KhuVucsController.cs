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
    public class KhuVucsController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/KhuVucs
        [AdminSessionCheck]
        public ActionResult Index()
        {
            return View(db.KhuVucs.Where(x => x.KhuVuc_ID >0).ToList());
        }

        // GET: Admin/KhuVucs/Details/5
        [AdminSessionCheck]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuVuc khuVuc = db.KhuVucs.Find(id);
            if (khuVuc == null)
            {
                return HttpNotFound();
            }
            return View(khuVuc);
        }

        // GET: Admin/KhuVucs/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhuVucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KhuVuc_ID,KhuVuc_Ten,KhuVuc_DiemUuTien,KhuVuc_GhiChu")] KhuVuc khuVuc)
        {
            if (ModelState.IsValid)
            {
                db.KhuVucs.Add(khuVuc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khuVuc);
        }

        // GET: Admin/KhuVucs/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuVuc khuVuc = db.KhuVucs.Find(id);
            if (khuVuc == null)
            {
                return HttpNotFound();
            }
            return View(khuVuc);
        }

        // POST: Admin/KhuVucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KhuVuc_ID,KhuVuc_Ten,KhuVuc_DiemUuTien,KhuVuc_GhiChu")] KhuVuc khuVuc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khuVuc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khuVuc);
        }

        // GET: Admin/KhuVucs/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuVuc khuVuc = db.KhuVucs.Find(id);
            if (khuVuc == null)
            {
                return HttpNotFound();
            }
            return View(khuVuc);
        }

        // POST: Admin/KhuVucs/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhuVuc khuVuc = db.KhuVucs.Find(id);
            db.KhuVucs.Remove(khuVuc);
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
