using MCV_Mini_Project.Areas.Admin.ViewModels.Setting;

namespace MCV_Mini_Project.Services.Interface
{
    public interface ISettingService
    {
        // UI (frontend)
        Task<Dictionary<string, string>> GetAllUIAsync();

        // Admin CRUD
        Task<IEnumerable<SettingListVM>> GetAllAsync();
        Task<SettingEditVM?> GetByIdAsync(int id);
        Task CreateAsync(SettingCreateVM vm);
        Task<bool> UpdateAsync(SettingEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
