using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HDU_AppXetTuyen.Models
{
    public class SendEmail
    {
        public void Sendemail(string email, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("xettuyen@hdu.edu.vn", email))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                NetworkCredential NetworkCred = new NetworkCredential("xettuyen@hdu.edu.vn", "hongduc1");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }
    public class TongHopSoLieuXetTuyen
    {
        public string ThiSinh_ID { get; set; }
        public string Dkxt_ID { get; set; }
        public string Dxt_ID { get; set; }
        public string HoDem { get; set; }
        public string Ten { get; set; }
        public string DienThoai { get; set; }
        public int Ptxt_ID { get; set; }
        public string NguyenVong { get; set; }
        public string NgayDangKy { get; set; }
        public int NganhID { get; set; }
        public string TenNganh { get; set; }
        public string TenToHop { get; set; }
        public string MinhChung_HB { get; set; }
        public string MinhChung_Bang { get; set; }
        public string MinhChung_CCCD { get; set; }
        public string MinhChung_UuTien { get; set; }
        public string MinhChung_XetTuyen { get; set; }
        public string TrangThai_HoSo { get; set; }
        public string TrangThai_KetQua { get; set; }

        public string TrangThaiLP { get; set; }
        public string SoThamChieuLP { get; set; }     
        public string NgayThangNopLP { get; set; }       
        public string NgayThangCheckLP { get; set; }
        public string MinhChungLP { get; set; }       

        
    }
    public class StatusTracking // khai báo để tạo dữ liệu cho dropdowlis trạng thái hồ sơ và trạng thái kinh phí
    {
        public int St_ID { get; set; }
        public string St_Name { get; set; }
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

    public class ThongTinTruongCapBaTs  // dùng để tách dữ liệu thông tin trường cấp 3 của thí sinh đăng ký
    {
        public int TCB_Tinh_ID { get; set; }
        public int TCB_Ma { get; set; }       
        public string TCB_Ten { get; set; }
    }

    public class ThongTinQueQuanTs  // dùng để tách dữ liệu thông nơi đăng ký hộ khẩu của thí sinh
    {      
        public string Phuongxa { get; set; }      
        public string Huyen_ID { get; set; }
        public string Tinh_ID { get; set; }

    }
   
    public class KinhPhiInfo
    {
        public string KinhPhi_SoTC { get; set; }
        public string KinhPhi_TepMinhChung { get; set; }
        public string key_dkxt_id { get; set; }
        public string key_dkxt_pt { get; set; }
    }
    public class BangDaiHoc
    {
        public string HocVien_BangDaiHoc_TenTruongTN { get; set; }
        public string HocVien_BangDaiHoc_HeDaoTao { get; set; }
        public string HocVien_BangDaiHoc_TenNganhTN { get; set; }
        public string HocVien_BangDaiHoc_NamTN { get; set; }
        public string HocVien_BangDaiHoc_ThangDiem { get; set; }
        public string HocVien_BangDaiHoc_DiemToanKhoa { get; set; }       
        public string HocVien_BangDaiHoc_LoaiTN { get; set; }
    }   
    public class ThongTinHoSoMinhChung
    {
        public string PhieuDangKyDuThi { get; set; }
        public string SoYeuLyLich { get; set; }
        public string BangDH { get; set; }
        public string BangDiemDH { get; set; }
        public string GiayKhamSucKhoe { get; set; }
        public string TuiAnh4x6 { get; set; }
        public string GiayMienNgoaiNgu { get; set; }
        public string GiayToKhac { get; set; }      
    }
}