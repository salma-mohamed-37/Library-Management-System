using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.Interfaces
{
    public interface IBorrowedRepository : IAsyncRepository<Borrowed>
    {
        public Task Return(ICollection<int> booksIds, string userId);
        public Task<ICollection<Borrowed>> GetCurrentlyBorrowedBooksByUser(string UserId);
        public Task<PaginationDto<Borrowed>> GetUserBorrowHistory(string userId, int pageSize, int pageNumber);
        public Task<PaginationDto<Borrowed>> GetBookBorrowHistory(int bookId, int pageSize, int pageNumber);
        public Task<bool>IsBorrowed(int bookId);

        public Task<bool> IsCurrentlyBorrower(string userId);



    }
}
