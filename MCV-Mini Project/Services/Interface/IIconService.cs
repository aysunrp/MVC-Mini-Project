using MCV_Mini_Project.Areas.Admin.ViewModels.Icon;
using MCV_Mini_Project.ViewModels.Icons;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IIconService
    {
        // UI (frontend)
        Task<IEnumerable<IconUIVM>> GetIconUIVMAsync();

        // Admin CRUD
        Task<IEnumerable<IconListVM>> GetAllAsync();
        Task<IconEditVM?> GetByIdAsync(int id);
        Task CreateAsync(IconCreateVM vm);
        Task<bool> UpdateAsync(IconEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
