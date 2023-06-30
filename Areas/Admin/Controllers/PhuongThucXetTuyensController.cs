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
    public class PhuongThucXetTuyensController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/PhuongThucXetTuyens
        [AdminSessionCheck]
        public ActionResult Index()
        {
            return View(db.PhuongThucXetTuyens.ToList());
        }

        // GET: Admin/PhuongThucXetTuyens/Details/5
        [AdminSessionCheck]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhuongThucXetTuyen phuongThucXetTuyen = db.PhuongThucXetTuyens.Find(id);
            if (phuongThucXetTuyen == null)
            {
                return HttpNotFound();
            }
            return View(phuongThucXetTuyen);
        }

        // GET: Admin/PhuongThucXetTuyens/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/PhuongThucXetTuyens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ptxt_ID,Ptxt_TenPhuongThuc,Ptxt_GhiChu")] PhuongThucXetTuyen phuongThucXetTuyen)
        {
            if (ModelState.IsValid)
            {
                db.PhuongThucXetTuyens.Add(phuongThucXetTuyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phuongThucXetTuyen);
        }

        // GET: Admin/PhuongThucXetTuyens/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhuongThucXetTuyen phuongThucXetTuyen = db.PhuongThucXetTuyens.Find(id);
            if (phuongThucXetTuyen == null)
            {
                return HttpNotFound();
            }
            return View(phuongThucXetTuyen);
        }

        // POST: Admin/PhuongThucXetTuyens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ptxt_ID,Ptxt_TenPhuongThuc,Ptxt_GhiChu")] PhuongThucXetTuyen phuongThucXetTuyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phuongThucXetTuyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phuongThucXetTuyen);
        }

        // GET: Admin/PhuongThucXetTuyens/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhuongThucXetTuyen phuongThucXetTuyen = db.PhuongThucXetTuyens.Find(id);
            if (phuongThucXetTuyen == null)
            {
                return HttpNotFound();
            }
            return View(phuongThucXetTuyen);
        }

        // POST: Admin/PhuongThucXetTuyens/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhuongThucXetTuyen phuongThucXetTuyen = db.PhuongThucXetTuyens.Find(id);
            db.PhuongThucXetTuyens.Remove(phuongThucXetTuyen);
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
