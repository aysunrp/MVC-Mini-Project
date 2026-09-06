using System.Security.Claims;
using MCV_Mini_Project.Constants;
using MCV_Mini_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace MCV_Mini_Project.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var config = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeeder");

            foreach (var roleName in AppRoles.SystemRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await EnsureRoleClaimsAsync(roleManager, AppRoles.SuperAdmin, new[]
            {
                Permissions.AccessAdminPanel,
                Permissions.ManageUsers,
                Permissions.ManageContent,
                Permissions.ManageRoles,
                Permissions.ManageSuperAdmins
            });

            await EnsureRoleClaimsAsync(roleManager, AppRoles.Admin, new[]
            {
                Permissions.AccessAdminPanel,
                Permissions.ManageUsers,
                Permissions.ManageContent
            });

            await EnsureUserAsync(
                userManager,
                logger,
                email: config["SUPERADMIN_EMAIL"] ?? "superadmin@elearn.local",
                password: config["SUPERADMIN_PASSWORD"] ?? "SuperAdmin!123",
                fullName: "Super Admin",
                role: AppRoles.SuperAdmin);

            await EnsureUserAsync(
                userManager,
                logger,
                email: config["ADMIN_EMAIL"] ?? "admin@elearn.local",
                password: config["ADMIN_PASSWORD"] ?? "Admin!12345",
                fullName: "Site Admin",
                role: AppRoles.Admin);
        }

        private static async Task EnsureUserAsync(
            UserManager<AppUser> userManager,
            ILogger logger,
            string email,
            string password,
            string fullName,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var create = await userManager.CreateAsync(user, password);
                if (!create.Succeeded)
                {
                    logger.LogError("Failed to seed {Role}: {Errors}",
                        role, string.Join("; ", create.Errors.Select(e => e.Description)));
                    return;
                }
            }
            else
            {
                user.FullName = fullName;
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var reset = await userManager.ResetPasswordAsync(user, token, password);
                if (!reset.Succeeded)
                {
                    logger.LogError("Failed to reset password for {Email}: {Errors}",
                        email, string.Join("; ", reset.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);

            logger.LogInformation("Ready {Role} account: {Email}", role, email);
        }

        private static async Task EnsureRoleClaimsAsync(
            RoleManager<IdentityRole> roleManager,
            string roleName,
            IEnumerable<string> permissions)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role is null)
                return;

            var existing = await roleManager.GetClaimsAsync(role);
            foreach (var permission in permissions)
            {
                if (existing.Any(c => c.Type == Permissions.ClaimType && c.Value == permission))
                    continue;

                await roleManager.AddClaimAsync(role, new Claim(Permissions.ClaimType, permission));
            }
        }
    }
}
