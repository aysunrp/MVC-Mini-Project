using MCV_Mini_Project.ViewModels.Teachers;

namespace MCV_Mini_Project.Services.Interface
{
    public interface ITeacherService
    {
        public  Task<IEnumerable<TeacherUIVM>> GetAllUIAsync();
    }
}
