using backend.Models;

namespace backend.Interfaces
{
    public interface IBorrowedRepository : IAsyncRepository<Borrowed>
    {
        public Task Return(int bookId, string userId);
        //public Task<List<Borrowed>> GetCurrentlyBorrowedBokksByUser(string UserId);
        //public Task<List<Borrowed>> GetUserBorrowHistory(string userId);
        //public Task<List<Borrowed>> GetbookBorrowHistory(int bookId);
    }
}
