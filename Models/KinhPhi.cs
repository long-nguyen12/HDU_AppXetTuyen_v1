namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KinhPhi")]
    public partial class KinhPhi
    {
        [Key]
        public long KinhPhi_ID { get; set; }
        [Display(Name = "Họ tên thí sinh")]
        public long? ThiSinh_ID { get; set; }   
        public long? Dkxt_ID { get; set; }

        [Display(Name = "Đợt xét")]
        public int? Dxt_ID { get; set; }
        [Display(Name = "Loại xét tuyển")]
        public int? Ptxt_ID { get; set; }
        
        [Display(Name = "Nguyện vọng")]
        public int? KinhPhi_NguyenVong { get; set; }

        [Display(Name = "Số tham chiếu")]
        [StringLength(20)]
        public string KinhPhi_SoTC { get; set; }

        [Display(Name = "Ngày nộp MC")]
        [StringLength(10)]
        public string KinhPhi_NgayThang_NopMC { get; set; }

        [Display(Name = "Ngày Kiểm tra MC")]
        [StringLength(10)]
        public string KinhPhi_NgayThang_CheckMC { get; set; }      

        [StringLength(4000)]
        public string KinhPhi_TepMinhChung { get; set; }

        [Display(Name = "Trạng thái")]
        public int? KinhPhi_TrangThai { get; set; }

        [StringLength(4000)]
        public string KinhPhi_GhiChu { get; set; }
        public virtual ThiSinhDangKy ThiSinhDangKy { get; set; }
        public virtual DotXetTuyen DotXetTuyen { get; set; }
        public virtual PhuongThucXetTuyen PhuongThucXetTuyen { get; set; }
    }
}

