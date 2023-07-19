using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Newtonsoft.Json;
using HDU_AppXetTuyen.Ultils;
using System.IO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace HDU_AppXetTuyen.Controllers
{
    public class DangKyXetTuyensController : Controller
    {
        #region Đăng ký dự thi năng khiếu Tiểu học, mầm non, gdtc
        [ThiSinhSessionCheck]
        public ActionResult dkdtnk()
        {
            return View();
        }
        [ThiSinhSessionCheck]
        public JsonResult DangKyDuThiNangKhieu_ListAll()
        {
            int ptxt_check = 7;
            DbConnecttion db_tsdk = new DbConnecttion();
            DbConnecttion db_dkdt_nk = new DbConnecttion();
            if (Session["login_session"] != null)
            {
                string str_login_session = Session["login_session"].ToString();

                var tsdk_Detail = db_tsdk.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault(ts => ts.ThiSinh_MatKhau == str_login_session);

                string _xeploai_hocluc_12 = "";
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 4) { _xeploai_hocluc_12 = "Xuất sắc"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 3) { _xeploai_hocluc_12 = "Giỏi"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 2) { _xeploai_hocluc_12 = "Khá"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 1) { _xeploai_hocluc_12 = "Trung bình"; }

                string _xeploai_hanhkiem_12 = "";
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 4) { _xeploai_hanhkiem_12 = "Tốt"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 3) { _xeploai_hanhkiem_12 = "Khá"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 2) { _xeploai_hanhkiem_12 = "Trung bình"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 1) { _xeploai_hanhkiem_12 = "Yếu"; }

                string _ut_doituong_ten_diem = "ĐT " + tsdk_Detail.DoiTuong.DoiTuong_Ten + ": ƯT " + tsdk_Detail.DoiTuong.DoiTuong_DiemUuTien + " đ";
                string _ut_khuvuv_ten_diem = tsdk_Detail.KhuVuc.KhuVuc_Ten + ": ƯT " + tsdk_Detail.KhuVuc.KhuVuc_DiemUuTien + " đ"; ;


                long _thisinh_id = tsdk_Detail.ThiSinh_ID;

                var list_dkdt_nk_ts = db_dkdt_nk.DangKyDuThiNangKhieus.Include(d => d.Nganh).Include(d => d.ThiSinhDangKy).Include(d => d.ToHopMon)
                                          .Where(ts => ts.ThiSinh_ID == _thisinh_id && ts.Ptxt_ID == ptxt_check).ToList();

                var view_list_dkdt_nangkhieu_ts = list_dkdt_nk_ts.Select(s => new
                {

                    dkdt_NK_ID = s.Dkdt_NK_ID,
                    thiSinh_ID = s.ThiSinh_ID,
                    ptxt_ID = s.Ptxt_ID,
                    nganh_ID = s.Nganh_ID,
                    thm_ID = s.Thm_ID,
                    doiTuong_ID = s.DoiTuong_ID,
                    khuVuc_ID = s.KhuVuc_ID,
                    dkdt_NK_TrangThai = s.Dkdt_NK_TrangThai,
                    dotXT_ID = s.DotXT_ID,
                    dkdt_NK_TenAll = new
                    {
                        khoiNganh_Ten = s.Nganh.KhoiNganh.KhoiNganh_Ten,
                        nganh_GhiChu = s.Nganh.Nganh_GhiChu,
                        thm_MaTen = s.ToHopMon.Thm_MaTen,
                    },
                    dkdt_NK_MonThi = s.Dkdt_NK_MonThi,                   

                }).ToList();

                var Tsdk_doituong_ten_diem = _ut_doituong_ten_diem;
                var Tsdk_khuvuv_ten_diem = _ut_khuvuv_ten_diem;
                var Tsdk_xeploai_hocluc_12 = _xeploai_hocluc_12;
                var Tsdk_xeploai_hanhkiem_12 = _xeploai_hanhkiem_12;
                return Json(new
                {
                    view_list_dkdt_nangkhieu_ts,
                    Tsdk_doituong_ten_diem,
                    Tsdk_khuvuv_ten_diem,
                    Tsdk_xeploai_hocluc_12,
                    Tsdk_xeploai_hanhkiem_12

                }, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NganhListForThiNK()
        {
            DbConnecttion nganh_db = new DbConnecttion();

            var selectResult_Nganh = nganh_db.Nganhs.Where(x => x.Nganh_ThiNK == 1).OrderBy(x => x.Nganh_ID).Select(s => new
            {
                nganh_ID = s.Nganh_ID,
                nganh_GhiChu = s.NganhTenNganh,
            });
            return Json(selectResult_Nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ToHopMonListForThiNK(ToHopMon thm_n)
        {
            int _thm_ID = int.Parse(thm_n.Thm_ID.ToString());
            DbConnecttion db_Thm = new DbConnecttion();

            var thm_get_item = db_Thm.ToHopMons.Find(_thm_ID);

            if (thm_get_item.Thm_Thi_NK.ToString() == "2")
            {
                thm_get_item.Thm_Mon3 = "Bật xa tại chỗ và Chạy 100m";
            }

            if (thm_get_item.Thm_Thi_NK.ToString() == "1")
            {
                thm_get_item.Thm_Mon3 = "Đọc diễn cảm và hát";
            }

            var thmObj = new
            {
                thm_Mon3 = thm_get_item.Thm_Mon3
            };

            return Json(thmObj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ToHopMonNganhListAll_ThiNK(int id)
        {
            DbConnecttion thm_nganh_db = new DbConnecttion();
            var selectResult_tohopmon_nganh = thm_nganh_db.ToHopMonNganhs.Include(n => n.ToHopMon)
                .Where(x => x.Nganh_ID == id && x.ToHopMon.Thm_Thi_NK > 0).OrderBy(x => x.ToHopMon.Thm_ID).Select(s => new
                {
                    thm_ID = s.Thm_ID,
                    thm_MaTen = s.ToHopMon.Thm_MaTen,
                    thm_TenToHop = s.ToHopMon.Thm_TenToHop
                });
            return Json(selectResult_tohopmon_nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyDuThi_UploadFile_Multi()
        {
            DbConnecttion db = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString(); //"$2a$11$jwPUP78RBpC9R3uW7Dqpau.SXwogLasbvVx3q0vqhoE93Lx044lcu"; //
            var ts = db.ThiSinhDangKies.FirstOrDefault(n => n.ThiSinh_MatKhau.Equals(str_login_session));
            string cccd = ts.ThiSinh_CCCD;

            HttpFileCollectionBase files = Request.Files;


            string _MinhChung_CCCD = "";

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname;
                // Checking for Internet Explorer      
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + file.FileName;
                }
                // lấy chuỗi lưu vào csdl             
                _MinhChung_CCCD += "#/Uploads/DKXTKQTTHPTQGFile/" + fname;
                // Get the complete folder path and store the file inside it.      
                fname = Path.Combine(Server.MapPath("~/Uploads/DKDTNangKhieuFile/"), fname);
                file.SaveAs(fname);
            }
            return Json(new
            {
                _MinhChung_CCCD,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyDuThi_NangKhieu_Insert(DangKyDuThiNangKhieu dkdt_nk_post)
        {

            DbConnecttion db_tsdk = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString(); //"$2a$11$jwPUP78RBpC9R3uW7Dqpau.SXwogLasbvVx3q0vqhoE93Lx044lcu";// 

            var ts = db_tsdk.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(str_login_session)).
                Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();

            DbConnecttion db_dxt = new DbConnecttion();
            var dotxettuyen = db_dxt.DotXetTuyens.Where(n => n.Dxt_TrangThai == 1).FirstOrDefault();

            if (ts != null)
            {
                DbConnecttion db_dkdt_nk = new DbConnecttion();

                var nvs = db_dkdt_nk.DangKyDuThiNangKhieus.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID).OrderByDescending(x => x.Dkdt_NK_NguyenVong).ToList();

                DangKyDuThiNangKhieu dkdt_nk_new = new DangKyDuThiNangKhieu();

                dkdt_nk_new.Nganh_ID = dkdt_nk_post.Nganh_ID;
                dkdt_nk_new.Thm_ID = dkdt_nk_post.Thm_ID;
                dkdt_nk_new.Dkdt_NK_NamTotNghiep = dkdt_nk_post.Dkdt_NK_NamTotNghiep;
                dkdt_nk_new.Dkdt_NK_MonThi = dkdt_nk_post.Dkdt_NK_MonThi;
                dkdt_nk_new.Dkdt_NK_MinhChung_CCCD = dkdt_nk_post.Dkdt_NK_MinhChung_CCCD;
                dkdt_nk_new.Dkdt_NK_NgayDangKy = DateTime.Now.ToString("dd/MM/YYYY");

                dkdt_nk_new.ThiSinh_ID = ts.ThiSinh_ID;
                dkdt_nk_new.DoiTuong_ID = ts.DoiTuong_ID;
                dkdt_nk_new.KhuVuc_ID = ts.KhuVuc_ID;
                dkdt_nk_new.Dkdt_NK_XepLoaiHocLuc_12 = ts.ThiSinh_HocLucLop12;
                dkdt_nk_new.Dkdt_NK_XepLoaiHanhKiem_12 = ts.ThiSinh_HanhKiemLop12;

                dkdt_nk_new.Ptxt_ID = 7;

                dkdt_nk_new.DotXT_ID = dotxettuyen.Dxt_ID;
                dkdt_nk_new.Dkdt_NK_NguyenVong = nvs.Count + 1;

                dkdt_nk_new.Dkdt_NK_TrangThai = 0;
                dkdt_nk_new.Dkdt_NK_GhiChu = ""; ;
                dkdt_nk_new.Dkdt_NK_TrangThai_KetQua = 0;


                db_dkdt_nk.DangKyDuThiNangKhieus.Add(dkdt_nk_new);
                db_dkdt_nk.SaveChanges();
                // add LePhiXetTuyen
                DbConnecttion db_lephixt = new DbConnecttion();
                var lePhiRecord = db_lephixt.LePhiXetTuyens.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID).FirstOrDefault();
                if (lePhiRecord == null)
                {
                    LePhiXetTuyen lpxt = new LePhiXetTuyen();
                    lpxt.ThiSinh_ID = ts.ThiSinh_ID;
                    lpxt.Lpxt_TrangThai = 0;
                    db_lephixt.LePhiXetTuyens.Add(lpxt);
                    db_lephixt.SaveChanges();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DangKyDuThiNK_Delete(DangKyDuThiNangKhieu dkxt_kqtqg_post)
        {
            DbConnecttion db = new DbConnecttion();
            int dkdt_NK_ID = int.Parse(dkxt_kqtqg_post.Dkdt_NK_ID.ToString());

            DangKyDuThiNangKhieu dkdt_item_getby_id = db.DangKyDuThiNangKhieus.Find(dkdt_NK_ID);

            int nv_current = int.Parse(dkdt_item_getby_id.Dkdt_NK_NguyenVong.ToString());
            int idThisinh = int.Parse(dkdt_item_getby_id.ThiSinh_ID.ToString());

            db.DangKyDuThiNangKhieus.Remove(dkdt_item_getby_id);

            foreach (var item in db.DangKyDuThiNangKhieus.Where(nv => nv.Dkdt_NK_NguyenVong > nv_current && nv.ThiSinh_ID == idThisinh).OrderBy(x => x.Dkdt_NK_NguyenVong))
            {
                DangKyDuThiNangKhieu dangduthi_NK_change = db.DangKyDuThiNangKhieus.FirstOrDefault(x => x.Dkdt_NK_NguyenVong == item.Dkdt_NK_NguyenVong && x.ThiSinh_ID == idThisinh);
                dangduthi_NK_change.Dkdt_NK_NguyenVong = item.Dkdt_NK_NguyenVong - 1;
            }

            db.SaveChanges();
            return Json(new
            {
                status = true,
                msg = "Xoá dữ liệu thành công"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Đăng ký xét tuyển kết quả thi THPT quốc gia
        [ThiSinhSessionCheck]
        public ActionResult dkxtthptqg()
        {
            return View();
        }
        public JsonResult DangKyXetTuyenKQTHPTQG_ListAll()
        {
            int ptxt_check = 2;
            DbConnecttion db_tsdk = new DbConnecttion();
            DbConnecttion db_thptqg = new DbConnecttion();
            if (Session["login_session"] != null)
            {
                string str_login_session = Session["login_session"].ToString();//  "$2a$11$jwPUP78RBpC9R3uW7Dqpau.SXwogLasbvVx3q0vqhoE93Lx044lcu";// 

                var tsdk_Detail = db_tsdk.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault(ts => ts.ThiSinh_MatKhau == str_login_session);

                string _xeploai_hocluc_12 = "";
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 4) { _xeploai_hocluc_12 = "Xuất sắc"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 3) { _xeploai_hocluc_12 = "Giỏi"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 2) { _xeploai_hocluc_12 = "Khá"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 1) { _xeploai_hocluc_12 = "Trung bình"; }

                string _xeploai_hanhkiem_12 = "";
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 4) { _xeploai_hanhkiem_12 = "Tốt"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 3) { _xeploai_hanhkiem_12 = "Khá"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 2) { _xeploai_hanhkiem_12 = "Trung bình"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 1) { _xeploai_hanhkiem_12 = "Yếu"; }

                string _ut_doituong_ten_diem = "ĐT " + tsdk_Detail.DoiTuong.DoiTuong_Ten + ": ƯT " + tsdk_Detail.DoiTuong.DoiTuong_DiemUuTien + " đ";
                string _ut_khuvuv_ten_diem = tsdk_Detail.KhuVuc.KhuVuc_Ten + ": ƯT " + tsdk_Detail.KhuVuc.KhuVuc_DiemUuTien + " đ"; ;


                long _thisinh_id = tsdk_Detail.ThiSinh_ID;

                var list_dkxt_thptqg_ts = db_thptqg.DangKyXetTuyenKQTQGs.Include(d => d.Nganh).Include(d => d.ThiSinhDangKy).Include(d => d.ToHopMon)
                                          .Where(ts => ts.ThiSinh_ID == _thisinh_id && ts.Ptxt_ID == ptxt_check).OrderBy(x => x.Dkxt_KQTQG_NguyenVong).ToList();

                var view_list_dkxt_thptqg_ts = list_dkxt_thptqg_ts.Select(s => new
                {
                    dkxt_KQTQG_ID = s.Dkxt_KQTQG_ID,
                    thiSinh_ID = s.ThiSinh_ID,
                    ptxt_ID = s.Ptxt_ID,
                    nganh_ID = s.Nganh_ID,
                    thm_ID = s.Thm_ID,
                    doiTuong_ID = s.DoiTuong_ID,
                    khuVuc_ID = s.KhuVuc_ID,
                    dkxt_KQTQG_TrangThai = s.Dkxt_KQTQG_TrangThai,
                    dkxt_KQTQG_NguyenVong = s.Dkxt_KQTQG_NguyenVong,
                    dotXT_ID = s.DotXT_ID,
                    dkxt_KQTQG_TenAll = new
                    {
                        khoiNganh_Ten = s.Nganh.KhoiNganh.KhoiNganh_Ten,
                        nganh_GhiChu = s.Nganh.Nganh_GhiChu,
                        thm_MaTen = s.ToHopMon.Thm_MaTen,
                    },
                    dkxt_KQTQG_Diem_M1 = s.Dkxt_KQTQG_Diem_M1,
                    dkxt_KQTQG_Diem_M2 = s.Dkxt_KQTQG_Diem_M2,
                    dkxt_KQTQG_Diem_M3 = s.Dkxt_KQTQG_Diem_M3,
                    dkxt_KQTQG_Diem_Tong = s.Dkxt_KQTQG_Diem_Tong,
                    dkxt_KQTQG_Diem_Tong_Full = s.Dkxt_KQTQG_TongDiem_Full,

                }).ToList();

                var Tsdk_doituong_ten_diem = _ut_doituong_ten_diem;
                var Tsdk_khuvuv_ten_diem = _ut_khuvuv_ten_diem;
                var Tsdk_xeploai_hocluc_12 = _xeploai_hocluc_12;
                var Tsdk_xeploai_hanhkiem_12 = _xeploai_hanhkiem_12;
                return Json(new
                {
                    view_list_dkxt_thptqg_ts,
                    Tsdk_doituong_ten_diem,
                    Tsdk_khuvuv_ten_diem,
                    Tsdk_xeploai_hocluc_12,
                    Tsdk_xeploai_hanhkiem_12

                }, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_Get_Data_MonHoc(DangKyXetTuyenKQTQG dkxt_kqtqd_item)
        {

            int _thm_ID = int.Parse(dkxt_kqtqd_item.Thm_ID.ToString());
            DbConnecttion db_Thm = new DbConnecttion();

            var thm_get_item = db_Thm.ToHopMons.Find(_thm_ID);
            var thmObj = new
            {
                thm_Mon1 = thm_get_item.Thm_Mon1,
                thm_Mon2 = thm_get_item.Thm_Mon2,
                thm_Mon3 = thm_get_item.Thm_Mon3
            };

            return Json(thmObj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_UploadFile_Multi_KQTQG()
        {
            DbConnecttion db = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString(); //"$2a$11$jwPUP78RBpC9R3uW7Dqpau.SXwogLasbvVx3q0vqhoE93Lx044lcu"; //
            var ts = db.ThiSinhDangKies.FirstOrDefault(n => n.ThiSinh_MatKhau.Equals(str_login_session));
            string cccd = ts.ThiSinh_CCCD;

            int so_file_cn_totnghiep = int.Parse(Request["so_file_cn_totnghiep"].ToString());
            int so_file_bang_tn = int.Parse(Request["so_file_bang_tn"].ToString());
            int so_file_hocba_ts = int.Parse(Request["so_file_hocba_ts"].ToString());
            int so_file_giayto_uutien = int.Parse(Request["so_file_giayto_uutien"].ToString());

            HttpFileCollectionBase files = Request.Files;

            string _KQTQG_MinhChung_CNTotNghiep = "";
            string _KQTQG_MinhChung_BangTN = "";
            string _KQTQG_MinhChung_HocBa = "";
            string _KQTQG_MinhChung_UuTien = "";

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname;
                // Checking for Internet Explorer      
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + file.FileName;
                }
                // lấy chuỗi lưu vào csdl
                if (i < so_file_cn_totnghiep)
                {
                    _KQTQG_MinhChung_CNTotNghiep += "#/Uploads/DKXTKQTTHPTQGFile/" + fname;
                }
                if (i >= so_file_cn_totnghiep && i < so_file_bang_tn + so_file_cn_totnghiep)
                {
                    _KQTQG_MinhChung_BangTN += "#/Uploads/DKXTKQTTHPTQGFile/" + fname;
                }

                if (i >= so_file_bang_tn + so_file_cn_totnghiep && i < so_file_hocba_ts + so_file_bang_tn + so_file_cn_totnghiep)
                {
                    _KQTQG_MinhChung_HocBa += "#/Uploads/DKXTKQTTHPTQGFile/" + fname;
                }
                if (i >= so_file_hocba_ts + so_file_bang_tn + so_file_cn_totnghiep && i < so_file_giayto_uutien + so_file_hocba_ts + so_file_bang_tn + so_file_cn_totnghiep)
                {
                    _KQTQG_MinhChung_UuTien += "#/Uploads/DKXTKQTTHPTQGFile/" + fname;
                }

                // Get the complete folder path and store the file inside it.      
                fname = Path.Combine(Server.MapPath("~/Uploads/DKXTHocBaFile/"), fname);
                file.SaveAs(fname);

            }
            return Json(new
            {
                _KQTQG_MinhChung_CNTotNghiep,
                _KQTQG_MinhChung_BangTN,
                _KQTQG_MinhChung_HocBa,
                _KQTQG_MinhChung_UuTien,

            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_KQTHPTQG_Insert(DangKyXetTuyenKQTQG dkxt_kqtqg_post)
        {

            DbConnecttion db_tsdk = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString(); //"$2a$11$jwPUP78RBpC9R3uW7Dqpau.SXwogLasbvVx3q0vqhoE93Lx044lcu";// 

            var ts = db_tsdk.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(str_login_session)).
                Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();

            DbConnecttion db_dxt = new DbConnecttion();
            var dotxettuyen = db_dxt.DotXetTuyens.Where(n => n.Dxt_TrangThai == 1).FirstOrDefault();

            if (ts != null)
            {
                DbConnecttion db_dkxt_kqtqg = new DbConnecttion();

                var nvs = db_dkxt_kqtqg.DangKyXetTuyenKQTQGs.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID).OrderByDescending(x => x.Dkxt_KQTQG_NguyenVong).ToList();

                DangKyXetTuyenKQTQG dkxt_kqtqg_new = new DangKyXetTuyenKQTQG();

                dkxt_kqtqg_new.Nganh_ID = dkxt_kqtqg_post.Nganh_ID;
                dkxt_kqtqg_new.Thm_ID = dkxt_kqtqg_post.Thm_ID;
                dkxt_kqtqg_new.Dkxt_KQTQG_NamTotNghiep = dkxt_kqtqg_post.Dkxt_KQTQG_NamTotNghiep;
                dkxt_kqtqg_new.Dkxt_KQTQG_Diem_M1 = dkxt_kqtqg_post.Dkxt_KQTQG_Diem_M1;
                dkxt_kqtqg_new.Dkxt_KQTQG_Diem_M2 = dkxt_kqtqg_post.Dkxt_KQTQG_Diem_M2;
                dkxt_kqtqg_new.Dkxt_KQTQG_Diem_M3 = dkxt_kqtqg_post.Dkxt_KQTQG_Diem_M3;
                dkxt_kqtqg_new.Dkxt_KQTQG_Diem_Tong = dkxt_kqtqg_post.Dkxt_KQTQG_Diem_Tong;
                dkxt_kqtqg_new.Dkxt_KQTQG_MinhChung_CNTotNghiep = dkxt_kqtqg_post.Dkxt_KQTQG_MinhChung_CNTotNghiep;
                dkxt_kqtqg_new.Dkxt_KQTQG_MinhChung_BangTN = dkxt_kqtqg_post.Dkxt_KQTQG_MinhChung_BangTN;
                dkxt_kqtqg_new.Dkxt_KQTQG_MinhChung_HocBa = dkxt_kqtqg_post.Dkxt_KQTQG_MinhChung_HocBa;
                dkxt_kqtqg_new.Dkxt_KQTQG_MinhChung_UuTien = dkxt_kqtqg_post.Dkxt_KQTQG_MinhChung_UuTien;
                dkxt_kqtqg_new.Dkxt_KQTQG_NgayDangKy = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                dkxt_kqtqg_new.Dkxt_KQTQG_TongDiem_Full = (double.Parse(dkxt_kqtqg_new.Dkxt_KQTQG_Diem_Tong.ToString()) +
                                                           double.Parse(ts.DoiTuong.DoiTuong_DiemUuTien.ToString()) +
                                                           double.Parse(ts.KhuVuc.KhuVuc_DiemUuTien.ToString())).ToString();

                dkxt_kqtqg_new.ThiSinh_ID = ts.ThiSinh_ID;
                dkxt_kqtqg_new.DoiTuong_ID = ts.DoiTuong_ID;
                dkxt_kqtqg_new.KhuVuc_ID = ts.KhuVuc_ID;
                dkxt_kqtqg_new.Dkxt_KQTQG_XepLoaiHocLuc_12 = ts.ThiSinh_HocLucLop12;
                dkxt_kqtqg_new.Dkxt_KQTQG_XepLoaiHanhKiem_12 = ts.ThiSinh_HanhKiemLop12;
                dkxt_kqtqg_new.Ptxt_ID = 2;
                dkxt_kqtqg_new.DotXT_ID = dotxettuyen.Dxt_ID;
                dkxt_kqtqg_new.Dkxt_KQTQG_NguyenVong = nvs.Count + 1;
                dkxt_kqtqg_new.Dkxt_KQTQG_TrangThai = 0;
                dkxt_kqtqg_new.Dkxt_KQTQG_GhiChu = ""; ;
                dkxt_kqtqg_new.Dkxt_KQTQG_TrangThai_KetQua = 0;


                db_dkxt_kqtqg.DangKyXetTuyenKQTQGs.Add(dkxt_kqtqg_new);
                db_dkxt_kqtqg.SaveChanges();
                // add LePhiXetTuyen
                DbConnecttion db_lephixt = new DbConnecttion();
                var lePhiRecord = db_lephixt.LePhiXetTuyens.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID).FirstOrDefault();
                if (lePhiRecord == null)
                {
                    LePhiXetTuyen lpxt = new LePhiXetTuyen();
                    lpxt.ThiSinh_ID = ts.ThiSinh_ID;
                    lpxt.Lpxt_TrangThai = 0;
                    db_lephixt.LePhiXetTuyens.Add(lpxt);
                    db_lephixt.SaveChanges();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DangKyXetTuyenKQTTHPTQG_Delete(DangKyXetTuyenKQTQG dkxt_kqtqg_post)
        {
            DbConnecttion db = new DbConnecttion();
            int Dkxt_KQTQG_ID = int.Parse(dkxt_kqtqg_post.Dkxt_KQTQG_ID.ToString());

            DangKyXetTuyenKQTQG dkxt_item_getby_id = db.DangKyXetTuyenKQTQGs.Find(Dkxt_KQTQG_ID);

            int nv_current = int.Parse(dkxt_item_getby_id.Dkxt_KQTQG_NguyenVong.ToString());
            int idThisinh = int.Parse(dkxt_item_getby_id.ThiSinh_ID.ToString());

            db.DangKyXetTuyenKQTQGs.Remove(dkxt_item_getby_id);

            foreach (var item in db.DangKyXetTuyenKQTQGs.Where(nv => nv.Dkxt_KQTQG_NguyenVong > nv_current && nv.ThiSinh_ID == idThisinh).OrderBy(x => x.Dkxt_KQTQG_NguyenVong))
            {
                DangKyXetTuyenKQTQG dangKyXetTuyen_change = db.DangKyXetTuyenKQTQGs.FirstOrDefault(x => x.Dkxt_KQTQG_NguyenVong == item.Dkxt_KQTQG_NguyenVong && x.ThiSinh_ID == idThisinh);
                dangKyXetTuyen_change.Dkxt_KQTQG_NguyenVong = item.Dkxt_KQTQG_NguyenVong - 1;
            }

            db.SaveChanges();
            return Json(new
            {
                status = true,
                msg = "Xoá dữ liệu thành công"
            }, JsonRequestBehavior.AllowGet);
        }

        #region hàm thay đổi nguyện vọng
        public JsonResult DangKyXetTuyenKQTTHPTQG_Change_UpData(DangKyXetTuyenKQTQG dkxt_kqtqg_change)
        {
            DbConnecttion db = new DbConnecttion();
            //  System.Diagnostics.Debug.WriteLine(dkxt_kqtqg_change.Dkxt_KQTQG_ID.ToString());
            long _dkxt_KQTQG_ID = long.Parse(dkxt_kqtqg_change.Dkxt_KQTQG_ID.ToString());

            DangKyXetTuyenKQTQG dkxt_item_getby_id = db.DangKyXetTuyenKQTQGs.Find(_dkxt_KQTQG_ID);

            int nv_current = int.Parse(dkxt_item_getby_id.Dkxt_KQTQG_NguyenVong.ToString());
            int idThisinh = int.Parse(dkxt_item_getby_id.ThiSinh_ID.ToString());

            dkxt_item_getby_id.Dkxt_KQTQG_NguyenVong = nv_current - 1;

            DangKyXetTuyenKQTQG dkxt_item_get_change = db.DangKyXetTuyenKQTQGs.FirstOrDefault(i => i.Dkxt_KQTQG_NguyenVong == nv_current - 1 && i.ThiSinh_ID == idThisinh);
            if (dkxt_item_get_change != null)
            {
                dkxt_item_get_change.Dkxt_KQTQG_NguyenVong = nv_current;
                db.SaveChanges();
                return Json(new { status = true, msg = "Thay đổi dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, msg = "Có lỗi xảy ra." }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DangKyXetTuyenKQTTHPTQG_Change_DownData(DangKyXetTuyenKQTQG dkxt_kqtqg_change)
        {
            DbConnecttion db = new DbConnecttion();
            long _dkxt_KQTQG_ID = long.Parse(dkxt_kqtqg_change.Dkxt_KQTQG_ID.ToString());

            DangKyXetTuyenKQTQG dkxt_item_getby_id = db.DangKyXetTuyenKQTQGs.Find(_dkxt_KQTQG_ID);

            int nv_current = int.Parse(dkxt_item_getby_id.Dkxt_KQTQG_NguyenVong.ToString());
            int idThisinh = int.Parse(dkxt_item_getby_id.ThiSinh_ID.ToString());

            dkxt_item_getby_id.Dkxt_KQTQG_NguyenVong = nv_current + 1;

            DangKyXetTuyenKQTQG dkxt_item_get_change = db.DangKyXetTuyenKQTQGs.FirstOrDefault(i => i.Dkxt_KQTQG_NguyenVong == nv_current + 1 && i.ThiSinh_ID == idThisinh);
            if (dkxt_item_get_change != null)
            {
                dkxt_item_get_change.Dkxt_KQTQG_NguyenVong = nv_current;
                db.SaveChanges();
                return Json(new { status = true, msg = "Thay đổi dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, msg = "Có lỗi xảy ra." }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult downData1(string idDkxt)
        {
            DbConnecttion db = new DbConnecttion();
            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                System.Diagnostics.Debug.WriteLine(id);
                DangKyXetTuyen dangKyXetTuyen_current = db.DangKyXetTuyens.Find(id);
                int nv_current = (int)dangKyXetTuyen_current.Dkxt_NguyenVong;
                int idThisinh = (int)dangKyXetTuyen_current.ThiSinh_ID;
                dangKyXetTuyen_current.Dkxt_NguyenVong = nv_current + 1;
                DangKyXetTuyen dangKyXetTuyen_down = db.DangKyXetTuyens.FirstOrDefault(i => i.Dkxt_NguyenVong == nv_current + 1 && i.ThiSinh_ID == idThisinh);
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

        #endregion
        #endregion
        #region Đăng ký xét tuyển học bạ
        [ThiSinhSessionCheck]
        public ActionResult dkxthocba2()
        {
            return View();
        }
        [ThiSinhSessionCheck]
        public ActionResult dkxthocba()
        {
            return View();
        }
        public JsonResult DangKyXetTuyen_ListAll()
        {

            int ptxt_check = 3;
            DbConnecttion db_tsdk = new DbConnecttion();
            DbConnecttion db_dkxt = new DbConnecttion();
            if (Session["login_session"] != null)
            {

                string str_login_session = Session["login_session"].ToString();
                //ViewBag.str_login_session = str_login_session;
                var tsdk_Detail = db_tsdk.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault(ts => ts.ThiSinh_MatKhau.Equals(str_login_session));


                string _xeploai_hocluc_12 = "";

                if (tsdk_Detail.ThiSinh_HocLucLop12 == 4) { _xeploai_hocluc_12 = "Xuất sắc"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 3) { _xeploai_hocluc_12 = "Giỏi"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 2) { _xeploai_hocluc_12 = "Khá"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 1) { _xeploai_hocluc_12 = "Trung bình"; }

                string _xeploai_hanhkiem_12 = "";
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 4) { _xeploai_hanhkiem_12 = "Tốt"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 3) { _xeploai_hanhkiem_12 = "Khá"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 2) { _xeploai_hanhkiem_12 = "Trung bình"; }
                if (tsdk_Detail.ThiSinh_HocLucLop12 == 1) { _xeploai_hanhkiem_12 = "Yếu"; }

                string _ut_doituong_ten_diem = "ĐT " + tsdk_Detail.DoiTuong.DoiTuong_Ten + ": ƯT " + tsdk_Detail.DoiTuong.DoiTuong_DiemUuTien + " đ";
                string _ut_khuvuv_ten_diem = tsdk_Detail.KhuVuc.KhuVuc_Ten + ": ƯT " + tsdk_Detail.KhuVuc.KhuVuc_DiemUuTien + " đ"; ;


                long _thisinh_id = tsdk_Detail.ThiSinh_ID;

                var dkxt_Detail_list = db_dkxt.DangKyXetTuyens.
                                       Include(d => d.DoiTuong).
                                       Include(d => d.DotXetTuyen).
                                       Include(d => d.KhuVuc).
                                       Include(d => d.Nganh).
                                       Include(d => d.PhuongThucXetTuyen).
                                       Include(d => d.ThiSinhDangKy).
                                       Include(d => d.ToHopMon).Where(ts => ts.ThiSinh_ID == _thisinh_id && ts.Ptxt_ID == ptxt_check).OrderBy(x => x.Dkxt_NguyenVong).ToList();
                var select_list_dkxt_model = dkxt_Detail_list.Select(s => new
                {
                    Dkxt_ID = s.Dkxt_ID,
                    ThiSinh_ID = s.ThiSinh_ID,
                    Ptxt_ID = s.Ptxt_ID,
                    Nganh_ID = s.Nganh_ID,
                    Thm_ID = s.Thm_ID,
                    DoiTuong_ID = s.DoiTuong_ID,
                    KhuVuc_ID = s.KhuVuc_ID,
                    Dkxt_TrangThai = s.Dkxt_TrangThai,
                    Dkxt_NguyenVong = s.Dkxt_NguyenVong,
                    DotXT_ID = s.DotXT_ID,
                    Dkxt_TenAll = new
                    {
                        KhoiNganh_Ten = s.Nganh.KhoiNganh.KhoiNganh_Ten,
                        Nganh_GhiChu = s.Nganh.Nganh_GhiChu,
                        Thm_MaTen = s.ToHopMon.Thm_MaTen,
                    },
                    Dkxt_Diem_M1 = s.Dkxt_Diem_M1,
                    Dkxt_Diem_M2 = s.Dkxt_Diem_M2,
                    Dkxt_Diem_M3 = s.Dkxt_Diem_M3,
                    Dkxt_Diem_Tong = s.Dkxt_Diem_Tong,
                    Dkxt_Diem_Tong_Full = s.Dkxt_Diem_Tong_Full,

                }).ToList();

                var Dkxt_doituong_ten_diem = _ut_doituong_ten_diem;
                var Dkxt_khuvuv_ten_diem = _ut_khuvuv_ten_diem;
                var Dkxt_xeploai_hocluc_12 = _xeploai_hocluc_12;
                var Dkxt_xeploai_hanhkiem_12 = _xeploai_hanhkiem_12;
                return Json(new
                {
                    select_list_dkxt_model,
                    Dkxt_doituong_ten_diem,
                    Dkxt_khuvuv_ten_diem,
                    Dkxt_xeploai_hocluc_12,
                    Dkxt_xeploai_hanhkiem_12

                }, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_Get_Data_Create(str_infor data)
        {
            DbConnecttion thm_db = new DbConnecttion();
            DbConnecttion dkxt_db = new DbConnecttion();
            DbConnecttion tsdk_db = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString();
            var get_tsdk_byid = tsdk_db.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault(ts => ts.ThiSinh_MatKhau.Equals(str_login_session));
            // lấy thông tin học lực


            if (data.Dkxt_ID_UpDate_post < 0)//  đăng ký mới
            {
                string _ten_Mon1 = "Chưa chọn";
                string _ten_Mon2 = "Chưa chọn";
                string _ten_Mon3 = "Chưa chọn";
                if (data.ToHopMon_ID_Select_Post > 0)
                {
                    var get_tohopmon_byid = thm_db.ToHopMons.FirstOrDefault(x => x.Thm_ID == data.ToHopMon_ID_Select_Post);
                    _ten_Mon1 = get_tohopmon_byid.Thm_Mon1;
                    _ten_Mon2 = get_tohopmon_byid.Thm_Mon2;
                    _ten_Mon3 = get_tohopmon_byid.Thm_Mon3;
                }

                var information_to_client = new
                {
                    dkxt_id_update = data.int_input_b,
                    thm_Mon1 = new { TenMon1 = _ten_Mon1, HK1 = "", HK2 = "", HK3 = "", DiemTrungBinh = "" },
                    thm_Mon2 = new { TenMon2 = _ten_Mon2, HK1 = "", HK2 = "", HK3 = "", DiemTrungBinh = "" },
                    thm_Mon3 = new { TenMon3 = _ten_Mon3, HK1 = "", HK2 = "", HK3 = "", DiemTrungBinh = "" },

                    ttKN_N_THM = new { _KhoiNganh_ID = data.KhoiNganh_ID_Select_Post, _Nganh_ID = data.Nganh_ID_Select_Post, _Thm_ID = data.ToHopMon_ID_Select_Post }

                };
                return Json(information_to_client, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_Get_KNTH(str_infor data)
        {
            DbConnecttion db_dkxt = new DbConnecttion();
            int id = int.Parse(data.Dkxt_ID_UpDate_post.ToString());
            var dkxt_infor_Detail = db_dkxt.DangKyXetTuyens.Find(id); ;

            int _nganh_id = dkxt_infor_Detail.Nganh.Nganh_ID;
            int _tohopmon_id = dkxt_infor_Detail.ToHopMon.Thm_ID;

            DbConnecttion db_kn = new DbConnecttion();
            var nganh_detail = db_kn.Nganhs.Include(n => n.KhoiNganh).FirstOrDefault(n => n.Nganh_ID == _nganh_id);
            var _khoinganh_id = nganh_detail.KhoiNganh.KhoiNganh_ID;

            var select_khoi_nganh_tohop = new
            {
                _khoinganhid = _khoinganh_id,
                _nganhid = _nganh_id,
                _tohopmonid = _tohopmon_id,

            };
            return Json(select_khoi_nganh_tohop, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_Get_Data_Edit(str_infor data)
        {
            DbConnecttion thm_db = new DbConnecttion();
            DbConnecttion dkxt_db = new DbConnecttion();
            DbConnecttion tsdk_db = new DbConnecttion();
            DbConnecttion kn_db = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString();
            var get_tsdk_byid = tsdk_db.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault(ts => ts.ThiSinh_MatKhau.Equals(str_login_session));
            // lấy thông tin học lực
            string _xeploai_hocluc_12 = "";

            if (get_tsdk_byid.ThiSinh_HocLucLop12 == 4) { _xeploai_hocluc_12 = "Xuất sắc"; }
            if (get_tsdk_byid.ThiSinh_HocLucLop12 == 3) { _xeploai_hocluc_12 = "Giỏi"; }
            if (get_tsdk_byid.ThiSinh_HocLucLop12 == 2) { _xeploai_hocluc_12 = "Khá"; }
            if (get_tsdk_byid.ThiSinh_HocLucLop12 == 1) { _xeploai_hocluc_12 = "Trung bình"; }

            string _xeploai_hanhkiem_12 = "";
            if (get_tsdk_byid.ThiSinh_HocLucLop12 == 4) { _xeploai_hanhkiem_12 = "Tốt"; }
            if (get_tsdk_byid.ThiSinh_HocLucLop12 == 3) { _xeploai_hanhkiem_12 = "Khá"; }
            if (get_tsdk_byid.ThiSinh_HocLucLop12 == 2) { _xeploai_hanhkiem_12 = "Trung bình"; }
            if (get_tsdk_byid.ThiSinh_HocLucLop12 == 1) { _xeploai_hanhkiem_12 = "Yếu"; }

            string _ut_doituong_ten_diem = "ĐT " + get_tsdk_byid.DoiTuong.DoiTuong_Ten + ": " + get_tsdk_byid.DoiTuong.DoiTuong_DiemUuTien + " đ";
            string _ut_khuvuv_ten_diem = get_tsdk_byid.KhuVuc.KhuVuc_Ten + ": " + get_tsdk_byid.KhuVuc.KhuVuc_DiemUuTien + " đ"; ;

            if (data.Dkxt_ID_UpDate_post > 0)// sửa đăng ký
            {
                var dkxt_detail_getby_id = dkxt_db.DangKyXetTuyens.Include(d => d.Nganh).Include(d => d.DoiTuong).Include(d => d.KhuVuc).FirstOrDefault(x => x.Dkxt_ID == data.Dkxt_ID_UpDate_post);

                var information_to_client = new
                {
                    Dkxt_ID_UpDate_post = data.Dkxt_ID_UpDate_post,
                    thm_Mon1 = JsonConvert.DeserializeObject<DiemMon1>(dkxt_detail_getby_id.Dkxt_Diem_M1),
                    thm_Mon2 = JsonConvert.DeserializeObject<DiemMon2>(dkxt_detail_getby_id.Dkxt_Diem_M2),
                    thm_Mon3 = JsonConvert.DeserializeObject<DiemMon3>(dkxt_detail_getby_id.Dkxt_Diem_M3),

                    ttBosung_Ut = new
                    {
                        ut_doituong_ten_diem = _ut_doituong_ten_diem,
                        ut_khuvuv_ten_diem = _ut_khuvuv_ten_diem,
                        xeploai_hocluc_12 = _xeploai_hocluc_12,
                        xeploai_hanhkiem_12 = _xeploai_hanhkiem_12,
                    },
                    ttKN_N_THM = new { _KhoiNganh_ID = dkxt_detail_getby_id.Nganh.KhoiNganh_ID, _Nganh_ID = dkxt_detail_getby_id.Nganh_ID, _Thm_ID = dkxt_detail_getby_id.Thm_ID }
                };
                return Json(information_to_client, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_UploadFile_Multi()
        {
            DbConnecttion db = new DbConnecttion();

            var session = Session["login_session"].ToString();
            var ts = db.ThiSinhDangKies.FirstOrDefault(n => n.ThiSinh_MatKhau.Equals(session));
            string cccd = ts.ThiSinh_CCCD;

            int so_file_hb = int.Parse(Request["so_file_hb"].ToString());
            int so_file_cccd = int.Parse(Request["so_file_cccd"].ToString());
            int so_file_btn = int.Parse(Request["so_file_btn"].ToString());
            int so_file_gtut = int.Parse(Request["so_file_gtut"].ToString());


            HttpFileCollectionBase files = Request.Files;

            string MinhChung_HB = "";
            string MinhChung_CCCD = "";
            string MinhChung_Bang = "";
            string MinhChung_UuTien = "";

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname;
                // Checking for Internet Explorer      
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = cccd + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + file.FileName;
                }
                // lấy chuỗi lưu vào csdl
                if (i < so_file_hb)
                {
                    MinhChung_HB += "#/Uploads/DKXTHocBaFile/" + fname;
                }
                if (i >= so_file_hb && i < so_file_hb + so_file_cccd)
                {
                    MinhChung_CCCD += "#/Uploads/DKXTHocBaFile/" + fname;
                }

                if (i >= so_file_hb + so_file_cccd && i < so_file_btn + so_file_hb + so_file_cccd)
                {
                    MinhChung_Bang += "#/Uploads/DKXTHocBaFile/" + fname;
                }
                if (i >= so_file_btn + so_file_hb + so_file_cccd && i < so_file_gtut + so_file_btn + so_file_hb + so_file_cccd)
                {
                    MinhChung_UuTien += "#/Uploads/DKXTHocBaFile/" + fname;
                }

                // Get the complete folder path and store the file inside it.      
                fname = Path.Combine(Server.MapPath("~/Uploads/DKXTHocBaFile/"), fname);
                file.SaveAs(fname);

            }
            return Json(new
            {
                MinhChung_HB,
                MinhChung_CCCD,
                MinhChung_Bang,
                MinhChung_UuTien,

            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_Insert(ThongTinNguyenVong nguyenvong)
        {
            DbConnecttion db = new DbConnecttion();
            DbConnecttion db_dkxt = new DbConnecttion();
            var session = Session["login_session"].ToString();
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).
                Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();

            var dotxettuyen = db.DotXetTuyens.Where(n => n.Dxt_TrangThai == 1).FirstOrDefault();
            if (ts != null)
            {
                var nvs = db.DangKyXetTuyens.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID).OrderByDescending(x => x.Dkxt_NguyenVong).ToList();

                DangKyXetTuyen dkxt = new DangKyXetTuyen();

                dkxt.Nganh_ID = int.Parse(nguyenvong.Nganh_ID.ToString());
                dkxt.Thm_ID = int.Parse(nguyenvong.Thm_ID.ToString());
                var diemTong = double.Parse(nguyenvong.Dkxt_Diem_Tong.ToString());

                dkxt.Dkxt_Diem_M1 = nguyenvong.Dkxt_Diem_M1;
                dkxt.Dkxt_Diem_M2 = nguyenvong.Dkxt_Diem_M2;
                dkxt.Dkxt_Diem_M3 = nguyenvong.Dkxt_Diem_M3;
                dkxt.Dkxt_Diem_Tong = diemTong.ToString();

                dkxt.Dkxt_MinhChung_HB = nguyenvong.Dkxt_MinhChung_HB;
                dkxt.Dkxt_MinhChung_CCCD = nguyenvong.Dkxt_MinhChung_CCCD;
                dkxt.Dkxt_MinhChung_Bang = nguyenvong.Dkxt_MinhChung_HB;
                dkxt.Dkxt_MinhChung_UuTien = nguyenvong.Dkxt_MinhChung_UuTien;

                dkxt.ThiSinh_ID = ts.ThiSinh_ID;
                dkxt.DoiTuong_ID = ts.DoiTuong_ID;
                dkxt.KhuVuc_ID = ts.KhuVuc_ID;
                dkxt.DotXT_ID = dotxettuyen.Dxt_ID;
                dkxt.Dkxt_TrangThai = 0;
                dkxt.Dkxt_TrangThai_KetQua = 0;

                dkxt.Dkxt_XepLoaiHocLuc_12 = ts.ThiSinh_HocLucLop12;
                dkxt.Dkxt_XepLoaiHanhKiem_12 = ts.ThiSinh_HanhKiemLop12;

                var diemDoiTuong = ts.DoiTuong.DoiTuong_DiemUuTien;
                var khuVucDoiTuong = ts.KhuVuc.KhuVuc_DiemUuTien;
                var diemTongFull = diemTong + diemDoiTuong + khuVucDoiTuong;

                dkxt.Dkxt_Diem_Tong_Full = diemTongFull.ToString();
                dkxt.Ptxt_ID = 3;

                dkxt.Dkxt_NguyenVong = nvs.Count + 1;

                db.DangKyXetTuyens.Add(dkxt);
                // add LePhiXetTuyen
                var lePhiRecord = db.LePhiXetTuyens.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID).FirstOrDefault();
                if (lePhiRecord == null)
                {
                    LePhiXetTuyen lpxt = new LePhiXetTuyen();
                    lpxt.ThiSinh_ID = ts.ThiSinh_ID;
                    lpxt.Lpxt_TrangThai = 0;
                    db.LePhiXetTuyens.Add(lpxt);
                }
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_Delete(str_infor data)
        {
            DbConnecttion db = new DbConnecttion();

            if (!string.IsNullOrWhiteSpace(data.Dkxt_ID_UpDate_post.ToString()))
            {
                int id = int.Parse(data.Dkxt_ID_UpDate_post.ToString());

                DangKyXetTuyen dangKyXetTuyen = db.DangKyXetTuyens.Find(id);
                int nv_current = (int)dangKyXetTuyen.Dkxt_NguyenVong;
                int idThisinh = (int)dangKyXetTuyen.ThiSinh_ID;

                db.DangKyXetTuyens.Remove(dangKyXetTuyen);
                foreach (var item in db.DangKyXetTuyens.Where(nv => nv.Dkxt_NguyenVong > nv_current))
                {
                    DangKyXetTuyen dangKyXetTuyen_change = db.DangKyXetTuyens.FirstOrDefault(i => i.Dkxt_NguyenVong == item.Dkxt_NguyenVong && i.ThiSinh_ID == idThisinh);
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
                msg = "Chưa thể xóa theo yêu cầu."
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region lấy dữ liệu ra dropdown list
        public JsonResult KhoiNganhListAll()
        {
            DbConnecttion khoinganh_db = new DbConnecttion();
            var selectResult_KhoiNganh = khoinganh_db.KhoiNganhs.Select(s => new
            {
                khoiNganh_ID = s.KhoiNganh_ID,
                khoiNganh_Ten = s.KhoiNganh_Ten
            });
            return Json(selectResult_KhoiNganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult NganhListAll(int id)
        {
            DbConnecttion nganh_db = new DbConnecttion();

            var selectResult_Nganh = nganh_db.Nganhs.Where(x => x.KhoiNganh_ID == id || x.Nganh_ID == 0).OrderBy(x => x.Nganh_ID).Select(s => new
            {
                nganh_ID = s.Nganh_ID,
                nganh_GhiChu = s.NganhTenNganh,
                khoiNganh_ID = s.KhoiNganh_ID
            });
            return Json(selectResult_Nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ToHopMonNganhListAll(int id)
        {
            DbConnecttion thm_nganh_db = new DbConnecttion();
            var selectResult_tohopmon_nganh = thm_nganh_db.ToHopMonNganhs.Include(n => n.ToHopMon).Where(x => x.Nganh_ID == id || x.ToHopMon.Thm_ID == 0).OrderBy(x => x.ToHopMon.Thm_ID).Select(s => new
            {
                thm_ID = s.Thm_ID,
                thm_MaTen = s.ToHopMon.Thm_MaTen,
                thm_TenToHop = s.ToHopMon.Thm_TenToHop
            });
            return Json(selectResult_tohopmon_nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region hàm thay đổi nguyện vọng
        [HttpPost, ActionName("upData")]
        public JsonResult upData(string idDkxt)
        {
            DbConnecttion db = new DbConnecttion();
            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                System.Diagnostics.Debug.WriteLine(id);
                DangKyXetTuyen dangKyXetTuyen_current = db.DangKyXetTuyens.Find(id);
                int nv_current = (int)dangKyXetTuyen_current.Dkxt_NguyenVong;
                int idThisinh = (int)dangKyXetTuyen_current.ThiSinh_ID;
                dangKyXetTuyen_current.Dkxt_NguyenVong = nv_current - 1;
                DangKyXetTuyen dangKyXetTuyen_up = db.DangKyXetTuyens.FirstOrDefault(i => i.Dkxt_NguyenVong == nv_current - 1 && i.ThiSinh_ID == idThisinh);
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
        [HttpPost, ActionName("downData")]
        public JsonResult downData(string idDkxt)
        {
            DbConnecttion db = new DbConnecttion();
            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                System.Diagnostics.Debug.WriteLine(id);
                DangKyXetTuyen dangKyXetTuyen_current = db.DangKyXetTuyens.Find(id);
                int nv_current = (int)dangKyXetTuyen_current.Dkxt_NguyenVong;
                int idThisinh = (int)dangKyXetTuyen_current.ThiSinh_ID;
                dangKyXetTuyen_current.Dkxt_NguyenVong = nv_current + 1;
                DangKyXetTuyen dangKyXetTuyen_down = db.DangKyXetTuyens.FirstOrDefault(i => i.Dkxt_NguyenVong == nv_current + 1 && i.ThiSinh_ID == idThisinh);
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
        #endregion

        #region test login
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registers()
        {
            return View();
        }
        #endregion
    }

    #region Khai báo thêm đối tượng để xử lý
    public class NguyenVong
    {
        public int Nganh_ID { get; set; }
        public int Thm_ID { get; set; }
        public string Dkxt_Diem_M1 { get; set; }
        public string Dkxt_Diem_M2 { get; set; }
        public string Dkxt_Diem_M3 { get; set; }
        public string Dkxt_Diem_Tong { get; set; }
        public string Dkxt_Diem_Tong_Full { get; set; }
    }
    public class str_infor
    {
        public int Dkxt_ID_UpDate_post { get; set; }
        public int ToHopMon_ID_Select_Post { get; set; }
        public int Nganh_ID_Select_Post { get; set; }
        public int KhoiNganh_ID_Select_Post { get; set; }
        public int int_input_b { get; set; }
    }
    public class ThongTinNguyenVong
    {
        public int Nganh_ID { get; set; }
        public int Thm_ID { get; set; }
        public string Dkxt_XepLoaiHocLuc_12 { get; set; }
        public string Dkxt_XepLoaiHanhKiem_12 { get; set; }
        public string Dkxt_Diem_M1 { get; set; }
        public string Dkxt_Diem_M2 { get; set; }
        public string Dkxt_Diem_M3 { get; set; }
        public double Dkxt_Diem_Tong { get; set; }
        public string Dkxt_MinhChung_HB { get; set; }
        public string Dkxt_MinhChung_CCCD { get; set; }
        public string Dkxt_MinhChung_Bang { get; set; }
        public string Dkxt_MinhChung_UuTien { get; set; }
    }
    #endregion 
}