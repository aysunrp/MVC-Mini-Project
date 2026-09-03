using MCV_Mini_Project.Areas.Admin.ViewModels.Teacher;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Helpers;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.Teachers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeacherService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<TeacherUIVM>> GetAllUIAsync()
        {
            var items = await _context.Teachers.Include(m => m.Position).ToListAsync();
            return items.Select(m => new TeacherUIVM
            {
                FullName = m.FullName,
                Image = FileHelper.GetFileName(m.Image),
                Position = m.Position.Name
            }).ToList();
        }

        public async Task<IEnumerable<TeacherListVM>> GetAllAsync()
        {
            var items = await _context.Teachers.Include(t => t.Position).ToListAsync();
            return items.Select(x => new TeacherListVM
            {
                Id = x.Id,
                FullName = x.FullName,
                Image = FileHelper.GetFileName(x.Image),
                PositionName = x.Position.Name
            }).ToList();
        }

        public async Task<TeacherEditVM?> GetByIdAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher is null) return null;
            return new TeacherEditVM
            {
                Id = teacher.Id, FullName = teacher.FullName,
                CurrentImage = FileHelper.GetFileName(teacher.Image), PositionId = teacher.PositionId
            };
        }

        public async Task CreateAsync(TeacherCreateVM vm)
        {
            var imagePath = await FileHelper.SaveImageAsync(vm.ImageFile, _env) ?? string.Empty;
            var teacher = new Teacher
            {
                FullName = vm.FullName, Image = imagePath, PositionId = vm.PositionId
            };
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(TeacherEditVM vm)
        {
            var teacher = await _context.Teachers.FindAsync(vm.Id);
            if (teacher is null) return false;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                FileHelper.DeleteImage(teacher.Image, _env);
                teacher.Image = await FileHelper.SaveImageAsync(vm.ImageFile, _env) ?? teacher.Image;
            }

            teacher.FullName = vm.FullName;
            teacher.PositionId = vm.PositionId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher is null) return false;
            FileHelper.DeleteImage(teacher.Image, _env);
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
