using backend.Dtos.Account;
using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.Interfaces
{
    public interface IUserRepository: IAsyncRepository<ApplicationUser>
    {
        public Task<PaginationDto<ApplicationUser>> GetAllReaders(int pageSize, int pageNumber);
        public Task<PaginationDto<ApplicationUser>> GetAllLibrarians(int pageSize, int pageNumber);
        public Task<PaginationDto<ApplicationUser>> SearchForaUser(string? name, int pageSize, int pageNumber);
    }
}
