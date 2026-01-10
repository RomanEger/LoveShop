using LoveShop.DTOs.Product;
using LoveShop.Models;
using LoveShop.Services.Contracts;
using LoveShop.Shared;
using Microsoft.AspNetCore.Mvc;

namespace LoveShop.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly IGenericCrudService<Product, ProductDTO, ProductCreateDTO, ProductUpdateDTO> _productService;

		public ProductController(
			IGenericCrudService<Product, ProductDTO, ProductCreateDTO, ProductUpdateDTO> productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsAsync(
			int pageNumber = 0,
			int pageSize = 20,
			CancellationToken cancellationToken = default)
		{
			var paginatedFiler = new PaginatedFilter<Product>(pageNumber, pageSize);
			var filter = new Filter<Product>(paginatedFiler);
			var sort = new Sort<Product, string>(x => x.Name);

			var products = await _productService.GetAsync(filter, sort, cancellationToken);
			return Ok(products);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<ProductDTO>> GetProductByIdAsync(
			Guid id,
			CancellationToken cancellationToken = default)
		{
			var product = await _productService.FindAsync(x => x.Id == id, cancellationToken);

			return product is null ? NotFound() : Ok(product);
		}

		[HttpPost]
		public async Task<ActionResult> CreateProductAsync(
			[FromBody] ProductCreateDTO productDTO,
			CancellationToken cancellationToken = default)
		{
			await _productService.CreateAsync(productDTO, cancellationToken);

			return Created();
		}

		[HttpPut]
		public async Task<ActionResult> UpdateCategoryAsync(
			[FromBody] ProductUpdateDTO productUpdateDTO,
			CancellationToken cancellationToken = default)
		{
			var updatedCategory = await _productService.UpdateAsync(productUpdateDTO, cancellationToken);

			return Ok(updatedCategory);
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteCategoryAsync(
			Guid id,
			CancellationToken cancellationToken = default)
		{
			await _productService.DeleteAsync(product => product.Id == id, cancellationToken);

			return Ok();
		}
	}
}