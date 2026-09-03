using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.Teacher
{
    public class TeacherEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [MaxLength(150)]
        public string FullName { get; set; }

        public string? CurrentImage { get; set; }

        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public int PositionId { get; set; }
    }
}
