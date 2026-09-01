using MCV_Mini_Project.ViewModels.CourseInfos;

namespace MCV_Mini_Project.Services.Interface
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseInfoUIVM>> GetAllAsync();
        Task<IEnumerable<CourseInfoUIVM>> SearchAsync(string name);
    }
}
