using backend.Data;
using backend.Dtos.GetDtos;
using backend.Models;
using System.Threading;

namespace backend.Interfaces
{
    public interface IBookRepository: IAsyncRepository<Book>
    {
        public Task<PaginationDto<Book>> GetBooksbyName(string name, int pageNumber, int pageSize);
        public Task<PaginationDto<Book>> GetAllForLibrarianAsync(int pageSize, int pageNumber);
        public Task<PaginationDto<Book>> GetBooksbyNameForLibrarian(string name, int pageNumber, int pageSize);
        public Task<PaginationDto<Book>> GetBooksbyAuthor(int authorId, int pageNumber, int pageSize);
        public Task<PaginationDto<Book>> GetBooksbyCategory(int categoryId, int pageNumber, int pageSize);
        public Task<PaginationDto<Book>> GetFilteredBooks(FilteringRequest request);
        public Task<PaginationDto<Book>> GetAvailablebyNameForLibrarian(string name, int pageNumber, int pageSize);
        public Task<bool> IsExists(int id, CancellationToken cancellationToken);
        public Task<bool> IsNameExists(string name, CancellationToken cancellationToken);
 
    }
}
