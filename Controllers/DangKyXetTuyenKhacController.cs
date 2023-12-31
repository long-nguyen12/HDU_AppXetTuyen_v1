﻿using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Net.Mail;
using System.Net;

namespace HDU_AppXetTuyen.Controllers
{
    public class StudentKhacInfo
    {
        public string Nganh_ID { get; set; }
        public string ChungChi_ID { get; set; }
        public string Dkxt_DonViToChuc { get; set; }
        public string Dkxt_KetQuaDatDuoc { get; set; }
        public string Dkxt_TongDiem { get; set; }
        public string Dkxt_NgayDuThi { get; set; }
        public string Dkxt_XepLoaiHocLuc_12 { get; set; }
        public string Dkxt_XepLoaiHanhKiem_12 { get; set; }
        public string Dkxt_ToHopXT { get; set; }
        public string Dkxt_MinhChung_HB { get; set; }
        public string Dkxt_MinhChung_CCCD { get; set; }
        public string Dkxt_MinhChung_Bang { get; set; }
        public string Dkxt_MinhChung_KetQua { get; set; }
        public string Dkxt_MinhChung_UuTien { get; set; }
    }
    public class DangKyXetTuyenKhacController : Controller
    {
        private DbConnecttion db = new DbConnecttion();
        // GET: DangKyXetTuyenKhac
        [ThiSinhSessionCheck]
        public ActionResult Ielts()
        {
            return View();
        }

        [ThiSinhSessionCheck]
        public ActionResult DanhGia()
        {
            return View();
        }

        [ThiSinhSessionCheck]
        public JsonResult GetAllNguyenVongs()
        {
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).Include(x =>x.DoiTuong).Include(x => x.KhuVuc).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_HocLucLop12 = n.ThiSinh_HocLucLop12,
                ThiSinh_HanhKiemLop12 = n.ThiSinh_HanhKiemLop12,
                ThiSinh_UTDT  = n.DoiTuong.DoiTuong_Ten + ", Ưu tiên: " + n.DoiTuong.DoiTuong_DiemUuTien,
                ThiSinh_UTKV = n.KhuVuc.KhuVuc_Ten + ", Ưu tiên: " + n.KhuVuc.KhuVuc_DiemUuTien,

            }).FirstOrDefault();
            var nguyenvongs = db.DangKyXetTuyenKhacs.Include(d => d.Nganh).Include(x => x.ChungChi)
                                .Where(n => n.ThiSinh_ID == ts.ThiSinh_ID)
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
                DoiTuong_ID = n.DoiTuong_ID,
                KhuVuc_ID = n.KhuVuc_ID,
                Dkxt_TrangThai = n.Dkxt_TrangThai,
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = n.DotXT_ID,
                Dkxt_GhiChu = n.Dkxt_GhiChu,
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                Dkxt_TongDiem = n.Dkxt_TongDiem,
                Dkxt_XepLoaiHocLuc_12 = n.Dkxt_XepLoaiHocLuc_12,
                Dkxt_XepLoaiHanhKiem_12 = n.Dkxt_XepLoaiHanhKiem_12,
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

        [ThiSinhSessionCheck]
        public JsonResult GetAllChungChis()
        {
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_HocLucLop12 = n.ThiSinh_HocLucLop12,
                ThiSinh_HanhKiemLop12 = n.ThiSinh_HanhKiemLop12
            }).FirstOrDefault();

            var chungchis = db.ChungChis.Where(n => n.ChungChi_TrangThai == 1).Select(n => new
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

        [HttpPost]
        public JsonResult Ielts(StudentKhacInfo student)
        {
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();
            var nvs = db.DangKyXetTuyenKhacs.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID && n.Dkxt_ToHopXT.Equals(student.Dkxt_ToHopXT)).ToList();
            var dotXT = db.DotXetTuyens.Where(n => n.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            string ToHop = student.Dkxt_ToHopXT;
            if (ts != null)
            {
                DangKyXetTuyenKhac dkxtt = new DangKyXetTuyenKhac();
                dkxtt.ThiSinh_ID = ts.ThiSinh_ID;
                dkxtt.Nganh_ID = int.Parse(student.Nganh_ID);
                dkxtt.Dkxt_DonViToChuc = student.Dkxt_DonViToChuc;
                dkxtt.Dkxt_KetQuaDatDuoc = float.Parse(student.Dkxt_KetQuaDatDuoc);
                dkxtt.Dkxt_TongDiem = float.Parse(student.Dkxt_TongDiem);
                dkxtt.Dkxt_NgayDuThi = student.Dkxt_NgayDuThi;
                dkxtt.Dkxt_XepLoaiHocLuc_12 = ts.ThiSinh_HocLucLop12;
                dkxtt.Dkxt_XepLoaiHanhKiem_12 = ts.ThiSinh_HanhKiemLop12;
                dkxtt.Dkxt_ToHopXT = student.Dkxt_ToHopXT;
                dkxtt.Dkxt_MinhChung_HB = student.Dkxt_MinhChung_HB;
                dkxtt.Dkxt_MinhChung_CCCD = student.Dkxt_MinhChung_CCCD;
                dkxtt.Dkxt_MinhChung_Bang = student.Dkxt_MinhChung_Bang;
                dkxtt.Dkxt_MinhChung_KetQua = student.Dkxt_MinhChung_KetQua;
                dkxtt.Dkxt_MinhChung_UuTien = student.Dkxt_MinhChung_UuTien;
                dkxtt.Dkxt_NgayDangKy = DateTime.Now.ToString("dd/MM/yyyy");
                dkxtt.Ptxt_ID = int.Parse(ToHop[ToHop.Length - 1].ToString());
                dkxtt.DoiTuong_ID = ts.DoiTuong_ID;
                dkxtt.KhuVuc_ID = ts.KhuVuc_ID;
                dkxtt.Dkxt_TrangThai = 0;
                dkxtt.Dkxt_TrangThai_KetQua = 0;
                dkxtt.Dkxt_NguyenVong = nvs != null ? nvs.Count() + 1 : 1;
                dkxtt.DotXT_ID = dotXT.Dxt_ID;
                db.DangKyXetTuyenKhacs.Add(dkxtt);
                db.SaveChanges();

                KinhPhi kp = new KinhPhi();
                kp.ThiSinh_ID = ts.ThiSinh_ID;
                kp.Dkxt_ID = dkxtt.Dkxt_ID;
                kp.Ptxt_ID = int.Parse(ToHop[ToHop.Length - 1].ToString());
                kp.Dxt_ID = dotXT.Dxt_ID;
                kp.KinhPhi_TrangThai = 0;
                db.KinhPhis.Add(kp);
                db.SaveChanges();

                int nganh_id = int.Parse(student.Nganh_ID);
                var nganh = db.Nganhs.Where(n => n.Nganh_ID == nganh_id).FirstOrDefault();
                var subject = "Đăng ký nguyện vọng";
                var body = "Thí sinh " + ts.ThiSinh_Ten + ", Số CCCD: " + ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +

                     " <br/><b>Thông tin nguyện vọng:</b><br/>" +
                     " <p> Phương thức đăng ký: Phương thức " + ToHop[ToHop.Length - 1].ToString() + "</p>" +
                     " <p> Mã ngành: " + nganh.Nganh_MaNganh + " </p>" +
                     " <p> Tên ngành: " + nganh.Nganh_TenNganh + " </p>" +
                     " <p> Loại chứng chỉ: " + student.Dkxt_DonViToChuc + " </p>" +
                     " <p> Kết quả: " + student.Dkxt_KetQuaDatDuoc + " </p>" +
                     " <p> Ngày dự thi: " + student.Dkxt_NgayDuThi + " </p>";
                SendEmail s = new SendEmail();
                s.Sendemail("xettuyen@hdu.edu.vn", body, subject);

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult DanhGia(StudentKhacInfo student)
        {
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();
            var nvs = db.DangKyXetTuyenKhacs.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID && n.Dkxt_ToHopXT.Equals(student.Dkxt_ToHopXT)).ToList();
            var dotXT = db.DotXetTuyens.Where(n => n.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            string ToHop = student.Dkxt_ToHopXT;
            if (ts != null)
            {
                DangKyXetTuyenKhac dkxtt = new DangKyXetTuyenKhac();
                dkxtt.ThiSinh_ID = ts.ThiSinh_ID;
                dkxtt.ChungChi_ID = int.Parse(student.ChungChi_ID); 
                dkxtt.Nganh_ID = int.Parse(student.Nganh_ID);
                dkxtt.Dkxt_DonViToChuc = student.Dkxt_DonViToChuc;
                dkxtt.Dkxt_KetQuaDatDuoc = float.Parse(student.Dkxt_KetQuaDatDuoc);
                dkxtt.Dkxt_TongDiem = float.Parse(student.Dkxt_TongDiem);
                dkxtt.Dkxt_NgayDuThi = student.Dkxt_NgayDuThi;
                dkxtt.Dkxt_XepLoaiHocLuc_12 = ts.ThiSinh_HocLucLop12;
                dkxtt.Dkxt_XepLoaiHanhKiem_12 = ts.ThiSinh_HanhKiemLop12;
                dkxtt.Dkxt_ToHopXT = student.Dkxt_ToHopXT;
                dkxtt.Dkxt_MinhChung_HB = student.Dkxt_MinhChung_HB;
                dkxtt.Dkxt_MinhChung_CCCD = student.Dkxt_MinhChung_CCCD;
                dkxtt.Dkxt_MinhChung_Bang = student.Dkxt_MinhChung_Bang;
                dkxtt.Dkxt_MinhChung_KetQua = student.Dkxt_MinhChung_KetQua;
                dkxtt.Dkxt_MinhChung_UuTien = student.Dkxt_MinhChung_UuTien;
                dkxtt.Dkxt_NgayDangKy = DateTime.Now.ToString("dd/MM/yyyy");
                dkxtt.Ptxt_ID = int.Parse(ToHop[ToHop.Length - 1].ToString());
                dkxtt.DoiTuong_ID = ts.DoiTuong_ID;
                dkxtt.KhuVuc_ID = ts.KhuVuc_ID;
                dkxtt.Dkxt_TrangThai = 0;
                dkxtt.Dkxt_TrangThai_KetQua = 0;
                dkxtt.Dkxt_NguyenVong = nvs != null ? nvs.Count() + 1 : 1;
                dkxtt.DotXT_ID = dotXT.Dxt_ID;
                db.DangKyXetTuyenKhacs.Add(dkxtt);
                db.SaveChanges();

                var NguyenVong = db.DangKyXetTuyenKhacs.OrderByDescending(n => n.Dkxt_ID).Take(1).FirstOrDefault();
                KinhPhi kp = new KinhPhi();
                kp.ThiSinh_ID = ts.ThiSinh_ID;
                kp.Dkxt_ID = NguyenVong.Dkxt_ID;
                kp.Ptxt_ID = int.Parse(ToHop[ToHop.Length - 1].ToString());
                kp.Dxt_ID = dotXT.Dxt_ID;
                kp.KinhPhi_TrangThai = 0;
                db.KinhPhis.Add(kp); db.SaveChanges();

                int nganh_id = int.Parse(student.Nganh_ID);
                var nganh = db.Nganhs.Where(n => n.Nganh_ID == nganh_id).FirstOrDefault();
                var subject = "Đăng ký nguyện vọng";
                var body = "Thí sinh " + ts.ThiSinh_Ten + ", Số CCCD: " + ts.ThiSinh_CCCD + " đã đăng ký nguyện vọng mới." +

                     " <br/><b>Thông tin nguyện vọng:</b><br/>" +
                     " <p> Phương thức đăng ký: Phương thức " + ToHop[ToHop.Length - 1].ToString() + "</p>" +
                     " <p> Mã ngành: " + nganh.Nganh_MaNganh + " </p>" +
                     " <p> Tên ngành: " + nganh.Nganh_TenNganh + " </p>" +
                     " <p> Đơn vị tổ chức: " + student.Dkxt_DonViToChuc + " </p>" +
                     " <p> Kết quả: " + student.Dkxt_KetQuaDatDuoc + " </p>" +
                     " <p> Ngày dự thi: " + student.Dkxt_NgayDuThi + " </p>";
                SendEmail s = new SendEmail();
                s.Sendemail("xettuyen@hdu.edu.vn", body, subject);

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        public JsonResult Delete(string idDkxt, string MaToHop)
        {
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

        public JsonResult upData(string idDkxt, string MaToHop)
        {
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