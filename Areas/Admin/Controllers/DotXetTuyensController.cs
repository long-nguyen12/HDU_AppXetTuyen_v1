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
    public class DotXetTuyensController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/DotXetTuyens
        [AdminSessionCheck]
        public ActionResult Index()
        {
            var dotXetTuyens = db.DotXetTuyens.Include(d => d.NamHoc).Where(d => d.Dxt_Classify ==0 ||d.Dxt_Classify == 1);
            return View(dotXetTuyens.ToList());
        }

        // GET: Admin/DotXetTuyens/Details/5
        [AdminSessionCheck]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DotXetTuyen dotXetTuyen = db.DotXetTuyens.Find(id);
            if (dotXetTuyen == null)
            {
                return HttpNotFound();
            }
            return View(dotXetTuyen);
        }

        // GET: Admin/DotXetTuyens/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            ViewBag.NamHoc_ID = new SelectList(db.NamHocs, "NamHoc_ID", "NamHoc_Ten");
            return View();
        }

        // POST: Admin/DotXetTuyens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Dxt_ID,Dxt_Ten,Dxt_TrangThai,Dxt_GhiChu,NamHoc_ID")] DotXetTuyen dotXetTuyen)
        {
            if (ModelState.IsValid)
            {
                db.DotXetTuyens.Add(dotXetTuyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NamHoc_ID = new SelectList(db.NamHocs, "NamHoc_ID", "NamHoc_Ten", dotXetTuyen.NamHoc_ID);
            return View(dotXetTuyen);
        }

        // GET: Admin/DotXetTuyens/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DotXetTuyen dotXetTuyen = db.DotXetTuyens.Find(id);
            if (dotXetTuyen == null)
            {
                return HttpNotFound();
            }
            ViewBag.NamHoc_ID = new SelectList(db.NamHocs, "NamHoc_ID", "NamHoc_Ten", dotXetTuyen.NamHoc_ID);
            return View(dotXetTuyen);
        }

        // POST: Admin/DotXetTuyens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Dxt_ID,Dxt_Ten,Dxt_TrangThai,Dxt_GhiChu,NamHoc_ID")] DotXetTuyen dotXetTuyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dotXetTuyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NamHoc_ID = new SelectList(db.NamHocs, "NamHoc_ID", "NamHoc_Ten", dotXetTuyen.NamHoc_ID);
            return View(dotXetTuyen);
        }

        // GET: Admin/DotXetTuyens/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DotXetTuyen dotXetTuyen = db.DotXetTuyens.Find(id);
            if (dotXetTuyen == null)
            {
                return HttpNotFound();
            }
            return View(dotXetTuyen);
        }

        // POST: Admin/DotXetTuyens/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DotXetTuyen dotXetTuyen = db.DotXetTuyens.Find(id);
            db.DotXetTuyens.Remove(dotXetTuyen);
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
