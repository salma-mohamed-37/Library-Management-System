using backend.Dtos.Account;
using backend.Dtos.GetDtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Interfaces
{
    public interface IUserRepository: IAsyncRepository<ApplicationUser>
    {
        public Task<PaginationDto<ApplicationUser>> GetAllReaders(int pageSize, int pageNumber);
        public Task<PaginationDto<ApplicationUser>> GetAllLibrarians(int pageSize, int pageNumber);
        public Task<PaginationDto<ApplicationUser>> SearchForaUser(string? name, int pageSize, int pageNumber);
        public Task<string> GetUserRole(string userId);

        public Task<bool> IsExists(string id, CancellationToken cancellationToken);

        public Task<bool> IsEmailExists(string email, CancellationToken cancellationToken);

        public Task<bool> IsUsernameExists(string username, CancellationToken cancellationToken);
    }
}
