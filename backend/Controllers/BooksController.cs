using AutoMapper;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos;
using backend.Dtos.GetDtos.Book;
using backend.Handlers;
using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
        private readonly IBorrowedRepository _borrowedRepository;
        public BooksController(IBookRepository bookRepository, IMapper mapper, IValidator<AddBookDto> validator, ImageHandler handler, IBorrowedRepository borrowedRepository)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _validator = validator;
            _imageHandler = handler;
            _borrowedRepository = borrowedRepository;
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationDto<GetBookDto>>> GetBooksForUsers([FromRoute] int pageSize = 4, [FromRoute] int pageNumber = 1)
        {
            var books = await _bookRepository.GetAllAsync(pageSize, pageNumber);

            var bookDtos = _mapper.Map<PaginationDto<GetBookDto>>(books);
            return Ok(bookDtos);
        }

        [Authorize(Roles ="lIBRARIAN")]
        [HttpGet("librarian/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationDto<GetBookForLibrarianDto>>> GetBooksForLibrarian([FromRoute] int pageNumber = 1, [FromRoute] int pageSize =4 )
        {
            var books = await _bookRepository.GetAllForLibrarianAsync(pageSize, pageNumber);

            var bookDtos = _mapper.Map<PaginationDto<GetBookForLibrarianDto>>(books);
            return Ok(bookDtos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GetBookDto>> GetBook(int id)
        {
            var book = await _bookRepository.GetbyIdAsync(id);
            if (book is null)
                return NotFound();
            var bookDto = _mapper.Map<GetBookDto>(book);
            return bookDto;
        }

        [HttpGet("search/{name}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationDto<GetBookDto>>> GetBooksbyName([FromRoute] string name, [FromRoute] int pageNumber = 1, [FromRoute ]int pageSize=4)
        {
            var books = await _bookRepository.GetBooksbyName(name, pageNumber, pageSize);
            var bookDtos = _mapper.Map<PaginationDto<GetBookDto>>(books);
            return Ok(bookDtos);
        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpGet("librarian/search/{name}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationDto<GetBookForLibrarianDto>>> GetBooksByNameForLibrarian([FromRoute] string name, [FromRoute]int pageNumber=1, [FromRoute]int pageSize=4)
        {
            var books = await _bookRepository.GetBooksbyNameForLibrarian(name, pageNumber, pageSize);
            var bookDtos = _mapper.Map<PaginationDto<GetBookForLibrarianDto>>(books);
            return Ok(bookDtos);
        }

        // PUT: api/books/5
        [Authorize(Roles = "lIBRARIAN")]
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

        // POST: api/books

        [HttpPost]
        [Authorize(Roles = "lIBRARIAN")]
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
        [Authorize(Roles = "lIBRARIAN")]
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

        [Authorize(Roles = "lIBRARIAN")]
        [HttpGet("borrow-history/{bookId}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<GetBorrowedBookForUserDto>> GetBorrowHistoryForBookByLibrarian([FromRoute] int bookId, [FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var res = await _borrowedRepository.GetBookBorrowHistory(bookId, pageSize, pageNumber);
            var bookDtos = _mapper.Map<PaginationDto<GetBorrowerDto>>(res);
            return Ok(bookDtos);
        }
    }
}

