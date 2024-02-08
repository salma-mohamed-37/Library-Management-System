using backend.Data;
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

        async Task<List<T>> IAsyncRepository<T>.GetAllAsync(int pageSize, int pageNumber)
        {
            var res = await _context.Set<T>().AsNoTracking().Skip(pageSize*(pageNumber-1)).Take(pageSize).ToListAsync();
            return res;
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
