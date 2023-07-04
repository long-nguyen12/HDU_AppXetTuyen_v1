using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HDU_AppXetTuyen.Models;

namespace HDU_AppXetTuyen.Controllers
{
    public class LienCapTieuHocsController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: LienCapTieuHocs
        public ActionResult Index()
        {
            return View(db.LienCapTieuHocs.ToList());
        }

        // GET: LienCapTieuHocs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            if (lienCapTieuHoc == null)
            {
                return HttpNotFound();
            }
            return View(lienCapTieuHoc);
        }

        // GET: LienCapTieuHocs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LienCapTieuHocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongMN,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_NgheNghiepCha,HocSinh_DienThoaiCha,HocSinh_ThongTinMe,HocSinh_NgheNghiepMe,HocSinh_DienThoaiMe,HocSinh_MinhChungMN,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_TrangThai,HocSinh_GhiChu")] LienCapTieuHoc lienCapTieuHoc,
           IEnumerable<HttpPostedFileBase> HocSinh_MinhChungMN, 
           IEnumerable<HttpPostedFileBase> HocSinh_MinhChungGiayKS,
           IEnumerable<HttpPostedFileBase>HocSinh_MinhChungMaDinhDanh,
           IEnumerable<HttpPostedFileBase>HocSinh_GiayUuTien,
           IEnumerable<HttpPostedFileBase> HocSinh_MinhChungLePhi)

        {
            if (ModelState.IsValid)
            {
                foreach (var file in HocSinh_MinhChungMN)
                {
                    //Kiểm tra tập tin có sẵn để lưu.  
                    if (file != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Da vao day files");
                        var InputFileName = Path.GetFileName(file.FileName);
                        System.Diagnostics.Debug.WriteLine("InputFileName");
                        InputFileName = lienCapTieuHoc.HocSinh_DinhDanh + "_" + DateTime.Now.ToFileTime() + "_" + InputFileName;
                        var urlFile = Server.MapPath("~/UploadLienCapTieuHoc/MinhChungMN/") + InputFileName;
                        // Lưu file vào thư mục trên server  
                        file.SaveAs(urlFile);
                        // lấy đường dẫn các file
                        lienCapTieuHoc.HocSinh_MinhChungMN += "UploadLienCapTieuHoc/MinhChungMN/" + InputFileName + "#";
                        // Hiển thị thông báo tổng số tệp đã lưu trên server.  
                        //ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully";
                        TempData["Result"] = "THANHCONG";
                    }
                    else
                    {
                        TempData["Result"] = "THATBAI";
                    }

                }
                foreach (var file in HocSinh_MinhChungGiayKS)
                {
                    //Kiểm tra tập tin có sẵn để lưu.  
                    if (file != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Da vao day files");
                        var InputFileName = Path.GetFileName(file.FileName);
                        System.Diagnostics.Debug.WriteLine("InputFileName");
                        InputFileName = lienCapTieuHoc.HocSinh_DinhDanh + "_" + DateTime.Now.ToFileTime() + "_" + InputFileName;
                        var urlFile = Server.MapPath("~/UploadLienCapTieuHoc/GiayKhaiSinh/") + InputFileName;
                        // Lưu file vào thư mục trên server  
                        file.SaveAs(urlFile);
                        // lấy đường dẫn các file
                        lienCapTieuHoc.HocSinh_MinhChungGiayKS += "UploadLienCapTieuHoc/GiayKhaiSinh/" + InputFileName + "#";
                        // Hiển thị thông báo tổng số tệp đã lưu trên server.  
                        //ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully";
                        TempData["Result"] = "THANHCONG";
                    }
                    else
                    {
                        TempData["Result"] = "THATBAI";
                    }

                }
                foreach (var file in HocSinh_MinhChungMaDinhDanh)
                {
                    //Kiểm tra tập tin có sẵn để lưu.  
                    if (file != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Da vao day files");
                        var InputFileName = Path.GetFileName(file.FileName);
                        System.Diagnostics.Debug.WriteLine("InputFileName");
                        InputFileName = lienCapTieuHoc.HocSinh_DinhDanh + "_" + DateTime.Now.ToFileTime() + "_" + InputFileName;
                        var urlFile = Server.MapPath("~/UploadLienCapTieuHoc/MaDinhDanh/") + InputFileName;
                        // Lưu file vào thư mục trên server  
                        file.SaveAs(urlFile);
                        // lấy đường dẫn các file
                        lienCapTieuHoc.HocSinh_MinhChungMaDinhDanh += "UploadLienCapTieuHoc/MaDinhDanh/" + InputFileName + "#";
                        // Hiển thị thông báo tổng số tệp đã lưu trên server.  
                        //ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully";
                        TempData["Result"] = "THANHCONG";
                    }
                    else
                    {
                        TempData["Result"] = "THATBAI";
                    }

                }
                foreach (var file in HocSinh_GiayUuTien)
                {
                    //Kiểm tra tập tin có sẵn để lưu.  
                    if (file != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Da vao day files");
                        var InputFileName = Path.GetFileName(file.FileName);
                        System.Diagnostics.Debug.WriteLine("InputFileName");
                        InputFileName = lienCapTieuHoc.HocSinh_DinhDanh + "_" + DateTime.Now.ToFileTime() + "_" + InputFileName;
                        var urlFile = Server.MapPath("~/UploadLienCapTieuHoc/GiayUuTien/") + InputFileName;
                        // Lưu file vào thư mục trên server  
                        file.SaveAs(urlFile);
                        // lấy đường dẫn các file
                        lienCapTieuHoc.HocSinh_GiayUuTien += "UploadLienCapTieuHoc/GiayUuTien/" + InputFileName + "#";
                        // Hiển thị thông báo tổng số tệp đã lưu trên server.  
                        //ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully";
                        TempData["Result"] = "THANHCONG";
                    }
                    else
                    {
                        TempData["Result"] = "THATBAI";
                    }

                }
                foreach (var file in HocSinh_MinhChungLePhi)
                {
                    //Kiểm tra tập tin có sẵn để lưu.  
                    if (file != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Da vao day files");
                        var InputFileName = Path.GetFileName(file.FileName);
                        System.Diagnostics.Debug.WriteLine("InputFileName");
                        InputFileName = lienCapTieuHoc.HocSinh_DinhDanh + "_" + DateTime.Now.ToFileTime() + "_" + InputFileName;
                        var urlFile = Server.MapPath("~/UploadLienCapTieuHoc/MinhChungLePhi/") + InputFileName;
                        // Lưu file vào thư mục trên server  
                        file.SaveAs(urlFile);
                        // lấy đường dẫn các file
                        lienCapTieuHoc.HocSinh_MinhChungLePhi += "UploadLienCapTieuHoc/MinhChungLePhi/" + InputFileName + "#";
                        // Hiển thị thông báo tổng số tệp đã lưu trên server.  
                        //ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully";
                        TempData["Result"] = "THANHCONG";
                    }
                    else
                    {
                        TempData["Result"] = "THATBAI";
                    }

                }
                lienCapTieuHoc.HocSinh_GhiChu = "";

                db.LienCapTieuHocs.Add(lienCapTieuHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lienCapTieuHoc);
        }

        // GET: LienCapTieuHocs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            if (lienCapTieuHoc == null)
            {
                return HttpNotFound();
            }
            return View(lienCapTieuHoc);
        }

        // POST: LienCapTieuHocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongMN,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_NgheNghiepCha,HocSinh_DienThoaiCha,HocSinh_ThongTinMe,HocSinh_NgheNghiepMe,HocSinh_DienThoaiMe,HocSinh_MinhChungMN,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_TrangThai,HocSinh_GhiChu")] LienCapTieuHoc lienCapTieuHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lienCapTieuHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lienCapTieuHoc);
        }

        // GET: LienCapTieuHocs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            if (lienCapTieuHoc == null)
            {
                return HttpNotFound();
            }
            return View(lienCapTieuHoc);
        }

        // POST: LienCapTieuHocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            db.LienCapTieuHocs.Remove(lienCapTieuHoc);
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
