namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Xa")]
    public partial class Xa
    {
        [Key]
        public int Xa_ID { get; set; }

        [Display(Name = "Mã xã")]
        [StringLength(50)]
        public string Xa_Ma { get; set; }

        [Display(Name = "Tên xã")]
        [StringLength(200)]
        public string Xa_Ten { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(200)]
        public string Xa_GhiChu { get; set; }

        [Display(Name = "Huyện")]
        public int? Huyen_ID { get; set; }

        public virtual Huyen Huyen { get; set; }
      
    }
}
