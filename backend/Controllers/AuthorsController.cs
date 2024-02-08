using AutoMapper;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos;
using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        //GET /api/Authors?pageSize=10&pageNumber=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAuthorDto>>> GetAuthors([FromQuery] int pageSize = 4, [FromQuery] int pageNumber = 1)
        {
            var authors = await _authorRepository.GetAllAsync(pageSize, pageNumber);
            var authorDtos = _mapper.Map<IEnumerable<GetAuthorDto>>(authors);
            return Ok(authorDtos);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAuthorDto>> GetAuthor(int id)
        {
            var author = await _authorRepository.GetbyIdAsync(id);
            if (author is null)
                return NotFound();

            var authorDto = _mapper.Map<GetAuthorDto>(author);
            return authorDto;
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<GetAuthorDto>> GetAuthors(string name)
        {
            var authors = await _authorRepository.GetAuthorsbyName(name);
            var authorDtos = _mapper.Map<IEnumerable<GetAuthorDto>>(authors);
            return Ok(authorDtos);

        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AddAuthorDto authorDto)
        {
            var validationResult = await _validator.ValidateAsync(authorDto);
            if (validationResult.Errors.Any())
            {
                return BadRequest(validationResult);
            }
            var existingAuthor = await _authorRepository.GetbyIdAsync(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            // Map the data from the DTO to the existing category entity
            _mapper.Map(authorDto, existingAuthor);

            await _authorRepository.UpdateAsync(existingAuthor);
            return NoContent();
        }

        // POST: api/Categories

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
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
