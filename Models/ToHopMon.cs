namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ToHopMon")]
    public partial class ToHopMon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ToHopMon()
        {
            DangKyXetTuyens = new HashSet<DangKyXetTuyen>();
            ToHopMonNganhs = new HashSet<ToHopMonNganh>();
        }

        [Key]
        public int Thm_ID { get; set; }

        [StringLength(50)]
        public string Thm_MaToHop { get; set; }

        [StringLength(200)]
        public string Thm_TenToHop { get; set; }

        [StringLength(200)]
        public string Thm_Mon1 { get; set; }

        [StringLength(200)]
        public string Thm_Mon2 { get; set; }

        [StringLength(200)]
        public string Thm_Mon3 { get; set; }

        [StringLength(250)]
        public string Thm_MaTen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyen> DangKyXetTuyens { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ToHopMonNganh> ToHopMonNganhs { get; set; }
    }
}
