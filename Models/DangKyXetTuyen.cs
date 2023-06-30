namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DangKyXetTuyen")]
    public partial class DangKyXetTuyen
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DangKyXetTuyen()
        {
            DangKyXetTuyen_DiemTS = new HashSet<DangKyXetTuyen_DiemTS>();
        }

        [Key]
        public long Dkxt_ID { get; set; }

        public long? ThiSinh_ID { get; set; }

        public int? Ptxt_ID { get; set; }

        public int? Nganh_ID { get; set; }

        public int? Thm_ID { get; set; }

        public int? DoiTuong_ID { get; set; }

        public int? KhuVuc_ID { get; set; }

        public int? Dkxt_TrangThai { get; set; }

        public int? Dkxt_NguyenVong { get; set; }

        public int? DotXT_ID { get; set; }

        [StringLength(500)]
        public string Dkxt_GhiChu { get; set; }

        [StringLength(1000)]
        public string Dkxt_Diem_M1 { get; set; }

        [StringLength(1000)]
        public string Dkxt_Diem_M2 { get; set; }

        [StringLength(1000)]
        public string Dkxt_Diem_M3 { get; set; }

        [StringLength(1000)]
        public string Dkxt_Diem_Tong { get; set; }

        [StringLength(1000)]
        public string Dkxt_Diem_Tong_Full { get; set; }
      
        public int? Dkxt_XepLoaiHocLuc_12 { get; set; }
      
        public int? Dkxt_XepLoaiHanhKiem_12 { get; set; }

        [StringLength(4000)]
        public string Dkxt_MinhChung_HB { get; set; }

        public int? Dkxt_TrangThai_KetQua { get; set; }

        [StringLength(4000)]
        public string Dkxt_MinhChung_CCCD { get; set; }
        [StringLength(4000)]
        public string Dkxt_MinhChung_Bang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyen_DiemTS> DangKyXetTuyen_DiemTS { get; set; }

        public virtual DoiTuong DoiTuong { get; set; }

        public virtual DotXetTuyen DotXetTuyen { get; set; }

        public virtual KhuVuc KhuVuc { get; set; }

        public virtual Nganh Nganh { get; set; }

        public virtual PhuongThucXetTuyen PhuongThucXetTuyen { get; set; }

        public virtual ThiSinhDangKy ThiSinhDangKy { get; set; }

        public virtual ToHopMon ToHopMon { get; set; }
    }
}
