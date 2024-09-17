using System.ComponentModel.DataAnnotations;

namespace BTL_LTWebNC.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Chưa nhập tên tài khoản")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Tên người dùng phải có ít nhất 8 ký tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Chưa nhập mật khẩu")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Chưa nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "Chưa nhập Email")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Chưa nhập tên người dùng")]
        public string FullName { get; set; }

        [Required]
        public int RoleID { get; set; }

        [RegularExpression(@"^\d{1}\w{9}$", ErrorMessage = "Verify key phải có 10 ký tự và bắt đầu là số")]
        public string VerifyKey { get; set; }
    }
}
