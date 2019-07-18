using BookStore.Domain;
using BookStore.Services.Admin;
using BookStore.Web.Areas.Admin.Models.Users;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name // roleName => RoleExistsAsync
                })
                .ToListAsync();

            return View(new AdminUserListingViewModel
            {
                Users = users,
                Roles = roles
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(AddRemoveUserRoleInputModel model)
        {
            var roleExists = await this.roleManager.RoleExistsAsync(model.Role);
            var user = await this.userManager.FindByIdAsync(model.UserId);
            var userExists = user != null;

            if (!roleExists || !userExists)
            {
                this.ModelState.AddModelError(string.Empty, WebAdminConstants.UserInvalidIdentityDetailsMsg);
            }

            if (!this.ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            await this.userManager.AddToRoleAsync(user, model.Role);

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.UserAddedToRoleMsg,
                user.UserName,
                model.Role));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(AddRemoveUserRoleInputModel model)
        {
            var roleExists = await this.roleManager.RoleExistsAsync(model.Role);
            var user = await this.userManager.FindByIdAsync(model.UserId);
            var userExists = user != null;

            if (!roleExists || !userExists)
            {
                this.ModelState.AddModelError(string.Empty, WebAdminConstants.UserInvalidIdentityDetailsMsg);
            }

            if (!this.ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            await this.userManager.RemoveFromRoleAsync(user, model.Role);

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.UserRemovedFromRoleMsg,
                user.UserName,
                model.Role));

            return RedirectToAction(nameof(Index));
        }
    }
}
