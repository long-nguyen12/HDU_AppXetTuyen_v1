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
        #region Colleger      
        [AdminSessionCheck]
        public ActionResult CollegerIndex()
        {
            var dotXetTuyens = db.DotXetTuyens.Include(d => d.NamHoc).Where(d => d.Dxt_Classify ==0);
            return View(dotXetTuyens.ToList());
        }
    
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
    
        [AdminSessionCheck]
        public ActionResult CollegerCreate()
        {
            ViewBag.NamHoc_ID = new SelectList(db.NamHocs, "NamHoc_ID", "NamHoc_Ten");
            return View();
        }

   
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CollegerCreate([Bind(Include = "Dxt_ID,Dxt_Ten,Dxt_TrangThai,Dxt_GhiChu,NamHoc_ID")] DotXetTuyen dotXetTuyen)
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
      
        [AdminSessionCheck]
        public ActionResult CollegerEdit(int? id)
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
      
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CollegerEdit([Bind(Include = "Dxt_ID,Dxt_Ten,Dxt_TrangThai,Dxt_GhiChu,NamHoc_ID")] DotXetTuyen dotXetTuyen)
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

       
        #endregion

        #region Master
        public ActionResult MasterIndex()
        {
            var model = db.DotXetTuyens.OrderByDescending(x => x.Dxt_ID).Where(x => x.Dxt_Classify == 2);

            return View(model.ToList());
        }
        #endregion
        #region Delete
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
        #endregion

        #region exam
        [AdminSessionCheck]
        public ActionResult ExamIndex()
        {
            var dotXetTuyens = db.DotXetTuyens.Include(d => d.NamHoc).Where(d => d.Dxt_Classify == 1);
            return View(dotXetTuyens.ToList());
        }
        #endregion
    }
}
