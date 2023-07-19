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
    public class ToHopMonsController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/ToHopMons
        [AdminSessionCheck]
        public ActionResult Index(int? page)
        {
            var thms = (from h in db.ToHopMons
                        select h).Where(x => x.Thm_ID > 0).OrderBy(x => x.Thm_MaToHop);
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            return View(thms.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/ToHopMons/Details/5
        [AdminSessionCheck]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToHopMon toHopMon = db.ToHopMons.Find(id);
            if (toHopMon == null)
            {
                return HttpNotFound();
            }
            return View(toHopMon);
        }

        // GET: Admin/ToHopMons/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ToHopMons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Thm_ID,Thm_MaToHop,Thm_TenToHop,Thm_Mon1,Thm_Mon2,Thm_Mon3,Thm_MaTen")] ToHopMon toHopMon)
        {
            if (ModelState.IsValid)
            {
                db.ToHopMons.Add(toHopMon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toHopMon);
        }

        // GET: Admin/ToHopMons/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToHopMon toHopMon = db.ToHopMons.Find(id);
            if (toHopMon == null)
            {
                return HttpNotFound();
            }
            return View(toHopMon);
        }

        // POST: Admin/ToHopMons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Thm_ID,Thm_MaToHop,Thm_TenToHop,Thm_Mon1,Thm_Mon2,Thm_Mon3,Thm_MaTen")] ToHopMon toHopMon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toHopMon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toHopMon);
        }

        // GET: Admin/ToHopMons/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToHopMon toHopMon = db.ToHopMons.Find(id);
            if (toHopMon == null)
            {
                return HttpNotFound();
            }
            return View(toHopMon);
        }

        // POST: Admin/ToHopMons/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToHopMon toHopMon = db.ToHopMons.Find(id);
            db.ToHopMons.Remove(toHopMon);
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
                return Json(new { success = true, tohops = tohops });
            }
            return Json(new { success = false });
        }
        public JsonResult getAllNganhs()
        {
            var nganhs = db.Nganhs.Select(n => new
            {
                Nganh_ID = n.Nganh_ID,
                Nganh_MaNganh = n.Nganh_MaNganh,
                NganhTenNganh = n.NganhTenNganh,
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
