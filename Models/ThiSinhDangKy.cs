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
            KinhPhis = new HashSet<KinhPhi>();
        }

        [Key]
        public long ThiSinh_ID { get; set; }

        [Display(Name = "Họ, tên đệm"), Required(ErrorMessage = "Vui lòng nhập họ, tên đệm")]
        public string ThiSinh_HoLot { get; set; }

        [Display(Name = "Tên"), Required(ErrorMessage = "Vui lòng nhập tên")]
        public string ThiSinh_Ten { get; set; }

        [Display(Name = "Ngày sinh"), Required(ErrorMessage = "Vui lòng chọn ngày sinh")]
        public string ThiSinh_NgaySinh { get; set; }

        public int? ThiSinh_GioiTinh { get; set; }

        public string ThiSinh_DanToc { get; set; }

        [StringLength(50), Required(ErrorMessage = "Nhập căn cước công dân")]
        public string ThiSinh_CCCD { get; set; }

        [StringLength(500, ErrorMessage = "Mật khẩu phải có ít nhất {2} kí tự", MinimumLength = 6), Required(ErrorMessage = "Vui lòng nhập mật khẩu"), DataType(DataType.Password)]
        public string ThiSinh_MatKhau { get; set; }

        [StringLength(500)]
        public string ThiSinh_ResetCode { get; set; }

        [StringLength(500), Display(Name = "Điện thoại"), Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string ThiSinh_DienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng"), StringLength(500), Display(Name = "Email"), Required(ErrorMessage = "Vui lòng nhập email")]
        public string ThiSinh_Email { get; set; }

        [Display(Name = "Địa chỉ nhận giấy báo")]
        [StringLength(300)]
        public string ThiSinh_DCNhanGiayBao { get; set; }

        [Display(Name = "Hộ khẩu thường trú"), StringLength(300)]
        public string ThiSinh_HoKhauThuongTru { get; set; }

        [Display(Name = "Tên trường cấp ba"), StringLength(2000)]
        public string TruongCapBa_Ten { get; set; }

        public string TruongCapBa_Ma { get; set; }

        public string ThiSinh_NamTotNghiep { get; set; }

        public string TruongCapBa_MaTinh { get; set; }

        public string TruongCapBa_TenTinh { get; set; }

        public int? KhuVuc_ID { get; set; }

        public int? DoiTuong_ID { get; set; }

        public int? ThiSinh_HocLucLop12 { get; set; }

        public int? ThiSinh_HanhKiemLop12 { get; set; }

        public string TruongCapBa_ID { get; set; }

        public string TruongCapBa_ThongTin { get; set; }

        public string ThiSinh_HoKhauThuongTru_Check { get; set; }

        public int? ThiSinh_TrangThai { get; set; }

        public string ThiSinh_GhiChu { get; set; }

        public string ThiSinh_Email_Temp { get; set; }

        public string ThiSinh_NgayDangKy { get; set; }


        public virtual ICollection<DangKyDuThiNangKhieu> DangKyDuThiNangKhieus { get; set; }
        public virtual ICollection<DangKyXetTuyenHB> DangKyXetTuyenHBs { get; set; }
        public virtual ICollection<DangKyXetTuyenKhac> DangKyXetTuyenKhacs { get; set; }
        public virtual ICollection<DangKyXetTuyenKQTQG> DangKyXetTuyenKQTQGs { get; set; }
        public virtual ICollection<DangKyXetTuyenThang> DangKyXetTuyenThangs { get; set; }
        public virtual ICollection<KinhPhi> KinhPhis { get; set; }
        [Display(Name = "Đối tượng")]
        public virtual DoiTuong DoiTuong { get; set; }
        [Display(Name = "Khu vực")]
        public virtual KhuVuc KhuVuc { get; set; }


    }
}
