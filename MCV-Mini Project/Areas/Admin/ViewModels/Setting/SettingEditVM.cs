using System.ComponentModel.DataAnnotations;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.Setting
{
    public class SettingEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Açar tələb olunur")]
        [MaxLength(150, ErrorMessage = "Açar maksimum 150 simvol ola bilər")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Dəyər tələb olunur")]
        public string Value { get; set; }
    }
}
