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
        
        public Nganh()
        {
            DangKyDuThiNangKhieus = new HashSet<DangKyDuThiNangKhieu>();
            DangKyXetTuyenHBs = new HashSet<DangKyXetTuyenHB>();
            DangKyXetTuyenKhacs = new HashSet<DangKyXetTuyenKhac>();
            DangKyXetTuyenKQTQGs = new HashSet<DangKyXetTuyenKQTQG>();
            DangKyXetTuyenThangs = new HashSet<DangKyXetTuyenThang>();
            ToHopMonNganhs = new HashSet<ToHopMonNganh>();
        }

     
        [Key]
        public int Nganh_ID { get; set; }

        [Display(Name = "Mã ngành")]
        [StringLength(50)]
        public string Nganh_MaNganh { get; set; }

        [Display(Name = "Tên ngành")]
        [StringLength(500)]
        public string Nganh_TenNganh { get; set; }

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
        
        public virtual ICollection<DangKyDuThiNangKhieu> DangKyDuThiNangKhieus { get; set; }
        
        public virtual ICollection<DangKyXetTuyenHB> DangKyXetTuyenHBs { get; set; }
        
        public virtual ICollection<DangKyXetTuyenKhac> DangKyXetTuyenKhacs { get; set; }
        
        public virtual ICollection<DangKyXetTuyenKQTQG> DangKyXetTuyenKQTQGs { get; set; }
        
        public virtual ICollection<DangKyXetTuyenThang> DangKyXetTuyenThangs { get; set; }   
        public virtual ICollection<ToHopMonNganh> ToHopMonNganhs { get; set; }
    }
}
