namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LePhiXetTuyen")]
    public partial class LePhiXetTuyen
    {
        [Key]
        public long Lpxt_ID { get; set; }

        public long? ThiSinh_ID { get; set; }

        public double? Lpxt_SoTienDong { get; set; }

        public int? Lpxt_TrangThai { get; set; }

        [StringLength(500)]
        public string Lpxt_MinhChung { get; set; }

        [StringLength(500)]
        public string Lpxt_GhiChu { get; set; }

        public virtual ThiSinhDangKy ThiSinhDangKy { get; set; }
    }
}
