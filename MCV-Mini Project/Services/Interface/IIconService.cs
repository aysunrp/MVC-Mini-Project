using MCV_Mini_Project.ViewModels.Icons;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IIconService
    {
        Task<IEnumerable<IconUIVM>> GetIconUIVMAsync();
    }
}
