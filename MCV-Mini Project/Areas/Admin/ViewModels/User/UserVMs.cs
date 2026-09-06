using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.User
{
    public class UserListVM
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public bool CanManage { get; set; }
    }

    public class UserCreateVM
    {
        [Required]
        [StringLength(120, MinimumLength = 2)]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number, and special character.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } = string.Empty;

        [ValidateNever]
        public List<SelectListItem> AvailableRoles { get; set; } = new();
    }

    public class UserEditVM
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(120, MinimumLength = 2)]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool EmailConfirmed { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "New password (optional)")]
        [RegularExpression(@"^$|^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number, and special character.")]
        public string? NewPassword { get; set; }

        [ValidateNever]
        public List<SelectListItem> AvailableRoles { get; set; } = new();
    }

    public class UserDetailsVM
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
    }
}
