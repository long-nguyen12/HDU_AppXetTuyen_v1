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
using Microsoft.SqlServer.Server;
using System.Globalization;
using Newtonsoft.Json;

namespace HDU_AppXetTuyen.Controllers
{

    public class AuthController : Controller
    {
        private DbConnecttion db = null;// new DbConnecttion();

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            db = new DbConnecttion();
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

            tinhList.Insert(0, new Tinh { Tinh_ID = 0, Tinh_Ten = "Chọn tỉnh" });
            huyenList.Insert(0, new Huyen { Huyen_ID = 0, Huyen_TenHuyen = "Chọn huyện" });

            ViewBag.KhuVuc_ID = new SelectList(khuvucList, "KhuVuc_ID", "KhuVuc_Ten");
            ViewBag.DoiTuong_ID = new SelectList(doituongList, "DoiTuong_ID", "DoiTuong_Ten");
            ViewBag.Tinh_ID = new SelectList(tinhList, "Tinh_ID", "Tinh_Ten");
            ViewBag.Huyen_ID = new SelectList(huyenList, "Huyen_ID", "Huyen_TenHuyen");
            return View();
        }

        [HttpPost]
        public JsonResult GetHuyen(string tinhID)
        {
            db = new DbConnecttion();
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


        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword3(ThiSinhDangKy thiSinh_login)
        {
            db = new DbConnecttion();
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
        [HttpPost]
        public JsonResult ForgotPassword(ThiSinhDangKy entity)
        {
            db = new DbConnecttion();
            string email = entity.ThiSinh_Email;
            string check_table = entity.ThiSinh_GhiChu;
            if (check_table == "1")
            {
                var model = db.ThiSinhDangKies.Where(x => x.ThiSinh_Email == email).FirstOrDefault();

                if (model != null)
                {
                    string randomPassword = GenerateRandomPassword(6);
                    string hashRandomPassword = ComputeHash(model.ThiSinh_CCCD, randomPassword);

                    model.ThiSinh_ResetCode = hashRandomPassword;
                    db.SaveChanges();

                    var subject = "Đặt lại mật khẩu";
                    var body = "";
                    body += "Xin chào " + model.ThiSinh_Ten + " ";
                    body += model.ThiSinh_Ten + ", <br/> Bạn vừa yêu cầu đổi mật khẩu. Vui lòng dùng mã phía dưới để đặt lại mật khẩu. ";
                    body += " <br/><b>" + randomPassword + "</b><br/>";

                    SendEmail(model.ThiSinh_Email, body, subject);
                    return Json(new { success = true, data = "Vui lòng kiểm tra email của bạn để lấy mã" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, data = "Yêu cầu đổi mật khẩu không thành công." }, JsonRequestBehavior.AllowGet);
                }
            }
            if (check_table == "2")
            {
                var model = db.HocVienDangKies.Where(x => x.HocVien_Email == email).FirstOrDefault();

                if (model != null)
                {
                    string randomPassword = GenerateRandomPassword(6);
                    string hashRandomPassword = ComputeHash(model.HocVien_CCCD, randomPassword);

                    model.HocVien_ResetCode = hashRandomPassword;
                    db.SaveChanges();

                    var subject = "Đặt lại mật khẩu";
                    var body = "";
                    body += "Xin chào " + model.HocVien_HoDem + " " + model.HocVien_Ten;
                    body += ", <br/> Bạn vừa yêu cầu đổi mật khẩu. Vui lòng dùng mã phía dưới để đặt lại mật khẩu. ";
                    body += " <br/><b>" + randomPassword + "</b><br/>";

                    SendEmail(model.HocVien_Email, body, subject);
                    return Json(new { success = true, data = "Vui lòng kiểm tra email của bạn để lấy mã" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, data = "Yêu cầu đổi mật khẩu không thành công." }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, data = "Có lỗi xảy ra." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ResetPassword(ThiSinhDangKy entity)
        {
            db = new DbConnecttion();
            string entity_cccd = entity.ThiSinh_CCCD;
            string entity_password = entity.ThiSinh_MatKhau;
            string entity_authCode = entity.ThiSinh_ResetCode;
            string check_table = entity.ThiSinh_GhiChu;


            if (check_table == "3")
            {
                var model = db.ThiSinhDangKies.Where(x => x.ThiSinh_CCCD == entity.ThiSinh_CCCD).FirstOrDefault();
                if (model != null)
                {
                    bool checkAuthCode = Verify(entity_cccd, entity_authCode, model.ThiSinh_ResetCode);
                    if (checkAuthCode == true)
                    {
                        string computeHashPassword = ComputeHash(model.ThiSinh_CCCD, entity_password);
                        model.ThiSinh_MatKhau = computeHashPassword;
                        model.ThiSinh_ResetCode = "";
                        db.SaveChanges();
                        return Json(new { success = true, data = new { id_check = 0, text_check = "Đặt lại mật khẩu thành công" } }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, data = new { id_check = 1, text_check = "Mã xác thực không chính xác. Vui lòng thử lại" } }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, data = new { id_check = 2, text_check = "Tên đăng nhập không chính xác, vui lòng kiểm tra lại" } }, JsonRequestBehavior.AllowGet);
                }
            }
            if (check_table == "4")
            {
                var model = db.HocVienDangKies.Where(x => x.HocVien_CCCD == entity_cccd).FirstOrDefault();
                if (model != null)
                {
                    bool checkAuthCode = Verify(entity_cccd, entity_authCode, model.HocVien_ResetCode);
                    if (checkAuthCode == true)
                    {
                        string computeHashPassword = ComputeHash(model.HocVien_CCCD, entity_password);
                        model.HocVien_MatKhau = computeHashPassword;
                        model.HocVien_ResetCode = "";
                        db.SaveChanges();
                        return Json(new { success = true, data = new { id_check = 0, text_check = "Đặt lại mật khẩu thành công" } }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, data = new { id_check = 1, text_check = "Mã xác thực không chính xác. Vui lòng thử lại" } }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, data = new { id_check = 2, text_check = "Tên đăng nhập không chính xác, vui lòng kiểm tra lại" } }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, data = "Có lỗi xảy ra." }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult ResetPassword3(ThiSinhDangKy thiSinh_reset_info, string authCode)
        {
            db = new DbConnecttion();
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
            db = new DbConnecttion();
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
            db = new DbConnecttion();
            string input = input_user.Trim() + input_pass.Trim();
            string hashedPassword = BC.HashPassword(input);
            return hashedPassword;
        }

        public bool Verify(string input_user, string input_pass, string hash_pass)
        {
            db = new DbConnecttion();
            string input = input_user.Trim() + input_pass.Trim();
            bool isPasswordValid = BC.Verify(input, hash_pass);
            return isPasswordValid;
        }

        private void SendEmail(string email, string body, string subject)
        {
            db = new DbConnecttion();
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

        #region Login 

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Login()
        {
            db = new DbConnecttion();
            if (Session["login_session"] != null)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }


        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult Login(string cccd, string password)
        {
            db = new DbConnecttion();
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

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LoginColleger(string cccd, string password)
        {
            db = new DbConnecttion();
            string mk_client = password;
            string user_client = cccd;

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
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LoginMaster(string cccd, string password)
        {
            db = new DbConnecttion();
            string mk_client = password;
            string user_client = cccd;

            /*string mk_db = ComputeHash(user_client, mk_client);*/
            var hv_login_details = db.HocVienDangKies.Where(x => x.HocVien_CCCD == cccd).FirstOrDefault();
            if (hv_login_details != null)
            {
                bool verifiedPassword = Verify(user_client, mk_client, hv_login_details.HocVien_MatKhau);
                if (hv_login_details.HocVien_TrangThai == 0)
                {
                    return Json(new { success = false, message = "Tài khoản chưa được kích hoạt" }, JsonRequestBehavior.AllowGet);
                }
                else if (verifiedPassword == true)
                {
                    Session["login_session"] = hv_login_details.HocVien_MatKhau;
                    //string str = Session["login_session"].ToString();
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
            db = null; ;
            Session["login_session"] = null;
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login", "Auth");
        }
        #endregion

        #region RegisterColleger

        public ActionResult RegisterColleger()
        {
            Session["login_session"] = null;
            Session.Clear();
            Session.RemoveAll();
            db = new DbConnecttion();

            if (Session["login_session"] != null)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }
        public JsonResult GetData_ForSelect_Tinh()
        {
            db = new DbConnecttion();

            var TinhList = db.Tinhs.Select(x => new
            {
                Tinh_ID = x.Tinh_ID,
                Tinh_Ma = x.Tinh_Ma,
                Tinh_Ten = x.Tinh_Ten,
                Tinh_Ten_Eng = x.Tinh_Ten_Eng,
                Tinh_MaTen = x.Tinh_MaTen,
                Tinh_GhiChu = x.Tinh_GhiChu
            }).ToList();

            return Json(new { success = true, TinhList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetData_ForSelect_Huyen(Huyen entity)
        {
            db = new DbConnecttion();
            int Tinh_ID = int.Parse(entity.Tinh_ID.ToString());
            if (Tinh_ID > 0)
            {
                var HuyenList = db.Huyens.Where(x => x.Tinh_ID == Tinh_ID).Select(x => new
                {
                    Huyen_ID = x.Huyen_ID,
                    Huyen_MaHuyen = x.Huyen_MaHuyen,
                    Huyen_TenHuyen = x.Huyen_TenHuyen,
                    Huyen_TenHuyen_Eng = x.Huyen_TenHuyen_Eng,
                    Huyen_GhiChu = x.Huyen_GhiChu,
                    Tinh_ID = x.Tinh_ID,

                }).ToList();
                return Json(new { success = true, HuyenList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var HuyenList = new
                {
                    Huyen_ID = -1,
                    Huyen_MaHuyen = "",
                    Huyen_TenHuyen = "",
                    Huyen_TenHuyen_Eng = "",
                    Huyen_GhiChu = "",
                    Tinh_ID = "",
                };
                return Json(new { success = true, HuyenList }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetData_ForSelect_Xa(Xa entity)
        {
            db = new DbConnecttion();
            int Huyen_ID = int.Parse(entity.Huyen_ID.ToString());
            if (Huyen_ID > 0)
            {
                var XaList = db.Xas.Where(x => x.Huyen_ID == Huyen_ID).Select(x => new
                {
                    Xa_ID = x.Xa_ID,
                    Xa_Ma = x.Xa_Ma,
                    Xa_Ten = x.Xa_Ten,
                    Xa_Ten_Eng = x.Xa_Ten_Eng,
                    Xa_GhiChu = x.Xa_GhiChu,
                    Huyen_ID = x.Huyen_ID,

                }).ToList();
                return Json(new { success = true, XaList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var XaList = new
                {
                    Xa_ID = -1,
                    Xa_Ma = "00",
                    Xa_Ten = "Không xác định",
                    Xa_Ten_Eng = "Khong xac dinh",
                    Xa_GhiChu = "",
                    Huyen_ID = "-1",
                };
                return Json(new { success = true, XaList }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetData_ForSelect_DoiTuong()
        {
            db = new DbConnecttion();
            var DoituongList = db.DoiTuongs.Select(x => new
            {
                DoiTuong_ID = x.DoiTuong_ID,
                DoiTuong_Ten = x.DoiTuong_Ten,
                DoiTuong_DiemUuTien = x.DoiTuong_DiemUuTien,
                DoiTuong_GhiChu = x.DoiTuong_GhiChu,
            }).ToList();

            return Json(new { success = true, DoituongList }, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult GetData_ForSelect_TinhCapBa()
        {
            db = new DbConnecttion();
            var TinhCapBaList = db.TruongCapBas.Select(x => new
            {
                Truong_MaTinh = x.Truong_MaTinh.ToString(),
                Truong_TenTinh = x.Truong_TenTinh.ToString(),
                Truong_TenTinh_Eng = x.Truong_TenTinh_Eng.ToString(),
            }).Distinct().ToList();
            return Json(new { success = true, TinhCapBaList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetData_ForSelect_TruongCapBa(TruongCapBa entity)
        {
            db = new DbConnecttion();
            string truong_matinh = entity.Truong_MaTinh.ToString();
            var TruongCapBaList = db.TruongCapBas.Where(x => x.Truong_MaTinh == truong_matinh).Select(x => new
            {
                Truong_ID = x.Truong_ID,
                Truong_MaTinh = x.Truong_MaTinh,
                Truong_TenTinh = x.Truong_TenTinh,
                Truong_MaTruong = x.Truong_MaTruong,
                Truong_TenTruong = x.Truong_TenTruong,
                Truong_KhuVuc_Ma = x.Truong_KhuVuc_Ma,
                Truong_KhuVuc_Ten = x.Truong_KhuVuc_Ten,
                Truong_TenTruong_Eng = x.Truong_TenTruong_Eng,
            }).ToList();
            return Json(new { success = true, TruongCapBaList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RegisterColleger_Check(ThiSinhDangKy entity)
        {
            db = new DbConnecttion();
            var ts_cccd = entity.ThiSinh_DienThoai;

            var ts_email = entity.ThiSinh_Email;

            if (!String.IsNullOrEmpty(ts_cccd))
            {
                var model = db.ThiSinhDangKies.Where(x => x.ThiSinh_CCCD == ts_cccd.ToString()).FirstOrDefault();
                if (model != null) { return Json(new { success = true, message = "1", presentcheck = "1" }, JsonRequestBehavior.AllowGet); }
                return Json(new { success = false, message = "0", presentcheck = "0" }, JsonRequestBehavior.AllowGet);
            }
            if (!String.IsNullOrEmpty(ts_email))
            {
                var model = db.ThiSinhDangKies.Where(x => x.ThiSinh_Email == ts_email.ToString()).FirstOrDefault();
                if (model != null) { return Json(new { success = true, message = "1", presentcheck = "1" }, JsonRequestBehavior.AllowGet); }
                return Json(new { success = false, message = "0", presentcheck = "0" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "-1", presentcheck = "-1" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RegisterColleger(ThiSinhDangKy entity)
        {
            db = new DbConnecttion();
            var truongcapba_id = int.Parse(entity.TruongCapBa_ID.ToString());   
           
            string ActivationToken = Guid.NewGuid().ToString();
            var HashPassWord = ComputeHash(entity.ThiSinh_CCCD, entity.ThiSinh_MatKhau);
            ThiSinhDangKy model = new ThiSinhDangKy();

            model.ThiSinh_HoLot = entity.ThiSinh_HoLot;// ts.ThiSinh_HoLot = ts_register.ThiSinh_HoLot;
            model.ThiSinh_Ten = entity.ThiSinh_Ten; //ts.ThiSinh_Ten = ts_register.ThiSinh_Ten;
            model.ThiSinh_NgaySinh = entity.ThiSinh_NgaySinh; //ts.ThiSinh_NgaySinh = ts_register.ThiSinh_NgaySinh;
            model.ThiSinh_GioiTinh = entity.ThiSinh_GioiTinh;//ts.ThiSinh_GioiTinh = ts_register.ThiSinh_GioiTinh;
            model.ThiSinh_DanToc = entity.ThiSinh_DanToc;//ts.ThiSinh_DanToc = ts_register.ThiSinh_DanToc;
            model.ThiSinh_CCCD = entity.ThiSinh_CCCD;//ts.ThiSinh_CCCD = ts_register.ThiSinh_CCCD;
            model.ThiSinh_MatKhau = HashPassWord; //ts.ThiSinh_MatKhau = hash_password;
            model.ThiSinh_ResetCode = ActivationToken;

            model.ThiSinh_DienThoai = entity.ThiSinh_DienThoai; //ts.ThiSinh_DienThoai = ts_register.ThiSinh_DienThoai;
            model.ThiSinh_Email = entity.ThiSinh_Email; //ts.ThiSinh_Email = ts_register.ThiSinh_Email;
            model.ThiSinh_DCNhanGiayBao = entity.ThiSinh_DCNhanGiayBao;// ts.ThiSinh_DCNhanGiayBao = ts_register.ThiSinh_DCNhanGiayBao;
            model.ThiSinh_HoKhauThuongTru = entity.ThiSinh_HoKhauThuongTru; //ts.ThiSinh_HoKhauThuongTru = ts_register.ThiSinh_HoKhauThuongTru;

            model.TruongCapBa_TenTinh = entity.TruongCapBa_TenTinh;
            model.TruongCapBa_MaTinh = entity.TruongCapBa_MaTinh;//ts.TruongCapBa_MaTinh = "";
            model.TruongCapBa_Ten = entity.TruongCapBa_Ten;//ts.TruongCapBa_Ten = "";
            model.TruongCapBa_Ma = entity.TruongCapBa_Ma;//ts.TruongCapBa_Ma = "";
            model.ThiSinh_NamTotNghiep = entity.ThiSinh_NamTotNghiep;
            model.KhuVuc_ID = entity.KhuVuc_ID; //ts.KhuVuc_ID = ts_register.KhuVuc_ID;
            model.DoiTuong_ID = entity.DoiTuong_ID;//ts.DoiTuong_ID = ts_register.DoiTuong_ID;
            model.ThiSinh_HocLucLop12 = entity.ThiSinh_HocLucLop12; //ts.ThiSinh_HocLucLop12 = ts_register.ThiSinh_HocLucLop12;
            model.ThiSinh_HanhKiemLop12 = entity.ThiSinh_HanhKiemLop12;//ts.ThiSinh_HanhKiemLop12 = ts_register.ThiSinh_HanhKiemLop12;
            model.ThiSinh_TrangThai = 1;
            model.TruongCapBa_ID = entity.TruongCapBa_ID;
            model.TruongCapBa_ThongTin = JsonConvert.SerializeObject(db.TruongCapBas.Where(x => x.Truong_ID == truongcapba_id).FirstOrDefault());
            model.ThiSinh_HoKhauThuongTru_Check = entity.ThiSinh_HoKhauThuongTru_Check; //ts.ThiSinh_HoKhauThuongTru_Check = ts_register.ThiSinh_HoKhauThuongTru_Check;        
            model.ThiSinh_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");           
            
            db.ThiSinhDangKies.Add(model);
            db.SaveChanges();

            #region Gửi mail xác thực
            string activationUrl = Url.Action("ActivationAccount", "Auth", new { token = ActivationToken }, Request.Url.Scheme);
            var subject = "Xác nhận tài khoản";
            var body = "Xin chào " + entity.ThiSinh_HoLot + " " + entity.ThiSinh_Ten + ", <br/> Bạn vừa đăng kí tài khoản tại hệ thống xét tuyển trực tuyến của trường Đại học Hồng Đức." +

                 " <br/>Để kích hoạt tài khoản, vui lòng ấn vào đường link sau: <br/>" +

                 " <br/>" + activationUrl + " <br/>";

            SendEmail(entity.ThiSinh_Email, body, subject);
            #endregion

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region RegisterMaster
        public ActionResult RegisterMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RegisterMasterCheckCCCD(HocVienDangKy entity)
        {
            db = new DbConnecttion();
            var model = db.HocVienDangKies.Where(x => x.HocVien_CCCD == entity.HocVien_CCCD).FirstOrDefault();

            if (model != null)
            {
                return Json(new { success = true, message = "Số Căn cước công dân tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = entity.HocVien_CCCD }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RegisterMasterCheckEmail(HocVienDangKy entity)
        {
            db = new DbConnecttion();
            var model = db.HocVienDangKies.Where(x => x.HocVien_Email == entity.HocVien_Email).FirstOrDefault();

            if (model != null)
            {
                return Json(new { success = true, message = "Email đã tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = entity.HocVien_Email }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RegisterMaster(HocVienDangKy hv_register)
        {
            db = new DbConnecttion();
            var register_check_CCCD_HV = db.HocVienDangKies.Where(x => x.HocVien_CCCD == hv_register.HocVien_CCCD).FirstOrDefault();

            if (register_check_CCCD_HV != null)
            {
                return Json(new { success = false, message = "Số Căn cước công dân đã có." }, JsonRequestBehavior.AllowGet);
            }

            var register_check_Email_HV = db.HocVienDangKies.Where(x => x.HocVien_Email == hv_register.HocVien_Email).FirstOrDefault();
            if (register_check_Email_HV != null)
            {
                return Json(new { success = false, message = "Email đã tồn tại." }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                HocVienDangKy hv_new = new HocVienDangKy();
                string activationToken = Guid.NewGuid().ToString();
                var hash_password = ComputeHash(hv_register.HocVien_CCCD, hv_register.HocVien_MatKhau);

                hv_new.HocVien_HoDem = hv_register.HocVien_HoDem;
                hv_new.HocVien_Ten = hv_register.HocVien_Ten;
                hv_new.HocVien_GioiTinh = hv_register.HocVien_GioiTinh;
                hv_new.HocVien_DanToc = hv_register.HocVien_DanToc;
                hv_new.HocVien_NgaySinh = hv_register.HocVien_NgaySinh;

                hv_new.HocVien_CCCD = hv_register.HocVien_CCCD;
                hv_new.HocVien_CCCD_NgayCap = hv_register.HocVien_CCCD_NgayCap;

                hv_new.HocVien_BangDaiHoc = "";
                hv_new.HocVien_BoTucKienThuc = -1;
                hv_new.HocVien_DoiTuongUuTien = "";

                hv_new.HocVien_DienThoai = hv_register.HocVien_DienThoai;
                hv_new.HocVien_Email = hv_register.HocVien_Email;
                hv_new.HocVien_HoKhauThuongTru = hv_register.HocVien_HoKhauThuongTru;
                hv_new.HocVien_NoiOHienNay = hv_register.HocVien_HoKhauThuongTru;
                hv_new.HocVien_DiaChiLienHe = hv_register.HocVien_DiaChiLienHe;
                hv_new.HocVien_NoiSinh = hv_register.HocVien_NoiSinh;

                hv_new.HocVien_MatKhau = hash_password;

                hv_new.HocVien_ResetCode = activationToken;
                hv_new.HocVien_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                hv_new.HocVien_TrangThai = 0;

                hv_new.HocVien_TenDonViCongTac = hv_register.HocVien_TenDonViCongTac;
                hv_new.HocVien_ChuyenMon = hv_register.HocVien_ChuyenMon;
                hv_new.HocVien_ThamNien = hv_register.HocVien_ThamNien;
                hv_new.HocVien_ChucVu = hv_register.HocVien_ChucVu;
                hv_new.HocVien_NamCT = hv_register.HocVien_NamCT;
                hv_new.HocVien_LoaiCB = hv_register.HocVien_LoaiCB;

                db.HocVienDangKies.Add(hv_new);
                db.SaveChanges();

                #region Gửi mail xác thực
                string activationUrl = Url.Action("ActivationAccountMaster", "Auth", new { token = activationToken }, Request.Url.Scheme);
                var subject = "Xác nhận tài khoản";
                var body = "Xin chào " + hv_register.HocVien_HoDem + " " + hv_register.HocVien_Ten + ", <br/> Bạn vừa đăng kí tài khoản tại hệ thống xét tuyển trực tuyến của trường Đại học Hồng Đức. " +

                     " <br/>Để kích hoạt tài khoản, vui lòng ấn vào đường link sau: <br/>" +

                     " <br/>" + activationUrl + " <br/>";

                SendEmail(hv_register.HocVien_Email, body, subject);
                #endregion

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTinh()
        {
            db = new DbConnecttion();
            var TinhList = db.Tinhs.OrderBy(x => x.Tinh_Ten).Select(t => new
            {
                tinh_ID = t.Tinh_ID,
                tinh_Ma = t.Tinh_Ma,
                tinh_Ten = t.Tinh_Ten
            });
            return Json(new { success = true, data = TinhList.ToList() });
        }

        public ActionResult ActivationAccountMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ActivationAccountMaster(string token)
        {
            db = new DbConnecttion();
            var hocvien = db.HocVienDangKies.Where(n => n.HocVien_ResetCode == token).FirstOrDefault();
            if (hocvien != null)
            {
                hocvien.HocVien_ResetCode = "";
                hocvien.HocVien_TrangThai = 1;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        #endregion
        #region Kiểm tra thời gian hệ thống mở đăng ký
        public JsonResult ThongTinDotXetTuyenHienTai()
        {
            db = new DbConnecttion();
            var CurrentColleger = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).Select(s => new
            {
                Dxt_ID = s.Dxt_ID,
                Dxt_Classify = s.Dxt_Classify,
                Dxt_Ten = s.Dxt_Ten,
                Dxt_TrangThai_Xt = s.Dxt_TrangThai_Xt,
                Dxt_ThoiGian_BatDau = s.Dxt_ThoiGian_BatDau,
                Dxt_ThoiGian_KetThuc = s.Dxt_ThoiGian_KetThuc,
                NamHoc_ID = s.NamHoc_ID,
                NamHoc_Ten = db.NamHocs.FirstOrDefault(x => x.NamHoc_ID == s.NamHoc_ID).NamHoc_Ten
            });
            var CurrentMaster = db.DotXetTuyens.Where(x => x.Dxt_Classify == 2 && x.Dxt_TrangThai_Xt == 1).Select(s => new
            {
                Dxt_ID = s.Dxt_ID,
                Dxt_Classify = s.Dxt_Classify,
                Dxt_Ten = s.Dxt_Ten,
                Dxt_TrangThai_Xt = s.Dxt_TrangThai_Xt,
                Dxt_ThoiGian_BatDau = s.Dxt_ThoiGian_BatDau,
                Dxt_ThoiGian_KetThuc = s.Dxt_ThoiGian_KetThuc,
                NamHoc_ID = s.NamHoc_ID,
                NamHoc_Ten = db.NamHocs.FirstOrDefault(x => x.NamHoc_ID == s.NamHoc_ID).NamHoc_Ten
            });

            var CurrentGifted = db.DotXetTuyens.Where(x => x.Dxt_Classify == 1 && x.Dxt_TrangThai_Xt == 1).Select(s => new
            {
                Dxt_ID = s.Dxt_ID,
                Dxt_Classify = s.Dxt_Classify,
                Dxt_Ten = s.Dxt_Ten,
                Dxt_TrangThai_Xt = s.Dxt_TrangThai_Xt,
                Dxt_ThoiGian_BatDau = s.Dxt_ThoiGian_BatDau,
                Dxt_ThoiGian_KetThuc = s.Dxt_ThoiGian_KetThuc,
                NamHoc_ID = s.NamHoc_ID,
                NamHoc_Ten = db.NamHocs.FirstOrDefault(x => x.NamHoc_ID == s.NamHoc_ID).NamHoc_Ten
            });

            return Json(new { success = true, data = new { CurrentColleger, CurrentMaster, CurrentGifted } }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

}