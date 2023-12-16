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
            var dotXetTuyens = db.DotXetTuyens.Include(d => d.NamHoc).Where(d => d.Dxt_Classify == 0);
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

        //[AdminSessionCheck]
        public ActionResult MasterCreate()
        {
            ViewBag.NamHoc_ID = new SelectList(db.NamHocs, "NamHoc_ID", "NamHoc_Ten");
            return View();
        }
        public ActionResult MasterEdit(int? Dxt_ID)
        {
            ViewBag.Dxt_ID = Dxt_ID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Dxt_ID,Dxt_Classify,Dxt_Ten,Dxt_TrangThai_Xt,Dxt_TrangThai_TNK,Dxt_ThoiGian_BatDau,Dxt_ThoiGian_KetThuc,Dxt_GhiChu,NamHoc_ID")] DotXetTuyen dotXetTuyen)
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
        public ActionResult NangKhieu()
        {
            var model = db.DotXetTuyens.OrderByDescending(x => x.Dxt_ID).Where(x => x.Dxt_Classify == 1);

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
        #region Json function
        public JsonResult DotXetTuyetGetNamHocHienTaiJson()
        {
            var NamHoc_ID = db.NamHocs.Where(x => x.NamHoc_TrangThai == 1).FirstOrDefault().NamHoc_Ten;
            return Json(new { success = true, data = NamHoc_ID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DotXetTuyetGetByIDJson(DotXetTuyen entity)
        {
            var model = db.DotXetTuyens.Where(x => x.Dxt_ID == entity.Dxt_ID).FirstOrDefault();
            var data = new
            {
                Dxt_Classify = model.Dxt_Classify,
                Dxt_TrangThai_Xt = model.Dxt_TrangThai_Xt,
                Dxt_Ten = model.Dxt_Ten,
                Dxt_ThoiGian_BatDau = model.Dxt_ThoiGian_BatDau,
                Dxt_ThoiGian_KetThuc = model.Dxt_ThoiGian_KetThuc,
                Dxt_GhiChu = model.Dxt_GhiChu,
                NamHoc_ID = db.NamHocs.Where(x => x.NamHoc_ID == model.NamHoc_ID).FirstOrDefault().NamHoc_Ten,

            };
            return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DotXetTuyetAddJson(DotXetTuyen entity)
        {
            var NamHoc_ID = db.NamHocs.Where(x => x.NamHoc_TrangThai == 1).FirstOrDefault().NamHoc_ID;
            DotXetTuyen model = new DotXetTuyen();
            model.Dxt_Classify = entity.Dxt_Classify;
            model.Dxt_Ten = entity.Dxt_Ten;
            model.Dxt_ThoiGian_BatDau = entity.Dxt_ThoiGian_BatDau;
            model.Dxt_ThoiGian_KetThuc = entity.Dxt_ThoiGian_KetThuc;
            model.Dxt_GhiChu = entity.Dxt_GhiChu;
            model.NamHoc_ID = NamHoc_ID;
            model.Dxt_TrangThai_Xt = 0;
            model.Dxt_TrangThai_TNK = 0;
            db.DotXetTuyens.Add(model);
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DotXetTuyetEditJson(DotXetTuyen entity)
        {
            var model = db.DotXetTuyens.Where(x => x.Dxt_ID == entity.Dxt_ID).FirstOrDefault();
            model.Dxt_Ten = entity.Dxt_Ten;
            model.Dxt_ThoiGian_BatDau = entity.Dxt_ThoiGian_BatDau;
            model.Dxt_ThoiGian_KetThuc = entity.Dxt_ThoiGian_KetThuc;
            model.Dxt_GhiChu = entity.Dxt_GhiChu;
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DotXetTuyetChangeTTXTJson(DotXetTuyen entity)
        {
            var model = db.DotXetTuyens.Where(t => t.Dxt_Classify == entity.Dxt_Classify).ToList();
            foreach (var item in model)
            {
                if (item.Dxt_ID == entity.Dxt_ID)
                {
                    item.Dxt_TrangThai_Xt = 1;
                }
                else
                {
                    item.Dxt_TrangThai_Xt = 0;
                }
            }
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DotXetTuyetDelete(DotXetTuyen entity)
        {
            try
            {
                var model = db.DotXetTuyens.Where(t => t.Dxt_ID == entity.Dxt_ID).FirstOrDefault();
                db.DotXetTuyens.Remove(model);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
