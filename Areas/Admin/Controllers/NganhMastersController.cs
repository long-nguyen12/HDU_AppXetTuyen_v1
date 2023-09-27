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

        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var nganhs_master = (from n in db.NganhMasters
                          select n).OrderBy(x => x.Nganh_Mt_TenKhoa).ThenBy(x => x.Nganh_Mt_TenNganh).Where(x => x.Nganh_Mt_ID > 0);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(nganhs_master.ToPagedList(pageNumber, pageSize));
        }
      
        public ActionResult Create()
        {          
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa");
            return View();
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nganh_Mt_ID,Nganh_Mt_MaNganh,Nganh_Mt_TenNganh,Nganh_Mt_NghienCuu_Ten,Nganh_Mt_NghienCuu_Ma,Khoa_ID,Nganh_Mt_TenKhoa,Nganh_Mt_TrangThai,Nganh_Mt_GhiChu")] NganhMaster nganhMaster)
        {
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa");      

            if (ModelState.IsValid)
            {
                nganhMaster.Nganh_Mt_TenKhoa = db.Khoas.Where(x => x.Khoa_ID == nganhMaster.Khoa_ID).FirstOrDefault().Khoa_TenKhoa;

                if (nganhMaster.Nganh_Mt_NghienCuu_Ma == 1)
                {
                    nganhMaster.Nganh_Mt_NghienCuu_Ten = "Ứng dụng";
                }

                if (nganhMaster.Nganh_Mt_NghienCuu_Ma == 2)
                {
                    nganhMaster.Nganh_Mt_NghienCuu_Ten = "Nghiên cứu";
                }

                if (nganhMaster.Nganh_Mt_NghienCuu_Ma == 3)
                {
                    nganhMaster.Nganh_Mt_NghienCuu_Ten = "Ứng dụng, Nghiên cứu";
                }

                db.NganhMasters.Add(nganhMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            //if (ModelState.IsValid)
            //{

            //    db.NganhMasters.Add(nganhMaster);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            return View(nganhMaster);
        }

     
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
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa", nganhMaster.Khoa_ID);
            return View(nganhMaster);
        }

      
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
