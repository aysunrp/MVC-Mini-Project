using System.ComponentModel.DataAnnotations;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.Author
{
    public class AuthorEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad tələb olunur")]
        [MaxLength(150, ErrorMessage = "Ad Soyad maksimum 150 simvol ola bilər")]
        public string FullName { get; set; }
    }
}
