using MCV_Mini_Project.Data;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.Slider;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _appDbContext;
        public SliderService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<SliderUIVM>> GetSliderUIVMAsync()
        {
            var sliders = await _appDbContext.Sliders.Select(m => new SliderUIVM
            {
                Image = m.Image,
                Logo = m.Logo,
                Title = m.Title,
                Description = m.Description
            }).ToListAsync();
            return sliders;
        }
    }
}

