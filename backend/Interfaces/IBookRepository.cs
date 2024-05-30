using backend.Data;
using backend.Models;

namespace backend.Interfaces
{
    public interface IBookRepository: IAsyncRepository<Book>
    {
        public Task<List<Book>> GetBooksbyName(string name);
        public Task<List<Book>> GetAllForLibrarianAsync(int pageSize, int pageNumber);
        public Task<List<Book>> GetBooksbyNameForLibrarian(string name);
    }
}
