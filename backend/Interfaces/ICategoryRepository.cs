using backend.Models;

namespace backend.Interfaces
{
    public interface ICategoryRepository:IAsyncRepository<Category>
    {
        public Task<List<Category>> GetCategoriesbyName(string name);
    }
}
