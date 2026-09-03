using MCV_Mini_Project.Areas.Admin.ViewModels.CourseInfo;

namespace MCV_Mini_Project.Services.Interface
{
    public interface ICourseInfoService
    {
        Task<IEnumerable<CourseInfoListVM>> GetAllAsync();
        Task<CourseInfoEditVM?> GetByIdAsync(int id);
        Task CreateAsync(CourseInfoCreateVM vm);
        Task<bool> UpdateAsync(CourseInfoEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
