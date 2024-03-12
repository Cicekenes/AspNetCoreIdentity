using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace AspNetCoreIdentityApp.Web.Localizations
{
    public class LocalizationIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError { Code= "DuplicateUserName",Description=$"Bu kullanıcı adı daha önce başka bir kullanıcı tarafından alınmıştır" };
            //return base.DuplicateUserName(userName);
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError() { Code="DuplicateEmail",Description=$"Bu mail adresi daha önce başka bir kullanıcı tarafından alınmıştır"};
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError() { Code = "PasswordTooShort", Description = $"Şifre en az 6 karakterli olmalıdır." };
        }
    }
}
