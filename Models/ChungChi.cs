namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChungChi")]
    public partial class ChungChi
    {
        [Key]
        public int ChungChi_ID { get; set; }

        [StringLength(500)]
        public string ChungChi_Ten { get; set; }

        [StringLength(500)]
        public string ChungChi_TruongTCThi { get; set; }

        public double? ChungChi_ThangDiem { get; set; }

        [StringLength(1000)]
        public string ChungChi_PhuongThuc { get; set; }

        public int? ChungChi_TrangThai { get; set; }

        [StringLength(1000)]
        public string ChungChi_GhiChu { get; set; }
    }
}