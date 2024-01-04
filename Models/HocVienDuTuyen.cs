namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HocVienDuTuyen")]
    public partial class HocVienDuTuyen
    {
        [Key]
        public long DuTuyen_ID { get; set; }

        public long? HocVien_ID { get; set; }

        public int? DuTuyen_TrangThai { get; set; }

        [StringLength(2500)]
        public string DuTuyen_ThongBaoKiemDuyet { get; set; }

        public int? Dxt_ID { get; set; }

        public int? DuTuyen_MaNghienCuu { get; set; }

        public int? Nganh_Mt_ID { get; set; }

        public int? HocVien_DKDTNgoaiNgu { get; set; }

        [StringLength(250)]
        public string HocVien_VanBangNgoaiNgu { get; set; }

        public int? HocVien_DoiTuongDuThi { get; set; }

        [StringLength(2500)]
        public string HocVien_SoYeuLyLich { get; set; }

        [StringLength(2500)]
        public string HocVien_MCBangDaiHoc { get; set; }

        [StringLength(2500)]
        public string HocVien_MCBangDiem { get; set; }

        [StringLength(2500)]
        public string HocVien_MCGiayKhamSucKhoe { get; set; }

        [StringLength(2500)]
        public string HocVien_Anh46 { get; set; }

        [StringLength(2500)]
        public string HocVien_MCCCNN { get; set; }

        [StringLength(2500)]
        public string HocVien_MCKhac { get; set; }

        [StringLength(100)]
        public string DuTuyen_NgayDangKy { get; set; }

        [StringLength(2500)]
        public string DuTuyen_GhiChu { get; set; }

        [StringLength(20)]
        public string HocVien_LePhi_MaThamChieu { get; set; }

        [StringLength(2500)]
        public string HocVien_LePhi_TepMinhChung { get; set; }

        [StringLength(100)]
        public string HocVien_LePhi_NgayNop { get; set; }
        public int? HocVien_LePhi_TrangThai { get; set; }

        public int? HocVien_SoHoSo { get; set; }
        public string DuTuyen_ThongTinHoSoMinhChung { get; set; }
        public virtual DotXetTuyen DotXetTuyen { get; set; }

        public virtual HocVienDangKy HocVienDangKy { get; set; }

        public virtual NganhMaster NganhMaster { get; set; }
    }
}
