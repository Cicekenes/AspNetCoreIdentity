using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "Yeni Şifre :")]
        [Required(ErrorMessage = "Şifre Alanı boş bırakılamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre Tekrar :")]
        [Required(ErrorMessage = "Şifre Tekrar Alanı boş bırakılamaz")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler aynı değildir.")]
        public string PasswordConfirm { get; set; }
    }
}
