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

        /*
        public Task Return(int bookId, Guid userId);
        public Task<List<Borrowed>> GetCurrentlyBorrowedBokksByUser(Guid UserId);
        public Task<List<Borrowed>> GetUserBorrowHistory(Guid userId);
        public Task<List<Borrowed>> GetbookBorrowHistory(int bookId);
         */

        public async Task Return(int bookId, string userId)
        {
            var b = await _context.Borrowed
                .FirstOrDefaultAsync(b => b.UserId == userId && b.BookId == bookId && b.currently_borrowed ==true);
            b.ReturnDate = DateTime.Now;
            b.currently_borrowed = false;
            await _context.SaveChangesAsync();
        }
    }
}
