using Microsoft.AspNetCore.Identity;

namespace MCV_Mini_Project.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? EmailConfirmTokenHash { get; set; }

        public DateTime? EmailConfirmTokenExpiresAt { get; set; }
    }
}
