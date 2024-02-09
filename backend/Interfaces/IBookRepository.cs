using backend.Data;
using backend.Models;

namespace backend.Interfaces
{
    public interface IBookRepository: IAsyncRepository<Book>
    {
        public Task<List<Book>> GetBooksbyName(string name);
    }
}
