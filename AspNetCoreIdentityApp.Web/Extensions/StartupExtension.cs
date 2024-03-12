using AspNetCoreIdentityApp.Web.CustomValidations;
using AspNetCoreIdentityApp.Web.Localizations;
using AspNetCoreIdentityApp.Web.Models.DatabaseContexts;
using AspNetCoreIdentityApp.Web.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class StartupExtension
    {
        public static void AddIdentityWithExt(this IServiceCollection services)
        {

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true; //Mail adresi eşsiz olsun.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwyz1234567890_"; //izin verilen karakterler
                options.Password.RequiredLength = 6; //6 karakter
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true; //küçük karakter zorunlu
                options.Password.RequireUppercase = false; // büyük karakter zorunlu değil
                options.Password.RequireDigit = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); // 1 dakika boyunca kitle.
                options.Lockout.MaxFailedAccessAttempts = 3; //3 kere başarısız girişte hesabı kilitlensin.
            })
              .AddPasswordValidator<PasswordValidator>()
              .AddUserValidator<UserValidator>()
              .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
              .AddDefaultTokenProviders() //default token değeri örnek : şifre yenileme linki 2 saat geçerli olsun.
              .AddEntityFrameworkStores<AppDbContext>();
             

        }
    }
}
