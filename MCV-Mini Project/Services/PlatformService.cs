using MCV_Mini_Project.Data;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.AboutPlatforms;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class PlatformService:IPlatformService
    {
        private readonly AppDbContext _context;
        public PlatformService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AboutPlatformUIVM> GetAllUIAsync()
        {
            var left = await _context.AboutPlatforms.OrderByDescending(m => m.Id).Select(m => new AboutPlatformUIVM
            {
                Title = m.Title,
                Description = m.Description,
                Image = m.Image
            }).FirstOrDefaultAsync();
            return left;
        }
    }
}

