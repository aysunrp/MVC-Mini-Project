using MCV_Mini_Project.Areas.Admin.ViewModels.Slider;
using MCV_Mini_Project.ViewModels.Slider;

namespace MCV_Mini_Project.Services.Interface
{
    public interface ISliderService
    {
        // UI (frontend)
        Task<IEnumerable<SliderUIVM>> GetSliderUIVMAsync();

        // Admin CRUD
        Task<IEnumerable<SliderListVM>> GetAllAsync();
        Task<SliderEditVM?> GetByIdAsync(int id);
        Task CreateAsync(SliderCreateVM vm);
        Task<bool> UpdateAsync(SliderEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
