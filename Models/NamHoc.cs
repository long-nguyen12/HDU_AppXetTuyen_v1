namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NamHoc")]
    public partial class NamHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NamHoc()
        {
            DotXetTuyens = new HashSet<DotXetTuyen>();
        }

        [Key]
        public int NamHoc_ID { get; set; }

        [StringLength(250)]
        public string NamHoc_Ten { get; set; }

        public int? NamHoc_TrangThai { get; set; }

        [StringLength(250)]
        public string NamHoc_GhiChu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DotXetTuyen> DotXetTuyens { get; set; }
    }
}
