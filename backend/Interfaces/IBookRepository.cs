using backend.Data;
using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.Interfaces
{
    public interface IBookRepository: IAsyncRepository<Book>
    {
        public Task<PaginationDto<Book>> GetBooksbyName(string name, int pageNumber, int pageSize);
        public Task<PaginationDto<Book>> GetAllForLibrarianAsync(int pageSize, int pageNumber);
        public Task<PaginationDto<Book>> GetBooksbyNameForLibrarian(string name, int pageNumber, int pageSize);
    }
}
