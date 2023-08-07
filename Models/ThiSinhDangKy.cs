namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThiSinhDangKy")]
    public partial class ThiSinhDangKy
    {
        
        public ThiSinhDangKy()
        {
            DangKyDuThiNangKhieus = new HashSet<DangKyDuThiNangKhieu>();
            DangKyXetTuyenHBs = new HashSet<DangKyXetTuyenHB>();
            DangKyXetTuyenKhacs = new HashSet<DangKyXetTuyenKhac>();
            DangKyXetTuyenKQTQGs = new HashSet<DangKyXetTuyenKQTQG>();
            DangKyXetTuyenThangs = new HashSet<DangKyXetTuyenThang>();
        }       

        [Key]
        public long ThiSinh_ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Vui lòng nhập CMND/CCCD")]
        [Display(Name = "CMND/CCCD")]
        public string ThiSinh_CCCD { get; set; }

        [StringLength(500, ErrorMessage = "Mật khẩu phải có ít nhất {2} kí tự", MinimumLength = 6)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]

        public string ThiSinh_MatKhau { get; set; }

        [StringLength(500)]
        [Display(Name = "Điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string ThiSinh_DienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(500)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string ThiSinh_Email { get; set; }

        [StringLength(500)]
        [Display(Name = "Họ, tên đệm")]
        [Required(ErrorMessage = "Vui lòng nhập họ, tên đệm")]
        public string ThiSinh_HoLot { get; set; }

        [StringLength(500)]
        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string ThiSinh_Ten { get; set; }

        [StringLength(500)]
        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng chọn ngày sinh")]
        public string ThiSinh_NgaySinh { get; set; }

        [Display(Name = "Năm tốt nghiệp")]
        [StringLength(4)]
        public string ThiSinh_NamTotNghiep { get; set; }

        [StringLength(500)]
        public string ThiSinh_ResetCode { get; set; }

        [Display(Name = "Ngày đăng ký")]
        [StringLength(300)]
        public string ThiSinh_NgayDangKy { get; set; }

        [Display(Name = "Dân tộc")]
        [StringLength(50)]
        public string ThiSinh_DanToc { get; set; }

        [Display(Name = "Giới tính")]
        public int? ThiSinh_GioiTinh { get; set; }

        [Display(Name = "Địa chỉ nhận giấy báo")]
        [StringLength(300)]
        public string ThiSinh_DCNhanGiayBao { get; set; }

        [Display(Name = "Hộ khẩu")]
        [StringLength(300)]
        public string ThiSinh_HoKhauThuongTru { get; set; }

        [StringLength(300)]
        public string ThiSinh_HoKhauThuongTru_Check { get; set; }

        [Display(Name = "Khu vực")]
        public int? KhuVuc_ID { get; set; }

        [Display(Name = "Đối tượng")]
        public int? DoiTuong_ID { get; set; }

        [Display(Name = "Mã trường cấp ba")]
        [StringLength(300)]
        public string ThiSinh_TruongCapBa_Ma { get; set; }

        [Display(Name = "Tên trường cấp ba")]
        [StringLength(300)]
        public string ThiSinh_TruongCapBa { get; set; }     

        [Display(Name = "Trạng thái")]
        public int? ThiSinh_TrangThai { get; set; }

        public int? ThiSinh_TruongCapBa_Tinh_ID { get; set; }

        [Display(Name = "Học lực lớp 12")]
        public int? ThiSinh_HocLucLop12 { get; set; }

        [Display(Name = "Hạnh kiểm lớp 12")]
        public int? ThiSinh_HanhKiemLop12 { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(200)]
        public string ThiSinh_GhiChu { get; set; }

        [StringLength(200)]
        public string ThiSinh_Email_Temp { get; set; }
        public virtual ICollection<DangKyDuThiNangKhieu> DangKyDuThiNangKhieus { get; set; }        
        public virtual ICollection<DangKyXetTuyenHB> DangKyXetTuyenHBs { get; set; }        
        public virtual ICollection<DangKyXetTuyenKhac> DangKyXetTuyenKhacs { get; set; }        
        public virtual ICollection<DangKyXetTuyenKQTQG> DangKyXetTuyenKQTQGs { get; set; }        
        public virtual ICollection<DangKyXetTuyenThang> DangKyXetTuyenThangs { get; set; }   
        [Display(Name = "Đối tượng")]
        public virtual DoiTuong DoiTuong { get; set; }

        [Display(Name = "Khu vực")]
        public virtual KhuVuc KhuVuc { get; set; }
    }
}
