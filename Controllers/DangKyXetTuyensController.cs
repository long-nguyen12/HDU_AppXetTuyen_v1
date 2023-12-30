using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Newtonsoft.Json;

using System.IO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Xceed.Words.NET;
using System.Net.Mail;
using System.Net;
using System.Xml.Linq;

namespace HDU_AppXetTuyen.Controllers
{
    public class DangKyXetTuyensController : Controller
    {
        private DbConnecttion db = null;
        #region Đăng ký dự thi năng khiếu Tiểu học, mầm non, gdtc

        [ThiSinhSessionCheck]
        public ActionResult Dkdtnk()
        {
            return View();
        }
        public ActionResult DkdtnkEdit()
        {
            return View();
        }
        public JsonResult Dkdtnk_GetData()
        {
            string str_login_session = Session["login_session"].ToString();
            db = new DbConnecttion();
            var Dothihientai = db.DotXetTuyens.Where(x => x.Dxt_Classify == 1 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            var model = db.DangKyDuThiNangKhieus
                                .Include(d => d.ThiSinhDangKy)
                                .Where(x => x.ThiSinhDangKy.ThiSinh_MatKhau == str_login_session && x.DotXT_ID == Dothihientai.Dxt_ID).FirstOrDefault();

            if (model != null)
            {
                // cần lấy tên ngành và tổ hợp môn để hiển thị ra drop nếu đã đăng ký; mỗi thí sinh chỉ đăng ký 1 lần trong 1 đợt
                var TenNganhThi = db.Nganhs.Where(x => x.Nganh_ID == model.Nganh_ID).FirstOrDefault().Nganh_TenNganh;
                var TenToHopThi = db.ToHopMons.Where(x => x.Thm_ID == model.Thm_ID).FirstOrDefault().Thm_TenToHop;
                if (!String.IsNullOrEmpty(model.Dkdt_LePhi_MinhChung_Tep)) { model.Dkdt_LePhi_MinhChung_Tep = model.Dkdt_LePhi_MinhChung_Tep.Replace("#", ""); }

                var data_return = new
                {
                    Dkdt_NK_ID = model.Dkdt_NK_ID,
                    Dxt_Ten = Dothihientai.Dxt_Ten,
                    Dxt_ThoiGian_BatDau = Dothihientai.Dxt_ThoiGian_BatDau,
                    Dxt_ThoiGian_KetThuc = Dothihientai.Dxt_ThoiGian_KetThuc,

                    Nganh_ID = model.Nganh_ID,
                    Nganh_TenNganh = TenNganhThi,
                    Thm_ID = model.Thm_ID,
                    Thm_TenToHop = TenToHopThi,
                    Dkdt_NK_MonThi = model.Dkdt_NK_MonThi,
                    Dkdt_NK_MinhChung_CCCD = model.Dkdt_NK_MinhChung_CCCD,
                    Dkdt_TrangThai_LePhi = model.Dkdt_TrangThai_LePhi,
                    Dkdt_LePhi_MinhChung_Tep = model.Dkdt_LePhi_MinhChung_Tep,
                    Dkdt_LePhi_MinhChung_MaThamChieu = model.Dkdt_LePhi_MinhChung_MaThamChieu,

                };
                return Json(new { success = true, data = data_return }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data_return = new
                {
                    dxt_Ten = Dothihientai.Dxt_Ten,
                    dxt_ThoiGian_BatDau = Dothihientai.Dxt_ThoiGian_BatDau,
                    dxt_ThoiGian_KetThuc = Dothihientai.Dxt_ThoiGian_KetThuc,

                };
                return Json(new { success = false, data = data_return }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Dkdtnk_DotThiHienTai()
        {
            db = new DbConnecttion();
            var model = db.DotXetTuyens.Where(d => d.Dxt_Classify == 1 && d.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var data_return = new
            {
                dxt_ID = model.Dxt_ID,
                dxt_Ten = model.Dxt_Ten,
                dxt_ThoiGian_BatDau = model.Dxt_ThoiGian_BatDau,
                dxt_ThoiGian_KetThuc = model.Dxt_ThoiGian_KetThuc,
                dxt_TrangThai_Xt = model.Dxt_TrangThai_Xt,
                dxt_GhiChu = model.Dxt_GhiChu
            };
            return Json(new { success = true, data = data_return }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Dkdtnk_UploadFile_Lephi()
        {
            db = new DbConnecttion();
            // string str_pas = Session["login_session"].ToString();

            string str_pas = Session["login_session"].ToString();
            var ts_detail = db.ThiSinhDangKies.Where(x => x.ThiSinh_MatKhau == str_pas).FirstOrDefault();
            string so_cc_congdan = ts_detail.ThiSinh_CCCD;

            bool check = false;
            string Dkdt_NK_LePhi_TepMC = "";
            try
            {
                HttpFileCollectionBase files = Request.Files;

                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;
                    // Checking for Internet Explorer      
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = so_cc_congdan + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = so_cc_congdan + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + file.FileName;
                    }
                    // lấy chuỗi lưu vào csdl
                    Dkdt_NK_LePhi_TepMC += "/Uploads/DKDTNangKhieuFile/" + fname + "#";

                    // Get the complete folder path and store the file inside it.      
                    fname = Path.Combine(Server.MapPath("~/Uploads/DKDTNangKhieuFile/"), fname);
                    file.SaveAs(fname);
                }
                check = true;
            }
            catch { check = false; }
            return Json(new { success = check, Dkdt_NK_LePhi_TepMC }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult NganhListForThiNK()
        {
            db = new DbConnecttion();

            var selectResult_Nganh = db.Nganhs.Where(x => x.Nganh_ThiNK == 1).OrderBy(x => x.Nganh_ID).Select(s => new
            {
                nganh_ID = s.Nganh_ID,
                nganh_GhiChu = s.Nganh_TenNganh,
            });
            return Json(selectResult_Nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ToHopMonListForThiNK(ToHopMon thm_n)
        {
            int _thm_ID = int.Parse(thm_n.Thm_ID.ToString());
            db = new DbConnecttion();

            var thm_get_item = db.ToHopMons.Find(_thm_ID);

            if (thm_get_item.Thm_Thi_NK.ToString() == "2")
            {
                thm_get_item.Thm_Mon3 = "Bật xa tại chỗ và Chạy 100m";
            }

            if (thm_get_item.Thm_Thi_NK.ToString() == "1")
            {
                thm_get_item.Thm_Mon3 = "Đọc diễn cảm và hát";
            }

            var thmObj = new
            {
                thm_Mon3 = thm_get_item.Thm_Mon3
            };

            return Json(thmObj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ToHopMonNganhListAll_ThiNK(int id)
        {
            db = new DbConnecttion();
            var selectResult_tohopmon_nganh = db.ToHopMonNganhs.Include(n => n.ToHopMon)
                .Where(x => x.Nganh_ID == id && x.ToHopMon.Thm_Thi_NK > 0).OrderBy(x => x.ToHopMon.Thm_ID).Select(s => new
                {
                    thm_ID = s.Thm_ID,
                    thm_MaTen = s.ToHopMon.Thm_MaTen,
                    thm_TenToHop = s.ToHopMon.Thm_TenToHop
                });
            return Json(selectResult_tohopmon_nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyDuThi_UploadFile_Multi()
        {
            db = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString(); //"$2a$11$jwPUP78RBpC9R3uW7Dqpau.SXwogLasbvVx3q0vqhoE93Lx044lcu"; //
            var ts = db.ThiSinhDangKies.FirstOrDefault(n => n.ThiSinh_MatKhau.Equals(str_login_session));
            string cccd = ts.ThiSinh_CCCD;

            HttpFileCollectionBase files = Request.Files;


            string _MinhChung_CCCD = "";

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname;
                // Checking for Internet Explorer      
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + file.FileName;
                }
                // lấy chuỗi lưu vào csdl             
                _MinhChung_CCCD += "/Uploads/DKDTNangKhieuFile/" + fname + "#";
                // Get the complete folder path and store the file inside it.      
                fname = Path.Combine(Server.MapPath("~/Uploads/DKDTNangKhieuFile/"), fname);
                file.SaveAs(fname);
            }
            return Json(new
            {
                _MinhChung_CCCD,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyDuThi_NangKhieu_Insert(DangKyDuThiNangKhieu dkdt_nk_post)
        {

            db = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString();

            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(str_login_session)).
                Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var dotxettuyen = db.DotXetTuyens.Where(x => x.Dxt_Classify == 1 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            if (ts != null)
            {
                DangKyDuThiNangKhieu model_new = new DangKyDuThiNangKhieu();

                model_new.Nganh_ID = dkdt_nk_post.Nganh_ID;
                model_new.Thm_ID = dkdt_nk_post.Thm_ID;

                model_new.Dkdt_NK_MonThi = dkdt_nk_post.Dkdt_NK_MonThi;
                model_new.Dkdt_NK_MinhChung_CCCD = dkdt_nk_post.Dkdt_NK_MinhChung_CCCD;
                model_new.Dkdt_NK_NgayDangKy = DateTime.Now.ToString("yyyy-MM-yy");

                model_new.ThiSinh_ID = ts.ThiSinh_ID;
                model_new.Ptxt_ID = 7;

                model_new.DotXT_ID = dotxettuyen.Dxt_ID;

                model_new.Dkdt_NK_GhiChu = "";
                model_new.Dkdt_LePhi_MinhChung_MaThamChieu = "";
                model_new.Dkdt_LePhi_MinhChung_Tep = "";
                model_new.Dkdt_LePhi_MinhChung_NgayGui = "";

                model_new.Dkdt_TrangThai_HoSo = 0;
                model_new.Dkdt_TrangThai_KetQua = 0;
                model_new.Dkdt_TrangThai_LePhi = 0;


                db.DangKyDuThiNangKhieus.Add(model_new);
                db.SaveChanges();
                // add LePhiXetTuyen             
                // gửi email
                #region gửi email
                int nganh_id = int.Parse(model_new.Nganh_ID.ToString());
                int thm_id = int.Parse(model_new.Thm_ID.ToString());
                var nganhdk = db.Nganhs.FirstOrDefault(n => n.Nganh_ID == nganh_id);
                var thmdk = db.ToHopMons.FirstOrDefault(t => t.Thm_ID == thm_id);
                var subject = "Đăng ký nguyện vọng";
                var body = "Thí sinh " + ts.ThiSinh_Ten + ", Số CCCD: " + ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +
                      " <br/><b>Thông tin nguyện vọng nộp dự thi năng khiếu :</b><br/>" +
                      " <p>- Mã ngành, tên ngành: " + nganhdk.Nganh_MaNganh + nganhdk.Nganh_TenNganh + " </p>" +
                      " <p>- Tên tổ hợp môn: " + model_new.Dkdt_NK_MonThi + " </p>";
                SendEmail s = new SendEmail();
                s.Sendemail("xettuyen@hdu.edu.vn", body, subject);
                #endregion
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DangKyDuThi_NangKhieu_Edit(DangKyDuThiNangKhieu entity)
        {

            db = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString();

            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(str_login_session)).
                Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var dotxettuyen = db.DotXetTuyens.Where(x => x.Dxt_TrangThai_Xt == 1 && x.Dxt_Classify == 1).FirstOrDefault();
            var model_edit = db.DangKyDuThiNangKhieus.Where(x => x.Dkdt_NK_ID == entity.Dkdt_NK_ID).FirstOrDefault();
            if (model_edit != null)
            {

                model_edit.Nganh_ID = entity.Nganh_ID;
                model_edit.Thm_ID = entity.Thm_ID;

                model_edit.Dkdt_NK_MonThi = entity.Dkdt_NK_MonThi;
                if (entity.Dkdt_NK_MinhChung_CCCD != null)
                {
                    model_edit.Dkdt_NK_MinhChung_CCCD += entity.Dkdt_NK_MinhChung_CCCD;
                }
                model_edit.Dkdt_NK_NgayDangKy = DateTime.Now.ToString("yyyy-MM-yy");

                model_edit.Dkdt_NK_GhiChu = "";
                model_edit.Dkdt_TrangThai_HoSo = 0;

                db.SaveChanges();
                // gửi email
                #region gửi email
                int nganh_id = int.Parse(model_edit.Nganh_ID.ToString());
                int thm_id = int.Parse(model_edit.Thm_ID.ToString());
                var nganhdk = db.Nganhs.FirstOrDefault(n => n.Nganh_ID == nganh_id);
                var thmdk = db.ToHopMons.FirstOrDefault(t => t.Thm_ID == thm_id);
                var subject = "Chỉnh sửa nguyện vọng";
                var body = "Thí sinh " + ts.ThiSinh_Ten + ", Số CCCD: " + ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +
                     " <br/><b>Thông tin nguyện vọng nộp dự thi năng khiếu :</b><br/>" +
                     " <p>- Mã ngành, tên ngành: " + nganhdk.Nganh_MaNganh + nganhdk.Nganh_TenNganh + " </p>" +
                     " <p>- Tên tổ hợp môn: " + model_edit.Dkdt_NK_MonThi + " </p>";
                SendEmail s = new SendEmail();
                s.Sendemail("xettuyen@hdu.edu.vn", body, subject);
                #endregion
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyDuThi_NangKhieu_Delete(DangKyDuThiNangKhieu entity)
        {
            db = new DbConnecttion();
            int dkdt_NK_ID = int.Parse(entity.Dkdt_NK_ID.ToString());

            DangKyDuThiNangKhieu dkdt_item_getby_id = db.DangKyDuThiNangKhieus.Find(dkdt_NK_ID);
            db.DangKyDuThiNangKhieus.Remove(dkdt_item_getby_id);

            db.SaveChanges();
            return Json(new
            {
                status = true,
                msg = "Xoá dữ liệu thành công"
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DangKyDuThi_NangKhieu_DeleteMC(DangKyDuThiNangKhieu entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyDuThiNangKhieus.Where(x => x.Dkdt_NK_ID == entity.Dkdt_NK_ID).FirstOrDefault();
            model.Dkdt_NK_MinhChung_CCCD = model.Dkdt_NK_MinhChung_CCCD.Replace(entity.Dkdt_NK_MinhChung_CCCD + "#", "");
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DkdtNK_UploadLePhiMinhChung()
        {
            db = new DbConnecttion();
            try
            {
                var session = Session["login_session"];
                var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();

                string minhchungs = "";
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = thiSinh.ThiSinh_CCCD + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + fileName;
                        var filePath = Path.Combine(Server.MapPath("~/Uploads/FileMinhChungKinhPhis"), fileName);
                        var savePath = "/Uploads/FileMinhChungKinhPhis/" + fileName;
                        minhchungs += savePath + "#";
                        file.SaveAs(filePath);
                    }
                    return Json(new { success = true, message = minhchungs }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, data = Request.Files.Count }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DkdtNK_Update_ThongTinLePhi(KinhPhiInfo entity)
        {
            db = new DbConnecttion();
            var Dkdt_NK_id = long.Parse(entity.key_dkxt_id.Trim());
            var model = db.DangKyDuThiNangKhieus.Include(x => x.ThiSinhDangKy).Where(x => x.Dkdt_NK_ID == Dkdt_NK_id).FirstOrDefault();
            if (entity.KinhPhi_TepMinhChung != "KHONGSUA")
            {
                model.Dkdt_LePhi_MinhChung_Tep = entity.KinhPhi_TepMinhChung;
            }
            model.Dkdt_LePhi_MinhChung_MaThamChieu = entity.KinhPhi_SoTC;
            model.Dkdt_TrangThai_LePhi = 1;
            model.Dkdt_LePhi_MinhChung_NgayGui = DateTime.Now.ToString("yyyy-MM-dd");
            db.SaveChanges();
            // gửi email
            #region gửi email
            var subject = "Gửi minh chứng nộp lệ phí";
            var body = "Thí sinh " + model.ThiSinhDangKy.ThiSinh_HoLot + model.ThiSinhDangKy.ThiSinh_Ten +
                "<br/> Số CCCD: " + model.ThiSinhDangKy.ThiSinh_CCCD +
                "<br/> Số đt: " + model.ThiSinhDangKy.ThiSinh_DienThoai +
                "<br/> đã cập nhật thông tin nộp lệ phí dự thi năng khiếu.";
            SendEmail s = new SendEmail();
            s.Sendemail("xettuyen@hdu.edu.vn", body, subject);
            #endregion
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DangKyDuThi_NangKhieu_UpdateMCLePhi(DangKyDuThiNangKhieu entity)
        {
            db = new DbConnecttion();
            var ts = db.DangKyDuThiNangKhieus.Include(x => x.ThiSinhDangKy).Where(x => x.Dkdt_NK_ID == entity.Dkdt_NK_ID).FirstOrDefault();

            ts.Dkdt_LePhi_MinhChung_Tep += entity.Dkdt_LePhi_MinhChung_Tep;
            ts.Dkdt_LePhi_MinhChung_MaThamChieu = entity.Dkdt_LePhi_MinhChung_MaThamChieu;
            ts.Dkdt_LePhi_MinhChung_NgayGui = DateTime.Now.ToString("yyyy-MM-dd");
            db.SaveChanges();

            // gửi email
            #region gửi email
            var subject = "Gửi minh chứng nộp lệ phí";
            var body = "Thí sinh " + ts.ThiSinhDangKy.ThiSinh_HoLot + ts.ThiSinhDangKy.ThiSinh_Ten +
                "<br/> Số CCCD: " + ts.ThiSinhDangKy.ThiSinh_CCCD +
                "<br/> Số đt: " + ts.ThiSinhDangKy.ThiSinh_DienThoai +
                "<br/> đã cập nhật thông tin nộp lệ phí dự thi năng khiếu.";
            SendEmail s = new SendEmail();
            s.Sendemail("xettuyen@hdu.edu.vn", body, subject);
            #endregion
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Đăng ký xét tuyển kết quả thi THPT quốc gia
        [ThiSinhSessionCheck]
        public ActionResult Dkxtthptqg()
        {
            return View();
        }
        [ThiSinhSessionCheck]
        public ActionResult DkxtthptqgEdit(int? dkxt_id)
        {
            ViewBag.Dkxt_ID = dkxt_id;
            return View();
        }
        [ThiSinhSessionCheck]
        public ActionResult DkxtthptqgDetail(int? dkxt_id)
        {
            ViewBag.Dkxt_ID = dkxt_id;
            return View();
        }
        [ThiSinhSessionCheck]
        public JsonResult DangKyXetTuyen_KQTHPTQG_ListAll()
        {
            db = new DbConnecttion();
            int ptxt_check = 2;
            var dxt_id_hientai = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault().Dxt_ID;


            if (Session["login_session"] != null)
            {
                string str_login_session = Session["login_session"].ToString();//  "$2a$11$jwPUP78RBpC9R3uW7Dqpau.SXwogLasbvVx3q0vqhoE93Lx044lcu";// 

                var tsdk_Detail = db.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault(ts => ts.ThiSinh_MatKhau == str_login_session);

                string _xeploai_hocluc_12 = "";
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 4) { _xeploai_hocluc_12 = "Xuất sắc"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 3) { _xeploai_hocluc_12 = "Giỏi"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 2) { _xeploai_hocluc_12 = "Khá"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 1) { _xeploai_hocluc_12 = "Trung bình"; }

                string _xeploai_hanhkiem_12 = "";
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 4) { _xeploai_hanhkiem_12 = "Tốt"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 3) { _xeploai_hanhkiem_12 = "Khá"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 2) { _xeploai_hanhkiem_12 = "Trung bình"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 1) { _xeploai_hanhkiem_12 = "Yếu"; }

                string _ut_doituong_ten_diem = "ĐT " + tsdk_Detail.DoiTuong.DoiTuong_Ten + ": ƯT " + tsdk_Detail.DoiTuong.DoiTuong_DiemUuTien + " đ";
                string _ut_khuvuv_ten_diem = tsdk_Detail.KhuVuc.KhuVuc_Ten + ": ƯT " + tsdk_Detail.KhuVuc.KhuVuc_DiemUuTien + " đ"; ;


                long _thisinh_id = tsdk_Detail.ThiSinh_ID;

                var list_dkxt_thptqg_ts = db.DangKyXetTuyenKQTQGs.
                                                    Include(d => d.ThiSinhDangKy).
                                                    Include(d => d.Nganh).
                                                    Include(d => d.ToHopMon).
                                                    Include(d => d.DotXetTuyen).
                                                    Where(x => x.ThiSinh_ID == _thisinh_id && x.Ptxt_ID == ptxt_check && x.DotXT_ID == dxt_id_hientai)
                                                    .OrderBy(x => x.Dkxt_KQTQG_NguyenVong).ToList();

                var view_list_dkxt_thptqg_ts = list_dkxt_thptqg_ts.Select(s => new
                {
                    dkxt_KQTQG_ID = s.Dkxt_KQTQG_ID,
                    thiSinh_ID = s.ThiSinh_ID,
                    ptxt_ID = s.Ptxt_ID,
                    nganh_ID = s.Nganh_ID,
                    thm_ID = s.Thm_ID,

                    dkxt_KQTQG_TrangThai_KinhPhi = s.Dkxt_KQTQG_TrangThai_KinhPhi,
                    dkxt_KQTQG_TrangThai_KetQua = s.Dkxt_KQTQG_TrangThai_KetQua,
                    dkxt_KQTQG_TrangThai_HoSo = s.Dkxt_KQTQG_TrangThai_HoSo,
                    dkxt_KQTQG_NguyenVong = s.Dkxt_KQTQG_NguyenVong,
                    dotXT_ID = s.DotXT_ID,
                    dkxt_KQTQG_TenAll = new
                    {
                        khoiNganh_Ten = s.Nganh.KhoiNganh.KhoiNganh_Ten,
                        nganh_GhiChu = s.Nganh.Nganh_MaNganh + " - " + s.Nganh.Nganh_TenNganh,
                        thm_MaTen = s.ToHopMon.Thm_MaToHop + " - " + s.ToHopMon.Thm_TenToHop,
                    },
                    Dkxt_NgayDK = s.Dkxt_KQTQG_NgayDangKy,
                    dkxt_KQTQG_Diem_M1 = s.Dkxt_KQTQG_Diem_M1,
                    dkxt_KQTQG_Diem_M2 = s.Dkxt_KQTQG_Diem_M2,
                    dkxt_KQTQG_Diem_M3 = s.Dkxt_KQTQG_Diem_M3,
                    dkxt_KQTQG_Diem_Tong = s.Dkxt_KQTQG_Diem_Tong,
                    dkxt_KQTQG_Diem_Tong_Full = s.Dkxt_KQTQG_TongDiem_Full,                    

                }).ToList();
                var Tsdk_doituong_ten_diem = _ut_doituong_ten_diem;
                var Tsdk_khuvuv_ten_diem = _ut_khuvuv_ten_diem;
                var Tsdk_xeploai_hocluc_12 = _xeploai_hocluc_12;
                var Tsdk_xeploai_hanhkiem_12 = _xeploai_hanhkiem_12;
                var Tsdk_id_dk = tsdk_Detail.ThiSinh_ID;
                return Json(new
                {
                    view_list_dkxt_thptqg_ts,
                    Tsdk_doituong_ten_diem,
                    Tsdk_khuvuv_ten_diem,
                    Tsdk_xeploai_hocluc_12,
                    Tsdk_xeploai_hanhkiem_12,
                    Tsdk_id_dk
                }, JsonRequestBehavior.AllowGet);;
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DangKyXetTuyen_KQTHPTQG_Get_Data_MonHoc(DangKyXetTuyenKQTQG entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenKQTQGs.Where(x => x.Nganh_ID == entity.Nganh_ID && x.Thm_ID == entity.Thm_ID && x.ThiSinh_ID == entity.ThiSinh_ID).ToList();
            if (model.Count > 0)
            {
                return Json(new { success = false, data = "TRUNG"}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int _thm_ID = int.Parse(entity.Thm_ID.ToString());
                var thm_get_item = db.ToHopMons.Find(_thm_ID);
                var thmObj = new
                {
                    thm_Mon1 = thm_get_item.Thm_Mon1,
                    thm_Mon2 = thm_get_item.Thm_Mon2,
                    thm_Mon3 = thm_get_item.Thm_Mon3
                };
                return Json(new { success = true, data = thmObj}, JsonRequestBehavior.AllowGet);
            }           
        }
        [ThiSinhSessionCheck]
        public JsonResult DangKyXetTuyen_KQTHPTQG_UploadFile_Multi()
        {
            db = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString(); //"$2a$11$jwPUP78RBpC9R3uW7Dqpau.SXwogLasbvVx3q0vqhoE93Lx044lcu"; //
            var ts = db.ThiSinhDangKies.FirstOrDefault(n => n.ThiSinh_MatKhau.Equals(str_login_session));
            string cccd = ts.ThiSinh_CCCD;

            int so_file_cn_totnghiep = int.Parse(Request["so_file_cn_totnghiep"].ToString());
            int so_file_hocba_ts = int.Parse(Request["so_file_hocba_ts"].ToString());
            int so_file_bang_tn = int.Parse(Request["so_file_bang_tn"].ToString());
            int so_file_cc_congdan = int.Parse(Request["so_file_cc_congdan"].ToString());
            int so_file_giayto_uutien = int.Parse(Request["so_file_giayto_uutien"].ToString());

            HttpFileCollectionBase files = Request.Files;

            string _KQTQG_MinhChung_CNTotNghiep = "";
            string _KQTQG_MinhChung_HocBa = "";
            string _KQTQG_MinhChung_BangTN = "";
            string _KQTQG_MinhChung_CCCD = "";
            string _KQTQG_MinhChung_UuTien = "";

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname;
                // Checking for Internet Explorer      
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + file.FileName;
                }
                // lấy chuỗi lưu vào csdl
                if (i < so_file_cn_totnghiep)
                {
                    _KQTQG_MinhChung_CNTotNghiep += "/Uploads/DKXTKQTTHPTQGFile/" + fname + "#";
                }
                if (i >= so_file_cn_totnghiep && i < so_file_hocba_ts + so_file_cn_totnghiep)
                {
                    _KQTQG_MinhChung_HocBa += "/Uploads/DKXTKQTTHPTQGFile/" + fname + "#";
                }

                if (i >= so_file_hocba_ts + so_file_cn_totnghiep && i < so_file_bang_tn + so_file_hocba_ts + so_file_cn_totnghiep)
                {
                    _KQTQG_MinhChung_BangTN += "/Uploads/DKXTKQTTHPTQGFile/" + fname + "#";
                }

                if (i >= so_file_bang_tn + so_file_hocba_ts + so_file_cn_totnghiep && i < so_file_cc_congdan + so_file_bang_tn + so_file_hocba_ts + so_file_cn_totnghiep)
                {
                    _KQTQG_MinhChung_CCCD += "/Uploads/DKXTKQTTHPTQGFile/" + fname + "#";
                }

                if (i >= so_file_cc_congdan + so_file_bang_tn + so_file_hocba_ts + so_file_cn_totnghiep && i < so_file_giayto_uutien + so_file_cc_congdan + so_file_bang_tn + so_file_hocba_ts + so_file_cn_totnghiep)
                {
                    _KQTQG_MinhChung_UuTien += "/Uploads/DKXTKQTTHPTQGFile/" + fname + "#";
                }

                // Get the complete folder path and store the file inside it.      
                fname = Path.Combine(Server.MapPath("~/Uploads/DKXTKQTTHPTQGFile/"), fname);
                file.SaveAs(fname);
            }
            return Json(new
            {
                _KQTQG_MinhChung_CNTotNghiep,
                _KQTQG_MinhChung_HocBa,
                _KQTQG_MinhChung_BangTN,
                _KQTQG_MinhChung_CCCD,
                _KQTQG_MinhChung_UuTien,

            }, JsonRequestBehavior.AllowGet);
        }
        [ThiSinhSessionCheck]
        public JsonResult DangKyXetTuyen_KQTHPTQG_GetByID(DangKyXetTuyenKQTQG entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenKQTQGs.Include(x => x.ThiSinhDangKy).Where(x => x.Dkxt_KQTQG_ID == entity.Dkxt_KQTQG_ID).FirstOrDefault();

            var khoinganh_id = db.Nganhs.Where(x => x.Nganh_ID == model.Nganh_ID).FirstOrDefault().KhoiNganh_ID;

            string _xeploai_hocluc_12 = "";
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 4) { _xeploai_hocluc_12 = "Xuất sắc"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 3) { _xeploai_hocluc_12 = "Giỏi"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 2) { _xeploai_hocluc_12 = "Khá"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 1) { _xeploai_hocluc_12 = "Trung bình"; }

            string _xeploai_hanhkiem_12 = "";
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 4) { _xeploai_hanhkiem_12 = "Tốt"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 3) { _xeploai_hanhkiem_12 = "Khá"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 2) { _xeploai_hanhkiem_12 = "Trung bình"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 1) { _xeploai_hanhkiem_12 = "Yếu"; }

            string _ut_doituong_ten_diem = model.ThiSinhDangKy.DoiTuong.DoiTuong_Ten + ": ƯT " + model.ThiSinhDangKy.DoiTuong.DoiTuong_DiemUuTien + " đ";
            string _ut_khuvuv_ten_diem = model.ThiSinhDangKy.KhuVuc.KhuVuc_Ten + ": ƯT " + model.ThiSinhDangKy.KhuVuc.KhuVuc_DiemUuTien + " đ"; ;

            MonDiemThiQG diemmon1 = JsonConvert.DeserializeObject<MonDiemThiQG>(model.Dkxt_KQTQG_Diem_M1);
            MonDiemThiQG diemmon2 = JsonConvert.DeserializeObject<MonDiemThiQG>(model.Dkxt_KQTQG_Diem_M2);
            MonDiemThiQG diemmon3 = JsonConvert.DeserializeObject<MonDiemThiQG>(model.Dkxt_KQTQG_Diem_M3);

            var data_return = new
            {
                KhuVuc_ID = _ut_khuvuv_ten_diem,
                DoiTuong_ID = _ut_doituong_ten_diem,
                ThiSinh_HocLucLop12 = _xeploai_hocluc_12,
                ThiSinh_HanhKiemLop12 = _xeploai_hanhkiem_12,

                Dkxt_KQTQG_ID = model.Dkxt_KQTQG_ID,
                ThiSinh_ID = model.ThiSinh_ID,
                DotXT_ID = model.DotXT_ID,
                Ptxt_ID = model.Ptxt_ID,

                Nganh_ID = model.Nganh_ID,
                Thm_ID = model.Thm_ID,
                KhoiNganh_ID = khoinganh_id,

                Dkxt_KQTQG_NamTotNghiep = model.Dkxt_KQTQG_NamTotNghiep,
                Dkxt_Diem_M1 = new { TenMon = diemmon1.TenMon, Diem = diemmon1.Diem, },
                Dkxt_Diem_M2 = new { TenMon = diemmon2.TenMon, Diem = diemmon2.Diem, },
                Dkxt_Diem_M3 = new { TenMon = diemmon3.TenMon, Diem = diemmon3.Diem, },

                Dkxt_KQTQG_Diem_Tong = model.Dkxt_KQTQG_Diem_Tong,
                Dkxt_KQTQG_TongDiem_Full = model.Dkxt_KQTQG_TongDiem_Full,
                Dkxt_KQTQG_NgayDangKy = model.Dkxt_KQTQG_NgayDangKy,

                Dkxt_KQTQG_MinhChung_CNTotNghiep = model.Dkxt_KQTQG_MinhChung_CNTotNghiep,
                Dkxt_KQTQG_MinhChung_HocBa = model.Dkxt_KQTQG_MinhChung_HocBa,
                Dkxt_KQTQG_MinhChung_BangTN = model.Dkxt_KQTQG_MinhChung_BangTN,
                Dkxt_KQTQG_MinhChung_CCCD = model.Dkxt_KQTQG_MinhChung_CCCD,
                Dkxt_KQTQG_MinhChung_UuTien = model.Dkxt_KQTQG_MinhChung_UuTien,

                Dkxt_KQTQG_KinhPhi_SoThamChieu = model.Dkxt_KQTQG_KinhPhi_SoThamChieu,
                Dkxt_KQTQG_KinhPhi_TepMinhChung = model.Dkxt_KQTQG_KinhPhi_TepMinhChung,
                Dkxt_KQTQG_KinhPhi_NgayThang_NopMC = model.Dkxt_KQTQG_KinhPhi_NgayThang_NopMC,
                Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC = model.Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC,

                Dkxt_KQTQG_TrangThai_KinhPhi = model.Dkxt_KQTQG_TrangThai_KinhPhi,
                Dkxt_KQTQG_TrangThai_HoSo = model.Dkxt_KQTQG_TrangThai_HoSo,
                Dkxt_KQTQG_TrangThai_KetQua = model.Dkxt_KQTQG_TrangThai_KetQua,

            };

            return Json(new { success = true, data = data_return }, JsonRequestBehavior.AllowGet);

        }
         
        [ThiSinhSessionCheck]
        public JsonResult DangKyXetTuyen_KQTHPTQG_Insert(DangKyXetTuyenKQTQG entity)
        {

            db = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString(); //"$2a$11$jwPUP78RBpC9R3uW7Dqpau.SXwogLasbvVx3q0vqhoE93Lx044lcu";// 

            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(str_login_session)).
                Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();


            var dotxettuyen = db.DotXetTuyens.Where(n => n.Dxt_Classify == 0 && n.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            if (ts != null)
            {


                var nvs = db.DangKyXetTuyenKQTQGs.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID && n.DotXT_ID == dotxettuyen.Dxt_ID).OrderByDescending(x => x.Dkxt_KQTQG_NguyenVong).ToList();

                long key_kp = 1;
                var sonv = db.DangKyXetTuyenKQTQGs.OrderByDescending(x => x.Dkxt_KQTQG_ID).FirstOrDefault();
                if (sonv != null) { key_kp = sonv.Dkxt_KQTQG_ID + 1; }

                DangKyXetTuyenKQTQG model = new DangKyXetTuyenKQTQG();

                model.Nganh_ID = entity.Nganh_ID;
                model.Thm_ID = entity.Thm_ID;
                model.Dkxt_KQTQG_NamTotNghiep = entity.Dkxt_KQTQG_NamTotNghiep;
                model.Dkxt_KQTQG_Diem_M1 = entity.Dkxt_KQTQG_Diem_M1;
                model.Dkxt_KQTQG_Diem_M2 = entity.Dkxt_KQTQG_Diem_M2;
                model.Dkxt_KQTQG_Diem_M3 = entity.Dkxt_KQTQG_Diem_M3;
                model.Dkxt_KQTQG_Diem_Tong = entity.Dkxt_KQTQG_Diem_Tong;

                model.Dkxt_KQTQG_MinhChung_CNTotNghiep = entity.Dkxt_KQTQG_MinhChung_CNTotNghiep;
                model.Dkxt_KQTQG_MinhChung_HocBa = entity.Dkxt_KQTQG_MinhChung_HocBa;
                model.Dkxt_KQTQG_MinhChung_BangTN = entity.Dkxt_KQTQG_MinhChung_BangTN;
                model.Dkxt_KQTQG_MinhChung_CCCD = entity.Dkxt_KQTQG_MinhChung_CCCD;
                model.Dkxt_KQTQG_MinhChung_UuTien = entity.Dkxt_KQTQG_MinhChung_UuTien;

                model.Dkxt_KQTQG_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd");

                model.Dkxt_KQTQG_TongDiem_Full = (double.Parse(entity.Dkxt_KQTQG_Diem_Tong.ToString()) +
                                                           double.Parse(ts.DoiTuong.DoiTuong_DiemUuTien.ToString()) +
                                                           double.Parse(ts.KhuVuc.KhuVuc_DiemUuTien.ToString())).ToString();

                model.ThiSinh_ID = ts.ThiSinh_ID;
                model.Ptxt_ID = 2;
                model.DotXT_ID = dotxettuyen.Dxt_ID;
                model.Dkxt_KQTQG_NguyenVong = nvs.Count + 1;

                model.Dkxt_KQTQG_GhiChu = ""; ;

                model.Dkxt_KQTQG_KinhPhi_SoThamChieu = "";
                model.Dkxt_KQTQG_KinhPhi_TepMinhChung = "";
                model.Dkxt_KQTQG_KinhPhi_NgayThang_NopMC = "";
                model.Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC = "";

                model.Dkxt_KQTQG_TrangThai_KinhPhi = 0;
                model.Dkxt_KQTQG_TrangThai_HoSo = 0;
                model.Dkxt_KQTQG_TrangThai_KetQua = 0;
                model.Dkxt_KinhPhi_NoiDungGiaoDich = "DKXT" + key_kp + " P2  NV" + model.Dkxt_KQTQG_NguyenVong + " " + ts.ThiSinh_CCCD + " Nộp lệ phí xét tuyển";
                db.DangKyXetTuyenKQTQGs.Add(model);
                db.SaveChanges();

                // gửi email
                #region gửi email
                DbConnecttion dbguimail = new DbConnecttion();
                int nganh_id = int.Parse(model.Nganh_ID.ToString());
                int thm_id = int.Parse(model.Thm_ID.ToString());
                var nganhdk = dbguimail.Nganhs.FirstOrDefault(n => n.Nganh_ID == nganh_id);
                var thmdk = dbguimail.ToHopMons.FirstOrDefault(t => t.Thm_ID == thm_id);

                var subject = "Đăng ký nguyện vọng";
                var body = "Thí sinh " + ts.ThiSinh_Ten + ", Số CCCD: " + ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +
                     " <br/><b>Thông tin nguyện vọng:</b><br/>" +
                     " <p>- Phương thức đăng ký: Xét tuyển bằng kết quả thi THPT quốc gia</p>" +
                     " <p>- Mã ngành, tên ngành: " + nganhdk.Nganh_MaNganh + nganhdk.Nganh_TenNganh + " </p>" +
                     " <p>- Tên tổ hợp môn: " + thmdk.Thm_MaTen + " </p>";
                SendEmail s = new SendEmail();
                s.Sendemail("xettuyen@hdu.edu.vn", body, subject);
                #endregion

                return Json(new { success = true, data = "DKXT" + model.Dkxt_KQTQG_ID + " P2  NV" + model.Dkxt_KQTQG_NguyenVong + " " + ts.ThiSinh_CCCD + " Nộp lệ phí xét tuyển" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, data = "LOI" }, JsonRequestBehavior.AllowGet);
            }

        }
        [ThiSinhSessionCheck]
        public JsonResult DangKyXetTuyen_KQTHPTQG_Edit(DangKyXetTuyenKQTQG entity)
        {

            db = new DbConnecttion();
            var model = db.DangKyXetTuyenKQTQGs.Where(x => x.Dkxt_KQTQG_ID == entity.Dkxt_KQTQG_ID).FirstOrDefault();
            if (model != null)
            {
                var model_ts = db.ThiSinhDangKies.Include(d => d.DoiTuong).Include(k => k.KhuVuc).Where(x => x.ThiSinh_ID == model.ThiSinh_ID).FirstOrDefault();

                model.Nganh_ID = entity.Nganh_ID;
                model.Thm_ID = entity.Thm_ID;
                model.Dkxt_KQTQG_NamTotNghiep = entity.Dkxt_KQTQG_NamTotNghiep;

                model.Dkxt_KQTQG_Diem_M1 = entity.Dkxt_KQTQG_Diem_M1;
                model.Dkxt_KQTQG_Diem_M2 = entity.Dkxt_KQTQG_Diem_M2;
                model.Dkxt_KQTQG_Diem_M3 = entity.Dkxt_KQTQG_Diem_M3;
                model.Dkxt_KQTQG_Diem_Tong = entity.Dkxt_KQTQG_Diem_Tong;

                model.Dkxt_KQTQG_MinhChung_CNTotNghiep += entity.Dkxt_KQTQG_MinhChung_CNTotNghiep;
                model.Dkxt_KQTQG_MinhChung_HocBa += entity.Dkxt_KQTQG_MinhChung_HocBa;
                model.Dkxt_KQTQG_MinhChung_BangTN += entity.Dkxt_KQTQG_MinhChung_BangTN;
                model.Dkxt_KQTQG_MinhChung_CCCD += entity.Dkxt_KQTQG_MinhChung_CCCD;
                model.Dkxt_KQTQG_MinhChung_UuTien += entity.Dkxt_KQTQG_MinhChung_UuTien;

                model.Dkxt_KQTQG_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd");

                model.Dkxt_KQTQG_TongDiem_Full = (double.Parse(model.Dkxt_KQTQG_Diem_Tong.ToString()) + double.Parse(model_ts.DoiTuong.DoiTuong_DiemUuTien.ToString()) + double.Parse(model_ts.KhuVuc.KhuVuc_DiemUuTien.ToString())).ToString();
                db.SaveChanges();
                // gửi email
                #region gửi email

                int nganh_id = int.Parse(model.Nganh_ID.ToString());
                int thm_id = int.Parse(model.Thm_ID.ToString());
                var nganhdk = db.Nganhs.FirstOrDefault(n => n.Nganh_ID == nganh_id);
                var thmdk = db.ToHopMons.FirstOrDefault(t => t.Thm_ID == thm_id);

                var subject = "Đăng ký nguyện vọng";
                var body = "Thí sinh " + model_ts.ThiSinh_Ten + ", Số CCCD: " + model_ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +
                     " <br/><b>Thông tin nguyện vọng:</b><br/>" +
                     " <p>- Phương thức đăng ký: Xét tuyển bằng kết quả thi THPT quốc gia</p>" +
                     " <p>- Mã ngành, tên ngành: " + nganhdk.Nganh_MaNganh + nganhdk.Nganh_TenNganh + " </p>" +
                     " <p>- Tên tổ hợp môn: " + thmdk.Thm_MaTen + " </p>";
                SendEmail send = new SendEmail();
                send.Sendemail("xettuyen@hdu.edu.vn", body, subject);
                #endregion

                return Json(new { success = true, data = "sửa thành công" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, data = "Không lấy được dữ liệu" }, JsonRequestBehavior.AllowGet);
            }

        }
        [ThiSinhSessionCheck]
        public JsonResult DangKyXetTuyen_KQTTHPTQG_Delete(DangKyXetTuyenKQTQG dkxt_kqtqg_post)
        {
            db = new DbConnecttion();
            int Dkxt_KQTQG_ID = int.Parse(dkxt_kqtqg_post.Dkxt_KQTQG_ID.ToString());
            DangKyXetTuyenKQTQG dkxt_item_getby_id = db.DangKyXetTuyenKQTQGs.Find(Dkxt_KQTQG_ID);
            int nv_current = int.Parse(dkxt_item_getby_id.Dkxt_KQTQG_NguyenVong.ToString());
            long idThisinh = long.Parse(dkxt_item_getby_id.ThiSinh_ID.ToString());

            db.DangKyXetTuyenKQTQGs.Remove(dkxt_item_getby_id);

            foreach (var item in db.DangKyXetTuyenKQTQGs.Where(nv => nv.Dkxt_KQTQG_NguyenVong > nv_current && nv.ThiSinh_ID == idThisinh).OrderBy(x => x.Dkxt_KQTQG_NguyenVong))
            {
                DangKyXetTuyenKQTQG dangKyXetTuyen_change = db.DangKyXetTuyenKQTQGs.FirstOrDefault(x => x.Dkxt_KQTQG_NguyenVong == item.Dkxt_KQTQG_NguyenVong && x.ThiSinh_ID == idThisinh);
                dangKyXetTuyen_change.Dkxt_KQTQG_NguyenVong = item.Dkxt_KQTQG_NguyenVong - 1;
            }

            db.SaveChanges();
            return Json(new
            {
                status = true,
                msg = "Xoá dữ liệu thành công"
            }, JsonRequestBehavior.AllowGet);
        }
        [ThiSinhSessionCheck]
        public JsonResult DangKyXetTuyen_KQTTHPTQG_Change_UpData(DangKyXetTuyenKQTQG entity)
        {
            db = new DbConnecttion();
            //  System.Diagnostics.Debug.WriteLine(dkxt_kqtqg_change.Dkxt_KQTQG_ID.ToString());
            long _dkxt_KQTQG_ID = long.Parse(entity.Dkxt_KQTQG_ID.ToString());

            DangKyXetTuyenKQTQG model_change = db.DangKyXetTuyenKQTQGs.Find(_dkxt_KQTQG_ID);

            int nv_current = int.Parse(model_change.Dkxt_KQTQG_NguyenVong.ToString());
            long idThisinh = long.Parse(model_change.ThiSinh_ID.ToString());

            model_change.Dkxt_KQTQG_NguyenVong = nv_current - 1;

            DangKyXetTuyenKQTQG dkxt_item_get_change = db.DangKyXetTuyenKQTQGs.FirstOrDefault(i => i.Dkxt_KQTQG_NguyenVong == nv_current - 1 && i.ThiSinh_ID == idThisinh);
            if (dkxt_item_get_change != null)
            {
                dkxt_item_get_change.Dkxt_KQTQG_NguyenVong = nv_current;
                db.SaveChanges();
                return Json(new { status = true, msg = "Thay đổi dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, msg = "Có lỗi xảy ra." }, JsonRequestBehavior.AllowGet);

        }
        [ThiSinhSessionCheck]
        public JsonResult DangKyXetTuyen_KQTTHPTQG_Change_DownData(DangKyXetTuyenKQTQG entity)
        {
            DbConnecttion db = new DbConnecttion();
            long _dkxt_KQTQG_ID = long.Parse(entity.Dkxt_KQTQG_ID.ToString());

            DangKyXetTuyenKQTQG model_get_byid = db.DangKyXetTuyenKQTQGs.Find(_dkxt_KQTQG_ID);

            int nv_current = int.Parse(model_get_byid.Dkxt_KQTQG_NguyenVong.ToString());
            int idThisinh = int.Parse(model_get_byid.ThiSinh_ID.ToString());

            model_get_byid.Dkxt_KQTQG_NguyenVong = nv_current + 1;

            DangKyXetTuyenKQTQG model_change = db.DangKyXetTuyenKQTQGs.FirstOrDefault(i => i.Dkxt_KQTQG_NguyenVong == nv_current + 1 && i.ThiSinh_ID == idThisinh);
            if (model_change != null)
            {
                model_change.Dkxt_KQTQG_NguyenVong = nv_current;
                db.SaveChanges();
                return Json(new { status = true, msg = "Thay đổi dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, msg = "Có lỗi xảy ra." }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DangKyXetTuyen_KQTTHPTQG_DeleteMC(ThongTinXoaMC entity)
        {
            db = new DbConnecttion();

            var model = db.DangKyXetTuyenKQTQGs.Where(x => x.Dkxt_KQTQG_ID == entity.Dkxt_ID).FirstOrDefault();
            // string modifiedString = originalString.Replace(stringToRemove, "");
            if (entity.Dkxt_LoaiMC == "1") { model.Dkxt_KQTQG_MinhChung_CNTotNghiep = model.Dkxt_KQTQG_MinhChung_CNTotNghiep.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "2") { model.Dkxt_KQTQG_MinhChung_HocBa = model.Dkxt_KQTQG_MinhChung_HocBa.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "3") { model.Dkxt_KQTQG_MinhChung_BangTN = model.Dkxt_KQTQG_MinhChung_BangTN.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "4") { model.Dkxt_KQTQG_MinhChung_CCCD = model.Dkxt_KQTQG_MinhChung_CCCD.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "5") { model.Dkxt_KQTQG_MinhChung_UuTien = model.Dkxt_KQTQG_MinhChung_UuTien.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "6") { model.Dkxt_KQTQG_KinhPhi_TepMinhChung = model.Dkxt_KQTQG_KinhPhi_TepMinhChung.Replace(entity.Dkxt_Url + "#", ""); }
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Đăng ký xét tuyển học bạ

        [ThiSinhSessionCheck]
        public ActionResult Dkxthocba()
        {
            return View();
        }
        public ActionResult DkxthocbaEdit(int? dkxt_id)
        {
            ViewBag.Dkxt_ID = dkxt_id;
            return View();
        }
        public ActionResult DkxthocbaDetail(int? dkxt_id)
        {
            ViewBag.Dkxt_ID = dkxt_id;
            return View();
        }
        public JsonResult DangKyXetTuyen_HB_ListAll()
        {
            db = new DbConnecttion();
            int ptxt_check = 3;
            var dxt_id_hientai = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault().Dxt_ID;
            if (Session["login_session"] != null)
            {

                string str_login_session = Session["login_session"].ToString();
                //ViewBag.str_login_session = str_login_session;
                var tsdk_Detail = db.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault(ts => ts.ThiSinh_MatKhau.Equals(str_login_session));


                string _xeploai_hocluc_12 = "";

                if (tsdk_Detail.ThiSinh_HocLucLop12 == 4) { _xeploai_hocluc_12 = "Xuất sắc"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 3) { _xeploai_hocluc_12 = "Giỏi"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 2) { _xeploai_hocluc_12 = "Khá"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 1) { _xeploai_hocluc_12 = "Trung bình"; }

                string _xeploai_hanhkiem_12 = "";
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 4) { _xeploai_hanhkiem_12 = "Tốt"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 3) { _xeploai_hanhkiem_12 = "Khá"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 2) { _xeploai_hanhkiem_12 = "Trung bình"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 1) { _xeploai_hanhkiem_12 = "Yếu"; }

                string _ut_doituong_ten_diem = tsdk_Detail.DoiTuong.DoiTuong_Ten + ": ƯT " + tsdk_Detail.DoiTuong.DoiTuong_DiemUuTien + " đ";
                string _ut_khuvuv_ten_diem = tsdk_Detail.KhuVuc.KhuVuc_Ten + ": ƯT " + tsdk_Detail.KhuVuc.KhuVuc_DiemUuTien + " đ"; ;


                long _thisinh_id = tsdk_Detail.ThiSinh_ID;

                var dkxt_Detail_list = db.DangKyXetTuyenHBs.
                                               Include(d => d.ThiSinhDangKy).
                                               Include(d => d.Nganh).
                                               Include(d => d.ToHopMon).
                                               Include(d => d.DotXetTuyen).
                                               Include(d => d.ThiSinhDangKy.DoiTuong).
                                               Include(d => d.ThiSinhDangKy.KhuVuc).
                                               Include(d => d.PhuongThucXetTuyen).
                                               Where(x => x.ThiSinh_ID == _thisinh_id && x.Ptxt_ID == ptxt_check && x.DotXT_ID == dxt_id_hientai).OrderBy(x => x.Dkxt_HB_NguyenVong).ToList();
                var select_list_dkxt_model = dkxt_Detail_list.Select(s => new
                {
                    dkxt_ID = s.Dkxt_HB_ID,
                    thiSinh_ID = s.ThiSinh_ID,
                    ptxt_ID = s.Ptxt_ID,
                    nganh_ID = s.Nganh_ID,
                    thm_ID = s.Thm_ID,

                    dkxt_HB_TrangThai_KinhPhi = s.Dkxt_HB_TrangThai_KinhPhi,
                    dkxt_HB_TrangThai_HoSo = s.Dkxt_HB_TrangThai_HoSo,
                    dkxt_HB_TrangThai_KetQua = s.Dkxt_HB_TrangThai_KetQua,
                    dkxt_NguyenVong = s.Dkxt_HB_NguyenVong,
                    dotXT_ID = s.DotXT_ID,
                    dkxt_TenAll = new
                    {
                        khoiNganh_Ten = s.Nganh.KhoiNganh.KhoiNganh_Ten,
                        nganh_GhiChu = s.Nganh.Nganh_MaNganh +" - " + s.Nganh.Nganh_TenNganh,
                        thm_MaTen = s.ToHopMon.Thm_MaToHop +" - " + s.ToHopMon.Thm_TenToHop,
                    },
                    Dkxt_NgayDK  = s.Dkxt_HB_NgayDangKy,
                    dkxt_Diem_M1 = s.Dkxt_HB_Diem_M1,
                    dkxt_Diem_M2 = s.Dkxt_HB_Diem_M2,
                    dkxt_Diem_M3 = s.Dkxt_HB_Diem_M3,
                    dkxt_Diem_Tong = s.Dkxt_HB_Diem_Tong,
                    dkxt_Diem_Tong_Full = s.Dkxt_HB_Diem_Tong_Full,

                }).ToList();

                var Dkxt_doituong_ten_diem = _ut_doituong_ten_diem;
                var Dkxt_khuvuv_ten_diem = _ut_khuvuv_ten_diem;
                var Dkxt_xeploai_hocluc_12 = _xeploai_hocluc_12;
                var Dkxt_xeploai_hanhkiem_12 = _xeploai_hanhkiem_12;
                var Tsdk_id_dk = tsdk_Detail.ThiSinh_ID;
                return Json(new
                {
                    select_list_dkxt_model,
                    Dkxt_doituong_ten_diem,
                    Dkxt_khuvuv_ten_diem,
                    Dkxt_xeploai_hocluc_12,
                    Dkxt_xeploai_hanhkiem_12,
                    Tsdk_id_dk

                }, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_HB_Get_Data_MonHoc(DangKyXetTuyenHB entity)
        {
            int _dkxt_ID = int.Parse(entity.Dkxt_HB_ID.ToString());
            db = new DbConnecttion();

            var model = db.DangKyXetTuyenHBs.Where(x => x.Nganh_ID == entity.Nganh_ID && x.Thm_ID == entity.Thm_ID && x.ThiSinh_ID == entity.ThiSinh_ID).ToList();
            if (model.Count > 0)
            {
                return Json(new { success = false, data = "TRUNG" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int _thm_ID = int.Parse(entity.Thm_ID.ToString());
                var thm_get_item = db.ToHopMons.Find(_thm_ID);
                var thmObj = new
                {
                    thm_Mon1 = thm_get_item.Thm_Mon1,
                    thm_Mon2 = thm_get_item.Thm_Mon2,
                    thm_Mon3 = thm_get_item.Thm_Mon3
                };
                return Json(new { success = true, data = thmObj }, JsonRequestBehavior.AllowGet);
            } 
        }
        public JsonResult DangKyXetTuyen_HB_Get_Data_MonHoc1(DangKyXetTuyenHB entity)
        {

            int _thm_ID = int.Parse(entity.Thm_ID.ToString());
            int _dkxt_ID = int.Parse(entity.Dkxt_HB_ID.ToString());
            db = new DbConnecttion();


            if (_dkxt_ID == 0 && _thm_ID > 0)// trường hợp lấy dữ liệu tên môn học cho thêm mới
            {
                var thm_get_item = db.ToHopMons.Find(_thm_ID);
                var thmObj = new
                {
                    thm_Mon1 = thm_get_item.Thm_Mon1,
                    thm_Mon2 = thm_get_item.Thm_Mon2,
                    thm_Mon3 = thm_get_item.Thm_Mon3
                };

                return Json(thmObj, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_HB_UploadFile_Multi()
        {
            db = new DbConnecttion();

            var session = Session["login_session"].ToString();
            var ts = db.ThiSinhDangKies.FirstOrDefault(n => n.ThiSinh_MatKhau.Equals(session));
            string cccd = ts.ThiSinh_CCCD;

            int so_file_hb = int.Parse(Request["so_file_hb"].ToString());
            int so_file_btn = int.Parse(Request["so_file_btn"].ToString());
            int so_file_cccd = int.Parse(Request["so_file_cccd"].ToString());
            int so_file_gtut = int.Parse(Request["so_file_gtut"].ToString());

            HttpFileCollectionBase files = Request.Files;

            string MinhChung_HB = "";
            string MinhChung_Bang = "";
            string MinhChung_CCCD = "";
            string MinhChung_UuTien = "";

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname;
                // Checking for Internet Explorer      
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + file.FileName;
                }
                // lấy chuỗi lưu vào csdl
                if (i < so_file_hb)
                {
                    MinhChung_HB += "/Uploads/DKXTHocBaFile/" + fname + "#";
                }
                if (i >= so_file_hb && i < so_file_hb + so_file_cccd)
                {
                    MinhChung_CCCD += "/Uploads/DKXTHocBaFile/" + fname + "#";
                }

                if (i >= so_file_hb + so_file_cccd && i < so_file_btn + so_file_hb + so_file_cccd)
                {
                    MinhChung_Bang += "/Uploads/DKXTHocBaFile/" + fname + "#";
                }
                if (i >= so_file_btn + so_file_hb + so_file_cccd && i < so_file_gtut + so_file_btn + so_file_hb + so_file_cccd)
                {
                    MinhChung_UuTien += "/Uploads/DKXTHocBaFile/" + fname + "#";
                }

                // Get the complete folder path and store the file inside it.      
                fname = Path.Combine(Server.MapPath("~/Uploads/DKXTHocBaFile/"), fname);
                file.SaveAs(fname);

            }
            return Json(new
            {
                MinhChung_HB,
                MinhChung_CCCD,
                MinhChung_Bang,
                MinhChung_UuTien,

            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_HB_GetByID(DangKyXetTuyenHB entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenHBs.Include(x => x.ThiSinhDangKy).Where(x => x.Dkxt_HB_ID == entity.Dkxt_HB_ID).FirstOrDefault();
           
            var NganhDK = db.Nganhs.Where(x => x.Nganh_ID == model.Nganh_ID).FirstOrDefault();
            var KhoiNganhDK = db.KhoiNganhs.Where(x => x.KhoiNganh_ID == NganhDK.KhoiNganh_ID).FirstOrDefault();
            var ToHopMonDK = db.ToHopMons.Where(x => x.Thm_ID == model.Thm_ID).FirstOrDefault();

            string _xeploai_hocluc_12 = "";

            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 4) { _xeploai_hocluc_12 = "Xuất sắc"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 3) { _xeploai_hocluc_12 = "Giỏi"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 2) { _xeploai_hocluc_12 = "Khá"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 1) { _xeploai_hocluc_12 = "Trung bình"; }

            string _xeploai_hanhkiem_12 = "";
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 4) { _xeploai_hanhkiem_12 = "Tốt"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 3) { _xeploai_hanhkiem_12 = "Khá"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 2) { _xeploai_hanhkiem_12 = "Trung bình"; }
            if (model.ThiSinhDangKy.ThiSinh_HocLucLop12 == 1) { _xeploai_hanhkiem_12 = "Yếu"; }

            string _ut_doituong_ten_diem = model.ThiSinhDangKy.DoiTuong.DoiTuong_Ten + ": ƯT " + model.ThiSinhDangKy.DoiTuong.DoiTuong_DiemUuTien + " đ";
            string _ut_khuvuv_ten_diem = model.ThiSinhDangKy.KhuVuc.KhuVuc_Ten + ": ƯT " + model.ThiSinhDangKy.KhuVuc.KhuVuc_DiemUuTien + " đ"; ;



            MonDiemHB diemmon1 = JsonConvert.DeserializeObject<MonDiemHB>(model.Dkxt_HB_Diem_M1);
            MonDiemHB diemmon2 = JsonConvert.DeserializeObject<MonDiemHB>(model.Dkxt_HB_Diem_M2);
            MonDiemHB diemmon3 = JsonConvert.DeserializeObject<MonDiemHB>(model.Dkxt_HB_Diem_M3);

            var data_return = new
            {
                KhuVuc_ID = _ut_khuvuv_ten_diem,
                DoiTuong_ID = _ut_doituong_ten_diem,
                ThiSinh_HocLucLop12 = _xeploai_hocluc_12,
                ThiSinh_HanhKiemLop12 = _xeploai_hanhkiem_12,

                Dkxt_HB_ID = model.Dkxt_HB_ID,
                ThiSinh_ID = model.ThiSinh_ID,
                DotXT_ID = model.DotXT_ID,
                Ptxt_ID = model.Ptxt_ID,

                Nganh_ID = NganhDK.Nganh_ID,
                Nganh_TeNganh = NganhDK.Nganh_TenNganh,
                Thm_ID = ToHopMonDK.Thm_ID,
                Thm_Ten = ToHopMonDK.Thm_TenToHop,
                KhoiNganh_ID = KhoiNganhDK.KhoiNganh_ID,
                KhoiNganh_Ten = KhoiNganhDK.KhoiNganh_Ten,

                Dkxt_HB_NguyenVong = model.Dkxt_HB_NguyenVong,
                Dkxt_HB_GhiChu = model.Dkxt_HB_GhiChu,

                Dkxt_Diem_M1 = new { TenMon = diemmon1.TenMon, HK1 = diemmon1.HK1, HK2 = diemmon1.HK2, HK3 = diemmon1.HK3, DTB = diemmon1.DiemTrungBinh, },
                Dkxt_Diem_M2 = new { TenMon = diemmon2.TenMon, HK1 = diemmon2.HK1, HK2 = diemmon2.HK2, HK3 = diemmon2.HK3, DTB = diemmon2.DiemTrungBinh, },
                Dkxt_Diem_M3 = new { TenMon = diemmon3.TenMon, HK1 = diemmon3.HK1, HK2 = diemmon3.HK2, HK3 = diemmon3.HK3, DTB = diemmon3.DiemTrungBinh, },

                Dkxt_HB_Diem_Tong = model.Dkxt_HB_Diem_Tong,
                Dkxt_HB_Diem_Tong_Full = model.Dkxt_HB_Diem_Tong_Full,
                Dkxt_HB_NgayDangKy = model.Dkxt_HB_NgayDangKy,

                Dkxt_HB_MinhChung_HB = model.Dkxt_HB_MinhChung_HB,
                Dkxt_HB_MinhChung_Bang = model.Dkxt_HB_MinhChung_Bang,
                Dkxt_HB_MinhChung_CCCD = model.Dkxt_HB_MinhChung_CCCD,
                Dkxt_HB_MinhChung_UuTien = model.Dkxt_HB_MinhChung_UuTien,

                Dkxt_HB_KinhPhi_SoThamChieu = model.Dkxt_HB_KinhPhi_SoThamChieu,
                Dkxt_HB_KinhPhi_TepMinhChung = model.Dkxt_HB_KinhPhi_TepMinhChung,
                Dkxt_HB_KinhPhi_NgayThang_NopMC = model.Dkxt_HB_KinhPhi_NgayThang_NopMC,
                Dkxt_HB_KinhPhi_NgayThang_CheckMC = model.Dkxt_HB_KinhPhi_NgayThang_CheckMC,

                Dkxt_HB_TrangThai_KinhPhi = model.Dkxt_HB_TrangThai_KinhPhi,
                Dkxt_HB_TrangThai_HoSo = model.Dkxt_HB_TrangThai_HoSo,
                Dkxt_HB_TrangThai_KetQua = model.Dkxt_HB_TrangThai_KetQua,
            };
            return Json(new { success = true, data = data_return }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_HB_Create(DangKyXetTuyenHB entity)
        {
            db = new DbConnecttion();

            var session = Session["login_session"].ToString();
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).
                Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();
            var dotxettuyen = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            if (ts != null)
            {
                var nvs = db.DangKyXetTuyenHBs.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID).OrderByDescending(x => x.Dkxt_HB_NguyenVong).ToList();
                long key_kp = 1;
                var sonv = db.DangKyXetTuyenHBs.OrderByDescending(x => x.Dkxt_HB_ID).FirstOrDefault();
                if (sonv != null) { key_kp = sonv.Dkxt_HB_ID + 1; }

                DangKyXetTuyenHB model_new = new DangKyXetTuyenHB();

                model_new.Nganh_ID = entity.Nganh_ID;
                model_new.Thm_ID = entity.Thm_ID;
                var diemTong = entity.Dkxt_HB_Diem_Tong.ToString();

                model_new.Dkxt_HB_Diem_M1 = entity.Dkxt_HB_Diem_M1;
                model_new.Dkxt_HB_Diem_M2 = entity.Dkxt_HB_Diem_M2;
                model_new.Dkxt_HB_Diem_M3 = entity.Dkxt_HB_Diem_M3;

                model_new.Dkxt_HB_Diem_Tong = entity.Dkxt_HB_Diem_Tong;

                model_new.Dkxt_HB_MinhChung_HB = entity.Dkxt_HB_MinhChung_HB;
                model_new.Dkxt_HB_MinhChung_CCCD = entity.Dkxt_HB_MinhChung_CCCD;
                model_new.Dkxt_HB_MinhChung_Bang = entity.Dkxt_HB_MinhChung_Bang;
                model_new.Dkxt_HB_MinhChung_UuTien = entity.Dkxt_HB_MinhChung_UuTien;

                model_new.ThiSinh_ID = ts.ThiSinh_ID;

                model_new.DotXT_ID = dotxettuyen.Dxt_ID;

                model_new.Dkxt_HB_KinhPhi_NgayThang_CheckMC = "";
                model_new.Dkxt_HB_KinhPhi_NgayThang_NopMC = "";
                model_new.Dkxt_HB_KinhPhi_SoThamChieu = "";
                model_new.Dkxt_HB_KinhPhi_TepMinhChung = "";

                model_new.Dkxt_HB_TrangThai_KetQua = 0;
                model_new.Dkxt_HB_TrangThai_HoSo = 0;
                model_new.Dkxt_HB_TrangThai_KinhPhi = 0;

                var diemDoiTuong = ts.DoiTuong.DoiTuong_DiemUuTien;
                var khuVucDoiTuong = ts.KhuVuc.KhuVuc_DiemUuTien;

                var diemTongFull = double.Parse(diemTong) + double.Parse(diemDoiTuong.ToString()) + double.Parse(khuVucDoiTuong.ToString());

                model_new.Dkxt_HB_Diem_Tong_Full = diemTongFull.ToString();
                model_new.Dkxt_HB_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd");
                model_new.Ptxt_ID = 3;

                model_new.Dkxt_HB_NguyenVong = nvs.Count + 1;
                model_new.Dkxt_KinhPhi_NoiDungGiaoDich = "DKXT" + key_kp + " P3  NV" + model_new.Dkxt_HB_NguyenVong + " " + ts.ThiSinh_CCCD + " Nộp lệ phí xét tuyển";

                db.DangKyXetTuyenHBs.Add(model_new);
                db.SaveChanges();

                #region gửi email

                int nganh_id = int.Parse(model_new.Nganh_ID.ToString());
                int thm_id = int.Parse(model_new.Thm_ID.ToString());
                var nganhdk = db.Nganhs.FirstOrDefault(n => n.Nganh_ID == nganh_id);
                var thmdk = db.ToHopMons.FirstOrDefault(t => t.Thm_ID == thm_id);

                var subject = "Đăng ký nguyện vọng";
                var body = "Thí sinh " + ts.ThiSinh_Ten + ", Số CCCD: " + ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +

                     " <br/><b>Thông tin nguyện vọng:</b><br/>" +
                     " <p>- Phương thức đăng ký: Xét tuyển bằng học bạ </p>" +
                     " <p>- Mã ngành: " + nganhdk.Nganh_MaNganh + " </p>" +
                     " <p>- Tên ngành: " + nganhdk.Nganh_TenNganh + " </p>" +
                     " <p>- Tên tổ hợp môn: " + thmdk.Thm_MaTen + " </p>";

                SendEmail s = new SendEmail();
                s.Sendemail("xettuyen@hdu.edu.vn", body, subject);

                #endregion
                return Json(new { success = true, data = "DKXT" + model_new.Dkxt_HB_ID + " P2  NV" + model_new.Dkxt_HB_NguyenVong + " " + ts.ThiSinh_CCCD + " Nộp lệ phí xét tuyển" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, data = "LOI" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DangKyXetTuyen_HB_Edit(DangKyXetTuyenHB entity)
        {
            db = new DbConnecttion();

            var session = Session["login_session"].ToString();

            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).
                Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var dotxettuyen = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            if (ts != null)
            {
                var model_edit = db.DangKyXetTuyenHBs.Include(x => x.Nganh).Include(x => x.DotXetTuyen).Where(x => x.Dkxt_HB_ID == entity.Dkxt_HB_ID).FirstOrDefault();

                model_edit.Nganh_ID = entity.Nganh_ID;
                model_edit.Thm_ID = entity.Thm_ID;
                var diemTong = entity.Dkxt_HB_Diem_Tong.ToString();

                model_edit.Dkxt_HB_Diem_M1 = entity.Dkxt_HB_Diem_M1;
                model_edit.Dkxt_HB_Diem_M2 = entity.Dkxt_HB_Diem_M2;
                model_edit.Dkxt_HB_Diem_M3 = entity.Dkxt_HB_Diem_M3;
                model_edit.Dkxt_HB_Diem_Tong = entity.Dkxt_HB_Diem_Tong;

                model_edit.Dkxt_HB_MinhChung_HB += entity.Dkxt_HB_MinhChung_HB;
                model_edit.Dkxt_HB_MinhChung_CCCD += entity.Dkxt_HB_MinhChung_CCCD;
                model_edit.Dkxt_HB_MinhChung_Bang += entity.Dkxt_HB_MinhChung_Bang;
                model_edit.Dkxt_HB_MinhChung_UuTien += entity.Dkxt_HB_MinhChung_UuTien;

                model_edit.Dkxt_HB_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd");

                model_edit.ThiSinh_ID = ts.ThiSinh_ID;
                model_edit.DotXT_ID = dotxettuyen.Dxt_ID;

                var diemDoiTuong = ts.DoiTuong.DoiTuong_DiemUuTien;
                var khuVucDoiTuong = ts.KhuVuc.KhuVuc_DiemUuTien;

                var diemTongFull = double.Parse(diemTong) + double.Parse(diemDoiTuong.ToString()) + double.Parse(khuVucDoiTuong.ToString());
                model_edit.Dkxt_HB_Diem_Tong_Full = diemTongFull.ToString();

                db.SaveChanges();

                #region gửi email
                int nganh_id = int.Parse(model_edit.Nganh_ID.ToString());
                int thm_id = int.Parse(model_edit.Thm_ID.ToString());

                var nganhdk = db.Nganhs.FirstOrDefault(n => n.Nganh_ID == nganh_id);
                var thmdk = db.ToHopMons.FirstOrDefault(t => t.Thm_ID == thm_id);

                var subject = "Đăng ký nguyện vọng";
                var body = "Thí sinh " + ts.ThiSinh_Ten + ", Số CCCD: " + ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +
                     " <br/><b>Thông tin nguyện vọng:</b><br/>" +
                     " <p>- Phương thức đăng ký: Xét tuyển bằng học bạ </p>" +
                     " <p>- Mã ngành: " + nganhdk.Nganh_MaNganh + " </p>" +
                     " <p>- Tên ngành: " + nganhdk.Nganh_TenNganh + " </p>" +
                     " <p>- Tên tổ hợp môn: " + thmdk.Thm_MaTen + " </p>";
                SendEmail s = new SendEmail();
                s.Sendemail("xettuyen@hdu.edu.vn", body, subject);
                #endregion
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_HB_Delete(DangKyXetTuyenHB entity)
        {

            db = new DbConnecttion();

            long _dkxt_ID = long.Parse(entity.Dkxt_HB_ID.ToString());

            DangKyXetTuyenHB model_delete = db.DangKyXetTuyenHBs.FirstOrDefault(x => x.Dkxt_HB_ID == _dkxt_ID);

            int nv_current = int.Parse(model_delete.Dkxt_HB_NguyenVong.ToString());
            long idThisinh = long.Parse(model_delete.ThiSinh_ID.ToString());

            db.DangKyXetTuyenHBs.Remove(model_delete);

            foreach (var item in db.DangKyXetTuyenHBs.Where(nv => nv.Dkxt_HB_NguyenVong > nv_current && nv.ThiSinh_ID == idThisinh).OrderBy(x => x.Dkxt_HB_NguyenVong))
            {
                DangKyXetTuyenHB _dkxt_item_change = db.DangKyXetTuyenHBs.FirstOrDefault(i => i.Dkxt_HB_NguyenVong == item.Dkxt_HB_NguyenVong && i.ThiSinh_ID == idThisinh);
                _dkxt_item_change.Dkxt_HB_NguyenVong = item.Dkxt_HB_NguyenVong - 1;
            }

            db.SaveChanges();

            return Json(new
            {
                status = true,
                msg = "Xoá dữ liệu thành công"
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DangKyXetTuyen_HB_DeleteMC(ThongTinXoaMC entity)
        {
            db = new DbConnecttion();

            var model = db.DangKyXetTuyenHBs.Where(x => x.Dkxt_HB_ID == entity.Dkxt_ID).FirstOrDefault();
            // string modifiedString = originalString.Replace(stringToRemove, "");
            if (entity.Dkxt_LoaiMC == "2") { model.Dkxt_HB_MinhChung_HB = model.Dkxt_HB_MinhChung_HB.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "3") { model.Dkxt_HB_MinhChung_Bang = model.Dkxt_HB_MinhChung_Bang.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "4") { model.Dkxt_HB_MinhChung_CCCD = model.Dkxt_HB_MinhChung_CCCD.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "5") { model.Dkxt_HB_MinhChung_UuTien = model.Dkxt_HB_MinhChung_UuTien.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "6") { model.Dkxt_HB_KinhPhi_TepMinhChung = model.Dkxt_HB_KinhPhi_TepMinhChung.Replace(entity.Dkxt_Url + "#", ""); }

            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_HB_Change_UpData(DangKyXetTuyenHB entity)
        {
            db = new DbConnecttion();
            //  System.Diagnostics.Debug.WriteLine(dkxt_kqtqg_change.Dkxt_KQTQG_ID.ToString());
            long _dkxt_ID = long.Parse(entity.Dkxt_HB_ID.ToString());

            DangKyXetTuyenHB model_change = db.DangKyXetTuyenHBs.Find(_dkxt_ID);

            int nv_current = int.Parse(model_change.Dkxt_HB_NguyenVong.ToString());
            int idThisinh = int.Parse(model_change.ThiSinh_ID.ToString());

            model_change.Dkxt_HB_NguyenVong = nv_current - 1;

            DangKyXetTuyenHB dkxt_item_get_change = db.DangKyXetTuyenHBs.FirstOrDefault(i => i.Dkxt_HB_NguyenVong == nv_current - 1 && i.ThiSinh_ID == idThisinh);
            if (dkxt_item_get_change != null)
            {
                dkxt_item_get_change.Dkxt_HB_NguyenVong = nv_current;
                db.SaveChanges();
                return Json(new { status = true, msg = "Thay đổi dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, msg = "Có lỗi xảy ra." }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_HB_Change_DownData(DangKyXetTuyenHB entity)
        {

            db = new DbConnecttion();
            //  System.Diagnostics.Debug.WriteLine(dkxt_kqtqg_change.Dkxt_KQTQG_ID.ToString());
            long _dkxt_ID = long.Parse(entity.Dkxt_HB_ID.ToString());

            DangKyXetTuyenHB model_change = db.DangKyXetTuyenHBs.Find(_dkxt_ID);

            int nv_current = int.Parse(model_change.Dkxt_HB_NguyenVong.ToString());
            int idThisinh = int.Parse(model_change.ThiSinh_ID.ToString());

            model_change.Dkxt_HB_NguyenVong = nv_current + 1;

            DangKyXetTuyenHB dkxt_item_get_change = db.DangKyXetTuyenHBs.FirstOrDefault(i => i.Dkxt_HB_NguyenVong == nv_current + 1 && i.ThiSinh_ID == idThisinh);
            if (dkxt_item_get_change != null)
            {
                dkxt_item_get_change.Dkxt_HB_NguyenVong = nv_current;
                db.SaveChanges();
                return Json(new { status = true, msg = "Thay đổi dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, msg = "Có lỗi xảy ra." }, JsonRequestBehavior.AllowGet);

        }


        #endregion
        #region lấy dữ liệu ra dropdown list
        public JsonResult KhoiNganhListAll()
        {
            db = new DbConnecttion();
            var selectResult_KhoiNganh = db.KhoiNganhs.Select(s => new
            {
                khoiNganh_ID = s.KhoiNganh_ID,
                khoiNganh_Ten = s.KhoiNganh_Ten
            });
            return Json(selectResult_KhoiNganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult NganhListAll(int id)
        {
            db = new DbConnecttion();

            var selectResult_Nganh = db.Nganhs.Where(x => x.KhoiNganh_ID == id || x.Nganh_ID == 0).OrderBy(x => x.Nganh_ID).Select(s => new
            {
                nganh_ID = s.Nganh_ID,
                nganh_GhiChu = s.Nganh_TenNganh,
                khoiNganh_ID = s.KhoiNganh_ID
            });
            return Json(selectResult_Nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ToHopMonNganhListAll(int id)
        {
            db = new DbConnecttion();
            var selectResult_tohopmon_nganh = db.ToHopMonNganhs.Include(n => n.ToHopMon).Where(x => x.Nganh_ID == id || x.ToHopMon.Thm_ID == 0).OrderBy(x => x.ToHopMon.Thm_ID).Select(s => new
            {
                thm_ID = s.Thm_ID,
                thm_MaTen = s.ToHopMon.Thm_MaTen,
                thm_TenToHop = s.ToHopMon.Thm_TenToHop
            });
            return Json(selectResult_tohopmon_nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region test login
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registers()
        {
            return View();
        }
        #endregion

    }
}