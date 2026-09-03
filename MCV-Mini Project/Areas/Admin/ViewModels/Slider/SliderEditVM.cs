using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.Slider
{
    public class SliderEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Logo class is required")]
        public string Logo { get; set; }

        // Existing image path stored in DB
        public string? CurrentImage { get; set; }

        // New file upload (optional on edit)
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}
