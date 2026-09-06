namespace MCV_Mini_Project.Constants
{
    public static class AppRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Member = "Member";

        public static readonly string[] SystemRoles = { SuperAdmin, Admin, Member };

        public static bool IsSystemRole(string? roleName)
            => SystemRoles.Any(r => string.Equals(r, roleName, StringComparison.OrdinalIgnoreCase));
    }
}
