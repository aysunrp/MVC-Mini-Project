using MCV_Mini_Project.Areas.Admin.ViewModels.News;
using MCV_Mini_Project.ViewModels.News;

namespace MCV_Mini_Project.Services.Interface
{
    public interface INewsService
    {
        // UI (frontend)
        Task<IEnumerable<NewsUIVM>> GetNewsUIVMAsync();

        // Admin CRUD
        Task<IEnumerable<NewsListVM>> GetAllAsync();
        Task<NewsEditVM?> GetByIdAsync(int id);
        Task CreateAsync(NewsCreateVM vm);
        Task<bool> UpdateAsync(NewsEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
