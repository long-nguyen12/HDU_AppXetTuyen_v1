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
using PagedList;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class StatusTracking
    {
        public int st_ID { get; set; }
        public string st_Name { get; set; }
    }
    public class DangKyXetTuyensController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        public ActionResult Dkxtth()
        {
            return View();
        }
        // GET: Admin/DangKyXetTuyens
        [AdminSessionCheck]
        public ActionResult DkxtKQTthpt(string filteriDotxt, string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            var model = (from ts in db.DangKyXetTuyenKQTQGs select ts)
                                            .OrderBy(x => x.ThiSinh_ID)
                                            .ThenBy(x => x.Dkxt_KQTQG_NguyenVong)
                                            .ThenBy(x => x.Nganh.NganhTenNganh)
                                            .Include(x => x.ThiSinhDangKy)
                                            .Include(x => x.Nganh)
                                            .Include(x => x.ToHopMon)
                                            .Include(x => x.DotXetTuyen)
                                            .Include(x => x.ThiSinhDangKy.DoiTuong)
                                            .Include(x => x.ThiSinhDangKy.KhuVuc)
                                            .Include(x => x.PhuongThucXetTuyen);



            ViewBag.filteriNam = db.NamHocs.Where(x => x.NamHoc_TrangThai == 1).FirstOrDefault().NamHoc_Ten;
            #region lọc dữ liệu theo đợt xét tuyển
            var dotxts = db.DotXetTuyens.Include(x => x.NamHoc).Where(x => x.NamHoc.NamHoc_TrangThai == 1).ToList();
            dotxts.Add(new DotXetTuyen() { Dxt_ID = 0, Dxt_Ten = "Tất cả" });

            int _dotxt_hientai = dotxts.Where(x => x.Dxt_TrangThai == 1).FirstOrDefault().Dxt_ID; // nếu không có truyền vào thì gán giá trị cho đợt xét tuyển là hiện tại
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

            #region lọc dữ liệu theo nguyện vọng
            var list_items_nv = (from item in model select (new { value_item = item.Dkxt_KQTQG_NguyenVong.ToString(), text_item = item.Dkxt_KQTQG_NguyenVong.ToString() })).Distinct().ToList();
            ViewBag.filteriNvong = new SelectList(list_items_nv.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            // thực hiện lọc theo nguyện vọng
            if (!String.IsNullOrEmpty(filteriNvong))
            {
                int _dkxt_KQTQG_NguyenVong = Int32.Parse(filteriNvong);
                model = model.Where(x => x.Dkxt_KQTQG_NguyenVong == _dkxt_KQTQG_NguyenVong);
            }
            #endregion

            #region lọc dữ liệu theo ngành
            var list_items_nganh = (from item in model select (new { value_item = item.Nganh.Nganh_ID, text_item = item.Nganh.NganhTenNganh })).Distinct().ToList();
            ViewBag.filteriNganh = new SelectList(list_items_nganh.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            if (!String.IsNullOrEmpty(filteriNganh))
            {
                int _dkxt_KQTQG_Nganh_ID = Int32.Parse(filteriNganh);
                model = model.Where(x => x.Nganh_ID == _dkxt_KQTQG_Nganh_ID);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in model select item.Dkxt_KQTQG_TrangThai).Distinct().ToList();
            List<StatusTracking> filteri_items_lephi = new List<StatusTracking>();

            foreach (var _item in list_items_lephi)
            {
                if (_item == 0)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 0, st_Name = "Chưa đóng phí" });
                }
                if (_item == 1)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 1, st_Name = "Đã nộp lệ phí" });
                }
                if (_item == 2)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 2, st_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.st_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int _dkxt_KQTQG_TrangThai = Int32.Parse(filteriLePhi);
                model = model.Where(x => x.Dkxt_KQTQG_TrangThai == _dkxt_KQTQG_TrangThai);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in model select item.Dkxt_KQTQG_TrangThai_KetQua).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 0, st_Name = "Chưa kiểm tra" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 1, st_Name = "Minh chứng sai" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 2, st_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.st_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _dkxt_KQTQG_TrangThai_KetQua = Int32.Parse(filteriHoSo);
                model = model.Where(x => x.Dkxt_KQTQG_TrangThai_KetQua == _dkxt_KQTQG_TrangThai_KetQua);
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

        [AdminSessionCheck]
        public ActionResult DkxtHocBa(string filteriDotxt, string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
            var model = (from item in db.DangKyXetTuyens select item)
                                                  .OrderBy(x => x.ThiSinh_ID)
                                                  .ThenBy(x => x.Dkxt_NguyenVong)
                                                  .ThenBy(x => x.Nganh.NganhTenNganh)
                                                  .Include(x => x.ThiSinhDangKy)
                                                  .Include(x => x.Nganh)
                                                  .Include(x => x.ToHopMon)
                                                  .Include(x => x.DotXetTuyen)
                                                  .Include(x => x.DoiTuong)
                                                  .Include(x => x.KhuVuc)
                                                  .Include(x => x.PhuongThucXetTuyen);
            ViewBag.filteriNam = db.NamHocs.Where(x => x.NamHoc_TrangThai == 1).FirstOrDefault().NamHoc_Ten;
            #region lọc dữ liệu theo đợt
            var dotxts = db.DotXetTuyens.Include(x => x.NamHoc).Where(x => x.NamHoc.NamHoc_TrangThai == 1).ToList();
            dotxts.Add(new DotXetTuyen() { Dxt_ID = 0, Dxt_Ten = "Tất cả" });
            int _dotxt_hientai = dotxts.Where(x => x.Dxt_TrangThai == 1).FirstOrDefault().Dxt_ID;
            ViewBag.filteriDotxt = new SelectList(dotxts.OrderBy(x => x.Dxt_ID).ToList(), "Dxt_ID", "Dxt_Ten", _dotxt_hientai);
            // nếu không có truyền vào thì gán giá trị cho đợt xét tuyển là hiện tại
            if (String.IsNullOrEmpty(filteriDotxt) == true)
            {
                filteriDotxt = dotxts.Where(x => x.Dxt_TrangThai == 1).FirstOrDefault().Dxt_ID.ToString();
            }
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
            var list_items_nganh = (from item in model select (new { value_item = item.Nganh.Nganh_ID, text_item = item.Nganh.NganhTenNganh })).Distinct().ToList();
            ViewBag.filteriNganh = new SelectList(list_items_nganh.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            if (!String.IsNullOrEmpty(filteriNganh))
            {
                int _dkxt_KQTQG_Nganh_ID = Int32.Parse(filteriNganh);
                model = model.Where(x => x.Nganh_ID == _dkxt_KQTQG_Nganh_ID);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in model select item.Dkxt_TrangThai).Distinct().ToList();
            List<StatusTracking> filteri_items_lephi = new List<StatusTracking>();

            foreach (var _item in list_items_lephi)
            {
                if (_item == 0)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 0, st_Name = "Chưa đóng phí" });
                }
                if (_item == 1)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 1, st_Name = "Đã nộp lệ phí" });
                }
                if (_item == 2)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 2, st_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.st_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int _dkxt_TrangThai = Int32.Parse(filteriLePhi);
                model = model.Where(x => x.Dkxt_TrangThai == _dkxt_TrangThai);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in model select item.Dkxt_TrangThai_KetQua).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 0, st_Name = "Chưa kiểm tra" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 1, st_Name = "Minh chứng sai" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 2, st_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.st_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _dkxt_TrangThai_KetQua = Int32.Parse(filteriHoSo);
                model = model.Where(x => x.Dkxt_TrangThai_KetQua == _dkxt_TrangThai_KetQua);
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
                                      || m.Dkxt_Diem_Tong_Full.Contains(searchString));
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
        //[AdminSessionCheck]
       
        public ActionResult DkxtTt(string filteriDotxt, string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
           
            var model = (from item in db.DangKyXetTuyenThangs select item)
                                        .OrderBy(x => x.ThiSinh_ID)
                                        .ThenBy(x => x.Dkxt_NguyenVong)
                                        .ThenBy(x => x.Nganh.NganhTenNganh)
                                        .Include(x => x.ThiSinhDangKy)
                                        .Include(x => x.Nganh)                                       
                                        .Include(x => x.ThiSinhDangKy.DoiTuong)                                       
                                        .Include(x => x.ThiSinhDangKy.KhuVuc);

            ViewBag.filteriNam = db.NamHocs.Where(x => x.NamHoc_TrangThai == 1).FirstOrDefault().NamHoc_Ten;
            #region lọc dữ liệu theo đợt
            var dotxts = db.DotXetTuyens.Include(x => x.NamHoc).Where(x => x.NamHoc.NamHoc_TrangThai == 1).ToList();
            dotxts.Add(new DotXetTuyen() { Dxt_ID = 0, Dxt_Ten = "Tất cả" });
            int _dotxt_hientai = dotxts.Where(x => x.Dxt_TrangThai == 1).FirstOrDefault().Dxt_ID;
            ViewBag.filteriDotxt = new SelectList(dotxts.OrderBy(x => x.Dxt_ID).ToList(), "Dxt_ID", "Dxt_Ten", _dotxt_hientai);
            // nếu không có truyền vào thì gán giá trị cho đợt xét tuyển là hiện tại
            if (String.IsNullOrEmpty(filteriDotxt) == true)
            {
                filteriDotxt = dotxts.Where(x => x.Dxt_TrangThai == 1).FirstOrDefault().Dxt_ID.ToString();
            }
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
            var list_items_nganh = (from item in model select (new { value_item = item.Nganh.Nganh_ID, text_item = item.Nganh.NganhTenNganh })).Distinct().ToList();
            ViewBag.filteriNganh = new SelectList(list_items_nganh.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            if (!String.IsNullOrEmpty(filteriNganh))
            {
                int _dkxt_TT_Nganh_ID = Int32.Parse(filteriNganh);
                model = model.Where(x => x.Nganh_ID == _dkxt_TT_Nganh_ID);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in model select item.Dkxt_TrangThai).Distinct().ToList();
            List<StatusTracking> filteri_items_lephi = new List<StatusTracking>();

            foreach (var _item in list_items_lephi)
            {
                if (_item == 0)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 0, st_Name = "Chưa đóng phí" });
                }
                if (_item == 1)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 1, st_Name = "Đã nộp lệ phí" });
                }
                if (_item == 2)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 2, st_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.st_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int _dkxt_TrangThai = Int32.Parse(filteriLePhi);
                model = model.Where(x => x.Dkxt_TrangThai == _dkxt_TrangThai);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in model select item.Dkxt_TrangThai_KetQua).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 0, st_Name = "Chưa kiểm tra" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 1, st_Name = "Minh chứng sai" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 2, st_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.st_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _dkxt_TrangThai_KetQua = Int32.Parse(filteriHoSo);
                model = model.Where(x => x.Dkxt_TrangThai_KetQua == _dkxt_TrangThai_KetQua);
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


        public ActionResult Dkxtccnn(int? page)
        {
            if (page == null) page = 1;
            var dangKyXetTuyens = (from h in db.DangKyXetTuyens
                                   select h).OrderBy(x => x.ThiSinh_ID).ThenBy(x => x.Dkxt_NguyenVong).ThenBy(x => x.Nganh.NganhTenNganh).Include(d => d.DoiTuong).Include(d => d.DotXetTuyen).Include(d => d.KhuVuc).Include(d => d.Nganh).Include(d => d.PhuongThucXetTuyen).Include(d => d.ThiSinhDangKy).Include(d => d.ToHopMon);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(dangKyXetTuyens.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DkxtkqDgnl(int? page)
        {
            if (page == null) page = 1;
            var dangKyXetTuyens = (from h in db.DangKyXetTuyens
                                   select h).OrderBy(x => x.ThiSinh_ID).ThenBy(x => x.Dkxt_NguyenVong).ThenBy(x => x.Nganh.NganhTenNganh).Include(d => d.DoiTuong).Include(d => d.DotXetTuyen).Include(d => d.KhuVuc).Include(d => d.Nganh).Include(d => d.PhuongThucXetTuyen).Include(d => d.ThiSinhDangKy).Include(d => d.ToHopMon);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(dangKyXetTuyens.ToPagedList(pageNumber, pageSize));
        }
        [AdminSessionCheck]
        public ActionResult Dkdttnk(string filteriDotxt, string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {
        //    public ActionResult Dkdttnk(string searchString, string currentFilter, string filteriDotxt, int? page)
        //{
        

            var model = (from item in db.DangKyDuThiNangKhieus select item)
                                        .OrderBy(x => x.ThiSinh_ID)
                                        .ThenBy(x => x.Dkdt_NK_NguyenVong)
                                        .ThenBy(x => x.Nganh.NganhTenNganh)
                                        .Include(x => x.ThiSinhDangKy)
                                        .Include(x => x.Nganh)
                                        .Include(x => x.ToHopMon)
                                        .Include(x => x.ThiSinhDangKy.DoiTuong)
                                        .Include(x => x.ThiSinhDangKy.KhuVuc)                                      
                                        .Include(x => x.PhuongThucXetTuyen);
           
            
            // lấy thông tin năm hiện tại
            ViewBag.filteriNam = db.NamHocs.Where(x => x.NamHoc_TrangThai == 1).FirstOrDefault().NamHoc_Ten;
            #region lọc dữ liệu theo đợt
            var dotxts = db.DotXetTuyens.Include(x => x.NamHoc).Where(x => x.NamHoc.NamHoc_TrangThai == 1).ToList();
            dotxts.Add(new DotXetTuyen() { Dxt_ID = 0, Dxt_Ten = "Tất cả" });
            int _dotxt_hientai = dotxts.Where(x => x.Dxt_TrangThai == 1).FirstOrDefault().Dxt_ID;
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

            #region lọc dữ liệu theo nguyện vọng
            var list_items_nv = (from item in model select (new { value_item = item.Dkdt_NK_NguyenVong.ToString(), text_item = item.Dkdt_NK_NguyenVong.ToString() })).Distinct().ToList();
            ViewBag.filteriNvong = new SelectList(list_items_nv.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            // thực hiện lọc theo nguyện vọng
            if (!String.IsNullOrEmpty(filteriNvong))
            {
                int dkdt_NK_NguyenVong = Int32.Parse(filteriNvong);
                model = model.Where(x => x.Dkdt_NK_NguyenVong == dkdt_NK_NguyenVong);
            }
            #endregion

            #region lọc dữ liệu theo ngành
            var list_items_nganh = (from item in model select (new { value_item = item.Nganh.Nganh_ID, text_item = item.Nganh.NganhTenNganh })).Distinct().ToList();
            ViewBag.filteriNganh = new SelectList(list_items_nganh.OrderBy(x => x.value_item).ToList(), "value_item", "text_item");
            if (!String.IsNullOrEmpty(filteriNganh))
            {
                int _dkdt_NK_Nganh_ID = Int32.Parse(filteriNganh);
                model = model.Where(x => x.Nganh_ID == _dkdt_NK_Nganh_ID);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi lệ phí
            var list_items_lephi = (from item in model select item.Dkdt_NK_TrangThai).Distinct().ToList();
            List<StatusTracking> filteri_items_lephi = new List<StatusTracking>();

            foreach (var _item in list_items_lephi)
            {
                if (_item == 0)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 0, st_Name = "Chưa đóng phí" });
                }
                if (_item == 1)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 1, st_Name = "Đã nộp lệ phí" });
                }
                if (_item == 2)
                {
                    filteri_items_lephi.Add(new StatusTracking() { st_ID = 2, st_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.st_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int _dkdt_nk_TrangThai = Int32.Parse(filteriLePhi);
                model = model.Where(x => x.Dkdt_NK_TrangThai == _dkdt_nk_TrangThai);
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in model select item.Dkdt_NK_TrangThai_KetQua).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 0, st_Name = "Chưa kiểm tra" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 1, st_Name = "Minh chứng sai" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { st_ID = 2, st_Name = "Đã kiểm duyệt" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.st_ID).ToList(), "st_ID", "st_Name");
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _dkdt_NK_TrangThai_KetQua = Int32.Parse(filteriHoSo);
                model = model.Where(x => x.Dkdt_NK_TrangThai_KetQua == _dkdt_NK_TrangThai_KetQua);
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
                                      || m.Nganh.NganhTenNganh.Contains(searchString)
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
        #region

        // GET: Admin/DangKyXetTuyens/Details/5
        [AdminSessionCheck]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyXetTuyen dangKyXetTuyen = db.DangKyXetTuyens.Find(id);
            if (dangKyXetTuyen == null)
            {
                return HttpNotFound();
            }
            return View(dangKyXetTuyen);
        }

        // GET: Admin/DangKyXetTuyens/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten");
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten");
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten");
            ViewBag.Nganh_ID = new SelectList(db.Nganhs, "Nganh_ID", "Nganh_MaNganh");
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc");
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD");
            ViewBag.Thm_ID = new SelectList(db.ToHopMons, "Thm_ID", "Thm_MaToHop");
            return View();
        }

        // POST: Admin/DangKyXetTuyens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Dkxt_ID,ThiSinh_ID,Ptxt_ID,Nganh_ID,Thm_ID,DoiTuong_ID,KhuVuc_ID,Dkxt_TrangThai,Dkxt_NguyenVong,DotXT_ID,Dkxt_GhiChu,Dkxt_Diem_M1,Dkxt_Diem_M2,Dkxt_Diem_M3,Dkxt_Diem_Tong,Dkxt_Diem_Tong_Full")] DangKyXetTuyen dangKyXetTuyen)
        {
            if (ModelState.IsValid)
            {
                db.DangKyXetTuyens.Add(dangKyXetTuyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", dangKyXetTuyen.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", dangKyXetTuyen.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", dangKyXetTuyen.KhuVuc_ID);
            ViewBag.Nganh_ID = new SelectList(db.Nganhs, "Nganh_ID", "Nganh_MaNganh", dangKyXetTuyen.Nganh_ID);
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc", dangKyXetTuyen.Ptxt_ID);
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", dangKyXetTuyen.ThiSinh_ID);
            ViewBag.Thm_ID = new SelectList(db.ToHopMons, "Thm_ID", "Thm_MaToHop", dangKyXetTuyen.Thm_ID);
            return View(dangKyXetTuyen);
        }

        // GET: Admin/DangKyXetTuyens/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyXetTuyen dangKyXetTuyen = db.DangKyXetTuyens.Find(id);
            if (dangKyXetTuyen == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", dangKyXetTuyen.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", dangKyXetTuyen.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", dangKyXetTuyen.KhuVuc_ID);
            ViewBag.Nganh_ID = new SelectList(db.Nganhs, "Nganh_ID", "Nganh_MaNganh", dangKyXetTuyen.Nganh_ID);
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc", dangKyXetTuyen.Ptxt_ID);
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", dangKyXetTuyen.ThiSinh_ID);
            ViewBag.Thm_ID = new SelectList(db.ToHopMons, "Thm_ID", "Thm_MaToHop", dangKyXetTuyen.Thm_ID);
            return View(dangKyXetTuyen);
        }

        // POST: Admin/DangKyXetTuyens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Dkxt_ID,ThiSinh_ID,Ptxt_ID,Nganh_ID,Thm_ID,DoiTuong_ID,KhuVuc_ID,Dkxt_TrangThai,Dkxt_NguyenVong,DotXT_ID,Dkxt_GhiChu,Dkxt_Diem_M1,Dkxt_Diem_M2,Dkxt_Diem_M3,Dkxt_Diem_Tong,Dkxt_Diem_Tong_Full")] DangKyXetTuyen dangKyXetTuyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dangKyXetTuyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", dangKyXetTuyen.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", dangKyXetTuyen.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", dangKyXetTuyen.KhuVuc_ID);
            ViewBag.Nganh_ID = new SelectList(db.Nganhs, "Nganh_ID", "Nganh_MaNganh", dangKyXetTuyen.Nganh_ID);
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc", dangKyXetTuyen.Ptxt_ID);
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", dangKyXetTuyen.ThiSinh_ID);
            ViewBag.Thm_ID = new SelectList(db.ToHopMons, "Thm_ID", "Thm_MaToHop", dangKyXetTuyen.Thm_ID);
            return View(dangKyXetTuyen);
        }

        // GET: Admin/DangKyXetTuyens/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyXetTuyen dangKyXetTuyen = db.DangKyXetTuyens.Find(id);
            if (dangKyXetTuyen == null)
            {
                return HttpNotFound();
            }
            return View(dangKyXetTuyen);
        }

        // POST: Admin/DangKyXetTuyens/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DangKyXetTuyen dangKyXetTuyen = db.DangKyXetTuyens.Find(id);
            db.DangKyXetTuyens.Remove(dangKyXetTuyen);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
