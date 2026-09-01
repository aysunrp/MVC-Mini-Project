using MCV_Mini_Project.ViewModels.Slider;

namespace MCV_Mini_Project.Services.Interface
{
    public interface ISliderService
    {
        Task<IEnumerable<SliderUIVM>> GetSliderUIVMAsync();
    }
}
