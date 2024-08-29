using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.Interfaces
{
    public interface IAuthorRepository : IAsyncRepository<Author>
    {
        public Task<PaginationDto<Author>> GetAuthorsbyName(string name, int pageNumber, int pageSize);

        public Task<bool> IsExists(int id, CancellationToken cancellationToken);
        public Task<bool> IsNameExists(string name, CancellationToken cancellationToken);
    }
}
