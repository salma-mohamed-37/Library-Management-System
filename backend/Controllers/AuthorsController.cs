using AutoMapper;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos;
using backend.Dtos.Responses;
using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AddAuthorDto> _validator;
        public AuthorsController(IAuthorRepository authorRepository, IMapper mapper, IValidator<AddAuthorDto> validator)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<ActionResult<APIResponse<PaginationDto<GetAuthorDto>>>> GetAuthors( [FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var authors = await _authorRepository.GetAllAsync(pageSize, pageNumber);
            var authorDtos = _mapper.Map<PaginationDto<GetAuthorDto>>(authors);
            return Ok(new APIResponse<PaginationDto<GetAuthorDto>> (200,"",authorDtos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<GetAuthorDto>>> GetAuthor(int id)
        {
            var author = await _authorRepository.GetbyIdAsync(id);
            if (author is null)
                return NotFound(new APIResponse<PaginationDto<GetAuthorDto>>(404, "Author doesn't exist", null));

            var authorDto = _mapper.Map<GetAuthorDto>(author);
            return new APIResponse<GetAuthorDto>(200, "", authorDto);
        }

        [HttpGet("search/{name}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<APIResponse<PaginationDto<GetAuthorDto>>>> GetAuthors(string name, int pageNumber =1, int pageSize=4)
        {
            var authors = await _authorRepository.GetAuthorsbyName(name, pageNumber, pageSize);
            var authorDtos = _mapper.Map< PaginationDto<GetAuthorDto>>(authors);
            return Ok(new APIResponse<PaginationDto<GetAuthorDto>>(200, "", authorDtos));
        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AddAuthorDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .Where(e => e.Value!.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                var response = new APIResponse<object>(400, string.Join("\n", errors), null);
                return BadRequest(response);
            }

            var existingAuthor = await _authorRepository.GetbyIdAsync(id);
            if (existingAuthor == null)
            {
                return NotFound(new APIResponse<object>(404,"This author doesn't exist." , null));
            }

            
            _mapper.Map(authorDto, existingAuthor);

            await _authorRepository.UpdateAsync(existingAuthor);
            return Ok(new APIResponse<object> (200,"The author updated successfully.", null));
        }


        [Authorize(Roles = "lIBRARIAN")]
        [HttpPost]
        public async Task<ActionResult> AddAuthor([FromBody] AddAuthorDto authorDto)
        {
            var validationResult = await _validator.ValidateAsync(authorDto);
            if (validationResult.Errors.Any())
            {
                return BadRequest(validationResult);
            }
            var author = _mapper.Map<Author>(authorDto);
            await _authorRepository.AddAsync(author);
            return Ok(new APIResponse<object>(200, "The author added successfully.", null));
        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorRepository.DeleteAsync(id);

            return Ok(new APIResponse<object>(200, "The author deleted successfully.", null));
        }

        [HttpGet("names")]
        public async Task<ActionResult<APIResponse<ICollection<string>>>> GetAuthorsNames()
        {
            var res = await _authorRepository.GetAuthorsNames();
            return Ok(new APIResponse<ICollection<string>>(200, "", res));
        }

        [HttpGet("all")]
        public async Task<ActionResult<APIResponse<ICollection<GetAuthorDto>>>> GetAll()
        {
            var authors = await _authorRepository.getAllAsync();
            var res = _mapper.Map<ICollection<GetAuthorDto>>(authors);
            return Ok(new APIResponse<ICollection<GetAuthorDto>>(200, "", res));
        }
    }
}
