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
                .Where(u =>u.Type == StaticUserRoles.USER.ToString());

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
               .Where(u => u.Type == StaticUserRoles.USER)
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

    }
}
