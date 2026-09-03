using MCV_Mini_Project.Areas.Admin.ViewModels.Video;
using MCV_Mini_Project.ViewModels.Videos;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IVideoService
    {
        // UI (frontend)
        Task<VideoUIVM> GettAllUIAsync();

        // Admin CRUD
        Task<IEnumerable<VideoListVM>> GetAllAsync();
        Task<VideoEditVM?> GetByIdAsync(int id);
        Task CreateAsync(VideoCreateVM vm);
        Task<bool> UpdateAsync(VideoEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
