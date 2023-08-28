namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NganhMaster")]
    public partial class NganhMaster
    {
        public NganhMaster()
        {
            HocVienDuTuyens = new HashSet<HocVienDuTuyen>();
        }

        [Key]
        public int Nganh_Mt_ID { get; set; }

        [StringLength(50)]
        public string Nganh_Mt_MaNganh { get; set; }

        [StringLength(1000)]
        public string Nganh_Mt_TenNganh { get; set; }

        [StringLength(1000)]
        public string Nganh_Mt_NghienCuu_Ten { get; set; }

        public int? Nganh_Mt_NghienCuu_Ma { get; set; }

        public int? Khoa_ID { get; set; }

        [StringLength(1000)]
        public string Nganh_Mt_TenKhoa { get; set; }

        public int? Nganh_Mt_TrangThai { get; set; }

        [StringLength(1000)]
        public string Nganh_Mt_GhiChu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HocVienDuTuyen> HocVienDuTuyens { get; set; }
    }
}
