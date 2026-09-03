using MCV_Mini_Project.Areas.Admin.ViewModels.Video;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.Videos;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class VideoService : IVideoService
    {
        private readonly AppDbContext _appDbContext;

        public VideoService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // UI (frontend)
        public async Task<VideoUIVM> GettAllUIAsync()
        {
            var videos = await _appDbContext.Videos
                .OrderByDescending(m => m.Id)
                .Select(m => new VideoUIVM { Name = m.Name })
                .FirstOrDefaultAsync();
            return videos;
        }

        // Admin CRUD
        public async Task<IEnumerable<VideoListVM>> GetAllAsync()
        {
            return await _appDbContext.Videos
                .Select(x => new VideoListVM { Id = x.Id, Name = x.Name })
                .ToListAsync();
        }

        public async Task<VideoEditVM?> GetByIdAsync(int id)
        {
            var video = await _appDbContext.Videos.FindAsync(id);
            if (video is null) return null;

            return new VideoEditVM { Id = video.Id, Name = video.Name };
        }

        public async Task CreateAsync(VideoCreateVM vm)
        {
            var video = new Video { Name = vm.Name };
            await _appDbContext.Videos.AddAsync(video);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(VideoEditVM vm)
        {
            var video = await _appDbContext.Videos.FindAsync(vm.Id);
            if (video is null) return false;

            video.Name = vm.Name;
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var video = await _appDbContext.Videos.FindAsync(id);
            if (video is null) return false;

            _appDbContext.Videos.Remove(video);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
