using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<Category>> GetCategoriesbyName (string name)
        {
           return await _context.Categories.Where(c => c.Name.Contains(name)).ToListAsync();
        }
    }
}
