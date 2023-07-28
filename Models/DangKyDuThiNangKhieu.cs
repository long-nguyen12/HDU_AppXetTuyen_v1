namespace HDU_AppXetTuyen.Models
{  
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DangKyDuThiNangKhieu")]
    public partial class DangKyDuThiNangKhieu
    {
        public DangKyDuThiNangKhieu()
        {
            KinhPhis = new HashSet<KinhPhi>();
        }

        [Key]
        public long Dkdt_NK_ID { get; set; }

        public long? ThiSinh_ID { get; set; }

        public int? Nganh_ID { get; set; }

        public int? Thm_ID { get; set; }

        public int? DoiTuong_ID { get; set; }

        public int? KhuVuc_ID { get; set; }

        public int? Dkdt_NK_TrangThai { get; set; }

        public int? DotXT_ID { get; set; }

        public int? Dkdt_NK_NamTotNghiep { get; set; }
        public int? Ptxt_ID { get; set; }    

        [StringLength(500)]
        public string Dkdt_NK_GhiChu { get; set; }

        [StringLength(1000)]
        public string Dkdt_NK_MonThi { get; set; }
        public int? Dkdt_NK_NguyenVong { get; set; }
        [StringLength(100)]
        public string Dkdt_NK_NgayDangKy { get; set; }

        public int? Dkdt_NK_XepLoaiHocLuc_12 { get; set; }

        public int? Dkdt_NK_XepLoaiHanhKiem_12 { get; set; }

        public int? Dkdt_NK_TrangThai_KetQua { get; set; }

        [StringLength(4000)]
        public string Dkdt_NK_MinhChung_CCCD { get; set; }

        [StringLength(4000)]
        public string Dkdt_NK_MinhChung_BangTN { get; set; }

        [StringLength(4000)]
        public string Dkdt_NK_MinhChung_HocBa { get; set; }

        [StringLength(4000)]
        public string Dkdt_NK_MinhChung_UuTien { get; set; }
        public virtual ICollection<KinhPhi> KinhPhis { get; set; }

        public virtual Nganh Nganh { get; set; }

        public virtual ThiSinhDangKy ThiSinhDangKy { get; set; }

        public virtual ToHopMon ToHopMon { get; set; }
        public virtual DotXetTuyen DotXetTuyen { get; set; }
        public virtual PhuongThucXetTuyen PhuongThucXetTuyen { get; set; }
    }
}
