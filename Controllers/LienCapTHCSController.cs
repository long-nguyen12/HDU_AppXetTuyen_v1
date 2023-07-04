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
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;

namespace HDU_AppXetTuyen.Controllers
{
    public class StudentInfo
    {
        public string HocSinh_DinhDanh { get; set; }
        public string HocSinh_HoTen { get; set; }
        public string HocSinh_GioiTinh { get; set; }
        public string HocSinh_NgaySinh { get; set; }
        public string HocSinh_NoiSinh { get; set; }
        public string HocSinh_Email { get; set; }
        public string HocSinh_NoiCuTru { get; set; }
        public string HocSinh_TruongTH { get; set; }
        public string HocSinh_UuTien { get; set; }
        public string HocSinh_ThongTinCha { get; set; }
        public string HocSinh_ThongTinMe { get; set; }
        public string HocSinh_DiemHocTap { get; set; }
        public string HocSinh_MucDoNangLuc { get; set; }
        public string HocSinh_MucDoPhamChat { get; set; }
        public string HocSinh_MinhChungHB { get; set; }
        public string HocSinh_MinhChungGiayKS { get; set; }
        public string HocSinh_MinhChungMaDinhDanh { get; set; }
        public string HocSinh_GiayUuTien { get; set; }
        public string HocSinh_XacNhanLePhi { get; set; }
        public string HocSinh_HoTenCha { get; set; }
        public string HocSinh_SDTCha { get; set; }
        public string HocSinh_NNCha { get; set; }
        public string HocSinh_HoTenMe { get; set; }
        public string HocSinh_SDTMe { get; set; }
        public string HocSinh_NNMe { get; set; }
        public string HocSinh_DiemToan { get; set; }
        public string HocSinh_DiemTiengViet { get; set; }
        public string HocSinh_DiemTN { get; set; }
        public string HocSinh_DiemLSDL { get; set; }
        public string HocSinh_DiemTA { get; set; }
    }
    public class LienCapTHCSController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: LienCapTHCS/Create

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadFiles()
        {
            var formName = Request.Form["formName"];
            var HocSinh_DinhDanh = Request.Form["HocSinh_DinhDanh"];

            string savePath = "";
            if (formName.Equals("HOCBA")) savePath = "Uploads/LienCap/HocBa";
            else if (formName.Equals("GIAYKS")) savePath = "Uploads/LienCap/GiayKhaiSinh";
            else if (formName.Equals("LEPHI")) savePath = "Uploads/LienCap/LePhi";
            else if (formName.Equals("DINHDANH")) savePath = "Uploads/LienCap/DinhDanh";
            else if (formName.Equals("UUTIEN")) savePath = "Uploads/LienCap/GiayUuTien";
            string fileNameStored = "";
            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = HocSinh_DinhDanh +"_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + fileName;
                    var filePath = Path.Combine(Server.MapPath("~/" + savePath), fileName);
                    var savedPath = savePath + "/" + fileName;
                    if (i != Request.Files.Count - 1)
                    {
                        fileNameStored = fileNameStored + savedPath + "#";
                    }
                    else
                    {
                        fileNameStored += savedPath;
                    }
                    file.SaveAs(filePath);
                }
            
                return Json(new { success = true, message = fileNameStored });
            }

            return Json(new { success = false, message = "No files were uploaded." });
        }

        public JsonResult RegisterStudent(StudentInfo student)
        {
            try
            {
                LienCapTHCS studentLC = new LienCapTHCS();
                string activationToken = Guid.NewGuid().ToString();

                studentLC.HocSinh_DinhDanh = student.HocSinh_DinhDanh;
                studentLC.HocSinh_HoTen = student.HocSinh_HoTen;
                studentLC.HocSinh_GioiTinh = int.Parse(student.HocSinh_GioiTinh);
                studentLC.HocSinh_NgaySinh = student.HocSinh_NgaySinh;
                studentLC.HocSinh_NoiSinh = student.HocSinh_NoiSinh;
                studentLC.HocSinh_Email = student.HocSinh_Email;
                studentLC.HocSinh_NoiCuTru = student.HocSinh_NoiCuTru;
                studentLC.HocSinh_TruongTH = student.HocSinh_TruongTH;
                studentLC.HocSinh_UuTien = student.HocSinh_UuTien;
                studentLC.HocSinh_ThongTinCha = student.HocSinh_ThongTinCha;
                studentLC.HocSinh_ThongTinMe = student.HocSinh_ThongTinMe;
                studentLC.HocSinh_DiemHocTap = student.HocSinh_DiemHocTap;
                studentLC.HocSinh_MucDoNangLuc = student.HocSinh_MucDoNangLuc;
                studentLC.HocSinh_MucDoPhamChat = student.HocSinh_MucDoPhamChat;
                studentLC.HocSinh_MinhChungHB = student.HocSinh_MinhChungHB;
                studentLC.HocSinh_MinhChungGiayKS = student.HocSinh_MinhChungGiayKS;
                studentLC.HocSinh_MinhChungMaDinhDanh = student.HocSinh_MinhChungMaDinhDanh;
                studentLC.HocSinh_GiayUuTien = student.HocSinh_GiayUuTien;
                studentLC.HocSinh_XacNhanLePhi = student.HocSinh_XacNhanLePhi;
                studentLC.HocSinh_TrangThai = 0;
                studentLC.HocSinh_Activation = activationToken;
                db.LienCapTHCSs.Add(studentLC);
                db.SaveChanges();

                string activationUrl = Url.Action("ConfirmInfomation", "LienCapTHCS", new { token = activationToken }, Request.Url.Scheme);
                var subject = "Xác nhận thông tin đăng ký";
                var body = "Xin chào " + student.HocSinh_HoTen + ", <br/> Bạn vừa đăng ký dự tuyển vào lớp 6 trường TH, THCS & THPT Hồng Đức. Vui lòng xác nhận lại các thông tin sau: " +

                     "<p> Họ và tên: " + student.HocSinh_HoTen + "</p>" +
                     "<p> Ngày tháng năm sinh: " + student.HocSinh_NgaySinh + "</p>" +
                     "<p> Nơi sinh: " + student.HocSinh_NoiSinh + "</p>" +
                     "<p> Nơi cư trú: " + student.HocSinh_NoiCuTru + "</p>" +
                     "<p> Họ và tên cha: " + student.HocSinh_HoTenCha + "</p>" +
                     "<p> Số điện thoại: " + student.HocSinh_SDTCha + "</p>" +
                     "<p> Họ và tên mẹ: " + student.HocSinh_HoTenMe + "</p>" +
                     "<p> Số điện thoại: " + student.HocSinh_SDTMe + "</p>" +
                     "<p> Điểm môn Toán: " + student.HocSinh_DiemToan + "</p>" +
                     "<p> Điểm môn Tiếng Việt: " + student.HocSinh_DiemTiengViet + "</p>" +
                     "<p> Điểm môn Tự nhiên và xã hội/khoa học: " + student.HocSinh_DiemTN + "</p>" +
                     "<p> Điểm môn Lịch sử và địa lý: " + student.HocSinh_DiemLSDL + "</p>" +
                     "<p> Điểm môn Tiếng Anh: " + student.HocSinh_DiemTA + "</p>" +
                     "<p>Nếu thông tin chính xác, vui lòng ấn vào đường link sau để xác nhận: " + activationUrl + " </p>";

                SendEmail(student.HocSinh_Email, body, subject);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ConfirmInfomation()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ConfirmInfomation(string token)
        {
            var hocSinh = db.LienCapTHCSs.Where(n => n.HocSinh_Activation == token).FirstOrDefault();
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
