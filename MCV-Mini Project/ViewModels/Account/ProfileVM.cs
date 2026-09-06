using System.ComponentModel.DataAnnotations;

namespace MCV_Mini_Project.ViewModels.Account
{
    public class ProfileVM
    {
        public string Email { get; set; } = string.Empty;

        public bool EmailConfirmed { get; set; }

        public IList<string> Roles { get; set; } = new List<string>();

        [Required]
        [StringLength(120, MinimumLength = 2)]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number, and special character.")]
        [Display(Name = "New password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm new password")]
        public string? ConfirmNewPassword { get; set; }
    }
}
