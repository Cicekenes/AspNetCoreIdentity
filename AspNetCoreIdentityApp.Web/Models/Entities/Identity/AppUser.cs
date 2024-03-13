using AspNetCoreIdentityApp.Web.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Models.Entities.Identity
{
    public class AppUser:IdentityUser
    {
        public string City { get; set; }
        public string Picture { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}
