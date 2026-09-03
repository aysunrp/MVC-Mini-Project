using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [Required(ErrorMessage = "Logo class is required")]
        public string Logo { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageFile { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}
