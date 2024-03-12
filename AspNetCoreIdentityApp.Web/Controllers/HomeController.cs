using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Models.Entities.Identity;
using AspNetCoreIdentityApp.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.Web.Extensions;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
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

            ModelStateExtensions.AddModelErrorList(ModelState, new List<string>() { "Email veya þifreniz yanlýþ",$" (Baþarýsýz giriþ sayýsý={await _userManager.GetAccessFailedCountAsync(hasUser)})"});

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
