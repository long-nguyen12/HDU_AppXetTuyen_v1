using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HDU_AppXetTuyen.Models;
using OfficeOpenXml;
using PagedList; 

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class HuyensController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/Huyens
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var huyens = (from h in db.Huyens
                         select h).OrderBy(x => x.Huyen_ID).Include(h => h.Tinh);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(huyens.ToPagedList(pageNumber, pageSize));
        }

        //[HttpPost, ActionName("Index")]
        public ActionResult ShowDataExcelFile()
        {
            var huyenList = new List<Huyen>();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["fileQuanHuyen"];
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
                            var item = new Huyen();
                            item.Huyen_ID = int.Parse(workSheet.Cells[rowIterator, 1].Value.ToString());
                            item.Huyen_MaHuyen = workSheet.Cells[rowIterator, 2].Value.ToString();
                            item.Huyen_TenHuyen = workSheet.Cells[rowIterator, 3].Value.ToString();
                            item.Huyen_GhiChu = " ";
                            item.Tinh_ID = int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                            var checkItem = db.Huyens.Where(s => s.Huyen_MaHuyen == item.Huyen_MaHuyen).FirstOrDefault();
                            if (checkItem != null)
                            {
                                break;
                            }
                            db.Huyens.Add(item);
                            huyenList.Add(item);
                        }
                        db.SaveChanges();
                    }
                }
                else
                {
                    ViewBag.show = "Vui lòng chọn file";
                }
            }
            var model = huyenList;
            return View(model.ToList());
        }


        // GET: Admin/Huyens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Huyen huyen = db.Huyens.Find(id);
            if (huyen == null)
            {
                return HttpNotFound();
            }
            return View(huyen);
        }

        // GET: Admin/Huyens/Create
        public ActionResult Create()
        {
            ViewBag.Tinh_ID = new SelectList(db.Tinhs, "Tinh_ID", "Tinh_Ten");
            return View();
        }

        // POST: Admin/Huyens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Huyen_ID,Huyen_MaHuyen,Huyen_TenHuyen,Huyen_GhiChu,Tinh_ID")] Huyen huyen)
        {
            if (ModelState.IsValid)
            {
                db.Huyens.Add(huyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Tinh_ID = new SelectList(db.Tinhs, "Tinh_ID", "Tinh_Ten", huyen.Tinh_ID);
            return View(huyen);
        }

        // GET: Admin/Huyens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Huyen huyen = db.Huyens.Find(id);
            if (huyen == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tinh_ID = new SelectList(db.Tinhs, "Tinh_ID", "Tinh_Ten", huyen.Tinh_ID);
            return View(huyen);
        }

        // POST: Admin/Huyens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Huyen_ID,Huyen_MaHuyen,Huyen_TenHuyen,Huyen_GhiChu,Tinh_ID")] Huyen huyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(huyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Tinh_ID = new SelectList(db.Tinhs, "Tinh_ID", "Tinh_Ten", huyen.Tinh_ID);
            return View(huyen);
        }

        // GET: Admin/Huyens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Huyen huyen = db.Huyens.Find(id);
            if (huyen == null)
            {
                return HttpNotFound();
            }
            return View(huyen);
        }

        // POST: Admin/Huyens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Huyen huyen = db.Huyens.Find(id);
            db.Huyens.Remove(huyen);
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
