using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BC = BCrypt.Net.BCrypt;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;

namespace HDU_AppXetTuyen.Controllers
{
    public class Register_ThiSinh
    {
        public string ThiSinh_CCCD { get; set; }
        public string ThiSinh_MatKhau { get; set; }
        public string ThiSinh_HoLot { get; set; }
        public string ThiSinh_Ten { get; set; }
        public string ThiSinh_DienThoai { get; set; }
        public string ThiSinh_Email { get; set; }
        public string ThiSinh_NgaySinh { get; set; }
        public string ThiSinh_DanToc { get; set; }
        public string ThiSinh_GioiTinh { get; set; }
        public string ThiSinh_DCNhanGiayBao { get; set; }
        public string ThiSinh_HoKhauThuongTru { get; set; }
        public string KhuVuc_ID { get; set; }
        public string DoiTuong_ID { get; set; }
        public string ThiSinh_TruongCapBa_Ma { get; set; }
        public string ThiSinh_TruongCapBa { get; set; }
        public string ThiSinh_TruongCapBa_Tinh_ID { get; set; }
        public string ThiSinh_HoKhauThuongTru_Check { get; set; }
        public string ThiSinh_HocLucLop12 { get; set; }
        public string ThiSinh_HanhKiemLop12 { get; set; }
    }
    public class AuthController : Controller
    {
        private DbConnecttion db = new DbConnecttion();
        // GET: Auth
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            if (Session["login_session"] != null)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                return RedirectToAction("Index", "ThiSinhDangKies");
            }
            var doituongList = db.DoiTuongs.ToList();
            var khuvucList = db.KhuVucs.ToList();
            var tinhList = db.Tinhs.ToList();
            var huyenList = db.Huyens.ToList();

            khuvucList.Insert(0, new KhuVuc { KhuVuc_ID = 0, KhuVuc_Ten = "Chọn khu vực" });
            tinhList.Insert(0, new Tinh { Tinh_ID = 0, Tinh_Ten = "Chọn tỉnh" });
            huyenList.Insert(0, new Huyen { Huyen_ID = 0, Huyen_TenHuyen = "Chọn huyện" });

            ViewBag.DoiTuong_ID = new SelectList(doituongList, "DoiTuong_ID", "DoiTuong_Ten");
            ViewBag.KhuVuc_ID = new SelectList(khuvucList, "KhuVuc_ID", "KhuVuc_Ten");
            ViewBag.Tinh_ID = new SelectList(tinhList, "Tinh_ID", "Tinh_Ten");
            ViewBag.Huyen_ID = new SelectList(huyenList, "Huyen_ID", "Huyen_TenHuyen");
            return View();
        }

        /*[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Login()
        {
            if (Session["login_session"] != null)
            {
                return RedirectToAction("tsdkttts", "ThiSinhDangKys");
            }
            return View();
        }*/

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult Login(string cccd, string password)
        {
            /* if (Session["login_session"] != null)
             {
                 return RedirectToAction("Index", "Home");
             }*/
            string mk_client = password;
            string user_client = cccd;

            /*string mk_db = ComputeHash(user_client, mk_client);*/
            var ts_login_details = db.ThiSinhDangKies.Where(x => x.ThiSinh_CCCD == cccd).FirstOrDefault();
            if (ts_login_details != null)
            {
                bool verifiedPassword = Verify(user_client, mk_client, ts_login_details.ThiSinh_MatKhau);
                if (ts_login_details.ThiSinh_TrangThai == 0)
                {
                    return Json(new { success = false, message = "Tài khoản chưa được kích hoạt" }, JsonRequestBehavior.AllowGet);
                }
                else if (verifiedPassword == true)
                {
                    Session["login_session"] = ts_login_details.ThiSinh_MatKhau;
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Mật khẩu không chính xác" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin tài khoản" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Logout()
        {
            Session["login_session"] = null;
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Index", "Auth");
        }

        /*public ActionResult Register()
        {
            ViewBag.Tinh_ID = new SelectList(db.Tinhs, "Tinh_Ma", "Tinh_Ten");
            return View();
        }*/

        [HttpPost]
        public JsonResult GetHuyen(string tinhID)
        {
            int id = int.Parse(tinhID);
            var huyenList = db.Huyens.Where(n => n.Tinh_ID == id).Select(s => new
            {
                Huyen_ID = s.Huyen_ID,
                Huyen_MaHuyen = s.Huyen_MaHuyen,
                Huyen_TenHuyen = s.Huyen_TenHuyen,
                Tinh_ID = s.Tinh_ID
            });
            return Json(new { success = true, data = huyenList.ToList() });
        }

        [HttpPost]
        public JsonResult Register(Register_ThiSinh thiSinh_register)
        {
            var ts_login_details = db.ThiSinhDangKies.Where(x => x.ThiSinh_CCCD == thiSinh_register.ThiSinh_CCCD).FirstOrDefault();
            if (ts_login_details != null)
            {
                return Json(new { success = false, message = "Số CMND/CCCD đã tồn tại. Vui lòng thử lại" }, JsonRequestBehavior.AllowGet);
            }
            var ts_login_details_email = db.ThiSinhDangKies.Where(x => x.ThiSinh_Email == thiSinh_register.ThiSinh_Email).FirstOrDefault();
            if (ts_login_details_email != null)
            {
                return Json(new { success = false, message = "Email đã tồn tại. Vui lòng thử lại" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ThiSinhDangKy ts = new ThiSinhDangKy();
                string activationToken = Guid.NewGuid().ToString();
                var hash_password = ComputeHash(thiSinh_register.ThiSinh_CCCD, thiSinh_register.ThiSinh_MatKhau);

                ts.ThiSinh_CCCD = thiSinh_register.ThiSinh_CCCD;
                ts.ThiSinh_HoLot = thiSinh_register.ThiSinh_HoLot;
                ts.ThiSinh_Ten = thiSinh_register.ThiSinh_Ten;
                ts.ThiSinh_DienThoai = thiSinh_register.ThiSinh_DienThoai;
                ts.ThiSinh_Email = thiSinh_register.ThiSinh_Email;
                ts.ThiSinh_NgaySinh = thiSinh_register.ThiSinh_NgaySinh;
                ts.ThiSinh_DanToc = thiSinh_register.ThiSinh_DanToc;
                ts.ThiSinh_GioiTinh = int.Parse(thiSinh_register.ThiSinh_GioiTinh);
                ts.ThiSinh_DCNhanGiayBao = thiSinh_register.ThiSinh_DCNhanGiayBao;
                ts.ThiSinh_HoKhauThuongTru = thiSinh_register.ThiSinh_HoKhauThuongTru;
                ts.KhuVuc_ID = int.Parse(thiSinh_register.KhuVuc_ID);
                ts.DoiTuong_ID = int.Parse(thiSinh_register.DoiTuong_ID);
                ts.ThiSinh_TruongCapBa_Ma = thiSinh_register.ThiSinh_TruongCapBa_Ma;
                ts.ThiSinh_TruongCapBa = thiSinh_register.ThiSinh_TruongCapBa;
                ts.ThiSinh_TruongCapBa_Tinh_ID = int.Parse(thiSinh_register.ThiSinh_TruongCapBa_Tinh_ID);
                ts.ThiSinh_HoKhauThuongTru_Check = thiSinh_register.ThiSinh_HoKhauThuongTru_Check;
                ts.ThiSinh_NgayDangKy = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                ts.ThiSinh_MatKhau = hash_password;
                ts.ThiSinh_TrangThai = 0;
                ts.ThiSinh_ResetCode = activationToken;
                ts.ThiSinh_HocLucLop12 = int.Parse(thiSinh_register.ThiSinh_HocLucLop12);
                ts.ThiSinh_HanhKiemLop12 = int.Parse(thiSinh_register.ThiSinh_HanhKiemLop12);
                db.ThiSinhDangKies.Add(ts);
                db.SaveChanges();

                #region Gửi mail xác thực
                string activationUrl = Url.Action("ActivationAccount", "Auth", new { token = activationToken }, Request.Url.Scheme);
                var subject = "Xác nhận tài khoản";
                var body = "Xin chào " + thiSinh_register.ThiSinh_Ten + ", <br/> Bạn vừa đăng kí tài khoản tại hệ thống xét tuyển trực tuyến của trường Đại học Hồng Đức. " +

                     " <br/>Để kích hoạt tài khoản, vui lòng ấn vào đường link sau: <br/>" +

                     " <br/>" + activationUrl + " <br/>";

                SendEmail(thiSinh_register.ThiSinh_Email, body, subject);
                #endregion

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ThiSinhDangKy thiSinh_login)
        {
            string email = thiSinh_login.ThiSinh_Email;
            var thisinh_info = db.ThiSinhDangKies.Where(x => x.ThiSinh_Email == email).FirstOrDefault();

            if (thisinh_info != null)
            {
                string randomPassword = GenerateRandomPassword(6);
                string hashRandomPassword = ComputeHash(thisinh_info.ThiSinh_CCCD, randomPassword);

                thisinh_info.ThiSinh_ResetCode = hashRandomPassword;
                db.SaveChanges();

                var subject = "Đặt lại mật khẩu";
                var body = "Xin chào " + thisinh_info.ThiSinh_Ten + ", <br/> Bạn vừa yêu cầu đổi mật khẩu. Vui lòng nhập mật khẩu mới được gửi qua email để đặt lại. " +

                     " <br/><b>" + randomPassword + "</b><br/>";

                SendEmail(thiSinh_login.ThiSinh_Email, body, subject);

                ViewBag.Message = "Vui lòng kiểm tra email của bạn";

                return RedirectToAction("ResetPassword", "Auth");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ThiSinhDangKy thiSinh_reset_info, string authCode)
        {
            string cccd = thiSinh_reset_info.ThiSinh_CCCD;
            string password = thiSinh_reset_info.ThiSinh_MatKhau;
            var thisinh_info = db.ThiSinhDangKies.Where(x => x.ThiSinh_CCCD == thiSinh_reset_info.ThiSinh_CCCD).FirstOrDefault();

            if (thisinh_info != null)
            {
                bool checkAuthCode = Verify(cccd, authCode, thisinh_info.ThiSinh_ResetCode);
                if (checkAuthCode == true)
                {
                    string computeHashPassword = ComputeHash(thisinh_info.ThiSinh_CCCD, password);
                    thisinh_info.ThiSinh_MatKhau = computeHashPassword;
                    thisinh_info.ThiSinh_ResetCode = "";
                    db.SaveChanges();
                    return RedirectToAction("Index", "Auth");
                }
                else
                {
                    ViewBag.ResetPasswordError = "Mã xác thực không chính xác. Vui lòng thử lại";
                    return View();
                }
            }
            else
            {
                ViewBag.ResetPasswordError = "Không tìm được thông tin";
                return View();
            }
        }

        public ActionResult ActivationAccount()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ActivationAccount(string token)
        {
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_ResetCode == token).FirstOrDefault();
            if (thiSinh != null)
            {
                thiSinh.ThiSinh_ResetCode = "";
                thiSinh.ThiSinh_TrangThai = 1;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public string ComputeHash(string input_user, string input_pass)
        {
            string input = input_user.Trim() + input_pass.Trim();
            string hashedPassword = BC.HashPassword(input);
            return hashedPassword;
        }

        public bool Verify(string input_user, string input_pass, string hash_pass)
        {
            string input = input_user.Trim() + input_pass.Trim();
            bool isPasswordValid = BC.Verify(input, hash_pass);
            return isPasswordValid;
        }

        private void SendEmail(string email, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("nguyenhoanglong@hdu.edu.vn", email))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                NetworkCredential NetworkCred = new NetworkCredential("nguyenhoanglong@hdu.edu.vn", "long.121100");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }

        public static string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();

            var randomString = new char[length];
            for (int i = 0; i < length; i++)
            {
                randomString[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomString);
        }
    }
}