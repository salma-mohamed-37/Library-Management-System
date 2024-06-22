using AutoMapper;
using backend.Dtos.Account;
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
        private readonly IUserRepository _userRepository;

        public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper, IBorrowedRepository borrowedRepository, IUserRepository userRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _borrowedRepository = borrowedRepository;
            _userRepository = userRepository;
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

        [Authorize(Roles ="lIBRARIAN")]
        [HttpGet("reader/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationDto<UserDto>>> GetUsers([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var res = await _userRepository.GetAllReaders(pageSize, pageNumber);
            var userDtos = _mapper.Map<PaginationDto<UserDto>>(res);
            return userDtos;
        }

        [Authorize(Roles ="ADMIN")]
        [HttpGet("librarian/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationDto<UserDto>>> GetLibrarians([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var res = await _userRepository.GetAllLibrarians(pageSize, pageNumber);
            var userDtos = _mapper.Map<PaginationDto<UserDto>>(res);
            return userDtos;
        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpGet("search/{username}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationDto<UserDto>>> SearchForUsersByUsername([FromRoute] string username, [FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var res = await _userRepository.SearchForaUser(username, pageSize, pageNumber);
            var userDtos = _mapper.Map<PaginationDto<UserDto>>(res);
            return userDtos;
        }
    }
}
