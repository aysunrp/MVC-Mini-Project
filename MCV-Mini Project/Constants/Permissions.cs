namespace MCV_Mini_Project.Constants
{
    public static class Permissions
    {
        public const string ClaimType = "Permission";

        public const string AccessAdminPanel = "AccessAdminPanel";
        public const string ManageUsers = "ManageUsers";
        public const string ManageContent = "ManageContent";
        public const string ManageRoles = "ManageRoles";
        public const string ManageSuperAdmins = "ManageSuperAdmins";

        public static readonly (string Value, string Label, string Description)[] Catalog =
        {
            (AccessAdminPanel, "Access Admin Panel", "Open /admin and administrative pages."),
            (ManageUsers, "Manage Users", "View and manage users within the allowed scope."),
            (ManageContent, "Manage Content", "Create, edit, and delete site content."),
            (ManageRoles, "Manage Roles", "Create, edit, and delete roles. SuperAdmin only."),
            (ManageSuperAdmins, "Manage SuperAdmins", "Create or modify SuperAdmin accounts. SuperAdmin only.")
        };
    }
}
