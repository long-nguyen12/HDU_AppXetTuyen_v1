namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhuVuc")]
    public partial class KhuVuc
    {
        
        public KhuVuc()
        {
            ThiSinhDangKies = new HashSet<ThiSinhDangKy>();
        }

        [Key]
        public int KhuVuc_ID { get; set; }
        [StringLength(250)]
        public string KhuVuc_Ten { get; set; }

        [Required(ErrorMessage = "Điểm ưu tiên phải lớn hơn 0")]
        public double? KhuVuc_DiemUuTien { get; set; }
        [StringLength(250)]
        public string KhuVuc_GhiChu { get; set; }        
        public virtual ICollection<ThiSinhDangKy> ThiSinhDangKies { get; set; }
    }
}
