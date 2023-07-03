namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LienCapTieuHoc")]
    public partial class LienCapTieuHoc
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
        [StringLength(20)]
        public string HocSinh_NgaySinh { get; set; }

        [Display(Name = "Nơi sinh")]
        [StringLength(500)]
        public string HocSinh_NoiSinh { get; set; }

        [Display(Name = "Email")]
        [StringLength(100)]
        public string HocSinh_Email { get; set; }

        [Display(Name = "Nơi cư trú")]
        [StringLength(500)]
        public string HocSinh_NoiCuTru { get; set; }

        [Display(Name = "Đã hoàn thành chương trình mẫu giáo tại trường")]
        [StringLength(500)]
        public string HocSinh_TruongMN { get; set; }

        [Display(Name = "Thuộc diện ưu tiên")]
        [StringLength(100)]
        public string HocSinh_UuTien { get; set; }

        [Display(Name = "Họ tên cha")]
        [StringLength(4000)]
        public string HocSinh_ThongTinCha { get; set; }

        [Display(Name = "Nghề nghiệp cha")]
        [StringLength(4000)]
        public string HocSinh_NgheNghiepCha { get; set; }

        [Display(Name = "Điện thoại cha")]
        [StringLength(20)]
        public string HocSinh_DienThoaiCha { get; set; }

        [Display(Name = "Họ tên mẹ")]
        [StringLength(4000)]
        public string HocSinh_ThongTinMe { get; set; }

        [Display(Name = "Nghề nghiệp mẹ")]
        [StringLength(4000)]
        public string HocSinh_NgheNghiepMe { get; set; }

        [Display(Name = "Điện thoại mẹ")]
        [StringLength(20)]
        public string HocSinh_DienThoaiMe { get; set; }

        [Display(Name = "Giấy chứng nhận hoàn thành chương trình giáo dục mầm non")]
        [StringLength(4000)]
        public string HocSinh_MinhChungMN { get; set; }

        [Display(Name = "Giấy khai sinh")]
        [StringLength(4000)]
        public string HocSinh_MinhChungGiayKS { get; set; }

        [Display(Name = "Mã định danh cá nhân")]
        [StringLength(4000)]
        public string HocSinh_MinhChungMaDinhDanh { get; set; }

        [Display(Name = "Giấy chứng nhận ưu tiên")]
        [StringLength(4000)]
        public string HocSinh_GiayUuTien { get; set; }

        [Display(Name = "Xác nhận lệ phí")]
        public int? HocSinh_XacNhanLePhi { get; set; }

        [Display(Name = "Trạng thái")]
        public int? HocSinh_TrangThai { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(4000)]
        public string HocSinh_GhiChu { get; set; }
    }
}
