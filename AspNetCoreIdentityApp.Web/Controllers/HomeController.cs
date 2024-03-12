using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Models.Entities.Identity;
using AspNetCoreIdentityApp.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Services;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel, string returnUrl = null)
        {

            returnUrl = returnUrl ?? Url.Action("Privacy", "Home");

            var hasUser = await _userManager.FindByEmailAsync(signInViewModel.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya þifre yanlýþ");
                return View();
            }
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, signInViewModel.Password, signInViewModel.RememberMe, true); //4.parametre lockout iþlemi içindir.

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelStateExtensions.AddModelErrorList(ModelState, new List<string>() { "1 dakika boyunca giriþ yapamazsýnýz" });
                return View();
            }

            ModelStateExtensions.AddModelErrorList(ModelState, new List<string>() { "Email veya þifreniz yanlýþ", $" (Baþarýsýz giriþ sayýsý={await _userManager.GetAccessFailedCountAsync(hasUser)})" });

            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {


            if (!ModelState.IsValid)
            {
                return View();
            }
            var identityResult = await _userManager.CreateAsync(new AppUser() { UserName = request.UserName, Email = request.Email, PhoneNumber = request.Phone }, request.PasswordConfirm);

            if (identityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik kayýt iþlemi baþarýyla gerçekleþmiþtir.";
                return RedirectToAction(nameof(HomeController.SignUp));
            }

            ModelStateExtensions.AddModelErrorList(ModelState, identityResult.Errors.Select(x => x.Description).ToList());

            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            var hasUser = await _userManager.FindByEmailAsync(model.Email);
            if (hasUser == null)
            {
                ModelStateExtensions.AddModelErrorList(ModelState, new List<string>() { "Bu mail adresine sahip kullanýcý bulunamamýþtýr." });
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action(nameof(ResetPassword),"Home", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme, "localhost:7076");

            //EmailService

            //https://localhost:7076?userId=12332&token=asdasgsdfgsdf

            await _emailService.SendResetPasswordEmail(passwordResetLink, hasUser.Email);

            TempData["success"] = "Þifre yenileme linki, eposta adresinize gönderilmiþtir";

            return RedirectToAction(nameof(ForgetPassword));
        }

        public Task<IActionResult> ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return Task.FromResult<IActionResult>(View());
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"].ToString();
            var token = TempData["token"].ToString();

            if(userId is null || token is null)
            {
                throw new Exception("Bir hata meydana geldi.");
            }

            var hasUser = await _userManager.FindByIdAsync(userId);
            if (hasUser == null)
            {
                ModelStateExtensions.AddModelErrorList(ModelState, new() { "Kullanýcý bulunamamýþtýr" });
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(hasUser,token,request.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Þifreniz baþarýyla yenilenmiþtir.";
            }
            else
            {
                ModelStateExtensions.AddModelErrorList(ModelState,result.Errors.Select(x=>x.Description).ToList());
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
