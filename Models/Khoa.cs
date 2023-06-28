namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Khoa")]
    public partial class Khoa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Khoa()
        {

            Nganhs = new HashSet<Nganh>();
        }

        [Key]

        public int Khoa_ID { get; set; }

        [StringLength(500)]
        public string Khoa_TenKhoa { get; set; }

        [StringLength(500)]
        public string Khoa_DienThoai { get; set; }

        [StringLength(500)]
        public string Khoa_TruongKhoa { get; set; }

        [StringLength(500)]
        public string Khoa_GhiChu { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Nganh> Nganhs { get; set; }     
    }
}
