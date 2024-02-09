using AutoMapper;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos;
using backend.Handlers;
using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AddBookDto> _validator;
        private readonly ImageHandler _imageHandler;
        public BooksController(IBookRepository bookRepository, IMapper mapper, IValidator<AddBookDto> validator, ImageHandler handler)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _validator = validator;
            _imageHandler = handler;
        }

        //GET /api/books?pageSize=10&pageNumber=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetBookDto>>> GetBooks([FromQuery] int pageSize = 4, [FromQuery] int pageNumber = 1)
        {
            var books = await _bookRepository.GetAllAsync(pageSize, pageNumber);
            var bookDtos = _mapper.Map<IEnumerable<GetBookDto>>(books);
            return Ok(bookDtos);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetBookDto>> GetBook(int id)
        {
            var book = await _bookRepository.GetbyIdAsync(id);
            if (book is null)
                return NotFound();

            var bookDto = _mapper.Map<GetBookDto>(book);
            return bookDto;
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<GetBookDto>> GetBooks(string name)
        {
            var books = await _bookRepository.GetBooksbyName(name);
            var bookDtos = _mapper.Map<IEnumerable<GetBookDto>>(books);
            return Ok(bookDtos);

        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] AddBookDto bookDto)
        {
            var validationResult = await _validator.ValidateAsync(bookDto);
            if (validationResult.Errors.Any())
            {
                return BadRequest(validationResult);
            }
            var existingBook = await _bookRepository.GetbyIdAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            if (bookDto.CoverFile == null)
            {
                existingBook.Name = bookDto.Name;
                existingBook.PublishDate = bookDto.PublishDate;
                existingBook.AuthorId = bookDto.AuthorId;
                existingBook.CategoryId = bookDto.CategoryId;
            }
           
            else
            {
                _imageHandler.DeleteImage(existingBook.CoverName, "Books");
                // Map the data from the DTO to the existing category entity
                _mapper.Map(bookDto, existingBook);
                await _imageHandler.SaveImageFile(bookDto.CoverFile, existingBook.CoverName, "Books");
            }

            await _bookRepository.UpdateAsync(existingBook);
            return NoContent();
        }

        // POST: api/Books

        [HttpPost]
        public async Task<ActionResult> AddBook([FromForm] AddBookDto bookDto)
        {
            var validationResult = await _validator.ValidateAsync(bookDto);
            if (validationResult.Errors.Any())
            {
                return BadRequest(validationResult);
            }
            var book = _mapper.Map<Book>(bookDto);
            await _imageHandler.SaveImageFile(bookDto.CoverFile,book.CoverName, "Books");
            await _bookRepository.AddAsync(book);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var existingBook = await _bookRepository.GetbyIdAsync(id);

            if (existingBook == null)
            {
                return NotFound();
            }
            _imageHandler.DeleteImage(existingBook.CoverName,"Books");
            await _bookRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

