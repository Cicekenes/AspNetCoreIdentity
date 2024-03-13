using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Models.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Display(Name = "Eski Şifre :")]
        [Required(ErrorMessage = "Eski Şifre Alanı boş bırakılamaz")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Şifreniz en az 6 karakter olabilir")]
        public string PasswordOld { get; set; }
        [Display(Name = "Yeni Şifre :")]
        [Required(ErrorMessage = "Yeni Şifre Alanı boş bırakılamaz")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        [DataType(DataType.Password)]
        public string PasswordNew { get; set; }
        [Display(Name = "Yeni Şifre Tekrar :")]
        [Required(ErrorMessage = "Yeni Şifre Tekrar Alanı boş bırakılamaz")]
        [DataType(DataType.Password)]
        [Compare(nameof(PasswordNew), ErrorMessage = "Şifreler aynı değildir.")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string PasswordConfirm { get; set; }
    }
}
