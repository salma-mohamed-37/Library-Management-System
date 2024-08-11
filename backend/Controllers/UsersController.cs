using AutoMapper;
using backend.Dtos.Account;
using backend.Dtos.GetDtos;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos.Book;
using backend.Dtos.Responses;
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

        //[HttpGet("librarian/current-borrow/{userId}")]
        //[Authorize(Roles = "lIBRARIAN")]
        //public async Task<ActionResult<APIResponse<ICollection<GetBorrowedBookForUserDto>>>> GetCurrentlyBorrowedBooksByUser([FromRoute] string userId)
        //{
        //    var res = await _borrowedRepository.GetCurrentlyBorrowedBooksByUser(userId);
        //    var bookDtos = _mapper.Map<ICollection<GetBorrowedBookForUserDto>>(res);
        //    return Ok(new APIResponse<ICollection<GetBorrowedBookForUserDto>>(200, "", bookDtos));
        //}

        //[HttpGet("current-borrow")]
        //[Authorize(Roles = "USER")]
        //public async Task<ActionResult<APIResponse<ICollection<GetBorrowedBookForUserDto>>>> GetCurrentlyBorrowedBooksByUser()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var res = await _borrowedRepository.GetCurrentlyBorrowedBooksByUser(userId);
        //    var bookDtos = _mapper.Map<ICollection<GetBorrowedBookForUserDto>>(res);
        //    return Ok(new APIResponse<ICollection<GetBorrowedBookForUserDto>>(200, "", bookDtos));
        //}

        [Authorize()]
        [HttpGet("current-history")]
        public async Task<ActionResult<APIResponse<ICollection<GetBorrowedBookForUserDto>>>> GetCurrentlyBorrowedBooksByUser([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4, [FromQuery] string userId = null)
        {
            if (userId == null)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = await _userRepository.GetUserRole(userId);
                if (userRole != StaticUserRoles.USER.ToString())
                {
                    return BadRequest(new APIResponse<object>(400, "You don't have borrow history.", null));
                }
            }
            else
            {
                var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var operatorRole = await _userRepository.GetUserRole(operatorId);
                var userRole = await _userRepository.GetUserRole(userId);

                if (userRole != StaticUserRoles.USER.ToString() || operatorRole != StaticUserRoles.lIBRARIAN.ToString())
                    return BadRequest(new APIResponse<object>(400, "Only Librarians can view borrowed history of users.", null));
            }

            var res = await _borrowedRepository.GetCurrentlyBorrowedBooksByUser(userId);
            var bookDtos = _mapper.Map<ICollection<GetBorrowedBookForUserDto>>(res);
            return Ok(new APIResponse<ICollection<GetBorrowedBookForUserDto>>(200, "", bookDtos));

        }


        //[Authorize(Roles ="lIBRARIAN")]
        //[HttpGet("borrow-history/{userId}/{pageNumber}/{pageSize}")]
        //public async Task<ActionResult<APIResponse<PaginationDto<GetBorrowedBookForUserDto>>>> GetBorrowHistoryForUserByLibrarian([FromRoute] string userId, [FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        //{
        //    var res = await _borrowedRepository.GetUserBorrowHistory(userId, pageSize, pageNumber);
        //    var bookDtos = _mapper.Map<PaginationDto<GetBorrowedBookForUserDto>>(res);
        //    return Ok(new APIResponse<PaginationDto<GetBorrowedBookForUserDto>>(200, "", bookDtos));
        //}

        //[Authorize(Roles ="USER")]
        //[HttpGet("borrow-history/{pageNumber}/{pageSize}")]
        //public async Task<ActionResult<APIResponse<PaginationDto<GetBorrowedBookForUserDto>>>> GetBorrowHistoryForUserByuser([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var res = await _borrowedRepository.GetUserBorrowHistory(userId!,pageSize, pageNumber);
        //    var bookDtos = _mapper.Map<PaginationDto<GetBorrowedBookForUserDto>>(res);
        //    return Ok(new APIResponse<PaginationDto<GetBorrowedBookForUserDto>>(200, "", bookDtos));
        //}

        [Authorize()]
        [HttpGet("borrow-history/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<APIResponse<PaginationDto<GetBorrowedBookForUserDto>>>> GetBorrowHistoryForUser([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4, [FromQuery]string userId=null)
        {
            if (userId ==null)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = await _userRepository.GetUserRole(userId);
                if (userRole != StaticUserRoles.USER.ToString())
                {
                    return BadRequest(new APIResponse<object>(400, "You don't have borrow history.", null));
                }
            }
            else
            {
                var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var operatorRole = await _userRepository.GetUserRole(operatorId);
                var userRole = await _userRepository.GetUserRole(userId);

                if(userRole != StaticUserRoles.USER.ToString() || operatorRole!= StaticUserRoles.lIBRARIAN.ToString())
                    return BadRequest(new APIResponse<object>(400, "Only Librarians can view borrowed books by users.", null));
            }

            var res = await _borrowedRepository.GetUserBorrowHistory(userId!, pageSize, pageNumber);
            var bookDtos = _mapper.Map<PaginationDto<GetBorrowedBookForUserDto>>(res);
            return Ok(new APIResponse<PaginationDto<GetBorrowedBookForUserDto>>(200, "", bookDtos));
        }

        [Authorize(Roles ="lIBRARIAN")]
        [HttpGet("reader/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<APIResponse<PaginationDto<UserDto>>>> GetUsers([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var res = await _userRepository.GetAllReaders(pageSize, pageNumber);
            var userDtos = _mapper.Map<PaginationDto<UserDto>>(res);
            return Ok(new APIResponse<PaginationDto<UserDto>>(200, "", userDtos));
        }

        [Authorize(Roles ="ADMIN")]
        [HttpGet("librarian/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<APIResponse<PaginationDto<UserDto>>>> GetLibrarians([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var res = await _userRepository.GetAllLibrarians(pageSize, pageNumber);
            var userDtos = _mapper.Map<PaginationDto<UserDto>>(res);
            return Ok(new APIResponse<PaginationDto<UserDto>>(200, "", userDtos));
        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpPost("search/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<APIResponse<PaginationDto<UserDto>>>> SearchForUsersByUsername([FromBody] SearchByNameDto request, [FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var res = await _userRepository.SearchForaUser(request.Name, pageSize, pageNumber);
            var userDtos = _mapper.Map<PaginationDto<UserDto>>(res);
            return Ok(new APIResponse<PaginationDto<UserDto>>(200, "", userDtos));
        }
    }
}
