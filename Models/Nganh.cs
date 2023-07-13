namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Nganh")]
    public partial class Nganh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nganh()
        {
         
            DangKyXetTuyens = new HashSet<DangKyXetTuyen>();
            ToHopMonNganhs = new HashSet<ToHopMonNganh>();
            DangKyDuThiNangKhieus = new HashSet<DangKyDuThiNangKhieu>();
            DangKyXetTuyenKQTQGs = new HashSet<DangKyXetTuyenKQTQG>();
            DangKyXetTuyenThangs = new HashSet<DangKyXetTuyenThang>();
            DangKyXetTuyenKhacs = new HashSet<DangKyXetTuyenKhac>();
        }

        [Key]
        public int Nganh_ID { get; set; }

        [Display(Name = "Mã ngành")]
        [StringLength(50)]
        public string Nganh_MaNganh { get; set; }

        [Display(Name = "Tên ngành")]
        [StringLength(500)]
        public string NganhTenNganh { get; set; }

        [Display(Name = "Khoa")]
        public int Khoa_ID { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string Nganh_GhiChu { get; set; }

        [Display(Name = "Khối ngành")]
        public int? KhoiNganh_ID { get; set; }

        public int? Nganh_ThiNK { get; set; }

        public virtual Khoa Khoa { get; set; }

        public virtual KhoiNganh KhoiNganh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyen> DangKyXetTuyens { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ToHopMonNganh> ToHopMonNganhs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyenKQTQG> DangKyXetTuyenKQTQGs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyenThang> DangKyXetTuyenThangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyXetTuyenKhac> DangKyXetTuyenKhacs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyDuThiNangKhieu> DangKyDuThiNangKhieus { get; set; }
    }
}
