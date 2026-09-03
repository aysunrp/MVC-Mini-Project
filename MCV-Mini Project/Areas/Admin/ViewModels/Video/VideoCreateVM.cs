using System.ComponentModel.DataAnnotations;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.Video
{
    public class VideoCreateVM
    {
        [Required(ErrorMessage = "Video adı/linki tələb olunur")]
        [MaxLength(500, ErrorMessage = "Ad maksimum 500 simvol ola bilər")]
        public string Name { get; set; }
    }
}
