using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDU_AppXetTuyen.Models
{
    
    public class StatusTracking // khai báo để tạo dữ liệu cho dropdowlis trạng thái hồ sơ và trạng thái kinh phí
    {
        public int st_ID { get; set; }
        public string st_Name { get; set; }
    }
    // Cương khai báo ..Chưa hiểu làm gì
    public class Constant
    {
        public static string DEFAULT_URL = "https://localhost:44308/";
        // PRODUCTION_URL = "https://dkxt.hdu.edu.vn/";
    }
   
    public class MonDiem  // dùng để tách dữ liệu trong bảng thí sinh đăng ký bằng học bạ
    {
        public string TenMon { get; set; }
        public string HK1 { get; set; }
        public string HK2 { get; set; }
        public string HK3 { get; set; }
        public string DiemTrungBinh { get; set; }
    }
    public class DiemThiGQMon  // dùng để tách dữ liệu trong bảng thí sinh đăng ký bằng điểm thi tốt nghiệp
    {
        public string TenMon { get; set; }
        public string Diem { get; set; }
    }
 
    public class KhoangDiem    // dùng để lọc dữ liệu theo khoảng điểm cho trường liên cấp trung học cơ sở
    {
        public int value_kd { get; set; }
        public string text_kd { get; set; }
    }
    public class MonHocTHCS  // dùng để tách dữ liệu điểm trong bảng trường liên cấp trung học cơ sở
    {
        public double Toan { get; set; }
        public double TiengViet { get; set; }
        public double TuNhien { get; set; }
        public double LichSuDiaLy { get; set; }
        public double TiengAnh { get; set; }
    }
    public class PhuHuynh  // dùng để tách dữ liệu phụ huynh trong bảng trường liên cấp trung học cơ sở 
    {
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string NgheNghiep { get; set; }
    }   
}