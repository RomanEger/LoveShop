using LoveShop.DTOs.Category;
using LoveShop.Models;
using LoveShop.Services.Contracts;
using LoveShop.Shared;
using Microsoft.AspNetCore.Mvc;

namespace LoveShop.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly IGenericCrudService<Category, CategoryDTO, CategoryCreateDTO, CategoryUpdateDTO>
			_categoryService;

		public CategoryController(
			IGenericCrudService<Category, CategoryDTO, CategoryCreateDTO, CategoryUpdateDTO> categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesAsync(
			int pageNumber = 0,
			int pageSize = 20,
			CancellationToken cancellationToken = default)
		{
			var paginatedFiler = new PaginatedFilter<Category>(pageNumber, pageSize);
			var filter = new Filter<Category>(paginatedFiler);
			var sort = new Sort<Category, string>(x => x.Name);

			var categories = await _categoryService.GetAsync(filter, sort, cancellationToken);
			return Ok(categories);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<Category>> GetCategoryByIdAsync(
			Guid id,
			CancellationToken cancellationToken = default)
		{
			var category = await _categoryService.FindAsync(x => x.Id == id, cancellationToken);

			return category is null ? NotFound() : Ok(category);
		}

		[HttpPost]
		public async Task<ActionResult> CreateCategoryAsync(
			[FromBody] CategoryCreateDTO categoryCreateDTO,
			CancellationToken cancellationToken = default)
		{
			await _categoryService.CreateAsync(categoryCreateDTO, cancellationToken);

			return Created();
		}

		[HttpPut]
		public async Task<ActionResult> UpdateCategoryAsync(
			[FromBody] CategoryUpdateDTO categoryUpdateDTO,
			CancellationToken cancellationToken = default)
		{
			var updatedCategory = await _categoryService.UpdateAsync(categoryUpdateDTO, cancellationToken);

			return Ok(updatedCategory);
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteCategoryAsync(
			Guid id,
			CancellationToken cancellationToken = default)
		{
			await _categoryService.DeleteAsync(category => category.Id == id, cancellationToken);

			return Ok();
		}
	}
}