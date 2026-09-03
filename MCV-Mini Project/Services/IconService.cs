using MCV_Mini_Project.Areas.Admin.ViewModels.Icon;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.Icons;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class IconService : IIconService
    {
        private readonly AppDbContext _appDbContext;

        public IconService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // UI (frontend)
        public async Task<IEnumerable<IconUIVM>> GetIconUIVMAsync()
        {
            var icon = await _appDbContext.Icons.Select(m => new IconUIVM
            {
                Name = m.Name
            }).ToListAsync();
            return icon;
        }

        // Admin CRUD
        public async Task<IEnumerable<IconListVM>> GetAllAsync()
        {
            return await _appDbContext.Icons
                .Select(x => new IconListVM
                {
                    Id   = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<IconEditVM?> GetByIdAsync(int id)
        {
            var icon = await _appDbContext.Icons.FindAsync(id);
            if (icon is null) return null;

            return new IconEditVM { Id = icon.Id, Name = icon.Name };
        }

        public async Task CreateAsync(IconCreateVM vm)
        {
            var icon = new Icon { Name = vm.Name };
            await _appDbContext.Icons.AddAsync(icon);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(IconEditVM vm)
        {
            var icon = await _appDbContext.Icons.FindAsync(vm.Id);
            if (icon is null) return false;

            icon.Name = vm.Name;
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var icon = await _appDbContext.Icons.FindAsync(id);
            if (icon is null) return false;

            _appDbContext.Icons.Remove(icon);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
