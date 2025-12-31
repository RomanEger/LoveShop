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
		public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync(int pageNumber = 0, int pageSize = 20)
		{
			var filter = new Filter<Product>(pageNumber, pageSize);
			var sort = new Sort<Product, string>(x => x.Name);
			var cancellationToken = new CancellationToken();

			var products = await _productService.GetProductsAsync(filter, sort, cancellationToken);
			return Ok(products);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<Product>> GetProductByIdAsync(Guid id)
		{
			var product = await _productService.GetProductAsync(x => x.Id == id);

			return product is null ? NotFound() : Ok(product);
		}

		[HttpPost]
		public async Task<ActionResult> CreateProductAsync([FromBody] ProductCreateDTO productDTO)
		{
			var product = productDTO.ToProduct();
			await _productService.CreateProductAsync(product);
			return Created();
		}
	}
}