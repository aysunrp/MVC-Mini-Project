using MCV_Mini_Project.Areas.Admin.ViewModels.Author;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;

        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuthorListVM>> GetAllAsync()
        {
            return await _context.Authors
                .Select(x => new AuthorListVM
                {
                    Id       = x.Id,
                    FullName = x.FullName
                })
                .ToListAsync();
        }

        public async Task<AuthorEditVM?> GetByIdAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author is null) return null;

            return new AuthorEditVM { Id = author.Id, FullName = author.FullName };
        }

        public async Task CreateAsync(AuthorCreateVM vm)
        {
            var author = new Author { FullName = vm.FullName };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(AuthorEditVM vm)
        {
            var author = await _context.Authors.FindAsync(vm.Id);
            if (author is null) return false;

            author.FullName = vm.FullName;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author is null) return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
