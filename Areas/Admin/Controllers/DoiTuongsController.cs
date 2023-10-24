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
    public class DoiTuongsController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/DoiTuongs
        [AdminSessionCheck]
        public ActionResult Index()
        {
            return View(db.DoiTuongs.Where(x => x.DoiTuong_ID >0).ToList());
        }

        // GET: Admin/DoiTuongs/Details/5
        [AdminSessionCheck]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoiTuong doiTuong = db.DoiTuongs.Find(id);
            if (doiTuong == null)
            {
                return HttpNotFound();
            }
            return View(doiTuong);
        }

        // GET: Admin/DoiTuongs/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DoiTuongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DoiTuong_ID,DoiTuong_Ten,DoiTuong_DiemUuTien,DoiTuong_GhiChu")] DoiTuong doiTuong)
        {
            if (ModelState.IsValid)
            {
                db.DoiTuongs.Add(doiTuong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(doiTuong);
        }

        // GET: Admin/DoiTuongs/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoiTuong doiTuong = db.DoiTuongs.Find(id);
            if (doiTuong == null)
            {
                return HttpNotFound();
            }
            return View(doiTuong);
        }

        // POST: Admin/DoiTuongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DoiTuong_ID,DoiTuong_Ten,DoiTuong_DiemUuTien,DoiTuong_GhiChu")] DoiTuong doiTuong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doiTuong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doiTuong);
        }

        // GET: Admin/DoiTuongs/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoiTuong doiTuong = db.DoiTuongs.Find(id);
            if (doiTuong == null)
            {
                return HttpNotFound();
            }
            return View(doiTuong);
        }

        // POST: Admin/DoiTuongs/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoiTuong doiTuong = db.DoiTuongs.Find(id);
            db.DoiTuongs.Remove(doiTuong);
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
