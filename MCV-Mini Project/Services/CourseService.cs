using MCV_Mini_Project.Data;
using MCV_Mini_Project.Models;
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
            var courses = await Query().ToListAsync();
            return courses.Select(Map).ToList();
        }

        public async Task<IEnumerable<CourseInfoUIVM>> SearchAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return await GetAllAsync();

            var term = name.Trim().ToLower();
            var courses = await Query()
                .Where(x => x.Title.ToLower().Contains(term))
                .ToListAsync();
            return courses.Select(Map).ToList();
        }

        public async Task<CourseInfoUIVM?> GetByIdAsync(int id)
        {
            var course = await Query().FirstOrDefaultAsync(x => x.Id == id);
            return course is null ? null : Map(course);
        }

        private IQueryable<CourseInfo> Query()
        {
            return _context.CourseInfos
                .Include(x => x.Teacher)
                    .ThenInclude(t => t.Position)
                .Include(x => x.CourseImages);
        }

        private static CourseInfoUIVM Map(CourseInfo x)
        {
            return new CourseInfoUIVM
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                SalesCount = x.SalesCount,
                IsFeatured = x.IsFeatured,
                IsNew = x.IsNew,
                TeacherId = x.TeacherId,
                Teacher = x.Teacher,
                CourseImages = x.CourseImages
            };
        }
    }
}
