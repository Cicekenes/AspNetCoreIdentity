using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Models.ViewModels
{
    public class SignInViewModel
    {
        public SignInViewModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public SignInViewModel()
        {
            
        }

        [Display(Name = "Email :")]
        [Required(ErrorMessage = "Email Alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Yanlış formatta mail girdiniz")]
        public string Email { get; set; }
        [Display(Name ="Password :")]
        [Required(ErrorMessage ="Şifre alanı boş bırakılamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
