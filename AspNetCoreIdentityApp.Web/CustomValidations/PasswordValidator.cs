﻿using AspNetCoreIdentityApp.Web.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.CustomValidations
{
    /// <summary>
    /// Şifreye müdahale edip kendimizdce customize edebiliyoruz.
    /// </summary>
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            var errors = new List<IdentityError>();
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError() { Code = "PasswordContainUserName", Description = "Şifre alanı kullanıcı adı içeremez" });
            }
            if (password.ToLower().StartsWith("1234"))
            {
                errors.Add(new IdentityError() { Code="PasswordContain1234",Description= "Şifre alanı ardışık sayılar içeremez"});
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
