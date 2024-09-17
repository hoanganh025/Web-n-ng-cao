using System.ComponentModel.DataAnnotations;

namespace BTL_LTWebNC.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "Không được để trống")]
        public string? UserPassword { get; set; }
    }
}
