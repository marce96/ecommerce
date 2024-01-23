using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Products.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider _productsProvider;

        public ProductsController(IProductsProvider productsProvider) => _productsProvider = productsProvider;

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var (statusCode, products, errorMessage) = await _productsProvider.GetProductsAsync();
            return statusCode switch
            {
                StatusCodes.Status200OK => Ok(products),
                _ when string.IsNullOrEmpty(errorMessage) => StatusCode(statusCode),
                _ => StatusCode(statusCode, errorMessage)
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var (statusCode, product, errorMessage) = await _productsProvider.GetProductAsync(id);
            return statusCode switch
            {
                StatusCodes.Status200OK => Ok(product),
                _ when string.IsNullOrEmpty(errorMessage) => StatusCode(statusCode),
                _ => StatusCode(statusCode, errorMessage)
            };
        }
    }
}
