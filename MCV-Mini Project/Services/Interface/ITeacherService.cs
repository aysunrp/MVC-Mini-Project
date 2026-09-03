using MCV_Mini_Project.Areas.Admin.ViewModels.Teacher;
using MCV_Mini_Project.ViewModels.Teachers;

namespace MCV_Mini_Project.Services.Interface
{
    public interface ITeacherService
    {
        // UI (frontend)
        Task<IEnumerable<TeacherUIVM>> GetAllUIAsync();

        // Admin CRUD
        Task<IEnumerable<TeacherListVM>> GetAllAsync();
        Task<TeacherEditVM?> GetByIdAsync(int id);
        Task CreateAsync(TeacherCreateVM vm);
        Task<bool> UpdateAsync(TeacherEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
