using backend.Data;
using backend.Dtos.Account;
using backend.Dtos.GetDtos;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PaginationDto<ApplicationUser>> GetAllReaders(int pageSize, int pageNumber)
        {
            var query = _context.Users.AsNoTracking()
                .Where(u => u.Type == StaticUserRoles.USER.ToString());

            var data = await query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize).ToListAsync();

            var res = new PaginationDto<ApplicationUser>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;
        }

        public async Task<PaginationDto<ApplicationUser>> GetAllLibrarians(int pageSize, int pageNumber)
        {
            var query = _context.Users.AsNoTracking()
               .Where(u => u.Type == StaticUserRoles.lIBRARIAN.ToString());

            var data = await query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize).ToListAsync();

            var res = new PaginationDto<ApplicationUser>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;
        }

        public async Task<PaginationDto<ApplicationUser>> SearchForaUser(string? name, int pageSize, int pageNumber)
        {
            var query = _context.Users
               .AsNoTracking()
               .Where(u => u.Type == StaticUserRoles.USER.ToString())
               .Where(u => u.FullName.Contains(name))
               .OrderBy(u => u.UserName);


            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<ApplicationUser>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;
        }

        public async Task<string> GetUserRole(string userId)
        {
            var role = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => u.Type)
                    .FirstOrDefaultAsync();
            return role!;

        }

        public async Task<bool> IsExists(string id, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(x=>x.Id ==id && x.IsDeleted==false, cancellationToken);
        }

        public async Task<bool> IsEmailExists(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(x => x.Email == email && x.IsDeleted == false, cancellationToken);
        }

        public async Task<bool> IsUsernameExists(string username, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username && x.IsDeleted == false, cancellationToken);
        }
    }
}
