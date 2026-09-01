using MCV_Mini_Project.Data;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.CourseInfos;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseInfoUIVM>> GetAllAsync()
        {
            return await _context.CourseInfos
                .Include(x => x.Teacher)
                .Include(x => x.CourseImages)
                .Select(x => new CourseInfoUIVM
                {
                    Title = x.Title,
                    Description = x.Description,
                    Price = x.Price,
                    SalesCount = x.SalesCount,
                    IsFeatured = x.IsFeatured,
                    IsNew = x.IsNew,
                    TeacherId = x.TeacherId,
                    Teacher = x.Teacher,
                    CourseImages = x.CourseImages
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseInfoUIVM>> SearchAsync(string name)
        {
            return await _context.CourseInfos
                .Include(x => x.Teacher)
                .Include(x => x.CourseImages)
                .Where(x => x.Title.Contains(name))
                .Select(x => new CourseInfoUIVM
                {
                    Title = x.Title,
                    Description = x.Description,
                    Price = x.Price,
                    SalesCount = x.SalesCount,
                    IsFeatured = x.IsFeatured,
                    IsNew = x.IsNew,
                    TeacherId = x.TeacherId,
                    Teacher = x.Teacher,
                    CourseImages = x.CourseImages
                })
                .ToListAsync();
        }
    }
}
