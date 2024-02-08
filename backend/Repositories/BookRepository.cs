using backend.Data;
using backend.Interfaces;
using backend.Models;

namespace backend.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
