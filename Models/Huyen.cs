namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Huyen")]
    public partial class Huyen
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Huyen()
        {
            Xas = new HashSet<Xa>();
        }

        [Key]
        public int Huyen_ID { get; set; }

        [StringLength(50)]

        [Display(Name = "Mã huyện")]
        public string Huyen_MaHuyen { get; set; }

        [Display(Name = "Tên huyện")]
        [StringLength(200)]
        public string Huyen_TenHuyen { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(200)]
        public string Huyen_GhiChu { get; set; }

        [Display(Name = "Tỉnh")]
        public int? Tinh_ID { get; set; }

        public virtual Tinh Tinh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Xa> Xas { get; set; }
    }
}
