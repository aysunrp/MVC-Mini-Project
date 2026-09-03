using MCV_Mini_Project.Areas.Admin.ViewModels.Setting;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _appDbContext;

        public SettingService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // UI (frontend)
        public async Task<Dictionary<string, string>> GetAllUIAsync()
        {
            var settings = await _appDbContext.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);
            return settings;
        }

        // Admin CRUD
        public async Task<IEnumerable<SettingListVM>> GetAllAsync()
        {
            return await _appDbContext.Settings
                .Select(x => new SettingListVM
                {
                    Id    = x.Id,
                    Key   = x.Key,
                    Value = x.Value
                })
                .ToListAsync();
        }

        public async Task<SettingEditVM?> GetByIdAsync(int id)
        {
            var setting = await _appDbContext.Settings.FindAsync(id);
            if (setting is null) return null;

            return new SettingEditVM { Id = setting.Id, Key = setting.Key, Value = setting.Value };
        }

        public async Task CreateAsync(SettingCreateVM vm)
        {
            var setting = new Setting { Key = vm.Key, Value = vm.Value };
            await _appDbContext.Settings.AddAsync(setting);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(SettingEditVM vm)
        {
            var setting = await _appDbContext.Settings.FindAsync(vm.Id);
            if (setting is null) return false;

            setting.Key   = vm.Key;
            setting.Value = vm.Value;

            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var setting = await _appDbContext.Settings.FindAsync(id);
            if (setting is null) return false;

            _appDbContext.Settings.Remove(setting);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
