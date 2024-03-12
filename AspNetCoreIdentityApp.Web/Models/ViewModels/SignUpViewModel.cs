using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Models.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel(string userName, string email, string phone, string password)
        {
            UserName = userName;
            Email = email;
            Phone = phone;
            Password = password;
        }
        public SignUpViewModel()
        {
            
        }
        [Display(Name ="Kullanıcı Adı :")]
        [Required(ErrorMessage ="Kullanıcı Ad Alanı boş bırakılamaz")]
        public string UserName { get; set; }
        [Display(Name = "Email :")]
        [Required(ErrorMessage = "Email Alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage ="Yanlış formatta mail girdiniz")]
        public string Email { get; set; }
        [Display(Name = "Telefon :")]
        [Required(ErrorMessage = "Telefon Alanı boş bırakılamaz")]
        public string Phone { get; set; }
        [Display(Name = "Şifre :")]
        [Required(ErrorMessage = "Şifre Alanı boş bırakılamaz")]
        public string Password { get; set; }
        [Display(Name = "Şifre Tekrar :")]
        [Required(ErrorMessage = "Şifre Tekrar Alanı boş bırakılamaz")]
        [Compare(nameof(Password),ErrorMessage ="Şifreler aynı değildir.")]
        public string PasswordConfirm { get; set; }
    }
}
