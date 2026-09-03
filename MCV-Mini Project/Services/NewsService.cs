using MCV_Mini_Project.Areas.Admin.ViewModels.News;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Helpers;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.News;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class NewsService : INewsService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public NewsService(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }

        public async Task<IEnumerable<NewsUIVM>> GetNewsUIVMAsync()
        {
            var items = await _appDbContext.News.Include(n => n.Authors).ToListAsync();
            return items.Select(n => new NewsUIVM
            {
                Date = n.Date,
                AuthorName = n.Authors.FullName,
                Question = n.Question,
                Image = FileHelper.GetFileName(n.Image)
            }).ToList();
        }

        public async Task<IEnumerable<NewsListVM>> GetAllAsync()
        {
            var items = await _appDbContext.News.Include(n => n.Authors).ToListAsync();
            return items.Select(x => new NewsListVM
            {
                Id = x.Id,
                Image = FileHelper.GetFileName(x.Image),
                Question = x.Question,
                Date = x.Date,
                AuthorName = x.Authors.FullName
            }).ToList();
        }

        public async Task<NewsEditVM?> GetByIdAsync(int id)
        {
            var news = await _appDbContext.News.FindAsync(id);
            if (news is null) return null;
            return new NewsEditVM
            {
                Id = news.Id, CurrentImage = FileHelper.GetFileName(news.Image), Question = news.Question,
                Date = news.Date, AuthorId = news.AuthorId
            };
        }

        public async Task CreateAsync(NewsCreateVM vm)
        {
            var imagePath = await FileHelper.SaveImageAsync(vm.ImageFile, _env) ?? string.Empty;
            var news = new News
            {
                Image = imagePath, Question = vm.Question, Date = vm.Date, AuthorId = vm.AuthorId
            };
            await _appDbContext.News.AddAsync(news);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(NewsEditVM vm)
        {
            var news = await _appDbContext.News.FindAsync(vm.Id);
            if (news is null) return false;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                FileHelper.DeleteImage(news.Image, _env);
                news.Image = await FileHelper.SaveImageAsync(vm.ImageFile, _env) ?? news.Image;
            }

            news.Question = vm.Question;
            news.Date = vm.Date;
            news.AuthorId = vm.AuthorId;

            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var news = await _appDbContext.News.FindAsync(id);
            if (news is null) return false;
            FileHelper.DeleteImage(news.Image, _env);
            _appDbContext.News.Remove(news);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
