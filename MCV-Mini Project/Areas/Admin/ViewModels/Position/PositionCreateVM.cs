using System.ComponentModel.DataAnnotations;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.Position
{
    public class PositionCreateVM
    {
        [Required(ErrorMessage = "Vəzifə adı tələb olunur")]
        [MaxLength(100, ErrorMessage = "Vəzifə adı maksimum 100 simvol ola bilər")]
        public string Name { get; set; }
    }
}
