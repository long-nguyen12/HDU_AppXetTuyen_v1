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

namespace HDU_AppXetTuyen.Controllers
{
    public class HocVienDangKiesController : Controller
    {
        private DbConnecttion db = new DbConnecttion();
        #region thông tin học viên
        public ActionResult HvThongTin()
        {            
            return View();
        }
        public JsonResult HvThongTinGetByID()
        {
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
        [HttpPost]
        public JsonResult HvThongTinUpdate_DangKy(HocVienDangKy entity)
        {
            var model = db.HocVienDangKies.Where(x => x.HocVien_ID == entity.HocVien_ID).FirstOrDefault();
            model.HocVien_HoDem = entity.HocVien_HoDem;
            model.HocVien_Ten = entity.HocVien_Ten;
            model.HocVien_GioiTinh = entity.HocVien_GioiTinh;
            model.HocVien_NgaySinh = entity.HocVien_NgaySinh;
            model.HocVien_CCCD_NgayCap = entity.HocVien_CCCD_NgayCap;

            model.HocVien_BangDaiHoc = entity.HocVien_BangDaiHoc;
            model.HocVien_DoiTuongUuTien = entity.HocVien_DoiTuongUuTien;
            model.HocVien_BoTucKienThuc = entity.HocVien_BoTucKienThuc;

            model.HocVien_DienThoai = entity.HocVien_DienThoai;
            model.HocVien_NoiSinh = entity.HocVien_NoiSinh;
            model.HocVien_NoiOHienNay = entity.HocVien_NoiOHienNay;
            model.HocVien_DiaChiLienHe = entity.HocVien_DiaChiLienHe;

            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Đăng ký dự tuyển
        public ActionResult DkDuTuyen()
        {
            return View();
        }
        public JsonResult DkDuTuyenGetByID()
        {
            var str_check = Session["login_session"].ToString();

            var id_dxt = db.DotXetTuyens.FirstOrDefault(x => x.Dxt_Classify == 2 && x.Dxt_TrangThai_Xt == 1).Dxt_ID;
            var model_data = db.HocVienDuTuyens.Include(h => h.DotXetTuyen).Include(h => h.HocVienDangKy).Include(h => h.NganhMaster).Where(x => x.Dxt_ID == id_dxt);

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
     
        [HttpPost]
        public JsonResult HvDangkyUpdateDonViCongTac(HocVienDangKy entity)
        {
            var model = db.HocVienDangKies.Where(x => x.HocVien_ID == entity.HocVien_ID).FirstOrDefault();
            model.HocVien_TenDonViCongTac = entity.HocVien_TenDonViCongTac;
            model.HocVien_ChuyenMon = entity.HocVien_ChuyenMon;
            model.HocVien_ThamNien = entity.HocVien_ThamNien;
            model.HocVien_ChucVu = entity.HocVien_ChucVu;
            model.HocVien_NamCT = entity.HocVien_NamCT;
            model.HocVien_LoaiCB = entity.HocVien_LoaiCB;

            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DkDuTuyen_DangKy(HocVienDuTuyen entity)
        {
            var hv_detail = db.HocVienDangKies.Where(x => x.HocVien_ID == entity.HocVien_ID).FirstOrDefault();
            var nganh_detail = db.NganhMasters.Where(x => x.Nganh_Mt_ID == entity.Nganh_Mt_ID).FirstOrDefault();
            var dotdt = db.DotXetTuyens.Where(x => x.Dxt_Classify == 2 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            HocVienDuTuyen model = new HocVienDuTuyen();
            model.HocVien_ID = entity.HocVien_ID;
            model.DuTuyen_TrangThai = 1;
            model.DuTuyen_NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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

            db.HocVienDuTuyens.Add(model);
            db.SaveChanges();
            #region Gửi mail xác thực
            SendEmail send = new SendEmail();
            var subject = "Đăng ký dự tuyển sau đại học";
            var body = "Xin chào " + hv_detail.HocVien_HoDem + " " + hv_detail.HocVien_Ten + ", <br/> Bạn vừa đăng ký thành công dự tuyển sau đại học trên hệ thống trực tuyến của trường Đại học Hồng Đức." +
                 " <br/>Chuyên ngành đăng ký:" +
                 " <br/> Mã chuyên ngành: " + nganh_detail.Nganh_Mt_MaNganh +
                 " <br/> Tên chuyên ngành: " + nganh_detail.Nganh_Mt_TenNganh + " <br/>";
            send.Sendemail(hv_detail.HocVien_Email, body, subject);
            #endregion
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult KqDuTuyen()
        {
            return View();
        }
        public ActionResult HdSD()
        {
            return View();
        }

        #region thi sinh dang ky cao học

        public JsonResult NganhMasterGetData()
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

        public JsonResult NganhMasterGetData_ByID(int id)
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

        public JsonResult DangKyDuTuyen_SDH_UploadFile_Multi()
        {
            string str_pas = Session["login_session"].ToString();
            var hv_detail = db.HocVienDangKies.Where(x => x.HocVien_MatKhau == str_pas).FirstOrDefault();

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
                string so_cc_congdan = hv_detail.HocVien_CCCD;


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

                    if (i >= so_file_syll + so_file_bangdh + so_file_bangdiem + so_file_gksk && i < so_file_syll + so_file_bangdh + so_file_bangdiem + so_file_gksk + so_file_anh4x6 + so_file_ccnn)
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

            if (check == true)
            {
                return Json(new { success = true, HocVien_SoYeuLyLich_url, HocVien_MCBangDaiHoc_url, HocVien_MCBangDiem_url, HocVien_MCGiayKhamSucKhoe_url, HocVien_Anh46_url, HocVien_MCCCNN_url, HocVien_MCKhac_url, }, JsonRequestBehavior.AllowGet);
            }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }
        #endregion
    }
}