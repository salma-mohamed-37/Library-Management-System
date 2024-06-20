using AutoMapper;
using backend.Dtos.GetDtos;
using backend.Dtos.GetDtos.Book;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBorrowedRepository _borrowedRepository;

        public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper, IBorrowedRepository borrowedRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _borrowedRepository = borrowedRepository;
        }

        [HttpGet("librarian/current-borrow/{userId}/{pageNumber}/{pageSize}")]
        [Authorize(Roles = "lIBRARIAN")]
        public async Task<ActionResult<PaginationDto<GetBorrowedBookForUserDto>>> GetCurrentlyBorrowedBooksByUser([FromRoute] string userId, [FromRoute]int pageNumber=1, [FromRoute] int pageSize =4)
        {
            var res = await _borrowedRepository.GetCurrentlyBorrowedBooksByUser(userId, pageSize, pageNumber);
            var bookDtos = _mapper.Map<PaginationDto<GetBorrowedBookForUserDto>>(res);
            return Ok(bookDtos);
        }

        [HttpGet("current-borrow/{pageNumber}/{pageSize}")]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult<PaginationDto<GetBorrowedBookForUserDto>>> GetCurrentlyBorrowedBooksByUser([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _borrowedRepository.GetCurrentlyBorrowedBooksByUser(userId, pageSize, pageNumber);
            var bookDtos = _mapper.Map<PaginationDto<GetBorrowedBookForUserDto>>(res);
            return Ok(bookDtos);
        }


        [Authorize(Roles ="lIBRARIAN")]
        [HttpGet("borrow-history/{userId}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationDto<GetBorrowedBookForUserDto>>> GetBorrowHistoryForUserByLibrarian([FromRoute] string userId, [FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var res = await _borrowedRepository.GetUserBorrowHistory(userId, pageSize, pageNumber);
            var bookDtos = _mapper.Map<PaginationDto<GetBorrowedBookForUserDto>>(res);
            return Ok(bookDtos);
        }

        [Authorize(Roles ="USER")]
        [HttpGet("borrow-history/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationDto<GetBorrowedBookForUserDto>>> GetBorrowHistoryForUserByuser([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _borrowedRepository.GetUserBorrowHistory(userId!,pageSize, pageNumber);
            var bookDtos = _mapper.Map<PaginationDto<GetBorrowedBookForUserDto>>(res);
            return Ok(bookDtos);
        }
    }
}
