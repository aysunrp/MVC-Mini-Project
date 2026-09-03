using MCV_Mini_Project.Areas.Admin.ViewModels.AboutVision;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Helpers;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.AboutVisions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class VisionService : IVisionService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public VisionService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<AboutVisionUIVM> GetAllUIAsync()
        {
            var vision = await _context.AboutVisions.OrderByDescending(m => m.Id).FirstOrDefaultAsync();
            if (vision is null) return null;
            return new AboutVisionUIVM
            {
                Title = vision.Title,
                Description = vision.Description,
                Image = FileHelper.GetFileName(vision.Image)
            };
        }

        public async Task<IEnumerable<AboutVisionListVM>> GetAllAdminAsync()
        {
            var items = await _context.AboutVisions.ToListAsync();
            return items.Select(x => new AboutVisionListVM
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Image = FileHelper.GetFileName(x.Image)
            }).ToList();
        }

        public async Task<AboutVisionEditVM?> GetByIdAsync(int id)
        {
            var vision = await _context.AboutVisions.FindAsync(id);
            if (vision is null) return null;
            return new AboutVisionEditVM
            {
                Id = vision.Id, Title = vision.Title,
                Description = vision.Description, CurrentImage = FileHelper.GetFileName(vision.Image)
            };
        }

        public async Task CreateAsync(AboutVisionCreateVM vm)
        {
            var imagePath = await FileHelper.SaveImageAsync(vm.ImageFile, _env) ?? string.Empty;
            var vision = new AboutVision
            {
                Title = vm.Title, Description = vm.Description, Image = imagePath
            };
            await _context.AboutVisions.AddAsync(vision);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(AboutVisionEditVM vm)
        {
            var vision = await _context.AboutVisions.FindAsync(vm.Id);
            if (vision is null) return false;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                FileHelper.DeleteImage(vision.Image, _env);
                vision.Image = await FileHelper.SaveImageAsync(vm.ImageFile, _env) ?? vision.Image;
            }

            vision.Title = vm.Title;
            vision.Description = vm.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vision = await _context.AboutVisions.FindAsync(id);
            if (vision is null) return false;
            FileHelper.DeleteImage(vision.Image, _env);
            _context.AboutVisions.Remove(vision);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
