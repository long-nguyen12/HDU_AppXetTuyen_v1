using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using HDU_AppXetTuyen.Models;
using PagedList;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class NganhsController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/Nganhs
        [AdminSessionCheck]
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var nganhs = (from h in db.Nganhs
                          select h).OrderBy(x => x.Khoa.Khoa_TenKhoa).ThenBy(x => x.Nganh_TenNganh). Include(n => n.KhoiNganh).Include(n => n.Khoa).Where(x => x.Nganh_ID >0);
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            return View(nganhs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Nganhs/Details/5
        [AdminSessionCheck]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganhs.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            return View(nganh);
        }

        // GET: Admin/Nganhs/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten");
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa");
            return View();
        }

        // POST: Admin/Nganhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nganh_ID,Nganh_MaNganh,Nganh_TenNganh,Khoa_ID,Nganh_GhiChu,KhoiNganh_ID")] Nganh nganh)
        {
            if (ModelState.IsValid)
            {
                nganh.Nganh_GhiChu = nganh.Nganh_MaNganh + " " + nganh.Nganh_TenNganh;
                db.Nganhs.Add(nganh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten", nganh.KhoiNganh_ID);
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa", nganh.Khoa_ID);
            return View(nganh);
        }

        // GET: Admin/Nganhs/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganhs.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten", nganh.KhoiNganh_ID);
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa", nganh.Khoa_ID);
        
            return View(nganh);
        }

        // POST: Admin/Nganhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nganh_ID,Nganh_MaNganh,Nganh_TenNganh,Khoa_ID,Nganh_GhiChu,KhoiNganh_ID")] Nganh nganh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nganh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten", nganh.KhoiNganh_ID);
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa", nganh.Khoa_ID);
            return View(nganh);
        }

        // GET: Admin/Nganhs/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganhs.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            return View(nganh);
        }

        // POST: Admin/Nganhs/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Nganh nganh = db.Nganhs.Find(id);
            db.Nganhs.Remove(nganh);
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
