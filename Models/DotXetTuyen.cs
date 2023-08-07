namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DotXetTuyen")]
    public partial class DotXetTuyen
    {
        public DotXetTuyen()
        {
            DangKyDuThiNangKhieus = new HashSet<DangKyDuThiNangKhieu>();
            DangKyXetTuyenHBs = new HashSet<DangKyXetTuyenHB>();
            DangKyXetTuyenKhacs = new HashSet<DangKyXetTuyenKhac>();
            DangKyXetTuyenKQTQGs = new HashSet<DangKyXetTuyenKQTQG>();
            DangKyXetTuyenThangs = new HashSet<DangKyXetTuyenThang>();
        }


        [Key]
        public int Dxt_ID { get; set; }
        [StringLength(250)]
        public string Dxt_Ten { get; set; }

        public int? Dxt_TrangThai { get; set; }

        [StringLength(250)]
        public string Dxt_GhiChu { get; set; }

        public int? NamHoc_ID { get; set; }
        public virtual NamHoc NamHoc { get; set; }
        public virtual ICollection<DangKyDuThiNangKhieu> DangKyDuThiNangKhieus { get; set; }
        public virtual ICollection<DangKyXetTuyenHB> DangKyXetTuyenHBs { get; set; }
        public virtual ICollection<DangKyXetTuyenKhac> DangKyXetTuyenKhacs { get; set; }
        public virtual ICollection<DangKyXetTuyenKQTQG> DangKyXetTuyenKQTQGs { get; set; }
        public virtual ICollection<DangKyXetTuyenThang> DangKyXetTuyenThangs { get; set; }

    }
}
