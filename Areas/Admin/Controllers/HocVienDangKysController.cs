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

using BC = BCrypt.Net.BCrypt;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class HocVienDangKysController : Controller
    {
        private DbConnecttion db = new DbConnecttion();
        [AdminSessionCheck]
        public ActionResult AdminCreateHocVien()
        {
            return View();
        }
        [AdminSessionCheck]
        public ActionResult AdCreateHocVienJson(HocVienDangKy entity)
        {
            HocVienDangKy hv_new = new HocVienDangKy();

            string activationToken = Guid.NewGuid().ToString();

            var hash_password = ComputeHash(entity.HocVien_CCCD, "123456");

            hv_new.HocVien_HoDem = entity.HocVien_HoDem;
            hv_new.HocVien_Ten = entity.HocVien_Ten;
            hv_new.HocVien_GioiTinh = entity.HocVien_GioiTinh;
            hv_new.HocVien_DanToc = entity.HocVien_DanToc;
            hv_new.HocVien_NgaySinh = entity.HocVien_NgaySinh;

            hv_new.HocVien_CCCD = entity.HocVien_CCCD;
            hv_new.HocVien_CCCD_NgayCap = entity.HocVien_CCCD_NgayCap;

            hv_new.HocVien_DienThoai = entity.HocVien_DienThoai;
            hv_new.HocVien_Email = entity.HocVien_Email;
            hv_new.HocVien_HoKhauThuongTru = entity.HocVien_HoKhauThuongTru;
            hv_new.HocVien_NoiOHienNay = entity.HocVien_HoKhauThuongTru;
            hv_new.HocVien_DiaChiLienHe = entity.HocVien_DiaChiLienHe;
            hv_new.HocVien_NoiSinh = entity.HocVien_NoiSinh;

            hv_new.HocVien_MatKhau = hash_password;
            hv_new.HocVien_ResetCode = activationToken;
            hv_new.HocVien_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            hv_new.HocVien_TrangThai = 0;

            hv_new.HocVien_TenDonViCongTac = entity.HocVien_TenDonViCongTac;
            hv_new.HocVien_ChuyenMon = entity.HocVien_ChuyenMon;
            hv_new.HocVien_ThamNien = entity.HocVien_ThamNien;
            hv_new.HocVien_ChucVu = entity.HocVien_ChucVu;
            hv_new.HocVien_NamCT = entity.HocVien_NamCT;
            hv_new.HocVien_LoaiCB = entity.HocVien_LoaiCB;

            hv_new.HocVien_BangDaiHoc = entity.HocVien_BangDaiHoc;
            hv_new.HocVien_BoTucKienThuc = entity.HocVien_BoTucKienThuc;
            hv_new.HocVien_DoiTuongUuTien = entity.HocVien_DoiTuongUuTien;
            db.HocVienDangKies.Add(hv_new);

            HocVienDuTuyen dt_new = new HocVienDuTuyen();
            HocVienDuTuyen entity_hv = JsonConvert.DeserializeObject<HocVienDuTuyen>(entity.HocVien_Email_Temp);

            dt_new.HocVien_ID = hv_new.HocVien_ID;
            dt_new.DuTuyen_TrangThai = 9;
            dt_new.DuTuyen_ThongBaoKiemDuyet = "Đã kiểm duyệt";
            dt_new.DuTuyen_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt_new.Dxt_ID = db.DotXetTuyens.Where(x => x.Dxt_TrangThai_Xt == 1 && x.Dxt_Classify == 2).FirstOrDefault().Dxt_ID;
            dt_new.DuTuyen_GhiChu = "Admin cập nhật dữ liệu";

            dt_new.DuTuyen_MaNghienCuu = entity_hv.DuTuyen_MaNghienCuu;
            dt_new.Nganh_Mt_ID = entity_hv.Nganh_Mt_ID;
            dt_new.HocVien_DKDTNgoaiNgu = entity_hv.HocVien_DKDTNgoaiNgu;
            dt_new.HocVien_VanBangNgoaiNgu = entity_hv.HocVien_VanBangNgoaiNgu;
            dt_new.HocVien_DoiTuongDuThi = entity_hv.HocVien_DoiTuongDuThi;
            dt_new.HocVien_SoYeuLyLich = entity_hv.HocVien_SoYeuLyLich;
            dt_new.HocVien_MCBangDaiHoc = entity_hv.HocVien_MCBangDaiHoc;
            dt_new.HocVien_MCBangDiem = entity_hv.HocVien_MCBangDiem;
            dt_new.HocVien_MCGiayKhamSucKhoe = entity_hv.HocVien_MCGiayKhamSucKhoe;
            dt_new.HocVien_Anh46 = entity_hv.HocVien_Anh46;
            dt_new.HocVien_MCCCNN = entity_hv.HocVien_MCCCNN;
            dt_new.HocVien_MCKhac = entity_hv.HocVien_MCKhac;

            dt_new.HocVien_LePhi_MaThamChieu = "Nộp trực tiếp";
            dt_new.HocVien_LePhi_NgayNop = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt_new.HocVien_LePhi_TrangThai = 9;

            db.HocVienDuTuyens.Add(dt_new);
            db.SaveChanges();

            #region Gửi mail xác thực
            SendEmail send = new SendEmail();
            var subject = "Xác nhận tài khoản";
            string activationUrl = Url.Action("ActivationAccount", "Auth", new { token = activationToken }, Request.Url.Scheme);
            var body = "Xin chào " + hv_new.HocVien_HoDem + " " + hv_new.HocVien_Ten + 
                "<br/> Hệ thống tuyển sinh sau đại học của trường Đại học Hồng Đức đã cập nhật thông tin hồ sơ của bạn." +
                "<br/> - Vui lòng click vào địa chỉ đính kèm bên dưới:" +
                "<br/>" + activationUrl + " <br/>" +
                "<br/> để xác thực tài khoản đồng thời có thể đăng nhập vào hệ thống để kiểm tra thông tin sau khi xác thực " +
                "<br/> + Tên đăng nhập: " + hv_new.HocVien_CCCD +
                "<br/> + Mật khẩu: 123456";
            send.Sendemail(hv_new.HocVien_Email, body, subject);
            #endregion

            return Json(new { success = true, data = entity_hv }, JsonRequestBehavior.AllowGet);
        }

        [AdminSessionCheck]
        public ActionResult AdminUpdateHocVien(int? duTuyen_ID, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            if (!String.IsNullOrEmpty(duTuyen_ID.ToString()))
            {
                ViewBag.DuTuyen_ID = duTuyen_ID;
                return View();
            }
            else
            {
                return RedirectToAction("DsHvDuTuyen", "HocVienDangKys");
            }
        }
        [AdminSessionCheck]
        public ActionResult AdUpdateHocVienJson(HocVienDangKy entity)
        {
            HocVienDuTuyen entity_hv = JsonConvert.DeserializeObject<HocVienDuTuyen>(entity.HocVien_Email_Temp);

            HocVienDuTuyen dt_update =db.HocVienDuTuyens.Where(x => x.DuTuyen_ID == entity_hv.DuTuyen_ID).FirstOrDefault();
            HocVienDangKy hv_update = db.HocVienDangKies.Where(x => x.HocVien_ID == dt_update.HocVien_ID).FirstOrDefault();

            hv_update.HocVien_HoDem = entity.HocVien_HoDem;
            hv_update.HocVien_Ten = entity.HocVien_Ten;
            hv_update.HocVien_GioiTinh = entity.HocVien_GioiTinh;
            hv_update.HocVien_DanToc = entity.HocVien_DanToc;
            hv_update.HocVien_NgaySinh = entity.HocVien_NgaySinh;
           
            hv_update.HocVien_CCCD_NgayCap = entity.HocVien_CCCD_NgayCap;

            hv_update.HocVien_DienThoai = entity.HocVien_DienThoai;
            hv_update.HocVien_Email = entity.HocVien_Email;
            hv_update.HocVien_HoKhauThuongTru = entity.HocVien_HoKhauThuongTru;
            hv_update.HocVien_NoiOHienNay = entity.HocVien_HoKhauThuongTru;
            hv_update.HocVien_DiaChiLienHe = entity.HocVien_DiaChiLienHe;
            hv_update.HocVien_NoiSinh = entity.HocVien_NoiSinh;
            
            hv_update.HocVien_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            hv_update.HocVien_TrangThai = 0;

            hv_update.HocVien_TenDonViCongTac = entity.HocVien_TenDonViCongTac;
            hv_update.HocVien_ChuyenMon = entity.HocVien_ChuyenMon;
            hv_update.HocVien_ThamNien = entity.HocVien_ThamNien;
            hv_update.HocVien_ChucVu = entity.HocVien_ChucVu;
            hv_update.HocVien_NamCT = entity.HocVien_NamCT;
            hv_update.HocVien_LoaiCB = entity.HocVien_LoaiCB;

            hv_update.HocVien_BangDaiHoc = entity.HocVien_BangDaiHoc;
            hv_update.HocVien_BoTucKienThuc = entity.HocVien_BoTucKienThuc;
            hv_update.HocVien_DoiTuongUuTien = entity.HocVien_DoiTuongUuTien;

            dt_update.DuTuyen_TrangThai = 9;
            dt_update.DuTuyen_ThongBaoKiemDuyet = "Đã kiểm duyệt";
            dt_update.DuTuyen_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt_update.Dxt_ID = db.DotXetTuyens.Where(x => x.Dxt_TrangThai_Xt == 1 && x.Dxt_Classify == 2).FirstOrDefault().Dxt_ID;
            dt_update.DuTuyen_GhiChu = "Admin cập nhập dữ liệu";

            dt_update.DuTuyen_MaNghienCuu = entity_hv.DuTuyen_MaNghienCuu;
            dt_update.Nganh_Mt_ID = entity_hv.Nganh_Mt_ID;
            dt_update.HocVien_DKDTNgoaiNgu = entity_hv.HocVien_DKDTNgoaiNgu;
            dt_update.HocVien_VanBangNgoaiNgu = entity_hv.HocVien_VanBangNgoaiNgu;
            dt_update.HocVien_DoiTuongDuThi = entity_hv.HocVien_DoiTuongDuThi;

            dt_update.HocVien_SoYeuLyLich += entity_hv.HocVien_SoYeuLyLich;
            dt_update.HocVien_MCBangDaiHoc += entity_hv.HocVien_MCBangDaiHoc;
            dt_update.HocVien_MCBangDiem += entity_hv.HocVien_MCBangDiem;
            dt_update.HocVien_MCGiayKhamSucKhoe += entity_hv.HocVien_MCGiayKhamSucKhoe;
            dt_update.HocVien_Anh46 += entity_hv.HocVien_Anh46;
            dt_update.HocVien_MCCCNN += entity_hv.HocVien_MCCCNN;
            dt_update.HocVien_MCKhac += entity_hv.HocVien_MCKhac;

            dt_update.HocVien_LePhi_MaThamChieu = "Nộp trực tiếp";
            dt_update.HocVien_LePhi_NgayNop = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt_update.HocVien_LePhi_TrangThai = 9;
          
            db.SaveChanges();

            #region Gửi mail xác thực
            SendEmail send = new SendEmail();
            var subject = "Thông báo cập nhật";
            var body = "Xin chào " + hv_update.HocVien_HoDem + " " + hv_update.HocVien_Ten +
                "<br/> Hệ thống tuyển sinh sau đại học của trường Đại học Hồng Đức đã cập nhật thông tin hồ sơ của bạn." +
                "<br/> - Vui lòng đăng nhập vào hệ thống để hệ thống để kiểm tra thông tin sau khi cập nhật";            
            send.Sendemail(hv_update.HocVien_Email, body, subject);
            #endregion

            return Json(new { success = true}, JsonRequestBehavior.AllowGet);
        }

        [AdminSessionCheck]
        public ActionResult DsHvDangKy(string searchString, string currentFilter, string filteriDotxt, int? page)
        {
            var hocviens = db.HocVienDangKies.ToList();
            // thưc hiện tìm kiếm: theo họ, tên, cccd, điện thoại, email
            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                hocviens = hocviens.Where(h => h.HocVien_Ten.ToUpper().Contains(searchString.ToUpper())
                                    || h.HocVien_HoDem.ToUpper().Contains(searchString.ToUpper())
                                    || h.HocVien_CCCD.Contains(searchString)
                                    || h.HocVien_DienThoai.Contains(searchString)).ToList();

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

            ViewBag.pageCurren = page;
            ViewBag.SearchString = searchString;

            ViewBag.totalRecod = hocviens.Count();

            #endregion
            return View(hocviens.ToPagedList(pageNumber, pageSize));
        }

        [AdminSessionCheck]
        public ActionResult DsHvDangKy_Update()
        {
            return View();
        }    
        [AdminSessionCheck]
        public ActionResult DsHvDuTuyen(string filteriNganhHoc, string filteriLePhi, string filteriHoSo, string searchString, string currentFilter,
            string filteriDotxt, string sortOrder, int? page)
        {
            var hocviens = db.HocVienDuTuyens
                             .Include(h => h.DotXetTuyen)
                             .Include(h => h.HocVienDangKy)
                             .Include(h => h.NganhMaster).ToList();


            #region lọc dữ liệu theo đợt
            var dotxts = db.DotXetTuyens.Include(x => x.NamHoc).Where(x => x.NamHoc.NamHoc_TrangThai == 1 && x.Dxt_Classify == 2).ToList();
            int _dotxt_hientai = dotxts.Where(x => x.Dxt_TrangThai_Xt == 1 && x.Dxt_Classify == 2).FirstOrDefault().Dxt_ID;
            ViewBag.filteriDotxt = new SelectList(dotxts.OrderBy(x => x.Dxt_ID).ToList(), "Dxt_ID", "Dxt_Ten", _dotxt_hientai);
            // nếu không có truyền vào thì gán giá trị cho đợt xét tuyển là hiện tại
            if (String.IsNullOrEmpty(filteriDotxt) == true)
            {
                filteriDotxt = dotxts.Where(x => x.Dxt_TrangThai_Xt == 1).FirstOrDefault().Dxt_ID.ToString();
            }
            // thực hiện lọc 

            #endregion

            #region lọc dữ liệu theo ngành
            var list_items_nganh = (from item in hocviens select new { item.NganhMaster.Nganh_Mt_ID, item.NganhMaster.Nganh_Mt_TenNganh }).Distinct().ToList();

            ViewBag.filteriNganhHoc = new SelectList(list_items_nganh.OrderBy(x => x.Nganh_Mt_ID).ToList(), "Nganh_Mt_ID", "Nganh_Mt_TenNganh");

            if (!String.IsNullOrEmpty(filteriNganhHoc))
            {
                int _nganh_Mt_ID = Int32.Parse(filteriNganhHoc);
                hocviens = hocviens.Where(x => x.Nganh_Mt_ID == _nganh_Mt_ID).ToList();
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in hocviens select item.HocVien_LePhi_TrangThai).Distinct().ToList();
            List<StatusTracking> filteri_items_lephi = new List<StatusTracking>();

            foreach (var _item in list_items_lephi)
            {
                if (_item == 0)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa đóng phí" });
                }
                if (_item == 1)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã nộp lệ phí" });
                }
                if (_item == 2)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 2, St_Name = "Mã tham chiếu sai" });
                }
                if (_item == 9)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int _dkxt_TrangThai = Int32.Parse(filteriLePhi);
                hocviens = hocviens.Where(x => x.HocVien_LePhi_TrangThai == _dkxt_TrangThai).ToList();
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in hocviens select item.DuTuyen_TrangThai).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "Chưa kiểm tra" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 2, St_Name = "Có sai sót" });
                }
                if (_item == 9)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _duTuyen_TrangThai = Int32.Parse(filteriHoSo);
                hocviens = hocviens.Where(x => x.DuTuyen_TrangThai == _duTuyen_TrangThai).ToList();
            }
            #endregion
            // thưc hiện tìm kiếm: theo họ, tên, cccd, điện thoại, email
            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                hocviens = hocviens.Where(h => h.HocVienDangKy.HocVien_Ten.ToUpper().Contains(searchString.ToUpper())
                                    || h.HocVienDangKy.HocVien_HoDem.ToUpper().Contains(searchString.ToUpper())
                                    || h.HocVienDangKy.HocVien_CCCD.Contains(searchString)
                                    || h.HocVienDangKy.HocVien_DienThoai.Contains(searchString)).ToList();

            }
            #endregion
            // thực hiện sắp xếp theo vài cột dữ liệu
            #region Phần sắp xếp theo các cột dữ liệu
            // 
            ViewBag.HocVien_HoDem = sortOrder == "hodem_asc" ? "hodem_desc" : "hodem_asc";
            ViewBag.HocVien_Ten = sortOrder == "ten_asc" ? "ten_desc" : "ten_asc";
            ViewBag.Nganh_Mt_TenNganh = sortOrder == "tennganh_asc" ? "tennganh_desc" : "tennganh_asc";
            ViewBag.DuTuyen_TrangThai = sortOrder == "hoSo_asc" ? "hoSo_desc" : "hoSo_asc";
            ViewBag.HocVien_LePhi_TrangThai = sortOrder == "lePhi_asc" ? "lePhi_desc" : "lePhi_asc";

            switch (sortOrder)
            {
                case "hodem_desc":
                    hocviens = hocviens.OrderByDescending(m => m.HocVienDangKy.HocVien_HoDem).ToList();
                    break;
                case "hodem_asc":
                    hocviens = hocviens.OrderBy(m => m.HocVienDangKy.HocVien_HoDem).ToList();
                    break;

                case "ten_desc":
                    hocviens = hocviens.OrderByDescending(m => m.HocVienDangKy.HocVien_Ten).ToList();
                    break;
                case "ten_asc":
                    hocviens = hocviens.OrderBy(m => m.HocVienDangKy.HocVien_Ten).ToList();
                    break;

                case "tennganh_desc":
                    hocviens = hocviens.OrderByDescending(m => m.NganhMaster.Nganh_Mt_TenNganh).ToList();
                    break;
                case "tennganh_asc":
                    hocviens = hocviens.OrderBy(m => m.NganhMaster.Nganh_Mt_TenNganh).ToList();
                    break;

                case "hoSo_asc":
                    hocviens = hocviens.OrderByDescending(m => m.DuTuyen_TrangThai).ToList();
                    break;
                case "hoSo_desc":
                    hocviens = hocviens.OrderBy(m => m.DuTuyen_TrangThai).ToList();
                    break;

                case "lePhi_desc":
                    hocviens = hocviens.OrderByDescending(m => m.HocVien_LePhi_TrangThai).ToList();
                    break;
                case "lePhi_asc":
                    hocviens = hocviens.OrderBy(m => m.HocVien_LePhi_TrangThai).ToList();
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

            ViewBag.pageCurren = page;
            ViewBag.SearchString = searchString;
            ViewBag.filteriDotxtSort = filteriDotxt;
            ViewBag.LePhiFilteri = filteriLePhi;
            ViewBag.HoSoFilteri = filteriHoSo;
            ViewBag.CurrentSort = sortOrder;

            ViewBag.totalRecod = hocviens.Count();

            #endregion
            return View(hocviens.ToPagedList(pageNumber, pageSize));
        }

        [AdminSessionCheck]
        public ActionResult DsHvDuTuyenKiemTraHoSo(int? duTuyen_ID, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            if (!String.IsNullOrEmpty(duTuyen_ID.ToString()))
            {
                ViewBag.NganhFilteri = filteriNganh;
                ViewBag.LePhiFilteri = filteriLePhi;
                ViewBag.HoSoFilteri = filteriHoSo;
                ViewBag.SearchString = searchString;
                ViewBag.pageCurren = page;
                ViewBag.DuTuyen_ID = duTuyen_ID;
                return View();
            }
            else
            {
                return RedirectToAction("DsHvDuTuyen", "HocVienDangKys");
            }
        }

        [AdminSessionCheck]
        public JsonResult DsHvDuTuyenKiemTraHoSoJson(HocVienDuTuyen entity)
        {
            HocVienDuTuyen model = db.HocVienDuTuyens
                                    .Include(h => h.DotXetTuyen)
                                    .Include(h => h.HocVienDangKy)
                                    .Include(h => h.NganhMaster)
                                    .Where(x => x.DuTuyen_ID == entity.DuTuyen_ID)
                                    .FirstOrDefault();
            string _hv_NoiSinh = db.Tinhs.Where(x => x.Tinh_ID == model.HocVienDangKy.HocVien_NoiSinh).FirstOrDefault().Tinh_Ten;
            BangDaiHoc bangDaiHoc = JsonConvert.DeserializeObject<BangDaiHoc>(model.HocVienDangKy.HocVien_BangDaiHoc);


            var data_hv = new
            {
                HocVien_HoDem = model.HocVienDangKy.HocVien_HoDem,
                HocVien_Ten = model.HocVienDangKy.HocVien_Ten,
                HocVien_GioiTinh = model.HocVienDangKy.HocVien_GioiTinh,
                HocVien_DanToc = model.HocVienDangKy.HocVien_DanToc,
                HocVien_NgaySinh = model.HocVienDangKy.HocVien_NgaySinh,
                HocVien_CCCD = model.HocVienDangKy.HocVien_CCCD,
                HocVien_CCCD_NgayCap = model.HocVienDangKy.HocVien_CCCD_NgayCap,
                HocVien_DienThoai = model.HocVienDangKy.HocVien_DienThoai,
                HocVien_Email = model.HocVienDangKy.HocVien_Email,

                HocVien_DoiTuongUuTien = model.HocVienDangKy.HocVien_DoiTuongUuTien,
                HocVien_BoTucKienThuc = model.HocVienDangKy.HocVien_BoTucKienThuc,
                HocVien_NoiSinh = _hv_NoiSinh,
                HocVien_NoiSinh_IDTinh = model.HocVienDangKy.HocVien_NoiSinh,
                HocVien_HoKhauThuongTru = model.HocVienDangKy.HocVien_HoKhauThuongTru,
                HocVien_NoiOHienNay = model.HocVienDangKy.HocVien_NoiOHienNay,
                HocVien_DiaChiLienHe = model.HocVienDangKy.HocVien_DiaChiLienHe,

                HocVien_TenDonViCongTac = model.HocVienDangKy.HocVien_TenDonViCongTac,
                HocVien_ChuyenMon = model.HocVienDangKy.HocVien_ChuyenMon,
                HocVien_ThamNien = model.HocVienDangKy.HocVien_ThamNien,
                HocVien_ChucVu = model.HocVienDangKy.HocVien_ChucVu,
                HocVien_NamCT = model.HocVienDangKy.HocVien_NamCT,
                HocVien_LoaiCB = model.HocVienDangKy.HocVien_LoaiCB,


                DuTuyen_ID = model.DuTuyen_ID,
                HocVien_ID = model.HocVien_ID,
                DuTuyen_TrangThai = model.DuTuyen_TrangThai,
                DuTuyen_ThongBaoKiemDuyet = model.DuTuyen_ThongBaoKiemDuyet,
                DuTuyen_NgayDangKy = model.DuTuyen_NgayDangKy,
                Dxt_ID = model.Dxt_ID,
                DuTuyen_MaNghienCuu = model.DuTuyen_MaNghienCuu,
                Nganh_Mt_ID = model.Nganh_Mt_ID,
                HocVien_DKDTNgoaiNgu = model.HocVien_DKDTNgoaiNgu,
                HocVien_VanBangNgoaiNgu = model.HocVien_VanBangNgoaiNgu,
                HocVien_DoiTuongDuThi = model.HocVien_DoiTuongDuThi,

                HocVien_SoYeuLyLich = model.HocVien_SoYeuLyLich,
                HocVien_MCBangDaiHoc = model.HocVien_MCBangDaiHoc,
                HocVien_MCBangDiem = model.HocVien_MCBangDiem,
                HocVien_MCGiayKhamSucKhoe = model.HocVien_MCGiayKhamSucKhoe,
                HocVien_Anh46 = model.HocVien_Anh46,
                HocVien_MCCCNN = model.HocVien_MCCCNN,
                HocVien_MCKhac = model.HocVien_MCKhac,

                HocVien_LePhi_MaThamChieu = model.HocVien_LePhi_MaThamChieu,
                HocVien_LePhi_TepMinhChung = model.HocVien_LePhi_TepMinhChung,
                HocVien_LePhi_NgayNop = model.HocVien_LePhi_NgayNop,
                HocVien_LePhi_TrangThai = model.HocVien_LePhi_TrangThai,

                Hv_BangDaiHoc_TenTruongTN = bangDaiHoc.HocVien_BangDaiHoc_TenTruongTN,
                Hv_BangDaiHoc_HeDaoTao = bangDaiHoc.HocVien_BangDaiHoc_HeDaoTao,
                Hv_BangDaiHoc_TenNganhTN = bangDaiHoc.HocVien_BangDaiHoc_TenNganhTN,
                Hv_BangDaiHoc_NamTN = bangDaiHoc.HocVien_BangDaiHoc_NamTN,
                Hv_BangDaiHoc_ThangDiem = bangDaiHoc.HocVien_BangDaiHoc_ThangDiem,
                Hv_BangDaiHoc_DiemToanKhoa = bangDaiHoc.HocVien_BangDaiHoc_DiemToanKhoa,
                Hv_BangDaiHoc_LoaiTN = bangDaiHoc.HocVien_BangDaiHoc_LoaiTN,

                Nganh_Mt_MaNganh = model.NganhMaster.Nganh_Mt_MaNganh,
                Nganh_Mt_TenNganh = model.NganhMaster.Nganh_Mt_TenNganh,
                Nganh_Mt_NghienCuu_Ten = model.NganhMaster.Nganh_Mt_NghienCuu_Ten,


            };
            return Json(new { success = true, data = data_hv }, JsonRequestBehavior.AllowGet);
        }
        [AdminSessionCheck]
        public JsonResult DsHvDuTuyenCapNhatThongTinHoso(HocVienDuTuyen entity)
        {
            var model = db.HocVienDuTuyens
                          .Include(h => h.DotXetTuyen)
                          .Include(h => h.HocVienDangKy)
                          .Include(h => h.NganhMaster)
                          .Where(x => x.DuTuyen_ID == entity.DuTuyen_ID).FirstOrDefault();

            model.DuTuyen_TrangThai = entity.DuTuyen_TrangThai;
            model.DuTuyen_ThongBaoKiemDuyet = entity.DuTuyen_ThongBaoKiemDuyet;
            model.HocVien_LePhi_TrangThai = entity.HocVien_LePhi_TrangThai;
            db.SaveChanges();

            #region Gửi mail xác thực         
            var dotdt = db.DotXetTuyens.Where(x => x.Dxt_Classify == 2 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            SendEmail send = new SendEmail();
            var subject = "Đăng ký dự tuyển sau đại học";
            string str_trangthai_hoso = "";
            string str_trangthai_lephi = "";
            if (model.DuTuyen_TrangThai == 2)
            {
                str_trangthai_hoso = "- Hồ sơ của bạn có thông tin sai, vui lòng đăng nhập hệ thống kiểm tra lại";
            }
            if (model.DuTuyen_TrangThai == 9)
            {
                str_trangthai_hoso = "- Hồ sơ của bạn đúng và đầy đủ thông tin";
            }

            if (model.HocVien_LePhi_TrangThai == 2)
            {
                str_trangthai_lephi = " Có sai sót, vui lòng đăng nhập và kiểm tra lại";
            }
            if (model.HocVien_LePhi_TrangThai == 9)
            {
                str_trangthai_lephi = " Đúng, đủ";
            }
            var body = "Xin chào " + model.HocVienDangKy.HocVien_HoDem + " " + model.HocVienDangKy.HocVien_Ten + ", <br/> Hệ thống đã kiểm duyệt hồ sơ  đăng ký dự tuyển sau đại học của bạn tại trường Đại học Hồng Đức." +
                 "<br/> - Chuyên ngành đăng ký:" +
                 "<br/> + Mã chuyên ngành: " + model.NganhMaster.Nganh_Mt_MaNganh +
                 "<br/> + Tên chuyên ngành: " + model.NganhMaster.Nganh_Mt_TenNganh +
                 "<br/> - Đợt tuyển: " + dotdt.Dxt_Ten + ", từ ngày:" + dotdt.Dxt_ThoiGian_BatDau + " đến ngày: " + dotdt.Dxt_ThoiGian_KetThuc +
                 "<br/> - Thông tin nộp hồ sơ: " + str_trangthai_hoso +
                 "<br/> - Thông tin nộp lệ phí: " + str_trangthai_lephi + "<br/>";
            send.Sendemail(model.HocVienDangKy.HocVien_Email, body, subject);
            #endregion

            return Json(new { success = true, data = "" }, JsonRequestBehavior.AllowGet);
        }
        /*
        public void ExportHvDKDuTuyen()
        {
          var ListHvDts = ListHvDuTuyenExport;// db.HocVienDuTuyens.Include(h => h.DotXetTuyen).Include(h => h.HocVienDangKy).Include(h => h.NganhMaster).ToList();
            var dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_Classify == 2 && d.Dxt_TrangThai_Xt == 1);
            try
            {
                using (ExcelPackage _excelpackage = new ExcelPackage())
                {
                    _excelpackage.Workbook.Properties.Author = "208Team";  // đặt tên người tạo file                       
                    _excelpackage.Workbook.Properties.Title = "TKHVDKDuTuyen"; // đặt tiêu đề cho file                    

                    //Tạo sheet để làm việc 
                    _excelpackage.Workbook.Worksheets.Add("ThongKeHVDKDuTuyen");
                    string[] arr_col_number = { "TT", "Họ, tên đệm", "Tên", "Ngày sinh", "Mã ngành",
                        "Tên ngành đăng ký", "ĐKDT Ngoại ngữ", "Nơi sinh", "Điện thoại","Email" ,"Nơi ở hiện nay", "Địa chỉ liên hệ"};

                    ExcelWorksheet ws = null; // khai báo để thao tác với ws

                    // lấy sheet vừa add ra để thao tác 

                    if (ListHvDts.Count > 0)
                    {
                        ws = _excelpackage.Workbook.Worksheets[1];

                        ws.Name = "ThongKeHVDKDuTuyen";  // đặt tên cho sheet                       
                        ws.Cells.Style.Font.Size = 12;  // fontsize mặc định cho cả sheet                       
                        ws.Cells.Style.Font.Name = "Times New Roman"; // font family mặc định cho cả sheet

                        ws.Cells[1, 1].Value = "DANH SÁCH HỌC VIÊN DỰ TUYỂN SAU ĐẠI HỌC";
                        ws.Cells[1, 1, 1, 12].Merge = true;
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[2, 1].Value = "Số liệu thống kê dự tuyển " + dxt_hientai.Dxt_Ten + ", Từ ngày " + dxt_hientai.Dxt_ThoiGian_BatDau + " đến ngày " + dxt_hientai.Dxt_ThoiGian_KetThuc;
                        ws.Cells[2, 1, 2, 12].Merge = true;
                        //ws.Cells["A1:F1"].Merge = true;

                        // Tạo danh sách các tiêu đề cho cột (column header)                         
                        int colIndex = 1, rowIndex = 3;
                        //tạo các header từ column header đã tạo từ bên trên
                        foreach (var item in arr_col_number)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = item;
                            colIndex++;
                        }

                        rowIndex = 3;
                        // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                        foreach (var item in ListHvDts)
                        {
                            colIndex = 1; // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                            rowIndex++;  // rowIndex tương ứng từng dòng dữ liệu
                            //gán giá trị cho từng cell                      
                            ws.Cells.Style.Font.Bold = false;
                            ws.Cells.Style.WrapText = true;
                            ws.Cells[rowIndex, colIndex++].Value = (rowIndex - 3);                          //  1 số thư tự 
                            ws.Cells[rowIndex, colIndex++].Value = item.HocVienDangKy.HocVien_HoDem;        //  2 
                            ws.Cells[rowIndex, colIndex++].Value = item.HocVienDangKy.HocVien_Ten;          //  3
                            ws.Cells[rowIndex, colIndex++].Value = item.HocVienDangKy.HocVien_NgaySinh;     //  4

                            ws.Cells[rowIndex, colIndex++].Value = item.NganhMaster.Nganh_Mt_MaNganh;       //  5
                            ws.Cells[rowIndex, colIndex++].Value = item.NganhMaster.Nganh_Mt_TenNganh;      //  6
                            if (item.HocVien_DKDTNgoaiNgu == 1)                                              //  7
                            {
                                ws.Cells[rowIndex, colIndex++].Value = "ĐK dự thi";
                            }
                            if (item.HocVien_DKDTNgoaiNgu == 0)
                            {
                                ws.Cells[rowIndex, colIndex++].Value = "Không DTNN";
                            }
                            ws.Cells[rowIndex, colIndex++].Value = db.Tinhs.FirstOrDefault(x => x.Tinh_ID == item.HocVienDangKy.HocVien_NoiSinh).Tinh_Ten;  // nơi sinh = tên tỉnh         //8
                            ws.Cells[rowIndex, colIndex++].Value = item.HocVienDangKy.HocVien_DienThoai;        //  9
                            ws.Cells[rowIndex, colIndex++].Value = item.HocVienDangKy.HocVien_Email;            //  10

                            ws.Cells[rowIndex, colIndex++].Value = item.HocVienDangKy.HocVien_NoiOHienNay;      //  11
                            ws.Cells[rowIndex, colIndex++].Value = item.HocVienDangKy.HocVien_DiaChiLienHe;     //  12

                            ws.Cells[rowIndex, colIndex++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        //{
                        //    "TT", "Họ, tên đệm", "Tên", "Ngày sinh", "Mã ngành đăng ký",
                        //"Tên ngành đăng ký", "ĐK Dự thi Ngoại ngữ", "Nơi sinh", "Điện thoại","Email" };
                        for (int indexCol = 1; indexCol <= arr_col_number.Count(); indexCol++)
                        {
                            if (indexCol == 1) { ws.Column(indexCol).Width = 6; }       //1
                            if (indexCol == 2) { ws.Column(indexCol).Width = 15; }      //2
                            if (indexCol == 3) { ws.Column(indexCol).Width = 7.3; }     //3
                            if (indexCol == 4) { ws.Column(indexCol).Width = 14.3; }    //4
                            if (indexCol == 5) { ws.Column(indexCol).Width = 13; }      //5
                            if (indexCol == 6) { ws.Column(indexCol).Width = 30; }      //6 
                            if (indexCol == 7) { ws.Column(indexCol).Width = 20; }      //7 
                            if (indexCol == 8) { ws.Column(indexCol).Width = 15; }      //8 
                            if (indexCol == 9) { ws.Column(indexCol).Width = 15; }      //9  
                            if (indexCol == 10) { ws.Column(indexCol).Width = 25; }     //10  
                            if (indexCol == 11) { ws.Column(indexCol).Width = 40; }     //11  
                            if (indexCol == 12) { ws.Column(indexCol).Width = 40; }     //12  
                            ws.Cells[3, 1, 3, indexCol].Style.Font.Bold = true;         // đặt tiêu đề cho bảng có kiểu chữ đậm
                        }
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[1, 1].Style.Font.Bold = true;
                        ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    }
                    //Lưu file lại   //string excelName = "ThongKeHVDKDuTuyen";
                    using (var memoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + ws.Name + ".xlsx"); // tên file lưu
                        _excelpackage.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }
        */
        [AdminSessionCheck]
        public ActionResult QLDotDuTuyenSDH_Add()
        {
            var model = db.DotXetTuyens.OrderByDescending(x => x.Dxt_ID).Where(x => x.Dxt_Classify == 2);

            return View(model.ToList());
        }
        [AdminSessionCheck]
        public ActionResult QLDotDuTuyenSDH_Update()
        {
            var model = db.DotXetTuyens.OrderByDescending(x => x.Dxt_ID).Where(x => x.Dxt_Classify == 2);

            return View(model.ToList());
        }
        [AdminSessionCheck]
        public ActionResult QLDotDuTuyenSDH_Delete()
        {
            var model = db.DotXetTuyens.OrderByDescending(x => x.Dxt_ID).Where(x => x.Dxt_Classify == 2);
            return View(model.ToList());
        }

        public JsonResult UpdateLePhiTrangThai(string dutuyenID, string trangthai)
        {
            int id = int.Parse(dutuyenID);
            var model = db.HocVienDuTuyens
                          .Include(h => h.DotXetTuyen)
                          .Include(h => h.HocVienDangKy)
                          .Include(h => h.NganhMaster)
                          .Where(x => x.DuTuyen_ID == id).FirstOrDefault();

            model.HocVien_LePhi_TrangThai = int.Parse(trangthai);
            db.SaveChanges();

            return Json(new { success = true, data = trangthai }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NganhMasterGetDataJson()
        {
            var NganhMasterList = db.NganhMasters.Select(n => new
            {
                nganh_Mt_ID = n.Nganh_Mt_ID,
                nganh_Mt_MaNganh = n.Nganh_Mt_MaNganh,
                nganh_Mt_TenNganh = n.Nganh_Mt_TenNganh,
                nganh_Mt_DinhHuongNghienCuu_Ten = n.Nganh_Mt_NghienCuu_Ten,
                nganh_Mt_DinhHuongNghienCuu_Ma = n.Nganh_Mt_NghienCuu_Ma,
                khoa_ID = n.Khoa_ID,

            });
            return Json(new { success = true, data = NganhMasterList.ToList() });
        }
        public string ComputeHash(string input_user, string input_pass)
        {
            string input = input_user.Trim() + input_pass.Trim();
            string hashedPassword = BC.HashPassword(input);
            return hashedPassword;
        }
        public JsonResult GetTinhJson()
        {
            var TinhList = db.Tinhs.Select(t => new
            {
                tinh_ID = t.Tinh_ID,
                tinh_Ma = t.Tinh_Ma,
                tinh_Ten = t.Tinh_Ten
            });
            return Json(new { success = true, data = TinhList.ToList() });
        }

        public JsonResult NganhMasterGetDataByIDJson(int id)
        {
            var NganhMasterList_byID = db.NganhMasters.Where(x => x.Nganh_Mt_ID == id).Select(n => new
            {
                nganh_Mt_ID = n.Nganh_Mt_ID,
                nganh_Mt_MaNganh = n.Nganh_Mt_MaNganh,
                nganh_Mt_TenNganh = n.Nganh_Mt_TenNganh,
                nganh_Mt_NghienCuu_Ten = n.Nganh_Mt_NghienCuu_Ten,
                nganh_Mt_NghienCuu_Ma = n.Nganh_Mt_NghienCuu_Ma,
                nganh_Mt_khoa_ID = n.Khoa_ID,
                nganh_Mt_TenKhoa = n.Nganh_Mt_TenKhoa,
                nganh_Mt_TrangThai = n.Nganh_Mt_TrangThai
            });
            return Json(new { success = true, data = NganhMasterList_byID.ToList(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult RegisterMasterCheckCCCDAD(HocVienDangKy entity)
        {
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
        public JsonResult RegisterMasterCheckEmailAD(HocVienDangKy entity)
        {
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

        public JsonResult DangKyDuTuyen_SDH_UploadFile_Multi()
        {

            bool check = false;
            string HocVien_SoYeuLyLich_url = "";
            string HocVien_MCBangDaiHoc_url = "";
            string HocVien_MCBangDiem_url = "";
            string HocVien_MCGiayKhamSucKhoe_url = "";
            string HocVien_Anh46_url = "";
            string HocVien_MCCCNN_url = "";
            string HocVien_MCKhac_url = "";
            try
            {
                int so_file_syll = int.Parse(Request["so_file_syll"].ToString());
                int so_file_bangdh = int.Parse(Request["so_file_bangdh"].ToString());
                int so_file_bangdiem = int.Parse(Request["so_file_bangdiem"].ToString());
                int so_file_gksk = int.Parse(Request["so_file_gksk"].ToString());
                int so_file_anh4x6 = int.Parse(Request["so_file_anh4x6"].ToString());
                int so_file_ccnn = int.Parse(Request["so_file_ccnn"].ToString());
                int so_file_mckhac = int.Parse(Request["so_file_mckhac"].ToString());
                string so_cc_congdan = Request["_hocVien_CCCD"].ToString();

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
                    if (i < so_file_syll)
                    {
                        HocVien_SoYeuLyLich_url += "/Uploads/DkSauDaiHocFile/" + fname + "#";
                    }
                    if (i >= so_file_syll && i < so_file_syll + so_file_bangdh)
                    {
                        HocVien_MCBangDaiHoc_url += "/Uploads/DkSauDaiHocFile/" + fname + "#";
                    }

                    if (i >= so_file_syll + so_file_bangdh && i < so_file_syll + so_file_bangdh + so_file_bangdiem)
                    {
                        HocVien_MCBangDiem_url += "/Uploads/DkSauDaiHocFile/" + fname + "#";
                    }
                    if (i >= so_file_syll + so_file_bangdh + so_file_bangdiem && i < so_file_syll + so_file_bangdh + so_file_bangdiem + so_file_gksk)
                    {
                        HocVien_MCGiayKhamSucKhoe_url += "/Uploads/DkSauDaiHocFile/" + fname + "#";
                    }

                    if (i >= so_file_syll + so_file_bangdh + so_file_bangdiem + so_file_gksk && i < so_file_syll + so_file_bangdh + so_file_bangdiem + so_file_gksk + so_file_anh4x6)
                    {
                        HocVien_Anh46_url += "/Uploads/DkSauDaiHocFile/" + fname + "#";
                    }

                    if (i >= so_file_syll + so_file_bangdh + so_file_bangdiem + so_file_gksk + so_file_anh4x6 && i < so_file_syll + so_file_bangdh + so_file_bangdiem + so_file_gksk + so_file_anh4x6 + so_file_ccnn)
                    {
                        HocVien_MCCCNN_url += "/Uploads/DkSauDaiHocFile/" + fname + "#";
                    }

                    if (i >= so_file_syll + so_file_bangdh + so_file_bangdiem + so_file_gksk + so_file_anh4x6 + so_file_ccnn && i < so_file_syll + so_file_bangdh + so_file_bangdiem + so_file_gksk + so_file_anh4x6 + so_file_ccnn + so_file_mckhac)
                    {
                        HocVien_MCKhac_url += "/Uploads/DkSauDaiHocFile/" + fname + "#";
                    }

                    // Get the complete folder path and store the file inside it.      
                    fname = Path.Combine(Server.MapPath("~/Uploads/DkSauDaiHocFile/"), fname);
                    file.SaveAs(fname);
                }
                check = true;
            }
            catch { check = false; }        
            return Json(new { success = check, data = new { HocVien_SoYeuLyLich_url, HocVien_MCBangDaiHoc_url, HocVien_MCBangDiem_url, HocVien_MCGiayKhamSucKhoe_url, HocVien_Anh46_url, HocVien_MCCCNN_url, HocVien_MCKhac_url } }, JsonRequestBehavior.AllowGet);

        }
    }
}