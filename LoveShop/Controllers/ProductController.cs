using LoveShop.Models;
using LoveShop.Services;
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
        public async Task<IActionResult> GetProductsAsync(int pageNumber, int pageSize)
        {
            var products = await _productService.GetProductsAsync(pageNumber, pageSize);
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var product = await _productService.GetProductAsync(x => x.Id == id);

            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] Product product)
        {
            await _productService.CreateProductAsync(product);
            return Created();
        }
    }
}