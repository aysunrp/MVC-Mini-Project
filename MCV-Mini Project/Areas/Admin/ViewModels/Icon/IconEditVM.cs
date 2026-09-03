using System.ComponentModel.DataAnnotations;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.Icon
{
    public class IconEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad tələb olunur")]
        [MaxLength(100, ErrorMessage = "Ad maksimum 100 simvol ola bilər")]
        public string Name { get; set; }
    }
}
