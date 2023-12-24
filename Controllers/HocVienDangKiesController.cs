using HDU_AppXetTuyen.Models;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Net;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using PagedList;

namespace HDU_AppXetTuyen.Controllers
{
    public class HocVienDangKiesController : Controller
    {
        private DbConnecttion db = null;// new DbConnecttion();
        #region thông tin học viên
        [ThiSinhSessionCheck]
        public ActionResult HvThongTin()
        {
            return View();
        }
        [ThiSinhSessionCheck]
        public JsonResult HvThongTinGetByID()
        {
            db = new DbConnecttion();
            var str_check = Session["login_session"].ToString();

            var model = db.HocVienDangKies.Where(x => x.HocVien_MatKhau == str_check).FirstOrDefault();

            if (model != null)
            {
                var TenTinh = db.Tinhs.FirstOrDefault(x => x.Tinh_ID == model.HocVien_NoiSinh).Tinh_Ten;
                var data = new
                {
                    HocVien_ID = model.HocVien_ID,
                    HocVien_HoDem = model.HocVien_HoDem,
                    HocVien_Ten = model.HocVien_Ten,
                    HocVien_GioiTinh = model.HocVien_GioiTinh,
                    HocVien_DanToc = model.HocVien_DanToc,
                    HocVien_NgaySinh = model.HocVien_NgaySinh,
                    HocVien_CCCD = model.HocVien_CCCD,
                    HocVien_CCCD_NgayCap = model.HocVien_CCCD_NgayCap,

                    HocVien_DienThoai = model.HocVien_DienThoai,
                    HocVien_Email = model.HocVien_Email,
                    HocVien_NoiSinh = TenTinh,
                    HocVien_HoKhauThuongTru = model.HocVien_HoKhauThuongTru,
                    HocVien_NoiOHienNay = model.HocVien_NoiOHienNay,
                    HocVien_DiaChiLienHe = model.HocVien_DiaChiLienHe,

                    // hiển thị cho phần đăng ký dự tuyển nếu có dữ liệu
                    HocVien_TenDonViCongTac = model.HocVien_TenDonViCongTac,
                    HocVien_ChuyenMon = model.HocVien_ChuyenMon,
                    HocVien_ThamNien = model.HocVien_ThamNien,
                    HocVien_ChucVu = model.HocVien_ChucVu,
                    HocVien_NamCT = model.HocVien_NamCT,
                    HocVien_LoaiCB = model.HocVien_LoaiCB,

                    HocVien_NoiSinh_ID = model.HocVien_NoiSinh,
                };
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }
        [ThiSinhSessionCheck]
        public JsonResult HvThongTinGetByID_2()
        {
            db = new DbConnecttion();
            var str_check = Session["login_session"].ToString();
         
            var model = db.HocVienDangKies.Where(x => x.HocVien_MatKhau == str_check).FirstOrDefault();

            if (model != null)
            {
                var TenTinh = db.Tinhs.FirstOrDefault(x => x.Tinh_ID == model.HocVien_NoiSinh).Tinh_Ten;
                BangDaiHoc bangDaiHoc = JsonConvert.DeserializeObject<BangDaiHoc>(model.HocVien_BangDaiHoc);
                var data = new
                {
                    HocVien_ID = model.HocVien_ID,
                    HocVien_HoDem = model.HocVien_HoDem,
                    HocVien_Ten = model.HocVien_Ten,
                    HocVien_GioiTinh = model.HocVien_GioiTinh,
                    HocVien_DanToc = model.HocVien_DanToc,
                    HocVien_NgaySinh = model.HocVien_NgaySinh,
                    HocVien_CCCD = model.HocVien_CCCD,
                    HocVien_CCCD_NgayCap = model.HocVien_CCCD_NgayCap,

                    HocVien_BDH_TenTruongTN = bangDaiHoc.HocVien_BangDaiHoc_TenTruongTN,
                    HocVien_BDH_TenNganhTN = bangDaiHoc.HocVien_BangDaiHoc_TenNganhTN,
                    HocVien_BDH_HeDaoTao = bangDaiHoc.HocVien_BangDaiHoc_HeDaoTao,
                    HocVien_BDH_NamTN = bangDaiHoc.HocVien_BangDaiHoc_NamTN,
                    HocVien_BDH_DiemToanKhoa = bangDaiHoc.HocVien_BangDaiHoc_DiemToanKhoa,
                    HocVien_BDH_LoaiTN = bangDaiHoc.HocVien_BangDaiHoc_LoaiTN,

                    HocVien_BoTucKienThuc = model.HocVien_BoTucKienThuc,
                    HocVien_DoiTuongUuTien = model.HocVien_DoiTuongUuTien,
                    HocVien_DienThoai = model.HocVien_DienThoai,
                    HocVien_Email = model.HocVien_Email,
                    HocVien_NoiSinh = TenTinh,
                    HocVien_HoKhauThuongTru = model.HocVien_HoKhauThuongTru,
                    HocVien_NoiOHienNay = model.HocVien_NoiOHienNay,
                    HocVien_DiaChiLienHe = model.HocVien_DiaChiLienHe,

                    // hiển thị cho phần đăng ký dự tuyển nếu có dữ liệu
                    HocVien_TenDonViCongTac = model.HocVien_TenDonViCongTac,
                    HocVien_ChuyenMon = model.HocVien_ChuyenMon,
                    HocVien_ThamNien = model.HocVien_ThamNien,
                    HocVien_ChucVu = model.HocVien_ChucVu,
                    HocVien_NamCT = model.HocVien_NamCT,
                    HocVien_LoaiCB = model.HocVien_LoaiCB,
                    HocVien_NoiSinh_ID = model.HocVien_NoiSinh,
                };
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }
        [ThiSinhSessionCheck]
        [HttpPost]
        public JsonResult HvThongTinUpdate_DangKy(HocVienDangKy entity)
        {
            db = new DbConnecttion();

            var model = db.HocVienDangKies.Where(x => x.HocVien_ID == entity.HocVien_ID).FirstOrDefault();
            model.HocVien_HoDem = entity.HocVien_HoDem;
            model.HocVien_Ten = entity.HocVien_Ten;
            model.HocVien_GioiTinh = entity.HocVien_GioiTinh;
            model.HocVien_NgaySinh = entity.HocVien_NgaySinh;
            model.HocVien_CCCD_NgayCap = entity.HocVien_CCCD_NgayCap;


            model.HocVien_DienThoai = entity.HocVien_DienThoai;
            model.HocVien_NoiSinh = entity.HocVien_NoiSinh;
            model.HocVien_HoKhauThuongTru = entity.HocVien_HoKhauThuongTru;
            model.HocVien_NoiOHienNay = entity.HocVien_NoiOHienNay;
            model.HocVien_DiaChiLienHe = entity.HocVien_DiaChiLienHe;


            model.HocVien_TenDonViCongTac = entity.HocVien_TenDonViCongTac;
            model.HocVien_ChuyenMon = entity.HocVien_ChuyenMon;
            model.HocVien_ThamNien = entity.HocVien_ThamNien;
            model.HocVien_ChucVu = entity.HocVien_ChucVu;
            model.HocVien_NamCT = entity.HocVien_NamCT;
            model.HocVien_LoaiCB = entity.HocVien_LoaiCB;

            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Đăng ký dự tuyển
        [ThiSinhSessionCheck]
        public ActionResult DkDuTuyen()
        {
            db = new DbConnecttion();
            string str_check = Session["login_session"].ToString(); //"$2a$11$G1.12RIjNpGpFHCRiAoDHuBrvXyWpI75atUG3zAj0Z8/D1VsCA1gq";
            ViewBag.HocVien_ID = db.HocVienDangKies.FirstOrDefault(x => x.HocVien_MatKhau == str_check).HocVien_ID;
            return View();
        }
        [ThiSinhSessionCheck]
        public JsonResult DkDuTuyenGetByID()
        {
            db = new DbConnecttion();
            string str_check = Session["login_session"].ToString(); //"$2a$11$G1.12RIjNpGpFHCRiAoDHuBrvXyWpI75atUG3zAj0Z8/D1VsCA1gq";

            var model_hv = db.HocVienDangKies.FirstOrDefault(x => x.HocVien_MatKhau == str_check);
            BangDaiHoc bangDaiHoc = JsonConvert.DeserializeObject<BangDaiHoc>(model_hv.HocVien_BangDaiHoc);

            var id_dxt = db.DotXetTuyens.FirstOrDefault(x => x.Dxt_Classify == 2 && x.Dxt_TrangThai_Xt == 1).Dxt_ID;

            var model_data = db.HocVienDuTuyens
                .Include(h => h.DotXetTuyen)
                .Include(h => h.HocVienDangKy)
                .Include(h => h.NganhMaster)
                .Where(x => x.HocVienDangKy.HocVien_MatKhau == str_check && x.Dxt_ID == id_dxt).ToList();

            if (model_data.Count > 0)
            {
                var data = model_data.Select(s => new
                {

                    DuTuyen_ID_Present = s.DuTuyen_ID,
                    Hv_ID = s.HocVien_ID,
                    Dt_TrangThai = s.DuTuyen_TrangThai,
                    Dt_ThongBaoKiemDuyet = s.DuTuyen_ThongBaoKiemDuyet,
                    maNganh_ID = s.Nganh_Mt_ID,
                    nmt_MaNganh = s.NganhMaster.Nganh_Mt_MaNganh,
                    nmt_TenNganh = s.NganhMaster.Nganh_Mt_TenNganh,
                    Dt_MaNghienCuu = s.DuTuyen_MaNghienCuu,
                    Hv_DKDTNgoaiNgu = s.HocVien_DKDTNgoaiNgu,
                    Hv_VanBangNgoaiNgu = s.HocVien_VanBangNgoaiNgu,
                    Hv_DoiTuongDuThi = s.HocVien_DoiTuongDuThi,

                    Hv_BoTucKienThuc = model_hv.HocVien_BoTucKienThuc,
                    Hv_DoiTuongUuTien = model_hv.HocVien_DoiTuongUuTien,
                    Hv_BangDaiHoc_TenTruongTN = bangDaiHoc.HocVien_BangDaiHoc_TenTruongTN,
                    Hv_BangDaiHoc_HeDaoTao = bangDaiHoc.HocVien_BangDaiHoc_HeDaoTao,
                    Hv_BangDaiHoc_TenNganhTN = bangDaiHoc.HocVien_BangDaiHoc_TenNganhTN,
                    Hv_BangDaiHoc_NamTN = bangDaiHoc.HocVien_BangDaiHoc_NamTN,
                    Hv_BangDaiHoc_ThangDiem = bangDaiHoc.HocVien_BangDaiHoc_ThangDiem,
                    Hv_BangDaiHoc_DiemToanKhoa = bangDaiHoc.HocVien_BangDaiHoc_DiemToanKhoa,
                    Hv_BangDaiHoc_LoaiTN = bangDaiHoc.HocVien_BangDaiHoc_LoaiTN,

                    Hv_SoYeuLyLich = s.HocVien_SoYeuLyLich,
                    Hv_MCBangDaiHoc = s.HocVien_MCBangDaiHoc,
                    Hv_MCBangDiem = s.HocVien_MCBangDiem,
                    Hv_MCGiayKhamSucKhoe = s.HocVien_MCGiayKhamSucKhoe,
                    Hv_Anh46 = s.HocVien_Anh46,
                    Hv_MCCCNN = s.HocVien_MCCCNN,
                    Hv_MCKhac = s.HocVien_MCKhac,

                    Hv_LePhi_MaThamChieu = s.HocVien_LePhi_MaThamChieu,
                    Hv_LePhi_TepMinhChung = s.HocVien_LePhi_TepMinhChung,
                    Hv_LePhi_TrangThai = s.HocVien_LePhi_TrangThai,


                });
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, data = "không có dữ liệu" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost, ThiSinhSessionCheck]
        public JsonResult HocVienDangkyUpdateBangDaiHoc(HocVienDangKy entity)
        {
            db = new DbConnecttion();
            string str_check = Session["login_session"].ToString();
            var model = db.HocVienDangKies.Where(x => x.HocVien_MatKhau == str_check).FirstOrDefault();
            //var model = db.HocVienDangKies.Where(x => x.HocVien_ID == entity.HocVien_ID).FirstOrDefault();
            model.HocVien_BoTucKienThuc = entity.HocVien_BoTucKienThuc;
            model.HocVien_BangDaiHoc = entity.HocVien_BangDaiHoc;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ThiSinhSessionCheck]
        public JsonResult DkDuTuyenAdd(HocVienDuTuyen entity)
        {
            db = new DbConnecttion();
            string str_check = Session["login_session"].ToString();

            //var hv_detail = db.HocVienDangKies.Where(x => x.HocVien_ID == entity.HocVien_ID).FirstOrDefault();
            var hv_detail = db.HocVienDangKies.Where(x => x.HocVien_MatKhau == str_check).FirstOrDefault();
            var dotdt = db.DotXetTuyens.Where(x => x.Dxt_Classify == 2 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            HocVienDuTuyen model = new HocVienDuTuyen();

            model.HocVien_ID = hv_detail.HocVien_ID;
            model.DuTuyen_TrangThai = 1;
            model.DuTuyen_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd");
            model.Dxt_ID = dotdt.Dxt_ID;
            model.DuTuyen_MaNghienCuu = entity.DuTuyen_MaNghienCuu;

            model.Nganh_Mt_ID = entity.Nganh_Mt_ID;
            model.HocVien_DKDTNgoaiNgu = entity.HocVien_DKDTNgoaiNgu;
            model.HocVien_VanBangNgoaiNgu = entity.HocVien_VanBangNgoaiNgu;
            model.HocVien_DoiTuongDuThi = entity.HocVien_DoiTuongDuThi;

            model.HocVien_SoYeuLyLich = entity.HocVien_SoYeuLyLich;
            model.HocVien_MCBangDaiHoc = entity.HocVien_MCBangDaiHoc;
            model.HocVien_MCBangDiem = entity.HocVien_MCBangDiem;
            model.HocVien_MCGiayKhamSucKhoe = entity.HocVien_MCGiayKhamSucKhoe;
            model.HocVien_Anh46 = entity.HocVien_Anh46;
            model.HocVien_MCCCNN = entity.HocVien_MCCCNN;
            model.HocVien_MCKhac = entity.HocVien_MCKhac;

            model.HocVien_LePhi_TrangThai = 0;
            model.HocVien_LePhi_MaThamChieu = "0";

            db.HocVienDuTuyens.Add(model);

            db.SaveChanges();
            #region Gửi mail xác thực

            var nganh_detail = db.NganhMasters.Where(x => x.Nganh_Mt_ID == entity.Nganh_Mt_ID).FirstOrDefault();
            SendEmail send = new SendEmail();
            var subject = "Đăng ký dự tuyển sau đại học";
            var body = "Xin chào " + hv_detail.HocVien_HoDem + " " + hv_detail.HocVien_Ten + ", <br/> Bạn vừa đăng ký thành công dự tuyển sau đại học trên hệ thống trực tuyến của trường Đại học Hồng Đức." +
                 "<br/>Chuyên ngành đăng ký:" +
                 "<br/> Mã chuyên ngành: " + nganh_detail.Nganh_Mt_MaNganh +
                 "<br/> Tên chuyên ngành: " + nganh_detail.Nganh_Mt_TenNganh +
                 "<br/> Đợt tuyển: " + dotdt.Dxt_Ten + ", từ ngày:" + dotdt.Dxt_ThoiGian_BatDau + " đến ngày: " + dotdt.Dxt_ThoiGian_KetThuc + "<br/>";
            send.Sendemail(hv_detail.HocVien_Email, body, subject);
            #endregion
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [ThiSinhSessionCheck]
        public ActionResult DkDuTuyenEdit()
        {
            return View();
        }
        [HttpPost]
        [ThiSinhSessionCheck]
        public JsonResult DkDuTuyenEdit(HocVienDuTuyen entity)
        {
            db = new DbConnecttion();

            HocVienDuTuyen model = db.HocVienDuTuyens.Where(x => x.DuTuyen_ID == entity.DuTuyen_ID).FirstOrDefault();         

            model.DuTuyen_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd");
            model.DuTuyen_MaNghienCuu = entity.DuTuyen_MaNghienCuu;

            model.Nganh_Mt_ID = entity.Nganh_Mt_ID;
            model.HocVien_DKDTNgoaiNgu = entity.HocVien_DKDTNgoaiNgu;
            model.HocVien_VanBangNgoaiNgu = entity.HocVien_VanBangNgoaiNgu;
            model.HocVien_DoiTuongDuThi = entity.HocVien_DoiTuongDuThi;

            model.HocVien_SoYeuLyLich += entity.HocVien_SoYeuLyLich;
            model.HocVien_MCBangDaiHoc += entity.HocVien_MCBangDaiHoc;
            model.HocVien_MCBangDiem += entity.HocVien_MCBangDiem;
            model.HocVien_MCGiayKhamSucKhoe += entity.HocVien_MCGiayKhamSucKhoe;
            model.HocVien_Anh46 += entity.HocVien_Anh46;
            model.HocVien_MCCCNN += entity.HocVien_MCCCNN;
            model.HocVien_MCKhac += entity.HocVien_MCKhac;

            db.SaveChanges();
            #region Gửi mail xác thực
            SendEmail send = new SendEmail();
            var hv_detail = db.HocVienDangKies.Where(x => x.HocVien_ID == model.HocVien_ID).FirstOrDefault();
            var nganh_detail = db.NganhMasters.Where(x => x.Nganh_Mt_ID == entity.Nganh_Mt_ID).FirstOrDefault();
            var dotdt = db.DotXetTuyens.Where(x => x.Dxt_Classify == 2 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            var subject = "Đăng ký dự tuyển sau đại học";
            var body = "Xin chào " + hv_detail.HocVien_HoDem + " " + hv_detail.HocVien_Ten + ", <br/> Bạn vừa sửa thông tin đăng ký dự tuyển sau đại học trên hệ thống trực tuyến của trường Đại học Hồng Đức." +
                 "<br/> Chuyên ngành đăng ký:" +
                 "<br/> Mã chuyên ngành: " + nganh_detail.Nganh_Mt_MaNganh +
                 "<br/> Tên chuyên ngành: " + nganh_detail.Nganh_Mt_TenNganh +
                 "<br/> Đợt tuyển: " + dotdt.Dxt_Ten + ", từ ngày:" + dotdt.Dxt_ThoiGian_BatDau + " đến ngày: " + dotdt.Dxt_ThoiGian_KetThuc + "<br/>";
            send.Sendemail(hv_detail.HocVien_Email, body, subject);
            #endregion
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        [ThiSinhSessionCheck]
        public JsonResult DkDuTuyenEdit_MCLePhi(HocVienDuTuyen entity)
        {
            db = new DbConnecttion();
            try
            {
                HocVienDuTuyen model = db.HocVienDuTuyens.Find(entity.DuTuyen_ID);

                model.HocVien_LePhi_NgayNop = (DateTime.Now.ToString("yyyy-MM-dd")).ToString();
                model.HocVien_LePhi_MaThamChieu = entity.HocVien_LePhi_MaThamChieu;
                model.HocVien_LePhi_TepMinhChung = entity.HocVien_LePhi_TepMinhChung;
                model.HocVien_LePhi_TrangThai = 1;

                db.SaveChanges();

                #region Gửi mail xác thực
                var hv_detail = db.HocVienDangKies.Where(x => x.HocVien_ID == model.HocVien_ID).FirstOrDefault();
                SendEmail send = new SendEmail();
                var nganh_detail = db.NganhMasters.Where(x => x.Nganh_Mt_ID == model.Nganh_Mt_ID).FirstOrDefault();

                var dotdt = db.DotXetTuyens.Where(x => x.Dxt_Classify == 2 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();

                var subject = "Đăng ký dự tuyển sau đại học";
                var body = "Xin chào " + hv_detail.HocVien_HoDem + " " + hv_detail.HocVien_Ten + ", <br/> Bạn cập nhật thông tin nộp lệ phí đăng ký dự tuyển sau đại học trên hệ thống trực tuyến của trường Đại học Hồng Đức." +
                     "<br/> Chuyên ngành đăng ký:" +
                     "<br/> Mã chuyên ngành: " + nganh_detail.Nganh_Mt_MaNganh +
                     "<br/> Tên chuyên ngành: " + nganh_detail.Nganh_Mt_TenNganh +
                     "<br/> Đợt tuyển: " + dotdt.Dxt_Ten + ", từ ngày:" + dotdt.Dxt_ThoiGian_BatDau + " đến ngày: " + dotdt.Dxt_ThoiGian_KetThuc + "<br/>";
                send.Sendemail(hv_detail.HocVien_Email, body, subject);
                #endregion
                return Json(new { success = true , data = model.HocVien_LePhi_MaThamChieu }, JsonRequestBehavior.AllowGet);
            }
            catch 
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }            
           
        }

        [HttpPost]
        [ThiSinhSessionCheck]
        public JsonResult DkDuTuyenDeleteMinhChungItem(HocVienDuTuyen entity)
        {
            db = new DbConnecttion();
            HocVienDuTuyen model = db.HocVienDuTuyens.Where(x => x.DuTuyen_ID == entity.DuTuyen_ID).FirstOrDefault();

            //if (entity.DuTuyen_MaNghienCuu == "2") { model.HocVien_SoYeuLyLich = model.HocVien_SoYeuLyLich.Replace(entity.Dkxt_Url + "#", ""); }
            //if (entity.Dkxt_LoaiMC == "3") { model.Dkxt_HB_MinhChung_Bang = model.Dkxt_HB_MinhChung_Bang.Replace(entity.Dkxt_Url + "#", ""); }
            //if (entity.Dkxt_LoaiMC == "4") { model.Dkxt_HB_MinhChung_CCCD = model.Dkxt_HB_MinhChung_CCCD.Replace(entity.Dkxt_Url + "#", ""); }
            //if (entity.Dkxt_LoaiMC == "5") { model.Dkxt_HB_MinhChung_UuTien = model.Dkxt_HB_MinhChung_UuTien.Replace(entity.Dkxt_Url + "#", ""); }
            //if (entity.Dkxt_LoaiMC == "6") { model.Dkxt_HB_KinhPhi_TepMinhChung = model.Dkxt_HB_KinhPhi_TepMinhChung.Replace(entity.Dkxt_Url + "#", ""); }

            if (entity.DuTuyen_MaNghienCuu == 1)
            {
                string str_new = "";
                string[] UrlArr = model.HocVien_SoYeuLyLich.Split(new char[] { '#' });
                foreach (string UrlItem in UrlArr)
                {
                    if (UrlItem != entity.DuTuyen_GhiChu && UrlItem.Length > 3) str_new += UrlItem + "#";
                }
                model.HocVien_SoYeuLyLich = str_new;
            }
            if (entity.DuTuyen_MaNghienCuu == 2)
            {
                string str_new = "";
                string[] UrlArr = model.HocVien_MCBangDaiHoc.Split(new char[] { '#' });
                foreach (string UrlItem in UrlArr)
                {
                    if (UrlItem != entity.DuTuyen_GhiChu && UrlItem.Length > 3) str_new += UrlItem + "#";
                }
                model.HocVien_MCBangDaiHoc = str_new;
            }

            if (entity.DuTuyen_MaNghienCuu == 3)
            {
                string str_new = "";
                string[] UrlArr = model.HocVien_MCBangDiem.Split(new char[] { '#' });
                foreach (string UrlItem in UrlArr)
                {
                    if (UrlItem != entity.DuTuyen_GhiChu && UrlItem.Length > 3) str_new += UrlItem + "#";
                }
                model.HocVien_MCBangDiem = str_new;
            }

            if (entity.DuTuyen_MaNghienCuu == 4)
            {
                string str_new = "";
                string[] UrlArr = model.HocVien_MCGiayKhamSucKhoe.Split(new char[] { '#' });
                foreach (string UrlItem in UrlArr)
                {
                    if (UrlItem != entity.DuTuyen_GhiChu && UrlItem.Length > 3) str_new += UrlItem + "#";
                }
                model.HocVien_MCGiayKhamSucKhoe = str_new;
            }

            if (entity.DuTuyen_MaNghienCuu == 5)
            {
                string str_new = "";
                string[] UrlArr = model.HocVien_Anh46.Split(new char[] { '#' });
                foreach (string UrlItem in UrlArr)
                {
                    if (UrlItem != entity.DuTuyen_GhiChu && UrlItem.Length > 3) str_new += UrlItem + "#";
                }
                model.HocVien_Anh46 = str_new;

            }
            if (entity.DuTuyen_MaNghienCuu == 6)
            {
                string str_new = "";
                string[] UrlArr = model.HocVien_MCCCNN.Split(new char[] { '#' });
                foreach (string UrlItem in UrlArr)
                {
                    if (UrlItem != entity.DuTuyen_GhiChu && UrlItem.Length > 3) str_new += UrlItem + "#";
                }
                model.HocVien_MCCCNN = str_new;

            }
            if (entity.DuTuyen_MaNghienCuu == 7)
            {
                string str_new = "";
                string[] UrlArr = model.HocVien_MCKhac.Split(new char[] { '#' });
                foreach (string UrlItem in UrlArr)
                {
                    if (UrlItem != entity.DuTuyen_GhiChu && UrlItem.Length > 3) str_new += UrlItem + "#";
                }
                model.HocVien_MCKhac = str_new;

            }
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ThiSinhSessionCheck]
        public JsonResult DkDuTuyenDelete(HocVienDuTuyen entity)
        {
            db = new DbConnecttion();
            //HocVienDuTuyen hocVienDuTuyen = db.HocVienDuTuyens.Include(h => h.HocVienDangKy).FirstOrDefault(x =>x.HocVien_ID == entity.DuTuyen_ID);
            HocVienDuTuyen hvdt_delete = db.HocVienDuTuyens.Find(entity.DuTuyen_ID);

            #region Gửi mail xác thực
            //SendEmail send = new SendEmail();

            //var hvdk_detail = db.HocVienDangKies.Find(hvdt_delete.HocVien_ID);

            //var subject = "Xóa thông tin dự tuyển sau đại học";
            //var body = "Xin chào " + hvdk_detail.HocVien_HoDem + " " + hvdk_detail.HocVien_Ten + ", <br/> Bạn vừa xóa thông tin đăng ký dự tuyển sau đại học trên hệ thống trực tuyến của trường Đại học Hồng Đức.";

            //send.Sendemail(hvdk_detail.HocVien_Email, body, subject);
            #endregion
            // thực hiện xóa
            db.HocVienDuTuyens.Remove(hvdt_delete);
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [ThiSinhSessionCheck]
        public ActionResult KqDuTuyen()
        {
            return View();
        }
        [ThiSinhSessionCheck]
        public ActionResult HdSD()
        {
            return View();
        }

        #region thi sinh dang ky cao học
        [ThiSinhSessionCheck]
        public JsonResult NganhMasterGetData()
        {
            db = new DbConnecttion();
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
        [ThiSinhSessionCheck]
        public JsonResult NganhMasterGetData_ByID(int id)
        {
            db = new DbConnecttion();
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
        [ThiSinhSessionCheck]
        public JsonResult DangKyDuTuyen_SDH_UploadFile_Lephi()
        {
            db = new DbConnecttion();
            string str_pas = Session["login_session"].ToString();

            var hv_detail = db.HocVienDangKies.Where(x => x.HocVien_MatKhau == str_pas).FirstOrDefault();
            string so_cc_congdan = hv_detail.HocVien_CCCD;

            bool check = false;

            string hocVien_LePhi_TepMC = "";
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
                    hocVien_LePhi_TepMC += "/Uploads/DkSauDaiHocFile/" + fname + "#";

                    // Get the complete folder path and store the file inside it.      
                    fname = Path.Combine(Server.MapPath("~/Uploads/DkSauDaiHocFile/"), fname);
                    file.SaveAs(fname);
                }
                check = true;
            }
            catch { check = false; }

            return Json(new { success = check, hocVien_LePhi_TepMC }, JsonRequestBehavior.AllowGet);

        }
        [ThiSinhSessionCheck]
        public JsonResult DangKyDuTuyen_SDH_UploadFile_Multi()
        {
            db = new DbConnecttion();

            string str_pas = Session["login_session"].ToString();
            //string str_pas = "$2a$11$G1.12RIjNpGpFHCRiAoDHuBrvXyWpI75atUG3zAj0Z8/D1VsCA1gq";
            var hv_detail = db.HocVienDangKies.Where(x => x.HocVien_MatKhau == str_pas).FirstOrDefault();
            string so_cc_congdan = hv_detail.HocVien_CCCD;

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

            return Json(new { success = check, HocVien_SoYeuLyLich_url, HocVien_MCBangDaiHoc_url, HocVien_MCBangDiem_url, HocVien_MCGiayKhamSucKhoe_url, HocVien_Anh46_url, HocVien_MCCCNN_url, HocVien_MCKhac_url, }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region luu tam
        public ActionResult DkDuTuyen2()
        {
            return View();
        }
        public JsonResult DkDuTuyenGetByID2()
        {
            db = new DbConnecttion();
            var str_check = Session["login_session"].ToString();
            //string str_check = "$2a$11$G1.12RIjNpGpFHCRiAoDHuBrvXyWpI75atUG3zAj0Z8/D1VsCA1gq";// Session["login_session"].ToString();
            var id = db.HocVienDangKies.FirstOrDefault(x => x.HocVien_MatKhau == str_check).HocVien_ID;
            var id_dxt = db.DotXetTuyens.FirstOrDefault(x => x.Dxt_Classify == 2 && x.Dxt_TrangThai_Xt == 1).Dxt_ID;
            var model_data = db.HocVienDuTuyens.Include(h => h.DotXetTuyen).Include(h => h.HocVienDangKy).Include(h => h.NganhMaster).Where(x => x.HocVien_ID == id).ToList();

            if (model_data != null)
            {
                var data = model_data.Select(s => new
                {
                    dt_ID = s.DuTuyen_ID,
                    nmt_MaNganh = s.NganhMaster.Nganh_Mt_MaNganh,
                    nmt_TenNganh = s.NganhMaster.Nganh_Mt_TenNganh,
                    Dt_MaNghienCuu = s.DuTuyen_MaNghienCuu,
                    Hv_DKDTNgoaiNgu = s.HocVien_DKDTNgoaiNgu,
                    Hv_DoiTuongDuThi = s.HocVien_DoiTuongDuThi,
                });
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion
    }
}