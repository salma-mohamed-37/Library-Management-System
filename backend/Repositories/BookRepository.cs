using backend.Data;
using backend.Dtos.GetDtos;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Drawing.Printing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace backend.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        async public Task<PaginationDto<Book>> GetBooksbyName(string name, int pageNumber, int pageSize)
        {
            var query = _context.Books
                .AsNoTracking()
                .Where(x=>x.IsDeleted==false)
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .Where(c => c.Name.Contains(name))
                .OrderBy(b => b.Name);


            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Book>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;
        }

        async public Task<PaginationDto<Book>> GetBooksbyNameForLibrarian(string? name, int pageNumber, int pageSize)
        {
            var query = _context.Books
              .AsNoTracking()
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .ThenInclude(borrow => borrow.User)
                .Where(b => b.Name.Contains(name))
                .OrderBy(b => b.Name);

            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Book>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;

        }

        public async Task<PaginationDto<Book>> GetAllAsync(int pageSize, int pageNumber)
        {
            var query = _context.Books
                .AsNoTracking()
                 .Where(x => x.IsDeleted == false)
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .OrderBy(b => b.Name);

            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Book>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;

        }

        public async Task<PaginationDto<Book>> GetAllForLibrarianAsync(int pageSize, int pageNumber)
        {
            var query = _context.Books
                .AsNoTracking()

                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .ThenInclude(borrow => borrow.User)
                .Where(b=> b.Borrowed.All(b=>b.User.IsDeleted ==false))
                 .OrderBy(b => b.Name);

            var data = await query
              .Skip(pageSize * (pageNumber - 1))
              .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Book>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };
            return res;

            
        }
        public async Task<Book> GetbyIdAsync(int id)
        {
            return await _context.Books
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .FirstOrDefaultAsync(book => book.Id == id);
        }

        public async Task<PaginationDto<Book>> GetBooksbyCategory(int categoryId, int pageNumber, int pageSize)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(book => book.Category)
                .Where(b=>b.CategoryId == categoryId)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .OrderBy(b => b.Name);


            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Book>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;
        }

        public async Task<PaginationDto<Book>> GetBooksbyAuthor(int authorId, int pageNumber, int pageSize)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(book => book.Author)
                .Where(b => b.AuthorId == authorId)
                .Include(book => book.Category)
                .Include(book => book.Borrowed)
                .OrderBy(b => b.Name);


            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Book>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;
        }

        public async Task<PaginationDto<Book>> GetFilteredBooks(FilteringRequest request)
        {
            var query = _context.Books
                .AsNoTracking()
                 .Where(x => x.IsDeleted == false)
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .AsQueryable();             

            // //filtering
            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(b=>b.Name.Contains(request.Name!));

            if (!string.IsNullOrEmpty(request.Category))     
                query = query.Where(b=>b.Category.Name.Equals(request.Category!));

            if (!string.IsNullOrEmpty(request.Author))
                query = query.Where(b=>b.Author.Name.Contains(request.Author!));

            if (request.FromDate.HasValue) 
                query=query.Where(b=>b.PublishDate >= request.FromDate);   

            if (request.ToDate.HasValue) 
                query=query.Where(b=>b.PublishDate <= request.ToDate);       

            //sorting
            if(request.SortDirection =="desc")
            {
                query = request.SortField switch
            {
                "name" => query.OrderByDescending(b => b.Name),
                "publish date" => query.OrderByDescending(b => b.PublishDate),
                "author" => query.OrderByDescending(b=> b.Author.Name),
                "genre"=> query.OrderByDescending(b=>b.Category.Name),
            _   => query.OrderByDescending(b => b.Name)
            };
            }
            
            else
            {
                query = request.SortField switch
                {
                    "name" => query.OrderBy(b => b.Name),
                    "publish date" => query.OrderBy(b => b.PublishDate),
                    "author" => query.OrderBy(b=> b.Author.Name),
                    "genre"=> query.OrderBy(b=>b.Category.Name),
                     _=> query.OrderBy(b => b.Name)
                };
            }

            //get right page

            var data = await query
                        .Skip(request.PageSize * (request.PageNumber - 1))
                        .Take(request.PageSize)
                        .ToListAsync();
            var res = new PaginationDto<Book>
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Count = await query.CountAsync(),
                Data = data
            };       
            return res;

        }

        public async Task<PaginationDto<Book>> GetAvailablebyNameForLibrarian(string name, int pageNumber, int pageSize)
        {
            var query = _context.Books
             .AsNoTracking()
                .Where(x => x.IsDeleted == false)
               .Include(book => book.Category)
               .Include(book => book.Author)
               .Include(book => book.Borrowed)
               .ThenInclude(borrow => borrow.User)
               .Where(b => b.Borrowed.All(b => b.User.IsDeleted == false))
               .Where(b => b.Borrowed.All(borrow => borrow.currently_borrowed == false))
               .Where(b => b.Name.Contains(name))
               .OrderBy(b => b.Name);

            var data = await query
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize).ToListAsync();

            var res = new PaginationDto<Book>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = await query.CountAsync(),
                Data = data
            };

            return res;

        }

        public async Task<bool> IsExists(int id, CancellationToken cancellationToken=default)
        {
            return await _context.Books.AnyAsync (b => b.Id == id && b.IsDeleted==false);
        }

        public async Task<bool> IsNameExists(string name, CancellationToken cancellationToken=default)
        {
            return await _context.Books.AnyAsync(b=>b.Name == name && b.IsDeleted == false, cancellationToken);
        }

        public async Task<PaginationDto<Book>> GetFilteredBooksForLibrarian(FilteringRequest request)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(book => book.Category)
                .Include(book => book.Author)
                .Include(book => book.Borrowed)
                .AsQueryable();

            // //filtering
            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(b => b.Name.Contains(request.Name!));

            if (!string.IsNullOrEmpty(request.Category))
                query = query.Where(b => b.Category.Name.Equals(request.Category!));

            if (!string.IsNullOrEmpty(request.Author))
                query = query.Where(b => b.Author.Name.Contains(request.Author!));

            if (request.FromDate.HasValue)
                query = query.Where(b => b.PublishDate >= request.FromDate);

            if (request.ToDate.HasValue)
                query = query.Where(b => b.PublishDate <= request.ToDate);

            if (request.IsDeleted.HasValue)
                query = query.Where(b => b.IsDeleted == request.IsDeleted);

            //sorting
            if (request.SortDirection == "desc")
            {
                query = request.SortField switch
                {
                    "name" => query.OrderByDescending(b => b.Name),
                    "publish date" => query.OrderByDescending(b => b.PublishDate),
                    "author" => query.OrderByDescending(b => b.Author.Name),
                    "genre" => query.OrderByDescending(b => b.Category.Name),
                    _ => query.OrderByDescending(b => b.Name)
                };
            }

            else
            {
                query = request.SortField switch
                {
                    "name" => query.OrderBy(b => b.Name),
                    "publish date" => query.OrderBy(b => b.PublishDate),
                    "author" => query.OrderBy(b => b.Author.Name),
                    "genre" => query.OrderBy(b => b.Category.Name),
                    _ => query.OrderBy(b => b.Name)
                };
            }

            //get right page

            var data = await query
                        .Skip(request.PageSize * (request.PageNumber - 1))
                        .Take(request.PageSize)
                        .ToListAsync();
            var res = new PaginationDto<Book>
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Count = await query.CountAsync(),
                Data = data
            };
            return res;
        }


    }
}
