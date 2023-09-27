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
        public string FilteriNganh { get; set; }
        public string FilteriLePhi { get; set; }
        public string FilteriHoSo { get; set; }
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
        public void ExportHocVienDuTuyen(string filteriNganhHoc, string filteriLePhi, string filteriHoSo, string searchString, string currentFilter, string filteriDotxt, string sortOrder, int? page)
        {
            var dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_Classify == 2 && d.Dxt_TrangThai_Xt == 1);

            var ListHvDts = db.HocVienDuTuyens
                              .Include(h => h.DotXetTuyen)
                              .Include(h => h.HocVienDangKy)
                              .Include(h => h.NganhMaster)
                              .Where(x => x.Dxt_ID == dxt_hientai.Dxt_ID).ToList();
            // lọc theo ngành
            if (!String.IsNullOrEmpty(filteriNganhHoc))
            {
                int FilteriNganh = Int32.Parse(filteriNganhHoc);
                ListHvDts = ListHvDts.Where(x => x.Nganh_Mt_ID == FilteriNganh).ToList();
            }

            // lọc theo trạng thái lệ phí
            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int FilteriLePhi = Int32.Parse(filteriLePhi);
                ListHvDts = ListHvDts.Where(x => x.HocVien_LePhi_TrangThai == FilteriLePhi).ToList();
            }
            // lọc theo trạng thái lệ hồ sơ
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int FilteriHoSo = Int32.Parse(filteriHoSo);
                ListHvDts = ListHvDts.Where(x => x.DuTuyen_TrangThai == FilteriHoSo).ToList();
            }

            // lọc theo tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                ListHvDts = ListHvDts.Where(h => h.HocVienDangKy.HocVien_Ten.ToUpper().Contains(searchString.ToUpper())
                                  || h.HocVienDangKy.HocVien_HoDem.ToUpper().Contains(searchString.ToUpper())
                                  || h.HocVienDangKy.HocVien_CCCD.Contains(searchString)
                                  || h.HocVienDangKy.HocVien_DienThoai.Contains(searchString)).ToList();
            }          
            try
            {
                using (ExcelPackage _excelpackage = new ExcelPackage())
                {
                    _excelpackage.Workbook.Properties.Author = "208Team";  // đặt tên người tạo file                       
                    _excelpackage.Workbook.Properties.Title = "TKHVDKDuTuyen"; // đặt tiêu đề cho file                  
                                                                               // khai báo đối tượng ExcelWorksheet để thao tác với sheet
                    ExcelWorksheet ews = null;
                    /*
                    1. đếm tổng số có bao nhiêu ngành có học viên đăng ký dự tuyển để tạo ra bấy nhiêu sheet
                    2. với mỗi 1 ngành thì đọc dữ liệu ghi vào sheet;
                    3. ghi ra file excel
                    */
                    if (ListHvDts.Count > 0)
                    {
                        var ListNganhHocVienDuTuyen = (from item in ListHvDts
                                                       select new { item.NganhMaster.Nganh_Mt_ID, item.NganhMaster.Nganh_Mt_MaNganh, item.NganhMaster.Nganh_Mt_TenNganh, item.NganhMaster.Nganh_Mt_NghienCuu_Ten })
                                                       .Distinct().ToList();
                        for (int index_nb_sheet = 1; index_nb_sheet <= ListNganhHocVienDuTuyen.Count; index_nb_sheet++)
                        {
                            _excelpackage.Workbook.Worksheets.Add(index_nb_sheet.ToString());
                        }

                        // tạo ra tên các cột của 1 sheet excel
                        string[] arr_col_number = { "TT"," Họ lót","Tên","Ngày sinh","Nơi sinh","Ngành đại học","Hoàn thành học bổ sung kiến thức",
                                "Xếp loại tốt nghiệp đại học","Hệ 10","Hệ 4","Phiếu đăng ký dự thi","Sơ yếu lý lịch","Bằng ĐH (công chứng)",
                                "Bảng điểm ĐH  (công chứng)","Giấy khám sức khỏe","Giấy tờ xét miễn ngoại ngữ","Giấy tờ khác"};

                        // khai báo chỉ số của sheet
                        int index_sheet = 0;
                        foreach (var item in ListNganhHocVienDuTuyen)
                        {
                            // tăng chỉ số sheet lên 1 đơn vị
                            index_sheet++;
                            // lấy ra sheet thứ i
                            ews = _excelpackage.Workbook.Worksheets[index_sheet];


                            // đặt tên cho sheet    
                            ews.Name = item.Nganh_Mt_TenNganh;
                            // fontsize mặc định cho cả sheet     
                            ews.Cells.Style.Font.Size = 12;
                            // font family mặc định cho cả sheet
                            ews.Cells.Style.Font.Name = "Times New Roman";

                            // lấy ra danh sách học viên có id_ngành bằng item.id
                            var ListHvDts_Customs = ListHvDts.Where(x => x.Nganh_Mt_ID == item.Nganh_Mt_ID).ToList();
                            // gán dữ liệu cho các cột
                            // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                            int row_item = 9, col_item = 0;
                            foreach (var item_master in ListHvDts_Customs)
                            {
                                BangDaiHoc bangDaiHoc_Item = JsonConvert.DeserializeObject<BangDaiHoc>(item_master.HocVienDangKy.HocVien_BangDaiHoc);
                                ThongTinHoSoMinhChung minhChung_Item = JsonConvert.DeserializeObject<ThongTinHoSoMinhChung>(item_master.DuTuyen_ThongTinHoSoMinhChung);

                                string bskienthuc = "";
                                if (item_master.HocVienDangKy.HocVien_BoTucKienThuc == 1) { bskienthuc = "Chưa học"; }
                                if (item_master.HocVienDangKy.HocVien_BoTucKienThuc == 2) { bskienthuc = "Đã học"; }
                                if (item_master.HocVienDangKy.HocVien_BoTucKienThuc == 3) { bskienthuc = "CN đúng"; }
                                // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                                col_item = 1;
                                // rowIndex tương ứng từng dòng dữ liệu
                                row_item++;
                                //gán giá trị cho từng cell          

                                ews.Cells.Style.Font.Bold = false;
                                ews.Cells.Style.WrapText = true;
                                ews.Cells[row_item, col_item++].Value = (row_item-9);                                 //  1 số thư tự 
                                ews.Cells[row_item, col_item++].Value = item_master.HocVienDangKy.HocVien_HoDem;        //  2 
                                ews.Cells[row_item, col_item++].Value = item_master.HocVienDangKy.HocVien_Ten;          //  3
                                ews.Cells[row_item, col_item++].Value = (DateTime.Parse(item_master.HocVienDangKy.HocVien_NgaySinh)).ToString("dd.MM.yyyy");     //  4
                                ews.Cells[row_item, col_item++].Value = db.Tinhs.Where(x => x.Tinh_ID == item_master.HocVienDangKy.HocVien_NoiSinh).FirstOrDefault().Tinh_Ten;                 // 5

                                ews.Cells[row_item, col_item++].Value = bangDaiHoc_Item.HocVien_BangDaiHoc_TenNganhTN;                //  6
                                ews.Cells[row_item, col_item++].Value = bskienthuc;          //  7
                                ews.Cells[row_item, col_item++].Value = bangDaiHoc_Item.HocVien_BangDaiHoc_LoaiTN;     //  8

                                string diem10 = "", diem4 = "";
                                if (bangDaiHoc_Item.HocVien_BangDaiHoc_ThangDiem == "10") { diem10 = bangDaiHoc_Item.HocVien_BangDaiHoc_DiemToanKhoa; }
                                if (bangDaiHoc_Item.HocVien_BangDaiHoc_ThangDiem == "4") { diem4 = bangDaiHoc_Item.HocVien_BangDaiHoc_DiemToanKhoa; }

                                ews.Cells[row_item, col_item++].Value = diem10;          // 9
                                ews.Cells[row_item, col_item++].Value = diem4;     // 10

                                ews.Cells[row_item, col_item++].Value = minhChung_Item.PhieuDangKyDuThi;        //  11
                                ews.Cells[row_item, col_item++].Value = minhChung_Item.SoYeuLyLich;
                                ews.Cells[row_item, col_item++].Value = minhChung_Item.BangDH;
                                ews.Cells[row_item, col_item++].Value = minhChung_Item.BangDiemDH;
                                ews.Cells[row_item, col_item++].Value = minhChung_Item.GiayKhamSucKhoe;
                                ews.Cells[row_item, col_item++].Value = minhChung_Item.GiayMienNgoaiNgu;
                                ews.Cells[row_item, col_item++].Value = minhChung_Item.GiayToKhac;
                                //ews.Cells[row_item, col_item++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                           
                            string str_add = "A8:Q" + (ListHvDts_Customs.ToList().Count + 9).ToString();
                           
                            ews.Cells[str_add].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            ews.Cells[str_add].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            ews.Cells[str_add].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            ews.Cells[str_add].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            string stt_add = "A8:A" + (ListHvDts_Customs.ToList().Count + 9).ToString();
                            ews.Cells[stt_add].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            string mc_add = "I10:Q" + (ListHvDts_Customs.ToList().Count + 10).ToString();
                            ews.Cells[mc_add].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            ews.Cells[1, 1].Value = ("HĐTS ĐT THẠC SĨ " + dxt_hientai.Dxt_Ten).ToUpper();                         
                           
                            ews.Cells[1, 1, 1, 4].Merge = true;
                            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                            ews.Cells[1, 1].Style.Font.Bold = true;
                            ews.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ews.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ews.Cells[2, 1].Value = "BAN RÀ SOÁT HỒ SƠ";
                            ews.Cells[2, 1, 2, 4].Merge = true;
                            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                            ews.Cells[2, 1].Style.Font.Bold = true;
                            ews.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ews.Cells[2, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ews.Cells[1, 7].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                            ews.Cells[1, 7, 1, 17].Merge = true;
                            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                            ews.Cells[1, 7].Style.Font.Bold = true;
                            ews.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ews.Cells[1, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ews.Cells[2, 7].Value = "Độc lập - Tự do - Hạnh phúc";
                            ews.Cells[2, 7, 2, 17].Merge = true;
                            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                            ews.Cells[2, 7].Style.Font.Bold = true;
                            ews.Cells[2, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ews.Cells[2, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ews.Cells[5, 1].Value =("RÀ SOÁT HỒ SƠ THÍ SINH ĐĂNG KÝ DỰ THI ĐÀO TẠO TRÌNH ĐỘ THẠC SĨ " + dxt_hientai.Dxt_Ten).ToUpper(); 
                            ews.Cells[5, 1, 5, 17].Merge = true;
                            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                            ews.Cells[5, 1].Style.Font.Bold = true;
                            ews.Cells[5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ews.Cells[5, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ews.Cells[7, 1].Value = "Chuyên ngành: " + item.Nganh_Mt_TenNganh;
                            ews.Cells[7, 1, 7, 4].Merge = true;
                            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                            ews.Cells[7, 1].Style.Font.Bold = true;
                            ews.Cells[7, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ews.Cells[7, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ews.Cells[7,7].Value = "Định hướng chương trình: " + item.Nganh_Mt_NghienCuu_Ten;
                            ews.Cells[7, 7, 7, 17].Merge = true;
                            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                            ews.Cells[7, 7].Style.Font.Bold = true;
                            ews.Cells[7, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ews.Cells[7, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ews.Cells[8, 9].Value = "Điểm TBC đại học";
                            ews.Cells[8, 9, 8, 10].Merge = true;
                            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                            ews.Cells[8, 9].Style.Font.Bold = true;
                            ews.Cells[8, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ews.Cells[8, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            ews.Cells[8, 11].Value = "Kết quả kiểm tra hồ sơ";
                            ews.Cells[8, 11, 8, 17].Merge = true;
                            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                            ews.Cells[8, 11].Style.Font.Bold = true;
                            ews.Cells[8, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ews.Cells[8, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            int colIndex = 1;
                            //tạo các header từ column header đã tạo từ bên trên
                            foreach (var item_title in arr_col_number)
                            {
                                if (colIndex < 9)
                                {
                                    var cell = ews.Cells[8, colIndex];
                                    cell.Value = item_title;
                                    ews.Cells[8, colIndex, 9, colIndex].Merge = true;
                                    //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                                    cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    cell.Style.WrapText = true;
                                }
                                if (colIndex >= 9)
                                {
                                    var cell = ews.Cells[9, colIndex];
                                    cell.Value = item_title;
                                    cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    cell.Style.WrapText = true;
                                }
                                colIndex++;
                            }
                            ews.Cells["I9:Q9"].Style.Font.Size = 9;
                            ews.Cells["A8:H8"].Style.Font.Size = 11;

                            for (int index_col = 1; index_col <= arr_col_number.Count(); index_col++)
                            {
                                if (index_col == 1) { ews.Column(index_col).Width = 6; }       //1
                                if (index_col == 2) { ews.Column(index_col).Width = 18; }      //2
                                if (index_col == 3) { ews.Column(index_col).Width = 9; }     //3
                                if (index_col == 4) { ews.Column(index_col).Width = 13; }    //4
                                if (index_col == 5) { ews.Column(index_col).Width = 17; }      //5
                                if (index_col == 6) { ews.Column(index_col).Width = 25; }      //6 
                                if (index_col == 7) { ews.Column(index_col).Width = 9.7; }      //7 
                                if (index_col == 8) { ews.Column(index_col).Width = 7; }      //8 
                                if (index_col == 9) { ews.Column(index_col).Width = 6; }      //9  
                                if (index_col == 10) { ews.Column(index_col).Width = 6; }     //10  
                                if (index_col == 11) { ews.Column(index_col).Width = 5; }     //11  
                                if (index_col == 12) { ews.Column(index_col).Width = 5; }     //12  
                                if (index_col == 13) { ews.Column(index_col).Width = 5; }     //13
                                if (index_col == 14) { ews.Column(index_col).Width = 5; }     //14
                                if (index_col == 15) { ews.Column(index_col).Width = 5; }     //15
                                if (index_col == 16) { ews.Column(index_col).Width = 5; }     //16
                                if (index_col == 17) { ews.Column(index_col).Width = 5; }     //17
                            }
                            ews.Row(8).Height = 45;
                            ews.Row(9).Height = 90;
                            ews.PrinterSettings.PaperSize = ePaperSize.A4;
                            ews.PrinterSettings.Orientation = eOrientation.Landscape;
                            
                            ews.PrinterSettings.TopMargin = 0.3M;
                            ews.PrinterSettings.RightMargin = 0.1M;
                            ews.PrinterSettings.BottomMargin = 0.3M;
                            ews.PrinterSettings.LeftMargin = 0.1M;
                            ews.PrinterSettings.HeaderMargin = 0;
                            ews.PrinterSettings.FooterMargin = 0;

                        }
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyy-MM-dd") + "-TongHop.xlsx"); // tên file lưu
                        _excelpackage.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
        public void ExportHocVienDuTuyenOld(string filteriNganhHoc, string filteriLePhi, string filteriHoSo, string searchString, string currentFilter, string filteriDotxt, string sortOrder, int? page)
        {
            var dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_Classify == 2 && d.Dxt_TrangThai_Xt == 1);

            var ListHvDts = db.HocVienDuTuyens
                              .Include(h => h.DotXetTuyen)
                              .Include(h => h.HocVienDangKy)
                              .Include(h => h.NganhMaster)
                              .Where(x => x.Dxt_ID == dxt_hientai.Dxt_ID).ToList();

            // lọc theo ngành
            if (!String.IsNullOrEmpty(filteriNganhHoc))
            {
                int FilteriNganh = Int32.Parse(filteriNganhHoc);
                ListHvDts = ListHvDts.Where(x => x.Nganh_Mt_ID == FilteriNganh).ToList();
            }
            // lọc theo trạng thái lệ phí
            if (!String.IsNullOrEmpty(filteriLePhi))
            {
                int FilteriLePhi = Int32.Parse(filteriLePhi);
                ListHvDts = ListHvDts.Where(x => x.HocVien_LePhi_TrangThai == FilteriLePhi).ToList();
            }
            // lọc theo trạng thái lệ hồ sơ
            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int FilteriHoSo = Int32.Parse(filteriHoSo);
                ListHvDts = ListHvDts.Where(x => x.DuTuyen_TrangThai == FilteriHoSo).ToList();
            }

            // lọc theo tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                ListHvDts = ListHvDts.Where(h => h.HocVienDangKy.HocVien_Ten.ToUpper().Contains(searchString.ToUpper())
                                  || h.HocVienDangKy.HocVien_HoDem.ToUpper().Contains(searchString.ToUpper())
                                  || h.HocVienDangKy.HocVien_CCCD.Contains(searchString)
                                  || h.HocVienDangKy.HocVien_DienThoai.Contains(searchString)).ToList();
            }

            try
            {
                using (ExcelPackage _excelpackage = new ExcelPackage())
                {
                    _excelpackage.Workbook.Properties.Author = "208Team";  // đặt tên người tạo file                       
                    _excelpackage.Workbook.Properties.Title = "TKHVDKDuTuyen"; // đặt tiêu đề cho file                    

                    //Tạo sheet để làm việc 
                    _excelpackage.Workbook.Worksheets.Add("01");

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
                            ws.Cells[rowIndex, colIndex++].Value = DateTime.Parse(item.HocVienDangKy.HocVien_NgaySinh).ToString("dd.MM.yyyy");     //  4

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
            }
            catch { }

        }
        public void ExportHocVienDuTuyenJson(ExportFilteriData entity)
        {
            var dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_Classify == 2 && d.Dxt_TrangThai_Xt == 1);

            var ListHvDts = db.HocVienDuTuyens
                              .Include(h => h.DotXetTuyen)
                              .Include(h => h.HocVienDangKy)
                              .Include(h => h.NganhMaster)
                              .Where(x => x.Dxt_ID == dxt_hientai.Dxt_ID).ToList();

            // lọc theo ngành
            if (!String.IsNullOrEmpty(entity.FilteriNganh))
            {
                int _FilteriNganh = Int32.Parse(entity.FilteriNganh);
                ListHvDts = ListHvDts.Where(x => x.Nganh_Mt_ID == _FilteriNganh).ToList();
            }
            // lọc theo trạng thái lệ phí
            if (!String.IsNullOrEmpty(entity.FilteriLePhi))
            {
                int _FilteriLePhi = Int32.Parse(entity.FilteriLePhi);
                ListHvDts = ListHvDts.Where(x => x.HocVien_LePhi_TrangThai == _FilteriLePhi).ToList();
            }
            // lọc theo trạng thái lệ hồ sơ
            if (!String.IsNullOrEmpty(entity.FilteriHoSo))
            {
                int _FilteriHoSo = Int32.Parse(entity.FilteriHoSo);
                ListHvDts = ListHvDts.Where(x => x.DuTuyen_TrangThai == _FilteriHoSo).ToList();
            }

            // lọc theo tìm kiếm
            if (!String.IsNullOrEmpty(entity.SearchString))
            {
                ListHvDts = ListHvDts.Where(h => h.HocVienDangKy.HocVien_Ten.ToUpper().Contains(entity.SearchString.ToUpper())
                                  || h.HocVienDangKy.HocVien_HoDem.ToUpper().Contains(entity.SearchString.ToUpper())
                                  || h.HocVienDangKy.HocVien_CCCD.Contains(entity.SearchString)
                                  || h.HocVienDangKy.HocVien_DienThoai.Contains(entity.SearchString)).ToList();
            }

            //return Json(new { success = true, data = model }, JsonRequestBehavior.AllowGet);

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
                //return Json(new { success = true, data = model }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                //return Json(new { success = false, data = model }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}