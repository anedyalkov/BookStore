using BookStore.Domain;
using BookStore.Services.Admin;
using BookStore.Web.Areas.Admin.Models.Users;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminController
    {
        private readonly IAdminUserService adminUserService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<BookStoreUser> userManager;

        public UsersController(
            IAdminUserService adminUserService,
            RoleManager<IdentityRole> roleManager,
            UserManager<BookStoreUser> userManager)
        {
            this.adminUserService = adminUserService;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await this.adminUserService.GetAllUsers().ToListAsync();

            var roles = await this.roleManager
                .Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name 
                })
                .ToListAsync();

            return View(new AdminUserListingViewModel
            {
                Users = users,
                Roles = roles
            });
        }
        public async Task<IActionResult> Roles(string id)
        {

            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.ModelState.AddModelError(string.Empty, WebAdminConstants.UserInvalidIdentityDetails);
            }
            var roles = await this.userManager.GetRolesAsync(user);

            return this.View(new UserRolesViewModel
            {
                User = user,
                UserRoles = roles
                .Select(role => new SelectListItem
                {
                    Text = role,
                    Value = role
                })
                .ToList(),
                Roles = await roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToListAsync()
        }); 
        }
        [HttpPost]
        public async Task<IActionResult> AddToRole(UserRoleInputModel model)
        {
            var roleExists = await this.roleManager.RoleExistsAsync(model.Role);
            var user = await this.userManager.FindByIdAsync(model.UserId);

            if (!roleExists || user == null)
            {
                this.ModelState.AddModelError(string.Empty, WebAdminConstants.UserInvalidIdentityDetails);
            }

            if (!this.ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            await this.userManager.AddToRoleAsync(user, model.Role);

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.UserAddedToRole,
                user.UserName,
                model.Role));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(UserRoleInputModel model)
        {
            var roleExists = await this.roleManager.RoleExistsAsync(model.Role);
            var user = await this.userManager.FindByIdAsync(model.UserId);

            if (!roleExists || user == null)
            {
                this.ModelState.AddModelError(string.Empty, WebAdminConstants.UserInvalidIdentityDetails);
            }

            if (!this.ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            await this.userManager.RemoveFromRoleAsync(user, model.Role);

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.UserRemovedFromRole,
                user.UserName,
                model.Role));

            return RedirectToAction(nameof(Index));
        }
    }
}
