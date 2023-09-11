using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HDU_AppXetTuyen.Models;
using System.Data.Entity;
using Newtonsoft.Json;

using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Xceed.Words.NET;
using System.Net.Mail;
using System.Net;
using System.Xml.Linq;
using System.Data;



namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class ExportFilteriData
    {
        public string FilteriDotxt { get; set; }
        public string SearchString { get; set; }
    }
    public class ExportDatasController : Controller
    {
        // GET: Admin/ExportDatas
        private DbConnecttion db = new DbConnecttion();
        public void ExportTsDkXetHocBa()
        {
            var model = (from item in db.DangKyXetTuyenHBs select item)
                                                 .OrderBy(x => x.ThiSinh_ID)
                                                 .ThenBy(x => x.Dkxt_HB_NguyenVong)
                                                 .ThenBy(x => x.Nganh.Nganh_TenNganh)
                                                 .Include(x => x.ThiSinhDangKy)
                                                 .Include(x => x.Nganh)
                                                 .Include(x => x.ToHopMon)
                                                 .Include(x => x.DotXetTuyen)
                                                 .Include(x => x.ThiSinhDangKy.DoiTuong)
                                                 .Include(x => x.ThiSinhDangKy.KhuVuc)
                                                 .Include(x => x.PhuongThucXetTuyen).ToList();

            var dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_Classify == 0 && d.Dxt_TrangThai_Xt == 1);
            try
            {
                using (ExcelPackage _excelpackage = new ExcelPackage())
                {
                    _excelpackage.Workbook.Properties.Author = "208Team";  // đặt tên người tạo file                       
                    _excelpackage.Workbook.Properties.Title = "DStsdt"; // đặt tiêu đề cho file                    

                    //Tạo sheet để làm việc 
                    _excelpackage.Workbook.Worksheets.Add("ThongKeThisinhDK");
                    string[] arr_col_number = { "TT", "Họ, tên đệm", "Tên", "Ngày sinh", "Nguyện vọng", "Mã ngành", "Tên ngành đăng ký", "Tổ hợp môn",  "Tổng điểm 3 môn", "Tổng điểm có ƯT",
                        "Điện thoại", "Email", "Hộ khẩu thường trú", "Địa chỉ nhận giấy báo" };

                    ExcelWorksheet ws = null; // khai báo để thao tác với ws
                    // lấy sheet vừa add ra để thao tác 
                    if (model.Count > 0)
                    {
                        ws = _excelpackage.Workbook.Worksheets[1];

                        ws.Name = "ThongKeThisinhDK";  // đặt tên cho sheet                       
                        ws.Cells.Style.Font.Size = 12;  // fontsize mặc định cho cả sheet                       
                        ws.Cells.Style.Font.Name = "Times New Roman"; // font family mặc định cho cả sheet

                        ws.Cells[1, 1].Value = "DANH SÁCH THÍ SINH ĐĂNG KÝ DỰ THI NĂNG KHIẾU";
                        ws.Cells[1, 1, 1, 14].Merge = true;
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[2, 1].Value = "Số liệu thống kê dự tuyển " + dxt_hientai.Dxt_Ten + ", Từ ngày " + dxt_hientai.Dxt_ThoiGian_BatDau + " đến ngày " + dxt_hientai.Dxt_ThoiGian_KetThuc;
                        ws.Cells[2, 1, 2, 14].Merge = true;
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
                        foreach (var item in model)
                        {
                            colIndex = 1; // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                            rowIndex++;  // rowIndex tương ứng từng dòng dữ liệu
                            //gán giá trị cho từng cell                      
                            ws.Cells.Style.Font.Bold = false;
                            ws.Cells.Style.WrapText = true;
                            ws.Cells[rowIndex, colIndex++].Value = (rowIndex - 3);                          //  1 số thư tự 
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_HoLot;        //  2 
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_Ten;          //  3
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_NgaySinh;     //  4
                            ws.Cells[rowIndex, colIndex++].Value = item.Dkxt_HB_NguyenVong;                // 5

                            ws.Cells[rowIndex, colIndex++].Value = item.Nganh.Nganh_MaNganh;       //  6
                            ws.Cells[rowIndex, colIndex++].Value = item.Nganh.Nganh_TenNganh;      //  7
                            ws.Cells[rowIndex, colIndex++].Value = item.ToHopMon.Thm_TenToHop;     //  8
                            ws.Cells[rowIndex, colIndex++].Value = item.Dkxt_HB_Diem_Tong;          // 9
                            ws.Cells[rowIndex, colIndex++].Value = item.Dkxt_HB_Diem_Tong_Full;     // 10

                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_DienThoai;        //  11
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_Email;            //  12

                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_HoKhauThuongTru;      //  13
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_DCNhanGiayBao;     //  14

                            ws.Cells[rowIndex, colIndex++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        //{ "TT", "Họ, tên đệm", "Tên", "Ngày sinh", "Nguyện vọng", "Mã ngành", "Tên ngành đăng ký", "Tổ hợp môn",  "Tổng điểm 3 môn", "Tổng điểm có ƯT",
                        //"Điện thoại", "Email", "Hộ khẩu thường trú", "Địa chỉ nhận giấy báo" };

                        for (int indexCol = 1; indexCol <= arr_col_number.Count(); indexCol++)
                        {
                            if (indexCol == 1) { ws.Column(indexCol).Width = 6; }       //1
                            if (indexCol == 2) { ws.Column(indexCol).Width = 15; }      //2
                            if (indexCol == 3) { ws.Column(indexCol).Width = 7.3; }     //3
                            if (indexCol == 4) { ws.Column(indexCol).Width = 14.3; }    //4
                            if (indexCol == 5) { ws.Column(indexCol).Width = 13; }      //5
                            if (indexCol == 6) { ws.Column(indexCol).Width = 30; }      //6 
                            if (indexCol == 7) { ws.Column(indexCol).Width = 20; }      //7 
                            if (indexCol == 8) { ws.Column(indexCol).Width = 40; }      //8 
                            if (indexCol == 9) { ws.Column(indexCol).Width = 40; }      //9  
                            if (indexCol == 10) { ws.Column(indexCol).Width = 40; }     //10                      
                            if (indexCol == 11) { ws.Column(indexCol).Width = 40; }     //11 
                            if (indexCol == 12) { ws.Column(indexCol).Width = 40; }     //12 
                            if (indexCol == 13) { ws.Column(indexCol).Width = 40; }     //13 
                            if (indexCol == 14) { ws.Column(indexCol).Width = 40; }     //14

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
        public void ExportTsDKThiNangKhieu()
        {

            var ListTsDTNangKhieu = (from item in db.DangKyDuThiNangKhieus select item)
                                        .OrderBy(x => x.ThiSinh_ID)
                                        .ThenBy(x => x.Dkdt_NK_NguyenVong)
                                        .ThenBy(x => x.Nganh.Nganh_TenNganh)
                                        .Include(x => x.ThiSinhDangKy)
                                        .Include(x => x.Nganh)
                                        .Include(x => x.ToHopMon)
                                        .Include(x => x.ThiSinhDangKy.DoiTuong)
                                        .Include(x => x.ThiSinhDangKy.KhuVuc).ToList();

            var dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_Classify == 1 && d.Dxt_TrangThai_Xt == 1);
            try
            {
                using (ExcelPackage _excelpackage = new ExcelPackage())
                {
                    _excelpackage.Workbook.Properties.Author = "208Team";  // đặt tên người tạo file                       
                    _excelpackage.Workbook.Properties.Title = "DStsdt"; // đặt tiêu đề cho file                    

                    //Tạo sheet để làm việc 
                    _excelpackage.Workbook.Worksheets.Add("ThongKeThisinhDK");
                    string[] arr_col_number = { "TT", "Họ, tên đệm", "Tên", "Ngày sinh", "Mã ngành", "Tên ngành đăng ký",
                        "Điện thoại","Email" , "Hộ khẩu thường trú", "Địa chỉ nhận giấy báo"};

                    ExcelWorksheet ws = null; // khai báo để thao tác với ws

                    // lấy sheet vừa add ra để thao tác 

                    if (ListTsDTNangKhieu.Count > 0)
                    {
                        ws = _excelpackage.Workbook.Worksheets[1];

                        ws.Name = "ThongKeThisinhDK";  // đặt tên cho sheet                       
                        ws.Cells.Style.Font.Size = 12;  // fontsize mặc định cho cả sheet                       
                        ws.Cells.Style.Font.Name = "Times New Roman"; // font family mặc định cho cả sheet

                        ws.Cells[1, 1].Value = "DANH SÁCH THÍ SINH ĐĂNG KÝ DỰ THI NĂNG KHIẾU";
                        ws.Cells[1, 1, 1, 10].Merge = true;
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[2, 1].Value = "Số liệu thống kê dự tuyển " + dxt_hientai.Dxt_Ten + ", Từ ngày " + dxt_hientai.Dxt_ThoiGian_BatDau + " đến ngày " + dxt_hientai.Dxt_ThoiGian_KetThuc;
                        ws.Cells[2, 1, 2, 10].Merge = true;
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
                        foreach (var item in ListTsDTNangKhieu)
                        {
                            colIndex = 1; // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                            rowIndex++;  // rowIndex tương ứng từng dòng dữ liệu
                            //gán giá trị cho từng cell                      
                            ws.Cells.Style.Font.Bold = false;
                            ws.Cells.Style.WrapText = true;
                            ws.Cells[rowIndex, colIndex++].Value = (rowIndex - 3);                          //  1 số thư tự 
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_HoLot;        //  2 
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_Ten;          //  3
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_NgaySinh;     //  4

                            ws.Cells[rowIndex, colIndex++].Value = item.Nganh.Nganh_MaNganh;       //  5
                            ws.Cells[rowIndex, colIndex++].Value = item.Nganh.Nganh_TenNganh;      //  6

                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_DienThoai;        //  7
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_Email;            //  8

                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_HoKhauThuongTru;      //  9
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_DCNhanGiayBao;     //  10

                            ws.Cells[rowIndex, colIndex++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        for (int indexCol = 1; indexCol <= arr_col_number.Count(); indexCol++)
                        {
                            if (indexCol == 1) { ws.Column(indexCol).Width = 6; }       //1
                            if (indexCol == 2) { ws.Column(indexCol).Width = 15; }      //2
                            if (indexCol == 3) { ws.Column(indexCol).Width = 7.3; }     //3
                            if (indexCol == 4) { ws.Column(indexCol).Width = 14.3; }    //4
                            if (indexCol == 5) { ws.Column(indexCol).Width = 13; }      //5
                            if (indexCol == 6) { ws.Column(indexCol).Width = 30; }      //6 
                            if (indexCol == 7) { ws.Column(indexCol).Width = 20; }      //7 
                            if (indexCol == 8) { ws.Column(indexCol).Width = 40; }      //8 
                            if (indexCol == 9) { ws.Column(indexCol).Width = 40; }      //9  
                            if (indexCol == 10) { ws.Column(indexCol).Width = 40; }     //10                        
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
        public void ExportHvDKDuTuyen()
        {
            var ListHvDts = db.HocVienDuTuyens.Include(h => h.DotXetTuyen).Include(h => h.HocVienDangKy).Include(h => h.NganhMaster).ToList();
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
        public JsonResult ExportHvDKDuTuyen2(ExportFilteriData entity)
        {

            string _FilteriDotxt = entity.FilteriDotxt;
            string _SearchString = entity.SearchString;
            DotXetTuyen dxt_hientai = null;


            var ListHvDts = db.HocVienDuTuyens.Include(h => h.DotXetTuyen).Include(h => h.HocVienDangKy).Include(h => h.NganhMaster).ToList();
            if (!String.IsNullOrEmpty(_SearchString))
            {
                ListHvDts = ListHvDts.Where(h => h.HocVienDangKy.HocVien_Ten.ToUpper().Contains(_SearchString.ToUpper())
                                   || h.HocVienDangKy.HocVien_HoDem.ToUpper().Contains(_SearchString.ToUpper())
                                   || h.HocVienDangKy.HocVien_CCCD.Contains(_SearchString)
                                   || h.HocVienDangKy.HocVien_DienThoai.Contains(_SearchString)).ToList();
            }
            if (!String.IsNullOrEmpty(_FilteriDotxt.ToString()))
            {
                ListHvDts = ListHvDts.Where(x => x.Dxt_ID == int.Parse(_FilteriDotxt)).ToList();

                dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_ID == int.Parse(_FilteriDotxt));
            }
            else
            {
                dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_Classify == 2 && d.Dxt_TrangThai_Xt == 1);
            }

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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public void ExportTsDkxtKQTthpt()
        {
            var model = (from item in db.DangKyXetTuyenKQTQGs select item)
                                                 .OrderBy(x => x.ThiSinh_ID)
                                                 .ThenBy(x => x.Dkxt_KQTQG_NguyenVong)
                                                 .ThenBy(x => x.Nganh.Nganh_TenNganh)
                                                 .Include(x => x.ThiSinhDangKy)
                                                 .Include(x => x.Nganh)
                                                 .Include(x => x.ToHopMon)
                                                 .Include(x => x.DotXetTuyen)
                                                 .Include(x => x.ThiSinhDangKy.DoiTuong)
                                                 .Include(x => x.ThiSinhDangKy.KhuVuc)
                                                 .Include(x => x.PhuongThucXetTuyen).ToList();

            var dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_Classify == 0 && d.Dxt_TrangThai_Xt == 1);
            try
            {
                using (ExcelPackage _excelpackage = new ExcelPackage())
                {
                    _excelpackage.Workbook.Properties.Author = "208Team";  // đặt tên người tạo file                       
                    _excelpackage.Workbook.Properties.Title = "DStsdt"; // đặt tiêu đề cho file                    

                    //Tạo sheet để làm việc 
                    _excelpackage.Workbook.Worksheets.Add("ThongKeThisinhDK");
                    string[] arr_col_number = { "TT", "Họ, tên đệm", "Tên", "Ngày sinh", "Nguyện vọng", "Mã ngành", "Tên ngành đăng ký", "Tổ hợp môn",  "Tổng điểm 3 môn", "Năm tốt nghiệp",
                        "Điện thoại", "Email", "Hộ khẩu thường trú", "Địa chỉ nhận giấy báo" };

                    ExcelWorksheet ws = null; // khai báo để thao tác với ws
                    // lấy sheet vừa add ra để thao tác 
                    if (model.Count > 0)
                    {
                        ws = _excelpackage.Workbook.Worksheets[1];

                        ws.Name = "ThongKeThisinh";  // đặt tên cho sheet                       
                        ws.Cells.Style.Font.Size = 12;  // fontsize mặc định cho cả sheet                       
                        ws.Cells.Style.Font.Name = "Times New Roman"; // font family mặc định cho cả sheet

                        ws.Cells[1, 1].Value = "DANH SÁCH THÍ SINH ĐĂNG KÝ XÉT TUYỂN SỬ DỤNG KẾT QUẢ THI TỐT NGHIỆP THPT";
                        ws.Cells[1, 1, 1, 14].Merge = true;
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[2, 1].Value = "Số liệu thống kê dự tuyển " + dxt_hientai.Dxt_Ten + ", Từ ngày " + dxt_hientai.Dxt_ThoiGian_BatDau + " đến ngày " + dxt_hientai.Dxt_ThoiGian_KetThuc;
                        ws.Cells[2, 1, 2, 14].Merge = true;
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
                        foreach (var item in model)
                        {
                            colIndex = 1; // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                            rowIndex++;  // rowIndex tương ứng từng dòng dữ liệu
                            //gán giá trị cho từng cell                      
                            ws.Cells.Style.Font.Bold = false;
                            ws.Cells.Style.WrapText = true;
                            ws.Cells[rowIndex, colIndex++].Value = (rowIndex - 3);                          //  1 số thư tự 
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_HoLot;        //  2 
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_Ten;          //  3
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_NgaySinh;     //  4
                            ws.Cells[rowIndex, colIndex++].Value = item.Dkxt_KQTQG_NguyenVong;                // 5

                            ws.Cells[rowIndex, colIndex++].Value = item.Nganh.Nganh_MaNganh;       //  6
                            ws.Cells[rowIndex, colIndex++].Value = item.Nganh.Nganh_TenNganh;      //  7
                            ws.Cells[rowIndex, colIndex++].Value = item.ToHopMon.Thm_TenToHop;     //  8
                            ws.Cells[rowIndex, colIndex++].Value = item.Dkxt_KQTQG_TongDiem_Full;          // 9
                            ws.Cells[rowIndex, colIndex++].Value = item.Dkxt_KQTQG_NamTotNghiep;     // 10

                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_DienThoai;        //  11
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_Email;            //  12

                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_HoKhauThuongTru;      //  13
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_DCNhanGiayBao;     //  14
                            ws.Cells[rowIndex, colIndex++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        //{"TT", "Họ, tên đệm", "Tên", "Ngày sinh", "Nguyện vọng", "Mã ngành", "Tên ngành đăng ký", "Tổ hợp môn",  "Tổng điểm 3 môn", "Năm tốt nghiệp",
                        //"Điện thoại", "Email", "Hộ khẩu thường trú", "Địa chỉ nhận giấy báo"}

                        for (int indexCol = 1; indexCol <= arr_col_number.Count(); indexCol++)
                        {
                            if (indexCol == 1) { ws.Column(indexCol).Width = 6; }       //1 STT
                            if (indexCol == 2) { ws.Column(indexCol).Width = 15; }      //2
                            if (indexCol == 3) { ws.Column(indexCol).Width = 7.3; }     //3
                            if (indexCol == 4) { ws.Column(indexCol).Width = 14.3; }    //4
                            if (indexCol == 5) { ws.Column(indexCol).Width = 13; }      //5
                            if (indexCol == 6) { ws.Column(indexCol).Width = 14.67; }      //6 
                            if (indexCol == 7) { ws.Column(indexCol).Width = 20; }      //7 
                            if (indexCol == 8) { ws.Column(indexCol).Width = 27; }      //8 
                            if (indexCol == 9) { ws.Column(indexCol).Width = 19.67; }      //9  
                            if (indexCol == 10) { ws.Column(indexCol).Width = 22; }     //10                      
                            if (indexCol == 11) { ws.Column(indexCol).Width = 22; }     //11 
                            if (indexCol == 12) { ws.Column(indexCol).Width = 30; }     //12 
                            if (indexCol == 13) { ws.Column(indexCol).Width = 40; }     //13 
                            if (indexCol == 14) { ws.Column(indexCol).Width = 40; }     //14

                            ws.Cells[3, 1, 3, indexCol].Style.Font.Bold = true;         // đặt tiêu đề cho bảng có kiểu chữ đậm
                        }
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[1, 1].Style.Font.Bold = true;
                        ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; ws.Cells[rowIndex, colIndex++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Column(10).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        //Lưu file lại  
                        string excelName = "DangKyXetTuyenSuDungKQThiTHPT";
                        using (var memoryStream = new MemoryStream())
                        {
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + excelName + ".xlsx"); // tên file lưu
                            _excelpackage.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                    //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public void ExportTsTuyenThang()
        {		

            var model = (from item in db.DangKyXetTuyenThangs select item)
                                                .OrderBy(x => x.ThiSinh_ID)
                                                .ThenBy(x => x.Dkxt_NguyenVong)
                                                .ThenBy(x => x.Nganh.Nganh_TenNganh)
                                                .Include(x => x.ThiSinhDangKy)
                                                .Include(x => x.Nganh)
                                                .Include(x => x.ThiSinhDangKy.DoiTuong)
                                                .Include(x => x.ThiSinhDangKy.KhuVuc).ToList();

            var dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_Classify == 0 && d.Dxt_TrangThai_Xt == 1);
            try
            {
                using (ExcelPackage _excelpackage = new ExcelPackage())
                {
                    _excelpackage.Workbook.Properties.Author = "208Team";  // đặt tên người tạo file                       
                    _excelpackage.Workbook.Properties.Title = "DStsdt"; // đặt tiêu đề cho file                    

                    //Tạo sheet để làm việc 
                    _excelpackage.Workbook.Worksheets.Add("ThongKeThisinhDK");
                    string[] arr_col_number = { "TT", "Họ, tên đệm", "Tên", "Ngày sinh", "Nguyện vọng", "Mã ngành", "Tên ngành đăng ký", "Môn đạt giải",  "Năm đạt giải", "Loại giải",
                        "Điện thoại", "Email", "Hộ khẩu thường trú", "Địa chỉ nhận giấy báo" };

                    ExcelWorksheet ws = null; // khai báo để thao tác với ws
                    // lấy sheet vừa add ra để thao tác 
                    if (model.Count > 0)
                    {
                        ws = _excelpackage.Workbook.Worksheets[1];

                        ws.Name = "ThongKeThisinh";  // đặt tên cho sheet                       
                        ws.Cells.Style.Font.Size = 12;  // fontsize mặc định cho cả sheet                       
                        ws.Cells.Style.Font.Name = "Times New Roman"; // font family mặc định cho cả sheet

                        ws.Cells[1, 1].Value = "DANH SÁCH THÍ SINH ĐĂNG KÝ XÉT TUYỂN THẲNG THEO QUY CHẾ CỦA NHÀ TRƯỜNG";
                        ws.Cells[1, 1, 1, 14].Merge = true;
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[2, 1].Value = "Số liệu thống kê dự tuyển " + dxt_hientai.Dxt_Ten + ", Từ ngày " + dxt_hientai.Dxt_ThoiGian_BatDau + " đến ngày " + dxt_hientai.Dxt_ThoiGian_KetThuc;
                        ws.Cells[2, 1, 2, 14].Merge = true;
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
                        foreach (var item in model)
                        {
                            colIndex = 1; // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                            rowIndex++;  // rowIndex tương ứng từng dòng dữ liệu
                            //gán giá trị cho từng cell                      
                            ws.Cells.Style.Font.Bold = false;
                            ws.Cells.Style.WrapText = true;
                            ws.Cells[rowIndex, colIndex++].Value = (rowIndex - 3);                          //  1 số thư tự 
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_HoLot;        //  2 
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_Ten;          //  3
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_NgaySinh;     //  4
                            ws.Cells[rowIndex, colIndex++].Value = item.Dkxt_NguyenVong;                // 5

                            ws.Cells[rowIndex, colIndex++].Value = item.Nganh.Nganh_MaNganh;       //  6
                            ws.Cells[rowIndex, colIndex++].Value = item.Nganh.Nganh_TenNganh;      //  7
                            ws.Cells[rowIndex, colIndex++].Value = item.Dkxt_MonDatGiai;     //  8
                            ws.Cells[rowIndex, colIndex++].Value = item.Dkxt_NamDatGiai;          // 9
                            ws.Cells[rowIndex, colIndex++].Value = item.Dkxt_LoaiGiai;          // 9


                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_DienThoai;        //  11
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_Email;            //  12

                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_HoKhauThuongTru;      //  13
                            ws.Cells[rowIndex, colIndex++].Value = item.ThiSinhDangKy.ThiSinh_DCNhanGiayBao;     //  14
                            ws.Cells[rowIndex, colIndex++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        //string[] arr_col_number = { "TT", "Họ, tên đệm", "Tên", "Ngày sinh", "Nguyện vọng", "Mã ngành", "Tên ngành đăng ký", "Môn đạt giải",  "Năm đạt giải", "Loại giải",
                        //"Điện thoại", "Email", "Hộ khẩu thường trú", "Địa chỉ nhận giấy báo" };

                        for (int indexCol = 1; indexCol <= arr_col_number.Count(); indexCol++)
                        {
                            if (indexCol == 1) { ws.Column(indexCol).Width = 6; }       //1 STT
                            if (indexCol == 2) { ws.Column(indexCol).Width = 15; }      //2
                            if (indexCol == 3) { ws.Column(indexCol).Width = 7.3; }     //3
                            if (indexCol == 4) { ws.Column(indexCol).Width = 14.3; }    //4
                            if (indexCol == 5) { ws.Column(indexCol).Width = 13; }      //5
                            if (indexCol == 6) { ws.Column(indexCol).Width = 14.67; }      //6 
                            if (indexCol == 7) { ws.Column(indexCol).Width = 20; }      //7 
                            if (indexCol == 8) { ws.Column(indexCol).Width = 27; }      //8 
                            if (indexCol == 9) { ws.Column(indexCol).Width = 19.67; }      //9  
                            if (indexCol == 10) { ws.Column(indexCol).Width = 22; }     //10                      
                            if (indexCol == 11) { ws.Column(indexCol).Width = 22; }     //11 
                            if (indexCol == 12) { ws.Column(indexCol).Width = 30; }     //12 
                            if (indexCol == 13) { ws.Column(indexCol).Width = 40; }     //13 
                            if (indexCol == 14) { ws.Column(indexCol).Width = 40; }     //13 

                            ws.Cells[3, 1, 3, indexCol].Style.Font.Bold = true;         // đặt tiêu đề cho bảng có kiểu chữ đậm
                        }
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[1, 1].Style.Font.Bold = true;
                        ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; ws.Cells[rowIndex, colIndex++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        //Lưu file lại  
                        string excelName = "DangKyXetTuyenThang";
                        using (var memoryStream = new MemoryStream())
                        {
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + excelName + ".xlsx"); // tên file lưu
                            _excelpackage.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                    //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}