using MCV_Mini_Project.Areas.Admin.ViewModels.Author;

namespace MCV_Mini_Project.Services.Interface
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorListVM>> GetAllAsync();
        Task<AuthorEditVM?> GetByIdAsync(int id);
        Task CreateAsync(AuthorCreateVM vm);
        Task<bool> UpdateAsync(AuthorEditVM vm);
        Task<bool> DeleteAsync(int id);
    }
}
