namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("ToHopMonNganh")]
    public partial class ToHopMonNganh
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Nganh_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Thm_ID { get; set; }

        public int? Thm_N_TrangThai { get; set; }

        [StringLength(500)]
        public string Thm_N_GhiChu { get; set; }

        public virtual Nganh Nganh { get; set; }

        public virtual ToHopMon ToHopMon { get; set; }
    }
}
