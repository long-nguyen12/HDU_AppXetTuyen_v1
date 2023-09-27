namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DangKyXetTuyenHB")]
    public partial class DangKyXetTuyenHB
    {
        [Key]
        public long Dkxt_HB_ID { get; set; }
        public long? ThiSinh_ID { get; set; }
        public int? DotXT_ID { get; set; }
        public int? Ptxt_ID { get; set; }
        public int? Nganh_ID { get; set; }
        public int? Thm_ID { get; set; }     
        public int? Dkxt_HB_NguyenVong { get; set; } 
        [StringLength(500)]
        public string Dkxt_HB_GhiChu { get; set; }      
        public string Dkxt_HB_Diem_M1 { get; set; }       
        public string Dkxt_HB_Diem_M2 { get; set; }      
        public string Dkxt_HB_Diem_M3 { get; set; }      
        public string Dkxt_HB_Diem_Tong { get; set; }      
        public string Dkxt_HB_Diem_Tong_Full { get; set; }   
        public string Dkxt_HB_NgayDangKy { get; set; }
        public string Dkxt_HB_MinhChung_HB { get; set; }      
        public string Dkxt_HB_MinhChung_Bang { get; set; }    
        public string Dkxt_HB_MinhChung_CCCD { get; set; }       
        public string Dkxt_HB_MinhChung_UuTien { get; set; }
        public string Dkxt_HB_KinhPhi_SoThamChieu { get; set; }
        public string Dkxt_HB_KinhPhi_TepMinhChung { get; set; }
        public string Dkxt_HB_KinhPhi_NgayThang_NopMC { get; set; }
        public string Dkxt_HB_KinhPhi_NgayThang_CheckMC { get; set; }
        public int? Dkxt_HB_TrangThai_KinhPhi { get; set; }
        public int? Dkxt_HB_TrangThai_HoSo { get; set; }
        public int? Dkxt_HB_TrangThai_KetQua { get; set; }
        public virtual DotXetTuyen DotXetTuyen { get; set; }

        public virtual Nganh Nganh { get; set; }

        public virtual PhuongThucXetTuyen PhuongThucXetTuyen { get; set; }

        public virtual ThiSinhDangKy ThiSinhDangKy { get; set; }

        public virtual ToHopMon ToHopMon { get; set; }
    }
}
