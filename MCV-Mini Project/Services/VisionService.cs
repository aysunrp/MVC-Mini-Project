
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.AboutVisions;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class VisionService:IVisionService
    {
        private readonly AppDbContext _context;
        public VisionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AboutVisionUIVM> GetAllUIAsync()
        {
            var rights = await _context.AboutVisions.OrderByDescending(m => m.Id).Select(m => new AboutVisionUIVM
            {
                Title = m.Title,
                Description = m.Description,
                Image = m.Image
            }).FirstOrDefaultAsync();

            return rights;
        }
    }
}

