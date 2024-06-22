using System;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Interfaces;
using backend.Dtos.AddDtos;
using AutoMapper;
using backend.Dtos.GetDtos;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
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

       
        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationDto<GetCategoryDto>>> GetCategories( [FromRoute] int pageNumber=1, [FromRoute] int pageSize = 4)
        {
                var categories = await _categoryRepository.GetAllAsync(pageSize,pageNumber);
                var categoryDtos = _mapper.Map<PaginationDto<GetCategoryDto>>(categories);
                return Ok(categoryDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCategoryDto>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetbyIdAsync(id);
            if (category is null)
                return NotFound();

            var categoryDto = _mapper.Map<GetCategoryDto>(category);
            return categoryDto;
        }

        [HttpGet("search/{name}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationDto<GetCategoryDto>>> GetCategories([FromRoute] string name, [FromRoute] int pageSize, [FromRoute] int pageNumber)
        {
            var categories = await  _categoryRepository.GetCategoriesbyName(name,pageNumber, pageSize);
            var categoryDtos = _mapper.Map<PaginationDto<GetCategoryDto>>(categories);
            return Ok(categoryDtos);

        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id,[FromBody] AddCategoryDto categoryDto)
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

        [Authorize(Roles = "lIBRARIAN")]
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory([FromBody]AddCategoryDto categoryDto)
        {
            var validationResult = await _validator.ValidateAsync(categoryDto);
            if(validationResult.Errors.Any())
            {
                return BadRequest(validationResult);
            }
                var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddAsync(category);
            return Ok();
        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
