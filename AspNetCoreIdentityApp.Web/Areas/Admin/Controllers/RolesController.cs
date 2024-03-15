using AspNetCoreIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize("Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //[Authorize(Roles = "role-action")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            return View(roles);
        }

        //[Authorize(Roles ="role-action")]
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        
        public async Task<IActionResult> RoleCreate(RoleCreateViewModel request)
        {
            var result = await _roleManager.CreateAsync(new AppRole() { Name = request.Name });

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }
            TempData["SuccessMessage"] = "Rol oluşturulmuştur";
            return RedirectToAction(nameof(RolesController.Index));
        }
        //[Authorize(Roles = "role-action")]
        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleUpdate = await _roleManager.FindByIdAsync(id);

            if (roleUpdate == null)
            {
                throw new Exception("Güncellenecek rol bulunamamıştır.");
            }

            return View(new RoleUpdateViewModel() { Id = roleUpdate.Id, Name = roleUpdate.Name });
        }
        [Authorize(Roles = "role-action")]
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel request)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(request.Id);
            if (roleToUpdate == null)
            {
                throw new Exception("Güncellenecek rol bulunamamıştır.");
            }
            roleToUpdate.Name = request.Name;
            await _roleManager.UpdateAsync(roleToUpdate);
            ViewData["SuccessMessage"] = "Rol bilgisi güncellenmiştir";
            return View();
        }
        //[Authorize(Roles = "role-action")]
        public async Task<IActionResult> RoleDelete(string id)
        {
            var roleToDelete = await _roleManager.FindByIdAsync(id);
            if (roleToDelete == null)
            {
                throw new Exception("Silinecek rol bulunamamıştır");
            }
            var result = await _roleManager.DeleteAsync(roleToDelete);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(x => x.Description).First());
            }
            TempData["SuccessMessage"] = "Rol silinmiştir";
            return RedirectToAction(nameof(RolesController.Index));
        }

        public async Task<IActionResult> AssignRoleToUser(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);
            ViewBag.userId = id;
            var roles = await _roleManager.Roles.ToListAsync();
            var roleViewModelList = new List<AssignRoleToUserViewModel>();
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            foreach (var role in roles)
            {
                var assignRoleToUserViewModel = new AssignRoleToUserViewModel() { RoleId = role.Id, Name = role.Name };
                if (userRoles.Contains(role.Name))
                {
                    assignRoleToUserViewModel.Exist = true;
                }
                roleViewModelList.Add(assignRoleToUserViewModel);
            }
            return View(roleViewModelList);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(string userId, List<AssignRoleToUserViewModel> requestList)
        {
            var userToAssignRoles = await _userManager.FindByIdAsync(userId);

            foreach (var role in requestList)
            {
                if (role.Exist)
                {
                    await _userManager.AddToRoleAsync(userToAssignRoles, role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(userToAssignRoles, role.Name);
                }
            }
            return RedirectToAction("UserList","Home");
        }
    }
}
