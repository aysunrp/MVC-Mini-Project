using MCV_Mini_Project.Areas.Admin.ViewModels.Position;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IPositionService
    {
        Task<IEnumerable<PositionListVM>> GetAllAsync();
        Task<PositionEditVM?> GetByIdAsync(int id);
        Task CreateAsync(PositionCreateVM vm);
        Task<bool> UpdateAsync(PositionEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
