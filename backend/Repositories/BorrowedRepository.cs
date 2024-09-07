using backend.Data;
using backend.Dtos.GetDtos;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace backend.Repositories
{
    public class BorrowedRepository : BaseRepository<Borrowed>, IBorrowedRepository
    {
        public BorrowedRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task Return(ICollection<int> booksIds, string userId)
        {
            var borrowedBooks = await _context.Borrowed
            .Where(b => b.UserId == userId && booksIds.Contains(b.BookId) && b.currently_borrowed == true)
            .ToListAsync();

            foreach (var book in borrowedBooks)
            {
                book.ReturnDate = DateTime.Now;
                book.currently_borrowed = false;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Borrowed>> GetCurrentlyBorrowedBooksByUser(string UserId)
        {
            var res = await _context.Borrowed
                .Where(b => b.currently_borrowed == true)
                .Include(b => b.User)
                .Where(b => b.User.Id == UserId)
                .Include(b => b.Book)
                .ThenInclude(b => b.Author)
                .Include(b => b.Book)
                .ThenInclude(b => b.Category)
                .OrderByDescending(b => b.BorrowDate)
                .ToListAsync();

            return res;
        }

        public async Task<PaginationDto<Borrowed>> GetUserBorrowHistory(string userId, int pageSize, int pageNumber)
        {

            var query = _context.Borrowed
                .Where(b => b.currently_borrowed == false)
                .Where(b => b.UserId == userId)
                .Include(b => b.Book)
                .ThenInclude(b => b.Author)
                .Include(b => b.Book)
                .ThenInclude(b => b.Category)
                .OrderByDescending(b => b.BorrowDate);

            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Borrowed>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;


        }

        public async Task<PaginationDto<Borrowed>> GetBookBorrowHistory(int bookId, int pageSize, int pageNumber)
        {
            var query = _context.Borrowed
                .Where(b => b.currently_borrowed == false)
                .Where(b => b.BookId == bookId)
                .Include(b => b.User)
                .OrderByDescending(b => b.BorrowDate);

            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Borrowed>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;


        }

        public async Task<bool> IsBorrowed(int bookId)
        {
            return await _context.Borrowed.AnyAsync(b => b.BookId == bookId && b.currently_borrowed == true);
        }

        public async Task<bool> IsCurrentlyBorrower(string userId)
        {
            return await _context.Borrowed.AnyAsync(b => b.UserId == userId && b.currently_borrowed == true);
        }

    }
}
