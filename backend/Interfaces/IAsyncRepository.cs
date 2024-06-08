using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {
        public Task<PaginationDto<T>> GetAllAsync(int pageSize, int pageNumber);
        public Task<T> GetbyIdAsync(int id);
        public Task<T> AddAsync(T t);
        public Task UpdateAsync(T t);
        public Task<bool> DeleteAsync(int id);

    }
}
