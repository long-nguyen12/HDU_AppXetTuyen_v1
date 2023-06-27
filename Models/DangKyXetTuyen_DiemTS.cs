namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DangKyXetTuyen_DiemTS
    {
        [Key]
        public long Dkxt_Diem_ID { get; set; }

        public long? Dkxt_ID { get; set; }

        public long? ThiSinh_ID { get; set; }

        public int? MonHoc_ID { get; set; }

        public double? Dkxt_Diem_Ky1 { get; set; }

        public double? Dkxt_Diem_Ky2 { get; set; }

        public double? Dkxt_Diem_Ky3 { get; set; }

        [StringLength(500)]
        public string Dkxt_GhiChu { get; set; }

        public virtual DangKyXetTuyen DangKyXetTuyen { get; set; }
    }
}
