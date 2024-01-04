namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DangKyDuThiNangKhieu")]
    public partial class DangKyDuThiNangKhieu
    {
        [Key]
      
        public long Dkdt_NK_ID { get; set; }
        public long ThiSinh_ID { get; set; }
        public int? Nganh_ID { get; set; }
        public int? Thm_ID { get; set; }
       
        public int? Ptxt_ID { get; set; }
        public int? DotXT_ID { get; set; }
        public string Dkdt_NK_GhiChu { get; set; }
        public string Dkdt_NK_MonThi { get; set; }
        public string Dkdt_NK_NgayDangKy { get; set; }
     
        public string Dkdt_NK_MinhChung_CCCD { get; set; }

        public string Dkdt_LePhi_MinhChung_MaThamChieu { get; set; }
        public string Dkdt_LePhi_MinhChung_Tep { get; set; }
        public string Dkdt_LePhi_MinhChung_NgayGui { get; set; }

        public int Dkdt_TrangThai_LePhi { get;set; }
        public int Dkdt_TrangThai_HoSo { get; set; }
        public int Dkdt_TrangThai_KetQua { get; set; }
        public string Dkxt_KinhPhi_NoiDungGiaoDich { get; set; }
        public string Dkdt_ThongBaoKiemDuyet_HoSo { get; set; }
        public string Dkdt_NgayThang_CheckLePhi { get; set; }
        public string Dkdt_NgayThang_CheckHoSo { get; set; }

        public virtual DotXetTuyen DotXetTuyen { get; set; }
        public virtual Nganh Nganh { get; set; }
        public virtual ThiSinhDangKy ThiSinhDangKy { get; set; }
        public virtual ToHopMon ToHopMon { get; set; }
    }
}
