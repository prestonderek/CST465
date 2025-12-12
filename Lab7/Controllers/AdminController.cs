using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Lab7.Models;

namespace Lab7.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Roles()
        {
            var roles = _roleManager.Roles.ToList();
            var model = new List<RolesWithUsersViewModel>();

            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

                model.Add(new RolesWithUsersViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Users = usersInRole.Select(u => u.UserName).ToList()
                });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {             
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _roleManager.RoleExistsAsync(model.RoleName))
            {
                ModelState.AddModelError(string.Empty, "Role already exists.");
                return View(model);
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));

            if (result.Succeeded)
                return RedirectToAction(nameof(Roles));

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        public IActionResult AddUserToRole()
        {
            var model = new AddUserToRoleViewModel
            {
                Users = _userManager.Users.ToList(),
                Roles = _roleManager.Roles.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (user == null || !roleExists)
            {
                ModelState.AddModelError(string.Empty, "Invalid user or role");
                return RedirectToAction(nameof(AddUserToRole));
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                TempData["Error"] = string.Join("; ", result.Errors.Select(e => e.Description));
            }
            else
            {
                TempData["Message"] = $"User {user.UserName} added to role {roleName}.";
            }

            return RedirectToAction(nameof(Roles));
        }
    }
}
