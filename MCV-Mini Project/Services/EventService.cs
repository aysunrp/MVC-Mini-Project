using MCV_Mini_Project.Areas.Admin.ViewModels.Event;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Models;
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

        // UI (frontend)
        public async Task<IEnumerable<EventUIVM>> GetEventUIVMAsync()
        {
            IEnumerable<EventUIVM> events = await _appDbContext.Events.Select(m => new EventUIVM
            {
                DateDay   = m.DateDay,
                Title     = m.Title,
                Location  = m.Location,
                DateMonth = m.DateMonth
            }).ToListAsync();
            return events;
        }

        // Admin CRUD
        public async Task<IEnumerable<EventListVM>> GetAllAsync()
        {
            return await _appDbContext.Events
                .Select(x => new EventListVM
                {
                    Id        = x.Id,
                    DateDay   = x.DateDay,
                    DateMonth = x.DateMonth,
                    Title     = x.Title,
                    Location  = x.Location
                })
                .ToListAsync();
        }

        public async Task<EventEditVM?> GetByIdAsync(int id)
        {
            var ev = await _appDbContext.Events.FindAsync(id);
            if (ev is null) return null;

            return new EventEditVM
            {
                Id        = ev.Id,
                DateDay   = ev.DateDay,
                DateMonth = ev.DateMonth,
                Title     = ev.Title,
                Location  = ev.Location
            };
        }

        public async Task CreateAsync(EventCreateVM vm)
        {
            var ev = new Event
            {
                DateDay   = vm.DateDay,
                DateMonth = vm.DateMonth,
                Title     = vm.Title,
                Location  = vm.Location
            };
            await _appDbContext.Events.AddAsync(ev);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(EventEditVM vm)
        {
            var ev = await _appDbContext.Events.FindAsync(vm.Id);
            if (ev is null) return false;

            ev.DateDay   = vm.DateDay;
            ev.DateMonth = vm.DateMonth;
            ev.Title     = vm.Title;
            ev.Location  = vm.Location;

            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ev = await _appDbContext.Events.FindAsync(id);
            if (ev is null) return false;

            _appDbContext.Events.Remove(ev);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
