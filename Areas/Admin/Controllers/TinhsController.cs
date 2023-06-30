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
    public class TinhsController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/Tinhs
        [AdminSessionCheck]
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var tinhs = (from h in db.Tinhs
                          select h).OrderBy(x => x.Tinh_ID);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(tinhs.ToPagedList(pageNumber, pageSize));
        }

        //[HttpPost, ActionName("Index")]
        [AdminSessionCheck]
        public ActionResult ShowDataExcelFile()
        {
            var tinhList = new List<Tinh>();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["fileTinhThanh"];
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
                            var item = new Tinh();
                            item.Tinh_ID = int.Parse(workSheet.Cells[rowIterator, 1].Value.ToString());
                            item.Tinh_Ma = workSheet.Cells[rowIterator, 2].Value.ToString();
                            item.Tinh_Ten = workSheet.Cells[rowIterator, 3].Value.ToString();
                            item.Tinh_MaTen = workSheet.Cells[rowIterator, 2].Value.ToString() + " - " + workSheet.Cells[rowIterator, 3].Value.ToString();
                            item.Tinh_GhiChu = " ";
                            var checkItem = db.Tinhs.Where(s => s.Tinh_Ma == item.Tinh_Ma).FirstOrDefault();
                            if (checkItem != null)
                            {
                                break;
                            }
                            db.Tinhs.Add(item);
                            tinhList.Add(item);
                        }
                        db.SaveChanges();
                    }
                }
                else
                {
                    ViewBag.show = "Vui lòng chọn file";
                }
            }
            var model = tinhList;
            return View(model.ToList());
        }

        // GET: Admin/Tinhs/Details/5
        [AdminSessionCheck]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tinh tinh = db.Tinhs.Find(id);
            if (tinh == null)
            {
                return HttpNotFound();
            }
            return View(tinh);
        }

        // GET: Admin/Tinhs/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tinhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tinh_ID,Tinh_Ma,Tinh_Ten,Tinh_MaTen,Tinh_GhiChu")] Tinh tinh)
        {
            if (ModelState.IsValid)
            {
                db.Tinhs.Add(tinh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tinh);
        }

        // GET: Admin/Tinhs/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tinh tinh = db.Tinhs.Find(id);
            if (tinh == null)
            {
                return HttpNotFound();
            }
            return View(tinh);
        }

        // POST: Admin/Tinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tinh_ID,Tinh_Ma,Tinh_Ten,Tinh_MaTen,Tinh_GhiChu")] Tinh tinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tinh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tinh);
        }

        // GET: Admin/Tinhs/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tinh tinh = db.Tinhs.Find(id);
            if (tinh == null)
            {
                return HttpNotFound();
            }
            return View(tinh);
        }

        // POST: Admin/Tinhs/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tinh tinh = db.Tinhs.Find(id);
            db.Tinhs.Remove(tinh);
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
