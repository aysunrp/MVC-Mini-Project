using MCV_Mini_Project.Data;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.Videos;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class VideoService :IVideoService
    {
        private readonly AppDbContext _appDbContext;
        public VideoService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<VideoUIVM> GettAllUIAsync()
        {
            var videos = await _appDbContext.Videos.OrderByDescending(m => m.Id).Select(m => new VideoUIVM
            {
                Name = m.Name
            }).FirstOrDefaultAsync();
            return videos;
        }
    }
}
