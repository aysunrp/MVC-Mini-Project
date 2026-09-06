using System.Security.Claims;
using MCV_Mini_Project.Areas.Admin.ViewModels.Role;
using MCV_Mini_Project.Constants;
using MCV_Mini_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Policies.SuperAdminOnly)]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            var list = new List<RoleListVM>();

            foreach (var role in roles)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                var users = string.IsNullOrWhiteSpace(role.Name)
                    ? new List<AppUser>()
                    : await _userManager.GetUsersInRoleAsync(role.Name);

                list.Add(new RoleListVM
                {
                    Id = role.Id,
                    Name = role.Name ?? string.Empty,
                    IsSystemRole = AppRoles.IsSystemRole(role.Name),
                    UserCount = users.Count,
                    Permissions = claims.Where(c => c.Type == Permissions.ClaimType).Select(c => c.Value).ToList()
                });
            }

            return View(list);
        }

        public IActionResult Create()
        {
            return View(new RoleCreateVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleCreateVM model)
        {
            if (!User.IsInRole(AppRoles.SuperAdmin))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), "A role with this name already exists.");
                return View(model);
            }

            var role = new IdentityRole(model.Name.Trim());
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            await SyncPermissionsAsync(role, SanitizePermissions(model.SelectedPermissions));
            TempData["Success"] = "Role created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();

            var claims = await _roleManager.GetClaimsAsync(role);
            return View(new RoleEditVM
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                IsSystemRole = AppRoles.IsSystemRole(role.Name),
                SelectedPermissions = claims.Where(c => c.Type == Permissions.ClaimType).Select(c => c.Value).ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleEditVM model)
        {
            if (!User.IsInRole(AppRoles.SuperAdmin))
                return Forbid();

            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role is null)
                return NotFound();

            model.IsSystemRole = AppRoles.IsSystemRole(role.Name);
            if (!ModelState.IsValid)
                return View(model);

            if (!model.IsSystemRole &&
                !string.Equals(role.Name, model.Name, StringComparison.OrdinalIgnoreCase) &&
                await _roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), "A role with this name already exists.");
                return View(model);
            }

            if (!model.IsSystemRole)
            {
                role.Name = model.Name.Trim();
                role.NormalizedName = _roleManager.NormalizeKey(role.Name);
                var update = await _roleManager.UpdateAsync(role);
                if (!update.Succeeded)
                {
                    foreach (var error in update.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View(model);
                }
            }

            if (model.IsSystemRole)
            {
                TempData["Error"] = "System role permissions are fixed and cannot be changed.";
                return RedirectToAction(nameof(Index));
            }

            await SyncPermissionsAsync(role, SanitizePermissions(model.SelectedPermissions));
            TempData["Success"] = "Role updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (!User.IsInRole(AppRoles.SuperAdmin))
                return Forbid();

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();

            if (AppRoles.IsSystemRole(role.Name))
            {
                TempData["Error"] = "System roles cannot be deleted.";
                return RedirectToAction(nameof(Index));
            }

            var users = await _userManager.GetUsersInRoleAsync(role.Name!);
            if (users.Count > 0)
            {
                TempData["Error"] = "Cannot delete a role that is assigned to users.";
                return RedirectToAction(nameof(Index));
            }

            await _roleManager.DeleteAsync(role);
            TempData["Success"] = "Role deleted.";
            return RedirectToAction(nameof(Index));
        }

        private static List<string> SanitizePermissions(IEnumerable<string>? selected)
        {
            var allowed = Permissions.Catalog.Select(p => p.Value).ToHashSet(StringComparer.Ordinal);
            return (selected ?? Enumerable.Empty<string>())
                .Where(allowed.Contains)
                .Where(p => p is not Permissions.ManageRoles and not Permissions.ManageSuperAdmins)
                .Distinct()
                .ToList();
        }

        private async Task SyncPermissionsAsync(IdentityRole role, IList<string> selected)
        {
            var existing = (await _roleManager.GetClaimsAsync(role))
                .Where(c => c.Type == Permissions.ClaimType)
                .ToList();

            foreach (var claim in existing)
            {
                if (!selected.Contains(claim.Value))
                    await _roleManager.RemoveClaimAsync(role, claim);
            }

            foreach (var permission in selected)
            {
                if (existing.All(c => c.Value != permission))
                    await _roleManager.AddClaimAsync(role, new Claim(Permissions.ClaimType, permission));
            }
        }
    }
}
