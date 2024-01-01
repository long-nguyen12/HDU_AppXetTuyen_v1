using HDU_AppXetTuyen.Models;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace HDU_AppXetTuyen.Controllers
{

    public class DangKyXetTuyenThangController : Controller
    {
        private DbConnecttion db = null;

        public int KiemTraThoiGian()
        {
            int set_check = -1;
            db = new DbConnecttion();
            var CurrentColleger = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            DateTime DateStart = DateTime.Parse(CurrentColleger.Dxt_ThoiGian_BatDau);
            DateTime DateEnd = DateTime.Parse(CurrentColleger.Dxt_ThoiGian_KetThuc);
            DateTime DatePresent = DateTime.Now;
            if (DateStart > DatePresent || DateEnd < DatePresent) { set_check = 0; }
            else if ((DateStart < DatePresent) && (DateEnd > DatePresent)) { set_check = 1; }
            return set_check;
        }

        // GET: DangKyXetTuyenThang
        [ThiSinhSessionCheck]
        public ActionResult Index()
        {
            return View();
        }
        [ThiSinhSessionCheck]
        public JsonResult GetAllNguyenVongs()
        {
            db = new DbConnecttion();

            var session = Session["login_session"];
            var dotXT = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_HocLucLop12 = n.ThiSinh_HocLucLop12,
                ThiSinh_HanhKiemLop12 = n.ThiSinh_HanhKiemLop12,

                ThiSinh_UTDT = n.DoiTuong.DoiTuong_Ten + ", Ưu tiên: " + n.DoiTuong.DoiTuong_DiemUuTien,
                ThiSinh_UTKV = n.KhuVuc.KhuVuc_Ten + ", Ưu tiên: " + n.KhuVuc.KhuVuc_DiemUuTien,
            }).FirstOrDefault();

            var nguyenvongs = db.DangKyXetTuyenThangs.Include(d => d.Nganh).Where(n => n.ThiSinh_ID == ts.ThiSinh_ID && n.DotXT_ID == dotXT.Dxt_ID).OrderBy(n => n.Dkxt_NguyenVong).Select(n => new
            {
                Dkxt_ID = n.Dkxt_ID,
                ThiSinh_ID = n.ThiSinh_ID,
                Ptxt_ID = n.Ptxt_ID,
                Nganh_All = new
                {
                    KhoiNganh_Ten = n.Nganh.KhoiNganh.KhoiNganh_Ten,
                    Nganh_GhiChu = n.Nganh.Nganh_MaNganh + " - " + n.Nganh.Nganh_TenNganh,
                },

                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = n.DotXT_ID,
                Dkxt_GhiChu = n.Dkxt_GhiChu,
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_MonDatGiai = n.Dkxt_MonDatGiai,
                Dkxt_NamDatGiai = n.Dkxt_NamDatGiai,
                Dkxt_LoaiGiai = n.Dkxt_LoaiGiai,
                Dkxt_NgayDangKy = n.Dkxt_NgayDangKy,
                Dkxt_MinhChung_HB = n.Dkxt_MinhChung_HB,
                Dkxt_MinhChung_CCCD = n.Dkxt_MinhChung_CCCD,
                Dkxt_MinhChung_Bang = n.Dkxt_MinhChung_Bang,
                Dkxt_MinhChung_Giai = n.Dkxt_MinhChung_Giai,
                Dkxt_MinhChung_UuTien = n.Dkxt_MinhChung_UuTien,

                Dkxt_TrangThai_KinhPhi = n.Dkxt_TrangThai_KinhPhi,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,

            }).ToList();
            return Json(new
            {
                success = true,
                thisinh = ts,
                data = nguyenvongs,
                Dkxt_CheckSetUpdate = KiemTraThoiGian(),
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Create(DangKyXetTuyenThang student)
        {
            db = new DbConnecttion();
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();
            var nvs = db.DangKyXetTuyenThangs.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID).ToList();

            long key_kp = 1;
            var sonv = db.DangKyXetTuyenThangs.OrderByDescending(x => x.Dkxt_ID).FirstOrDefault();
            if (sonv != null) { key_kp = sonv.Dkxt_ID + 1; }

            var dotXT = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            if (ts != null)
            {
                DangKyXetTuyenThang model = new DangKyXetTuyenThang();
                model.ThiSinh_ID = ts.ThiSinh_ID;
                model.Nganh_ID = int.Parse(student.Nganh_ID.ToString());
                model.Dkxt_ToHopXT = student.Dkxt_ToHopXT;
                model.Dkxt_MonDatGiai = student.Dkxt_MonDatGiai;
                model.Dkxt_LoaiGiai = student.Dkxt_LoaiGiai;
                model.Dkxt_NamDatGiai = student.Dkxt_NamDatGiai;
                model.Dkxt_CapDoGiai = student.Dkxt_CapDoGiai;
                model.Dkxt_ToHopXT = student.Dkxt_ToHopXT;

                model.Dkxt_MinhChung_HB = student.Dkxt_MinhChung_HB;
                model.Dkxt_MinhChung_CCCD = student.Dkxt_MinhChung_CCCD;
                model.Dkxt_MinhChung_Bang = student.Dkxt_MinhChung_Bang;
                model.Dkxt_MinhChung_Giai = student.Dkxt_MinhChung_Giai;
                model.Dkxt_MinhChung_UuTien = student.Dkxt_MinhChung_UuTien;

                model.Dkxt_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd");
                model.Ptxt_ID = 4;

                model.Dkxt_KinhPhi_NgayThang_CheckMC = "";
                model.Dkxt_KinhPhi_NgayThang_NopMC = "";
                model.Dkxt_KinhPhi_SoThamChieu = "";
                model.Dkxt_KinhPhi_TepMinhChung = "";

                model.Dkxt_TrangThai_KinhPhi = 0;
                model.Dkxt_TrangThai_HoSo = 0;
                model.Dkxt_TrangThai_KetQua = 0;

                model.Dkxt_NguyenVong = nvs != null ? nvs.Count() + 1 : 1;
                model.DotXT_ID = dotXT.Dxt_ID;

                model.Dkxt_KinhPhi_NoiDungGiaoDich = "DKXT" + key_kp + " P4  NV" + model.Dkxt_NguyenVong + " " + ts.ThiSinh_CCCD + " Nộp lệ phí xét tuyển";
                db.DangKyXetTuyenThangs.Add(model);
                db.SaveChanges();

                int nganh_id = int.Parse(student.Nganh_ID.ToString());
                var nganh = db.Nganhs.Where(n => n.Nganh_ID == nganh_id).FirstOrDefault();

                var subject = "Đăng ký nguyện vọng";
                var body = "Thí sinh " + ts.ThiSinh_Ten + ", Số CCCD: " + ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +
                     " <br/><b>Thông tin nguyện vọng:</b><br/>" +
                     " <p> Phương thức đăng ký: Phương thức 4 </p>" +
                     " <p> Mã ngành: " + nganh.Nganh_MaNganh + " </p>" +
                     " <p> Tên ngành: " + nganh.Nganh_TenNganh + " </p>" +
                     " <p> Môn đạt giải: " + student.Dkxt_MonDatGiai + " </p>" +
                     " <p> Loại giải: " + student.Dkxt_LoaiGiai + " </p>" +
                     " <p> Năm đạt giải: " + student.Dkxt_NamDatGiai + " </p>";

                try
                {
                    SendEmail s = new SendEmail();
                    s.Sendemail("xettuyen@hdu.edu.vn", body, subject);
                    return Json(new { success = true, data = "DKXT" + model.Dkxt_ID + " P4  NV" + model.Dkxt_NguyenVong + " " + ts.ThiSinh_CCCD + " Nộp lệ phí xét tuyển" }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new { success = false, data = "DKXT" + model.Dkxt_ID + " P4  NV" + model.Dkxt_NguyenVong + " " + ts.ThiSinh_CCCD + " Nộp lệ phí xét tuyển" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false, data = "LOI" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Edit(int? dkxt_id)
        {
            ViewBag.Dkxt_ID = dkxt_id;
            return View();
        }
        public ActionResult Detail(long? dkxt_id)
        {
            ViewBag.Dkxt_ID = dkxt_id;
            return View();
        }
        public JsonResult DangKyXetTuyenThang_GetByID(DangKyXetTuyenThang entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenThangs.Include(x => x.ThiSinhDangKy).Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();

            var NganhDK = db.Nganhs.Where(x => x.Nganh_ID == model.Nganh_ID).FirstOrDefault();
            var KhoiNganhDK = db.KhoiNganhs.Where(x => x.KhoiNganh_ID == NganhDK.KhoiNganh_ID).FirstOrDefault();
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

            var data_return = new
            {
                KhuVuc_ID = _ut_khuvuv_ten_diem,
                DoiTuong_ID = _ut_doituong_ten_diem,
                ThiSinh_HocLucLop12 = _xeploai_hocluc_12,
                ThiSinh_HanhKiemLop12 = _xeploai_hanhkiem_12,

                Dkxt_ID = model.Dkxt_ID,
                ThiSinh_ID = model.ThiSinh_ID,
                DotXT_ID = model.DotXT_ID,
                Ptxt_ID = model.Ptxt_ID,

                Dkxt_ToHopXT = model.Dkxt_ToHopXT,

                Nganh_ID = model.Nganh_ID,
                Nganh_TenNganh = NganhDK.Nganh_TenNganh,

                KhoiNganh_ID = KhoiNganhDK.KhoiNganh_ID,
                KhoiNganh_Ten = KhoiNganhDK.KhoiNganh_Ten,

                Dkxt_NguyenVong = model.Dkxt_NguyenVong,
                Dkxt_GhiChu = model.Dkxt_GhiChu,

                Dkxt_CapDoGiai = model.Dkxt_CapDoGiai,
                Dkxt_LoaiGiai = model.Dkxt_LoaiGiai,
                Dkxt_NamDatGiai = model.Dkxt_NamDatGiai,
                Dkxt_MonDatGiai = model.Dkxt_MonDatGiai,

                Dkxt_MinhChung_Giai = model.Dkxt_MinhChung_Giai,
                Dkxt_MinhChung_HB = model.Dkxt_MinhChung_HB,
                Dkxt_MinhChung_CCCD = model.Dkxt_MinhChung_CCCD,
                Dkxt_MinhChung_Bang = model.Dkxt_MinhChung_Bang,
                Dkxt_MinhChung_UuTien = model.Dkxt_MinhChung_UuTien,

                Dkxt_KinhPhi_SoThamChieu = model.Dkxt_KinhPhi_SoThamChieu,
                Dkxt_KinhPhi_TepMinhChung = model.Dkxt_KinhPhi_TepMinhChung,
                Dkxt_KinhPhi_NgayThang_NopMC = model.Dkxt_KinhPhi_NgayThang_NopMC,
                Dkxt_KinhPhi_NgayThang_CheckMC = model.Dkxt_KinhPhi_NgayThang_CheckMC,

                Dkxt_TrangThai_KinhPhi = model.Dkxt_TrangThai_KinhPhi,
                Dkxt_TrangThai_HoSo = model.Dkxt_TrangThai_HoSo,
                Dkxt_TrangThai_KetQua = model.Dkxt_TrangThai_KetQua,
                Dkxt_CheckSetUpdate = KiemTraThoiGian(),

            };
            return Json(new { success = true, data = data_return }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyenThang_Edit(DangKyXetTuyenThang entity)
        {
            db = new DbConnecttion();

            var session = Session["login_session"].ToString();

            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).
                Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var dotxettuyen = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            if (ts != null)
            {
                var model_edit = db.DangKyXetTuyenThangs.Include(x => x.Nganh).Include(x => x.DotXetTuyen).Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();

                model_edit.Nganh_ID = entity.Nganh_ID;

                model_edit.Dkxt_MinhChung_HB += entity.Dkxt_MinhChung_HB;
                model_edit.Dkxt_MinhChung_CCCD += entity.Dkxt_MinhChung_CCCD;
                model_edit.Dkxt_MinhChung_Bang += entity.Dkxt_MinhChung_Bang;
                model_edit.Dkxt_MinhChung_UuTien += entity.Dkxt_MinhChung_UuTien;
                model_edit.Dkxt_MinhChung_Giai += entity.Dkxt_MinhChung_Giai;
                model_edit.Dkxt_NamDatGiai = entity.Dkxt_NamDatGiai;
                model_edit.Dkxt_MonDatGiai = entity.Dkxt_MonDatGiai;
                model_edit.Dkxt_LoaiGiai = entity.Dkxt_LoaiGiai;

                model_edit.Dkxt_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd");

                model_edit.ThiSinh_ID = ts.ThiSinh_ID;
                model_edit.DotXT_ID = dotxettuyen.Dxt_ID;

                var diemDoiTuong = ts.DoiTuong.DoiTuong_DiemUuTien;
                var khuVucDoiTuong = ts.KhuVuc.KhuVuc_DiemUuTien;

                db.SaveChanges();

                #region gửi email
                int nganh_id = int.Parse(model_edit.Nganh_ID.ToString());

                var nganhdk = db.Nganhs.FirstOrDefault(n => n.Nganh_ID == nganh_id);

                var subject = "Đăng ký nguyện vọng";
                var body = "Thí sinh " + ts.ThiSinh_Ten + ", Số CCCD: " + ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +
                     " <br/><b>Thông tin nguyện vọng:</b><br/>" +
                     " <p>- Phương thức đăng ký: Xét tuyển thẳng </p>" +
                     " <p>- Mã ngành: " + nganhdk.Nganh_MaNganh + " </p>" +
                     " <p>- Tên ngành: " + nganhdk.Nganh_TenNganh + " </p>";
                SendEmail s = new SendEmail();
                s.Sendemail("xettuyen@hdu.edu.vn", body, subject);
                #endregion
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllKhoiNganh()
        {
            db = new DbConnecttion();
            var khoinganhs = db.KhoiNganhs.Select(n => new
            {
                KhoiNganh_ID = n.KhoiNganh_ID,
                KhoiNganh_Ten = n.KhoiNganh_Ten
            });
            return Json(khoinganhs.ToList(), JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllNganhs(string idKhoiNganh)
        {
            db = new DbConnecttion();
            int id = int.Parse(idKhoiNganh);
            var nganhs = db.Nganhs.Where(n => n.KhoiNganh_ID == id).Select(n => new
            {
                Nganh_ID = n.Nganh_ID,
                Nganh_MaNganh = n.Nganh_MaNganh,
                NganhTenNganh = n.Nganh_TenNganh
            });
            if (nganhs != null)
                return Json(new { success = true, data = nganhs.ToList() }, JsonRequestBehavior.AllowGet);
            else return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UploadFiles()
        {
            db = new DbConnecttion();

            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();
            string savePath = "/Uploads/UploadMinhChungs/";
            string fileNameStored = "";

            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = ts.ThiSinh_CCCD + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + fileName;
                    var filePath = Path.Combine(Server.MapPath("~/" + savePath), fileName);
                    var savedPath = savePath + fileName;
                    fileNameStored = fileNameStored + savedPath + "#";
                    file.SaveAs(filePath);
                }

                return Json(new { success = true, message = fileNameStored });
            }

            return Json(new { success = false, message = "No files were uploaded." });
        }

        public JsonResult Delete(string idDkxt)
        {
            db = new DbConnecttion();
            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                DangKyXetTuyenThang dangKyXetTuyen = db.DangKyXetTuyenThangs.Find(id);
                int nv_current = (int)dangKyXetTuyen.Dkxt_NguyenVong;
                int idThisinh = (int)dangKyXetTuyen.ThiSinh_ID;
                db.DangKyXetTuyenThangs.Remove(dangKyXetTuyen);
                foreach (var item in db.DangKyXetTuyenThangs.Where(nv => nv.Dkxt_NguyenVong > nv_current && nv.ThiSinh_ID == idThisinh))
                {
                    DangKyXetTuyenThang dangKyXetTuyen_change = db.DangKyXetTuyenThangs.FirstOrDefault(i => i.Dkxt_NguyenVong == item.Dkxt_NguyenVong && i.ThiSinh_ID == idThisinh);
                    dangKyXetTuyen_change.Dkxt_NguyenVong = item.Dkxt_NguyenVong - 1;
                }
                db.SaveChanges();
                return Json(new
                {
                    status = true,
                    msg = "Xoá dữ liệu thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false,
                msg = "Có lỗi xảy ra."
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete_MinhChung(ThongTinXoaMC entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenThangs.Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();
            if (entity.Dkxt_LoaiMC == "1") { model.Dkxt_MinhChung_Giai = model.Dkxt_MinhChung_Giai.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "2") { model.Dkxt_MinhChung_HB = model.Dkxt_MinhChung_HB.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "3") { model.Dkxt_MinhChung_Bang = model.Dkxt_MinhChung_Bang.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "4") { model.Dkxt_MinhChung_CCCD = model.Dkxt_MinhChung_CCCD.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "5") { model.Dkxt_MinhChung_UuTien = model.Dkxt_MinhChung_UuTien.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "6") { model.Dkxt_KinhPhi_TepMinhChung = model.Dkxt_KinhPhi_TepMinhChung.Replace(entity.Dkxt_Url + "#", ""); }

            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult upData(string idDkxt)
        {
            db = new DbConnecttion();
            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                DangKyXetTuyenThang dangKyXetTuyen_current = db.DangKyXetTuyenThangs.Find(id);
                int nv_current = (int)dangKyXetTuyen_current.Dkxt_NguyenVong;
                int idThisinh = (int)dangKyXetTuyen_current.ThiSinh_ID;
                dangKyXetTuyen_current.Dkxt_NguyenVong = nv_current - 1;
                DangKyXetTuyenThang dangKyXetTuyen_up = db.DangKyXetTuyenThangs.FirstOrDefault(i => i.Dkxt_NguyenVong == nv_current - 1 && i.ThiSinh_ID == idThisinh);
                dangKyXetTuyen_up.Dkxt_NguyenVong = nv_current;
                if (ModelState.IsValid)
                {
                    db.Entry(dangKyXetTuyen_current).State = EntityState.Modified;
                    db.Entry(dangKyXetTuyen_up).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new
                    {
                        status = true,
                        msg = "Thay đổi dữ liệu thành công"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                status = false,
                msg = "Có lỗi xảy ra."
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult downData(string idDkxt)
        {
            db = new DbConnecttion();

            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                DangKyXetTuyenThang dangKyXetTuyen_current = db.DangKyXetTuyenThangs.Find(id);
                int nv_current = (int)dangKyXetTuyen_current.Dkxt_NguyenVong;
                int idThisinh = (int)dangKyXetTuyen_current.ThiSinh_ID;
                dangKyXetTuyen_current.Dkxt_NguyenVong = nv_current + 1;
                DangKyXetTuyenThang dangKyXetTuyen_down = db.DangKyXetTuyenThangs.FirstOrDefault(i => i.Dkxt_NguyenVong == nv_current + 1 && i.ThiSinh_ID == idThisinh);
                dangKyXetTuyen_down.Dkxt_NguyenVong = nv_current;
                if (ModelState.IsValid)
                {
                    db.Entry(dangKyXetTuyen_current).State = EntityState.Modified;
                    db.Entry(dangKyXetTuyen_down).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new
                    {
                        status = true,
                        msg = "Thay đổi dữ liệu thành công"
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new
            {
                status = false,
                msg = "Có lỗi xảy ra."
            }, JsonRequestBehavior.AllowGet);
        }



    }
}