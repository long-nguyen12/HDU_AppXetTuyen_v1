using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using HDU_AppXetTuyen.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace HDU_AppXetTuyen.Controllers
{


    public class LePhiXetTuyensController : Controller
    {
        private DbConnecttion db = null;

        // GET: LePhiXetTuyens
        [ThiSinhSessionCheck]
        public ActionResult Index()
        {
            db = new DbConnecttion();          
            var dxt_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            ViewBag.Dxt_ThoiGian_BatDau = dxt_present.Dxt_ThoiGian_BatDau;
            ViewBag.Dxt_ThoiGian_KetThuc = dxt_present.Dxt_ThoiGian_KetThuc;
            return View();
        }

        [HttpGet]
        [ThiSinhSessionCheck]
        public JsonResult GetAllLePhiOLD()
        {
            db = new DbConnecttion();
            var session = Session["login_session"].ToString();
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var nguyenvongTHPTQG = db.DangKyXetTuyenKQTQGs.Include(l => l.Nganh).Include(l => l.Nganh).Include(l => l.ToHopMon).Include(l => l.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Thm = new
                {
                    ThmMaTen = n.ToHopMon.Thm_MaTen,
                },
                Dkxt_ID = n.Dkxt_KQTQG_ID,

                Dkxt_NguyenVong = n.Dkxt_KQTQG_NguyenVong,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_Diem_Tong = n.Dkxt_KQTQG_Diem_Tong,
                Dkxt_Diem_Tong_Full = n.Dkxt_KQTQG_TongDiem_Full,
                Dkxt_TrangThai_KetQua = n.Dkxt_KQTQG_TrangThai_KetQua,
                Dkxt_TrangThai_HoSo = n.Dkxt_KQTQG_TrangThai_HoSo,
                Dkxt_TrangThai_KinhPhi = n.Dkxt_KQTQG_TrangThai_KinhPhi,
                Dkxt_NgayDK = n.Dkxt_KQTQG_NgayDangKy,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            var nguyenvongHocBa = db.DangKyXetTuyenHBs.Include(l => l.Nganh).Include(l => l.ToHopMon).Include(l => l.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Thm = new
                {
                    ThmMaTen = n.ToHopMon.Thm_MaTen,
                },
                Dkxt_ID = n.Dkxt_HB_ID,
                Dkxt_NguyenVong = n.Dkxt_HB_NguyenVong,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_Diem_Tong = n.Dkxt_HB_Diem_Tong,
                Dkxt_Diem_Tong_Full = n.Dkxt_HB_Diem_Tong_Full,
                Dkxt_NgayDK = n.Dkxt_HB_NgayDangKy,
                Dkxt_TrangThai_KetQua = n.Dkxt_HB_TrangThai_KetQua,
                Dkxt_TrangThai_HoSo = n.Dkxt_HB_TrangThai_HoSo,
                Dkxt_TrangThai_KinhPhi = n.Dkxt_HB_TrangThai_KinhPhi,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            var nguyenVongTuyenThang = db.DangKyXetTuyenThangs.Include(l => l.Nganh).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Dkxt_ID = n.Dkxt_ID,
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_MonDatGiai = n.Dkxt_MonDatGiai,
                Dkxt_NamDatGiai = n.Dkxt_NamDatGiai,
                Dkxt_LoaiGiai = n.Dkxt_LoaiGiai,
                Dkxt_NgayDK = n.Dkxt_NgayDangKy,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,

            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            var nguyenVongNgoaiNgu = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.ChungChi).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP5")).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                ChungChi_ID = new { Ten = n.ChungChi.ChungChi_Ten },
                Dkxt_ID = n.Dkxt_ID,

                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                Dkxt_TongDiem = n.Dkxt_TongDiem,
                Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                Dkxt_NgayDK = n.Dkxt_NgayDangKy,

                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,

            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
            //var nguyenVongNgoaiNgu = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.ChungChi).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP5")).Select(n => new
            var nguyenVongDanhGia = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.ChungChi).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP6")).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },

                ChungChi_ID = new { Ten = n.ChungChi.ChungChi_Ten },
                Dkxt_ID = n.Dkxt_ID,
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                Dkxt_TongDiem = n.Dkxt_TongDiem,
                Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            var dangkyduthiNK = db.DangKyDuThiNangKhieus.Include(x => x.Nganh).Include(x => x.ToHopMon).Include(x => x.DotXetTuyen).Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(x => new
            {
                Ptxt_ID = x.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = x.Nganh.Nganh_MaNganh,
                    NganhTenNganh = x.Nganh.Nganh_TenNganh
                },
                Thm = new
                {
                    ToHopMon = x.ToHopMon.Thm_MaToHop
                },
                Dkxt_ID = x.Dkdt_NK_ID,
                Dkxt_TrangThai_HoSo = x.Dkdt_TrangThai_HoSo,

                DotXT_ID = new { TenDotXet = "Ngày " + x.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_TrangThai_KetQua = x.Dkdt_TrangThai_KetQua,

            }).ToList();

            return Json(new
            {
                success = true,
                TrungHocPT = nguyenvongTHPTQG,
                HocBa = nguyenvongHocBa,
                TuyenThang = nguyenVongTuyenThang,
                NgoaiNgu = nguyenVongNgoaiNgu,
                DanhGia = nguyenVongDanhGia,
                DuThiNK = dangkyduthiNK
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllLePhi()
        {
            db = new DbConnecttion();
            var session = Session["login_session"].ToString();
            var DotXT_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 1 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var nguyenvongTHPTQG = db.DangKyXetTuyenKQTQGs.Include(n => n.Nganh).Include(t => t.ToHopMon).Include(d => d.DotXetTuyen).Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID && x.DotXT_ID == DotXT_present.Dxt_ID).Select(n => new
            {
                Dkxt_ID = n.Dkxt_KQTQG_ID,
                Ptxt_ID = n.Ptxt_ID,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_NguyenVong = n.Dkxt_KQTQG_NguyenVong,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Thm = new
                {
                    ThmMaTen = n.ToHopMon.Thm_MaTen,
                },
                Dkxt_NgayDK = n.Dkxt_KQTQG_NgayDangKy,
                Dkxt_Diem_Tong = n.Dkxt_KQTQG_Diem_Tong,
                Dkxt_Diem_Tong_Full = n.Dkxt_KQTQG_TongDiem_Full,
                Dkxt_TrangThai_HoSo = n.Dkxt_KQTQG_TrangThai_HoSo,
                Dkxt_TrangThai_KinhPhi = n.Dkxt_KQTQG_TrangThai_KinhPhi,
                Dkxt_TrangThai_KetQua = n.Dkxt_KQTQG_TrangThai_KetQua,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            //var  = db..Include(l => l.Nganh).Include(l => l.ToHopMon).Include(l => l.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
            var nguyenvongHocBa = db.DangKyXetTuyenHBs.Include(n => n.Nganh).Include(t => t.ToHopMon).Include(d => d.DotXetTuyen).Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID && x.DotXT_ID == DotXT_present.Dxt_ID).Select(n => new
            {
                Dkxt_ID = n.Dkxt_HB_ID,
                Ptxt_ID = n.Ptxt_ID,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_NguyenVong = n.Dkxt_HB_NguyenVong,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Thm = new
                {
                    ThmMaTen = n.ToHopMon.Thm_MaTen,
                },
                Dkxt_NgayDK = n.Dkxt_HB_NgayDangKy,
                Dkxt_Diem_Tong = n.Dkxt_HB_Diem_Tong,
                Dkxt_Diem_Tong_Full = n.Dkxt_HB_Diem_Tong_Full,
                Dkxt_TrangThai_HoSo = n.Dkxt_HB_TrangThai_HoSo,
                Dkxt_TrangThai_KinhPhi = n.Dkxt_HB_TrangThai_KinhPhi,
                Dkxt_TrangThai_KetQua = n.Dkxt_HB_TrangThai_KetQua,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            var nguyenVongTuyenThang = db.DangKyXetTuyenThangs.Include(n => n.Nganh).Include(d => d.DotXetTuyen).Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID && x.DotXT_ID == DotXT_present.Dxt_ID).Select(n => new
            {
                Dkxt_ID = n.Dkxt_ID,
                Ptxt_ID = n.Ptxt_ID,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_MonDatGiai = n.Dkxt_MonDatGiai,
                Dkxt_NamDatGiai = n.Dkxt_NamDatGiai,
                Dkxt_LoaiGiai = n.Dkxt_LoaiGiai,
                Dkxt_NgayDK = n.Dkxt_NgayDangKy,
                Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,
                Dkxt_TrangThai_KinhPhi = n.Dkxt_TrangThai_KinhPhi,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,

            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            var nguyenVongNgoaiNgu = db.DangKyXetTuyenKhacs.Include(n => n.Nganh).Include(c => c.ChungChi).Include(d => d.DotXetTuyen)
            .Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID && x.Dkxt_ToHopXT.Equals("HDP5") && x.DotXT_ID == DotXT_present.Dxt_ID).Select(n => new
            {
                Dkxt_ID = n.Dkxt_ID,
                Ptxt_ID = n.Ptxt_ID,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                ChungChi_ID = new { Ten = n.ChungChi.ChungChi_Ten },
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                Dkxt_TongDiem = n.Dkxt_TongDiem,
                Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                Dkxt_NgayDK = n.Dkxt_NgayDangKy,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,
                Dkxt_TrangThai_KinhPhi = n.Dkxt_TrangThai_KinhPhi,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
            //var nguyenVongDanhGia = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.ChungChi).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP6")).Select(n => new
            var nguyenVongDanhGia = db.DangKyXetTuyenKhacs.Include(n => n.Nganh).Include(c => c.ChungChi).Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID && x.Dkxt_ToHopXT.Equals("HDP6") && x.DotXT_ID == DotXT_present.Dxt_ID)
                                     .Include(d => d.DotXetTuyen).Select(n => new
                                     {
                                         Dkxt_ID = n.Dkxt_ID,
                                         Ptxt_ID = n.Ptxt_ID,
                                         DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                                         Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                                         Nganh_ID = new
                                         {
                                             Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                                             NganhTenNganh = n.Nganh.Nganh_TenNganh
                                         },
                                         ChungChi_ID = new { Ten = n.ChungChi.ChungChi_Ten },
                                         Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                                         Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                                         Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                                         Dkxt_TongDiem = n.Dkxt_TongDiem,
                                         Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                                         Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                                         Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,
                                         Dkxt_TrangThai_KinhPhi = n.Dkxt_TrangThai_KinhPhi,
                                     }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            return Json(new
            {
                success = true,
                TrungHocPT = nguyenvongTHPTQG,
                HocBa = nguyenvongHocBa,
                TuyenThang = nguyenVongTuyenThang,
                NgoaiNgu = nguyenVongNgoaiNgu,
                DanhGia = nguyenVongDanhGia,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAll_Nv_ThiSinh_XtDungKQTHPTQG()
        {
            db = new DbConnecttion();

            var session = Session["login_session"].ToString();
            var DotXT_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var nguyenvongTHPTQG = db.DangKyXetTuyenKQTQGs.Include(n => n.Nganh).Include(t => t.ToHopMon).Include(d => d.DotXetTuyen).Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID && x.DotXT_ID == DotXT_present.Dxt_ID).Select(n => new
            {
                Dkxt_ID = n.Dkxt_KQTQG_ID,
                Ptxt_ID = n.Ptxt_ID,
                DotXT_ID = new { Dxt_ThoiGian_BatDau = n.DotXetTuyen.Dxt_ThoiGian_BatDau, Dxt_ThoiGian_KetThuc = n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_NguyenVong = n.Dkxt_KQTQG_NguyenVong,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Thm = new
                {
                    ThmMaTen = n.ToHopMon.Thm_MaToHop +" - "+ n.ToHopMon.Thm_TenToHop,
                },

                Dkxt_NgayDK = n.Dkxt_KQTQG_NgayDangKy,
                Dkxt_Diem_Tong = n.Dkxt_KQTQG_Diem_Tong,
                Dkxt_Diem_Tong_Full = n.Dkxt_KQTQG_TongDiem_Full,
                Dkxt_TrangThai_HoSo = n.Dkxt_KQTQG_TrangThai_HoSo,
                Dkxt_TrangThai_KinhPhi = n.Dkxt_KQTQG_TrangThai_KinhPhi,
                Dkxt_TrangThai_KetQua = n.Dkxt_KQTQG_TrangThai_KetQua,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
            return Json(new
            {
                success = true,
                TrungHocPT = nguyenvongTHPTQG
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAll_Nv_ThiSinh_XtDungHocBa()
        {
            db = new DbConnecttion();
            var session = Session["login_session"].ToString();
            var DotXT_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var nguyenvongHocBa = db.DangKyXetTuyenHBs.Include(n => n.Nganh).Include(t => t.ToHopMon).Include(d => d.DotXetTuyen).Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID && x.DotXT_ID == DotXT_present.Dxt_ID).Select(n => new
            {
                Dkxt_ID = n.Dkxt_HB_ID,
                Ptxt_ID = n.Ptxt_ID,
                DotXT_ID = new { Dxt_ThoiGian_BatDau = n.DotXetTuyen.Dxt_ThoiGian_BatDau, Dxt_ThoiGian_KetThuc = n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_NguyenVong = n.Dkxt_HB_NguyenVong,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Thm = new
                {
                    ThmMaTen = n.ToHopMon.Thm_MaTen,
                },
                Dkxt_NgayDK = n.Dkxt_HB_NgayDangKy,
                Dkxt_Diem_Tong = n.Dkxt_HB_Diem_Tong,
                Dkxt_Diem_Tong_Full = n.Dkxt_HB_Diem_Tong_Full,
                Dkxt_TrangThai_HoSo = n.Dkxt_HB_TrangThai_HoSo,
                Dkxt_TrangThai_KinhPhi = n.Dkxt_HB_TrangThai_KinhPhi,
                Dkxt_TrangThai_KetQua = n.Dkxt_HB_TrangThai_KetQua,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
            return Json(new
            {
                success = true,
                HocBa = nguyenvongHocBa,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAll_Nv_ThiSinh_Xt_TuyenThang()
        {
            db = new DbConnecttion();
            var session = Session["login_session"].ToString();
            var DotXT_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var nguyenVongTuyenThang = db.DangKyXetTuyenThangs.Include(n => n.Nganh).Include(d => d.DotXetTuyen).Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID && x.DotXT_ID == DotXT_present.Dxt_ID).Select(n => new
            {
                Dkxt_ID = n.Dkxt_ID,
                Ptxt_ID = n.Ptxt_ID,
                DotXT_ID = new { Dxt_ThoiGian_BatDau = n.DotXetTuyen.Dxt_ThoiGian_BatDau, Dxt_ThoiGian_KetThuc = n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_MonDatGiai = n.Dkxt_MonDatGiai,
                Dkxt_NamDatGiai = n.Dkxt_NamDatGiai,
                Dkxt_LoaiGiai = n.Dkxt_LoaiGiai,
                Dkxt_NgayDK = n.Dkxt_NgayDangKy,
                Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,
                Dkxt_TrangThai_KinhPhi = n.Dkxt_TrangThai_KinhPhi,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,

            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            return Json(new
            {
                success = true,
                TuyenThang = nguyenVongTuyenThang,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAll_Nv_ThiSinh_Xt_DungCCNgoaiNgu()
        {
            db = new DbConnecttion();
            var session = Session["login_session"].ToString();
            var DotXT_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var nguyenVongNgoaiNgu = db.DangKyXetTuyenKhacs.Include(n => n.Nganh).Include(c => c.ChungChi).Include(d => d.DotXetTuyen)
            .Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID && x.Dkxt_ToHopXT.Equals("HDP5") && x.DotXT_ID == DotXT_present.Dxt_ID).Select(n => new
            {
                Dkxt_ID = n.Dkxt_ID,
                Ptxt_ID = n.Ptxt_ID,
                DotXT_ID = new { Dxt_ThoiGian_BatDau = n.DotXetTuyen.Dxt_ThoiGian_BatDau, Dxt_ThoiGian_KetThuc = n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                ChungChi_ID = new { Ten = n.ChungChi.ChungChi_Ten },
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                Dkxt_TongDiem = n.Dkxt_TongDiem,
                Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                Dkxt_NgayDK = n.Dkxt_NgayDangKy,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,
                Dkxt_TrangThai_KinhPhi = n.Dkxt_TrangThai_KinhPhi,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
            return Json(new
            {
                success = true,

                NgoaiNgu = nguyenVongNgoaiNgu,

            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAll_Nv_ThiSinh_Xt_KQDGNangLuc()
        {
            db = new DbConnecttion();
            var session = Session["login_session"].ToString();
            var DotXT_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();


            //var nguyenVongDanhGia = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.ChungChi).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP6")).Select(n => new
            var nguyenVongDanhGia = db.DangKyXetTuyenKhacs.Include(n => n.Nganh).Include(c => c.ChungChi).Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID && x.Dkxt_ToHopXT.Equals("HDP6") && x.DotXT_ID == DotXT_present.Dxt_ID)
            .Include(d => d.DotXetTuyen).Select(n => new
            {
                Dkxt_ID = n.Dkxt_ID,
                Ptxt_ID = n.Ptxt_ID,
                DotXT_ID = new { Dxt_ThoiGian_BatDau = n.DotXetTuyen.Dxt_ThoiGian_BatDau, Dxt_ThoiGian_KetThuc = n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                ChungChi_ID = new { Ten = n.ChungChi.ChungChi_Ten},
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                Dkxt_TongDiem = n.Dkxt_TongDiem,
                Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                Dkxt_NgayDK = n.Dkxt_NgayDangKy,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,
                Dkxt_TrangThai_KinhPhi = n.Dkxt_TrangThai_KinhPhi,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            return Json(new
            {
                success = true,
                DanhGia = nguyenVongDanhGia,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadLePhiMinhChung()
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
        public JsonResult Create(KinhPhiInfo entity)
        {
            db = new DbConnecttion();
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();

            var Dkxt_ID = int.Parse(entity.key_dkxt_id);
            var Ptxt_ID = int.Parse(entity.key_dkxt_pt);
            if (Ptxt_ID == 2)
            {
                var model = db.DangKyXetTuyenKQTQGs.FirstOrDefault(x => x.Dkxt_KQTQG_ID == Dkxt_ID);              
                if (!String.IsNullOrEmpty(entity.KinhPhi_TepMinhChung)) { model.Dkxt_KQTQG_KinhPhi_TepMinhChung = entity.KinhPhi_TepMinhChung; }
                model.Dkxt_KQTQG_KinhPhi_SoThamChieu = entity.KinhPhi_SoTC;
                model.Dkxt_KQTQG_TrangThai_KinhPhi = 1;
                model.Dkxt_KQTQG_KinhPhi_NgayThang_NopMC = DateTime.Now.ToString("yyyy-MM-dd");
                db.SaveChanges();
            }
            if (Ptxt_ID == 3)
            {
                var model = db.DangKyXetTuyenHBs.FirstOrDefault(x => x.Dkxt_HB_ID == Dkxt_ID);
                if (!String.IsNullOrEmpty(entity.KinhPhi_TepMinhChung)) { model.Dkxt_HB_KinhPhi_TepMinhChung = entity.KinhPhi_TepMinhChung; }                
                model.Dkxt_HB_KinhPhi_SoThamChieu = entity.KinhPhi_SoTC;
                model.Dkxt_HB_TrangThai_KinhPhi = 1;
                model.Dkxt_HB_KinhPhi_NgayThang_NopMC = DateTime.Now.ToString("yyyy-MM-dd");
                db.SaveChanges();
            }
            if (Ptxt_ID == 4)
            {
                var model = db.DangKyXetTuyenThangs.FirstOrDefault(x => x.Dkxt_ID == Dkxt_ID);              
                if (!String.IsNullOrEmpty(entity.KinhPhi_TepMinhChung)) { model.Dkxt_KinhPhi_TepMinhChung = entity.KinhPhi_TepMinhChung; }
                model.Dkxt_KinhPhi_SoThamChieu = entity.KinhPhi_SoTC;
                model.Dkxt_TrangThai_KinhPhi = 1;
                model.Dkxt_KinhPhi_NgayThang_NopMC = DateTime.Now.ToString("yyyy-MM-dd");
                db.SaveChanges();
            }
            if (Ptxt_ID == 5 || Ptxt_ID == 6)
            {
                var model = db.DangKyXetTuyenKhacs.FirstOrDefault(x => x.Dkxt_ID == Dkxt_ID);
                if (!String.IsNullOrEmpty(entity.KinhPhi_TepMinhChung)) { model.Dkxt_KinhPhi_TepMinhChung = entity.KinhPhi_TepMinhChung; }               
                model.Dkxt_KinhPhi_SoThamChieu = entity.KinhPhi_SoTC;
                model.Dkxt_TrangThai_KinhPhi = 1;
                model.Dkxt_KinhPhi_NgayThang_NopMC = DateTime.Now.ToString("yyyy-MM-dd");
                db.SaveChanges();
            }
            return Json(new { success = true, data = new { Dkxt_ID, Ptxt_ID } }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit(KinhPhiInfo entity)
        {
            db = new DbConnecttion();
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();

            var Dkxt_ID = int.Parse(entity.key_dkxt_id);
            var Ptxt_ID = int.Parse(entity.key_dkxt_pt);
            if (Ptxt_ID == 2)
            {
                var model = db.DangKyXetTuyenKQTQGs.FirstOrDefault(x => x.Dkxt_KQTQG_ID == Dkxt_ID);
                model.Dkxt_KQTQG_KinhPhi_TepMinhChung += entity.KinhPhi_TepMinhChung;
                model.Dkxt_KQTQG_KinhPhi_SoThamChieu = entity.KinhPhi_SoTC;
                model.Dkxt_KQTQG_TrangThai_KinhPhi = 1;
                model.Dkxt_KQTQG_KinhPhi_NgayThang_NopMC = DateTime.Now.ToString("yyyy-MM-dd");
                db.SaveChanges();
            }
            if (Ptxt_ID == 3)
            {
                var model = db.DangKyXetTuyenHBs.FirstOrDefault(x => x.Dkxt_HB_ID == Dkxt_ID);

                model.Dkxt_HB_KinhPhi_TepMinhChung += entity.KinhPhi_TepMinhChung;
                model.Dkxt_HB_KinhPhi_SoThamChieu = entity.KinhPhi_SoTC;
                model.Dkxt_HB_TrangThai_KinhPhi = 1;
                model.Dkxt_HB_KinhPhi_NgayThang_NopMC = DateTime.Now.ToString("yyyy-MM-dd");

                db.SaveChanges();
            }
            if (Ptxt_ID == 4)
            {
                var model = db.DangKyXetTuyenThangs.FirstOrDefault(x => x.Dkxt_ID == Dkxt_ID);

                model.Dkxt_KinhPhi_TepMinhChung += entity.KinhPhi_TepMinhChung;
                model.Dkxt_KinhPhi_SoThamChieu = entity.KinhPhi_SoTC;
                model.Dkxt_TrangThai_KinhPhi = 1;
                model.Dkxt_KinhPhi_NgayThang_NopMC = DateTime.Now.ToString("yyyy-MM-dd");

                db.SaveChanges();
            }
            if (Ptxt_ID == 5 || Ptxt_ID == 6)
            {
                var model = db.DangKyXetTuyenKhacs.FirstOrDefault(x => x.Dkxt_ID == Dkxt_ID);
                model.Dkxt_KinhPhi_TepMinhChung += entity.KinhPhi_TepMinhChung;
                model.Dkxt_KinhPhi_SoThamChieu = entity.KinhPhi_SoTC;
                model.Dkxt_TrangThai_KinhPhi = 1;
                model.Dkxt_KinhPhi_NgayThang_NopMC = DateTime.Now.ToString("yyyy-MM-dd");
                db.SaveChanges();
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetByID(KinhPhiInfo entity)  // lấy thông tin kinh phí đã nộp
        {
            db = new DbConnecttion();
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();

            var Dkxt_ID = int.Parse(entity.key_dkxt_id);
            var Ptxt_ID = int.Parse(entity.key_dkxt_pt);

            KinhPhiInfo DataReturn = new KinhPhiInfo();
            DataReturn.key_dkxt_id = entity.key_dkxt_id;
            DataReturn.key_dkxt_pt = entity.key_dkxt_pt;
           

            if (Ptxt_ID == 2)
            {
                var model = db.DangKyXetTuyenKQTQGs.FirstOrDefault(x => x.Dkxt_KQTQG_ID == Dkxt_ID);            
                DataReturn.KinhPhi_TepMinhChung = model.Dkxt_KQTQG_KinhPhi_TepMinhChung;
                DataReturn.KinhPhi_SoTC = model.Dkxt_KQTQG_KinhPhi_SoThamChieu;
            }
            if (Ptxt_ID == 3)
            {
                var model = db.DangKyXetTuyenHBs.FirstOrDefault(x => x.Dkxt_HB_ID == Dkxt_ID);               
                DataReturn.KinhPhi_TepMinhChung = model.Dkxt_HB_KinhPhi_TepMinhChung;
                DataReturn.KinhPhi_SoTC = model.Dkxt_HB_KinhPhi_SoThamChieu;
            }
            if (Ptxt_ID == 4)
            {
                var model = db.DangKyXetTuyenThangs.FirstOrDefault(x => x.Dkxt_ID == Dkxt_ID);
                DataReturn.KinhPhi_TepMinhChung = model.Dkxt_KinhPhi_TepMinhChung;
                DataReturn.KinhPhi_SoTC = model.Dkxt_KinhPhi_SoThamChieu;
            }
            if (Ptxt_ID == 5 || Ptxt_ID == 6)
            {
                var model = db.DangKyXetTuyenKhacs.FirstOrDefault(x => x.Dkxt_ID == Dkxt_ID);
                DataReturn.KinhPhi_TepMinhChung = model.Dkxt_KinhPhi_TepMinhChung;
                DataReturn.KinhPhi_SoTC = model.Dkxt_KinhPhi_SoThamChieu;
            }
            return Json(new { success = true, data = DataReturn }, JsonRequestBehavior.AllowGet);
        }
       
    }
}
