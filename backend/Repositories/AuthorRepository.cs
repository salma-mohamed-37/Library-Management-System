using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext context) : base(context)
        {
        }

        async Task<List<Author>> IAuthorRepository.GetAuthorsbyName(string name)
        {
           return await _context.Authors.Where(c => c.Name.Contains(name)).ToListAsync();
        }
    }
}
