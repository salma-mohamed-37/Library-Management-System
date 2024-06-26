﻿using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.Interfaces
{
    public interface IBorrowedRepository : IAsyncRepository<Borrowed>
    {
        public Task Return(int bookId, string userId);
        public Task<PaginationDto<Borrowed>> GetCurrentlyBorrowedBooksByUser(string UserId, int pageSize, int pageNumber);
        public Task<PaginationDto<Borrowed>> GetUserBorrowHistory(string userId, int pageSize, int pageNumber);
        public Task<PaginationDto<Borrowed>> GetBookBorrowHistory(int bookId, int pageSize, int pageNumber);
    }
}
