using backend.Data;
using backend.Dtos.GetDtos;
using backend.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Threading;

namespace backend.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginationDto<T>> GetAllAsync(int pageSize, int pageNumber)
        {
            var query =  _context.Set<T>().AsNoTracking();

            var data = await query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize).ToListAsync();

            var res = new PaginationDto<T>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;
        }

        async Task<T> IAsyncRepository<T>.AddAsync(T t)
        {
            await _context.Set<T>().AddAsync(t);
            await _context.SaveChangesAsync();
            return t;
        }

        async Task<bool> IAsyncRepository<T>.DeleteAsync(int id)
        {
            var res = await _context.Set<T>().FindAsync(id);
            if (res is not null)
            {
                _context.Set<T>().Remove(res);
                await _context.SaveChangesAsync();
                return true;
            }
                
            else
            {
                return false;
            }
        }

        async Task<T> IAsyncRepository<T>.GetbyIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        async Task IAsyncRepository<T>.UpdateAsync(T t)
        {
            _context.Entry(t).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
