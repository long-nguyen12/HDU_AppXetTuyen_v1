namespace HDU_AppXetTuyen.Models
{
    using HDU_AppXetTuyen.Controllers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics;

    [Table("LienCapTHCS")]
    public partial class LienCapTHCS
    {
        [Key]
        public long HocSinh_ID { get; set; }

        [Display(Name = "Mã định danh cá nhân")]
        [StringLength(100)]
        public string HocSinh_DinhDanh { get; set; }
        
        [Display(Name = "Họ và tên")]
        [StringLength(100)]
        public string HocSinh_HoTen { get; set; }

        [Display(Name = "Giới tính")]
        public int? HocSinh_GioiTinh { get; set; }

        [Display(Name = "Ngày sinh")]
        [StringLength(100)]
        public string HocSinh_NgaySinh { get; set; }

        [Display(Name = "Nơi sinh")]
        [StringLength(500)]
        public string HocSinh_NoiSinh { get; set; }

        [Display(Name = "Email")]
        [StringLength(500)]
        public string HocSinh_Email { get; set; }

        [Display(Name = "Nơi cư trú")]
        [StringLength(500)]
        public string HocSinh_NoiCuTru { get; set; }

        [Display(Name = "Trường tiểu học")]
        [StringLength(500)]
        public string HocSinh_TruongTH { get; set; }

        [Display(Name = "Thuộc diện ưu tiên")]
        [StringLength(100)]
        public string HocSinh_UuTien { get; set; }

        [Display(Name = "Thông tin cha")]
        [StringLength(4000)]
        public string HocSinh_ThongTinCha { get; set; }

        [Display(Name = "Thông tin mẹ")]
        [StringLength(4000)]
        public string HocSinh_ThongTinMe { get; set; }

        [Display(Name = "Điểm học tập")]
        [StringLength(4000)]
        public string HocSinh_DiemHocTap { get; set; }

        [Display(Name = "Năng lực")]
        [StringLength(100)]
        public string HocSinh_MucDoNangLuc { get; set; }

        [Display(Name = "Phẩm chất")]
        [StringLength(100)]
        public string HocSinh_MucDoPhamChat { get; set; }

        [Display(Name = "Minh chứng học bạ")]
        [StringLength(4000)]
        public string HocSinh_MinhChungHB { get; set; }

        [Display(Name = "Minh chứng giấy khai sinh")]
        [StringLength(4000)]
        public string HocSinh_MinhChungGiayKS { get; set; }

        [Display(Name = "Minh chứng mã định danh")]
        [StringLength(4000)]
        public string HocSinh_MinhChungMaDinhDanh { get; set; }

        [Display(Name = "Minh chứng giấy ưu tiên")]
        [StringLength(4000)]
        public string HocSinh_GiayUuTien { get; set; }

        [Display(Name = "Xác nhận lệ phí")]
        [StringLength(4000)]
        public string HocSinh_XacNhanLePhi { get; set; }

        [Display(Name = "Mã kích hoạt tài khoản")]
        [StringLength(4000)]
        public string HocSinh_Activation { get; set; }

        [Display(Name = "Trạng thái")]
        public int? HocSinh_TrangThai { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(4000)]
        public string HocSinh_GhiChu { get; set; }
        
        [NotMapped]  
        public MonHocTHCS Monhocs { get; set; }

        [NotMapped]
        public double TongDiem { get; set; }

        [NotMapped]
        public PhuHuynh PhBo { get; set; }

        [NotMapped]
        public PhuHuynh PhMe { get; set; }

    }
    public class MonHocTHCS
    {
        public double Toan { get; set; }
        public double TiengViet { get; set; }
        public double TuNhien { get; set; }
        public double LichSuDiaLy { get; set; }
        public double TiengAnh { get; set; }
       
    }   
    public class PhuHuynh
    {
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string NgheNghiep { get; set; }       
    }
}
