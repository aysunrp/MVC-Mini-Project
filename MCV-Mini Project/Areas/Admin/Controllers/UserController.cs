using MCV_Mini_Project.Areas.Admin.ViewModels.User;
using MCV_Mini_Project.Constants;
using MCV_Mini_Project.Helpers;
using MCV_Mini_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Policies.ManageUsers)]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
            var list = new List<UserListVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!RoleGuard.IsSuperAdmin(User) && !RoleGuard.CanManageTarget(User, roles))
                    continue;

                list.Add(new UserListVM
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = roles,
                    CreatedAt = user.CreatedAt,
                    CanManage = RoleGuard.CanManageTarget(User, roles) && user.Id != _userManager.GetUserId(User)
                });
            }

            return View(list);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            if (!RoleGuard.CanManageTarget(User, roles) && user.Id != _userManager.GetUserId(User))
                return Forbid();

            return View(new UserDetailsVM
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles,
                CreatedAt = user.CreatedAt
            });
        }

        public async Task<IActionResult> Create()
        {
            return View(new UserCreateVM
            {
                Role = AppRoles.Member,
                AvailableRoles = await GetAssignableRolesAsync()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateVM model)
        {
            model.AvailableRoles = await GetAssignableRolesAsync();

            if (!ModelState.IsValid)
                return View(model);

            if (!RoleGuard.CanAssignRole(User, model.Role))
            {
                ModelState.AddModelError(nameof(model.Role), "You are not allowed to assign this role.");
                return View(model);
            }

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                ModelState.AddModelError(nameof(model.Email), "This email address is already registered.");
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                FullName = model.FullName.Trim(),
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, model.Role);
            TempData["Success"] = "User created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            if (!RoleGuard.CanManageTarget(User, roles))
                return Forbid();

            return View(new UserEditVM
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                EmailConfirmed = user.EmailConfirmed,
                Role = roles.FirstOrDefault() ?? AppRoles.Member,
                AvailableRoles = await GetAssignableRolesAsync()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditVM model)
        {
            model.AvailableRoles = await GetAssignableRolesAsync();

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null)
                return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!RoleGuard.CanManageTarget(User, currentRoles))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            if (!RoleGuard.CanAssignRole(User, model.Role))
            {
                ModelState.AddModelError(nameof(model.Role), "You are not allowed to assign this role.");
                return View(model);
            }

            if (currentRoles.Contains(AppRoles.SuperAdmin) &&
                model.Role != AppRoles.SuperAdmin &&
                await IsLastSuperAdminAsync(user.Id))
            {
                ModelState.AddModelError(nameof(model.Role), "Cannot remove the last SuperAdmin.");
                return View(model);
            }

            var emailOwner = await _userManager.FindByEmailAsync(model.Email);
            if (emailOwner is not null && emailOwner.Id != user.Id)
            {
                ModelState.AddModelError(nameof(model.Email), "This email address is already registered.");
                return View(model);
            }

            user.FullName = model.FullName.Trim();
            user.Email = model.Email.Trim();
            user.UserName = model.Email.Trim();
            user.EmailConfirmed = model.EmailConfirmed || user.EmailConfirmed;

            var update = await _userManager.UpdateAsync(user);
            if (!update.Succeeded)
            {
                foreach (var error in update.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            var rolesToRemove = currentRoles.Where(r => !string.Equals(r, model.Role, StringComparison.OrdinalIgnoreCase)).ToList();
            if (rolesToRemove.Count > 0)
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (!currentRoles.Contains(model.Role))
                await _userManager.AddToRoleAsync(user, model.Role);

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View(model);
                }
            }

            TempData["Success"] = "User updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!RoleGuard.CanManageTarget(User, roles))
                return Forbid();

            if (roles.Contains(AppRoles.SuperAdmin) && await IsLastSuperAdminAsync(user.Id))
            {
                TempData["Error"] = "Cannot delete the last SuperAdmin.";
                return RedirectToAction(nameof(Index));
            }

            await _userManager.DeleteAsync(user);
            TempData["Success"] = "User deleted.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> IsLastSuperAdminAsync(string userId)
        {
            var superAdmins = await _userManager.GetUsersInRoleAsync(AppRoles.SuperAdmin);
            return superAdmins.Count <= 1 && superAdmins.Any(u => u.Id == userId);
        }

        private async Task<List<SelectListItem>> GetAssignableRolesAsync()
        {
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            return roles
                .Where(r => r.Name is not null && RoleGuard.CanAssignRole(User, r.Name))
                .Select(r => new SelectListItem(r.Name, r.Name))
                .ToList();
        }
    }
}
