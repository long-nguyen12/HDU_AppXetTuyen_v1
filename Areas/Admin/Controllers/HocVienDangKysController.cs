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

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class HocVienDangKysController : Controller
    {
        private DbConnecttion db = new DbConnecttion();
        public ActionResult testMtDR()
        {
            return View();  
        }
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

        protected IList<HocVienDuTuyen> ListHvDuTuyenExport;
        // GET: Admin/HocVienDangKys
        public ActionResult DsHvDuTuyen(string filteriLePhi, string filteriHoSo, string searchString, string currentFilter, string filteriDotxt, int? page)
        {
            var hocviens = db.HocVienDuTuyens.Include(h => h.DotXetTuyen).Include(h => h.HocVienDangKy).Include(h => h.NganhMaster).ToList();


            #region lọc dữ liệu theo đợt
            var dotxts = db.DotXetTuyens.Include(x => x.NamHoc).Where(x => x.NamHoc.NamHoc_TrangThai == 1 && x.Dxt_Classify == 2).ToList();
            dotxts.Add(new DotXetTuyen() { Dxt_ID = 0, Dxt_Ten = "Tất cả" });
            int _dotxt_hientai = dotxts.Where(x => x.Dxt_TrangThai_Xt == 1 && x.Dxt_Classify == 2).FirstOrDefault().Dxt_ID;
            ViewBag.filteriDotxt = new SelectList(dotxts.OrderBy(x => x.Dxt_ID).ToList(), "Dxt_ID", "Dxt_Ten", _dotxt_hientai);
            // nếu không có truyền vào thì gán giá trị cho đợt xét tuyển là hiện tại
            if (String.IsNullOrEmpty(filteriDotxt) == true)
            {
                filteriDotxt = dotxts.Where(x => x.Dxt_TrangThai_Xt == 1).FirstOrDefault().Dxt_ID.ToString();
            }
            // thực hiện lọc 

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
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã kiểm duyệt" });
                }
                if (_item == 3)
                {
                    filteri_items_lephi.Add(new StatusTracking() { St_ID = 3, St_Name = "Thông tin lệ phí sai" });
                }
            }
            ViewBag.filteriLePhi = new SelectList(filteri_items_lephi.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int _dkxt_TrangThai = Int32.Parse(filteriLePhi);
                hocviens = hocviens.Where(x => x.HocVien_LePhi_TrangThai ==  _dkxt_TrangThai).ToList();
            }
            #endregion

            #region lọc dữ liệu theo trạng thái theo dõi hồ sơ
            var list_items_hoso = (from item in hocviens select item.DuTuyen_TrangThai).Distinct().ToList();
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
                int _dkxt_TrangThai_KetQua = Int32.Parse(filteriHoSo);
                hocviens = hocviens.Where(x => x.DuTuyen_TrangThai == _dkxt_TrangThai_KetQua).ToList();
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

            ViewBag.totalRecod = hocviens.Count();
            ListHvDuTuyenExport = hocviens.ToList();
            #endregion
            return View(hocviens.ToPagedList(pageNumber, pageSize));
        }

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
        public ActionResult QLDotDuTuyenSDH_Add()
        {
            var model = db.DotXetTuyens.OrderByDescending(x => x.Dxt_ID).Where(x => x.Dxt_Classify == 2);

            return View(model.ToList());
        }
        public ActionResult QLDotDuTuyenSDH_Update()
        {
            var model = db.DotXetTuyens.OrderByDescending(x => x.Dxt_ID).Where(x => x.Dxt_Classify == 2);

            return View(model.ToList());
        }
        public ActionResult QLDotDuTuyenSDH_Delete()
        {
            var model = db.DotXetTuyens.OrderByDescending(x => x.Dxt_ID).Where(x => x.Dxt_Classify == 2);

            return View(model.ToList());
        }
    }
}