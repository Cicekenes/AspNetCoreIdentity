using AspNetCoreIdentityApp.Web.Models.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.ClaimProviders
{
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            //ClaimsIdentity, claimlerden oluşan kimliktir.
            var identityUser = principal.Identity as ClaimsIdentity;

            var currentUser = await _userManager.FindByNameAsync(identityUser.Name);

            if (string.IsNullOrEmpty(currentUser.City))
            {
                return principal;
            }

            if (principal.HasClaim(x => x.Type != "city"))
            {
                Claim cityClaim = new Claim("city", currentUser.City);
                identityUser.AddClaim(cityClaim);
            }
            return principal;
        }
    }
}
