using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using OfficeOpenXml;
using System.IO;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Xceed.Words.NET;
using System.Net.Mail;
using System.Xml.Linq;
using Microsoft.Ajax.Utilities;
using System.Web.Script.Serialization;
using System.Data.Entity.Validation;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class TongHopSoLieuDKXTsController : Controller
    {
        private DbConnecttion db = null;
        #region Theo dõi hồ sơ thí sinh
        private IList<TongHopSoLieuXetTuyen> ListHoSoXetTuyen;
        public ActionResult TheoDoiNopHoSo(string filteriPhuongThuc, string filteriNganhHoc, string filteriLePhi, string filteriHoSo, string searchString, string currentFilter,
            string filteriDotxt, string sortOrder, int? page)
        {
            ListHoSoXetTuyen = new List<TongHopSoLieuXetTuyen>();
            db = new DbConnecttion();
            #region Lấy dữ liệu ở các bảng xét tuyển đưa vào list ảo
            var model_dxt_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            var model_xt2 = db.DangKyXetTuyenKQTQGs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_KQTQG_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                PtxtID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_KQTQG_NguyenVong,
                NgayDangKy = s.Dkxt_KQTQG_NgayDangKy,
                NganhID = s.Nganh_ID,
                TenNganh = s.Nganh.Nganh_TenNganh,
                TenToHop = s.ToHopMon.Thm_TenToHop,
                MinhChung_HB = s.Dkxt_KQTQG_MinhChung_HocBa,
                MinhChung_Bang = s.Dkxt_KQTQG_MinhChung_BangTN,
                MinhChung_CCCD = "",
                MinhChung_UuTien = s.Dkxt_KQTQG_MinhChung_UuTien,
                MinhChung_XetTuyen = s.Dkxt_KQTQG_MinhChung_CNTotNghiep,
                TrangThai_HoSo = s.Dkxt_KQTQG_TrangThai_HoSo,
                TrangThai_KetQua = s.Dkxt_KQTQG_TrangThai_KetQua,
                TrangThaiLP = s.Dkxt_KQTQG_TrangThai_KinhPhi,
                SoThamChieuLP = s.Dkxt_KQTQG_KinhPhi_SoThamChieu,
                NgayThangNopLP = s.Dkxt_KQTQG_KinhPhi_NgayThang_NopMC,
                NgayThangCheckLP = s.Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC,
                MinhChungLP = s.Dkxt_KQTQG_KinhPhi_TepMinhChung
            }).ToList();

            foreach (var item in model_xt2)
            {
                TongHopSoLieuXetTuyen item_hs = new TongHopSoLieuXetTuyen();
                item_hs.ThiSinh_ID = item.ThiSinh_ID;
                item_hs.Dkxt_ID = item.Dkxt_ID;
                item_hs.Dxt_ID = item.Dxt_ID;
                item_hs.HoDem = item.HoDem;
                item_hs.Ten = item.Ten;
                item_hs.DienThoai = item.DienThoai;
                item_hs.Ptxt_ID = int.Parse(item.PtxtID.ToString());
                item_hs.NguyenVong = item.NguyenVong.ToString();
                item_hs.NgayDangKy = item.NgayDangKy;
                item_hs.NganhID = int.Parse(item.NganhID.ToString());
                item_hs.TenNganh = item.TenNganh;
                item_hs.TenToHop = item.TenToHop;
                item_hs.MinhChung_HB = item.MinhChung_HB;
                item_hs.MinhChung_Bang = item.MinhChung_Bang;
                item_hs.MinhChung_CCCD = item.MinhChung_CCCD;
                item_hs.MinhChung_UuTien = item.MinhChung_UuTien;
                item_hs.MinhChung_XetTuyen = item.MinhChung_XetTuyen;
                item_hs.TrangThai_HoSo = item.TrangThai_HoSo.ToString();
                item_hs.TrangThai_KetQua = item.TrangThai_KetQua.ToString();
                item_hs.TrangThaiLP = item.TrangThaiLP.ToString();
                item_hs.SoThamChieuLP = item.SoThamChieuLP;
                item_hs.NgayThangNopLP = item.NgayThangNopLP;
                item_hs.NgayThangCheckLP = item.NgayThangCheckLP;
                item_hs.MinhChungLP = item.MinhChungLP;

                ListHoSoXetTuyen.Add(item_hs);
            }
            var model_xt3 = db.DangKyXetTuyenHBs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_HB_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                PtxtID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_HB_NguyenVong,
                NgayDangKy = s.Dkxt_HB_NgayDangKy,
                NganhID = s.Nganh.Nganh_ID,
                TenNganh = s.Nganh.Nganh_TenNganh,
                TenToHop = s.ToHopMon.Thm_TenToHop,

                MinhChung_HB = s.Dkxt_HB_MinhChung_HB,
                MinhChung_Bang = s.Dkxt_HB_MinhChung_Bang,
                MinhChung_CCCD = s.Dkxt_HB_MinhChung_CCCD,
                MinhChung_UuTien = s.Dkxt_HB_MinhChung_UuTien,
                MinhChung_XetTuyen = s.Dkxt_HB_MinhChung_HB,

                TrangThai_HoSo = s.Dkxt_HB_TrangThai_HoSo,
                TrangThai_KetQua = s.Dkxt_HB_TrangThai_KetQua,
                TrangThaiLP = s.Dkxt_HB_TrangThai_KinhPhi,
                SoThamChieuLP = s.Dkxt_HB_KinhPhi_SoThamChieu,
                NgayThangNopLP = s.Dkxt_HB_KinhPhi_NgayThang_NopMC,
                NgayThangCheckLP = s.Dkxt_HB_KinhPhi_NgayThang_CheckMC,
                MinhChungLP = s.Dkxt_HB_KinhPhi_TepMinhChung
            }).ToList();

            foreach (var item in model_xt3)
            {
                TongHopSoLieuXetTuyen item_hs = new TongHopSoLieuXetTuyen();
                item_hs.ThiSinh_ID = item.ThiSinh_ID;
                item_hs.Dkxt_ID = item.Dkxt_ID;
                item_hs.Dxt_ID = item.Dxt_ID;
                item_hs.HoDem = item.HoDem;
                item_hs.Ten = item.Ten;
                item_hs.DienThoai = item.DienThoai;
                item_hs.Ptxt_ID = int.Parse(item.PtxtID.ToString());
                item_hs.NguyenVong = item.NguyenVong.ToString();
                item_hs.NgayDangKy = item.NgayDangKy;
                item_hs.NganhID = int.Parse(item.NganhID.ToString());
                item_hs.TenNganh = item.TenNganh;
                item_hs.TenToHop = item.TenToHop;
                item_hs.MinhChung_HB = item.MinhChung_HB;
                item_hs.MinhChung_Bang = item.MinhChung_Bang;
                item_hs.MinhChung_CCCD = item.MinhChung_CCCD;
                item_hs.MinhChung_UuTien = item.MinhChung_UuTien;
                item_hs.MinhChung_XetTuyen = item.MinhChung_XetTuyen;
                item_hs.TrangThai_HoSo = item.TrangThai_HoSo.ToString();
                item_hs.TrangThai_KetQua = item.TrangThai_KetQua.ToString();
                item_hs.TrangThaiLP = item.TrangThaiLP.ToString();
                item_hs.SoThamChieuLP = item.SoThamChieuLP;
                item_hs.NgayThangNopLP = item.NgayThangNopLP;
                item_hs.NgayThangCheckLP = item.NgayThangCheckLP;
                item_hs.MinhChungLP = item.MinhChungLP;
                ListHoSoXetTuyen.Add(item_hs);
            }

            var model_xt4 = db.DangKyXetTuyenThangs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                PtxtID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_NguyenVong,
                NgayDangKy = s.Dkxt_NgayDangKy,
                NganhID = s.Nganh.Nganh_ID,
                TenNganh = s.Nganh.Nganh_TenNganh,
                TenToHop = s.Dkxt_MonDatGiai + " - " + s.Dkxt_LoaiGiai,//s.ToHopMon.Thm_TenToHop,

                MinhChung_HB = s.Dkxt_MinhChung_HB,
                MinhChung_Bang = s.Dkxt_MinhChung_Bang,
                MinhChung_CCCD = s.Dkxt_MinhChung_CCCD,
                MinhChung_UuTien = s.Dkxt_MinhChung_UuTien,
                MinhChung_XetTuyen = s.Dkxt_MinhChung_Giai,

                TrangThai_HoSo = s.Dkxt_TrangThai_HoSo,
                TrangThai_KetQua = s.Dkxt_TrangThai_KetQua,
                TrangThaiLP = s.Dkxt_TrangThai_KinhPhi,
                SoThamChieuLP = s.Dkxt_KinhPhi_SoThamChieu,
                NgayThangNopLP = s.Dkxt_KinhPhi_NgayThang_NopMC,
                NgayThangCheckLP = s.Dkxt_KinhPhi_NgayThang_CheckMC,
                MinhChungLP = s.Dkxt_KinhPhi_TepMinhChung
            }).ToList();
            foreach (var item in model_xt4)
            {
                TongHopSoLieuXetTuyen item_hs = new TongHopSoLieuXetTuyen();
                item_hs.ThiSinh_ID = item.ThiSinh_ID;
                item_hs.Dkxt_ID = item.Dkxt_ID;
                item_hs.Dxt_ID = item.Dxt_ID;
                item_hs.HoDem = item.HoDem;
                item_hs.Ten = item.Ten;
                item_hs.DienThoai = item.DienThoai;
                item_hs.Ptxt_ID = int.Parse(item.PtxtID.ToString());
                item_hs.NguyenVong = item.NguyenVong.ToString();
                item_hs.NgayDangKy = item.NgayDangKy;
                item_hs.NganhID = int.Parse(item.NganhID.ToString());
                item_hs.TenNganh = item.TenNganh;
                item_hs.TenToHop = item.TenToHop;
                item_hs.MinhChung_HB = item.MinhChung_HB;
                item_hs.MinhChung_Bang = item.MinhChung_Bang;
                item_hs.MinhChung_CCCD = item.MinhChung_CCCD;
                item_hs.MinhChung_UuTien = item.MinhChung_UuTien;
                item_hs.MinhChung_XetTuyen = item.MinhChung_XetTuyen;
                item_hs.TrangThai_HoSo = item.TrangThai_HoSo.ToString();
                item_hs.TrangThai_KetQua = item.TrangThai_KetQua.ToString();
                item_hs.TrangThaiLP = item.TrangThaiLP.ToString();
                item_hs.SoThamChieuLP = item.SoThamChieuLP;
                item_hs.NgayThangNopLP = item.NgayThangNopLP;
                item_hs.NgayThangCheckLP = item.NgayThangCheckLP;
                item_hs.MinhChungLP = item.MinhChungLP;
                ListHoSoXetTuyen.Add(item_hs);
            }
            var model_xt5 = db.DangKyXetTuyenKhacs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                PtxtID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_NguyenVong,
                NgayDangKy = s.Dkxt_NgayDangKy,
                NganhID = s.Nganh.Nganh_ID,
                TenNganh = s.Nganh.Nganh_TenNganh,
                TenToHop = s.ChungChi.ChungChi_Ten,//s.ToHopMon.Thm_TenToHop,

                MinhChung_HB = s.Dkxt_MinhChung_HB,
                MinhChung_Bang = s.Dkxt_MinhChung_Bang,
                MinhChung_CCCD = s.Dkxt_MinhChung_CCCD,
                MinhChung_UuTien = s.Dkxt_MinhChung_UuTien,
                MinhChung_XetTuyen = s.Dkxt_MinhChung_KetQua,

                TrangThai_HoSo = s.Dkxt_TrangThai_HoSo,
                TrangThai_KetQua = s.Dkxt_TrangThai_KetQua,
                TrangThaiLP = s.Dkxt_TrangThai_KinhPhi,
                SoThamChieuLP = s.Dkxt_KinhPhi_SoThamChieu,
                NgayThangNopLP = s.Dkxt_KinhPhi_NgayThang_NopMC,
                NgayThangCheckLP = s.Dkxt_KinhPhi_NgayThang_CheckMC,
                MinhChungLP = s.Dkxt_KinhPhi_TepMinhChung
            }).ToList();
            foreach (var item in model_xt5)
            {
                TongHopSoLieuXetTuyen item_hs = new TongHopSoLieuXetTuyen();
                item_hs.ThiSinh_ID = item.ThiSinh_ID;
                item_hs.Dkxt_ID = item.Dkxt_ID;
                item_hs.Dxt_ID = item.Dxt_ID;
                item_hs.HoDem = item.HoDem;
                item_hs.Ten = item.Ten;
                item_hs.DienThoai = item.DienThoai;
                item_hs.Ptxt_ID = int.Parse(item.PtxtID.ToString());
                item_hs.NguyenVong = item.NguyenVong.ToString();
                item_hs.NgayDangKy = item.NgayDangKy;
                item_hs.NganhID = int.Parse(item.NganhID.ToString());
                item_hs.TenNganh = item.TenNganh;
                item_hs.TenToHop = item.TenToHop;
                item_hs.MinhChung_HB = item.MinhChung_HB;
                item_hs.MinhChung_Bang = item.MinhChung_Bang;
                item_hs.MinhChung_CCCD = item.MinhChung_CCCD;
                item_hs.MinhChung_UuTien = item.MinhChung_UuTien;
                item_hs.MinhChung_XetTuyen = item.MinhChung_XetTuyen;
                item_hs.TrangThai_HoSo = item.TrangThai_HoSo.ToString();
                item_hs.TrangThai_KetQua = item.TrangThai_KetQua.ToString();
                item_hs.TrangThaiLP = item.TrangThaiLP.ToString();
                item_hs.SoThamChieuLP = item.SoThamChieuLP;
                item_hs.NgayThangNopLP = item.NgayThangNopLP;
                item_hs.NgayThangCheckLP = item.NgayThangCheckLP;
                item_hs.MinhChungLP = item.MinhChungLP;
                ListHoSoXetTuyen.Add(item_hs);
            }
            #endregion

            #region sắp xếp lại dữ liệu đã gộp
            ListHoSoXetTuyen = ListHoSoXetTuyen.OrderByDescending(x => x.NgayThangNopLP)
                                                 .ThenBy(x => x.TrangThaiLP)
                                                 .ThenByDescending(x => x.Dkxt_ID)
                                                 .ThenByDescending(x => x.ThiSinh_ID).ToList();
            #endregion

            #region lọc dữ liệu theo đợt
            var dotxts = db.DotXetTuyens.Include(x => x.NamHoc).Where(x => x.NamHoc.NamHoc_TrangThai == 1 && x.Dxt_Classify == 0).ToList();
            var _dotxt_hientai = dotxts.Where(x => x.Dxt_TrangThai_Xt == 1 && x.Dxt_Classify == 0).FirstOrDefault();
            ViewBag.DotXetTuyenHT = _dotxt_hientai.Dxt_Ten + ", từ " + (DateTime.Parse(_dotxt_hientai.Dxt_ThoiGian_BatDau)).ToString("dd/MM/yyyy") + " đến " + (DateTime.Parse(_dotxt_hientai.Dxt_ThoiGian_KetThuc)).ToString("dd/MM/yyyy");
            //ViewBag.filteriDotxt = new SelectList(dotxts.OrderBy(x => x.Dxt_ID).ToList(), "Dxt_ID", "Dxt_Ten", _dotxt_hientai.Dxt_ID);            

            #endregion

            #region lọc dữ liệu theo phương thức
            var list_items_phuongthuc = (from item in ListHoSoXetTuyen select item.Ptxt_ID).Distinct().ToList();
            List<StatusTracking> filteri_items_phuongthuc = new List<StatusTracking>();
            foreach (var _item in list_items_phuongthuc)
            {
                if (_item == 2)
                {
                    filteri_items_phuongthuc.Add(new StatusTracking() { St_ID = 2, St_Name = "Phương thức 2" });
                }
                if (_item == 3)
                {
                    filteri_items_phuongthuc.Add(new StatusTracking() { St_ID = 3, St_Name = "Phương thức 3" });
                }
                if (_item == 4)
                {
                    filteri_items_phuongthuc.Add(new StatusTracking() { St_ID = 4, St_Name = "Phương thức 4" });
                }
                if (_item == 5)
                {
                    filteri_items_phuongthuc.Add(new StatusTracking() { St_ID = 5, St_Name = "Phương thức 5" });
                }
                if (_item == 6)
                {
                    filteri_items_phuongthuc.Add(new StatusTracking() { St_ID = 6, St_Name = "Phương thức 6" });
                }
            }
            ViewBag.filteriPhuongThuc = new SelectList(filteri_items_phuongthuc.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriPhuongThuc))
            {
                ListHoSoXetTuyen = ListHoSoXetTuyen.Where(x => x.Ptxt_ID == int.Parse(filteriPhuongThuc)).ToList();
            }

            #endregion

            #region lọc dữ liệu theo ngành
            var list_items_nganh = (from item in ListHoSoXetTuyen select new { item.NganhID, item.TenNganh }).Distinct().ToList();

            ViewBag.filteriNganhHoc = new SelectList(list_items_nganh.OrderBy(x => x.TenNganh).ToList(), "NganhID", "TenNganh");

            if (!String.IsNullOrEmpty(filteriNganhHoc))
            {
                ListHoSoXetTuyen = ListHoSoXetTuyen.Where(x => x.NganhID == int.Parse(filteriNganhHoc)).ToList();
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in ListHoSoXetTuyen select item.TrangThaiLP).Distinct().ToList();
            List<StatusTracking> filteri_items_lephi = new List<StatusTracking>();

            foreach (var _item in list_items_lephi)
            {
                if (_item == "0")
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa đóng phí" });
                }
                if (_item == "1")
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã nộp, chưa KT" });
                }
                if (_item == "2")
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã KT, Có sai sót" });
                }
                if (_item == "3")
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 2, St_Name = "Sửa,  chưa KT" });
                }
                if (_item == "9")
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã KT, Đúng" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                ListHoSoXetTuyen = ListHoSoXetTuyen.Where(x => x.TrangThaiLP == filteriLePhi).ToList();
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in ListHoSoXetTuyen select item.TrangThai_HoSo).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == "0")
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "Chưa kiểm tra" });
                }
                if (_item == "1")
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã KT, Có sai sót" });
                }
                if (_item == "2")
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã sửa, Chưa KT" });
                }
                if (_item == "9")
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã KT, HS đúng, đủ" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                string _duTuyen_TrangThai = filteriHoSo;
                ListHoSoXetTuyen = ListHoSoXetTuyen.Where(x => x.TrangThai_HoSo == _duTuyen_TrangThai).ToList();
            }
            #endregion

            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                ListHoSoXetTuyen = ListHoSoXetTuyen.Where(h => h.HoDem.ToUpper().Contains(searchString.ToUpper())
                                    || h.Ten.ToUpper().Contains(searchString.ToUpper())
                                    || h.DienThoai.Contains(searchString)
                                    || h.NguyenVong.Contains(searchString)).ToList();

            }
            #endregion
            #region Sắp xếp
            ViewBag.TsHoDem = sortOrder == "hodem_asc" ? "hodem_desc" : "hodem_asc";
            ViewBag.TsTen = sortOrder == "ten_asc" ? "ten_desc" : "ten_asc";
            ViewBag.TsPhuongThuc = sortOrder == "phuongthuc_asc" ? "phuongthuc_desc" : "phuongthuc_asc";
            ViewBag.TsTenNganh = sortOrder == "tennganh_asc" ? "tennganh_desc" : "tennganh_asc";
            ViewBag.TsNguyenVong = sortOrder == "nguyenvong_asc" ? "nguyenvong_desc" : "nguyenvong_asc";
            ViewBag.TsTrangThaiHs = sortOrder == "hoSo_asc" ? "hoSo_desc" : "hoSo_asc";
            ViewBag.TsTrangThaiLP = sortOrder == "lePhi_asc" ? "lePhi_desc" : "lePhi_asc";

            switch (sortOrder)
            {
                case "hodem_desc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderByDescending(m => m.HoDem).ToList();
                    break;
                case "hodem_asc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderBy(m => m.HoDem).ToList();
                    break;

                case "ten_desc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderByDescending(m => m.Ten).ToList();
                    break;
                case "ten_asc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderBy(m => m.Ten).ToList();
                    break;

                case "phuongthuc_desc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderByDescending(m => m.Ptxt_ID).ToList();
                    break;
                case "phuongthuc_asc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderBy(m => m.Ptxt_ID).ToList();
                    break;

                case "tennganh_desc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderByDescending(m => m.TenNganh).ToList();
                    break;
                case "tennganh_asc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderBy(m => m.TenNganh).ToList();
                    break;

                case "nguyenvong_desc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderByDescending(m => m.NguyenVong).ToList();
                    break;
                case "nguyenvong_asc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderBy(m => m.NguyenVong).ToList();
                    break;

                case "hoSo_asc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderByDescending(m => m.TrangThai_HoSo).ToList();
                    break;
                case "hoSo_desc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderBy(m => m.TrangThai_HoSo).ToList();
                    break;

                case "lePhi_desc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderByDescending(m => m.TrangThaiLP).ToList();
                    break;
                case "lePhi_asc":
                    ListHoSoXetTuyen = ListHoSoXetTuyen.OrderBy(m => m.TrangThaiLP).ToList();
                    break;
            }
            #endregion
            // thực hiện phân trang
            #region Phân trang
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            #endregion
            // tham số khác
            #region Tham số khác
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }

            ViewBag.PhuongThucFilteri = filteriPhuongThuc;
            ViewBag.NganhHocFilteri = filteriNganhHoc;
            ViewBag.LePhiFilteri = filteriLePhi;
            ViewBag.HoSoFilteri = filteriHoSo;
            ViewBag.SearchString = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.pageCurren = page;
            ViewBag.totalRecod = ListHoSoXetTuyen.Count();

            #endregion
            return View(ListHoSoXetTuyen.ToPagedList(pageNumber, pageSize));

        }
        #endregion
        #region Kiểm tra hồ sơ xét tuyển dùng kết quả trung học phổ thông
        public ActionResult TheoDoiNopHoSoDetailPt2(string filteriPhuongThuc, string filteriNganhHoc, string filteriLePhi, string filteriHoSo, string searchString, string currentFilter,
            string filteriDotxt, string sortOrder, int? page, double? Dkxt_ID)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;
            return View();
        }
        public ActionResult TheoDoiNopHoSoDetailPt3(string filteriPhuongThuc, string filteriNganhHoc, string filteriLePhi, string filteriHoSo, string searchString, string currentFilter,
          string filteriDotxt, string sortOrder, int? page, double? Dkxt_ID)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;
            return View();
        }
        public ActionResult TheoDoiNopHoSoDetailPt4(string filteriPhuongThuc, string filteriNganhHoc, string filteriLePhi, string filteriHoSo, string searchString, string currentFilter,
          string filteriDotxt, string sortOrder, int? page, double? Dkxt_ID)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;
            return View();
        }
        public ActionResult TheoDoiNopHoSoDetailPt5(string filteriPhuongThuc, string filteriNganhHoc, string filteriLePhi, string filteriHoSo, string searchString, string currentFilter,
          string filteriDotxt, string sortOrder, int? page, double? Dkxt_ID)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;
            return View();
        }

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

            DiemThiGQMon diemmon1 = JsonConvert.DeserializeObject<DiemThiGQMon>(model.Dkxt_KQTQG_Diem_M1);
            DiemThiGQMon diemmon2 = JsonConvert.DeserializeObject<DiemThiGQMon>(model.Dkxt_KQTQG_Diem_M2);
            DiemThiGQMon diemmon3 = JsonConvert.DeserializeObject<DiemThiGQMon>(model.Dkxt_KQTQG_Diem_M3);
            string ThiSinh_GT = "Nam";
            if (model.ThiSinhDangKy.ThiSinh_GioiTinh == 1) { ThiSinh_GT = "Nữ"; }
            var data_return = new
            {
                ThiSinh_HoLot = model.ThiSinhDangKy.ThiSinh_HoLot,
                ThiSinh_Ten = model.ThiSinhDangKy.ThiSinh_Ten,
                ThiSinh_NgaySinh = model.ThiSinhDangKy.ThiSinh_NgaySinh,
                ThiSinh_GioiTinh = ThiSinh_GT,
                ThiSinh_DanToc = model.ThiSinhDangKy.ThiSinh_DanToc,
                ThiSinh_CCCD = model.ThiSinhDangKy.ThiSinh_CCCD,
                ThiSinh_DienThoai = model.ThiSinhDangKy.ThiSinh_DienThoai,
                ThiSinh_Email = model.ThiSinhDangKy.ThiSinh_Email,
                ThiSinh_TruongCapBa_Tinh_ID = db.Tinhs.Where(x => x.Tinh_ID == model.ThiSinhDangKy.ThiSinh_TruongCapBa_Tinh_ID).FirstOrDefault().Tinh_Ten,
                ThiSinh_TruongCapBa_Ma = model.ThiSinhDangKy.ThiSinh_TruongCapBa_Ma,
                ThiSinh_TruongCapBa = model.ThiSinhDangKy.ThiSinh_TruongCapBa,
                ThiSinh_DCNhanGiayBao = model.ThiSinhDangKy.ThiSinh_DCNhanGiayBao,
                ThiSinh_HoKhauThuongTru = model.ThiSinhDangKy.ThiSinh_HoKhauThuongTru,


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
                Dkxt_KQTQG_MinhChung_UuTien = model.Dkxt_KQTQG_MinhChung_UuTien,

                Dkxt_KQTQG_KinhPhi_SoThamChieu = model.Dkxt_KQTQG_KinhPhi_SoThamChieu,
                Dkxt_KQTQG_KinhPhi_TepMinhChung = model.Dkxt_KQTQG_KinhPhi_TepMinhChung,
                Dkxt_KQTQG_KinhPhi_NgayThang_NopMC = model.Dkxt_KQTQG_KinhPhi_NgayThang_NopMC,
                Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC = model.Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC,

                Dkxt_KQTQG_TrangThai_KinhPhi = model.Dkxt_KQTQG_TrangThai_KinhPhi,
                Dkxt_KQTQG_TrangThai_HoSo = model.Dkxt_KQTQG_TrangThai_HoSo,
                Dkxt_KQTQG_TrangThai_KetQua = model.Dkxt_KQTQG_TrangThai_KetQua,
                Dkxt_ThongBaoKiemDuyet_HoSo = model.Dkxt_ThongBaoKiemDuyet_HoSo,

            };

            return Json(new { success = true, data = data_return }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DangKyXetTuyen_HB_GetByID(DangKyXetTuyenHB entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenHBs.Include(x => x.ThiSinhDangKy).Where(x => x.Dkxt_HB_ID == entity.Dkxt_HB_ID).FirstOrDefault();

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
            string _ut_khuvuv_ten_diem = model.ThiSinhDangKy.KhuVuc.KhuVuc_Ten + ": ƯT " + model.ThiSinhDangKy.KhuVuc.KhuVuc_DiemUuTien + " đ";

            MonDiem diemmon1 = JsonConvert.DeserializeObject<MonDiem>(model.Dkxt_HB_Diem_M1);
            MonDiem diemmon2 = JsonConvert.DeserializeObject<MonDiem>(model.Dkxt_HB_Diem_M2);
            MonDiem diemmon3 = JsonConvert.DeserializeObject<MonDiem>(model.Dkxt_HB_Diem_M3);
            string ThiSinh_GT = "Nam";
            if (model.ThiSinhDangKy.ThiSinh_GioiTinh == 1) { ThiSinh_GT = "Nữ"; }
            var data_return = new
            {
                ThiSinh_HoLot = model.ThiSinhDangKy.ThiSinh_HoLot,
                ThiSinh_Ten = model.ThiSinhDangKy.ThiSinh_Ten,
                ThiSinh_NgaySinh = model.ThiSinhDangKy.ThiSinh_NgaySinh,
                ThiSinh_GioiTinh = ThiSinh_GT,
                ThiSinh_DanToc = model.ThiSinhDangKy.ThiSinh_DanToc,
                ThiSinh_CCCD = model.ThiSinhDangKy.ThiSinh_CCCD,
                ThiSinh_DienThoai = model.ThiSinhDangKy.ThiSinh_DienThoai,
                ThiSinh_Email = model.ThiSinhDangKy.ThiSinh_Email,
                ThiSinh_TruongCapBa_Tinh_ID = db.Tinhs.Where(x => x.Tinh_ID == model.ThiSinhDangKy.ThiSinh_TruongCapBa_Tinh_ID).FirstOrDefault().Tinh_Ten,
                ThiSinh_TruongCapBa_Ma = model.ThiSinhDangKy.ThiSinh_TruongCapBa_Ma,
                ThiSinh_TruongCapBa = model.ThiSinhDangKy.ThiSinh_TruongCapBa,
                ThiSinh_DCNhanGiayBao = model.ThiSinhDangKy.ThiSinh_DCNhanGiayBao,
                ThiSinh_HoKhauThuongTru = model.ThiSinhDangKy.ThiSinh_HoKhauThuongTru,

                KhuVuc_ID = _ut_khuvuv_ten_diem,
                DoiTuong_ID = _ut_doituong_ten_diem,
                ThiSinh_HocLucLop12 = _xeploai_hocluc_12,
                ThiSinh_HanhKiemLop12 = _xeploai_hanhkiem_12,

                Dkxt_HB_ID = model.Dkxt_HB_ID,
                ThiSinh_ID = model.ThiSinh_ID,
                DotXT_ID = model.DotXT_ID,
                Ptxt_ID = model.Ptxt_ID,

                Nganh_ID = model.Nganh_ID,
                Thm_ID = model.Thm_ID,
                KhoiNganh_ID = khoinganh_id,

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
                Dkxt_ThongBaoKiemDuyet_HoSo = model.Dkxt_ThongBaoKiemDuyet_HoSo,

                Dkxt_HB_TrangThai_KinhPhi = model.Dkxt_HB_TrangThai_KinhPhi,
                Dkxt_HB_TrangThai_HoSo = model.Dkxt_HB_TrangThai_HoSo,
                Dkxt_HB_TrangThai_KetQua = model.Dkxt_HB_TrangThai_KetQua,
            };
            return Json(new { success = true, data = data_return }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_TT_GetByID(DangKyXetTuyenThang entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenThangs.Include(x => x.ThiSinhDangKy).Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();
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
            string ThiSinh_GT = "Nam";
            if (model.ThiSinhDangKy.ThiSinh_GioiTinh == 1) { ThiSinh_GT = "Nữ"; }
            var data_return = new
            {
                ThiSinh_HoLot = model.ThiSinhDangKy.ThiSinh_HoLot,
                ThiSinh_Ten = model.ThiSinhDangKy.ThiSinh_Ten,
                ThiSinh_NgaySinh = model.ThiSinhDangKy.ThiSinh_NgaySinh,
                ThiSinh_GioiTinh = ThiSinh_GT,
                ThiSinh_DanToc = model.ThiSinhDangKy.ThiSinh_DanToc,
                ThiSinh_CCCD = model.ThiSinhDangKy.ThiSinh_CCCD,
                ThiSinh_DienThoai = model.ThiSinhDangKy.ThiSinh_DienThoai,
                ThiSinh_Email = model.ThiSinhDangKy.ThiSinh_Email,
                ThiSinh_TruongCapBa_Tinh_ID = db.Tinhs.Where(x => x.Tinh_ID == model.ThiSinhDangKy.ThiSinh_TruongCapBa_Tinh_ID).FirstOrDefault().Tinh_Ten,
                ThiSinh_TruongCapBa_Ma = model.ThiSinhDangKy.ThiSinh_TruongCapBa_Ma,
                ThiSinh_TruongCapBa = model.ThiSinhDangKy.ThiSinh_TruongCapBa,
                ThiSinh_DCNhanGiayBao = model.ThiSinhDangKy.ThiSinh_DCNhanGiayBao,
                ThiSinh_HoKhauThuongTru = model.ThiSinhDangKy.ThiSinh_HoKhauThuongTru,

                KhuVuc_ID = _ut_khuvuv_ten_diem,
                DoiTuong_ID = _ut_doituong_ten_diem,
                ThiSinh_HocLucLop12 = _xeploai_hocluc_12,
                ThiSinh_HanhKiemLop12 = _xeploai_hanhkiem_12,

                Dkxt_ID = model.Dkxt_ID,
                ThiSinh_ID = model.ThiSinh_ID,
                DotXT_ID = model.DotXT_ID,
                Ptxt_ID = model.Ptxt_ID,

                Nganh_ID = model.Nganh_ID,
                Dkxt_ToHopXT = model.Dkxt_ToHopXT,
                KhoiNganh_ID = khoinganh_id,

                Dkxt_NguyenVong = model.Dkxt_NguyenVong,
                Dkxt_GhiChu = model.Dkxt_GhiChu,

                Dkxt_LoaiGiai = model.Dkxt_LoaiGiai,
                Dkxt_NamDatGiai = model.Dkxt_NamDatGiai,
                Dkxt_MonDatGiai = model.Dkxt_MonDatGiai,

                Dkxt_MinhChung_HB = model.Dkxt_MinhChung_HB,
                Dkxt_MinhChung_Bang = model.Dkxt_MinhChung_Bang,
                Dkxt_MinhChung_CCCD = model.Dkxt_MinhChung_CCCD,
                Dkxt_MinhChung_UuTien = model.Dkxt_MinhChung_UuTien,
                Dkxt_MinhChung_Giai = model.Dkxt_MinhChung_Giai,

                Dkxt_KinhPhi_SoThamChieu = model.Dkxt_KinhPhi_SoThamChieu,
                Dkxt_KinhPhi_TepMinhChung = model.Dkxt_KinhPhi_TepMinhChung,
                Dkxt_KinhPhi_NgayThang_NopMC = model.Dkxt_KinhPhi_NgayThang_NopMC,
                Dkxt_KinhPhi_NgayThang_CheckMC = model.Dkxt_KinhPhi_NgayThang_CheckMC,
                Dkxt_ThongBaoKiemDuyet_HoSo = model.Dkxt_ThongBaoKiemDuyet_HoSo,

                Dkxt_TrangThai_KinhPhi = model.Dkxt_TrangThai_KinhPhi,
                Dkxt_TrangThai_HoSo = model.Dkxt_TrangThai_HoSo,
                Dkxt_TrangThai_KetQua = model.Dkxt_TrangThai_KetQua,


            };
            return Json(new { success = true, data = data_return }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_DanhGia_GetByID(DangKyXetTuyenKhac entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenKhacs.Include(d => d.Nganh).Include(x => x.ChungChi).FirstOrDefault(x => x.Dkxt_ID == entity.Dkxt_ID);
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
            string _ut_khuvuv_ten_diem = model.ThiSinhDangKy.KhuVuc.KhuVuc_Ten + ": ƯT " + model.ThiSinhDangKy.KhuVuc.KhuVuc_DiemUuTien + " đ";
            string ThiSinh_GT = "Nam";
            if (model.ThiSinhDangKy.ThiSinh_GioiTinh == 1) { ThiSinh_GT = "Nữ"; }

            var data_return = new
            {
                ThiSinh_HoLot = model.ThiSinhDangKy.ThiSinh_HoLot,
                ThiSinh_Ten = model.ThiSinhDangKy.ThiSinh_Ten,
                ThiSinh_NgaySinh = model.ThiSinhDangKy.ThiSinh_NgaySinh,
                ThiSinh_GioiTinh = ThiSinh_GT,
                ThiSinh_DanToc = model.ThiSinhDangKy.ThiSinh_DanToc,
                ThiSinh_CCCD = model.ThiSinhDangKy.ThiSinh_CCCD,
                ThiSinh_DienThoai = model.ThiSinhDangKy.ThiSinh_DienThoai,
                ThiSinh_Email = model.ThiSinhDangKy.ThiSinh_Email,
                ThiSinh_TruongCapBa_Tinh_ID = db.Tinhs.Where(x => x.Tinh_ID == model.ThiSinhDangKy.ThiSinh_TruongCapBa_Tinh_ID).FirstOrDefault().Tinh_Ten,
                ThiSinh_TruongCapBa_Ma = model.ThiSinhDangKy.ThiSinh_TruongCapBa_Ma,
                ThiSinh_TruongCapBa = model.ThiSinhDangKy.ThiSinh_TruongCapBa,
                ThiSinh_DCNhanGiayBao = model.ThiSinhDangKy.ThiSinh_DCNhanGiayBao,
                ThiSinh_HoKhauThuongTru = model.ThiSinhDangKy.ThiSinh_HoKhauThuongTru,

                KhuVuc_ID = _ut_khuvuv_ten_diem,
                DoiTuong_ID = _ut_doituong_ten_diem,
                ThiSinh_HocLucLop12 = _xeploai_hocluc_12,
                ThiSinh_HanhKiemLop12 = _xeploai_hanhkiem_12,

                Dkxt_ID = model.Dkxt_ID,
                Dkxt_ChungChi_Ten = model.ChungChi.ChungChi_Ten,
                ThiSinh_ID = model.ThiSinh_ID,
                Ptxt_ID = model.Ptxt_ID,
                ChungChi_ID = model.ChungChi_ID,

                KhoiNganh_Ten = model.Nganh.KhoiNganh.KhoiNganh_Ten,
                KhoiNganh_ID = model.Nganh.KhoiNganh_ID,
                Nganh_TenNganh = model.Nganh.Nganh_TenNganh,
                Nganh_ID = model.Nganh_ID,

              
                Dkxt_NguyenVong = model.Dkxt_NguyenVong,
                DotXT_ID = model.DotXT_ID,
                Dkxt_GhiChu = model.Dkxt_GhiChu,
                Dkxt_ToHopXT = model.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = model.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = model.Dkxt_KetQuaDatDuoc,
                Dkxt_NgayDuThi = model.Dkxt_NgayDuThi,
                Dkxt_TongDiem = model.Dkxt_TongDiem,

                Dkxt_MinhChung_HB = model.Dkxt_MinhChung_HB,              
                Dkxt_MinhChung_CCCD = model.Dkxt_MinhChung_CCCD,
                Dkxt_MinhChung_Bang = model.Dkxt_MinhChung_Bang,
                Dkxt_MinhChung_KetQua = model.Dkxt_MinhChung_KetQua,
                Dkxt_MinhChung_UuTien = model.Dkxt_MinhChung_UuTien,
              
                Dkxt_KinhPhi_SoThamChieu = model.Dkxt_KinhPhi_SoThamChieu,
                Dkxt_KinhPhi_TepMinhChung = model.Dkxt_KinhPhi_TepMinhChung,
                Dkxt_KinhPhi_NgayThang_NopMC = model.Dkxt_KinhPhi_NgayThang_NopMC,
                Dkxt_KinhPhi_NgayThang_CheckMC = model.Dkxt_KinhPhi_NgayThang_CheckMC,
                Dkxt_ThongBaoKiemDuyet_HoSo = model.Dkxt_ThongBaoKiemDuyet_HoSo,

                Dkxt_TrangThai_KinhPhi = model.Dkxt_TrangThai_KinhPhi,
                Dkxt_TrangThai_HoSo = model.Dkxt_TrangThai_HoSo,
                Dkxt_TrangThai_KetQua = model.Dkxt_TrangThai_KetQua,
            };

            return Json(new { success = true, data = data_return, }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Theo dõi thí sinh nộp kinh phí
        private IList<TongHopSoLieuXetTuyen> ListLePhiXetTuyen;
        public ActionResult TheoDoiNopLePhi()
        {
            IList<TongHopSoLieuXetTuyen> ListLePhiXT = new List<TongHopSoLieuXetTuyen>();
            db = new DbConnecttion();
            /*
            var model_dxt_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            var model_xt2 = db.DangKyXetTuyenKQTQGs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_KQTQG_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_KQTQG_NguyenVong,
                TrangThai = s.Dkxt_KQTQG_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KQTQG_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KQTQG_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KQTQG_KinhPhi_TepMinhChung,
            }).ToList();

            foreach (var item in model_xt2)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = int.Parse(item.Ptxt_ID.ToString());
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXT.Add(item_lp);
            }

            var model_xt3 = db.DangKyXetTuyenHBs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_HB_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_HB_NguyenVong,
                TrangThai = s.Dkxt_HB_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_HB_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_HB_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_HB_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_HB_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt3)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = int.Parse(item.Ptxt_ID.ToString());
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXT.Add(item_lp);
            }
            var model_xt4 = db.DangKyXetTuyenThangs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_NguyenVong,
                TrangThai = s.Dkxt_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt4)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = int.Parse(item.Ptxt_ID.ToString());
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXT.Add(item_lp);
            }
            var model_xt5 = db.DangKyXetTuyenKhacs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_NguyenVong,
                TrangThai = s.Dkxt_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt5)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = int.Parse(item.Ptxt_ID.ToString());
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXT.Add(item_lp);
            }
            ListLePhiXT = ListLePhiXT.OrderByDescending(x => x.NgayThangNopLP)
                                                 .ThenBy(x => x.TrangThaiLP).ThenByDescending(x => x.Dkxt_ID)
                                                 .ThenByDescending(x => x.ThiSinh_ID).ToList();
            */
            return View(ListLePhiXT);
        }
        public JsonResult GetAllDataKinhPhi()
        {
            ListLePhiXetTuyen = new List<TongHopSoLieuXetTuyen>();
            db = new DbConnecttion();
            var model_dxt_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var data_md = new
            {
                model_dxt_present.Dxt_ID,
            };
            /*
            var model_xt2 = db.DangKyXetTuyenKQTQGs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_KQTQG_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_KQTQG_NguyenVong,

                TrangThai = s.Dkxt_KQTQG_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KQTQG_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KQTQG_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KQTQG_KinhPhi_TepMinhChung,
            }).ToList();

            foreach (var item in model_xt2)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = int.Parse(item.Ptxt_ID.ToString());
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXetTuyen.Add(item_lp);
            }

            var model_xt3 = db.DangKyXetTuyenHBs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_HB_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_HB_NguyenVong,
                TrangThai = s.Dkxt_HB_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_HB_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_HB_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_HB_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_HB_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt3)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = int.Parse(item.Ptxt_ID.ToString());
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXetTuyen.Add(item_lp);
            }
            var model_xt4 = db.DangKyXetTuyenThangs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_NguyenVong,
                TrangThai = s.Dkxt_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt4)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = int.Parse(item.Ptxt_ID.ToString());
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXetTuyen.Add(item_lp);
            }
            var model_xt5 = db.DangKyXetTuyenKhacs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_NguyenVong,
                TrangThai = s.Dkxt_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt5)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = int.Parse(item.Ptxt_ID.ToString());
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXetTuyen.Add(item_lp);
            }
            ListLePhiXetTuyen = ListLePhiXetTuyen.OrderByDescending(x => x.NgayThangNopLP)
                                                 .ThenBy(x => x.TrangThaiLP).ThenByDescending(x => x.Dkxt_ID)
                                                 .ThenByDescending(x => x.ThiSinh_ID).ToList();
            */
            return Json(new { success = true, data = ListLePhiXetTuyen }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}