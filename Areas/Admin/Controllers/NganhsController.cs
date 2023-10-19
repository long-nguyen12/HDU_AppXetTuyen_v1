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

        public JsonResult getAllToHopMon()
        {
            var tohops = db.ToHopMons.Select(n => new
            {
                Thm_ID = n.Thm_ID,
                Thm_MaToHop = n.Thm_MaToHop,
                Thm_TenToHop = n.Thm_TenToHop,
                Thm_Mon1 = n.Thm_Mon1,
                Thm_Mon2 = n.Thm_Mon2,
                Thm_Mon3 = n.Thm_Mon3,
                Thm_MaTen = n.Thm_MaTen,
                Thm_Thi_NK = n.Thm_Thi_NK,
            }).ToList();
            if (tohops != null && tohops.Count > 0)
            {
                return Json(new { success = true, tohops = tohops }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAllNganhs()
        {
            var nganhs = db.Nganhs.Select(n => new
            {
                Nganh_ID = n.Nganh_ID,
                Nganh_MaNganh = n.Nganh_MaNganh,
                NganhTenNganh = n.Nganh_TenNganh,
                Khoa_ID = n.Khoa_ID,
                Nganh_GhiChu = n.Nganh_GhiChu,
                KhoiNganh_ID = n.KhoiNganh_ID,
                Nganh_ThiNK = n.Nganh_ThiNK,
            }).ToList();
            if (nganhs != null && nganhs.Count > 0)
            {
                return Json(new { success = true, nganhs = nganhs }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
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
