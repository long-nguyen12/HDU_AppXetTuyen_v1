using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using HDU_AppXetTuyen.Models;
using Newtonsoft.Json;
using PagedList;



namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class DangKyXetTuyensController : Controller
    {
        private DbConnecttion db = null;
        public ActionResult Dkxtth()
        {
            return View();
        }

        #region hiển thị và kiểm tra thông tin thí sinh đăng ký xét tuyển sử dụng kết quả thi tn thpt qg
        [AdminSessionCheck]
        public ActionResult DkxtKQTthpt(string filteriNvong, string filteriNganh, string filteriLePhi,
            string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            db = new DbConnecttion();
            var dot_hientai = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var model = (from ts in db.DangKyXetTuyenKQTQGs.Where(x =>x.DotXT_ID == dot_hientai.Dxt_ID) select ts)
                                            .OrderBy(x => x.ThiSinh_ID)
                                            .ThenBy(x => x.Dkxt_KQTQG_NguyenVong)
                                            .ThenBy(x => x.Nganh.Nganh_TenNganh)
                                            .Include(x => x.ThiSinhDangKy)
                                            .Include(x => x.Nganh)
                                            .Include(x => x.ToHopMon)
                                            .Include(x => x.DotXetTuyen)
                                            .Include(x => x.ThiSinhDangKy.DoiTuong)
                                            .Include(x => x.ThiSinhDangKy.KhuVuc)
                                            .Include(x => x.PhuongThucXetTuyen);

            ViewBag.filteriNam = db.NamHocs.Where(x => x.NamHoc_TrangThai == 1).FirstOrDefault().NamHoc_Ten;

            #region lọc dữ liệu theo nguyện vọng
            var list_items_nv = (from item in model select (new { value_item = item.Dkxt_KQTQG_NguyenVong.ToString(), text_item = "NV " + item.Dkxt_KQTQG_NguyenVong.ToString() })).Distinct().ToList();
            ViewBag.filteriNvong = new SelectList(list_items_nv.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            // thực hiện lọc theo nguyện vọng
            if (!String.IsNullOrEmpty(filteriNvong))
            {
                int _dkxt_KQTQG_NguyenVong = Int32.Parse(filteriNvong);
                model = model.Where(x => x.Dkxt_KQTQG_NguyenVong == _dkxt_KQTQG_NguyenVong);
            }
            #endregion

            #region lọc dữ liệu theo ngành
            var list_items_nganh = (from item in model select (new { value_item = item.Nganh.Nganh_ID, text_item = item.Nganh.Nganh_TenNganh })).Distinct().ToList();
            ViewBag.filteriNganh = new SelectList(list_items_nganh.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            if (!String.IsNullOrEmpty(filteriNganh))
            {
                int _dkxt_KQTQG_Nganh_ID = Int32.Parse(filteriNganh);
                model = model.Where(x => x.Nganh_ID == _dkxt_KQTQG_Nganh_ID);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in model select item.Dkxt_KQTQG_TrangThai_KinhPhi).Distinct().ToList();
            List<StatusTracking> filteri_items_lephi = new List<StatusTracking>();

            foreach (var _item in list_items_lephi)
            {
                if (_item == 0)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa nộp minh chứng lệ phí" });
                }
                if (_item == 1)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã nộp, chưa kiểm tra" });
                }
                if (_item == 2)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã kiểm tra, Có sai sót" });
                }
                if (_item == 3)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 3, St_Name = "Đã sửa, chưa kiểm tra" });
                }
                if (_item == 9)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm tra, đúng" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int trangThai_LePhi = Int32.Parse(filteriLePhi);
                model = model.Where(x => x.Dkxt_KQTQG_TrangThai_KinhPhi == trangThai_LePhi);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in model select item.Dkxt_KQTQG_TrangThai_HoSo).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();
            //filteri_items_hoso.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa kiểm tra" });
            //filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã kiểm tra, Có sai sót" });
            //filteri_items_hoso.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã sửa, chưa kiểm tra" });
            //filteri_items_hoso.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm tra, hồ sơ đúng, đủ" });
            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa kiểm tra" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã kiểm tra, Có sai sót" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã sửa, chưa kiểm tra" });
                }
                if (_item == 9)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm tra, hồ sơ đúng, đủ" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _dkxt_KQTQG_TrangThai_HoSo= Int32.Parse(filteriHoSo);
                model = model.Where(x => x.Dkxt_KQTQG_TrangThai_HoSo == _dkxt_KQTQG_TrangThai_HoSo);
            }
            #endregion

            // thưc hiện tìm kiếm: theo họ, tên, cccd, điện thoại, email
            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.ThiSinhDangKy.ThiSinh_Ten.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_HoLot.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_CCCD.Contains(searchString)
                                      || m.ThiSinhDangKy.ThiSinh_DienThoai.Contains(searchString)
                                      || m.ThiSinhDangKy.ThiSinh_Email.Contains(searchString)
                                      || m.Nganh.Nganh_GhiChu.Contains(searchString)
                                      || m.ToHopMon.Thm_MaTen.Contains(searchString)
                                      || m.Dkxt_KQTQG_TongDiem_Full.Contains(searchString));
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

            ViewBag.NvongFilteri = filteriNvong;
            ViewBag.NganhFilteri = filteriNganh;
            ViewBag.LePhiFilteri = filteriLePhi;
            ViewBag.HoSoFilteri = filteriHoSo;
            ViewBag.SearchString = searchString;
            ViewBag.pageCurren = page;

            ViewBag.totalRecod = model.Count();

            #endregion
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DkxtKQTthpt_hs_view(long? Dkxt_ID, string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;
            return View();
        }

        public JsonResult DkxtKQTthpt_hs_Detail(DangKyXetTuyenKQTQG entity)
        {
            db = new DbConnecttion();
            long dkxt_KQTQG_ID = entity.Dkxt_KQTQG_ID;

            DangKyXetTuyenKQTQG model_diem = db.DangKyXetTuyenKQTQGs.Include(x => x.ThiSinhDangKy).Where(x => x.Dkxt_KQTQG_ID == dkxt_KQTQG_ID).FirstOrDefault();
            string _xeploai_hocluc_12 = "";
            if (model_diem.ThiSinhDangKy.ThiSinh_HocLucLop12 == 4) { _xeploai_hocluc_12 = "Học lực 12: Xuất sắc"; }
            if (model_diem.ThiSinhDangKy.ThiSinh_HocLucLop12 == 3) { _xeploai_hocluc_12 = "Học lực 12: Giỏi"; }
            if (model_diem.ThiSinhDangKy.ThiSinh_HocLucLop12 == 2) { _xeploai_hocluc_12 = "Học lực 12: Khá"; }
            if (model_diem.ThiSinhDangKy.ThiSinh_HocLucLop12 == 1) { _xeploai_hocluc_12 = "Học lực 12: Trung bình"; }

            string _xeploai_hanhkiem_12 = "";
            if (model_diem.ThiSinhDangKy.ThiSinh_HanhKiemLop12 == 4) { _xeploai_hanhkiem_12 = "Hạnh kiểm 12: Tốt"; }
            if (model_diem.ThiSinhDangKy.ThiSinh_HanhKiemLop12 == 3) { _xeploai_hanhkiem_12 = "Hạnh kiểm 12: Khá"; }
            if (model_diem.ThiSinhDangKy.ThiSinh_HanhKiemLop12 == 2) { _xeploai_hanhkiem_12 = "Hạnh kiểm 12: Trung bình"; }
            if (model_diem.ThiSinhDangKy.ThiSinh_HanhKiemLop12 == 1) { _xeploai_hanhkiem_12 = "Hạnh kiểm 12: Yếu"; }

            // DiemThiGQMon khai báo trong  Model.LibraryUsers
            MonDiemThiQG _mondiem1 = JsonConvert.DeserializeObject<MonDiemThiQG>(model_diem.Dkxt_KQTQG_Diem_M1);
            MonDiemThiQG _mondiem2 = JsonConvert.DeserializeObject<MonDiemThiQG>(model_diem.Dkxt_KQTQG_Diem_M2);
            MonDiemThiQG _mondiem3 = JsonConvert.DeserializeObject<MonDiemThiQG>(model_diem.Dkxt_KQTQG_Diem_M3);

            DbConnecttion dbthpt = new DbConnecttion();
            var model_item = dbthpt.DangKyXetTuyenKQTQGs.Where(x => x.Dkxt_KQTQG_ID == dkxt_KQTQG_ID).ToList();
            var tt_ts_dk = model_item.Select(s => new
            {
                dkxt_KQTQG_ID = s.Dkxt_KQTQG_ID,
                thiSinh_ID = s.ThiSinh_ID,
                thiSinh_HoTen = s.ThiSinhDangKy.ThiSinh_HoLot + " " + s.ThiSinhDangKy.ThiSinh_Ten,
                nganh_Ten = s.Nganh.Nganh_TenNganh,
                thm_ID = s.Thm_ID,
                doiTuong_All = s.ThiSinhDangKy.DoiTuong.DoiTuong_Ten + " - Ưu tiên: " + s.ThiSinhDangKy.DoiTuong.DoiTuong_DiemUuTien,
                khuVuc_All = s.ThiSinhDangKy.KhuVuc.KhuVuc_Ten + " - Ưu tiên: " + s.ThiSinhDangKy.KhuVuc.KhuVuc_DiemUuTien,
                dkxt_KQTQG_NguyenVong = s.Dkxt_KQTQG_NguyenVong,
                dkxt_KQTQG_NamTotNghiep = s.Dkxt_KQTQG_NamTotNghiep,

                tenMon1 = _mondiem1.TenMon,
                tenMon2 = _mondiem2.TenMon,
                tenMon3 = _mondiem3.TenMon,

                diemMon1 = _mondiem1.Diem,
                diemMon2 = _mondiem2.Diem,
                diemMon3 = _mondiem3.Diem,

                dkxt_KQTQG_Diem_Tong = s.Dkxt_KQTQG_Diem_Tong,
                khoiNganh_Ten = s.Nganh.KhoiNganh.KhoiNganh_Ten,
                nganh_GhiChu = s.Nganh.Nganh_GhiChu,
                thm_MaTen = s.ToHopMon.Thm_MaTen,


                dkxt_KQTQG_NgayDangKy = s.Dkxt_KQTQG_NgayDangKy,
                dkxt_KQTQG_XepLoaiHocLuc_12 = _xeploai_hocluc_12,
                dkxt_KQTQG_XepLoaiHanhKiem_12 = _xeploai_hanhkiem_12,
                // lấy ra chuỗi các minh chứng
                dkxt_KQTQG_MinhChung_CNTotNghiep = s.Dkxt_KQTQG_MinhChung_CNTotNghiep,
                dkxt_KQTQG_MinhChung_HocBa = s.Dkxt_KQTQG_MinhChung_HocBa,
                dkxt_KQTQG_MinhChung_UuTien = s.Dkxt_KQTQG_MinhChung_UuTien,
                dkxt_KQTQG_MinhChung_CCCD = s.Dkxt_KQTQG_MinhChung_BangTN, // lưu ý là minh chứng căn cước công dân              

            });
            return Json(new { success = true, data = tt_ts_dk }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DkxtKQTthpt_hs_DeleteMC(ThongTinXoaMC entity)
        {
            db = new DbConnecttion();
           
            var model = db.DangKyXetTuyenKQTQGs.Where(x => x.Dkxt_KQTQG_ID ==  entity.Dkxt_ID).FirstOrDefault();
            // string modifiedString = originalString.Replace(stringToRemove, "");
            if (entity.Dkxt_LoaiMC == "1") { model.Dkxt_KQTQG_MinhChung_CNTotNghiep = model.Dkxt_KQTQG_MinhChung_CNTotNghiep.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "2") { model.Dkxt_KQTQG_MinhChung_HocBa = model.Dkxt_KQTQG_MinhChung_HocBa.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "3") { model.Dkxt_KQTQG_MinhChung_BangTN = model.Dkxt_KQTQG_MinhChung_BangTN.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "4") { model.Dkxt_KQTQG_MinhChung_CCCD = model.Dkxt_KQTQG_MinhChung_CCCD.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "5") { model.Dkxt_KQTQG_MinhChung_UuTien = model.Dkxt_KQTQG_MinhChung_UuTien.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "6") { model.Dkxt_KQTQG_KinhPhi_TepMinhChung = model.Dkxt_KQTQG_KinhPhi_TepMinhChung.Replace(entity.Dkxt_Url + "#", ""); }
            
            if (String.IsNullOrEmpty(model.Dkxt_KQTQG_KinhPhi_TepMinhChung) && model.Dkxt_KQTQG_TrangThai_KinhPhi != 0)                 
            {
                model.Dkxt_KQTQG_TrangThai_KinhPhi = 2;                
                model.Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (String.IsNullOrEmpty(model.Dkxt_KQTQG_MinhChung_CNTotNghiep) && model.Dkxt_KQTQG_TrangThai_HoSo != 0)
            {
                model.Dkxt_KQTQG_TrangThai_HoSo = 1;               
            }

            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region hiển thị và kiểm tra thông tin thí sinh đăng ký xét tuyển sử dụng học bạ

        [AdminSessionCheck]
        public ActionResult DkxtHocBa(string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            db = new DbConnecttion();
            var dot_hientai = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var model = (from item in db.DangKyXetTuyenHBs.Where(x => x.DotXT_ID == dot_hientai.Dxt_ID) select item)
                                                  .OrderBy(x => x.ThiSinh_ID)
                                                  .ThenBy(x => x.Dkxt_HB_NguyenVong)
                                                  .ThenBy(x => x.Nganh.Nganh_TenNganh)
                                                  .Include(x => x.ThiSinhDangKy)
                                                  .Include(x => x.Nganh)
                                                  .Include(x => x.ToHopMon)
                                                  .Include(x => x.DotXetTuyen)
                                                  .Include(x => x.ThiSinhDangKy.DoiTuong)
                                                  .Include(x => x.ThiSinhDangKy.KhuVuc)
                                                  .Include(x => x.PhuongThucXetTuyen);
            ViewBag.filteriNam = db.NamHocs.Where(x => x.NamHoc_TrangThai == 1).FirstOrDefault().NamHoc_Ten;

            #region lọc dữ liệu theo nguyện vọng
            var list_items_nv = (from item in model select (new { value_item = item.Dkxt_HB_NguyenVong.ToString(), text_item = "NV " + item.Dkxt_HB_NguyenVong.ToString() })).Distinct().ToList();

            ViewBag.filteriNvong = new SelectList(list_items_nv.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            // thực hiện lọc theo nguyện vọng
            if (!String.IsNullOrEmpty(filteriNvong))
            {
                int _dkxt_KQTQG_NguyenVong = Int32.Parse(filteriNvong);
                model = model.Where(x => x.Dkxt_HB_NguyenVong == _dkxt_KQTQG_NguyenVong);
            }
            #endregion

            #region lọc dữ liệu theo ngành
            var list_items_nganh = (from item in model select (new { value_item = item.Nganh.Nganh_ID, text_item = item.Nganh.Nganh_TenNganh })).Distinct().ToList();
            ViewBag.filteriNganh = new SelectList(list_items_nganh.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            if (!String.IsNullOrEmpty(filteriNganh))
            {
                int _dkxt_KQTQG_Nganh_ID = Int32.Parse(filteriNganh);
                model = model.Where(x => x.Nganh_ID == _dkxt_KQTQG_Nganh_ID);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in model select item.Dkxt_HB_TrangThai_KinhPhi).Distinct().ToList();
            List<StatusTracking> filteri_items_lephi = new List<StatusTracking>();

            foreach (var _item in list_items_lephi)
            {
                if (_item == 0)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa nộp minh chứng lệ phí" });
                }
                if (_item == 1)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã nộp, chưa kiểm tra" });
                }
                if (_item == 2)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã kiểm tra, Có sai sót" });
                }
                if (_item == 3)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 3, St_Name = "Đã sửa, chưa kiểm tra" });
                }
                if (_item == 9)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm tra, đúng" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int _dkxt_TrangThai = Int32.Parse(filteriLePhi);
                model = model.Where(x => x.Dkxt_HB_TrangThai_KinhPhi == _dkxt_TrangThai);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in model select item.Dkxt_HB_TrangThai_HoSo).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa kiểm tra" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã kiểm tra, Có sai sót" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã sửa, chưa kiểm tra" });
                }
                if (_item == 9)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm tra, hồ sơ đúng, đủ" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _dkxt_TrangThai_Hoso = Int32.Parse(filteriHoSo);
                model = model.Where(x => x.Dkxt_HB_TrangThai_HoSo == _dkxt_TrangThai_Hoso);
            }
            #endregion
            // thưc hiện tìm kiếm: theo họ, tên, cccd, điện thoại, email
            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.ThiSinhDangKy.ThiSinh_Ten.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_HoLot.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_CCCD.Contains(searchString)
                                      || m.ThiSinhDangKy.ThiSinh_DienThoai.Contains(searchString)
                                      || m.ThiSinhDangKy.ThiSinh_Email.Contains(searchString)
                                      || m.Nganh.Nganh_GhiChu.Contains(searchString)
                                      || m.ToHopMon.Thm_MaTen.Contains(searchString)
                                      || m.Dkxt_HB_Diem_Tong_Full.Contains(searchString));
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

            ViewBag.NvongFilteri = filteriNvong;
            ViewBag.NganhFilteri = filteriNganh;
            ViewBag.LePhiFilteri = filteriLePhi;
            ViewBag.HoSoFilteri = filteriHoSo;
            ViewBag.SearchString = searchString;
            ViewBag.pageCurren = page;

            ViewBag.totalRecod = model.Count();

            #endregion
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        [AdminSessionCheck]
        public ActionResult DkxtHocBa_hs_view(long Dkxt_ID, string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;
            return View();
        }
        public JsonResult DkxtHocBa_hs_DeleteMC(ThongTinXoaMC entity)
        {
            db = new DbConnecttion();

            var model = db.DangKyXetTuyenHBs.Where(x => x.Dkxt_HB_ID == entity.Dkxt_ID).FirstOrDefault();
            // string modifiedString = originalString.Replace(stringToRemove, "");
            if (entity.Dkxt_LoaiMC == "2") { model.Dkxt_HB_MinhChung_HB = model.Dkxt_HB_MinhChung_HB.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "3") { model.Dkxt_HB_MinhChung_Bang = model.Dkxt_HB_MinhChung_Bang.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "4") { model.Dkxt_HB_MinhChung_CCCD = model.Dkxt_HB_MinhChung_CCCD.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "5") { model.Dkxt_HB_MinhChung_UuTien = model.Dkxt_HB_MinhChung_UuTien.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "6") { model.Dkxt_HB_KinhPhi_TepMinhChung = model.Dkxt_HB_KinhPhi_TepMinhChung.Replace(entity.Dkxt_Url + "#", ""); }

            if (String.IsNullOrEmpty(model.Dkxt_HB_KinhPhi_TepMinhChung) && model.Dkxt_HB_TrangThai_KinhPhi != 0)                
            {
                model.Dkxt_HB_TrangThai_KinhPhi = 2;              
                model.Dkxt_HB_KinhPhi_NgayThang_CheckMC = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (String.IsNullOrEmpty(model.Dkxt_HB_MinhChung_HB) && model.Dkxt_HB_TrangThai_HoSo != 0)
            {
                model.Dkxt_HB_TrangThai_HoSo = 1;
            }
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DkxtHocBa_hs_Detail(DangKyXetTuyenHB entity)
        {
            db = new DbConnecttion();
            long dkxt_id = entity.Dkxt_HB_ID;

            DangKyXetTuyenHB model_01 = db.DangKyXetTuyenHBs.Include(x => x.ThiSinhDangKy).Where(x => x.Dkxt_HB_ID == dkxt_id).FirstOrDefault();
            string _xeploai_hocluc_12 = "";
            if (model_01.ThiSinhDangKy.ThiSinh_HocLucLop12 == 4) { _xeploai_hocluc_12 = "Học lực 12: Xuất sắc"; }
            if (model_01.ThiSinhDangKy.ThiSinh_HocLucLop12 == 3) { _xeploai_hocluc_12 = "Học lực 12: Giỏi"; }
            if (model_01.ThiSinhDangKy.ThiSinh_HocLucLop12 == 2) { _xeploai_hocluc_12 = "Học lực 12: Khá"; }
            if (model_01.ThiSinhDangKy.ThiSinh_HocLucLop12 == 1) { _xeploai_hocluc_12 = "Học lực 12: Trung bình"; }

            string _xeploai_hanhkiem_12 = "";
            if (model_01.ThiSinhDangKy.ThiSinh_HanhKiemLop12 == 4) { _xeploai_hanhkiem_12 = "Hạnh kiểm 12: Tốt"; }
            if (model_01.ThiSinhDangKy.ThiSinh_HanhKiemLop12 == 3) { _xeploai_hanhkiem_12 = "Hạnh kiểm 12: Khá"; }
            if (model_01.ThiSinhDangKy.ThiSinh_HanhKiemLop12 == 2) { _xeploai_hanhkiem_12 = "Hạnh kiểm 12: Trung bình"; }
            if (model_01.ThiSinhDangKy.ThiSinh_HanhKiemLop12 == 1) { _xeploai_hanhkiem_12 = "Hạnh kiểm 12: Yếu"; }

            // DiemThiGQMon khai báo trong  Model.LibraryUsers

            MonDiemHB _mondiem1 = JsonConvert.DeserializeObject<MonDiemHB>(model_01.Dkxt_HB_Diem_M1);
            MonDiemHB _mondiem2 = JsonConvert.DeserializeObject<MonDiemHB>(model_01.Dkxt_HB_Diem_M2);
            MonDiemHB _mondiem3 = JsonConvert.DeserializeObject<MonDiemHB>(model_01.Dkxt_HB_Diem_M3);

            DbConnecttion dbhb = new DbConnecttion();
            var model_item = dbhb.DangKyXetTuyenHBs.Where(x => x.Dkxt_HB_ID == dkxt_id).ToList();
            var tt_ts_dk = model_item.Select(s => new
            {
                dkxt_ID = s.Dkxt_HB_ID,
                thiSinh_ID = s.ThiSinh_ID,
                thiSinh_HoTen = s.ThiSinhDangKy.ThiSinh_HoLot + " " + s.ThiSinhDangKy.ThiSinh_Ten,
                nganh_Ten = s.Nganh.Nganh_TenNganh,
                thm_ID = s.Thm_ID,
                doiTuong_All = s.ThiSinhDangKy.DoiTuong.DoiTuong_Ten + " - Ưu tiên: " + s.ThiSinhDangKy.DoiTuong.DoiTuong_DiemUuTien,
                khuVuc_All = s.ThiSinhDangKy.KhuVuc.KhuVuc_Ten + " - Ưu tiên: " + s.ThiSinhDangKy.KhuVuc.KhuVuc_DiemUuTien,
                dkxt_NguyenVong = s.Dkxt_HB_NguyenVong,

                tenMon1 = _mondiem1.TenMon,
                diemMon1_HK1 = _mondiem1.HK1,
                diemMon1_HK2 = _mondiem1.HK2,
                diemMon1_HK3 = _mondiem1.HK3,
                diemMon1_TBMon = _mondiem1.DiemTrungBinh,


                tenMon2 = _mondiem2.TenMon,
                diemMon2_HK1 = _mondiem2.HK1,
                diemMon2_HK2 = _mondiem2.HK2,
                diemMon2_HK3 = _mondiem2.HK3,
                diemMon2_TBMon = _mondiem2.DiemTrungBinh,

                tenMon3 = _mondiem3.TenMon,
                diemMon3_HK1 = _mondiem3.HK1,
                diemMon3_HK2 = _mondiem3.HK2,
                diemMon3_HK3 = _mondiem3.HK3,
                diemMon3_TBMon = _mondiem3.DiemTrungBinh,

                dkxt_Diem_Tong = s.Dkxt_HB_Diem_Tong,
                khoiNganh_Ten = s.Nganh.KhoiNganh.KhoiNganh_Ten,
                nganh_GhiChu = s.Nganh.Nganh_GhiChu,
                thm_MaTen = s.ToHopMon.Thm_MaTen,

                //dkxt_NgayDangKy = s.Dkxt_NgayDangKy,
                dkxt_XepLoaiHocLuc_12 = _xeploai_hocluc_12,
                dkxt_XepLoaiHanhKiem_12 = _xeploai_hanhkiem_12,
                // lấy ra chuỗi các minh chứng

                dkxt_MinhChung_HocBa = s.Dkxt_HB_MinhChung_HB,
                dkxt_MinhChung_Bang = s.Dkxt_HB_MinhChung_Bang,
                dkxt_MinhChung_CCCD = s.Dkxt_HB_MinhChung_CCCD,
                dkxt_MinhChung_UuTien = s.Dkxt_HB_MinhChung_UuTien,
            });
            return Json(new { success = true, data = tt_ts_dk }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region hiển thị và kiểm tra thông tin thí sinh đăng ký xét tuyển tuyển thẳng
        [AdminSessionCheck]
        public ActionResult DkxtTt(string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            db = new DbConnecttion();
            var dot_hientai = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var model = (from item in db.DangKyXetTuyenThangs.Where(x => x.DotXT_ID == dot_hientai.Dxt_ID) select item)
                                        .OrderBy(x => x.ThiSinh_ID)
                                        .ThenBy(x => x.Dkxt_NguyenVong)
                                        .ThenBy(x => x.Nganh.Nganh_TenNganh)
                                        .Include(x => x.ThiSinhDangKy)
                                        .Include(x => x.Nganh)
                                        .Include(x => x.ThiSinhDangKy.DoiTuong)
                                        .Include(x => x.ThiSinhDangKy.KhuVuc);

            ViewBag.filteriNam = db.NamHocs.Where(x => x.NamHoc_TrangThai == 1).FirstOrDefault().NamHoc_Ten;


            #region lọc dữ liệu theo nguyện vọng
            var list_items_nv = (from item in model select (new { value_item = item.Dkxt_NguyenVong.ToString(), text_item = item.Dkxt_NguyenVong.ToString() })).Distinct().ToList();
            ViewBag.filteriNvong = new SelectList(list_items_nv.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            // thực hiện lọc theo nguyện vọng
            if (!String.IsNullOrEmpty(filteriNvong))
            {
                int _dkxt_KQTQG_NguyenVong = Int32.Parse(filteriNvong);
                model = model.Where(x => x.Dkxt_NguyenVong == _dkxt_KQTQG_NguyenVong);
            }
            #endregion

            #region lọc dữ liệu theo ngành
            var list_items_nganh = (from item in model select (new { value_item = item.Nganh.Nganh_ID, text_item = item.Nganh.Nganh_TenNganh })).Distinct().ToList();
            ViewBag.filteriNganh = new SelectList(list_items_nganh.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            if (!String.IsNullOrEmpty(filteriNganh))
            {
                int _dkxt_TT_Nganh_ID = Int32.Parse(filteriNganh);
                model = model.Where(x => x.Nganh_ID == _dkxt_TT_Nganh_ID);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in model select item.Dkxt_TrangThai_KinhPhi).Distinct().ToList();
            List<StatusTracking> filteri_items_lephi = new List<StatusTracking>();

            foreach (var _item in list_items_lephi)
            {
                if (_item == 0)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa nộp minh chứng lệ phí" });
                }
                if (_item == 1)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã nộp, chưa kiểm tra" });
                }
                if (_item == 2)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã kiểm tra, có sai sót" });
                }
                if (_item == 3)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã sửa, chưa kiểm tra" });
                }
                if (_item == 9)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm tra, đúng" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int _dkxt_TrangThai = Int32.Parse(filteriLePhi);
                model = model.Where(x => x.Dkxt_TrangThai_KinhPhi == _dkxt_TrangThai);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in model select item.Dkxt_TrangThai_HoSo).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa kiểm tra" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã kiểm tra, Có sai sót" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã sửa, chưa kiểm tra" });
                }
                if (_item == 9)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm tra, hồ sơ đúng, đủ" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _dkxt_TrangThai_HoSo = Int32.Parse(filteriHoSo);
                model = model.Where(x => x.Dkxt_TrangThai_HoSo == _dkxt_TrangThai_HoSo);
            }
            #endregion

            // thưc hiện tìm kiếm: theo họ, tên, cccd, điện thoại, email
            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.ThiSinhDangKy.ThiSinh_Ten.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_HoLot.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_CCCD.Contains(searchString)
                                      || m.ThiSinhDangKy.ThiSinh_DienThoai.Contains(searchString)
                                      || m.ThiSinhDangKy.ThiSinh_Email.Contains(searchString)
                                      || m.Nganh.Nganh_GhiChu.Contains(searchString)
                                      || m.Dkxt_MonDatGiai.Contains(searchString)
                                      || m.Dkxt_NamDatGiai.Contains(searchString));
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

            ViewBag.NvongFilteri = filteriNvong;
            ViewBag.NganhFilteri = filteriNganh;
            ViewBag.LePhiFilteri = filteriLePhi;
            ViewBag.HoSoFilteri = filteriHoSo;
            ViewBag.SearchString = searchString;
            ViewBag.pageCurren = page;

            ViewBag.totalRecod = model.Count();

            #endregion
            return View(model.ToPagedList(pageNumber, pageSize));
        }
 
        [AdminSessionCheck]
        public ActionResult DkxtTt_hs_view(long Dkxt_ID, string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;
            return View();
        }
     
        public JsonResult DkxtTt_hs_DeleteMC(ThongTinXoaMC entity)
        {
            db = new DbConnecttion();

            var model = db.DangKyXetTuyenThangs.Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();
            // string modifiedString = originalString.Replace(stringToRemove, "");
            if (entity.Dkxt_LoaiMC == "1") { model.Dkxt_MinhChung_Giai = model.Dkxt_MinhChung_Giai.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "2") { model.Dkxt_MinhChung_HB = model.Dkxt_MinhChung_HB.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "3") { model.Dkxt_MinhChung_Bang = model.Dkxt_MinhChung_Bang.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "4") { model.Dkxt_MinhChung_CCCD = model.Dkxt_MinhChung_CCCD.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "5") { model.Dkxt_MinhChung_UuTien = model.Dkxt_MinhChung_UuTien.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "6") { model.Dkxt_KinhPhi_TepMinhChung = model.Dkxt_KinhPhi_TepMinhChung.Replace(entity.Dkxt_Url + "#", ""); }

            if (String.IsNullOrEmpty(model.Dkxt_KinhPhi_TepMinhChung)  && model.Dkxt_TrangThai_KinhPhi != 0)
            {
                model.Dkxt_TrangThai_KinhPhi = 2;
                model.Dkxt_KinhPhi_NgayThang_CheckMC = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (String.IsNullOrEmpty(model.Dkxt_MinhChung_Giai) && model.Dkxt_TrangThai_HoSo != 0)
            {
                model.Dkxt_TrangThai_HoSo = 1;               
            }

            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region hiển thị và kiểm tra thông tin thí sinh đăng ký xét tuyển khác sử dụng kết quả thi ngoại ngữ

        [AdminSessionCheck]
        public ActionResult DkxtkqCcnn(string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            db = new DbConnecttion();
            var dot_hientai = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var model = (from h in db.DangKyXetTuyenKhacs.Where(x => x.DotXT_ID == dot_hientai.Dxt_ID) select h)
                .Where(x => x.Dkxt_ToHopXT.Equals("HDP5"))
                .OrderBy(x => x.ThiSinh_ID)
                .ThenBy(x => x.Dkxt_NguyenVong)
                .ThenBy(x => x.Nganh.Nganh_TenNganh)
                .Include(d => d.ThiSinhDangKy.DoiTuong)
                .Include(d => d.DotXetTuyen)
                .Include(d => d.ThiSinhDangKy.KhuVuc)
                .Include(d => d.Nganh)
                .Include(d => d.PhuongThucXetTuyen)
                .Include(d => d.ThiSinhDangKy);

            #region lọc dữ liệu theo nguyện vọng
            var list_items_nv = (from item in model select (new { value_item = item.Dkxt_NguyenVong.ToString(), text_item = "NV " + item.Dkxt_NguyenVong.ToString() })).Distinct().ToList();

            ViewBag.filteriNvong = new SelectList(list_items_nv.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            // thực hiện lọc theo nguyện vọng
            if (!String.IsNullOrEmpty(filteriNvong))
            {
                int _dkxt_KQTQG_NguyenVong = Int32.Parse(filteriNvong);
                model = model.Where(x => x.Dkxt_NguyenVong == _dkxt_KQTQG_NguyenVong);
            }
            #endregion

            #region lọc dữ liệu theo ngành
            var list_items_nganh = (from item in model select (new { value_item = item.Nganh.Nganh_ID, text_item = item.Nganh.Nganh_TenNganh })).Distinct().ToList();
            ViewBag.filteriNganh = new SelectList(list_items_nganh.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            if (!String.IsNullOrEmpty(filteriNganh))
            {
                int _dkxt_KQTQG_Nganh_ID = Int32.Parse(filteriNganh);
                model = model.Where(x => x.Nganh_ID == _dkxt_KQTQG_Nganh_ID);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in model select item.Dkxt_TrangThai_KinhPhi).Distinct().ToList();
            List<StatusTracking> filteri_items_lephi = new List<StatusTracking>();

            foreach (var _item in list_items_lephi)
            {
                if (_item == 0)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa nộp minh chứng lệ phí" });
                }
                if (_item == 1)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã nộp, chưa kiểm tra" });
                }
                if (_item == 2)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã kiểm tra, Có sai sót" });
                }
                if (_item == 3)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 3, St_Name = "Đã sửa, chưa kiểm tra" });
                }
                if (_item == 9)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm tra, đúng" });
                }               
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int _dkxt_TrangThai = Int32.Parse(filteriLePhi);
                model = model.Where(x => x.Dkxt_TrangThai_KinhPhi == _dkxt_TrangThai);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in model select item.Dkxt_TrangThai_HoSo).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa kiểm tra" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã kiểm tra, Có sai sót" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã sửa, chưa kiểm tra" });
                }
                if (_item == 9)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm tra, hồ sơ đúng, đủ" });
                }
            }
            
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _dkxt_TrangThai_HoSo = Int32.Parse(filteriHoSo);
                model = model.Where(x => x.Dkxt_TrangThai_HoSo== _dkxt_TrangThai_HoSo);
            }
            #endregion
            // thưc hiện tìm kiếm: theo họ, tên, cccd, điện thoại, email
            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.ThiSinhDangKy.ThiSinh_Ten.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_HoLot.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_CCCD.Contains(searchString)
                                      || m.ThiSinhDangKy.ThiSinh_DienThoai.Contains(searchString)
                                      || m.ThiSinhDangKy.ThiSinh_Email.Contains(searchString)
                                      || m.Nganh.Nganh_GhiChu.Contains(searchString)
                                      || m.Dkxt_ToHopXT.Contains(searchString)
                                      || m.Dkxt_TongDiem.ToString().Equals(searchString));
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

            ViewBag.NvongFilteri = filteriNvong;
            ViewBag.NganhFilteri = filteriNganh;
            ViewBag.LePhiFilteri = filteriLePhi;
            ViewBag.HoSoFilteri = filteriHoSo;
            ViewBag.SearchString = searchString;
            ViewBag.pageCurren = page;

            ViewBag.totalRecod = model.Count();

            #endregion
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        [AdminSessionCheck]
        public ActionResult DkxtkqCcnn_hs_view(string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page,long Dkxt_ID)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;          
            return View();
        }
        public JsonResult DkxtkqCcnn_hs_DeleteMC(ThongTinXoaMC entity)
        {
            db = new DbConnecttion();

            var model = db.DangKyXetTuyenKhacs.Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();
            // string modifiedString = originalString.Replace(stringToRemove, "");
            if (entity.Dkxt_LoaiMC == "1") { model.Dkxt_MinhChung_KetQua = model.Dkxt_MinhChung_KetQua.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "2") { model.Dkxt_MinhChung_HB = model.Dkxt_MinhChung_HB.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "3") { model.Dkxt_MinhChung_Bang = model.Dkxt_MinhChung_Bang.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "4") { model.Dkxt_MinhChung_CCCD = model.Dkxt_MinhChung_CCCD.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "5") { model.Dkxt_MinhChung_UuTien = model.Dkxt_MinhChung_UuTien.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "6") { model.Dkxt_KinhPhi_TepMinhChung = model.Dkxt_KinhPhi_TepMinhChung.Replace(entity.Dkxt_Url + "#", ""); }

            if (String.IsNullOrEmpty(model.Dkxt_KinhPhi_TepMinhChung) && model.Dkxt_TrangThai_KinhPhi != 0)
            {
                model.Dkxt_TrangThai_KinhPhi = 2;
                model.Dkxt_KinhPhi_NgayThang_CheckMC = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (String.IsNullOrEmpty(model.Dkxt_MinhChung_KetQua) && model.Dkxt_TrangThai_HoSo != 0)
            {
                model.Dkxt_TrangThai_HoSo = 1;
            }
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region hiển thị và kiểm tra thông tin thí sinh đăng ký xét tuyển khác sử dụng kết quả thi dgnl
        [AdminSessionCheck]
        public ActionResult DkxtkqDgnl(string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            db = new DbConnecttion();
            var dot_hientai = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var model = (from h in db.DangKyXetTuyenKhacs.Where(x => x.DotXT_ID == dot_hientai.Dxt_ID)
                         select h).Where(x => x.Dkxt_ToHopXT.Equals("HDP6")).OrderBy(x => x.ThiSinh_ID).ThenBy(x => x.Dkxt_NguyenVong).ThenBy(x => x.Nganh.Nganh_TenNganh).Include(d => d.ThiSinhDangKy.DoiTuong).Include(d => d.DotXetTuyen).Include(d => d.ThiSinhDangKy.KhuVuc).Include(d => d.Nganh).Include(d => d.PhuongThucXetTuyen).Include(d => d.ThiSinhDangKy);

            #region lọc dữ liệu theo nguyện vọng
            var list_items_nv = (from item in model select (new { value_item = item.Dkxt_NguyenVong.ToString(), text_item = "NV " + item.Dkxt_NguyenVong.ToString() })).Distinct().ToList();

            ViewBag.filteriNvong = new SelectList(list_items_nv.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            // thực hiện lọc theo nguyện vọng
            if (!String.IsNullOrEmpty(filteriNvong))
            {
                int _dkxt_NguyenVong = Int32.Parse(filteriNvong);
                model = model.Where(x => x.Dkxt_NguyenVong == _dkxt_NguyenVong);
            }
            #endregion

            #region lọc dữ liệu theo ngành
            var list_items_nganh = (from item in model select (new { value_item = item.Nganh.Nganh_ID, text_item = item.Nganh.Nganh_TenNganh })).Distinct().ToList();
            ViewBag.filteriNganh = new SelectList(list_items_nganh.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            if (!String.IsNullOrEmpty(filteriNganh))
            {
                int _dkxt_Nganh_ID = Int32.Parse(filteriNganh);
                model = model.Where(x => x.Nganh_ID == _dkxt_Nganh_ID);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in model select item.Dkxt_TrangThai_KinhPhi).Distinct().ToList();
            List<StatusTracking> filteri_items_lephi = new List<StatusTracking>();

            foreach (var _item in list_items_lephi)
            {
                if (_item == 0)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa nộp minh chứng lệ phí" });
                }
                if (_item == 1)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã nộp, chưa kiểm tra" });
                }
                if (_item == 2)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã kiểm tra, Có sai sót" });
                }
                if (_item == 3)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 3, St_Name = "Đã sửa, chưa kiểm tra" });
                }
                if (_item == 9)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm tra, đúng" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int _dkxt_TrangThai = Int32.Parse(filteriLePhi);
                model = model.Where(x => x.Dkxt_TrangThai_KinhPhi == _dkxt_TrangThai);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in model select item.Dkxt_TrangThai_HoSo).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa kiểm tra" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "Đã kiểm tra, có sai sót" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã sửa, chưa kiểm tra" });
                }
                if (_item == 9)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 9, St_Name = "Đã kiểm tra, hồ sơ đúng, đủ" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _dkxt_TrangThai_HoSo = Int32.Parse(filteriHoSo);
                model = model.Where(x => x.Dkxt_TrangThai_HoSo == _dkxt_TrangThai_HoSo);
            }
            #endregion
            // thưc hiện tìm kiếm: theo họ, tên, cccd, điện thoại, email
            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.ThiSinhDangKy.ThiSinh_Ten.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_HoLot.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_CCCD.Contains(searchString)
                                      || m.ThiSinhDangKy.ThiSinh_DienThoai.Contains(searchString)
                                      || m.ThiSinhDangKy.ThiSinh_Email.Contains(searchString)
                                      || m.Nganh.Nganh_GhiChu.Contains(searchString)
                                      || m.Dkxt_ToHopXT.Contains(searchString)
                                      || m.Dkxt_TongDiem.ToString().Equals(searchString));
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

            ViewBag.NvongFilteri = filteriNvong;
            ViewBag.NganhFilteri = filteriNganh;
            ViewBag.LePhiFilteri = filteriLePhi;
            ViewBag.HoSoFilteri = filteriHoSo;
            ViewBag.SearchString = searchString;
            ViewBag.pageCurren = page;

            ViewBag.totalRecod = model.Count();

            #endregion
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        [AdminSessionCheck]
        public ActionResult DkxtkqDgnl_hs_view(string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page, long Dkxt_ID)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;           
            return View();
        }
        public JsonResult DkxtkqDgnl_hs_DeleteMC(ThongTinXoaMC entity)
        {
            db = new DbConnecttion();            
            var model = db.DangKyXetTuyenKhacs.Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();
            
            if (entity.Dkxt_LoaiMC == "1") { model.Dkxt_MinhChung_KetQua = model.Dkxt_MinhChung_KetQua.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "2") { model.Dkxt_MinhChung_HB = model.Dkxt_MinhChung_HB.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "3") { model.Dkxt_MinhChung_Bang = model.Dkxt_MinhChung_Bang.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "4") { model.Dkxt_MinhChung_CCCD = model.Dkxt_MinhChung_CCCD.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "5") { model.Dkxt_MinhChung_UuTien = model.Dkxt_MinhChung_UuTien.Replace(entity.Dkxt_Url + "#", ""); }
            if (entity.Dkxt_LoaiMC == "6") { model.Dkxt_KinhPhi_TepMinhChung = model.Dkxt_KinhPhi_TepMinhChung.Replace(entity.Dkxt_Url + "#", ""); }

            if (String.IsNullOrEmpty(model.Dkxt_KinhPhi_TepMinhChung) && model.Dkxt_TrangThai_KinhPhi != 0)
            {
                model.Dkxt_TrangThai_KinhPhi = 2;
                model.Dkxt_KinhPhi_NgayThang_CheckMC = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (String.IsNullOrEmpty(model.Dkxt_MinhChung_KetQua) && model.Dkxt_TrangThai_HoSo != 0)
            {
                model.Dkxt_TrangThai_HoSo = 1;
            }
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region hiển thị và kiểm tra thông tin thí sinh đăng ký thi năng khiếu
        [AdminSessionCheck]
        public ActionResult Dkdttnk(string filteriDotxt, string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            db = new DbConnecttion();
            var dot_hientai = db.DotXetTuyens.Where(x => x.Dxt_Classify == 1 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var model = (from item in db.DangKyDuThiNangKhieus select item)
                                        .OrderBy(x => x.ThiSinh_ID)
                                        .ThenBy(x => x.Nganh.Nganh_TenNganh)
                                        .Include(x => x.ThiSinhDangKy)
                                        .Include(x => x.Nganh)
                                        .Include(x => x.ToHopMon)
                                        .Include(x => x.ThiSinhDangKy.DoiTuong)
                                        .Include(x => x.ThiSinhDangKy.KhuVuc);

            // lấy thông tin năm hiện tại
            ViewBag.filteriNam = db.NamHocs.Where(x => x.NamHoc_TrangThai == 1).FirstOrDefault().NamHoc_Ten;
            #region lọc dữ liệu theo đợt
            var dotxts = db.DotXetTuyens.Include(x => x.NamHoc).Where(x => x.NamHoc.NamHoc_TrangThai == 1 && x.Dxt_Classify == 1).ToList();

            dotxts.Add(new DotXetTuyen() { Dxt_ID = 0, Dxt_Ten = "Tất cả" });
            int _dotxt_hientai = dotxts.FirstOrDefault(x => x.Dxt_TrangThai_Xt == 1 && x.Dxt_Classify == 1).Dxt_ID;
            ViewBag.filteriDotxt = new SelectList(dotxts.OrderBy(x => x.Dxt_ID).ToList(), "Dxt_ID", "Dxt_Ten", _dotxt_hientai);

            // thực hiện lọc 
            if (!String.IsNullOrEmpty(filteriDotxt))
            {
                int _Dotxt_ID = Int32.Parse(filteriDotxt);
                if (_Dotxt_ID > 0)
                {
                    model = model.Where(x => x.DotXT_ID == _Dotxt_ID);
                }
            }
            #endregion

            #region lọc dữ liệu theo ngành
            var list_items_nganh = (from item in model select (new { value_item = item.Nganh.Nganh_ID, text_item = item.Nganh.Nganh_TenNganh })).Distinct().ToList();
            ViewBag.filteriNganh = new SelectList(list_items_nganh.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            if (!String.IsNullOrEmpty(filteriNganh))
            {
                int _dkdt_NK_Nganh_ID = Int32.Parse(filteriNganh);
                model = model.Where(x => x.Nganh_ID == _dkdt_NK_Nganh_ID);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in model select item.Dkdt_TrangThai_LePhi).Distinct().ToList();
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
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int _dkdt_nk_TrangThai = Int32.Parse(filteriLePhi);
                model = model.Where(x => x.Dkdt_TrangThai_LePhi == _dkdt_nk_TrangThai);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in model select item.Dkdt_TrangThai_HoSo).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa kiểm tra" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "Minh chứng sai" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _dkdt_TrangThai_HoSo = Int32.Parse(filteriHoSo);
                model = model.Where(x => x.Dkdt_TrangThai_HoSo == _dkdt_TrangThai_HoSo);
            }
            #endregion

            // thưc hiện tìm kiếm: theo họ, tên, cccd, điện thoại, email
            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.ThiSinhDangKy.ThiSinh_Ten.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_HoLot.ToUpper().Contains(searchString.ToUpper())
                                      || m.ThiSinhDangKy.ThiSinh_CCCD.Contains(searchString)
                                      || m.ThiSinhDangKy.ThiSinh_DienThoai.Contains(searchString)
                                      || m.Nganh.Nganh_TenNganh.Contains(searchString)
                                      || m.ToHopMon.Thm_TenToHop.Contains(searchString));
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

            ViewBag.DotxtFilteri = filteriDotxt;
            ViewBag.NvongFilteri = filteriNvong;
            ViewBag.NganhFilteri = filteriNganh;
            ViewBag.LePhiFilteri = filteriLePhi;
            ViewBag.HoSoFilteri = filteriHoSo;
            ViewBag.SearchString = searchString;
            ViewBag.pageCurren = page;

            ViewBag.totalRecod = model.Count();

            #endregion
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Dkdttnk_hs_view(string filteriDotxt, string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page, long? Dkxt_ID)
        {
            ViewBag.Dkxt_ID = Dkxt_ID;
            return View();
        }

        public JsonResult Dkdttnk_hs_view_getdata(DangKyDuThiNangKhieu entity)
        {
          
            db = new DbConnecttion();
            var Dothihientai = db.DotXetTuyens.Where(x => x.Dxt_Classify == 1 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();

            var model = db.DangKyDuThiNangKhieus
                                .Include(d => d.ThiSinhDangKy)
                                .Where(x => x.Dkdt_NK_ID == entity.Dkdt_NK_ID && x.DotXT_ID == Dothihientai.Dxt_ID)
                                .FirstOrDefault();

            // cần lấy tên ngành và tổ hợp môn để hiển thị ra drop nếu đã đăng ký; mỗi thí sinh chỉ đăng ký 1 lần trong 1 đợt
            var TenNganhThi = db.Nganhs.Where(x => x.Nganh_ID == model.Nganh_ID).FirstOrDefault().Nganh_TenNganh;
            var TenToHopThi = db.ToHopMons.Where(x => x.Thm_ID == model.Thm_ID).FirstOrDefault().Thm_TenToHop;
            string ThiSinh_GT = "Nam";
            if (model.ThiSinhDangKy.ThiSinh_GioiTinh == 1) { ThiSinh_GT = "Nữ"; }

            var data_return = new
            {
                
                Dkdt_NK_ID = model.Dkdt_NK_ID,

                ThiSinh_HoLot = model.ThiSinhDangKy.ThiSinh_HoLot,
                ThiSinh_Ten = model.ThiSinhDangKy.ThiSinh_Ten,
                ThiSinh_NgaySinh = model.ThiSinhDangKy.ThiSinh_NgaySinh,
                ThiSinh_GioiTinh = ThiSinh_GT,
                ThiSinh_DanToc = model.ThiSinhDangKy.ThiSinh_DanToc,
                ThiSinh_CCCD = model.ThiSinhDangKy.ThiSinh_CCCD,
                ThiSinh_DienThoai = model.ThiSinhDangKy.ThiSinh_DienThoai,
                ThiSinh_Email = model.ThiSinhDangKy.ThiSinh_Email,

                dxt_Ten = Dothihientai.Dxt_Ten,
                dxt_ThoiGian_BatDau = Dothihientai.Dxt_ThoiGian_BatDau,
                dxt_ThoiGian_KetThuc = Dothihientai.Dxt_ThoiGian_KetThuc,

                nganh_ID = model.Nganh_ID,
                nganh_TenNganh = TenNganhThi,
                thm_ID = model.Thm_ID,
                thm_TenToHop = TenToHopThi,
                dkdt_NK_MonThi = model.Dkdt_NK_MonThi,

                dkdt_nk_Mc_CCCD = model.Dkdt_NK_MinhChung_CCCD,
                dkdt_nk_Mc_Lephi = model.Dkdt_LePhi_MinhChung_Tep,
                dkdt_nk_Matc = model.Dkdt_LePhi_MinhChung_MaThamChieu,

                dkdt_TrangThai_LePhi = model.Dkdt_TrangThai_LePhi,
                dkdt_TrangThai_HoSo=  model.Dkdt_TrangThai_HoSo,
                dkdt_ThongBaoKiemDuyet_HoSo = model.Dkdt_ThongBaoKiemDuyet_HoSo,

            };
            return Json(new { success = true, data = data_return }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Dkdtnk_hs_DeleteMC(ThongTinXoaMC entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyDuThiNangKhieus.Where(x => x.Dkdt_NK_ID == entity.Dkxt_ID).FirstOrDefault();           
            if (entity.Dkxt_LoaiMC == "4") { model.Dkdt_NK_MinhChung_CCCD = model.Dkdt_NK_MinhChung_CCCD.Replace(entity.Dkxt_Url + "#", ""); }
            
            if (entity.Dkxt_LoaiMC == "6") { model.Dkdt_LePhi_MinhChung_Tep = model.Dkdt_LePhi_MinhChung_Tep.Replace(entity.Dkxt_Url + "#", ""); }

            if (String.IsNullOrEmpty(model.Dkdt_LePhi_MinhChung_Tep) && model.Dkdt_TrangThai_LePhi != 0)
            {
                model.Dkdt_TrangThai_LePhi = 2;                
            }
            if (String.IsNullOrEmpty(model.Dkdt_NK_MinhChung_CCCD))
            {
                model.Dkdt_TrangThai_HoSo = 1;
            }
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Cập nhật trạng thái lệ phí và trạng thái hồ sơ
        public JsonResult Dkxt_KQTHPTQG_Update_KiemDuyet(DangKyXetTuyenKQTQG entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenKQTQGs.Where(x => x.Dkxt_KQTQG_ID == entity.Dkxt_KQTQG_ID).FirstOrDefault();
            model.Dkxt_KQTQG_TrangThai_KinhPhi = entity.Dkxt_KQTQG_TrangThai_KinhPhi;
            model.Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC = DateTime.Now.ToString("yyyy-MM-dd");
            model.Dkxt_KQTQG_TrangThai_HoSo = entity.Dkxt_KQTQG_TrangThai_HoSo;
            model.Dkxt_ThongBaoKiemDuyet_HoSo = entity.Dkxt_ThongBaoKiemDuyet_HoSo;
           
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Dkxt_HocBa_Update_KiemDuyet(DangKyXetTuyenHB entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenHBs.Where(x => x.Dkxt_HB_ID == entity.Dkxt_HB_ID).FirstOrDefault();
            model.Dkxt_HB_TrangThai_KinhPhi = entity.Dkxt_HB_TrangThai_KinhPhi;
            model.Dkxt_HB_KinhPhi_NgayThang_CheckMC = DateTime.Now.ToString("yyyy-MM-dd");
            model.Dkxt_HB_TrangThai_HoSo = entity.Dkxt_HB_TrangThai_HoSo;
            model.Dkxt_ThongBaoKiemDuyet_HoSo = entity.Dkxt_ThongBaoKiemDuyet_HoSo;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Dkxt_TuyenThang_Update_KiemDuyet(DangKyXetTuyenThang entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenThangs.Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();
            model.Dkxt_TrangThai_KinhPhi = entity.Dkxt_TrangThai_KinhPhi;
            model.Dkxt_KinhPhi_NgayThang_CheckMC = DateTime.Now.ToString("yyyy-MM-dd");
            model.Dkxt_TrangThai_HoSo = entity.Dkxt_TrangThai_HoSo;
            model.Dkxt_ThongBaoKiemDuyet_HoSo = entity.Dkxt_ThongBaoKiemDuyet_HoSo;
            db.SaveChanges();
            return Json(new { success = true}, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Dkxt_DGNL_NN_Update_KiemDuyet(DangKyXetTuyenKhac entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyXetTuyenKhacs.Where(x => x.Dkxt_ID == entity.Dkxt_ID).FirstOrDefault();
            model.Dkxt_TrangThai_KinhPhi = entity.Dkxt_TrangThai_KinhPhi;
            model.Dkxt_KinhPhi_NgayThang_CheckMC = DateTime.Now.ToString("yyyy-MM-dd");
            model.Dkxt_TrangThai_HoSo = entity.Dkxt_TrangThai_HoSo;
            model.Dkxt_ThongBaoKiemDuyet_HoSo = entity.Dkxt_ThongBaoKiemDuyet_HoSo;
            db.SaveChanges();
            return Json(new { success = true}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Dkdt_NK_Update_KiemDuyet(DangKyDuThiNangKhieu entity)
        {
            db = new DbConnecttion();
            var model = db.DangKyDuThiNangKhieus.Where(x => x.Dkdt_NK_ID == entity.Dkdt_NK_ID).FirstOrDefault();
            model.Dkdt_TrangThai_HoSo = entity.Dkdt_TrangThai_HoSo;
            model.Dkdt_TrangThai_LePhi = entity.Dkdt_TrangThai_LePhi;
            model.Dkdt_NgayThang_CheckHoSo = DateTime.Now.ToString("yyyy-MM-dd");
            model.Dkdt_NgayThang_CheckLePhi = DateTime.Now.ToString("yyyy-MM-dd");
            model.Dkdt_ThongBaoKiemDuyet_HoSo = entity.Dkdt_ThongBaoKiemDuyet_HoSo;
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
