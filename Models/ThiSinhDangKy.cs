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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThiSinhDangKy()
        {
            DangKyDuThiNangKhieus = new HashSet<DangKyDuThiNangKhieu>();
            DangKyXetTuyens = new HashSet<DangKyXetTuyen>();
            LePhiXetTuyens = new HashSet<LePhiXetTuyen>();

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

        [StringLength(300)]
        public string ThiSinh_TruongCapBa_Ma { get; set; }

        [StringLength(300)]
        public string ThiSinh_TruongCapBa { get; set; }

        public int? DotXT_ID { get; set; }

        public int? ThiSinh_TrangThai { get; set; }

        public int? ThiSinh_TruongCapBa_Tinh_ID { get; set; }

        public int? ThiSinh_HocLucLop12 { get; set; }

        public int? ThiSinh_HanhKiemLop12 { get; set; }

        [StringLength(200)]
        public string ThiSinh_GhiChu { get; set; }

        [StringLength(200)]
        public string ThiSinh_Email_Temp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyen> DangKyXetTuyens { get; set; }

        public virtual DoiTuong DoiTuong { get; set; }

        public virtual DotXetTuyen DotXetTuyen { get; set; }

        public virtual KhuVuc KhuVuc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LePhiXetTuyen> LePhiXetTuyens { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyenKhac> DangKyXetTuyenKhacs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyenKQTQG> DangKyXetTuyenKQTQGs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyenThang> DangKyXetTuyenThangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyDuThiNangKhieu> DangKyDuThiNangKhieus { get; set; }
    }
}
