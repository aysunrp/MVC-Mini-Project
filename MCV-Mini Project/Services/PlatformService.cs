using MCV_Mini_Project.Areas.Admin.ViewModels.AboutPlatform;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Helpers;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.AboutPlatforms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlatformService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<AboutPlatformUIVM> GetAllUIAsync()
        {
            var platform = await _context.AboutPlatforms.OrderByDescending(m => m.Id).FirstOrDefaultAsync();
            if (platform is null) return null;
            return new AboutPlatformUIVM
            {
                Title = platform.Title,
                Description = platform.Description,
                Image = FileHelper.GetFileName(platform.Image)
            };
        }

        public async Task<IEnumerable<AboutPlatformListVM>> GetAllAdminAsync()
        {
            var items = await _context.AboutPlatforms.ToListAsync();
            return items.Select(x => new AboutPlatformListVM
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Image = FileHelper.GetFileName(x.Image)
            }).ToList();
        }

        public async Task<AboutPlatformEditVM?> GetByIdAsync(int id)
        {
            var platform = await _context.AboutPlatforms.FindAsync(id);
            if (platform is null) return null;
            return new AboutPlatformEditVM
            {
                Id = platform.Id, Title = platform.Title,
                Description = platform.Description, CurrentImage = FileHelper.GetFileName(platform.Image)
            };
        }

        public async Task CreateAsync(AboutPlatformCreateVM vm)
        {
            var imagePath = await FileHelper.SaveImageAsync(vm.ImageFile, _env) ?? string.Empty;
            var platform = new AboutPlatform
            {
                Title = vm.Title, Description = vm.Description, Image = imagePath
            };
            await _context.AboutPlatforms.AddAsync(platform);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(AboutPlatformEditVM vm)
        {
            var platform = await _context.AboutPlatforms.FindAsync(vm.Id);
            if (platform is null) return false;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                FileHelper.DeleteImage(platform.Image, _env);
                platform.Image = await FileHelper.SaveImageAsync(vm.ImageFile, _env) ?? platform.Image;
            }

            platform.Title = vm.Title;
            platform.Description = vm.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var platform = await _context.AboutPlatforms.FindAsync(id);
            if (platform is null) return false;
            FileHelper.DeleteImage(platform.Image, _env);
            _context.AboutPlatforms.Remove(platform);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
