using AutoMapper;
using backend.Data;
using backend.Dtos.AddDtos;
using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBorrowedRepository _borrowedRepository;

        public BorrowsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, IBorrowedRepository borrowedRepository)
        {
            _context =context;
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
                return BadRequest("User doesn't exist");
            }

            var borrow = _mapper.Map<Borrowed>(dto);

            await _borrowedRepository.AddAsync(borrow);
            return Ok("Book borrowed successfully");
        }

        [HttpPut("return")]
        [Authorize(Roles = "lIBRARIAN")]
        public async Task<IActionResult>ReturnBook([FromBody] AddBorrowDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null)
            {
                return BadRequest("User doesn't exist");
            }
            await _borrowedRepository.Return(dto.BookId, dto.UserId);

            return Ok("Book returned successfully");
        }
    }
}
