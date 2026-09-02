using MCV_Mini_Project.ViewModels.AboutVisions;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IVisionService
    {
        Task<AboutVisionUIVM> GetAllUIAsync();
    }
}
