using backend.Models;

namespace backend.Interfaces
{
    public interface IAuthorRepository : IAsyncRepository<Author>
    {
        public Task<List<Author>> GetAuthorsbyName(string name);
    }
}
