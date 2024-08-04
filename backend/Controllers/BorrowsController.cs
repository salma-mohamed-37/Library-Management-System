using AutoMapper;
using backend.Data;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos.Book;
using backend.Dtos.Responses;
using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using static System.Reflection.Metadata.BlobBuilder;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBorrowedRepository _borrowedRepository;

        public BorrowsController(UserManager<ApplicationUser> userManager, IMapper mapper, IBorrowedRepository borrowedRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _borrowedRepository = borrowedRepository;
        }
        [HttpPost("borrow")]
        [Authorize(Roles = "lIBRARIAN")]
        public async Task<IActionResult> BorrowBook([FromBody] AddBorrowDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null)
            {
                return NotFound(new APIResponse<object>(404, "This book doesn't exist.", null));
            }

            var borrow = _mapper.Map<Borrowed>(dto);

            await _borrowedRepository.AddAsync(borrow);
            return Ok(new APIResponse<object>(200, "Book borrowed successfully.", null));
        }

        [HttpPut("return")]
        [Authorize(Roles = "lIBRARIAN")]
        public async Task<IActionResult>ReturnBook([FromBody] AddBorrowDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null)
            {
                return NotFound(new APIResponse<object>(404, "This book doesn't exist.", null));
            }
            await _borrowedRepository.Return(dto.BookId, dto.UserId);

            return Ok(new APIResponse<object>(200, "Book returned successfully.", null));
        }

        
    }
}
