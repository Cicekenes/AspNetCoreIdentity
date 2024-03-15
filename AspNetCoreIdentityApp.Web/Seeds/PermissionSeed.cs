using AspNetCoreIdentityApp.Web.Models.DatabaseContexts;
using AspNetCoreIdentityApp.Web.Models.Entities.Identity;
using AspNetCoreIdentityApp.Web.Permissions;
using Microsoft.AspNetCore.Identity;
namespace AspNetCoreIdentityApp.Web.Seeds
{
    public class PermissionSeed
    {
        public static async Task Seed(RoleManager<AppRole> roleManager)
        {
            var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");
            var hasAdvancedRole = await roleManager.RoleExistsAsync("AdvancedRole");
            var hasAdminRole = await roleManager.RoleExistsAsync("AdminRole");
            if (!hasBasicRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "BasicRole" });
                var basicRole = await roleManager.FindByNameAsync("BasicRole");
                await AddReadPermission(basicRole, roleManager);
            }
            if (!hasAdvancedRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdvancedRole" });
                var advancedRole = await roleManager.FindByNameAsync("AdvancedRole");
                await AddReadPermission(advancedRole, roleManager);
                await AddUpdateAndPermission(advancedRole, roleManager);
            }
            if (!hasAdminRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdminRole" });
                var adminRole = await roleManager.FindByNameAsync("AdminRole");
                await AddReadPermission(adminRole, roleManager);
                await AddUpdateAndPermission(adminRole, roleManager);
                await AddDeletePermission(adminRole, roleManager);
            }
        }

        public static async Task AddReadPermission(AppRole role, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Stock.Read));
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Order.Read));
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Catalog.Read));
        }
        public static async Task AddUpdateAndPermission(AppRole role, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Stock.Create));
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Order.Create));
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Catalog.Create));
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Stock.Update));
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Order.Update));
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Catalog.Update));
        }
        public static async Task AddDeletePermission(AppRole role, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Stock.Delete));
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Order.Delete));
            await roleManager.AddClaimAsync(role, new(type: "Permission", value: Permission.Catalog.Delete));
        }
    }
}
