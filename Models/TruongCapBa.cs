namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TruongCapBa")]
    public partial class TruongCapBa
    {
        [Key]
        public int Truong_ID { get; set; }

        [StringLength(50)]
        public string Truong_Ma { get; set; }

        [StringLength(200)]
        public string Truong_Ten { get; set; }

        [StringLength(200)]
        public string Truong_MaTen { get; set; }

        [StringLength(200)]
        public string Truong_GhiChu { get; set; }

        public int? Tinh_ID { get; set; }

        public virtual Tinh Tinh { get; set; }
    }
}
