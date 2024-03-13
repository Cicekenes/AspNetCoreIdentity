using AspNetCoreIdentityApp.Web.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AspNetCoreIdentityApp.Web.TagHelpers
{
    public class UserRolesNamesTagHelper : TagHelper
    {
        public string UserId { get; set; }
        private readonly UserManager<AppUser> _userManager;

        public UserRolesNamesTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var stringBuilder = new StringBuilder();
            userRoles.ToList().ForEach(x => stringBuilder.Append(@$"<span style='background:gray;' class='badge text-bg-info mx-1'>{x.ToLower()}</span>"));
            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
