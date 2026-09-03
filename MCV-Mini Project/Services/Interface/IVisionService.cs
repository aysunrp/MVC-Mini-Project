using MCV_Mini_Project.Areas.Admin.ViewModels.AboutVision;
using MCV_Mini_Project.ViewModels.AboutVisions;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IVisionService
    {
        // UI (frontend)
        Task<AboutVisionUIVM> GetAllUIAsync();

        // Admin CRUD
        Task<IEnumerable<AboutVisionListVM>> GetAllAdminAsync();
        Task<AboutVisionEditVM?> GetByIdAsync(int id);
        Task CreateAsync(AboutVisionCreateVM vm);
        Task<bool> UpdateAsync(AboutVisionEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
