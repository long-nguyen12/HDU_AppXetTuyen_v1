namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DoiTuong")]
    public partial class DoiTuong
    {
       public DoiTuong()
        {
            ThiSinhDangKies = new HashSet<ThiSinhDangKy>();
        }

        [Key]
        public int DoiTuong_ID { get; set; }

        [StringLength(250)]
        public string DoiTuong_Ten { get; set; }

        public double? DoiTuong_DiemUuTien { get; set; }

        [StringLength(2000)]
        public string DoiTuong_GhiChu { get; set; }

       public virtual ICollection<ThiSinhDangKy> ThiSinhDangKies { get; set; }
    }
}
