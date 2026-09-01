using MCV_Mini_Project.Data;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.News;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class NewsService : INewsService
    {
        private readonly AppDbContext _appDbContext;
        public NewsService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<IEnumerable<NewsUIVM>> GetNewsUIVMAsync()
        {
            var news = await _appDbContext.News.Include(n => n.Authors).Select(n => new NewsUIVM
            {
                Date = n.Date,
                AuthorName = n.Authors.FullName,
                Question = n.Question,
                Image = n.Image
            }).ToListAsync();
            return news;
        }
    }
}
