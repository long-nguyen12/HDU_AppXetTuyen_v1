namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DotXetTuyen")]
    public partial class DotXetTuyen
    {
        public DotXetTuyen()
        {
            DangKyDuThiNangKhieus = new HashSet<DangKyDuThiNangKhieu>();
            DangKyXetTuyenHBs = new HashSet<DangKyXetTuyenHB>();
            DangKyXetTuyenKhacs = new HashSet<DangKyXetTuyenKhac>();
            DangKyXetTuyenKQTQGs = new HashSet<DangKyXetTuyenKQTQG>();
            DangKyXetTuyenThangs = new HashSet<DangKyXetTuyenThang>();
            HocVienDuTuyens = new HashSet<HocVienDuTuyen>();
        }


        [Key]
        public int Dxt_ID { get; set; }
        // để phân biệt là xét tuyển đại học hay thi năng khiếu hay là sau đại học
        [Display(Name = "Loại")]
        public int? Dxt_Classify { get; set; }

        [StringLength(250)]
        [Display(Name = "Tên đợt dự tuyển")]
        public string Dxt_Ten { get; set; }

        [Display(Name = "Trạng thái")]
        public int? Dxt_TrangThai_Xt { get; set; } // biểu thị cho việc thiết lập đợt xét tuyển là hiện tại
        [Display(Name = "Trạng thái")]
        public int? Dxt_TrangThai_TNK { get; set; } // biểu thị cho việc thiết lập đợt thi NK hiện tại

        [StringLength(250)]
        [Display(Name = "Thời gian bắt đầu")]
        public string Dxt_ThoiGian_BatDau { get; set; } // thời gian bắt đầu của xét tuyển hoặc thi

        [StringLength(250)]
        [Display(Name = "Thời gian kết thúc")]
        public string Dxt_ThoiGian_KetThuc { get; set; } // thời gian kết thúc của xét tuyển hoặc thi

        [StringLength(250)]
        [Display(Name = "Ghi chú")]
        public string Dxt_GhiChu { get; set; }
        [Display(Name = "Năm học")]
        public int? NamHoc_ID { get; set; }

        [Display(Name = "Năm học")]
        public virtual NamHoc NamHoc { get; set; }
        public virtual ICollection<DangKyDuThiNangKhieu> DangKyDuThiNangKhieus { get; set; }
        public virtual ICollection<DangKyXetTuyenHB> DangKyXetTuyenHBs { get; set; }
        public virtual ICollection<DangKyXetTuyenKhac> DangKyXetTuyenKhacs { get; set; }
        public virtual ICollection<DangKyXetTuyenKQTQG> DangKyXetTuyenKQTQGs { get; set; }
        public virtual ICollection<DangKyXetTuyenThang> DangKyXetTuyenThangs { get; set; }    
        public virtual ICollection<HocVienDuTuyen> HocVienDuTuyens { get; set; }

    }
}
