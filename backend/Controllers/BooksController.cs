using AutoMapper;
using backend.Dtos.Account;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos;
using backend.Dtos.GetDtos.Book;
using backend.Dtos.Responses;
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
        public async Task<ActionResult<APIResponse<PaginationDto<GetBookDto>>>> GetBooksForUsers([FromRoute] int pageSize = 4, [FromRoute] int pageNumber = 1)
        {
            var books = await _bookRepository.GetAllAsync(pageSize, pageNumber);

            var bookDtos = _mapper.Map<PaginationDto<GetBookDto>>(books);
            return Ok(new APIResponse<PaginationDto<GetBookDto>>(200, "", bookDtos));
        }

        [HttpPost("Filtered")]
        public async Task<ActionResult<APIResponse<PaginationDto<GetBookDto>>>> GetFilteredBooksForUsers([FromBody] FilteringRequest request)
        {
            var books = await _bookRepository.GetFilteredBooks(request);

            var bookDtos = _mapper.Map<PaginationDto<GetBookDto>>(books);
            return Ok(new APIResponse<PaginationDto<GetBookDto>>(200, "", bookDtos));
        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpPost("librarian/search/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<APIResponse<PaginationDto<GetBookForLibrarianDto>>>> GetBooksByNameForLibrarian([FromBody] SearchByNameDto request, [FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var books = await _bookRepository.GetBooksbyNameForLibrarian(request.Name, pageNumber, pageSize);
            var bookDtos = _mapper.Map<PaginationDto<GetBookForLibrarianDto>>(books);
            return Ok(new APIResponse<PaginationDto<GetBookForLibrarianDto>>(200, "", bookDtos));
        }


        [Authorize(Roles = "lIBRARIAN")]
        [HttpPost("librarian/available/search/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<APIResponse<PaginationDto<GetBookForLibrarianDto>>>> GetAvailableBooksByNameForLibrarian([FromBody] SearchByNameDto request, [FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var books = await _bookRepository.GetAvailablebyNameForLibrarian(request.Name, pageNumber, pageSize);
            var bookDtos = _mapper.Map<PaginationDto<GetBookForLibrarianDto>>(books);
            return Ok(new APIResponse<PaginationDto<GetBookForLibrarianDto>>(200, "", bookDtos));
        }



        [Authorize(Roles ="lIBRARIAN")]
        [HttpGet("librarian/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<APIResponse<PaginationDto<GetBookForLibrarianDto>>>> GetBooksForLibrarian([FromRoute] int pageNumber = 1, [FromRoute] int pageSize =4 )
        {
            var books = await _bookRepository.GetAllForLibrarianAsync(pageSize, pageNumber);

            var bookDtos = _mapper.Map<PaginationDto<GetBookForLibrarianDto>>(books);
            return Ok(new APIResponse<PaginationDto<GetBookForLibrarianDto>>(200, "", bookDtos));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<GetBookDto>>> GetBook(int id)
        {
            var book = await _bookRepository.GetbyIdAsync(id);
            if (book is null)
                return NotFound(new APIResponse<object>(404, "This book doesn't exist.", null));
            var bookDto = _mapper.Map<GetBookDto>(book);
            return new APIResponse<GetBookDto>(200, "", bookDto);
        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] AddBookDto bookDto)
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

            var validationResult = await _validator.ValidateAsync(bookDto);
            if (!validationResult.IsValid)
            {
                var fluentErrors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = new APIResponse<object>(400, string.Join("\n", fluentErrors), null);
                return BadRequest(response);
            }

            var existingBook = await _bookRepository.GetbyIdAsync(id);
            if (existingBook == null)
            {
                return NotFound(new APIResponse<object>(404, "This book doesn't exist.", null));
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
            return Ok(new APIResponse<object>(200, "The book is updated successfully.", null));
        }

  

        [HttpPost]
        [Authorize(Roles = "lIBRARIAN")]
        public async Task<IActionResult> AddBook([FromForm] AddBookDto bookDto)
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

            var validationResult = await _validator.ValidateAsync(bookDto);
            if (!validationResult.IsValid)
            {
                var fluentErrors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = new APIResponse<object>(400, string.Join("\n", fluentErrors), null);
                return BadRequest(response);
            }

            var book = _mapper.Map<Book>(bookDto);
            await _imageHandler.SaveImageFile(bookDto.CoverFile,book.CoverName, "Books");
            await _bookRepository.AddAsync(book);
            return Ok(new APIResponse<object>(200, "The book is added successfully.", null));
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "lIBRARIAN")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var existingBook = await _bookRepository.GetbyIdAsync(id);

            if (existingBook == null)
            {
                return NotFound(new APIResponse<object>(404, "This book doesn't exist.", null));
            }
            _imageHandler.DeleteImage(existingBook.CoverName,"Books");
            await _bookRepository.DeleteAsync(id);
            return Ok(new APIResponse<object>(200, "The book is deleted successfully.", null));
        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpGet("borrow-history/{bookId}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<APIResponse<PaginationDto<GetBorrowerDto>>>> GetBorrowHistoryForBookByLibrarian([FromRoute] int bookId, [FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        {
            var res = await _borrowedRepository.GetBookBorrowHistory(bookId, pageSize, pageNumber);
            var bookDtos = _mapper.Map<PaginationDto<GetBorrowerDto>>(res);
            return Ok(new APIResponse<PaginationDto<GetBorrowerDto>>(200, "", bookDtos));
        }

        //[HttpGet("category/{categoryId}/{pageNumber}/{pageSize}")]
        //public async Task<ActionResult<PaginationDto<GetBookDto>>> GetBooksbyCategoryId([FromRoute] int categoryId, [FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        //{
        //    var books = await _bookRepository.GetBooksbyCategory(categoryId, pageNumber, pageSize);
        //    var bookDtos = _mapper.Map<PaginationDto<GetBookDto>>(books);
        //    return Ok(bookDtos);
        //}

        //[HttpGet("author/{authorId}/{pageNumber}/{pageSize}")]
        //public async Task<ActionResult<PaginationDto<GetBookDto>>> GetBooksbyAuthorId([FromRoute] int authorId, [FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 4)
        //{
        //    var books = await _bookRepository.GetBooksbyAuthor(authorId, pageNumber, pageSize);
        //    var bookDtos = _mapper.Map<PaginationDto<GetBookDto>>(books);
        //    return Ok(bookDtos);
        //}



        [HttpPost("librarian/Filtered")]
        public async Task<ActionResult<APIResponse<PaginationDto<GetBookForLibrarianDto>>>> GetFilteredBooksForLibrarian([FromBody] FilteringRequest request)
        {
            var books = await _bookRepository.GetFilteredBooksForLibrarian(request);

            var bookDtos = _mapper.Map<PaginationDto<GetBookForLibrarianDto>>(books);
            return Ok(new APIResponse<PaginationDto<GetBookForLibrarianDto>>(200, "", bookDtos));
        }


    }
}

