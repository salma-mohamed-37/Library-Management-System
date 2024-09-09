using AutoMapper;
using backend.Data;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos.Book;
using backend.Dtos.Responses;
using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
        private readonly IValidator<AddBorrowDto> _validator;

        public BorrowsController(UserManager<ApplicationUser> userManager, IMapper mapper, IBorrowedRepository borrowedRepository, IValidator<AddBorrowDto> validator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _borrowedRepository = borrowedRepository;
            _validator = validator;
        }
        [HttpPost("borrow")]
        [Authorize(Roles = "lIBRARIAN")]
        public async Task<IActionResult> BorrowBook([FromBody] AddBorrowDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var fluentErrors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = new APIResponse<object>(400, string.Join("\n", fluentErrors), null);
                return BadRequest(response);
            }
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null)
            {
                return NotFound(new APIResponse<object>(404, "This user doesn't exist.", null));
            }

            foreach (var i in dto.BooksIds)
            {
                if(await _borrowedRepository.IsBorrowed(i))
                {
                    return BadRequest(new APIResponse<object>(400, "This book "+i+" is already borrowed.", null));
                }
                var borrow = new Borrowed
                {
                    UserId = dto.UserId,
                    BookId = i,
                    currently_borrowed =true,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    ReturnDate = new DateTime(9999, 1, 1)
                };
                await _borrowedRepository.AddAsync(borrow);
            }
        
            return Ok(new APIResponse<object>(200, "Books borrowed successfully.", null));
        }

        [HttpPut("return")]
        [Authorize(Roles = "lIBRARIAN")]
        public async Task<IActionResult>ReturnBook([FromBody] AddBorrowDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var fluentErrors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = new APIResponse<object>(400, string.Join("\n", fluentErrors), null);
                return BadRequest(response);
            }

            var user = await _userManager.FindByIdAsync(dto.UserId);

            foreach (var i in dto.BooksIds)
            {
                if (! await _borrowedRepository.IsBorrowed(i))
                {
                    return BadRequest(new APIResponse<object>(400, "This book "+i+" is not borrowed.", null));

                }
            }
                await _borrowedRepository.Return(dto.BooksIds, dto.UserId);

            return Ok(new APIResponse<object>(200, "Book returned successfully.", null));
        }

        
    }
}
