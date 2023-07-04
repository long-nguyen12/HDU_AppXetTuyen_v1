using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        public ActionResult Create(LienCapTieuHoc lienCapTieuHoc,
           IEnumerable<HttpPostedFileBase> HocSinh_MinhChungMN,
           IEnumerable<HttpPostedFileBase> HocSinh_MinhChungGiayKS,
           IEnumerable<HttpPostedFileBase> HocSinh_MinhChungMaDinhDanh,
           IEnumerable<HttpPostedFileBase> HocSinh_GiayUuTien,
           IEnumerable<HttpPostedFileBase> HocSinh_MinhChungLePhi)

        {
            string activationToken = Guid.NewGuid().ToString();
            try
            {
                TempData["Result"] = "";

                if (ModelState.IsValid)
                {
                    foreach (HttpPostedFileBase file in HocSinh_MinhChungMN)
                    {
                        //Kiểm tra tập tin có sẵn để lưu.  
                        if (file != null)
                        {
                            var InputFileName = Path.GetFileName(file.FileName);
                            InputFileName = lienCapTieuHoc.HocSinh_DinhDanh + "_" + DateTime.Now.ToFileTime() + "_" + InputFileName;
                            var urlFile = Path.Combine(Server.MapPath("~/Uploads/LienCapTieuHoc/MinhChungMN/") + InputFileName);
                            // Lưu file vào thư mục trên server  
                            file.SaveAs(urlFile);
                            string fileUrl = "Uploads/LienCapTieuHoc/MinhChungMN/" + InputFileName;
                            // lấy đường dẫn các file
                            lienCapTieuHoc.HocSinh_MinhChungMN += fileUrl + "#";
                        }
                    }
                    foreach (HttpPostedFileBase file in HocSinh_MinhChungGiayKS)
                    {
                        //Kiểm tra tập tin có sẵn để lưu.  
                        if (file != null)
                        {
                            var InputFileName = Path.GetFileName(file.FileName);
                            InputFileName = lienCapTieuHoc.HocSinh_DinhDanh + "_" + DateTime.Now.ToFileTime() + "_" + InputFileName;
                            var urlFile = Server.MapPath("~/Uploads/LienCapTieuHoc/GiayKhaiSinh/") + InputFileName;
                            // Lưu file vào thư mục trên server  
                            file.SaveAs(urlFile);
                            // lấy đường dẫn các file
                            string fileUrl = "Uploads/LienCapTieuHoc/GiayKhaiSinh/" + InputFileName;
                            lienCapTieuHoc.HocSinh_MinhChungGiayKS += fileUrl + "#";

                        }
                    }
                    foreach (HttpPostedFileBase file in HocSinh_MinhChungMaDinhDanh)
                    {
                        //Kiểm tra tập tin có sẵn để lưu.  
                        if (file != null)
                        {
                            var InputFileName = Path.GetFileName(file.FileName);
                            InputFileName = lienCapTieuHoc.HocSinh_DinhDanh + "_" + DateTime.Now.ToFileTime() + "_" + InputFileName;
                            var urlFile = Server.MapPath("~/Uploads/LienCapTieuHoc/MaDinhDanh/") + InputFileName;
                            // Lưu file vào thư mục trên server  
                            file.SaveAs(urlFile);
                            // lấy đường dẫn các file
                            file.SaveAs(urlFile);
                            string fileUrl = "Uploads/LienCapTieuHoc/MaDinhDanh/" + InputFileName;
                            lienCapTieuHoc.HocSinh_MinhChungMaDinhDanh += fileUrl+  "#";

                        }

                    }
                    foreach (HttpPostedFileBase file in HocSinh_GiayUuTien)
                    {
                        //Kiểm tra tập tin có sẵn để lưu.  
                        if (file != null)
                        {
                            var InputFileName = Path.GetFileName(file.FileName);                            
                            InputFileName = lienCapTieuHoc.HocSinh_DinhDanh + "_" + DateTime.Now.ToFileTime() + "_" + InputFileName;
                            var urlFile = Server.MapPath("~/Uploads/LienCapTieuHoc/GiayUuTien/") + InputFileName;
                            // Lưu file vào thư mục trên server  
                            file.SaveAs(urlFile);
                            // lấy đường dẫn các file
                            string fileUrl = "Uploads/LienCapTieuHoc/GiayUuTien/" + InputFileName;
                            lienCapTieuHoc.HocSinh_GiayUuTien += fileUrl + "#";
                        }
                    }
                    foreach (HttpPostedFileBase file in HocSinh_MinhChungLePhi)
                    {
                        //Kiểm tra tập tin có sẵn để lưu.  
                        if (file != null)
                        {
                            var InputFileName = Path.GetFileName(file.FileName);
                            InputFileName = lienCapTieuHoc.HocSinh_DinhDanh + "_" + DateTime.Now.ToFileTime() + "_" + InputFileName;
                            var urlFile = Server.MapPath("~/Uploads/LienCapTieuHoc/MinhChungLePhi/") + InputFileName;
                            // Lưu file vào thư mục trên server  
                            file.SaveAs(urlFile);
                            // lấy đường dẫn các file
                            string fileUrl = "Uploads/LienCapTieuHoc/MinhChungLePhi/" + InputFileName;
                            lienCapTieuHoc.HocSinh_MinhChungLePhi += fileUrl + "#";
                        }
                    }
                }
                lienCapTieuHoc.HocSinh_GhiChu = "";
                lienCapTieuHoc.HocSinh_MinhChungMN = lienCapTieuHoc.HocSinh_MinhChungMN != null ? lienCapTieuHoc.HocSinh_MinhChungMN.ToString().Replace("System.Web.HttpPostedFileWrapper", "") : "";
                lienCapTieuHoc.HocSinh_MinhChungGiayKS = lienCapTieuHoc.HocSinh_MinhChungGiayKS != null ? lienCapTieuHoc.HocSinh_MinhChungGiayKS.ToString().Replace("System.Web.HttpPostedFileWrapper", "") : "";
                lienCapTieuHoc.HocSinh_MinhChungMaDinhDanh = lienCapTieuHoc.HocSinh_MinhChungMaDinhDanh != null ? lienCapTieuHoc.HocSinh_MinhChungMaDinhDanh.ToString().Replace("System.Web.HttpPostedFileWrapper", "") : "";
                lienCapTieuHoc.HocSinh_GiayUuTien = lienCapTieuHoc.HocSinh_GiayUuTien != null ? lienCapTieuHoc.HocSinh_GiayUuTien.ToString().Replace("System.Web.HttpPostedFileWrapper", "") : "";
                lienCapTieuHoc.HocSinh_MinhChungLePhi = lienCapTieuHoc.HocSinh_MinhChungLePhi != null ? lienCapTieuHoc.HocSinh_MinhChungLePhi.ToString().Replace("System.Web.HttpPostedFileWrapper", "") : "";
                lienCapTieuHoc.HocSinh_Activation = activationToken;

                db.LienCapTieuHocs.Add(lienCapTieuHoc);
                db.SaveChanges();
                TempData["Result"] = "THANHCONG";
                ViewBag.status = "Thành công, Đăng ký thông tin thành công! Vui lòng xác thực lại thông tin đã gửi đến email";
                string activationUrl = Url.Action("ConfirmInfomation", "LienCapTieuHocs", new { token = activationToken }, Request.Url.Scheme);
                var subject = "Xác nhận thông tin đăng ký";
                var body = "Xin chào " + lienCapTieuHoc.HocSinh_HoTen + ", <br/> Bạn vừa đăng ký dự tuyển vào lớp 1 trường TH, THCS & THPT Hồng Đức. Vui lòng xác nhận lại các thông tin sau: " +

                     "<p> Họ và tên: " + lienCapTieuHoc.HocSinh_HoTen + "</p>" +
                     "<p> Ngày tháng năm sinh: " + lienCapTieuHoc.HocSinh_NgaySinh + "</p>" +
                     "<p> Nơi sinh: " + lienCapTieuHoc.HocSinh_NoiSinh + "</p>" +
                     "<p> Nơi cư trú: " + lienCapTieuHoc.HocSinh_NoiCuTru + "</p>" +
                     "<p> Họ và tên cha: " + lienCapTieuHoc.HocSinh_ThongTinCha + "</p>" +
                     "<p> Số điện thoại: " + lienCapTieuHoc.HocSinh_DienThoaiCha + "</p>" +
                     "<p> Họ và tên mẹ: " + lienCapTieuHoc.HocSinh_ThongTinMe + "</p>" +
                     "<p> Số điện thoại: " + lienCapTieuHoc.HocSinh_DienThoaiMe + "</p>" +
                     "<p>Nếu thông tin chính xác, vui lòng ấn vào đường link sau để xác nhận: " + activationUrl + " </p>";

                SendEmail(lienCapTieuHoc.HocSinh_Email, body, subject);
                return View();
            }
            catch (Exception e)
            {
                TempData["Result"] = "THAIBAI";
                System.Diagnostics.Debug.WriteLine(e);
            }
            return View();
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

        public ActionResult ConfirmInfomation()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ConfirmInfomation(string token)
        {
            var hocSinh = db.LienCapTieuHocs.Where(n => n.HocSinh_Activation == token).FirstOrDefault();
            if (hocSinh != null)
            {
                hocSinh.HocSinh_Activation = "";
                hocSinh.HocSinh_TrangThai = 1;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        private void SendEmail(string email, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("xettuyen@hdu.edu.vn", email))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                NetworkCredential NetworkCred = new NetworkCredential("xettuyen@hdu.edu.vn", "hongduc1");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
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
