using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models.Entities.Identity;
using AspNetCoreIdentityApp.Web.Models.Enums;
using AspNetCoreIdentityApp.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize(Policy = "")]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        //private readonly IHttpContextAccessor _contextAccessor;//
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index()
        {

            var userClaims = User.Claims.ToList();

            //claim yakalamak
            //var email = User.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.Email);
            //var x = 5;
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var userViewModel = new UserViewModel() { Email = currentUser.Email, Phone = currentUser.PhoneNumber, UserName = currentUser.UserName, PictureUrl = currentUser.Picture };
            return View(userViewModel);
        }

        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Index","Home");
        //}

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
                return View();
            }
            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, request.PasswordOld, request.PasswordNew);

            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x => x.Description).ToList());
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, false);

            TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıyla gerçekleşmiştir";

            return View();
        }

        public async Task<IActionResult> UserEdit()
        {
            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender,
                Phone = currentUser.PhoneNumber
            };
            return View(userEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            currentUser.UserName = request.UserName;
            currentUser.Email = request.Email;
            currentUser.BirthDate = request.BirthDate;
            currentUser.City = request.City;
            currentUser.PhoneNumber = request.Phone;
            currentUser.Gender = request.Gender;



            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userpictures").PhysicalPath, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await request.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;
            }

            var updateToUser = await _userManager.UpdateAsync(currentUser);

            if (!updateToUser.Succeeded)
            {
                ModelStateExtensions.AddModelErrorList(ModelState, updateToUser.Errors);
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            if (string.IsNullOrEmpty(request.BirthDate.ToString()))
            {
                await _signInManager.SignInWithClaimsAsync(currentUser, true, new[] { new Claim("birthdate", currentUser.BirthDate.ToString()) });
                TempData["SuccessMessage"] = "Üye bilgileri başarıyla değiştirilmiştir.";
            }
            else
            {
                await _signInManager.SignInAsync(currentUser, true);
            }


            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender,
                Phone = currentUser.PhoneNumber
            };

            return View(userEditViewModel);
        }

        [HttpGet]
        public IActionResult Claims()
        {
            var userClaimList = User.Claims.Select(x => new ClaimViewModel()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();
            return View(userClaimList);
        }
        [Authorize(Policy = "Ankara")]
        public IActionResult AnkaraPage()
        {
            return View();
        }
        [Authorize(Policy = "ExchangePolicy")]
        [HttpGet]
        public IActionResult ExchangePage()
        {
            return View();
        }
        [Authorize(Policy = "ViolencePolicy")]
        [HttpGet]
        public IActionResult ViolencePage()
        {
            return View();
        }
        public async Task<IActionResult> AccessDenied(string ReturnUrl)
        {
            string message = string.Empty;
            message = "Bu sayfayı görmeye yetkiniz yoktur. Yetki almak için yöneticinizle görüşebilirsiniz";
            ViewBag.message = message;
            return View();
        }
    }
}
