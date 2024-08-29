using AutoMapper;
using FluentValidation;
using Library.Core;
using LibraryDataAcces.Models;
using LibraryDataAcces.Repozitories;
using LibraryWebAPI.Dtos.Category;
using LibraryWebAPI.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCategoryDto> _createValidator;

        public CategoriesController(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IValidator<CreateCategoryDto> createValidator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _createValidator = createValidator;
        }
        [HttpGet]
        public async Task<PaginatedList<CategoryDto>> Get(int page, int nr)
        {
            var categories = await _categoryRepository.GetCategoriesAsync(page, nr);
            return _mapper.Map<PaginatedList<CategoryDto>>(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoryDto>(category));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto category)
        {
            var validationResult = await _createValidator.ValidateAsync(category);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.GetErrorMessages());
            }

            var categoryToCreate = _mapper.Map<Category>(category);

            var createResult = await _categoryRepository.CreateCategoryAsync(categoryToCreate);
            return Ok(_mapper.Map<CategoryDto>(createResult));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDto category)
        {
            var categoryToUpdate = _mapper.Map<Category>(category);

            try
            {
                var updateResult = await _categoryRepository.UpdateCategoryAsync(categoryToUpdate);
                return Ok(_mapper.Map<CategoryDto>(updateResult));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
