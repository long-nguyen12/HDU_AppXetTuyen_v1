namespace HDU_AppXetTuyen.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdminAccount")]
    public partial class AdminAccount
    {
        [Key]
        public int Admin_ID { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Tên tài khoản là bắt buộc nhập")]
        public string Admin_Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc nhập")]
        [StringLength(500)]
        public string Admin_Pass { get; set; }

        [StringLength(500)]
        public string Admin_Ho { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Tên là bắt buộc nhập")]
        public string Admin_Ten { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Quyền là bắt buộc chọn")]
        public string Admin_Quyen { get; set; }

        public int? Khoa_ID { get; set; }
       
        [StringLength(500)]
        public string Admin_Note { get; set; }
    }
}
