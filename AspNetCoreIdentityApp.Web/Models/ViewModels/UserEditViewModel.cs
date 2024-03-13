using AspNetCoreIdentityApp.Web.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Models.ViewModels
{
    public class UserEditViewModel
    {
        [Display(Name = "Kullanıcı Adı :")]
        [Required(ErrorMessage = "Kullanıcı Ad Alanı boş bırakılamaz")]
        public string UserName { get; set; }
        [Display(Name = "Email :")]
        [Required(ErrorMessage = "Email Alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Yanlış formatta mail girdiniz")]
        public string Email { get; set; }
        [Display(Name = "Telefon :")]
        [Required(ErrorMessage = "Telefon Alanı boş bırakılamaz")]
        public string Phone { get; set; }
        [Display(Name = "Doğum Tarihi :")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Şehir :")]
        public string City { get; set; }
        [Display(Name = "Profil Resmi :")]
        public IFormFile Picture { get; set; }
        [Display(Name = "Cinsiyet :")]
        public Gender Gender { get; set; }

    }
}
