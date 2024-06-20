using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.Interfaces
{
    public interface ICategoryRepository:IAsyncRepository<Category>
    {
        public Task<PaginationDto<Category>> GetCategoriesbyName(string name, int pageNumber, int pageSize);
    }
}
