namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DangKyXetTuyenKQTQG")]
    public partial class DangKyXetTuyenKQTQG
    {
        [Key]
        public long Dkxt_KQTQG_ID { get; set; }

        public long? ThiSinh_ID { get; set; }

        public int? Ptxt_ID { get; set; }
        public int? DotXT_ID { get; set; }
        public int? Nganh_ID { get; set; }
        public int? Thm_ID { get; set; }
        public int? Dkxt_KQTQG_NguyenVong { get; set; }

        public int? Dkxt_KQTQG_NamTotNghiep { get; set; }

        [StringLength(500)]
        public string Dkxt_KQTQG_GhiChu { get; set; }

        [StringLength(1000)]
        public string Dkxt_KQTQG_Diem_M1 { get; set; }

        [StringLength(1000)]
        public string Dkxt_KQTQG_Diem_M2 { get; set; }

        [StringLength(1000)]
        public string Dkxt_KQTQG_Diem_M3 { get; set; }

        [StringLength(1000)]
        public string Dkxt_KQTQG_Diem_Tong { get; set; }

        [StringLength(1000)]
        public string Dkxt_KQTQG_TongDiem_Full { get; set; }

        [StringLength(100)]
        public string Dkxt_KQTQG_NgayDangKy { get; set; }
        [StringLength(4000)]
        public string Dkxt_KQTQG_MinhChung_CNTotNghiep { get; set; }
        [StringLength(4000)]
        public string Dkxt_KQTQG_MinhChung_HocBa { get; set; }
        [StringLength(4000)]
        public string Dkxt_KQTQG_MinhChung_BangTN { get; set; }
        [StringLength(4000)] 
        public string Dkxt_KQTQG_MinhChung_CCCD { get; set; }
        
        [StringLength(4000)]
        public string Dkxt_KQTQG_MinhChung_UuTien { get; set; }
        public string Dkxt_KQTQG_KinhPhi_SoThamChieu { get; set; }
        public string Dkxt_KQTQG_KinhPhi_TepMinhChung { get; set; }
        public string Dkxt_KQTQG_KinhPhi_NgayThang_NopMC { get; set; }
        public string Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC { get; set; }
        public int? Dkxt_KQTQG_TrangThai_KinhPhi { get; set; }
        public int? Dkxt_KQTQG_TrangThai_HoSo { get; set; }
        public int? Dkxt_KQTQG_TrangThai_KetQua { get; set; }

        public string Dkxt_ThongBaoKiemDuyet_LePhi { get; set; }
        public string Dkxt_ThongBaoKiemDuyet_HoSo { get; set; }
        public string Dkxt_KinhPhi_NoiDungGiaoDich { get; set; }
        public virtual DotXetTuyen DotXetTuyen { get; set; }

        public virtual Nganh Nganh { get; set; }

        public virtual PhuongThucXetTuyen PhuongThucXetTuyen { get; set; }

        public virtual ThiSinhDangKy ThiSinhDangKy { get; set; }

        public virtual ToHopMon ToHopMon { get; set; }
    }
}
