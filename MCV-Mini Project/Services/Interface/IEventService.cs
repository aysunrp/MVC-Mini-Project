using MCV_Mini_Project.ViewModels.Events;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IEventService
    {
        Task<IEnumerable<EventUIVM>> GetEventUIVMAsync();
    }
}
