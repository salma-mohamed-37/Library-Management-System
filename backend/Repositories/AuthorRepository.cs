using backend.Data;
using backend.Dtos.GetDtos;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace backend.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Author>>getAllAsync()
        {
            return await  _context.Authors.AsNoTracking().ToListAsync();
        }

        public async Task<PaginationDto<Author>>GetAuthorsbyName(string name, int pageNumber, int pageSize)
        {
            var query = _context.Authors
                .Where(c => c.Name.Contains(name))
               .OrderBy(b => b.Name);


            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Author>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;
            
        }

        public async Task<bool> IsExists(int id, CancellationToken cancellationToken)
        {
            return await  _context.Authors.AnyAsync(c => c.Id == id, cancellationToken);  
        }

        public async Task<bool> IsNameExists(string name, CancellationToken cancellationToken)
        {
            return await _context.Authors.AnyAsync(c => c.Name == name, cancellationToken);
        }

        public async Task<ICollection<string>> GetAuthorsNames()
        {
            var res = await _context.Authors
            .AsNoTracking()
            .Select(c => c.Name)
            .ToListAsync();

            return res;
        }
    }
}
