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
        }

        [Key]
        public int Thm_ID { get; set; }

        [StringLength(50)]
        public string Thm_MaToHop { get; set; }

        [StringLength(200)]
        public string Thm_TenToHop { get; set; }

        public int? Thm_mon1_ID { get; set; }

        public int? Thm_mon2_ID { get; set; }

        public int? Thm_mon3_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyen> DangKyXetTuyens { get; set; }

        public virtual MonHoc MonHoc { get; set; }

        public virtual MonHoc MonHoc1 { get; set; }

        public virtual MonHoc MonHoc2 { get; set; }
    }
}
