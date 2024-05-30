using backend.Data;
using backend.Dtos.AddDtos;
using backend.Models;
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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;  

        public BorrowsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context =context;
            _userManager = userManager;
        }
        [HttpPost("borrow")]
        [Authorize(Roles = "lIBRARIAN")]
        public async Task<IActionResult> BorrowBook([FromBody] AddBorrowDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.UserEmail);
            if (user is null)
            {
                return BadRequest("User doesn't exist");
            }

            var borrow = new Borrowed
            {
                UserId = user.Id,
                BookId = dto.BookId,
                BorrowDate = dto.BorrowDate,
                ReturnDate = dto.ReturnDate,
                currently_borrowed = true
            };
            await _context.Borrowed.AddAsync(borrow);
            await _context.SaveChangesAsync();
            return Ok("Book borrowed successfully");
        }

        [HttpPut("return")]
        public async Task<IActionResult>ReturnBook([FromBody] AddReturnDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.UserEmail);
            if (user is null)
            {
                return BadRequest("User doesn't exist");
            }
            var b = await _context.Borrowed.FirstOrDefaultAsync(b => b.UserId == user.Id && b.BookId == dto.BookId);
            b.currently_borrowed = false;
            await _context.SaveChangesAsync();

            return Ok("Book returned successfully");
        }
    }
}
