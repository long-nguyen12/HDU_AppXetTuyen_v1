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
using OfficeOpenXml;
using PagedList;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class XasController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/Xas
        [AdminSessionCheck]
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var xas = (from h in db.Xas
                          select h).OrderBy(x => x.Xa_ID).Include(h => h.Huyen);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(xas.ToPagedList(pageNumber, pageSize));
        }

        //[HttpPost, ActionName("Index")]
        [AdminSessionCheck]
        public ActionResult ShowDataExcelFile()
        {
            var xaList = new List<Xa>();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["filePhuongXa"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var item = new Xa();
                            item.Xa_ID = int.Parse(workSheet.Cells[rowIterator, 1].Value.ToString());
                            item.Xa_Ma = workSheet.Cells[rowIterator, 2].Value.ToString();
                            item.Xa_Ten = workSheet.Cells[rowIterator, 3].Value.ToString();
                            item.Xa_GhiChu = " ";
                            item.Huyen_ID = int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                            var checkItem = db.Xas.Where(s => s.Xa_Ma == item.Xa_Ma).FirstOrDefault();
                            if (checkItem != null)
                            {
                                break;
                            }
                            db.Xas.Add(item);
                            xaList.Add(item);
                        }
                        db.SaveChanges();
                    }
                }
                else
                {
                    ViewBag.show = "Vui lòng chọn file";
                }
            }
            var model = xaList;
            return View(model.ToList());
        }

        // GET: Admin/Xas/Details/5
        [AdminSessionCheck]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Xa xa = db.Xas.Find(id);
            if (xa == null)
            {
                return HttpNotFound();
            }
            return View(xa);
        }

        // GET: Admin/Xas/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            ViewBag.Huyen_ID = new SelectList(db.Huyens, "Huyen_ID", "Huyen_TenHuyen");
            return View();
        }

        // POST: Admin/Xas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Xa_ID,Xa_Ma,Xa_Ten,Xa_GhiChu,Huyen_ID")] Xa xa)
        {
            if (ModelState.IsValid)
            {
                db.Xas.Add(xa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Huyen_ID = new SelectList(db.Huyens, "Huyen_ID", "Huyen_TenHuyen", xa.Huyen_ID);
            return View(xa);
        }

        // GET: Admin/Xas/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Xa xa = db.Xas.Find(id);
            if (xa == null)
            {
                return HttpNotFound();
            }
            ViewBag.Huyen_ID = new SelectList(db.Huyens, "Huyen_ID", "Huyen_TenHuyen", xa.Huyen_ID);
            return View(xa);
        }

        // POST: Admin/Xas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Xa_ID,Xa_Ma,Xa_Ten,Xa_GhiChu,Huyen_ID")] Xa xa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(xa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Huyen_ID = new SelectList(db.Huyens, "Huyen_ID", "Huyen_TenHuyen", xa.Huyen_ID);
            return View(xa);
        }

        // GET: Admin/Xas/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Xa xa = db.Xas.Find(id);
            if (xa == null)
            {
                return HttpNotFound();
            }
            return View(xa);
        }

        // POST: Admin/Xas/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Xa xa = db.Xas.Find(id);
            db.Xas.Remove(xa);
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
