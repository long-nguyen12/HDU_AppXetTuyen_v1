using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace HDU_AppXetTuyen.Controllers
{
    public class DangKyXetTuyenKhacController : Controller
    {
        private DbConnecttion db = null;
        #region Đăng ký dự thi sử dụng đánh gia năng lực
        [ThiSinhSessionCheck]
        public ActionResult DanhGia()
        {
            return View();
        }
        public JsonResult DanhGiaGetAllNguyenVongs()
        {
            db = new DbConnecttion();
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).Include(x => x.DoiTuong).Include(x => x.KhuVuc).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_HocLucLop12 = n.ThiSinh_HocLucLop12,
                ThiSinh_HanhKiemLop12 = n.ThiSinh_HanhKiemLop12,
                ThiSinh_UTDT = n.DoiTuong.DoiTuong_Ten + ", Ưu tiên: " + n.DoiTuong.DoiTuong_DiemUuTien,
                ThiSinh_UTKV = n.KhuVuc.KhuVuc_Ten + ", Ưu tiên: " + n.KhuVuc.KhuVuc_DiemUuTien,

            }).FirstOrDefault();
            var nguyenvongs = db.DangKyXetTuyenKhacs.Include(d => d.Nganh).Include(x => x.ChungChi)
                                .Where(n => n.ThiSinh_ID == ts.ThiSinh_ID)
                                .Where(n => n.Dkxt_ToHopXT == "HDP6")
                                .OrderBy(n => n.Ptxt_ID)
                                .ThenBy(n => n.Dkxt_NguyenVong)
                                .Select(n => new
                                {
                                    Dkxt_ID = n.Dkxt_ID,
                                    Dkxt_ChungChi_Ten = new { LoaiCC = n.ChungChi.ChungChi_Ten },
                                    ThiSinh_ID = n.ThiSinh_ID,
                                    Ptxt_ID = n.Ptxt_ID,
                                    Nganh_All = new
                                    {
                                        KhoiNganh_Ten = n.Nganh.KhoiNganh.KhoiNganh_Ten,
                                        Nganh_GhiChu = n.Nganh.Nganh_GhiChu,
                                    },

                                    Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,
                                    Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                                    DotXT_ID = n.DotXT_ID,
                                    Dkxt_GhiChu = n.Dkxt_GhiChu,
                                    Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                                    Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                                    Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                                    Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                                    Dkxt_TongDiem = n.Dkxt_TongDiem,
                                    Dkxt_MinhChung_HB = n.Dkxt_MinhChung_HB,
                                    Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                                    Dkxt_MinhChung_CCCD = n.Dkxt_MinhChung_CCCD,
                                    Dkxt_MinhChung_Bang = n.Dkxt_MinhChung_Bang,
                                    Dkxt_MinhChung_KetQua = n.Dkxt_MinhChung_KetQua,
                                    Dkxt_MinhChung_UuTien = n.Dkxt_MinhChung_UuTien,
                                }).ToList();

            return Json(new
            {
                success = true,
                thisinh = ts,
                data = nguyenvongs,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DanhGiaAdd(DangKyXetTuyenKhac entity)
        {
            db = new DbConnecttion();
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();


            var nvs = db.DangKyXetTuyenKhacs.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP6")).ToList();
            var dotXT = db.DotXetTuyens.Where(n => n.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            string ToHop = entity.Dkxt_ToHopXT;
            if (ts != null)
            {
                DangKyXetTuyenKhac model = new DangKyXetTuyenKhac();
                model.ThiSinh_ID = ts.ThiSinh_ID;
                model.ChungChi_ID = int.Parse(entity.ChungChi_ID.ToString());
                model.Nganh_ID = int.Parse(entity.Nganh_ID.ToString());
                model.Dkxt_DonViToChuc = entity.Dkxt_DonViToChuc;
                model.Dkxt_KetQuaDatDuoc = double.Parse(entity.Dkxt_KetQuaDatDuoc.ToString());
                model.Dkxt_TongDiem = double.Parse(entity.Dkxt_TongDiem.ToString());
                model.Dkxt_NgayDuThi = entity.Dkxt_NgayDuThi;

                model.Dkxt_ToHopXT = entity.Dkxt_ToHopXT;

                if (String.IsNullOrEmpty(entity.Dkxt_MinhChung_HB)) { model.Dkxt_MinhChung_HB = ""; }
                else { model.Dkxt_MinhChung_HB = entity.Dkxt_MinhChung_HB; }

                if (String.IsNullOrEmpty(entity.Dkxt_MinhChung_CCCD)) { model.Dkxt_MinhChung_CCCD = ""; }
                else { model.Dkxt_MinhChung_CCCD = entity.Dkxt_MinhChung_CCCD; }

                if (String.IsNullOrEmpty(entity.Dkxt_MinhChung_Bang)) { model.Dkxt_MinhChung_Bang = ""; }
                else { model.Dkxt_MinhChung_Bang = entity.Dkxt_MinhChung_Bang; }

                if (String.IsNullOrEmpty(entity.Dkxt_MinhChung_KetQua)) { model.Dkxt_MinhChung_KetQua = ""; }
                else { model.Dkxt_MinhChung_KetQua = entity.Dkxt_MinhChung_KetQua; }

                if (String.IsNullOrEmpty(entity.Dkxt_MinhChung_UuTien)) { model.Dkxt_MinhChung_UuTien = ""; }
                else { model.Dkxt_MinhChung_UuTien = entity.Dkxt_MinhChung_UuTien; }

                model.Dkxt_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd");
                model.Ptxt_ID = int.Parse(ToHop[ToHop.Length - 1].ToString());

                model.Dkxt_KinhPhi_NgayThang_CheckMC = "";
                model.Dkxt_KinhPhi_NgayThang_NopMC = "";
                model.Dkxt_KinhPhi_SoThamChieu = "";
                model.Dkxt_KinhPhi_TepMinhChung = "";

                model.Dkxt_TrangThai_KinhPhi = 0;
                model.Dkxt_TrangThai_HoSo = 0;
                model.Dkxt_TrangThai_KetQua = 0;

                model.Dkxt_NguyenVong = nvs != null ? nvs.Count() + 1 : 1;
                model.DotXT_ID = dotXT.Dxt_ID;
                db.DangKyXetTuyenKhacs.Add(model);
                db.SaveChanges();

                int nganh_id = int.Parse(entity.Nganh_ID.ToString());
                var nganh = db.Nganhs.Where(n => n.Nganh_ID == nganh_id).FirstOrDefault();
                var subject = "Đăng ký nguyện vọng";
                var body = "Thí sinh " + ts.ThiSinh_Ten + ", Số CCCD: " + ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +

                     " <br/><b>Thông tin nguyện vọng:</b><br/>" +
                     " <p> Phương thức đăng ký: Phương thức " + ToHop[ToHop.Length - 1].ToString() + "</p>" +
                     " <p> Mã ngành: " + nganh.Nganh_MaNganh + " </p>" +
                     " <p> Tên ngành: " + nganh.Nganh_TenNganh + " </p>" +
                     " <p> Đơn vị tổ chức: " + entity.Dkxt_DonViToChuc + " </p>" +
                     " <p> Kết quả: " + entity.Dkxt_KetQuaDatDuoc + " </p>" +
                     " <p> Ngày dự thi: " + entity.Dkxt_NgayDuThi + " </p>";
                SendEmail s = new SendEmail();
                s.Sendemail("xettuyen@hdu.edu.vn", body, subject);

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [ThiSinhSessionCheck]
        public ActionResult DanhGiaEdit(long? Dkxt_ID)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;
            return View();
        }
        [ThiSinhSessionCheck]
        public JsonResult DanhGiaEdit_Json(DangKyXetTuyenKhac entity)
        {
            db = new DbConnecttion();

            var session = Session["login_session"].ToString();

            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).
                Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var dotxettuyen = db.DotXetTuyens.Where(n => n.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            if (ts != null)
            {
                var model_edit = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.DotXetTuyen).Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();

                model_edit.Nganh_ID = entity.Nganh_ID;

                model_edit.Dkxt_MinhChung_HB += entity.Dkxt_MinhChung_HB;
                model_edit.Dkxt_MinhChung_CCCD += entity.Dkxt_MinhChung_CCCD;
                model_edit.Dkxt_MinhChung_Bang += entity.Dkxt_MinhChung_Bang;
                model_edit.Dkxt_MinhChung_UuTien += entity.Dkxt_MinhChung_UuTien;
                model_edit.Dkxt_MinhChung_KetQua += entity.Dkxt_MinhChung_KetQua;
                model_edit.Dkxt_DonViToChuc = entity.Dkxt_DonViToChuc;
                model_edit.Dkxt_TongDiem = entity.Dkxt_TongDiem;
                model_edit.Dkxt_KetQuaDatDuoc = entity.Dkxt_KetQuaDatDuoc;
                model_edit.Dkxt_NgayDuThi = entity.Dkxt_NgayDuThi;
                model_edit.ChungChi_ID = entity.ChungChi_ID;

                model_edit.Dkxt_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

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
        #endregion

        #region Đăng ký dự thi sử dung cc ngoại ngữ
        [ThiSinhSessionCheck]
        public ActionResult Ielts()
        {
            return View();
        }
        [ThiSinhSessionCheck]
        public JsonResult IeltsGetAllNguyenVongs()
        {
            db = new DbConnecttion();
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).Include(x => x.DoiTuong).Include(x => x.KhuVuc).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_HocLucLop12 = n.ThiSinh_HocLucLop12,
                ThiSinh_HanhKiemLop12 = n.ThiSinh_HanhKiemLop12,
                ThiSinh_UTDT = n.DoiTuong.DoiTuong_Ten + ", Ưu tiên: " + n.DoiTuong.DoiTuong_DiemUuTien,
                ThiSinh_UTKV = n.KhuVuc.KhuVuc_Ten + ", Ưu tiên: " + n.KhuVuc.KhuVuc_DiemUuTien,

            }).FirstOrDefault();
            var nguyenvongs = db.DangKyXetTuyenKhacs.Include(d => d.Nganh).Include(x => x.ChungChi)
                                .Where(n => n.ThiSinh_ID == ts.ThiSinh_ID)
                                .Where(n => n.Dkxt_ToHopXT == "HDP5")
                                .OrderBy(n => n.Ptxt_ID)
                                .ThenBy(n => n.Dkxt_NguyenVong)
                                .Select(n => new
                                {
                                    Dkxt_ID = n.Dkxt_ID,
                                    Dkxt_ChungChi_Ten = new { LoaiCC = n.ChungChi.ChungChi_Ten },
                                    ThiSinh_ID = n.ThiSinh_ID,
                                    Ptxt_ID = n.Ptxt_ID,
                                    Nganh_All = new
                                    {
                                        KhoiNganh_Ten = n.Nganh.KhoiNganh.KhoiNganh_Ten,
                                        Nganh_GhiChu = n.Nganh.Nganh_GhiChu,
                                    },

                                    Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,
                                    Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                                    DotXT_ID = n.DotXT_ID,
                                    Dkxt_GhiChu = n.Dkxt_GhiChu,
                                    Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                                    Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                                    Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                                    Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                                    Dkxt_TongDiem = n.Dkxt_TongDiem,
                                    Dkxt_MinhChung_HB = n.Dkxt_MinhChung_HB,
                                    Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                                    Dkxt_MinhChung_CCCD = n.Dkxt_MinhChung_CCCD,
                                    Dkxt_MinhChung_Bang = n.Dkxt_MinhChung_Bang,
                                    Dkxt_MinhChung_KetQua = n.Dkxt_MinhChung_KetQua,
                                    Dkxt_MinhChung_UuTien = n.Dkxt_MinhChung_UuTien,
                                }).ToList();

            return Json(new
            {
                success = true,
                thisinh = ts,
                data = nguyenvongs,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult IeltsAdd(DangKyXetTuyenKhac entity)
        {
            db = new DbConnecttion();
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();


            var nvs = db.DangKyXetTuyenKhacs.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP5")).ToList();
            var dotXT = db.DotXetTuyens.Where(n => n.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            string ToHop = entity.Dkxt_ToHopXT;
            if (ts != null)
            {
                DangKyXetTuyenKhac model = new DangKyXetTuyenKhac();
                model.ThiSinh_ID = ts.ThiSinh_ID;
                model.ChungChi_ID = int.Parse(entity.ChungChi_ID.ToString());
                model.Nganh_ID = int.Parse(entity.Nganh_ID.ToString());
                model.Dkxt_DonViToChuc = entity.Dkxt_DonViToChuc;
                model.Dkxt_KetQuaDatDuoc = double.Parse(entity.Dkxt_KetQuaDatDuoc.ToString());
                model.Dkxt_TongDiem = double.Parse(entity.Dkxt_TongDiem.ToString());
                model.Dkxt_NgayDuThi = entity.Dkxt_NgayDuThi;

                model.Dkxt_ToHopXT = entity.Dkxt_ToHopXT;
                model.Dkxt_MinhChung_HB = entity.Dkxt_MinhChung_HB;
                model.Dkxt_MinhChung_CCCD = entity.Dkxt_MinhChung_CCCD;
                model.Dkxt_MinhChung_Bang = entity.Dkxt_MinhChung_Bang;
                model.Dkxt_MinhChung_KetQua = entity.Dkxt_MinhChung_KetQua;
                model.Dkxt_MinhChung_UuTien = entity.Dkxt_MinhChung_UuTien;

                model.Dkxt_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd");
                model.Ptxt_ID = int.Parse(ToHop[ToHop.Length - 1].ToString());

                model.Dkxt_KinhPhi_NgayThang_CheckMC = "";
                model.Dkxt_KinhPhi_NgayThang_NopMC = "";
                model.Dkxt_KinhPhi_SoThamChieu = "";
                model.Dkxt_KinhPhi_TepMinhChung = "";

                model.Dkxt_TrangThai_KinhPhi = 0;
                model.Dkxt_TrangThai_HoSo = 0;
                model.Dkxt_TrangThai_KetQua = 0;

                model.Dkxt_NguyenVong = nvs != null ? nvs.Count() + 1 : 1;
                model.DotXT_ID = dotXT.Dxt_ID;
                db.DangKyXetTuyenKhacs.Add(model);
                db.SaveChanges();

                int nganh_id = int.Parse(entity.Nganh_ID.ToString());
                var nganh = db.Nganhs.Where(n => n.Nganh_ID == nganh_id).FirstOrDefault();
                var subject = "Đăng ký nguyện vọng";
                var body = "Thí sinh " + ts.ThiSinh_Ten + ", Số CCCD: " + ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +

                     " <br/><b>Thông tin nguyện vọng:</b><br/>" +
                     " <p> Phương thức đăng ký: Phương thức " + ToHop[ToHop.Length - 1].ToString() + "</p>" +
                     " <p> Mã ngành: " + nganh.Nganh_MaNganh + " </p>" +
                     " <p> Tên ngành: " + nganh.Nganh_TenNganh + " </p>" +
                     " <p> Đơn vị tổ chức: " + entity.Dkxt_DonViToChuc + " </p>" +
                     " <p> Kết quả: " + entity.Dkxt_KetQuaDatDuoc + " </p>" +
                     " <p> Ngày dự thi: " + entity.Dkxt_NgayDuThi + " </p>";
                SendEmail s = new SendEmail();
                s.Sendemail("xettuyen@hdu.edu.vn", body, subject);

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [ThiSinhSessionCheck]
        public ActionResult IeltsEdit(long? Dkxt_ID)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;
            return View();
        }

        [HttpPost]
        public JsonResult IeltsEdit_Json(DangKyXetTuyenKhac entity)
        {
            db = new DbConnecttion();
            var session = Session["login_session"].ToString();

            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).
                Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var dotxettuyen = db.DotXetTuyens.Where(n => n.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            if (ts != null)
            {
                var model_edit = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.DotXetTuyen).Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();

                model_edit.Nganh_ID = entity.Nganh_ID;
                model_edit.Dkxt_MinhChung_HB += entity.Dkxt_MinhChung_HB;
                model_edit.Dkxt_MinhChung_CCCD += entity.Dkxt_MinhChung_CCCD;
                model_edit.Dkxt_MinhChung_Bang += entity.Dkxt_MinhChung_Bang;
                model_edit.Dkxt_MinhChung_UuTien += entity.Dkxt_MinhChung_UuTien;
                model_edit.Dkxt_MinhChung_KetQua += entity.Dkxt_MinhChung_KetQua;
                model_edit.Dkxt_DonViToChuc = entity.Dkxt_DonViToChuc;
                model_edit.Dkxt_TongDiem = entity.Dkxt_TongDiem;
                model_edit.Dkxt_KetQuaDatDuoc = entity.Dkxt_KetQuaDatDuoc;
                model_edit.Dkxt_NgayDuThi = entity.Dkxt_NgayDuThi;
                model_edit.ChungChi_ID = entity.ChungChi_ID;
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
                     " <p>- Phương thức đăng ký: Xét tuyển bằng chứng chỉ ngoại ngữ </p>" +
                     " <p>- Mã ngành: " + nganhdk.Nganh_MaNganh + " </p>" +
                     " <p>- Tên ngành: " + nganhdk.Nganh_TenNganh + " </p>";
                SendEmail s = new SendEmail();
                s.Sendemail("xettuyen@hdu.edu.vn", body, subject);
                #endregion
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [ThiSinhSessionCheck]
        public JsonResult IeltsGetAllChungChis()
        {
            db = new DbConnecttion();
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_HocLucLop12 = n.ThiSinh_HocLucLop12,
                ThiSinh_HanhKiemLop12 = n.ThiSinh_HanhKiemLop12
            }).FirstOrDefault();

            var chungchis = db.ChungChis.Where(n => n.ChungChi_TrangThai == 1 && n.ChungChi_PhuongThuc == "5").Select(n => new
            {
                ChungChi_ID = n.ChungChi_ID,
                ChungChi_Ten = n.ChungChi_Ten,
                ChungChi_TruongTCThi = n.ChungChi_TruongTCThi,
                ChungChi_ThangDiem = n.ChungChi_ThangDiem,
                ChungChi_PhuongThuc = n.ChungChi_PhuongThuc
            }).ToList();

            return Json(new
            {
                success = true,
                chungchi = chungchis
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult GetNguyenVongByID(DangKyXetTuyenKhac entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenKhacs.Include(d => d.Nganh).Include(x => x.ChungChi).FirstOrDefault(x => x.Dkxt_ID == entity.Dkxt_ID);
            var model_ts = db.ThiSinhDangKies.Include(x => x.DoiTuong).Include(x => x.KhuVuc).Where(x => x.ThiSinh_ID == model.ThiSinh_ID).FirstOrDefault();

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

            var data_return = new
            {

                ThiSinh_HocLucLop12 = _xeploai_hocluc_12,
                ThiSinh_HanhKiemLop12 = _xeploai_hanhkiem_12,
                ThiSinh_UTDT = model_ts.DoiTuong.DoiTuong_Ten + ", Ưu tiên: " + model_ts.DoiTuong.DoiTuong_DiemUuTien,
                ThiSinh_UTKV = model_ts.KhuVuc.KhuVuc_Ten + ", Ưu tiên: " + model_ts.KhuVuc.KhuVuc_DiemUuTien,


                Dkxt_ID = model.Dkxt_ID,
                Dkxt_ChungChi_Ten = new { LoaiCC = model.ChungChi.ChungChi_Ten },
                ThiSinh_ID = model.ThiSinh_ID,
                Ptxt_ID = model.Ptxt_ID,
                ChungChi_ID = model.ChungChi_ID,

                KhoiNganh_Ten = model.Nganh.KhoiNganh.KhoiNganh_Ten,
                KhoiNganh_ID = model.Nganh.KhoiNganh_ID,
                Nganh_GhiChu = model.Nganh.Nganh_GhiChu,
                Nganh_ID = model.Nganh_ID,


                Dkxt_NguyenVong = model.Dkxt_NguyenVong,
                DotXT_ID = model.DotXT_ID,
                Dkxt_GhiChu = model.Dkxt_GhiChu,
                Dkxt_ToHopXT = model.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = model.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = model.Dkxt_KetQuaDatDuoc,
                Dkxt_NgayDuThi = model.Dkxt_NgayDuThi,
                Dkxt_TongDiem = model.Dkxt_TongDiem,

                Dkxt_TrangThai_KetQua = model.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai_HoSo = model.Dkxt_TrangThai_HoSo,               
            };

            return Json(new { success = true, data = data_return, }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetImgNguyenVongByID(DangKyXetTuyenKhac entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenKhacs.Include(d => d.Nganh).Include(x => x.ChungChi).FirstOrDefault(x => x.Dkxt_ID == entity.Dkxt_ID);          
            var data_return = new
            {
                Dkxt_ID = model.Dkxt_ID,
                Dkxt_TrangThai_KinhPhi = model.Dkxt_TrangThai_KinhPhi,
                Dkxt_MinhChung_KetQua = model.Dkxt_MinhChung_KetQua,
                Dkxt_MinhChung_HB = model.Dkxt_MinhChung_HB,
                Dkxt_MinhChung_CCCD = model.Dkxt_MinhChung_CCCD,
                Dkxt_MinhChung_Bang = model.Dkxt_MinhChung_Bang,
                Dkxt_MinhChung_UuTien = model.Dkxt_MinhChung_UuTien,
            };

            return Json(new { success = true, data = data_return, }, JsonRequestBehavior.AllowGet);
        }

        [ThiSinhSessionCheck]
        public JsonResult GetAllChungChis()
        {
            db = new DbConnecttion();
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_HocLucLop12 = n.ThiSinh_HocLucLop12,
                ThiSinh_HanhKiemLop12 = n.ThiSinh_HanhKiemLop12
            }).FirstOrDefault();

            var chungchis = db.ChungChis.Where(n => n.ChungChi_TrangThai == 1 && n.ChungChi_PhuongThuc == "6").Select(n => new
            {
                ChungChi_ID = n.ChungChi_ID,
                ChungChi_Ten = n.ChungChi_Ten,
                ChungChi_TruongTCThi = n.ChungChi_TruongTCThi,
                ChungChi_ThangDiem = n.ChungChi_ThangDiem,
                ChungChi_PhuongThuc = n.ChungChi_PhuongThuc
            }).ToList();

            return Json(new
            {
                success = true,
                chungchi = chungchis
            }, JsonRequestBehavior.AllowGet);
        }

        [ThiSinhSessionCheck]
        public JsonResult GetChungChiByID(string id)
        {
            db = new DbConnecttion();
            var session = Session["login_session"];
            int ID = int.Parse(id);
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_HocLucLop12 = n.ThiSinh_HocLucLop12,
                ThiSinh_HanhKiemLop12 = n.ThiSinh_HanhKiemLop12
            }).FirstOrDefault();

            var chungchi = db.ChungChis.Where(n => n.ChungChi_TrangThai == 1 && n.ChungChi_ID == ID).Select(n => new
            {
                ChungChi_ID = n.ChungChi_ID,
                ChungChi_Ten = n.ChungChi_Ten,
                ChungChi_TruongTCThi = n.ChungChi_TruongTCThi,
                ChungChi_ThangDiem = n.ChungChi_ThangDiem,
                ChungChi_PhuongThuc = n.ChungChi_PhuongThuc
            }).FirstOrDefault();

            return Json(new
            {
                success = true,
                chungchi = chungchi
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(string idDkxt, string MaToHop)
        {
            db = new DbConnecttion();
            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                DangKyXetTuyenKhac dangKyXetTuyen = db.DangKyXetTuyenKhacs.Where(n => n.Dkxt_ID == id && n.Dkxt_ToHopXT.Equals(MaToHop)).FirstOrDefault();
                int nv_current = (int)dangKyXetTuyen.Dkxt_NguyenVong;
                int idThisinh = (int)dangKyXetTuyen.ThiSinh_ID;
                db.DangKyXetTuyenKhacs.Remove(dangKyXetTuyen);
                foreach (var item in db.DangKyXetTuyenKhacs.Where(nv => nv.Dkxt_NguyenVong > nv_current && nv.ThiSinh_ID == idThisinh && nv.Dkxt_ToHopXT.Equals(MaToHop)))
                {
                    DangKyXetTuyenKhac dangKyXetTuyen_change = db.DangKyXetTuyenKhacs.FirstOrDefault(i => i.Dkxt_NguyenVong == item.Dkxt_NguyenVong && i.ThiSinh_ID == idThisinh && i.Dkxt_ToHopXT.Equals(MaToHop));
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
            var model = db.DangKyXetTuyenKhacs.Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();
            if (model == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (entity.Dkxt_LoaiMC == "1") { model.Dkxt_MinhChung_KetQua = model.Dkxt_MinhChung_KetQua.Replace(entity.Dkxt_Url + "#", ""); }
                if (entity.Dkxt_LoaiMC == "2") { model.Dkxt_MinhChung_HB = model.Dkxt_MinhChung_HB.Replace(entity.Dkxt_Url + "#", ""); }
                if (entity.Dkxt_LoaiMC == "3") { model.Dkxt_MinhChung_CCCD = model.Dkxt_MinhChung_CCCD.Replace(entity.Dkxt_Url + "#", ""); }
                if (entity.Dkxt_LoaiMC == "4") { model.Dkxt_MinhChung_Bang = model.Dkxt_MinhChung_Bang.Replace(entity.Dkxt_Url + "#", ""); }
                if (entity.Dkxt_LoaiMC == "5") { model.Dkxt_MinhChung_UuTien = model.Dkxt_MinhChung_UuTien.Replace(entity.Dkxt_Url + "#", ""); }
                if (entity.Dkxt_LoaiMC == "6") { model.Dkxt_KinhPhi_TepMinhChung = model.Dkxt_KinhPhi_TepMinhChung.Replace(entity.Dkxt_Url + "#", ""); }
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult upData(string idDkxt, string MaToHop)
        {
            db = new DbConnecttion();
            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                DangKyXetTuyenKhac dangKyXetTuyen_current = db.DangKyXetTuyenKhacs.Where(n => n.Dkxt_ID == id && n.Dkxt_ToHopXT.Equals(MaToHop)).FirstOrDefault();
                int nv_current = (int)dangKyXetTuyen_current.Dkxt_NguyenVong;
                int idThisinh = (int)dangKyXetTuyen_current.ThiSinh_ID;
                dangKyXetTuyen_current.Dkxt_NguyenVong = nv_current - 1;
                DangKyXetTuyenKhac dangKyXetTuyen_up = db.DangKyXetTuyenKhacs.FirstOrDefault(i => i.Dkxt_NguyenVong == nv_current - 1 && i.ThiSinh_ID == idThisinh && i.Dkxt_ToHopXT.Equals(MaToHop));
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
        public JsonResult downData(string idDkxt, string MaToHop)
        {
            db = new DbConnecttion();
            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                DangKyXetTuyenKhac dangKyXetTuyen_current = db.DangKyXetTuyenKhacs.Where(n => n.Dkxt_ID == id && n.Dkxt_ToHopXT.Equals(MaToHop)).FirstOrDefault();
                int nv_current = (int)dangKyXetTuyen_current.Dkxt_NguyenVong;
                int idThisinh = (int)dangKyXetTuyen_current.ThiSinh_ID;
                dangKyXetTuyen_current.Dkxt_NguyenVong = nv_current + 1;
                DangKyXetTuyenKhac dangKyXetTuyen_down = db.DangKyXetTuyenKhacs.FirstOrDefault(i => i.Dkxt_NguyenVong == nv_current + 1 && i.ThiSinh_ID == idThisinh && i.Dkxt_ToHopXT.Equals(MaToHop));
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