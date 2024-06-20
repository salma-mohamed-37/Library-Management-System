using backend.Data;
using backend.Dtos.GetDtos;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace backend.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        async public Task<PaginationDto<Book>> GetBooksbyName(string name, int pageNumber, int pageSize)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .Where(c => c.Name.Contains(name))
                .OrderBy(b => b.Name);


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

        async public Task<PaginationDto<Book>> GetBooksbyNameForLibrarian(string name, int pageNumber, int pageSize)
        {
            var query = _context.Books
              .AsNoTracking()
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .ThenInclude(borrow => borrow.User)
                .Where(b => b.Name.Contains(name))
                .OrderBy(b => b.Name);

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

        public async Task<PaginationDto<Book>> GetAllAsync(int pageSize, int pageNumber)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .OrderBy(b => b.Name);

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
                .ThenInclude(borrow => borrow.User)
                 .OrderBy(b => b.Name);

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
