using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.Teacher
{
    public class TeacherCreateVM
    {
        [Required(ErrorMessage = "Full name is required")]
        [MaxLength(150)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageFile { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public int PositionId { get; set; }
    }
}
