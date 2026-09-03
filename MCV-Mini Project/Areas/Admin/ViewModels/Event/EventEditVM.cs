using System.ComponentModel.DataAnnotations;

namespace MCV_Mini_Project.Areas.Admin.ViewModels.Event
{
    public class EventEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Gün tələb olunur")]
        [MaxLength(2, ErrorMessage = "Gün maksimum 2 simvol ola bilər")]
        public string DateDay { get; set; }

        [Required(ErrorMessage = "Ay tələb olunur")]
        [MaxLength(20, ErrorMessage = "Ay maksimum 20 simvol ola bilər")]
        public string DateMonth { get; set; }

        [Required(ErrorMessage = "Başlıq tələb olunur")]
        [MaxLength(200, ErrorMessage = "Başlıq maksimum 200 simvol ola bilər")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Yer tələb olunur")]
        [MaxLength(200, ErrorMessage = "Yer maksimum 200 simvol ola bilər")]
        public string Location { get; set; }
    }
}
