using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnTap_Net104.Models
{
    public class Account
    {
        [Required]
        [StringLength(50, MinimumLength =10, ErrorMessage ="Độ dài username phải từ 10 - 50 kí tự")]
        [Key]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài password phải từ 6 - 50 kí tự")]
        public string Password { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [DisplayName("Số điện thoại")]

        public string Phone { get; set; }
        [DisplayName("Địa chỉ")]

        public string Address { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual List<Bill> Bills { get; set; }

    }
}
