using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Models.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Display(Name = "Email :")]
        [Required(ErrorMessage = "Email Alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Yanlış formatta mail girdiniz")]
        public string Email { get; set; }
    }
}
