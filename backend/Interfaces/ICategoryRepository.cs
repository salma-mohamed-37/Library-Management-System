using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.Interfaces
{
    public interface ICategoryRepository:IAsyncRepository<Category>
    {
        public Task<PaginationDto<Category>> GetCategoriesbyName(string name, int pageNumber, int pageSize);

        public Task<ICollection<string>> GetCategoriesNames();

        public Task<bool> IsExists(int id, CancellationToken cancellationToken);
        public Task<bool> IsNameExists(string name, CancellationToken cancellationToken);

    }
}
