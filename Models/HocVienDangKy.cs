namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HocVienDangKy")]
    public partial class HocVienDangKy
    {
        public HocVienDangKy()
        {
            HocVienDuTuyens = new HashSet<HocVienDuTuyen>();
        }

        [Key]
        public long HocVien_ID { get; set; }

        [StringLength(500)]
        public string HocVien_HoDem { get; set; }

        [StringLength(500)]
        public string HocVien_Ten { get; set; }

        public int? HocVien_GioiTinh { get; set; }

        [StringLength(100)]
        public string HocVien_DanToc { get; set; }

        [StringLength(500)]
        public string HocVien_NgaySinh { get; set; }

        [StringLength(500)]
        public string HocVien_CCCD { get; set; }

        [StringLength(500)]
        public string HocVien_CCCD_NgayCap { get; set; }

        [StringLength(500)]
        public string HocVien_DienThoai { get; set; }

        [StringLength(500)]
        public string HocVien_Email { get; set; }

        [StringLength(3000)]
        public string HocVien_BangDaiHoc { get; set; }

        [StringLength(3000)]
        public string HocVien_DoiTuongUuTien { get; set; }

        public int? HocVien_BoTucKienThuc { get; set; }

        public int? HocVien_NoiSinh { get; set; }

        [StringLength(500)]
        public string HocVien_HoKhauThuongTru { get; set; }

        [StringLength(500)]
        public string HocVien_NoiOHienNay { get; set; }

        [StringLength(500)]
        public string HocVien_DiaChiLienHe { get; set; }

        [StringLength(500)]
        public string HocVien_TenDonViCongTac { get; set; }

        [StringLength(500)]
        public string HocVien_ChuyenMon { get; set; }

        [StringLength(500)]
        public string HocVien_ThamNien { get; set; }

        [StringLength(500)]
        public string HocVien_ChucVu { get; set; }

        [StringLength(500)]
        public string HocVien_NamCT { get; set; }

        public int? HocVien_LoaiCB { get; set; }

        [StringLength(500)]
        public string HocVien_MatKhau { get; set; }

        [StringLength(500)]
        public string HocVien_ResetCode { get; set; }

        [StringLength(500)]
        public string HocVien_Email_Temp { get; set; }

        [StringLength(500)]
        public string HocVien_NgayDangKy { get; set; }

        public int? HocVien_TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HocVienDuTuyen> HocVienDuTuyens { get; set; }
    }
}
