using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.News
{
    public class NewsEditVM
    {
        public int Id { get; set; }

        public string? CurrentImage { get; set; }

        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Title / Question is required")]
        [MaxLength(300)]
        public string Question { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Author is required")]
        public int AuthorId { get; set; }
    }
}
