﻿namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KinhPhi")]
    public partial class KinhPhi
    {
        [Key]
        public long KinhPhi_ID { get; set; }

        public long? ThiSinh_ID { get; set; }

        public long? Dkxt_ID { get; set; }

        public long? Dkxt_KQTQG_ID { get; set; }

        public long? Dkxt_TT_ID { get; set; }

        public long? Dkxt_Khac_ID { get; set; }

        public long? Dkdt_NK_ID { get; set; }

        public int? Ptxt_ID { get; set; }

        public int? Dxt_ID { get; set; }

        [StringLength(10)]
        public string KinhPhi_NgayThang { get; set; }

        [StringLength(20)]
        public string KinhPhi_SoTC { get; set; }

        [StringLength(4000)]
        public string KinhPhi_TepMinhChung { get; set; }

        public int? KinhPhi_TrangThai { get; set; }

        [StringLength(4000)]
        public string KinhPhi_GhiChu { get; set; }

        public virtual DangKyDuThiNangKhieu DangKyDuThiNangKhieu { get; set; }

        public virtual DangKyXetTuyen DangKyXetTuyen { get; set; }

        public virtual DangKyXetTuyenKhac DangKyXetTuyenKhac { get; set; }

        public virtual DangKyXetTuyenKQTQG DangKyXetTuyenKQTQG { get; set; }

        public virtual DangKyXetTuyenThang DangKyXetTuyenThang { get; set; }

        public virtual ThiSinhDangKy ThiSinhDangKy { get; set; }
    }
}