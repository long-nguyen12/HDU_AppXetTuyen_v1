namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DangKyXetTuyenKhac")]
    public partial class DangKyXetTuyenKhac
    {
        [Key]
        public long Dkxt_ID { get; set; }

        public long? ThiSinh_ID { get; set; }

        public int? Ptxt_ID { get; set; }

        public int? Nganh_ID { get; set; }

        public int? DoiTuong_ID { get; set; }

        public int? KhuVuc_ID { get; set; }

        public int? Dkxt_TrangThai { get; set; }

        public int? Dkxt_NguyenVong { get; set; }

        public int? DotXT_ID { get; set; }

        [StringLength(500)]
        public string Dkxt_GhiChu { get; set; }

        [StringLength(50)]
        public string Dkxt_ToHopXT { get; set; }

        [StringLength(1000)]
        public string Dkxt_DonViToChuc { get; set; }

        public double? Dkxt_KetQuaDatDuoc { get; set; }

        public double? Dkxt_TongDiem { get; set; }

        [StringLength(1000)]
        public string Dkxt_NgayDuThi { get; set; }

        public int? Dkxt_XepLoaiHocLuc_12 { get; set; }

        public int? Dkxt_XepLoaiHanhKiem_12 { get; set; }

        [StringLength(4000)]
        public string Dkxt_MinhChung_HB { get; set; }

        public int? Dkxt_TrangThai_KetQua { get; set; }

        [StringLength(4000)]
        public string Dkxt_MinhChung_CCCD { get; set; }

        [StringLength(4000)]
        public string Dkxt_MinhChung_Bang { get; set; }

        [StringLength(4000)]
        public string Dkxt_MinhChung_KetQua { get; set; }

        public virtual ThiSinhDangKy ThiSinhDangKy { get; set; }

        public virtual Nganh Nganh { get; set; }
    }
}
