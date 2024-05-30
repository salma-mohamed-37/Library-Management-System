using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Book>> GetAllAsync(int pageSize, int pageNumber)
        {
            var res = await _context.Books
                .AsNoTracking()
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book=>book.Borrowed)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
            return res;
        }

        public async Task<List<Book>> GetAllForLibrarianAsync(int pageSize, int pageNumber)
        {
            var res = await _context.Books
                .AsNoTracking()
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .ThenInclude(borrow => borrow.User)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
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

        public async Task<Borrowed> GetBorrowDetails(int bookId)
        {
            return await _context.Borrowed
                .Include(b => b.User)
                .Include(b => b.Book)
                .Where(b => b.BookId == bookId)
                .OrderByDescending(b => b.ReturnDate)
                .FirstOrDefaultAsync();
        }
    }
}
