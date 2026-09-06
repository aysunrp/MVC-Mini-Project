using System.Security.Claims;
using MCV_Mini_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MCV_Mini_Project.Identity
{
    public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, IdentityRole>
    {
        public AppUserClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            if (!string.IsNullOrWhiteSpace(user.FullName))
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FullName));

            identity.AddClaim(new Claim("email_confirmed", user.EmailConfirmed ? "true" : "false"));

            var roleNames = await UserManager.GetRolesAsync(user);
            foreach (var roleName in roleNames)
            {
                var role = await RoleManager.FindByNameAsync(roleName);
                if (role is null)
                    continue;

                var claims = await RoleManager.GetClaimsAsync(role);
                foreach (var claim in claims)
                    identity.AddClaim(claim);
            }

            return identity;
        }
    }
}
