namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhoiNganh")]
    public partial class KhoiNganh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhoiNganh()
        {
            Nganhs = new HashSet<Nganh>();
        }

        [Key]
        public int KhoiNganh_ID { get; set; }

        [StringLength(50)]
        public string KhoiNganh_Ten { get; set; }

        [StringLength(500)]
        public string KhoiNganh_GhiChu { get; set; }

        public int? KhoiNganh_TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Nganh> Nganhs { get; set; }
    }
}
