using MCV_Mini_Project.ViewModels.Videos;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IVideoService
    {
        Task<VideoUIVM> GettAllUIAsync();
    }
}
