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
using backend.Dtos.Responses;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<ActionResult<APIResponse<PaginationDto<GetCategoryDto>>>> GetCategories( [FromRoute] int pageNumber=1, [FromRoute] int pageSize = 4)
        {
                var categories = await _categoryRepository.GetAllAsync(pageSize,pageNumber);
                var categoryDtos = _mapper.Map<PaginationDto<GetCategoryDto>>(categories);
                return Ok(new APIResponse<PaginationDto<GetCategoryDto >>(200, "",categoryDtos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<GetCategoryDto>>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetbyIdAsync(id);
            if (category is null)
                return NotFound(new APIResponse<object>(404, "The category is not found.",null));

            var categoryDto = _mapper.Map<GetCategoryDto>(category);
            return Ok(new APIResponse<GetCategoryDto>(200, "", categoryDto));
        }

        [HttpGet("search/{name}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<APIResponse<PaginationDto<GetCategoryDto>>>> GetCategories([FromRoute] string name, [FromRoute] int pageSize, [FromRoute] int pageNumber)
        {
            var categories = await  _categoryRepository.GetCategoriesbyName(name,pageNumber, pageSize);
            var categoryDtos = _mapper.Map<PaginationDto<GetCategoryDto>>(categories);
            return Ok(new APIResponse<PaginationDto<GetCategoryDto>>(200, "", categoryDtos));

        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id,[FromBody] AddCategoryDto categoryDto)
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

            var existingCategory = await _categoryRepository.GetbyIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound(new APIResponse<object>(404,"This category doesn't exists", null));
            }

            // Map the data from the DTO to the existing category entity
            _mapper.Map(categoryDto, existingCategory);

            await _categoryRepository.UpdateAsync(existingCategory);
            return Ok(new APIResponse<object>(200, "The category is updated successfully.", null));
        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory([FromBody]AddCategoryDto categoryDto)
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

            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddAsync(category);
            return Ok(new APIResponse<object>(200,"The categoey added successfully." , null));
        }

        [Authorize(Roles = "lIBRARIAN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryRepository.DeleteAsync(id);

            return Ok(new APIResponse<object>(200, "The catgegory deleted successfully.", null));
        }

        [HttpGet("names")]
        public async Task<ActionResult<APIResponse<ICollection<string>>>> GetCategoriesNames()
        {
            var res = await _categoryRepository.GetCategoriesNames();
            return Ok(new APIResponse<ICollection<string>>(200,"", res));
        }
    }
}
