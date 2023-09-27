namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DangKyXetTuyenKhac")]
    public partial class DangKyXetTuyenKhac
    {
        [Key]       
        public long Dkxt_ID { get; set; }
        public int? ChungChi_ID { get; set; }
        public long? ThiSinh_ID { get; set; }
        public int? DotXT_ID { get; set; }
        public int? Ptxt_ID { get; set; }
        public int? Nganh_ID { get; set; }  
        public int? Dkxt_NguyenVong { get; set; }
        public string Dkxt_GhiChu { get; set; }
        [StringLength(50)]
        public string Dkxt_ToHopXT { get; set; }
        [StringLength(1000)]
        public string Dkxt_DonViToChuc { get; set; }
        public double? Dkxt_KetQuaDatDuoc { get; set; }
        public double? Dkxt_TongDiem { get; set; }
        [StringLength(1000)]
        public string Dkxt_NgayDuThi { get; set; }       
        [StringLength(4000)]
        public string Dkxt_MinhChung_HB { get; set; }     
        [StringLength(4000)]
        public string Dkxt_MinhChung_CCCD { get; set; }
        [StringLength(4000)]
        public string Dkxt_MinhChung_Bang { get; set; }
        [StringLength(4000)]
        public string Dkxt_MinhChung_KetQua { get; set; }
        [StringLength(4000)]
        public string Dkxt_MinhChung_UuTien { get; set; }
        [StringLength(200)]
        public string Dkxt_NgayDangKy { get; set; }
        public string Dkxt_KinhPhi_SoThamChieu { get; set; }
        public string Dkxt_KinhPhi_TepMinhChung { get; set; }
        public string Dkxt_KinhPhi_NgayThang_NopMC { get; set; }
        public string Dkxt_KinhPhi_NgayThang_CheckMC { get; set; }
        public int? Dkxt_TrangThai_KinhPhi { get; set; }
        public int? Dkxt_TrangThai_HoSo { get; set; }
        public int? Dkxt_TrangThai_KetQua { get; set; }

        public virtual ChungChi ChungChi { get; set; }
        public virtual DotXetTuyen DotXetTuyen { get; set; }
        public virtual Nganh Nganh { get; set; }
        public virtual PhuongThucXetTuyen PhuongThucXetTuyen { get; set; }
        public virtual ThiSinhDangKy ThiSinhDangKy { get; set; }
    }
}
