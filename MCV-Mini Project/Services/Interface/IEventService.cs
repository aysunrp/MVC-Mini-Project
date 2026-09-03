using MCV_Mini_Project.Areas.Admin.ViewModels.Event;
using MCV_Mini_Project.ViewModels.Events;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IEventService
    {
        // UI (frontend)
        Task<IEnumerable<EventUIVM>> GetEventUIVMAsync();

        // Admin CRUD
        Task<IEnumerable<EventListVM>> GetAllAsync();
        Task<EventEditVM?> GetByIdAsync(int id);
        Task CreateAsync(EventCreateVM vm);
        Task<bool> UpdateAsync(EventEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
