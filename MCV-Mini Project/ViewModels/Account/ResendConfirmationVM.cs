using System.ComponentModel.DataAnnotations;

namespace MCV_Mini_Project.ViewModels.Account
{
    public class ResendConfirmationVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
