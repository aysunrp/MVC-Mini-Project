using MCV_Mini_Project.Data;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.Events;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class EventService : IEventService
    {
        private readonly AppDbContext _appDbContext;
        public EventService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<EventUIVM>> GetEventUIVMAsync()
        {
            IEnumerable<EventUIVM> events = await _appDbContext.Events.Select(m => new EventUIVM
            {
                DateDay = m.DateDay,
                Title = m.Title,
                Location = m.Location,
                DateMonth = m.DateMonth
            }).ToListAsync();
            return events;
        }
    }
}
