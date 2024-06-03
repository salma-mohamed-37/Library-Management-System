using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class BorrowedRepository : BaseRepository<Borrowed>, IBorrowedRepository
    {
        public BorrowedRepository(ApplicationDbContext context) : base(context)
        {
        }


        public async Task Return(int bookId, string userId)
        {
            var b = await _context.Borrowed
                .FirstOrDefaultAsync(b => b.UserId == userId && b.BookId == bookId && b.currently_borrowed ==true);
            b.ReturnDate = DateTime.Now;
            b.currently_borrowed = false;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Borrowed>> GetCurrentlyBorrowedBooksByUser(string UserId)
        {
            var b = await _context.Borrowed
                .Where(b => b.currently_borrowed == true)
                .Include(b => b.User)
                .Where(b => b.User.Id == UserId)
                .Include(b => b.Book)
                .ToListAsync();
            return b;

        }

        public async Task<List<Borrowed>> GetUserBorrowHistory(string userId)
        {
            var res = await _context.Borrowed
                .Where(b => b.currently_borrowed == false)
                .Where(b => b.UserId == userId)
                .Include(b => b.Book)
                .OrderByDescending(b => b.BorrowDate)
                .ToListAsync();
            return res;
        }
       
        public async Task<List<Borrowed>> GetBookBorrowHistory(int bookId)
        {
            var res = await _context.Borrowed
                .Where(b => b.currently_borrowed == false)
                .Where(b => b.BookId == bookId)
                .Include(b => b.User)
                .OrderByDescending(b=>b.BorrowDate)
                .ToListAsync();
            return res;
        }
    }
}
