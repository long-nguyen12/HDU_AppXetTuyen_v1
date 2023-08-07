namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ToHopMon")]
    public partial class ToHopMon
    {
        public ToHopMon()
        {
            DangKyDuThiNangKhieus = new HashSet<DangKyDuThiNangKhieu>();
            DangKyXetTuyenHBs = new HashSet<DangKyXetTuyenHB>();
            DangKyXetTuyenKQTQGs = new HashSet<DangKyXetTuyenKQTQG>();         
            ToHopMonNganhs = new HashSet<ToHopMonNganh>();
        }


        [Key]
        public int Thm_ID { get; set; }

        [Display(Name = "Mã tổ hợp")]
        [StringLength(50)]
        public string Thm_MaToHop { get; set; }

        [Display(Name = "Tên tổ hợp")]
        [StringLength(200)]
        public string Thm_TenToHop { get; set; }

        [Display(Name = "Môn 1")]
        [StringLength(200)]
        public string Thm_Mon1 { get; set; }

        [Display(Name = "Môn 2")]
        [StringLength(200)]
        public string Thm_Mon2 { get; set; }

        [Display(Name = "Môn 3")]
        [StringLength(200)]
        public string Thm_Mon3 { get; set; }

        [Display(Name = "Mã - Tên tổ hợp")]
        [StringLength(250)]
        public string Thm_MaTen { get; set; }
        public int? Thm_Thi_NK { get; set; }
        public virtual ICollection<DangKyDuThiNangKhieu> DangKyDuThiNangKhieus { get; set; }
        public virtual ICollection<DangKyXetTuyenHB> DangKyXetTuyenHBs { get; set; }
        public virtual ICollection<DangKyXetTuyenKQTQG> DangKyXetTuyenKQTQGs { get; set; }    
        public virtual ICollection<ToHopMonNganh> ToHopMonNganhs { get; set; }
    }
}
