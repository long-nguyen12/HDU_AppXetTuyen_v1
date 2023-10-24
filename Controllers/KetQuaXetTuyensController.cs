using HDU_AppXetTuyen.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HDU_AppXetTuyen.Controllers
{
    public class KetQuaXetTuyensController : Controller
    {
        private DbConnecttion db = new DbConnecttion();
        // GET: KetQuaXetTuyens
        [ThiSinhSessionCheck]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllNguyenVongs()
        {
            try
            {
                var session = Session["login_session"].ToString();
                var thiSinh = db.ThiSinhDangKies.Where(t => t.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

                var nguyenvongTHPTQG = db.DangKyXetTuyenKQTQGs.Include(x => x.Nganh).Include(x => x.ToHopMon).Include(x => x.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
                {
                    Ptxt_ID = n.Ptxt_ID,
                    Nganh_ID = new { NganhXet = n.Nganh.Nganh_GhiChu },
                    Thm_ID = new { ThmXet = n.ToHopMon.Thm_MaToHop + " " + n.ToHopMon.Thm_TenToHop },                 

                    Dkxt_TrangThai = n.Dkxt_KQTQG_TrangThai_HoSo,
                    Dkxt_NguyenVong = n.Dkxt_KQTQG_NguyenVong,
                    DotXT_ID = n.DotXT_ID,
                    Dkxt_Diem_Tong = n.Dkxt_KQTQG_Diem_Tong,
                    Dkxt_Diem_Tong_Full = n.Dkxt_KQTQG_TongDiem_Full,
                    Dkxt_TrangThai_KetQua = n.Dkxt_KQTQG_TrangThai_KetQua,
                    Dkxt_TrangThai_HoSo = n.Dkxt_KQTQG_TrangThai_HoSo,
                    Dkxt_TrangThai_KinhPhi = n.Dkxt_KQTQG_TrangThai_KinhPhi,
                }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

                var nguyenvongHocBa = db.DangKyXetTuyenHBs.Include(x => x.Nganh).Include(x => x.ToHopMon).Include(x => x.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
                {
                    Ptxt_ID = n.Ptxt_ID,
                    Nganh_ID = new { NganhXet = n.Nganh.Nganh_GhiChu },                   
                    Thm_ID = new { ThmXet = n.ToHopMon.Thm_MaToHop + " " + n.ToHopMon.Thm_TenToHop },                   
                   
                    Dkxt_NguyenVong = n.Dkxt_HB_NguyenVong,
                    DotXT_ID = n.DotXT_ID,
                    Dkxt_Diem_Tong = n.Dkxt_HB_Diem_Tong,
                    Dkxt_Diem_Tong_Full = n.Dkxt_HB_Diem_Tong_Full,
                    Dkxt_TrangThai_KetQua = n.Dkxt_HB_TrangThai_KetQua,
                    Dkxt_TrangThai_HoSo = n.Dkxt_HB_TrangThai_HoSo,
                    Dkxt_TrangThai_KinhPhi = n.Dkxt_HB_TrangThai_KinhPhi,
                }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
               
                var nguyenVongTuyenThang = db.DangKyXetTuyenThangs.Include(x => x.Nganh).Include(x => x.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
                {
                    Ptxt_ID = n.Ptxt_ID,
                    Nganh_ID = new { NganhXet = n.Nganh.Nganh_GhiChu },                  
                    Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                    DotXT_ID = n.DotXT_ID,
                    Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                    Dkxt_NamDatGiai = n.Dkxt_NamDatGiai,
                    Dkxt_MonDatGiai = n.Dkxt_MonDatGiai,               
                    Dkxt_LoaiGiai = n.Dkxt_LoaiGiai,

                    Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                    Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,
                }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

                var nguyenVongNgoaiNgu = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.ChungChi).Include(x => x.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP5")).Select(n => new
                {
                    Ptxt_ID = n.Ptxt_ID,
                    Nganh_ID = new { NganhXet = n.Nganh.Nganh_GhiChu },
                    Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                    DotXT_ID = n.DotXT_ID,
                    Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                    Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                    ChungChi_ID =  new { chungChi_Ten = n.ChungChi.ChungChi_Ten },
                    Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                    Dkxt_TongDiem = n.Dkxt_TongDiem,
                    Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,

                    Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                    Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,

                }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

                var nguyenVongDanhGia = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.ChungChi).Include(x => x.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP6")).Select(n => new
                {
                    Ptxt_ID = n.Ptxt_ID,
                    Nganh_ID = new { NganhXet = n.Nganh.Nganh_GhiChu },                   
                    Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                    DotXT_ID = n.DotXT_ID,
                    Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                    Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                    ChungChi_ID =  new { chungChi_Ten = n.ChungChi.ChungChi_Ten },
                    Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                    Dkxt_TongDiem = n.Dkxt_TongDiem,
                    Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                    Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                    Dkxt_TrangThai_HoSo = n.Dkxt_TrangThai_HoSo,
                }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

                var dangkyDuThiNK = db.DangKyDuThiNangKhieus.Include(x => x.Nganh).Include(x => x.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
                {
                    Ptxt_ID = n.Ptxt_ID,                   
               
                    Nganh_ID = new { NganhXet = n.Nganh.Nganh_GhiChu },
                    Dkxt_MonThi = n.Dkdt_NK_MonThi,
                    DotXT_ID = new { TenDot = n.DotXetTuyen.Dxt_Ten },
                    Dkdt_NK_NgayThi = new{ NgayThi = n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                    Dkxt_TrangThai_KetQua = n.Dkdt_TrangThai_KetQua,
                    Dkxt_TrangThai_HoSo = n.Dkdt_TrangThai_HoSo,
        
                }).ToList();

                return Json(new
                {
                    success = true,
                    ThptQg = nguyenvongTHPTQG,
                    HocBa = nguyenvongHocBa,
                    TuyenThang = nguyenVongTuyenThang,
                    NgoaiNgu = nguyenVongNgoaiNgu,
                    DanhGia = nguyenVongDanhGia, 
                    DtNangKhieu = dangkyDuThiNK
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return Json(new
                {
                    success = false
                });
            }
        }
    }
}