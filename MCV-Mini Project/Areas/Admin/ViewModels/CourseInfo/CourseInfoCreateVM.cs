using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.CourseInfo
{
    public class CourseInfoCreateVM
    {
        [Required(ErrorMessage = "Başlıq tələb olunur")]
        [MaxLength(200, ErrorMessage = "Başlıq maksimum 200 simvol ola bilər")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıqlama tələb olunur")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Qiymət tələb olunur")]
        [Range(0, int.MaxValue, ErrorMessage = "Qiymət 0-dan böyük olmalıdır")]
        public int Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Satış sayı 0-dan böyük olmalıdır")]
        public int SalesCount { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsNew { get; set; }

        [Required(ErrorMessage = "Müəllim tələb olunur")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Şəkil tələb olunur")]
        public IFormFile ImageFile { get; set; }
    }
}
