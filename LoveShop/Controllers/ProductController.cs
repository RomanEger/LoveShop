using LoveShop.DTOs;
using LoveShop.Models;
using LoveShop.Services;
using LoveShop.Shared;
using Microsoft.AspNetCore.Mvc;

namespace LoveShop.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly ProductService _productService;

		public ProductController(ProductService productService)
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

			var products = await _productService.GetProductsAsync(filter, sort, cancellationToken);
			return Ok(products);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<ProductDTO>> GetProductByIdAsync(
			Guid id,
			CancellationToken cancellationToken = default)
		{
			var product = await _productService.GetProductAsync(x => x.Id == id, cancellationToken);

			return product is null ? NotFound() : Ok(product);
		}

		[HttpPost]
		public async Task<ActionResult> CreateProductAsync(
			[FromBody] ProductCreateDTO productDto,
			CancellationToken cancellationToken = default)
		{
			await _productService.CreateProductAsync(productDto, cancellationToken);
			return Created();
		}
	}
}