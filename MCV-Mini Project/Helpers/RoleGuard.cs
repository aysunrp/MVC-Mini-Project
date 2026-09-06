using System.Security.Claims;
using MCV_Mini_Project.Constants;

namespace MCV_Mini_Project.Helpers
{
    public static class RoleGuard
    {
        public static bool IsSuperAdmin(ClaimsPrincipal user)
            => user.IsInRole(AppRoles.SuperAdmin);

        public static bool IsAdmin(ClaimsPrincipal user)
            => user.IsInRole(AppRoles.Admin);

        public static bool CanManageRoles(ClaimsPrincipal user)
            => IsSuperAdmin(user);

        public static bool CanAssignRole(ClaimsPrincipal actor, string targetRole)
        {
            if (IsSuperAdmin(actor))
                return true;

            if (IsAdmin(actor))
                return string.Equals(targetRole, AppRoles.Member, StringComparison.OrdinalIgnoreCase);

            return false;
        }

        public static bool CanManageTarget(ClaimsPrincipal actor, IList<string> targetRoles)
        {
            if (IsSuperAdmin(actor))
                return true;

            if (!IsAdmin(actor))
                return false;

            if (targetRoles.Any(r => string.Equals(r, AppRoles.SuperAdmin, StringComparison.OrdinalIgnoreCase)))
                return false;

            if (targetRoles.Any(r => string.Equals(r, AppRoles.Admin, StringComparison.OrdinalIgnoreCase)))
                return false;

            return true;
        }
    }
}
