using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.Interfaces
{
    public interface IAuthorRepository : IAsyncRepository<Author>
    {
        public Task<PaginationDto<Author>> GetAuthorsbyName(string name, int pageNumber, int pageSize);
    }
}
