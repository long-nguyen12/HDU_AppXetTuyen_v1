namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Nganh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nganh()
        {
            DangKyXetTuyens = new HashSet<DangKyXetTuyen>();
        }

        [Key]
        public int Nganh_ID { get; set; }

        [StringLength(50)]
        public string Nganh_MaNganh { get; set; }

        [StringLength(500)]
        public string NganhTenNganh { get; set; }

        public int? Khoa_ID { get; set; }

        [StringLength(500)]
        public string Nganh_GhiChu { get; set; }

        public int? KhoiNganh_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyen> DangKyXetTuyens { get; set; }

        public virtual KhoiNganh KhoiNganh { get; set; }
    }
}
