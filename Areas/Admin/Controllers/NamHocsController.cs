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
    public class NamHocsController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/NamHocs
        [AdminSessionCheck]
        public ActionResult Index()
        {
            return View(db.NamHocs.ToList());
        }

        // GET: Admin/NamHocs/Details/5
        [AdminSessionCheck]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NamHoc namHoc = db.NamHocs.Find(id);
            if (namHoc == null)
            {
                return HttpNotFound();
            }
            return View(namHoc);
        }

        // GET: Admin/NamHocs/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NamHocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NamHoc_ID,NamHoc_Ten,NamHoc_TrangThai,NamHoc_GhiChu")] NamHoc namHoc)
        {
            if (ModelState.IsValid)
            {
                db.NamHocs.Add(namHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(namHoc);
        }

        // GET: Admin/NamHocs/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NamHoc namHoc = db.NamHocs.Find(id);
            if (namHoc == null)
            {
                return HttpNotFound();
            }
            return View(namHoc);
        }

        // POST: Admin/NamHocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NamHoc_ID,NamHoc_Ten,NamHoc_TrangThai,NamHoc_GhiChu")] NamHoc namHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(namHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(namHoc);
        }

        // GET: Admin/NamHocs/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NamHoc namHoc = db.NamHocs.Find(id);
            if (namHoc == null)
            {
                return HttpNotFound();
            }
            return View(namHoc);
        }

        // POST: Admin/NamHocs/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NamHoc namHoc = db.NamHocs.Find(id);
            db.NamHocs.Remove(namHoc);
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
