using backend.Data;
using backend.Dtos.GetDtos;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace backend.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        async public Task<List<Book>> GetBooksbyName(string name)
        {
            return await _context.Books
                .AsNoTracking()
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book=>book.Borrowed)
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }

        async public Task<List<Book>> GetBooksbyNameForLibrarian(string name)
        {
            return await _context.Books
                .AsNoTracking()
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .ThenInclude(borrow => borrow.User)
                .Where(b => b.Name.Contains(name))
                .ToListAsync();


        }

        public async Task<PaginationDto<Book>> GetAllAsync(int pageSize, int pageNumber)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed);

            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Book>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;

        }

        public async Task<PaginationDto<Book>> GetAllForLibrarianAsync(int pageSize, int pageNumber)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .ThenInclude(borrow => borrow.User);

            var data = await query
              .Skip(pageSize * (pageNumber - 1))
              .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Book>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };
            return res;

            
        }
        public async Task<Book> GetbyIdAsync(int id)
        {
            return await _context.Books
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .FirstOrDefaultAsync(book => book.Id == id);
        }


    }
}
