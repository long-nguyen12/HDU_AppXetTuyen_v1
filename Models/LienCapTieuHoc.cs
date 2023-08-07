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

        [Required(ErrorMessage = "Mã định danh bắt buộc nhập")]
        [Display(Name = "Mã định danh cá nhân")]
        [StringLength(100)]
        public string HocSinh_DinhDanh { get; set; }

        [Required(ErrorMessage = "Họ và tên bắt buộc nhập")]
        [Display(Name = "Họ và tên")]
        [StringLength(100)]
        public string HocSinh_HoTen { get; set; }

        [Required(ErrorMessage = "Giới tính bắt buộc chọn")]
        [Display(Name = "Giới tính")]
        public int? HocSinh_GioiTinh { get; set; }

        [Required(ErrorMessage = "Ngày sinh bắt buộc nhập")]
        [Display(Name = "Ngày sinh")]
        [StringLength(20)]
        public string HocSinh_NgaySinh { get; set; }

        [Required(ErrorMessage = "Nơi sinh bắt buộc nhập")]
        [Display(Name = "Nơi sinh")]
        [StringLength(500)]
        public string HocSinh_NoiSinh { get; set; }

        [Required(ErrorMessage = "Email bắt buộc nhập")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail sai định dạng")]
        [EmailAddress(ErrorMessage = "E-mail sai định dạng")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail sai định dạng")]

        [Display(Name = "Email")]
        [StringLength(100)]
        public string HocSinh_Email { get; set; }

        [Required(ErrorMessage = "Nơi cư trú bắt buộc nhập")]
        [Display(Name = "Nơi cư trú")]
        [StringLength(500)]
        public string HocSinh_NoiCuTru { get; set; }

        [Display(Name = "Đã hoàn thành chương trình mẫu giáo tại trường")]
        [StringLength(500)]
        public string HocSinh_TruongMN { get; set; }

        [Display(Name = "Thuộc diện ưu tiên")]
        [StringLength(100)]
        public string HocSinh_UuTien { get; set; }

        [Required(ErrorMessage = "Họ tên cha bắt buộc nhập")]
        [Display(Name = "Họ tên cha")]
        [StringLength(4000)]
        public string HocSinh_ThongTinCha { get; set; }

        [Display(Name = "Nghề nghiệp cha")]
        [StringLength(4000)]

        public string HocSinh_NgheNghiepCha { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại sai đinh dạng")]
        [Display(Name = "Điện thoại cha")]
        [Required(ErrorMessage = "Điện thoại cha bắt buộc nhập")]

        [StringLength(20)]
        public string HocSinh_DienThoaiCha { get; set; }

        [Required(ErrorMessage = "Họ tên mẹ bắt buộc nhập")]
        [Display(Name = "Họ tên mẹ")]
        [StringLength(4000)]
        public string HocSinh_ThongTinMe { get; set; }

        [Display(Name = "Nghề nghiệp mẹ")]
        [StringLength(4000)]
        public string HocSinh_NgheNghiepMe { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại sai đinh dạng.")]
        [Required(ErrorMessage = "Điện thoại mẹ bắt buộc nhập")]
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

        [NotMapped]
        [StringLength(4000)]
        public string HocSinh_GhiChu2 { get; set; }

        [Display(Name = "Minh chứng lệ phí")]
        [StringLength(4000)]

        public string HocSinh_MinhChungLePhi { get; set; }

        [StringLength(4000)]
        public string HocSinh_Activation { get; set; }
    }
}
