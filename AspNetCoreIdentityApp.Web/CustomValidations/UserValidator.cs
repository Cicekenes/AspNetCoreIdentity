using AspNetCoreIdentityApp.Web.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            if (int.TryParse(user.UserName[0].ToString(), out _))
            {
                errors.Add(new IdentityError() { Code="UserNameContainFirstLetterDigit",Description="Kullanıcı adı sayısal değerle başlayamaz."});
            }

            if (errors.Any())
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
