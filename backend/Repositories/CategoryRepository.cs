using backend.Data;
using backend.Dtos.GetDtos;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace backend.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<PaginationDto<Category>> GetCategoriesbyName (string name, int pageNumber, int pageSize)
        {
            var query = _context.Categories
                .Where(c => c.Name.Contains(name))
               .OrderBy(b => b.Name);

            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Category>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;

        }

        public async Task<ICollection<string>> GetCategoriesNames()
        {
            var res =await _context.Categories
            .AsNoTracking()
            .Select(c=>c.Name)
            .ToListAsync();

            return res;
        }

        public async Task<bool> IsExists(int id, CancellationToken cancellationToken)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id,cancellationToken);
        }

        public async Task<bool> IsNameExists(string name, CancellationToken cancellationToken)
        {
            return await _context.Categories.AnyAsync(c => c.Name == name, cancellationToken);

        }
    }
}
