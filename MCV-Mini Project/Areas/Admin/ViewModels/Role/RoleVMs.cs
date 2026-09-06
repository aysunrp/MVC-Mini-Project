using System.ComponentModel.DataAnnotations;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.Role
{
    public class RoleListVM
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsSystemRole { get; set; }
        public int UserCount { get; set; }
        public IList<string> Permissions { get; set; } = new List<string>();
    }

    public class RoleCreateVM
    {
        [Required]
        [StringLength(64, MinimumLength = 2)]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]*$",
            ErrorMessage = "Role name must start with a letter and contain only letters, numbers, or underscores.")]
        [Display(Name = "Role name")]
        public string Name { get; set; } = string.Empty;

        public List<string> SelectedPermissions { get; set; } = new();
    }

    public class RoleEditVM
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(64, MinimumLength = 2)]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]*$",
            ErrorMessage = "Role name must start with a letter and contain only letters, numbers, or underscores.")]
        [Display(Name = "Role name")]
        public string Name { get; set; } = string.Empty;

        public bool IsSystemRole { get; set; }

        public List<string> SelectedPermissions { get; set; } = new();
    }
}
