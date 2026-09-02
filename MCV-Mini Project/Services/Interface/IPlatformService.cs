using MCV_Mini_Project.ViewModels.AboutPlatforms;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IPlatformService
    {
        Task<AboutPlatformUIVM> GetAllUIAsync();
    }
}
