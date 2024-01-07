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

        public string Truong_MaTinh { get; set; }

        public string Truong_TenTinh { get; set; }

        public string Truong_MaHuyen { get; set; }

        public string Truong_TenHuyen { get; set; }

        public string Truong_MaTruong { get; set; }

        public string Truong_TenTruong { get; set; }

        public string Truong_DiaChi { get; set; }

        public string Truong_KhuVuc_Ma { get; set; }

        public string Truong_KhuVuc_Ten { get; set; }

        public string Truong_DanToc_NoiTru { get; set; }

        public string Truong_GhiChu { get; set; }

        public string Truong_TenTruong_Eng { get; set; }
        public string Truong_TenTinh_Eng { get; set; }
    }
}
