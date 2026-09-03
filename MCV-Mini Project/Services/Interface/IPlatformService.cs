using MCV_Mini_Project.Areas.Admin.ViewModels.AboutPlatform;
using MCV_Mini_Project.ViewModels.AboutPlatforms;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IPlatformService
    {
        // UI (frontend)
        Task<AboutPlatformUIVM> GetAllUIAsync();

        // Admin CRUD
        Task<IEnumerable<AboutPlatformListVM>> GetAllAdminAsync();
        Task<AboutPlatformEditVM?> GetByIdAsync(int id);
        Task CreateAsync(AboutPlatformCreateVM vm);
        Task<bool> UpdateAsync(AboutPlatformEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
