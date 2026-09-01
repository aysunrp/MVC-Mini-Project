using MCV_Mini_Project.ViewModels.News;

namespace MCV_Mini_Project.Services.Interface
{
    public interface INewsService
    {
        Task<IEnumerable<NewsUIVM>> GetNewsUIVMAsync();
    }
}
