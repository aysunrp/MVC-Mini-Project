using MCV_Mini_Project.Areas.Admin.ViewModels.Position;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
    public class PositionService : IPositionService
    {
        private readonly AppDbContext _context;

        public PositionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PositionListVM>> GetAllAsync()
        {
            return await _context.Positions
                .Select(x => new PositionListVM
                {
                    Id           = x.Id,
                    Name         = x.Name,
                    TeacherCount = x.Teachers.Count
                })
                .ToListAsync();
        }

        public async Task<PositionEditVM?> GetByIdAsync(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position is null) return null;

            return new PositionEditVM { Id = position.Id, Name = position.Name };
        }

        public async Task CreateAsync(PositionCreateVM vm)
        {
            var position = new Position { Name = vm.Name };
            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(PositionEditVM vm)
        {
            var position = await _context.Positions.FindAsync(vm.Id);
            if (position is null) return false;

            position.Name = vm.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position is null) return false;

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
