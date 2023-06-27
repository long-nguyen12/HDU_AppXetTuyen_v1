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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhuVuc()
        {
            DangKyXetTuyens = new HashSet<DangKyXetTuyen>();
            ThiSinhDangKies = new HashSet<ThiSinhDangKy>();
        }

        [Key]
        public int KhuVuc_ID { get; set; }

        [StringLength(250)]
        public string KhuVuc_Ten { get; set; }

        public double? KhuVuc_DiemUuTien { get; set; }

        [StringLength(250)]
        public string KhuVuc_GhiChu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyen> DangKyXetTuyens { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThiSinhDangKy> ThiSinhDangKies { get; set; }
    }
}
