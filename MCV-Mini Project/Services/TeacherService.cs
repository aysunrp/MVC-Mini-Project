using MCV_Mini_Project.Data;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.Teachers;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class TeacherService:ITeacherService
    {
        private readonly AppDbContext _context;
        public TeacherService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherUIVM>> GetAllUIAsync()
        {
            var teachers = await _context.Teachers.Include(m => m.Position).Select(m => new TeacherUIVM
            {
                FullName = m.FullName,
                Image = m.Image,
                Position = m.Position.Name
            }).ToListAsync();
            return teachers;
        }
    }
}
