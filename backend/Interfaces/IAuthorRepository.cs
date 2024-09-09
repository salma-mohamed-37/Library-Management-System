using backend.Dtos.GetDtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Interfaces
{
    public interface IAuthorRepository : IAsyncRepository<Author>
    {
        public Task<PaginationDto<Author>> GetAuthorsbyName(string name, int pageNumber, int pageSize);

        public Task<bool> IsExists(int id, CancellationToken cancellationToken);
        public Task<bool> IsNameExists(string name, CancellationToken cancellationToken);
        public Task<ICollection<string>> GetAuthorsNames();
        public Task<ICollection<Author>> getAllAsync();


    }
}
