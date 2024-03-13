using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Models
{
    public class RoleCreateViewModel
    {
        [Display(Name = "Rol İsmi :")]
        [Required(ErrorMessage = "Rol ismi alanı boş bırakılamaz")]
        public string Name { get; set; }
    }
}
