namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tinh")]
    public partial class Tinh
    {
        
        public Tinh()
        {
            Huyens = new HashSet<Huyen>();
        }

        [Key]
        public int Tinh_ID { get; set; }

        [Display(Name = "Mã tỉnh")]
        [StringLength(250)]
        public string Tinh_Ma { get; set; }

        [Display(Name = "Tên tỉnh")]
        [StringLength(250)]
        public string Tinh_Ten { get; set; }

        [Display(Name = "Mã tỉnh - Tên tỉnh")]
        [StringLength(250)]
        public string Tinh_MaTen { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(250)]
        public string Tinh_GhiChu { get; set; }

        
        public virtual ICollection<Huyen> Huyens { get; set; }
    }
}
