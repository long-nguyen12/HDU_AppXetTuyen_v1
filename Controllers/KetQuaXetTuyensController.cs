using HDU_AppXetTuyen.Models;
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
                var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();
                var nguyenvongs = db.DangKyXetTuyens.Include(l => l.Nganh).Include(l => l.ToHopMon).Include(l => l.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
                {
                    Ptxt_ID = n.Ptxt_ID,
                    Nganh_ID = new
                    {
                        Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                        NganhTenNganh = n.Nganh.NganhTenNganh
                    },
                    Thm = new
                    {
                        ToHopMon = n.ToHopMon.Thm_MaToHop
                    },
                    Dkxt_TrangThai = n.Dkxt_TrangThai,
                    Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                    DotXT_ID = n.DotXT_ID,
                    Dkxt_Diem_Tong = n.Dkxt_Diem_Tong,
                    Dkxt_Diem_Tong_Full = n.Dkxt_Diem_Tong_Full,
                    Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
                var nguyenVongTuyenThang = db.DangKyXetTuyenThangs.Include(l => l.Nganh).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
                {
                    Ptxt_ID = n.Ptxt_ID,
                    Nganh_ID = new
                    {
                        Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                        NganhTenNganh = n.Nganh.NganhTenNganh
                    },
                    Dkxt_TrangThai = n.Dkxt_TrangThai,
                    Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                    DotXT_ID = n.DotXT_ID,
                    Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                    Dkxt_MonDatGiai = n.Dkxt_MonDatGiai,
                    Dkxt_NamDatGiai = n.Dkxt_NamDatGiai,
                    Dkxt_LoaiGiai = n.Dkxt_LoaiGiai,
                    Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
                var nguyenVongNgoaiNgu = db.DangKyXetTuyenKhacs.Include(l => l.Nganh).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP5")).Select(n => new
                {
                    Ptxt_ID = n.Ptxt_ID,
                    Nganh_ID = new
                    {
                        Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                        NganhTenNganh = n.Nganh.NganhTenNganh
                    },
                    Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                    Dkxt_TrangThai = n.Dkxt_TrangThai,
                    Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                    DotXT_ID = n.DotXT_ID,
                    Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                    Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                    Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                    Dkxt_TongDiem = n.Dkxt_TongDiem,
                    Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
                var nguyenVongDanhGia = db.DangKyXetTuyenKhacs.Include(l => l.Nganh).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP6")).Select(n => new
                {
                    Ptxt_ID = n.Ptxt_ID,
                    Nganh_ID = new
                    {
                        Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                        NganhTenNganh = n.Nganh.NganhTenNganh
                    },
                    Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                    Dkxt_TrangThai = n.Dkxt_TrangThai,
                    Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                    DotXT_ID = n.DotXT_ID,
                    Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                    Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                    Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                    Dkxt_TongDiem = n.Dkxt_TongDiem,
                    Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

                return Json(new
                {
                    success = true,
                    HocBa = nguyenvongs,
                    TuyenThang = nguyenVongTuyenThang,
                    NgoaiNgu = nguyenVongNgoaiNgu,
                    DanhGia = nguyenVongDanhGia
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