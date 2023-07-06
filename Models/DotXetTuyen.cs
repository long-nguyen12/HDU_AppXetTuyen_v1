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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DotXetTuyen()
        {
            DangKyXetTuyens = new HashSet<DangKyXetTuyen>();
            ThiSinhDangKies = new HashSet<ThiSinhDangKy>();       
        }

        [Key]
        public int Dxt_ID { get; set; }

        [StringLength(250)]
        public string Dxt_Ten { get; set; }

        public int? Dxt_TrangThai { get; set; }

        [StringLength(250)]
        public string Dxt_GhiChu { get; set; }

        public int? NamHoc_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyen> DangKyXetTuyens { get; set; }

        public virtual NamHoc NamHoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThiSinhDangKy> ThiSinhDangKies { get; set; }
    }
}
