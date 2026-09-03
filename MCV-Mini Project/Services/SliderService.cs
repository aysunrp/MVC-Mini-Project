using MCV_Mini_Project.Areas.Admin.ViewModels.Slider;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Helpers;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.Slider;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public SliderService(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }

        public async Task<IEnumerable<SliderUIVM>> GetSliderUIVMAsync()
        {
            var items = await _appDbContext.Sliders.ToListAsync();
            return items.Select(m => new SliderUIVM
            {
                Image = FileHelper.GetFileName(m.Image),
                Logo = FileHelper.GetFileName(m.Logo) ?? m.Logo,
                Title = m.Title,
                Description = m.Description
            }).ToList();
        }

        public async Task<IEnumerable<SliderListVM>> GetAllAsync()
        {
            var items = await _appDbContext.Sliders.ToListAsync();
            return items.Select(x => new SliderListVM
            {
                Id = x.Id,
                Logo = x.Logo,
                Image = FileHelper.GetFileName(x.Image),
                Title = x.Title,
                Description = x.Description
            }).ToList();
        }

        public async Task<SliderEditVM?> GetByIdAsync(int id)
        {
            var slider = await _appDbContext.Sliders.FindAsync(id);
            if (slider is null) return null;
            return new SliderEditVM
            {
                Id = slider.Id, Logo = slider.Logo, CurrentImage = FileHelper.GetFileName(slider.Image),
                Title = slider.Title, Description = slider.Description
            };
        }

        public async Task CreateAsync(SliderCreateVM vm)
        {
            var imagePath = await FileHelper.SaveImageAsync(vm.ImageFile, _env) ?? string.Empty;
            var slider = new Slider
            {
                Logo = vm.Logo, Image = imagePath, Title = vm.Title, Description = vm.Description
            };
            await _appDbContext.Sliders.AddAsync(slider);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(SliderEditVM vm)
        {
            var slider = await _appDbContext.Sliders.FindAsync(vm.Id);
            if (slider is null) return false;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                FileHelper.DeleteImage(slider.Image, _env);
                slider.Image = await FileHelper.SaveImageAsync(vm.ImageFile, _env) ?? slider.Image;
            }

            slider.Logo = vm.Logo;
            slider.Title = vm.Title;
            slider.Description = vm.Description;

            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var slider = await _appDbContext.Sliders.FindAsync(id);
            if (slider is null) return false;
            FileHelper.DeleteImage(slider.Image, _env);
            _appDbContext.Sliders.Remove(slider);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
