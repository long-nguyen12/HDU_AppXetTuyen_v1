using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using HDU_AppXetTuyen.Models;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace HDU_AppXetTuyen.Controllers
{

    public class ThiSinhSessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session == null || filterContext.HttpContext.Session["login_session"] == null)
            {
                filterContext.Result = new RedirectResult("~/Auth/Login");
            }
        }
    }
    public class ThiSinhDangKiesController : Controller
    {
         private DbConnecttion db = null;

        [ThiSinhSessionCheck]
        public ActionResult Index()
        {
            var session = Session["login_session"].ToString();
            if (session != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Auth");
        }
        [ThiSinhSessionCheck]
        public JsonResult Index_Get_Json()
        {
            db = new DbConnecttion();
            var session = Session["login_session"].ToString();
            if (session != null)
            {
                var s = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();
                string Gt_Ten = "Nam"; if (s.ThiSinh_GioiTinh == 1) { Gt_Ten = "Nữ"; }
                string Hk_ten = "Tốt";
                if(s.ThiSinh_HanhKiemLop12 == 3) { Hk_ten = "Khá"; }
                if (s.ThiSinh_HanhKiemLop12 == 2) { Hk_ten = "Trung bình"; }
                if (s.ThiSinh_HanhKiemLop12 == 1) { Hk_ten = "Yếu"; }

                string Hl_ten = "Xuất sắc";
                if (s.ThiSinh_HocLucLop12 == 3) { Hl_ten = "Giỏi"; }
                if (s.ThiSinh_HocLucLop12 == 2) { Hl_ten = "Khá"; }
                if (s.ThiSinh_HocLucLop12 == 1) { Hl_ten = "Trung bình"; }

                var ts_ngaysinh = (Convert.ToDateTime(s.ThiSinh_NgaySinh)).ToString("dd/MM/yyyy");
                string[] HoKhauArr = s.ThiSinh_HoKhauThuongTru.Split('-');

                var model = new
                {
                    ThiSinh_HoLot = s.ThiSinh_HoLot,
                    ThiSinh_Ten = s.ThiSinh_Ten,
                    ThiSinh_NgaySinh = ts_ngaysinh,
                    ThiSinh_GioiTinh = Gt_Ten,
                    ThiSinh_DanToc = s.ThiSinh_DanToc,
                    ThiSinh_CCCD = s.ThiSinh_CCCD,
                    ThiSinh_DienThoai = s.ThiSinh_DienThoai,
                    ThiSinh_Email = s.ThiSinh_Email,
                    ThiSinh_DCNhanGiayBao = s.ThiSinh_DCNhanGiayBao,

                    Select_Xa_ID = HoKhauArr[0].Trim(),
                    Select_Huyen_ID = HoKhauArr[1].Trim(),
                    Select_Tinh_ID = HoKhauArr[2].Trim(),

                    TruongCapBa_TenTinh = s.TruongCapBa_TenTinh,
                    TruongCapBa_MaTinh = s.TruongCapBa_MaTinh,
                    TruongCapBa_Ten = s.TruongCapBa_Ten,
                    TruongCapBa_Ma = s.TruongCapBa_Ma,
                    ThiSinh_NamTotNghiep = s.ThiSinh_NamTotNghiep,
                    KhuVuc_ID = s.KhuVuc.KhuVuc_Ten,
                    DoiTuong_ID = s.DoiTuong.DoiTuong_Ten,
                    ThiSinh_HanhKiemLop12 = Hk_ten,
                    ThiSinh_HocLucLop12 = Hl_ten,
                   
                };
                return Json(new { success = true, data = model }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false , data ="Loi" }, JsonRequestBehavior.AllowGet);
        }
        
        [ThiSinhSessionCheck]
        public ActionResult Edit()
        {
            var session = Session["login_session"].ToString();
            if (session != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Auth");
        }
        [ThiSinhSessionCheck]
        public JsonResult Edit_Get_Json()
        {
            db = new DbConnecttion();
            var session = Session["login_session"].ToString();
            if (session != null)
            {
                var tsGet = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();
                ThongTinHoKhauThuongTru item_hokhau = JsonConvert.DeserializeObject<ThongTinHoKhauThuongTru>(tsGet.ThiSinh_HoKhauThuongTru_Check);
                TruongCapBa item_truong = JsonConvert.DeserializeObject<TruongCapBa>(tsGet.TruongCapBa_ThongTin);
                var model = new
                {
                    ThiSinh_HoLot = tsGet.ThiSinh_HoLot,
                    ThiSinh_Ten = tsGet.ThiSinh_Ten,
                    ThiSinh_NgaySinh = tsGet.ThiSinh_NgaySinh,
                    ThiSinh_GioiTinh = tsGet.ThiSinh_GioiTinh,
                    ThiSinh_DanToc = tsGet.ThiSinh_DanToc,
                    ThiSinh_CCCD = tsGet.ThiSinh_CCCD,

                    ThiSinh_DienThoai = tsGet.ThiSinh_DienThoai,
                    ThiSinh_Email = tsGet.ThiSinh_Email,
                    ThiSinh_DCNhanGiayBao = tsGet.ThiSinh_DCNhanGiayBao,
                    
                    Select_Tinh_ID = item_hokhau.Tinh_Ten,
                    Select_Tinh_ID_Hidden = item_hokhau.Tinh_ID,

                    Select_Huyen_ID = item_hokhau.Huyen_TenHuyen,
                    Select_Huyen_ID_Hidden = item_hokhau.Huyen_ID,

                    Select_Xa_ID = item_hokhau.Xa_Ten,
                    Select_Xa_ID_Hidden = item_hokhau.Xa_ID,
                    TruongCapBa_TenTinh = tsGet.TruongCapBa_TenTinh,
                    TruongCapBa_MaTinh = tsGet.TruongCapBa_MaTinh,
                    TruongCapBa_Ten = tsGet.TruongCapBa_Ten,
                    TruongCapBa_ID_Hidden = tsGet.TruongCapBa_ID,
                    TruongCapBa_Ma = tsGet.TruongCapBa_Ma,
                    ThiSinh_NamTotNghiep = tsGet.ThiSinh_NamTotNghiep,
                    Select_KhuVuc = item_truong.Truong_KhuVuc_Ten,
                    Select_KhuVuc_Hidden = item_truong.Truong_KhuVuc_Ma,
                    Select_DoiTuong = tsGet.DoiTuong_ID,
                    Select_HocLucLop12 = tsGet.ThiSinh_HocLucLop12,
                    Select_HanhKiemLop12 = tsGet.ThiSinh_HanhKiemLop12,
                };
                return Json(new { success = true, data = model }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, data = "Loi" }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Edit_Post_Json(ThiSinhDangKy entity)
        {
            db = new DbConnecttion();
            var session = Session["login_session"].ToString();
            if (session != null)
            {
                var model = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).FirstOrDefault();
                model.ThiSinh_HoLot = entity.ThiSinh_HoLot;
                model.ThiSinh_Ten = entity.ThiSinh_Ten;
                model.ThiSinh_NgaySinh = entity.ThiSinh_NgaySinh;
                model.ThiSinh_GioiTinh = entity.ThiSinh_GioiTinh;
                model.ThiSinh_DanToc = entity.ThiSinh_DanToc;
                model.ThiSinh_DienThoai = entity.ThiSinh_DienThoai;
                model.ThiSinh_DCNhanGiayBao = entity.ThiSinh_DCNhanGiayBao;
                model.ThiSinh_HoKhauThuongTru = entity.ThiSinh_HoKhauThuongTru;
                model.TruongCapBa_TenTinh = entity.TruongCapBa_TenTinh;
                model.TruongCapBa_MaTinh = entity.TruongCapBa_MaTinh;
                model.TruongCapBa_Ten = entity.TruongCapBa_Ten;
                model.TruongCapBa_Ma = entity.TruongCapBa_Ma;
                model.ThiSinh_NamTotNghiep = entity.ThiSinh_NamTotNghiep;
                model.KhuVuc_ID = entity.KhuVuc_ID;
                model.DoiTuong_ID = entity.DoiTuong_ID;
                model.ThiSinh_HocLucLop12 = entity.ThiSinh_HocLucLop12;
                model.ThiSinh_HanhKiemLop12 = entity.ThiSinh_HanhKiemLop12;
                model.TruongCapBa_ID = entity.TruongCapBa_ID;
                model.ThiSinh_HoKhauThuongTru_Check = entity.ThiSinh_HoKhauThuongTru_Check;
                db.SaveChanges();
                return Json(new { success = true, data = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, data = "error" }, JsonRequestBehavior.AllowGet);            
        }
          
        [HttpPost]
        public JsonResult GetThiSinhInfo(string id)
        {
            db = new DbConnecttion();
            int idThiSinh = int.Parse(id);
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_ID == idThiSinh).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_CCCD = n.ThiSinh_CCCD,
                ThiSinh_MatKhau = n.ThiSinh_MatKhau,
                ThiSinh_HoLot = n.ThiSinh_HoLot,
                ThiSinh_Ten = n.ThiSinh_Ten,
                ThiSinh_DienThoai = n.ThiSinh_DienThoai,
                ThiSinh_Email = n.ThiSinh_Email,
                ThiSinh_NgaySinh = n.ThiSinh_NgaySinh,
                ThiSinh_DanToc = n.ThiSinh_DanToc,
                ThiSinh_GioiTinh = n.ThiSinh_GioiTinh,
                ThiSinh_DCNhanGiayBao = n.ThiSinh_DCNhanGiayBao,
                ThiSinh_HoKhauThuongTru = n.ThiSinh_HoKhauThuongTru,
                KhuVuc_ID = n.KhuVuc_ID,
                DoiTuong_ID = n.DoiTuong_ID,
                ThiSinh_TruongCapBa_Ma = n.TruongCapBa_Ma,
                ThiSinh_TruongCapBa = n.TruongCapBa_Ten,
                ThiSinh_TruongCapBa_Tinh_ID = n.TruongCapBa_MaTinh,
                ThiSinh_TrangThai = n.ThiSinh_TrangThai,
                ThiSinh_GhiChu = n.ThiSinh_GhiChu,
                ThiSinh_HoKhauThuongTru_Check = n.ThiSinh_HoKhauThuongTru_Check,
                ThiSinh_HocLucLop12 = n.ThiSinh_HocLucLop12,
                ThiSinh_HanhKiemLop12 = n.ThiSinh_HanhKiemLop12
            }).FirstOrDefault();
            var tinhs = db.Tinhs.Select(n => new
            {
                Tinh_ID = n.Tinh_ID,
                Tinh_Ma = n.Tinh_Ma,
                Tinh_Ten = n.Tinh_Ten,
                Tinh_MaTen = n.Tinh_MaTen,
                Tinh_GhiChu = n.Tinh_GhiChu
            }).ToList();
            var khuvucs = db.KhuVucs.Select(n => new
            {
                KhuVuc_ID = n.KhuVuc_ID,
                KhuVuc_Ten = n.KhuVuc_Ten,
                KhuVuc_DiemUuTien = n.KhuVuc_DiemUuTien,
                KhuVuc_GhiChu = n.KhuVuc_GhiChu
            }).ToList();
            var doituongs = db.DoiTuongs.Select(n => new
            {
                DoiTuong_ID = n.DoiTuong_ID,
                DoiTuong_Ten = n.DoiTuong_Ten,
                DoiTuong_DiemUuTien = n.DoiTuong_DiemUuTien,
                DoiTuong_GhiChu = n.DoiTuong_GhiChu
            }).ToList();
            return Json(new { success = true, data = thiSinh, tinhs = tinhs, khuvucs = khuvucs, doituongs = doituongs }, JsonRequestBehavior.AllowGet);
        } 
        private void SendEmail(string email, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("cict@hdu.edu.vn", email))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                NetworkCredential NetworkCred = new NetworkCredential("cict@hdu.edu.vn", "hongduc1");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }    
}
