using MCV_Mini_Project.Data;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.Icons;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Services
{
 
    
        public class IconService : IIconService
        {
            private readonly AppDbContext _appDbContext;
            public IconService(AppDbContext appDbContext)
            {
                _appDbContext = appDbContext;
            }



            public async Task<IEnumerable<IconUIVM>> GetIconUIVMAsync()
            {
                var icon = await _appDbContext.Icons.Select(m => new IconUIVM
                {
                    Name = m.Name
                }).ToListAsync();
                return icon;
            }
        }
    }

