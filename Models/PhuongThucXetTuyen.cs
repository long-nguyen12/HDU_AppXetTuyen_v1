namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhuongThucXetTuyen")]
    public partial class PhuongThucXetTuyen
    {
        
        public PhuongThucXetTuyen()
        {
            DangKyXetTuyenHBs = new HashSet<DangKyXetTuyenHB>();
            DangKyXetTuyenKhacs = new HashSet<DangKyXetTuyenKhac>();
            DangKyXetTuyenKQTQGs = new HashSet<DangKyXetTuyenKQTQG>();
            DangKyXetTuyenThangs = new HashSet<DangKyXetTuyenThang>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Ptxt_ID { get; set; }

        [StringLength(1000)]
        public string Ptxt_TenPhuongThuc { get; set; }

        [StringLength(200)]
        public string Ptxt_GhiChu { get; set; }

        
        public virtual ICollection<DangKyXetTuyenHB> DangKyXetTuyenHBs { get; set; }

        
        public virtual ICollection<DangKyXetTuyenKhac> DangKyXetTuyenKhacs { get; set; }

        
        public virtual ICollection<DangKyXetTuyenKQTQG> DangKyXetTuyenKQTQGs { get; set; }

        
        public virtual ICollection<DangKyXetTuyenThang> DangKyXetTuyenThangs { get; set; }
    }
}
