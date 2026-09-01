using MCV_Mini_Project.Data;
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

        public async Task<Dictionary<string, string>> GetAllUIAsync()
        {
            var settings = await _appDbContext.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);
            return settings;
        }
    }
}
