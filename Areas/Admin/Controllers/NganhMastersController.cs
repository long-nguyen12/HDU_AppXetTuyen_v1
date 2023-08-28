using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HDU_AppXetTuyen.Models;
using PagedList;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class NganhMastersController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/NganhMasters
      
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var nganhs_master = (from n in db.NganhMasters
                          select n).OrderBy(x => x.Nganh_Mt_TenKhoa).ThenBy(x => x.Nganh_Mt_TenNganh).Where(x => x.Nganh_Mt_ID > 0);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(nganhs_master.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/NganhMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhMaster nganhMaster = db.NganhMasters.Find(id);
            if (nganhMaster == null)
            {
                return HttpNotFound();
            }
            return View(nganhMaster);
        }

        // GET: Admin/NganhMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NganhMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nganh_Mt_ID,Nganh_Mt_MaNganh,Nganh_Mt_TenNganh,Nganh_Mt_NghienCuu_Ten,Nganh_Mt_NghienCuu_Ma,Khoa_ID,Nganh_Mt_TenKhoa,Nganh_Mt_TrangThai,Nganh_Mt_GhiChu")] NganhMaster nganhMaster)
        {
            if (ModelState.IsValid)
            {
                db.NganhMasters.Add(nganhMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nganhMaster);
        }

        // GET: Admin/NganhMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhMaster nganhMaster = db.NganhMasters.Find(id);
            if (nganhMaster == null)
            {
                return HttpNotFound();
            }
            return View(nganhMaster);
        }

        // POST: Admin/NganhMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nganh_Mt_ID,Nganh_Mt_MaNganh,Nganh_Mt_TenNganh,Nganh_Mt_NghienCuu_Ten,Nganh_Mt_NghienCuu_Ma,Khoa_ID,Nganh_Mt_TenKhoa,Nganh_Mt_TrangThai,Nganh_Mt_GhiChu")] NganhMaster nganhMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nganhMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nganhMaster);
        }

        // GET: Admin/NganhMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhMaster nganhMaster = db.NganhMasters.Find(id);
            if (nganhMaster == null)
            {
                return HttpNotFound();
            }
            return View(nganhMaster);
        }

        // POST: Admin/NganhMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NganhMaster nganhMaster = db.NganhMasters.Find(id);
            db.NganhMasters.Remove(nganhMaster);
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
