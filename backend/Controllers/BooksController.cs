using AutoMapper;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos;
using backend.Interfaces;
using backend.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AddCategoryDto> _validator;
        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper, IValidator<AddCategoryDto> validator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _validator = validator;
        }

        //GET /api/categories?pageSize=10&pageNumber=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategoryDto>>> GetCategories([FromQuery] int pageSize = 4, [FromQuery] int pageNumber = 1)
        {
            var categories = await _categoryRepository.GetAllAsync(pageSize, pageNumber);
            var categoryDtos = _mapper.Map<IEnumerable<GetCategoryDto>>(categories);
            return Ok(categoryDtos);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCategoryDto>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetbyIdAsync(id);
            if (category is null)
                return NotFound();

            var categoryDto = _mapper.Map<GetCategoryDto>(category);
            return categoryDto;
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<GetCategoryDto>> GetCategories(string name)
        {
            var categories = await _categoryRepository.GetCategoriesbyName(name);
            var categoryDtos = _mapper.Map<IEnumerable<GetCategoryDto>>(categories);
            return Ok(categoryDtos);

        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] AddCategoryDto categoryDto)
        {
            var validationResult = await _validator.ValidateAsync(categoryDto);
            if (validationResult.Errors.Any())
            {
                return BadRequest(validationResult);
            }
            var existingCategory = await _categoryRepository.GetbyIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            // Map the data from the DTO to the existing category entity
            _mapper.Map(categoryDto, existingCategory);

            await _categoryRepository.UpdateAsync(existingCategory);
            return NoContent();
        }

        // POST: api/Categories

        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory([FromBody] AddCategoryDto categoryDto)
        {
            var validationResult = await _validator.ValidateAsync(categoryDto);
            if (validationResult.Errors.Any())
            {
                return BadRequest(validationResult);
            }
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddAsync(category);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
}
