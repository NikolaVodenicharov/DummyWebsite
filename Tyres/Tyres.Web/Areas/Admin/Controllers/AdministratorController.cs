using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tyres.Data.Models;
using Tyres.Service.Interfaces;
using Tyres.Web.Areas.Admin.Models;
using Tyres.Web.Infrastructure;

namespace Tyres.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Administrator)]
    public class AdministratorController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserService userService;

        public AdministratorController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userService = userService;
        }

        public async Task<IActionResult> AllUsers(int page = 1)
        {
            var model = await this.userService.AllAsync(page);

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserRoleViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                CurrentRoles = await this.userManager.GetRolesAsync(user),
                ChooseRoles = this.roleManager
                    .Roles
                    .OrderBy(r => r.Name)
                    .Select(r => new SelectListItem
                    {
                        Text = r.Name,
                        Value = r.Name
                    })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetRole(UserRoleViewModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            var isRoleExist = await this.roleManager.RoleExistsAsync(model.ChosedRole);
            if (!isRoleExist)
            {
                return NotFound();
            }

            await this.userManager.AddToRoleAsync(user, model.ChosedRole);

            return RedirectToAction(nameof(AllUsers));
        }
    }
}