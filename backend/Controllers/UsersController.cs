using AutoMapper;
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

        [HttpGet("librarian/current-borrow")]
        [Authorize(Roles = "lIBRARIAN")]
        public async Task<IActionResult> GetCurrentlyBorrowedBooksByUser([FromQuery] string userId)
        {
            var res = await _borrowedRepository.GetCurrentlyBorrowedBooksByUser(userId);
            var bookDtos = _mapper.Map<IEnumerable<GetBorrowedBookForUserDto>>(res);
            return Ok(bookDtos);
        }

        [HttpGet("current-borrow")]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> GetCurrentlyBorrowedBooksByUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _borrowedRepository.GetCurrentlyBorrowedBooksByUser(userId);
            var bookDtos = _mapper.Map<IEnumerable<GetBorrowedBookForUserDto>>(res);
            return Ok(bookDtos);
        }


        [Authorize(Roles ="lIBRARIAN")]
        [HttpGet("borrow-history/{userId}")]
        public async Task<ActionResult<GetBorrowedBookForUserDto>> GetBorrowHistoryForUserByLibrarian([FromRoute] string userId)
        {
            var res = await _borrowedRepository.GetUserBorrowHistory(userId);
            var bookDtos = _mapper.Map<IEnumerable<GetBorrowedBookForUserDto>>(res);
            return Ok(bookDtos);
        }

        [Authorize(Roles ="USER")]
        [HttpGet("borrow-history")]
        public async Task<ActionResult<GetBorrowedBookForUserDto>> GetBorrowHistoryForUserByuser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _borrowedRepository.GetUserBorrowHistory(userId!);
            var bookDtos = _mapper.Map<IEnumerable<GetBorrowedBookForUserDto>>(res);
            return Ok(bookDtos);
        }
    }
}
