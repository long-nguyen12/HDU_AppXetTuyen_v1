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
                TenToHop = s.Dkxt_MonDatGiai + " - " +s.Dkxt_LoaiGiai,//s.ToHopMon.Thm_TenToHop,

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
                if (_item == "1")
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "Chưa kiểm tra" });
                }
                if (_item == "2")
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã KT, Có sai sót" });
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
        public ActionResult TheoDoiNopHoSoDetail()
        {
            return View();
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