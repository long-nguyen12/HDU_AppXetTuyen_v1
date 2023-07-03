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

        [StringLength(100)]
        public string HocSinh_DinhDanh { get; set; }

        [StringLength(100)]
        public string HocSinh_HoTen { get; set; }

        public int? HocSinh_GioiTinh { get; set; }

        [StringLength(20)]
        public string HocSinh_NgaySinh { get; set; }

        [StringLength(500)]
        public string HocSinh_NoiSinh { get; set; }

        [StringLength(100)]
        public string HocSinh_Email { get; set; }

        [StringLength(500)]
        public string HocSinh_NoiCuTru { get; set; }

        [StringLength(500)]
        public string HocSinh_TruongMN { get; set; }

        [StringLength(100)]
        public string HocSinh_UuTien { get; set; }

        [StringLength(4000)]
        public string HocSinh_ThongTinCha { get; set; }

        [StringLength(4000)]
        public string HocSinh_NgheNghiepCha { get; set; }

        [StringLength(20)]
        public string HocSinh_DienThoaiCha { get; set; }

        [StringLength(4000)]
        public string HocSinh_ThongTinMe { get; set; }

        [StringLength(4000)]
        public string HocSinh_NgheNghiepMe { get; set; }

        [StringLength(20)]
        public string HocSinh_DienThoaiMe { get; set; }

        [StringLength(4000)]
        public string HocSinh_MinhChungMN { get; set; }

        [StringLength(4000)]
        public string HocSinh_MinhChungGiayKS { get; set; }

        [StringLength(4000)]
        public string HocSinh_MinhChungMaDinhDanh { get; set; }

        [StringLength(4000)]
        public string HocSinh_GiayUuTien { get; set; }

        public int? HocSinh_XacNhanLePhi { get; set; }

        public int? HocSinh_TrangThai { get; set; }

        [StringLength(4000)]
        public string HocSinh_GhiChu { get; set; }
    }
}
