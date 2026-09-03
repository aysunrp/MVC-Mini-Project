using MCV_Mini_Project.Areas.Admin.ViewModels.CourseInfo;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Helpers;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class CourseInfoService : ICourseInfoService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CourseInfoService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<CourseInfoListVM>> GetAllAsync()
        {
            return await _context.CourseInfos
                .Include(x => x.Teacher)
                .Include(x => x.CourseImages)
                .Select(x => new CourseInfoListVM
                {
                    Id          = x.Id,
                    Title       = x.Title,
                    Price       = x.Price,
                    SalesCount  = x.SalesCount,
                    IsFeatured  = x.IsFeatured,
                    IsNew       = x.IsNew,
                    TeacherName = x.Teacher.FullName,
                    Image       = x.CourseImages.Where(i => i.IsMain).Select(i => i.Name).FirstOrDefault()
                                  ?? x.CourseImages.Select(i => i.Name).FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<CourseInfoEditVM?> GetByIdAsync(int id)
        {
            var course = await _context.CourseInfos
                .Include(x => x.CourseImages)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (course is null) return null;

            var imageName = course.CourseImages.FirstOrDefault(x => x.IsMain)?.Name
                            ?? course.CourseImages.FirstOrDefault()?.Name;

            return new CourseInfoEditVM
            {
                Id           = course.Id,
                Title        = course.Title,
                Description  = course.Description,
                Price        = course.Price,
                SalesCount   = course.SalesCount,
                IsFeatured   = course.IsFeatured,
                IsNew        = course.IsNew,
                TeacherId    = course.TeacherId,
                CurrentImage = FileHelper.GetFileName(imageName)
            };
        }

        public async Task CreateAsync(CourseInfoCreateVM vm)
        {
            var course = new CourseInfo
            {
                Title       = vm.Title,
                Description = vm.Description,
                Price       = vm.Price,
                SalesCount  = vm.SalesCount,
                IsFeatured  = vm.IsFeatured,
                IsNew       = vm.IsNew,
                TeacherId   = vm.TeacherId
            };
            await _context.CourseInfos.AddAsync(course);
            await _context.SaveChangesAsync();

            var imageName = await FileHelper.SaveImageAsync(vm.ImageFile, _env);
            if (!string.IsNullOrEmpty(imageName))
            {
                await _context.CourseImages.AddAsync(new CourseImage
                {
                    Name         = imageName,
                    IsMain       = true,
                    CourseInfoId = course.Id
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateAsync(CourseInfoEditVM vm)
        {
            var course = await _context.CourseInfos
                .Include(x => x.CourseImages)
                .FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (course is null) return false;

            course.Title       = vm.Title;
            course.Description = vm.Description;
            course.Price       = vm.Price;
            course.SalesCount  = vm.SalesCount;
            course.IsFeatured  = vm.IsFeatured;
            course.IsNew       = vm.IsNew;
            course.TeacherId   = vm.TeacherId;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                var newName = await FileHelper.SaveImageAsync(vm.ImageFile, _env);
                var main = course.CourseImages.FirstOrDefault(x => x.IsMain)
                           ?? course.CourseImages.FirstOrDefault();

                if (main != null)
                {
                    FileHelper.DeleteImage(main.Name, _env);
                    main.Name = newName ?? main.Name;
                    main.IsMain = true;
                }
                else if (!string.IsNullOrEmpty(newName))
                {
                    await _context.CourseImages.AddAsync(new CourseImage
                    {
                        Name         = newName,
                        IsMain       = true,
                        CourseInfoId = course.Id
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.CourseInfos
                .Include(x => x.CourseImages)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (course is null) return false;

            foreach (var img in course.CourseImages)
                FileHelper.DeleteImage(img.Name, _env);

            _context.CourseImages.RemoveRange(course.CourseImages);
            _context.CourseInfos.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
